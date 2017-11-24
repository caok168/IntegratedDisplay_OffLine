using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using InGraph.Classes;
using System.Data.OleDb;
using System.IO;
using DataProcess;

namespace InGraph.Forms
{
    public partial class DIYDefectForm : Form
    {
        #region 变量
        #region 中文线路名
        /// <summary>
        /// 中文线路名
        /// </summary>
        String lineNameCh = null;//中文线路名
        #endregion
        #region 线路编号
        /// <summary>
        /// 线路编号
        /// </summary>
        public String lineCode;
        #endregion
        #region 行别
        /// <summary>
        /// 行别
        /// </summary>
        String sDir = null;//行别
        #endregion
        #region 大值类型
        /// <summary>
        /// 大值类型
        /// </summary>
        String excptnType = null; //大值类型
        #endregion
        #region 大值等级
        /// <summary>
        /// 大值等级
        /// </summary>
        int excptnClass = 1; //大值等级
        #endregion     
        #region 列表--自定义超限显示
        /// <summary>
        /// 列表--自定义超限显示
        /// </summary>
        List<ExceptionValueDIYClass> excptnValClsList = new List<ExceptionValueDIYClass>();
        #endregion
        #region 列表--超限标准值和自定义值
        /// <summary>
        /// 列表--超限标准值和自定义值
        /// </summary>
        List<ExceptionStndAndDiyClass> excptnStnAndDiyClsList = new List<ExceptionStndAndDiyClass>();
        #endregion
        #region 列表--存放cit文件所属线路的速度区段信息
        /// <summary>
        /// 列表--存放cit文件所属线路的速度区段信息
        /// </summary>
        public List<SudujiClass> sdjClsList = new List<SudujiClass>();
        #endregion
        #region csv文件名--全路径
        /// <summary>
        /// csv文件名--全路径
        /// </summary>
        public String csvFileName = null;
        #endregion
        #region 列表--超限标准值和自定义值--存放设置信息
        /// <summary>
        /// 列表--超限标准值和自定义值--存放设置信息
        /// </summary>
        List<ExceptionStndAndDiyClass> excptnStnAndDiyClsListSave = new List<ExceptionStndAndDiyClass>();
        #endregion


        #region 字典--存放项目类型的中文英文对照
        /// <summary>
        /// 字典--存放项目类型的中文英文对照
        /// </summary>
        public Dictionary<String, String> dicExcptnType = new Dictionary<String, String>();
        #endregion
        #region 字典--存放通道名称的中文英文对照
        /// <summary>
        /// 字典--存放通道名称的中文英文对照
        /// </summary>
        public Dictionary<String, String> dicChannelEnToCh = new Dictionary<String, String>();
        #endregion

        public Dictionary<String, String> dicValid = new Dictionary<String, String>();
        public List<CheckBox> checkBoxList_ExcptnClass = new List<CheckBox>();
        public List<CheckBox> checkBoxList_ExcptnType = new List<CheckBox>();

        #region 列表的里程是否按增里程排列
        /// <summary>
        /// 列表的里程是否按增里程排列
        /// </summary>
        public Boolean isAcsend_Mile = true;
        #endregion
        #region 列表的里程是否按实际速度增排列
        /// <summary>
        /// 列表的里程是否按实际速度增排列
        /// </summary>
        public Boolean isAcsend_Speed = true;
        #endregion
        #region 列表的里程是否按峰值长度增排列
        /// <summary>
        /// 列表的里程是否按峰值长度增排列
        /// </summary>
        public Boolean isAcsend_Length = true;
        #endregion
        #region 列表的里程是否按自定义超限值增排列
        /// <summary>
        /// 列表的里程是否按自定义超限值增排列
        /// </summary>
        public Boolean isAcsend_DiyVal = true;
        #endregion

        #region iic文件路径
        /// <summary>
        /// iic文件路径
        /// </summary>
        public string iicFilePath = "";
        #endregion
        #region 字典--存放自定义偏差写入iic时的通道和偏差英文名的映射
        /// <summary>
        /// 字典--存放自定义偏差写入iic时的通道和偏差英文名的映射
        /// </summary>
        public Dictionary<String, String> dicChannelChToEn = new Dictionary<String, String>();
        #endregion
        #endregion

        public DIYDefectForm()
        {
            InitializeComponent();

            csvFileName = Path.Combine(Path.GetDirectoryName(CommonClass.listDIC[0].sFilePath), "Exception_diy.csv");

            lineNameCh = CommonClass.listDIC[0].sTrackName;
            lineCode = CommonClass.listDIC[0].sLineCode;
            sDir = CommonClass.listDIC[0].sDir;

            InitCheckBoxList_ExcptnClass();
            InitCheckBoxList_ExcptnType();

            InitDicValid();

            //输入框绑定验证事件
            //foreach (TextBox tb in tbExcptnValList)
            //{
            //    tb.TextChanged += new EventHandler(TextBoxTextChanged);
            //}
        }

        private void InitCheckBoxList_ExcptnClass()
        {
            checkBoxList_ExcptnClass.Clear();

            checkBoxList_ExcptnClass.Add(checkBoxLevel1);
            checkBoxList_ExcptnClass.Add(checkBoxLevel2);
            checkBoxList_ExcptnClass.Add(checkBoxLevel3);
            checkBoxList_ExcptnClass.Add(checkBoxLevel4);

            
        }
        private void InitCheckBoxList_ExcptnType()
        {
            checkBoxList_ExcptnType.Clear();

            checkBoxList_ExcptnType.Add(checkBox_Prof_SC);
            checkBoxList_ExcptnType.Add(checkBox_Prof_SC_70);
            checkBoxList_ExcptnType.Add(checkBox_Prof_SC_120);

            checkBoxList_ExcptnType.Add(checkBox_Align_SC);
            checkBoxList_ExcptnType.Add(checkBox_Align_SC_70);
            checkBoxList_ExcptnType.Add(checkBox_Align_SC_120);

            checkBoxList_ExcptnType.Add(checkBox_WideGage);
            checkBoxList_ExcptnType.Add(checkBox_NarrowGage);

            checkBoxList_ExcptnType.Add(checkBox_CrossLevel);

            checkBoxList_ExcptnType.Add(checkBox_Short_Twist);

            checkBoxList_ExcptnType.Add(checkBox_VACC);
            checkBoxList_ExcptnType.Add(checkBox_LACC);
        }

        private void InitDicValid()
        {
            dicValid.Clear();
            dicValid.Add("N", "无效");
            dicValid.Add("E", "有效");
        }

