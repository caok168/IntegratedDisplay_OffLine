using System;
using System.Collections.Generic;
using System.Text;
using MathWorks.MATLAB.NET.Arrays;
using InvalidDataProcessing.Model;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.IO;
using System.Collections;
using NPOI.XSSF.UserModel;

namespace InvalidDataProcessing
{
    /// <summary>
    /// 波磨计算类
    /// </summary>
    public class CalculateCorrugationClass
    {
        public AccelerateNew.AccelerateNewClass anc;

        public CalculateCorrugationClass()
        {
            anc = new AccelerateNew.AccelerateNewClass();
        }

        #region 波磨的有效值相关操作

        /// <summary>
        /// 计算波磨的移动有效值
        /// </summary>
        /// <param name="Wdisp">里程</param>
        /// <param name="Wvelo">速度</param>
        /// <param name="Wacc">轴箱加速度</param>
        /// <param name="fs">采样频率</param>
        /// <param name="len_win_imp">计算焊接接头移动有效值时的窗长</param>
        /// <param name="FilterFreq_L">计算焊接接头移动有效值时的下限滤波频率</param>
        /// <param name="FilterFreq_H">计算焊接接头移动有效值时的上限滤波频率</param>
        /// <param name="len_win">计算波磨移动有效值时的窗长</param>
        /// <param name="Wavelen_L">计算波磨的下限滤波波长</param>
        /// <param name="Wavelen_H">计算波磨的上限滤波波长</param>
        /// <param name="len_downsample">重采样间隔</param>
        public MWNumericArray CalcCorrugationRmsProcess(double[] Wdisp, double[] Wvelo, double[] Wacc,
            int fs,int len_win_imp,int FilterFreq_L,int FilterFreq_H,
            int len_win, double Wavelen_L, double Wavelen_H, int len_downsample)
        {
            MWNumericArray dataArray_mile = new MWNumericArray(Wdisp);
            MWNumericArray dataArray_speed = new MWNumericArray(Wvelo);
            MWNumericArray dataArray_data = new MWNumericArray(Wacc);

            MWNumericArray fsArray = fs;
            MWNumericArray len_win_impArray = len_win_imp;
            MWNumericArray FilterFreq_LArray = FilterFreq_L;
            MWNumericArray FilterFreq_HArray = FilterFreq_H;
            MWNumericArray len_winArray = len_win;
            MWNumericArray Wavelen_LArray = Wavelen_L;
            MWNumericArray Wavelen_HArray = Wavelen_H;
            MWNumericArray len_downsampleArray = len_downsample;

            MWNumericArray resultArray = (MWNumericArray)anc.sub_calculate_ABA_rms_for_corrugation(dataArray_mile, dataArray_speed, dataArray_data,
                fsArray, len_win_impArray, FilterFreq_LArray, FilterFreq_HArray, len_winArray, Wavelen_LArray, Wavelen_HArray, len_downsampleArray);

            return resultArray;
        }

        /// <summary>
        /// 对波磨的移动有效值进行转换
        /// </summary>
        /// <param name="resultArray"></param>
        /// <returns></returns>
        public List<double[]> GetCalcCorrugationRms(MWNumericArray resultArray)
        {
            Array array = resultArray.ToArray();
            int length = array.Length / 3;
            double[] dmile = new double[length];
            double[] dvalue = new double[length];//有效值
            double[] dspeed = new double[length];

            double[,] dArray = (double[,])array;

            for (int i = 0; i < length; i++)
            {
                dmile[i] = dArray[i, 0];
                dvalue[i] = dArray[i, 1];
                dspeed[i] = dArray[i, 2];
            }

            List<double[]> list = new List<double[]>();
            list.Add(dmile);
            list.Add(dvalue);
            list.Add(dspeed);

            return list;
        }

        /// <summary>
        /// 只获取波磨的移动有效值
        /// </summary>
        /// <param name="resultArray"></param>
        /// <returns></returns>
        public double[] GetCalcCorrugationRmsValue(MWNumericArray resultArray)
        {
            Array array = resultArray.ToArray();
            int length = array.Length / 3;
            double[] dvalue = new double[length];//有效值

            double[,] dArray = (double[,])array;

            for (int i = 0; i < length; i++)
            {
                dvalue[i] = dArray[i, 1];
            }
            return dvalue;
        }

        #endregion

        #region 波磨的区段大值相关操作

        /// <summary>
        /// 计算波磨的区段大值
        /// </summary>
        /// <param name="mileData">里程</param>
        /// <param name="rmsData">有效值</param>
        /// <param name="speedData">速度</param>
        /// <param name="len_merge">段的长度</param>
        /// <returns>
        /// ww_rms_merge(:,1)  每段最大有效值对应的里程
        /// ww_rms_merge(:,2)  每段最大有效值对应的速度
        /// ww_rms_merge(:,3)   每段最大有效值
        ///</returns>
        public double[] CalcCorrugationMaxProcess(double[] mileData, double[] rmsData,double[] speedData, int len_merge)
        {
            MWNumericArray dataArray_rmsData = new MWNumericArray(rmsData);
            MWNumericArray len_mergeArray = len_merge;

            MWNumericArray resultArray = (MWNumericArray)anc.sub_extract_segment_maxmium_value_for_corrugation(dataArray_rmsData, len_mergeArray);

            double[] dResultArray = new double[rmsData.Length];

            if (resultArray.IsEmpty || !resultArray.IsNumericArray)
            {

            }
            else
            {
                double[,] rmsArray = (double[,])(resultArray.ToArray(MWArrayComponent.Real));

                for (int j = 0; j < rmsArray.GetLength(1); j++)
                {
                    dResultArray[j] = rmsArray[0, j];
                }
            }

            return dResultArray;
        }

