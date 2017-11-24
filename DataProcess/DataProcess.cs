using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Xml;
using System.Data.OleDb;
using System.Net.Sockets;
using MathWorks.MATLAB.NET.Utility;
using MathWorks.MATLAB.NET.Arrays;
using System.Data;
using System.Globalization;

namespace DataProcess
{
    //波形算法相关
    namespace CalcSpace
    {
        #region 网络版使用
        /// <summary>
        /// 网络版使用
        /// </summary>
        public class QueryClass
        {
            public int planid = 0;
            public int taskid = 0;
            public int recordid = 0;
            public int backid = 0;
            public string sFilePath = "";
            public string sRun = "";
            public string sTrain = "";
        }
        #endregion
        #region 波形计算使用--网络版调用
        /// <summary>
        /// 波形计算使用--网络版调用
        /// </summary>
        public class CalcClass1
        {
            public int planid = 0;
            public int taskid = 0;
            public int recordid = 0;
            public int backid = 0;
            public float startkm = 0f;
            public float startm = 0f;
            public float endkm = 0f;
            public float endm = 0f;
            public float speed = 0f;
            public double value = 0.0;
            public string des = "";
            public string channel = "";
            public string para = "";
        }
        #endregion

        #region 波形计算相关类
        /// <summary>
        /// 波形计算相关类
        /// </summary>
        public class CalcProcess
        {
            public CITDataProcess citdataprocess = new CITDataProcess();
            public WaveformDataProcess wdp = new WaveformDataProcess();

            //Matlab算法类
            CalcDC.CalcDC CalcDCInst = new CalcDC.CalcDC();

            #region 滤波---网络版使用
            //滤波
            public void ProcessCalculateCitFilter(string sFile)
            {
                try
                {
                    #region 获取通道序号
                    string[] sTQIItem = new string[] { "L_Prof_SC", "R_Prof_SC", "L_Align_SC", 
                "R_Align_SC", "Gage", "Crosslevel", "Short_Twist"};
                    int[] sTQIItemIndex = new int[sTQIItem.Length];
                    //获取通道信息
                    CITDataProcess citdp = new CITDataProcess();
                    WaveformDataProcess wdp = new WaveformDataProcess();
                    CITDataProcess.DataHeadInfo m_dhi = citdp.GetDataInfoHeadNew(sFile);
                    List<CITDataProcess.DataChannelInfo> m_dciL = citdp.GetDataChannelInfoHeadNew(sFile);
                    float[] f = new float[m_dhi.iChannelNumber];
                    float[] g = new float[m_dhi.iChannelNumber];
                    wdp.GetChannelScale(sFile, m_dhi.iChannelNumber, ref f, ref g);
                    //给通道绑定序号
                    for (int i = 0; i < sTQIItem.Length; i++)
                    {

                        for (int j = 0; j < m_dciL.Count; j++)
                        {
                            if (sTQIItem[i].Equals(m_dciL[j].sNameEn))
                            {
                                sTQIItemIndex[i] = j;
                                break;
                            }
                        }
                    }
                    #endregion
                    #region 存取文件
                    FileStream fsRead = new FileStream(sFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    FileStream fsWrite = new FileStream(sFile + ".bak", FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                    BinaryReader br = new BinaryReader(fsRead, Encoding.UTF8);
                    BinaryWriter bw1 = new BinaryWriter(fsWrite, Encoding.UTF8);
                    byte[] bHead = br.ReadBytes(120);
                    byte[] bChannels = br.ReadBytes(65 * m_dhi.iChannelNumber);
                    byte[] bData = new byte[m_dhi.iChannelNumber * 2];
                    byte[] bDataNew = new byte[m_dhi.iChannelNumber * 2];
                    byte[] bTail = br.ReadBytes(BitConverter.ToInt32(br.ReadBytes(4), 0));
                    bw1.Write(bHead);
                    bw1.Write(bChannels);
                    bw1.Write(bTail.Length);
                    bw1.Write(bTail);
                    //
                    List<Int16>[] listRecord = new List<Int16>[m_dhi.iChannelNumber];
                    while (true)
                    {
                        for (int iVar = 0; iVar < m_dhi.iChannelNumber; iVar++)
                        {
                            listRecord[iVar] = new List<short>();
                        }
                        int iIndex = 0;
                        while (iIndex < 16000 && br.BaseStream.Position < br.BaseStream.Length)
                        {
                            bData = br.ReadBytes(m_dhi.iChannelNumber * 2);

                            if (m_dhi.sDataVersion.StartsWith("3."))
                            {
                                bData = CITDataProcess.ByteXORByte(bData);
                            }

                            for (int cl = 0; cl < m_dhi.iChannelNumber; cl++)
                            {
                                listRecord[cl].Add(BitConverter.ToInt16(bData, cl * 2));
                            }
                            iIndex++;
                        }
                        //
                        for (int iNum = 0; iNum < sTQIItemIndex.Length; iNum++)
                        {
                            float[] fNumber = new float[iIndex];
                            for (int iVal = 0; iVal < iIndex; iVal++)
                            {
                                fNumber[iVal] = listRecord[sTQIItemIndex[iNum]][iVal] / f[sTQIItemIndex[iNum]];
                            }
                            MWNumericArray mwna = fNumber;
                            MWArray mwa = CalcDCInst.cit2filterfun(mwna);
                            Array arr = mwa.ToArray();
                            listRecord[sTQIItemIndex[iNum]].Clear();

                            double[,] fResult = (double[,])arr;
                            Int16[] i16Result = new short[iIndex];
                            for (int iVal = 0; iVal < iIndex; iVal++)
                            {
                                i16Result[iVal] = (Int16)(fResult[0, iVal] * f[sTQIItemIndex[iNum]]);
                            }
                            for (int iVal = 0; iVal < iIndex; iVal++)
                            {
                                listRecord[sTQIItemIndex[iNum]].Add(i16Result[iVal]);
                            }
                        }
                        //写入文件
                        for (int i = 0; i < iIndex; i++)
                        {
                            for (int iCL = 0; iCL < m_dhi.iChannelNumber; iCL++)
                            {
                                //bw1.Write(listRecord[iCL][i]);

                                byte[] b = BitConverter.GetBytes(listRecord[iCL][i]);

                                if (m_dhi.sDataVersion.StartsWith("3"))
                                {
                                    b = CITDataProcess.ByteXORByte(b);
                                }

                                bw1.Write(b);
                            }
                        }
                        if (!(br.BaseStream.Position < br.BaseStream.Length))
                        {
                            break;
                        }
                    }
                    //
                    bw1.Close();
                    br.Close();
                    fsWrite.Close();
                    fsRead.Close();
                    //删除bak
                    Application.DoEvents();
                    File.Delete(sFile);
                    Application.DoEvents();
                    File.Move(sFile + ".bak", sFile);
                    Application.DoEvents();
                    #endregion
                }
                catch
                {

                }
            }
            #endregion

            #region 获取CIT文件中通道下标为iChannelIndex的所有数据，并返回
            /// <summary>
            /// 获取CIT文件中通道下标为iChannelIndex的所有数据，并返回
            /// </summary>
            /// <param name="sFileName">CIT文件名</param>
            /// <param name="lPos">通道数据的起始位置</param>
            /// <param name="iCount">总的采样点数</param>
            /// <param name="iChannelIndex">需要取出数据的通道下标(从0开始)</param>
            /// <param name="gjtds">总的通道数量</param>
            /// <param name="bEncrypt">通道数据是否加密：true-加密；false-不加密</param>
            /// <returns>下标为iChannelIndex的通道数据</returns>
            public float[] PointToByteArray(string sFileName, long lPos, int iCount, int iChannelIndex, int gjtds, bool bEncrypt)
            {
                float[] fData = new float[iCount];
                FileStream fs = new FileStream(sFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(fs);
                br.BaseStream.Position = lPos;
                float[] f = new float[gjtds]; //所有的通道比例
                float[] g = new float[gjtds]; //所有的通道基线值
                wdp.GetChannelScale(sFileName, gjtds, ref f, ref g);
                int i = 0;
                while (br.BaseStream.Position < br.BaseStream.Length && i < iCount)
                {
                    byte[] b = br.ReadBytes(gjtds * 2);
                    if (bEncrypt)
                    { b = WaveformDataProcess.ByteXORByte(b); }
                    //取出通道值
                    fData[i] = float.Parse((BitConverter.ToInt16(b, iChannelIndex * 2)).ToString()) / f[iChannelIndex] + g[iChannelIndex];
                    i++;
                }

                br.Close();
                fs.Close();
                return fData;
            }
            #endregion

            #region 获取CIT文件中位置为lPos的采样点的公里标(单位：厘米)
            /// <summary>
            /// 获取CIT文件中位置为lPos的采样点的公里标(单位：厘米)
            /// </summary>
            /// <param name="sFileName">CIT文件名</param>
            /// <param name="lPos">某一个采样点在文件中的位置</param>
            /// <param name="gjtds">文件中总的通道数</param>
            /// <param name="bEncrypt">是否加密</param>
            /// <returns>采样点的公里标(单位：厘米)</returns>
            public int PointToMile(string sFileName, long lPos, int gjtds, bool bEncrypt)
            {
                int fData = 0;
                FileStream fs = new FileStream(sFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(fs);
                br.BaseStream.Position = lPos;
                float[] f = new float[gjtds];//通道比例
                float[] g = new float[gjtds];//通道基线值
                wdp.GetChannelScale(sFileName, gjtds, ref f, ref g);
                if (br.BaseStream.Position < br.BaseStream.Length)
                {
                    byte[] b = br.ReadBytes(gjtds * 2);
                    if (bEncrypt)
                    { b = WaveformDataProcess.ByteXORByte(b); }
                    short km;
                    ushort m;
                    km = BitConverter.ToInt16(b, 0);
                    m = BitConverter.ToUInt16(b, 2);
                    fData = (int)(km * 100000 + (m / (float)4 * 100));//单位为厘米
                }

                br.Close();
                fs.Close();
                return fData;
            }
            #endregion

            #region 利用相关性修正索引
            /// <summary>
            /// 利用相关性修正一个cit文件的索引
            /// 输入：
            ///     被参考的带有索引的cit文件(一个文件)；
            ///     需要修正的cit文件(一个文件)；
            ///     一些门阀值；
            ///     原始和目标数据点；
            /// </summary>
            /// <param name="sWaveFileName">被参考的带有索引的cit文件</param>
            /// <param name="sDistWaveFileName">需要修正的，没有索引的cit文件</param>
            /// <param name="listIC">被参考的cit文件的索引</param>
            /// <param name="cyjg">被参考的cit文件的采样频率</param>
            /// <param name="gjtds">被参考的cit文件的通道数</param>
            /// <param name="bEncrypt">被参考的cit文件是否加密</param>
            /// <param name="sKmInc">被参考的cit文件的增减里程标</param>
            /// <param name="fSuperelevation">超高门阀值</param>
            /// <param name="fGage">轨距门阀值</param>
            /// <param name="fL_Prof_SC">左高低门阀值</param>
            /// <param name="fR_Prof_SC">右高低门阀值</param>
            /// <param name="iSourceCount">原始数据点</param>
            /// <param name="iDestCount">目标数据点</param>
            /// <param name="iChannelNumber">被修正的cit文件的通道数</param>
            /// <returns></returns>
            public List<IndexOriClass> CorrelationCalc(string sWaveFileName, string sDistWaveFileName, List<IndexOriClass> listIC, int cyjg, int gjtds, bool bEncrypt, string sKmInc, float fSuperelevation, float fGage, float fL_Prof_SC, float fR_Prof_SC, int iSourceCount, int iDestCount,int iChannelNumber)
            {
                string[] sTQIItem = new string[] { "Gage", "Superelevation", "L_Prof_SC", "R_Prof_SC" };
                int[] sTQIItemIndex = new int[sTQIItem.Length];
                int[] sDestTQIItemIndex = new int[sTQIItem.Length];
                //获取通道信息
                CITDataProcess citdp = new CITDataProcess();
                citdp.QueryDataInfoHead(sWaveFileName);
                citdp.QueryDataChannelInfoHead(sWaveFileName);
                //给通道绑定序号
                for (int i = 0; i < sTQIItem.Length; i++)
                {

                    for (int j = 0; j < citdp.dciL.Count; j++)
                    {
                        if (sTQIItem[i].Equals(citdp.dciL[j].sNameEn))
                        {
                            sTQIItemIndex[i] = j;
                            break;
                        }
                    }
                }
                CITDataProcess citdp1 = new CITDataProcess();
                citdp1.QueryDataInfoHead(sDistWaveFileName);
                citdp1.QueryDataChannelInfoHead(sDistWaveFileName);
                //给通道绑定序号
                for (int i = 0; i < sTQIItem.Length; i++)
                {

                    for (int j = 0; j < citdp1.dciL.Count; j++)
                    {
                        if (sTQIItem[i].Equals(citdp1.dciL[j].sNameEn))
                        {
                            sDestTQIItemIndex[i] = j;
                            break;
                        }
                    }
                }
                List<IndexOriClass> listIOC = new List<IndexOriClass>();
                List<float[]> listSourceGageByte = new List<float[]>();
                List<float[]> listSourceSuperelevationByte = new List<float[]>();
                List<float[]> listSourceL_Prof_SCByte = new List<float[]>();
                List<float[]> listSourceR_Prof_SCByte = new List<float[]>();
                List<int> listSourceMile = new List<int>();
                //
                for (int i = 0; i < listIC.Count; i++)
                {
                    float[] b1 = PointToByteArray(sWaveFileName, long.Parse(listIC[i].IndexPoint), iSourceCount * 2, sTQIItemIndex[0], gjtds, bEncrypt);
                    float[] b2 = PointToByteArray(sWaveFileName, long.Parse(listIC[i].IndexPoint), iSourceCount * 2, sTQIItemIndex[1], gjtds, bEncrypt);
                    float[] b3 = PointToByteArray(sWaveFileName, long.Parse(listIC[i].IndexPoint), iSourceCount * 2, sTQIItemIndex[2], gjtds, bEncrypt);
                    float[] b4 = PointToByteArray(sWaveFileName, long.Parse(listIC[i].IndexPoint), iSourceCount * 2, sTQIItemIndex[3], gjtds, bEncrypt);
                    int b0 = PointToMile(sWaveFileName, long.Parse(listIC[i].IndexPoint), gjtds, bEncrypt);
                    listSourceGageByte.Add(b1);
                    listSourceSuperelevationByte.Add(b2);
                    listSourceL_Prof_SCByte.Add(b3);
                    listSourceR_Prof_SCByte.Add(b4);
                    listSourceMile.Add(b0);
                }
                if (listSourceMile.Count == 0)
                {
                    return listIOC;
                }
                File.Delete(sDistWaveFileName + ".log");
                File.Delete(sDistWaveFileName + "_All.log");
                FileStream fs = new FileStream(sDistWaveFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(fs);
                float[] f = new float[iChannelNumber];
                float[] g = new float[iChannelNumber];
                wdp.GetChannelScale(sDistWaveFileName, iChannelNumber, ref f, ref g);
                int iIndex = 1;
                for (int i = 0; i < listIC.Count; i++)
                {
                    long lFindPos = wdp.MeterToPoint(sDistWaveFileName, listSourceMile[i], 4, iChannelNumber, bEncrypt);
                    br.BaseStream.Position = lFindPos;
                    string sStr = "";
                    if (br.BaseStream.Position < br.BaseStream.Length && lFindPos != 0)
                    {
                        float[] fDataGage1 = new float[iDestCount * 2];
                        float[] fDataCrosslevel1 = new float[iDestCount * 2];
                        float[] fDataL_Prof_SC1 = new float[iDestCount * 2];
                        float[] fDataR_Prof_SC1 = new float[iDestCount * 2];
                        int j = 0;
                        long lPP = (br.BaseStream.Position - (iDestCount * 2 * iChannelNumber));
                        long lPP2 = (lPP <= (120 + 65 * iChannelNumber + 4) ? (120 + 65 * iChannelNumber + 4) : lPP);
                        long lCurPos = lFindPos - lPP2;
                        br.BaseStream.Position = lPP2;
                        while (br.BaseStream.Position < br.BaseStream.Length && j < iDestCount * 2)
                        {
                            byte[] b = br.ReadBytes(iChannelNumber * 2);
                            if (bEncrypt)
                            { b = WaveformDataProcess.ByteXORByte(b); }
                            fDataGage1[j] = float.Parse((BitConverter.ToInt16(b, sDestTQIItemIndex[0] * 2)).ToString()) / f[sDestTQIItemIndex[0]];
                            fDataCrosslevel1[j] = float.Parse((BitConverter.ToInt16(b, sDestTQIItemIndex[1] * 2)).ToString()) / f[sDestTQIItemIndex[1]];
                            fDataL_Prof_SC1[j] = float.Parse((BitConverter.ToInt16(b, sDestTQIItemIndex[2] * 2)).ToString()) / f[sDestTQIItemIndex[2]];
                            fDataR_Prof_SC1[j] = float.Parse((BitConverter.ToInt16(b, sDestTQIItemIndex[3] * 2)).ToString()) / f[sDestTQIItemIndex[3]];
                            j++;
                        }
                        float[] fDataGage2 = new float[j];
                        Array.Copy(fDataGage1, fDataGage2, j);
                        float[] fDataCrosslevel2 = new float[j];
                        Array.Copy(fDataCrosslevel1, fDataCrosslevel2, j);
                        float[] fDataL_Prof_SC2 = new float[j];
                        Array.Copy(fDataL_Prof_SC1, fDataL_Prof_SC2, j);
                        float[] fDataR_Prof_SC2 = new float[j];
                        Array.Copy(fDataR_Prof_SC1, fDataR_Prof_SC2, j);
                        //判断
                        MWNumericArray ns1 = listSourceGageByte[i];
                        MWNumericArray nd1 = fDataGage2;
                        MWNumericArray ns2 = listSourceSuperelevationByte[i];
                        MWNumericArray nd2 = fDataCrosslevel2;
                        MWNumericArray ns3 = listSourceL_Prof_SCByte[i];
                        MWNumericArray nd3 = fDataL_Prof_SC2;
                        MWNumericArray ns4 = listSourceR_Prof_SCByte[i];
                        MWNumericArray nd4 = fDataR_Prof_SC2;
                        //存取数据到文件
                        #region 存文件
                        //存轨距
                        //StreamWriter sw = new StreamWriter(sDistWaveFileName + "_Gage800_"
                        //    + (i + 1).ToString() + ".log", false, Encoding.Default);
                        //sw.AutoFlush = true;
                        //for (int k = 0; k < listSourceGageByte[i].Length; k++)
                        //{
                        //    sw.WriteLine(listSourceGageByte[i][k].ToString());
                        //}
                        //sw.Close();
                        //sw = new StreamWriter(sDistWaveFileName + "_Gage8000_"
                        //    + (i + 1).ToString() + ".log", false, Encoding.Default);
                        //sw.AutoFlush = true;
                        //for (int k = 0; k < fDataGage2.Length; k++)
                        //{
                        //    sw.WriteLine(fDataGage2[k].ToString());
                        //}
                        //sw.Close();
                        ////存超高
                        //sw = new StreamWriter(sDistWaveFileName + "_Superelevation800_"
                        //    + (i + 1).ToString() + ".log", false, Encoding.Default);
                        //sw.AutoFlush = true;
                        //for (int k = 0; k < listSourceSuperelevationByte[i].Length; k++)
                        //{
                        //    sw.WriteLine(listSourceSuperelevationByte[i][k].ToString());
                        //}
                        //sw.Close();
                        //sw = new StreamWriter(sDistWaveFileName + "_Superelevation8000_"
                        //    + (i + 1).ToString() + ".log", false, Encoding.Default);
                        //sw.AutoFlush = true;
                        //for (int k = 0; k < fDataCrosslevel2.Length; k++)
                        //{
                        //    sw.WriteLine(fDataCrosslevel2[k].ToString());
                        //}
                        //sw.Close();
                        ////存左高低
                        //sw = new StreamWriter(sDistWaveFileName + "_L_Prof_SC800_"
                        //    + (i + 1).ToString() + ".log", false, Encoding.Default);
                        //sw.AutoFlush = true;
                        //for (int k = 0; k < listSourceL_Prof_SCByte[i].Length; k++)
                        //{
                        //    sw.WriteLine(listSourceL_Prof_SCByte[i][k].ToString());
                        //}
                        //sw.Close();
                        //sw = new StreamWriter(sDistWaveFileName + "_L_Prof_SC8000_"
                        //    + (i + 1).ToString() + ".log", false, Encoding.Default);
                        //sw.AutoFlush = true;
                        //for (int k = 0; k < fDataL_Prof_SC2.Length; k++)
                        //{
                        //    sw.WriteLine(fDataL_Prof_SC2[k].ToString());
                        //}
                        //sw.Close();
                        ////存右高低
                        //sw = new StreamWriter(sDistWaveFileName + "_R_Prof_SC800_"
                        //    + (i + 1).ToString() + ".log", false, Encoding.Default);
                        //sw.AutoFlush = true;
                        //for (int k = 0; k < listSourceR_Prof_SCByte[i].Length; k++)
                        //{
                        //    sw.WriteLine(listSourceR_Prof_SCByte[i][k].ToString());
                        //}
                        //sw.Close();
                        //sw = new StreamWriter(sDistWaveFileName + "_R_Prof_SC8000_"
                        //    + (i + 1).ToString() + ".log", false, Encoding.Default);
                        //sw.AutoFlush = true;
                        //for (int k = 0; k < fDataR_Prof_SC2.Length; k++)
                        //{
                        //    sw.WriteLine(fDataR_Prof_SC2[k].ToString());
                        //}
                        //sw.Close();
                        #endregion
                        //

                        int ArrayValue0 = 0;
                        float ArrayValue1 = 0f;
                        int ArrayValue00 = 0;
                        float ArrayValue11 = 0f;
                        int ArrayValue000 = 0;
                        float ArrayValue111 = 0f;
                        int ArrayValue0000 = 0;
                        float ArrayValue1111 = 0f;
                        try
                        {
                            MWArray[] mw1 = CalcDCInst.sub_correlation(2, ns1, nd1, fGage);
                            MWArray[] mw2 = CalcDCInst.sub_correlation(2, ns2, nd2, fSuperelevation);
                            MWArray[] mw3 = CalcDCInst.sub_correlation(2, ns3, nd3, fL_Prof_SC);
                            MWArray[] mw4 = CalcDCInst.sub_correlation(2, ns4, nd4, fR_Prof_SC);
                            ArrayValue0 = (int)((MWNumericArray)mw1.GetValue(0));
                            ArrayValue1 = (float)((MWNumericArray)mw1.GetValue(1));
                            ArrayValue00 = (int)((MWNumericArray)mw2.GetValue(0));
                            ArrayValue11 = (float)((MWNumericArray)mw2.GetValue(1));
                            ArrayValue000 = (int)((MWNumericArray)mw3.GetValue(0));
                            ArrayValue111 = (float)((MWNumericArray)mw3.GetValue(1));
                            ArrayValue0000 = (int)((MWNumericArray)mw4.GetValue(0));
                            ArrayValue1111 = (float)((MWNumericArray)mw4.GetValue(1));
                        }
                        catch (System.Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            MessageBox.Show(ex.StackTrace);
                        }

                        sStr = "";
                        File.AppendAllText(sDistWaveFileName + "_All.log",
                            "超高：" + ArrayValue11.ToString() + "轨距：" + ArrayValue1.ToString() +
                            "左高低：" + ArrayValue111.ToString() + "右高低：" + ArrayValue1111.ToString() + "||", Encoding.Default);
                        //判断轨距
                        bool bTrue = true;
                        if (ArrayValue00 != -1)//判断超高
                        {
                            IndexOriClass ioc = new IndexOriClass();
                            if (ArrayValue0 != -1)
                            {
                                ioc.iIndexId = 0;
                                ioc.iId = iIndex;
                                ioc.IndexMeter = listIC[i].IndexMeter;
                                ioc.IndexPoint = (lFindPos - lCurPos + ((ArrayValue0 - 1) * 2 * iChannelNumber)).ToString();
                                listIOC.Add(ioc);
                                iIndex++;
                                sStr =
                                    "原始点:" + listIC[i].IndexPoint +
                                    ",原始里程:" + listSourceMile[i].ToString() +
                                    ",原始索引里程:" + listIC[i].IndexMeter +
                                    ",新文件点:" + lFindPos.ToString() +
                                    ",新文件匹配点:" + ioc.IndexPoint +
                                    ",新点与原点的差:" + (Math.Abs(long.Parse(ioc.IndexPoint) - long.Parse(listIC[i].IndexPoint)) / 2 / iChannelNumber) +
                                    ",匹配通道:轨距" +
                                    ",匹配值:" + ArrayValue1.ToString() +
                                    "||";
                                bTrue = false;
                                //留存记录
                                File.AppendAllText(sDistWaveFileName + "轨距.log", sStr, Encoding.Default);
                            }
                            else if (ArrayValue000 != -1)//判断左高低
                            {
                                ioc.iIndexId = 0;
                                ioc.iId = iIndex;
                                ioc.IndexMeter = listIC[i].IndexMeter;
                                ioc.IndexPoint = (lFindPos - lCurPos + ((ArrayValue000 - 1) * 2 * iChannelNumber)).ToString();
                                listIOC.Add(ioc);
                                iIndex++;
                                sStr =
                                    "原始点:" + listIC[i].IndexPoint +
                                    ",原始里程:" + listSourceMile[i].ToString() +
                                    ",原始索引里程:" + listIC[i].IndexMeter +
                                    ",新文件点:" + lFindPos.ToString() +
                                    ",新文件匹配点:" + ioc.IndexPoint +
                                    ",新点与原点的差:" + (Math.Abs(long.Parse(ioc.IndexPoint) - long.Parse(listIC[i].IndexPoint)) / 2 / iChannelNumber) +
                                    ",匹配通道:左高低" +
                                    ",匹配值:" + ArrayValue111.ToString() +
                                    "||";
                                bTrue = false;
                                //留存记录
                                File.AppendAllText(sDistWaveFileName + "左高低.log", sStr, Encoding.Default);
                            }
                            else if (ArrayValue0000 != -1)//判断右高低
                            {
                                ioc.iIndexId = 0;
                                ioc.iId = iIndex;
                                ioc.IndexMeter = listIC[i].IndexMeter;
                                ioc.IndexPoint = (lFindPos - lCurPos + ((ArrayValue0000 - 1) * 2 * iChannelNumber)).ToString();
                                listIOC.Add(ioc);
                                iIndex++;
                                sStr =
                                    "原始点:" + listIC[i].IndexPoint +
                                    ",原始里程:" + listSourceMile[i].ToString() +
                                    ",原始索引里程:" + listIC[i].IndexMeter +
                                    ",新文件点:" + lFindPos.ToString() +
                                    ",新文件匹配点:" + ioc.IndexPoint +
                                    ",新点与原点的差:" + (Math.Abs(long.Parse(ioc.IndexPoint) - long.Parse(listIC[i].IndexPoint)) / 2 / iChannelNumber) +
                                    ",匹配通道:右高低" +
                                    ",匹配值:" + ArrayValue1111.ToString() +
                                    "||";
                                bTrue = false;
                                //留存记录
                                File.AppendAllText(sDistWaveFileName + "右高低.log", sStr, Encoding.Default);
                            }
                            else
                            {
                                ioc.iIndexId = 0;
                                ioc.iId = iIndex;
                                ioc.IndexMeter = listIC[i].IndexMeter;
                                ioc.IndexPoint = (lFindPos - lCurPos + ((ArrayValue00 - 1) * 2 * iChannelNumber)).ToString();
                                //listIOC.Add(ioc)
                                iIndex++;
                                sStr =
                                    "原始点:" + listIC[i].IndexPoint +
                                    ",原始里程:" + listSourceMile[i].ToString() +
                                    ",原始索引里程:" + listIC[i].IndexMeter +
                                    ",新文件点:" + lFindPos.ToString() +
                                    ",新文件匹配点:" + ioc.IndexPoint +
                                    ",新点与原点的差:" + (Math.Abs(long.Parse(ioc.IndexPoint) - long.Parse(listIC[i].IndexPoint)) / 2 / iChannelNumber) +
                                    ",匹配通道:超高" +
                                    ",匹配值:" + ArrayValue11.ToString() +
                                    "||";
                                bTrue = false;
                                //留存记录
                                File.AppendAllText(sDistWaveFileName + "超高.log", sStr, Encoding.Default);
                            }
                        }

                        if (bTrue)//都没有找到
                        {
                            sStr =
                                "原始点:" + listIC[i].IndexPoint +
                                ",原始里程:" + listSourceMile[i].ToString() +
                                ",原始索引里程:" + listIC[i].IndexMeter +
                                ",新文件点:" + lFindPos.ToString() +
                                ",没有相匹配的数据" +
                                "||";
                        }
                    }
                    else
                    {
                        sStr =
                            "原始点:" + listIC[i].IndexPoint +
                            ",原始里程:" + listSourceMile[i].ToString() +
                            ",原始索引里程:" + listIC[i].IndexMeter +
                            ",没有找到新文件点" +
                            "||";
                    }
                    //留存记录
                    File.AppendAllText(sDistWaveFileName + ".log", sStr, Encoding.Default);
                }
                br.Close();
                fs.Close();
                //获取目标
                return listIOC;
            }
            #endregion

            #region 接口函数---没有使用--3个
            //sub_calculate_dynamic_criteria_peak
            public List<CalcClass1> ProcessCalculateDynamicCriteriaPeak(string sFileName, int sPlan, int sTask, int sRecord, int back)
            {
                //LACC横向加速度,VACC垂向加速度,11,12
                DataProcess.CITDataProcess.DataHeadInfo dhi = citdataprocess.GetDataInfoHead(sFileName);
                List<DataProcess.CITDataProcess.DataChannelInfo> listDCI = citdataprocess.GetDataChannelInfoHead(sFileName);
                long fStartPostion = 0;
                long fEndPostion = 0;
                int iLACCIndex = 11;
                //int iVACCIndex = 12;
                float fLACC = 10000f;
                //float fVACC = 10000f;
                for (int i = 0; i < listDCI.Count; i++)
                {
                    if (listDCI[i].sNameEn.Equals("LACC"))
                    {
                        iLACCIndex = i;
                        fLACC = listDCI[i].fScale;
                        break;
                    }
                }
                //for (int i = 0; i < listDCI.Count; i++)
                //{
                //    if (listDCI[i].sNameEn.Equals("VACC"))
                //    {
                //        iVACCIndex = i;
                //        fVACC = listDCI[i].fScale;
                //        break;
                //    }
                //}
                wdp.GetDataStartPositionEndPositionInfoIncludeIndex(ref fStartPostion, ref fEndPostion, sFileName, dhi.iChannelNumber, -1, -1, false);
                List<int> listChannelsVisible = new List<int>();
                listChannelsVisible.Add(iLACCIndex);
                //listChannelsVisible.Add(iVACCIndex);
                string sKmInc = "";
                switch (dhi.iKmInc)
                {
                    case 0:
                        sKmInc = "增";
                        break;
                    case 1:
                        sKmInc = "减";
                        break;
                }
                List<DataProcess.CalcSpace.CalcClass1> listCalcDB = new List<DataProcess.CalcSpace.CalcClass1>();
                for (; fStartPostion < fEndPostion; )
                {
                    float[][] arrayPointF = new float[listChannelsVisible.Count][];
                    List<DataProcess.WaveMeter> listWaveMeter = new List<DataProcess.WaveMeter>();
                    for (int i = 0; i < listChannelsVisible.Count; i++)
                    {
                        arrayPointF[i] = new float[dhi.iSmaleRate * 20000];
                    }
                    wdp.GetAutoDataInfo(listChannelsVisible, ref arrayPointF, ref listWaveMeter, sFileName, ref fStartPostion, 20000, dhi.iSmaleRate, dhi.iChannelNumber, fStartPostion
                        , fEndPostion, sKmInc, true, 20000);
                    for (int j = 0; j < arrayPointF[0].Length; j++)
                    {
                        arrayPointF[0][j] = arrayPointF[0][j] / fLACC;
                    }
                    //for (int j = 0; j < arrayPointF[1].Length; j++)
                    //{
                    //    arrayPointF[1][j] = arrayPointF[1][j] / fVACC;
                    //}
                    //
                    MWNumericArray MWNLACC = arrayPointF[0];
                    //MWNumericArray MWNVACC = arrayPointF[1];
                    //LACC和VACC滤波
                    MWArray MWPROLACC = CalcDCInst.sub_filter_wavelet(MWNLACC, 11);
                    //MWArray MWPROVACC = CalcDCInst.sub_filter_wavelet(MWNVACC, 12);
                    //原始数据计算偏差位置
                    MWArray[] MWPeakLACC = CalcDCInst.sub_calculate_dynamic_criteria_peak(3, MWPROLACC, 0.06);
                    //MWArray[] MWPeakVACC = CalcDCInst.sub_calculate_dynamic_criteria_peak(3, MWNVACC, 0.1);
                    double[,] ArrayPeakLACCValue0 = (double[,])((MWNumericArray)MWPeakLACC.GetValue(0)).ToArray();
                    double[,] ArrayPeakLACCValue1 = (double[,])((MWNumericArray)MWPeakLACC.GetValue(1)).ToArray();
                    double[,] ArrayPeakLACCValue2 = (double[,])((MWNumericArray)MWPeakLACC.GetValue(2)).ToArray();
                    //double[,] ArrayPeakVACCValue0 = (double[,])((MWNumericArray)MWPeakVACC.GetValue(0)).ToArray();
                    //double[,] ArrayPeakVACCValue1 = (double[,])((MWNumericArray)MWPeakVACC.GetValue(1)).ToArray();
                    //double[,] ArrayPeakVACCValue2 = (double[,])((MWNumericArray)MWPeakVACC.GetValue(2)).ToArray();
                    if (ArrayPeakLACCValue0[0, 0] != 0)
                    {
                        int iCount = (int)ArrayPeakLACCValue0[0, 0];
                        for (int i = 0; i < iCount; i++)
                        {
                            int iStart = (int)ArrayPeakLACCValue1[i, 0];
                            int iEnd = (int)ArrayPeakLACCValue1[i, 1];
                            double dValue = (double)ArrayPeakLACCValue2[0, i];
                            DataProcess.CalcSpace.CalcClass1 c1 = new DataProcess.CalcSpace.CalcClass1();
                            c1.planid = sPlan;
                            c1.taskid = sTask;
                            c1.recordid = sRecord;
                            c1.backid = back;
                            c1.startkm = listWaveMeter[iStart - 1].Km;
                            c1.startm = listWaveMeter[iStart - 1].Meter;
                            c1.endkm = listWaveMeter[iEnd - 1].Km;
                            c1.endm = listWaveMeter[iEnd - 1].Meter;
                            c1.value = dValue;
                            c1.des = "CalcDCInst.sub_calculate_dynamic_criteria_peak";
                            c1.channel = "LACC";
                            c1.para = "0.06";
                            listCalcDB.Add(c1);
                        }

                    }
                    //if (ArrayPeakVACCValue0[0, 0] != 0)
                    //{
                    //    int iCount = (int)ArrayPeakVACCValue0[0, 0];
                    //    for (int i = 0; i < iCount; i++)
                    //    {
                    //        int iStart = (int)ArrayPeakVACCValue1[i, 0];
                    //        int iEnd = (int)ArrayPeakVACCValue1[i, 1];
                    //        double dValue = (double)ArrayPeakVACCValue2[0, i];
                    //        DataProcess.CalcSpace.CalcClass1 c1 = new DataProcess.CalcSpace.CalcClass1();
                    //        c1.planid = sPlan;
                    //        c1.taskid = sTask;
                    //        c1.recordid = sRecord;
                    //        c1.backid = back;
                    //        c1.startkm = listWaveMeter[iStart - 1].Km;
                    //        c1.startm = listWaveMeter[iStart - 1].Meter;
                    //        c1.endkm = listWaveMeter[iEnd - 1].Km;
                    //        c1.endm = listWaveMeter[iEnd - 1].Meter;
                    //        c1.value = dValue;
                    //        c1.des = "CalcDCInst.sub_calculate_dynamic_criteria_peak";
                    //        c1.channel = "VACC";
                    //        c1.para = "0.2";
                    //        listCalcDB.Add(c1);
                    //    }

                    //}
                }
                return listCalcDB;
            }

            //sub_calculate_dynamic_criteria_p2p
            public List<CalcClass1> ProcessCalculateDynamicCriteriaP2P(string sFileName, int sPlan, int sTask, int sRecord, int back)
            {
                //LACC横向加速度,VACC垂向加速度,11,12
                DataProcess.CITDataProcess.DataHeadInfo dhi = citdataprocess.GetDataInfoHead(sFileName);
                List<DataProcess.CITDataProcess.DataChannelInfo> listDCI = citdataprocess.GetDataChannelInfoHead(sFileName);
                long fStartPostion = 0;
                long fEndPostion = 0;
                int iLACCIndex = 11;
                //int iVACCIndex = 12;
                float fLACC = 10000f;
                //float fVACC = 10000f;
                for (int i = 0; i < listDCI.Count; i++)
                {
                    if (listDCI[i].sNameEn.Equals("LACC"))
                    {
                        iLACCIndex = i;
                        fLACC = listDCI[i].fScale;
                        break;
                    }
                }
                //for (int i = 0; i < listDCI.Count; i++)
                //{
                //    if (listDCI[i].sNameEn.Equals("VACC"))
                //    {
                //        iVACCIndex = i;
                //        fVACC = listDCI[i].fScale;
                //        break;
                //    }
                //}
                wdp.GetDataStartPositionEndPositionInfoIncludeIndex(ref fStartPostion, ref fEndPostion, sFileName, dhi.iChannelNumber, -1, -1, false);
                List<int> listChannelsVisible = new List<int>();
                listChannelsVisible.Add(iLACCIndex);
                //listChannelsVisible.Add(iVACCIndex);
                string sKmInc = "";
                switch (dhi.iKmInc)
                {
                    case 0:
                        sKmInc = "增";
                        break;
                    case 1:
                        sKmInc = "减";
                        break;
                }
                List<DataProcess.CalcSpace.CalcClass1> listCalcDB = new List<DataProcess.CalcSpace.CalcClass1>();
                for (; fStartPostion < fEndPostion; )
                {
                    float[][] arrayPointF = new float[listChannelsVisible.Count][];
                    List<DataProcess.WaveMeter> listWaveMeter = new List<DataProcess.WaveMeter>();
                    for (int i = 0; i < listChannelsVisible.Count; i++)
                    {
                        arrayPointF[i] = new float[dhi.iSmaleRate * 20000];
                    }
                    wdp.GetAutoDataInfo(listChannelsVisible, ref arrayPointF, ref listWaveMeter, sFileName, ref fStartPostion, 20000, dhi.iSmaleRate, dhi.iChannelNumber, fStartPostion
                        , fEndPostion, sKmInc, true, 20000);
                    for (int j = 0; j < arrayPointF[0].Length; j++)
                    {
                        arrayPointF[0][j] = arrayPointF[0][j] / fLACC;
                    }
                    //for (int j = 0; j < arrayPointF[1].Length; j++)
                    //{
                    //    arrayPointF[1][j] = arrayPointF[1][j] / fVACC;
                    //}
                    //
                    MWNumericArray MWNLACC = arrayPointF[0];
                    //MWNumericArray MWNVACC = arrayPointF[1];
                    ////LACC和VACC滤波
                    MWArray MWPROLACC = CalcDCInst.sub_filter_wavelet(MWNLACC, 11);
                    //MWArray MWPROVACC = CalcDCInst.sub_filter_wavelet(MWNVACC, 11);
                    //原始数据计算偏差位置
                    MWArray[] MWPeakLACC = CalcDCInst.sub_calculate_dynamic_criteria_p2p(3, MWPROLACC, 0.06 * 1.5);
                    //MWArray[] MWPeakVACC = CalcDCInst.sub_calculate_dynamic_criteria_p2p(3, MWPROVACC, 0.06 * 1);
                    double[,] ArrayPeakLACCValue0 = (double[,])((MWNumericArray)MWPeakLACC.GetValue(0)).ToArray();
                    double[,] ArrayPeakLACCValue1 = (double[,])((MWNumericArray)MWPeakLACC.GetValue(1)).ToArray();
                    double[,] ArrayPeakLACCValue2 = (double[,])((MWNumericArray)MWPeakLACC.GetValue(2)).ToArray();
                    //double[,] ArrayPeakVACCValue0 = (double[,])((MWNumericArray)MWPeakVACC.GetValue(0)).ToArray();
                    //double[,] ArrayPeakVACCValue1 = (double[,])((MWNumericArray)MWPeakVACC.GetValue(1)).ToArray();
                    //double[,] ArrayPeakVACCValue2 = (double[,])((MWNumericArray)MWPeakVACC.GetValue(2)).ToArray();
                    if (ArrayPeakLACCValue0[0, 0] != 0)
                    {
                        int iCount = (int)ArrayPeakLACCValue0[0, 0];
                        for (int i = 0; i < iCount; i++)
                        {
                            int iStart = (int)ArrayPeakLACCValue1[i, 0];
                            int iEnd = (int)ArrayPeakLACCValue1[i, 1];
                            double dValue = (double)ArrayPeakLACCValue2[0, i];
                            DataProcess.CalcSpace.CalcClass1 c1 = new DataProcess.CalcSpace.CalcClass1();
                            c1.planid = sPlan;
                            c1.taskid = sTask;
                            c1.recordid = sRecord;
                            c1.backid = back;
                            c1.startkm = listWaveMeter[iStart - 1].Km;
                            c1.startm = listWaveMeter[iStart - 1].Meter;
                            c1.endkm = listWaveMeter[iEnd - 1].Km;
                            c1.endm = listWaveMeter[iEnd - 1].Meter;
                            c1.value = dValue;
                            c1.des = "CalcDCInst.sub_calculate_dynamic_criteria_p2p";
                            c1.channel = "LACC";
                            c1.para = "0.06*1.5";
                            listCalcDB.Add(c1);
                        }

                    }
                    //if (ArrayPeakVACCValue0[0, 0] != 0)
                    //{
                    //    int iCount = (int)ArrayPeakVACCValue0[0, 0];
                    //    for (int i = 0; i < iCount; i++)
                    //    {
                    //        int iStart = (int)ArrayPeakVACCValue1[i, 0];
                    //        int iEnd = (int)ArrayPeakVACCValue1[i, 1];
                    //        double dValue = (double)ArrayPeakVACCValue2[0, i];
                    //        DataProcess.CalcSpace.CalcClass1 c1 = new DataProcess.CalcSpace.CalcClass1();
                    //        c1.planid = sPlan;
                    //        c1.taskid = sTask;
                    //        c1.recordid = sRecord;
                    //        c1.backid = back;
                    //        c1.startkm = listWaveMeter[iStart - 1].Km;
                    //        c1.startm = listWaveMeter[iStart - 1].Meter;
                    //        c1.endkm = listWaveMeter[iEnd - 1].Km;
                    //        c1.endm = listWaveMeter[iEnd - 1].Meter;
                    //        c1.value = dValue;
                    //        c1.des = "CalcDCInst.sub_calculate_dynamic_criteria_p2p";
                    //        c1.channel = "VACC";
                    //        c1.para = "0.06*1";
                    //        listCalcDB.Add(c1);
                    //    }

                    //}
                }
                return listCalcDB;
            }

            //sub_calculate_comprehensive_criteria_on_switch
            public List<CalcClass1> ProcessCalculateComprehensiveCriteriaOnSwitch(string sFileName, int sPlan, int sTask, int sRecord, int back)
            {
                //LACC横向加速度,Gage轨距,11,6
                DataProcess.CITDataProcess.DataHeadInfo dhi = citdataprocess.GetDataInfoHead(sFileName);
                List<DataProcess.CITDataProcess.DataChannelInfo> listDCI = citdataprocess.GetDataChannelInfoHead(sFileName);
                long fStartPostion = 0;
                long fEndPostion = 0;
                int iLACCIndex = 11;
                int iGageIndex = 6;
                float fLACC = 10000f;
                float fGage = 100f;
                //
                for (int i = 0; i < listDCI.Count; i++)
                {
                    if (listDCI[i].sNameEn.Equals("LACC"))
                    {
                        iLACCIndex = i;
                        fLACC = listDCI[i].fScale;
                        break;
                    }
                }
                for (int i = 0; i < listDCI.Count; i++)
                {
                    if (listDCI[i].sNameEn.Equals("Gage"))
                    {
                        iGageIndex = i;
                        fGage = listDCI[i].fScale;
                        break;
                    }
                }
                wdp.GetDataStartPositionEndPositionInfoIncludeIndex(ref fStartPostion, ref fEndPostion, sFileName, dhi.iChannelNumber, -1, -1, false);
                List<int> listChannelsVisible = new List<int>();
                listChannelsVisible.Add(iLACCIndex);
                listChannelsVisible.Add(iGageIndex);
                string sKmInc = "";
                switch (dhi.iKmInc)
                {
                    case 0:
                        sKmInc = "增";
                        break;
                    case 1:
                        sKmInc = "减";
                        break;
                }
                List<DataProcess.CalcSpace.CalcClass1> listCalcDB = new List<DataProcess.CalcSpace.CalcClass1>();
                for (; fStartPostion < fEndPostion; )
                {
                    float[][] arrayPointF = new float[listChannelsVisible.Count][];
                    List<DataProcess.WaveMeter> listWaveMeter = new List<DataProcess.WaveMeter>();
                    for (int i = 0; i < listChannelsVisible.Count; i++)
                    {
                        arrayPointF[i] = new float[dhi.iSmaleRate * 20000];
                    }
                    wdp.GetAutoDataInfo(listChannelsVisible, ref arrayPointF, ref listWaveMeter, sFileName, ref fStartPostion, 20000, dhi.iSmaleRate, dhi.iChannelNumber, fStartPostion
                        , fEndPostion, sKmInc, true, 20000);
                    for (int j = 0; j < arrayPointF[0].Length; j++)
                    {
                        arrayPointF[0][j] = arrayPointF[0][j] / fLACC;
                    }
                    for (int j = 0; j < arrayPointF[1].Length; j++)
                    {
                        arrayPointF[1][j] = arrayPointF[1][j] / fGage;
                    }
                    //
                    MWNumericArray MWNLACC = arrayPointF[0];
                    MWNumericArray MWNGage = arrayPointF[1];
                    //LACC和VACC滤波
                    MWArray MWPROLACC = CalcDCInst.sub_filter_wavelet(MWNLACC, 11);
                    //MWArray MWPROVACC = CalcDCInst.sub_filter_wavelet(MWNVACC, 11);
                    //原始数据计算偏差位置
                    MWArray[] MWPeakLACC = CalcDCInst.sub_calculate_comprehensive_criteria_on_switch(3, MWPROLACC, MWNGage);
                    //MWArray[] MWPeakVACC = CalcDCInst.sub_calculate_dynamic_criteria_p2p(3, MWPROVACC, 0.06 * 1);
                    double[,] ArrayPeakLACCValue0 = (double[,])((MWNumericArray)MWPeakLACC.GetValue(0)).ToArray();
                    double[,] ArrayPeakLACCValue1 = (double[,])((MWNumericArray)MWPeakLACC.GetValue(1)).ToArray();
                    double[,] ArrayPeakLACCValue2 = (double[,])((MWNumericArray)MWPeakLACC.GetValue(2)).ToArray();
                    //double[,] ArrayPeakVACCValue0 = (double[,])((MWNumericArray)MWPeakVACC.GetValue(0)).ToArray();
                    //double[,] ArrayPeakVACCValue1 = (double[,])((MWNumericArray)MWPeakVACC.GetValue(1)).ToArray();
                    //double[,] ArrayPeakVACCValue2 = (double[,])((MWNumericArray)MWPeakVACC.GetValue(2)).ToArray();
                    if (ArrayPeakLACCValue0[0, 0] != 0)
                    {
                        int iCount = (int)ArrayPeakLACCValue0[0, 0];
                        for (int i = 0; i < iCount; i++)
                        {
                            int iStart = (int)ArrayPeakLACCValue1[i, 0];
                            int iEnd = (int)ArrayPeakLACCValue1[i, 1];
                            double dValue = (double)ArrayPeakLACCValue2[0, i];
                            DataProcess.CalcSpace.CalcClass1 c1 = new DataProcess.CalcSpace.CalcClass1();
                            c1.planid = sPlan;
                            c1.taskid = sTask;
                            c1.recordid = sRecord;
                            c1.backid = back;
                            c1.startkm = listWaveMeter[iStart - 1].Km;
                            c1.startm = listWaveMeter[iStart - 1].Meter;
                            c1.endkm = listWaveMeter[iEnd - 1].Km;
                            c1.endm = listWaveMeter[iEnd - 1].Meter;
                            c1.value = dValue;
                            c1.des = "CalcDCInst.sub_calculate_comprehensive_criteria_on_switch";
                            c1.channel = "LACC_Gage";
                            c1.para = "0";
                            listCalcDB.Add(c1);
                        }

                    }
                    //if (ArrayPeakVACCValue0[0, 0] != 0)
                    //{
                    //    int iCount = (int)ArrayPeakVACCValue0[0, 0];
                    //    for (int i = 0; i < iCount; i++)
                    //    {
                    //        int iStart = (int)ArrayPeakVACCValue1[i, 0];
                    //        int iEnd = (int)ArrayPeakVACCValue1[i, 1];
                    //        double dValue = (double)ArrayPeakVACCValue2[0, i];
                    //        DataProcess.CalcSpace.CalcClass1 c1 = new DataProcess.CalcSpace.CalcClass1();
                    //        c1.planid = sPlan;
                    //        c1.taskid = sTask;
                    //        c1.recordid = sRecord;
                    //        c1.backid = back;
                    //        c1.startkm = listWaveMeter[iStart - 1].Km;
                    //        c1.startm = listWaveMeter[iStart - 1].Meter;
                    //        c1.endkm = listWaveMeter[iEnd - 1].Km;
                    //        c1.endm = listWaveMeter[iEnd - 1].Meter;
                    //        c1.value = dValue;
                    //        c1.des = "CalcDCInst.sub_calculate_dynamic_criteria_p2p";
                    //        c1.channel = "VACC";
                    //        c1.para = "0.06*1";
                    //        listCalcDB.Add(c1);
                    //    }

                    //}
                }
                return listCalcDB;
            }
            #endregion

            #region sub_calculate_GEI----网络版使用
            //sub_calculate_GEI
            public List<CalcClass1> ProcessCalculateGEI(string sFileName, int sPlan, int sTask, int sRecord, int back, string sRun, string sTrain)
            {
                //速度
                StreamReader sr = new StreamReader(@"C:\programming\energy weight coefficient\velocity-" + sTrain + "-" + sRun + ".txt", Encoding.Default);
                List<double> listvelocity = new List<double>();
                while (sr.Peek() != -1)
                {
                    string sStr = sr.ReadLine();
                    listvelocity.Add(double.Parse(sStr));
                }
                sr.Close();
                MWNumericArray MWNvelocity = listvelocity.ToArray();
                //prf
                sr = new StreamReader(@"C:\programming\energy weight coefficient\energy-weight-coefficient-profile-" + sTrain + "-" + sRun + ".txt", Encoding.Default);
                List<string[]> listprofile = new List<string[]>();
                while (sr.Peek() != -1)
                {
                    string[] sStr = sr.ReadLine().Split(',');
                    listprofile.Add(sStr);
                }
                sr.Close();
                double[,] profile = new double[listprofile[0].Length, listprofile.Count];
                for (int i = 0; i < listprofile[0].Length; i++)
                {
                    for (int j = 0; j < listprofile.Count; j++)
                    {
                        profile[i, j] = double.Parse(listprofile[j][i]);
                    }
                }
                MWNumericArray MWNprofile = profile;
                //alignment
                sr = new StreamReader(@"C:\programming\energy weight coefficient\energy-weight-coefficient-alignment-" + sTrain + "-" + sRun + ".txt", Encoding.Default);
                List<string[]> alignment = new List<string[]>();
                while (sr.Peek() != -1)
                {
                    string[] sStr = sr.ReadLine().Split(',');
                    alignment.Add(sStr);
                }
                sr.Close();
                double[,] dalignment = new double[alignment[0].Length, alignment.Count];
                for (int i = 0; i < alignment[0].Length; i++)
                {
                    for (int j = 0; j < alignment.Count; j++)
                    {
                        dalignment[i, j] = double.Parse(alignment[j][i]);
                    }
                }
                MWNumericArray MWNalignment = dalignment;
                //lel
                sr = new StreamReader(@"C:\programming\energy weight coefficient\energy-weight-coefficient-level-" + sTrain + "-" + sRun + ".txt", Encoding.Default);
                List<string[]> lel = new List<string[]>();
                while (sr.Peek() != -1)
                {
                    string[] sStr = sr.ReadLine().Split(',');
                    lel.Add(sStr);
                }
                sr.Close();
                double[,] dlel = new double[lel[0].Length, lel.Count];
                for (int i = 0; i < lel[0].Length; i++)
                {
                    for (int j = 0; j < lel.Count; j++)
                    {
                        dlel[i, j] = double.Parse(lel[j][i]);
                    }
                }
                MWNumericArray MWNlel = dlel;
                #region 3
                DataProcess.CITDataProcess.DataHeadInfo dhi = citdataprocess.GetDataInfoHead(sFileName);
                List<DataProcess.CITDataProcess.DataChannelInfo> listDCI = citdataprocess.GetDataChannelInfoHead(sFileName);
                long fStartPostion = 0;
                long fEndPostion = 0;
                int iL_Prof_SC = 2;
                int iR_Prof_SC = 3;
                int iL_Align_SC = 4;
                int iR_Align_SC = 5;
                int iCrosslevel = 8;
                int iGage = 6;
                int iShort_Twist = 9;
                int iSpeed = 13;
                //int iVACCIndex = 12;
                float fL_Prof_SC = 0f;
                float fR_Prof_SC = 0f;
                float fL_Align_SC = 0f;
                float fR_Align_SC = 0f;
                float fCrosslevel = 0f;
                float fGage = 0f;
                float fShort_Twist = 0f;
                float fSpeed = 0f;
                #endregion
                #region 1
                for (int i = 0; i < listDCI.Count; i++)
                {
                    if (listDCI[i].sNameEn.Equals("L_Prof_SC"))
                    {
                        iL_Prof_SC = i;
                        fL_Prof_SC = listDCI[i].fScale;
                        break;
                    }
                }
                for (int i = 0; i < listDCI.Count; i++)
                {
                    if (listDCI[i].sNameEn.Equals("R_Prof_SC"))
                    {
                        iR_Prof_SC = i;
                        fR_Prof_SC = listDCI[i].fScale;
                        break;
                    }
                }
                for (int i = 0; i < listDCI.Count; i++)
                {
                    if (listDCI[i].sNameEn.Equals("L_Align_SC"))
                    {
                        iL_Align_SC = i;
                        fL_Align_SC = listDCI[i].fScale;
                        break;
                    }
                }
                for (int i = 0; i < listDCI.Count; i++)
                {
                    if (listDCI[i].sNameEn.Equals("R_Align_SC"))
                    {
                        iR_Align_SC = i;
                        fR_Align_SC = listDCI[i].fScale;
                        break;
                    }
                }
                for (int i = 0; i < listDCI.Count; i++)
                {
                    if (listDCI[i].sNameEn.Equals("Crosslevel"))
                    {
                        iCrosslevel = i;
                        fCrosslevel = listDCI[i].fScale;
                        break;
                    }
                }
                for (int i = 0; i < listDCI.Count; i++)
                {
                    if (listDCI[i].sNameEn.Equals("Gage"))
                    {
                        iGage = i;
                        fGage = listDCI[i].fScale;
                        break;
                    }
                }
                for (int i = 0; i < listDCI.Count; i++)
                {
                    if (listDCI[i].sNameEn.Equals("Short_Twist"))
                    {
                        iShort_Twist = i;
                        fShort_Twist = listDCI[i].fScale;
                        break;
                    }
                }
                for (int i = 0; i < listDCI.Count; i++)
                {
                    if (listDCI[i].sNameEn.Equals("Speed"))
                    {
                        iSpeed = i;
                        fSpeed = listDCI[i].fScale;
                        break;
                    }
                }

                wdp.GetDataStartPositionEndPositionInfoIncludeIndex(ref fStartPostion, ref fEndPostion, sFileName, dhi.iChannelNumber, -1, -1, false);
                List<int> listChannelsVisible = new List<int>();
                listChannelsVisible.Add(iL_Prof_SC);
                listChannelsVisible.Add(iR_Prof_SC);
                listChannelsVisible.Add(iL_Align_SC);
                listChannelsVisible.Add(iR_Align_SC);
                listChannelsVisible.Add(iCrosslevel);
                listChannelsVisible.Add(iGage);
                listChannelsVisible.Add(iShort_Twist);
                listChannelsVisible.Add(iSpeed);
                string sKmInc = "";
                switch (dhi.iKmInc)
                {
                    case 0:
                        sKmInc = "增";
                        break;
                    case 1:
                        sKmInc = "减";
                        break;
                }
                #endregion
                List<DataProcess.CalcSpace.CalcClass1> listCalcDB = new List<DataProcess.CalcSpace.CalcClass1>();
                for (; fStartPostion < fEndPostion; )
                {
                    float[][] arrayPointF = new float[listChannelsVisible.Count][];
                    List<DataProcess.WaveMeter> listWaveMeter = new List<DataProcess.WaveMeter>();
                    for (int i = 0; i < listChannelsVisible.Count; i++)
                    {
                        arrayPointF[i] = new float[dhi.iSmaleRate * 40000];
                    }
                    int iResultValue = wdp.GetAutoDataInfo(listChannelsVisible, ref arrayPointF, ref listWaveMeter, sFileName, ref fStartPostion, 40000, dhi.iSmaleRate, dhi.iChannelNumber, fStartPostion
                        , fEndPostion, sKmInc, true, 40000);
                    if (iResultValue == -100)
                    {
                        break;
                    }
                    #region 2
                    for (int j = 0; j < arrayPointF[0].Length; j++)
                    {
                        arrayPointF[0][j] = arrayPointF[0][j] / fL_Prof_SC;
                    }
                    for (int j = 0; j < arrayPointF[1].Length; j++)
                    {
                        arrayPointF[1][j] = arrayPointF[1][j] / fR_Prof_SC;
                    }
                    for (int j = 0; j < arrayPointF[2].Length; j++)
                    {
                        arrayPointF[2][j] = arrayPointF[2][j] / fL_Align_SC;
                    }
                    for (int j = 0; j < arrayPointF[3].Length; j++)
                    {
                        arrayPointF[3][j] = arrayPointF[3][j] / fR_Align_SC;
                    }
                    for (int j = 0; j < arrayPointF[4].Length; j++)
                    {
                        arrayPointF[4][j] = arrayPointF[4][j] / fCrosslevel;
                    }
                    for (int j = 0; j < arrayPointF[5].Length; j++)
                    {
                        arrayPointF[5][j] = arrayPointF[5][j] / fGage;
                    }
                    for (int j = 0; j < arrayPointF[6].Length; j++)
                    {
                        arrayPointF[6][j] = arrayPointF[6][j] / fShort_Twist;
                    }
                    for (int j = 0; j < arrayPointF[7].Length; j++)
                    {
                        arrayPointF[7][j] = arrayPointF[7][j] / fSpeed;
                    }
                    #endregion
                    int iCountIndex = listWaveMeter.Count;
                    float[,] arrayPointF7 = new float[7, iCountIndex];
                    for (int i = 0; i < 7; i++)
                    {
                        for (int j = 0; j < iCountIndex; j++)
                        {
                            arrayPointF7[i, j] = arrayPointF[i][j];
                        }
                    }

                    MWNumericArray MWNF7 = arrayPointF7;
                    MWNumericArray MWNSpeed = arrayPointF[7];
                    float[] fArray = new float[iCountIndex];
                    for (int i = 0; i < iCountIndex; i++)
                    {
                        fArray[i] = i * 0.25f;
                    }
                    MWNumericArray MWFloat = fArray;
                    //过滤
                    MWArray[] MWNF7New = (MWArray[])CalcDCInst.sub_preprocessing_track_irre(1, MWFloat, MWNF7, 4f);
                    MWNumericArray MWNF7Last = MWNF7New[0].ToArray();
                    //TQI
                    MWArray[] MWPeakLACC = CalcDCInst.sub_calculate_GEI(3, MWNF7Last, MWNSpeed, MWNvelocity, MWNprofile, MWNalignment, MWNlel);

                    double[,] ArrayPeakLACCValue0 = (double[,])((MWNumericArray)MWPeakLACC.GetValue(0)).ToArray();
                    double[,] ArrayPeakLACCValue1 = (double[,])((MWNumericArray)MWPeakLACC.GetValue(1)).ToArray();
                    double[,] ArrayPeakLACCValue2 = (double[,])((MWNumericArray)MWPeakLACC.GetValue(2)).ToArray();
                    if (ArrayPeakLACCValue0[0, 0] != 0)
                    {
                        int iCount = (int)ArrayPeakLACCValue0.GetLength(1);
                        for (int i = 0; i < iCount; i++)
                        {
                            float iStart = (float)ArrayPeakLACCValue0[0, i];
                            float iEnd = (float)ArrayPeakLACCValue1[0, i];
                            double dValue = (double)ArrayPeakLACCValue2[0, i];
                            DataProcess.CalcSpace.CalcClass1 c1 = new DataProcess.CalcSpace.CalcClass1();
                            c1.planid = sPlan;
                            c1.taskid = sTask;
                            c1.recordid = sRecord;
                            c1.backid = back;
                            c1.startkm = iStart;
                            c1.startm = 0;
                            c1.endkm = iEnd;
                            c1.endm = 0;
                            c1.value = dValue;
                            c1.des = "CalcDCInst.sub_calculate_GEI";
                            c1.channel = "1-7";
                            c1.para = "0";
                            listCalcDB.Add(c1);
                        }

                    }

                }
                return listCalcDB;
            }
            #endregion

            #region 接口函数--没有使用
            //sub_calculate_moving_RMS_on_axlebox_acc
            public List<DataProcess.CalcSpace.CalcClass1>[] ProcessCalculateMovingRMSOnAxleboxAcc(string sFileName, int sPlan, int sTask, int sRecord, int back)
            {
                //LACC横向加速度,VACC垂向加速度,11,12
                DataProcess.CITDataProcess.DataHeadInfo dhi = citdataprocess.GetDataInfoHead(sFileName);
                List<DataProcess.CITDataProcess.DataChannelInfo> listDCI = citdataprocess.GetDataChannelInfoHead(sFileName);

                long fStartPostion = 0;
                long fEndPostion = 0;
                int[] fIndex = new int[7];
                float[] fScale = new float[7];
                //
                List<DataProcess.CalcSpace.CalcClass1>[] listCalcDB = new List<DataProcess.CalcSpace.CalcClass1>[6];
                for (int i = 0; i < listDCI.Count; i++)
                {
                    if (listDCI[i].sNameEn.Equals("左轴垂1"))
                    {
                        fIndex[0] = i;
                        fScale[0] = listDCI[i].fScale;
                        listCalcDB[0] = new List<DataProcess.CalcSpace.CalcClass1>();
                        break;
                    }
                }
                for (int i = 0; i < listDCI.Count; i++)
                {
                    if (listDCI[i].sNameEn.Equals("左轴垂2"))
                    {
                        fIndex[1] = i;
                        fScale[1] = listDCI[i].fScale;
                        listCalcDB[1] = new List<DataProcess.CalcSpace.CalcClass1>();
                        break;
                    }
                }
                for (int i = 0; i < listDCI.Count; i++)
                {
                    if (listDCI[i].sNameEn.Equals("右轴垂1"))
                    {
                        fIndex[2] = i;
                        fScale[2] = listDCI[i].fScale;
                        listCalcDB[2] = new List<DataProcess.CalcSpace.CalcClass1>();
                        break;
                    }
                }
                for (int i = 0; i < listDCI.Count; i++)
                {
                    if (listDCI[i].sNameEn.Equals("右轴垂2"))
                    {
                        fIndex[3] = i;
                        fScale[3] = listDCI[i].fScale;
                        listCalcDB[3] = new List<DataProcess.CalcSpace.CalcClass1>();
                        break;
                    }
                }
                for (int i = 0; i < listDCI.Count; i++)
                {
                    if (listDCI[i].sNameEn.Equals("右轴横1"))
                    {
                        fIndex[4] = i;
                        fScale[4] = listDCI[i].fScale;
                        listCalcDB[4] = new List<DataProcess.CalcSpace.CalcClass1>();
                        break;
                    }
                }
                for (int i = 0; i < listDCI.Count; i++)
                {
                    if (listDCI[i].sNameEn.Equals("左轴横2"))
                    {
                        fIndex[5] = i;
                        fScale[5] = listDCI[i].fScale;
                        listCalcDB[5] = new List<DataProcess.CalcSpace.CalcClass1>();
                        break;
                    }
                }
                for (int i = 0; i < listDCI.Count; i++)
                {
                    if (listDCI[i].sNameEn.Equals("速度"))
                    {
                        fIndex[6] = i;
                        fScale[6] = listDCI[i].fScale;
                        break;
                    }
                }
                wdp.GetDataStartPositionEndPositionInfoIncludeIndex(ref fStartPostion, ref fEndPostion, sFileName, dhi.iChannelNumber, -1, -1, false);
                List<int> listChannelsVisible = new List<int>();
                for (int i = 0; i < fIndex.Length; i++)
                {
                    listChannelsVisible.Add(fIndex[i]);
                }

                string sKmInc = "";
                switch (dhi.iKmInc)
                {
                    case 0:
                        sKmInc = "增";
                        break;
                    case 1:
                        sKmInc = "减";
                        break;
                }

                for (; fStartPostion < fEndPostion; )
                {
                    float[][] arrayPointF = new float[listChannelsVisible.Count][];
                    List<DataProcess.WaveMeter> listWaveMeter = new List<DataProcess.WaveMeter>();
                    for (int i = 0; i < listChannelsVisible.Count; i++)
                    {
                        arrayPointF[i] = new float[dhi.iSmaleRate * 20000];
                    }
                    int iResultValue = wdp.GetAutoDataInfo(listChannelsVisible, ref arrayPointF, ref listWaveMeter, sFileName, ref fStartPostion, 20000, dhi.iSmaleRate, dhi.iChannelNumber, fStartPostion
                        , fEndPostion, sKmInc, false, 20000);
                    if (iResultValue == -100)
                    {
                        break;
                    }
                    MWNumericArray[] MWNLACC = new MWNumericArray[6];
                    for (int i = 0; i < fScale.Length - 1; i++)
                    {
                        for (int j = 0; j < arrayPointF[i].Length; j++)
                        {
                            arrayPointF[i][j] = arrayPointF[i][j] / fScale[i];
                        }
                        MWNLACC[i] = arrayPointF[i];
                    }
                    for (int g = 0; g < fIndex.Length - 1; g++)
                    {
                        //原始数据计算偏差位置
                        MWArray[] MWPeakLACC = CalcDCInst.sub_calculate_moving_RMS_on_axlebox_acc(3, MWNLACC[g], 2000, 60);
                        //MWArray[] MWPeakVACC = CalcDCInst.sub_calculate_dynamic_criteria_p2p(3, MWPROVACC, 0.06 * 1);
                        double[,] ArrayPeakLACCValue0 = (double[,])((MWNumericArray)MWPeakLACC.GetValue(0)).ToArray();
                        double[,] ArrayPeakLACCValue1 = (double[,])((MWNumericArray)MWPeakLACC.GetValue(1)).ToArray();
                        double[,] ArrayPeakLACCValue2 = (double[,])((MWNumericArray)MWPeakLACC.GetValue(2)).ToArray();
                        //double[,] ArrayPeakVACCValue0 = (double[,])((MWNumericArray)MWPeakVACC.GetValue(0)).ToArray();
                        //double[,] ArrayPeakVACCValue1 = (double[,])((MWNumericArray)MWPeakVACC.GetValue(1)).ToArray();
                        //double[,] ArrayPeakVACCValue2 = (double[,])((MWNumericArray)MWPeakVACC.GetValue(2)).ToArray();
                        if (ArrayPeakLACCValue0[0, 0] != 0)
                        {
                            int iCount = (int)ArrayPeakLACCValue0[0, 0];
                            for (int i = 0; i < iCount; i++)
                            {
                                int iStart = (int)ArrayPeakLACCValue1[i, 0];
                                int iEnd = (int)ArrayPeakLACCValue1[i, 1];
                                double dValue = (double)ArrayPeakLACCValue2[0, i];
                                DataProcess.CalcSpace.CalcClass1 c1 = new DataProcess.CalcSpace.CalcClass1();
                                c1.planid = sPlan;
                                c1.taskid = sTask;
                                c1.recordid = sRecord;
                                c1.backid = back;
                                c1.startkm = listWaveMeter[iStart - 1].Km;
                                c1.startm = listWaveMeter[iStart - 1].Meter;
                                c1.endkm = listWaveMeter[iEnd - 1].Km;
                                c1.endm = listWaveMeter[iEnd - 1].Meter;
                                double fSum = arrayPointF[6][iStart - 1] / fScale[6];
                                for (int ii = iStart; ii <= iEnd - 1; ii++)
                                {
                                    fSum += (arrayPointF[6][ii] / fScale[6]);
                                }
                                int iResult = (iEnd - (iStart - 1));
                                if (iResult < 1)
                                {
                                    iResult = 1;
                                }
                                c1.speed = (float)(fSum / iResult);
                                c1.value = dValue;
                                c1.des = "CalcDCInst.sub_calculate_moving_RMS_on_axlebox_acc";
                                c1.channel = (g + 1).ToString();
                                c1.para = "";
                                listCalcDB[g].Add(c1);
                            }
                        }
                    }

                }
                return listCalcDB;
            }
            #endregion

        }
        #endregion

    }

    #region 数据类----网络版使用
    /// <summary>
    /// 数据类----网络版使用
    /// </summary>
    public class GEO2CITBind
    {
        public string sGEO = "";
        public string sCIT = "";
        public string sChinese = "";
    }
    #endregion

    #region 数据类---里程标
    /// <summary>
    /// 数据类---里程标
    /// </summary>
    [Serializable]
    public class WaveMeter
    {
        #region 变量

        #region 公里
        /// <summary>
        /// 单位：公里--对应于第一个通道数
        /// </summary>
        public int Km = 0;
        #endregion
        #region 米
        /// <summary>
        /// 保存cit原始文件时，这里表示采样点数
        /// 保存包含有索引的文件时，这里表示米数
        /// </summary>
        public float Meter = 0;
        #endregion
        #region 文件指针
        /// <summary>
        /// 文件指针：里程标在cit文件中的位置
        /// </summary>
        public long lPosition = 0;
        #endregion

        #endregion

        #region 函数
        #region 获取公里标
        /// <summary>
        /// 获取里程标：单位为米
        /// 不带索引--参数为采样通道的通道比例
        /// 带索引--参数为1
        /// </summary>
        /// <param name="f">取样通道的通道比例</param>
        /// <returns>公里标</returns>
        public int GetMeter(float f)
        {
            return Km * 1000 + (int)(Meter / f);
        }
        #endregion
        #endregion
    }
    #endregion

    #region 数据类--字符串表达式---没有使用
    /// <summary>
    /// 字符串表达式
    /// </summary>
    public class StringExpression
    {
        #region 中缀转后缀
        /// <summary>
        /// 中缀表达式转换为后缀表达式
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string InfixToPostfix(string expression)
        {
            Stack<char> operators = new Stack<char>();
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < expression.Length; i++)
            {
                char ch = expression[i];
                if (char.IsWhiteSpace(ch)) continue;
                switch (ch)
                {
                    case '+':
                    case '-':
                        while (operators.Count > 0)
                        {
                            char c = operators.Pop();   //pop Operator
                            if (c == '(')
                            {
                                operators.Push(c);      //push Operator
                                break;
                            }
                            else
                            {
                                result.Append(c);
                            }
                        }
                        operators.Push(ch);
                        result.Append(" ");
                        break;
                    case '*':
                    case '/':
                        while (operators.Count > 0)
                        {
                            char c = operators.Pop();
                            if (c == '(')
                            {
                                operators.Push(c);
                                break;
                            }
                            else
                            {
                                if (c == '+' || c == '-')
                                {
                                    operators.Push(c);
                                    break;
                                }
                                else
                                {
                                    result.Append(c);
                                }
                            }
                        }
                        operators.Push(ch);
                        result.Append(" ");
                        break;
                    case '(':
                        operators.Push(ch);
                        break;
                    case ')':
                        while (operators.Count > 0)
                        {
                            char c = operators.Pop();
                            if (c == '(')
                            {
                                break;
                            }
                            else
                            {
                                result.Append(c);
                            }
                        }
                        break;
                    default:
                        result.Append(ch);
                        break;
                }
            }
            while (operators.Count > 0)
            {
                result.Append(operators.Pop()); //pop All Operator
            }
            return result.ToString();
        }

        #endregion
        /// <summary>
        /// 求值的经典算法
        /// </summary>
        /// <param name="expression">字符串表达式</param>
        /// <returns></returns>
        public static double Parse(string expression)
        {
            string postfixExpression = InfixToPostfix(expression);
            Stack<double> results = new Stack<double>();
            double x, y;
            for (int i = 0; i < postfixExpression.Length; i++)
            {
                char ch = postfixExpression[i];
                if (char.IsWhiteSpace(ch)) continue;
                switch (ch)
                {
                    case '+':
                        y = results.Pop();
                        x = results.Pop();
                        results.Push(x + y);
                        break;
                    case '-':
                        y = results.Pop();
                        x = results.Pop();
                        results.Push(x - y);
                        break;
                    case '*':
                        y = results.Pop();
                        x = results.Pop();
                        results.Push(x * y);
                        break;
                    case '/':
                        y = results.Pop();
                        x = results.Pop();
                        if (y == 0)
                        {
                            results.Push(0);
                        }
                        else
                        {
                            results.Push(x / y);
                        }
                        break;
                    default:
                        int pos = i;
                        StringBuilder operand = new StringBuilder();
                        do
                        {
                            operand.Append(postfixExpression[pos]);
                            pos++;
                        } while (char.IsDigit(postfixExpression[pos]) || postfixExpression[pos] == '.');
                        i = --pos;
                        results.Push(double.Parse(operand.ToString()));
                        break;
                }
            }
            return results.Peek();
        }
    }
    #endregion

    #region 丢失数据类---网络版使用
    /// <summary>
    /// 丢失数据类---网络版使用
    /// </summary>
    public class LostDataClass
    {
        public int id;
        public long lStartPos;
        public double dStartMeter;
        public long lEndPos;
        public double dSEndMeter;
        public int iCount;
    }
    #endregion

    #region 数据类---长短链
    /// <summary>
    /// 数据类---长短链
    /// </summary>
    public class CDLClass
    {
        /*
         * 长链时：  dKM+iMeter---表示发生长链的公里标，实际长链=iMeter-1000.(长链时iMeter〉1000)
         *          
         * 短链时：  表示在dKM公里附近发生短链，短链=iMeter。
         * 
         * 
         * 
         * 
         */

        /// <summary>
        /// 单位--公里
        /// </summary>
        public float dKM;
        /// <summary>
        /// 单位--米
        /// </summary>
        public int iMeter;
        /// <summary>
        /// 长短链类型
        /// </summary>
        public string sType;
    }
    #endregion

    #region 数据类--原始索引信息__对应与cit同名的idf数据库中的IndexOri表
    /// <summary>
    /// 原始索引信息__对应与cit同名的idf数据库中的IndexOri表
    /// </summary>
    public class IndexOriClass
    {
        /// <summary>
        /// 索引id
        /// </summary>
        public int iId;
        /// <summary>
        /// 索引类型：0-原有的数据；1-新插入的数据
        /// </summary>
        public int iIndexId;
        /// <summary>
        /// 索引对应的文件指针
        /// </summary>
        public string IndexPoint;
        /// <summary>
        /// 索引对应的里程数
        /// </summary>
        public string IndexMeter;
    }
    #endregion

    #region 数据类--与数据库中IndexSta表对应的长短链索引数据类
    /// <summary>
    /// 与数据库中IndexSta表对应的长短链索引数据类
    /// </summary>
    public class IndexStaClass
    {
        /// <summary>
        /// 长短链索引id
        /// </summary>
        public int iID;
        /// <summary>
        /// 这个值估计没什么特殊含义
        /// </summary>
        public int iIndexID;
        /// <summary>
        /// 长短链对应的起始文件指针
        /// </summary>
        public long lStartPoint;
        /// <summary>
        /// 长短链对应的起始公里标
        /// </summary>
        public string lStartMeter;
        /// <summary>
        /// 长短链对应的终止文件指针
        /// </summary>
        public long lEndPoint;
        /// <summary>
        /// 长短链对应的终止公里标
        /// </summary>
        public string LEndMeter;
        /// <summary>
        /// 长短链所包含的采样点数
        /// </summary>
        public long lContainsPoint;
        /// <summary>
        /// 长短链所包含的公里数（单位为公里）
        /// </summary>
        public string lContainsMeter;
        /// <summary>
        /// 长短链类别
        /// </summary>
        public string sType;
    }
    #endregion

    #region 数据类--保存普通标注信息
    /// <summary>
    /// 数据类--保存普通标注信息
    /// </summary>
    public class LabelInfoClass
    {
        #region 标注id
        /// <summary>
        /// 标注id
        /// </summary>
        public int iID { set; get; }
        #endregion
        #region 文件指针
        /// <summary>
        ///  文件指针
        /// </summary>
        public string sMileIndex { set; get; }
        #endregion
        #region 里程位置
        /// <summary>
        /// 里程位置
        /// </summary>
        public string sMile { set; get; } 
        #endregion
        #region 标注信息
        /// <summary>
        /// 标注信息
        /// </summary>
        public string sMemoText { set; get; }
        #endregion
        #region 标注日期
        /// <summary>
        /// 标注日期
        /// </summary>
        public String logDate { set; get; }
        #endregion

        #region 标注的位置和占用的矩形框
        /// <summary>
        /// 标注的位置和占用的矩形框
        /// </summary>
        public Rectangle r = new Rectangle();
        #endregion
    }
    #endregion

    #region 数据类--保存无效标注信息
    /// <summary>
    /// 数据类--保存无效标注信息
    /// </summary>
    public class InvalidDataClass
    {
        #region id---实际上保存的是标注的日期
        /// <summary>
        /// id---实际上保存的是标注的日期
        /// </summary>
        public int iId { set; get; }
        #endregion
        #region 无效标注起始点的文件指针
        /// <summary>
        /// 无效标注起始点的文件指针
        /// </summary>
        public string sStartPoint { set; get; }
        #endregion
        #region 无效标注结束点的文件指针
        /// <summary>
        /// 无效标注结束点的文件指针
        /// </summary>
        public string sEndPoint { set; get; }
        #endregion
        #region 起始点的里程标---起始点文件指针对应的里程标
        /// <summary>
        /// 起始点的里程标---起始点文件指针对应的里程标
        /// </summary>
        public string sStartMile { set; get; }
        #endregion
        #region 结束点的里程标---结束点文件指针对应的里程标
        /// <summary>
        /// 结束点的里程标---结束点文件指针对应的里程标
        /// </summary>
        public string sEndMile { set; get; }
        #endregion
        #region 标注类型--来源于Inner.idf文件
        /// <summary>
        /// 标注类型--来源于Inner.idf文件
        /// 0--数据缺失；1--阳光干扰；2--过分相；3--低速；4--进出站；5--加宽道岔；6--其他
        /// </summary>
        public int iType { set; get; }
        #endregion
        #region 标注内容
        /// <summary>
        /// 标注内容
        /// </summary>
        public string sMemoText { set; get; }
        #endregion
        #region 是否显示
        /// <summary>
        /// 是否显示
        /// </summary>
        public int iIsShow { set; get; }
        #endregion
        #region 无效自动剔除时无效数据所属的通道(手动标注时，该值为空字符串)
        /// <summary>
        /// 无效自动剔除时无效数据所属的通道(手动标注时，该值为空字符串)
        /// </summary>
        public String ChannelType { set; get; } 
        #endregion        
    }
    #endregion

    #region 数据类--实际上没有使用--含义不明？
    public class AreaClass
    {
        public double dStartMeter;
        public double dEndMeter;
    }
    #endregion

    #region 数据类--保存案例库信息---20141008--ygx
    /// <summary>
    /// 数据类--保存案例库信息
    /// </summary>
    public class AnalysisInfoClass
    {
        public int id;
        public String lineCode;
        public String lineName;
        public String lineDir;
        public String detectDate;
        public int secFlag;
        public String startPoint;
        public String startMile;
        public String endPoint;
        public String endMile;
        public int analysisType;
        public int manageType;
        public int importFlag;
        public String opDate;
        public String citName;
        public String attachNum;
        public String memoInfo;
    }
    #endregion

    

    #region 数据类--网络版使用
    public class AutoPointClass
    {
        public string sFileName;
        /// <summary>
        /// iDataType：1轨检、2动力学、3弓网，
        /// </summary>
        public int iDataType;
        public List<int> listChannel;
        public long lReviseValue;
        public long lPos;
        public int XZoomIn;
        public int iSmaleRate;
        public int iChannelNumber;
        public long lStartPosition;
        public long lEndPosition;
        public string sKmInc;
        public bool bEncrypt;
    }
    #endregion

    #region 通用格式数据处理类---CIT
    /// <summary>
    /// 通用格式数据处理类---CIT
    /// </summary>
    public class CITDataProcess
    {
        #region 变量定义
        #region cit文件的文件头信息
        /// <summary>
        /// cit文件的文件头信息
        /// </summary>
        public DataHeadInfo dhi;
        #endregion

        #region cit文件的通道定义信息
        /// <summary>
        /// cit文件的通道定义信息
        /// </summary>
        public List<DataChannelInfo> dciL;
        #endregion
        #endregion

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
            #region 线路编号
            /// <summary>
            /// 线路编号，同PWMIS----1+4个字节
            /// </summary>
            public string sTrackCode;
            #endregion
            #region 中文线路名
            /// <summary>
            /// 中文线路名---1+20个字节
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
            #region 采样数，（距离采样>0, 时间采样<0)
            /// <summary>
            /// 采样数，距离采样>0, 时间采样小于0 ----4个字节
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
        public struct DataChannelInfo
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

        //内部函数
        #region 按照文件头结构体的读取各个数据
        /// <summary>
        /// 读取cit文件头中的文件信息信息，并返回文件头信息结构体
        /// </summary>
        /// <param name="bDataInfo">文件头中包含文件信息的120个字节 </param>
        /// <returns>文件信息结构体</returns>
        private DataHeadInfo GetDataInfoHead(byte[] bDataInfo)
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
            for (int i = 1; i <= (int)bDataInfo[30]; i++, i++)
            {
                sbTrackName.Append(UnicodeEncoding.Default.GetString(bDataInfo, 30 + i, 2));
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
            //为了处理加速度数据，暂时把采样频率设为4 ---- 严广学---20140422
            //dhi.iSmaleRate = 4;
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

        #region 获取CIT文件头里程
        //获取CIT文件头里程
        private string GetCITIndexMile(string sFile)
        {
            try
            {
                List<string> listStart = new List<string>();
                List<string> listEnd = new List<string>();
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFile + ";Persist Security Info=True"))
                {
                    string sqlCreate = "select startmeter,endmeter from indexsta order by id";
                    OleDbCommand sqlcom = new OleDbCommand(sqlCreate, sqlconn);
                    sqlconn.Open();
                    OleDbDataReader oleDBReader = sqlcom.ExecuteReader();

                    while (oleDBReader.Read())
                    {
                        listStart.Add(oleDBReader.GetValue(0).ToString());
                        listEnd.Add(oleDBReader.GetValue(1).ToString());

                    }
                    oleDBReader.Close();
                    sqlconn.Close();
                }
                if (listStart.Count == 0 || listEnd.Count == 0)
                {
                    throw new Exception();
                }
                return "0," + listStart[0].ToString() + "-" + listEnd[listEnd.Count - 1].ToString();
            }
            catch
            {
                return "1,没有找到索引信息";
            }

        }
        #endregion

        #region 获取导出数据的坐标定位
        /// <summary>
        /// 获取导出数据的坐标定位--起点位置的文件指针和终点位置的文件指针
        /// </summary>
        /// <param name="sFile">文件名</param>
        /// <param name="iStartKM">起始里程</param>
        /// <param name="iEndKM">结束里程</param>
        /// <returns>数组：bRet[0]-起始里程在文件中的位置；bRet[1]-结束里程在文件中的位置</returns>
        private long[] GetExportDataInfoPosition(string sFile, int iStartKM, int iEndKM)
        {
            FileStream fs = new FileStream(sFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            BinaryReader br = new BinaryReader(fs, Encoding.Default);
            br.BaseStream.Position = 0;
            br.ReadBytes(120);
            br.ReadBytes(65 * dhi.iChannelNumber);
            br.ReadBytes(BitConverter.ToInt32(br.ReadBytes(4), 0));
            byte[] b = new byte[dhi.iChannelNumber * 2];
            long[] bRet = new long[2];
            bool bRight = true;
            bRet[0] = br.BaseStream.Position; //如果文件中没有找到起始里程和结束里程，则导出整个文件。
            bRet[1] = br.BaseStream.Length;
            //找到
            while (br.PeekChar() != -1)
            {

                b = br.ReadBytes(dhi.iChannelNumber * 2);
                b = ByteXORByte(b);
                short sKM = BitConverter.ToInt16(b, 0);
                short sM = BitConverter.ToInt16(b, 2);
                int iKM = sKM * 1000 + sM / dhi.iSmaleRate;
                if (bRight && iKM == iStartKM)
                {

                    bRight = false;
                    bRet[0] = br.BaseStream.Position - dhi.iChannelNumber * 2;
                }

                if (iKM == iEndKM)
                {
                    bRet[1] = br.BaseStream.Position;
                    break;
                }

            }

            br.Close();
            fs.Close();
            return bRet;

        }
        #endregion





        //接口函数
        #region 查询CIT文件头信息--返回String，同时dhi全局变量赋值
        /// <summary>
        /// 查询CIT文件头信息--返回String，同时dhi全局变量赋值
        /// </summary>
        /// <param name="sFile">全路径文件名</param>
        /// <returns>用逗号分隔</returns>
        public string QueryDataInfoHead(string sFile)
        {
            try
            {
                using (FileStream fs = new FileStream(sFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (BinaryReader br = new BinaryReader(fs, Encoding.Default))
                    {
                        br.BaseStream.Position = 0;
                        dhi = GetDataInfoHead(br.ReadBytes(120));  
                        StringBuilder sbName = new StringBuilder();
                        ///////////////
                        switch (dhi.iDataType) //iDataType：1轨检、2动力学、3弓网，
                        {
                            case 1:
                                sbName.Append("轨检,");
                                break;
                            case 2:
                                sbName.Append("动力学,");
                                break;
                            case 3:
                                sbName.Append("弓网,");
                                break;
                        }

                        ///////////////

                        sbName.Append(dhi.sDataVersion + ",");
                        sbName.Append(dhi.sTrackCode + ",");
                        sbName.Append(dhi.sTrackName + ",");
                        ///////////////
                        switch (dhi.iDir)
                        {
                            case 1:
                                sbName.Append("上,");
                                break;
                            case 2:
                                sbName.Append("下,");
                                break;
                            case 3:
                                sbName.Append("单,");
                                break;
                            default:
                                sbName.Append("上,");
                                break;
                        }

                        ///////////////
                        sbName.Append(dhi.sTrain + ",");
                        sbName.Append(dhi.sDate + ",");
                        sbName.Append(dhi.sTime + ",");
                        //////////////
                        switch (dhi.iRunDir)
                        {
                            case 0:
                                sbName.Append("正,");
                                break;
                            case 1:
                                sbName.Append("反,");
                                break;
                        }

                        ///////////////
                        ///////////////////
                        switch (dhi.iKmInc)
                        {
                            case 0:
                                sbName.Append("增,");
                                break;
                            case 1:
                                sbName.Append("减,");
                                break;
                        }
                        //////////////////
                        sbName.Append(dhi.fkmFrom.ToString() + ",");
                        sbName.Append(dhi.fkmTo.ToString() + ",");
                        sbName.Append(dhi.iSmaleRate.ToString() + ",");
                        sbName.Append(dhi.iChannelNumber.ToString());

                        br.Close();
                        fs.Close();
                        return "0," + sbName.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                return "1," + ex.Message;
            }
        }
        #endregion

        #region 查询CIT文件头信息--返回文件头信息结构体，同时dhi全局变量赋值
        /// <summary>
        /// 查询CIT文件头信息--返回文件头信息结构体，同时dhi全局变量赋值
        /// </summary>
        /// <param name="sFile"></param>
        /// <returns>结构体</returns>
        public DataHeadInfo GetDataInfoHead(string sFile)
        {
            try
            {
                using (FileStream fs = new FileStream(sFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (BinaryReader br = new BinaryReader(fs, Encoding.Default))
                    {
                        br.BaseStream.Position = 0;
                        dhi = GetDataInfoHead(br.ReadBytes(120));

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return dhi;
        }
        #endregion

        #region 查询CIT文件头信息--返回文件头信息结构体
        /// <summary>
        /// 查询CIT文件头信息--返回文件头信息结构体
        /// </summary>
        /// <param name="sFile"></param>
        /// <returns>结构体</returns>
        public DataHeadInfo GetDataInfoHeadNew(string sFile)
        {
            DataHeadInfo m_dhi=new DataHeadInfo();
            try
            {
                using (FileStream fs = new FileStream(sFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (BinaryReader br = new BinaryReader(fs, Encoding.Default))
                    {
                        br.BaseStream.Position = 0;
                        m_dhi = GetDataInfoHead(br.ReadBytes(120));

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return m_dhi;
        }
        #endregion

        #region 把文件头结构体按照文件格式转换为字节流----ygx---20140717
        /// <summary>
        /// 把文件头结构体按照文件格式转换为字节流----ygx---20140717
        /// </summary>
        /// <param name="m_dhi">文件头结构体</param>
        /// <returns>字节流</returns>
        public Byte[] GetBytesFromDataHeadInfo(DataHeadInfo structCITHead)
        {
            List<Byte> byteList = new List<Byte>();

            MemoryStream mStream=new MemoryStream();
            BinaryWriter bw=new BinaryWriter(mStream,Encoding.UTF8);

            //文件类型---1
            bw.Write(structCITHead.iDataType);
            //文件版本号----2
            Byte[] tmpBytes = UnicodeEncoding.Default.GetBytes(structCITHead.sDataVersion);
            bw.Write((Byte)(tmpBytes.Length));
            bw.Write(tmpBytes);
            if (tmpBytes.Length < 20)
            {
                for (int i = 0; i < (20 - tmpBytes.Length); i++)
                {
                    bw.Write((byte)0);
                }
            }

            //线路代码----3
            tmpBytes = UnicodeEncoding.Default.GetBytes(structCITHead.sTrackCode);
            bw.Write((Byte)(tmpBytes.Length));
            bw.Write(tmpBytes);
            if (tmpBytes.Length < 4)
            {
                for (int i = 0; i < (4 - tmpBytes.Length); i++)
                {
                    bw.Write((byte)0);
                }
            }
            //线路名，英文最好----4
            tmpBytes = UnicodeEncoding.Default.GetBytes(structCITHead.sTrackName);
            bw.Write((Byte)(tmpBytes.Length));
            bw.Write(tmpBytes);
            if (tmpBytes.Length < 20)
            {
                for (int i = 0; i < (20 - tmpBytes.Length); i++)
                {
                    bw.Write((byte)0);
                }
            }
            //行别：1-上，2-下，3-单线----5
            bw.Write(structCITHead.iDir);
            //检测车号---6
            tmpBytes = UnicodeEncoding.Default.GetBytes(structCITHead.sTrain);
            bw.Write((Byte)(tmpBytes.Length));
            bw.Write(tmpBytes);
            for (int i = 0; i < 20 - tmpBytes.Length; i++)
            {
                bw.Write((byte)0);
            }
            //检测日期：yyyy-MM-dd-----7
            tmpBytes = UnicodeEncoding.Default.GetBytes(structCITHead.sDate);
            bw.Write((Byte)(tmpBytes.Length));
            bw.Write(tmpBytes);
            for (int i = 0; i < 10 - tmpBytes.Length; i++)
            {
                bw.Write((byte)0);
            }
            //检测起始时间：HH:mm:ss----8
            tmpBytes = UnicodeEncoding.Default.GetBytes(structCITHead.sTime);
            bw.Write((Byte)(tmpBytes.Length));
            bw.Write(tmpBytes);
            for (int i = 0; i < 8 - tmpBytes.Length; i++)
            {
                bw.Write((byte)0);
            }
            //检测方向，正0，反1-----9
            bw.Write(structCITHead.iRunDir);
            //增里程0，减里程1-----10
            bw.Write(structCITHead.iKmInc);
            //开始里程-----11
            bw.Write(structCITHead.fkmFrom);
            //结束里程，检测结束后更新----12
            bw.Write(structCITHead.fkmTo);
            //采样数，（距离采样>0, 时间采样<0）----13
            bw.Write(structCITHead.iSmaleRate);
            //数据块中通道总数----14
            bw.Write(structCITHead.iChannelNumber);

            bw.Flush();
            bw.Close();

            byte[] tmp = mStream.ToArray();

            mStream.Flush();
            mStream.Close();

            byteList.AddRange(tmp);

            return byteList.ToArray();
        }
        #endregion

        #region 查询CIT通道定义信息，同时dciL全局变量赋值
        /// <summary>
        /// 查询CIT通道定义信息，同时dciL全局变量赋值
        /// 返回：所有通道的英文名，用逗号隔开
        /// </summary>
        /// <param name="sFile">cit文件名(全路径)</param>
        /// <returns>所有通道的英文名，用逗号隔开</returns>
        public string QueryDataChannelInfoHead(string sFile)
        {
            try
            {
                using (FileStream fs = new FileStream(sFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (BinaryReader br = new BinaryReader(fs, Encoding.Default))
                    {
                        br.BaseStream.Position = 0;

                        br.ReadBytes(120);
                        byte[] bChannelData = br.ReadBytes(dhi.iChannelNumber * 65);
                        StringBuilder sbName = new StringBuilder();
                        //全局变量赋值
                        dciL = new List<DataChannelInfo>();
                        for (int i = 0; i < dhi.iChannelNumber * 65; i += 65)
                        {
                            DataChannelInfo dci = GetChannelInfo(bChannelData, i);
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
                        return "0," + sbName.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                return "1," + ex.Message;
            }
        }
        #endregion

        #region 查询CIT通道信息--返回通道定义结构体列表，同时dciL全局变量赋值
        /// <summary>
        /// 查询CIT通道信息--返回通道定义结构体列表，同时dciL全局变量赋值
        /// 返回：通道定义信息结构体对象列表
        /// </summary>
        /// <param name="sFile">CIT文件名（全路径）</param>
        /// <returns>返回结构体</returns>
        public List<DataChannelInfo> GetDataChannelInfoHead(string sFile)
        {
            try
            {
                using (FileStream fs = new FileStream(sFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (BinaryReader br = new BinaryReader(fs, Encoding.Default))
                    {
                        br.BaseStream.Position = 0;

                        br.ReadBytes(120);
                        byte[] bChannelData = br.ReadBytes(dhi.iChannelNumber * 65);
                        dciL = new List<DataChannelInfo>();
                        for (int i = 0; i < dhi.iChannelNumber * 65; i += 65)
                        {
                            DataChannelInfo dci = GetChannelInfo(bChannelData, i);
                            if (i == 65)
                            {
                                dci.fScale = 4;
                            }
                            dciL.Add(dci);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return dciL;
        }
        #endregion

        #region 查询CIT通道信息--返回通道定义结构体列表--ygx-20140624
        /// <summary>
        /// 查询CIT通道信息--返回通道定义结构体列表--ygx-20140624
        /// 返回：通道定义信息结构体对象列表
        /// </summary>
        /// <param name="sFile">CIT文件名（全路径）</param>
        /// <returns>返回结构体</returns>
        public List<DataChannelInfo> GetDataChannelInfoHeadNew(string sFile)
        {
            List<DataChannelInfo> dciL = new List<DataChannelInfo>();
            try
            {
                using (FileStream fs = new FileStream(sFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (BinaryReader br = new BinaryReader(fs, Encoding.Default))
                    {
                        br.BaseStream.Position = 0;
                        DataHeadInfo dhi = GetDataInfoHead(br.ReadBytes(120));
                        //br.ReadBytes(120);
                        byte[] bChannelData = br.ReadBytes(dhi.iChannelNumber * 65);
                        dciL = new List<DataChannelInfo>();
                        for (int i = 0; i < dhi.iChannelNumber * 65; i += 65)
                        {
                            DataChannelInfo dci = GetChannelInfo(bChannelData, i);
                            if (i == 65)
                            {
                                dci.fScale = 4;
                            }
                            dciL.Add(dci);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return dciL;
        }
        #endregion

        #region 把通道结构体列表按照文件格式，转换为字节流----ygx---20140717
        /// <summary>
        /// 把通道结构体列表按照文件格式，转换为字节流----ygx---20140717
        /// </summary>
        /// <param name="m_dciL">通道结构体列表</param>
        /// <returns>字节流</returns>
        public Byte[] GetBytesFromChannelDataInfoList(List<DataChannelInfo> m_dciL)
        {
            List<Byte> byteList = new List<Byte>();

            if (m_dciL == null || m_dciL.Count == 0)
            {
                MessageBox.Show("通道结构体为空");
                return byteList.ToArray();
            }

            MemoryStream mStream = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(mStream, Encoding.UTF8);

            foreach (DataChannelInfo m_dci in m_dciL)
            {
                //1----轨检通道从1～1000定义,动力学从1001~2000,弓网从2001~3000
                bw.Write(m_dci.sID);
                //2----通道英文名，不足补空格
                Byte[] tmpBytes = UnicodeEncoding.Default.GetBytes(m_dci.sNameEn);
                bw.Write((Byte)(tmpBytes.Length));
                bw.Write(tmpBytes);
                if (tmpBytes.Length < 20)
                {
                    for (int i = 0; i < (20 - tmpBytes.Length); i++)
                    {
                        bw.Write((byte)0);
                    }
                }
                //3----通道中文名，不足补空格
                tmpBytes = UnicodeEncoding.Default.GetBytes(m_dci.sNameCh);
                bw.Write((Byte)(tmpBytes.Length));
                bw.Write(tmpBytes);
                if (tmpBytes.Length < 20)
                {
                    for (int i = 0; i < (20 - tmpBytes.Length); i++)
                    {
                        bw.Write((byte)0);
                    }
                }
                //4----通道比例
                bw.Write(m_dci.fScale);
                //5----通道基线值
                bw.Write(m_dci.fOffset);
                //6----通道单位
                tmpBytes = UnicodeEncoding.Default.GetBytes(m_dci.sUnit);
                bw.Write((Byte)(tmpBytes.Length));
                bw.Write(tmpBytes);
                if (tmpBytes.Length < 10)
                {
                    for (int i = 0; i < (10 - tmpBytes.Length); i++)
                    {
                        bw.Write((byte)0);
                    }
                }

            }

            bw.Flush();
            bw.Close();

            byte[] tmp = mStream.ToArray();

            mStream.Flush();
            mStream.Close();

            byteList.AddRange(tmp);


            return byteList.ToArray();
        }
        #endregion

        #region 创建波形配置文件
        /// <summary>
        /// 创建波形配置文件
        /// </summary>
        /// <param name="sFileName"></param>
        /// <returns></returns>
        public string CreateWaveXMLConfig(string sFileName, int iType)
        {
            string sDestFileName = "";
            if (iType == 0)
            {
                if (Path.GetDirectoryName(sFileName).EndsWith("\\"))
                {
                    sDestFileName = Path.GetDirectoryName(sFileName) + "默认配置文件.xml";
                }
                else
                {
                    sDestFileName = Path.GetDirectoryName(sFileName) + "\\" + "默认配置文件.xml";
                }
            }
            else
            {
                sDestFileName = sFileName;
            }
            if (File.Exists(sDestFileName))
            {
                File.Delete(sDestFileName);
            }
            if (dciL != null && dciL.Count > 0)
            {
                XmlDocument xmldoc = new XmlDocument();
                XmlDeclaration xmldecl;
                xmldecl = xmldoc.CreateXmlDeclaration("1.0", "utf-8", null);
                xmldoc.AppendChild(xmldecl);
                XmlElement xmlelem = xmldoc.CreateElement("", "app", "");
                xmldoc.AppendChild(xmlelem);
                XmlNode root = xmldoc.SelectSingleNode("app");
                XmlElement xmlelem1 = xmldoc.CreateElement("channels");
                xmlelem1.SetAttribute("count", (dciL.Count - 2).ToString());
                root.AppendChild(xmlelem1);
                XmlNode root1 = root.SelectSingleNode("channels");

                int interLen = (int)(100 / (dciL.Count - 2 + 1));

                for (int i = 2; i < dciL.Count; i++)
                {
                    DataChannelInfo dci = dciL[i];
                    XmlElement xmlelem2 = xmldoc.CreateElement("channel");
                    xmlelem2.SetAttribute("id", dci.sID.ToString());
                    xmlelem2.SetAttribute("name", dci.sNameEn);
                    xmlelem2.SetAttribute("non-chinese_name", dci.sNameEn);
                    xmlelem2.SetAttribute("chinese_name", dci.sNameCh);
                    xmlelem2.SetAttribute("color", "ff0000b4");
                    xmlelem2.SetAttribute("visible", "True");

                    if (dciL[i].sNameEn == "Superelevation" || dciL[i].sNameEn == "Speed")
                    {
                        xmlelem2.SetAttribute("zoomin", "20");
                    }
                    else if (dciL[i].sNameEn == "LACC" || dciL[i].sNameEn == "VACC")
                    {
                        xmlelem2.SetAttribute("zoomin", "0.01");
                    }
                    else
                    {
                        xmlelem2.SetAttribute("zoomin", "1");
                    }
                    
                    xmlelem2.SetAttribute("units", dci.sUnit);
                    xmlelem2.SetAttribute("mea-offset", "False");
                    xmlelem2.SetAttribute("location", ((i-1)*interLen).ToString());
                    xmlelem2.SetAttribute("line_width", "2");

                    root1.AppendChild(xmlelem2);

                }
                xmldoc.Save(sDestFileName);
            }
            return sDestFileName;

        }
        #endregion        

        #region 获取数据文件里程范围--cit文件的起始和结束位置的里程标
        /// <summary>
        /// 获取数据文件里程范围--cit文件的起始和结束位置的里程标
        /// </summary>
        /// <param name="sFile">文件名</param>
        /// <param name="bIndex">是否加载索引里程</param>
        /// <param name="bEncrypt">是否加密</param>
        /// <returns></returns>
        public string QueryDataMileageRange(string sFile, bool bIndex, bool bEncrypt)
        {
            try
            {
                if (bIndex)//有索引
                {
                    return GetCITIndexMile(Path.GetDirectoryName(sFile) + "\\" + Path.GetFileNameWithoutExtension(sFile) + ".idf");
                }
                else
                {
                    #region 未索引获取里程
                    using (FileStream fs = new FileStream(sFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (BinaryReader br = new BinaryReader(fs, Encoding.Default))
                        {
                            br.BaseStream.Position = 0;
                            br.ReadBytes(120);
                            br.ReadBytes(65 * dhi.iChannelNumber);
                            br.ReadBytes(BitConverter.ToInt32(br.ReadBytes(4), 0));
                            byte[] b = new byte[dhi.iChannelNumber * 2];
                            string bRet = string.Empty;

                            //定位头
                            long iCurPos = 0;
                            while (br.PeekChar() != -1)
                            {

                                b = br.ReadBytes(dhi.iChannelNumber * 2);
                                if (bEncrypt)
                                { b = ByteXORByte(b); }
                                short sKM = BitConverter.ToInt16(b, 0);
                                short sM = BitConverter.ToInt16(b, 2);
                                int iKM = sKM * 1000 + sM / 4;
                                if (!((iKM <= 0) || (sKM > 9999 || sKM < 0) || (sM < 0)))
                                {
                                    bRet += (iKM / 1000.0).ToString("f3");
                                    iCurPos = br.BaseStream.Position;
                                    break;
                                }


                            }

                            //定位
                            int iReturnPosition = dhi.iChannelNumber * 2;
                            while (iCurPos <= br.BaseStream.Position)
                            {
                                br.BaseStream.Position = br.BaseStream.Length - iReturnPosition;
                                b = br.ReadBytes(dhi.iChannelNumber * 2);
                                if (bEncrypt)
                                { b = ByteXORByte(b); }
                                short sKM = BitConverter.ToInt16(b, 0);
                                short sM = BitConverter.ToInt16(b, 2);
                                int iKM = sKM * 1000 + sM / 4;
                                if (!((iKM <= 0) || (sKM > 9999 ||sKM < 0) || (sM < 0)))
                                {
                                    bRet += ("-" + ((iKM / 1000.0).ToString("f3")));
                                    break;
                                }
                                iReturnPosition += (dhi.iChannelNumber * 2);

                            }


                            br.Close();
                            fs.Close();
                            return "0," + bRet;
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                return "1," + ex.Message;
            }

        }
        #endregion

        #region 针对通道数据的解密算法
        /// <summary>
        /// 针对通道数据的解密算法
        /// </summary>
        /// <param name="b">通道原数据</param>
        /// <returns>解密之后的通道数据</returns>
        public static byte[] ByteXORByte(byte[] b)
        {
            for (int iIndex = 0; iIndex < b.Length; iIndex++)
            {
                b[iIndex] = (byte)(b[iIndex] ^ 128);
            }
            return b;
        }
        #endregion        

        #region 获取指定通道所有数据----没有使用
        public double[,] GetSingleChannelDataAll(string sSourceFile, int iChannelNumber)
        {
            try
            {
                FileStream fs = new FileStream(sSourceFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(fs, Encoding.Default);
                br.BaseStream.Position = 0;
                br.ReadBytes(120);
                br.ReadBytes(65 * dhi.iChannelNumber);
                br.ReadBytes(BitConverter.ToInt32(br.ReadBytes(4), 0));
                int iChannelNumberSize = dhi.iChannelNumber * 2;
                byte[] b = new byte[iChannelNumberSize];
                long iArray = (br.BaseStream.Length - br.BaseStream.Position) / iChannelNumberSize;
                double[,] fReturnArray = new double[2, iArray];
                for (int i = 0; i < iArray; i++)
                {
                    b = br.ReadBytes(iChannelNumberSize);
                    if (dhi.sDataVersion.StartsWith("3."))
                    {
                        b = ByteXORByte(b);
                    }
                    double fGL = (BitConverter.ToInt16(b, 0) / dciL[0].fScale) + dciL[0].fOffset;
                    //根据采样点数计算公里数
                    fGL += ( ( ( BitConverter.ToInt16(b, 2) / dciL[1].fScale + dciL[1].fOffset ) ) / 1000.0 );
                    fReturnArray[0, i] = fGL;
                    fGL = (BitConverter.ToInt16(b, (iChannelNumber - 1) * 2) / dciL[iChannelNumber - 1].fScale + dciL[iChannelNumber - 1].fOffset);

                    fReturnArray[1, i] = fGL;
                }


                br.Close();
                fs.Close();

                return fReturnArray;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return new double[1, 1];
            }
        }
        #endregion

        #region 获取指定通道数据
        /// <summary>
        /// 获取指定通道数据
        /// </summary>
        /// <param name="sSourceFile">cit文件</param>
        /// <param name="iChannelNumber">通道号（从1开始的）</param>
        /// <returns>通道数据</returns>
        public double[] GetSingleChannelData(string sSourceFile, int iChannelNumber)
        {
            try
            {
                List<DataChannelInfo> m_dcil = GetDataChannelInfoHeadNew(sSourceFile);

                FileStream fs = new FileStream(sSourceFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(fs, Encoding.Default);
                br.BaseStream.Position = 0;

                DataHeadInfo m_dhi = GetDataInfoHead(br.ReadBytes(120));

                br.ReadBytes(65 * m_dhi.iChannelNumber);
                br.ReadBytes(BitConverter.ToInt32(br.ReadBytes(4), 0));
                int iChannelNumberSize = m_dhi.iChannelNumber * 2;
                byte[] b = new byte[iChannelNumberSize];
                long iArray = (br.BaseStream.Length - br.BaseStream.Position) / iChannelNumberSize;
                double[] fReturnArray = new double[iArray];
                for (int i = 0; i < iArray; i++)
                {
                    b = br.ReadBytes(iChannelNumberSize);
                    if (m_dhi.sDataVersion.StartsWith("3."))
                    {
                        b = ByteXORByte(b);
                    }

                    double fGL = (BitConverter.ToInt16(b, (iChannelNumber - 1) * 2) / m_dcil[iChannelNumber - 1].fScale + m_dcil[iChannelNumber - 1].fOffset);

                    fReturnArray[i] = fGL;
                }


                br.Close();
                fs.Close();

                return fReturnArray;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return new double[1];
            }
        }
        #endregion

        #region 根据通道英文名和中文名获取通道序号----ygx---20140719
        /// <summary>
        /// 根据通道英文名和中文名获取通道序号----ygx---20140719
        /// </summary>
        /// <param name="FileName">cit文件名</param>
        /// <param name="EnName">通道英文名</param>
        /// <param name="CnName">通道中文名</param>
        /// <returns>通道序号</returns>
        public int GetChannelNumberByChannelName(string FileName, string EnName, string CnName)
        {
            List<CITDataProcess.DataChannelInfo> dci = GetDataChannelInfoHeadNew(FileName);
            int ChannelNumber = -1;
            for (int i = 0; i < dci.Count; i++)
            {
                if ((dci[i].sNameEn.Equals(EnName) && (EnName != "")) || (dci[i].sNameCh.Equals(CnName)) && (CnName != ""))
                {
                    ChannelNumber = i + 1;
                    break;
                }
            }
            return ChannelNumber;
        }

        /// <summary>
        /// 根据通道英文名和中文名获取通道序号(模糊查询)
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="EnName"></param>
        /// <param name="CnName"></param>
        /// <returns></returns>
        public int GetChannelNumberByChannelNameLike(string FileName, string EnName, string CnName)
        {
            List<CITDataProcess.DataChannelInfo> dci = GetDataChannelInfoHeadNew(FileName);
            int ChannelNumber = -1;
            for (int i = 0; i < dci.Count; i++)
            {
                if ((dci[i].sNameEn.Contains(EnName) && (EnName != "")) || (dci[i].sNameCh.Contains(CnName)) && (CnName != ""))
                {
                    ChannelNumber = i + 1;
                    break;
                }
            }
            return ChannelNumber;
        }
        #endregion

        #region 获取指定通道数据---指定范围内--ygx--20140624
        /// <summary>
        /// 获取指定通道数据---指定范围内--ygx--20140624
        /// </summary>
        /// <param name="sSourceFile">cit文件</param>
        /// <param name="iChannelNumber">通道号（从1开始的）</param>
        /// <param name="startPos">起始文件指针</param>
        /// <param name="endPos">结束文件指针</param>
        /// <returns>通道数据</returns>
        public double[] GetSingleChannelData(string sSourceFile, int iChannelNumber,long startPos,long endPos)
        {
            try
            {
                List<DataChannelInfo> m_dcil = GetDataChannelInfoHeadNew(sSourceFile);

                FileStream fs = new FileStream(sSourceFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(fs, Encoding.Default);
                br.BaseStream.Position = 0;

                DataHeadInfo m_dhi = GetDataInfoHead(br.ReadBytes(120));

                //br.ReadBytes(120);
                

                br.ReadBytes(65 * m_dhi.iChannelNumber);
                br.ReadBytes(BitConverter.ToInt32(br.ReadBytes(4), 0));
                int iChannelNumberSize = m_dhi.iChannelNumber * 2;
                byte[] b = new byte[iChannelNumberSize];

                br.BaseStream.Position = startPos;

                long iArray = (endPos - br.BaseStream.Position) / iChannelNumberSize;
                double[] fReturnArray = new double[iArray];
                for (int i = 0; i < iArray; i++)
                {
                    try
                    {
                        b = br.ReadBytes(iChannelNumberSize);
                        if (m_dhi.sDataVersion.StartsWith("3."))
                        {
                            b = ByteXORByte(b);
                        }

                        double fGL = (BitConverter.ToInt16(b, (iChannelNumber - 1) * 2) / m_dcil[iChannelNumber - 1].fScale + m_dcil[iChannelNumber - 1].fOffset);

                        fReturnArray[i] = fGL;
                    }
                    catch (System.Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }


                br.Close();
                fs.Close();

                return fReturnArray;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return new double[1];
            }
        }
        #endregion

        #region 获取指定通道数据--返回单精度浮点数--20140105-ygx
        /// <summary>
        /// 获取指定通道数据--返回单精度浮点数
        /// </summary>
        /// <param name="sSourceFile">cit文件</param>
        /// <param name="iChannelNumber">通道号（从1开始的）</param>
        /// <returns>通道数据</returns>
        public float[] GetSingleChannelDataFloat(String citFilePath, int channelId)
        {
            try
            {
                FileStream fs = new FileStream(citFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(fs, Encoding.Default);
                br.BaseStream.Position = 0;
                br.ReadBytes(120);
                br.ReadBytes(65 * dhi.iChannelNumber);
                br.ReadBytes(BitConverter.ToInt32(br.ReadBytes(4), 0));
                int iChannelNumberSize = dhi.iChannelNumber * 2;
                byte[] b = new byte[iChannelNumberSize];
                long iArray = (br.BaseStream.Length - br.BaseStream.Position) / iChannelNumberSize;
                float[] fReturnArray = new float[iArray];
                for (int i = 0; i < iArray; i++)
                {
                    b = br.ReadBytes(iChannelNumberSize);
                    if (dhi.sDataVersion.StartsWith("3."))
                    {
                        b = ByteXORByte(b);
                    }

                    float fGL = (BitConverter.ToInt16(b, (channelId - 1) * 2) / dciL[channelId - 1].fScale + dciL[channelId - 1].fOffset);

                    fReturnArray[i] = fGL;
                }


                br.Close();
                fs.Close();

                return fReturnArray;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return new float[1];
            }
        }
        #endregion

        #region 获取cit文件中的所有公里标---注意：是cit文件里的
        /// <summary>
        /// 获取cit文件中的所有公里标---注意：是cit文件里的
        /// </summary>
        /// <param name="citFilePath">cit文件名</param>
        /// <returns>cit文件中的里程--单位为公里</returns>
        public float[] GetMilesData(String citFilePath)
        {
            float[] retVal = null;

            DataHeadInfo m_dhi = GetDataInfoHeadNew(citFilePath);

            try
            {
                FileStream fs = new FileStream(citFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(fs, Encoding.Default);
                br.BaseStream.Position = 0;
                br.ReadBytes(120);
                br.ReadBytes(65 * m_dhi.iChannelNumber);
                br.ReadBytes(BitConverter.ToInt32(br.ReadBytes(4), 0));
                int iChannelNumberSize = m_dhi.iChannelNumber * 2;
                byte[] b = new byte[iChannelNumberSize];
                long iArray = (br.BaseStream.Length - br.BaseStream.Position) / iChannelNumberSize;
                retVal = new float[iArray];
                for (int i = 0; i < iArray; i++)
                {
                    b = br.ReadBytes(iChannelNumberSize);
                    if (m_dhi.sDataVersion.StartsWith("3."))
                    {
                        b = ByteXORByte(b);
                    }

                    short km = BitConverter.ToInt16(b, 0);

                    short m = BitConverter.ToInt16(b, 2);
                    float fGL = km + (float)m / m_dhi.iSmaleRate / 1000;//单位为公里

                    retVal[i] = fGL;
                }


                br.Close();
                fs.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return retVal;
        }
        #endregion

        #region 获取cit文件中的所有公里标---注意：是cit文件里的
        /// <summary>
        /// 获取cit文件中的所有公里标---注意：是cit文件里的
        /// </summary>
        /// <param name="citFilePath">cit文件名</param>
        /// <returns>cit文件中的里程--单位为公里</returns>
        public double[] GetMilesDataDouble(String citFilePath)
        {
            double[] retVal = null;

            DataHeadInfo m_dhi = GetDataInfoHeadNew(citFilePath);

            try
            {
                FileStream fs = new FileStream(citFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(fs, Encoding.Default);
                br.BaseStream.Position = 0;
                br.ReadBytes(120);
                br.ReadBytes(65 * m_dhi.iChannelNumber);
                br.ReadBytes(BitConverter.ToInt32(br.ReadBytes(4), 0));
                int iChannelNumberSize = m_dhi.iChannelNumber * 2;
                byte[] b = new byte[iChannelNumberSize];
                long iArray = (br.BaseStream.Length - br.BaseStream.Position) / iChannelNumberSize;
                retVal = new double[iArray];
                for (int i = 0; i < iArray; i++)
                {
                    b = br.ReadBytes(iChannelNumberSize);
                    if (m_dhi.sDataVersion.StartsWith("3."))
                    {
                        b = ByteXORByte(b);
                    }

                    short km = BitConverter.ToInt16(b, 0);

                    short m = BitConverter.ToInt16(b, 2);
                    float fGL = km + (float)m / m_dhi.iSmaleRate / 1000;//单位为公里

                    retVal[i] = fGL;
                }


                br.Close();
                fs.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return retVal;
        }
        #endregion

        #region 从里程序列中，判断增减里程----ygx--20140717
        /// <summary>
        /// 从里程序列中，判断增减里程----ygx--20140717
        /// </summary>
        /// <param name="kMeter">里程序列</param>
        /// <param name="isKmInc">true：增里程；false：减里程</param>
        /// <returns>true：函数执行成功；false：函数执行失败</returns>
        public Boolean IsCitKmInc(String citFileName,ref Boolean isKmInc)
        {
            float[] kMeter = GetMilesData(citFileName);

            int len=kMeter.Length;
            int sum=0;
            if (len < 10)
            {
                MessageBox.Show("点数太少");
                return false;
            }

            for (int i = 0; i < 10; i++)
            {
                if (kMeter[(i+0)*len/10] < kMeter[(i+1)*len/10 - 1])
                {
                    sum += 1;
                }
            }

            if (sum >= 5)
            {
                isKmInc = true;
            }
            else
            {
                isKmInc = false;
            }

            return true;
        }
        #endregion


        #region 获取cit文件中的所有速度值--20140105--ygx
        /// <summary>
        /// 获取cit文件中的所有速度值
        /// </summary>
        /// <param name="citFilePath">cit文件名</param>
        /// <returns>速度</returns>
        public float[] GetSpeedData(String citFilePath)
        {
            float[] retVal = null;
            int channelId=0;

            for (int i = 0; i < dciL.Count;i++ )
            {
                if (dciL[i].sNameEn.ToLower().Equals("speed"))
                {
                    channelId = i + 1;
                }
                
            }

            retVal = GetSingleChannelDataFloat(citFilePath, channelId);


            return retVal;
        }
        #endregion

        #region 根据索引值获取对应的文件指针。
        public double GetPosByIdx(string sSourceFile, double idx)
        {
            try
            {
                FileStream fs = new FileStream(sSourceFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(fs, Encoding.Default);
                br.BaseStream.Position = 0;
                br.ReadBytes(120);
                br.ReadBytes(65 * dhi.iChannelNumber);
                br.ReadBytes(BitConverter.ToInt32(br.ReadBytes(4), 0));

                double pos = br.BaseStream.Position + (idx - 1)*2*dhi.iChannelNumber;

                br.Close();
                fs.Close();

                return pos;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
        }
        #endregion

        #region 根据文件指针，获取里程信息---20140506--ygx
        /// <summary>
        /// 根据文件指针，获取里程信息---只获取文件中的原始里程
        /// </summary>
        /// <param name="sFile">idf文件名</param>
        /// <param name="sFileName">cit文件名</param>
        /// <param name="Pos">文件指针</param>
        /// <returns>里程：公里</returns>
        public double GetMileByPos(string sFile,string sFileName, double Pos)
        {
            List<DataChannelInfo> m_dciL = GetDataChannelInfoHeadNew(sFileName);
            DataHeadInfo m_dhi = GetDataInfoHeadNew(sFileName);

            try
            {
                double mile = -1;
                double lStartPoint = 0;
                double lStartMeter = 0;
                double lEndPoint = 0;
                double lEndMeter = 0;
                bool ifCorrect = false;
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFile + ";Persist Security Info=False"))
                {
                    OleDbCommand sqlcom = new OleDbCommand("select * from IndexOri", sqlconn);
                    sqlconn.Open();
                    OleDbDataReader sqloledr = sqlcom.ExecuteReader();
                    if (sqloledr.Read())
                    {
                        ifCorrect = true;
                    }
                    sqlconn.Close();
                }
                //考虑到无效数据显示的地方有更新里程功能，所以这里只需要取cit原始里程，所以把ifCorrect=false;
                ifCorrect = false;
                if (ifCorrect)
                {
                    List<IndexStaClass> listIC = new List<IndexStaClass>();
                    try
                    {
                        using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFile + ";Persist Security Info=False"))
                        {
                            OleDbCommand sqlcom = new OleDbCommand("select * from IndexSta order by clng(StartPoint)", sqlconn);
                            sqlconn.Open();
                            OleDbDataReader sqloledr = sqlcom.ExecuteReader();
                            while (sqloledr.Read())
                            {
                                IndexStaClass ic = new IndexStaClass();
                                ic.iID = (int)sqloledr.GetInt32(0);
                                ic.iIndexID = (int)sqloledr.GetInt32(1);
                                ic.lStartPoint = long.Parse(sqloledr.GetString(2));
                                ic.lStartMeter = sqloledr.GetString(3);
                                ic.lEndPoint = long.Parse(sqloledr.GetString(4));
                                ic.LEndMeter = sqloledr.GetString(5);
                                ic.lContainsPoint = long.Parse(sqloledr.GetString(6));
                                ic.lContainsMeter = sqloledr.GetString(7);
                                ic.sType = sqloledr.GetString(8);

                                listIC.Add(ic);
                            }
                            sqlconn.Close();
                        }
                    }
                    catch
                    {

                    }

                    if (Pos < listIC[0].lStartPoint)
                    {
                        FileStream fs = new FileStream(sFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        BinaryReader br = new BinaryReader(fs, Encoding.Default);
                        br.BaseStream.Position = Convert.ToInt64(Pos); ;
                        int iChannelNumberSize = m_dhi.iChannelNumber * 2;
                        byte[] b = new byte[iChannelNumberSize];

                        b = br.ReadBytes(iChannelNumberSize);
                        if (dhi.sDataVersion.StartsWith("3."))
                        {
                            b = ByteXORByte(b);
                        }
                        double fGL = (BitConverter.ToInt16(b, 0) / m_dciL[0].fScale) + m_dciL[0].fOffset;
                        if (fGL < 0)
                            fGL = 0;

                        //根据采样点数计算公里数
                        fGL += (((BitConverter.ToInt16(b, 2) / m_dciL[1].fScale + m_dciL[1].fOffset)) / 1000.0);
                        mile = fGL;
                        return mile;
                    }
                    if (Pos > listIC[listIC.Count - 1].lStartPoint)
                    {
                        lStartPoint = listIC[listIC.Count - 1].lStartPoint;
                        lStartMeter = double.Parse(listIC[listIC.Count - 1].lStartMeter);
                        lEndPoint = listIC[listIC.Count - 1].lEndPoint;
                        lEndMeter = double.Parse(listIC[listIC.Count - 1].LEndMeter);
                        mile = ((Pos - lStartPoint) / (lEndPoint - lStartPoint) * (lEndMeter - lStartMeter)) + lStartMeter;
                        return mile;
                    }

                    List<IndexStaClass>.Enumerator listCredentials = listIC.GetEnumerator();
                    while (listCredentials.MoveNext())
                    {
                        if (Pos >= listCredentials.Current.lStartPoint && Pos <= listCredentials.Current.lEndPoint)
                        {
                            lStartPoint = listCredentials.Current.lStartPoint;
                            lStartMeter = double.Parse(listCredentials.Current.lStartMeter);
                            lEndPoint = listCredentials.Current.lEndPoint;
                            lEndMeter = double.Parse(listCredentials.Current.LEndMeter);
                            double Absmile = ((Pos - lStartPoint) / (lEndPoint - lStartPoint) * Math.Abs(lEndMeter - lStartMeter));
                            if (lStartMeter < lEndMeter)
                            { 
                                mile = Absmile + lStartMeter; 
                            }
                            else
                            {
                                mile = Absmile + lEndMeter;
                            }
                            break;
                        }
                    }
                }
                else
                {
                    FileStream fs = new FileStream(sFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    BinaryReader br = new BinaryReader(fs, Encoding.Default);
                    br.BaseStream.Position = Convert.ToInt64(Pos); ;
                    int iChannelNumberSize = m_dhi.iChannelNumber * 2;
                    byte[] b = new byte[iChannelNumberSize];

                    b = br.ReadBytes(iChannelNumberSize);
                    if (dhi.sDataVersion.StartsWith("3."))
                    {
                        b = ByteXORByte(b);
                    }
                    double fGL = (BitConverter.ToInt16(b, 0) / m_dciL[0].fScale) + m_dciL[0].fOffset;
                    if (fGL < 0)
                        fGL = 0;

                    //根据采样点数计算公里数
                    fGL += (((BitConverter.ToInt16(b, 2) / m_dciL[1].fScale + m_dciL[1].fOffset)) / 1000.0);
                    mile = fGL;
                }

                return mile;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }
        }
        #endregion

        #region 根据通道英文名获取通道序号--从0开始----20140117--ygx
        /// <summary>
        /// 根据通道英文名获取通道序号--从0开始
        /// </summary>
        /// <param name="channelNameEn">通道英文名</param>
        /// <returns>通道序号</returns>
        public int GetChannelIndex(String channelNameEn)
        {
            int channelIndex = 0;

            for (int i = 0; i < dciL.Count;i++ )
            {
                if (dciL[i].sNameEn.ToLower().Equals(channelNameEn))
                {
                    channelIndex = i;
                    break;
                }
            }

            return channelIndex;
        }
        #endregion

        double ToDouble(string str)
        {
            string[] num = str.Split(new Char[] { 'K', 'k', '+', ' ', '、' });
            List<string> strs = new List<string>();
            foreach (string s in num)
            {
                if (!string.IsNullOrEmpty(s))
                    strs.Add(s);
            }
            num = strs.ToArray();
            double km = Convert.ToDouble(num.GetValue(0).ToString());
            double m = Convert.ToDouble(num.GetValue(1).ToString());

            return (km * 1000 + m);
        }

        #region 精确数据导出

        #region 精确数据导出数据操作---处理本地cit文件
        /// <summary>
        /// 精确数据导出数据操作---处理本地cit文件
        /// </summary>
        /// <param name="sSourceFile">原始cit文件(全路径)</param>
        /// <param name="sDestinationFile">目标导出txt文件(全路径)</param>
        /// <param name="iStartKM">起始公里标</param>
        /// <param name="iEndKM">结束公里标</param>
        /// <param name="sParameter">导出参数</param>
        /// <param name="listAC"></param>
        /// <param name="bEncrypt">通道数据是否加密</param>
        /// <returns>函数执行结果：0-成功；1-失败</returns>
        public int ExportData(string sSourceFile, string sDestinationFile, int iStartKM, int iEndKM, string[] sParameter, List<AreaClass> listAC, bool bEncrypt)
        {
            try
            {
                string[] sCNs = sParameter[0].Split(new char[] { ',' });//通道次序

                //导出数据在cit文件中的起始位置和结束位置
                long[] bRet = GetExportDataInfoPosition(sSourceFile, iStartKM, iEndKM);

                FileStream fs = new FileStream(sSourceFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(fs, Encoding.Default);

                //设置读取的起始位置
                br.BaseStream.Position = bRet[0];
                int iChannelNumberSize = dhi.iChannelNumber * 2;
                byte[] b = new byte[iChannelNumberSize];
                StreamWriter sw = new StreamWriter(sDestinationFile, false, Encoding.Default);

                sw.WriteLine("Meter," + sParameter[1]);

                while (br.BaseStream.Position < bRet[1])
                {                      
                    b = br.ReadBytes(iChannelNumberSize);
                    if (bEncrypt)
                    { b = ByteXORByte(b); }
                    short km = BitConverter.ToInt16(b, 0);
                    if (km < 0 || km > 9999)
                    {
                        continue;
                    }
                    short m = BitConverter.ToInt16(b, 2);
                    StringBuilder sb = new StringBuilder();
                    double dMeter = km  + (float)m / 4/1000 ;//单位为公里
                    //double dMeter = km + (float)m / dhi.iSmaleRate / 1000;//单位为公里

                    if (km == 40 && m == 863)
                    {
                        int tt = 0;
                    }


                    //疑问--？---listAC干嘛用的
                    bool bContinue = false;
                    if (listAC != null || listAC.Count > 0)
                    {
                        foreach (AreaClass ac in listAC)
                        {
                            if (dMeter >= ac.dStartMeter && dMeter <= ac.dEndMeter)
                            {

                                bContinue = true;
                                break;
                            }
                        }
                    }
                    if (bContinue)
                    {
                        continue;
                    }

                    //写入公里标：km+m
                    sb.Append(dMeter.ToString());
                    sb.Append(",");
                    //if (dMeter < 0)
                    //{
                    //    sb.Remove(sb.Length - 1, 1);
                    //    sw.WriteLine(sb);

                    //    continue;
                    //}

                    //写入通道数据
                    for (int i = 0; i < sCNs.Length; i++)
                    {
                        float fOffset = true ? dciL[int.Parse(sCNs[i])].fOffset : 0;
                        float fM = BitConverter.ToInt16(b, int.Parse(sCNs[i]) * 2) / dciL[int.Parse(sCNs[i])].fScale + fOffset;
                        sb.Append(fM.ToString());
                        sb.Append(",");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sw.WriteLine(sb);

                }


                sw.Close();
                br.Close();
                fs.Close();


                return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 1;
            }
        }
        #endregion

        #region 导出数据操作---网络版使用
        public int ExportData(string orapath, ref byte[] bResult, long StartPos, long EndPos)
        {
            try
            {

                FileStream fs = new FileStream(orapath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(fs);
                byte[] b = new byte[dhi.iChannelNumber * 2];
                long p = 0;
                short gongli;
                short mi1;
                long s1;
                long a1, a2;
                bool bEn = false;
                //读取文件头
                byte[] head = br.ReadBytes(120);
                if (head[5] == 51)
                {
                    bEn = true;
                }
                byte[] tdval = br.ReadBytes(65 * dhi.iChannelNumber);
                byte[] last4 = br.ReadBytes(4);
                byte[] zh = br.ReadBytes(BitConverter.ToInt32(last4, 0));
                p = br.BaseStream.Position;
                br.BaseStream.Seek(p, SeekOrigin.Begin);
                //取得开始位置
                int x1 = 0;
                int x2 = 2;
                try
                {
                    while (true)
                    {
                        a1 = br.BaseStream.Position;
                        b = br.ReadBytes(dhi.iChannelNumber * 2);
                        if (bEn)
                        {
                            b = WaveformDataProcess.ByteXORByte(b);
                        }
                        gongli = BitConverter.ToInt16(b, x1);
                        mi1 = BitConverter.ToInt16(b, x2);
                        s1 = gongli * 1000 + mi1 / dhi.iSmaleRate;
                        if (s1 == StartPos)
                        {
                            break;
                        }
                    }
                }
                catch// (ArgumentOutOfRangeException ex)
                {
                    a1 = p;
                    return 1;

                }
                br.BaseStream.Position = p;
                //取得结束位置
                try
                {
                    while (true)
                    {
                        a2 = br.BaseStream.Position;
                        b = br.ReadBytes(dhi.iChannelNumber * 2);
                        if (bEn)
                        {
                            b = WaveformDataProcess.ByteXORByte(b);
                        }
                        gongli = BitConverter.ToInt16(b, x1);
                        mi1 = BitConverter.ToInt16(b, x2);
                        s1 = gongli * 1000 + mi1 / dhi.iSmaleRate;
                        if (s1 == EndPos)
                        {
                            break;
                        }
                    }
                }
                catch
                {
                    a2 = br.BaseStream.Length - 1;
                    return 1;
                }
                br.BaseStream.Seek(a1, SeekOrigin.Begin);
                MemoryStream ms = new MemoryStream();
                ms.Write(head, 0, head.Length);
                ms.Write(tdval, 0, tdval.Length);
                ms.Write(last4, 0, last4.Length);
                ms.Write(zh, 0, zh.Length);
                byte[] reb = new byte[dhi.iChannelNumber * 2];
                while (br.BaseStream.Position <= a2)
                {
                    int c1 = br.Read(reb, 0, reb.Length);

                    ms.Write(reb, 0, c1);
                }
                bResult = ms.ToArray();
                ms.Close();
                fs.Close();
                return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 1;
            }

        }
        #endregion

        #region 导出数据操作---没有使用
        public int ExportData(string orapath, string savePath, long StartPos, long EndPos, List<AreaClass> listAC)
        {
            try
            {
                FileStream fs = new FileStream(orapath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(fs);
                byte[] b = new byte[dhi.iChannelNumber * 2];
                long p = 0;
                short gongli;
                short mi1;
                long s1;
                long a1, a2;
                //读取文件头
                byte[] head = br.ReadBytes(120);
                byte[] tdval = br.ReadBytes(65 * dhi.iChannelNumber);
                byte[] last4 = br.ReadBytes(4);
                byte[] zh = br.ReadBytes(BitConverter.ToInt32(last4, 0));
                p = br.BaseStream.Position;
                br.BaseStream.Seek(p, SeekOrigin.Begin);
                //取得开始位置
                int x1 = 0;
                int x2 = 2;
                try
                {
                    while (true)
                    {
                        a1 = br.BaseStream.Position;
                        b = br.ReadBytes(dhi.iChannelNumber * 2);
                        gongli = BitConverter.ToInt16(b, x1);
                        mi1 = BitConverter.ToInt16(b, x2);
                        s1 = gongli * 1000 + mi1 / dhi.iSmaleRate;
                        if (s1 == StartPos)
                        {
                            break;
                        }
                    }
                }
                catch// (ArgumentOutOfRangeException ex)
                {
                    a1 = p;

                }
                br.BaseStream.Position = p;
                //取得结束位置
                try
                {
                    while (true)
                    {
                        a2 = br.BaseStream.Position;
                        b = br.ReadBytes(dhi.iChannelNumber * 2);
                        gongli = BitConverter.ToInt16(b, x1);
                        mi1 = BitConverter.ToInt16(b, x2);
                        s1 = gongli * 1000 + mi1 / dhi.iSmaleRate;
                        if (s1 == EndPos)
                        {
                            break;
                        }
                    }
                }
                catch
                {
                    a2 = br.BaseStream.Length - 1;
                }
                br.BaseStream.Seek(a1, SeekOrigin.Begin);
                FileStream fs1 = new FileStream(savePath, FileMode.Create, FileAccess.Write, FileShare.Read);
                fs1.Write(head, 0, head.Length);
                fs1.Write(tdval, 0, tdval.Length);
                fs1.Write(last4, 0, last4.Length);
                fs1.Write(zh, 0, zh.Length);
                byte[] reb = new byte[dhi.iChannelNumber * 2];
                while (br.BaseStream.Position <= a2)
                {
                    int c1 = br.Read(reb, 0, reb.Length);
                    //

                    short km = BitConverter.ToInt16(reb, 0);
                    short m = BitConverter.ToInt16(reb, 2);
                    double dMeter = km + m / dciL[1].fScale / 1000.0;

                    bool bContinue = false;
                    if (listAC != null || listAC.Count > 0)
                    {
                        foreach (AreaClass ac in listAC)
                        {
                            if (dMeter >= ac.dStartMeter && dMeter <= ac.dEndMeter)
                            {
                                bContinue = true;
                                break;
                            }
                        }
                    }
                    if (bContinue)
                    {
                        continue;
                    }

                    //

                    fs1.Write(reb, 0, c1);
                }
                fs1.Flush();
                fs1.Close();
                fs.Close();
                return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 1;
            }

        }
        #endregion

        #endregion

        #region 数据连续性判断----没有使用
        public bool DataMileageRangeSequenceVerify(string sFile)
        {
            using (FileStream fs = new FileStream(sFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (BinaryReader br = new BinaryReader(fs, Encoding.Default))
                {
                    br.BaseStream.Position = 0;
                    br.ReadBytes(120);
                    br.ReadBytes(65 * dhi.iChannelNumber);
                    br.ReadBytes(BitConverter.ToInt32(br.ReadBytes(4), 0));






                    br.Close();
                    fs.Close();
                }
            }
            return true;
        }
        #endregion

        #region 修改文件--波形软件网络版使用
        public bool ModifyCITHeader(string sFile, string sLineCode, string sLineName, List<DataProcess.GEO2CITBind> listGEO2CIT)
        {
            try
            {
                #region 修改文件头
                using (FileStream fs = new FileStream(sFile, FileMode.Open, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs, Encoding.Default))
                    {
                        bw.BaseStream.Position = 4;
                        string sVer = "1.1.0";
                        bw.Write(sVer);
                        if (sVer.Length < 20)
                        {
                            for (int i = 0; i < (20 - sVer.Length); i++)
                            {
                                bw.Write((byte)0);
                            }
                        }

                        bw.Write(sLineCode);

                        if (sLineCode.Length < 4)
                        {
                            for (int i = 0; i < (4 - sLineCode.Length); i++)
                            {
                                bw.Write((byte)0);
                            }
                        }

                        bw.Write(sLineName);
                        if (Encoding.Default.GetBytes(sLineName).Length < 20)
                        {
                            for (int i = 0; i < (20 - Encoding.Default.GetBytes(sLineName).Length); i++)
                            {
                                bw.Write((byte)0);
                            }
                        }
                        bw.Close();

                    }
                    fs.Close();
                }
                #endregion
                Application.DoEvents();
                #region 修改通道名称
                List<int> listIndex = new List<int>();
                GetDataInfoHead(sFile);
                List<DataChannelInfo> listDCI = GetDataChannelInfoHead(sFile);
                for (int i = 0; i < listGEO2CIT.Count; i++)
                {
                    for (int j = 0; j < listDCI.Count; j++)
                    {
                        if (listGEO2CIT[i].sGEO.Equals(listDCI[j].sNameEn))
                        {
                            listIndex.Add(j);
                            break;
                        }
                    }
                }

                using (FileStream fs = new FileStream(sFile, FileMode.Open, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs, Encoding.Default))
                    {
                        for (int k = 0; k < listIndex.Count; k++)
                        {
                            bw.BaseStream.Position = 120 + 65 * listIndex[k] + 4;
                            for (int i = 0; i < 42; i++)
                            {
                                bw.Write((byte)0);
                            }
                            bw.BaseStream.Position = 120 + 65 * listIndex[k] + 4;
                            bw.Write(listGEO2CIT[k].sCIT);
                            if (listGEO2CIT[k].sCIT.Length < 20)
                            {
                                for (int i = 0; i < (20 - listGEO2CIT[k].sCIT.Length); i++)
                                {
                                    bw.Write((byte)0);
                                }
                            }
                            bw.Write(listGEO2CIT[k].sChinese);
                            if (Encoding.Default.GetBytes(listGEO2CIT[k].sChinese).Length < 20)
                            {
                                for (int i = 0; i < (20 - Encoding.Default.GetBytes(listGEO2CIT[k].sChinese).Length); i++)
                                {
                                    bw.Write((byte)0);
                                }
                            }
                        }
                        bw.Close();
                    }
                    fs.Close();
                }
                #endregion
                Application.DoEvents();
                #region 滤波
                CalcSpace.CalcProcess calcPro = new CalcSpace.CalcProcess();
                calcPro.ProcessCalculateCitFilter(sFile);
                #endregion
                Application.DoEvents();
                #region 补零加密
                FileStream fsRead = new FileStream(sFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                FileStream fsWrite = new FileStream(sFile + ".bak", FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(fsRead, Encoding.UTF8);
                BinaryWriter bw1 = new BinaryWriter(fsWrite, Encoding.UTF8);
                byte[] bHead = br.ReadBytes(120);
                int iKmInc = BitConverter.ToInt32(bHead, 100);
                int iChannelNumber = BitConverter.ToInt32(bHead, 116);

                byte[] bChannels = br.ReadBytes(65 * iChannelNumber);
                byte[] bData = new byte[iChannelNumber * 2];
                byte[] bDataNew = new byte[iChannelNumber * 2];
                byte[] bTail = br.ReadBytes(BitConverter.ToInt32(br.ReadBytes(4), 0));

                bw1.Write(bHead);
                bw1.BaseStream.Position = 4;
                string sVersion = "3.1.0";
                bw1.Write(sVersion);
                if (sVersion.Length < 20)
                {
                    for (int i = 0; i < (20 - sVersion.Length); i++)
                    {
                        bw1.Write((byte)0);
                    }
                }
                bw1.BaseStream.Position = 120;
                bw1.Write(bChannels);
                bw1.Write(bTail.Length);
                bw1.Write(bTail);
                WaveformDataProcess wdp = new WaveformDataProcess();
                List<LostDataClass> ldc = wdp.GetLostData(sFile, "", false,false);
                while (br.BaseStream.Position < br.BaseStream.Length)
                {
                    long lPosition = br.BaseStream.Position;

                    bData = br.ReadBytes(iChannelNumber * 2);
                    bData = WaveformDataProcess.ByteXORByte(bData);

                    bw1.Write(bData);

                    for (int i = 0; i < ldc.Count; i++)
                    {
                        if (ldc[i].lStartPos == lPosition)
                        {
                            for (int j = 0; j < ldc[i].iCount; j++)
                            {
                                byte[] b = new byte[iChannelNumber * 2];
                                b = WaveformDataProcess.ByteXORByte(b);
                                bw1.Write(b);
                            }
                            break;
                        }
                    }
                }
                bw1.Close();
                br.Close();
                fsWrite.Close();
                fsRead.Close();
                //删除bak
                Application.DoEvents();
                File.Delete(sFile);
                Application.DoEvents();
                File.Move(sFile + ".bak", sFile);
                Application.DoEvents();
                #endregion

                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 修改文件头--从0号车高频滤波中剥离出来--ygx--20150123
        public void ModifyCitHeader_New(string sFile, string sLineCode, string sLineName, List<DataProcess.GEO2CITBind> listGEO2CIT)
        {
            #region 修改文件头
            using (FileStream fs = new FileStream(sFile, FileMode.Open, FileAccess.Write, FileShare.ReadWrite))
            {
                using (BinaryWriter bw = new BinaryWriter(fs, Encoding.Default))
                {
                    bw.BaseStream.Position = 4;
                    string sVer = "1.1.0";
                    bw.Write(sVer);
                    if (sVer.Length < 20)
                    {
                        for (int i = 0; i < (20 - sVer.Length); i++)
                        {
                            bw.Write((byte)0);
                        }
                    }

                    bw.Write(sLineCode);

                    if (sLineCode.Length < 4)
                    {
                        for (int i = 0; i < (4 - sLineCode.Length); i++)
                        {
                            bw.Write((byte)0);
                        }
                    }

                    bw.Write(sLineName);
                    if (Encoding.Default.GetBytes(sLineName).Length < 20)
                    {
                        for (int i = 0; i < (20 - Encoding.Default.GetBytes(sLineName).Length); i++)
                        {
                            bw.Write((byte)0);
                        }
                    }
                    bw.Close();

                }
                fs.Close();
            }
            #endregion
            Application.DoEvents();
            #region 修改通道名称
            List<int> listIndex = new List<int>();
            GetDataInfoHead(sFile);
            List<DataChannelInfo> listDCI = GetDataChannelInfoHead(sFile);
            for (int i = 0; i < listGEO2CIT.Count; i++)
            {
                for (int j = 0; j < listDCI.Count; j++)
                {
                    if (listGEO2CIT[i].sGEO.Equals(listDCI[j].sNameEn))
                    {
                        listIndex.Add(j);
                        break;
                    }
                }
            }

            using (FileStream fs = new FileStream(sFile, FileMode.Open, FileAccess.Write, FileShare.ReadWrite))
            {
                using (BinaryWriter bw = new BinaryWriter(fs, Encoding.Default))
                {
                    for (int k = 0; k < listIndex.Count; k++)
                    {
                        bw.BaseStream.Position = 120 + 65 * listIndex[k] + 4;
                        for (int i = 0; i < 42; i++)
                        {
                            bw.Write((byte)0);
                        }
                        bw.BaseStream.Position = 120 + 65 * listIndex[k] + 4;
                        bw.Write(listGEO2CIT[k].sCIT);
                        if (listGEO2CIT[k].sCIT.Length < 20)
                        {
                            for (int i = 0; i < (20 - listGEO2CIT[k].sCIT.Length); i++)
                            {
                                bw.Write((byte)0);
                            }
                        }
                        bw.Write(listGEO2CIT[k].sChinese);
                        if (Encoding.Default.GetBytes(listGEO2CIT[k].sChinese).Length < 20)
                        {
                            for (int i = 0; i < (20 - Encoding.Default.GetBytes(listGEO2CIT[k].sChinese).Length); i++)
                            {
                                bw.Write((byte)0);
                            }
                        }
                    }
                    bw.Close();
                }
                fs.Close();
            }
            #endregion
            Application.DoEvents();
        }
        #endregion

        #region 高频滤波--从0号车高频滤波剥离出--ygx--20150123
        public void CitFilter(String sFile)
        {
            #region 滤波
            CalcSpace.CalcProcess calcPro = new CalcSpace.CalcProcess();
            calcPro.ProcessCalculateCitFilter(sFile);
            #endregion
            Application.DoEvents();
            
        }
        #endregion

        #region 补零加密--从0号车高频滤波中剥离出来--ygx--20150123
        public void CitEncrypt(String sFile)
        {
            CITDataProcess citdp = new CITDataProcess();
            CITDataProcess.DataHeadInfo m_dhi = citdp.GetDataInfoHeadNew(sFile);
            if (m_dhi.sDataVersion.StartsWith("3"))
            {
                return;
            }

            #region 补零加密
            FileStream fsRead = new FileStream(sFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            FileStream fsWrite = new FileStream(sFile + ".bak", FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            BinaryReader br = new BinaryReader(fsRead, Encoding.UTF8);
            BinaryWriter bw1 = new BinaryWriter(fsWrite, Encoding.UTF8);
            byte[] bHead = br.ReadBytes(120);
            int iKmInc = BitConverter.ToInt32(bHead, 100);
            int iChannelNumber = BitConverter.ToInt32(bHead, 116);

            byte[] bChannels = br.ReadBytes(65 * iChannelNumber);
            byte[] bData = new byte[iChannelNumber * 2];
            byte[] bDataNew = new byte[iChannelNumber * 2];
            byte[] bTail = br.ReadBytes(BitConverter.ToInt32(br.ReadBytes(4), 0));

            bw1.Write(bHead);
            bw1.BaseStream.Position = 4;
            string sVersion = "3.1.0";
            bw1.Write(sVersion);
            if (sVersion.Length < 20)
            {
                for (int i = 0; i < (20 - sVersion.Length); i++)
                {
                    bw1.Write((byte)0);
                }
            }
            bw1.BaseStream.Position = 120;
            bw1.Write(bChannels);
            bw1.Write(bTail.Length);
            bw1.Write(bTail);
            WaveformDataProcess wdp = new WaveformDataProcess();
            List<LostDataClass> ldc = wdp.GetLostData(sFile, "", false, false);
            while (br.BaseStream.Position < br.BaseStream.Length)
            {
                long lPosition = br.BaseStream.Position;

                bData = br.ReadBytes(iChannelNumber * 2);
                bData = WaveformDataProcess.ByteXORByte(bData);

                bw1.Write(bData);

                for (int i = 0; i < ldc.Count; i++)
                {
                    if (ldc[i].lStartPos == lPosition)
                    {
                        for (int j = 0; j < ldc[i].iCount; j++)
                        {
                            byte[] b = new byte[iChannelNumber * 2];
                            b = WaveformDataProcess.ByteXORByte(b);
                            bw1.Write(b);
                        }
                        break;
                    }
                }
            }
            bw1.Close();
            br.Close();
            fsWrite.Close();
            fsRead.Close();
            //删除bak
            Application.DoEvents();
            File.Delete(sFile);
            Application.DoEvents();
            File.Move(sFile + ".bak", sFile);
            Application.DoEvents();
            #endregion
        }
        #endregion

        #region 修改通道名--150C的加速度文件通道名由中文改为英文--严广学--20140513
        public bool ModifyCITHeader_150C(string sFile, string sLineCode, string sLineName, List<DataProcess.GEO2CITBind> listGEO2CIT)
        {
            try
            {
                #region 修改文件头
                using (FileStream fs = new FileStream(sFile, FileMode.Open, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs, Encoding.Default))
                    {
                        bw.BaseStream.Position = 4;
                        string sVer = "1.1.0";
                        bw.Write(sVer);
                        if (sVer.Length < 20)
                        {
                            for (int i = 0; i < (20 - sVer.Length); i++)
                            {
                                bw.Write((byte)0);
                            }
                        }

                        bw.Write(sLineCode);

                        if (sLineCode.Length < 4)
                        {
                            for (int i = 0; i < (4 - sLineCode.Length); i++)
                            {
                                bw.Write((byte)0);
                            }
                        }

                        bw.Write(sLineName);
                        if (Encoding.Default.GetBytes(sLineName).Length < 20)
                        {
                            for (int i = 0; i < (20 - Encoding.Default.GetBytes(sLineName).Length); i++)
                            {
                                bw.Write((byte)0);
                            }
                        }
                        bw.Close();

                    }
                    fs.Close();
                }
                #endregion
                Application.DoEvents();
                #region 修改通道名称
                List<int> listIndex = new List<int>();
                GetDataInfoHead(sFile);
                List<DataChannelInfo> listDCI = GetDataChannelInfoHead(sFile);
                for (int i = 0; i < listGEO2CIT.Count; i++)
                {
                    for (int j = 0; j < listDCI.Count; j++)
                    {
                        if (listGEO2CIT[i].sGEO.Equals(listDCI[j].sNameEn))
                        {
                            listIndex.Add(j);
                            break;
                        }
                    }
                }

                using (FileStream fs = new FileStream(sFile, FileMode.Open, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs, Encoding.Default))
                    {
                        for (int k = 0; k < listIndex.Count; k++)
                        {
                            bw.BaseStream.Position = 120 + 65 * listIndex[k] + 4;
                            for (int i = 0; i < 42; i++)
                            {
                                bw.Write((byte)0);
                            }
                            bw.BaseStream.Position = 120 + 65 * listIndex[k] + 4;
                            bw.Write(listGEO2CIT[k].sCIT);
                            if (listGEO2CIT[k].sCIT.Length < 20)
                            {
                                for (int i = 0; i < (20 - listGEO2CIT[k].sCIT.Length); i++)
                                {
                                    bw.Write((byte)0);
                                }
                            }
                            bw.Write(listGEO2CIT[k].sChinese);
                            if (Encoding.Default.GetBytes(listGEO2CIT[k].sChinese).Length < 20)
                            {
                                for (int i = 0; i < (20 - Encoding.Default.GetBytes(listGEO2CIT[k].sChinese).Length); i++)
                                {
                                    bw.Write((byte)0);
                                }
                            }
                        }
                        bw.Close();
                    }
                    fs.Close();
                }
                #endregion
                Application.DoEvents();
                

                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 接口函数----没有使用---2个
        public bool ModifyCITHeader(string sFile, List<DataProcess.GEO2CITBind> listGEO2CIT
            , CITDataProcess.DataHeadInfo structCITHead)
        {
            try
            {
                #region 修改文件头
                using (FileStream fs = new FileStream(sFile, FileMode.Open, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs, Encoding.Default))
                    {
                        bw.BaseStream.Position = 4;
                        bw.Write(structCITHead.sDataVersion);
                        if (structCITHead.sDataVersion.Length < 20)
                        {
                            for (int i = 0; i < (20 - structCITHead.sDataVersion.Length); i++)
                            {
                                bw.Write((byte)0);
                            }
                        }

                        bw.Write(structCITHead.sTrackCode);

                        if (structCITHead.sTrackCode.Length < 4)
                        {
                            for (int i = 0; i < (4 - structCITHead.sTrackCode.Length); i++)
                            {
                                bw.Write((byte)0);
                            }
                        }

                        bw.Write(structCITHead.sTrackName);
                        if (Encoding.Default.GetBytes(structCITHead.sTrackName).Length < 20)
                        {
                            for (int i = 0; i < (20 - Encoding.Default.GetBytes(structCITHead.sTrackName).Length); i++)
                            {
                                bw.Write((byte)0);
                            }
                        }
                        bw.Write(structCITHead.iDir);

                        bw.Write(structCITHead.sTrain);
                        for (int i = 0; i < 20 - structCITHead.sTrain.Length; i++)
                        {
                            bw.Write((byte)0);
                        }

                        bw.Write(structCITHead.sDate);
                        bw.Write(structCITHead.sTime);
                        bw.Write(structCITHead.iRunDir);

                        bw.Write(structCITHead.iKmInc);

                        bw.Write(structCITHead.fkmFrom);
                        bw.Write(structCITHead.fkmTo);

                        bw.Close();

                    }
                    fs.Close();
                }
                #endregion
                Application.DoEvents();
                #region 修改通道名称
                List<int> listIndex = new List<int>();
                GetDataInfoHead(sFile);
                List<DataChannelInfo> listDCI = GetDataChannelInfoHead(sFile);
                for (int i = 0; i < listGEO2CIT.Count; i++)
                {
                    for (int j = 0; j < listDCI.Count; j++)
                    {
                        if (listGEO2CIT[i].sGEO.Equals(listDCI[j].sNameEn))
                        {
                            listIndex.Add(j);
                            break;
                        }
                    }
                }

                using (FileStream fs = new FileStream(sFile, FileMode.Open, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs, Encoding.Default))
                    {
                        for (int k = 0; k < listIndex.Count; k++)
                        {
                            bw.BaseStream.Position = 120 + 65 * listIndex[k] + 4;
                            for (int i = 0; i < 42; i++)
                            {
                                bw.Write((byte)0);
                            }
                            bw.BaseStream.Position = 120 + 65 * listIndex[k] + 4;
                            bw.Write(listGEO2CIT[k].sCIT);
                            if (listGEO2CIT[k].sCIT.Length < 20)
                            {
                                for (int i = 0; i < (20 - listGEO2CIT[k].sCIT.Length); i++)
                                {
                                    bw.Write((byte)0);
                                }
                            }
                            bw.Write(listGEO2CIT[k].sChinese);
                            if (Encoding.Default.GetBytes(listGEO2CIT[k].sChinese).Length < 20)
                            {
                                for (int i = 0; i < (20 - Encoding.Default.GetBytes(listGEO2CIT[k].sChinese).Length); i++)
                                {
                                    bw.Write((byte)0);
                                }
                            }
                        }
                        bw.Close();
                    }
                    fs.Close();
                }
                #endregion
                Application.DoEvents();
                #region 滤波
                CalcSpace.CalcProcess calcPro = new CalcSpace.CalcProcess();
                calcPro.ProcessCalculateCitFilter(sFile);
                #endregion
                Application.DoEvents();
                #region 补零加密
                FileStream fsRead = new FileStream(sFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                FileStream fsWrite = new FileStream(sFile + ".bak", FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(fsRead, Encoding.UTF8);
                BinaryWriter bw1 = new BinaryWriter(fsWrite, Encoding.UTF8);
                byte[] bHead = br.ReadBytes(120);
                int iKmInc = BitConverter.ToInt32(bHead, 100);
                int iChannelNumber = BitConverter.ToInt32(bHead, 116);

                byte[] bChannels = br.ReadBytes(65 * iChannelNumber);
                byte[] bData = new byte[iChannelNumber * 2];
                byte[] bDataNew = new byte[iChannelNumber * 2];
                byte[] bTail = br.ReadBytes(BitConverter.ToInt32(br.ReadBytes(4), 0));

                bw1.Write(bHead);
                bw1.BaseStream.Position = 4;
                string sVersion = "3.1.0";
                bw1.Write(sVersion);
                if (sVersion.Length < 20)
                {
                    for (int i = 0; i < (20 - sVersion.Length); i++)
                    {
                        bw1.Write((byte)0);
                    }
                }
                bw1.BaseStream.Position = 120;
                bw1.Write(bChannels);
                bw1.Write(bTail.Length);
                bw1.Write(bTail);
                WaveformDataProcess wdp = new WaveformDataProcess();
                List<LostDataClass> ldc = wdp.GetLostData(sFile, "", false, false);
                while (br.BaseStream.Position < br.BaseStream.Length)
                {
                    long lPosition = br.BaseStream.Position;

                    bData = br.ReadBytes(iChannelNumber * 2);
                    bData = WaveformDataProcess.ByteXORByte(bData);

                    bw1.Write(bData);

                    for (int i = 0; i < ldc.Count; i++)
                    {
                        if (ldc[i].lStartPos == lPosition)
                        {
                            for (int j = 0; j < ldc[i].iCount; j++)
                            {
                                byte[] b = new byte[iChannelNumber * 2];
                                b = WaveformDataProcess.ByteXORByte(b);
                                bw1.Write(b);
                            }
                            break;
                        }
                    }
                }
                bw1.Close();
                br.Close();
                fsWrite.Close();
                fsRead.Close();
                //删除bak
                Application.DoEvents();
                File.Delete(sFile);
                Application.DoEvents();
                File.Move(sFile + ".bak", sFile);
                Application.DoEvents();
                #endregion

                return true;
            }
            catch
            {
                return false;
            }
        }
        //
        public bool ModifyCITHeader(string sFile , CITDataProcess.DataHeadInfo structCITHead)
        {
            try
            {
                #region 修改文件头
                using (FileStream fs = new FileStream(sFile, FileMode.Open, FileAccess.Write, FileShare.ReadWrite))
                {
                    using (BinaryWriter bw = new BinaryWriter(fs, Encoding.Default))
                    {
                        bw.BaseStream.Position = 25;
                        bw.Write(structCITHead.sTrackCode);

                        if (structCITHead.sTrackCode.Length < 4)
                        {
                            for (int i = 0; i < (4 - structCITHead.sTrackCode.Length); i++)
                            {
                                bw.Write((byte)0);
                            }
                        }

                        bw.Write(structCITHead.sTrackName);
                        if (Encoding.Default.GetBytes(structCITHead.sTrackName).Length < 20)
                        {
                            for (int i = 0; i < (20 - Encoding.Default.GetBytes(structCITHead.sTrackName).Length); i++)
                            {
                                bw.Write((byte)0);
                            }
                        }
                        bw.Write(structCITHead.iDir);

                        bw.Write(structCITHead.sTrain);
                        for (int i = 0; i < 20 - structCITHead.sTrain.Length; i++)
                        {
                            bw.Write((byte)0);
                        }

                        bw.Write(structCITHead.sDate);
                        bw.Write(structCITHead.sTime);
                        bw.Write(structCITHead.iRunDir);

                        bw.Write(structCITHead.iKmInc);

                        bw.Write(structCITHead.fkmFrom);
                        bw.Write(structCITHead.fkmTo);

                        bw.Close();

                    }
                    fs.Close();
                }
                #endregion
                Application.DoEvents();

                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region 把单行线都统一为增里程(包括正方向和反方向)----ygx--20140717
        /// <summary>
        /// 把单行线都统一为增里程(包括正方向和反方向)----ygx--20140717
        /// </summary>
        /// <param name="citFileName">cit文件名</param>
        /// <returns></returns>
        public void ModifyCitMergeKmInc(String citFileName)
        {
            DataHeadInfo m_dhi = GetDataInfoHeadNew(citFileName);

            if (m_dhi.iDir != 3)
            {
                MessageBox.Show("非单行线，没有必要统一为增里程");
                return ;
            }

            Boolean isKmInc = true;
            Boolean bVal = IsCitKmInc(citFileName, ref isKmInc);

            if (bVal == false)
            {
                return ;
            }

            //文件头中指示为增里程，且文件中确实为增里程，则不需要处理，直接返回。
            if (m_dhi.iKmInc == 0 && isKmInc == true)
            {
                return;
            }

            //以下情况：有可能是文件头指示错误或是实际文件确实为减里程
            //统一为增里程
            if (m_dhi.iKmInc != 0)
            {
                m_dhi.iKmInc = 0;
            }


            try
            {                
                #region 存取文件
                FileStream fsRead = new FileStream(citFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                FileStream fsWrite = new FileStream(citFileName + ".bak", FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(fsRead, Encoding.UTF8);
                BinaryWriter bw1 = new BinaryWriter(fsWrite, Encoding.UTF8);
                byte[] bHead = br.ReadBytes(120);
                byte[] bChannels = br.ReadBytes(65 * m_dhi.iChannelNumber);
                byte[] bData = new byte[m_dhi.iChannelNumber * 2];
                byte[] bDataNew = new byte[m_dhi.iChannelNumber * 2];
                byte[] bTail = br.ReadBytes(BitConverter.ToInt32(br.ReadBytes(4), 0));
                
                //bw1.Write(bHead);

                bw1.Write(GetBytesFromDataHeadInfo(m_dhi));//文件头
                DataHeadInfo mmdhi = GetDataInfoHead(GetBytesFromDataHeadInfo(m_dhi));


                bw1.Write(bChannels);
                bw1.Write(bTail.Length);
                bw1.Write(bTail);

                long startPos = br.BaseStream.Position;//记录数据开始位置的文件指针

                //增里程时，不反转
                if (isKmInc == true)
                {
                    while (br.BaseStream.Position < br.BaseStream.Length)
                    {
                        bw1.Write(br.ReadBytes(m_dhi.iChannelNumber * 2));
                        //br.BaseStream.Position += m_dhi.iChannelNumber * 2;
                    }
                } 
                else
                {
                    br.BaseStream.Position = br.BaseStream.Length - m_dhi.iChannelNumber * 2;

                    while (br.BaseStream.Position >= startPos)
                    {
                        bw1.Write(br.ReadBytes(m_dhi.iChannelNumber * 2));
                        br.BaseStream.Position -= m_dhi.iChannelNumber * 2 * 2;
                    }
                }

                //
                bw1.Close();
                br.Close();
                fsWrite.Close();
                fsRead.Close();
                //删除bak
                Application.DoEvents();
                File.Delete(citFileName);
                Application.DoEvents();
                File.Move(citFileName + ".bak", citFileName);
                Application.DoEvents();
                #endregion
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            return ;
        }
        #endregion

        #region 把反方向检测转换为正方向检测----ygx----20140717
        /// <summary>
        /// 把反方向检测转换为正方向检测----ygx----20140717
        /// </summary>
        /// <param name="citFileName">citFileName</param>
        /// <returns></returns>
        public void ModifyCitReverseToForward(String citFileName)
        {
            DataHeadInfo m_dhi = GetDataInfoHeadNew(citFileName);
            List<DataChannelInfo> m_dciL = GetDataChannelInfoHeadNew(citFileName);


            //左高低与右高低对调
            ChannelExchange(m_dciL, "L_Prof_SC", "R_Prof_SC", false);
            ChannelExchange(m_dciL, "L_Prof_SC_70", "R_Prof_SC_70", false);
            ChannelExchange(m_dciL, "L_Prof_SC_120", "R_Prof_SC_120", false);

            DataChannelInfo m_dci_a = new DataChannelInfo();

            //左轨向与右轨向对调，然后幅值*（-1）
            ChannelExchange(m_dciL, "L_Align_SC", "R_Align_SC", true);
            ChannelExchange(m_dciL, "L_Align_SC_70", "R_Align_SC_70", true);
            ChannelExchange(m_dciL, "L_Align_SC_120", "R_Align_SC_120", true);


            //水平、超高、三角坑、曲率、曲率变化率*（-1）
            for (int i = 0; i < m_dciL.Count;i++ )
            {
                if (m_dciL[i].sNameEn.Equals("Crosslevel"))
                {
                    m_dci_a = m_dciL[i];
                    m_dci_a.fScale = m_dci_a.fScale * (-1);
                    m_dciL[i] = m_dci_a;
                }

                if (m_dciL[i].sNameEn.Equals("Superelevation"))
                {
                    m_dci_a = m_dciL[i];
                    m_dci_a.fScale = m_dci_a.fScale * (-1);
                    m_dciL[i] = m_dci_a;
                }

                if (m_dciL[i].sNameEn.Equals("Short_Twist"))
                {
                    m_dci_a = m_dciL[i];
                    m_dci_a.fScale = m_dci_a.fScale * (-1);
                    m_dciL[i] = m_dci_a;
                }

                if (m_dciL[i].sNameEn.Equals("Curvature"))
                {
                    m_dci_a = m_dciL[i];
                    m_dci_a.fScale = m_dci_a.fScale * (-1);
                    m_dciL[i] = m_dci_a;
                }

                if (m_dciL[i].sNameEn.Equals("Curvature_Rate"))
                {
                    m_dci_a = m_dciL[i];
                    m_dci_a.fScale = m_dci_a.fScale * (-1);
                    m_dciL[i] = m_dci_a;
                }

            }           


            try
            {
                #region 存取文件
                FileStream fsRead = new FileStream(citFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                FileStream fsWrite = new FileStream(citFileName + ".bak", FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(fsRead, Encoding.UTF8);
                BinaryWriter bw1 = new BinaryWriter(fsWrite, Encoding.UTF8);
                byte[] bHead = br.ReadBytes(120);
                byte[] bChannels = br.ReadBytes(65 * m_dhi.iChannelNumber);
                byte[] bData = new byte[m_dhi.iChannelNumber * 2];
                byte[] bDataNew = new byte[m_dhi.iChannelNumber * 2];
                byte[] bTail = br.ReadBytes(BitConverter.ToInt32(br.ReadBytes(4), 0));

                //bw1.Write(bHead);

                bw1.Write(GetBytesFromDataHeadInfo(m_dhi));//文件头
                //反向--转换为正向
                if (m_dhi.iRunDir == 0)
                {
                    bw1.Write(bChannels);
                } 
                else
                {
                    bw1.Write(GetBytesFromChannelDataInfoList(m_dciL));
                }
                
                bw1.Write(bTail.Length);
                bw1.Write(bTail);

                long startPos = br.BaseStream.Position;//记录数据开始位置的文件指针

                while (br.BaseStream.Position < br.BaseStream.Length)
                {
                    bw1.Write(br.ReadBytes(m_dhi.iChannelNumber * 2));
                    //br.BaseStream.Position += m_dhi.iChannelNumber * 2;
                }

                //
                bw1.Close();
                br.Close();
                fsWrite.Close();
                fsRead.Close();
                //删除bak
                Application.DoEvents();
                File.Delete(citFileName);
                Application.DoEvents();
                File.Move(citFileName + ".bak", citFileName);
                Application.DoEvents();
                #endregion
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            return;

        }

        private void ChannelExchange(List<DataChannelInfo> m_dciL,String channel_L,String channel_R,Boolean isInverted)
        {
            int index_a = 0;
            int index_b = 0;
            DataChannelInfo m_dci_a = new DataChannelInfo();
            DataChannelInfo m_dci_b = new DataChannelInfo();
            foreach (DataChannelInfo m_dci in m_dciL)
            {
                if (m_dci.sNameEn == channel_L)
                {
                    index_a = m_dciL.IndexOf(m_dci);
                    m_dci_a = m_dci;
                    if (isInverted)
                    {
                        m_dci_a.fScale = m_dci_a.fScale * (-1);
                    }                    
                }

                if (m_dci.sNameEn == channel_R)
                {
                    index_b = m_dciL.IndexOf(m_dci);
                    m_dci_b = m_dci;
                    if (isInverted)
                    {
                        m_dci_b.fScale = m_dci_b.fScale * (-1);
                    }                    
                }
            }
            if (index_a < index_b)
            {
                m_dciL.RemoveAt(index_a);
                m_dciL.Insert(index_a, m_dci_b);

                m_dciL.RemoveAt(index_b);
                m_dciL.Insert(index_b, m_dci_a);
            }
            else if (index_a > index_b)
            {
                m_dciL.RemoveAt(index_b);
                m_dciL.Insert(index_b, m_dci_a);

                m_dciL.RemoveAt(index_a);
                m_dciL.Insert(index_a, m_dci_b);
            }
        }
        #endregion

        public double GetMaxValue(double[] data)
        {
            if (data.Length == 0)
            {
                return 0;
            }
            double maxVal = data[0];

            for (int i = 1; i < data.Length;i++ )
            {
                if (data[i] > maxVal)
                {
                    maxVal = data[i];
                }
            }

            return maxVal;
        }

        public double GetMinValue(double[] data)
        {
            if (data.Length == 0)
            {
                return 0;
            }
            double minVal = data[0];

            for (int i = 1; i < data.Length; i++)
            {
                if (data[i] < minVal)
                {
                    minVal = data[i];
                }
            }

            return minVal;
        }

        public double GetAverageValue(double[] data)
        {
            if (data.Length == 0)
            {
                return 0;
            }
            double sumVal = 0;

            for (int i = 0; i < data.Length; i++)
            {
                sumVal += data[i];
            }

            return sumVal/data.Length;
        }

        public double GetStanderdDeviation(double[] data)
        {
            double val = 0;

            WaveformDataProcess wdp = new WaveformDataProcess();
            val = wdp.CalcStardard(data);

            return val;
        }

        public int GetSeverityValue(String iicFileName, String channelNameEnArray, float startMile, float endMile)
        {
            float minMile = Math.Min(startMile, endMile);
            float maxMile = Math.Max(startMile, endMile);
            int retVal = 0;

            String[] strArr = channelNameEnArray.Split(',');

            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + iicFileName + ";Persist Security Info=True"))
                {
                    sqlconn.Open();

                    foreach (String channelNameEn in strArr)
                    {
                        //查询某一个通道的一定范围内的扣分
                        string sqlCreate = "select * from fix_defects where (valid<>'N' and defectclass in (1,2,3,4) and defecttype='" + channelNameEn + "' and (maxpost+maxminor/1000 >=" + minMile + ") and (maxpost+maxminor/1000<=" + maxMile + "))";
                        OleDbCommand sqlcom = new OleDbCommand(sqlCreate, sqlconn);
                        OleDbDataReader dr = sqlcom.ExecuteReader();

                        while (dr.Read())
                        {
                            retVal += (int)(dr.GetValue(13));
                        }

                        dr.Close();
                    }

                    sqlconn.Close();
                }
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }

            return retVal;
        }

        public int GetDefectNum(String iicFileName, String channelNameEnArray, float startMile, float endMile,int defectClass)
        {
            float minMile = Math.Min(startMile, endMile);
            float maxMile = Math.Max(startMile, endMile);
            int retVal = 0;

            String[] strArr = channelNameEnArray.Split(',');

            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + iicFileName + ";Persist Security Info=True"))
                {
                    sqlconn.Open();

                    foreach (String channelNameEn in strArr)
                    {
                        //查询某一个通道的一定范围内的大值数量
                        string sqlCreate = "select * from fix_defects where (valid<>'N' and defecttype='" + channelNameEn + "' and (maxpost+maxminor/1000 >=" + minMile + ") and (maxpost+maxminor/1000<=" + maxMile + ") and defectclass=" + defectClass + ")";
                        OleDbCommand sqlcom = new OleDbCommand(sqlCreate, sqlconn);
                        OleDbDataReader dr = sqlcom.ExecuteReader();

                        while (dr.Read())
                        {
                            retVal += (int)(dr.GetValue(13));
                        }

                        dr.Close();
                    }

                    sqlconn.Close();
                }
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 0;
            }

            return retVal;
        }
    }
    #endregion

    #region STE格式数据处理类
    /// <summary>
    /// STE格式数据处理类 
    /// </summary>
    public class STEDataProcess
    {
        float[] fChannelScale;

        public STEDataProcess()
        {
            fChannelScale = new float[14] { 1, 4, 100, 100, 100, 100, 100, 100, 100, 100, 100, 1000, 1000, 100 };
        }



        #region 获取数据文件里程范围
        public string GetExportDataMileageRange(string sFile)
        {
            try
            {
                FileStream fs = new FileStream(sFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(fs, Encoding.Default);
                br.BaseStream.Position = 0;
                byte[] b = new byte[28];
                string bRet = string.Empty;

                //定位头
                long iCurPos = 0;
                while (br.PeekChar() != -1)
                {

                    b = br.ReadBytes(28);
                    short sKM = BitConverter.ToInt16(b, 0);
                    short sM = BitConverter.ToInt16(b, 2);
                    int iKM = sKM * 1000 + sM / 4;
                    if (!(iKM <= 0 || (sKM > 3000)))
                    {

                        bRet += (Math.Abs(iKM / 1000.0)).ToString("f3");
                        iCurPos = br.BaseStream.Position;
                        break;
                    }


                }

                //定位
                int iReturnPosition = 28;
                while (iCurPos <= br.BaseStream.Position)
                {
                    br.BaseStream.Position = br.BaseStream.Length - iReturnPosition;
                    b = br.ReadBytes(28);
                    short sKM = BitConverter.ToInt16(b, 0);
                    short sM = BitConverter.ToInt16(b, 2);
                    int iKM = sKM * 1000 + sM / 4;
                    if (!(iKM <= 0 || (sKM > 3000)))
                    {
                        bRet += ("-" + ((Math.Abs(iKM / 1000.0)).ToString("f3")));
                        break;
                    }
                    iReturnPosition += 28;

                }


                br.Close();
                fs.Close();
                return "0," + bRet;
            }
            catch (Exception ex)
            {
                return "1," + ex.Message;
            }


        }
        #endregion


        #region 获取导出数据的坐标定位
        private long[] GetExportDataInfoPosition(string sFile, int iStartKM, int iEndKM)
        {
            FileStream fs = new FileStream(sFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            BinaryReader br = new BinaryReader(fs, Encoding.Default);
            br.BaseStream.Position = 0;
            byte[] b = new byte[28];
            long[] bRet = new long[2];
            bool bRight = true;
            bRet[0] = br.BaseStream.Position;
            bRet[1] = br.BaseStream.Length;
            //找到
            while (br.PeekChar() != -1)
            {

                b = br.ReadBytes(28);
                short sKM = BitConverter.ToInt16(b, 0);
                short sM = BitConverter.ToInt16(b, 2);
                int iKM = sKM * 1000 + sM / 4;
                if (bRight && iKM == iStartKM)
                {

                    bRight = false;
                    bRet[0] = br.BaseStream.Position - 28;
                }

                if (iKM == iEndKM)
                {
                    bRet[1] = br.BaseStream.Position;
                    break;
                }

            }

            br.Close();
            fs.Close();
            return bRet;

        }
        #endregion

        #region 导出数据操作
        public int ExportData(string sSourceFile, string sDestinationFile, int iStartKM, int iEndKM, string[] sParameter)
        {
            try
            {
                string[] sCNs = sParameter[0].Split(new char[] { ',' });
                long[] bRet = GetExportDataInfoPosition(sSourceFile, iStartKM, iEndKM);
                FileStream fs = new FileStream(sSourceFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(fs, Encoding.Default);
                br.BaseStream.Position = bRet[0];
                int iChannelNumberSize = 28;
                byte[] b = new byte[iChannelNumberSize];
                StreamWriter sw = new StreamWriter(sDestinationFile);

                while (br.BaseStream.Position < bRet[1])
                {

                    b = br.ReadBytes(iChannelNumberSize);
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < sCNs.Length; i++)
                    {
                        sb.Append((BitConverter.ToInt16(b, int.Parse(sCNs[i]) * 2) / fChannelScale[i]).ToString());
                        sb.Append(",");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sw.WriteLine(sb);

                }


                sw.Close();
                br.Close();
                fs.Close();


                return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 1;
            }
        }
        #endregion

        #region 导出原始数据操作
        public int ExportOraData(string sSourceFile, string sDestinationFile, int iStartKM, int iEndKM, string[] sParameter)
        {
            try
            {
                string[] sCNs = sParameter[0].Split(new char[] { ',' });
                long[] bRet = GetExportDataInfoPosition(sSourceFile, iStartKM, iEndKM);
                FileStream fs = new FileStream(sSourceFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(fs, Encoding.Default);
                br.BaseStream.Position = bRet[0];
                int iChannelNumberSize = 28;
                byte[] b = new byte[iChannelNumberSize];
                FileStream fsWrite = new FileStream(sDestinationFile, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                while (br.BaseStream.Position < bRet[1])
                {

                    b = br.ReadBytes(iChannelNumberSize);
                    fsWrite.Write(b, 0, b.Length);
                }


                fsWrite.Close();
                br.Close();
                fs.Close();


                return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return 1;
            }
        }
        #endregion


    }
    #endregion

    #region GEO格式数据处理类
    /// <summary>
    /// GEO格式数据处理类
    /// </summary>
    public class GEODataProcess
    {
        const int GEO_FILE_VERSION = 0x3031;
        const int GEO_FILE_HEADER_SIZE = 0x2800;
        const int GEOMETRY_RECORD_ID = 0x5a5a;
        int iChannelLength = 0;
        int iChannelCount = 0;
        List<DataChannelInfo> dciL;
        GEO_FILE_HEADER gfh;

        #region 文件信息结构体
        private struct GEO_FILE_HEADER
        {
            public short file_version;
            public short dir_flag;
            public int data_record_length;
            public float sample_interval;
            public float post_units;
            public string date;
            public string time;
            public string area;
            public string division;
            public string region;

        }
        #endregion

        #region 通道信息结构体
        private struct DataChannelInfo
        {
            public string sNameEn;
            public float fScale;
            public string sUnit;

        }
        #endregion

        #region 按照结构体的读取各个数据
        private GEO_FILE_HEADER GetDataInfoHead(byte[] bDataInfo)
        {

            GEO_FILE_HEADER gfh = new GEO_FILE_HEADER();

            gfh.file_version = BitConverter.ToInt16(bDataInfo, 0);
            gfh.dir_flag = BitConverter.ToInt16(bDataInfo, 2);
            gfh.data_record_length = BitConverter.ToInt32(bDataInfo, 4);
            gfh.sample_interval = BitConverter.ToSingle(bDataInfo, 8);
            gfh.post_units = BitConverter.ToSingle(bDataInfo, 12);

            //获取GEO Date
            StringBuilder sbDate = new StringBuilder();
            for (int i = 0; i < 8; i++)
            {
                if (bDataInfo[42 + i] == 0)
                {
                    break;
                }
                sbDate.Append(UnicodeEncoding.Default.GetString(bDataInfo, 42 + i, 1));
            }
            gfh.date = sbDate.ToString();

            //获取GEO Time
            StringBuilder sbTime = new StringBuilder();
            for (int i = 0; i < 8; i++)
            {
                if (bDataInfo[54 + i] == 0)
                {
                    break;
                }
                sbTime.Append(UnicodeEncoding.Default.GetString(bDataInfo, 54 + i, 1));
            }
            gfh.time = sbTime.ToString();

            //获取GEO Area
            StringBuilder sbArea = new StringBuilder();
            for (int i = 0; i < 60; i++)
            {
                if (bDataInfo[66 + i] == 0)
                {
                    break;
                }
                sbArea.Append(UnicodeEncoding.Default.GetString(bDataInfo, 66 + i, 1));
            }
            gfh.area = sbArea.ToString();

            //获取GEO Division
            StringBuilder sbDivision = new StringBuilder();
            for (int i = 0; i < 16; i++)
            {
                if (bDataInfo[130 + i] == 0)
                {
                    break;
                }
                sbDivision.Append(UnicodeEncoding.Default.GetString(bDataInfo, 130 + i, 1));
            }
            gfh.division = sbDivision.ToString();

            //获取GEO Region
            StringBuilder sbRegion = new StringBuilder();
            for (int i = 0; i < 16; i++)
            {
                if (bDataInfo[150 + i] == 0)
                {
                    break;
                }
                sbRegion.Append(UnicodeEncoding.Default.GetString(bDataInfo, 150 + i, 1));
            }
            gfh.region = sbRegion.ToString();

            return gfh;
        }
        #endregion

        #region 获取通道信息
        private DataChannelInfo GetChannelInfo(byte[] bDataInfo, ref int start)
        {
            DataChannelInfo dci = new DataChannelInfo();
            StringBuilder sNameEn = new StringBuilder();
            StringBuilder sUnit = new StringBuilder();
            int iNameEnLen = (int)bDataInfo[start];
            for (int i = 1; i <= iNameEnLen; i++)
            {
                sNameEn.Append(UnicodeEncoding.Default.GetString(bDataInfo, i + start, 1));
            }
            start += (1 + iNameEnLen);
            int iUnitLen = (int)bDataInfo[start];
            for (int i = 1; i <= iUnitLen; i++)
            {
                sUnit.Append(UnicodeEncoding.Default.GetString(bDataInfo, i + start, 1));
            }
            start += (1 + iUnitLen);

            dci.sNameEn = sNameEn.ToString();
            dci.sUnit = sUnit.ToString();
            dci.fScale = BitConverter.ToInt32(bDataInfo, start);
            start += 4;
            while (start < bDataInfo.Length && (bDataInfo[start] == (byte)0))
            {
                start++;
            }

            return dci;

        }
        #endregion



        #region 获取数据文件前后坐标
        public long[] GetDataMileagePositionRange(string sFile)
        {
            try
            {
                FileStream fs = new FileStream(sFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(fs, Encoding.Default);
                br.BaseStream.Position = 0;
                br.ReadBytes(GEO_FILE_HEADER_SIZE);
                br.ReadBytes(4);
                br.ReadBytes(iChannelLength - 2);
                int iB = (iChannelCount * 2 + 8 * 2);
                byte[] b = new byte[iB];
                string bRet = string.Empty;
                long[] lPosition = new long[2];
                lPosition[0] = br.BaseStream.Position;
                lPosition[1] = br.BaseStream.Length;

                br.Close();
                fs.Close();
                return lPosition;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return new long[2];
            }


        }
        #endregion

        #region 获取通道的比例
        public float[] GetChannelScale()
        {
            float[] fScale = new float[dciL.Count];
            for (int i = 0; i < dciL.Count; i++)
            {
                fScale[i] = dciL[i].fScale;
            }
            return fScale;
        }
        #endregion

        #region 获取数据文件里程范围
        public string GetExportDataMileageRange(string sFile)
        {
            try
            {
                FileStream fs = new FileStream(sFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(fs, Encoding.Default);
                br.BaseStream.Position = 0;
                br.ReadBytes(GEO_FILE_HEADER_SIZE);
                br.ReadBytes(4);
                br.ReadBytes(iChannelLength - 2);
                int iB = (gfh.data_record_length);
                byte[] b = new byte[iB];
                string bRet = string.Empty;

                //定位头
                long iCurPos = 0;
                while (br.PeekChar() != -1)
                {

                    b = br.ReadBytes(iB);
                    short sI = BitConverter.ToInt16(b, 0);
                    if (sI != GEOMETRY_RECORD_ID)
                    {
                        continue;
                    }
                    short sKM = BitConverter.ToInt16(b, 8);
                    short sM = BitConverter.ToInt16(b, 10);

                    int iKM = sKM * 1000 + sM / 4;
                    if (!(iKM == 0 || (sKM > 3000)))
                    {
                        bRet += (Math.Abs(iKM / 1000.0)).ToString("f3");
                        iCurPos = br.BaseStream.Position;
                        break;
                    }


                }

                //定位
                int iReturnPosition = iB;
                while (iCurPos <= br.BaseStream.Position)
                {
                    br.BaseStream.Position = br.BaseStream.Length - iReturnPosition;
                    b = br.ReadBytes(iB);
                    short sI = BitConverter.ToInt16(b, 0);
                    if (sI != GEOMETRY_RECORD_ID)
                    {
                        iReturnPosition += (iB);
                        continue;
                    }
                    short sKM = BitConverter.ToInt16(b, 8);
                    short sM = BitConverter.ToInt16(b, 10);
                    int iKM = sKM * 1000 + sM / 4;
                    if (!(iKM == 0 || (sKM > 3000)))
                    {
                        bRet += ("-" + ((Math.Abs(iKM / 1000.0)).ToString("f3")));
                        break;
                    }
                    iReturnPosition += (iB);

                }


                br.Close();
                fs.Close();
                return "0," + bRet;
            }
            catch (Exception ex)
            {
                return "1,dw " + ex.Message;
            }


        }
        #endregion

        #region 获取导出数据的坐标定位
        private long[] GetExportDataInfoPosition(string sFile, int iStartKM, int iEndKM)
        {
            FileStream fs = new FileStream(sFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            BinaryReader br = new BinaryReader(fs, Encoding.Default);
            br.BaseStream.Position = 0;
            br.ReadBytes(GEO_FILE_HEADER_SIZE);
            br.ReadBytes(4);
            br.ReadBytes(iChannelLength - 2);
            int iB = (gfh.data_record_length);
            byte[] b = new byte[iB];
            long[] bRet = new long[2];
            bool bRight = true;
            bRet[0] = br.BaseStream.Position;
            bRet[1] = br.BaseStream.Length;
            //找到
            while (br.BaseStream.Position < br.BaseStream.Length)
            {

                b = br.ReadBytes(iB);
                short sI = BitConverter.ToInt16(b, 0);
                if (sI != GEOMETRY_RECORD_ID)
                {
                    continue;
                }
                short sKM = BitConverter.ToInt16(b, 8);
                short sM = BitConverter.ToInt16(b, 10);
                int iKM = sKM * 1000 + sM / 4;
                if (bRight && iKM == iStartKM)
                {

                    bRight = false;
                    bRet[0] = br.BaseStream.Position - iB;
                }

                if (iKM == iEndKM)
                {

                    bRet[1] = br.BaseStream.Position;
                    break;
                }

            }
            br.Close();
            fs.Close();
            return bRet;

        }
        #endregion

        #region 导出数据操作1
        public int ExportData(string sSourceFile, string sDestinationFile, int iStartKM, int iEndKM, List<AreaClass> listAC)
        {
            try
            {
                FileStream fs = new FileStream(sSourceFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(fs, Encoding.Default);
                br.BaseStream.Position = 0;
                int iChannelNumberSize = (iChannelCount * 2 + 8 * 2);
                byte[] b = new byte[iChannelNumberSize];
                byte[] b1 = br.ReadBytes(GEO_FILE_HEADER_SIZE);
                byte[] b2 = br.ReadBytes(4);
                byte[] b3 = br.ReadBytes(iChannelLength - 2);
                FileStream fs1 = new FileStream(sDestinationFile, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                BinaryWriter bw = new BinaryWriter(fs1);
                long[] bRet = GetExportDataInfoPosition(sSourceFile, iStartKM, iEndKM);
                br.BaseStream.Position = bRet[0];

                bw.Write(b1);
                bw.Write(b2);
                bw.Write(b3);
                long ll1 = 0;

                while (br.BaseStream.Position < bRet[1])
                {

                    b = br.ReadBytes(gfh.data_record_length);
                    short sI = BitConverter.ToInt16(b, 0);
                    if (sI != GEOMETRY_RECORD_ID)
                    {

                        continue;
                    }
                    //导入里程
                    short sKM = BitConverter.ToInt16(b, 8);
                    short sM = BitConverter.ToInt16(b, 10);
                    double dMeter = sKM + sM / (1 / gfh.sample_interval) / 1000.0;

                    bool bContinue = false;
                    if (listAC != null || listAC.Count > 0)
                    {
                        foreach (AreaClass ac in listAC)
                        {
                            if (dMeter >= ac.dStartMeter && dMeter <= ac.dEndMeter)
                            {

                                bContinue = true;
                                break;
                            }
                        }
                    }
                    if (bContinue)
                    {
                        ll1++;
                        continue;
                    }

                    bw.Write(b);

                }

                bw.Close();
                fs1.Close();
                br.Close();
                fs.Close();


                return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ed " + ex.Message);
                return 1;
            }
        }
        #endregion

        #region 导出数据操作2
        public int ExportData(string sSourceFile, string sDestinationFile, int iStartKM, int iEndKM, string[] sParameter, List<AreaClass> listAC)
        {
            try
            {
                string[] sCNs = sParameter[0].Split(new char[] { ',' });
                long[] bRet = GetExportDataInfoPosition(sSourceFile, iStartKM, iEndKM);
                FileStream fs = new FileStream(sSourceFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(fs, Encoding.Default);
                br.BaseStream.Position = bRet[0];
                int iChannelNumberSize = (iChannelCount * 2 + 8 * 2);
                byte[] b = new byte[iChannelNumberSize];
                StreamWriter sw = new StreamWriter(sDestinationFile);
                //MessageBox.Show(bool.Parse(sParameter[3]).ToString());
                if (bool.Parse(sParameter[3]))
                {
                    sw.WriteLine("Meter," + sParameter[2]);
                }
                while (br.BaseStream.Position < bRet[1])
                {

                    b = br.ReadBytes(iChannelNumberSize);
                    short sI = BitConverter.ToInt16(b, 0);
                    if (sI != GEOMETRY_RECORD_ID)
                    {
                        continue;
                    }
                    StringBuilder sb = new StringBuilder();
                    //导入里程
                    short sKM = BitConverter.ToInt16(b, 8);
                    short sM = BitConverter.ToInt16(b, 10);
                    double dMeter = sKM + sM / (1 / gfh.sample_interval) / 1000.0;

                    bool bContinue = false;
                    if (listAC != null || listAC.Count > 0)
                    {
                        foreach (AreaClass ac in listAC)
                        {
                            if (dMeter >= ac.dStartMeter && dMeter <= ac.dEndMeter)
                            {

                                bContinue = true;
                                break;
                            }
                        }
                    }
                    if (bContinue)
                    {
                        continue;
                    }
                    sb.Append(dMeter.ToString());
                    sb.Append(",");
                    if (dMeter < 0)
                    {
                        continue;
                    }

                    //sb.Append(sKM.ToString() + "," + (sM / (1 / gfh.sample_interval)).ToString() + ",");
                    for (int i = 0; i < sCNs.Length; i++)
                    {
                        sb.Append((BitConverter.ToInt16(b, (int.Parse(sCNs[i]) + 8) * 2) / dciL[int.Parse(sCNs[i])].fScale).ToString());
                        sb.Append(",");
                    }
                    sb.Remove(sb.Length - 1, 1);
                    sw.WriteLine(sb);

                }


                sw.Close();
                br.Close();
                fs.Close();


                return 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("ed " + ex.Message);
                return 1;
            }
        }
        #endregion


        #region 转换数据操作 ---- 网络版使用
        //网络版使用
        public int ConvertData(string sSourceFile, string sDestinationFile, string[] sParameter, int iType, List<GEO2CITBind> listGG)
        {
            //try
            //{
            //int sCNs = int.Parse(sParameter[0]);


            //10号车通道对应表
            int iPostion = 18;
            byte[] bb = Encoding.Default.GetBytes(sParameter[1]);

            using (FileStream fs = new FileStream(sSourceFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (BinaryReader br = new BinaryReader(fs, Encoding.Default))
                {
                    br.BaseStream.Position = 0;
                    gfh = GetDataInfoHead(br.ReadBytes(GEO_FILE_HEADER_SIZE));
                    //添加通道信息
                    iChannelLength = BitConverter.ToInt16(br.ReadBytes(2), 0);
                    iChannelCount = BitConverter.ToInt16(br.ReadBytes(2), 0);
                    //iChannelCount = iChannelCount - 1;
                    FileStream fsWrite = new FileStream(sDestinationFile, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                    BinaryWriter bw = new BinaryWriter(fsWrite, Encoding.Default);
                    //存取文件头
                    bw.Write(1);

                    bw.Write("3.0.0");
                    for (int i = 0; i < 15; i++)
                    {
                        bw.Write((byte)0);
                    }

                    bw.Write(sParameter[0]);

                    bw.Write(sParameter[1]);
                    for (int i = 0; i < 20 - bb.Length; i++)
                    {
                        bw.Write((byte)0);
                    }

                    bw.Write(int.Parse(sParameter[2]));

                    bw.Write(sParameter[5]);
                    for (int i = 0; i < 20 - sParameter[5].Length; i++)
                    {
                        bw.Write((byte)0);
                    }

                    bw.Write(sParameter[3]);
                    bw.Write(sParameter[4]);

                    bw.Write(0);

                    bw.Write(int.Parse(sParameter[6].ToString()));


                    bw.Write((float)0.0);
                    bw.Write((float)0.0);


                    bw.Write((int)(1 / gfh.sample_interval));
                    //if (iType == 10)
                    //{
                    //    bw.Write(2 + listGG.Count);
                    //}
                    //else
                    //{
                    bw.Write(2 + listGG.Count);
                    //}
                    bw.Flush();
                    //公里，米
                    bw.Write(1);
                    bw.Write("Km");
                    for (int i = 0; i < 18; i++)
                    {
                        bw.Write((byte)0);
                    }
                    bw.Write("Km");
                    for (int i = 0; i < 18; i++)
                    {
                        bw.Write((byte)0);
                    }
                    bw.Write((float)1.0);
                    bw.Write((float)0.0);
                    for (int i = 0; i < 11; i++)
                    {
                        bw.Write((byte)0);
                    }

                    bw.Write(2);
                    bw.Write("Meter");
                    for (int i = 0; i < 15; i++)
                    {
                        bw.Write((byte)0);
                    }
                    bw.Write("Meter");
                    for (int i = 0; i < 15; i++)
                    {
                        bw.Write((byte)0);
                    }
                    bw.Write((float)4.0);
                    bw.Write((float)0.0);
                    for (int i = 0; i < 11; i++)
                    {
                        bw.Write((byte)0);
                    }


                    StringBuilder sbName = new StringBuilder();
                    dciL = new List<DataChannelInfo>();
                    byte[] bChannelData = br.ReadBytes(iChannelLength - 2);


                    //读取通道
                    for (int j = 0; j < iChannelLength - 2; )
                    {
                        DataChannelInfo dci = GetChannelInfo(bChannelData, ref j);
                        dciL.Add(dci);
                        if (dciL.Count > iChannelCount)
                        {
                            break;
                        }
                        if (iType == 10)
                        {
                            if (dci.sNameEn.ToLower().Contains("null"))
                            {
                                break;
                            }
                        }

                    }
                    //
                    //通道匹配
                    int g = 2;
                    List<int> listID = new List<int>();
                    for (int gg = 0; gg < listGG.Count; gg++)
                    {

                        for (int g1 = 0; g1 < dciL.Count; g1++)
                        {
                            if (dciL[g1].sNameEn.Equals(listGG[gg].sGEO))
                            {
                                listID.Add(g1);
                                bw.Write(g);
                                if (listGG[gg].sCIT.Length > 20)
                                {
                                    bw.Write(listGG[gg].sCIT.Substring(0, 20));
                                }
                                else
                                {
                                    bw.Write(listGG[gg].sCIT);
                                    for (int i = 0; i < 20 - listGG[gg].sCIT.Length; i++)
                                    {
                                        bw.Write((byte)0);
                                    }
                                }
                                if (Encoding.Default.GetBytes(listGG[gg].sChinese).Length > 20)
                                {
                                    bw.Write(listGG[gg].sCIT.Substring(0, 20));
                                }
                                else
                                {
                                    bw.Write(listGG[gg].sChinese);
                                    for (int i = 0; i < 20 - Encoding.Default.GetBytes(listGG[gg].sChinese).Length; i++)
                                    {
                                        bw.Write((byte)0);
                                    }
                                }
                                bw.Write((float)dciL[g1].fScale);
                                bw.Write((float)0.0);
                                if (dciL[g1].sUnit.Length > 10)
                                {
                                    bw.Write(dciL[g1].sUnit.Substring(0, 10));
                                }
                                else
                                {
                                    bw.Write(dciL[g1].sUnit);
                                    for (int i = 0; i < 10 - dciL[g1].sUnit.Length; i++)
                                    {
                                        bw.Write((byte)0);
                                    }
                                }

                                ++g;

                                break;
                            }
                        }

                    }

                    bw.Write(0);
                    br.BaseStream.Position = 0;
                    br.ReadBytes(GEO_FILE_HEADER_SIZE);
                    br.ReadBytes(4);
                    br.ReadBytes(iChannelLength - 2);

                    int iChannelNumberSize = (gfh.data_record_length);
                    byte[] b = new byte[iChannelNumberSize];
                    if (iType == 10)
                    {
                        iChannelCount -= 1;
                    }
                    else
                    {

                    }
                    while (br.BaseStream.Position < br.BaseStream.Length)
                    {

                        b = br.ReadBytes(iChannelNumberSize);
                        int iGeometryRecordId = BitConverter.ToInt16(b, 0);
                        if (iGeometryRecordId != GEOMETRY_RECORD_ID)
                        {
                            continue;
                        }
                        short sKM = BitConverter.ToInt16(b, 4 * 2);
                        short sM = BitConverter.ToInt16(b, 5 * 2);
                        byte[] bResult = new byte[4 + listID.Count * 2];
                        //bResult

                        byte[] b1 = BitConverter.GetBytes(BitConverter.ToInt16(b, 4 * 2));
                        bResult[0] = b1[0];
                        bResult[1] = b1[1];
                        b1 = BitConverter.GetBytes(BitConverter.ToInt16(b, 5 * 2));
                        bResult[2] = b1[0];
                        bResult[3] = b1[1];
                        //12
                        for (int i = 0; i < listID.Count; i++)
                        {
                            if (iType == 307)
                            {
                                if (listID[i] > 22)
                                {
                                    b1 = BitConverter.GetBytes(BitConverter.ToInt16(b, iPostion + 12 + listID[i] * 2));
                                }
                                else
                                {
                                    b1 = BitConverter.GetBytes(BitConverter.ToInt16(b, iPostion + listID[i] * 2));
                                }

                            }
                            else
                            {
                                b1 = BitConverter.GetBytes(BitConverter.ToInt16(b, iPostion + listID[i] * 2));

                            }
                            bResult[4 + i * 2] = b1[0];
                            bResult[4 + i * 2 + 1] = b1[1];

                        }
                        bResult = WaveformDataProcess.ByteXORByte(bResult);
                        bw.Write(bResult);

                    }



                    bw.Close();
                    fsWrite.Close();


                    br.Close();
                    fs.Close();

                }
            }



            return 0;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("ed " + ex.Message);
            //    return 1;
            //}
        }
        //没有使用
        public int ConvertData(string sSourceFile, string sDestinationFile, List<GEO2CITBind> listGG, int iType,
            CITDataProcess.DataHeadInfo structCITHead)
        {
            //try
            //{
            //int sCNs = int.Parse(sParameter[0]);


            //10号车通道对应表
            int iPostion = 18;
            byte[] bb = Encoding.Default.GetBytes(structCITHead.sTrackName);

            using (FileStream fs = new FileStream(sSourceFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (BinaryReader br = new BinaryReader(fs, Encoding.Default))
                {
                    br.BaseStream.Position = 0;
                    gfh = GetDataInfoHead(br.ReadBytes(GEO_FILE_HEADER_SIZE));
                    //添加通道信息
                    iChannelLength = BitConverter.ToInt16(br.ReadBytes(2), 0);
                    iChannelCount = BitConverter.ToInt16(br.ReadBytes(2), 0);
                    //iChannelCount = iChannelCount - 1;
                    FileStream fsWrite = new FileStream(sDestinationFile, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                    BinaryWriter bw = new BinaryWriter(fsWrite, Encoding.Default);
                    //存取文件头
                    bw.Write(structCITHead.iDataType);//iDataType：1轨检、2动力学、3弓网，


                    Byte[] tmpBytes = UnicodeEncoding.Default.GetBytes("3.0.0");
                    bw.Write((byte)(tmpBytes.Length));
                    bw.Write(tmpBytes);
                    for (int i = 0; i < 20-tmpBytes.Length; i++)
                    {
                        bw.Write((byte)0);
                    }

                    bw.Write(structCITHead.sTrackCode);

                    bw.Write(structCITHead.sTrackName);
                    for (int i = 0; i < 20 - bb.Length; i++)
                    {
                        bw.Write((byte)0);
                    }

                    bw.Write(structCITHead.iDir);

                    bw.Write(structCITHead.sTrain);
                    for (int i = 0; i < 20 - structCITHead.sTrain.Length; i++)
                    {
                        bw.Write((byte)0);
                    }

                    bw.Write(structCITHead.sDate);
                    bw.Write(structCITHead.sTime);

                    bw.Write(structCITHead.iRunDir);

                    bw.Write(structCITHead.iKmInc);

                    bw.Write(structCITHead.fkmFrom);
                    bw.Write(structCITHead.fkmTo);


                    bw.Write(structCITHead.iSmaleRate);
                    //if (iType == 10)
                    //{
                    //    bw.Write(2 + listGG.Count);
                    //}
                    //else
                    //{
                    bw.Write(2 + listGG.Count);
                    //}
                    bw.Flush();
                    //公里，米
                    bw.Write(1);
                    bw.Write("Km");
                    for (int i = 0; i < 18; i++)
                    {
                        bw.Write((byte)0);
                    }
                    bw.Write("Km");
                    for (int i = 0; i < 18; i++)
                    {
                        bw.Write((byte)0);
                    }
                    bw.Write((float)1.0);
                    bw.Write((float)0.0);
                    for (int i = 0; i < 11; i++)
                    {
                        bw.Write((byte)0);
                    }

                    bw.Write(2);
                    bw.Write("Meter");
                    for (int i = 0; i < 15; i++)
                    {
                        bw.Write((byte)0);
                    }
                    bw.Write("Meter");
                    for (int i = 0; i < 15; i++)
                    {
                        bw.Write((byte)0);
                    }
                    bw.Write((float)4.0);
                    bw.Write((float)0.0);
                    for (int i = 0; i < 11; i++)
                    {
                        bw.Write((byte)0);
                    }


                    StringBuilder sbName = new StringBuilder();
                    dciL = new List<DataChannelInfo>();
                    byte[] bChannelData = br.ReadBytes(iChannelLength - 2);


                    //读取通道
                    for (int j = 0; j < iChannelLength - 2; )
                    {
                        DataChannelInfo dci = GetChannelInfo(bChannelData, ref j);
                        dciL.Add(dci);
                        if (dciL.Count > iChannelCount)
                        {
                            break;
                        }
                        if (iType == 10)
                        {
                            if (dci.sNameEn.ToLower().Contains("null"))
                            {
                                break;
                            }
                        }


                    }
                    //
                    //通道匹配
                    int g = 2;
                    List<int> listID = new List<int>();
                    for (int gg = 0; gg < listGG.Count; gg++)
                    {

                        for (int g1 = 0; g1 < dciL.Count; g1++)
                        {
                            if (dciL[g1].sNameEn.Equals(listGG[gg].sGEO))
                            {
                                listID.Add(g1);
                                bw.Write(g);
                                if (listGG[gg].sCIT.Length > 20)
                                {
                                    bw.Write(listGG[gg].sCIT.Substring(0, 20));
                                }
                                else
                                {
                                    bw.Write(listGG[gg].sCIT);
                                    for (int i = 0; i < 20 - listGG[gg].sCIT.Length; i++)
                                    {
                                        bw.Write((byte)0);
                                    }
                                }
                                if (Encoding.Default.GetBytes(listGG[gg].sChinese).Length > 20)
                                {
                                    bw.Write(listGG[gg].sCIT.Substring(0, 20));
                                }
                                else
                                {
                                    bw.Write(listGG[gg].sChinese);
                                    for (int i = 0; i < 20 - Encoding.Default.GetBytes(listGG[gg].sChinese).Length; i++)
                                    {
                                        bw.Write((byte)0);
                                    }
                                }
                                bw.Write((float)dciL[g1].fScale);
                                bw.Write((float)0.0);
                                if (dciL[g1].sUnit.Length > 10)
                                {
                                    bw.Write(dciL[g1].sUnit.Substring(0, 10));
                                }
                                else
                                {
                                    bw.Write(dciL[g1].sUnit);
                                    for (int i = 0; i < 10 - dciL[g1].sUnit.Length; i++)
                                    {
                                        bw.Write((byte)0);
                                    }
                                }

                                ++g;

                                break;
                            }
                        }

                    }

                    bw.Write(0);
                    br.BaseStream.Position = 0;
                    br.ReadBytes(GEO_FILE_HEADER_SIZE);
                    br.ReadBytes(4);
                    br.ReadBytes(iChannelLength - 2);

                    int iChannelNumberSize = (gfh.data_record_length);
                    byte[] b = new byte[iChannelNumberSize];
                    if (iType == 10)
                    {
                        iChannelCount -= 1;
                    }
                    else
                    {

                    }
                    while (br.BaseStream.Position < br.BaseStream.Length)
                    {

                        b = br.ReadBytes(iChannelNumberSize);
                        int iGeometryRecordId = BitConverter.ToInt16(b, 0);
                        if (iGeometryRecordId != GEOMETRY_RECORD_ID)
                        {
                            continue;
                        }
                        short sKM = BitConverter.ToInt16(b, 4 * 2);
                        short sM = BitConverter.ToInt16(b, 5 * 2);
                        byte[] bResult = new byte[4 + listID.Count * 2];
                        //bResult

                        byte[] b1 = BitConverter.GetBytes(BitConverter.ToInt16(b, 4 * 2));
                        bResult[0] = b1[0];
                        bResult[1] = b1[1];
                        b1 = BitConverter.GetBytes(BitConverter.ToInt16(b, 5 * 2));
                        bResult[2] = b1[0];
                        bResult[3] = b1[1];
                        //12
                        for (int i = 0; i < listID.Count; i++)
                        {
                            if (iType == 307)
                            {
                                if (listID[i] > 22)
                                {
                                    b1 = BitConverter.GetBytes(BitConverter.ToInt16(b, iPostion + 12 + listID[i] * 2));
                                }
                                else
                                {
                                    b1 = BitConverter.GetBytes(BitConverter.ToInt16(b, iPostion + listID[i] * 2));
                                }

                            }
                            else
                            {
                                b1 = BitConverter.GetBytes(BitConverter.ToInt16(b, iPostion + listID[i] * 2));

                            }
                            bResult[4 + i * 2] = b1[0];
                            bResult[4 + i * 2 + 1] = b1[1];

                        }
                        bResult = WaveformDataProcess.ByteXORByte(bResult);
                        bw.Write(bResult);

                    }



                    bw.Close();
                    fsWrite.Close();


                    br.Close();
                    fs.Close();

                }
            }



            return 0;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("ed " + ex.Message);
            //    return 1;
            //}
        }

        #endregion

        #region 获取GEO文件头信息
        public string QueryDataInfoHead(string sFile)
        {
            try
            {
                using (FileStream fs = new FileStream(sFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (BinaryReader br = new BinaryReader(fs, Encoding.Default))
                    {
                        br.BaseStream.Position = 0;
                        gfh = GetDataInfoHead(br.ReadBytes(GEO_FILE_HEADER_SIZE));
                        br.Close();
                        fs.Close();
                        StringBuilder sbName = new StringBuilder();
                        sbName.Append(gfh.file_version);
                        sbName.Append(",");
                        sbName.Append(gfh.dir_flag);
                        sbName.Append(",");
                        sbName.Append(gfh.data_record_length);
                        sbName.Append(",");
                        sbName.Append(gfh.sample_interval);
                        sbName.Append(",");
                        sbName.Append(gfh.post_units);
                        sbName.Append(",");
                        sbName.Append(gfh.date);
                        sbName.Append(",");
                        sbName.Append(gfh.time);
                        sbName.Append(",");
                        sbName.Append(gfh.area);
                        sbName.Append(",");
                        sbName.Append(gfh.division);
                        sbName.Append(",");
                        sbName.Append(gfh.region);
                        return "0," + sbName.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                return "1," + ex.Message;
            }
        }
        #endregion

        #region 查询通道信息---没有使用
        public string QueryDataChannelInfoHead(string sFile)
        {

            try
            {
                using (FileStream fs = new FileStream(sFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (BinaryReader br = new BinaryReader(fs, Encoding.Default))
                    {
                        br.BaseStream.Position = 0;
                        gfh = GetDataInfoHead(br.ReadBytes(GEO_FILE_HEADER_SIZE));
                        iChannelLength = BitConverter.ToInt16(br.ReadBytes(2), 0);
                        iChannelCount = BitConverter.ToInt16(br.ReadBytes(2), 0);
                        StringBuilder sbName = new StringBuilder();
                        dciL = new List<DataChannelInfo>();
                        byte[] bChannelData = br.ReadBytes(iChannelLength - 2);
                        int iGeometryRecordId = BitConverter.ToInt16(br.ReadBytes(2), 0);
                        //if (iGeometryRecordId != GEOMETRY_RECORD_ID)
                        //{
                        //    return "1,文件格式错误！";
                        //}
                        for (int i = 0; i < iChannelLength - 2; )
                        {
                            DataChannelInfo dci = GetChannelInfo(bChannelData, ref i);
                            dciL.Add(dci);
                            sbName.Append(dci.sNameEn + ",");

                        }
                        sbName.Remove(sbName.Length - 1, 1);
                        br.Close();
                        fs.Close();
                        return "0," + sbName.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                return "1,qc" + ex.Message;
            }
        }
        #endregion


        #region 接口函数---没有使用
        public void ProcessFile(string sFile)
        {
            FileStream fsRead = new FileStream(sFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            FileStream fsWrite = new FileStream(sFile + ".bak", FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
            BinaryReader br = new BinaryReader(fsRead, Encoding.UTF8);
            BinaryWriter bw1 = new BinaryWriter(fsWrite, Encoding.UTF8);
            byte[] bHead = br.ReadBytes(120);
            int iKmInc = BitConverter.ToInt32(bHead, 100);
            int iChannelNumber = BitConverter.ToInt32(bHead, 116);

            byte[] bChannels = br.ReadBytes(65 * iChannelNumber);
            byte[] bData = new byte[iChannelNumber * 2];
            byte[] bDataNew = new byte[iChannelNumber * 2];
            byte[] bTail = br.ReadBytes(BitConverter.ToInt32(br.ReadBytes(4), 0));

            bw1.Write(bHead);
            bw1.BaseStream.Position = 4;
            string sVersion = "3.1.1";
            bw1.Write(sVersion);
            if (sVersion.Length < 20)
            {
                for (int i = 0; i < (20 - sVersion.Length); i++)
                {
                    bw1.Write((byte)0);
                }
            }
            bw1.BaseStream.Position = 120;
            bw1.Write(bChannels);
            bw1.Write(bTail.Length);
            bw1.Write(bTail);
            WaveformDataProcess wdp = new WaveformDataProcess();
            List<LostDataClass> ldc = wdp.GetLostData(sFile, "", false,true);
            while (br.BaseStream.Position < br.BaseStream.Length)
            {
                long lPosition = br.BaseStream.Position;

                bData = br.ReadBytes(iChannelNumber * 2);
                bw1.Write(bData);

                for (int i = 0; i < ldc.Count; i++)
                {
                    if (ldc[i].lStartPos == lPosition)
                    {
                        for (int j = 0; j < ldc[i].iCount; j++)
                        {
                            byte[] b = new byte[iChannelNumber * 2];
                            b = WaveformDataProcess.ByteXORByte(b);
                            bw1.Write(b);
                        }
                        break;
                    }
                }
            }
            bw1.Close();
            br.Close();
            fsWrite.Close();
            fsRead.Close();
            //删除bak
            Application.DoEvents();
            File.Delete(sFile);
            Application.DoEvents();
            File.Move(sFile + ".bak", sFile);
            Application.DoEvents();
        }
        #endregion
    }
    #endregion

    #region 波形数据处理类
    /// <summary>
    /// 波形数据处理类
    /// </summary>
    public class WaveformDataProcess
    {
        #region iic修正时使用的数据类
        /// <summary>
        /// iic修正时使用的数据类
        /// </summary>
        public class cPointFindMeter
        {
            /// <summary>
            /// 公里标：单位为厘米
            /// </summary>
            public long lMeter = 0;
            /// <summary>
            /// 里程对应的文件指针
            /// </summary>
            public long lLoc = 0;
        }
        #endregion

        //获得各个通道的比例

        #region 获得各个通道的比例
        /// <summary>
        /// 获得各个通道的比例
        /// </summary>
        /// <param name="filename">cit波形文件名</param>
        /// <param name="tds">通道数</param>
        /// <param name="f">通道比例</param>
        /// <param name="g">通道基线值</param>
        /// <returns>函数执行结果：true-成功；false-失败</returns>
        public bool GetChannelScale(string filename, int tds, ref float[] f, ref  float[] g)
        {
            try
            {
                FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(fs);
                byte[] b = new byte[tds * 2];
                //读取文件头
                br.ReadBytes(120);
                byte[] tdval = br.ReadBytes(65 * tds);
                //通道比例
                for (int l = 0; l < f.Length; l++)
                {
                    f[l] = BitConverter.ToSingle(tdval, 46 + l * 65);
                }

                //这里应该另外对f[1]赋值。严广学
                f[1] = 4;//有时为了加密数据。通道基线可能会设置成别的值。这里统一校正成4
                //通道基线值
                for (int l = 0; l < g.Length; l++)
                {
                    g[l] = BitConverter.ToSingle(tdval, 50 + l * 65);
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        #endregion
       
        #region 导出原始数据功能---截取波形片段功能调用
        /// <summary>
        /// 导出原始数据功能--截取波形片段控件类调用
        /// </summary>
        /// <param name="savepath"></param>
        /// <param name="StartPos"></param>
        /// <param name="EndPos"></param>
        /// <param name="isExport"></param>
        /// <param name="resultStr"></param>
        /// <param name="resultInt"></param>
        /// <param name="resultTDS"></param>
        /// <param name="riqi"></param>
        /// <param name="xm"></param>
        /// <param name="xb"></param>
        /// <param name="jcxh"></param>
        /// <param name="suffix"></param>
        /// <param name="bEncrypt"></param>
        /// <returns></returns>
        public bool ExportData(string savepath, long StartPos, long EndPos, bool[] isExport, string[] resultStr, int[] resultInt, int[] resultTDS, string riqi, string xm, string xb, string jcxh, string suffix, bool bEncrypt)
        {
            //Boolean isAcsend = true;//true:表示增里程；false：表示减里程
            //if (StartPos > EndPos)
            //{
            //    isAcsend = false;
            //} 

            try
            {
                if (!Directory.Exists(savepath))
                {
                    Directory.CreateDirectory(savepath);
                }
                //string[] datatype = new string[3] { "轨检", "动力学", "弓网" };
                //依次导出请求的数据
                for (int k = 0; k < resultStr.Length; k++)
                {
                    if (!isExport[k])
                    {
                        continue;
                    }
                    string FileName = resultStr[k];
                    FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    BinaryReader br = new BinaryReader(fs);
                    byte[] b = new byte[resultTDS[k] * 2];
                    long p = 0;
                    short gongli;
                    short mi1;
                    long s1;
                    long a1, a2;
                    //读取文件头
                    byte[] head = br.ReadBytes(120);
                    byte[] tdval = br.ReadBytes(65 * resultTDS[k]);
                    byte[] last4 = br.ReadBytes(4);
                    byte[] zh = br.ReadBytes(BitConverter.ToInt32(last4, 0));
                    p = br.BaseStream.Position;
                    br.BaseStream.Seek(p, SeekOrigin.Begin);
                    //取得开始位置
                    int x1 = 0;
                    int x2 = 2;
                    if (k == 1)
                    {
                        x1 = 2;
                        x2 = 4;
                    }
                    try
                    {
                        while (true)
                        {
                            a1 = br.BaseStream.Position;
                            b = br.ReadBytes(resultTDS[k] * 2);
                            if (bEncrypt)
                            {
                                b = WaveformDataProcess.ByteXORByte(b);
                            }
                            gongli = BitConverter.ToInt16(b, x1);
                            mi1 = BitConverter.ToInt16(b, x2);
                            s1 = gongli * 1000 + (long)(mi1 / resultInt[k]);
                            //if (isAcsend)
                            //{
                            //    if (s1 >= StartPos)
                            //    {
                            //        break;
                            //    }
                            //} 
                            //else
                            //{
                            //    if (s1 <= StartPos)
                            //    {
                            //        break;
                            //    }
                            //}
                            if (s1 == StartPos)
                            {
                                break;
                            }
                        }
                    }
                    catch// (ArgumentOutOfRangeException ex)
                    {
                        a1 = p;

                    }
                    br.BaseStream.Position = p;
                    //取得结束位置
                    try
                    {
                        while (true)
                        {
                            a2 = br.BaseStream.Position;
                            b = br.ReadBytes(resultTDS[k] * 2);
                            if (bEncrypt)
                            {
                                b = WaveformDataProcess.ByteXORByte(b);
                            }
                            gongli = BitConverter.ToInt16(b, x1);
                            mi1 = BitConverter.ToInt16(b, x2);
                            s1 = gongli * 1000 + (long)(mi1 / resultInt[k]);
                            //if (isAcsend)
                            //{
                            //    if (s1 >= EndPos)
                            //    {
                            //        break;
                            //    }
                            //} 
                            //else
                            //{
                            //    if (s1 <= EndPos)
                            //    {
                            //        break;
                            //    }
                            //}
                            if (s1 == EndPos)
                            {
                                break;
                            }

                        }
                    }
                    catch
                    {
                        a2 = br.BaseStream.Length - 1;
                    }
                    br.BaseStream.Seek(a1, SeekOrigin.Begin);
                    FileName = savepath + DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + xm + "_" + xb + "." + suffix;
                    FileStream fs1 = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.Read);
                    fs1.Write(head, 0, head.Length);
                    fs1.Write(tdval, 0, tdval.Length);
                    fs1.Write(last4, 0, last4.Length);
                    fs1.Write(zh, 0, zh.Length);
                    byte[] reb = new byte[resultTDS[k] * 2];
                    while (br.BaseStream.Position <= a2)
                    {
                        int c1 = br.Read(reb, 0, reb.Length);
                        fs1.Write(reb, 0, c1);
                    }
                    fs1.Flush();
                    fs1.Close();
                    fs.Close();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            return true;
        }
        #endregion


        #region 创建idf数据库里的所有数据表
        /// <summary>
        /// 创建idf数据库里的所有数据表
        /// </summary>
        /// <param name="sFilePath"></param>
        public void CreateTable(string sFilePath)
        {
            #region 创建LabelInfo表
            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFilePath + ";Persist Security Info=True"))
                {
                    string sqlCreate = "CREATE TABLE LabelInfo (" +
                        "Id integer primary key,MeterIndex varchar(255) NULL,Meter varchar(255) NULL," +
                        "MemoText varchar(255) NULL,LogDate date null) ;";
                    OleDbCommand sqlcom = new OleDbCommand(sqlCreate, sqlconn);
                    sqlconn.Open();
                    sqlcom.ExecuteNonQuery();
                    sqlconn.Close();
                }
            }
            catch
            {

            }
            #endregion

            #region 创建InvalidData
            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFilePath + ";Persist Security Info=True"))
                {
                    string sqlCreate = "CREATE TABLE InvalidData (" +
                        "Id integer ,StartPoint varchar(255) NULL,EndPoint varchar(255) NULL," +
                        "StartMile varchar(255) NULL,EndMile varchar(255) NULL,InvalidType integer," +
                        "MemoText varchar(255) NULL,IsShow integer NULL,ChannelType varchar(255) NULL);";
                    OleDbCommand sqlcom = new OleDbCommand(sqlCreate, sqlconn);
                    sqlconn.Open();
                    sqlcom.ExecuteNonQuery();
                    sqlconn.Close();
                }
            }
            catch
            {

            }
            #endregion

            #region 创建IndexOri
            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFilePath + ";Persist Security Info=True"))
                {
                    string sqlCreate = "CREATE TABLE IndexOri (" +
                        "Id integer primary key," +
                        "IndexId integer NULL," +
                        "IndexPoint varchar(255) NULL," +
                        "IndexMeter varchar(255) NULL);";
                    OleDbCommand sqlcom = new OleDbCommand(sqlCreate, sqlconn);
                    sqlconn.Open();
                    sqlcom.ExecuteNonQuery();
                    sqlconn.Close();
                }
            }
            catch
            {

            }
            #endregion

            #region 创建IndexSta
            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFilePath + ";Persist Security Info=True"))
                {
                    string sqlCreate = "CREATE TABLE IndexSta (" +
                        "Id integer primary key," +
                        "IndexId integer NULL," +
                        "StartPoint varchar(255) NULL," +
                        "StartMeter varchar(255) NULL," +
                        "EndPoint varchar(255) NULL," +
                        "EndMeter varchar(255) NULL," +
                        "ContainsPoint varchar(255) NULL," +
                        "ContainsMeter varchar(255) NULL," +
                        "IndexType varchar(255) NULL);";
                    OleDbCommand sqlcom = new OleDbCommand(sqlCreate, sqlconn);
                    sqlconn.Open();
                    sqlcom.ExecuteNonQuery();
                    sqlconn.Close();
                }
            }
            catch
            {

            }
            #endregion

            #region 创建AnalysisInfo
            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFilePath + ";Persist Security Info=True"))
                {
                    string sqlCreate = "CREATE TABLE AnalysisInfo (" +
                        "Id integer primary key," +
                        "LineCode varchar(255) NULL," +
                        "LineName varchar(255) NULL," +
                        "LineDir varchar(255) NULL," +
                        "DetectDate date NULL," +
                        "SecFlag varchar(255) NULL," +
                        "StartPoint varchar(255) NULL," +
                        "StartMile varchar(255) NULL," +
                        "EndPoint varchar(255) NULL," +
                        "EndMile varchar(255) NULL," +
                        "AnalySisType varchar(255) NULL," +
                        "ManageType varchar(255) NULL," +
                        "ImportFlag varchar(255) NULL," +
                        "Opdate date NULL," +
                        "CITName varchar(255) NULL," +
                        "MemoInfo varchar(255) NULL);";
                    OleDbCommand sqlcom = new OleDbCommand(sqlCreate, sqlconn);
                    sqlconn.Open();
                    sqlcom.ExecuteNonQuery();
                    sqlconn.Close();
                }
            }
            catch
            {

            }
            #endregion

            #region 创建AnalyAttach
            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFilePath + ";Persist Security Info=True"))
                {
                    string sqlCreate = "CREATE TABLE AnalyAttach (" +
                        "Id integer NULL," +
                        "AttachId integer primary key," +
                        "AnalysisDes varchar(255) NULL," +
                        "AttachType integer NULL," +
                        "AttachContent image NULL," +
                        "AttachFileName varchar(255) NULL," +
                        "AttachMemo varchar(255) NULL);";
                    OleDbCommand sqlcom = new OleDbCommand(sqlCreate, sqlconn);
                    sqlconn.Open();
                    sqlcom.ExecuteNonQuery();
                    sqlconn.Close();
                }
            }
            catch(Exception ex)
            {

            }
            #endregion

            #region 创建ChangeInfo
            CreatTableChangeInfo(sFilePath);
            #endregion

        }
        #endregion

        #region idf数据库操作：查询--LabelInfo表格
        /// <summary>
        /// idf数据库操作：查询--LabelInfo表格
        /// </summary>
        /// <param name="sFile"></param>
        /// <returns></returns>
        public List<LabelInfoClass> GetDataLabelInfo(string sFile)
        {
            List<LabelInfoClass> listIC = new List<LabelInfoClass>();
            try
            {

                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFile + ";Persist Security Info=False"))
                {
                    OleDbCommand sqlcom = new OleDbCommand("select * from LabelInfo", sqlconn);
                    sqlconn.Open();
                    OleDbDataReader sqloledr = sqlcom.ExecuteReader();
                    while (sqloledr.Read())
                    {
                        LabelInfoClass lic = new LabelInfoClass();
                        lic.iID = int.Parse(sqloledr.GetString(0));
                        lic.sMileIndex = sqloledr.GetString(1);
                        lic.sMile = sqloledr.GetString(2);
                        lic.sMemoText = sqloledr.GetString(2);
                        lic.logDate= sqloledr.GetString(2);

                        listIC.Add(lic);
                    }
                    sqlconn.Close();
                }

            }
            catch
            {

            }
            return listIC;
        }
        #endregion


        #region idf数据库操作：查询--InvalidData表格
        /// <summary>
        ///idf数据库操作：查询--InvalidData表格
        /// </summary>
        /// <param name="sFilePath"></param>
        /// <returns></returns>
        public List<InvalidDataClass> InvalidDataList(string sFilePath)
        {
            List<InvalidDataClass> listIDC = new List<InvalidDataClass>();
            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFilePath + ";Persist Security Info=True"))
                {
                    string sSql = "select * from InvalidData order by clng(StartPoint)";
                    OleDbCommand sqlcom = new OleDbCommand(sSql, sqlconn);
                    sqlconn.Open();
                    OleDbDataReader oleDBr = sqlcom.ExecuteReader();
                    int columnNum = oleDBr.FieldCount;
                    while (oleDBr.Read())
                    {
                        InvalidDataClass idc = new InvalidDataClass();
                        idc.iId = int.Parse(oleDBr.GetValue(0).ToString());
                        idc.sStartPoint = oleDBr.GetValue(1).ToString();
                        idc.sEndPoint = oleDBr.GetValue(2).ToString();
                        idc.sStartMile = oleDBr.GetValue(3).ToString();
                        idc.sEndMile = oleDBr.GetValue(4).ToString();
                        idc.iType = int.Parse(oleDBr.GetValue(5).ToString());
                        idc.sMemoText = oleDBr.GetValue(6).ToString();
                        idc.iIsShow = int.Parse(oleDBr.GetValue(7).ToString());
                        if (columnNum == 9)
                        {
                            idc.ChannelType = oleDBr.GetValue(8).ToString();
                        } 
                        else
                        {
                            idc.ChannelType = "";
                        }

                        listIDC.Add(idc);
                    }
                    oleDBr.Close();
                    sqlconn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("无效区段读取异常:" + ex.Message);
            }
            return listIDC;
        }
        #endregion

        #region idf数据库操作：删除--InvalidData表格的一条数据
        /// <summary>
        /// idf数据库操作：删除--InvalidData表格的一条数据
        /// </summary>
        /// <param name="sFilePath"></param>
        /// <param name="sStartPoint"></param>
        /// <param name="sEndPoint"></param>
        public void InvalidDataDelete(string sFilePath, string sStartPoint, string sEndPoint)
        {
            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFilePath + ";Persist Security Info=True"))
                {
                    string sSql = "delete from InvalidData where StartPoint='" + sStartPoint + "' and EndPoint='" + sEndPoint + "'";
                    OleDbCommand sqlcom = new OleDbCommand(sSql, sqlconn);
                    sqlconn.Open();
                    sqlcom.ExecuteNonQuery();
                    sqlconn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("无效区段设置异常:" + ex.Message);
            }
        }
        #endregion

        #region idf数据库操作：修改--一条InvalidData表格的数据
        /// <summary>
        /// idf数据库操作：修改--一条InvalidData表格的数据
        /// </summary>
        /// <param name="sFilePath"></param>
        /// <param name="sStartPoint"></param>
        /// <param name="sEndPoint"></param>
        /// <param name="sStartMile"></param>
        /// <param name="sEndMile"></param>
        public void InvalidDataUpdate(string sFilePath, string sStartPoint, string sEndPoint, string sStartMile, string sEndMile)
        {
            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFilePath + ";Persist Security Info=True"))
                {
                    string sSql = "update InvalidData set StartMile='" +
                        sStartMile + "',EndMile='" +
                        sEndMile + "' where StartPoint='" +
                        sStartPoint + "' and EndPoint='" + sEndPoint + "'";
                    OleDbCommand sqlcom = new OleDbCommand(sSql, sqlconn);
                    sqlconn.Open();
                    sqlcom.ExecuteNonQuery();
                    sqlconn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("无效区段设置异常:" + ex.Message);
            }
        }
        #endregion

        #region idf数据库操作：新增--一条InvalidData表格的数据
        /// <summary>
        /// idf数据库操作：新增--一条InvalidData表格的数据
        /// </summary>
        /// <param name="sFilePath"></param>
        /// <param name="sStartPoint"></param>
        /// <param name="sEndPoint"></param>
        /// <param name="sStartMile"></param>
        /// <param name="sEndMile"></param>
        /// <param name="iType"></param>
        /// <param name="sMemo"></param>
        public void InvalidDataInsertInto(string sFilePath, string sStartPoint, string sEndPoint, string sStartMile, string sEndMile, int iType, string sMemo)
        {
            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFilePath + ";Persist Security Info=True"))
                {
                    string sSql = "insert into InvalidData values(" + DateTime.Now.ToString("yyyyMMdd") + ",'" + sStartPoint +
                        "','" + sEndPoint + "','" + sStartMile + "','" + sEndMile + "'," + iType.ToString() + ",'" + sMemo + "',0,'')";
                    OleDbCommand sqlcom = new OleDbCommand(sSql, sqlconn);
                    sqlconn.Open();
                    sqlcom.ExecuteNonQuery();
                    sqlconn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("无效区段设置异常:" + ex.Message);
            }
        }
        public void InvalidDataInsertInto(string sFilePath, string sStartPoint, string sEndPoint, string sStartMile, string sEndMile, int iType, string sMemo, string Type)
        {
            int id = 0;//无效区段id
            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFilePath + ";Persist Security Info=True"))
                {
                    sqlconn.Open();

                    String sSql = "select max(Id) from InvalidData";
                    OleDbCommand sqlcom = new OleDbCommand(sSql, sqlconn);
                    OleDbDataReader oledbReader = sqlcom.ExecuteReader();
                    Boolean isNull = oledbReader.HasRows;//是否是第一条记录，第一条记录id为1；
                    if (isNull == false)
                    {
                        id = 1;
                    }
                    else
                    {
                        while (oledbReader.Read())
                        {
                            if (String.IsNullOrEmpty(oledbReader.GetValue(0).ToString()))
                            {
                                id = 1;
                            }
                            else
                            {
                                id = int.Parse(oledbReader.GetValue(0).ToString()) + 1;
                            }
                        }
                    }

                    sqlconn.Close();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("获取无效区段id异常：" + ex.Message);
            }

            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFilePath + ";Persist Security Info=True"))
                {
                    string sSql = "insert into InvalidData values(" + id.ToString() + ",'" + sStartPoint +
                        "','" + sEndPoint + "','" + sStartMile + "','" + sEndMile + "'," + iType.ToString() + ",'" + sMemo + "',0,'" + Type + "')";
                    OleDbCommand sqlcom = new OleDbCommand(sSql, sqlconn);
                    sqlconn.Open();
                    sqlcom.ExecuteNonQuery();
                    sqlconn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("无效区段设置异常:" + ex.Message);
            }
        }
        #endregion

        #region 如果有需要，把无效数据表格新增一列：通道类型---20140106--ygx
        /// <summary>
        /// 如果有需要，把无效数据表格新增一列：通道类型
        /// </summary>
        /// <param name="sFilePath">idf文件名</param>
        public void InvalidDataOldToNew(String sFilePath)
        {
            List<InvalidDataClass> listIDC = new List<InvalidDataClass>();
            Boolean oldToNew = false;//是否需要转换成新的表--比原来的多一列：通道类型
            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFilePath + ";Persist Security Info=True"))
                {
                    string sSql = "select * from InvalidData order by clng(StartPoint)";
                    OleDbCommand sqlcom = new OleDbCommand(sSql, sqlconn);
                    sqlconn.Open();
                    OleDbDataReader oleDBr = sqlcom.ExecuteReader();

                    int columnNum = oleDBr.FieldCount;
                    //老表需要转换为新的表
                    if (columnNum == 8)
                    {
                        oldToNew = true;
                    }

                    while (oleDBr.Read())
                    {
                        InvalidDataClass idc = new InvalidDataClass();
                        idc.iId = int.Parse(oleDBr.GetValue(0).ToString());
                        idc.sStartPoint = oleDBr.GetValue(1).ToString();
                        idc.sEndPoint = oleDBr.GetValue(2).ToString();
                        idc.sStartMile = oleDBr.GetValue(3).ToString();
                        idc.sEndMile = oleDBr.GetValue(4).ToString();
                        idc.iType = int.Parse(oleDBr.GetValue(5).ToString());
                        idc.sMemoText = oleDBr.GetValue(6).ToString();
                        idc.iIsShow = int.Parse(oleDBr.GetValue(7).ToString());
                        if (columnNum == 9)
                        {
                            idc.ChannelType = oleDBr.GetValue(8).ToString();
                        }
                        else
                        {
                            idc.ChannelType = "手工标识";
                        }

                        listIDC.Add(idc);
                    }

                    oleDBr.Close();
                    sqlconn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("无效区段读取异常:" + ex.Message);
            }
            //先把老表数据备份，然后删除，创建新表。
            if (oldToNew)
            {
                try
                {
                    using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFilePath + ";Persist Security Info=True"))
                    {
                        sqlconn.Open();

                        string sSql = "drop table InvalidData";
                        OleDbCommand sqlcom = new OleDbCommand(sSql, sqlconn);
                        sqlcom.ExecuteNonQuery();

                        sSql = "CREATE TABLE InvalidData (" +
"Id integer ,StartPoint varchar(255) NULL,EndPoint varchar(255) NULL," +
"StartMile varchar(255) NULL,EndMile varchar(255) NULL,InvalidType integer," +
"MemoText varchar(255) NULL,IsShow integer NULL,ChannelType varchar(255) NULL);";
                        sqlcom = new OleDbCommand(sSql, sqlconn);
                        sqlcom.ExecuteNonQuery();

                        foreach (InvalidDataClass idCls in listIDC)
                        {
                            sSql = "insert into InvalidData values(" + idCls.iId + ",'" + idCls.sStartPoint +
                        "','" + idCls.sEndPoint + "','" + idCls.sStartMile + "','" + idCls.sEndMile + "'," + idCls.iType + ",'" + idCls.sMemoText + "',0,'"+idCls.ChannelType+"')";
                            sqlcom = new OleDbCommand(sSql, sqlconn);
                            sqlcom.ExecuteNonQuery();
                        }

                        sqlconn.Close();
                    }

                    listIDC.Clear();
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("无效区段新增列异常:" + ex.Message);
                    listIDC.Clear();
                }
            }
            else
            {
                listIDC.Clear();
            }
        }
        #endregion

        #region 动态创建数据库---idf文件
        /// <summary>
        /// 动态创建数据库---idf文件
        /// </summary>
        /// <param name="sFilePath"></param>
        public void CreateDB(string sFilePath)
        {
            string ConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFilePath + ";Jet OLEDB:Engine Type=5";
            // Create a Catalog object
            //MessageBox.Show(ConnectionString);
            ADOX.Catalog ct = new ADOX.Catalog();
            try
            {
                ct.Create(ConnectionString);
            }
            catch
            {
            }
            ct = null;
            Application.DoEvents();
        }
        #endregion

        #region idf数据库操作：新增--一条IndexSta表格的数据
        /// <summary>
        /// 根据标定的里程特征点信息创建索引,并插入到文件idf中的IndexSta的表格
        /// </summary>
        /// <param name="sFileName">cit文件全路径名</param>
        /// <param name="sFilePath">与cit文件同名的idf文件</param>
        /// <param name="sID"></param>
        /// <param name="listCDL">长短链信息</param>
        /// <param name="sKmInc">公里标增减标志</param>
        /// <param name="iChannelNumber">通道数</param>
        /// <param name="iSmaleRate">采样频率</param>
        /// <returns></returns>
        public int CreateIndexInfo(string sFileName, string sFilePath, string sID, List<CDLClass> listCDL, string sKmInc, int iChannelNumber, float iSmaleRate)
        {
            long lStartPosition = 0;
            long lEndPosition = 0;
            GetDataStartPositionEndPositionInfoIncludeIndex(ref lStartPosition, ref lEndPosition, sFileName, iChannelNumber, -1, -1, false);
            //删除已创建的索引
            using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFilePath + ";Persist Security Info=True"))
            {
                string sSql = "delete from IndexSta";
                OleDbCommand sqlcom = new OleDbCommand(sSql, sqlconn);
                sqlconn.Open();
                sqlcom.ExecuteNonQuery();
                sqlconn.Close();
            }

            //获取原始索引信息
            List<IndexOriClass> listIOC = GetLayerIndexInfo(sFilePath, sID, sKmInc);

            if (listIOC.Count < 1)
            {
                return -20;
            }

            List<IndexStaClass> listIC = new List<IndexStaClass>();
            List<IndexStaClass> listProIC = new List<IndexStaClass>();

            int iID = 0;
            #region 处理索引
            if (sKmInc.Equals("增"))
            {
                for (int i = 0; i < listIOC.Count - 1; i++)
                {
                    iID++;
                    IndexStaClass ic = new IndexStaClass();

                    ic.iID = iID;
                    ic.iIndexID = int.Parse(sID);

                    ic.lStartPoint = long.Parse(listIOC[i].IndexPoint);
                    ic.lStartMeter = listIOC[i].IndexMeter;
                    ic.lEndPoint = long.Parse(listIOC[i + 1].IndexPoint);
                    ic.LEndMeter = listIOC[i + 1].IndexMeter;
                    ic.lContainsPoint = ((long.Parse(listIOC[i + 1].IndexPoint) - long.Parse(listIOC[i].IndexPoint)) / (iChannelNumber * 2));

                    int iCDLCount = 0;
                    int iCDLSumC = 0;
                    int iCDLSumD = 0;
                    for (int j = 0; j < listCDL.Count; j++)
                    {
                        if ((float.Parse(ic.lStartMeter) <= listCDL[j].dKM) &&
                            (float.Parse(ic.LEndMeter) > listCDL[j].dKM) && listCDL[j].sType.Equals("长链"))
                        {
                            iCDLCount++;
                            iCDLSumC += listCDL[j].iMeter;
                        }
                        if ((float.Parse(ic.lStartMeter) <= listCDL[j].dKM) &&
                            (float.Parse(ic.LEndMeter) > listCDL[j].dKM) && listCDL[j].sType.Equals("短链"))
                        {
                            iCDLSumD += listCDL[j].iMeter;
                        }
                    }
                    int iCDLMeter = 0;
                    iCDLMeter = iCDLSumC - (iCDLCount * 1000);
                    iCDLMeter = iCDLMeter - iCDLSumD;
                    ic.lContainsMeter = (float.Parse(listIOC[i + 1].IndexMeter) - float.Parse(listIOC[i].IndexMeter) + iCDLMeter / 1000.0).ToString("f3");

                    ic.sType = "正常";
                    listIC.Add(ic);
                }
            }
            else//减里程
            {
                for (int i = 0; i < listIOC.Count - 1; i++)
                {
                    iID++;
                    IndexStaClass ic = new IndexStaClass();

                    ic.iID = iID;
                    ic.iIndexID = int.Parse(sID);

                    ic.lStartPoint = long.Parse(listIOC[i].IndexPoint);
                    ic.lStartMeter = listIOC[i].IndexMeter;
                    ic.lEndPoint = long.Parse(listIOC[i + 1].IndexPoint);
                    ic.LEndMeter = listIOC[i + 1].IndexMeter;
                    ic.lContainsPoint = ((long.Parse(listIOC[i + 1].IndexPoint) - long.Parse(listIOC[i].IndexPoint)) / (iChannelNumber * 2));

                    int iCDLCount = 0;
                    int iCDLSumC = 0;
                    int iCDLSumD = 0;
                    for (int j = 0; j < listCDL.Count; j++)
                    {
                        if ((float.Parse(ic.lStartMeter) >= listCDL[j].dKM) &&
                            (float.Parse(ic.LEndMeter) < listCDL[j].dKM) && listCDL[j].sType.Equals("长链"))
                        {
                            iCDLCount++;
                            iCDLSumC += listCDL[j].iMeter;
                        }
                        if ((float.Parse(ic.lStartMeter) >= listCDL[j].dKM) &&
                            (float.Parse(ic.LEndMeter) < listCDL[j].dKM) && listCDL[j].sType.Equals("短链"))
                        {
                            iCDLSumD += listCDL[j].iMeter;

                        }
                    }
                    int iCDLMeter = 0;
                    iCDLMeter = iCDLSumC - (iCDLCount * 1000);
                    iCDLMeter = iCDLMeter - iCDLSumD;
                    ic.lContainsMeter = (float.Parse(listIOC[i].IndexMeter) - float.Parse(listIOC[i + 1].IndexMeter) + iCDLMeter / 1000.0).ToString("f3");

                    ic.sType = "正常";
                    listIC.Add(ic);
                }
            }
            #endregion
            //
            //处理长短链
            #region 处理长短链
            int iConut = 0;
            //listProIC = listIC;
            if (sKmInc.Equals("增"))
            {
                for (int k = 0; k < listIC.Count; k++)
                {
                    //判断每个索引区段内是否包含CDL
                    bool bCDL = false;

                    IndexStaClass ic3 = new IndexStaClass();
                    ic3 = listIC[k];
                    for (int j = 0; j < listCDL.Count; j++)
                    {

                        IndexStaClass ic1 = new IndexStaClass();
                        IndexStaClass ic2 = new IndexStaClass();

                        if ((float.Parse(ic3.lStartMeter) <= listCDL[j].dKM) &&
                            (float.Parse(ic3.LEndMeter) > listCDL[j].dKM))
                        {
                            bCDL = true;

                            //CDL前面
                            float iMeterLength = listCDL[j].dKM - float.Parse(ic3.lStartMeter);
                            iConut++;
                            ic1.iID = iConut;
                            ic1.iIndexID = ic3.iIndexID;
                            ic1.lStartPoint = ic3.lStartPoint;
                            ic1.lStartMeter = ic3.lStartMeter;
                            ic1.lContainsMeter = iMeterLength.ToString();
                            ic1.lContainsPoint =
                                (int)(Math.Ceiling((iMeterLength * 1000) /
                                ((float.Parse(listIC[k].lContainsMeter) * 1000) / listIC[k].lContainsPoint)));
                            //- (int)(ic3.dSmaleRat * 1000)
                            ic1.LEndMeter = listCDL[j].dKM.ToString();
                            ic1.lEndPoint = ic1.lStartPoint + ic1.lContainsPoint * iChannelNumber * 2;
                            ic1.sType = "正常";
                            listProIC.Add(ic1);


                            //CDL
                            iConut++;
                            if (listCDL[j].sType == "短链")
                            {
                                //短链信息分2段处理，长链信息分3段处理，其总长度没有损失---严广学
                            }
                            else
                            {
                                ic2.iIndexID = ic3.iIndexID;
                                ic2.iID = iConut;
                                ic2.lStartPoint = ic1.lEndPoint;
                                ic2.lStartMeter = listCDL[j].dKM.ToString();
                                ic2.lContainsMeter = (listCDL[j].iMeter / 1000.0).ToString("f3");
                                ic2.lContainsPoint = (int)(Math.Ceiling(listCDL[j].iMeter /
                                    ((float.Parse(listIC[k].lContainsMeter) * 1000) / listIC[k].lContainsPoint)));

                                ic2.lEndPoint = ic2.lStartPoint + ic2.lContainsPoint * iChannelNumber * 2;
                                ic2.LEndMeter = (listCDL[j].dKM + (listCDL[j].iMeter / 1000.0)).ToString();

                                ic2.sType = "长链";

                                listProIC.Add(ic2);
                            }

                            if (listCDL[j].sType == "短链")
                            {
                                ic3.lStartMeter = (listCDL[j].dKM + listCDL[j].iMeter / 1000.0).ToString();
                                ic3.lStartPoint = ic1.lEndPoint;
                                ic3.lContainsMeter = (float.Parse(ic3.lContainsMeter) -
                                    float.Parse(ic1.lContainsMeter)).ToString();
                                ic3.lContainsPoint = ic3.lContainsPoint - ic1.lContainsPoint;
                                ic3.sType = "正常";
                            }
                            else
                            {
                                ic3.lStartMeter = (listCDL[j].dKM + 1).ToString();
                                ic3.lStartPoint = ic2.lEndPoint;
                                ic3.lContainsMeter = (float.Parse(ic3.lContainsMeter) -
                                    float.Parse(ic2.lContainsMeter) - float.Parse(ic1.lContainsMeter)).ToString();
                                ic3.lContainsPoint = ic3.lContainsPoint - ic2.lContainsPoint - ic1.lContainsPoint;
                                ic3.sType = "正常";
                            }

                        }

                    }
                    //判断是否进行了长短链修正，没有则添加
                    if (!bCDL)
                    {
                        iConut++;
                        listIC[k].iID = iConut;
                        listProIC.Add(listIC[k]);
                    }
                    else
                    {
                        iConut++;
                        ic3.iID = iConut;
                        listProIC.Add(ic3);
                    }


                }

            }
            else//减里程
            {
                for (int k = 0; k < listIC.Count; k++)
                {
                    //判断每个索引区段内是否包含CDL
                    bool bCDL = false;

                    IndexStaClass ic3 = new IndexStaClass();
                    ic3 = listIC[k];
                    for (int j = 0; j < listCDL.Count; j++)
                    {

                        IndexStaClass ic1 = new IndexStaClass();
                        IndexStaClass ic2 = new IndexStaClass();

                        if ((float.Parse(ic3.lStartMeter) >= listCDL[j].dKM) &&
                            (float.Parse(ic3.LEndMeter) < listCDL[j].dKM))
                        {
                            bCDL = true;
                            if (listCDL[j].sType == "长链")
                            {
                                //CDL前面
                                float iMeterLength = float.Parse(ic3.lStartMeter) - listCDL[j].dKM - 1;
                                iConut++;
                                ic1.iID = iConut;
                                ic1.iIndexID = ic3.iIndexID;
                                ic1.lStartPoint = ic3.lStartPoint;
                                ic1.lStartMeter = ic3.lStartMeter;
                                ic1.lContainsPoint =
                                    (int)(Math.Ceiling((iMeterLength * 1000) /
                                    ((float.Parse(listIC[k].lContainsMeter) * 1000) / listIC[k].lContainsPoint)));
                                //- (int)(ic3.dSmaleRat * 1000)
                                ic1.LEndMeter = (listCDL[j].dKM + 1).ToString("f3");
                                ic1.lContainsMeter = (float.Parse(ic1.lStartMeter) - float.Parse(ic1.LEndMeter)).ToString("f3");
                                ic1.lEndPoint = ic1.lStartPoint + ic1.lContainsPoint * iChannelNumber * 2;
                                ic1.sType = "正常";
                                listProIC.Add(ic1);
                            }
                            else
                            {
                                float iMeterLength = float.Parse(ic3.lStartMeter) - listCDL[j].dKM - listCDL[j].iMeter / 1000f;
                                iConut++;
                                ic1.iID = iConut;
                                ic1.iIndexID = ic3.iIndexID;
                                ic1.lStartPoint = ic3.lStartPoint;
                                ic1.lStartMeter = ic3.lStartMeter;
                                ic1.lContainsPoint =
                                    (int)(Math.Ceiling((iMeterLength * 1000) /
                                    ((float.Parse(listIC[k].lContainsMeter) * 1000) / listIC[k].lContainsPoint)));
                                //- (int)(ic3.dSmaleRat * 1000)
                                ic1.LEndMeter = (listCDL[j].dKM + listCDL[j].iMeter / 1000f).ToString("f3");
                                ic1.lContainsMeter = (float.Parse(ic1.lStartMeter) - float.Parse(ic1.LEndMeter)).ToString("f3");
                                ic1.lEndPoint = ic1.lStartPoint + ic1.lContainsPoint * iChannelNumber * 2;
                                ic1.sType = "正常";
                                listProIC.Add(ic1);
                            }
                            //CDL
                            iConut++;
                            if (listCDL[j].sType == "短链")
                            {

                            }
                            else
                            {
                                ic2.iIndexID = ic3.iIndexID;
                                ic2.iID = iConut;
                                ic2.lStartPoint = ic1.lEndPoint;
                                ic2.lStartMeter = (listCDL[j].dKM + (listCDL[j].iMeter / 1000f)).ToString("f3");
                                ic2.lContainsMeter = (listCDL[j].iMeter / 1000.0).ToString("f3");
                                ic2.lContainsPoint = (int)(Math.Ceiling(listCDL[j].iMeter /
                                    ((float.Parse(listIC[k].lContainsMeter) * 1000) / listIC[k].lContainsPoint)));

                                ic2.lEndPoint = ic2.lStartPoint + ic2.lContainsPoint * iChannelNumber * 2;
                                ic2.LEndMeter = (listCDL[j].dKM).ToString();
                                ic2.sType = "长链";
                                listProIC.Add(ic2);
                            }


                            if (listCDL[j].sType == "短链")
                            {
                                ic3.lStartMeter = (listCDL[j].dKM).ToString();
                                ic3.lStartPoint = ic1.lEndPoint;
                                //ic3.lContainsMeter = (float.Parse(ic3.lStartMeter) - float.Parse(ic3.LEndMeter)).ToString("f3");
                                ic3.lContainsMeter = (float.Parse(ic3.lContainsMeter) -
                                    float.Parse(ic1.lContainsMeter)).ToString();
                                ic3.lContainsPoint = ic3.lContainsPoint - ic1.lContainsPoint;
                                ic3.sType = "正常";
                            }
                            else
                            {
                                ic3.lStartMeter = (listCDL[j].dKM).ToString();
                                ic3.lStartPoint = ic2.lEndPoint;
                                ic3.lContainsMeter = (float.Parse(ic3.lContainsMeter) -
                                    float.Parse(ic2.lContainsMeter) - float.Parse(ic1.lContainsMeter)).ToString();
                                ic3.lContainsPoint = ic3.lContainsPoint - ic2.lContainsPoint - ic1.lContainsPoint;
                                ic3.sType = "正常";
                            }
                        }

                    }
                    //判断是否进行了长短链修正，没有则添加
                    if (!bCDL)
                    {
                        iConut++;
                        listIC[k].iID = iConut;
                        listProIC.Add(listIC[k]);
                    }
                    else
                    {
                        iConut++;
                        ic3.iID = iConut;
                        listProIC.Add(ic3);
                    }


                }
            }
            # endregion
            //插入头尾点
            if (listProIC.Count >= 1)
            {
                //补头
                IndexStaClass ic1 = new IndexStaClass();
                ic1.iIndexID = 1;
                ic1.sType = "正常";
                ic1.lEndPoint = listProIC[0].lStartPoint;
                ic1.LEndMeter = listProIC[0].lStartMeter;
                ic1.lStartPoint = lStartPosition;
                ic1.lContainsPoint = (listProIC[0].lStartPoint - ic1.lStartPoint) / 2 / iChannelNumber;
                ic1.lContainsMeter = (ic1.lContainsPoint / iSmaleRate / 1000).ToString("f3");
                ic1.lStartMeter = sKmInc.Equals("增") ? (double.Parse(ic1.LEndMeter) - double.Parse(ic1.lContainsMeter)).ToString("f3") : (double.Parse(ic1.LEndMeter) + double.Parse(ic1.lContainsMeter)).ToString("f3");
                listProIC.Insert(0, ic1);
                //补尾
                IndexStaClass ic2 = new IndexStaClass();
                ic2.iIndexID = 1;
                ic2.sType = "正常";
                ic2.lEndPoint = lEndPosition;
                ic2.lStartPoint = listProIC[listProIC.Count - 1].lEndPoint;
                ic2.lStartMeter = listProIC[listProIC.Count - 1].LEndMeter;
                ic2.lContainsPoint = (lEndPosition - ic2.lStartPoint) / 2 / iChannelNumber;
                ic2.lContainsMeter = (ic2.lContainsPoint / iSmaleRate / 1000).ToString("f3");
                ic2.LEndMeter = sKmInc.Equals("增") ? (double.Parse(ic2.lStartMeter) + double.Parse(ic2.lContainsMeter)).ToString("f3") : (double.Parse(ic2.lStartMeter) - double.Parse(ic2.lContainsMeter)).ToString("f3");
                listProIC.Add(ic2);
                //重新排序
                for (int i = 0; i < listProIC.Count; i++)
                {
                    listProIC[i].iID = i + 1;
                }
            }
            //保存
            for (int i = 0; i < listProIC.Count; i++)
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFilePath + ";Persist Security Info=True"))
                {
                    string sSql = "insert into IndexSta values(" + listProIC[i].iID.ToString() + "," + listProIC[i].iIndexID.ToString() +
                        ",'" + listProIC[i].lStartPoint.ToString() + "','" + listProIC[i].lStartMeter.ToString() +
                        "','" + listProIC[i].lEndPoint.ToString() + "','" + listProIC[i].LEndMeter.ToString() + "','" +
                        listProIC[i].lContainsPoint.ToString() + "','" + listProIC[i].lContainsMeter.ToString() +
                        "','" + listProIC[i].sType + "')";
                    OleDbCommand sqlcom = new OleDbCommand(sSql, sqlconn);
                    sqlconn.Open();
                    sqlcom.ExecuteNonQuery();
                }
            }


            //MessageBox.Show("创建成功！");
            return 0;

        }
        #endregion

        #region  idf数据库操作：查询--IndexSta表格
        /// <summary>
        ///  idf数据库操作：查询--IndexSta表格
        /// </summary>
        /// <param name="sFile">idf全路径文件名</param>
        /// <returns>修正索引信息</returns>
        public List<IndexStaClass> GetDataIndexInfo(string sFile)
        {
            List<IndexStaClass> listIC = new List<IndexStaClass>();

            try
            {

                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFile + ";Persist Security Info=False"))
                {
                    OleDbCommand sqlcom = new OleDbCommand("select * from IndexSta order by id", sqlconn);
                    sqlconn.Open();
                    OleDbDataReader sqloledr = sqlcom.ExecuteReader();
                    while (sqloledr.Read())
                    {
                        IndexStaClass ic = new IndexStaClass();
                        ic.iID = (int)sqloledr.GetInt32(0);
                        ic.iIndexID = (int)sqloledr.GetInt32(1);
                        ic.lStartPoint = long.Parse(sqloledr.GetString(2));
                        ic.lStartMeter = sqloledr.GetString(3);
                        ic.lEndPoint = long.Parse(sqloledr.GetString(4));
                        ic.LEndMeter = sqloledr.GetString(5);
                        ic.lContainsPoint = long.Parse(sqloledr.GetString(6));
                        ic.lContainsMeter = sqloledr.GetString(7);
                        ic.sType = sqloledr.GetString(8);

                        listIC.Add(ic);
                    }
                    sqlconn.Close();
                }

            }
            catch
            {

            }

            return listIC;
        }
        #endregion

        #region idf数据库表查询--获取所有的表格名---ygx--20140320
        /// <summary>
        /// idf数据库表查询--获取所有的表格名
        /// </summary>
        /// <param name="idfFilePath">idf文件名</param>
        /// <returns>所有的表名</returns>
        public  List<String> GetTableNames(String idfFilePath)
        {
            List<String> ret = new List<String>();
            using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + idfFilePath + ";Persist Security Info=True"))
            {
                sqlconn.Open();

                DataTable dt = sqlconn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    ret.Add(dt.Rows[i]["TABLE_NAME"].ToString());
                }

                sqlconn.Close();

            }

            return ret;
        }
        #endregion

        #region idf数据库操作：创建表格--ChangeInfo（微小变化信息表）----ygx--20150407
        /// <summary>
        /// idf数据库操作：创建表格--ChangeInfo（微小变化信息表）
        /// </summary>
        /// <param name="idfFilePath">idf文件名（全路径）</param>
        public void CreatTableChangeInfo(String idfFilePath)
        {
            List<String> tableNames = GetTableNames(idfFilePath);
            if (!tableNames.Contains("ChangeInfo"))
            {
                #region 创建ChangeInfo
                try
                {
                    using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + idfFilePath + ";Persist Security Info=True"))
                    {
                        string sqlCreate = "CREATE TABLE ChangeInfo (" +
                            "Id integer primary key," +
                            "对比的文件名 varchar(255) NULL," +
                            "通道名 varchar(255) NULL," +
                            "起始里程 varchar(255) NULL," +
                            "结束里程 varchar(255) NULL," +
                            "幅值绝对值差 varchar(255) NULL);";
                        OleDbCommand sqlcom = new OleDbCommand(sqlCreate, sqlconn);
                        sqlconn.Open();
                        sqlcom.ExecuteNonQuery();
                        sqlconn.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("创建ChangeInfo表格出错：" + ex.Message);
                }
                #endregion
            }
        }
        #endregion

        #region idf数据库操作：创建表格--UnitInfo（单元信息表）----ygx--20151021
        /// <summary>
        /// idf数据库操作：创建表格--ChangeInfo（微小变化信息表）
        /// </summary>
        /// <param name="idfFilePath">idf文件名（全路径）</param>
        public void CreatTableUnitInfo(String idfFilePath)
        {
            List<String> tableNames = GetTableNames(idfFilePath);
            if (tableNames == null || !tableNames.Contains("UnitInfo"))
            {
                #region 创建单元信息表
                try
                {
                    using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + idfFilePath + ";Persist Security Info=True"))
                    {
                        string sqlCreate = "CREATE TABLE UnitInfo (" +
                            "Id integer NULL," +
                            "UnitId varchar(255) NULL," +
                            "UnitName varchar(255) NULL," +
                            "LineName varchar(255) NULL," +
                            "LineDir varchar(255) NULL," +
                            "UnitStart varchar(255) NULL," +
                            "UnitEnd varchar(255) NULL," +
                            "UnitType varchar(255) NULL," +
                            "CalcTime varchar(255) NULL," +
                            "UnitLevel varchar(255) NULL," +
                            "PictureContent image NULL," +
                            "PictureType varchar(255) NULL);";

                        OleDbCommand sqlcom = new OleDbCommand(sqlCreate, sqlconn);
                        sqlconn.Open();
                        sqlcom.ExecuteNonQuery();
                        sqlconn.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("创建UnitInfo表格出错：" + ex.Message);
                    
                }
                #endregion
            }
        }
        #endregion


        #region idf数据库操作：查询--表格中数据的id---ygx--20150407
        /// <summary>
        /// idf数据库操作：查询--表格中数据的id
        /// </summary>
        /// <param name="sFilePath">idf文件名（全路径）</param>
        /// <param name="tableName">表格名</param>
        /// <returns>id</returns>
        public int GetIdInTableFromIdf(String sFilePath, String tableName)
        {
            #region 获取表格中数据的id

            int id = 0;

            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFilePath + ";Persist Security Info=True"))
                {
                    sqlconn.Open();

                    String sSql = "select max(Id) from " + tableName;
                    OleDbCommand sqlcom = new OleDbCommand(sSql, sqlconn);
                    OleDbDataReader oledbReader = sqlcom.ExecuteReader();
                    Boolean isNull = oledbReader.HasRows;//是否是第一条记录，第一条记录id为1；
                    if (isNull == false)
                    {
                        id = 1;
                    }
                    else
                    {
                        while (oledbReader.Read())
                        {
                            if (String.IsNullOrEmpty(oledbReader.GetValue(0).ToString()))
                            {
                                id = 1;
                            }
                            else
                            {
                                id = int.Parse(oledbReader.GetValue(0).ToString()) + 1;
                            }
                        }
                    }

                    sqlconn.Close();

                    return id;
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("获取id异常：" + ex.Message);
                return id;
               
            }
            #endregion
        }
        #endregion


        #region idf数据库操作：删除--全部或是一条IndexOri表格的数据
        #region 删除cit文件同名的数据库中的索引表(IndexOri)的一条数据
        /// <summary>
        /// 删除cit文件同名的数据库中的索引表(IndexOri)的一条数据
        /// </summary>
        /// <param name="sFilePath">cit文件同名的数据库的全路径文件名</param>
        /// <param name="sID">索引id</param>
        /// <returns></returns>
        public int deleteLayerIndexInfo(string sFilePath, string sID)
        {

            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFilePath + ";Persist Security Info=True"))
                {
                    string sqlCreate = "delete from IndexOri where id=" + sID;
                    OleDbCommand sqlcom = new OleDbCommand(sqlCreate, sqlconn);
                    sqlconn.Open();
                    sqlcom.ExecuteNonQuery();

                    sqlconn.Close();
                }
            }
            catch
            {

            }

            return 0;

        }
        #endregion

        #region 删除cit文件同名的数据库中的索引表(IndexOri)的全部数据
        /// <summary>
        /// 删除cit文件同名的数据库中的索引表(IndexOri)的全部数据
        /// </summary>
        /// <param name="sFilePath">cit文件同名的数据库的全路径文件</param>
        /// <returns></returns>
        public int deleteLayerIndexInfo(string sFilePath)
        {
            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFilePath + ";Persist Security Info=True"))
                {
                    string sqlCreate = "delete from IndexOri";
                    OleDbCommand sqlcom = new OleDbCommand(sqlCreate, sqlconn);
                    sqlconn.Open();
                    sqlcom.ExecuteNonQuery();
                    sqlconn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return 0;
        }
        #endregion
        #endregion

        #region idf数据库操作：插入--在第一行或是其他位置插入一条IndexOri表格的数据
        #region cit文件同名的数据库中德索引表(IndexOri)中插入一条数据
        /// <summary>
        /// cit文件同名的数据库中德索引表(IndexOri)中插入一条数据
        /// </summary>
        /// <param name="sFilePath">与cit同名的idf数据库全路径文件名</param>
        /// <param name="sID">索引id</param>
        /// <param name="sIndexID">索引状态：0-原有的数据；1-新插入的数据</param>
        /// <param name="lPostion">索引对应的文件指针</param>
        /// <param name="sIndexKm">索引对应的里程数</param>
        /// <returns></returns>
        public int InsertLayerAllIndexInfo(string sFilePath, string sID, string sIndexID, string lPostion, string sIndexKm)
        {
            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFilePath + ";Persist Security Info=True"))
                {
                    string sqlCreate = "insert into IndexOri values(" + sID + ",0,'" + lPostion + "','" + sIndexKm + "')";
                    OleDbCommand sqlcom = new OleDbCommand(sqlCreate, sqlconn);
                    sqlconn.Open();
                    sqlcom.ExecuteNonQuery();

                    sqlconn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return 0;

        }
        #endregion

        #region cit文件同名的数据库中德索引表(IndexOri)中的第一行插入一条数据
        /// <summary>
        /// cit文件同名的数据库中德索引表(IndexOri)中的第一行插入一条数据
        /// </summary>
        /// <param name="sFilePath">idf数据库文件路径</param>
        /// <param name="sID">索引表的主id</param>
        /// <param name="sIndexID">索引状态：0-原有的数据；1-新插入的数据</param>
        /// <param name="lPostion">索引对应的文件指针</param>
        /// <param name="sIndexKm">索引对应的里程数</param>
        /// <returns></returns>
        public int InsertLayerIndexInfo(string sFilePath, string sID, string sIndexID, string lPostion, string sIndexKm)
        {

            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFilePath + ";Persist Security Info=True"))
                {
                    string sqlCreate = "select max(id)+1 from IndexOri";
                    OleDbCommand sqlcom = new OleDbCommand(sqlCreate, sqlconn);
                    sqlconn.Open();
                    sID = sqlcom.ExecuteScalar().ToString();
                    if (string.IsNullOrEmpty(sID))
                    {
                        sID = "1";
                    }
                    sqlconn.Close();
                }

                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFilePath + ";Persist Security Info=True"))
                {
                    string sqlCreate = "insert into IndexOri values(" + sID + ",0,'" + lPostion + "','" + sIndexKm + "')";
                    OleDbCommand sqlcom = new OleDbCommand(sqlCreate, sqlconn);
                    sqlconn.Open();
                    sqlcom.ExecuteNonQuery();

                    sqlconn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return 0;

        }
        #endregion
        #endregion

        #region 获取指定cit文件的全部索引---IndexOri
        /// <summary>
        /// 获取指定cit文件的全部索引---IndexOri
        /// </summary>
        /// <param name="sFilePath">与cit文件同名的idf文件全路径名</param>
        /// <param name="sIndexID">此参数没有使用</param>
        /// <param name="sKmInc">公里标增减标志</param>
        /// <returns>cit文件索引信息</returns>
        public List<IndexOriClass> GetLayerIndexInfo(string sFilePath, string sIndexID, string sKmInc)
        {
            List<IndexOriClass> listioc = new List<IndexOriClass>();

            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFilePath + ";Persist Security Info=True"))
                {
                    string sqlCreate = "select * from IndexOri order by val(indexmeter) ";
                    if (sKmInc.Equals("增"))
                    {
                        sqlCreate += "";
                    }
                    else
                    {
                        sqlCreate += " desc";
                    }
                    OleDbCommand sqlcom = new OleDbCommand(sqlCreate, sqlconn);
                    sqlconn.Open();
                    OleDbDataReader oleDBR = sqlcom.ExecuteReader();

                    while (oleDBR.Read())
                    {
                        IndexOriClass ioc = new IndexOriClass();
                        ioc.iId = oleDBR.GetInt32(0);
                        ioc.iIndexId = oleDBR.GetInt32(1);
                        ioc.IndexPoint = oleDBR.GetString(2);
                        ioc.IndexMeter = oleDBR.GetString(3);

                        listioc.Add(ioc);

                    }

                    sqlconn.Close();
                }
            }
            catch
            {

            }

            return listioc;

        }
        #endregion

        public void AnalysisInfoInsertInto(String sFilePath,String startMile,String endMile,int analysisType,int manageType,int secFlag,String analysisDes,String memoInfo,String attachPath)
        {
            #region 获取案例id
            int id = 0;//案例id
            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFilePath + ";Persist Security Info=True"))
                {
                    sqlconn.Open();

                    String sSql = "select max(Id) from AnalysisInfo";
                    OleDbCommand sqlcom = new OleDbCommand(sSql, sqlconn);
                    OleDbDataReader oledbReader = sqlcom.ExecuteReader();
                    Boolean isNull = oledbReader.HasRows;//是否是第一条记录，第一条记录id为1；
                    if (isNull == false)
                    {
                        id = 1;
                    }
                    else
                    {
                        while (oledbReader.Read())
                        {
                            if (String.IsNullOrEmpty(oledbReader.GetValue(0).ToString()))
                            {
                                id = 1;
                            }
                            else
                            {
                                id = int.Parse(oledbReader.GetValue(0).ToString()) + 1;
                            }
                        }
                    }

                    sqlconn.Close();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("获取案例id异常：" + ex.Message);
            }
            #endregion

            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFilePath + ";Persist Security Info=True"))
                {
                    sqlconn.Open();
                    OleDbCommand com = sqlconn.CreateCommand();
                    com.CommandText = "insert into AnalysisInfo values(@Id,@LineCode,@LineName,@LineDir,@DetectDate,@SecFlag,@StartPoint,@StartMile,@EndPoint,"
                    + "@EndMile,@AnalySisType,@ManageType,@ImportFlag,@Opdate,@CITName,@AttachDes,@AttachType,@AttachContent,@MemoInfo)";

                    com.Parameters.AddWithValue("@Id", id);
                    com.Parameters.AddWithValue("@LineCode", id);
                    com.Parameters.AddWithValue("@LineName", id);
                    com.Parameters.AddWithValue("@LineDir", id);
                    com.Parameters.AddWithValue("@DetectDate", id);
                    com.Parameters.AddWithValue("@SecFlag", id);
                    com.Parameters.AddWithValue("@StartPoint", id);
                    com.Parameters.AddWithValue("@StartMile", id);
                    com.Parameters.AddWithValue("@EndPoint", id);
                    com.Parameters.AddWithValue("@EndMile", id);
                    com.Parameters.AddWithValue("@AnalySisType", id);
                    com.Parameters.AddWithValue("@ManageType", id);
                    com.Parameters.AddWithValue("@ImportFlag", id);
                    com.Parameters.AddWithValue("@Opdate", id);
                    com.Parameters.AddWithValue("@CITName", id);
                    com.Parameters.AddWithValue("@AttachDes", id);
                    com.Parameters.AddWithValue("@AttachType", id);
                    com.Parameters.AddWithValue("@AttachContent", id);
                    com.Parameters.AddWithValue("@MemoInfo", id);




                    com.ExecuteNonQuery();
                    sqlconn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("无效区段设置异常:" + ex.Message);
            }
        }

        public List<AnalysisInfoClass> AnalysisInfoList(String sAddFile)
        {
            List<AnalysisInfoClass> listAIC = new List<AnalysisInfoClass>();
            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sAddFile + ";Persist Security Info=True"))
                {
                    sqlconn.Open();

                    string sSql = "select * from AnalysisInfo order by clng(Id)";
                    OleDbCommand sqlcom = new OleDbCommand(sSql, sqlconn);                    
                    OleDbDataReader oleDBr = sqlcom.ExecuteReader();
                    int columnNum = oleDBr.FieldCount;
                    while (oleDBr.Read())
                    {
                        AnalysisInfoClass aic = new AnalysisInfoClass();
                        aic.id = int.Parse(oleDBr.GetValue(0).ToString());
                        aic.lineCode = oleDBr.GetValue(1).ToString();
                        aic.lineName = oleDBr.GetValue(2).ToString();
                        aic.lineDir = oleDBr.GetValue(3).ToString();
                        aic.detectDate = oleDBr.GetValue(4).ToString();
                        aic.secFlag = int.Parse(oleDBr.GetValue(5).ToString());
                        aic.startPoint = oleDBr.GetValue(6).ToString();
                        aic.startMile = oleDBr.GetValue(7).ToString();

                        aic.endPoint = oleDBr.GetValue(8).ToString();
                        aic.endMile = oleDBr.GetValue(9).ToString();
                        aic.analysisType = int.Parse(oleDBr.GetValue(10).ToString());
                        aic.manageType = int.Parse(oleDBr.GetValue(11).ToString());
                        aic.importFlag = int.Parse(oleDBr.GetValue(12).ToString());
                        aic.opDate = oleDBr.GetValue(13).ToString();
                        aic.citName = oleDBr.GetValue(14).ToString();
                        aic.memoInfo = oleDBr.GetValue(15).ToString();

                        string tmpSql = "select count(Id) from AnalyAttach where Id="+aic.id;
                        OleDbCommand tmpSqlcom = new OleDbCommand(tmpSql, sqlconn);
                        OleDbDataReader tmpOleDBr = tmpSqlcom.ExecuteReader();
                        while (tmpOleDBr.Read())
                        {
                            aic.attachNum = tmpOleDBr.GetValue(0).ToString();
                        }
                        tmpOleDBr.Close();

                        listAIC.Add(aic);
                    }
                    oleDBr.Close();
                    sqlconn.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("案例库读取异常:" + ex.Message);
            }
            return listAIC;
        }

        public int DeleteAnalysisInfo(String sAddFile, int id)
        {
            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sAddFile + ";Persist Security Info=True"))
                {
                    string sqlCreate = "delete from AnalysisInfo where id=" + id;
                    OleDbCommand sqlcom = new OleDbCommand(sqlCreate, sqlconn);
                    sqlconn.Open();
                    sqlcom.ExecuteNonQuery();

                    sqlconn.Close();
                }
            }
            catch
            {

            }

            return 0;
        }

        public byte[] GetEncryptByteArray(string FileName, int tds)
        {
            byte[] iArray;
            try
            {
                using (FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        byte[] b = new byte[tds * 2];
                        br.ReadBytes(120);//120
                        br.ReadBytes(65 * tds);//65
                        iArray = br.ReadBytes(BitConverter.ToInt32(br.ReadBytes(4), 0));
                        br.Close();
                    }
                    fs.Close();

                }
                return iArray;
            }
            catch
            {
                iArray = new byte[1];
                return iArray;
            }
        }

        //
        public int[] GetDataMileageInfo(string FileName, int tds, int cyjg, bool bEncrypt, List<IndexStaClass> listIC, bool bIndex, long lStart, long lEnd, string sKmInc)
        {
            int[] iArray;
            try
            {
                using (FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        byte[] b = new byte[tds * 2];
                        br.ReadBytes(120);//120
                        br.ReadBytes(65 * tds);//65
                        br.ReadBytes(BitConverter.ToInt32(br.ReadBytes(4), 0));
                        long lDataLength = (br.BaseStream.Length - br.BaseStream.Position) / 2 / tds;
                        if (bIndex)
                        {
                            br.BaseStream.Position = lStart;
                            lDataLength = (lEnd - lStart) / 2 / tds;
                            iArray = new int[lDataLength];
                            //
                            for (int i = 0; i < lDataLength; i++)
                            {
                                iArray[i] = PointToMeter(listIC, br.BaseStream.Position, tds, sKmInc);
                                br.ReadBytes(tds * 2);
                            }
                        }
                        else
                        {
                            iArray = new int[lDataLength];
                            for (int i = 0; i < lDataLength; i++)
                            {
                                b = br.ReadBytes(b.Length);
                                //加密处理
                                if (bEncrypt)
                                {
                                    b = ByteXORByte(b);
                                }
                                short gongli;
                                short mi;
                                gongli = BitConverter.ToInt16(b, 0);
                                mi = BitConverter.ToInt16(b, 2);
                                iArray[i] = (int)(gongli * 1000 + (mi / (float)cyjg));
                            }
                        }

                        br.Close();
                    }
                    fs.Close();

                }
                return iArray;
            }
            catch
            {
                iArray = new int[2];
                return iArray;
            }
        }

        public double[] GetDataMileageInfoDouble(string FileName, int tds, int cyjg, bool bEncrypt, List<IndexStaClass> listIC, bool bIndex, long lStart, long lEnd, string sKmInc)
        {
            double[] iArray;
            try
            {
                using (FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (BinaryReader br = new BinaryReader(fs))
                    {
                        byte[] b = new byte[tds * 2];
                        br.ReadBytes(120);//120
                        br.ReadBytes(65 * tds);//65
                        br.ReadBytes(BitConverter.ToInt32(br.ReadBytes(4), 0));
                        long lDataLength = (br.BaseStream.Length - br.BaseStream.Position) / 2 / tds;
                        if (bIndex)
                        {
                            br.BaseStream.Position = lStart;
                            lDataLength = (lEnd - lStart) / 2 / tds;
                            iArray = new double[lDataLength];
                            //
                            for (int i = 0; i < lDataLength; i++)
                            {
                                iArray[i] = PointToMeter(listIC, br.BaseStream.Position, tds, sKmInc)/1000f;
                                br.ReadBytes(tds * 2);
                            }
                        }
                        else
                        {
                            iArray = new double[lDataLength];
                            for (int i = 0; i < lDataLength; i++)
                            {
                                b = br.ReadBytes(b.Length);
                                //加密处理
                                if (bEncrypt)
                                {
                                    b = ByteXORByte(b);
                                }
                                short gongli;
                                short mi;
                                gongli = BitConverter.ToInt16(b, 0);
                                mi = BitConverter.ToInt16(b, 2);
                                iArray[i] = (gongli * 1000 + (mi / (float)cyjg))/1000;
                            }
                        }

                        br.Close();
                    }
                    fs.Close();

                }
                return iArray;
            }
            catch
            {
                iArray = new double[2];
                return iArray;
            }
        }

        #region 获取cit文件中通道数据的起始文件指针和结束文件指针
        /// <summary>
        /// 获取cit文件中通道数据的起始文件指针和结束文件指针
        /// 索引：
        ///      存在，则使用索引中的起始文件指针和结束文件指针
        ///      不存在，获取cit文件中通道数据的起始文件指针和结束文件指针
        /// </summary>
        /// <param name="lStartPosition">通道数据的起始文件指针</param>
        /// <param name="lEndtPosition">通道数据的结束文件指针</param>
        /// <param name="FileName">CIT波形文件名称</param>
        /// <param name="gjtds">通道总数</param>
        /// <param name="lFixStartPosition">索引中通道数据的起始文件指针</param>
        /// <param name="lFixEndPosition">索引中通道数据的结束文件指针</param>
        /// <param name="bIndex">是否使用索引</param>
        /// <returns></returns>
        public bool GetDataStartPositionEndPositionInfoIncludeIndex(ref long lStartPosition, ref long lEndtPosition, string FileName, int gjtds, long lFixStartPosition, long lFixEndPosition, bool bIndex)
        {
            try
            {
                FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(fs);
                byte[] b = new byte[gjtds * 2];
                br.ReadBytes(120);//120
                br.ReadBytes(65 * gjtds);//65
                br.ReadBytes(BitConverter.ToInt32(br.ReadBytes(4), 0));
                //if (lFixStartPosition == -1 && lFixEndPosition == -1 && !bIndex)---改为下面--ygx--20140113
                if (lFixStartPosition == -1 && lFixEndPosition == -1 && !bIndex)
                {
                    lStartPosition = br.BaseStream.Position;
                    lEndtPosition = br.BaseStream.Length;
                }
                else
                {
                    lStartPosition = lFixStartPosition;
                    lEndtPosition = lFixEndPosition;
                }
            }
            catch
            {
                //MessageBox.Show(ex.Message);
            }
            return true;
        }
        #endregion

        #region 标注操作时，提取标注点所对应的文件指针
        /// <summary>
        /// 标注操作时，提取标注点所对应的文件指针
        /// 返回结果：标注点所对应的文件指针
        /// </summary>
        /// <param name="iPoint">标注点的x轴点数：范围(0--XZoomIn*4)</param>
        /// <param name="FileName">cit文件名</param>
        /// <param name="mi"></param>
        /// <param name="xZoomIn">x轴显示米数</param>
        /// <param name="cyjg">采样频率(4个点/米)</param>
        /// <param name="gjtds">通道总数</param>
        /// <param name="bl">多余的参数，没有使用</param>
        /// <param name="gl">多余的参数，没有使用</param>
        /// <returns>标注点所对应的文件指针</returns>
        public long GetDataInfoPosition(int iPoint, string FileName, long mi, int xZoomIn, int cyjg, int gjtds, float[] bl, float[] gl)
        {
            byte[] b = new byte[gjtds * 2];
            int i = 0;
            long p = 0;
            long lReturn = 0;
            FileStream fs;
            try
            {
                fs = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(fs);
                br.ReadBytes(120);
                br.ReadBytes(65 * gjtds);
                br.ReadBytes(BitConverter.ToInt32(br.ReadBytes(4), 0));
                p = br.BaseStream.Position;
                //当前屏的起始文件指针
                br.BaseStream.Seek(p + (long)(mi * gjtds * 2 * cyjg * (xZoomIn / 10)), SeekOrigin.Begin);

                while (i < xZoomIn * cyjg)
                {  //处理不同比例的X轴
                    lReturn = br.BaseStream.Position;
                    b = br.ReadBytes(gjtds * 2);
                    //iPoint:标注点的x轴点数：范围(0--XZoomIn*4)
                    if (i == iPoint)
                    {

                        break;
                    }
                    i++;

                }
                br.Close();
                fs.Close();
                return lReturn;
            }
            catch
            {
                return lReturn;
            }
        }
        #endregion
        #region 标注操作时，提取标注点所对应的文件里程(单位为公里)
        /// <summary>
        /// 标注操作时，提取标注点所对应的文件里程(单位为公里)
        /// 返回结果：标注点所对应的文件指针
        /// </summary>
        /// <param name="iPoint">标注点的x轴点数：范围(0--XZoomIn*4)</param>
        /// <param name="FileName">cit文件名</param>
        /// <param name="mi"></param>
        /// <param name="xZoomIn">x轴显示米数</param>
        /// <param name="cyjg">采样频率(4个点/米)</param>
        /// <param name="gjtds">通道总数</param>
        /// <returns>标注点所对应的文件里程</returns>
        public float GetDataInfoPositionGL(int iPoint, string FileName, long mi, int xZoomIn, int cyjg, int gjtds)
        {
            byte[] b = new byte[gjtds * 2];
            int i = 0;
            long p = 0;
            float lReturn = 0f;
            FileStream fs;
            try
            {
                fs = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(fs);
                br.ReadBytes(120);
                br.ReadBytes(65 * gjtds);
                br.ReadBytes(BitConverter.ToInt32(br.ReadBytes(4), 0));
                p = br.BaseStream.Position;
                //当前屏的起始文件指针
                br.BaseStream.Seek(p + (long)(mi * gjtds * 2 * cyjg * (xZoomIn / 10)), SeekOrigin.Begin);

                while (i < xZoomIn * cyjg)
                {  //处理不同比例的X轴
                    //lReturn = br.BaseStream.Position;
                    b = br.ReadBytes(gjtds * 2);
                    //iPoint:标注点的x轴点数：范围(0--XZoomIn*4)
                    if (i == iPoint)
                    {
                        short gongli = BitConverter.ToInt16(b, 0);
                        short mi1 = BitConverter.ToInt16(b, 2);
                        lReturn = (int)(gongli + (mi1 / (float)cyjg));
                        break;
                    }
                    i++;

                }
                br.Close();
                fs.Close();
                return lReturn;
            }
            catch
            {
                return lReturn;
            }
        }
        #endregion

        #region 标准差计算
        /// <summary>
        /// 标准差计算
        /// </summary>
        /// <param name="dItems"></param>
        /// <returns></returns>
        public double CalcStardard(double[] dItems)
        {
            double dResult = 0;
            double dSum = 0;
            for (int i = 0; i < dItems.Length; i++)
            {
                dSum += dItems[i];
            }
            dSum /= dItems.Length;
            double dAve = 0;
            for (int i = 0; i < dItems.Length; i++)
            {
                dAve += Math.Pow((dItems[i] - dSum), 2);
            }
            dAve /= dItems.Length;
            dResult = Math.Pow(dAve, 0.5);

            return dResult;
        }
        #endregion

        #region 在iic当中创建新的TQI表和偏差表，用于重新计算
        /// <summary>
        /// 在iic当中创建新的TQI表和偏差表，用于重新计算
        /// </summary>
        /// <param name="sFileName"></param>
        /// <returns></returns>
        public void CreateIICTable(string sFileName)
        {
            //删除表
            DropFixTalbe(sFileName);

            //创建fix_defects表
            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFileName + ";Persist Security Info=True"))
                {
                    sqlconn.Open();

                    //原来这里是拷贝所有记录
                    //考虑到很多超限值车上人员已经确认过是无效的，因此这里只拷贝有效的--20140114--和赵主任确认的结果
                    string sqlCreate = "select * into fix_defects from defects where valid<>'N'";
                    OleDbCommand sqlcom = new OleDbCommand(sqlCreate, sqlconn);                    
                    sqlcom.ExecuteNonQuery();

                    //段级系统要求要保留校正前的里程，因此把maxpost,maxminor拷贝到frompost,fromminor---20140225--严广学
                    sqlCreate = "update  fix_defects set frompost=maxpost";
                    sqlcom = new OleDbCommand(sqlCreate, sqlconn);
                    sqlcom.ExecuteNonQuery();
                    sqlCreate = "update  fix_defects set fromminor=maxminor";
                    sqlcom = new OleDbCommand(sqlCreate, sqlconn);
                    sqlcom.ExecuteNonQuery();

                    sqlconn.Close();
                }
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //创建fix_tqi表
            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFileName + ";Persist Security Info=True"))
                {
                    string sqlCreate = "CREATE TABLE fix_tqi (" +
    "SubCode varchar(255) NULL," +
    "RunDate date NULL," +
    "FromPost integer NULL," +
    "FromMinor real NULL," +
    "L_Prof_Value real NULL," +
    "R_Prof_Value real NULL," +
    "L_Align_Value real NULL," +
    "R_Align_Value real NULL," +
    "Gage_Value real NULL," +
    "Crosslevel_TQIValue real NULL," +
    "ShortTwist_Value real NULL," +
    "TQISum_Value real NULL," +
    "LATACCEL_Value real NULL," +
    "VERTACCEL_Value real NULL," +
    "AVERAGE_SPEED integer NULL," +
    "valid integer NULL" +
    ");";
                    OleDbCommand sqlcom = new OleDbCommand(sqlCreate, sqlconn);
                    sqlconn.Open();
                    sqlcom.ExecuteNonQuery();
                    sqlconn.Close();
                }
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
        #region 删除已经创建的表--ygx--20140320
        /// <summary>
        /// 删除已经创建的表
        /// </summary>
        /// <param name="sFileName">iic文件</param>
        public void DropFixTalbe(String sFileName)
        {
            //删除已经创建的表            
            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFileName + ";Persist Security Info=True"))
                {
                    string sqlCreate = "drop table fix_defects";
                    OleDbCommand sqlcom = new OleDbCommand(sqlCreate, sqlconn);
                    sqlconn.Open();
                    sqlcom.ExecuteNonQuery();
                    sqlconn.Close();
                }
                Application.DoEvents();
            }
            catch
            {
                //直接使用原始的iic文件，里面没有fix_defects表，因此删除出错，但是不处理。
            }
            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFileName + ";Persist Security Info=True"))
                {
                    string sqlCreate = "drop table fix_tqi";
                    OleDbCommand sqlcom = new OleDbCommand(sqlCreate, sqlconn);
                    sqlconn.Open();
                    sqlcom.ExecuteNonQuery();
                    sqlconn.Close();
                }
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                //直接使用原始的iic文件，里面没有fix_tqi表，因此删除出错，但是不处理。
            }
        }
        #endregion

        #region 接口函数---不使用
        public string GetDataIndexID(string sFile)
        {
            StringBuilder sb = new StringBuilder();
            try
            {

                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFile + ";Persist Security Info=False"))
                {
                    OleDbCommand sqlcom = new OleDbCommand("select * from IndexMain order by id", sqlconn);
                    sqlconn.Open();
                    OleDbDataReader sqloledr = sqlcom.ExecuteReader();
                    while (sqloledr.Read())
                    {
                        sb.Append((int)sqloledr.GetInt32(0));
                        sb.Append("-");
                        sb.Append(sqloledr.GetString(1));
                        sb.Append("-");
                        sb.Append(sqloledr.GetDateTime(2).ToString());
                        sb.Append(",");
                    }
                    sqlconn.Close();
                    if (sb.Length > 1)
                    { sb.Remove(sb.Length - 1, 1); }
                }

            }
            catch
            {

            }
            return sb.ToString();
        }
        #endregion      

        #region 主层索引相关里程计算
        /// <summary>
        /// 主层索引相关里程计算
        /// </summary>
        /// <param name="listIC">CIT文件索引信息</param>
        /// <param name="lPosition">文件中具体通道的指针</param>
        /// <param name="jvli">包含的里程数(单位为米)</param>
        /// <param name="tds">通道数</param>
        /// <param name="sKmInc">增减里程标志</param>
        /// <returns></returns>
        public List<WaveMeter> GetDataIndexInfoCalc(List<IndexStaClass> listIC, long lPosition, int jvli, int tds, string sKmInc)
        {
            List<WaveMeter> iMeter = new List<WaveMeter>();

            //处理当两个波形加载索引打开时，第二层的里程较短的波形最后一段显示错误的问题--20141128--严广学
            if (lPosition == 0)
            {
                for (int i = 0; i < jvli * 4;i++ )
                {
                    WaveMeter wm = new WaveMeter();
                    wm.Km = 0;
                    wm.Meter = 0f;
                    wm.lPosition = 0;

                    iMeter.Add(wm);
                }
                return iMeter;
            }


            //处理里程
            for (int i = 0; i < listIC.Count; i++)
            {
                if (lPosition >= listIC[i].lStartPoint && lPosition < listIC[i].lEndPoint)
                {
                    int iCount = (int)(jvli / (float.Parse(listIC[i].lContainsMeter) * 1000 / listIC[i].lContainsPoint));
                    long lCurPos = lPosition - listIC[i].lStartPoint;
                    int iIndex = 0;
                    if (listIC[i].sType.Contains("长链"))
                    {
                        int iKM = 0;
                        double dCDLMeter = float.Parse(listIC[i].lContainsMeter) * 1000;
                        if (sKmInc.Equals("减"))
                        {
                            iKM = (int)float.Parse(listIC[i].LEndMeter);
                        }
                        else
                        {
                            iKM = (int)float.Parse(listIC[i].lStartMeter);
                        }
                        for (iIndex = 0; iIndex < iCount && (lPosition + iIndex * tds * 2) < listIC[i].lEndPoint; iIndex++)
                        {
                            float f = (lCurPos / tds / 2 + iIndex) * ((float.Parse(listIC[i].lContainsMeter) * 1000 / listIC[i].lContainsPoint));
                            WaveMeter wm = new WaveMeter();
                            if (sKmInc.Equals("减"))
                            {
                                wm.Km = iKM;
                                wm.Meter = (float)(dCDLMeter - f);
                            }
                            else
                            {
                                wm.Km = iKM;
                                wm.Meter = f;
                            }
                            wm.lPosition = (lPosition + (iIndex * tds * 2));
                            iMeter.Add(wm);
                        }
                    }
                    else
                    {
                        double dMeter = float.Parse(listIC[i].lStartMeter) * 1000;
                        for (iIndex = 0; iIndex < iCount && (lPosition + iIndex * tds * 2) < listIC[i].lEndPoint; iIndex++)
                        {
                            float f = (lCurPos / tds / 2 + iIndex) * ((float.Parse(listIC[i].lContainsMeter) * 1000 / listIC[i].lContainsPoint));
                            WaveMeter wm = new WaveMeter();
                            if (sKmInc.Equals("减"))
                            {
                                wm.Km = (int)((dMeter - f) / 1000);
                                wm.Meter = (float)((dMeter - f) % 1000);
                            }
                            else
                            {
                                wm.Km = (int)((dMeter + f) / 1000);
                                wm.Meter = (float)((dMeter + f) % 1000);
                            }
                            wm.lPosition = (lPosition + (iIndex * tds * 2));
                            iMeter.Add(wm);
                        }
                    }
                    if ((i + 1) < listIC.Count && iIndex != iCount)
                    {
                        int iNextCount = (int)(jvli / (float.Parse(listIC[i + 1].lContainsMeter) * 1000 / listIC[i + 1].lContainsPoint));
                        long lNextCurPos = listIC[i + 1].lStartPoint;
                        double dChaCount = iCount / 1.0 / iNextCount;
                        int iChaValue = iCount - iIndex;
                        int iValue = (int)(iChaValue / dChaCount);
                        if (listIC[i + 1].sType.Contains("长链"))
                        {
                            int iKM = 0;
                            double dNextCDLMeter = float.Parse(listIC[i + 1].lContainsMeter) * 1000;
                            if (sKmInc.Equals("减"))
                            {
                                iKM = (int)float.Parse(listIC[i + 1].LEndMeter);
                            }
                            else
                            {
                                iKM = (int)float.Parse(listIC[i + 1].lStartMeter);
                            }
                            for (int iNextIndex = 0; iNextIndex < iValue && (listIC[i + 1].lStartPoint + iNextCount * tds * 2) < listIC[i + 1].lEndPoint; iNextIndex++)
                            {
                                float f = iNextIndex * ((float.Parse(listIC[i + 1].lContainsMeter) * 1000 / listIC[i + 1].lContainsPoint));
                                WaveMeter wm = new WaveMeter();
                                if (sKmInc.Equals("减"))
                                {
                                    wm.Km = iKM;
                                    wm.Meter = (float)(dNextCDLMeter - f);
                                }
                                else
                                {
                                    wm.Km = iKM;
                                    wm.Meter = f;
                                }
                                wm.lPosition = (listIC[i + 1].lStartPoint + (iNextIndex * tds * 2));
                                iMeter.Add(wm);
                            }
                        }
                        else
                        {
                            double dNextMeter = float.Parse(listIC[i + 1].lStartMeter) * 1000;
                            for (int iNextIndex = 0; iNextIndex < iValue && (listIC[i + 1].lStartPoint + iNextIndex * tds * 2) < listIC[i + 1].lEndPoint; iNextIndex++)
                            {
                                float f = (iNextIndex) * ((float.Parse(listIC[i + 1].lContainsMeter) * 1000 / listIC[i + 1].lContainsPoint));
                                WaveMeter wm = new WaveMeter();
                                if (sKmInc.Equals("减"))
                                {
                                    wm.Km = (int)((dNextMeter - f) / 1000);
                                    wm.Meter = (float)((dNextMeter - f) % 1000);
                                }
                                else
                                {
                                    wm.Km = (int)((dNextMeter + f) / 1000);
                                    wm.Meter = (float)((dNextMeter + f) % 1000);
                                }
                                wm.lPosition = (listIC[i + 1].lStartPoint + (iNextIndex * tds * 2));
                                iMeter.Add(wm);
                            }
                        }
                    }
                    break;

                }

            }
            return iMeter;
        }
        #endregion

        #region 获取里程信息--不使用
        public List<WaveMeter> GetNewDataIndexInfoCalc(List<IndexStaClass> listIC, long lPosition, int jvli, int tds, string sKmInc)
        {
            List<WaveMeter> iMeter = new List<WaveMeter>();

            for (int i = 0; i < listIC.Count; i++)
            {
                if (lPosition >= listIC[i].lStartPoint && lPosition < listIC[i].lEndPoint)
                {
                    //动态采样点为((float.Parse(listIC[i].lContainsMeter)*1000)/listIC[i].lContainsPoint)

                    int iLeftPosMeter = (int)((lPosition - listIC[i].lStartPoint) /
                        (tds * 2) * ((float.Parse(listIC[i].lContainsMeter) * 1000) / listIC[i].lContainsPoint));

                    double iSum = 0;
                    if (sKmInc.Equals("增"))
                    {
                        iSum = float.Parse(listIC[i].lStartMeter) + iLeftPosMeter / 1000.0;
                    }
                    else
                    {
                        iSum = float.Parse(listIC[i].lStartMeter) - iLeftPosMeter / 1000.0;
                    }
                    double iDataLength = 0;
                    if (sKmInc.Equals("增"))
                    {
                        iDataLength = float.Parse(listIC[i].LEndMeter) - iSum;
                    }
                    else
                    {
                        iDataLength = iSum - float.Parse(listIC[i].LEndMeter);
                    }
                    if ((iDataLength * 1000) < jvli && ((i + 1) != listIC.Count))
                    {
                        int iCount = ((int)Math.Ceiling(((iDataLength * 1000) / ((float.Parse(listIC[i].lContainsMeter) * 1000) / listIC[i].lContainsPoint))));
                        if (sKmInc.Equals("增"))
                        {
                            for (int j = 0; j < iCount; j++)
                            {
                                //iSum
                                WaveMeter wm = new WaveMeter();
                                if (!listIC[i].sType.Equals("长链"))
                                {
                                    wm.Km = (int)iSum;
                                    wm.Meter = (float)(iSum - wm.Km) * 1000;
                                    iSum += (((float.Parse(listIC[i].lContainsMeter) * 1000) / listIC[i].lContainsPoint) / 1000);
                                }
                                else
                                {
                                    wm.Km = int.Parse(listIC[i].lStartMeter);
                                    wm.Meter = (float)(iSum - wm.Km) * 1000;
                                    iSum += (((float.Parse(listIC[i].lContainsMeter) * 1000) / listIC[i].lContainsPoint) / 1000);
                                }
                                iMeter.Add(wm);
                            }
                        }
                        else
                        {
                            for (int j = 0; j < iCount; j++)
                            {
                                //iSum
                                WaveMeter wm = new WaveMeter();
                                if (!listIC[i].sType.Equals("长链"))
                                {
                                    wm.Km = (int)iSum;
                                    wm.Meter = (float)(iSum - wm.Km) * 1000;
                                    iSum -= (((float.Parse(listIC[i].lContainsMeter) * 1000) / listIC[i].lContainsPoint) / 1000);


                                }
                                else
                                {
                                    wm.Km = int.Parse(listIC[i].LEndMeter);
                                    wm.Meter = (float)(iSum - wm.Km) * 1000;
                                    iSum -= (((float.Parse(listIC[i].lContainsMeter) * 1000) / listIC[i].lContainsPoint) / 1000);
                                }
                                iMeter.Add(wm);
                            }

                        }
                        double iNewLength = jvli - iDataLength * 1000;
                        int iNewCount = (int)Math.Ceiling(iNewLength / ((float.Parse(listIC[i + 1].lContainsMeter) * 1000) / listIC[i + 1].lContainsPoint));
                        if (sKmInc.Equals("增"))
                        {
                            iSum = float.Parse(listIC[i + 1].lStartMeter);
                        }
                        else
                        {
                            iSum = float.Parse(listIC[i + 1].lStartMeter);
                        }
                        if (sKmInc.Equals("增"))
                        {
                            for (int j = 0; j < iNewCount; j++)
                            {
                                WaveMeter wm = new WaveMeter();
                                if (!listIC[i].sType.Equals("长链"))
                                {
                                    wm.Km = (int)iSum;
                                    wm.Meter = (float)(iSum - wm.Km) * 1000;
                                    iSum += (((float.Parse(listIC[i + 1].lContainsMeter) * 1000) / listIC[i + 1].lContainsPoint) / 1000);
                                }
                                else
                                {
                                    wm.Km = int.Parse(listIC[i + 1].lStartMeter);
                                    wm.Meter = (float)(iSum - wm.Km) * 1000;
                                    iSum += (((float.Parse(listIC[i + 1].lContainsMeter) * 1000) / listIC[i + 1].lContainsPoint) / 1000);
                                }
                                iMeter.Add(wm);
                            }
                        }
                        else
                        {
                            for (int j = 0; j < iNewCount; j++)
                            {

                                WaveMeter wm = new WaveMeter();
                                if (!listIC[i].sType.Equals("长链"))
                                {
                                    wm.Km = (int)iSum;
                                    wm.Meter = (float)(iSum - wm.Km) * 1000;
                                    iSum -= (((float.Parse(listIC[i + 1].lContainsMeter) * 1000) / listIC[i + 1].lContainsPoint) / 1000);
                                }
                                else
                                {
                                    wm.Km = int.Parse(listIC[i + 1].LEndMeter);
                                    wm.Meter = (float)(iSum - wm.Km) * 1000;
                                    iSum -= (((float.Parse(listIC[i + 1].lContainsMeter) * 1000) / listIC[i + 1].lContainsPoint) / 1000);
                                }
                                iMeter.Add(wm);
                            }
                        }
                    }
                    else
                    {
                        int iCount = ((int)Math.Ceiling((jvli / ((float.Parse(listIC[i].lContainsMeter) * 1000) / listIC[i].lContainsPoint))));
                        if (sKmInc.Equals("增"))
                        {
                            for (int j = 0; j < iCount; j++)
                            {
                                WaveMeter wm = new WaveMeter();
                                if (!listIC[i].sType.Equals("长链"))
                                {
                                    wm.Km = (int)iSum;
                                    wm.Meter = (float)(iSum - wm.Km) * 1000;
                                    iSum += (((float.Parse(listIC[i].lContainsMeter) * 1000) / listIC[i].lContainsPoint) / 1000);
                                }
                                else
                                {
                                    wm.Km = int.Parse(listIC[i].lStartMeter);
                                    wm.Meter = (float)(iSum - wm.Km) * 1000;
                                    iSum += (((float.Parse(listIC[i].lContainsMeter) * 1000) / listIC[i].lContainsPoint) / 1000);
                                }
                                iMeter.Add(wm);
                            }
                        }
                        else
                        {
                            for (int j = 0; j < iCount; j++)
                            {
                                WaveMeter wm = new WaveMeter();
                                if (!listIC[i].sType.Equals("长链"))
                                {
                                    wm.Km = (int)iSum;
                                    wm.Meter = (float)(iSum - wm.Km) * 1000;
                                    iSum -= (((float.Parse(listIC[i].lContainsMeter) * 1000) / listIC[i].lContainsPoint) / 1000);
                                }
                                else
                                {
                                    wm.Km = int.Parse(listIC[i].LEndMeter);
                                    wm.Meter = (float)(iSum - wm.Km) * 1000;
                                    iSum -= (((float.Parse(listIC[i].lContainsMeter) * 1000) / listIC[i].lContainsPoint) / 1000);
                                }
                                iMeter.Add(wm);
                            }
                        }
                    }
                    return iMeter;
                }
            }
            return iMeter;
        }
        #endregion


        #region 在索引修正前提下，第一次打开文件，获取通道数据起始文件指针
        /// <summary>
        /// 在索引修正前提下，第一次打开文件，获取通道数据起始文件指针
        /// </summary>
        /// <param name="listIC">文件索引</param>
        /// <param name="lStartPos">通道数据的起始文件指针</param>
        /// <param name="iXScrollBarValue">左右拖动框当前值</param>
        /// <param name="iXZoomIn">x轴显示的米数</param>
        /// <param name="tds">通道数</param>
        /// <returns>通道数据起始文件指针</returns>
        private long GetIndexMeterPositon(List<IndexStaClass> listIC, long lStartPos, long iXScrollBarValue, int iXZoomIn, int tds)
        {
            long iCurPos = lStartPos + iXScrollBarValue * tds * 2 * 4 * (iXZoomIn / 10);
            for (int i = 0; i < listIC.Count; i++)
            {
                if (listIC[i].lStartPoint <= iCurPos && listIC[i].lEndPoint > iCurPos)
                {
                    return iCurPos;
                }
            }
            return -1;
        }
        #endregion

        #region 索引修正条件下：根据当前位置(单位：米)，获取相应的在修正之后的文件中的文件指针
        /// <summary>
        /// 索引修正条件下：
        /// 根据当前位置(单位：米)，获取相应的在修正之后的文件中的文件指针
        /// </summary>
        /// <param name="listIC">与数据库中IndexSta表对应的长短链索引数据类对象</param>
        /// <param name="iCurrentMeter">当前位置(单位：米)</param>
        /// <param name="tds">通道数</param>
        /// <param name="sKmInc">增减里程标志</param>
        /// <param name="lReviseValue">修正值(采样点的个数)</param>
        /// <returns>经索引修正后，当前位置(单位：米)在文件中的文件指针</returns>
        public long GetNewIndexMeterPositon(List<IndexStaClass> listIC, long iCurrentMeter, int tds, string sKmInc, long lReviseValue)
        {
            //增里程
            if (sKmInc.Contains("增"))
            {
                for (int i = 0; i < listIC.Count; i++)
                {
                    if (iCurrentMeter >= (long)(double.Parse(listIC[i].lStartMeter) * 1000) &&
                        iCurrentMeter <= (long)(double.Parse(listIC[i].LEndMeter) * 1000))
                    {
                        if (iCurrentMeter == (long)(double.Parse(listIC[i].LEndMeter) * 1000))
                        {
                            return listIC[i].lEndPoint;
                        }
                        long lDivMeter = (iCurrentMeter - (long)(double.Parse(listIC[i].lStartMeter) * 1000));
                        long lPos = (long)Math.Ceiling(lDivMeter / (double.Parse(listIC[i].lContainsMeter) * 1000 / listIC[i].lContainsPoint));
                        return listIC[i].lStartPoint + (lReviseValue * 2 * tds) + (lPos * 2 * tds);
                    }
                }
            }
            else//减里程
            {
                for (int i = 0; i < listIC.Count; i++)
                {
                    if (iCurrentMeter <= (long)(double.Parse(listIC[i].lStartMeter) * 1000) &&
                        iCurrentMeter >= (long)(double.Parse(listIC[i].LEndMeter) * 1000))
                    {

                        if (iCurrentMeter == (long)(double.Parse(listIC[i].LEndMeter) * 1000))
                        {
                            return listIC[i].lEndPoint;
                        }

                        long lDivMeter = ((long)(double.Parse(listIC[i].lStartMeter) * 1000) - iCurrentMeter);
                        long lPos = (long)Math.Ceiling((lDivMeter / ((double.Parse(listIC[i].lContainsMeter) * 1000 / listIC[i].lContainsPoint))));
                        return listIC[i].lStartPoint + (lReviseValue * 2 * tds) + (lPos * 2 * tds);
                    }
                }
            }
            return -1;
        }
        #endregion


        #region 缺失数据相关的函数----三个---没有使用
        #region idf数据库操作：查询--缺少的里程数据
        public List<LostDataClass> GetLostData(string sFileName, string sDBFileName, bool bWriteDB, bool bEncrypt)
        {

            FileStream fs = new FileStream(sFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            BinaryReader br = new BinaryReader(fs, Encoding.UTF8);
            byte[] bHead = br.ReadBytes(120);
            int iKmInc = BitConverter.ToInt32(bHead, 100);
            int iChannelNumber = BitConverter.ToInt32(bHead, 116);
            long fLen = 0;

            br.ReadBytes(65 * iChannelNumber);
            byte[] b = new byte[iChannelNumber * 2];
            br.ReadBytes(BitConverter.ToInt32(br.ReadBytes(4), 0));

            short oldGL = -2;
            short oldM = -18030;

            short GL;
            short M;
            int i = 1;
            switch (iKmInc)
            {
                case 0:

                    break;
                case 1:
                    i = -i;
                    break;
            }
            fLen = fs.Length - br.BaseStream.Position;
            Application.DoEvents();
            List<LostDataClass> listLDC = new List<LostDataClass>();
            int iCount = 0;
            while (br.BaseStream.Position < br.BaseStream.Length)
            {
                b = br.ReadBytes(iChannelNumber * 2);
                if (bEncrypt)
                {
                    b = ByteXORByte(b);
                }
                if (b.Length != iChannelNumber * 2)
                {
                    break;
                }
                GL = BitConverter.ToInt16(b, 0);
                M = BitConverter.ToInt16(b, 2);

                LostDataClass ldc = new LostDataClass();
                if (((M - i) != oldM) && oldM != -18030 && (M != 3999 && oldM != 0) && (M != 1 && oldM != 4000) && ((M != 0 && GL != 0) && (oldGL != 0 && oldM != 0)))
                {
                    ldc.id = ++iCount;
                    ldc.lStartPos = br.BaseStream.Position - iChannelNumber * 2 - iChannelNumber * 2;
                    ldc.dStartMeter = (oldGL * 1000 + oldM / 4.0) / 1000.0;
                    ldc.lEndPos = br.BaseStream.Position - iChannelNumber * 2;
                    ldc.dSEndMeter = (GL * 1000 + M / 4.0) / 1000.0;
                    ldc.iCount = Math.Abs((GL - oldGL) * 4000 + (M - oldM)) - 1;
                    if (ldc.iCount >= 800)
                    {
                        continue;
                    }
                    listLDC.Add(ldc);

                }
                oldGL = GL;
                oldM = M;

            }
            br.Close();
            fs.Close();

            //往数据库插缺失数据
            if (bWriteDB)
            { SetLostDataToDB(listLDC, sDBFileName); }
            return listLDC;
        }
        #endregion

        #region 往数据库插缺失数据
        /// <summary>
        /// 往数据库插缺失数据
        /// </summary>
        /// <param name="listLDC"></param>
        /// <param name="sDBFileName"></param>
        private void SetLostDataToDB(List<LostDataClass> listLDC, string sDBFileName)
        {

            try
            {
                for (int j = 0; j < listLDC.Count; j++)
                {
                    using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sDBFileName + ";Persist Security Info=True"))
                    {
                        //listLDC 
                        string sqlCreate = "insert into 缺失数据表 values(" + listLDC[j].id.ToString() + ",'" + listLDC[j].lStartPos.ToString() +
                            "','" + listLDC[j].dStartMeter.ToString() + "','" + listLDC[j].lEndPos.ToString() +
                            "','" + listLDC[j].dSEndMeter.ToString() + "')";
                        OleDbCommand sqlcom = new OleDbCommand(sqlCreate, sqlconn);
                        sqlconn.Open();
                        sqlcom.ExecuteNonQuery();
                        sqlconn.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region 获取文件缺失数据
        /// <summary>
        /// 获取文件缺失数据
        /// </summary>
        /// <param name="sDBFileName"></param>
        /// <returns></returns>
        private List<LostDataClass> GetLostDataFromDB(string sDBFileName)
        {
            List<LostDataClass> listLDC = new List<LostDataClass>();

            using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sDBFileName + ";Persist Security Info=True"))
            {
                //listLDC 
                string sqlCreate = "select * from 缺失数据表 order by Id";
                OleDbCommand sqlcom = new OleDbCommand(sqlCreate, sqlconn);
                sqlconn.Open();
                OleDbDataReader oddr = sqlcom.ExecuteReader();

                while (oddr.Read())
                {
                    LostDataClass ldc = new LostDataClass();
                    ldc.id = int.Parse(oddr.GetValue(0).ToString());
                    ldc.lStartPos = long.Parse(oddr.GetValue(1).ToString());
                    ldc.dStartMeter = double.Parse(oddr.GetValue(2).ToString());
                    ldc.lEndPos = long.Parse(oddr.GetValue(3).ToString());
                    ldc.dSEndMeter = double.Parse(oddr.GetValue(4).ToString());
                    listLDC.Add(ldc);
                }
                oddr.Close();
                sqlconn.Close();
            }


            return listLDC;
        }
        #endregion
        #endregion

        #region 根据索引导出数据---有两个，一个有用，另一个没有使用

        #region 根据索引导出数据--在用
        /// <summary>
        /// 根据索引导出数据
        /// </summary>
        /// <param name="FileName">CIT文件名</param>
        /// <param name="iChannelNumber">通道数</param>
        /// <param name="sKmInc">增减里程标志</param>
        /// <param name="bEncrypt">通道数据是否被加密</param>
        /// <param name="fStartMile">待导出数据的起点公里数</param>
        /// <param name="fEndMile">待导出数据的终止公里数</param>
        /// <param name="sParameter">待导出数据的参数</param>
        public void ExportIndexDataOld(string FileName, int iChannelNumber, string sKmInc, bool bEncrypt,float fStartMile,float fEndMile, string[] sParameter)
        {
            byte[] b = new byte[iChannelNumber * 2];
            FileStream fs;
            File.Delete(Path.GetDirectoryName(FileName) + "\\" + Path.GetFileName(FileName) + ".csv");
            StreamWriter sw = new StreamWriter(Path.GetDirectoryName(FileName) + "\\" + Path.GetFileName(FileName) + "_" + fStartMile.ToString() + "-" + fEndMile .ToString()+ ".csv", false, Encoding.UTF8);
            try
            {
                List<IndexStaClass> listIC = GetDataIndexInfo(Path.GetDirectoryName(FileName) + "\\" + Path.GetFileNameWithoutExtension(FileName) + ".idf");
                //根据信息读取里程及数据信息
                fs = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(fs);
                br.ReadBytes(120);//120
                br.ReadBytes(65 * iChannelNumber);//65
                br.ReadBytes(BitConverter.ToInt32(br.ReadBytes(4), 0));
                float[] f = new float[iChannelNumber];//通道比例值
                float[] g = new float[iChannelNumber];//通道偏移量
                //获取通道比例，包括通道比例和偏移
                GetChannelScale(FileName, iChannelNumber, ref f, ref g);
                //列名称
                sw.WriteLine("Km_Index,Meter_Index,"+sParameter[1]);
                string[] strIndex = sParameter[0].Split(new char[]{','});
                for (int i = 0; i < listIC.Count; i++)
                {
                    if (sKmInc.Equals("增"))
                    {
                        if (fStartMile > float.Parse(listIC[i].LEndMeter))
                        {
                            continue;
                        }
                        if (fEndMile < float.Parse(listIC[i].lStartMeter))
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (fStartMile < float.Parse(listIC[i].LEndMeter))
                        {
                            continue;
                        }
                        if (fEndMile > float.Parse(listIC[i].lStartMeter) )
                        {
                            continue;
                        }
                    }
                    br.BaseStream.Position = listIC[i].lStartPoint;
                    int iKM = (int)float.Parse(listIC[i].lStartMeter);
                    float iMeter = (int)(float.Parse(listIC[i].lStartMeter) * 1000) - (iKM * 1000);
                    if (listIC[i].sType.Equals("长链") && sKmInc.Equals("减"))//这里需要关注。严广学
                    {
                        iKM = (int)float.Parse(listIC[i].LEndMeter);
                        iMeter = float.Parse(listIC[i].lContainsMeter) * 1000f;
                    }
                    //处理未索引的数据导出
                    while (br.BaseStream.Position < listIC[i].lEndPoint)
                    {  //处理不同比例的X轴
                        b = br.ReadBytes(iChannelNumber * 2);
                        if (bEncrypt)
                        {
                            b = ByteXORByte(b);
                        }
                        //处理数据通道
                        StringBuilder sb = new StringBuilder();
                        for (int n = 0; n < strIndex.Length; n++)
                        {
                            //通道下标
                            int index = int.Parse(strIndex[n]);
                            //通道值
                            float fValue = 0;
                            if (index == 0)
                            {
                                fValue = float.Parse((BitConverter.ToInt16(b, index * 2)).ToString());
                            }
                            else if (index ==1)
                            {
                                fValue = float.Parse((BitConverter.ToInt16(b, index * 2)).ToString()) / f[1];
                            } 
                            else
                            {
                                fValue = float.Parse((BitConverter.ToInt16(b, index * 2)).ToString()) / f[index] + g[index];
                            }

                            sb.Append("," + fValue.ToString("f4"));

                        }

                        float fMile = float.Parse((iKM + "." + (int)iMeter).ToString());     
                        if (sKmInc.Equals("增"))
                        {
                            if(fStartMile <= fMile && fMile <= fEndMile)
                            {
                                sw.WriteLine(iKM.ToString() + "," + iMeter.ToString("f3") + sb.ToString());
                            }
                        }
                        else
                        {
                            if(fEndMile <= fMile && fMile <= fStartMile)
                            {
                                sw.WriteLine(iKM.ToString() + "," + iMeter.ToString("f3") + sb.ToString());
                            }
                        }
                        
                        if (sKmInc.Equals("增"))
                        {
                            iMeter += ((float.Parse(listIC[i].lContainsMeter) * 1000) / listIC[i].lContainsPoint);
                            if (iMeter >= 1000.0f && listIC[i].sType.Equals("正常"))
                            {
                                iKM++;
                                iMeter = iMeter - 1000;
                            }
                        }
                        else
                        {
                            iMeter -= ((float.Parse(listIC[i].lContainsMeter) * 1000) / listIC[i].lContainsPoint);
                            if (iMeter <= 0.0 && listIC[i].sType.Equals("正常"))
                            {
                                iKM--;
                                iMeter = 1000 + iMeter;
                            }
                        }
                    }
                }
                br.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            sw.Close();
        }
        #endregion

        #region 根据索引导出数据--不使用
        public void ExportIndexData(string FileName, int iChannelNumber, string sKmInc, bool bEncrypt, string sLineCode, string sDir)
        {
            //byte[] b = new byte[iChannelNumber * 2];
            //FileStream fs;
            //File.Delete(Application.StartupPath + "\\" + Path.GetFileName(FileName) + ".txt");
            //StreamWriter sw = new StreamWriter(Application.StartupPath + "\\" + Path.GetFileName(FileName) + ".txt", false, Encoding.UTF8);
            ////try
            ////{
            //    List<IndexClass> listIC = GetDataIndexInfo(Path.GetDirectoryName(FileName) + "\\" + Path.GetFileNameWithoutExtension(FileName) + ".idf");
            //    //根据信息读取里程及数据信息
            //    fs = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            //    BinaryReader br = new BinaryReader(fs);
            //    br.ReadBytes(120);//120
            //    br.ReadBytes(65 * iChannelNumber);//65
            //    br.ReadBytes(BitConverter.ToInt32(br.ReadBytes(4), 0));
            //    float[] f=new float[iChannelNumber];
            //    float[] g=new float[iChannelNumber];
            //    GetChannelScale(FileName, iChannelNumber, ref f, ref g);
            //    for (int i = 0; i < listIC.Count; i++)
            //    {
            //        br.BaseStream.Position = listIC[i].lStartPoint;
            //        int iKM=(int)float.Parse(listIC[i].lStartMeter);
            //        float iMeter =(int)(float.Parse(listIC[i].lStartMeter) * 1000)-(iKM*1000);
            //        if (listIC[i].sType.Equals("长链") && sKmInc.Equals("减"))
            //        {
            //            iKM = (int)float.Parse(listIC[i].LEndMeter);
            //            iMeter = float.Parse(listIC[i].lContainsMeter) * 1000f;
            //        }
            //        int iCount = 0;
            //        while (br.BaseStream.Position < listIC[i].lEndPoint)
            //        {  //处理不同比例的X轴
            //            b = br.ReadBytes(iChannelNumber * 2);
            //            if (bEncrypt)
            //            {
            //                b = ByteXORByte(b);
            //            }
            //            //处理数据通道
            //            StringBuilder sb = new StringBuilder();
            //            for (int n = 2; n < iChannelNumber; n++)
            //            {
            //                float fValue = float.Parse((BitConverter.ToInt16(b, n * 2)).ToString())/f[n];
            //                sb.Append("," + fValue.ToString("f2"));

            //            }
            //            if (sKmInc.Equals("增"))
            //            {
            //                sw.WriteLine(sLineCode + "," + iKM.ToString() + "," +
            //                (iMeter + iCount * ((float.Parse(listIC[i].lContainsMeter) * 1000) / listIC[i].lContainsPoint)).ToString("f3") + "," + sDir + sb.ToString());
            //            }
            //            else
            //            {
            //                sw.WriteLine(sLineCode + "," + iKM.ToString() + "," +
            //                (iMeter - iCount * ((float.Parse(listIC[i].lContainsMeter) * 1000) / listIC[i].lContainsPoint)).ToString("f3") + "," + sDir + sb.ToString());
            //            }

            //            if (sKmInc.Equals("增"))
            //            {
            //                if (iMeter >= 1000.0f && listIC[i].sType.Equals("正常"))
            //                {
            //                    iKM++;
            //                    iMeter = iMeter - 1000;
            //                }
            //            }
            //            else
            //            {
            //                if (iMeter <= 0.0 && listIC[i].sType.Equals("正常"))
            //                {
            //                    iKM--;
            //                    iMeter = 1000 + iMeter;
            //                }
            //            }
            //            iCount++;
            //        }
            //    }
            //    br.Close();
            //    fs.Close();

            //    MessageBox.Show("保存成功!");
            ////}
            ////catch (Exception ex)
            ////{
            ////    MessageBox.Show(ex.Message);

            ////}
            //sw.Close();
        }
        #endregion

        #endregion

        
        #region 获取数据信息--不使用
        public bool GetNewDataInfo(List<int> listChannelsVisible, ref float[][] arrayPointF, ref List<WaveMeter> listMeter, string FileName, long mi, int jvli, int iChannelNumber, bool bIndex, string sKmInc, bool bEncrypt, bool bReverse)
        {
            byte[] b = new byte[iChannelNumber * 2];
            int i = 0;
            long lStartP = 0;
            FileStream fs;
            try
            {
                List<IndexStaClass> listIC = GetDataIndexInfo(Path.GetDirectoryName(FileName) + "\\" + Path.GetFileNameWithoutExtension(FileName) + ".idf");
                //根据信息读取里程及数据信息
                fs = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(fs);
                br.ReadBytes(120);//120
                br.ReadBytes(65 * iChannelNumber);//65
                br.ReadBytes(BitConverter.ToInt32(br.ReadBytes(4), 0));
                lStartP = br.BaseStream.Position;
                //获取当前里程位置
                if (bIndex)
                {
                    long lReturn = GetNewIndexMeterPositon(listIC, mi, iChannelNumber, sKmInc, 0);
                    if (lReturn != -1)
                    {
                        br.BaseStream.Seek(lReturn, SeekOrigin.Begin);
                    }
                    else
                    {
                        return false;
                    }
                }
                //获取里程坐标
                if (bIndex)
                {
                    listMeter = GetNewDataIndexInfoCalc(listIC, br.BaseStream.Position, jvli, iChannelNumber, sKmInc);
                    for (int n = 0; n < listChannelsVisible.Count; n++)
                    {
                        arrayPointF[n] = new float[listMeter.Count];

                    }
                }
                while (br.BaseStream.Position < br.BaseStream.Length && i < listMeter.Count)
                {  //处理不同比例的X轴
                    b = br.ReadBytes(iChannelNumber * 2);

                    if (bEncrypt)
                    {
                        b = ByteXORByte(b);
                    }

                    listMeter[i].lPosition = br.BaseStream.Position - iChannelNumber * 2;
                    //处理数据通道
                    for (int n = 0; n < listChannelsVisible.Count; n++)
                    {
                        arrayPointF[n][i] = float.Parse((BitConverter.ToInt16(b, (listChannelsVisible[n]) * 2)).ToString());

                    }
                    i++;
                }
                br.Close();
                fs.Close();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }
        #endregion
        //新的多功能获取数据信息
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listChannelsVisible"></param>
        /// <param name="arrayPointF"></param>
        /// <param name="listMeter"></param>
        /// <param name="FileName"></param>
        /// <param name="mi"></param>
        /// <param name="jvli"></param>
        /// <param name="iSmaleRate"></param>
        /// <param name="iChannelNumber"></param>
        /// <param name="lStartPosition"></param>
        /// <param name="lEndPosition"></param>
        /// <param name="sKmInc"></param>
        /// <param name="bEncrypt"></param>
        /// <param name="iReadCount"></param>
        /// <returns></returns>
        public int GetAutoDataInfo(List<int> listChannelsVisible, ref float[][] arrayPointF, ref List<WaveMeter> listMeter, string FileName, ref long mi, int xZoomIn, int iSmaleRate, int iChannelNumber, long lStartPosition, long lEndPosition, string sKmInc, bool bEncrypt, int iReadCount)
        {
            int iChannelLength = iChannelNumber * 2;
            int iKMP = 0;
            int iMP = 2;

            byte[] b = new byte[iChannelLength]; ;
            int i = 0;

            FileStream fs;
            try
            {//
                //根据信息读取里程及数据信息
                fs = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(fs);
                br.BaseStream.Position = mi;


                listMeter = new List<WaveMeter>(iReadCount * iSmaleRate);
                for (int iIndex = 0; iIndex < iReadCount * iSmaleRate; iIndex++)
                {
                    WaveMeter wm = new WaveMeter();
                    listMeter.Add(wm);
                }
                for (int n = 0; n < arrayPointF.Length; n++)
                {
                    arrayPointF[n] = new float[listMeter.Count];
                }

                while (br.BaseStream.Position < br.BaseStream.Length && i < listMeter.Count)
                {  //处理不同比例的X轴
                    b = br.ReadBytes(iChannelLength);

                    if (bEncrypt)
                    {
                        b = ByteXORByte(b);
                    }
                    short km;
                    short m;
                    km = BitConverter.ToInt16(b, iKMP);
                    m = BitConverter.ToInt16(b, iMP);
                    listMeter[i].Km = km;
                    listMeter[i].Meter = m / (float)iSmaleRate;

                    listMeter[i].lPosition = br.BaseStream.Position - iChannelNumber * 2;
                    //处理数据通道

                    for (int n = 0; n < listChannelsVisible.Count; n++)
                    {
                        arrayPointF[n][i] = float.Parse((BitConverter.ToInt16(b, (listChannelsVisible[n]) * 2)).ToString());

                    }

                    i++;
                    mi = br.BaseStream.Position;

                }
                //


                br.Close();
                fs.Close();
            }
            catch
            {
                return -100;
            }
            finally
            {
            }
            return i;

        }
        #region 根据点返回索引文件里对应的里程信息
        /// <summary>
        /// 根据点返回索引文件里对应的里程信息
        /// </summary>
        /// <param name="listIC">索引信息</param>
        /// <param name="lPosition">点的位置</param>
        /// <param name="tds">文件通道书</param>
        /// <param name="sKmInc">增减里程标</param>
        /// <returns>索引里程：单位为米</returns>
        public int PointToMeter(List<IndexStaClass> listIC, long lPosition, int tds, string sKmInc)
        {
            int iMeter = 0;

            //处理里程
            for (int i = 0; i < listIC.Count; i++)
            {
                if (lPosition >= listIC[i].lStartPoint && lPosition < listIC[i].lEndPoint)
                {
                    int iCount = 1;
                    long lCurPos = lPosition - listIC[i].lStartPoint;
                    int iIndex = 0;
                    if (listIC[i].sType.Contains("长链"))
                    {
                        int iKM = 0;
                        double dCDLMeter = float.Parse(listIC[i].lContainsMeter) * 1000;
                        if (sKmInc.Equals("减"))
                        {
                            iKM = (int)float.Parse(listIC[i].LEndMeter);
                        }
                        else
                        {
                            iKM = (int)float.Parse(listIC[i].lStartMeter);
                        }
                        for (iIndex = 0; iIndex < iCount && (lPosition + iIndex * tds * 2) < listIC[i].lEndPoint; )
                        {
                            float f = (lCurPos / tds / 2 + iIndex) * ((float.Parse(listIC[i].lContainsMeter) * 1000 / listIC[i].lContainsPoint));
                            WaveMeter wm = new WaveMeter();
                            if (sKmInc.Equals("减"))
                            {
                                wm.Km = iKM;
                                wm.Meter = (float)(dCDLMeter - f);
                            }
                            else
                            {
                                wm.Km = iKM;
                                wm.Meter = (float)(dCDLMeter + f);
                            }
                            wm.lPosition = (lPosition + (iIndex * tds * 2));
                            iMeter = wm.GetMeter(1);
                            return iMeter;
                        }
                    }
                    else
                    {
                        double dMeter = float.Parse(listIC[i].lStartMeter) * 1000;
                        for (iIndex = 0; iIndex < iCount && (lPosition + iIndex * tds * 2) < listIC[i].lEndPoint; )
                        {
                            float f = (lCurPos / tds / 2 + iIndex) * ((float.Parse(listIC[i].lContainsMeter) * 1000 / listIC[i].lContainsPoint));
                            WaveMeter wm = new WaveMeter();
                            if (sKmInc.Equals("减"))
                            {
                                wm.Km = (int)((dMeter - f) / 1000);
                                wm.Meter = (float)((dMeter - f) % 1000);
                            }
                            else
                            {
                                wm.Km = (int)((dMeter + f) / 1000);
                                wm.Meter = (float)((dMeter + f) % 1000);
                            }
                            wm.lPosition = (lPosition + (iIndex * tds * 2));
                            iMeter = wm.GetMeter(1);
                            return iMeter;
                        }
                    }
                    break;

                }

            }
            return iMeter;
        }
        #endregion

        #region 获取数据信息:arrayPointF是直接从文件中读取出来的数，还没有经过换算（data/通道比例+通道基线）。
        /// <summary>
        /// 获取数据信息:arrayPointF是直接从文件中读取出来的数，还没有经过换算（data/通道比例+通道基线）。
        /// </summary>
        /// <param name="listChannelsVisible"></param>
        /// <param name="arrayPointF"></param>
        /// <param name="listMeter"></param>
        /// <param name="lReviseValue"></param>
        /// <param name="FileName">文件名</param>
        /// <param name="mi"></param>
        /// <param name="iXZoomIn">x轴显示的米数</param>
        /// <param name="iSmaleRate">采样频率(每米采样几个点)</param>
        /// <param name="iChannelNumber">通道数量</param>
        /// <param name="lStartPosition">cit文件中通道数据的起始文件指针</param>
        /// <param name="lEndPosition">cit文件中通道数据的结束文件指针</param>
        /// <param name="listIC">索引列表</param>
        /// <param name="bIndex">是否加载索引</param>
        /// <param name="sKmInc">增减里程</param>
        /// <param name="bEncrypt">是否加密</param>
        /// <param name="bReverse">是否反转</param>
        /// <param name="bSubIndex">在使用索引前提下，是否第一次打开cit文件</param>
        /// <param name="iDataType"></param>
        /// <returns>函数执行结果：true-成功；false-失败</returns>
        public bool GetDataInfo(List<int> listChannelsVisible, ref float[][] arrayPointF, ref List<WaveMeter> listMeter, long lReviseValue, string FileName, long mi, int iXZoomIn, int iSmaleRate, int iChannelNumber, long lStartPosition, long lEndPosition, List<IndexStaClass> listIC, bool bIndex, string sKmInc, bool bEncrypt, bool bReverse, bool bSubIndex, int iDataType)
        {
            //单个采样点的所有通道数据的总长度
            int iChannelDataLength = 0;
            int iKMP = 0;
            int iMP = 0;//采样数

            
            iChannelDataLength = iChannelNumber * 2;
            iKMP = 0;
            iMP = 2;
            //iBLChannelsOffset = 0;

            byte[] b = new byte[iChannelDataLength]; 
            int i = 0;
            long lRuntimeP = 0;
            FileStream fs;
            try
            {//
                //根据信息读取里程及数据信息
                fs = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(fs);
                br.BaseStream.Position = lStartPosition;
                //p = lStartPosition;
                if (bIndex)     //有索引
                {
                    if (!bSubIndex)//关注：在有索引的前提下，第一次打开文件走这个分支
                    {
                        long lReturn = GetIndexMeterPositon(listIC, lStartPosition, mi, iXZoomIn, iChannelNumber);
                        if (lReturn != -1)
                        {
                            br.BaseStream.Seek(lReturn, SeekOrigin.Begin);
                        }
                        else
                        {
                            br.BaseStream.Position = 0;
                        }
                    }
                    else  //关注：在有索引的前提下，第二次以上打开文件走这个分支
                    {
                        long lReturn = GetNewIndexMeterPositon(listIC, mi, iChannelNumber, sKmInc, lReviseValue);
                        if (lReturn != -1)
                        {
                            br.BaseStream.Seek(lReturn, SeekOrigin.Begin);
                        }
                        else
                        {
                            br.BaseStream.Position = 0;
                        }
                    }
                }
                else        //没有索引
                {
                    if (bReverse)//需要翻转
                    {
                        //疑问？--严广学。
                        //程序中mi其实是水平拖动条的值，我觉得应该 mi*4* iChannelDataLength * (iXZoomIn / 10.0)
                        //翻转时，为什么要多减去- iChannelDataLength * iXZoomIn 

                        lRuntimeP = (long)(lEndPosition - iChannelDataLength * iXZoomIn - (int)Math.Ceiling(mi * iChannelDataLength * (iXZoomIn / 10.0)) - lReviseValue * 1 * iChannelDataLength);

                        if (lRuntimeP < lStartPosition)
                        {
                            br.BaseStream.Seek(lStartPosition, SeekOrigin.Begin);
                        }
                        else//这个分支不可能走到
                        {
                            br.BaseStream.Seek(lRuntimeP, SeekOrigin.Begin);
                        }
                    }
                    else//不需要翻转
                    {
                        lRuntimeP = lStartPosition + (int)(mi * (iChannelNumber) * 2 * 4 * (iXZoomIn / 10)) + lReviseValue * (iChannelNumber) * 2;

                        //lRuntimeP = lStartPosition + + lReviseValue * (iChannelNumber) * 2;

                        if (lRuntimeP < lStartPosition)//这个分支不可能走到
                        {
                            br.BaseStream.Seek(lStartPosition, SeekOrigin.Begin);
                        }
                        else
                        {

                            br.BaseStream.Seek(lRuntimeP, SeekOrigin.Begin);
                        }
                    }
                }

                if (bIndex)
                {
                    listMeter = GetDataIndexInfoCalc(listIC, br.BaseStream.Position, iXZoomIn, iChannelNumber, sKmInc);
                    for (int n = 0; n < listChannelsVisible.Count; n++)
                    {
                        arrayPointF[n] = new float[listMeter.Count];

                    }
                }
                else
                {
                    listMeter = new List<WaveMeter>(iSmaleRate * iXZoomIn);
                    for (int iIndex = 0; iIndex < iSmaleRate * iXZoomIn; iIndex++)
                    {
                        WaveMeter wm = new WaveMeter();
                        listMeter.Add(wm);
                    }
                    for (int n = 0; n < arrayPointF.Length; n++)
                    {
                        arrayPointF[n] = new float[listMeter.Count];
                    }
                    if (bReverse)
                    {
                        while (lRuntimeP < lStartPosition && i < listMeter.Count)
                        {
                            lRuntimeP += ((iChannelNumber) * 2);

                            for (int n = 0; n < listChannelsVisible.Count; n++)
                            {
                                //arrayPointF[n][i].X = (float)i;
                                arrayPointF[n][i] = 0f;

                            }
                            i++;
                        }
                    }
                    else
                    {
                        while (lRuntimeP < lStartPosition && i < listMeter.Count)
                        {
                            lRuntimeP += ((iChannelNumber) * 2);

                            for (int n = 0; n < listChannelsVisible.Count; n++)
                            {
                                //arrayPointF[n][i].X = (float)i;
                                arrayPointF[n][i] = 0f;

                            }
                            i++;
                        }
                    }
                }

                //弓网波形处理            
                int iBase = 0;
                byte[] bKey = new byte[401];
                if (iDataType == 3)   //iDataType：1轨检、2动力学、3弓网，
                {
                    byte[] bbb = GetEncryptByteArray(FileName, iChannelNumber);
                    for (int iIndex = 0; iIndex < 401; iIndex++)
                    {
                        iBase = (iBase + iIndex) % 722;
                        bKey[iIndex] = (byte)(bbb[iBase] ^ bbb[722 - iBase - 1]);
                    }
                }
                
                //当打开多次波形（加载索引的），第二层以上的里程较短的波形在数据结束后，自动补零---20141128---严广学
                if (br.BaseStream.Position == 0)
                {
                    for (int k = 0; k < listMeter.Count;k++ )
                    {
                        for (int n = 0; n < listChannelsVisible.Count; n++)
                        {
                            arrayPointF[n][k] = 0;
                        }
                    }
                } 
                else
                {
                    while (br.BaseStream.Position < lEndPosition && i < listMeter.Count)//对i的使用有疑问，严广学---觉得这里i应该先i=0；
                    {  //处理不同比例的X轴
                        long ll = br.BaseStream.Position;
                        b = br.ReadBytes(iChannelDataLength);

                        if (bEncrypt)
                        {
                            if (iDataType == 3)//iDataType：1轨检、2动力学、3弓网，
                            {
                                for (int g = 0; g < b.Length; g++)
                                {
                                    b[g] = (byte)(b[g] ^ bKey[(ll + g) % 401]);
                                }
                            }
                            else
                            {
                                b = ByteXORByte(b);
                            }
                        }
                        if (!bIndex)
                        {
                            short km;
                            ushort m;
                            km = BitConverter.ToInt16(b, iKMP);
                            m = BitConverter.ToUInt16(b, iMP);
                            listMeter[i].Km = km;
                            /*有索引时，Meter单位为米
                             *没有索引时，Meter单位为采样数
                             */
                            listMeter[i].Meter = m;//这里要关注，严广学

                        }
                        listMeter[i].lPosition = br.BaseStream.Position - iChannelNumber * 2;
                        //处理数据通道

                        for (int n = 0; n < listChannelsVisible.Count; n++)
                        {
                            //arrayPointF[n][i].X = (float)i;
                            //arrayPointF[n][i].Y = float.Parse((BitConverter.ToInt16(b, (listChannelsVisible[n] + ChannelsOffset) * 2) /
                            //    bl[listChannelsVisible[n] + ChannelsOffset - iBLChannelsOffset]).ToString());
                            arrayPointF[n][i] = float.Parse((BitConverter.ToInt16(b, (listChannelsVisible[n]) * 2)).ToString());
                        }

                        i++;
                        //if (br.BaseStream.Position >= lEndLength)
                        //{
                        //    while (i < listMeter.Count)
                        //    {
                        //        for (int n = 0; n < listChannelsVisible.Count; n++)
                        //        {
                        //            arrayPointF[n][i].X = (float)i;
                        //        }
                        //        i++;
                        //    }
                        //    break;
                        //}
                        //pc.gjky = i;
                    }
                }
                //


                br.Close();
                fs.Close();

                if (bIndex)
                {

                }
                else
                {
                    if (bReverse)
                    {
                        listMeter.Reverse();
                        for (int iArrayID = 0; iArrayID < arrayPointF.Length; iArrayID++)
                        {
                            Array.Reverse(arrayPointF[iArrayID]);
                        }

                    }
                }
                //listMeter.Reverse();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
            return true;

        }
        #endregion

        #region 加解密算法
        /// <summary>
        /// 加解密算法：数组中的每一个字节都与128异或
        /// </summary>
        /// <param name="b">需要被加密的byte数组</param>
        /// <returns>加密之后的byte数组</returns>
        public static byte[] ByteXORByte(byte[] b)
        {
            for (int iIndex = 0; iIndex < b.Length; iIndex++)
            {
                b[iIndex] = (byte)(b[iIndex] ^ 128);
            }
            return b;
        }
        #endregion
        //TQI类
        public class TQIClass
        {
            public double zgd = 0;
            public double ygd = 0;
            public double zgx = 0;
            public double ygx = 0;
            public double gj = 0;
            public double sp = 0;
            public double sjk = 0;
            public double hj = 0;
            public double cj = 0;
            public int pjsd = 0;
            public int iKM = 0;
            public float iMeter = 0;
            public double GetTQISum()
            {
                return zgd + ygd + zgx + ygx + gj + sp + sjk;
            }
            public int iValid = 1;

            public String subCode = null;
            public DateTime runDate = DateTime.Now;
        }
        //TQIMileClass
        public class TQIMileClass
        {
            public int iKM = 0;
            public int iMeter = 0;
            public long lPostion = 0;
        }
        //TQI修正
        public void TQIFix(string sWaveFileName, string sIICFileName, List<IndexStaClass> listIC, int cyjg, int gjtds, bool bEncrypt, string sKmInc, List<InvalidDataClass> listIDC, string sTrain)
        {
            int iStartKM = 0;
            int iEndKM = 0;
            String subCode = null; //ashx
            DateTime runDate=DateTime.Now;

            #region 获取线路代码和检测时间
            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sIICFileName + ";Persist Security Info=True"))
                {
                    string sqlCreate = "select DISTINCT SubCode,RunDate from tqi";
                    OleDbCommand sqlcom = new OleDbCommand(sqlCreate, sqlconn);
                    sqlconn.Open();
                    OleDbDataReader oleDBdr = sqlcom.ExecuteReader();
                    
                    if (oleDBdr.Read())
                    {
                        subCode = oleDBdr.GetValue(0).ToString();
                        runDate = DateTime.Parse(oleDBdr.GetValue(1).ToString());
                    }


                    oleDBdr.Close();
                    sqlconn.Close();
                }
                Application.DoEvents();
            }
            catch
            {

            }
            #endregion

            #region 获取TQI里程
            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sIICFileName + ";Persist Security Info=True"))
                {
                    string sqlCreate = "select min(FromPost),max(FromPost) from tqi";
                    OleDbCommand sqlcom = new OleDbCommand(sqlCreate, sqlconn);
                    sqlconn.Open();
                    OleDbDataReader oleDBdr = sqlcom.ExecuteReader();
                    if (oleDBdr.Read())
                    {
                        iStartKM = int.Parse(oleDBdr.GetValue(0).ToString());
                        iEndKM = int.Parse(oleDBdr.GetValue(1).ToString());
                    }

                    oleDBdr.Close();
                    sqlconn.Close();
                }
                Application.DoEvents();
            }
            catch
            {

            }
            #endregion

            #region 获取通道序号
            string[] sTQIItem = new string[] { "L_Prof_SC", "R_Prof_SC", "L_Align_SC", 
                "R_Align_SC", "Gage", "Crosslevel", "Short_Twist", "LACC", "VACC","Speed"};
            int[] sTQIItemIndex = new int[sTQIItem.Length];
            //获取通道信息
            CITDataProcess citdp = new CITDataProcess();
            citdp.QueryDataInfoHead(sWaveFileName);
            citdp.QueryDataChannelInfoHead(sWaveFileName);
            float[] f = new float[gjtds];
            float[] g = new float[gjtds];
            GetChannelScale(sWaveFileName, gjtds, ref f, ref g);
            //给通道绑定序号
            for (int i = 0; i < sTQIItem.Length; i++)
            {

                for (int j = 0; j < citdp.dciL.Count; j++)
                {
                    if (sTQIItem[i].Equals(citdp.dciL[j].sNameEn))
                    {
                        sTQIItemIndex[i] = j;
                        break;
                    }
                }
            }
            citdp = null;
            if (sKmInc.Contains("减"))
            {
                int iChange = 0;
                iChange = iStartKM;
                iStartKM = iEndKM;
                iEndKM = iChange;
            }
            //if ((iStartKM) > double.Parse(listIC[0].lStartMeter))
            //{
            //    iStartKM = (int)double.Parse(listIC[0].lStartMeter);
            //    if (sKmInc.Contains("减"))
            //    {
            //        iStartKM += 1;
            //    }
            //}
            //if ((iEndKM) < double.Parse(listIC[listIC.Count - 1].LEndMeter))
            //{
            //    iEndKM = (int)double.Parse(listIC[listIC.Count - 1].LEndMeter);
            //}
            #endregion

            #region 计算TQI
            /*
             * 修正后的里程是连续的里程。
             * tqi的计算必须以修正后的里程为依据，否则原始里程存在有跳变的地方。没法计算tqi。
             */
            FileStream fs = new FileStream(sWaveFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            BinaryReader br = new BinaryReader(fs);
            List<TQIClass> listTQI = new List<TQIClass>();
            //long lReturn = GetIndexMeterPositon(listIC, lStartPosition, mi, jvli, iChannelNumber);
            for (int i = 0; i < listIC.Count; i++)
            {
                List<TQIMileClass> listWM = new List<TQIMileClass>();
                double dStartMile = 0d;
                double dEndMile = 0d;
                dStartMile = double.Parse(listIC[i].lStartMeter);
                dEndMile = double.Parse(listIC[i].LEndMeter);

                //test
                if (dStartMile == 93 && dEndMile == 95.055)
                {
                    int b = 0;
                }

                int iKM = (int)float.Parse(listIC[i].lStartMeter);
                int iMeter = (int)(float.Parse(listIC[i].lStartMeter) * 1000) - (iKM * 1000);
                if (sKmInc.Equals("增"))
                {
                    while (true)
                    {
                        TQIMileClass wm = new TQIMileClass();
                        int iMod = iMeter % 200;
                        //例如当起点为90.0，则应该把90.0归到0-200这一段  --ygx20151123
                        //if (iMod == 0)
                        //{
                        //    wm.iMeter = iMeter;
                        //} 
                        //else
                        //{
                        //    wm.iMeter = iMeter + (200 - iMod);
                        //}
                        wm.iMeter = iMeter + (200 - iMod);

                        wm.iKM = iKM;
                        if (listIC[i].sType.Equals("正常"))
                        {
                            if (wm.iMeter == 1000)
                            {
                                wm.iMeter = 0;
                                wm.iKM = iKM + 1;
                            }
                        }
                        else
                        {

                        }
                        wm.lPostion = GetNewIndexMeterPositon(listIC, wm.iKM * 1000 + wm.iMeter, gjtds, sKmInc, 0);
                        iMeter = wm.iMeter;
                        iKM = wm.iKM;
                        if ((iKM + iMeter / 1000f) < dEndMile)
                        { listWM.Add(wm); }
                        else
                        {
                            break;
                        }
                    }
                }
                else//jian
                {
                    if (listIC[i].sType.Equals("长链"))
                    {
                        iMeter = (int)(float.Parse(listIC[i].lContainsMeter) * 1000);
                    }
                    while (true)
                    {
                        TQIMileClass wm = new TQIMileClass();
                        int iMod = iMeter % 200;
                        wm.iMeter = iMeter - (iMod == 0 ? 200 : iMod);
                        wm.iKM = iKM;
                        if (listIC[i].sType.Equals("正常"))
                        {
                            if (wm.iMeter < 0)
                            {
                                wm.iMeter = 800;
                                wm.iKM = iKM - 1;
                            }
                        }
                        else
                        {

                        }
                        wm.lPostion = GetNewIndexMeterPositon(listIC, wm.iKM * 1000 + wm.iMeter, gjtds, sKmInc, 0);
                        iMeter = wm.iMeter;
                        iKM = wm.iKM;
                        if (wm.iMeter == 0 && listIC[i].sType.Equals("正常"))
                        {
                            wm.iMeter = 800;
                            wm.iKM -= 1;
                        }
                        else
                        {
                            wm.iMeter -= 200;
                        }
                        if ((iKM + iMeter / 1000f) > dEndMile)
                        { listWM.Add(wm); }
                        else
                        {
                            break;
                        }
                    }
                }
                //
                int iRate = (int)(200 / (float.Parse(listIC[i].lContainsMeter) * 1000 / listIC[i].lContainsPoint));
                for (int k = 0; k < listWM.Count; k++)
                {
                    if (listWM[k].lPostion == -1)
                    {
                        continue;
                    }
                    br.BaseStream.Position = listWM[k].lPostion;
                    double[][] fArray = new double[10][];
                    for (int j = 0; j < 10; j++)
                    {
                        fArray[j] = new double[iRate];
                    }
                    for (int l = 0; l < iRate; l++)
                    {
                        if (br.BaseStream.Position < br.BaseStream.Length)
                        {
                            byte[] b = br.ReadBytes(gjtds * 2);
                            if (bEncrypt)
                            {
                                b = ByteXORByte(b);
                            }
                            //处理数据通道
                            for (int n = 0; n < sTQIItemIndex.Length; n++)
                            {
                                float fValue = float.Parse((BitConverter.ToInt16(b, sTQIItemIndex[n] * 2)).ToString()) / f[sTQIItemIndex[n]];
                                //sb.Append("," + fValue.ToString("f2"));
                                fArray[n][l] = fValue;
                            }
                        }
                    }
                    //计算
                    TQIClass tqic = new TQIClass();
                    tqic.zgd = Math.Round(CalcStardard(fArray[0]),2);
                    tqic.ygd = Math.Round(CalcStardard(fArray[1]),2);
                    tqic.zgx = Math.Round(CalcStardard(fArray[2]),2);
                    tqic.ygx = Math.Round(CalcStardard(fArray[3]),2);
                    tqic.gj = Math.Round(CalcStardard(fArray[4]),2);
                    tqic.sp = Math.Round(CalcStardard(fArray[5]),2);
                    tqic.sjk = Math.Round(CalcStardard(fArray[6]),2);
                    tqic.hj = Math.Round(CalcStardard(fArray[7]),2);
                    tqic.cj = Math.Round(CalcStardard(fArray[8]),2);
                    tqic.pjsd = CalcAvgSpeed(fArray[9]);
                    tqic.iKM = listWM[k].iKM;
                    tqic.iMeter = listWM[k].iMeter;
                    listTQI.Add(tqic);
                }
            }
            br.Close();
            fs.Close();
            #endregion

            #region 插入TQI
            try
            {

                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sIICFileName + ";Persist Security Info=True"))
                {
                    string sqlCreate = "";
                    OleDbCommand sqlcom = new OleDbCommand(sqlCreate, sqlconn);
                    sqlconn.Open();
                    for (int i = 0; i < listTQI.Count; i++)
                    {
                        sqlcom.CommandText = "insert into fix_tqi " +
                            "values('"+subCode+"','"+runDate+"'," + listTQI[i].iKM.ToString()
                            + "," + listTQI[i].iMeter.ToString()
                            + "," + listTQI[i].zgd.ToString()
                            + "," + listTQI[i].ygd.ToString()
                            + "," + listTQI[i].zgx.ToString()
                            + "," + listTQI[i].ygx.ToString()
                            + "," + listTQI[i].gj.ToString()
                            + "," + listTQI[i].sp.ToString()
                            + "," + listTQI[i].sjk.ToString()
                            + "," + listTQI[i].GetTQISum().ToString()
                            + "," + listTQI[i].hj.ToString()
                            + "," + listTQI[i].cj.ToString()
                            + "," + listTQI[i].pjsd.ToString()
                            + "," + listTQI[i].iValid.ToString() + ")";
                        sqlcom.ExecuteNonQuery();
                    }
                    sqlconn.Close();
                }
                Application.DoEvents();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            #endregion

            #region 删除不在范围内的TQI
            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sIICFileName + ";Persist Security Info=True"))
                {
                    string sqlCreate = "";
                    if (sKmInc.Contains("增"))
                    {
                        sqlCreate = " FromPost<" + iStartKM.ToString() + " or FromPost>" + iEndKM.ToString();
                    }
                    else
                    {
                        sqlCreate = " FromPost>" + iStartKM.ToString() + " or FromPost<" + iEndKM.ToString();
                    }
                    OleDbCommand sqlcom = new OleDbCommand(sqlCreate, sqlconn);
                    sqlconn.Open();
                    sqlcom.CommandText = "delete from fix_tqi where " + sqlCreate;
                    sqlcom.ExecuteNonQuery();
                    sqlconn.Close();
                }
                Application.DoEvents();
            }
            catch
            {

            }
            #endregion

            #region 无效TQI滤除
            //重新计算的tqi，都是用小里程表示。---已经和赵主任确认-20140526
            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sIICFileName + ";Persist Security Info=True"))
                {
                    string sqlCreate = "";
                    OleDbCommand sqlcom = new OleDbCommand(sqlCreate, sqlconn);
                    sqlconn.Open();
                    for (int iVar = 0; iVar < listIDC.Count; iVar++)
                    {
                        int iStartMeter = PointToMeter(listIC, long.Parse(listIDC[iVar].sStartPoint), gjtds, sKmInc);
                        int iEndMeter = PointToMeter(listIC, long.Parse(listIDC[iVar].sEndPoint), gjtds, sKmInc);
                        //根据点获取里程

                        if (sKmInc.Contains("增"))
                        {
                            sqlCreate = " (FromPost*1000+fromminor)>=" + (iStartMeter - 200).ToString() +
                                " and (FromPost*1000+fromminor)<=" + (iEndMeter).ToString();
                        }
                        else
                        {
                            sqlCreate = "  (FromPost*1000+fromminor)<=" + (iStartMeter).ToString() +
                                " and (FromPost*1000+fromminor)>=" + (iEndMeter - 200).ToString();   /*减里程时，iEndMeter是小里程*/
                        }
                        sqlcom.CommandText = "update fix_tqi set valid=0 where " + sqlCreate;
                        int tmp=sqlcom.ExecuteNonQuery();
                    }
                    sqlconn.Close();
                }
                Application.DoEvents();
            }
            catch
            {

            }
            #endregion

        }

        #region TQI拷贝，把车上计算出来的tqi由纵向排列变为横向排列--ygx--20140320
        /// <summary>
        /// TQI拷贝，把车上计算出来的tqi由纵向排列变为横向排列
        /// </summary>
        /// <param name="sIICFileName">iic文件名</param>
        /// <param name="sKmInc">增减里程</param>
        /// <param name="listIDC">无效区段</param>
        public void TQICopy(string sIICFileName,string sKmInc, List<InvalidDataClass> listIDC)
        {
            int iKMLast = 0;
            int iMeterLast = 0;
            int iKMNow = 0;
            int iMeterNow = 0;
            String channelNameGeo = null;
            double channelTqiValueGeo = 0.0d;
            float basePost = 0f;
            int baseMinor = 0;

            //geo文件中的通道名
            string[] sTQIItem = new string[] { "L_STDSURF", "R_STDSURF", "L_STDALIGN", 
                "R_STDALIGN", "STDGAUGE", "STDCROSSLEVEL", "STDTWIST", "STDLATACCEL", "STDVERTACCEL","MAXSPEED"};

            List<TQIClass> listTQI = new List<TQIClass>();
            TQIClass tqiCls = new TQIClass();

            String subCode = null; //ashx
            DateTime runDate = DateTime.Now;

            

            #region 获取TQI，并写入到listTQI
            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sIICFileName + ";Persist Security Info=True"))
                {                    
                    string sqlCreate = "select FromPost,FromMinor,TQIMetricName,TQIValue,BasePost,SubCode,RunDate from tqi order by BasePost";
                    OleDbCommand sqlcom = new OleDbCommand(sqlCreate, sqlconn);
                    sqlconn.Open();
                    OleDbDataReader oleDBdr = sqlcom.ExecuteReader();
                    
                    if (!oleDBdr.HasRows)
                    {
                        MessageBox.Show("TQI列表为空！");
                        return;
                    }
                    
                    while(oleDBdr.Read())
                    {
                        basePost = float.Parse(oleDBdr.GetValue(4).ToString());
                        baseMinor = (int)(Math.Round(basePost,2) * 1000) % 1000;
                        iKMNow = (int)basePost;
                        iMeterNow = baseMinor;
                        channelNameGeo = oleDBdr.GetValue(2).ToString();
                        channelTqiValueGeo = Math.Round(double.Parse(oleDBdr.GetValue(3).ToString()),2);

                        subCode = oleDBdr.GetValue(5).ToString();
                        runDate = DateTime.Parse(oleDBdr.GetValue(6).ToString());

                        if ((iKMNow != iKMLast) || (iMeterNow != iMeterLast))
                        {
                            tqiCls = new TQIClass();

                            listTQI.Add(tqiCls);

                            tqiCls.iKM = iKMNow;
                            tqiCls.iMeter = iMeterNow;

                            tqiCls.subCode = subCode;
                            tqiCls.runDate = runDate;

                            iKMLast = iKMNow;
                            iMeterLast = iMeterNow;
                        }

                        #region TQI赋值
                        foreach (String tqiName in sTQIItem)
                        {
                            if (tqiName == channelNameGeo)
                            {
                                //左高低
                                if (channelNameGeo == "L_STDSURF")
                                {
                                    tqiCls.zgd = channelTqiValueGeo;
                                }
                                //右高低
                                if (channelNameGeo == "R_STDSURF")
                                {
                                    tqiCls.ygd = channelTqiValueGeo;
                                }
                                //左轨向
                                if (channelNameGeo == "L_STDALIGN")
                                {
                                    tqiCls.zgx = channelTqiValueGeo;
                                }
                                //右轨向
                                if (channelNameGeo == "R_STDALIGN")
                                {
                                    tqiCls.ygx = channelTqiValueGeo;
                                }
                                //轨距
                                if (channelNameGeo == "STDGAUGE")
                                {
                                    tqiCls.gj = channelTqiValueGeo;
                                }
                                //水平
                                if (channelNameGeo == "STDCROSSLEVEL")
                                {
                                    tqiCls.sp = channelTqiValueGeo;
                                }
                                //三角坑
                                if (channelNameGeo == "STDTWIST")
                                {
                                    tqiCls.sjk = channelTqiValueGeo;
                                }
                                //车体横向加速度
                                if (channelNameGeo == "STDLATACCEL")
                                {
                                    tqiCls.hj = channelTqiValueGeo;
                                }
                                //车体垂向加速度
                                if (channelNameGeo == "STDVERTACCEL")
                                {
                                    tqiCls.cj = channelTqiValueGeo;
                                }
                                //平均速度
                                if (channelNameGeo == "MAXSPEED")
                                {
                                    tqiCls.pjsd = (int)channelTqiValueGeo;
                                }


                                break;
                            }
                        }
                        #endregion
                    }
                    sqlconn.Close();
                }
                Application.DoEvents();
            }
            catch
            {

            }
            #endregion

            #region 插入TQI
            try
            {

                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sIICFileName + ";Persist Security Info=True"))
                {
                    string sqlCreate = "";
                    OleDbCommand sqlcom = new OleDbCommand(sqlCreate, sqlconn);
                    sqlconn.Open();
                    for (int i = 0; i < listTQI.Count; i++)
                    {
                        sqlcom.CommandText = "insert into fix_tqi " +
                            "values('" + listTQI[i].subCode + "','" + listTQI[i].runDate + "'," + listTQI[i].iKM.ToString()
                            + "," + listTQI[i].iMeter.ToString()
                            + "," + listTQI[i].zgd.ToString()
                            + "," + listTQI[i].ygd.ToString()
                            + "," + listTQI[i].zgx.ToString()
                            + "," + listTQI[i].ygx.ToString()
                            + "," + listTQI[i].gj.ToString()
                            + "," + listTQI[i].sp.ToString()
                            + "," + listTQI[i].sjk.ToString()
                            + "," + listTQI[i].GetTQISum().ToString()
                            + "," + listTQI[i].hj.ToString()
                            + "," + listTQI[i].cj.ToString()
                            + "," + listTQI[i].pjsd.ToString()
                            + "," + listTQI[i].iValid.ToString() + ")";
                        sqlcom.ExecuteNonQuery();
                    }
                    sqlconn.Close();
                }
                Application.DoEvents();
            }
            catch
            {

            }
            #endregion            

            #region 删除无效的TQI
            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sIICFileName + ";Persist Security Info=True"))
                {
                    string sqlCreate = "TQISum_Value=0 or AVERAGE_SPEED=0";
                    OleDbCommand sqlcom = new OleDbCommand(sqlCreate, sqlconn);
                    sqlconn.Open();
                    sqlcom.CommandText = "delete from fix_tqi where " + sqlCreate;
                    sqlcom.ExecuteNonQuery();
                    sqlconn.Close();
                }
                Application.DoEvents();
            }
            catch
            {

            }
            #endregion
            

        }
        #endregion

        #region 把无效区的tqi置无效---ygx--20140422
        /// <summary>
        /// 把无效区的tqi置无效---ygx--20140422---适用于未修正的iic（车上的tqi）
        /// </summary>
        /// <param name="sIICFileName"></param>
        /// <param name="sKmInc"></param>
        /// <param name="listIDC"></param>
        public void TQIInvalid(string sIICFileName, string sKmInc, List<InvalidDataClass> listIDC)
        {
            #region 无效TQI滤除
            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sIICFileName + ";Persist Security Info=True"))
                {
                    string sqlCreate = "";
                    OleDbCommand sqlcom = new OleDbCommand(sqlCreate, sqlconn);
                    sqlconn.Open();
                    for (int iVar = 0; iVar < listIDC.Count; iVar++)
                    {
                        int iStartMeter = (int)(float.Parse(listIDC[iVar].sStartMile) * 1000);
                        int iEndMeter = (int)(float.Parse(listIDC[iVar].sEndMile) * 1000);
                        //根据点获取里程

                        if (sKmInc.Contains("增"))
                        {
                            sqlCreate = " (FromPost*1000+fromminor)>=" + (iStartMeter-200).ToString() +
                                " and (FromPost*1000+fromminor)<=" + (iEndMeter).ToString();
                        }
                        else
                        {
                            sqlCreate = "  (FromPost*1000+fromminor)<=" + (iStartMeter + 200).ToString() +
                                " and (FromPost*1000+fromminor)>=" + (iEndMeter).ToString();
                        }
                        sqlcom.CommandText = "update fix_tqi set valid=0 where " + sqlCreate;
                        sqlcom.ExecuteNonQuery();
                    }
                    sqlconn.Close();
                }
                Application.DoEvents();
            }
            catch
            {

            }
            #endregion
        }
        #endregion

        //左右高低，左右轨向对调
        public void IICChannelSwap(String iicFilePath, String tableName, String column_A,String channel_A, String column_B,String channel_B)
        {
            #region 把一列中的某两个值对调
            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + iicFilePath + ";Persist Security Info=True"))
                {
                    string sqlCreate = "";
                    OleDbCommand sqlcom = new OleDbCommand(sqlCreate, sqlconn);
                    sqlconn.Open();

                    String sql = string.Format("update {0} set {1}='{2}' where {3}='{4}'", tableName, column_A, "Temp", column_B,channel_A);
                    sqlcom.CommandText = sql;
                    sqlcom.ExecuteNonQuery();

                    sql = string.Format("update {0} set {1}='{2}' where {3}='{4}'", tableName, column_A, channel_A, column_B, channel_B);
                    sqlcom.CommandText = sql;
                    sqlcom.ExecuteNonQuery();

                    sql = string.Format("update {0} set {1}='{2}' where {3}='{4}'", tableName, column_A, channel_B, column_B, "Temp");
                    sqlcom.CommandText = sql;
                    sqlcom.ExecuteNonQuery();

                    sqlconn.Close();
                }
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            #endregion
        }
        //幅值乘(-1)
        public void IICChannelFlip(String iicFilePath, String tableName, String column_A, String channel_A, String column_B, String channel_B)
        {
            #region 幅值乘(-1)
            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + iicFilePath + ";Persist Security Info=True"))
                {
                    string sqlCreate = "";
                    OleDbCommand sqlcom = new OleDbCommand(sqlCreate, sqlconn);
                    sqlconn.Open();

                    String sql = string.Format("update {0} set {1}={2}*(-1) where {3}='{4}'", tableName, column_A, channel_A, column_B,channel_B);
                    sqlcom.CommandText = sql;
                    sqlcom.ExecuteNonQuery();

                    sqlconn.Close();
                }
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            #endregion
        }

        //IIC中的tqi中的里程统一减去0.2：单线由减里程调整为增里程后，200代表200--400
        public void IICTqi(String iicFilePath, String tableName, String column_A, String channel_A)
        {
            #region IIC中的tqi中的里程统一减去0.2：单线由减里程调整为增里程后，200代表200--400
            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + iicFilePath + ";Persist Security Info=True"))
                {
                    string sqlCreate = "";
                    OleDbCommand sqlcom = new OleDbCommand(sqlCreate, sqlconn);
                    sqlconn.Open();

                    String sql = string.Format("update {0} set {1}={2}-0.2", tableName, column_A, channel_A);
                    sqlcom.CommandText = sql;
                    sqlcom.ExecuteNonQuery();

                    sqlconn.Close();
                }
                Application.DoEvents();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            #endregion
        }



        public int CalcAvgSpeed(double[] dSpeed)
        {
            int iSpeed = 0;
            double dSum = 0.0;
            for (int i = 0; i < dSpeed.Length; i++)
            {
                dSum += dSpeed[i];
            }
            iSpeed = (int)(dSum / dSpeed.Length);
            return iSpeed;
        }
        #region 偏差类
        /// <summary>
        /// 偏差类
        /// </summary>
        public class DefectsClass
        {
            public int iRecordNumber = 0;
            /// <summary>
            /// 单位为公里
            /// </summary>
            public int iMaxpost = 0;
            /// <summary>
            /// 单位为米
            /// </summary>
            public double dMaxminor = 0;
            public bool bFix = false;
            /// <summary>
            /// 获取公里标，单位为厘米
            /// </summary>
            /// <returns></returns>
            public int GetMeter()
            {
                return iMaxpost * 100000 + (int)(dMaxminor * 100);
            }
        }
        #endregion
        //偏差类型
        public class ExceptionTypeClass
        {
            public string EXCEPTIONEN = "";
            public string EXCEPTIONCN = "";
        }
        //偏差修正
        public void ExceptionFix(string sWaveFileName, string sIICFileName, List<IndexStaClass> listIC, int cyjg, int gjtds, bool bEncrypt, string sKmInc, List<WaveformDataProcess.ExceptionTypeClass> listETC)
        {
            List<DefectsClass> listDC = new List<DefectsClass>();
            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sIICFileName + ";Persist Security Info=True"))
                {
                    string sqlCreate = "select RecordNumber,maxpost,maxminor from fix_defects where maxval2 is null or maxval2<>-200";
                    OleDbCommand sqlcom = new OleDbCommand(sqlCreate, sqlconn);
                    sqlconn.Open();
                    OleDbDataReader oleDBdr = sqlcom.ExecuteReader();
                    while (oleDBdr.Read())
                    {
                        DefectsClass dc = new DefectsClass();
                        dc.iRecordNumber = int.Parse(oleDBdr.GetValue(0).ToString());
                        dc.iMaxpost = int.Parse(oleDBdr.GetValue(1).ToString());
                        dc.dMaxminor = double.Parse(oleDBdr.GetValue(2).ToString());
                        listDC.Add(dc);
                    }

                    oleDBdr.Close();
                    sqlconn.Close();
                }
                Application.DoEvents();
            }
            catch
            {

            }

            //
            List<cPointFindMeter> listcpfm = MeterToPoint(sWaveFileName, cyjg, gjtds, bEncrypt);
            for (int i = 0; i < listDC.Count; i++)
            {
                //里程找点
                for (int j = 0; j < listcpfm.Count; j++)
                {
                    if (listcpfm[j].lMeter == listDC[i].GetMeter())
                    {
                        int iValue = PointToMeter(listIC, listcpfm[j].lLoc, gjtds, sKmInc);
                        if (iValue > 0)
                        {
                            listDC[i].bFix = true;
                            listDC[i].iMaxpost = iValue / 1000;
                            listDC[i].dMaxminor = iValue % 1000;
                        }
                        break;
                    }
                }
            }
            //将修正后的偏差数据存储到iic中
            
            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sIICFileName + ";Persist Security Info=True"))
                {
                    string sqlCreate = "";
                    OleDbCommand sqlcom = new OleDbCommand(sqlCreate, sqlconn);
                    sqlconn.Open();
                    for (int i = 0; i < listDC.Count; i++)
                    {
                        if (listDC[i].bFix)
                        {
                            sqlcom.CommandText = "update fix_defects set maxpost=" + listDC[i].iMaxpost.ToString() +
                                ",maxminor=" + listDC[i].dMaxminor.ToString() + ",maxval2=-200 where RecordNumber=" + listDC[i].iRecordNumber.ToString();
                            sqlcom.ExecuteNonQuery();
                        }
                    }
                    sqlconn.Close();
                }
                Application.DoEvents();
            }
            catch
            {

            }

        }

        private void IICToCSV(string sFile, string sTable, string sType, string sOrderBy, List<ExceptionTypeClass> listETC)
        {
            //保存为csv
            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sFile + ";Persist Security Info=True"))
                {
                    string sqlCreate = "";
                    OleDbCommand sqlcom = new OleDbCommand(sqlCreate, sqlconn);
                    sqlconn.Open();
                    sqlcom.CommandText = "select * from " + sTable + sOrderBy;
                    OleDbDataReader oledbdr = sqlcom.ExecuteReader();
                    StreamWriter sw = new StreamWriter(sFile + "." + sType + ".csv", false, Encoding.Default);
                    sw.AutoFlush = true;
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < oledbdr.FieldCount; i++)
                    {
                        sb.Append(oledbdr.GetName(i) + ",");
                    }
                    sw.WriteLine(sb.ToString().TrimEnd(','));
                    while (oledbdr.Read())
                    {
                        sb = new StringBuilder();
                        for (int i = 0; i < oledbdr.FieldCount; i++)
                        {
                            if (listETC.Count > 0)
                            {
                                if (i == 5)
                                {
                                    string sSubType = oledbdr.GetValue(i).ToString();
                                    for (int k = 0; k < listETC.Count; k++)
                                    {
                                        if (sSubType.Equals(listETC[k].EXCEPTIONEN))
                                        {
                                            sb.Append(listETC[k].EXCEPTIONCN + ",");
                                            break;
                                        }
                                    }
                                }
                                else
                                {
                                    sb.Append(oledbdr.GetValue(i).ToString() + ",");
                                }
                            }
                            else
                            {
                                sb.Append(oledbdr.GetValue(i).ToString() + ",");
                            }
                        }
                        sw.WriteLine(sb.ToString().TrimEnd(','));
                        Application.DoEvents();
                    }
                    oledbdr.Close();
                    sw.Close();
                    sqlconn.Close();
                }
                Application.DoEvents();
            }
            catch
            {

            }
        }

        #region 获取文件中的每一个采样点的文件指针和该点的公里标(单位为厘米)。
        /// <summary>
        /// 获取文件中的每一个采样点的文件指针和该点的公里标(单位为厘米)。
        /// 返回值：cPointFindMeter的list
        /// </summary>
        /// <param name="FileName">CIT文件名</param>
        /// <param name="cyjg">采样频率(一米4个点)</param>
        /// <param name="gjtds">通道总数</param>
        /// <param name="bEncrypt">是否加密</param>
        /// <returns></returns>
        public List<cPointFindMeter> MeterToPoint(string FileName, int cyjg, int gjtds, bool bEncrypt)
        {
            try
            {
                List<cPointFindMeter> listcpfm = new List<cPointFindMeter>();
                FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(fs);
                byte[] b = new byte[gjtds * 2];
                //long p = 0;
                br.ReadBytes(120);//120
                br.ReadBytes(65 * gjtds);//65
                br.ReadBytes(BitConverter.ToInt32(br.ReadBytes(4), 0));
                int i1 = 0, i2 = 2;
                while (br.BaseStream.Position < br.BaseStream.Length)
                {
                    cPointFindMeter cpfm = new cPointFindMeter();

                    cpfm.lLoc = br.BaseStream.Position;
                    b = br.ReadBytes(gjtds * 2);
                    if (bEncrypt)
                    {
                        b = ByteXORByte(b);
                    }

                    short gongli = BitConverter.ToInt16(b, i1);
                    short mi1 = BitConverter.ToInt16(b, i2);
                    cpfm.lMeter = (int)(gongli * 100000 + (mi1 / (float)cyjg * 100));
                    listcpfm.Add(cpfm);
                }
                br.Close();
                fs.Close();
                return listcpfm;
            }
            catch
            {
                return new List<cPointFindMeter>();
            }
        }
        #endregion

        #region 根据里程(单位为厘米)定位点(文件中的指针)
        /// <summary>
        /// 根据里程(单位为厘米)定位点(文件中的指针)
        /// 返回值：文件指针
        /// </summary>
        /// <param name="FileName">cit文件</param>
        /// <param name="mi">里程：单位为厘米</param>
        /// <param name="cyjg">采样频率：一米4个点</param>
        /// <param name="gjtds">通道总数</param>
        /// <param name="bEncrypt">是否加密</param>
        /// <returns>文件指针</returns>
        public long MeterToPoint(string FileName, long mi, int cyjg, int gjtds, bool bEncrypt)
        {
            try
            {
                FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(fs);
                byte[] b = new byte[gjtds * 2];
                //long p = 0;
                br.ReadBytes(120);//120
                br.ReadBytes(65 * gjtds);//65
                br.ReadBytes(BitConverter.ToInt32(br.ReadBytes(4), 0));
                long curlc = 0;
                long findpos = 0;
                int i1 = 0, i2 = 2;
                while (true)
                {
                    findpos = br.BaseStream.Position;
                    b = br.ReadBytes(gjtds * 2);
                    if (bEncrypt)
                    {
                        b = ByteXORByte(b);
                    }

                    short gongli = BitConverter.ToInt16(b, i1);
                    short mi1 = BitConverter.ToInt16(b, i2);
                    curlc = (int)(gongli * 100000 + (mi1 / (float)cyjg * 100));

                    if (curlc == mi)
                    {
                        break;
                    }

                }
                br.Close();
                fs.Close();
                return findpos;
            }
            catch
            {
                return 0;
            }
        }
        #endregion

        #region 根据里程点(公里)计算出波形移动的距离--拖动条的值---里程未修正
        /// <summary>
        /// 根据里程点(公里)计算出波形移动的距离--拖动条的值
        /// </summary>
        /// <param name="FileName">cit全路径文件名</param>
        /// <param name="currentMeter">里程标(单位为米)</param>
        /// <param name="xZoomIn">x轴放大倍数</param>
        /// <param name="cyjg">采样频率</param>
        /// <param name="gjtds">轨检通道数</param>
        /// <param name="datatype">数据类型</param>
        /// <param name="bEncrypt">是否加密</param>
        /// <returns>修正波形需要移动的波形下方的拖动条的值</returns>
        public int AnalyseDataAutoFindCorLC(string FileName, long currentMeter, int xZoomIn, int cyjg, int gjtds, int datatype, bool bEncrypt)
        {
            int p = 0;//修正波形需要移动的波形下方的拖动条的值
            long startPos = 0;//通道数据起始位置所对应的文件指针
            try
            {
                FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(fs);
                byte[] b = new byte[gjtds * 2];
                //long p = 0;
                br.ReadBytes(120);//120
                br.ReadBytes(65 * gjtds);//65
                br.ReadBytes(BitConverter.ToInt32(br.ReadBytes(4), 0));

                startPos = br.BaseStream.Position;

                long curlc = 0;
                long findpos = 0;
                int i1 = 0, i2 = 2;
                //这里有点疑问，严广学
                /*
                 * 
                 * 
                 */
                if (datatype == 1) //DataType：1轨检、2动力学、3弓网，
                {
                    i1 = 2;
                    i2 = 4;
                }

                while (true)
                {
                    findpos = br.BaseStream.Position;
                    b = br.ReadBytes(gjtds * 2);
                    if (bEncrypt)
                    {
                        b = ByteXORByte(b);
                    }

                    short gongli = BitConverter.ToInt16(b, i1);//单位：千米
                    short mi1 = BitConverter.ToInt16(b, i2);//单位：采样点的采样次数
                    curlc = (int)(gongli * 1000 + (mi1 / (float)cyjg));//单位：米

                    //文件中找到里程标
                    if (curlc == currentMeter)
                    {
                        break;
                    }

                }


                p = (int)(((((findpos - startPos) / ((long)(xZoomIn / 10))) / cyjg) / 2L) / ((long)gjtds));
                br.Close();
                fs.Close();

            }
            catch
            {
                p = 0;
            }
            return p;
        }
        #endregion
        #region 根据里程点(公里)计算出波形移动的距离--拖动条的值---里程未修正--NEW
        /// <summary>
        /// 根据里程点(公里)计算出波形移动的距离--拖动条的值
        /// </summary>
        /// <param name="FileName">cit全路径文件名</param>
        /// <param name="currentMeter">里程标(单位为米)</param>
        /// <param name="xZoomIn">x轴放大倍数</param>
        /// <param name="cyjg">采样频率</param>
        /// <param name="gjtds">轨检通道数</param>
        /// <param name="datatype">数据类型</param>
        /// <param name="bEncrypt">是否加密</param>
        /// <returns>修正波形需要移动的波形下方的拖动条的值</returns>
        public int AnalyseDataAutoFindCorLC_New(string FileName, long currentMeter, int xZoomIn, int cyjg, int gjtds, int datatype, bool bEncrypt,out long meterFindPos)
        {
            int p = 0;//修正波形需要移动的波形下方的拖动条的值
            long startPos = 0;//通道数据起始位置所对应的文件指针
            long findpos = 0;
            try
            {
                FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(fs);
                byte[] b = new byte[gjtds * 2];
                //long p = 0;
                br.ReadBytes(120);//120
                br.ReadBytes(65 * gjtds);//65
                br.ReadBytes(BitConverter.ToInt32(br.ReadBytes(4), 0));

                startPos = br.BaseStream.Position;

                long curlc = 0;
                findpos = 0;
                int i1 = 0, i2 = 2;
                //这里有点疑问，严广学
                /*
                 * 
                 * 
                 */
                if (datatype == 1) //DataType：1轨检、2动力学、3弓网，
                {
                    i1 = 2;
                    i2 = 4;
                }

                while (true)
                {
                    findpos = br.BaseStream.Position;
                    b = br.ReadBytes(gjtds * 2);
                    if (bEncrypt)
                    {
                        b = ByteXORByte(b);
                    }

                    short gongli = BitConverter.ToInt16(b, i1);//单位：千米
                    short mi1 = BitConverter.ToInt16(b, i2);//单位：采样点的采样次数
                    curlc = (int)(gongli * 1000 + (mi1 / (float)cyjg));//单位：米

                    //文件中找到里程标
                    if (curlc == currentMeter)
                    {
                        break;
                    }

                }


                p = (int)(((((findpos - startPos) / ((long)(xZoomIn / 10))) / cyjg) / 2L) / ((long)gjtds));
                br.Close();
                fs.Close();
                
            }
            catch
            {
                p = 0;
            }

            meterFindPos = findpos;
            return p;
        }
        #endregion
        
        #region 根据里程修正点(文件指针)计算出波形移动的距离--拖动条的值---20140106--ygx
        /// <summary>
        /// 根据里程修正点(文件指针)计算出波形移动的距离--拖动条的值
        /// </summary>
        /// <param name="FileName">cit全路径文件名</param>
        /// <param name="currentMeter">文件指针</param>
        /// <param name="xZoomIn">x轴放大倍数</param>
        /// <param name="cyjg">采样频率</param>
        /// <param name="gjtds">轨检通道数</param>
        /// <returns>修正波形需要移动的波形下方的拖动条的值</returns>
        public int AnalyseDataAutoFindCorLC(string FileName, long currentPos, int xZoomIn, int cyjg, int gjtds)
        {
            int p = 0;//修正波形需要移动的波形下方的拖动条的值
            long startPos = 0;//通道数据起始位置所对应的文件指针

            p = (int)(((((currentPos - startPos) / ((long)(xZoomIn / 10))) / cyjg) / 2L) / ((long)gjtds));

            return p;
        }
        #endregion
        

        #region 根据索引修正点，计算出波形移动的距离---修正的采样点的个数
        /// <summary>
        /// 根据索引修正点，计算出波形移动的距离---修正的采样点的个数
        /// </summary>
        /// <param name="FileName">cit文件名</param>
        /// <param name="currentMeter">修正点的位置（单位：米）</param>
        /// <param name="xZoomIn">X轴的放大倍数</param>
        /// <param name="cyjg">cit文件的采样率(一般为4：表示1米采样4个点)</param>
        /// <param name="gjtds">cit文件通道数</param>
        /// <param name="datatype">文件中的数据类型：1轨检、2动力学、3弓网，</param>
        /// <param name="iCur">拖动框的游标值</param>
        /// <returns>修正的采样点的个数</returns>
        public int AutoFindCorLC(string FileName, long currentMeter, int xZoomIn, int cyjg, int gjtds, int datatype, int iCur)
        {
            int p = 0; //修正的采样点的个数
            long startPos = 0;//通道数据起始位置所对应的文件指针
            try
            {
                FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(fs);
                byte[] b = new byte[gjtds * 2];
                //long p = 0;
                br.ReadBytes(120);//120
                br.ReadBytes(65 * gjtds);//65
                br.ReadBytes(BitConverter.ToInt32(br.ReadBytes(4), 0));
                startPos = br.BaseStream.Position;
                long curlc = 0;
                long findpos = 0; //文件指针
                int i1 = 0, i2 = 2;
                //这里有点疑问，严广学
                if (datatype == 1) //iDataType：1轨检、2动力学、3弓网，
                {
                    i1 = 2;
                    i2 = 4;
                }
                while (br.BaseStream.Position < br.BaseStream.Length)
                {
                    findpos = br.BaseStream.Position;
                    b = br.ReadBytes(gjtds * 2);
                    short gongli = BitConverter.ToInt16(b, i1);
                    short mi1 = BitConverter.ToInt16(b, i2);
                    curlc = (int)(gongli * 1000 + (mi1 / (float)cyjg));

                    if (curlc == currentMeter)
                    {
                        break;
                    }

                }
                p = (int)((findpos - startPos) / (cyjg * gjtds * 2)) / (xZoomIn / 10);
                p = (int)((p - iCur) * (xZoomIn / 10) * cyjg + (p - iCur) * cyjg);//为什么要+ (p - iCur) * cyjg  --严广学
                br.Close();
                fs.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                p = 0;
            }
            return p;
        }
        #endregion

        public long AnalyseDataLCGJAuto(long startPos, long endPos, string FileName, int jvli, int cyjg, int gjtds)
        {
            try
            {
                FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(fs);
                byte[] b = new byte[gjtds * 2];
                long p = 0;
                br.ReadBytes(120);
                br.ReadBytes(65 * gjtds);
                br.ReadBytes(BitConverter.ToInt32(br.ReadBytes(4), 0));
                p = br.BaseStream.Position;

                long lastLength = br.BaseStream.Length - ((gjtds * 2) * cyjg * jvli);

                if (lastLength - p > 0)
                {
                    return lastLength;
                }
                else
                {
                    return p;
                }

            }
            catch
            {
                return 0;
            }
        }
    }
    #endregion
}