        private void InitDicExcptnType()
        {
            dicExcptnType.Clear();

            dicExcptnType.Add("Prof_SC", "高低_中波");
            dicExcptnType.Add("Prof_SC_70", "高低_70长波");
            dicExcptnType.Add("Prof_SC_120", "高低_120长波");
            dicExcptnType.Add("Align_SC", "轨向_中波");
            dicExcptnType.Add("Align_SC_70", "轨向_70长波");
            dicExcptnType.Add("Align_SC_120", "轨向_120长波");
            dicExcptnType.Add("WideGage", "大轨距");
            dicExcptnType.Add("NarrowGage", "小轨距");
            dicExcptnType.Add("CrossLevel", "水平");
            dicExcptnType.Add("Short_Twist", "三角坑");
            dicExcptnType.Add("LACC", "车体横加");
            dicExcptnType.Add("VACC", "车体垂加");
        }

        private void InitDicChannelEnToCh()
        {
            dicChannelEnToCh.Clear();

            dicChannelEnToCh.Add("L_Prof_SC", "左高低_中波");
            dicChannelEnToCh.Add("L_Prof_SC_70", "左高低_70长波");
            dicChannelEnToCh.Add("L_Prof_SC_120", "左高低_120长波");

            dicChannelEnToCh.Add("R_Prof_SC", "右高低_中波");
            dicChannelEnToCh.Add("R_Prof_SC_70", "右高低_70长波");
            dicChannelEnToCh.Add("R_Prof_SC_120", "右高低_120长波");

            dicChannelEnToCh.Add("L_Align_SC", "左轨向_中波");
            dicChannelEnToCh.Add("L_Align_SC_70", "左轨向_70长波");
            dicChannelEnToCh.Add("L_Align_SC_120", "左轨向_120长波");

            dicChannelEnToCh.Add("R_Align_SC", "右轨向_中波");
            dicChannelEnToCh.Add("R_Align_SC_70", "右轨向_70长波");
            dicChannelEnToCh.Add("R_Align_SC_120", "右轨向_120长波");

            dicChannelEnToCh.Add("WideGage", "大轨距");
            dicChannelEnToCh.Add("NarrowGage", "小轨距");
            dicChannelEnToCh.Add("CrossLevel", "水平");
            dicChannelEnToCh.Add("Short_Twist", "三角坑");
            dicChannelEnToCh.Add("LACC", "车体横向加速度");
            dicChannelEnToCh.Add("VACC", "车体垂向加速度");
        }
        private void InitDicChannelChToEn()
        {
            dicChannelChToEn.Clear();

            dicChannelChToEn.Add("左高低_中波", "L SURFACE");
            dicChannelChToEn.Add("左高低_70长波", "L SURFACE 70M");
            dicChannelChToEn.Add("左高低_120长波", "L SURFACE 120M");

            dicChannelChToEn.Add("右高低_中波", "R SURFACE");
            dicChannelChToEn.Add("右高低_70长波", "R SURFACE 70M");
            dicChannelChToEn.Add("右高低_120长波", "R SURFACE 120M");

            dicChannelChToEn.Add("左轨向_中波", "L ALIGNMENT");
            dicChannelChToEn.Add("左轨向_70长波", "L ALIGNMENT 70M");
            dicChannelChToEn.Add("左轨向_120长波", "L ALIGNMENT 120M");

            dicChannelChToEn.Add("右轨向_中波", "R ALIGNMENT");
            dicChannelChToEn.Add("右轨向_70长波", "R ALIGNMENT 70M");
            dicChannelChToEn.Add("右轨向_120长波", "R ALIGNMENT 120M");

            dicChannelChToEn.Add("大轨距", "WDGA");
            dicChannelChToEn.Add("小轨距", "TGTGA");
            dicChannelChToEn.Add("水平", "CROSSLEVEL");
            dicChannelChToEn.Add("三角坑", "TWIST");
            dicChannelChToEn.Add("车体横向加速度", "LAT ACCEL");
            dicChannelChToEn.Add("车体垂向加速度", "VERT ACCEL");
        }

        private void InitSdjClsList()
        {
            using (OleDbConnection sqlconn = new OleDbConnection(CommonClass.sDBConnectionString))
            {
                String cmd = String.Format("select *  from Suduji where 线编号='{0}' and 行别='{1}'", lineCode, sDir);
                OleDbCommand sqlcom = new OleDbCommand(cmd, sqlconn);

                sqlconn.Open();
                OleDbDataReader oddr = sqlcom.ExecuteReader();

                sdjClsList.Clear();
                SudujiClass tmpCls;

                while (oddr.Read())
                {
                    //把当前的超限值读取
                    tmpCls = new SudujiClass();
                    tmpCls.Id = int.Parse(oddr[0].ToString());
                    tmpCls.lineNameCh = oddr[2].ToString();
                    tmpCls.lineCode = oddr[3].ToString();
                    tmpCls.sDir = oddr[4].ToString();
                    tmpCls.sDirCode = int.Parse(oddr[5].ToString());
                    tmpCls.startMile = float.Parse(oddr[6].ToString());
                    tmpCls.endMile = float.Parse(oddr[7].ToString());
                    tmpCls.speedClass = int.Parse(oddr[8].ToString());//速度等级
                    tmpCls.tqiSpeedClass = int.Parse(oddr[9].ToString());//tqi速度等级

                    sdjClsList.Add(tmpCls);
                }

                oddr.Close();
                sqlconn.Close();
            }
        }
        /// <summary>
        /// 把excptnStnAndDiyClsList中的类型转换成cit文件中的通道英文名
        /// </summary>
        private List<ExceptionStndAndDiyClass> DisplayExcptnStnAndDiyClsList(List<ExceptionStndAndDiyClass> oldList)
        {
            List<ExceptionStndAndDiyClass> newList = new List<ExceptionStndAndDiyClass>();

            foreach (ExceptionStndAndDiyClass tmpCls in oldList)
            {
                if (tmpCls.type.Contains("Prof_SC") || tmpCls.type.Contains("Align_SC"))
                {
                    String newTypeNameL = String.Format("L_{0}", tmpCls.type);
                    ExceptionStndAndDiyClass esdClsL = new ExceptionStndAndDiyClass();
                    esdClsL.id = tmpCls.id;
                    esdClsL.level = tmpCls.level;
                    esdClsL.speed = tmpCls.speed;
                    esdClsL.type = newTypeNameL;
                    esdClsL.valueDIY = tmpCls.valueDIY;
                    esdClsL.valueStandard = tmpCls.valueStandard;
                    newList.Add(esdClsL);

                    String newTypeNameR = String.Format("R_{0}", tmpCls.type);
                    ExceptionStndAndDiyClass esdClsR = new ExceptionStndAndDiyClass();
                    esdClsR.id = tmpCls.id;
                    esdClsR.level = tmpCls.level;
                    esdClsR.speed = tmpCls.speed;
                    esdClsR.type = newTypeNameR;
                    esdClsR.valueDIY = tmpCls.valueDIY;
                    esdClsR.valueStandard = tmpCls.valueStandard;
                    newList.Add(esdClsR);
                }
                else
                {
                    ExceptionStndAndDiyClass esdCls = new ExceptionStndAndDiyClass();

                    esdCls.id = tmpCls.id;
                    esdCls.level = tmpCls.level;
                    esdCls.speed = tmpCls.speed;
                    esdCls.type = tmpCls.type;
                    esdCls.valueDIY = tmpCls.valueDIY;
                    esdCls.valueStandard = tmpCls.valueStandard;


                    newList.Add(esdCls);
                }
            }


            return newList;
        }

