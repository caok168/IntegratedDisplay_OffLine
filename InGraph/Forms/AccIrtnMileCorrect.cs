using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using InvalidDataProcessing;
using MathWorks.MATLAB.NET.Arrays;
using System.IO;
using CitFileProcess;

namespace InGraph.Forms
{
    public partial class AccIrtnMileCorrect : Form
    {
        CitFileProcess.CitFileHelper citHelper = new CitFileProcess.CitFileHelper();
        /// <summary>
        /// cit文件路径
        /// </summary>
        string citFilePath = "";
        //曲线台账模板路径
        string curveFilePath = "";
        /// <summary>
        /// 长短链模板文件路径
        /// </summary>
        string abruptMileFilePath = "";


        public AccIrtnMileCorrect()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 开始处理按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnProcess_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(citFilePath))
            {
                MessageBox.Show("请选择轨检cit文件");
                return;
            }
            if (String.IsNullOrEmpty(curveFilePath))
            {
                MessageBox.Show("曲率台账模板文件");
                return;
            }
            if (String.IsNullOrEmpty(abruptMileFilePath))
            {
                MessageBox.Show("请选择长短链模板文件");
                return;
            }

            CalculateCorrugationClass calculateCorrugation = new CalculateCorrugationClass();

            

            var header = citHelper.GetDataInfoHead(citFilePath);
            var channelList = citHelper.GetDataChannelInfoHead(citFilePath);

            string correctMileFilePath = citFilePath.Substring(0, citFilePath.Length - 4) + "correctMileStone.cit";
            CreateCitHeader(correctMileFilePath, header, channelList);

            long startPos = citHelper.GetSamplePointStartOffset(header.iChannelNumber);
            long endPos = 0;

            int sampleNum=Convert.ToInt32((citHelper.GetFileLength(citFilePath)-startPos)/(header.iChannelNumber*2));

            double[,] wcCurveData; //台账曲率
            double[,] abruptMileData;//长短链

            wcCurveData = GetData(curveFilePath);
            abruptMileData = GetData(abruptMileFilePath);

            int fs = Convert.ToInt32(this.txt_fs.Text.Trim());
            double thresh_curve = Convert.ToDouble(this.txt_thresh_curve.Text.Trim());

            int count = 10;

            for (int k = 0; k < count; k++)
            {
                List<double[]> dataList = citHelper.GetAllChannelDataInRange(citFilePath, startPos, sampleNum/count, ref endPos);

                startPos = endPos;

                List<double[]> dataList_input = new List<double[]>();
                dataList_input.Add(dataList[0]);
                dataList_input.Add(dataList[1]);
                dataList_input.Add(dataList[7]);
                //double[] dataKm = citHelper.GetSingleChannelData(citFilePath, 1);
                //double[] dataM = citHelper.GetSingleChannelData(citFilePath, 2);
                //double[] dataChaoGao = citHelper.GetSingleChannelData(citFilePath, 7);

                //dataList.Add(dataKm);
                //dataList.Add(dataM);
                //dataList.Add(dataChaoGao);

                //string path = @"H:\工作文件汇总\铁科院\程序\离线加速度\data\ww_save.txt";
                //dataList = ReadTxt(path);

                MWNumericArray array = calculateCorrugation.GetProcessAbnormalDispResultProcess(dataList_input);

                List<double[]> listData = calculateCorrugation.GetProcessAbnormalDispResult(array);


                double[] mileData;
                double[] curveData;//超高

                mileData = listData[0];
                curveData = listData[1];


                MWNumericArray array2 = calculateCorrugation.GetVerifyKilometerResultProcess(mileData, curveData, wcCurveData, abruptMileData, fs, thresh_curve);

                var correctMileData = calculateCorrugation.GetVerifyKilometerResult(array2);

                double[] kmData = new double[correctMileData.Length];
                double[] mData = new double[correctMileData.Length];

                for (int i = 0; i < correctMileData.Length; i++)
                {
                    kmData[i] = correctMileData[i] / 1000;
                    mData[i] = correctMileData[i] % 1000;
                }

                dataList[0] = kmData;
                dataList[1] = mData;

                CreateCitData(correctMileFilePath, dataList);
            }
            