        /// <summary>
        /// 计算波磨的区段大值
        /// </summary>
        /// <param name="dataArray_rmsData"></param>
        /// <param name="len_merge"></param>
        /// <returns></returns>
        public MWNumericArray CalcCorrugationMaxProcess(MWNumericArray dataArray_rmsData, int len_merge)
        {
            MWNumericArray len_mergeArray = len_merge;

            MWNumericArray resultArray = (MWNumericArray)anc.sub_extract_segment_maxmium_value_for_corrugation(dataArray_rmsData, len_mergeArray);

            return resultArray;
        }

        /// <summary>
        /// 获取区段大值
        /// </summary>
        /// <param name="resultArray"></param>
        /// <returns></returns>
        public List<double[]> GetCalcCorrugationMax(MWNumericArray resultArray)
        {
            Array array = resultArray.ToArray();
            int length = array.Length / 3;
            double[] dmile = new double[length];
            double[] dvalue = new double[length];//大值
            double[] dspeed = new double[length];

            double[,] dArray = (double[,])array;

            for (int i = 0; i < length; i++)
            {
                dmile[i] = dArray[i, 0];
                dvalue[i] = dArray[i, 1];
                dspeed[i] = dArray[i, 2];
            }

            List<double[]> list = new List<double[]>();
            list.Add(dmile);//里程
            list.Add(dvalue);//每段最大有效值
            list.Add(dspeed);//速度
            
            return list;
        }

        #endregion

        #region 波磨指数平均值相关操作

        /// <summary>
        /// 计算波磨指数平均值
        /// </summary>
        /// <param name="mileData">里程</param>
        /// <param name="maxData">波磨有效值的区段大值</param>
        /// <param name="speedData">速度</param>
        /// <returns>
        /// mean_value_sub(:,1)  平均速度
        /// mean_value_sub(:,2)  平均波磨有效值   假设为：value
        /// mean_value_sub(:,3)  求平均值的样本点的个数   假设为：number
        /// s =（(value1 * number1) + (value2 * number2) + (value3 * number3) + … )+… 
        /// k = number1 + number2 + number3 +…
        /// 最终的平均值 = s / k
        /// </returns>
        public MWNumericArray CalcCorrugationAvgProcess(double[] mileData, double[] maxData, double[] speedData)
        {
            int length = mileData.Length;
            double[,] data_array = new double[length, 3];
            for (int i = 0; i < length; i++)
            {
                data_array[i, 0] = mileData[i];
                data_array[i, 1] = maxData[i];
                data_array[i, 2] = speedData[i];
            }

            Array array = data_array;

            MWNumericArray array_result = (MWNumericArray)array;

            MWNumericArray resultArray = CalcCorrugationAvgProcess(array_result);

            return resultArray;
        }

        public MWNumericArray CalcCorrugationAvgProcess(MWNumericArray array)
        {
            MWNumericArray resultArray = (MWNumericArray)anc.sub_calculate_mean_value_for_corrugation(array);

            return resultArray;
        }

        public List<double[]> GetCalcCorrugationAvg(MWNumericArray resultArray)
        {
            Array array = resultArray.ToArray();
            int length = array.Length / 3;
            double[] davgSpeed = new double[length];
            double[] davgValue = new double[length];//有效值
            double[] dsample = new double[length];

            double[,] dArray = (double[,])array;

            for (int i = 0; i < length; i++)
            {
                davgSpeed[i] = dArray[i, 0];
                davgValue[i] = dArray[i, 1];
                dsample[i] = dArray[i, 2];
            }

            List<double[]> list = new List<double[]>();
            list.Add(davgSpeed);//平均速度
            list.Add(davgValue);//平均波磨有效值
            list.Add(dsample);  //求平均值的样本点的个数

            return list;
        }

        #endregion

        #region 提取波磨信息相关操作

        /// <summary>
        /// 提取波磨信息，这个函数的输入来自于计算“有效值”那个函数的输出(一个cit)
        /// </summary>
        /// <param name="rmsArray">
        /// ww_RMS(:,1)   里程
        /// ww_RMS(:,2)   波磨有效值：来自于“有效值”的那三个通道
        /// ww_RMS(:,3)   速度
        /// </param>
        /// <param name="fs">采样频率</param>
        /// <param name="len_downsample">重采样间隔</param>
        /// <param name="mean_rms_nacc">波磨有效值的平均值</param>
        /// <param name="thresh_tii">波磨指数阈值</param>
        /// <returns>
        /// ww_coru(:,1)  波磨区段开始里程
        /// ww_coru(:,2)  波磨区段结束里程
        /// ww_coru(:,3)  波磨有效值
        /// ww_coru(:,4)  波磨指数
        /// ww_coru(:,5)  波磨区段平均速度
        /// ww_coru(:,6)  第1主频
        /// ww_coru(:,7)  波长
        /// ww_coru(:,8)  能量集中率
        /// </returns>
        public MWNumericArray GetCorrugationInfomationProcess(MWNumericArray rmsArray, int fs, int len_downsample, double mean_rms_nacc, double thresh_tii)
        {
            MWNumericArray resultArray = (MWNumericArray)anc.sub_extract_information_for_corrugation(rmsArray, fs, len_downsample, mean_rms_nacc, thresh_tii);

            return resultArray;
        }