        delegate void DisplayExcptnValClsListCallBack();

        private void DisplayExcptnValClsDataGridview()
        {
            if (dataGridView1.InvokeRequired)
            {
                DisplayExcptnValClsListCallBack d = new DisplayExcptnValClsListCallBack(DisplayExcptnValClsDataGridview);
                this.Invoke(d, new object[] { });
            }
            else
            {
                dataGridView1.Rows.Clear();

                if (excptnValClsList.Count == 0)
                {
                    //String str = String.Format("没有找到{0}类型的接近{1}超限", excptnType, excptnClass);
                    //MessageBox.Show(str);
                    return;
                }


                foreach (ExceptionValueDIYClass tmpCls in excptnValClsList)
                {
                    DataGridViewRow dgvr = new DataGridViewRow();

                    dgvr.CreateCells(dataGridView1);

                    dgvr.Cells[0].Value = tmpCls.id;
                    dgvr.Cells[1].Value = dicChannelEnToCh[tmpCls.exceptionType];
                    dgvr.Cells[2].Value = tmpCls.milePos;
                    dgvr.Cells[3].Value = tmpCls.length;
                    dgvr.Cells[4].Value = tmpCls.excptnClass;
                    dgvr.Cells[5].Value = tmpCls.maxSpeed;
                    dgvr.Cells[6].Value = tmpCls.speed;
                    dgvr.Cells[7].Value = tmpCls.excptnValueStandard;
                    dgvr.Cells[8].Value = tmpCls.excptnValRecmmd;
                    dgvr.Cells[9].Value = tmpCls.exceptionValue;
                    dgvr.Cells[10].Value = dicValid[tmpCls.valid];

                    if (tmpCls.valid == "N")
                    {
                        dgvr.DefaultCellStyle.BackColor = Color.Gray;
                    }

                    dataGridView1.Rows.Add(dgvr);
                }
            }
        }

        private void ReSortExcptnValClsListById()
        {
            for (int i = 0; i < excptnValClsList.Count;i++ )
            {
                excptnValClsList[i].id = i + 1;
            }
        }
        private void ExportCsvExcptnValClsList()
        {
            if (excptnValClsList.Count == 0)
            {
                //String str = String.Format("没有找到{0}类型的接近{1}超限", excptnType, excptnClass);
                //MessageBox.Show(str);
                return;
            }

            String head = "ID,类型,里程(公里),起点,终点,长度(米),限速(km/h),速度(km/h),行业标准,铁路局标准,超限值,有效性,原始里程";

            StringBuilder sbToCsv = new StringBuilder();
            String strFormat = "{0},";

            sbToCsv.AppendLine(head);

            foreach (ExceptionValueDIYClass tmpCls in excptnValClsList)
            {
                sbToCsv.AppendFormat(strFormat,tmpCls.id);
                sbToCsv.AppendFormat(strFormat, dicChannelEnToCh[tmpCls.exceptionType]);
                sbToCsv.AppendFormat(strFormat,tmpCls.milePos);
                sbToCsv.AppendFormat(strFormat,tmpCls.startMilePos);
                sbToCsv.AppendFormat(strFormat,tmpCls.endMilePos);
                sbToCsv.AppendFormat(strFormat,tmpCls.length);
                sbToCsv.AppendFormat(strFormat,tmpCls.maxSpeed);
                sbToCsv.AppendFormat(strFormat,tmpCls.speed);
                sbToCsv.AppendFormat(strFormat, tmpCls.excptnValueStandard);
                sbToCsv.AppendFormat(strFormat,tmpCls.excptnValRecmmd);
                sbToCsv.AppendFormat(strFormat,tmpCls.exceptionValue);
                sbToCsv.AppendFormat(strFormat,dicValid[tmpCls.valid]);
                sbToCsv.AppendFormat(strFormat, tmpCls.milePos_Original);

                sbToCsv.AppendLine();
            }

            File.WriteAllText(csvFileName, sbToCsv.ToString(),Encoding.Default);

            MessageBox.Show("导出成功！");
        }

        #region 从iic文件中获取所有的表名称
        /// <summary>
        /// 从iic文件中获取所有的表名称
        /// </summary>
        /// <param name="m_iicFilePath">iic文件名全路径</param>
        /// <returns>所有的表名称</returns>
        private List<String> GetTableNames(String m_iicFilePath)
        {
            List<String> ret = new List<String>();
            using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + m_iicFilePath + ";Persist Security Info=True"))
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

        #region 判断iic文件中表格是否存在
        /// <summary>
        /// 判断iic文件中表格是否存在
        /// </summary>
        /// <param name="tableName">表格名称</param>
        /// <returns>true：存在；false:不存在</returns>
        private Boolean IsTableExist(String tableName)
        {
            Boolean retVal = false;

            List<String> tablesInIIC = GetTableNames(iicFilePath);

            foreach (String strTableName in tablesInIIC)
            {
                if (strTableName == tableName)
                {
                    retVal = true;
                    break;
                }
            }


            return retVal;
        }
        #endregion

        #region 获取与行业标准相对应的铁路局标准的偏差等级
        /// <summary>
        /// 获取与行业标准相对应的铁路局标准的偏差等级
        /// </summary>
        /// <param name="oldClass">行业标准偏差等级</param>
        /// <returns>铁路局标准的偏差等级</returns>
        private int GetNewDefectClass(int standardClass)
        {
            int retVal = 0;

            if (standardClass == 1)
            {
                retVal = 21;
            }
            if (standardClass == 2)
            {
                retVal = 22;
            }
            if (standardClass == 3)
            {
                retVal = 23;
            }
            if (standardClass == 4)
            {
                retVal = 24;
            }

            return retVal;
        }
        #endregion

        private void AddListMenuItem()
        {
            contextMenuStrip1.Items.Clear();

            ToolStripMenuItem tsmi_Valid = new ToolStripMenuItem();
            tsmi_Valid.Text = "设置有效";
            tsmi_Valid.Click += new EventHandler(tsmi_Valid_Click);
            contextMenuStrip1.Items.Add(tsmi_Valid);

            ToolStripMenuItem tsmi_Invalid = new ToolStripMenuItem();
            tsmi_Invalid.Text = "设置无效";
            tsmi_Invalid.Click += new EventHandler(tsmi_Invalid_Click);
            contextMenuStrip1.Items.Add(tsmi_Invalid);


            ToolStripMenuItem tsmi_delete = new ToolStripMenuItem();
            tsmi_delete.Text = "删除无效";
            tsmi_delete.Click += new EventHandler(tsmi_delete_Click);
            contextMenuStrip1.Items.Add(tsmi_delete);
        }


