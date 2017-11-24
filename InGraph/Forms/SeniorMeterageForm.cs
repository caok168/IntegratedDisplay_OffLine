using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using InGraph.Classes;
using DataProcess;

namespace InGraph.Forms
{
    public partial class SeniorMeterageForm : Form
    {
        string[] positionArray = null;
        string[] channelChinNameArray = null;
        int meterageRadius = 0;
        string resultFilePath = null;
        string configFilePath = null;


        public SeniorMeterageForm()
        {
            InitializeComponent();
            meterageRadius = CommonClass.MeterageRadius;
        }

        private void buttonLoadConfigFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "配置文件|*.txt";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBoxConfigFile.Text = openFileDialog1.FileName;
                textBoxResultFile.Text = Path.Combine(Path.GetDirectoryName(openFileDialog1.FileName), "测量结果.csv");
                
            }
        }

        private void textBoxConfigFile_TextChanged(object sender, EventArgs e)
        {
            textBoxResultFile.Text = textBoxConfigFile.Text;
            resultFilePath = textBoxResultFile.Text;
        }

        private void buttonCancer_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonExecute_Click(object sender, EventArgs e)
        {
            configFilePath = openFileDialog1.FileName;
            resultFilePath = textBoxResultFile.Text;

            ReadConfig();

            string str = GetResults();
            try
            {
                File.WriteAllText(resultFilePath, str,Encoding.Default);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("写文件出错！");
            }
            toolStripStatusLabel1.Text = "取值完毕！";
            //MessageBox.Show("取值完毕！");
        }
        private void ReadConfig()
        {
            try
            {
                StreamReader sr = new StreamReader(configFilePath,Encoding.Default);
                positionArray = sr.ReadLine().Split(',');
                channelChinNameArray = sr.ReadLine().Split(',');
                sr.Close();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("读取配置文件出错！");
            }
            finally
            {
                
            }
        }

        #region 20131130 Add by yan
        #region 根据里程(单位为厘米)定位点(文件中的指针)--里程与修正后的里程做对比，然后根据修正后的里程找到点
        /// <summary>
        /// 里程与修正后的里程做对比，然后根据修正后的里程找到点（文件中的指针）
        /// </summary>
        /// <param name="addFileName">idf文件</param>
        /// <param name="mi">输入的里程（单位为公里）</param>
        /// <returns>当前点在修正后的里程中相对应的文件指针</returns>
        private long MeterToPointNew(DataInfoClass dic, float mi,  out IndexStaClass indexStaClsout,out int resultType)
        {
            List<IndexStaClass> listIndexStaCls = CommonClass.wdp.GetDataIndexInfo(dic.sAddFile);
            float len = 0;
            long currentPos = 0;
            indexStaClsout = new IndexStaClass();
            resultType = 0;//代表在校正里程里没有找到里程点，并且在文件原始里程中也找不到。
            foreach (IndexStaClass indexStaCls in listIndexStaCls)
            {

                if (dic.sKmInc == "增")
                {
                    if (mi > float.Parse(indexStaCls.lStartMeter) && mi <= float.Parse(indexStaCls.LEndMeter))
                    {
                        len = mi - float.Parse(indexStaCls.lStartMeter);
                    }
                }
                else
                {
                    if (mi < float.Parse(indexStaCls.lStartMeter) && mi >= float.Parse(indexStaCls.LEndMeter))
                    {
                        len = float.Parse(indexStaCls.lStartMeter) - mi;
                    }
                }

                if (len != 0)
                {
                    currentPos = indexStaCls.lStartPoint + (long)((float)(len * indexStaCls.lContainsPoint) / float.Parse(indexStaCls.lContainsMeter)) * dic.fScale.Length * 2;
                    indexStaClsout = indexStaCls;
                    resultType = 1;//代表在校正后的里程范围中找到测量点
                    break;
                }

            }
            if (len == 0)
            {
                /*
                 * 遍历结束，里程点不在indexStaClass的范围内。有两种情况：
                 * 一：输入的里程点不在cit文件的里程范围内。返回0.
                 * 二：cit文件还没有经过里程校正，直接寻找文件中的里程，找到则返回，否则返回0.
                */
                currentPos = CommonClass.wdp.MeterToPoint(dic.sFilePath, (long)(mi*10000),4, dic.fOffset.Length, dic.bEncrypt);
                if (currentPos != 0)
                {
                    resultType = 2;//代表在文件中的原始数据找到测量点
                }
            }

            //FileStream fs = new FileStream(dic.sFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            //BinaryReader br = new BinaryReader(fs);
            //br.BaseStream.Position = indexStaClsout.lStartPoint;
            //Byte[] b = br.ReadBytes(dic.fScale.Length * 2);
            //if (dic.bEncrypt)
            //{
            //    b = WaveformDataProcess.ByteXORByte(b);
            //}
            //short km = BitConverter.ToInt16(b, 0);
            //short count = BitConverter.ToInt16(b, 2);

            return currentPos;
        }
        #endregion

        #region 把测量半径转化为在cit文件中实际上(里程校正之后)覆盖的测量点的个数(默认是一米4个点)
        /// <summary>
        /// 把测量半径转化为在cit文件中实际上(里程校正之后)覆盖的测量点的个数(默认是一米4个点)
        /// </summary>
        /// <param name="meterageRadius">测量半径(单位为米)</param>
        /// <param name="indexStaCls">取测量点的区域</param>
        /// <returns>测量半径覆盖的测量点的个数(默认是一米4个点)</returns>
        public int GetDotFromMeterageRadius(int m_meterageRadius, IndexStaClass indexStaCls)
        {
            int dotNum = 0;
            dotNum = (int)(m_meterageRadius * indexStaCls.lContainsPoint / (float.Parse(indexStaCls.lContainsMeter) * 1000));

            return dotNum;
        }
        #endregion

        private string GetMaxValuesOfChannels(DataInfoClass dic,long startPos,long endPos,string[] channelNames)
        {
            StringBuilder retSB = new StringBuilder();
            string tmpStr = null;
            foreach (string channelName in channelNames)
            {
                ChannelsClass m_channelCls = null;
                foreach (ChannelsClass channelCls in dic.listCC)
                {
                    if (channelCls.ChineseName == channelName)
                    {
                        m_channelCls = channelCls;
                        break;
                    }
                }

                tmpStr = GetMaxValuesOfChannelById(dic, startPos, endPos, m_channelCls).ToString();
                retSB.AppendFormat("{0},",tmpStr);
                
            }
            retSB.Remove(retSB.Length - 1, 1);
            
            return retSB.ToString();
        }
        #region 根据通道Id把文件中起始点和结束点之间的绝对值最大的点的原始值
        /// <summary>
        /// 根据通道Id把文件中起始点和结束点之间的绝对值最大的点的原始值
        /// </summary>
        /// <param name="dic">cit文件</param>
        /// <param name="startPos">起始指针</param>
        /// <param name="endPos">结束指针</param>
        /// <param name="channelId">通道号</param>
        /// <returns>起始指针和结束指针之间绝对值最大的点的值</returns>
        private float GetMaxValuesOfChannelById(DataInfoClass dic,long startPos,long endPos,ChannelsClass channelCls)
        {
            try
            {
                FileStream fs = new FileStream(dic.sFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(fs);
                int gjtds = dic.fScale.Length;
                bool bEncrypt = dic.bEncrypt;
                byte[] b = new byte[gjtds * 2];

                long findpos = startPos;
                br.BaseStream.Position = startPos;
                List<float> dataArray = new List<float>();

                while (findpos < endPos)
                {
                    b = br.ReadBytes(gjtds * 2);
                    if (bEncrypt)
                    {
                        b = WaveformDataProcess.ByteXORByte(b);
                    }

                    short km = BitConverter.ToInt16(b, 0);
                    short count = BitConverter.ToInt16(b, 2);
                    short channelValueS = BitConverter.ToInt16(b, channelCls.Id*2);
                    float channelValueF = channelValueS / dic.fScale[channelCls.Id] + dic.fOffset[channelCls.Id];

                    dataArray.Add(channelValueF);

                    findpos += gjtds * 2;

                }

                br.Close();
                fs.Close();

                float fRet = MaxValueOfDataArray(dataArray.ToArray());
                return fRet;
            }
            catch
            {
                return 0;
            }
        }
        #endregion

        #region 求一组数中绝对值最大的数，并返回
        /// <summary>
        /// 求一组数中绝对值最大的数，并返回
        /// </summary>
        /// <param name="data">数组</param>
        /// <returns>绝对值最大的数</returns>
        public float MaxValueOfDataArray(float[] data)
        {
            float retValue = 0f;
            float resultF = 0;
            for (int i = 0; i < data.Length; i++)
            {
                if (Math.Abs(data[i]) > retValue)
                {
                    retValue = Math.Abs(data[i]);
                    resultF = data[i];
                }
            }
            return resultF;
        }
        #endregion

        private string GetCsvHeadStr()
        {


            return null;
        }


        private string GetValuesOfChannelPerCit(string citHead,DataInfoClass dic,String[] positions,int m_meterageRadius,string[] channelChinNames)
        {
            StringBuilder retSB = new StringBuilder();
            string tmpStr = null;
            foreach (string position in positions)
            {
                IndexStaClass indexStaCls;
                int resultType;
                int dotNum = 0;
                string tips = null;
                long currentPos = MeterToPointNew(dic, float.Parse(position), out indexStaCls,out resultType);
                if (resultType == 0)
                {//测量点在校正里程范围内找不到，并且在文件原始里程里也找不到
                    tmpStr = "";
                    StringBuilder sb = new StringBuilder();
                    foreach(string channelName in channelChinNames)
                    {
                        sb.AppendFormat(",{0}", "0");
                    }
                    sb.Remove(0, 1);
                    tmpStr = sb.ToString();
                    tips = "测量点在校正里程和文件原始里程范围内找不到";
                } 
                else
                {
                    if (resultType == 1)
                    {//测量点在校正里程范围内。
                        dotNum = GetDotFromMeterageRadius(m_meterageRadius, indexStaCls);
                        tips = "测量点在校正里程范围内";
                    }
                    else if (resultType == 2)
                    {//测量点在文件原始里程范围内
                        dotNum = m_meterageRadius * 4;
                        tips = "测量点在文件原始里程内";
                    }
                    //int dotNum = GetDotFromMeterageRadius(m_meterageRadius, indexStaCls);

                    long startPos = currentPos - dotNum * dic.fScale.Length * 2;
                    long endPos = currentPos + dotNum * dic.fScale.Length * 2;

                    tmpStr = GetMaxValuesOfChannels(dic, startPos, endPos, channelChinNames);
                }

                retSB.AppendFormat("{0},{1},{2},{3}", citHead,position, tmpStr,tips);
                retSB.AppendLine();

            }
            return retSB.ToString();
        }

        private string GetCsvHead()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("线路名,线路代码,行别,增减里程,检测日期,检测时间,检测车号,文件名,取值点");
            foreach (string channelName in channelChinNameArray)
            {
                sb.AppendFormat(",{0}", channelName);
            }
            sb.AppendFormat(",{0}", "备注");
            sb.AppendLine();
            return sb.ToString();
        }

        public string GetResults()
        {
            StringBuilder sb = new StringBuilder();
            string str=null;
            StringBuilder sbCitHead ;

            sb.Append(GetCsvHead());

            for (int i = 0; i < CommonClass.listDIC.Count;i++ )
            {
                sbCitHead = new StringBuilder();
                sbCitHead.Append(CommonClass.listDIC[i].sTrackName);//线路名
                sbCitHead.AppendFormat(",{0}", CommonClass.listDIC[i].sTrackCode);//线路代码
                sbCitHead.AppendFormat(",{0}", CommonClass.listDIC[i].sDir);//行别
                sbCitHead.AppendFormat(",{0}", CommonClass.listDIC[i].sKmInc);//增减里程
                sbCitHead.AppendFormat(",{0}", CommonClass.listDIC[i].sDate);//检测日期
                sbCitHead.AppendFormat(",{0}", CommonClass.listDIC[i].sTime);//检测时间
                sbCitHead.AppendFormat(",{0}", CommonClass.listDIC[i].sTrain);//检测车号
                sbCitHead.AppendFormat(",{0}", Path.GetFileName(CommonClass.listDIC[i].sFilePath));//cit文件名

                str = GetValuesOfChannelPerCit(sbCitHead.ToString(),CommonClass.listDIC[i], positionArray, meterageRadius, channelChinNameArray);
                sb.Append(str);
            }

            return sb.ToString();
        }

        #endregion
    }
}