        public MWNumericArray GetCorrugationInfomationProcess(double[] mileData,double[] rmsData,double[] speedData, int fs, int len_downsample, double mean_rms_nacc, double thresh_tii)
        {
            int length = mileData.Length;
            double[,] data_array = new double[length, 3];
            for (int i = 0; i < length; i++)
            {
                data_array[i, 0] = mileData[i];
                data_array[i, 1] = rmsData[i];
                data_array[i, 2] = speedData[i];
            }

            Array array = data_array;

            MWNumericArray array_result = (MWNumericArray)array;

            MWNumericArray resultArray = GetCorrugationInfomationProcess(array_result, fs, len_downsample, mean_rms_nacc, thresh_tii);

            return resultArray;
        }

        public List<double[]> GetCorrugationInfomation(MWNumericArray resultArray)
        {
            Array array = resultArray.ToArray();
            int length = array.Length / 8;

            double[] dStartMile = new double[length];
            double[] dEndMile = new double[length];
            double[] dRmsValue = new double[length];
            double[] dPeakValue = new double[length];
            double[] dAvgSpeed = new double[length];
            double[] dFirstBasicFrequency = new double[length];
            double[] dWaveLength = new double[length];
            double[] dEnergyRatio = new double[length];

            double[,] dArray = (double[,])array;

            for (int i = 0; i < length; i++)
            {
                dStartMile[i] = dArray[i, 0];
                dEndMile[i] = dArray[i, 1];
                dRmsValue[i] = dArray[i, 2];
                dPeakValue[i] = dArray[i, 3];
                dAvgSpeed[i] = dArray[i, 4];
                dFirstBasicFrequency[i] = dArray[i, 5];
                dWaveLength[i] = dArray[i, 6];
                dEnergyRatio[i] = dArray[i, 7];
            }

            List<double[]> list = new List<double[]>();
            list.Add(dStartMile);//平均速度
            list.Add(dEndMile);//平均波磨有效值
            list.Add(dRmsValue);  //求平均值的样本点的个数
            list.Add(dPeakValue);//平均速度
            list.Add(dAvgSpeed);//平均波磨有效值
            list.Add(dFirstBasicFrequency);  //求平均值的样本点的个数
            list.Add(dWaveLength);//平均速度
            list.Add(dEnergyRatio);//平均波磨有效值

            return list;
        }


        /// <summary>
        /// 生成Excel文件
        /// </summary>
        /// <param name="list">波磨信息的集合</param>
        /// <param name="folderPath">文件夹路径</param>
        /// <param name="fileName">文件名称</param>
        public void CreateExcel(List<CorrugationInfoModel> list, string folderPath, string fileName)
        {
            //HSSFWorkbook workbook = new HSSFWorkbook();
            XSSFWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();

            IRow rowHeader = sheet.CreateRow(0);
            ICellStyle style = workbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;

            ICell cellHeader1 = rowHeader.CreateCell(0);
            cellHeader1.SetCellValue("开始里程");
            cellHeader1.CellStyle = style;

            ICell cellHeader2 = rowHeader.CreateCell(1);
            cellHeader2.SetCellValue("结束里程");
            cellHeader2.CellStyle = style;

            ICell cellHeader3 = rowHeader.CreateCell(2);
            cellHeader3.SetCellValue("有效值");
            cellHeader3.CellStyle = style;

            ICell cellHeader4 = rowHeader.CreateCell(3);
            cellHeader4.SetCellValue("波磨指数");
            cellHeader4.CellStyle = style;

            ICell cellHeader5 = rowHeader.CreateCell(4);
            cellHeader5.SetCellValue("平均速度");
            cellHeader5.CellStyle = style;

            ICell cellHeader6 = rowHeader.CreateCell(5);
            cellHeader6.SetCellValue("第1主频");
            cellHeader6.CellStyle = style;

            ICell cellHeader7 = rowHeader.CreateCell(6);
            cellHeader7.SetCellValue("波长");
            cellHeader7.CellStyle = style;

            ICell cellHeader8 = rowHeader.CreateCell(7);
            cellHeader8.SetCellValue("能量集中率");
            cellHeader8.CellStyle = style;

            for (int i = 0; i < list.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 1);

                ICell cell1 = row.CreateCell(0);
                cell1.SetCellValue(list[i].StartMile);
                cell1.CellStyle = style;

                ICell cell2 = row.CreateCell(1);
                cell2.SetCellValue(list[i].EndMile);
                cell2.CellStyle = style;

                ICell cell3 = row.CreateCell(2);
                cell3.SetCellValue(list[i].RmsValue);
                cell3.CellStyle = style;

                ICell cell4 = row.CreateCell(3);
                cell4.SetCellValue(list[i].PeakValue);
                cell4.CellStyle = style;

                ICell cell5 = row.CreateCell(4);
                cell5.SetCellValue(list[i].AvgSpeed);
                cell5.CellStyle = style;

                ICell cell6 = row.CreateCell(5);
                cell6.SetCellValue(list[i].FirstBasicFrequency);
                cell6.CellStyle = style;

                ICell cell7 = row.CreateCell(6);
                cell7.SetCellValue(list[i].WaveLength);
                cell7.CellStyle = style;

                ICell cell8 = row.CreateCell(7);
                cell8.SetCellValue(list[i].EnergyRatio);
                cell8.CellStyle = style;
            }

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            string fileNamePath = folderPath + "\\" + fileName + ".xlsx";
            FileStream fs = new FileStream(fileNamePath, FileMode.Create);
            workbook.Write(fs);
            fs.Close();
            workbook = null;
        }

