using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using MathWorks.MATLAB.NET.Utility;
using MathWorks.MATLAB.NET.Arrays;
//using Acceleration;
using ADOX;
using System.Data.OleDb;

using AccelerateNew;

namespace InGraph.DataProcessClass
{
    public class CitProcessClass
    {
        //Matlab算法类
        //private Acceleration.AccelerationClass acclrtnCls = new Acceleration.AccelerationClass();

        private AccelerateNew.AccelerateNewClass acclrtnCls = new AccelerateNew.AccelerateNewClass();

        /// <summary>
        /// 第一断面所包含原始cit文件中的通道序号
        /// </summary>
        private List<Byte> firstSection = new List<Byte>() { 1, 2, 26, 3, 4, 5, 6, 7, 8, 9, 10 };
        // 

        /// <summary>
        /// 第二段面所包含原始cit文件中的通道序号
        /// </summary>
        private List<Byte> secondSection = new List<Byte>() { 1, 2, 26, 14, 15, 16, 17, 18, 19, 20, 21 };
        // 

        /// <summary>
        /// 第二段面所包含原始cit文件中的通道序号
        /// </summary>
        private List<Byte> thirdSection = new List<Byte>() { 1, 2, 26,  27, 28, 29, 30, 31, 32, 33, 34 };
        // 

        private List<Byte> citHeadInfoList = new List<Byte>();//存放文件头信息

        private Byte[] citChannelInfo = new Byte[65 * 37];//存放原始cit文件的通道定义信息

        private List<String> sectionFilePathList = new List<String>();

        private List<List<Byte>> sctnIdList = new List<List<Byte>>();//存放每个断面文件的通道id


        #region 变量定义
        #region cit文件的文件头信息
        /// <summary>
        /// cit文件的文件头信息
        /// </summary>
        private DataHeadInfo dhi;
        #endregion

        #region cit文件的通道定义信息
        /// <summary>
        /// cit文件的通道定义信息
        /// </summary>
        private  List<DataChannelInfo> dciL;
        #endregion
        #endregion


        #region 按照文件头结构体的读取各个数据
        /// <summary>
        /// 读取cit文件头中的文件信息信息，并返回文件头信息结构体
        /// </summary>
        /// <param name="bDataInfo">文件头中包含文件信息的120个字节 </param>
        /// <returns>文件信息结构体</returns>
        public DataHeadInfo GetDataInfoHead(byte[] bDataInfo)
        {

            DataHeadInfo dhi = new DataHeadInfo(); ;
            StringBuilder sbDataVersion = new StringBuilder();
            StringBuilder sbTrackCode = new StringBuilder();
            StringBuilder sbTrackName = new StringBuilder();
            StringBuilder sbTrain = new StringBuilder();
            StringBuilder sbDate = new StringBuilder();
            StringBuilder sbTime = new StringBuilder();

            //数据类型
            dhi.iDataType = BitConverter.ToInt32(bDataInfo, 0); //iDataType：1轨检、2动力学、3弓网，

            //1+20个字节，数据版本
            for (int i = 1; i <= (int)bDataInfo[4]; i++)
            {
                sbDataVersion.Append(UnicodeEncoding.Default.GetString(bDataInfo, 4 + i, 1));
            }
            //1+4个字节，线路代码
            for (int i = 1; i <= (int)bDataInfo[25]; i++)
            {
                sbTrackCode.Append(UnicodeEncoding.Default.GetString(bDataInfo, 25 + i, 1));
            }
            //1+20个字节，线路名
            //for (int i = 1; i <= (int)bDataInfo[30]; i++, i++)
            //{
            //    sbTrackName.Append(UnicodeEncoding.Default.GetString(bDataInfo, 30 + i, 2));
            //}
            for (int i = 1; i <= (int)bDataInfo[30]; i++)
            {
                sbTrackName.Append(UnicodeEncoding.Default.GetString(bDataInfo, 30 + i, 1));
            }

            //检测方向
            dhi.iDir = BitConverter.ToInt32(bDataInfo, 51);

            //1+20个字节，检测车号
            for (int i = 1; i <= (int)bDataInfo[55]; i++)
            {
                sbTrain.Append(UnicodeEncoding.Default.GetString(bDataInfo, 55 + i, 1));
            }
            //1+10个字节，检测日期
            for (int i = 1; i <= (int)bDataInfo[76]; i++)
            {
                sbDate.Append(UnicodeEncoding.Default.GetString(bDataInfo, 76 + i, 1));
            }
            //1+8个字节，检测时间
            for (int i = 1; i <= (int)bDataInfo[87]; i++)
            {
                sbTime.Append(UnicodeEncoding.Default.GetString(bDataInfo, 87 + i, 1));
            }

            dhi.iRunDir = BitConverter.ToInt32(bDataInfo, 96);
            dhi.iKmInc = BitConverter.ToInt32(bDataInfo, 100);
            dhi.fkmFrom = BitConverter.ToSingle(bDataInfo, 104);
            dhi.fkmTo = BitConverter.ToSingle(bDataInfo, 108);
            dhi.iSmaleRate = BitConverter.ToInt32(bDataInfo, 112);
            dhi.iChannelNumber = BitConverter.ToInt32(bDataInfo, 116);
            dhi.sDataVersion = sbDataVersion.ToString();
            dhi.sDate = DateTime.Parse(sbDate.ToString()).ToString("yyyy-MM-dd");
            dhi.sTime = DateTime.Parse(sbTime.ToString()).ToString("HH:mm:ss");
            dhi.sTrackCode = sbTrackCode.ToString();
            dhi.sTrackName = sbTrackName.ToString();
            dhi.sTrain = sbTrain.ToString();

            return dhi;
        }
        #endregion

        #region 获取单个通道定义信息
        /// <summary>
        /// 获取单个通道定义信息
        /// </summary>
        /// <param name="bDataInfo">包含通道定义信息的字节数组</param>
        /// <param name="start">起始下标</param>
        /// <returns>通道定义信息结构体对象</returns>
        private DataChannelInfo GetChannelInfo(byte[] bDataInfo, int start)
        {
            DataChannelInfo dci = new DataChannelInfo();
            StringBuilder sUnit = new StringBuilder();


            dci.sID = BitConverter.ToInt32(bDataInfo, start);//通道起点为0，导致通道id取的都是第一个通道的id，把0改为start，
            //1+20   通道英文名
            dci.sNameEn = UnicodeEncoding.Default.GetString(bDataInfo, 4 + 1 + start, (int)bDataInfo[4 + start]);
            //1+20    通道中文名
            dci.sNameCh = UnicodeEncoding.Default.GetString(bDataInfo, 25 + 1 + start, (int)bDataInfo[25 + start]);
            //通道单位 1+10
            for (int i = 1; i <= (int)bDataInfo[54 + start]; i++)
            {
                sUnit.Append(UnicodeEncoding.Default.GetString(bDataInfo, 54 + i + start, 1));
            }
            dci.sUnit = sUnit.ToString();

            //4  通道比例
            dci.fScale = BitConverter.ToSingle(bDataInfo, 46 + start);
            //4   通道基线值
            dci.fOffset = BitConverter.ToSingle(bDataInfo, 50 + start);

            return dci;

        }
        #endregion

        #region 针对通道数据的解密算法
        /// <summary>
        /// 针对通道数据的解密算法
        /// </summary>
        /// <param name="b">通道原数据</param>
        /// <returns>解密之后的通道数据</returns>
        public  byte[] ByteXORByte(byte[] b)
        {
            for (int iIndex = 0; iIndex < b.Length; iIndex++)
            {
                b[iIndex] = (byte)(b[iIndex] ^ 128);
            }
            return b;
        }
        #endregion        

        #region 生成断面的cit文件的通道定义
        /// <summary>
        /// 生成断面的cit文件的通道定义
        /// </summary>
        /// <param name="citFilePath">cit波形文件</param>
        /// <param name="sectionChannelId">断面cit文件的通道号</param>
        /// <param name="sectionFilePath">断面cit文件名</param>
        /// <returns>断面cit文件的通道定义</returns>
        private Byte[]  CreateChannelInfo(String citFilePath,List<Byte> sectionChannelId, out String sectionFilePath)
        {

            List<Byte> retByteList = new List<Byte>();
            StringBuilder sectionFileName=new StringBuilder();
            

            String citFileName = Path.GetFileName(citFilePath);
            String citFileDiretory = Path.GetDirectoryName(citFilePath);

            String channel_AB_Vt = "AB_Vt";
            String channel_AB_Lt = "AB_Lt";
            String sectionId = null;

            String[] tmpStrCitFileName = citFileName.Split(new Char[] { '.' });

            List<DataChannelInfo> sectionChannelInfoList = new List<DataChannelInfo>();
            foreach(Byte channelId in sectionChannelId)
            {
                foreach (DataChannelInfo dci in dciL)
                {
                    if (channelId == dci.sID)
                    {
                        DataChannelInfo tmpDci = new DataChannelInfo();
                        tmpDci.sID = dci.sID;
                        tmpDci.sNameCh = dci.sNameCh;
                        tmpDci.sNameEn = dci.sNameEn;
                        tmpDci.sUnit = dci.sUnit;
                        tmpDci.fOffset = dci.fOffset;
                        tmpDci.fScale = dci.fScale;
                        sectionChannelInfoList.Add(tmpDci);
                    }
                }
            }

            //找到断面号
            foreach (DataChannelInfo dci in sectionChannelInfoList)
            {
                if (dci.sNameEn.StartsWith(channel_AB_Vt) || dci.sNameEn.StartsWith(channel_AB_Lt))
                {
                    sectionId = dci.sNameEn.Substring(dci.sNameEn.Length-3, 3);
                    break;
                }
            }
            sectionFileName.Append(tmpStrCitFileName[0]);
            sectionFileName.Append(sectionId);
            sectionFileName.AppendFormat(".{0}",tmpStrCitFileName[1]);
            sectionFilePath = Path.Combine(citFileDiretory, sectionFileName.ToString());


            //List<DataChannelInfo> tmpList = new List<DataChannelInfo>();
            //foreach (DataChannelInfo dci in sectionChannelInfoList)
            //{
            //    if (dci.sNameEn.StartsWith(channel_AB_Vt) || dci.sNameEn.StartsWith(channel_AB_Lt))
            //    {
            //        DataChannelInfo dci_AB_RMS = new DataChannelInfo();

            //        dci_AB_RMS.sID = 0;
            //        String tmpChannelName = dci.sNameEn.Substring(0, dci.sNameEn.Length - 3);
            //        dci_AB_RMS.sNameEn = tmpChannelName + "_RMS" + sectionId;
            //        dci_AB_RMS.sNameCh = dci_AB_RMS.sNameEn;
            //        dci_AB_RMS.fScale = 1000;
            //        dci_AB_RMS.fOffset = 0;
            //        dci_AB_RMS.sUnit = "";

            //        tmpList.Add(dci_AB_RMS);
            //    }
            //}

            for (int i = 0; i < sectionChannelInfoList.Count;i++ )
            {
                DataChannelInfo m_dci = sectionChannelInfoList[i];
                if (m_dci.sNameEn.StartsWith(channel_AB_Vt) || m_dci.sNameEn.StartsWith(channel_AB_Lt))
                {
                    DataChannelInfo dci_AB_RMS = new DataChannelInfo();

                    dci_AB_RMS.sID = 0;
                    String tmpChannelName = m_dci.sNameEn.Substring(0, m_dci.sNameEn.Length - 3);
                    dci_AB_RMS.sNameEn = tmpChannelName + "_RMS" + sectionId;
                    dci_AB_RMS.sNameCh = dci_AB_RMS.sNameEn;
                    //dci_AB_RMS.fScale = 1000; // 严广学

                    // liyang: 
                    dci_AB_RMS.fScale = 100;
                    
                    
                    dci_AB_RMS.fOffset = 0;
                    dci_AB_RMS.sUnit = "";

                    sectionChannelInfoList[i] = dci_AB_RMS;
                }
            }

            //sectionChannelInfoList.AddRange(tmpList);


            //重新排序
            for (int i = 1; i <= sectionChannelInfoList.Count; i++)
            {
                sectionChannelInfoList[i - 1].sID = i;
                byte[] tmpArray = ChannelInfoToBytes(sectionChannelInfoList[i - 1]);
                
                retByteList.AddRange(tmpArray);
            }



            return retByteList.ToArray();
        }
        #endregion

