using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using InGraph.Classes;
using System.Windows.Forms;

namespace InGraph
{
    class TestClass
    {
        //轨检数据文件结构
        struct GuijianStructs
        {
            public short gongli;
            public short mi;
            public short c0;
            public short c1;
            public short c2;
            public short c3;
            public short c4;
            public short c5;
            public short c6;
            public short c7;
            public short c8;
            public short c9;
            public short c10;
            public short c11;
        }

        public bool AnalyseData(string FileName, short gongli, short mi, int jvli)
        {
            try
            {
                FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(fs);
                byte[] b = new byte[28];
                int i = 0;
                long p = 0;
                if (gongli == 0)
                {
                    if (mi == 0)
                    {
                    }
                    else
                    {
                        p = 28 * 4 * mi;
                    }

                }
                else
                {
                    if (mi == 0)
                    {
                        p = 28 * 4 * gongli;
                    }
                    else
                    {
                        p = 28 * 4 * gongli  * mi;
                    }
                }
                br.BaseStream.Seek(p, SeekOrigin.Begin);
                while (i < jvli * 4)
                {
                    b = br.ReadBytes(28);
                    GuijianStructs gs;
                    gs.gongli = BitConverter.ToInt16(b, 0);
                    gs.mi = BitConverter.ToInt16(b, 2);
                    gs.c0 = BitConverter.ToInt16(b, 4);
                    gs.c1 = BitConverter.ToInt16(b, 6);
                    gs.c2 = BitConverter.ToInt16(b, 8);
                    gs.c3 = BitConverter.ToInt16(b, 10);
                    gs.c4 = BitConverter.ToInt16(b, 12);
                    gs.c5 = BitConverter.ToInt16(b, 14);
                    gs.c6 = BitConverter.ToInt16(b, 16);
                    gs.c7 = BitConverter.ToInt16(b, 18);
                    gs.c8 = BitConverter.ToInt16(b, 20);
                    gs.c9 = BitConverter.ToInt16(b, 22);
                    gs.c10 = BitConverter.ToInt16(b, 24);
                    gs.c11 = BitConverter.ToInt16(b, 26);
                    CommonClass.PCCommon.Channel_1[i].X = (float)i;
                    CommonClass.PCCommon.Channel_2[i].X = (float)i;
                    CommonClass.PCCommon.Channel_3[i].X = (float)i;
                    CommonClass.PCCommon.Channel_4[i].X = (float)i;
                    CommonClass.PCCommon.Channel_5[i].X = (float)i;
                    CommonClass.PCCommon.Channel_6[i].X = (float)i;
                    CommonClass.PCCommon.Channel_7[i].X = (float)i;
                    CommonClass.PCCommon.Channel_8[i].X = (float)i;
                    CommonClass.PCCommon.Channel_9[i].X = (float)i;
                    CommonClass.PCCommon.Channel_10[i].X = (float)i;
                    CommonClass.PCCommon.Channel_11[i].X = (float)i;
                    CommonClass.PCCommon.Channel_12[i].X = (float)i;
                    CommonClass.PCCommon.Channel_1[i].Y = float.Parse((gs.c0 / 100.0).ToString());
                    CommonClass.PCCommon.Channel_2[i].Y = float.Parse((gs.c1 / 100.0).ToString());
                    CommonClass.PCCommon.Channel_3[i].Y = float.Parse((gs.c2 / 100.0).ToString());
                    CommonClass.PCCommon.Channel_4[i].Y = float.Parse((gs.c3 / 100.0).ToString());
                    CommonClass.PCCommon.Channel_5[i].Y = float.Parse((gs.c4 / 100.0).ToString());
                    CommonClass.PCCommon.Channel_6[i].Y = float.Parse((gs.c5 / 100.0).ToString());
                    CommonClass.PCCommon.Channel_7[i].Y = float.Parse((gs.c6 / 100.0).ToString());
                    CommonClass.PCCommon.Channel_8[i].Y = float.Parse((gs.c7 / 100.0).ToString());
                    CommonClass.PCCommon.Channel_9[i].Y = float.Parse((gs.c8 / 100.0).ToString());
                    CommonClass.PCCommon.Channel_10[i].Y = float.Parse((gs.c9 / 1000.0).ToString());
                    CommonClass.PCCommon.Channel_11[i].Y = float.Parse((gs.c10 / 1000.0).ToString());
                    CommonClass.PCCommon.Channel_12[i].Y = float.Parse((gs.c11 / 100.0).ToString());
                    i++;

                }
                fs.Close();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
            }
            return true;

        }
        public bool AnalyseData1(string FileName, short gongli, short mi, int jvli)
        {
            try{
            FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            BinaryReader br = new BinaryReader(fs);
            byte[] b = new byte[52];
            int i = 0;
            long p = 0;

                if (mi == 0)
                {
                }
                else
                {
                    p = 52  * mi*2000;
                }

 

            br.BaseStream.Seek(p, SeekOrigin.Begin);
         
                while (i < jvli * 4)
                {
           
                        b = br.ReadBytes(52);
                        BitConverter.ToInt16(b, 0);
                        BitConverter.ToInt16(b, 2);
                        BitConverter.ToInt16(b, 4);
                        BitConverter.ToInt16(b, 6);
                        BitConverter.ToInt16(b, 8);
                        BitConverter.ToInt16(b, 10);
                        BitConverter.ToInt16(b, 12);
                        BitConverter.ToInt16(b, 14);
                        BitConverter.ToInt16(b, 16);
                        CommonClass.PCCommon.Channel_13[i].Y = BitConverter.ToInt16(b, 18) * 0.01526f - 248.3f + 0.5f;//
                        CommonClass.PCCommon.Channel_14[i].Y = (BitConverter.ToInt16(b, 20) * 0.01526f - 247.5f + 0.5f)/10.0f;//
                        CommonClass.PCCommon.Channel_15[i].Y = (BitConverter.ToInt16(b, 22) * 0.1709f + 4494 + 0.5f)/100.0f;//
                        BitConverter.ToInt16(b, 24);
                        CommonClass.PCCommon.Channel_16[i].Y = (BitConverter.ToInt16(b, 26) * 0.1f)/1.0f;//
                        BitConverter.ToInt32(b, 28);
                        BitConverter.ToInt16(b, 32);
                        BitConverter.ToInt16(b, 34);
                        BitConverter.ToInt16(b, 36);
                        BitConverter.ToInt16(b, 38);
                        BitConverter.ToInt16(b, 40);
                        BitConverter.ToInt16(b, 42);
                        BitConverter.ToInt16(b, 44);
                        BitConverter.ToInt16(b, 46);
                        BitConverter.ToInt16(b, 48);
                        BitConverter.ToInt16(b, 50);
                        CommonClass.PCCommon.Channel_13[i].X = (float)i;
                        CommonClass.PCCommon.Channel_14[i].X = (float)i;
                        CommonClass.PCCommon.Channel_15[i].X = (float)i;
                        CommonClass.PCCommon.Channel_16[i].X = (float)i;
                        i++;
                   
                }
            
            fs.Close();
            }
            catch (Exception ex)
            {
              //  MessageBox.Show(ex.Message);

            }
            return true;

        }
        public bool AnalyseData2(string FileName, short gongli, short mi, int jvli)
        {
            try
            {
                FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(fs);
                
                byte[] b = new byte[52];
                int i = 0;
                long p = 0;

                    if (mi == 0)
                    {
                    }
                    else
                    {
                        p = 52 * mi*2000;
                    }



                    //p = 0;
                br.BaseStream.Seek(p, SeekOrigin.Begin);

                while (i < jvli * 4)
                {

                    b = br.ReadBytes(52);
                    BitConverter.ToSingle(b, 0);
                     BitConverter.ToSingle(b, 4);
                    BitConverter.ToSingle(b, 8);
                    BitConverter.ToSingle(b, 12);
                    BitConverter.ToSingle(b, 16);
                    BitConverter.ToSingle(b, 20);
                     BitConverter.ToSingle(b, 24);
                     BitConverter.ToSingle(b, 28);
                    BitConverter.ToSingle(b, 32);
                    CommonClass.PCCommon.Channel_17[i].Y = BitConverter.ToSingle(b, 36);
                    CommonClass.PCCommon.Channel_18[i].Y = BitConverter.ToSingle(b, 40);
                   CommonClass.PCCommon.Channel_19[i].Y= BitConverter.ToSingle(b, 44);
                   CommonClass.PCCommon.Channel_20[i].Y = BitConverter.ToSingle(b, 48);
                    CommonClass.PCCommon.Channel_17[i].X = (float)i;
                    CommonClass.PCCommon.Channel_18[i].X = (float)i;
                    CommonClass.PCCommon.Channel_19[i].X = (float)i;
                    CommonClass.PCCommon.Channel_20[i].X = (float)i;
                    i++;

                }

                fs.Close();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);

            }
            return true;

        }

    }
}
