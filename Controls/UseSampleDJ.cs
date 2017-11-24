using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.OracleClient;
using System.Data.OleDb;
namespace UseCase
{
    [ObsoleteAttribute("OracleConnection has been deprecated. http://go.microsoft.com/fwlink/?LinkID=144260", false)]
    public class UseSampleDJ
    {
        /// <summary>
        /// IIC文件导入
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IICDataImp(int id)
        {
            #region IIC文件导入
            DataTable dtReadIICFiles = new DataTable();
            DataSet ds = new DataSet();
            IDictionary<string, Model.DecisionProjectObjDJ> ts = null;
            IDictionary<int, Model.DetectRecordObjDJ> tsDetectRecord = null;
            DataTable dtUnitAreaConfig = new DataTable("UnitAreaConfig");
            DataTable dtSystemConfig = new DataTable("SystemConfig");
            string strOra = "select * from tbl00_detect_data_manage t where completestatus = 1 and id=" + id;
            using (OracleConnection cn = new OracleConnection(System.Configuration.ConfigurationManager.ConnectionStrings["OraConnStringDJ"].ConnectionString))
            {
                try
                {
                    OracleDataAdapter dap = new OracleDataAdapter();
                    OracleCommand cmd = new OracleCommand();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = strOra;
                    cmd.Connection = cn;
                    dap.SelectCommand = cmd;
                    dap.Fill(ds);

                    strOra = "select id, unittypeid, unit_code, unit_name, line_code, line_id, line_dir, startmile, endmile, status from tbl00_dic_unit_areaconfig";
                    dap.SelectCommand.CommandText = strOra;
                    dap.Fill(dtUnitAreaConfig);

                    strOra = "select * from tbl00_system_config";
                    dap.SelectCommand.CommandText = strOra;
                    dap.Fill(dtSystemConfig);

                    strOra = "select exceptionid,exceptioncn,exceptionitem,exceptionen from tbl01_dic_01exceptiontype where exceptionid<200";
                    cmd.CommandText = strOra;
                    cn.Open();
                    OracleDataReader red = cmd.ExecuteReader();
                    ts = new Dictionary<string, Model.DecisionProjectObjDJ>();
                    while (red.Read())
                    {
                        Model.DecisionProjectObjDJ obj = new Model.DecisionProjectObjDJ
                        {
                            ProjectID = int.Parse(red["exceptionid"].ToString()),
                            MethodID = int.Parse(red["exceptionid"].ToString()),
                            ProjectList = red["exceptioncn"].ToString(),
                            ProjectDes = red["exceptionitem"].ToString(),
                            ProjectSourceName = red["exceptionen"].ToString()
                        };
                        ts.Add(obj.ProjectSourceName, obj);
                    }
                    if (!red.IsClosed) red.Close();

                    strOra = "select d.detecttrain,c.linetype,b.type detecttype,a.detectid, a.taskid, a.planid, a.detectdate, "+
          "     a.lineid, a.linedirection, a.linename, a.rundirection, a.sys_train_id, '' as des,d.taskindex "+
        "  from pw_run_record a,tbl00_detect_plan b,tbl01_dic_06iiclinecode c,tbl00_detect_task d "+
           " where a.planid=b.id and a.lineid=c.linecode and a.taskid=d.id";
                    cmd.CommandText = strOra;
                    red = cmd.ExecuteReader();
                    tsDetectRecord = new Dictionary<int, Model.DetectRecordObjDJ>();
                    while (red.Read())
                    {
                        Model.DetectRecordObjDJ obj = new Model.DetectRecordObjDJ
                        {
                            DetectTrain = red["detecttrain"].ToString(),
                            LineType = red["linetype"].ToString(),
                            DetectType = red["detecttype"].ToString(),
                            ID = int.Parse(red["detectid"].ToString()),
                            TaskID = int.Parse(red["taskid"].ToString()),
                            PlanID = int.Parse(red["planid"].ToString()),
                            DetectDate = DateTime.Parse(red["detectdate"].ToString()),
                            LineName = red["linename"].ToString(),
                            LineDir = red["linedirection"].ToString(),
                            TrainDir = red["rundirection"].ToString(),
                            LineCode = red["lineid"].ToString(),
                            TrainNumber = red["sys_train_id"].ToString(),
                            Des = red["des"].ToString(),
                            iXunCi = red["taskindex"].ToString()
                        };
                        tsDetectRecord.Add(obj.ID, obj);
                    }
                    if (!red.IsClosed) red.Close();
                }
                catch (Exception ex)
                {
                    string ss = ex.Message.ToString();
                }
                finally
                {
                    if (cn.State != ConnectionState.Closed) cn.Close();
                    if (cn != null) cn.Dispose();
                }
            }

            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                try
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        string strIICPath = row["DATAPATH"].ToString();
                        string strIICFileName = row["DATANAME"].ToString();
                        int intDataID = int.Parse(row["ID"].ToString());
                        int intRecordID = int.Parse(row["RECORDID"].ToString());
                        int intTaskID = int.Parse(row["TASKID"].ToString());
                        int intPlanID = int.Parse(row["PLANID"].ToString());

                        if (tsDetectRecord.ContainsKey(intRecordID))
                        {
                            Model.DetectRecordObjDJ obj = tsDetectRecord[intRecordID];
                            ReadIIC(ts, obj, dtUnitAreaConfig, dtSystemConfig, intDataID, intRecordID, intTaskID, intPlanID, strIICPath, strIICFileName, string.Empty);
                        }
                    }
                }
                catch
                {
                    
                    return false;
                }
            }
            return true;
            #endregion
        }

        /// <summary>
        /// IIC文件更新
        /// </summary>
        /// <param name="intImpID">导入ID</param>
        /// <returns></returns>
        public bool IICDataUpdate(long intImpID, int intFlag)
        {
            OracleConnection cn = new OracleConnection(System.Configuration.ConfigurationManager.ConnectionStrings["OraConnStringDJ"].ConnectionString);
            DataTable dtSystemConfig = new DataTable();
            string strOra = "select * from tbl00_system_config";
            OracleDataAdapter dap = new OracleDataAdapter();
            OracleCommand cmd = new OracleCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = strOra;
            cmd.Connection = cn;
            dap.SelectCommand = cmd;
            dap.Fill(dtSystemConfig);

            UtilityDBDJ utility = new UtilityDBDJ();
            try
            {
                if (cn.State != ConnectionState.Open) cn.Open();
                UtilityDBDJ.CreateErrorLog("开始执行存储过程!");
                utility.OperatorStore(cmd, intImpID, intImpID, dtSystemConfig, intFlag);
                UtilityDBDJ.CreateErrorLog("完成执行存储过程!");
            }
            catch 
            {
                return false;
            }
            finally
            {
                cmd.Dispose();
                if (cn.State != ConnectionState.Closed) cn.Close();
                if (cn != null) cn.Dispose();
            }
            return true;
        }

        /// <summary>
        /// 晃车数据导入
        /// </summary>
        /// <param name="intSType">0:世恒数据；1:三岭数据；</param>
        /// <param name="strFileName">导入的文件名</param>
        /// <returns></returns>
        public bool HCDataImp(int intSType, string strFileName)
        {
            bool returnValue = true;
            #region 晃车数据导入
            string strOra = string.Empty;
            DataSet ds = null;
            try
            {
                ds = ReadExcel(strFileName);
            }
            catch (Exception)
            {
                returnValue = false;
            }
            IDictionary<string, string> tsLine = null;
            IDictionary<string, string> tsLineDir = null;
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                using (OracleConnection cn = new OracleConnection(System.Configuration.ConfigurationManager.ConnectionStrings["OraConnStringDJ"].ConnectionString))
                {
                    #region using体
                    long intImpID = long.Parse(DateTime.Now.ToString("yyyyMMddHHmmss"));
                    try
                    {
                        OracleCommand cmd = new OracleCommand();
                        cmd.CommandType = CommandType.Text;
                        cmd.Connection = cn;

                        cn.Open();
                        #region 获取字典表数据
                        cmd.CommandText = "select ID,NAME from tbl00_dic_line_dir where NAME in ('上','下','单')";
                        OracleDataReader red = cmd.ExecuteReader();
                        tsLineDir = new Dictionary<string, string>();
                        while (red.Read())
                        {
                            tsLineDir.Add(red["NAME"].ToString(), red["ID"].ToString());
                        }
                        if (!red.IsClosed) red.Close();

                        //线别
                        cmd.CommandText = "select ID,Line_Code,Line_Name,PWMIS_CODE from tbl00_dic_line";
                        red = cmd.ExecuteReader();
                        tsLine = new Dictionary<string, string>();
                        while (red.Read())
                        {
                            tsLine.Add(red["Line_Name"].ToString(), red["Line_Code"].ToString());
                        }
                        if (!red.IsClosed) red.Close();
                        #endregion

                        #region 参数定义
                        strOra = "call p_tbl00_detect_data_hcy";
                        string strParamters = string.Empty;
                        string strShuiJia, strShuiJiaLevel, strChuiJia, strChuiJiaLevel;
                        string strDate, strTime, strExceptionType = string.Empty;
                        string strLineID, strLineDir, strDetectTrainID, strTrainNum;
                        float fSpeed = 0F, fMile = 0F;
                        float fDecisionValue = 0F;//值
                        int intExceptionLevel = 0;//偏差等级
                        int intDecisionID = 0;//评判项目序号
                        DateTime dt = new DateTime();
                        #endregion

                        //世恒数据
                        if (intSType == 0)
                        {
                            for (int i = 1; i < ds.Tables[0].Rows.Count; i++)
                            {
                                #region 遍历数据
                                strShuiJia = "";
                                strShuiJiaLevel = "";
                                strChuiJia = "";
                                strChuiJiaLevel = "";
                                strShuiJia = ds.Tables[0].Rows[i][4].ToString();
                                strShuiJiaLevel = ds.Tables[0].Rows[i][5].ToString();
                                strChuiJia = ds.Tables[0].Rows[i][6].ToString();
                                strChuiJiaLevel = ds.Tables[0].Rows[i][7].ToString();

                                string strLine = ds.Tables[0].Rows[i][0].ToString();//线名
                                strLineID = SwitchLineID(tsLine, strLine);
                                if (string.IsNullOrEmpty(strLineID)) continue;
                                strLineDir = ds.Tables[0].Rows[i][1].ToString();//行别
                                strLineDir = SwitchLineDir(tsLineDir, strLineDir);

                                fMile = float.Parse(ds.Tables[0].Rows[i][2].ToString());//里程
                                fSpeed = float.Parse(ds.Tables[0].Rows[i][3].ToString());//速度
                                strDate = ds.Tables[0].Rows[i][8].ToString();//日期
                                strDate = strDate.Insert(4, "-");
                                strDate = strDate.Insert(7, "-");
                                strTime = ds.Tables[0].Rows[i][9].ToString();//时间
                                strTime = strTime.Insert(2, ":");
                                strTime = strTime.Insert(5, ":");
                                strDate = strDate + " " + strTime;
                                strDetectTrainID = ds.Tables[0].Rows[i][10].ToString();//机车号
                                strTrainNum = ds.Tables[0].Rows[i][11].ToString();//车次
                                //获取
                                dt = DateTime.Parse(strDate);
                                if (dt.Hour >= 16)
                                {
                                    dt=dt.AddDays(1);
                                }

                                //string strGq = ds.Tables[0].Rows[i][12].ToString();//工区;
                                //string strLgq = ds.Tables[0].Rows[i][13].ToString();//领工区;
                                //string strGwdID = ds.Tables[0].Rows[i][14].ToString();//工务段;
                                //string strBureauID = ds.Tables[0].Rows[i][15].ToString();//路局;
                                //strBureauID = SwitchBureau(tsBureau, strBureauID);
                                //strGwdID = SwitchGwd(tsGwd, strGwdID);
                                string strDC = ds.Tables[0].Rows[i][16].ToString() == "假" ? "0" : "1";//道岔;
                                string strDK = ds.Tables[0].Rows[i][17].ToString() == "假" ? "0" : "1";//道口;
                                string strQX = ds.Tables[0].Rows[i][18].ToString() == "假" ? "0" : "1";//曲线;
                                string strQL = ds.Tables[0].Rows[i][19].ToString() == "假" ? "0" : "1";//桥梁;
                                string strHD = ds.Tables[0].Rows[i][20].ToString() == "假" ? "0" : "1";//涵洞;
                                string strSD = ds.Tables[0].Rows[i][21].ToString() == "假" ? "0" : "1";//隧道;
                                string strCX = ds.Tables[0].Rows[i][22].ToString() == "假" ? "0" : "1";//侧向;
                                string strCL = ds.Tables[0].Rows[i][23].ToString() == "假" ? "0" : "1";//长链;

                                //(id, impid, stype, linecode, linedir, detectdate, trainnumber, traintype, detecttrain, mile, speed, decisionid, decisionlevel,
                                //decisionvalue, linelabel, bureaucode, pwsectionid, /*pwplantid, pwteamid,*/ dc, dk, qx, ql, hd, sd, cx, cl)
                                #region 导入
                                if (!string.IsNullOrEmpty(strShuiJia.Trim()) && !string.IsNullOrEmpty(strShuiJiaLevel.Trim()))
                                {
                                    intDecisionID = 303;
                                    fDecisionValue = float.Parse(strShuiJia);
                                    intExceptionLevel = int.Parse(strShuiJiaLevel);
                                    if (intExceptionLevel < 2)
                                    {
                                    }
                                    else
                                    {
                                        int iValue = -1;
                                        switch (intExceptionLevel)
                                        {
                                            case 2:
                                                iValue = -1;
                                                break;
                                            default:
                                                iValue = 1;
                                                break;
                                        }
                                        strParamters = "(" + intImpID + "," + intSType + ",'" + strLineID + "','" + strLineDir + "','" + strDate +
                                                       "','" + strDetectTrainID + "','','" + strTrainNum + "'," + fMile + "," + fSpeed +
                                                       "," + intDecisionID + "," + intExceptionLevel + "," + fDecisionValue + ",''," +
                                                       "'" + strDC + "','" + strDK + "','" + 
                                                       strQX + "','" + strQL + "','" + strHD + "','" + strSD +
                                                       "','" + strCX + "','" + strCL + "',to_date('" + dt.ToString("yyyy-MM-dd") + "','yyyy-MM-dd hh24:mi:ss')," + iValue.ToString() + ")";
                                        cmd.CommandText = strOra + strParamters;
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                if (!string.IsNullOrEmpty(strChuiJia.Trim()) && !string.IsNullOrEmpty(strChuiJiaLevel.Trim()))
                                {
                                    intDecisionID = 304;
                                    fDecisionValue = float.Parse(strChuiJia);
                                    intExceptionLevel = int.Parse(strChuiJiaLevel);
                                    if (intExceptionLevel < 2)
                                    {
                                    }
                                    else
                                    {
                                        int iValue = -1;
                                        switch (intExceptionLevel)
                                        {
                                            case 2:
                                                iValue = -1;
                                                break;
                                            default:
                                                iValue = 1;
                                                break;
                                        }
                                        strParamters = "(" + intImpID + "," + intSType + ",'" + strLineID + "','" + strLineDir + "','" + strDate +
                                                       "','" + strDetectTrainID + "','','" + strTrainNum + "'," + fMile + "," + fSpeed +
                                                       "," + intDecisionID + "," + intExceptionLevel + "," + fDecisionValue + ",''," +
                                                       "'" + strDC + "','" + strDK + "','" + strQX + "','" + strQL + 
                                                       "','" + strHD + "','" + strSD + "','" + strCX + "','" +
                                                       strCL + "',to_date('" + dt.ToString("yyyy-MM-dd") + "','yyyy-MM-dd hh24:mi:ss')," + iValue.ToString() + ")";
                                        cmd.CommandText = strOra + strParamters;
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                #endregion
                                #endregion
                            }
                        }
                        else
                        {
                            //三岭数据
                            for (int i = 2; i < ds.Tables[0].Rows.Count; i++)
                            {
                                #region 遍历数据
                                strShuiJia = "";
                                strShuiJiaLevel = "";
                                strChuiJia = "";
                                strChuiJiaLevel = "";
                                //string strGwdID = ds.Tables[0].Rows[i][1].ToString();//工务段;
                                //if (strGwdID.Contains("工务段"))
                                //    strGwdID = SwitchGwd(tsGwd, strGwdID.Substring(strGwdID.Length - 3));
                                //else
                                //    strGwdID = SwitchGwd(tsGwd, strGwdID.Substring(strGwdID.Length - 2));
                                //string strBureauID = "13";
                                string strLine = ds.Tables[0].Rows[i][2].ToString();//线名
                                if (string.IsNullOrEmpty(strLine)) continue;
                                if (strLine.Substring(strLine.Length - 1, 1) == "线")
                                    strLine = strLine.Substring(0, strLine.Length - 1);
                                strLineID = SwitchLineID(tsLine, strLine);
                                strLineDir = ds.Tables[0].Rows[i][3].ToString();//行别
                                strLineDir = SwitchLineDir(tsLineDir, strLineDir.Substring(0, strLine.Length - 1));
                                strTrainNum = ds.Tables[0].Rows[i][5].ToString();//车次
                                fMile = float.Parse(ds.Tables[0].Rows[i][6].ToString());//里程
                                string strTrainType = ds.Tables[0].Rows[i][7].ToString();//车型
                                strDetectTrainID = ds.Tables[0].Rows[i][8].ToString();//机车号
                                fSpeed = float.Parse(ds.Tables[0].Rows[i][9].ToString());//速度
                                strShuiJia = ds.Tables[0].Rows[i][12].ToString();//水加
                                strShuiJiaLevel = ds.Tables[0].Rows[i][13].ToString();//水加等级
                                strChuiJia = ds.Tables[0].Rows[i][10].ToString();//垂加
                                strChuiJiaLevel = ds.Tables[0].Rows[i][11].ToString();//垂加等级
                                strDate = ds.Tables[0].Rows[i][14].ToString();//日期
                                dt = DateTime.Parse(strDate);
                                if (dt.Hour >= 16)
                                {
                                    dt = dt.AddDays(1);
                                }
                                string strLabel = ds.Tables[0].Rows[i][16].ToString();//标记
                                #region 导入
                                if (!string.IsNullOrEmpty(strShuiJia.Trim()) && !string.IsNullOrEmpty(strShuiJiaLevel.Trim()))
                                {
                                    intDecisionID = 303;
                                    fDecisionValue = float.Parse(strShuiJia);
                                    intExceptionLevel = int.Parse(strShuiJiaLevel);
                                    if (intExceptionLevel < 2)
                                    {
                                    }
                                    else
                                    {
                                        int iValue = -1;
                                        switch (intExceptionLevel)
                                        {
                                            case 2:
                                                iValue = -1;
                                                break;
                                            default:
                                                iValue = 1;
                                                break;
                                        }
                                        strParamters = "(" + intImpID + "," + intSType + ",'" + strLineID + "','" + strLineDir + "','" + strDate +
                                                       "','" + strDetectTrainID + "','" + strTrainType + "','" + strTrainNum + "'," + fMile + "," + fSpeed +
                                                       "," + intDecisionID + "," + intExceptionLevel + "," +
                                                       fDecisionValue + ",'','','','','','','','','',to_date('" + dt.ToString("yyyy-MM-dd") + "','yyyy-MM-dd hh24:mi:ss')," + iValue.ToString() + ")";
                                        cmd.CommandText = strOra + strParamters;
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                if (!string.IsNullOrEmpty(strChuiJia.Trim()) && !string.IsNullOrEmpty(strChuiJiaLevel.Trim()))
                                {
                                    intDecisionID = 304;
                                    fDecisionValue = float.Parse(strChuiJia);
                                    intExceptionLevel = int.Parse(strChuiJiaLevel);
                                    if (intExceptionLevel < 2)
                                    {

                                    }
                                    else
                                    {
                                        int iValue = -1;
                                        switch (intExceptionLevel)
                                        {
                                            case 2:
                                                iValue = -1;
                                                break;
                                            default:
                                                iValue = 1;
                                                break;
                                        }
                                        strParamters = "(" + intImpID + "," + intSType + ",'" + strLineID + "','" + strLineDir + "','" + strDate +
                                                       "','" + strDetectTrainID + "','" + strTrainType + "','" + strTrainNum + "'," + fMile + "," + fSpeed +
                                                       "," + intDecisionID + "," + intExceptionLevel + "," + fDecisionValue +
                                                       ",'','','','','','','','','',to_date('" + dt.ToString("yyyy-MM-dd") + "','yyyy-MM-dd hh24:mi:ss')," + iValue.ToString() + ")";
                                        cmd.CommandText = strOra + strParamters;
                                        cmd.ExecuteNonQuery();
                                    }
                                }
                                #endregion
                                #endregion
                            }
                        }
                        try
                        {
                            cmd.CommandText = "call P_TBL00_DETECT_HCY_UNIT(" + intImpID + ")";
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception e)
                        {
                            UtilityDBDJ.CreateErrorLog("导入晃车数据发生错误！" + e.Message.ToString());
                            returnValue = false;
                        }
                        try
                        {
                            cmd.CommandText = "call P_TBL00_DETECT_HCY_ITEMR3(" + intImpID + ")";
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception e)
                        {
                            UtilityDBDJ.CreateErrorLog("导入晃车数据发生错误！" + e.Message.ToString());
                            returnValue = false;
                        }
                        try
                        {
                            cmd.CommandText = "call p_Tbl00_Dic_Trainnumber(0)";
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception e)
                        {
                            UtilityDBDJ.CreateErrorLog("导入晃车数据发生错误！" + e.Message.ToString());
                            returnValue = false;
                        }
                    }
                    catch (Exception e)
                    {
                        UtilityDBDJ.CreateErrorLog("导入晃车数据发生错误！" + e.Message.ToString());
                        UtilityDBDJ utility = new UtilityDBDJ();
                        utility.SetHcDataRollback(intImpID);
                        returnValue = false;
                    }
                    finally
                    {
                        if (cn.State != ConnectionState.Closed) cn.Close();
                        if (cn != null) cn.Dispose();
                    }
                    #endregion
                }
            }
            return returnValue;
            #endregion
        }

        /// <summary>
        /// 晃车重复
        /// </summary>
        /// <param name="intImpID"></param>
        /// <returns></returns>
        public bool HCDataR3Update(long intImpID)
        {
            bool returnValue = true;
            using (OracleConnection cn = new OracleConnection(System.Configuration.ConfigurationManager.ConnectionStrings["OraConnStringDJ"].ConnectionString))
            {
                OracleCommand cmd = null;
                try
                {
                    string strOra = "call P_TBL00_DETECT_HCY_ITEMR3(" + intImpID + ")";
                    cmd = new OracleCommand(strOra, cn);
                    cn.Open();
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        returnValue = false;
                    }
                    return returnValue;
                }
                catch 
                {
                    returnValue = false;
                }
                finally
                {
                    if (cmd != null) cmd.Dispose();
                    if (cn.State != ConnectionState.Closed) cn.Close();
                    if (cn != null) cn.Dispose();
                }
            }
            return returnValue;
        }

        /// <summary>
        /// 晃车数据导入更新
        /// </summary>
        /// <param name="intImpID"></param>
        /// <returns></returns>
        public bool HCDataUpdate(long intImpID)
        {
            bool returnValue = true;
            using (OracleConnection cn = new OracleConnection(System.Configuration.ConfigurationManager.ConnectionStrings["OraConnStringDJ"].ConnectionString))
            {
                OracleCommand cmd = null;
                try
                {
                    string strOra = "call P_TBL00_DETECT_HCY_UNIT(" + intImpID + ")";
                    cmd = new OracleCommand(strOra, cn);
                    cn.Open();
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        returnValue = false;
                    }
                  
                    return returnValue;
                }
                catch 
                {
                    returnValue = false;
                }
                finally
                {
                    if (cmd != null) cmd.Dispose();
                    if (cn.State != ConnectionState.Closed) cn.Close();
                    if (cn != null) cn.Dispose();
                }
            }
            return returnValue;
        }

        public bool test()
        {
            bool returnValue = true;
            using (OracleConnection cn = new OracleConnection(System.Configuration.ConfigurationManager.ConnectionStrings["OraConnStringDJ"].ConnectionString))
            {
                OracleCommand cmd = null;
                try
                {
                    //string strOra = "call P_TBL00_test";
                    //cmd = new OracleCommand(strOra, cn);
                    OracleDataAdapter dap = new OracleDataAdapter();
                    dap.SelectCommand = new OracleCommand();
                    dap.SelectCommand.CommandType = CommandType.StoredProcedure;
                    dap.SelectCommand.CommandText = "P_TBL00_test";
                    dap.SelectCommand.Connection = cn;
                    dap.SelectCommand.Parameters.Add("cur_name", OracleType.Cursor);
                    dap.SelectCommand.Parameters["cur_name"].Direction = ParameterDirection.Output;
                    DataSet ds = new DataSet();

                    //cn.Open();
                    try
                    {
                        //cmd.ExecuteNonQuery();
                        dap.Fill(ds);
                    }
                    catch (Exception)
                    {
                        returnValue = false;
                    }
                }
                catch
                {
                    returnValue = false;
                }
                finally
                {
                    if (cmd != null) cmd.Dispose();
                    if (cn.State != ConnectionState.Closed) cn.Close();
                    if (cn != null) cn.Dispose();
                }
            }
            return returnValue;
        }

        /// <summary>
        /// 汇总调用
        /// </summary>
        /// <param name="intDetectType">汇总类型;1,3为轨检汇总！2为动检汇总</param>
        /// <param name="intWeekOrMonth">周月; 0为周；1为月</param>
        /// <param name="intYear">年</param>
        /// <param name="intMonth">月</param>
        /// <param name="intWeek">周</param>
        /// <param name="vStartDate">起始日期</param>
        /// <param name="vEndDate">结束日期</param>
        /// <returns></returns>
        public bool HzForGjDjWeekMonth(int intDetectType, int intWeekOrMonth, int intYear, int intMonth, int intWeek, DateTime vStartDate, DateTime vEndDate)
        {
            try
            {
                using (OracleConnection cn = new OracleConnection(System.Configuration.ConfigurationManager.ConnectionStrings["OraConnStringDJ"].ConnectionString))
                {
                    string strOra = "call P_TBL00_S_DETECT_GJDJWEEKMONTH(" + intDetectType + "," + intWeekOrMonth + ",'" + intYear.ToString() + "'," + intMonth + "," + intWeek + ",'" + vStartDate.ToString("yyyy-MM-dd") + "','" + vEndDate.ToString("yyyy-MM-dd") + "')";
                    OracleCommand cmd = new OracleCommand(strOra, cn);
                    try
                    {
                        cn.Open();
                        cmd.ExecuteNonQuery();
                    }
                    catch 
                    {
                        return false;
                    }
                    finally
                    {
                        cmd.Dispose();
                        if (cn.State != ConnectionState.Closed) cn.Close();
                        if (cn != null) cn.Dispose();
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 晃车仪汇总
        /// </summary>
        /// <param name="intWeekOrMonth">周月; 0为周；1为月</param>
        /// <param name="intYear">年</param>
        /// <param name="intMonth">月</param>
        /// <param name="intWeek">周</param>
        /// <param name="vStartDate">起始日期</param>
        /// <param name="vEndDate">结束日期</param>
        /// <returns></returns>
        public bool HzForHcyWeekMonth(int intWeekOrMonth, int intYear, int intMonth, int intWeek, DateTime vStartDate, DateTime vEndDate)
        {
            bool returnValue = true;
            OracleCommand cmd = null;
            using (OracleConnection cn = new OracleConnection(System.Configuration.ConfigurationManager.ConnectionStrings["OraConnStringDJ"].ConnectionString))
            {
                string strOra = "call P_TBL00_S_DETECT_HCY(" + intWeekOrMonth + ",'" + intYear.ToString() + "'," + intMonth + "," + intWeek + ",'" + vStartDate.ToString("yyyy-MM-dd") + "','" + vEndDate.ToString("yyyy-MM-dd") + "')";
                cmd = new OracleCommand(strOra, cn);
                try
                {
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch 
                {
                    returnValue = false;
                }
                finally
                {
                    if (cmd != null) cmd.Dispose();
                    if (cn.State != ConnectionState.Closed) cn.Close();
                    if (cn != null) cn.Dispose();
                }
            }
            return returnValue;
        }

        /// <summary>
        /// 晃车日汇总
        /// </summary>
        /// <param name="intYear"></param>
        /// <param name="intMonth"></param>
        /// <param name="intDay"></param>
        /// <param name="vStartDate"></param>
        /// <param name="vEndDate"></param>
        /// <returns></returns>
        public bool HzForHcyDay(int intYear, int intMonth, int intDay, DateTime vStartDate, DateTime vEndDate)
        {
            bool returnValue = true;
            OracleCommand cmd = null;
            using (OracleConnection cn = new OracleConnection(System.Configuration.ConfigurationManager.ConnectionStrings["OraConnStringDJ"].ConnectionString))
            {
                string strOra = "call P_TBL00_S_DETECT_HCY_DAY(" + intYear + "," + intMonth + "," + intDay + ",'" + vStartDate.ToString("yyyy-MM-dd") + "','" + vEndDate.ToString("yyyy-MM-dd") + "')";
                cmd = new OracleCommand(strOra, cn);
                try
                {
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    returnValue = false;
                }
                finally
                {
                    if (cmd != null) cmd.Dispose();
                    if (cn.State != ConnectionState.Closed) cn.Close();
                    if (cn != null) cn.Dispose();
                }
            }
            return returnValue;
        }

        /// <summary>
        /// 添乘仪汇总
        /// </summary>
        /// <param name="intWeekOrMonth">周月; 0为周；1为月</param>
        /// <param name="intYear">年</param>
        /// <param name="intMonth">月</param>
        /// <param name="intWeek">周</param>
        /// <param name="vStartDate">起始日期</param>
        /// <param name="vEndDate">结束日期</param>
        /// <returns></returns>
        public bool HzForTcyWeekMonth(int intWeekOrMonth, int intYear, int intMonth, int intWeek, DateTime vStartDate, DateTime vEndDate)
        {
            bool returnValue = true;
            OracleCommand cmd = null;
            using (OracleConnection cn = new OracleConnection(System.Configuration.ConfigurationManager.ConnectionStrings["OraConnStringDJ"].ConnectionString))
            {
                string strOra = "call P_TBL00_S_DETECT_TCY(" + intWeekOrMonth + ",'" + intYear.ToString() + "'," + intMonth + "," + intWeek + ",'" + vStartDate.ToString("yyyy-MM-dd") + "','" + vEndDate.ToString("yyyy-MM-dd") + "')";
                cmd = new OracleCommand(strOra, cn);
                try
                {
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    returnValue = false;
                }
                finally
                {
                    if (cmd != null) cmd.Dispose();
                    if (cn.State != ConnectionState.Closed) cn.Close();
                    if (cn != null) cn.Dispose();
                }
            }
            return returnValue;
        }

        private DataSet ReadExcel(string strFileName)
        {
            //string strConnection = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source =" + strFileName + ";Extended Properties = Excel 8.0";
            string strConnection = "Provider=Microsoft.Jet.Oledb.4.0;Data Source=" + strFileName + ";Extended Properties='Excel 8.0;HDR=No;IMEX=1;'";
            OleDbConnection oleConnection = new OleDbConnection(strConnection);
            try
            {
                oleConnection.Open();
                DataTable dtSheetName = oleConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" });
                //包含excel中表名的字符串数组
                string[] strTableNames = new string[dtSheetName.Rows.Count];
                for (int k = 0; k < dtSheetName.Rows.Count; k++)
                {
                    strTableNames[k] = dtSheetName.Rows[k]["TABLE_NAME"].ToString();
                }

                string strCommondText = "select * from [" + strTableNames[0] + "]";
                DataSet dsRead = new DataSet();
                OleDbDataAdapter oleAdper = new OleDbDataAdapter(strCommondText, oleConnection);
                oleAdper.Fill(dsRead, "Pantent");
                return dsRead;
            }
            catch (System.Exception e)
            {
                throw new ApplicationException("读取数据源文件时出错！" + e.Message.ToString());
            }
            finally
            {
                oleConnection.Close();
            }
        }

        private void ReadIIC(IDictionary<string, Model.DecisionProjectObjDJ> ts, Model.DetectRecordObjDJ detectRecordObj, DataTable dtUnitAreaConfig, DataTable dtSystemConfig, int intDataID, int intRecordID, int intTaskID, int intPlanID, string strIICPath, string strFileName, string strImpDes)
        {
            string strIICFullPath = GetIICDirection(dtSystemConfig) + strIICPath + "\\" + strFileName;
            using (OleDbConnection oleCn = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strIICFullPath + ";Persist Security Info=True"))
            {
                oleCn.Open();
                OleDbCommand cmd = new OleDbCommand("select * from fix_defects", oleCn);
                OleDbDataReader myRed;
                bool bFxIsExists = true;
                try
                {
                    myRed = cmd.ExecuteReader();
                }
                catch
                {
                    bFxIsExists = false;
                }
                string strSQLForDefects = string.Empty;
                string strSQLForTQI = string.Empty;
                if (!bFxIsExists)
                {
                    strSQLForDefects = "select * from defects where valid <> 'N' and severity is not null";
                    strSQLForTQI = "TRANSFORM max([tqivalue]) SELECT [FROMPOST],round((round([basepost],3)-[FROMPOST])*1000,0) as [FROMMINOR] FROM tqi GROUP BY [FROMPOST],round((round([basepost],3)-[FROMPOST])*1000,0) PIVOT [tqimetricname]";
                }
                else
                {
                    //new表存在的情况
                    strSQLForDefects = "select * from fix_defects where  valid <> 'N' and severity is not null and maxval2=-200";
                    //strSQLForTQI = "TRANSFORM max([tqivalue]) SELECT [FROMPOST],[FROMMINOR] FROM fix_tqi GROUP BY [FROMPOST],[FROMMINOR] PIVOT [tqimetricname]";
                    strSQLForTQI = "select * FROM fix_tqi";
                }

                DataTable dtDefectsData = new DataTable("defects");
                OleDbDataAdapter dapt = new OleDbDataAdapter();
                dapt.SelectCommand = new OleDbCommand();
                dapt.SelectCommand.Connection = oleCn;
                dapt.SelectCommand.CommandText = strSQLForDefects;
                dapt.Fill(dtDefectsData);

                DataTable dtTQIData = new DataTable("tqi");
                dapt.SelectCommand.CommandText = strSQLForTQI;
                dapt.Fill(dtTQIData);

                UtilityDBDJ utility = new UtilityDBDJ();
                //if (!bFxIsExists)
                //{ strImpDes="TQI无效,-1009";}
                int intImpID = utility.OperatorImpLog(intDataID, intRecordID, strFileName, strIICPath, strImpDes);

                OracleConnection cn = new OracleConnection(System.Configuration.ConfigurationManager.ConnectionStrings["OraConnStringDJ"].ConnectionString);
                cn.Open();
                //OracleTransaction transaction = cn.BeginTransaction();
                OracleCommand cmdInner = new OracleCommand();
                cmdInner.CommandType = CommandType.Text;
                cmdInner.Connection = cn;
                //cmdInner.Transaction = transaction;
                try
                {
                    UtilityDBDJ.CreateErrorLog("开始导入TQI数据!");
                    utility.OperatorTQI(cmdInner, detectRecordObj, dtUnitAreaConfig, dtTQIData, bFxIsExists, intImpID, intRecordID, intTaskID, intPlanID);
                    UtilityDBDJ.CreateErrorLog("完成导入TQI数据!");
                    UtilityDBDJ.CreateErrorLog("开始导入号为:" + intImpID.ToString() + "的文件");
                    utility.OperatorDefects(cmdInner, ts, detectRecordObj, dtUnitAreaConfig, dtDefectsData, intImpID, intRecordID, intTaskID, intPlanID);
                    UtilityDBDJ.CreateErrorLog("完成导入号为:" + intImpID.ToString() + "的文件");
                    UtilityDBDJ.CreateErrorLog("开始执行存储过程!");
                    utility.OperatorStore(cmdInner, intImpID, intRecordID, dtSystemConfig, 1);
                    utility.OperatorStore(cmdInner, intImpID, intRecordID, dtSystemConfig, 0);
                    UtilityDBDJ.CreateErrorLog("完成执行存储过程!");
                    //transaction.Commit();
                    utility.SetDetectDataManage(intDataID);
                }
                catch (Exception e)
                {
                    //transaction.Rollback();
                    utility.SetImpLog(e.Message.ToString(), intImpID);
                }
                finally
                {
                    cmdInner.Dispose();
                    if (cn.State != ConnectionState.Closed) cn.Close();
                    if (cn != null) cn.Dispose();
                }
            }
        }

        public DataSet LoadAttchmentType()
        {
            UseCase.UtilityDBDJ utilityDB = new UseCase.UtilityDBDJ();
            return utilityDB.GetDictionaryForAttchmentType();
        }

        public string GetIICDirection(DataTable dt)
        {
            DataRow[] drs = dt.Select("ConfigName='ServerPath'");
            return drs[0]["configvalue"].ToString();
        }

        //行别
        private string SwitchLineDir(IDictionary<string, string> tsLineDir, string strLineID)
        {
            string returnValue = string.Empty;
            string strLineDirection = strLineID.Substring(0, 1);
            if (tsLineDir.TryGetValue(strLineDirection, out returnValue))
            {
                return returnValue;
            }
            return returnValue;
        }

        //线别
        private string SwitchLineID(IDictionary<string, string> tsLine, string strLinePart)
        {
            string returnValue = string.Empty;
            if (tsLine.TryGetValue(strLinePart, out returnValue))
            {
                return returnValue;
            }
            return returnValue;
        }
    }
}