        /// <summary>
        /// 通过excel文件获取波磨信息集合
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public List<CorrugationInfoModel> GetCorrugationInfoList(string filePath)
        {
            List<CorrugationInfoModel> list = new List<CorrugationInfoModel>();

            FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite);//打开模板 只读
            HSSFWorkbook wb = new HSSFWorkbook(file);

            ISheet sheet = wb.GetSheetAt(0);

            for (int i = (sheet.FirstRowNum) + 1; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);

                CorrugationInfoModel model = new CorrugationInfoModel();

                model.StartMile = row.GetCell(0) == null ? "" : row.GetCell(0).ToString();
                model.EndMile = row.GetCell(1) == null ? "" : row.GetCell(1).ToString();
                model.RmsValue = row.GetCell(2) == null ? "" : row.GetCell(2).ToString();
                model.PeakValue = row.GetCell(3) == null ? "" : row.GetCell(3).ToString();
                model.AvgSpeed = row.GetCell(4) == null ? "" : row.GetCell(4).ToString();
                model.FirstBasicFrequency = row.GetCell(5) == null ? "" : row.GetCell(5).ToString();
                model.WaveLength = row.GetCell(6) == null ? "" : row.GetCell(6).ToString();
                model.EnergyRatio = row.GetCell(7) == null ? "" : row.GetCell(7).ToString();