        #region 初始化全局变量--dciL
        /// <summary>
        /// 初始化全局变量--dciL
        /// </summary>
        /// <param name="citFileName">cit波形文件</param>
        /// <returns>初始化结果</returns>
        private bool InitCIT(String citFileName)
        {
            try
            {
                using (FileStream fs = new FileStream(citFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (BinaryReader br = new BinaryReader(fs, Encoding.Default))
                    {
                        br.BaseStream.Position = 0;
                        citHeadInfoList.Clear();
                        citHeadInfoList.AddRange(br.ReadBytes(120));
                        dhi = GetDataInfoHead(citHeadInfoList.ToArray());


                        if (dhi.iDataType != 2)
                        {
                            MessageBox.Show("该波形文件不是动力学波形文件！");
                            return false;
                        }

                        byte[] bChannelData = br.ReadBytes(dhi.iChannelNumber * 65);
                        StringBuilder sbName = new StringBuilder();
                        //全局变量赋值
                        dciL = new List<DataChannelInfo>();
                        int newSId = 0;
                        for (int i = 0; i < dhi.iChannelNumber * 65; i += 65)
                        {
                            DataChannelInfo dci = GetChannelInfo(bChannelData, i);
                            dci.sID = ++newSId;
                            if (i == 65)
                            {
                                dci.fScale = 4;
                            }
                            dciL.Add(dci);
                            sbName.Append(dci.sNameEn + ",");
                        }

                        sbName.Remove(sbName.Length - 1, 1);
                        br.Close();
                        fs.Close();

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        #endregion

        #region 初始化全局变量--sctnIdList
        /// <summary>
        /// 初始化全局变量--sctnIdList
        /// </summary>
        private void InitSctnIdList()
        {            
            List<String> sctnIdListStr = new List<String>();
            
            foreach (DataChannelInfo dci in dciL)
            {
                if (dci.sNameEn.StartsWith("AB"))
                {
                    String sctnId = dci.sNameEn.Substring(dci.sNameEn.Length - 3, 3);
                    if (sctnIdListStr == null || sctnIdListStr.Count == 0)
                    {
                        sctnIdListStr.Add(sctnId);
                    }
                    else
                    {
                        if (sctnIdListStr.Contains(sctnId) == false)
                        {
                            sctnIdListStr.Add(sctnId);
                        }
                    }
                }
            }

            sctnIdList.Clear();

            //每个断面的通道顺序都是：KM，Count,Speed,各种断面通道
            foreach (String sctnId in sctnIdListStr)
            {
                List<Byte> mChannelId = new List<Byte>();

                foreach (DataChannelInfo dci in dciL)
                {
                    if (dci.sNameEn.Equals("KM"))
                    {
                        mChannelId.Add((Byte)(dci.sID));
                        break;
                    }
                }

                foreach (DataChannelInfo dci in dciL)
                {
                    if (dci.sNameEn.Equals("M"))
                    {
                        mChannelId.Add((Byte)(dci.sID));
                        break;
                    }
                }

                foreach (DataChannelInfo dci in dciL)
                {
                    if (dci.sNameEn.Equals("SPEED"))
                    {
                        mChannelId.Add((Byte)(dci.sID));
                        break;
                    }
                }

                foreach (DataChannelInfo dci in dciL)
                {
                    if (dci.sNameEn.EndsWith(sctnId))
                    {
                        mChannelId.Add((Byte)(dci.sID));
                        continue;                        
                    }                    
                }
                sctnIdList.Add(mChannelId);
            }           
        }
        #endregion

        #region 判断通道数据是否被加密
        /// <summary>
        /// 判断通道数据是否被加密
        /// </summary>
        /// <param name="dhi">文件头结构体</param>
        /// <returns>true：加密；false:不加密</returns>
        private bool IsEncrypt(DataHeadInfo dhi)
        {
            if (dhi.sDataVersion.StartsWith("3"))
            {
                return true;
            } 
            else
            {
                return false;
            }
        }
        #endregion

        private Byte[] ChannelInfoToBytes(DataChannelInfo dci)
        {
            List<Byte> channelInfoBytes = new List<Byte>(65);
            channelInfoBytes.AddRange(BitConverter.GetBytes(dci.sID));


            Byte[] tmpNameEn = UnicodeEncoding.Default.GetBytes(dci.sNameEn);
            channelInfoBytes.Add((Byte)(tmpNameEn.Length));
            channelInfoBytes.AddRange(tmpNameEn);

            if (tmpNameEn.Length < 20)
            {
                for (int i = tmpNameEn.Length; i < 20; i++)
                {
                    channelInfoBytes.Add((Byte)0);
                }
            }

            Byte[] tmpNameCh = UnicodeEncoding.Default.GetBytes(dci.sNameCh);
            channelInfoBytes.Add((Byte)(tmpNameCh.Length));
            channelInfoBytes.AddRange(tmpNameCh);
            
            if (tmpNameCh.Length < 20)
            {
                for (int i = tmpNameCh.Length; i < 20; i++)
                {
                    channelInfoBytes.Add((Byte)0);
                }                
            }

            channelInfoBytes.AddRange(BitConverter.GetBytes(dci.fScale));

            channelInfoBytes.AddRange(BitConverter.GetBytes(dci.fOffset));

            channelInfoBytes.Add((Byte)dci.sUnit.Length);
            Char[] unit = dci.sUnit.ToCharArray();
            foreach (Char c in unit)
            {
                channelInfoBytes.Add((Byte)c);
            }
            if (dci.sUnit.Length < 10)
            {
                for (int i = dci.sUnit.Length; i < 10;i++ )
                {
                    channelInfoBytes.Add((Byte)0);
                }                
            }

            
            return channelInfoBytes.ToArray();
        }


        private void ParseFrep(List<TextBox> frep_L, List<TextBox> frep_H, List<TextBox> frep_L_L, List<TextBox> frep_H_H, String channelNamePre, out float lowFrep, out float highFrep, out float lowLowFrep, out float highHighFrep)
        {

            lowLowFrep = 0;
            highHighFrep = 0;
            lowFrep = 0;
            highFrep = 0;

            foreach (TextBox tb in frep_L)
            {
                if (tb.Name.Contains(channelNamePre))
                {
                    lowFrep = float.Parse(tb.Text);
                }
            }
            foreach (TextBox tb in frep_H)
            {
                if (tb.Name.Contains(channelNamePre))
                {
                    highFrep = float.Parse(tb.Text);
                }
            }

            foreach (TextBox tb in frep_L_L)
            {
                if (tb.Name.Contains(channelNamePre))
                {
                    lowLowFrep = float.Parse(tb.Text);
                }
            }
            foreach (TextBox tb in frep_H_H)
            {
                if (tb.Name.Contains(channelNamePre))
                {
                    highHighFrep = float.Parse(tb.Text);
                }
            }
        }

        private void CreateSectionFileNew(String citFileName, List<Byte> sectionChannelIdList, Byte[] citHeadInfo, Byte[] citChannelInfo, String sectionFileName, int sampleFrep, int maxFrep,int minFreq, int windowLen, List<TextBox> frep_L, List<TextBox> frep_H,List<TextBox> frep_L_L, List<TextBox> frep_H_H)
        {
            double[] array_AB_RMS_1 = null;
            double[] array_AB_RMS_2 = null;
            double[] array_AB_RMS_3 = null;
            List<double[]> arrayDone = new List<double[]>();
            double[] arrayTmp = null;

            if (File.Exists(sectionFileName))
            {
                File.Delete(sectionFileName);
            }

            long iArray = 0;  //总的采样点数

            //先保存文件头和通道定义
            try
            {
                using (FileStream fs = new FileStream(citFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (BinaryReader br = new BinaryReader(fs, Encoding.Default))
                    {
                        using (FileStream fsSectionFile = new FileStream(sectionFileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                        {
                            using (BinaryWriter bwSectionFile = new BinaryWriter(fsSectionFile, Encoding.Default))
                            {
                                br.BaseStream.Position = 0;
                                br.ReadBytes(120);
                                br.ReadBytes(65 * dhi.iChannelNumber);
                                //br.ReadBytes(BitConverter.ToInt32(br.ReadBytes(4), 0));

                                byte[] last4 = br.ReadBytes(4);
                                byte[] zh = br.ReadBytes(BitConverter.ToInt32(last4, 0));

                                int iChannelNumberSize = dhi.iChannelNumber * 2;
                                byte[] dataArray = new byte[iChannelNumberSize];
                                iArray = (br.BaseStream.Length - br.BaseStream.Position) / iChannelNumberSize;

                                bwSectionFile.Write(citHeadInfo);

                                bwSectionFile.Write(citChannelInfo);
                                bwSectionFile.Write(last4);
                                if (zh.Length != 0)
                                {
                                    bwSectionFile.Write(zh);
                                }

                                bwSectionFile.Close();
                            }
                            fsSectionFile.Close();
                        }
                        br.Close();
                    }
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.StackTrace);
            }

            long oneTimeLength = 5000000; //一次处理的点数,500万个点，大约250公里

            for (long i = 0; i < iArray; i += oneTimeLength)
            {
                long index = i;
                long remain = iArray - i;
                long ThisTimeLength = oneTimeLength < remain ? oneTimeLength : remain ;

                arrayDone = new List<double[]>();



                //一次处理一段ThisTimeLength
                //String channel_AB_Vt = "AB_Vt";
                //String channel_AB_Lt = "AB_Lt";
                List<String> channelNames = new List<String>();

                foreach (Byte channelId in sectionChannelIdList)
                {
                    foreach (DataChannelInfo dci in dciL)
                    {
                        if (channelId == dci.sID && (dci.sNameEn.StartsWith("AB") || dci.sNameEn.StartsWith("CB") || dci.sNameEn.StartsWith("Fr")))
                        {
                            channelNames.Add(dci.sNameEn);

                            break;

                        }
                    }
                }



                try
                {
                    //array_AB_RMS_1 = CalcRMSNew(citFileName, index, ThisTimeLength, sectionChannelIdList, channelNames[0], sampleFrep, maxFrep, windowLen);
                    //array_AB_RMS_2 = CalcRMSNew(citFileName, index, ThisTimeLength, sectionChannelIdList, channelNames[1], sampleFrep, maxFrep, windowLen);
                    //array_AB_RMS_3 = CalcRMSNew(citFileName, index, ThisTimeLength, sectionChannelIdList, channelNames[2], sampleFrep, maxFrep, windowLen);

                    for (int iTmp = 0; iTmp < channelNames.Count; iTmp++)
                    {
                        String tmpChannelName = channelNames[iTmp];

                        if (tmpChannelName.StartsWith("AB"))
                        {
                            double[] arrayTmp_01 = CalcRMSNew(citFileName, index, ThisTimeLength, sectionChannelIdList, tmpChannelName, sampleFrep, maxFrep,minFreq, windowLen);
                            arrayDone.Add(arrayTmp_01);
                        }

                        
                        String channelNamePre = "Fr_Vt";
                        if (tmpChannelName.StartsWith(channelNamePre))
                        {
                            float lowFreq = 0;
                            float highFreq = 0;
                            float lowLowFreq = 0;
                            float highHighFreq = 0;

                            ParseFrep(frep_L, frep_H, frep_L_L, frep_H_H, channelNamePre, out lowFreq, out highFreq, out lowLowFreq, out highHighFreq);


                            double[] arrayTmp_02 = Filter_CB_Fr(citFileName, index, ThisTimeLength, sectionChannelIdList, tmpChannelName, sampleFrep, lowFreq, highFreq,lowLowFreq,highHighFreq);
                            arrayDone.Add(arrayTmp_02);
                        }


                        channelNamePre = "Fr_Lt";
                        if (tmpChannelName.StartsWith(channelNamePre))
                        {
                            float lowFreq = 0;
                            float highFreq = 0;
                            float lowLowFreq = 0;
                            float highHighFreq = 0;

                            ParseFrep(frep_L, frep_H, frep_L_L, frep_H_H, channelNamePre, out lowFreq, out highFreq, out lowLowFreq, out highHighFreq);

                            double[] arrayTmp_03 = Filter_CB_Fr(citFileName, index, ThisTimeLength, sectionChannelIdList, tmpChannelName, sampleFrep, lowFreq, highFreq, lowLowFreq, highHighFreq);
                            arrayDone.Add(arrayTmp_03);
                        }


                        channelNamePre = "CB_Lt";
                        if (tmpChannelName.StartsWith(channelNamePre))
                        {
                            float lowFreq = 0;
                            float highFreq = 0;
                            float lowLowFreq = 0;
                            float highHighFreq = 0;

                            ParseFrep(frep_L, frep_H, frep_L_L, frep_H_H, channelNamePre, out lowFreq, out highFreq, out lowLowFreq, out highHighFreq);

                            double[] arrayTmp_04 = Filter_CB_Fr(citFileName, index, ThisTimeLength, sectionChannelIdList, tmpChannelName, sampleFrep, lowFreq, highFreq, lowLowFreq, highHighFreq);
                            arrayDone.Add(arrayTmp_04);
                        }


                        channelNamePre = "CB_Vt";
                        if (tmpChannelName.StartsWith(channelNamePre))
                        {
                            float lowFreq = 0;
                            float highFreq = 0;
                            float lowLowFreq = 0;
                            float highHighFreq = 0;

                            ParseFrep(frep_L, frep_H, frep_L_L, frep_H_H, channelNamePre, out lowFreq, out highFreq, out lowLowFreq, out highHighFreq);

                            double[] arrayTmp_05 = Filter_CB_Fr(citFileName, index, ThisTimeLength, sectionChannelIdList, tmpChannelName, sampleFrep, lowFreq, highFreq, lowLowFreq, highHighFreq);
                            arrayDone.Add(arrayTmp_05);
                        }


                        channelNamePre = "CB_Lg";
                        if (tmpChannelName.StartsWith(channelNamePre))
                        {
                            float lowFreq = 0;
                            float highFreq = 0;
                            float lowLowFreq = 0;
                            float highHighFreq = 0;

                            ParseFrep(frep_L, frep_H, frep_L_L, frep_H_H, channelNamePre, out lowFreq, out highFreq, out lowLowFreq, out highHighFreq);

                            double[] arrayTmp_06 = Filter_CB_Fr(citFileName, index, ThisTimeLength, sectionChannelIdList, tmpChannelName, sampleFrep, lowFreq, highFreq, lowLowFreq, highHighFreq);
                            arrayDone.Add(arrayTmp_06);
                        }

                    }
                }
                catch (System.Exception ex)
                {
                    //2017-08-15注释
                    //MessageBox.Show(ex.Message);
                    //MessageBox.Show(ex.StackTrace);
                    //MessageBox.Show(ex.Source);
                }


                try
                {
                    using (FileStream fs = new FileStream(citFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (BinaryReader br = new BinaryReader(fs, Encoding.Default))
                        {
                            using (FileStream fsSectionFile = new FileStream(sectionFileName, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
                            {
                                using (BinaryWriter bwSectionFile = new BinaryWriter(fsSectionFile, Encoding.Default))
                                {
                                    br.BaseStream.Position = 0;
                                    br.ReadBytes(120);
                                    br.ReadBytes(65 * dhi.iChannelNumber);
                                    //br.ReadBytes(BitConverter.ToInt32(br.ReadBytes(4), 0));

                                    byte[] last4 = br.ReadBytes(4);
                                    byte[] zh = br.ReadBytes(BitConverter.ToInt32(last4, 0));

                                    int iChannelNumberSize = dhi.iChannelNumber * 2;
                                    byte[] dataArray = new byte[iChannelNumberSize];
                                    long iArrayLen = (br.BaseStream.Length - br.BaseStream.Position) / iChannelNumberSize;

                                    List<Byte> dataList = new List<Byte>();
                                    short tmpRmsData = 0;
                                    Byte[] tmpBytes = new Byte[2];

                                    //bwSectionFile.Write(citHeadInfo);
                                    //bwSectionFile.Write(citChannelInfo);
                                    //bwSectionFile.Write(last4);
                                    //if (zh.Length != 0)
                                    //{
                                    //    bwSectionFile.Write(zh);
                                    //}

                                    iArrayLen = ThisTimeLength;
                                    br.BaseStream.Position = br.BaseStream.Position + iChannelNumberSize * index;

                                    for (int k = 0; k < iArrayLen; k++)
                                    {
                                        dataArray = br.ReadBytes(iChannelNumberSize);

                                        //每隔六个数采样一个数据
                                        if (k % 6 != 0)
                                        {
                                            continue;
                                        }

                                        //foreach (Byte channelId in sectionChannelIdList)
                                        //{
                                        //    foreach (DataChannelInfo tmpDci in dciL)
                                        //    {
                                        //        if (channelId == tmpDci.sID)
                                        //        {
                                        //            int j = tmpDci.sID - 1;
                                        //            dataList.Add(dataArray[j * 2]);
                                        //            dataList.Add(dataArray[j * 2 + 1]);

                                        //            break;
                                        //        }
                                        //    }
                                        //}

                                        for (int j = 0; j < sectionChannelIdList.Count; j++)
                                        {
                                            if (j == 3)
                                            {
                                                break;
                                            }
                                            foreach (DataChannelInfo tmpDci in dciL)
                                            {
                                                if (sectionChannelIdList[j] == tmpDci.sID)
                                                {
                                                    int m = tmpDci.sID - 1;
                                                    dataList.Add(dataArray[m * 2]);
                                                    dataList.Add(dataArray[m * 2 + 1]);

                                                    break;
                                                }
                                            }
                                        }

                                        if (IsEncrypt(dhi))
                                        {
                                            //tmpRmsData = (short)(array_AB_RMS_1[k] * 1000 + 0);
                                            //tmpBytes = ByteXORByte(BitConverter.GetBytes(tmpRmsData));
                                            //dataList.AddRange(tmpBytes);

                                            //tmpRmsData = (short)(array_AB_RMS_2[k] * 1000 + 0);
                                            //tmpBytes = ByteXORByte(BitConverter.GetBytes(tmpRmsData));
                                            //dataList.AddRange(tmpBytes);

                                            //tmpRmsData = (short)(array_AB_RMS_3[k] * 1000 + 0);
                                            //tmpBytes = ByteXORByte(BitConverter.GetBytes(tmpRmsData));
                                            //dataList.AddRange(tmpBytes);

                                            for (int iTmp = 0; i < channelNames.Count; iTmp++)
                                            {
                                                String tmpChannelName = channelNames[iTmp];

                                                if (tmpChannelName.StartsWith("AB"))
                                                {
                                                    //tmpRmsData = (short)(arrayDone[iTmp][k] * 1000 + 0); // 严广学
                                                    // liyang:
                                                    tmpRmsData = (short)(arrayDone[iTmp][k] * 100 + 0);

                                                    tmpBytes = ByteXORByte(BitConverter.GetBytes(tmpRmsData));
                                                    dataList.AddRange(tmpBytes);
                                                }
                                                else
                                                {
                                                    foreach (DataChannelInfo dci in dciL)
                                                    {
                                                        if (dci.sNameEn == tmpChannelName)
                                                        {
                                                            tmpRmsData = (short)(arrayDone[iTmp][k] * dci.fScale + dci.fOffset);
                                                            tmpBytes = ByteXORByte(BitConverter.GetBytes(tmpRmsData));
                                                            dataList.AddRange(tmpBytes);
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            //tmpRmsData = (short)(array_AB_RMS_1[k] * 1000 + 0);
                                            //dataList.AddRange(BitConverter.GetBytes(tmpRmsData));

                                            //tmpRmsData = (short)(array_AB_RMS_2[k] * 1000 + 0);
                                            //dataList.AddRange(BitConverter.GetBytes(tmpRmsData));

                                            //tmpRmsData = (short)(array_AB_RMS_3[k] * 1000 + 0);
                                            //dataList.AddRange(BitConverter.GetBytes(tmpRmsData));

                                            for (int iTmp = 0; iTmp < channelNames.Count; iTmp++)
                                            {
                                                String tmpChannelName = channelNames[iTmp];

                                                if (tmpChannelName.StartsWith("AB"))
                                                {
                                                    //tmpRmsData = (short)(arrayDone[iTmp][k] * 1000 + 0); // 严广学
                                                   
                                                    //liyang:
                                                    tmpRmsData = (short)(arrayDone[iTmp][k] * 100 + 0);

                                                    //liyang: test
                                                    /*if (tmpRmsData < 0)
                                                    {
                                                        double tttt = arrayDone[iTmp][k];
                                                        double t2 = arrayDone[iTmp][k] * 1000 + 0;
                                                        int a = 4;
                                                        int b = a + 5;
                                                    }
                                                    else
                                                    {
                                                        double tttt = arrayDone[iTmp][k];
                                                        double t2 = arrayDone[iTmp][k] * 1000 + 0;
                                                        int a = 4;
                                                        int b = a + 5;

                                                         
                                                    }*/
                                                    // end test




                                                    dataList.AddRange(BitConverter.GetBytes(tmpRmsData));
                                                }
                                                else
                                                {
                                                    foreach (DataChannelInfo dci in dciL)
                                                    {
                                                        if (dci.sNameEn == tmpChannelName)
                                                        {
                                                            tmpRmsData = (short)(arrayDone[iTmp][k] * dci.fScale + dci.fOffset);
                                                            dataList.AddRange(BitConverter.GetBytes(tmpRmsData));
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        bwSectionFile.Write(dataList.ToArray());
                                        bwSectionFile.Flush();
                                        fsSectionFile.Flush();

                                        Application.DoEvents();

                                        dataList.Clear();

                                    }

                                    bwSectionFile.Close();
                                }
                                fsSectionFile.Close();
                            }
                            br.Close();
                        }
                        fs.Close();

                    }
                }
                catch (Exception ex)
                {
                    //2017-08-15注释
                    //MessageBox.Show(ex.Message);
                    //MessageBox.Show(ex.StackTrace);
                }

                array_AB_RMS_1 = null;
                array_AB_RMS_2 = null;
                array_AB_RMS_3 = null;
            }

            array_AB_RMS_1 = null;
            array_AB_RMS_2 = null;
            array_AB_RMS_3 = null;
        }


        //private void CreateSectionFile(String citFileName, List<Byte> sectionChannelIdList, Byte[] citHeadInfo, Byte[] citChannelInfo, String sectionFileName, int sampleFrep, int maxFrep, int windowLen)
        //{
        //    double[] array_AB_RMS_1 = null;
        //    double[] array_AB_RMS_2 = null;
        //    double[] array_AB_RMS_3 = null;

        //    String channel_AB_Vt = "AB_Vt";
        //    String channel_AB_Lt = "AB_Lt";
        //    List<String> channelName_AB_RMS = new List<String>();

        //    foreach (Byte channelId in sectionChannelIdList)
        //    {
        //        foreach (DataChannelInfo dci in dciL)
        //        {
        //            if (channelId == dci.sID && (dci.sNameEn.StartsWith(channel_AB_Vt) || dci.sNameEn.StartsWith(channel_AB_Lt)))
        //            {
        //                channelName_AB_RMS.Add(dci.sNameEn); 

        //                break;

        //            }
        //        }
        //    }



        //    try
        //    {
        //        array_AB_RMS_1 = CalcRMS(citFileName, sectionChannelIdList, channelName_AB_RMS[0], sampleFrep, maxFrep, windowLen);
        //        array_AB_RMS_2 = CalcRMS(citFileName, sectionChannelIdList, channelName_AB_RMS[1], sampleFrep, maxFrep, windowLen);
        //        array_AB_RMS_3 = CalcRMS(citFileName, sectionChannelIdList, channelName_AB_RMS[2], sampleFrep, maxFrep, windowLen);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //        MessageBox.Show(ex.StackTrace);
        //    }


        //    try
        //    {
        //        using (FileStream fs = new FileStream(citFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        //        {
        //            using (BinaryReader br = new BinaryReader(fs, Encoding.Default))
        //            {
        //                using (FileStream fsSectionFile = new FileStream(sectionFileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
        //                {
        //                    using (BinaryWriter bwSectionFile = new BinaryWriter(fsSectionFile, Encoding.Default))
        //                    {
        //                        br.BaseStream.Position = 0;
        //                        br.ReadBytes(120);
        //                        br.ReadBytes(65 * dhi.iChannelNumber);
        //                        //br.ReadBytes(BitConverter.ToInt32(br.ReadBytes(4), 0));

        //                        byte[] last4 = br.ReadBytes(4);
        //                        byte[] zh = br.ReadBytes(BitConverter.ToInt32(last4, 0));

        //                        int iChannelNumberSize = dhi.iChannelNumber * 2;
        //                        byte[] dataArray = new byte[iChannelNumberSize];
        //                        long iArray = (br.BaseStream.Length - br.BaseStream.Position) / iChannelNumberSize;

        //                        List<Byte> dataList = new List<Byte>();
        //                        short tmpRmsData = 0;
        //                        Byte[] tmpBytes = new Byte[2];

        //                        bwSectionFile.Write(citHeadInfo);



        //                        bwSectionFile.Write(citChannelInfo);
        //                        bwSectionFile.Write(last4);
        //                        if (zh.Length != 0)
        //                        {
        //                            bwSectionFile.Write(zh);
        //                        }

        //                        for (int i = 0; i < iArray; i++)
        //                        {
        //                            dataArray = br.ReadBytes(iChannelNumberSize);

        //                            foreach (Byte channelId in sectionChannelIdList)
        //                            {
        //                                foreach (DataChannelInfo tmpDci in dciL)
        //                                {
        //                                    if (channelId == tmpDci.sID)
        //                                    {
        //                                        int j = tmpDci.sID - 1;
        //                                        dataList.Add(dataArray[j * 2]);
        //                                        dataList.Add(dataArray[j * 2 + 1]);

        //                                        break;
        //                                    }
        //                                }
        //                            }

        //                            if (IsEncrypt(dhi))
        //                            {
        //                                tmpRmsData = (short)(array_AB_RMS_1[i] * 1000 + 0);
        //                                tmpBytes = ByteXORByte(BitConverter.GetBytes(tmpRmsData));
        //                                dataList.AddRange(tmpBytes);

        //                                tmpRmsData = (short)(array_AB_RMS_2[i] * 1000 + 0);
        //                                tmpBytes = ByteXORByte(BitConverter.GetBytes(tmpRmsData));
        //                                dataList.AddRange(tmpBytes);

        //                                tmpRmsData = (short)(array_AB_RMS_3[i] * 1000 + 0);
        //                                tmpBytes = ByteXORByte(BitConverter.GetBytes(tmpRmsData));
        //                                dataList.AddRange(tmpBytes);
        //                            }
        //                            else
        //                            {
        //                                tmpRmsData = (short)(array_AB_RMS_1[i] * 1000 + 0);
        //                                dataList.AddRange(BitConverter.GetBytes(tmpRmsData));

        //                                tmpRmsData = (short)(array_AB_RMS_2[i] * 1000 + 0);
        //                                dataList.AddRange(BitConverter.GetBytes(tmpRmsData));

        //                                tmpRmsData = (short)(array_AB_RMS_3[i] * 1000 + 0);
        //                                dataList.AddRange(BitConverter.GetBytes(tmpRmsData));
        //                            }

        //                            bwSectionFile.Write(dataList.ToArray());
        //                            bwSectionFile.Flush();
        //                            fsSectionFile.Flush();

        //                            Application.DoEvents();

        //                            dataList.Clear();

        //                        }

        //                        bwSectionFile.Close();
        //                    }
        //                    fsSectionFile.Close();
        //                }
        //                br.Close();
        //            }
        //            fs.Close();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //        MessageBox.Show(ex.StackTrace);
        //    }

        //    array_AB_RMS_1 = null;
        //    array_AB_RMS_2 = null;
        //    array_AB_RMS_3 = null;


        //}

        //private double[] CalcRMS(String fileName, List<Byte> sectionChannelId, String channelName, int sampleFreq, int maxFreq, int windowLen)
        //{
        //    double[] dSourceArray = null;
        //    Byte mSectionChannelId = 0;
        //    //查找通道号。
        //    foreach (Byte channelId in sectionChannelId)
        //    {
        //        foreach (DataChannelInfo dci in dciL)
        //        {
        //            if (channelId == dci.sID && dci.sNameEn.StartsWith(channelName))
        //            {
        //                mSectionChannelId = channelId;

        //                break;
        //            }
        //        }
                
        //    }

        //    try
        //    {
        //        using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        //        {
        //            using (BinaryReader br = new BinaryReader(fs, Encoding.Default))
        //            {
        //                br.BaseStream.Position = 0;
        //                br.ReadBytes(120);
        //                br.ReadBytes(65 * dhi.iChannelNumber);
        //                br.ReadBytes(BitConverter.ToInt32(br.ReadBytes(4), 0));

        //                int iChannelNumberSize = dhi.iChannelNumber * 2;
        //                byte[] b = new byte[iChannelNumberSize];
        //                long iArray = (br.BaseStream.Length - br.BaseStream.Position) / iChannelNumberSize;
        //                dSourceArray = new double[iArray];
        //                for (int i = 0; i < iArray; i++)
        //                {
        //                    b = br.ReadBytes(iChannelNumberSize);
        //                    //double fGL = (BitConverter.ToInt16(b, 0) / dciL[0].fScale);
        //                    //fGL += ((BitConverter.ToInt16(b, 2) / dciL[0].fScale / 4) / 1000.0);
        //                    //fReturnArray[0, i] = fGL;
        //                    double fGL = (BitConverter.ToInt16(b, (mSectionChannelId - 1) * 2) / dciL[mSectionChannelId - 1].fScale + dciL[mSectionChannelId - 1].fOffset);

        //                    dSourceArray[i] = Math.Round(fGL,4);
        //                }


        //                br.Close();
        //                fs.Close();
        //            }
        //        }
        //    }
        //    catch (System.Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //        MessageBox.Show(ex.StackTrace);
        //    }

        //    double[,] rmsArray = null;
        //    Int32 dSourceArrayLen = dSourceArray.Length;
        //    Int32 mArrayLen = 1024 * 1024 * 1;//matlab处理数组的长度
        //    Int32 mArrayNum = 0;

        //    double[] dResultArray = new double[dSourceArrayLen];


        //    //针对有时候仪器故障，通道数据全为0，
        //    //通道数据都为零时，求有效值会出错
        //    double sum_dSourceArray = 0;
        //    for (int i = 0; i < dSourceArrayLen; i++)
        //    {
        //        sum_dSourceArray += Math.Abs(dSourceArray[i]);
        //    }

        //    if (sum_dSourceArray <= 300)
        //    {
        //        for (int i = 0; i < dSourceArrayLen; i++)
        //        {
        //            dResultArray[i] = 0;
        //        }

        //        return dResultArray;
        //    }


        //    if (dSourceArrayLen%mArrayLen != 0)
        //    {
        //        mArrayNum = dSourceArrayLen / mArrayLen + 1;
        //    } 
        //    else
        //    {
        //        mArrayNum = dSourceArrayLen / mArrayLen;
        //    }
        //    for (int i = 1; i <= mArrayNum;i++ )
        //    {
        //        try
        //        {
        //            Int32 tmpArrayLen = 0;
        //            Int32 tmpArrayStartIndex = (i - 1) * mArrayLen;
        //            if (i == mArrayNum)
        //            {
        //                tmpArrayLen = dSourceArrayLen - mArrayLen * (i - 1);
        //            }
        //            else
        //            {
        //                tmpArrayLen = mArrayLen;
        //            }

        //            double[] tmpDataArray = new double[tmpArrayLen];
        //            Array.Copy(dSourceArray, tmpArrayStartIndex, tmpDataArray, 0, tmpArrayLen);
                    

        //            MWNumericArray dataArray = new MWNumericArray(tmpDataArray);

        //            MWNumericArray fsArray = sampleFreq;
        //            MWNumericArray filterFreqArray = maxFreq;
        //            MWNumericArray windowLenArray = windowLen;



        //            MWNumericArray resultArray;

        //            resultArray = (MWNumericArray)acclrtnCls.sub_calculate_moving_RMS_on_axlebox_acc(dataArray, fsArray, filterFreqArray, windowLenArray);
        //            rmsArray = (double[,])(resultArray.ToArray());

        //            for (int j = 0; j < rmsArray.GetLength(1);j++ )
        //            {
        //                dResultArray[tmpArrayStartIndex+j]=rmsArray[0,j];
        //            }

        //            dataArray = null;
        //            tmpDataArray = null;
        //        }
        //        catch (System.Exception ex)
        //        {
        //            MessageBox.Show(ex.Message);
        //            MessageBox.Show(ex.StackTrace);
        //        }

        //    }

        //    return dResultArray;
        //}

        private double[] CalcRMSNew(String fileName,long startPointIndex,long pointLen, List<Byte> sectionChannelId, String channelName, int sampleFreq, int maxFreq,int minFreq, int windowLen)
        {
            double[] dSourceArray = null;
            //2017-06-24添加 里程、速度
            double[] dSourceArray_Mile = null;
            double[] dSourceArray_Speed = null;

            Byte mSectionChannelId = 0;
            //查找通道号。
            foreach (Byte channelId in sectionChannelId)
            {
                foreach (DataChannelInfo dci in dciL)
                {
                    if (channelId == dci.sID && dci.sNameEn.StartsWith(channelName))
                    {
                        mSectionChannelId = channelId;

                        break;
                    }
                }

            }

            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (BinaryReader br = new BinaryReader(fs, Encoding.Default))
                    {
                        br.BaseStream.Position = 0;
                        br.ReadBytes(120);
                        br.ReadBytes(65 * dhi.iChannelNumber);
                        br.ReadBytes(BitConverter.ToInt32(br.ReadBytes(4), 0));

                        int iChannelNumberSize = dhi.iChannelNumber * 2;
                        byte[] b = new byte[iChannelNumberSize];
                        long iArray = (br.BaseStream.Length - br.BaseStream.Position) / iChannelNumberSize;

                        //把文件指针移到该段数据的起始点
                        br.BaseStream.Position = br.BaseStream.Position + dhi.iChannelNumber * 2 * startPointIndex;
                        //一次获取一段长度
                        iArray = pointLen;

                        dSourceArray = new double[iArray];
                        //2017-06-24添加
                        dSourceArray_Mile = new double[iArray];
                        dSourceArray_Speed = new double[iArray];

                        for (int i = 0; i < iArray; i++)
                        {
                            b = br.ReadBytes(iChannelNumberSize);
                            //double fGL = (BitConverter.ToInt16(b, 0) / dciL[0].fScale);
                            //fGL += ((BitConverter.ToInt16(b, 2) / dciL[0].fScale / 4) / 1000.0);
                            //fReturnArray[0, i] = fGL;
                            double fGL = (BitConverter.ToInt16(b, (mSectionChannelId - 1) * 2) / dciL[mSectionChannelId - 1].fScale + dciL[mSectionChannelId - 1].fOffset);

                            dSourceArray[i] = Math.Round(fGL, 4);


                            //2017-06-24添加 
                            //里程信息
                            short km = BitConverter.ToInt16(b, 0);
                            short m = BitConverter.ToInt16(b, 2);
                            //单位为公里
                            float mile = km + (float)m / 1000;
                            dSourceArray_Mile[i] = mile;

                            double speed = (BitConverter.ToInt16(b, (3 - 1) * 2) / dciL[3 - 1].fScale + dciL[3 - 1].fOffset);
                            dSourceArray_Speed[i] = speed;
                        }


                        br.Close();
                        fs.Close();
                    }
                }
            }
            catch (System.Exception ex)
            {
                //2017-08-15注释
                //MessageBox.Show(ex.Message);
                //MessageBox.Show(ex.StackTrace);
                //MessageBox.Show(ex.Source);
            }

            double[,] rmsArray = null;
            Int32 dSourceArrayLen = dSourceArray.Length;
            Int32 mArrayLen = 1024 * 1024 * 1;//matlab处理数组的长度
            Int32 mArrayNum = 0;

            double[] dResultArray = new double[dSourceArrayLen];


            //针对有时候仪器故障，通道数据全为0，
            //通道数据都为零时，求有效值会出错
            double sum_dSourceArray = 0;
            for (int i = 0; i < dSourceArrayLen; i++)
            {
                sum_dSourceArray += Math.Abs(dSourceArray[i]);
            }

            if (sum_dSourceArray <= 300)
            {
                for (int i = 0; i < dSourceArrayLen; i++)
                {
                    dResultArray[i] = 0;
                }

                return dResultArray;
            }


            if (dSourceArrayLen % mArrayLen != 0)
            {
                mArrayNum = dSourceArrayLen / mArrayLen + 1;
            }
            else
            {
                mArrayNum = dSourceArrayLen / mArrayLen;
            }
            for (int i = 1; i <= mArrayNum; i++)
            {
                try
                {
                    Int32 tmpArrayLen = 0;
                    Int32 tmpArrayStartIndex = (i - 1) * mArrayLen;
                    if (i == mArrayNum)
                    {
                        tmpArrayLen = dSourceArrayLen - mArrayLen * (i - 1);
                    }
                    else
                    {
                        tmpArrayLen = mArrayLen;
                    }

                    double[] tmpDataArray = new double[tmpArrayLen];
                    Array.Copy(dSourceArray, tmpArrayStartIndex, tmpDataArray, 0, tmpArrayLen);

                    //添加2017-06-24添加
                    //里程
                    double[] tmpDataArray_mile = new double[tmpArrayLen];
                    Array.Copy(dSourceArray_Mile, tmpArrayStartIndex, tmpDataArray_mile, 0, tmpArrayLen);
                    //速度
                    double[] tmpDataArray_speed = new double[tmpArrayLen];
                    Array.Copy(dSourceArray_Speed, tmpArrayStartIndex, tmpDataArray_speed, 0, tmpArrayLen);

                    MWNumericArray dataArray = new MWNumericArray(tmpDataArray);
                    //添加2017-06-24
                    MWNumericArray dataArray_mile = new MWNumericArray(tmpDataArray_mile);
                    MWNumericArray dataArray_speed = new MWNumericArray(tmpDataArray_speed);

                    MWNumericArray fsArray = sampleFreq;
                    MWNumericArray filterFreqArray = maxFreq;
                    //2017-06-24添加 下线频率
                    MWNumericArray filterFreqArray_L = minFreq;

                    MWNumericArray windowLenArray = windowLen;

                    //string path = @"H:\mileData.txt";
                    //using (StreamWriter sw = new StreamWriter(path))
                    //{
                    //    for (int k = 0; k < tmpDataArray_mile.Length; k++)
                    //    {
                    //        sw.WriteLine(tmpDataArray_mile[k]);
                    //    }
                    //}
                    //path = @"H:\speedData.txt";
                    //using (StreamWriter sw = new StreamWriter(path))
                    //{
                    //    for (int k = 0; k < tmpDataArray_speed.Length; k++)
                    //    {
                    //        sw.WriteLine(tmpDataArray_speed[k]);
                    //    }
                    //}
                    //path = @"H:\Data.txt";
                    //using (StreamWriter sw = new StreamWriter(path))
                    //{
                    //    for (int k = 0; k < tmpDataArray.Length; k++)
                    //    {
                    //        sw.WriteLine(tmpDataArray[k]);
                    //    }
                    //}

                    MWNumericArray resultArray;

                    //resultArray = (MWNumericArray)acclrtnCls.sub_calculate_moving_RMS_on_axlebox_acc(dataArray, fsArray, filterFreqArray, windowLenArray);


                    //2017-06-24添加
                    resultArray = (MWNumericArray)acclrtnCls.sub_calculate_moving_RMS_on_axlebox_acc(dataArray_mile, dataArray, dataArray_speed, fsArray, windowLenArray, filterFreqArray_L, filterFreqArray);

                    if (resultArray.IsEmpty || !resultArray.IsNumericArray)
                    {
                        int b = 0;
                    }

                    rmsArray = (double[,])(resultArray.ToArray(MWArrayComponent.Real));

                    for (int j = 0; j < rmsArray.GetLength(1); j++)
                    {
                        dResultArray[tmpArrayStartIndex + j] = rmsArray[0, j];
                    }

                    resultArray = null;
                    rmsArray = null;
                    dataArray = null;
                    tmpDataArray = null;
                }
                catch (System.Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                    //MessageBox.Show(ex.StackTrace);
                    //MessageBox.Show(ex.Source);
                }

            }

            dSourceArray = null;

            return dResultArray;
        }

        #region 车体和构架滤波函数
        /// <summary>
        /// 车体和构架滤波函数
        /// </summary>
        /// <param name="fileName">cit全路径文件名</param>
        /// <param name="startPointIndex">数据起点位置</param>
        /// <param name="pointLen">数据长度</param>
        /// <param name="sectionChannelId">需要滤波的通道id列表</param>
        /// <param name="channelName">通道名</param>
        /// <param name="filterFreq">采样频率：2000</param>
        /// <param name="lowFreq">通带下限</param>
        /// <param name="HighFreq">通带上限</param>
        /// <param name="lowLowFreq">阻带下限</param>
        /// <param name="highHighFreq">阻带上限</param>
        /// <returns>滤波后的数据队列</returns>
        private double[] Filter_CB_Fr(String fileName, long startPointIndex, long pointLen, List<Byte> sectionChannelId, String channelName, int filterFreq, float lowFreq, float HighFreq, float lowLowFreq, float highHighFreq)
        {
            double[] dSourceArray = null;
            Byte mSectionChannelId = 0;
            //查找通道号。
            foreach (Byte channelId in sectionChannelId)
            {
                foreach (DataChannelInfo dci in dciL)
                {
                    if (channelId == dci.sID && dci.sNameEn.StartsWith(channelName))
                    {
                        mSectionChannelId = channelId;

                        break;
                    }
                }

            }

            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (BinaryReader br = new BinaryReader(fs, Encoding.Default))
                    {
                        br.BaseStream.Position = 0;
                        br.ReadBytes(120);
                        br.ReadBytes(65 * dhi.iChannelNumber);
                        br.ReadBytes(BitConverter.ToInt32(br.ReadBytes(4), 0));

                        int iChannelNumberSize = dhi.iChannelNumber * 2;
                        byte[] b = new byte[iChannelNumberSize];
                        long iArray = (br.BaseStream.Length - br.BaseStream.Position) / iChannelNumberSize;

                        //把文件指针移到该段数据的起始点
                        br.BaseStream.Position = br.BaseStream.Position + dhi.iChannelNumber * 2 * startPointIndex;
                        //一次获取一段长度
                        iArray = pointLen;

                        dSourceArray = new double[iArray];
                        for (int i = 0; i < iArray; i++)
                        {
                            b = br.ReadBytes(iChannelNumberSize);
                            //double fGL = (BitConverter.ToInt16(b, 0) / dciL[0].fScale);
                            //fGL += ((BitConverter.ToInt16(b, 2) / dciL[0].fScale / 4) / 1000.0);
                            //fReturnArray[0, i] = fGL;
                            double fGL = (BitConverter.ToInt16(b, (mSectionChannelId - 1) * 2) / dciL[mSectionChannelId - 1].fScale + dciL[mSectionChannelId - 1].fOffset);

                            dSourceArray[i] = Math.Round(fGL, 6);
                        }


                        br.Close();
                        fs.Close();
                    }
                }
            }
            catch (System.Exception ex)
            {
                //2017-08-15注释
                //MessageBox.Show(ex.Message);
                //MessageBox.Show(ex.StackTrace);
                //MessageBox.Show(ex.Source);
            }

            //for  test
            //StringBuilder sb = new StringBuilder();
            //for (int i = 1; i < dSourceArray.Length;i++ )
            //{
            //    sb.AppendFormat("{0},", dSourceArray[i]);
            //    if (i%100 == 0)
            //    {
            //        sb.Append("\n");
            //    }
            //}
            //String txtFilePath = String.Format(@"D:\test-{0}-{1}-{2}.txt", channelName, lowFreq, HighFreq);
            //File.WriteAllText(txtFilePath, sb.ToString(), Encoding.Default);
            //sb.Clear();
            

            double[,] rmsArray = null;
            Int32 dSourceArrayLen = dSourceArray.Length;
            Int32 mArrayLen = 1024 * 1024 * 1;//matlab处理数组的长度
            Int32 mArrayNum = 0;

            //double[] dResultArray = new double[dSourceArrayLen];
            double[] dResultArray = new double[dSourceArrayLen];


            //针对有时候仪器故障，通道数据全为0，
            //通道数据都为零时，求有效值会出错
            double sum_dSourceArray = 0;
            for (int i = 0; i < dSourceArrayLen; i++)
            {
                sum_dSourceArray += Math.Abs(dSourceArray[i]);
            }

            if (sum_dSourceArray <= 300)
            {
                for (int i = 0; i < dSourceArrayLen; i++)
                {
                    dResultArray[i] = 0;
                }

                return dResultArray;
            }


            if (dSourceArrayLen % mArrayLen != 0)
            {
                mArrayNum = dSourceArrayLen / mArrayLen + 1;
            }
            else
            {
                mArrayNum = dSourceArrayLen / mArrayLen;
            }
            for (int i = 1; i <= mArrayNum; i++)
            {
                try
                {
                    Int32 tmpArrayLen = 0;
                    Int32 tmpArrayStartIndex = (i - 1) * mArrayLen;
                    if (i == mArrayNum)
                    {
                        tmpArrayLen = dSourceArrayLen - mArrayLen * (i - 1);
                    }
                    else
                    {
                        tmpArrayLen = mArrayLen;
                    }

                    double[] tmpDataArray = new double[tmpArrayLen];
                    Array.Copy(dSourceArray, tmpArrayStartIndex, tmpDataArray, 0, tmpArrayLen);

                    //if (i == 4)
                    //{
                    //    string path = @"H:\bat_output.txt";
                    //    using (StreamWriter sw = new StreamWriter(path))
                    //    {
                    //        for (int s = 0; s < tmpDataArray.Length; s++)
                    //        {
                    //            sw.WriteLine(tmpDataArray[s]);
                    //        }
                    //    }
                    //}

                    MWNumericArray dataArray = new MWNumericArray(tmpDataArray);


                    MWNumericArray filterFreqLow = (double)lowFreq; //通带下限
                    MWNumericArray filterFreqHigh = (double)HighFreq; //通带上限
                    MWNumericArray m_filterFreq = (double)filterFreq;//采样频率
                    MWNumericArray filterFrepLowLow = (double)lowLowFreq;//阻带下限
                    MWNumericArray filterFrepHighHigh = (double)highHighFreq;//阻带上限



                    MWNumericArray resultArray = null;

                    ////resultArray = (MWNumericArray)acclrtnCls.sub_calculate_moving_RMS_on_axlebox_acc(dataArray, fsArray, filterFreqArray, windowLenArray);

                    //resultArray = (MWNumericArray)acclrtnCls.sub_filter(m_filterFreq, lowFreq, HighFreq, dataArray);


                    //2017-06-23注释 新的dll参数重载不匹配
                    //resultArray = (MWNumericArray)acclrtnCls.sub_filter_acc(m_filterFreq, lowFreq, HighFreq,lowLowFreq,highHighFreq, dataArray);

                    resultArray = (MWNumericArray)acclrtnCls.sub_filter_acc(m_filterFreq, lowFreq, HighFreq, dataArray);


                    if (resultArray.IsEmpty || !resultArray.IsNumericArray)
                    {
                        int b = 0;
                    }

                    rmsArray = (double[,])(resultArray.ToArray(MWArrayComponent.Real));

                    for (int j = 0; j < rmsArray.GetLength(1); j++)
                    {
                        dResultArray[tmpArrayStartIndex + j] = Math.Round(rmsArray[0, j],6);
                    }

                    resultArray = null;
                    rmsArray = null;
                    dataArray = null;
                    tmpDataArray = null;

                }
                catch (System.Exception ex)
                {
                    //2017-08-15注释
                    //MessageBox.Show(ex.Message);
                    //MessageBox.Show(ex.StackTrace);
                    //MessageBox.Show(ex.Source);
                }

            }

            dSourceArray = null;

            return dResultArray;
        }
        #endregion

        #region 轴箱加速度求50米区段大值---调用matlab算法
        /// <summary>
        /// 轴箱加速度求50米区段大值---调用matlab算法
        /// </summary>
        /// <param name="sectionFilePath">加速度断面cit文件的全路径文件名</param>
        /// <param name="segmentLen">区段的段长</param>
        private void ProcMaxIn50M(String sectionFilePath,int segmentLen)
        {
            /*
             * 从断面cit文件中计算出每个有效值通道的50米区段大值
             * 把50米区段大值保存在idf文件中
             * 50米区段大值导出到csv文件
             */
            String sectionFileName = Path.GetFileNameWithoutExtension(sectionFilePath);
            String sectionFileDiretory = Path.GetDirectoryName(sectionFilePath);
            String idfFileName = String.Format("{0}.idf", sectionFileName+"_Rms");
            String idfFilePath = Path.Combine(sectionFileDiretory, idfFileName);

            double[] data_GL = null;
            double[] data_SPEED = null;
            List<double[]> data_AB_RMS_List = new List<double[]>();

            List<List<double>> list_AB_RMS = new List<List<double>>();
            List<List<double>> list_AB_RMS_PEAK = new List<List<double>>();



            int channelId_SPEED = 0;

            DataHeadInfo m_dhi ;
            List<DataChannelInfo> m_dciL = new List<DataChannelInfo>();

            Int32 matlabArrayNum = 0;
            Int32 matlabArrayLen = 0;

            try
            {
                using (FileStream fs = new FileStream(sectionFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (BinaryReader br = new BinaryReader(fs, Encoding.Default))
                    {
                        br.BaseStream.Position = 0;
                        m_dhi = GetDataInfoHead(br.ReadBytes(120));

                        byte[] bChannelData = br.ReadBytes(m_dhi.iChannelNumber * 65);                        
                        //全局变量赋值
                        m_dciL = new List<DataChannelInfo>();
                        List<Byte> rmsChannelIdList = new List<Byte>();
                        List<String> segmentRmsTableNames = new List<String>();
                        for (int i = 0; i < m_dhi.iChannelNumber * 65; i += 65)
                        {
                            DataChannelInfo dci = GetChannelInfo(bChannelData, i);

                            if (i == 65)
                            {
                                dci.fScale = 4;
                            }
                            if (dci.sNameEn.Contains("AB") && dci.sNameEn.Contains("RMS"))
                            {
                                rmsChannelIdList.Add((Byte)(dci.sID));
                                String tmpStr = "segmentRms_" + dci.sNameEn;
                                segmentRmsTableNames.Add(tmpStr);
                            }
                            if (dci.sNameEn.Equals("SPEED"))
                            {
                                channelId_SPEED = dci.sID;
                            }

                            m_dciL.Add(dci);                            
                        }

                        CreateDB(idfFilePath);
                        foreach (String tableName in segmentRmsTableNames)
                        {
                            CreateTable(idfFilePath, tableName);
                        }

                        CreateTableCitFileInfo(idfFilePath);//创建文件信息表
                        WriteTableCitFileInfo(idfFilePath, m_dhi);//写入文件信息

                        br.ReadBytes(BitConverter.ToInt32(br.ReadBytes(4), 0));

                        int iChannelNumberSize = m_dhi.iChannelNumber * 2;
                        byte[] b = new byte[iChannelNumberSize];
                        Int32 iArray = (Int32)(br.BaseStream.Length - br.BaseStream.Position) / iChannelNumberSize;
                        matlabArrayLen = 1000 * 20 * 50;

                        if (iArray % matlabArrayLen != 0)
                        {
                            matlabArrayNum = iArray / matlabArrayLen + 1;
                        } 
                        else
                        {
                            matlabArrayNum = iArray / matlabArrayLen;
                        }

                        List<double> list_AB_KM = new List<double>();
                        List<double> list_AB_SPEED = new List<double>();

                        for (int j = 0; j < rmsChannelIdList.Count; j++)
                        {
                            List<double> list = new List<double>();
                            list_AB_RMS.Add(list);
                            List<double> list1 = new List<double>();
                            list_AB_RMS_PEAK.Add(list1);
                        }


                        for (int i = 1; i <= matlabArrayNum;i++ )
                        {
                            Int32 tmpMatlabArrayLen = 0;
                            if (i == matlabArrayNum)
                            {
                                tmpMatlabArrayLen = iArray - (i - 1) * matlabArrayLen;
                            } 
                            else
                            {
                                tmpMatlabArrayLen=matlabArrayLen;
                            }

                            data_GL = new double[tmpMatlabArrayLen];
                            data_SPEED = new double[tmpMatlabArrayLen];

                            data_AB_RMS_List.Clear();
                            for (int j = 0; j < rmsChannelIdList.Count;j++ )
                            {
                                double[] data_AB_RMS = new double[tmpMatlabArrayLen];
                                data_AB_RMS_List.Add(data_AB_RMS);
                            }

                            for (int k = 0; k < tmpMatlabArrayLen; k++)
                            {
                                b = br.ReadBytes(iChannelNumberSize);
                                if (IsEncrypt(m_dhi))
                                {
                                    b = ByteXORByte(b);
                                }
                                
                                double fGL = (BitConverter.ToInt16(b, 0) / m_dciL[0].fScale);
                                int tmp = BitConverter.ToInt16(b, 2);
                                fGL += ((BitConverter.ToInt16(b, 2) / m_dciL[1].fScale) / 1000.0);
                                data_GL[k] = fGL;
                                data_SPEED[k] = (BitConverter.ToInt16(b, (channelId_SPEED - 1) * 2) / m_dciL[channelId_SPEED - 1].fScale + m_dciL[channelId_SPEED - 1].fOffset);
                                for (int m = 0; m < rmsChannelIdList.Count;m++ )
                                {
                                    data_AB_RMS_List[m][k] = (BitConverter.ToInt16(b, (rmsChannelIdList[m] - 1) * 2) / m_dciL[rmsChannelIdList[m] - 1].fScale + m_dciL[rmsChannelIdList[m] - 1].fOffset);
                                }
                            }

                            Boolean addFlag = true;

                            for (int j = 0; j < rmsChannelIdList.Count; j++)
                            {
                                double[,] resultArrayAB_KM;
                                double[,] resultArrayAB_SPEED;
                                double[,] resultArrayAB_RMS;
                                //double[,] resultArrayAB_RMS_PEAK;
                                
                                MWNumericArray mwArrayGL = new MWNumericArray(data_GL);
                                MWNumericArray mwArraySpeed = new MWNumericArray(data_SPEED);
                                MWNumericArray mwLen = segmentLen;

                                MWNumericArray mwArrayAB_RMS = new MWNumericArray(data_AB_RMS_List[j]);
                                MWArray[] resultArrayAB = (MWArray[])acclrtnCls.sub_calculate_segment_rms(3, mwArrayGL, mwArrayAB_RMS, mwArraySpeed, mwLen);
                                resultArrayAB_KM = (double[,])resultArrayAB[0].ToArray();
                                resultArrayAB_SPEED = (double[,])resultArrayAB[1].ToArray();
                                resultArrayAB_RMS = (double[,])resultArrayAB[2].ToArray();
                                //resultArrayAB_RMS_PEAK = (double[,])resultArrayAB[3].ToArray();
                                

                                for (int n = 0; n < resultArrayAB_SPEED.Length; n++)
                                {
                                    if (addFlag)
                                    {
                                        list_AB_KM.Add(resultArrayAB_KM[0, n]);
                                        list_AB_SPEED.Add(resultArrayAB_SPEED[0, n]);
                                        
                                    }
                                    
                                    list_AB_RMS[j].Add(resultArrayAB_RMS[0, n]);                                    
                                }
                                addFlag = false;

                                mwArraySpeed = null;
                                mwArrayGL = null;
                                resultArrayAB = null;
                                resultArrayAB_KM = null;
                                resultArrayAB_SPEED = null;
                                resultArrayAB_RMS = null;
                                //resultArrayAB_RMS_PEAK = null;
                            }

                        }                        

                        data_GL = null;
                        data_SPEED = null;
                        for (int m = 0; m < data_AB_RMS_List.Count;m++ )
                        {
                            data_AB_RMS_List[m] = null;
                        }

                        br.Close();
                        fs.Close();

                        for (int n = 0; n < rmsChannelIdList.Count; n++)
                        {
                            double[,] resultArrayAB_RMS_PEAK_REAL;
                            MWNumericArray mwtmpSpeed = new MWNumericArray(list_AB_SPEED.ToArray());
                            MWNumericArray mwtmpRms = new MWNumericArray(list_AB_RMS[n].ToArray());

                            MWArray[] resultArrayAB_PEAK = (MWArray[])acclrtnCls.sub_calculate_peak_factor(1, mwtmpSpeed, mwtmpRms);
                            resultArrayAB_RMS_PEAK_REAL = (double[,])resultArrayAB_PEAK[0].ToArray();

                            for (int i = 0; i < list_AB_RMS[n].Count; i++)
                            {
                                //list_AB_RMS_PEAK[n].Add(resultArrayAB_RMS_PEAK_REAL[0, i]);
                                list_AB_RMS_PEAK[n].Add(0);
                            }

                            InsertIntoSegmentRmsTable(idfFilePath, segmentRmsTableNames[n], list_AB_KM, list_AB_SPEED, list_AB_RMS[n], list_AB_RMS_PEAK[n]);
                            resultArrayAB_RMS_PEAK_REAL = null;
                        }



                        list_AB_KM.Clear();
                        list_AB_RMS.Clear();
                        list_AB_RMS_PEAK.Clear();
                        list_AB_SPEED.Clear();
                        
                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.StackTrace);
            }
        }
        #endregion

        #region 车体和构架加速度求50米区段大值---按照刘博士提供的规则直接求
        /// <summary>
        /// 车体和构架加速度求50米区段大值---按照刘博士提供的规则直接求
        /// </summary>
        /// <param name="sectionFilePath">加速度断面cit文件的全路径文件名</param>
        /// <param name="segmentLen">区段的段长</param>
        private void ProcMaxIn50M_Fr_CB(String sectionFilePath, int segmentLen)
        {
            /*
             * 架构和车体的50米区段大值的原理和轴箱不一样
             * 架构和车体取的是50米范围内绝对值的最大值的所在位置的点的原值（有正负号）
             * 轴箱加速度取得是50米范围内的绝对值的最大值
             * 
             * 从断面cit文件中计算出每个车体和架构通道的50米区段大值
             * 把50米区段大值保存在idf文件中
             * 50米区段大值导出到csv文件
             */
            String sectionFileName = Path.GetFileNameWithoutExtension(sectionFilePath);
            String sectionFileDiretory = Path.GetDirectoryName(sectionFilePath);
            String idfFileName = String.Format("{0}.idf", sectionFileName + "_Rms");
            String idfFilePath = Path.Combine(sectionFileDiretory, idfFileName);

            double[] data_GL = null;
            double[] data_SPEED = null;
            List<double[]> data_CB_Fr_RMS_List = new List<double[]>();//存放构架和车体的50米区段大值

            List<List<double>> list_AB_RMS = new List<List<double>>();
            List<List<double>> list_AB_RMS_PEAK = new List<List<double>>();



            int channelId_SPEED = 0;

            DataHeadInfo m_dhi;
            List<DataChannelInfo> m_dciL = new List<DataChannelInfo>();

            Int32 matlabArrayNum = 0;
            Int32 matlabArrayLen = 0;

            try
            {
                using (FileStream fs = new FileStream(sectionFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (BinaryReader br = new BinaryReader(fs, Encoding.Default))
                    {
                        br.BaseStream.Position = 0;
                        m_dhi = GetDataInfoHead(br.ReadBytes(120));

                        byte[] bChannelData = br.ReadBytes(m_dhi.iChannelNumber * 65);
                        //全局变量赋值
                        m_dciL = new List<DataChannelInfo>();
                        List<Byte> rmsChannelIdList = new List<Byte>();
                        List<String> segmentRmsTableNames = new List<String>();
                        for (int i = 0; i < m_dhi.iChannelNumber * 65; i += 65)
                        {
                            DataChannelInfo dci = GetChannelInfo(bChannelData, i);

                            if (i == 65)
                            {
                                dci.fScale = 4;
                            }
                            if (dci.sNameEn.Contains("CB") || dci.sNameEn.Contains("Fr"))
                            {
                                rmsChannelIdList.Add((Byte)(dci.sID));
                                String tmpStr = "segmentRms_" + dci.sNameEn;
                                segmentRmsTableNames.Add(tmpStr);
                            }
                            if (dci.sNameEn.Equals("SPEED"))
                            {
                                channelId_SPEED = dci.sID;
                            }

                            m_dciL.Add(dci);
                        }

                        //在前面轴箱加速度进行50米区段大值计算时，已经创建过idf文件，所以这里不需要再次创建
                        //CreateDB(idfFilePath);
                        foreach (String tableName in segmentRmsTableNames)
                        {
                            CreateTable(idfFilePath, tableName);
                        }

                        //在前面轴箱加速度进行50米区段大值计算时，已经创建过idf文件并且写入cit文件信息，所以这里不需要再次写入
                        //CreateTableCitFileInfo(idfFilePath);//创建文件信息表
                        //WriteTableCitFileInfo(idfFilePath, m_dhi);//写入文件信息

                        br.ReadBytes(BitConverter.ToInt32(br.ReadBytes(4), 0));

                        int iChannelNumberSize = m_dhi.iChannelNumber * 2;
                        byte[] b = new byte[iChannelNumberSize];
                        Int32 iArray = (Int32)(br.BaseStream.Length - br.BaseStream.Position) / iChannelNumberSize;
                        //matlabArrayLen = 1000 * 20 * 50;
                        matlabArrayLen = segmentLen;//构架和车体改为直接求区段大值，所以这里的长度就是区段大值的段长

                        if (iArray % matlabArrayLen != 0)
                        {
                            matlabArrayNum = iArray / matlabArrayLen + 1;
                        }
                        else
                        {
                            matlabArrayNum = iArray / matlabArrayLen;
                        }

                        List<double> list_AB_KM = new List<double>();
                        List<double> list_AB_SPEED = new List<double>();

                        for (int j = 0; j < rmsChannelIdList.Count; j++)
                        {
                            List<double> list = new List<double>();
                            list_AB_RMS.Add(list);
                            List<double> list1 = new List<double>();
                            list_AB_RMS_PEAK.Add(list1);
                        }


                        for (int i = 1; i <= matlabArrayNum; i++)
                        {
                            Int32 tmpMatlabArrayLen = 0;
                            if (i == matlabArrayNum)
                            {
                                tmpMatlabArrayLen = iArray - (i - 1) * matlabArrayLen;
                            }
                            else
                            {
                                tmpMatlabArrayLen = matlabArrayLen;
                            }

                            data_GL = new double[tmpMatlabArrayLen];
                            data_SPEED = new double[tmpMatlabArrayLen];

                            data_CB_Fr_RMS_List.Clear();
                            for (int j = 0; j < rmsChannelIdList.Count; j++)
                            {
                                double[] data_AB_RMS = new double[tmpMatlabArrayLen];
                                data_CB_Fr_RMS_List.Add(data_AB_RMS);
                            }

                            for (int k = 0; k < tmpMatlabArrayLen; k++)
                            {
                                b = br.ReadBytes(iChannelNumberSize);
                                if (IsEncrypt(m_dhi))
                                {
                                    b = ByteXORByte(b);
                                }

                                double fGL = (BitConverter.ToInt16(b, 0) / m_dciL[0].fScale);
                                int tmp = BitConverter.ToInt16(b, 2);
                                fGL += ((BitConverter.ToInt16(b, 2) / m_dciL[1].fScale) / 1000.0);
                                data_GL[k] = fGL;
                                data_SPEED[k] = (BitConverter.ToInt16(b, (channelId_SPEED - 1) * 2) / m_dciL[channelId_SPEED - 1].fScale + m_dciL[channelId_SPEED - 1].fOffset);
                                for (int m = 0; m < rmsChannelIdList.Count; m++)
                                {
                                    data_CB_Fr_RMS_List[m][k] = (BitConverter.ToInt16(b, (rmsChannelIdList[m] - 1) * 2) / m_dciL[rmsChannelIdList[m] - 1].fScale + m_dciL[rmsChannelIdList[m] - 1].fOffset);
                                }
                            }

                            Boolean addFlag = true;

                            for (int j = 0; j < rmsChannelIdList.Count; j++)
                            {
                                //double[,] resultArrayAB_KM;
                                //double[,] resultArrayAB_SPEED;
                                //double[,] resultArrayAB_RMS;
                                ////double[,] resultArrayAB_RMS_PEAK;

                                //MWNumericArray mwArrayGL = new MWNumericArray(data_GL);
                                //MWNumericArray mwArraySpeed = new MWNumericArray(data_SPEED);
                                //MWNumericArray mwLen = segmentLen;

                                //MWNumericArray mwArrayAB_RMS = new MWNumericArray(data_AB_RMS_List[j]);
                                //MWArray[] resultArrayAB = (MWArray[])acclrtnCls.sub_calculate_segment_rms(3, mwArrayGL, mwArrayAB_RMS, mwArraySpeed, mwLen);
                                //resultArrayAB_KM = (double[,])resultArrayAB[0].ToArray();
                                //resultArrayAB_SPEED = (double[,])resultArrayAB[1].ToArray();
                                //resultArrayAB_RMS = (double[,])resultArrayAB[2].ToArray();
                                //resultArrayAB_RMS_PEAK = (double[,])resultArrayAB[3].ToArray();

                                int index = 0;//存放50米绝对值最大值的下标
                                double maxVal = 0;//存放下标对应得大值
                                for (int k = 0; k < data_CB_Fr_RMS_List[j].Length;k++ )
                                {
                                    //绝对值的最大值所在的位置
                                    if (Math.Abs(data_CB_Fr_RMS_List[j][k]) > maxVal)
                                    {
                                        maxVal = data_CB_Fr_RMS_List[j][k];
                                        index = k;
                                    }
                                }

                                if (addFlag)
                                {
                                    list_AB_KM.Add(data_GL[index]);
                                    list_AB_SPEED.Add(data_SPEED[index]);

                                }

                                list_AB_RMS[j].Add(data_CB_Fr_RMS_List[j][index]);


                                //for (int n = 0; n < resultArrayAB_SPEED.Length; n++)
                                //{
                                //    if (addFlag)
                                //    {
                                //        list_AB_KM.Add(resultArrayAB_KM[0, n]);
                                //        list_AB_SPEED.Add(resultArrayAB_SPEED[0, n]);

                                //    }

                                //    list_AB_RMS[j].Add(resultArrayAB_RMS[0, n]);
                                //}
                                addFlag = false;

                                //mwArraySpeed = null;
                                //mwArrayGL = null;
                                //resultArrayAB = null;
                                //resultArrayAB_KM = null;
                                //resultArrayAB_SPEED = null;
                                //resultArrayAB_RMS = null;
                                //resultArrayAB_RMS_PEAK = null;
                            }

                        }

                        data_GL = null;
                        data_SPEED = null;
                        for (int m = 0; m < data_CB_Fr_RMS_List.Count; m++)
                        {
                            data_CB_Fr_RMS_List[m] = null;
                        }

                        br.Close();
                        fs.Close();

                        for (int n = 0; n < rmsChannelIdList.Count; n++)
                        {
                            //double[,] resultArrayAB_RMS_PEAK_REAL;
                            //MWNumericArray mwtmpSpeed = new MWNumericArray(list_AB_SPEED.ToArray());
                            //MWNumericArray mwtmpRms = new MWNumericArray(list_AB_RMS[n].ToArray());

                            //MWArray[] resultArrayAB_PEAK = (MWArray[])acclrtnCls.sub_calculate_peak_factor(1, mwtmpSpeed, mwtmpRms);
                            //resultArrayAB_RMS_PEAK_REAL = (double[,])resultArrayAB_PEAK[0].ToArray();

                            for (int i = 0; i < list_AB_RMS[n].Count; i++)
                            {
                                //list_AB_RMS_PEAK[n].Add(resultArrayAB_RMS_PEAK_REAL[0, i]);
                                list_AB_RMS_PEAK[n].Add(0);
                            }

                            InsertIntoSegmentRmsTable(idfFilePath, segmentRmsTableNames[n], list_AB_KM, list_AB_SPEED, list_AB_RMS[n], list_AB_RMS_PEAK[n]);
                            //resultArrayAB_RMS_PEAK_REAL = null;
                        }



                        list_AB_KM.Clear();
                        list_AB_RMS.Clear();
                        list_AB_RMS_PEAK.Clear();
                        list_AB_SPEED.Clear();

                    }
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.StackTrace);
            }
        }
        #endregion

        #region 动态创建数据库---idf文件
        /// <summary>
        /// 动态创建数据库---idf文件
        /// </summary>
        /// <param name="sFilePath"></param>
        private void CreateDB(string idfFilePath)
        {
            string ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + idfFilePath + ";Jet OLEDB:Engine Type=5";

            ADOX.Catalog ct = new ADOX.Catalog();
            try
            {
                if (File.Exists(idfFilePath))
                {
                    File.Delete(idfFilePath);
                }
                ct.Create(ConnectionString);       
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.StackTrace);
            }
            ct = null;
            Application.DoEvents();
        }
        #endregion

        #region 创建idf数据库里的表
        /// <summary>
        /// 创建idf数据库里的表
        /// </summary>
        /// <param name="sFilePath"></param>
        private void CreateTable(string idfFilePath,String tableName)
        {

            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + idfFilePath + ";Persist Security Info=True"))
                {
                    string sqlCreate = "CREATE TABLE "+tableName+" (" +
                                                                        "Id integer identity primary key,"+
                                                                        "KiloMeter varchar(255) NULL," +
                                                                        "Speed varchar(255) NULL,"+
                                                                        "Segment_RMS varchar(255) NULL,"+
                                                                        "Segment_RMS_Peak varchar(255) NULL," +
                                                                        "valid integer NULL);";
                    OleDbCommand sqlcom = new OleDbCommand(sqlCreate, sqlconn);
                    sqlconn.Open();
                    sqlcom.ExecuteNonQuery();
                    sqlconn.Close();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.StackTrace);
            }
        }
        #endregion

        #region 创建idf数据库里的表：cit文件信息表
        /// <summary>
        /// 创建idf数据库里的表：cit文件信息表
        /// </summary>
        /// <param name="sFilePath"></param>
        private void CreateTableCitFileInfo(string idfFilePath)
        {
            String tableName = "CitFileInfo";

            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + idfFilePath + ";Persist Security Info=True"))
                {
                    string sqlCreate = "CREATE TABLE " + tableName + " (" +
                                                                        "Id integer identity primary key," +
                                                                        "LineName varchar(255) NULL," +
                                                                        "LineCode varchar(255) NULL," +
                                                                        "LineDir varchar(255) NULL," +
                                                                        "KmInc varchar(255) NULL," +
                                                                        "SDate date NULL," +
                                                                        "STime date NULL," +
                                                                        "Train varchar(255) NULL);";
                    OleDbCommand sqlcom = new OleDbCommand(sqlCreate, sqlconn);
                    sqlconn.Open();
                    sqlcom.ExecuteNonQuery();
                    sqlconn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.StackTrace);
            }
        }
        #endregion

        private Dictionary<int, String> dicKmInc = null;
        private void InitDicKmInc()
        {
            if (dicKmInc == null || dicKmInc.Count == 0)
            {
                dicKmInc = new Dictionary<int, String>();
                dicKmInc.Add(0, "增");
                dicKmInc.Add(1, "减");
            }
        }

        private Dictionary<int, String> dicDir = null;
        private void InitDicDir()
        {
            if (dicDir == null || dicDir.Count == 0)
            {
                dicDir = new Dictionary<int, String>();
                dicDir.Add(1, "上");
                dicDir.Add(2, "下");
                dicDir.Add(3, "单");
            }
        }

        private void WriteTableCitFileInfo(String idfFilePath,DataHeadInfo m_dhi)
        {
            InitDicKmInc();//初始化字典
            InitDicDir();

            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + idfFilePath + ";Persist Security Info=True"))
                {
                    sqlconn.Open();

                    // 临时修改开始，此处如果m_dhi.sTrackCode是空字符串（检测车软件没有配置好），sqlcom.ExecuteNonQuery()会出错，临时修改为：
                    if (m_dhi.sTrackCode.StartsWith("\0"))
                    {
                        m_dhi.sTrackCode = "0000"; // 这里的值统一设置为0000，对后面的计算不影响
                    }
                    // 临时修改完毕

                    string sSql = String.Format("insert into {0}(Id,LineName,LineCode,LineDir,KmInc,SDate,STime,Train) values( {1},'{2}','{3}','{4}','{5}','{6}','{7}','{8}');",                                               
                                                "CitFileInfo",
                                                 1,
                                                m_dhi.sTrackName,
                                                m_dhi.sTrackCode,
                                                dicDir[m_dhi.iDir],
                                                dicKmInc[m_dhi.iKmInc],
                                                m_dhi.sDate,
                                                m_dhi.sTime,
                                                m_dhi.sTrain);

                    OleDbCommand sqlcom = new OleDbCommand(sSql, sqlconn);
                    sqlcom.ExecuteNonQuery();

                    sqlconn.Close();
                }
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("写入{0}文件中的{1}异常", idfFilePath, "CitFileInfo");
                MessageBox.Show(sb.ToString() + "\n" + ex.Message);
                MessageBox.Show(ex.StackTrace);
            }
        }
        public String ReadTableCitFileInfo(String idfFilePath)
        {
            StringBuilder sb = new StringBuilder();

            try
            {

                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + idfFilePath + ";Persist Security Info=False"))
                {
                    OleDbCommand sqlcom = new OleDbCommand("select * from CitFileInfo", sqlconn);
                    sqlconn.Open();
                    OleDbDataReader sqloledr = sqlcom.ExecuteReader();
                    while (sqloledr.Read())
                    {
                        //sb.AppendFormat("{0},", sqloledr.GetValue(0).ToString());
                        sb.AppendFormat("{0},", sqloledr.GetValue(1).ToString());
                        sb.AppendFormat("{0},", sqloledr.GetValue(2).ToString());
                        sb.AppendFormat("{0},", sqloledr.GetValue(3).ToString());
                        sb.AppendFormat("{0},", sqloledr.GetValue(4).ToString());
                        sb.AppendFormat("{0},", DateTime.Parse(sqloledr.GetValue(5).ToString()).Date.ToShortDateString());
                        sb.AppendFormat("{0},", DateTime.Parse(sqloledr.GetValue(6).ToString()).TimeOfDay.ToString());
                        sb.AppendFormat("{0}", sqloledr.GetValue(7).ToString());
                    }
                    sqlconn.Close();
                }

            }
            catch(Exception ex)
            {
                return "1," + ex.Message;
            }

            return "0,"+sb.ToString();
        }

        public void ReadTableRms(String idfFilePath, String tableName, out List<double> rmsData, out List<double> spdData)
        {
            rmsData = new List<double>();
            spdData = new List<double>();
            try
            {

                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + idfFilePath + ";Persist Security Info=False"))
                {
                    String sql = String.Format("select Speed,Segment_RMS,Id from {0} order by Id", tableName);
                    OleDbCommand sqlcom = new OleDbCommand(sql, sqlconn);
                    sqlconn.Open();
                    OleDbDataReader sqloledr = sqlcom.ExecuteReader();
                    while (sqloledr.Read())
                    {
                        double spd = double.Parse(sqloledr.GetValue(0).ToString());
                        double rms = double.Parse(sqloledr.GetValue(1).ToString());
                        spdData.Add(spd);
                        rmsData.Add(rms);
                        if (spd == 301.5 && rms == 0.672)
                        {
                            int i = 0;
                        }
                    }
                    sqlconn.Close();
                }

            }
            catch (Exception ex)
            {
                rmsData = null;
                spdData = null;
                return ;
            }
            
            return ;
        }
        public void sub_calculate_mean_rms(double[] spdArray, double[] rmsArray, out double avg_rms, out double avg_spd)
        {
            avg_rms = 0;
            avg_spd = 0;

            double[,] resultArray_Avg = null;

            MWNumericArray mwtmpSpeed = new MWNumericArray(spdArray);
            MWNumericArray mwtmpRms = new MWNumericArray(rmsArray);

            MWArray[] resultArray = (MWArray[])acclrtnCls.sub_calculate_mean_rms(2, mwtmpSpeed, mwtmpRms);

            resultArray_Avg = (double[,])resultArray[0].ToArray();
            avg_rms = Math.Round(resultArray_Avg[0, 0],3);
            resultArray_Avg = (double[,])resultArray[1].ToArray();
            avg_spd = Math.Round(resultArray_Avg[0, 0],3);

            //for (int i = 0; i < resultArray_Avg.Length; i++)
            //{
            //    //list_AB_RMS_PEAK[n].Add(resultArrayAB_RMS_PEAK_REAL[0, i]);
                
            //}

            return;
        }
        public double[] sub_calculate_peak_factor(double[] rmsArray, double avg_rms)
        {
            List<double> peak = new List<double>();

            //double[,] resultArray_Peak = null;

            //MWNumericArray mwtmpAvg_rms = new MWNumericArray(avg_rms);
            //MWNumericArray mwtmpRms = new MWNumericArray(rmsArray);

            //MWArray[] resultArray = (MWArray[])acclrtnCls.sub_calculate_peak_factor(1, mwtmpRms,mwtmpAvg_rms);
            //resultArray_Peak = (double[,])resultArray[0].ToArray();

            //for (int i = 0; i < resultArray_Peak.Length; i++)
            //{
            //    peak.Add(resultArray_Peak[0, i]);

            //}

            for (int i = 0; i < rmsArray.Length;i++ )
            {
                peak.Add(rmsArray[i] / avg_rms);
            }

            return peak.ToArray();
        }
        public void UpdatePeak(String rmsIdfPath, String tableName, double[] peakArray)
        {
            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + rmsIdfPath + ";Persist Security Info=True"))
                {
                    sqlconn.Open();
                    OleDbTransaction trans = sqlconn.BeginTransaction();
                    for (int i = 0; i < peakArray.Length; i++)
                    {
                        string sSql = "update " + tableName + " set Segment_RMS_Peak='" + peakArray[i] + "' where Id = " + (i+1);
                        OleDbCommand sqlcom = new OleDbCommand(sSql, sqlconn);
                        sqlcom.Transaction = trans;
                        sqlcom.ExecuteNonQuery();                        
                    }

                    trans.Commit();

                    trans.Dispose();

                    sqlconn.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("写入峰值因子出错:" + ex.Message);
            }
        }



        #region idf数据库操作：新增--SegmentRms表格的数据
        /// <summary>
        /// idf数据库操作：新增--SegmentRms表格的数据
        /// </summary>
        /// <param name="idfFilePath">idf文件路径</param>
        /// <param name="tableName">表名</param>
        /// <param name="data_KM">公里标</param>
        /// <param name="data_SPEED">速度</param>
        /// <param name="data_RMS">50米区段大值</param>
        /// <param name="data_RMS_Peak">峰值因子</param>
        private void InsertIntoSegmentRmsTable(string idfFilePath, String tableName, List<double> data_KM, List<double> data_SPEED, List<double> data_RMS, List<double> data_RMS_Peak)
        {
            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + idfFilePath + ";Persist Security Info=True"))
                {
                    sqlconn.Open();

                    for (int i = 0; i < data_KM.Count; i++)
                    {
                        string sSql = String.Format("insert into {0} (KiloMeter,Speed,Segment_RMS,Segment_RMS_Peak,Valid) values( '{1}','{2}','{3}','{4}',{5} )",
                                                    tableName,
                                                    data_KM[i].ToString(),
                                                    data_SPEED[i].ToString(),
                                                    data_RMS[i].ToString(),
                                                    data_RMS_Peak[i].ToString(),
                                                    1);

                        OleDbCommand sqlcom = new OleDbCommand(sSql, sqlconn);
                        sqlcom.ExecuteNonQuery();
                    }

                    sqlconn.Close();
                }
            }
            catch (Exception ex)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat("生成{0}文件中的{1}异常", idfFilePath, "SegmentRms_AB_Vt_L");
                MessageBox.Show(sb.ToString()+"\n" + ex.Message);
                MessageBox.Show(ex.StackTrace);
            }
        }
        #endregion            