        private void ExcptnValProcess()
        {
            //if (!checkBoxAdd.Checked)
            //{
            //    excptnValClsList.Clear();
            //}

            excptnValClsList.Clear();

            InitExcptnStnAndDiyClsList();
            

            if (excptnStnAndDiyClsList.Count == 0)
            {
                return;
            }

            List<ExceptionStndAndDiyClass> newList = DisplayExcptnStnAndDiyClsList(excptnStnAndDiyClsList);

            foreach (ExceptionStndAndDiyClass tmpCls in newList)
            {
                excptnValClsList.AddRange(GetExcptnValByType(tmpCls));
            }

            for (int i = 0; i < excptnValClsList.Count; i++)
            {
                excptnValClsList[i].id = i + 1;
            }

            ReSortExcptnValClsListById();
            //显示到listview
            //DisplayExcptnValClsList();
            DisplayExcptnValClsDataGridview();
            //给listview添加右键菜单
            AddListMenuItem();
        }

        private List<ExceptionValueDIYClass> GetExcptnValByType(ExceptionStndAndDiyClass esdCls)
        {
            List<ExceptionValueDIYClass> retList = new List<ExceptionValueDIYClass>();

            Boolean isNarrowGauge = false;
            if (esdCls.type.ToLower().Equals("narrowgage"))
            {
                isNarrowGauge = true;
            }

            Boolean isWideGauge = false;
            if (esdCls.type.ToLower().Equals("widegage"))
            {
                isWideGauge = true;
            }

            int channelIndex = GetChannelIndex(esdCls.type);
            int speedIndex = GetChannelIndex("speed");
            float excptnStd = esdCls.valueStandard;
            float excptnDiy = esdCls.valueDIY;
            int maxSpeed = esdCls.speed;
            String excptnType=esdCls.type;
            int m_excptnClass=esdCls.level;
            

            FileStream fs = new FileStream(CommonClass.listDIC[0].sFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            BinaryReader br = new BinaryReader(fs);
            br.ReadBytes(120);
            int channelNumbers = CommonClass.listDIC[0].iChannelNumber;
            br.ReadBytes(65 * channelNumbers);
            br.ReadBytes(BitConverter.ToInt32(br.ReadBytes(4), 0));

            long arrayLen = (br.BaseStream.Length - br.BaseStream.Position) / (2 * channelNumbers);
            long pos = 0;
            float maxVal = 0;
            long maxIndex = 0;
            float maxKmAndMeter = 0;
            float maxValSpeed = 0;//波峰时的列车速度
            
            float value = 0f;
            float speed=0f;
            float kmAndMeter=0f;
            int km=0;
            int meter=0;
            ExceptionValueDIYClass excptnCls = null;

            for (long i = 0; i < arrayLen; i++)
            {
                pos = br.BaseStream.Position;
                Byte[] data = br.ReadBytes(2 * channelNumbers);

                if (CommonClass.listDIC[0].bEncrypt)
                {
                    data = CITDataProcess.ByteXORByte(data);
                }

                km = BitConverter.ToInt16(data, 0);
                meter = BitConverter.ToInt16(data, 2);
                kmAndMeter = km + meter / 4.0f/1000;

                speed = BitConverter.ToInt16(data, speedIndex * 2) / CommonClass.listDIC[0].fScale[speedIndex] + CommonClass.listDIC[0].fOffset[speedIndex];
                value = BitConverter.ToInt16(data, channelIndex * 2) / CommonClass.listDIC[0].fScale[channelIndex] + CommonClass.listDIC[0].fOffset[channelIndex];

                //if (isWideGauge)
                if ((excptnDiy > 0 && excptnStd >0) && excptnStd > excptnDiy)
                {
                    if (value >= excptnDiy)
                    {
                        if (excptnCls == null)
                        {
                            excptnCls = new ExceptionValueDIYClass();

                            excptnCls.startMileIndex = pos;
                            excptnCls.startMilePos = kmAndMeter;
                        }

                        if (value > maxVal)
                        {
                            maxVal = value;
                            maxIndex = pos;
                            maxKmAndMeter = kmAndMeter;
                            maxValSpeed = speed;
                        }
                    }

                    if (value < excptnDiy)
                    {
                        if (excptnCls != null)
                        {
                            excptnCls.exceptionValue = maxVal;
                            excptnCls.endMileIndex = pos;
                            excptnCls.endMilePos = kmAndMeter;
                            excptnCls.length = Math.Abs(excptnCls.endMileIndex - excptnCls.startMileIndex) / (2 * channelNumbers) * 0.25f;

                            excptnCls.mileIndex = maxIndex;
                            excptnCls.milePos = maxKmAndMeter;
                            excptnCls.milePos_Original = maxKmAndMeter;
                            excptnCls.speed = maxValSpeed;
                            excptnCls.maxSpeed = maxSpeed;
                            excptnCls.valid = "N";
                            excptnCls.exceptionType = excptnType;
                            excptnCls.excptnValRecmmd = excptnDiy;
                            excptnCls.excptnValueStandard = excptnStd;
                            excptnCls.excptnClass = m_excptnClass;



                            ExceptionValueDIYClass tmpCls = new ExceptionValueDIYClass();
                            tmpCls.id = excptnCls.id;
                            tmpCls.length = excptnCls.length;
                            tmpCls.maxSpeed = excptnCls.maxSpeed;
                            tmpCls.mileIndex = excptnCls.mileIndex;
                            tmpCls.milePos = excptnCls.milePos;
                            tmpCls.milePos_Original = excptnCls.milePos_Original;
                            tmpCls.speed = excptnCls.speed;
                            tmpCls.startMileIndex = excptnCls.startMileIndex;
                            tmpCls.startMilePos = excptnCls.startMilePos;
                            tmpCls.valid = excptnCls.valid;
                            tmpCls.endMileIndex = excptnCls.endMileIndex;
                            tmpCls.endMilePos = excptnCls.endMilePos;
                            tmpCls.exceptionType = excptnCls.exceptionType;
                            tmpCls.exceptionValue = excptnCls.exceptionValue;
                            tmpCls.excptnValRecmmd = excptnCls.excptnValRecmmd;
                            tmpCls.excptnClass = excptnCls.excptnClass;
                            tmpCls.excptnValueStandard = excptnCls.excptnValueStandard;

                            retList.Add(tmpCls);

                            excptnCls = null;

                            maxVal = 0;
                            maxIndex = 0;
                            maxKmAndMeter = 0;
                            maxValSpeed = 0;
                        }
                    }

                } 
                //else if (isNarrowGauge)
                if ((excptnDiy < 0 && excptnStd < 0) && excptnStd < excptnDiy)
                {
                    if (value <= excptnDiy)
                    {
                        if (excptnCls == null)
                        {
                            excptnCls = new ExceptionValueDIYClass();

                            excptnCls.startMileIndex = pos;
                            excptnCls.startMilePos = kmAndMeter;
                        }

                        if (value < maxVal)
                        {
                            maxVal = value;
                            maxIndex = pos;
                            maxKmAndMeter = kmAndMeter;
                            maxValSpeed = speed;
                        }
                    }


                    if (value > excptnDiy)
                    {
                        if (excptnCls != null)
                        {
                            excptnCls.exceptionValue = maxVal;
                            excptnCls.endMileIndex = pos;
                            excptnCls.endMilePos = kmAndMeter;
                            excptnCls.length = Math.Abs(excptnCls.endMileIndex - excptnCls.startMileIndex) / (2 * channelNumbers) * 0.25f;

                            excptnCls.mileIndex = maxIndex;
                            excptnCls.milePos = maxKmAndMeter;
                            excptnCls.milePos_Original = maxKmAndMeter;
                            excptnCls.speed = maxValSpeed;
                            excptnCls.maxSpeed = maxSpeed;
                            excptnCls.valid = "N";
                            excptnCls.exceptionType = excptnType;
                            excptnCls.excptnValRecmmd = excptnDiy;
                            excptnCls.excptnValueStandard = excptnStd;
                            excptnCls.excptnClass = m_excptnClass;


                            ExceptionValueDIYClass tmpCls = new ExceptionValueDIYClass();
                            tmpCls.id = excptnCls.id;
                            tmpCls.length = excptnCls.length;
                            tmpCls.maxSpeed = excptnCls.maxSpeed;
                            tmpCls.mileIndex = excptnCls.mileIndex;
                            tmpCls.milePos = excptnCls.milePos;
                            tmpCls.milePos_Original = excptnCls.milePos_Original;
                            tmpCls.speed = excptnCls.speed;
                            tmpCls.startMileIndex = excptnCls.startMileIndex;
                            tmpCls.startMilePos = excptnCls.startMilePos;
                            tmpCls.valid = excptnCls.valid;
                            tmpCls.endMileIndex = excptnCls.endMileIndex;
                            tmpCls.endMilePos = excptnCls.endMilePos;
                            tmpCls.exceptionType = excptnCls.exceptionType;
                            tmpCls.exceptionValue = excptnCls.exceptionValue;
                            tmpCls.excptnValRecmmd = excptnCls.excptnValRecmmd;
                            tmpCls.excptnClass = excptnCls.excptnClass;
                            tmpCls.excptnValueStandard = excptnCls.excptnValueStandard;

                            retList.Add(tmpCls);

                            excptnCls = null;

                            maxVal = 0;
                            maxIndex = 0;
                            maxKmAndMeter = 0;
                            maxValSpeed = 0;
                        }
                    }
                }
                else
                {

                }

               
            }

            //删除无效的大值：大于等于标准值；长度小于等于1米；速度区段之外的

            for (int i = retList.Count-1; i >= 0; i--)
            {
                if (retList[i].length <= 1)
                {
                    retList.RemoveAt(i);
                }
            }

            for (int i = retList.Count-1; i >= 0; i--)
            {
                //滤除负半边的值
                if ((excptnDiy < 0 && excptnStd < 0) && excptnStd < excptnDiy)
                {
                    if (retList[i].exceptionValue <= excptnStd)
                    {
                        retList.RemoveAt(i);
                    }
                }
                //滤除正半边的值
                if ((excptnDiy > 0 && excptnStd > 0) && excptnStd > excptnDiy)
                {
                    if (retList[i].exceptionValue >= excptnStd)
                    {
                        retList.RemoveAt(i);
                    }
                }

            }

            foreach (SudujiClass tmpCls in sdjClsList)
            {
                if (tmpCls.speedClass == maxSpeed)
                {
                    float startMile = 0;
                    float endMile = 0;

                    if (tmpCls.startMile < tmpCls.endMile)
                    {
                        startMile = tmpCls.startMile;
                        endMile = tmpCls.endMile;
                    } 
                    else
                    {
                        startMile = tmpCls.endMile;
                        endMile = tmpCls.startMile;
                    }

                    for (int i = retList.Count-1; i >= 0; i--)
                    {
                        if (retList[i].milePos >= startMile && retList[i].milePos <= endMile)
                        {
                            retList[i].valid = "E";
                        }
                    }

                    
                }
            }

            for (int i = retList.Count-1; i >= 0; i--)
            {
                if (retList[i].valid == "N")
                {
                    retList.RemoveAt(i);
                }
            }

            return retList;
        }

        private int GetChannelIndex(String excptnType88)
        {
            String channelNameEn = null;
            String excptnTypeLower = excptnType88.ToLower();
            if (excptnTypeLower.Contains("gage"))
            {
                channelNameEn = "gage";
            }
            else
            {
                channelNameEn = excptnTypeLower;
            }

            int channelIndex = CommonClass.cdp.GetChannelIndex(channelNameEn);
            return channelIndex;
        }

        #region 判断是否含有fix表
        /// <summary>
        /// 判断是否含有fix表
        /// </summary>
        /// <param name="mIICFilePath"></param>
        /// <returns></returns>
        private Boolean IsHasFixTable(String mIICFilePath)
        {
            Boolean isHasFixTalbe = false;

            List<String> tableNames = CommonClass.wdp.GetTableNames(mIICFilePath);

            foreach (String tableName in tableNames)
            {
                if (tableName.Contains("fix"))
                {
                    isHasFixTalbe = true;
                    break;
                }
            }

            return isHasFixTalbe;
        }
        #endregion

        #region 判断IIc文件是否被修正过---ygx--20140320
        /// <summary>
        /// 判断IIc文件是否被修正过
        /// </summary>
        /// <returns>true：已修正；false：未修正</returns>
        private Boolean IsIICFixed(String mIICFilePath)
        {
            Boolean retVal = false;
            Boolean isHasFix = IsHasFixTable(mIICFilePath);


            if (isHasFix == true)
            {
                try
                {
                    using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + mIICFilePath + ";Persist Security Info=True"))
                    {
                        sqlconn.Open();

                        string sqlCreate = "select DISTINCT maxval2 from fix_defects ";
                        OleDbCommand sqlcom = new OleDbCommand(sqlCreate, sqlconn);

                        OleDbDataReader oldr = sqlcom.ExecuteReader();

                        int maxval2 = 0;

                        while (oldr.Read())
                        {
                            if (int.TryParse(oldr[0].ToString(), out maxval2))
                            {
                                if (maxval2 == -200)
                                {
                                    retVal = true;//里程已经修正
                                    break;
                                }
                            }
                        }

                        oldr.Close();
                        sqlconn.Close();
                        //return retVal;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    retVal = false;
                }
            }
            else
            {
                retVal = false;//里程未修正
            }

            return retVal;
        }
        #endregion