            MessageBox.Show("处理完成");
        }


        /// <summary>
        /// 创建cit文件头
        /// </summary>
        /// <param name="citFileName"></param>
        /// <param name="dataHeadInfo"></param>
        /// <param name="channelList"></param>
        private void CreateCitHeader(string citFileName, DataHeadInfo dataHeadInfo, List<DataChannelInfo> channelList)
        {
            citHelper.WriteCitFileHeadInfo(citFileName, dataHeadInfo, channelList);

            citHelper.WriteDataExtraInfo(citFileName, "");
        }

        /// <summary>
        /// 写入cit数据
        /// </summary>
        /// <param name="citFileName"></param>
        /// <param name="dataList"></param>
        private void CreateCitData(string citFileName, List<double[]> dataList)
        {
            citHelper.WriteChannelData(citFileName, dataList);
        }

        
        /// <summary>
        /// 选择cit文件按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_CitSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            //判断用户是否正确的选择了文件
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                citFilePath = fileDialog.FileName;
                this.txt_CitFilePath.Text = citFilePath;
            }
        }

        /// <summary>
        /// 选择曲线台账按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_CurveSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            //判断用户是否正确的选择了文件
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                curveFilePath = fileDialog.FileName;
                this.txt_CurveFilePath.Text = curveFilePath;
            }
        }

        /// <summary>
        /// 选择长短链按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_AbruptMileSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            //判断用户是否正确的选择了文件
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                abruptMileFilePath = fileDialog.FileName;
                this.txt_AbruptMileFilePath.Text = abruptMileFilePath;
            }
        }

        /// <summary>
        /// 获取模板数据
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private double[,] GetData(string filePath)
        {
            List<double[]> list = new List<double[]>();

            StreamReader sr = new StreamReader(filePath);

            while (true)
            {
                string str = sr.ReadLine();
                if (!string.IsNullOrEmpty(str))
                {
                    str = str.TrimStart();

                    str = str.Replace("\t", " ");
                    str = str.Replace("  ", ",");
                    str = str.Replace(" ", ",");

                    string[] datas = str.Split(',');
                    int length = datas.Length;
                    if (length > 0)
                    {
                        double[] darray = new double[length];
                        for (int i = 0; i < darray.Length; i++)
                        {
                            darray[i] = Convert.ToDouble(datas[i]);
                        }
                        list.Add(darray);
                    }
                }
                else
                {
                    break;
                }
            }
            int count = 0;
            int listLength = list.Count;
            if (list.Count > 0)
            {
                count = list[0].Length;
            }
            double[,] data = new double[listLength, count];

            for (int i = 0; i < listLength; i++)
            {
                for (int j = 0; j < count; j++)
                {
                    data[i, j] = list[i][j];
                }
            }

            return data;
        }


        private List<double[]> ReadTxt(string filepath)
        {
            StreamReader sr = new StreamReader(filepath);

            List<double> list1 = new List<double>();
            List<double> list2 = new List<double>();
            List<double> list3 = new List<double>();

            List<double[]> listAll = new List<double[]>();

            while (true)
            {
                string str = sr.ReadLine();
                if (!string.IsNullOrEmpty(str))
                {
                    str = str.TrimStart();
                    str = str.Replace("  ", ",");
                    str = str.Replace(" ", ",");

                    string[] datas = str.Split(',');
                    int length = datas.Length;
                    if (length > 0)
                    {
                        //double[] darray = new double[length];
                        //for (int i = 0; i < darray.Length; i++)
                        //{
                        //    darray[i] = Convert.ToDouble(datas[i]);
                        //}

                        list1.Add(Convert.ToDouble(datas[0]));
                        list2.Add(Convert.ToDouble(datas[1]));
                        list3.Add(Convert.ToDouble(datas[2]));
                    }
                }
                else
                {
                    listAll.Add(list1.ToArray());
                    listAll.Add(list2.ToArray());
                    listAll.Add(list3.ToArray());

                    break;
                }
            }

            return listAll;
        }
    }
}