        private List<Byte> CreatHeadInfo(List<Byte> mCitHeadInfo,int sectionNum)
        {
            List<Byte> tmpList = new List<Byte>(120);
            tmpList.AddRange(mCitHeadInfo.ToArray());

            tmpList.RemoveRange(116, 4);
            tmpList.AddRange(BitConverter.GetBytes(sectionNum));

            return tmpList;
        }

        public void CalcSegmentRMS(List<String> sctnFilePathList,int segmentLen)
        {
            foreach (String sctnFilePath in sctnFilePathList)
            {
                ProcMaxIn50M(sctnFilePath, segmentLen);
                ProcMaxIn50M_Fr_CB(sctnFilePath, segmentLen);
            }
        }

        //public void CreateAllSectionFile(String citFilePath, int sampleFreq, int maxFreq, int windowLen,List<TextBox> frep_L,List<TextBox> frep_H)
        //{
        //    Boolean resultInit = InitCIT(citFilePath);
        //    if (resultInit == false)
        //    {
        //        return;
        //    }
        //    InitSctnIdList();

        //    List<Byte> mCitHeadInfo = null;

        //    foreach (List<Byte> channelIdList in sctnIdList)
        //    {
        //        String SectionFilePath = null;
        //        Byte[] SectionChannelInfo = CreateChannelInfo(citFilePath, channelIdList, out SectionFilePath);
        //        mCitHeadInfo = CreatHeadInfo(citHeadInfoList, (int)(SectionChannelInfo.Length / 65));
        //        //CreateSectionFile(citFilePath, channelIdList, mCitHeadInfo.ToArray(), SectionChannelInfo, SectionFilePath, sampleFreq, maxFreq, windowLen);
        //        CreateSectionFileNew(citFilePath, channelIdList, mCitHeadInfo.ToArray(), SectionChannelInfo, SectionFilePath, sampleFreq, maxFreq, windowLen, frep_L, frep_H);
        //        SectionChannelInfo = null;
        //        if (sectionFilePathList.Contains(SectionFilePath) == false)
        //        {
        //            sectionFilePathList.Add(SectionFilePath);
        //        }                
        //    }
        //}