                list.Add(model);
            }

            return list;
        }

        #endregion

        #region 提取波磨波形相关操作

        /// <summary>
        /// 提取波磨波形：输入是最原始的那个cit ，上面的excel表，界面参数【参考文档】
        /// </summary>
        /// <param name="corrugationInfoArray"></param>
        /// <param name="mileData">里程</param>
        /// <param name="speedData">速度</param>
        /// <param name="channelData">通道数据</param>
        /// <param name="fs">采样频率</param>
        /// <param name="thresh_en">能量集中率的阈值</param>
        /// <param name="len_win">计算波磨有效值时的窗长</param>
        /// <returns>
        /// ww_coru_wave (1:mcoru,1)  波磨区段开始里程
        /// ww_coru_wave (1:mcoru,2)  波磨区段结束里程
        /// ww_coru_wave (1:mcoru,3)  波磨有效值
        /// ww_coru_wave (1:mcoru,4)  波磨指数
        /// ww_coru_wave (1:mcoru,5)  波磨区段平均速度
        /// ww_coru_wave (1:mcoru,6)  第1主频
        /// ww_coru_wave (1:mcoru,7)  波长
        /// ww_coru_wave (1:mcoru,8)  能量集中率
        /// ww_coru_wave (:,9) = kdiag  各波磨区段长度的指示数组
        /// ww_coru_wave (:,10) = wave_coru  各波磨区段的原始波形数据
        /// ww_coru_wave (:,11) = disp_coru  各波磨区段的里程数据
        /// ww_coru_wave (1:11,12) 各子数据列的长度: 存储了前面各个列的长度
        /// </returns>
        public MWNumericArray GetCorrugationWaveProcess(MWNumericArray corrugationInfoArray, double[] mileData, double[] speedData, 
            double[] channelData, int fs, double thresh_en, int len_win)
        {
            MWNumericArray dataArray_mile = new MWNumericArray(mileData);
            MWNumericArray dataArray_speed = new MWNumericArray(speedData);
            MWNumericArray dataArray_data = new MWNumericArray(channelData);

            //InvalidDataProcessing.Common.NOPIHelper nopi = new InvalidDataProcessing.Common.NOPIHelper();
            //string excelpath = @"H:\c4_1.xls";
            //nopi.CreateExcelForData(mileData, "第一个参数", excelpath, "里程");

            //excelpath = @"H:\c4_2.xls";
            //nopi.CreateExcelForData(speedData, "第二个参数", excelpath, "速度");

            //excelpath = @"H:\c4_3.xls";
            //nopi.CreateExcelForData(channelData, "第三个参数", excelpath, "通道数据");

            MWNumericArray fsArray = fs;
            MWNumericArray thresh_enArray = thresh_en;
            MWNumericArray len_winArray = len_win;

            MWNumericArray resultArray = (MWNumericArray)anc.sub_extract_wave_for_corrugation_with_one_output(corrugationInfoArray,
                dataArray_mile, dataArray_speed, dataArray_data, fsArray, thresh_en, len_winArray);

            return resultArray;
        }

        public MWNumericArray GetCorrugationWaveProcess(List<double[]> corrugationInfoList, double[] mileData, double[] speedData,
            double[] channelData, int fs, double thresh_en, int len_win)
        {
            if (corrugationInfoList.Count != 8)
            {
                return null;
            }
            int length = corrugationInfoList[0].Length;
            double[,] data_array = new double[length, 8];
            for (int i = 0; i < length; i++)
            {
                data_array[i, 0] = corrugationInfoList[0][i];
                data_array[i, 1] = corrugationInfoList[1][i];
                data_array[i, 2] = corrugationInfoList[2][i];
                data_array[i, 3] = corrugationInfoList[3][i];
                data_array[i, 4] = corrugationInfoList[4][i];
                data_array[i, 5] = corrugationInfoList[5][i];
                data_array[i, 6] = corrugationInfoList[6][i];
                data_array[i, 7] = corrugationInfoList[7][i];
            }

            Array array = data_array;

            MWNumericArray array_result = (MWNumericArray)array;

            MWNumericArray resultArray = GetCorrugationWaveProcess(array_result, mileData, speedData, channelData, fs, thresh_en, len_win);

            return resultArray;
        }

        public MWNumericArray GetCorrugationWaveProcess(List<CorrugationInfoModel> listModels, double[] mileData, double[] speedData,
            double[] channelData, int fs, double thresh_en, int len_win)
        {
            int length = listModels.Count;
            double[] value1 = new double[length];
            double[] value2 = new double[length];
            double[] value3 = new double[length];
            double[] value4 = new double[length];
            double[] value5 = new double[length];
            double[] value6 = new double[length];
            double[] value7 = new double[length];
            double[] value8 = new double[length];

            for (int i = 0; i < listModels.Count; i++)
            {
                value1[i] = Convert.ToDouble(listModels[i].StartMile);
                value2[i] = Convert.ToDouble(listModels[i].EndMile);
                value3[i] = Convert.ToDouble(listModels[i].RmsValue);
                value4[i] = Convert.ToDouble(listModels[i].PeakValue);
                value5[i] = Convert.ToDouble(listModels[i].AvgSpeed);
                value6[i] = Convert.ToDouble(listModels[i].FirstBasicFrequency);
                value7[i] = Convert.ToDouble(listModels[i].WaveLength);
                value8[i] = Convert.ToDouble(listModels[i].EnergyRatio);
            }



            //InvalidDataProcessing.Common.NOPIHelper nopi = new InvalidDataProcessing.Common.NOPIHelper();
            //string excelpath = @"H:\d_1.xls";
            //nopi.CreateExcelForData(value1, "第一个参数", excelpath, "a");
            //excelpath = @"H:\d_2.xls";
            //nopi.CreateExcelForData(value2, "第一个参数", excelpath, "a");
            //excelpath = @"H:\d_3.xls";
            //nopi.CreateExcelForData(value3, "第一个参数", excelpath, "a");
            //excelpath = @"H:\d_4.xls";
            //nopi.CreateExcelForData(value4, "第一个参数", excelpath, "a");
            //excelpath = @"H:\d_5.xls";
            //nopi.CreateExcelForData(value5, "第一个参数", excelpath, "a");
            //excelpath = @"H:\d_6.xls";
            //nopi.CreateExcelForData(value6, "第一个参数", excelpath, "a");
            //excelpath = @"H:\d_7.xls";
            //nopi.CreateExcelForData(value7, "第一个参数", excelpath, "a");
            //excelpath = @"H:\d_8.xls";
            //nopi.CreateExcelForData(value8, "第一个参数", excelpath, "a");



            double[,] data_array = new double[length, 8];
            for (int i = 0; i < length; i++)
            {
                data_array[i, 0] = value1[i];
                data_array[i, 1] = value2[i];
                data_array[i, 2] = value3[i];
                data_array[i, 3] = value4[i];
                data_array[i, 4] = value5[i];
                data_array[i, 5] = value6[i];
                data_array[i, 6] = value7[i];
                data_array[i, 7] = value8[i];
            }

            Array array = data_array;

            MWNumericArray array_result = (MWNumericArray)array;

            MWNumericArray resultArray = GetCorrugationWaveProcess(array_result, mileData, speedData, channelData, fs, thresh_en, len_win);

            return resultArray;
        }

        public List<double[]> GetCorrugationWave(MWNumericArray resultArray)
        {
            Array array = resultArray.ToArray();
            int length = array.Length / 12;

            double[] dStartMile = new double[length];
            double[] dEndMile = new double[length];
            double[] dRmsValue = new double[length];
            double[] dPeakValue = new double[length];
            double[] dAvgSpeed = new double[length];
            double[] dFirstBasicFrequency = new double[length];
            double[] dWaveLength = new double[length];
            double[] dEnergyRatio = new double[length];

            double[] dlength = new double[length];
            double[] dchanneldata = new double[length];
            double[] dmile = new double[length];
            double[] ddatalength = new double[length];

            double[,] dArray = (double[,])array;

            for (int i = 0; i < length; i++)
            {
                dStartMile[i] = dArray[i, 0];
                dEndMile[i] = dArray[i, 1];
                dRmsValue[i] = dArray[i, 2];
                dPeakValue[i] = dArray[i, 3];
                dAvgSpeed[i] = dArray[i, 4];
                dFirstBasicFrequency[i] = dArray[i, 5];
                dWaveLength[i] = dArray[i, 6];
                dEnergyRatio[i] = dArray[i, 7];

                dlength[i] = dArray[i, 8];
                dchanneldata[i] = dArray[i, 9];
                dmile[i] = dArray[i, 10];
                ddatalength[i] = dArray[i, 11];
            }

            List<double[]> list = new List<double[]>();
            list.Add(dStartMile);
            list.Add(dEndMile);
            list.Add(dRmsValue);
            list.Add(dPeakValue);
            list.Add(dAvgSpeed);
            list.Add(dFirstBasicFrequency);
            list.Add(dWaveLength);
            list.Add(dEnergyRatio);

            list.Add(dlength);
            list.Add(dchanneldata);
            list.Add(dmile);
            list.Add(ddatalength);

            return list;
        }


        #endregion

        #region 提取焊接接头信息相关操作

        /// <summary>
        /// 提取焊接接头信息 输入来自于“有效值”计算的输出的那个cit和界面
        /// </summary>
        /// <param name="mileData">里程</param>
        /// <param name="speedData">速度</param>
        /// <param name="rmsData">焊接接头移动有效值：这个还是之前计算的移动有效值</param>
        /// <param name="peakData">轨道冲击指数：有效值数组中的每个元素除以平均值得到的数组</param>
        /// <param name="fs">采样频率</param>
        /// <param name="dist_weld">焊接接头之间的距离</param>
        /// <returns>
        /// w_tpi(:,1)  焊接接头处的里程
        /// w_tpi(:,2)  焊接接头处的速度
        /// w_tpi(:,3)  焊接接头处的有效值
        /// w_tpi(:,4)  焊接接头处的冲击指数
        /// w_tpi(:,5)  焊接接头处的采样编号
        /// </returns>
        public MWNumericArray GetWeldInfomationProcess(double[] mileData, double[] speedData, double[] rmsData, double[] peakData, int fs, int dist_weld)
        {
            MWNumericArray dataArray_mile = new MWNumericArray(mileData);
            MWNumericArray dataArray_speed = new MWNumericArray(speedData);
            MWNumericArray dataArray_rms = new MWNumericArray(rmsData);
            MWNumericArray dataArray_peak = new MWNumericArray(peakData);

            MWNumericArray fsArray = fs;
            MWNumericArray dist_weldArray = dist_weld;

            MWNumericArray resultArray = (MWNumericArray)anc.sub_find_welding_point_for_highlight(dataArray_mile, dataArray_speed, dataArray_rms, fsArray, dist_weldArray);

            return resultArray;
        }

        /// <summary>
        /// 提取焊接接头信息 返回结果集合
        /// </summary>
        /// <param name="resultArray"></param>
        /// <returns></returns>
        public List<double[]> GetWeldInfomation(MWNumericArray resultArray)
        {
            Array array = resultArray.ToArray();
            int length = array.Length / 5;

            double[] dmile = new double[length];      //焊接接头处的里程
            double[] dspeed = new double[length];     //焊接接头处的速度
            double[] drms = new double[length];       //焊接接头处的有效值
            double[] dpeak = new double[length];      //焊接接头处的冲击指数
            double[] dsampleNo = new double[length];  //焊接接头处的采样编号

            double[,] dArray = (double[,])array;

            for (int i = 0; i < length; i++)
            {
                dmile[i] = dArray[i, 0];
                dspeed[i] = dArray[i, 1];
                drms[i] = dArray[i, 2];
                dpeak[i] = dArray[i, 3];
                dsampleNo[i] = dArray[i, 4];
            }

            List<double[]> list = new List<double[]>();
            list.Add(dmile);
            list.Add(dspeed);
            list.Add(drms);
            list.Add(dpeak);
            list.Add(dsampleNo);

            return list;
        }


        public List<double[]> GetWeldInfomation(double[] mileData, double[] speedData, double[] rmsData, double[] peakData, int fs, double dist_weld)
        {
            
            //double[] mileDataNew = new double[65530];
            //double[] speedDataNew = new double[65530];
            //double[] rmsDataNew = new double[65530];
            //double[] peakDataNew = new double[65530];

            //for (int i = 0; i < mileDataNew.Length; i++)
            //{
            //    mileDataNew[i] = mileData[i];
            //    speedDataNew[i] = speedData[i];
            //    rmsDataNew[i] = rmsData[i];
            //    peakDataNew[i] = peakData[i];
            //}

            //InvalidDataProcessing.Common.NOPIHelper nopi = new InvalidDataProcessing.Common.NOPIHelper();
            //string excelpath = @"H:\f_1.xls";
            //nopi.CreateExcelForData(mileDataNew, "里程", excelpath, "a");
            //excelpath = @"H:\f_2.xls";
            //nopi.CreateExcelForData(speedDataNew, "速度", excelpath, "a");
            //excelpath = @"H:\f_3.xls";
            //nopi.CreateExcelForData(rmsDataNew, "有效值", excelpath, "a");
            //excelpath = @"H:\f_4.xls";
            //nopi.CreateExcelForData(peakDataNew, "轨道冲击指数", excelpath, "a");

            //MWNumericArray dataArray_mile = new MWNumericArray(mileDataNew);
            //MWNumericArray dataArray_speed = new MWNumericArray(speedDataNew);
            //MWNumericArray dataArray_rms = new MWNumericArray(rmsDataNew);
            //MWNumericArray dataArray_peak = new MWNumericArray(peakDataNew);

            MWNumericArray dataArray_mile = new MWNumericArray(mileData);
            MWNumericArray dataArray_speed = new MWNumericArray(speedData);
            MWNumericArray dataArray_rms = new MWNumericArray(rmsData);
            MWNumericArray dataArray_peak = new MWNumericArray(peakData);

            MWNumericArray fsArray = fs;
            MWNumericArray dist_weldArray = dist_weld;

            MWNumericArray resultArray = (MWNumericArray)anc.sub_find_welding_point_for_highlight(dataArray_mile, dataArray_speed, dataArray_rms,dataArray_peak, fsArray, dist_weldArray);

            return GetWeldInfomation(resultArray);
        }

        /// <summary>
        /// 导出焊接接头信息
        /// </summary>
        /// <param name="list"></param>
        /// <param name="folderPath"></param>
        /// <param name="fileName"></param>
        public void CreateWeltExcel(List<WeldModel> list, string folderPath, string fileName)
        {
            HSSFWorkbook workbook = new HSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();

            IRow rowHeader = sheet.CreateRow(0);
            ICellStyle style = workbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;

            ICell cellHeader1 = rowHeader.CreateCell(0);
            cellHeader1.SetCellValue("里程");
            cellHeader1.CellStyle = style;

            ICell cellHeader2 = rowHeader.CreateCell(1);
            cellHeader2.SetCellValue("速度");
            cellHeader2.CellStyle = style;

            ICell cellHeader3 = rowHeader.CreateCell(2);
            cellHeader3.SetCellValue("有效值");
            cellHeader3.CellStyle = style;

            ICell cellHeader4 = rowHeader.CreateCell(3);
            cellHeader4.SetCellValue("冲击指数");
            cellHeader4.CellStyle = style;

            ICell cellHeader5 = rowHeader.CreateCell(4);
            cellHeader5.SetCellValue("采样编号");
            cellHeader5.CellStyle = style;

            for (int i = 0; i < list.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 1);

                ICell cell1 = row.CreateCell(0);
                cell1.SetCellValue(list[i].Mile);
                cell1.CellStyle = style;

                ICell cell2 = row.CreateCell(1);
                cell2.SetCellValue(list[i].Speed);
                cell2.CellStyle = style;

                ICell cell3 = row.CreateCell(2);
                cell3.SetCellValue(list[i].RmsValue);
                cell3.CellStyle = style;

                ICell cell4 = row.CreateCell(3);
                cell4.SetCellValue(list[i].PeakValue);
                cell4.CellStyle = style;

                ICell cell5 = row.CreateCell(4);
                cell5.SetCellValue(list[i].SampleNo);
                cell5.CellStyle = style;
            }

            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            string fileNamePath = folderPath + "\\" + fileName + ".csv";
            FileStream fs = new FileStream(fileNamePath, FileMode.Create);
            workbook.Write(fs);
            fs.Close();
            workbook = null;
        }

        /// <summary>
        /// 导出焊接接头信息
        /// </summary>
        /// <param name="list"></param>
        /// <param name="filePath"></param>
        public void CreateWeltExcel(List<WeldModel> list, string filePath)
        {
            //HSSFWorkbook workbook = new HSSFWorkbook();
            XSSFWorkbook workbook = new XSSFWorkbook();
            ISheet sheet = workbook.CreateSheet();

            IRow rowHeader = sheet.CreateRow(0);
            ICellStyle style = workbook.CreateCellStyle();
            style.Alignment = HorizontalAlignment.Center;
            style.VerticalAlignment = VerticalAlignment.Center;

            ICell cellHeader1 = rowHeader.CreateCell(0);
            cellHeader1.SetCellValue("里程");
            cellHeader1.CellStyle = style;

            ICell cellHeader2 = rowHeader.CreateCell(1);
            cellHeader2.SetCellValue("速度");
            cellHeader2.CellStyle = style;

            ICell cellHeader3 = rowHeader.CreateCell(2);
            cellHeader3.SetCellValue("有效值");
            cellHeader3.CellStyle = style;

            ICell cellHeader4 = rowHeader.CreateCell(3);
            cellHeader4.SetCellValue("冲击指数");
            cellHeader4.CellStyle = style;

            ICell cellHeader5 = rowHeader.CreateCell(4);
            cellHeader5.SetCellValue("采样编号");
            cellHeader5.CellStyle = style;

            for (int i = 0; i < list.Count; i++)
            {
                IRow row = sheet.CreateRow(i + 1);

                ICell cell1 = row.CreateCell(0);
                cell1.SetCellValue(list[i].Mile);
                cell1.CellStyle = style;

                ICell cell2 = row.CreateCell(1);
                cell2.SetCellValue(list[i].Speed);
                cell2.CellStyle = style;

                ICell cell3 = row.CreateCell(2);
                cell3.SetCellValue(list[i].RmsValue);
                cell3.CellStyle = style;

                ICell cell4 = row.CreateCell(3);
                cell4.SetCellValue(list[i].PeakValue);
                cell4.CellStyle = style;

                ICell cell5 = row.CreateCell(4);
                cell5.SetCellValue(list[i].SampleNo);
                cell5.CellStyle = style;
            }

            string fileNamePath = filePath;
            FileStream fs = new FileStream(fileNamePath, FileMode.Create);
            workbook.Write(fs);
            fs.Close();
            workbook = null;
        }

        #endregion

        #region 根据台账曲率信息对里程进行校正

        /// <summary>
        /// 根据台账曲率信息对里程进行校正
        /// </summary>
        /// <param name="mileData">实测里程：里程通道</param>
        /// <param name="curveData">实测曲率：曲率通道</param>
        /// <param name="wcCurveData">台账曲率：有模板</param>
        /// <param name="abruptMileData">长短链里程：有模版</param>
        /// <param name="fs">采样频率</param>
        /// <param name="thresh_curve">曲率控制阈值，对于高速一般取 0.1。来自于界面</param>
        /// <returns>校正后的里程：这是一个数组，数组中每个元素对应的是原始波形文件中的采样点，数组中的值就是此采样点的校正里程。</returns>
        public MWNumericArray GetVerifyKilometerResultProcess(double[] mileData,double[] curveData,double[] wcCurveData, double[] abruptMileData,int fs,double thresh_curve)
        {
            MWNumericArray dataArray_mile = new MWNumericArray(mileData);
            MWNumericArray dataArray_curve = new MWNumericArray(curveData);
            MWNumericArray dataArray_wcCurve = new MWNumericArray(wcCurveData);
            MWNumericArray dataArray_abruptMile = new MWNumericArray(abruptMileData);

            MWNumericArray fsArray = fs;
            MWNumericArray thresh_curveArray = thresh_curve;

            MWNumericArray resultArray = (MWNumericArray)anc.sub_verify_kilometer(dataArray_mile,dataArray_curve,dataArray_wcCurve,dataArray_abruptMile,fsArray,thresh_curveArray);

            return resultArray;
        }

        public MWNumericArray GetProcessAbnormalDispResultProcess(List<double[]> listData)
        {
            int length = listData[0].Length;
            int count = listData.Count;

            double[,] data_array = new double[length, count];

            for (int j = 0; j < count; j++)
            {
                for (int i = 0; i < length; i++)
                {
                    data_array[i, j] = listData[j][i];
                }
            }

            Array array = data_array;

            MWNumericArray ww0 = (MWNumericArray)array;
            MWNumericArray resultArray = (MWNumericArray)anc.sub_process_abnormal_disp(ww0);
            return resultArray;
        }


        public List<double[]> GetProcessAbnormalDispResult(MWNumericArray resultArray)
        {
            Array array = resultArray.ToArray();
            int length = array.Length / 3;

            List<double[]> list = new List<double[]>();

            double[,] dArray = (double[,])array;

            for (int j = 0; j < 3; j++)
            {
                double[] data = new double[length];
                for (int i = 0; i < length; i++)
                {
                    data[i] = dArray[i, j];
                }

                list.Add(data);
            }

            return list;
        }


        /// <summary>
        /// 根据台账曲率信息对里程进行校正
        /// </summary>
        /// <param name="Wdisp_0">实测里程，上一个函数的km和m 通道</param>
        /// <param name="Wcurve_0">实测超高 （上一个函数的超高输出）</param>
        /// <param name="Wc">台账曲率，数组的数组（有模板）</param>
        /// <param name="ww_abrupt_kilo">长短链里程，数组的数组(有模板)</param>
        /// <param name="fs">采样频率   4(一米4个点)</param>
        /// <param name="thresh_curve">超高控制阈值，对于高速一般取 50.0</param>
        /// <returns></returns>
        public MWNumericArray GetVerifyKilometerResultProcess(double[] Wdisp_0, double[] Wcurve_0, double[,] Wc, double[,] ww_abrupt_kilo, int fs, double thresh_curve)
        {
            MWNumericArray dataArray_mile = new MWNumericArray(Wdisp_0);
            MWNumericArray dataArray_curve = new MWNumericArray(Wcurve_0);
            MWNumericArray dataArray_wcCurve = new MWNumericArray(Wc);
            MWNumericArray dataArray_abruptMile = new MWNumericArray(ww_abrupt_kilo);

            MWNumericArray fsArray = fs;
            MWNumericArray thresh_curveArray = thresh_curve;

            MWNumericArray resultArray = (MWNumericArray)anc.sub_verify_kilometer(dataArray_mile, dataArray_curve, dataArray_wcCurve, dataArray_abruptMile, fsArray, thresh_curveArray);

            return resultArray;
        }

        public double[] GetVerifyKilometerResult(MWNumericArray resultArray)
        {
            Array array = resultArray.ToArray();

            double[] data = new double[array.Length];

            double[,] dArray = (double[,])array;

            for (int i = 0; i < array.Length; i++)
            {
                data[i] = dArray[0, i];
            }

            return data;
        }

        #endregion



    }
}