        private void DIYDefectForm_Load(object sender, EventArgs e)
        {
            //初始化

            String formText = String.Format("线路名：  {0}  行别：  {1}", lineNameCh, sDir);
            this.Text = formText;

            InitDicChannelEnToCh();
            InitDicChannelChToEn();
            InitDicExcptnType();
            try
            {
                InitSdjClsList();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }


        private void InitExcptnStnAndDiyClsList()
        {
            excptnStnAndDiyClsList.Clear();

            String speeds = null;
            foreach (SudujiClass sudujiCls in sdjClsList)
            {
                if (String.IsNullOrEmpty(speeds) || !speeds.Contains(sudujiCls.speedClass.ToString()))
                {
                    speeds += sudujiCls.speedClass.ToString() + ",";
                }
            }
            speeds = speeds.Remove(speeds.Length - 1, 1);

            int standardType = 0; //标准类型
            if (radioButton_bubiao.Checked)
            {
                standardType = 0;
            } 
            if (radioButton_jubiao.Checked)
            {
                standardType = 1;
            }
            

            foreach (CheckBox cbLevel in checkBoxList_ExcptnClass)
            {
                if (cbLevel.Checked == true)
                {
                    foreach (CheckBox cbType in checkBoxList_ExcptnType)
                    {
                        if (cbType.Checked == true)
                        {
                            //String excptnTypeCh = (String)(comboBoxChannelName.SelectedItem);

                            foreach (KeyValuePair<String, String> kvp in dicExcptnType)
                            {
                                if (kvp.Value == cbType.Text)
                                {
                                    excptnType = kvp.Key;
                                    break;
                                }
                            }

                            using (OleDbConnection sqlconn = new OleDbConnection(CommonClass.sDBConnectionString))
                            {
                                String cmd = String.Format("select *  from 大值国家标准表 where CLASS = {0} and TYPE = '{1}' and SPEED in ({2}) and STANDARDTYPE = {3}", int.Parse((String)(cbLevel.Tag)), excptnType,speeds,standardType);
                                OleDbCommand sqlcom = new OleDbCommand(cmd, sqlconn);

                                sqlconn.Open();
                                OleDbDataReader oddr = sqlcom.ExecuteReader();

                                
                                ExceptionStndAndDiyClass tmpCls;

                                while (oddr.Read())
                                {
                                    String channenlType = oddr[3].ToString();
                                    if (channenlType.Contains("WideGage") || channenlType.Contains("NarrowGage"))
                                    {
                                        //把当前的超限值读取
                                        tmpCls = new ExceptionStndAndDiyClass();
                                        tmpCls.id = int.Parse(oddr[0].ToString());
                                        tmpCls.speed = int.Parse(oddr[1].ToString());
                                        tmpCls.level = int.Parse(oddr[2].ToString());
                                        tmpCls.type = oddr[3].ToString();
                                        tmpCls.valueStandard = float.Parse(oddr[4].ToString());
                                        //如果自定义值不存在，自动置比国家标准值小1
                                        if (oddr[5].ToString() == "")
                                        {
                                            if (tmpCls.valueStandard >= 0)
                                            {
                                                tmpCls.valueDIY = tmpCls.valueStandard - 1;
                                            }
                                            else
                                            {
                                                tmpCls.valueDIY = tmpCls.valueStandard + 1;
                                            }
                                        }

                                        else
                                        {
                                            tmpCls.valueDIY = float.Parse(oddr[5].ToString());
                                        }

                                        excptnStnAndDiyClsList.Add(tmpCls);
                                    }
                                    else
                                    {
                                        //把当前的超限值读取
                                        tmpCls = new ExceptionStndAndDiyClass();
                                        tmpCls.id = int.Parse(oddr[0].ToString());
                                        tmpCls.speed = int.Parse(oddr[1].ToString());
                                        tmpCls.level = int.Parse(oddr[2].ToString());
                                        tmpCls.type = oddr[3].ToString(); //正值部分
                                        tmpCls.valueStandard = float.Parse(oddr[4].ToString());
                                        //如果自定义值不存在，自动置比国家标准值小1
                                        if (oddr[5].ToString() == "")
                                        {
                                            //tmpCls.valueDIY = 0f;

                                            if (tmpCls.valueStandard >= 0)
                                            {
                                                tmpCls.valueDIY = tmpCls.valueStandard - 1;
                                            }
                                            else
                                            {
                                                tmpCls.valueDIY = tmpCls.valueStandard + 1;
                                            }
                                        }

                                        else
                                        {
                                            tmpCls.valueDIY = float.Parse(oddr[5].ToString());
                                        }

                                        excptnStnAndDiyClsList.Add(tmpCls);


                                        //负值部分
                                        //把当前的超限值读取
                                        tmpCls = new ExceptionStndAndDiyClass();
                                        tmpCls.id = int.Parse(oddr[0].ToString());
                                        tmpCls.speed = int.Parse(oddr[1].ToString());
                                        tmpCls.level = int.Parse(oddr[2].ToString());
                                        tmpCls.type = oddr[3].ToString();
                                        //负值部分
                                        tmpCls.valueStandard = float.Parse(oddr[4].ToString()) * (-1);
                                        //如果自定义值不存在，自动置比国家标准值小1
                                        if (oddr[5].ToString() == "")
                                        {
                                            //tmpCls.valueDIY = 0f;

                                            if (tmpCls.valueStandard >= 0)
                                            {
                                                tmpCls.valueDIY = tmpCls.valueStandard - 1;
                                            }
                                            else
                                            {
                                                tmpCls.valueDIY = tmpCls.valueStandard + 1;
                                            }
                                        }

                                        else
                                        {
                                            tmpCls.valueDIY = float.Parse(oddr[5].ToString()) * (-1);
                                        }

                                        excptnStnAndDiyClsList.Add(tmpCls);
                                    }
                                }

                                oddr.Close();
                                sqlconn.Close();
                            }

                        }

                    }
                }
            }

        }


        private void buttonOK_Click(object sender, EventArgs e)
        {
            //Boolean retval = UpdateExcptnValRecmmd();
            //if (retval == false)
            //{
            //    return;
            //}

            //ExcptnValProcess();
            SetEnable(false);
            backgroundWorker1.RunWorkerAsync();
        }

        private void SetEnable(Boolean b)
        {
            buttonOK.Enabled = b;
            buttonExportExcel.Enabled = b;            

            panel_export.Enabled = b;
            groupBox_select.Enabled = b;
        }

        private void tsmi_Valid_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dgvw in dataGridView1.SelectedRows)
            {


                dgvw.Cells[10].Value = dicValid["E"];
                dgvw.DefaultCellStyle.BackColor = Color.White;

                int id = int.Parse(dgvw.Cells[0].Value.ToString());

                foreach (ExceptionValueDIYClass tmpCls in excptnValClsList)
                {
                    if (tmpCls.id == id)
                    {
                        tmpCls.valid = "E";
                        break;
                    }
                }
            }

        }
        private void tsmi_Invalid_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dgvw in dataGridView1.SelectedRows)
            {


                dgvw.Cells[10].Value = dicValid["N"];
                dgvw.DefaultCellStyle.BackColor = Color.Gray;

                int id = int.Parse(dgvw.Cells[0].Value.ToString());

                foreach (ExceptionValueDIYClass tmpCls in excptnValClsList)
                {
                    if (tmpCls.id == id)
                    {
                        tmpCls.valid = "N";
                        break;
                    }
                }
            }
        }
        private void tsmi_delete_Click(object sender, EventArgs e)
        {
            List<int> idList = new List<int>();//待删除的id

            foreach (DataGridViewRow dgvw in dataGridView1.SelectedRows)
            {
                int id = int.Parse(dgvw.Cells[0].Value.ToString());

                String valid = dgvw.Cells[10].Value.ToString();

                if (valid == "无效")
                {
                    idList.Add(id);
                }

                
            }

            idList.Reverse();

            for (int i =0;i<excptnValClsList.Count;i++)
            {
                foreach (int id in idList)
                {
                    if (excptnValClsList[i].id == id)
                    {
                        excptnValClsList.Remove(excptnValClsList[i]);
                    }

                }
            }


            //ReSortExcptnValClsListById();
            //DisplayExcptnValClsList();

            DisplayExcptnValClsDataGridview();
        }

        private void buttonExportExcel_Click(object sender, EventArgs e)
        {
            ExportCsvExcptnValClsList();            
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            ExcptnValProcess();            
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            SetEnable(true);
            panel_export.Enabled = true;
        }

        private void buttonSelectIIC_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.FileName = "";
                DialogResult dr = openFileDialog1.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    labelIICName.Text = openFileDialog1.SafeFileName;
                    iicFilePath = openFileDialog1.FileName;

                    buttonExportIIC.Enabled = true;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonExportIIC_Click(object sender, EventArgs e)
        {
            SetEnable(false);

            long m_RecordNumber = 0;
            long m_defectnum = 0;

            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + iicFilePath + ";Persist Security Info=True"))
                {

                    String sSQL = "select max(RecordNumber),max(defectnum) from defects where valid<>'N'";
                    OleDbCommand sqlcom = new OleDbCommand(sSQL, sqlconn);

                    sqlconn.Open();
                    OleDbDataReader sdr = sqlcom.ExecuteReader();

                    while (sdr.Read())
                    {
                        m_RecordNumber = long.Parse(sdr.GetValue(0).ToString());
                        m_defectnum = long.Parse(sdr.GetValue(1).ToString());
                    }
                    sdr.Close();
                    sqlconn.Close();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("查询defects表中的最大值出错："+"\n"+ex.Source);
                return;
            }

            //查询线路代码，日期和时间
            String trackCode = null;
            String runTime = null;
            String runDate = null;
            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + iicFilePath + ";Persist Security Info=True"))
                {
                    sqlconn.Open();

                    String sSQL = "select distinct SubCode from defects";
                    OleDbCommand sqlcom = new OleDbCommand(sSQL, sqlconn);
                    OleDbDataReader sdr = sqlcom.ExecuteReader();
                    while (sdr.Read())
                    {
                        trackCode = sdr.GetValue(0).ToString();
                    }
                    sdr.Close();

                    sSQL = "select distinct RunDate from defects";
                    sqlcom = new OleDbCommand(sSQL, sqlconn);
                    sdr = sqlcom.ExecuteReader();
                    while (sdr.Read())
                    {
                        runDate = DateTime.Parse(sdr.GetValue(0).ToString()).Date.ToShortDateString();
                        runDate = runDate.Replace("/", "-");
                    }
                    sdr.Close();

                    sSQL = "select distinct RunTime from defects";
                    sqlcom = new OleDbCommand(sSQL, sqlconn);
                    sdr = sqlcom.ExecuteReader();
                    while (sdr.Read())
                    {
                        runTime = sdr.GetValue(0).ToString();
                    }
                    sdr.Close();

                    sqlconn.Close();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("查询fix_defects表中的日期，时间出错：" + "\n" + ex.Source);
                return;
            }



            String str = CommonClass.listDIC[0].sTrackCode;
            try
            {
                using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + iicFilePath + ";Persist Security Info=True"))
                {
                    sqlconn.Open();
                    int diyDefectClass = 0;
                    String sSQL = null;
                    OleDbCommand sqlcom = null;
                    String sqlFormat = "insert into defects(RecordNumber,SubCode,RunDate,RunTime,defectnum,defecttype,tbce,length,maxpost,maxminor,maxval1,maxval2,speedatmaxval,severity,postedspd,defectclass,valid,frompost,fromminor)"
                        + " values({0},'{1}','{2}','{3}',{4},'{5}','{6}',{7},{8},{9},{10},{11},{12},{13},{14},{15},'{16}',{17},{18})";
                    for (int i = 0; i < excptnValClsList.Count;i++ )
                    {
                        diyDefectClass = GetNewDefectClass(excptnValClsList[i].excptnClass);

                        sSQL = string.Format(sqlFormat,
                            m_RecordNumber+i+1,
                            trackCode,
                            runDate,
                            runTime,
                            m_defectnum+i+1,
                            dicChannelChToEn[dicChannelEnToCh[excptnValClsList[i].exceptionType]],
                            "",
                            (int)(excptnValClsList[i].length),
                            (int)(excptnValClsList[i].milePos),
                            (int)(excptnValClsList[i].milePos*1000%1000),
                            excptnValClsList[i].exceptionValue,
                            0,
                            excptnValClsList[i].speed,
                            1,
                            excptnValClsList[i].maxSpeed,
                            diyDefectClass,
                            excptnValClsList[i].valid,
                            (int)(excptnValClsList[i].milePos),
                            (int)(excptnValClsList[i].milePos * 1000 % 1000)
                            );
                        sqlcom = new OleDbCommand(sSQL, sqlconn);
                        sqlcom.ExecuteNonQuery();
                    }

                    sqlconn.Close();
                }               

            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.Source);
                return;
            }

            //如果fix_defects表存在，拷贝自定义大值数据到fix_defects表
            //分两种情况：一种是iic已经修正，另一种是iic未修正。
            if (IsHasFixTable(iicFilePath))
            {
                Boolean m_isIICFixed = IsIICFixed(iicFilePath);

                //删除已经创建的表            
                try
                {
                    using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + iicFilePath + ";Persist Security Info=True"))
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
                    using (OleDbConnection sqlconn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + iicFilePath + ";Persist Security Info=True"))
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

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }

                if (m_isIICFixed)
                {   //IIC中的里程已经修正

                    CommonClass.wdp.ExceptionFix(CommonClass.listDIC[0].sFilePath, iicFilePath, CommonClass.listDIC[0].listIC,
    CommonClass.listDIC[0].iSmaleRate, CommonClass.listDIC[0].iChannelNumber, CommonClass.listDIC[0].bEncrypt, CommonClass.listDIC[0].sKmInc, CommonClass.listETC);

                }
            }

            Application.DoEvents();

            MessageBox.Show("导出iic成功");

            SetEnable(true);
        }

        private void buttonUpdateMilePos_Click(object sender, EventArgs e)
        {
            if (CommonClass.listDIC[0].listIC == null || CommonClass.listDIC[0].listIC.Count == 0)
            {
                MessageBox.Show("没有进行里程校正，不需要更新里程！");
                return;
            }
            if (excptnValClsList.Count == 0)
            {
                return;
            }
            try
            {
                for (int i = 0; i < excptnValClsList.Count; i++)
                {

                    int milePos = CommonClass.wdp.PointToMeter(CommonClass.listDIC[0].listIC,
                        excptnValClsList[i].mileIndex, CommonClass.listDIC[0].iChannelNumber,
                        CommonClass.listDIC[0].sKmInc);
                    excptnValClsList[i].milePos = (float)milePos/1000;
                    Application.DoEvents();
                }
            }
            catch
            {

            }

            //DisplayExcptnValClsList();

            DisplayExcptnValClsDataGridview();

            MessageBox.Show("更新成功！");
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex == -1)
            {
                return;
            }

            int id = int.Parse(dataGridView1[0, e.RowIndex].Value.ToString());

            //MouseEventArgs mea = (MouseEventArgs)e;
            //ListViewItem lvi = listView1.GetItemAt(mea.X, mea.Y);
            //int id = int.Parse(lvi.Text);

            long mileIndex = excptnValClsList[id - 1].mileIndex;
            MainForm.sMainform.MeterFind(mileIndex);
        }

    }

    #region 数据类--自定义超限显示
    /// <summary>
    /// 数据类--自定义超限显示
    /// </summary>
    public class ExceptionValueDIYClass
    {
        #region 序号
        /// <summary>
        /// 序号
        /// </summary>
        public int id;
        #endregion
        #region 超限类型
        /// <summary>
        /// 超限类型
        /// </summary>
        public String exceptionType;
        #endregion
        #region 超限峰值的文件指针
        /// <summary>
        /// 超限峰值的文件指针
        /// </summary>
        public long mileIndex;
        #endregion
        #region 超限峰值里程--单位：公里
        /// <summary>
        /// 超限峰值里程--单位：公里
        /// </summary>
        public float milePos;
        #endregion
        #region 超限峰值原始里程--单位：公里
        /// <summary>
        /// 超限峰值原始里程--单位：公里
        /// </summary>
        public float milePos_Original;
        #endregion
        #region 超限起点文件指针
        /// <summary>
        /// 超限起点文件指针
        /// </summary>
        public long startMileIndex;
        #endregion
        #region 超限起点里程--单位：公里
        /// <summary>
        /// 超限起点里程--单位：公里
        /// </summary>
        public float startMilePos;
        #endregion
        #region 超限终点文件指针
        /// <summary>
        /// 超限终点文件指针
        /// </summary>
        public long endMileIndex;
        #endregion
        #region 超限终点里程--单位：公里
        /// <summary>
        /// 超限终点里程--单位：公里
        /// </summary>
        public float endMilePos;
        #endregion
        #region 超限长度--单位：米
        /// <summary>
        /// 超限长度--单位：米
        /// </summary>
        public float length;
        #endregion
        #region 超限等级
        /// <summary>
        /// 超限等级
        /// </summary>
        public int excptnClass;
        #endregion
        #region 最高限速
        /// <summary>
        /// 最高限速
        /// </summary>
        public float maxSpeed;
        #endregion
        #region 列车速度--单位：km/h
        /// <summary>
        /// 列车速度--单位：km/h
        /// </summary>
        public float speed;
        #endregion
        #region 行业标准
        /// <summary>
        /// 行业标准
        /// </summary>
        public float excptnValueStandard;
        #endregion
        #region 铁路局标准
        /// <summary>
        /// 铁路局标准
        /// </summary>
        public float excptnValRecmmd;
        #endregion
        #region 超限值
        /// <summary>
        /// 超限值
        /// </summary>
        public float exceptionValue;
        #endregion
        #region 是否有效--有效：E；无效：N
        /// <summary>
        /// 是否有效--有效：E；无效：N
        /// </summary>
        public String valid;
        #endregion
    }
    #endregion

    #region 数据类--Suduji
    /// <summary>
    /// 数据类--Suduji
    /// </summary>
    public class SudujiClass
    {
        #region 速度区段Id
        /// <summary>
        /// 速度区段Id
        /// </summary>
        public int Id;
        #endregion
        #region 中文线路名
        /// <summary>
        /// 中文线路名
        /// </summary>
        public String lineNameCh;
        #endregion
        #region 线路编号
        /// <summary>
        /// 线路编号
        /// </summary>
        public String lineCode;
        #endregion
        #region 行别
        /// <summary>
        /// 行别
        /// </summary>
        public String sDir;
        #endregion
        #region 起止里程
        /// <summary>
        /// 起止里程
        /// </summary>
        public float startMile;
        #endregion
        #region 行别ID
        /// <summary>
        /// 行别ID
        /// </summary>
        public int sDirCode;
        #endregion
        #region 速度等级
        /// <summary>
        /// 速度等级
        /// </summary>
        public int speedClass;
        #endregion
        #region 终止里程
        /// <summary>
        /// 终止里程
        /// </summary>
        public float endMile;
        #endregion
        #region TQI速度等级
        /// <summary>
        /// TQI速度等级
        /// </summary>
        public int tqiSpeedClass;
        #endregion
        #region 允许速度
        /// <summary>
        /// 允许速度
        /// </summary>
        public int speed;
        #endregion
    }
    #endregion

    #region 数据类--超限标准值和自定义值表
    /// <summary>
    /// 数据类--超限标准值和自定义值表
    /// </summary>
    public class ExceptionStndAndDiyClass
    {
        #region 序号
        /// <summary>
        /// 序号
        /// </summary>
        public int id;
        #endregion
        #region 速度等级
        /// <summary>
        /// 速度等级
        /// </summary>
        public int speed;
        #endregion
        #region 超限等级
        /// <summary>
        /// 超限等级
        /// </summary>
        public int level;
        #endregion
        #region 超限类型
        /// <summary>
        /// 超限类型
        /// </summary>
        public String type;
        #endregion
        #region 超限值--国家标准
        /// <summary>
        /// 超限值--国家标准
        /// </summary>
        public float valueStandard;
        #endregion
        #region 超限值--自定义
        /// <summary>
        /// 超限值--自定义
        /// </summary>
        public float valueDIY;
        #endregion
        #region 标准类型
        /// <summary>
        /// 标准类型
        /// </summary>
        public int standardType;
        #endregion
    }
    #endregion
}