        /// <summary>
        /// 创建区段文件：对轴箱加速度计算有效值，滤波，采样六分之一；对车体和构架加速度，进行滤波，采样六分之一。
        /// </summary>
        /// <param name="citFilePath">加速度全路径文件名</param>
        /// <param name="sampleFreq">采样频率：2000</param>
        /// <param name="maxFreq">频率上限：500</param>
        /// <param name="windowLen">窗长：60</param>
        /// <param name="frep_L">通带下限</param>
        /// <param name="frep_H">通带上限</param>
        /// <param name="frep_L_L">阻带下限</param>
        /// <param name="frep_H_H">阻带上限</param>
        /// <returns></returns>
        public List<String> CreateAllSectionFileNew(String citFilePath, int sampleFreq, int maxFreq,int minFreq, int windowLen, List<TextBox> frep_L, List<TextBox> frep_H, List<TextBox> frep_L_L, List<TextBox> frep_H_H)
        {
            List<String> m_sectionFilePathList = new List<String>();

            Boolean resultInit = InitCIT(citFilePath);
            if (resultInit == false)
            {
                return m_sectionFilePathList;
            }
            InitSctnIdList();

            List<Byte> mCitHeadInfo = null;

            foreach (List<Byte> channelIdList in sctnIdList)
            {
                String SectionFilePath = null;
                Byte[] SectionChannelInfo = CreateChannelInfo(citFilePath, channelIdList, out SectionFilePath);
                mCitHeadInfo = CreatHeadInfo(citHeadInfoList, (int)(SectionChannelInfo.Length / 65));
                //CreateSectionFile(citFilePath, channelIdList, mCitHeadInfo.ToArray(), SectionChannelInfo, SectionFilePath, sampleFreq, maxFreq, windowLen);
                CreateSectionFileNew(citFilePath, channelIdList, mCitHeadInfo.ToArray(), SectionChannelInfo, SectionFilePath, sampleFreq, maxFreq, minFreq, windowLen, frep_L, frep_H, frep_L_L, frep_H_H);
                SectionChannelInfo = null;
                if (m_sectionFilePathList.Contains(SectionFilePath) == false)
                {
                    m_sectionFilePathList.Add(SectionFilePath);
                }
            }

            return m_sectionFilePathList;
        }
    }

    #region 结构体声明
    #region CIT文件头信息结构体
    /// <summary>
    /// CIT文件头信息结构体，120字节
    /// </summary>
    public struct DataHeadInfo
    {
        #region 数据类型
        /// <summary>
        /// iDataType：1轨检、2动力学、3弓网----4个字节
        /// </summary>
        public int iDataType;
        #endregion
        #region 文件版本号
        /// <summary>
        /// 文件版本号，用X.X.X表示 第一位大于等于3代表加密后,只加密数据块部分---1+20个字节，第一个字节表示实际长度，以下类同
        /// </summary>
        public string sDataVersion;
        #endregion
        #region 线路代码
        /// <summary>
        /// 线路代码，同PWMIS----1+4个字节
        /// </summary>
        public string sTrackCode;
        #endregion
        #region 线路名
        /// <summary>
        /// 线路名 中文线路名---1+20个字节
        /// </summary>
        public string sTrackName;
        #endregion
        #region 行别
        /// <summary>
        /// 行别：1上行、2下行、3单线----4个字节
        /// </summary>
        public int iDir;
        #endregion
        #region 检测车号
        /// <summary>
        /// 检测车号，不足补空格---1+20个字节
        /// </summary>
        public string sTrain;
        #endregion
        #region 检测日期
        /// <summary>
        /// 检测日期：yyyy-MM-dd---1+10个字节
        /// </summary>
        public string sDate;
        #endregion
        #region 检测起始时间
        /// <summary>
        /// 检测起始时间：HH:mm:ss---1+8个字节
        /// </summary>
        public string sTime;
        #endregion
        #region 检测方向
        /// <summary>
        /// 检测方向，正0，反1----4个字节
        /// </summary>
        public int iRunDir;
        #endregion
        #region 增里程0，减里程1
        /// <summary>
        /// 增里程0，减里程1----4个字节
        /// </summary>
        public int iKmInc;
        #endregion
        #region 开始里程
        /// <summary>
        /// 开始里程----4个字节
        /// </summary>
        public float fkmFrom;
        #endregion
        #region 结束里程
        /// <summary>
        /// 结束里程，检测结束后更新----4个字节
        /// </summary>
        public float fkmTo;
        #endregion
        #region 采样数，（距离采样>0, 时间采样<0
        /// <summary>
        /// 采样数，（距离采样>0, 时间采样<0）----4个字节
        /// </summary>
        public int iSmaleRate;
        #endregion
        #region 数据块中通道总数
        /// <summary>
        /// 数据块中通道总数----4个字节
        /// </summary>
        public int iChannelNumber;
        #endregion
    }
    #endregion

    #region 通道定义信息结构体
    /// <summary>
    /// 通道定义信息结构体---65个字节
    /// </summary>    
    public class DataChannelInfo
    {
        #region 通道Id
        /// <summary>
        /// 通道Id：轨检通道从1～1000定义；动力学从1001~2000；弓网从2001~3000-----4个字节
        /// </summary>
        public int sID;
        #endregion
        #region 通道名称英文
        /// <summary>
        /// 通道名称英文，不足补空格-----1+20个字节
        /// </summary>
        public string sNameEn;
        #endregion
        #region 通道名称中文
        /// <summary>
        /// 通道名称中文，不足补空格-----1+20个字节
        /// </summary>
        public string sNameCh;
        #endregion
        #region 通道比例
        /// <summary>
        /// 通道比例-----4个字节
        /// </summary>
        public float fScale;
        #endregion
        #region 通道基线值
        /// <summary>
        /// 通道基线值-----4个字节
        /// </summary>
        public float fOffset;
        #endregion
        #region 通道单位
        /// <summary>
        /// 通道单位，不足补空格-----1+10个字节
        /// </summary>
        public string sUnit;
        #endregion

    }
    #endregion
    #endregion
}
