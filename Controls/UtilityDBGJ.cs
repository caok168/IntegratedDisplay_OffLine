namespace UseCase
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.OracleClient;
    using MyComponets.DBUtility;
    using System.Configuration;
    using System.IO;
    using System.Diagnostics;

    [ObsoleteAttribute("OracleConnection has been deprecated. http://go.microsoft.com/fwlink/?LinkID=144260", false)]
    public class UtilityDBGJ : OracleDataBaseGJ
    {
        private string strOra;
        private OracleDataReader myRed;

        public UtilityDBGJ()
        {
        }

        public void OperatorDefects(OracleCommand cmd, IDictionary<string, Model.DecisionProjectObjGJ> ts, Model.DetectRecordObjGJ detectRecordObj, DataTable dtUnitAreaConfig, DataTable dtDefectsData, int intImpID, int intRecordID, int intTaskID, int intPlanID)
        {
            try
            {
                int intDectsionProjectID = 0;
                int intDurationTime = 0;//偏差持续时间                
                string str = "CALL P_TBL00_DETECT_EXCEPTION";
                //string strMain = "CALL P_TBL00_DETECT_EXCEPTION_MAIN";
                Model.DecisionProjectObjGJ obj = null;
                string strValue = string.Empty;
                //int intNewRecordID = 0, intDetectType = 0;

                //1、根据RecordID从record表 获得，detectdate、linecode，linedir、plan表中的type
                //2、select TaskID from record表 where 检测日期<：detectdate and linecode==  and linedir== and type==order by 检测日期 DESC
                //cmd.CommandText = "select id RecordID, detectType from (" +
                //                  " select r.id,linecode,linedir,p.type detectType from tbl00_detect_record r,tbl00_detect_plan p " +
                //                  " where linecode = '" + detectRecordObj.LineCode + "' and r.linedir = '" + detectRecordObj.LineDir + "' and " +
                //                  " r.detectdate < to_date('" + detectRecordObj.DetectDate.ToString("yyyy-MM-dd") + "','yyyy-MM-dd') " +
                //                  " and r.planid = " + detectRecordObj.PlanID + " order by detectdate desc) " +
                //                  "where rownum = 1";
                //this.myRed = cmd.ExecuteReader();
                //if (this.myRed.Read())
                //{
                //    intNewRecordID = int.Parse(this.myRed["RecordID"].ToString());
                //    intDetectType = int.Parse(this.myRed["detectType"].ToString());
                //}
                //if (!this.myRed.IsClosed) this.myRed.Close();
                string sYear = detectRecordObj.iXunCi.Substring(0, 4);
                string sMonth = detectRecordObj.iXunCi.Substring(4, 2);
                string sCi = detectRecordObj.iXunCi.Substring(7, 1);
                foreach (DataRow row in dtDefectsData.Rows)
                {
                    #region 内部循环遍历
                    float fMile = 0F;
                    int intRepeateTime = 0;//1重复;0未重复
                    int intDefectClass = int.Parse(row["defectclass"].ToString());
                    string strDefectType = row["defecttype"].ToString();
                    if (ts.TryGetValue(strDefectType, out obj))
                    {
                        intDectsionProjectID = obj.ProjectID;
                    }
                    else
                    {
                        intDectsionProjectID = 0;
                        continue;
                    }

                    float maxM = float.Parse(row["MAXMINOR"].ToString()) / 1000;
                    fMile = float.Parse(row["MAXPOST"].ToString()) + maxM;

                    float fFromPost = string.IsNullOrEmpty(row["frompost"].ToString()) ? 0F : float.Parse(row["frompost"].ToString());
                    float fFromMinor = string.IsNullOrEmpty(row["fromminor"].ToString()) ? 0F : float.Parse(row["fromminor"].ToString());
                    float fToPost = string.IsNullOrEmpty(row["topost"].ToString()) ? 0F : float.Parse(row["topost"].ToString());
                    float fToMinor = string.IsNullOrEmpty(row["tominor"].ToString()) ? 0F : float.Parse(row["tominor"].ToString());

                    //if (intDefectClass >= 2)
                    //{
                    //    if (intNewRecordID != 0)
                    //        intRepeateTime = SpExeFor(cmd, intNewRecordID, intDectsionProjectID, intDefectClass, detectRecordObj.LineCode, detectRecordObj.LineDir,
                    //            detectRecordObj.DetectDate.ToString("yyyy-MM-dd"), intDetectType, fMile);
                    //}

                    //IMPID,RECORDID,TASKID,PLANID,DETECTTYPE,LINEID,LINEDIRECTION,DETECT_DATE,MAX_KM,MAX_M,EXCEPTIONITEMID,EXCEPTION_LEVEL,START_KM,START_M,END_KM,END_M,DURATION_TIME,DURATION_DISTANCE,
                    //EXCEPTION_VALUE,SPEEDSECTION,DETECTTRAINID,SPEED,TRACKLINETYPE,SEVERITY,TBCE,valid,DECISIONPROJECTID,REPEATTIME)
                    strValue = "(" + intImpID + "," + intRecordID + "," + intTaskID + "," + intPlanID + ",'" +
                                detectRecordObj.DetectType + "','" + detectRecordObj.LineCode + "','" + detectRecordObj.LineDir + "'," +
                                "'" + detectRecordObj.DetectDate.ToString("yyyy-MM-dd") + "'," + row["MAXPOST"] + "," + row["MAXMINOR"] + "," +
                                intDectsionProjectID + "," + row["defectclass"] + "," + fFromPost + "," + fFromMinor + "," +
                                fToPost + "," + fToMinor + "," + intDurationTime + ",'" + row["LENGTH"] + "'," + row["MAXVAL1"] + "," + row["postedspd"] + ",'" +
                                detectRecordObj.DetectTrain + "'," + row["speedatmaxval"] + ",'" + detectRecordObj.LineType + "'," +
                                row["severity"] + ",'" + row["TBCE"] + "'," + intDectsionProjectID + "," + intRepeateTime + ",'" + sYear + "','" +
                                sMonth + "','" + sCi + "')";
                    //基础偏差
                    this.strOra = str + strValue;
                    cmd.CommandText = this.strOra; ;
                    cmd.ExecuteNonQuery();
                    #endregion
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
        }

        public int SpExeFor(OracleCommand cmd, int intNewRecordID, int intProjectID, int intExceptionLevel, string strLineCode, string strLineDir,
            string strDetectDate, int intDetectType, float fMile)
        {
            //存储过程的参数声明
            OracleParameter[] parameters = 
            {
                new OracleParameter("p_recordid", OracleType.Number),
                new OracleParameter("p_decisionprojectid", OracleType.Number),
                new OracleParameter("p_exception_level", OracleType.Number),
                new OracleParameter("p_LineCode", OracleType.VarChar,6),
                new OracleParameter("p_LineDir", OracleType.VarChar,2),
                new OracleParameter("p_Detectdate", OracleType.VarChar,10),
                new OracleParameter("p_DetectType", OracleType.Number),
                new OracleParameter("p_mile", OracleType.Number),
                new OracleParameter("v_flag", OracleType.Number)
            };
            parameters[0].Value = intNewRecordID;
            parameters[1].Value = intProjectID;
            parameters[2].Value = intExceptionLevel;
            parameters[3].Value = strLineCode;
            parameters[4].Value = strLineDir;
            parameters[5].Value = strDetectDate;
            parameters[6].Value = intDetectType;
            parameters[7].Value = fMile;
            parameters[0].Direction = ParameterDirection.Input;
            parameters[1].Direction = ParameterDirection.Input;
            parameters[2].Direction = ParameterDirection.Input;
            parameters[3].Direction = ParameterDirection.Input;
            parameters[4].Direction = ParameterDirection.Input;
            parameters[5].Direction = ParameterDirection.Input;
            parameters[6].Direction = ParameterDirection.Input;
            parameters[7].Direction = ParameterDirection.Input;
            parameters[8].Direction = ParameterDirection.Output;
            try
            {
                RunProcedure(cmd, "P_TBL00_S_DETECT_REPEATTIME", parameters);
                return int.Parse(parameters[8].Value.ToString());
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
        }

        private void RunProcedure(OracleCommand cmd, string storedProcName, OracleParameter[] parameters)
        {
            cmd.CommandText = storedProcName;//声明存储过程名
            cmd.CommandType = CommandType.StoredProcedure;
            foreach (OracleParameter parameter in parameters)
            {
                cmd.Parameters.Add(parameter);
            }
            cmd.ExecuteNonQuery();//执行存储过程
        }

        public void OperatorTQI(OracleCommand cmd, Model.DetectRecordObjGJ detectRecordObj, DataTable dtUnitAreaConfig, DataTable dtTQI, bool isFixIsExists, int intImpID, int intRecordID, int intTaskID, int intPlanID)
        {
            try
            {
                int intTQILength = 0;
                int intUnitTypeID = 0;
                string strUnitCode = "00";//单位编码

                if (string.IsNullOrEmpty(strUnitCode)) strUnitCode = "00";
                foreach (DataRow row in dtTQI.Rows)
                {
                    string strMile = string.Empty;
                    float fMile = 0.0F;
                    if (!isFixIsExists)
                    {
                        float fMinor = float.Parse(row["FROMMINOR"].ToString());
                        if (fMinor >= 0.0F)
                            strMile = row["FROMPOST"] + "." + row["FROMMINOR"];
                        else
                            fMile = float.Parse(row["FROMPOST"].ToString()) + fMinor / 1000;
                    }
                    else
                    {
                        float fMinor = float.Parse(row["Start_M"].ToString());
                        if (fMinor >= 0.0F)
                            strMile = row["Start_KM"] + "." + row["Start_M"];
                        else
                            fMile = float.Parse(row["Start_KM"].ToString()) + fMinor / 1000;
                    }
                    try
                    {
                        if (!string.IsNullOrEmpty(strMile)) fMile = float.Parse(strMile);
                    }
                    catch (Exception)
                    {
                        //发生异常则将此条记录不导入！
                        continue;
                    }
                    DataRow[] drs = dtUnitAreaConfig.Select("line_code = '" + detectRecordObj.LineCode + "' and line_dir='" + detectRecordObj.LineDir + "' and startmile <= " + fMile + " and " + fMile + " < endmile", "unittypeid desc");
                    if (drs.Length >= 1)
                    {
                        intUnitTypeID = int.Parse(drs[0]["unittypeid"].ToString());
                        strUnitCode = GetUnitCode(int.Parse(drs[0]["unittypeid"].ToString()), drs[0]["unit_code"].ToString());
                    }
                    string sYear = detectRecordObj.iXunCi.Substring(0, 4);
                    string sMonth = detectRecordObj.iXunCi.Substring(4, 2);
                    string sCi = detectRecordObj.iXunCi.Substring(7, 1);
                    if (!isFixIsExists)
                    {
                        float fSTDLATACCEL = string.IsNullOrEmpty(row["STDLATACCEL"].ToString()) ? 0F : float.Parse(row["STDLATACCEL"].ToString());
                        float fSTDVERTACCEL = string.IsNullOrEmpty(row["STDVERTACCEL"].ToString()) ? 0F : float.Parse(row["STDVERTACCEL"].ToString());
                        float fMAXSPEED = string.IsNullOrEmpty(row["MAXSPEED"].ToString()) ? 0F : float.Parse(row["MAXSPEED"].ToString());
                        float fL_STDSURF = string.IsNullOrEmpty(row["L_STDSURF"].ToString()) ? 0F : float.Parse(row["L_STDSURF"].ToString());
                        float fR_STDSURF = string.IsNullOrEmpty(row["R_STDSURF"].ToString()) ? 0F : float.Parse(row["R_STDSURF"].ToString());
                        float fL_STDALIGNF = string.IsNullOrEmpty(row["L_STDALIGN"].ToString()) ? 0F : float.Parse(row["L_STDALIGN"].ToString());
                        float fR_STDALIGN = string.IsNullOrEmpty(row["R_STDALIGN"].ToString()) ? 0F : float.Parse(row["R_STDALIGN"].ToString());
                        float fSTDGAUGE = string.IsNullOrEmpty(row["STDGAUGE"].ToString()) ? 0F : float.Parse(row["STDGAUGE"].ToString());
                        float fSTDCROSSLEVEL = string.IsNullOrEmpty(row["STDCROSSLEVEL"].ToString()) ? 0F : float.Parse(row["STDCROSSLEVEL"].ToString());
                        float fSTDTWIST = string.IsNullOrEmpty(row["STDTWIST"].ToString()) ? 0F : float.Parse(row["STDTWIST"].ToString());
                        float fSTDSUMS = string.IsNullOrEmpty(row["STDSUMS"].ToString()) ? 0F : float.Parse(row["STDSUMS"].ToString());

                        this.strOra = "CALL P_TBL00_DETECT_TQI('" + detectRecordObj.DetectDate.ToString("yyyy-MM-dd")+"','" + 
                            detectRecordObj.LineCode + "','" + detectRecordObj.LineDir + "'," + intImpID + "," + intRecordID + "," + intTaskID + "," + intPlanID + "," +
                                      row["FROMPOST"] + "," + row["FROMMINOR"] + "," + intTQILength + "," + fL_STDSURF + "," + fR_STDSURF + "," +
                                      fL_STDALIGNF + "," + fR_STDALIGN + "," + fSTDGAUGE + "," + fSTDCROSSLEVEL + "," + fSTDTWIST + "," +
                                      fSTDSUMS + "," + fSTDLATACCEL + "," + fSTDVERTACCEL + "," + fMAXSPEED + ",'" + row["TBCE"] + "','" +
                                      strUnitCode + "','" + intUnitTypeID + "','" + sYear + "','" + sMonth + "','" + sCi + "')";
                    }
                    else
                    {
                        //fix 的情况下
                        this.strOra = "CALL P_TBL00_DETECT_TQI('" + detectRecordObj.DetectDate.ToString("yyyy-MM-dd") + "','" +
                            detectRecordObj.LineCode + "','" + detectRecordObj.LineDir + "'," + intImpID + "," + intRecordID + "," + intTaskID + "," + intPlanID + "," +
                                      row["Start_KM"] + "," + row["Start_M"] + "," + intTQILength + "," + row["STD_LSURF"] + "," + row["STD_RSURF"] + "," +
                                      row["STD_LALIGN"] + "," + row["STD_RALIGN"] + "," + row["STD_GAUGE"] + "," + row["STD_CROSSLEVEL"] + "," + row["STD_TWIST"] + "," +
                                      row["TQI"] + "," + row["STD_LACC"] + "," + row["STD_VACC"] + "," + row["MEAN_SPEED"] + ",'','" +
                                      strUnitCode + "','" + intUnitTypeID + "','" + sYear + "','" + sMonth + "','" + sCi + "')";
                    }
                    cmd.CommandText = this.strOra;
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
        }

        public void OperatorStore(OracleCommand cmd, long intImpID,long intRecordID, DataTable dtToBegin, int intFlag)
        {
            try
            {
                #region 执行存储过程体
                if (intFlag == 1)
                {
                    #region 基础台帐与管界速度级
                    this.strOra = "call p_Tbl00_d_Exception_u_base(" + intImpID + ")";
                    cmd.CommandText = this.strOra;
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                    }
                    this.strOra = "call P_TBL00_D_EXCEPTION_U(" + intImpID + ")";
                    cmd.CommandText = this.strOra;
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                    }
                    #endregion
                }
                else
                {
                    this.strOra = "call P_TBL00_DETECT_EXCEPTION_Main(" + intImpID + "," + GetToBegin(dtToBegin) + ")";
                    cmd.CommandText = this.strOra;
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                    }

                    //UtilityDB.CreateErrorLog("开始执行汇总存储过程!");
                    //this.strOra = "call P_TBL00_S_DETECT_E(" + intImpID + ")";
                    //cmd.CommandText = this.strOra;
                    //try
                    //{
                    //    cmd.ExecuteNonQuery();
                    //}
                    //catch (Exception)
                    //{
                    //}
                    //UtilityDB.CreateErrorLog("完成执行汇总存储过程!");

                    UtilityDBDJ.CreateErrorLog("开始执行KM数据更新!");
                    this.strOra = "call p_Tbl00_Dectect_E_KM(" + intImpID + ")";
                    cmd.CommandText = this.strOra;
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                    }
                    UtilityDBDJ.CreateErrorLog("完成执行KM数据更新!");
                    //
                    UtilityDBDJ.CreateErrorLog("执行各种情况为空!");
                    this.strOra = "update tbl02_01pw_exception_gj t set t.valid=0 where t.standard is null and t.batchimpseq=" + intImpID;
                    cmd.CommandText = this.strOra;
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                    }
                    UtilityDBDJ.CreateErrorLog("执行各种情况为空!");
                    //
                    UtilityDBDJ.CreateErrorLog("开始执行长天数据更新!1");
                    this.strOra = "call wxjc_dev.static_unit_gj.STATIC_UNIT_EXCEPTION_GJ(" + intImpID + ")";
                    cmd.CommandText = this.strOra;
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                    }
                    UtilityDBDJ.CreateErrorLog("完成执行长天数据更新!1");
                    UtilityDBDJ.CreateErrorLog("开始执行长天数据更新!2");
                    this.strOra = "call wxjc_dev.static_unit_gj.STATIC_UNIT_TQI_GJ(" + intImpID + ")";
                    cmd.CommandText = this.strOra;
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                    }
                    UtilityDBDJ.CreateErrorLog("完成执行长天数据更新!2");
                    UtilityDBDJ.CreateErrorLog("开始执行长天数据更新!3");
                    this.strOra = "call wxjc_dev.static_unit_gj.STATIC_UNIT_KM_GJ(" + intImpID + ")";
                    cmd.CommandText = this.strOra;
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                    }
                    UtilityDBDJ.CreateErrorLog("完成执行长天数据更新!3");
                    UtilityDBDJ.CreateErrorLog("开始执行长天数据更新!4");
                    this.strOra = "call wxjc_dev.SUMM_DEPARTMENT_GJ.INDEX_TYPE_0_GJ(" + intImpID + ")";
                    cmd.CommandText = this.strOra;
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                    }
                    UtilityDBDJ.CreateErrorLog("完成执行长天数据更新!4");
                    UtilityDBDJ.CreateErrorLog("开始执行长天数据更新!5");
                    this.strOra = "call wxjc_dev.SUMM_DEPARTMENT_GJ.INDEX_TYPE_2_GJ(" + intImpID + ")";
                    cmd.CommandText = this.strOra;
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                    }
                    UtilityDBDJ.CreateErrorLog("完成执行长天数据更新!5");
                    UtilityDBDJ.CreateErrorLog("开始执行长天数据更新!6");
                    this.strOra = "call wxjc_dev.SUMM_DEPARTMENT_GJ.INDEX_TYPE_11_GJ(" + intImpID + ")";
                    cmd.CommandText = this.strOra;
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                    }
                    UtilityDBDJ.CreateErrorLog("完成执行长天数据更新!6");
                    UtilityDBDJ.CreateErrorLog("开始执行长天数据更新!7");
                    this.strOra = "call wxjc_dev.SUMM_DEPARTMENT_GJ.INDEX_TYPE_3_GJ(" + intImpID + ")";
                    cmd.CommandText = this.strOra;
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                    }
                    UtilityDBDJ.CreateErrorLog("完成执行长天数据更新!7");
                    UtilityDBDJ.CreateErrorLog("开始执行长天数据更新!8");
                    this.strOra = "call wxjc_dev.SUMM_DEPARTMENT_GJ.INDEX_TYPE_13_GJ(" + intImpID + ")";
                    cmd.CommandText = this.strOra;
                    try
                    {
                        cmd.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                    }
                    UtilityDBDJ.CreateErrorLog("完成执行长天数据更新!8");
                }
                #endregion
            }
            catch
            {
                
                //throw new Exception(e.Message.ToString());
            }
        }

        public int OperatorImpLog(int intDataID, int intRecordID, string strFileName, string strFullPath, string strDescription)
        {
            int returnValue = 0;
            try
            {
                Open();
                string strImpPerson = string.Empty;
                int intImpStatus = 1;
                this.strOra = "select SEQ_TBL00_LOG_DETECT_DATA_IMP.nextval ID from dual";
                this.myRed = RunOracleNo(this.strOra);
                int intID = 0;
                if (this.myRed.Read())
                {
                    intID = int.Parse(this.myRed["ID"].ToString());
                }
                if (!this.myRed.IsClosed) this.myRed.Close();
                System.Text.StringBuilder sbOra = new System.Text.StringBuilder();
                sbOra.Append("insert into TBL00_LOG_DETECT_DATA_IMP(");
                sbOra.Append("IMPID,RECORDID,DATAID,DATANAME,DATAPATH,IMPDATE,IMPPERSON,IMPSTATUS,IMPDES)");
                sbOra.Append(" values (");
                sbOra.Append(":IMPID,:RECORDID,:DATAID,:DATANAME,:DATAPATH,:IMPDATE,:IMPPERSON,:IMPSTATUS,:IMPDES)");
                this.strOra = sbOra.ToString();
                OracleParameter[] parameters = 
                {
                    new OracleParameter(":IMPID", intID),
				    new OracleParameter(":RECORDID", intRecordID),
				    new OracleParameter(":DATAID", intDataID),
				    new OracleParameter(":DATANAME", strFileName),
				    new OracleParameter(":DATAPATH", strFullPath),
				    new OracleParameter(":IMPDATE", DateTime.Now),
				    new OracleParameter(":IMPPERSON", strImpPerson),
				    new OracleParameter(":IMPSTATUS", intImpStatus),
				    new OracleParameter(":IMPDES", strDescription)
                };
                RunOracleNo(this.strOra, parameters, null);
                returnValue = intID;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                Close();
            }
            return returnValue;
        }

        public void SetImpLog(string strDES, int intID)
        {
            try
            {
                Open();
                this.strOra = "update TBL00_LOG_DETECT_DATA_IMP set IMPSTATUS = 0,IMPDES = :IMPDES where IMPID = :IMPID";
                OracleParameter[] parameters = 
                {
				    new OracleParameter(":IMPDES", strDES),
                    new OracleParameter(":IMPID", intID)
                };
                RunOracleNo(this.strOra, parameters, null);
                this.strOra = "delete from TBL00_DETECT_EXCEPTION where impid = :IMPID";
                OracleParameter[] parametersDelete = 
                {
                    new OracleParameter(":IMPID", intID)
                };
                RunOracleNo(this.strOra, parametersDelete, null);
                this.strOra = "delete from TBL00_DETECT_EXCEPTION_Main where impid = :IMPID";
                RunOracleNo(this.strOra, parametersDelete, null);
                this.strOra = "delete from TBL00_DETECT_TQI where impid = :IMPID";
                RunOracleNo(this.strOra, parametersDelete, null);
                this.strOra = "delete from TBL00_SUMMARY_DETECT_E where impid = :IMPID";
                RunOracleNo(this.strOra, parametersDelete, null);
                this.strOra = "delete from TBL00_SUMMARY_DETECT_EXCEPTION where impid = :IMPID";
                RunOracleNo(this.strOra, parametersDelete, null);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                Close();
            }
        }

        public void SetHcDataRollback(long intImpID)
        {
            try
            {
                Open();
                this.strOra = "delete from tbl00_detect_data_hcy where impid = :IMPID";
                OracleParameter[] parametersDelete = 
                {
                    new OracleParameter(":IMPID", intImpID)
                };
                RunOracleNo(this.strOra, parametersDelete, null);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                Close();
            }
        }

        public void SetDetectDataManage(int intDataID)
        {
            try
            {
                Open();
                this.strOra = "update tbl00_detect_data_manage set completestatus = 2 where ID = :ID";
                OracleParameter[] parameters = 
                {
                    new OracleParameter(":ID", intDataID)
                };
                RunOracleNo(this.strOra, parameters, null);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                Close();
            }
        }

        /// <summary>
        /// 记录日志至文本文件
        /// </summary>
        /// <param name="message">记录的内容</param>
        public static void CreateErrorLog(string message)
        {
            try
            {
                string DirectoryString = Directory.GetCurrentDirectory();
                string FileName = DirectoryString + "\\Log" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                StreamWriter sr = null;

                if (File.Exists(FileName))
                {
                    ///如果日志文件已经存在，则直接写入日志文件
                    sr = File.AppendText(FileName);
                    try
                    {
                        sr.WriteLine("\n");
                        sr.WriteLine(DateTime.Now.ToString() + message);
                    }
                    catch (Exception e)
                    {
                        throw new Exception("写入日志文件失败！" + e.Message.ToString());
                    }
                    finally
                    {
                        sr.Close();
                    }
                }
                else
                {
                    try
                    {
                        ///创建日志文件
                        if (!Directory.Exists(DirectoryString))
                        {
                            Directory.CreateDirectory(DirectoryString);
                        }
                        sr = File.CreateText(FileName);
                        //创建成功后写入日志
                        sr.WriteLine(DateTime.Now.ToString() + message);
                    }
                    catch (Exception e)
                    {
                        throw new Exception("创建日志文件失败！" + e.Message.ToString());
                    }
                    finally
                    {
                        sr.Close();
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        static public int GetToBegin(DataTable dt)
        {
            DataRow[] drs = dt.Select("ConfigName='ToBegin'");
            return int.Parse(drs[0]["configvalue"].ToString());
        }

        static public string GetUnitCode(int intUnitTypeID, string strUnitCode)
        {
            string returnValue = string.Empty;
            switch (intUnitTypeID)
            {
                case 1:
                    returnValue = strUnitCode.Substring(0, 2);
                    break;
                case 2:
                    returnValue = strUnitCode.Substring(0, 5);
                    break;
                case 3:
                    returnValue = strUnitCode.Substring(0, 7);
                    break;
                case 4:
                    returnValue = strUnitCode.Substring(0, 9);
                    break;
            }
            return returnValue;
        }

        public DataSet GetDictionaryForAttchmentType()
        {
            this.strOra = "select attachment_code, attachment_name from tbl00_dic_attachment_type";
            DataSet ds=null;
            try
            {
                ds= MyComponets.DBUtility.OracleDbHelperGJ.Query(this.strOra);
            }
            catch
            {

            }
            return ds;
        }
    }
}