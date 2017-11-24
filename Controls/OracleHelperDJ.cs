#pragma warning disable 0618
namespace MyComponets.DBUtility
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Data.OracleClient;
    using System.Collections;


    #region OracleConnectionHelper 基类
    internal static class OracleConnectionHelperDJ
    {
        /// <summary>
        /// 返回数据库的连接字符串
        /// </summary>
        /// <returns></returns>
        static internal string GetConnection()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["OraConnStringDJ"].ConnectionString;
        }
    }
    #endregion

    #region OracleHelper基类定义

    /// <summary>
    /// A helper class used to execute queries against an Oracle database
    /// </summary>
    public abstract class OracleHelperDJ
    {
        // Read the connection strings from the configuration file
        public static readonly string ConnectionStringOracle = OracleConnectionHelperDJ.GetConnection();
        public static readonly string ConnectionStringLocalTransaction = ConfigurationManager.AppSettings["OraConnString1"];
        public static readonly string ConnectionStringInventoryDistributedTransaction = ConfigurationManager.AppSettings["OraConnString2"];
        public static readonly string ConnectionStringOrderDistributedTransaction = ConfigurationManager.AppSettings["OraConnString3"];
        public static readonly string ConnectionStringProfile = ConfigurationManager.AppSettings["OraProfileConnString"];
        public static readonly string ConnectionStringMembership = ConfigurationManager.AppSettings["OraMembershipConnString"];

        //Create a hashtable for the parameter cached
        private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());

        /// <summary>
        /// Execute a database query which does not include a select
        /// </summary>
        /// <param name="connString">Connection string to database</param>
        /// <param name="cmdType">Command type either stored procedure or SQL</param>
        /// <param name="cmdText">Acutall SQL Command</param>
        /// <param name="commandParameters">Parameters to bind to the command</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string connectionString, CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
        {
            // Create a new Oracle command
            OracleCommand cmd = new OracleCommand();

            //Create a connection
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                //Prepare the command
                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);

                //Execute the command
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        /// 自定义只有SQL语句和参数的值
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="commandParameters"></param>
        /// <returns></returns>
        public static int ExecuteNonQuery(string cmdText, params OracleParameter[] commandParameters)
        {
            // Create a new Oracle command
            OracleCommand cmd = new OracleCommand();

            //Create a connection
            using (OracleConnection connection = new OracleConnection(ConnectionStringOracle))
            {
                //Prepare the command

                //PrepareCommand(cmd, connection, null, CommandType.Text, cmdText, commandParameters);

                if (commandParameters != null)
                {
                    foreach (OracleParameter parm in commandParameters)
                        cmd.Parameters.AddWithValue(parm.ParameterName, parm.Value);
                }

                //Execute the command
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        /// Execute an OracleCommand (that returns no resultset) against an existing database transaction 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(trans, CommandType.StoredProcedure, "PublishOrders", new OracleParameter(":prodid", 24));
        /// </remarks>
        /// <param name="trans">an existing database transaction</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <param name="commandParameters">an array of OracleParamters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(OracleTransaction trans, CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
        {
            OracleCommand cmd = new OracleCommand();
            PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// Execute an OracleCommand (that returns no resultset) against an existing database connection 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  int result = ExecuteNonQuery(connString, CommandType.StoredProcedure, "PublishOrders", new OracleParameter(":prodid", 24));
        /// </remarks>
        /// <param name="conn">an existing database connection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <param name="commandParameters">an array of OracleParamters used to execute the command</param>
        /// <returns>an int representing the number of rows affected by the command</returns>
        public static int ExecuteNonQuery(OracleConnection connection, CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
        {
            OracleCommand cmd = new OracleCommand();

            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
            int val = cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// Execute a select query that will return a result set
        /// </summary>
        /// <param name="connString">Connection string</param>
        //// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <param name="commandParameters">an array of OracleParamters used to execute the command</param>
        /// <returns></returns>
        public static OracleDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
        {

            //Create the command and connection
            OracleCommand cmd = new OracleCommand();
            OracleConnection conn = new OracleConnection(connectionString);

            try
            {
                //Prepare the command to execute
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);

                //Execute the query, stating that the connection should close when the resulting datareader has been read
                OracleDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;

            }
            catch
            {

                //If an error occurs close the connection as the reader will not be used and we expect it to close the connection
                conn.Close();
                throw;
            }
        }

        /// <summary>
        /// Execute an OracleCommand that returns the first column of the first record against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new OracleParameter(":prodid", 24));
        /// </remarks>
        /// <param name="connectionString">a valid connection string for a SqlConnection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <param name="commandParameters">an array of OracleParamters used to execute the command</param>
        /// <returns>An object that should be converted to the expected type using Convert.To{Type}</returns>
        public static object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
        {
            OracleCommand cmd = new OracleCommand();

            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
        }

        ///	<summary>
        ///	Execute	a OracleCommand (that returns a 1x1 resultset)	against	the	specified SqlTransaction
        ///	using the provided parameters.
        ///	</summary>
        ///	<param name="transaction">A	valid SqlTransaction</param>
        ///	<param name="commandType">The CommandType (stored procedure, text, etc.)</param>
        ///	<param name="commandText">The stored procedure name	or PL/SQL command</param>
        ///	<param name="commandParameters">An array of	OracleParamters used to execute the command</param>
        ///	<returns>An	object containing the value	in the 1x1 resultset generated by the command</returns>
        public static object ExecuteScalar(OracleTransaction transaction, CommandType commandType, string commandText, params OracleParameter[] commandParameters)
        {
            if (transaction == null)
                throw new ArgumentNullException("transaction");
            if (transaction != null && transaction.Connection == null)
                throw new ArgumentException("The transaction was rollbacked	or commited, please	provide	an open	transaction.", "transaction");

            // Create a	command	and	prepare	it for execution
            OracleCommand cmd = new OracleCommand();

            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters);

            // Execute the command & return	the	results
            object retval = cmd.ExecuteScalar();

            // Detach the SqlParameters	from the command object, so	they can be	used again
            cmd.Parameters.Clear();
            return retval;
        }

        /// <summary>
        /// Execute an OracleCommand that returns the first column of the first record against an existing database connection 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  Object obj = ExecuteScalar(conn, CommandType.StoredProcedure, "PublishOrders", new OracleParameter(":prodid", 24));
        /// </remarks>
        /// <param name="conn">an existing database connection</param>
        /// <param name="commandType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="commandText">the stored procedure name or PL/SQL command</param>
        /// <param name="commandParameters">an array of OracleParamters used to execute the command</param>
        /// <returns>An object that should be converted to the expected type using Convert.To{Type}</returns>
        public static object ExecuteScalar(OracleConnection connectionString, CommandType cmdType, string cmdText, params OracleParameter[] commandParameters)
        {
            OracleCommand cmd = new OracleCommand();

            PrepareCommand(cmd, connectionString, null, cmdType, cmdText, commandParameters);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;
        }

        /// <summary>
        /// Add a set of parameters to the cached
        /// </summary>
        /// <param name="cacheKey">Key value to look up the parameters</param>
        /// <param name="commandParameters">Actual parameters to cached</param>
        public static void CacheParameters(string cacheKey, params OracleParameter[] commandParameters)
        {
            parmCache[cacheKey] = commandParameters;
        }

        /// <summary>
        /// Fetch parameters from the cache
        /// </summary>
        /// <param name="cacheKey">Key to look up the parameters</param>
        /// <returns></returns>
        public static OracleParameter[] GetCachedParameters(string cacheKey)
        {
            OracleParameter[] cachedParms = (OracleParameter[])parmCache[cacheKey];

            if (cachedParms == null)
                return null;

            // If the parameters are in the cache
            OracleParameter[] clonedParms = new OracleParameter[cachedParms.Length];

            // return a copy of the parameters
            for (int i = 0, j = cachedParms.Length; i < j; i++)
                clonedParms[i] = (OracleParameter)((ICloneable)cachedParms[i]).Clone();

            return clonedParms;
        }

        /// <summary>
        /// Internal function to prepare a command for execution by the database
        /// </summary>
        /// <param name="cmd">Existing command object</param>
        /// <param name="conn">Database connection object</param>
        /// <param name="trans">Optional transaction object</param>
        /// <param name="cmdType">Command type, e.g. stored procedure</param>
        /// <param name="cmdText">Command test</param>
        /// <param name="commandParameters">Parameters for the command</param>
        private static void PrepareCommand(OracleCommand cmd, OracleConnection conn, OracleTransaction trans, CommandType cmdType, string cmdText, OracleParameter[] commandParameters)
        {

            //Open the connection if required
            if (conn.State != ConnectionState.Open)
                conn.Open();

            //Set up the command
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.CommandType = cmdType;

            //Bind it to the transaction if it exists
            if (trans != null)
                cmd.Transaction = trans;

            // Bind the parameters passed in
            if (commandParameters != null)
            {
                foreach (OracleParameter parm in commandParameters)
                    cmd.Parameters.Add(parm);
            }
        }

        /// <summary>
        /// Converter to use boolean data type with Oracle
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns></returns>
        public static string OraBit(bool value)
        {
            if (value)
                return "Y";
            else
                return "N";
        }

        /// <summary>
        /// Converter to use boolean data type with Oracle
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns></returns>
        public static bool OraBool(string value)
        {
            if (value.Equals("Y"))
                return true;
            else
                return false;
        }
    }

    #endregion

    #region OracleDataBase基类定义

    /// <summary>
    /// 数据库操作方法与属性
    /// </summary>
    public abstract class OracleDataBaseDJ
    {
        //获取Web.Config数据库连接字符串
        private readonly string OracleConnectionString = OracleConnectionHelperDJ.GetConnection();

        private OracleConnection cn;		//创建Oracle连接
        private OracleDataAdapter sda;		//创建Oracle数据适配器
        private OracleDataReader sdr;		//创建Oracle数据读取器
        private OracleCommand cmd;			//创建Oracle命令对象
        private OracleParameter param;      //创建Oracle参数
        private DataSet ds;				    //创建数据集
        private DataView dv;			    //创建视图

        /// <summary>
        /// 打开数据库连接
        /// </summary>
        public void Open()
        {
            #region
            cn = new OracleConnection(OracleConnectionString);
            cn.Open();
            #endregion
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        public void Close()
        {
            #region
            if (cn != null)
            {
                cn.Close();
                cn.Dispose();
            }
            #endregion
        }

        /// <summary>
        /// 返回DataSet数据集
        /// </summary>
        /// <param name="strOracle">Oracle语句</param>
        public DataSet GetDs(string strOracle)
        {
            #region
            Open();
            sda = new OracleDataAdapter(strOracle, cn);
            ds = new DataSet();
            sda.Fill(ds);
            Close();
            return ds;
            #endregion
        }

        /// <summary>
        /// 返回DataSet数据集,无打开与关闭连接操作
        /// </summary>
        /// <param name="strOracle"></param>
        /// <returns></returns>
        public DataSet GetDsNo(string strOracle)
        {
            #region
            sda = new OracleDataAdapter(strOracle, cn);
            ds = new DataSet();
            sda.Fill(ds);
            return ds;
            #endregion
        }

        public DataSet AddDataTableNo(DataSet das, string strOracle, string tableName)
        {
            #region
            //sda = new OracleDataAdapter(strOracle, cn);
            sda = new OracleDataAdapter();

            sda.TableMappings.Add("Table", tableName);
            OracleCommand cmd = new OracleCommand(strOracle, cn);
            sda.SelectCommand = cmd;
            sda.Fill(das);
            return das;

            #endregion
        }
        /// <summary>
        /// 添加DataSet表
        /// </summary>
        /// <param name="ds">DataSet对象</param>
        /// <param name="strOracle">Oracle语句</param>
        /// <param name="strTableName">表名</param>
        public void GetDs(DataSet ds, string strOracle, string strTableName)
        {
            #region
            Open();
            sda = new OracleDataAdapter(strOracle, cn);
            sda.Fill(ds, strTableName);
            Close();
            #endregion
        }

        /// <summary>
        /// 返回DataView数据视图
        /// </summary>
        /// <param name="strOracle">Oracle语句</param>
        public DataView GetDv(string strOracle)
        {
            #region
            dv = GetDs(strOracle).Tables[0].DefaultView;
            return dv;
            #endregion
        }

        /// <summary>
        /// 获得DataTable对象
        /// </summary>
        /// <param name="strOracle">Oracle语句</param>
        /// <returns></returns>
        public DataTable GetTable(string strOracle)
        {
            #region
            return GetDs(strOracle).Tables[0];
            #endregion
        }

        /// <summary>
        /// 获得DataTable对象,连接未关闭
        /// </summary>
        /// <param name="strOracle">Oracle语句</param>
        /// <returns></returns>
        public DataTable GetNoTable(string strOracle)
        {
            #region
            return GetDsNo(strOracle).Tables[0];
            #endregion
        }

        /// <summary>
        /// 设置DataTable对象
        /// </summary>
        /// <param name="strOracle">Oracle语句</param>
        /// <returns></returns>
        public void SetTable(DataTable dt, string strOracle)
        {
            #region
            sda = new OracleDataAdapter(strOracle, cn);
            sda.Fill(dt);
            #endregion
        }

        /// <summary>
        /// 获得OracleDataReader对象 使用完须关闭DataReader,关闭数据库连接
        /// </summary>
        /// <param name="strOracle">Oracle语句</param>
        /// <returns></returns>
        public OracleDataReader GetDataReader(string strOracle)
        {
            #region
            Open();
            cmd = new OracleCommand(strOracle, cn);
            sdr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            return sdr;
            #endregion
        }

        /// <summary>
        /// 执行Oracle语句
        /// </summary>
        /// <param name="strOracle"></param>
        public void RunOracle(string strOracle)
        {
            #region
            Open();
            cmd = new OracleCommand(strOracle, cn);
            cmd.ExecuteNonQuery();
            Close();
            #endregion
        }

        /// <summary>
        /// 执行存储过程返回OracleDataReader对象
        /// </summary>
        /// <param name="procName">Oracle语句</param>
        public OracleDataReader RunOracleNo(string strOracle)
        {
            #region
            cmd = new OracleCommand(strOracle, cn);
            return cmd.ExecuteReader();
            #endregion
        }

        /// <summary>
        /// 进行事务类型执行存储过程或者SQL语句;参数执行
        /// </summary>
        /// <param name="strOracle"></param>
        /// <param name="typeIsolationLevel"></param>
        /// <param name="prams"></param>
        public void RunOracleNo(string strOracle, IsolationLevel typeIsolationLevel, OracleParameter[] prams)
        {
            #region
            OracleTransaction transaction = cn.BeginTransaction(typeIsolationLevel);
            try
            {
                cmd = CreateCommand(strOracle, CommandType.Text, null, prams);
                cmd.Transaction = transaction;
                cmd.ExecuteNonQuery();
                transaction.Commit();
            }
            catch (System.Data.OracleClient.OracleException e)
            {
                transaction.Rollback();
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                cmd.Dispose();
            }
            #endregion
        }

        /// <summary>
        /// 执行Hashtable多条Sql语句
        /// </summary>
        /// <param name="ht"></param>
        public void RunOracleNo(Hashtable ht)
        {
            cmd = new OracleCommand();
            OracleTransaction transaction = cn.BeginTransaction();
            try
            {
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;
                cmd.Transaction = transaction;
                foreach (DictionaryEntry de in ht)
                {
                    cmd.CommandText = de.Value.ToString();
                    cmd.ExecuteNonQuery();
                }
                transaction.Commit();
            }
            catch (System.Data.OracleClient.OracleException e)
            {
                transaction.Rollback();
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                cmd.Dispose();
            }
        }

        /// <summary>
        /// 执行Hashtable多条Sql语句,违反PK规则不返回错误
        /// </summary>
        /// <param name="ht"></param>
        public void RunOracleNoReturnPkError(Hashtable ht)
        {
            cmd = new OracleCommand();
            OracleTransaction transaction = cn.BeginTransaction();
            try
            {
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;
                cmd.Transaction = transaction;
                foreach (DictionaryEntry de in ht)
                {
                    cmd.CommandText = de.Value.ToString();
                    cmd.ExecuteNonQuery();
                }
                transaction.Commit();
            }
            catch (System.Data.OracleClient.OracleException e)
            {
                transaction.Rollback();
                if (!e.Message.ToUpper().Contains("PK"))//如果触犯PK规则，忽略错误，否则返回错误
                {
                    throw new Exception(e.Message.ToString());
                }
            }
            finally
            {
                cmd.Dispose();
            }
        }

        /// <summary>
        /// 执行多条Sql语句,进行事务处理
        /// </summary>
        /// <param name="OracleStringList"></param>
        public void RunBatchOracleNo(ArrayList OracleStringList)
        {
            cmd = new OracleCommand();
            OracleTransaction transaction = cn.BeginTransaction();
            try
            {
                cmd.Connection = cn;
                cmd.CommandType = CommandType.Text;
                cmd.Transaction = transaction;

                for (int i = 0; i < OracleStringList.Count; i++)
                {
                    string strSql = OracleStringList[i].ToString();
                    cmd.CommandText = strSql;
                    cmd.ExecuteNonQuery();
                }
                transaction.Commit();
            }
            catch (System.Data.OracleClient.OracleException e)
            {
                transaction.Rollback();
                throw new Exception(e.Message.ToString());
            }
            finally
            {
                cmd.Dispose();
            }
        }

        /// <summary>
        /// 更新没有任何返回值
        /// </summary>
        /// <param name="strOracle">Oracle语句</param>
        public string RunOracleNo(string strOracle, string operState)
        {
            #region
            cmd = new OracleCommand(strOracle, cn);
            return cmd.ExecuteNonQuery().ToString();
            #endregion
        }

        public object RunOracleNo(string strOracle, int operState)
        {
            #region
            cmd = new OracleCommand(strOracle, cn);
            return cmd.ExecuteScalar();
            #endregion
        }

        public OracleDataReader RunOracleNo(string strOracle, OracleParameter[] prams)
        {
            #region
            cmd = CreateCommand(strOracle, CommandType.Text, null, prams);
            return cmd.ExecuteReader();
            #endregion
        }

        public void RunOracleNo(string strOracle, OracleParameter[] prams, string operStat)
        {
            #region
            cmd = CreateCommand(strOracle, CommandType.Text, null, prams);
            cmd.ExecuteNonQuery();
            #endregion
        }

        /// <summary>
        /// 执行Oracle语句，并返回第一行第一列结果
        /// </summary>
        /// <param name="strOracle">Oracle语句</param>
        /// <returns></returns>
        public string RunOracleReturn(string strOracle)
        {
            #region
            string strReturn = "";
            Open();
            try
            {
                cmd = new OracleCommand(strOracle, cn);
                strReturn = cmd.ExecuteScalar().ToString();
            }
            catch { }
            finally
            {
                Close();
            }
            return strReturn;
            #endregion
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName">存储过程的名称</param>
        /// <returns>返回存储过程返回值</returns>
        public int RunProc(string procName)
        {
            #region
            cmd = CreateCommand(procName, null);
            cmd.ExecuteNonQuery();
            Close();
            return (int)cmd.Parameters["ReturnValue"].Value;
            #endregion
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="prams">存储过程所需参数</param>
        /// <returns>返回存储过程返回值</returns>
        public object RunProc(string strValue, OracleParameter[] prams)
        {
            #region
            object obj = null;
            try
            {
                cmd = CreateCommand(strValue, CommandType.StoredProcedure, null, prams);
                obj = cmd.ExecuteScalar();
                if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                {
                    return null;
                }
                else
                {
                    return obj;
                }
            }
            catch 
            {
              
            }
            return obj;
            #endregion
        }

        /// <summary>
        /// 执行存储过程返回DataReader对象
        /// </summary>
        /// <param name="procName">Oracle语句</param>
        /// <param name="dataReader">DataReader对象</param>
        public void RunProc(string procName, OracleDataReader dataReader)
        {
            #region
            cmd = CreateCommand(procName, null);
            dataReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            #endregion
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName">存储过程的名称</param>
        /// <param name="prams">存储过程所需参数</param>
        /// <param name="dataReader">DataReader对象</param>
        public void RunProc(string procName, OracleParameter[] prams, OracleDataReader dataReader)
        {
            #region
            cmd = CreateCommand(procName, prams);
            dataReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            #endregion
        }

        /// <summary>
        /// 执行存储过程(后两个参数可置为null)
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="prams">存储过程所需参数</param>
        /// <returns>返回存储过程返回值</returns>
        public void RunProc(string procName, CommandType cmdType, OracleParameter[] prams, string state, string returnValue)
        {
            #region
            cmd = CreateCommand(procName, cmdType, null, prams);
            cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return;
            #endregion
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="prams"></param>
        /// <param name="state"></param>
        public void RunProc(string strValue, OracleParameter[] prams, string state)
        {
            #region
            cmd = CreateCommand(strValue, CommandType.Text, null, prams);
            cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            return;
            #endregion
        }

        /// <summary>
        /// 创建参数
        /// </summary>
        /// <param name="procName">存储过程的名称或者SQL语句</param>
        /// <param name="cmdType">参数类型</param>
        /// <param name="state">状态</param>
        /// <param name="paramObj">参数的值</param>
        /// <returns></returns>
        private OracleCommand CreateCommand(string procName, CommandType cmdType, string state, params OracleParameter[] paramObj)
        {
            cmd = new OracleCommand(procName, cn);
            cmd.CommandType = cmdType;
            if (paramObj != null)
            {
                foreach (OracleParameter parm in paramObj)
                    cmd.Parameters.AddWithValue(parm.ParameterName, parm.Value);
            }
            return cmd;
        }

        /// <summary>
        /// 创建一个OracleCommand对象以此来执行存储过程
        /// </summary>
        /// <param name="procName">存储过程的名称</param>
        /// <param name="prams">存储过程所需参数</param>
        /// <returns>返回OracleCommand对象</returns>
        private OracleCommand CreateCommand(string procName, OracleParameter[] prams)
        {
            #region
            // 确认打开连接
            Open();
            cmd = new OracleCommand(procName, cn);
            cmd.CommandType = CommandType.StoredProcedure;

            // 依次把参数传入存储过程
            if (prams != null)
            {
                foreach (OracleParameter parameter in prams)
                    cmd.Parameters.Add(parameter);
            }

            // 加入返回参数
            //cmd.Parameters.Add(
            //    new OracleParameter("ReturnValue", OracleType.Int32, 4,
            //    ParameterDirection.ReturnValue, false, 0, 0,
            //    string.Empty, DataRowVersion.Default, null));

            return cmd;
            #endregion
        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="prams">存储过程所需参数</param>
        /// <returns>无返回值类型</returns>
        public void RunProcNoResult(string procName, OracleParameter[] prams)
        {
            #region
            cmd = CreateCommand(procName, prams);
            cmd.ExecuteNonQuery();
            Close();
            #endregion
        }

        /// <summary>
        /// 传入输入参数
        /// </summary>
        /// <param name="ParamName">存储过程名称</param>
        /// <param name="DbType">参数类型</param></param>
        /// <param name="Size">参数大小(若为0,则表示不输入)</param>
        /// <param name="Value">参数值</param>
        /// <returns>新的 parameter 对象</returns>
        public OracleParameter MakeInParam(string ParamName, OracleType DbType, int Size, object Value)
        {
            #region
            return MakeParam(ParamName, DbType, Size, ParameterDirection.Input, Value);

            #endregion
        }

        /// <summary>
        /// 传入返回值参数
        /// </summary>
        /// <param name="ParamName">存储过程名称</param>
        /// <param name="DbType">参数类型</param>
        /// <param name="Size">参数大小(若为0,则表示不输入)</param>
        /// <returns>新的 parameter 对象</returns>
        public OracleParameter MakeOutParam(string ParamName, OracleType DbType, int Size)
        {
            #region
            return MakeParam(ParamName, DbType, Size, ParameterDirection.Output, null);
            #endregion
        }

        /// <summary>
        /// 生成存储过程参数
        /// </summary>
        /// <param name="ParamName">存储过程名称</param>
        /// <param name="DbType">参数类型</param>
        /// <param name="Size">参数大小(若为0,则表示不输入)</param>
        /// <param name="Direction">参数方向</param>
        /// <param name="Value">参数值</param>
        /// <returns>新的 parameter 对象</returns>
        public OracleParameter MakeParam(string ParamName, OracleType DbType, Int32 Size, ParameterDirection Direction, object Value)
        {
            #region

            if (Size > 0)
                param = new OracleParameter(ParamName, DbType, Size);
            else
                param = new OracleParameter(ParamName, DbType);

            param.Direction = Direction;
            if (!(Direction == ParameterDirection.Output && Value == null))
                param.Value = Value;

            return param;
            #endregion
        }
    }

    #endregion

    #region OracleDbHelper基类定义

    /// <summary>
    /// 数据访问抽象基础类
    /// Copyright (C) 2004-2008 Xia Wengang
    /// All rights reserved
    /// </summary>
    public abstract class OracleDbHelperDJ
    {
        private static readonly string connectionString = OracleConnectionHelperDJ.GetConnection();

        public static readonly string conString = connectionString;

        public OracleDbHelperDJ() { }

        #region 公用方法

        public static int GetMaxID(string FieldName, string TableName)
        {
            string strOracle = "select max(" + FieldName + ")+1 from " + TableName;
            object obj = OracleDbHelperDJ.GetSingle(strOracle);
            if (obj == null)
            {
                return 1;
            }
            else
            {
                return int.Parse(obj.ToString());
            }
        }

        public static int GetMaxPxh(string FieldName, string TableName)
        {
            string strOracle = "select max(" + FieldName + ")+10 from " + TableName;
            object obj = OracleDbHelperDJ.GetSingle(strOracle);
            if (obj == null)
            {
                return 1;
            }
            else
            {
                return int.Parse(obj.ToString());
            }
        }

        public static bool Exists(string strOracle)
        {
            object obj = OracleDbHelperDJ.GetSingle(strOracle);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static bool Exists(string strOracle, params OracleParameter[] cmdParms)
        {
            object obj = OracleDbHelperDJ.GetSingle(strOracle, cmdParms);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        // 摘要:
        //      取得对象的ID号值.
        public static string GetSequencesID(string seqName, string seqFormat)
        {
            string strOra = "select trim(to_char(" + seqName + ".nextval,'" + seqFormat + "')) seqID from dual";
            object obj = OracleDbHelperDJ.GetSingle(strOra);
            if (obj == null)
            {
                return string.Empty;
            }
            else
            {
                return obj.ToString();
            }

            //using (OracleConnection conn = new OracleConnection(connectionString))
            //{
            //    string strOra = "select trim(to_char(" + seqName + ".nextval,'" + seqFormat + "')) seqID from dual";
            //    using (OracleCommand cmd = new OracleCommand(strOra, conn))
            //    {
            //        try
            //        {
            //            conn.Open();
            //            cmd.EX
            //        }
            //        catch (OracleException E)
            //        {
            //            throw new Exception(E.Message);
            //        }

            //    }
            //}
        }

        #endregion

        #region  执行简单Oracle语句

        /// <summary>
        /// 执行Oracle语句，返回影响的记录数
        /// </summary>
        /// <param name="OracleString">Oracle语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteOracle(string OracleString)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                using (OracleCommand cmd = new OracleCommand(OracleString, connection))
                {
                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (System.Data.OracleClient.OracleException E)
                    {
                        throw new Exception(E.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 执行Oracle语句，返回影响的记录数
        /// </summary>
        /// <param name="OracleString">Oracle语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteOracleScalar(string OracleString)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                using (OracleCommand cmd = new OracleCommand(OracleString, connection))
                {
                    try
                    {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if (obj == null)
                        {
                            return 0;
                        }
                        else
                        {
                            return int.Parse(obj.ToString());
                        }
                    }
                    catch (System.Data.OracleClient.OracleException E)
                    {
                        throw new Exception(E.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }

                }
            }
        }

        /// <summary>
        /// 执行Oracle语句，返回影响的一个Object类型的值
        /// </summary>
        /// <param name="OracleString"></param>
        /// <param name="ReturnObject">无意义的参数</param>
        /// <returns></returns>
        public static object ExecuteOracleScalar(string OracleString, string ReturnObject)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                using (OracleCommand cmd = new OracleCommand(OracleString, connection))
                {
                    try
                    {
                        connection.Open();
                        object obj = cmd.ExecuteOracleScalar();
                        return obj;
                    }
                    catch (System.Data.OracleClient.OracleException E)
                    {
                        throw new Exception(E.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// 执行Oracle语句，返回影响的记录数
        /// </summary>
        /// <param name="OracleString">Oracle语句</param>
        /// <returns>影响的记录数</returns>
        public static string ExecuteOracle(Hashtable ht)
        {
            string returnValue = null;
            using (OracleConnection con = new OracleConnection(connectionString))
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    try
                    {
                        cmd.Connection = con;
                        cmd.CommandType = CommandType.Text;
                        con.Open();
                        cmd.Transaction = con.BeginTransaction();
                        foreach (DictionaryEntry de in ht)
                        {
                            cmd.CommandText = de.Value.ToString();
                            cmd.ExecuteNonQuery();
                        }
                        cmd.Transaction.Commit();
                    }
                    catch (OracleException e)
                    {
                        cmd.Transaction.Rollback();
                        returnValue = e.Message.ToString();
                    }
                    finally
                    {
                        cmd.Dispose();
                        con.Close();
                    }
                }
            }
            return returnValue;
        }

        /// <summary>
        /// 执行Oracle语句，设置命令的执行等待时间
        /// </summary>
        /// <param name="OracleString"></param>
        /// <param name="Times"></param>
        /// <returns></returns>
        public static int ExecuteOracleByTime(string OracleString, int Times)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                using (OracleCommand cmd = new OracleCommand(OracleString, connection))
                {
                    try
                    {
                        connection.Open();
                        cmd.CommandTimeout = Times;
                        int rows = cmd.ExecuteNonQuery();
                        return rows;
                    }
                    catch (System.Data.OracleClient.OracleException E)
                    {
                        connection.Close();
                        throw new Exception(E.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 执行多条Oracle语句，实现数据库事务。
        /// </summary>
        /// <param name="OracleStringList">多条Oracle语句</param>		
        public static void ExecuteOracleTran(ArrayList OracleStringList)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = conn;
                OracleTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    for (int n = 0; n < OracleStringList.Count; n++)
                    {
                        string strOracle = OracleStringList[n].ToString();
                        if (strOracle.Trim().Length > 1)
                        {
                            cmd.CommandText = strOracle;
                            cmd.ExecuteNonQuery();
                        }
                    }
                    tx.Commit();
                }
                catch (System.Data.OracleClient.OracleException E)
                {
                    tx.Rollback();
                    throw new Exception(E.Message);
                }
            }
        }

        /// <summary>
        /// 执行带一个存储过程参数的的Oracle语句。
        /// </summary>
        /// <param name="OracleString">Oracle语句</param>
        /// <param name="content">参数内容,比如一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteOracle(string OracleString, string content)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                OracleCommand cmd = new OracleCommand(OracleString, connection);
                System.Data.OracleClient.OracleParameter myParameter = new System.Data.OracleClient.OracleParameter("@content", OracleType.LongVarChar);
                myParameter.Value = content;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (System.Data.OracleClient.OracleException E)
                {
                    throw new Exception(E.Message);
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 执行带一个存储过程参数的的Oracle语句。
        /// </summary>
        /// <param name="OracleString">Oracle语句</param>
        /// <param name="content">参数内容,比如一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>
        /// <returns>影响的记录数</returns>
        public static object ExecuteOracleGet(string OracleString, string content)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                OracleCommand cmd = new OracleCommand(OracleString, connection);
                System.Data.OracleClient.OracleParameter myParameter = new System.Data.OracleClient.OracleParameter("@content", OracleType.LongVarChar);
                myParameter.Value = content;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    object obj = cmd.ExecuteScalar();
                    if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                    {
                        return null;
                    }
                    else
                    {
                        return obj;
                    }
                }
                catch (System.Data.OracleClient.OracleException E)
                {
                    throw new Exception(E.Message);
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 向数据库里插入图像格式的字段(和上面情况类似的另一种实例)
        /// </summary>
        /// <param name="strOracle">Oracle语句</param>
        /// <param name="fs">图像字节,数据库的字段类型为image的情况</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteOracleInsertImg(string strOracle, byte[] fs)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                OracleCommand cmd = new OracleCommand(strOracle, connection);
                System.Data.OracleClient.OracleParameter myParameter = new System.Data.OracleClient.OracleParameter("@fs", OracleType.Blob);
                myParameter.Value = fs;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows;
                }
                catch (System.Data.OracleClient.OracleException E)
                {
                    throw new Exception(E.Message);
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="OracleString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string OraString)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                using (OracleCommand cmd = new OracleCommand(OraString, connection))
                {
                    try
                    {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.OracleClient.OracleException e)
                    {
                        connection.Close();
                        throw new Exception(e.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回OracleDataReader(使用该方法切记要手工关闭OracleDataReader和连接)
        /// </summary>
        /// <param name="strOracle">查询语句</param>
        /// <returns>OracleDataReader</returns>
        public static OracleDataReader ExecuteReader(string strOracle)
        {
            OracleConnection connection = new OracleConnection(connectionString);
            OracleCommand cmd = new OracleCommand(strOracle, connection);
            try
            {
                connection.Open();
                OracleDataReader myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                return myReader;
            }
            catch (System.Data.OracleClient.OracleException e)
            {
                throw new Exception(e.Message);
            }
            //finally //不能在此关闭，否则，返回的对象将无法使用
            //{
            //	cmd.Dispose();
            //	connection.Close();
            //}
        }

        /// <summary>
        /// 执行查询语句，返回OracleDataReader (使用该方法切记要手工关闭OracleDataReader和连接)
        /// </summary>
        /// <param name="strOracle">查询语句</param>
        /// <returns>OracleDataReader</returns>
        public static OracleDataReader ExecuteReader(string OracleString, params OracleParameter[] cmdParms)
        {
            OracleConnection connection = new OracleConnection(connectionString);
            OracleCommand cmd = new OracleCommand();
            try
            {
                PrepareCommand(cmd, connection, null, OracleString, cmdParms);
                OracleDataReader myReader = cmd.ExecuteReader();
                cmd.Parameters.Clear();
                return myReader;
            }
            catch (System.Data.OracleClient.OracleException e)
            {
                throw new Exception(e.Message);
            }
            //finally //不能在此关闭，否则，返回的对象将无法使用
            //{
            //	cmd.Dispose();
            //	connection.Close();
            //}
        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="OracleString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string OracleString)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    OracleDataAdapter command = new OracleDataAdapter(OracleString, connection);
                    command.Fill(ds, "ds");
                }
                catch (System.Data.OracleClient.OracleException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }

        /// <summary>
        /// 执行查询语句，返回DataSet,设置命令的执行等待时间
        /// </summary>
        /// <param name="OracleString"></param>
        /// <param name="Times"></param>
        /// <returns></returns>
        public static DataSet Query(string OracleString, int Times)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    OracleDataAdapter command = new OracleDataAdapter(OracleString, connection);
                    command.SelectCommand.CommandTimeout = Times;
                    command.Fill(ds, "ds");
                }
                catch (System.Data.OracleClient.OracleException ex)
                {
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }

        /// <summary>
        /// 执行查询语句，设置DataTable对象
        /// </summary>
        /// <param name="OracleString"></param>
        /// <param name="Times"></param>
        /// <returns></returns>
        public static void SetDataTable(ref DataTable dt, string OraString)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                OracleDataAdapter adp = new OracleDataAdapter(OraString, conn);
                adp.Fill(dt);
            }
        }

        #endregion

        #region 执行带参数的Oracle语句

        /// <summary>
        /// 执行Oracle语句，返回影响的记录数
        /// </summary>
        /// <param name="OracleString">Oracle语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteOracle(string OracleString, params OracleParameter[] cmdParms)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, OracleString, cmdParms);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        return rows;
                    }
                    catch (System.Data.OracleClient.OracleException E)
                    {
                        throw new Exception(E.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 执行Oracle语句，返回影响的记录数,返回值类型为Object
        /// </summary>
        /// <param name="OracleString">Oracle语句</param>
        /// <param name="returnObject">无意义的参数,可置为null</param>
        /// <param name="cmdParms">参数值</param>
        /// <returns></returns>
        public static object ExecuteOracle(string OracleString, string returnObject, params OracleParameter[] cmdParms)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, OracleString, cmdParms);
                        object obj = cmd.ExecuteOracleScalar();
                        cmd.Parameters.Clear();
                        return obj;
                    }
                    catch (System.Data.OracleClient.OracleException E)
                    {
                        throw new Exception(E.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 执行多条Oracle语句，实现数据库事务。
        /// </summary>
        /// <param name="OracleStringList">Oracle语句的哈希表（key为Oracle语句，value是该语句的OracleParameter[]）</param>
        public static void ExecuteOracleTran(Hashtable OracleStringList)
        {
            using (OracleConnection conn = new OracleConnection(connectionString))
            {
                conn.Open();
                using (OracleTransaction trans = conn.BeginTransaction())
                {
                    OracleCommand cmd = new OracleCommand();
                    try
                    {
                        //循环
                        foreach (DictionaryEntry myDE in OracleStringList)
                        {
                            string cmdText = myDE.Key.ToString();
                            OracleParameter[] cmdParms = (OracleParameter[])myDE.Value;
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                            int val = cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();

                            trans.Commit();
                        }
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="OracleString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string OraString, params OracleParameter[] cmdParms)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                using (OracleCommand cmd = new OracleCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, OraString, cmdParms);
                        object obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            return null;
                        }
                        else
                        {
                            return obj;
                        }
                    }
                    catch (System.Data.OracleClient.OracleException e)
                    {
                        throw new Exception(e.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="OracleString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string OracleString, params OracleParameter[] cmdParms)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                OracleCommand cmd = new OracleCommand();
                PrepareCommand(cmd, connection, null, OracleString, cmdParms);
                using (OracleDataAdapter da = new OracleDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                    }
                    catch (System.Data.OracleClient.OracleException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return ds;
                }
            }
        }

        private static void PrepareCommand(OracleCommand cmd, OracleConnection conn, OracleTransaction trans, string cmdText, OracleParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (OracleParameter parameter in cmdParms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    cmd.Parameters.Add(parameter);
                }
            }
        }

        #endregion

        #region 存储过程操作

        ///// <summary>
        ///// 执行存储过程
        ///// </summary>
        ///// <param name="storedProcName">存储过程名</param>
        ///// <param name="parameters">存储过程参数</param>
        ///// <returns>string</returns>
        //public static string RunProcedure(string storedProcName, OracleParameter[] parameters)
        //{
        //    OracleConnection con = new OracleConnection(connectionString);
        //    OracleCommand cmd = new OracleCommand(storedProcName, con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //}

        /// <summary>
        /// 执行存储过程  (使用该方法切记要手工关闭OracleDataReader和连接)
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>OracleDataReader</returns>
        public static OracleDataReader RunProcedure(string storedProcName, IDataParameter[] parameters)
        {
            OracleConnection connection = new OracleConnection(connectionString);
            OracleDataReader returnReader;
            connection.Open();
            OracleCommand command = BuildQueryCommand(connection, storedProcName, parameters);
            command.CommandType = CommandType.StoredProcedure;
            returnReader = command.ExecuteReader();
            //Connection.Close(); 不能在此关闭，否则，返回的对象将无法使用            
            return returnReader;

        }

        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="tableName">DataSet结果中的表名</param>
        /// <returns>DataSet</returns>
        public static DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                DataSet dataSet = new DataSet();
                connection.Open();
                OracleDataAdapter OracleDA = new OracleDataAdapter();
                OracleDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
                OracleDA.Fill(dataSet, tableName);
                connection.Close();
                return dataSet;
            }
        }

        public static DataSet RunProcedure(string storedProcName, IDataParameter[] parameters, string tableName, int Times)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                DataSet dataSet = new DataSet();
                connection.Open();
                OracleDataAdapter OracleDA = new OracleDataAdapter();
                OracleDA.SelectCommand = BuildQueryCommand(connection, storedProcName, parameters);
                OracleDA.SelectCommand.CommandTimeout = Times;
                OracleDA.Fill(dataSet, tableName);
                connection.Close();
                connection.Dispose();
                return dataSet;
            }
        }

        /// <summary>
        /// 构建 OracleCommand 对象(用来返回一个结果集，而不是一个整数值)
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>OracleCommand</returns>
        private static OracleCommand BuildQueryCommand(OracleConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            OracleCommand command = new OracleCommand(storedProcName, connection);
            command.CommandType = CommandType.StoredProcedure;
            foreach (OracleParameter parameter in parameters)
            {
                if (parameter != null)
                {
                    // 检查未分配值的输出参数,将其分配以DBNull.Value.
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) &&
                        (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                    command.Parameters.Add(parameter);
                }
            }

            return command;
        }

        /// <summary>
        /// 执行存储过程，返回影响的行数		
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <param name="rowsAffected">影响的行数</param>
        /// <returns></returns>
        public static int RunProcedure(string storedProcName, IDataParameter[] parameters, out int rowsAffected)
        {
            using (OracleConnection connection = new OracleConnection(connectionString))
            {
                int result;
                connection.Open();
                OracleCommand command = BuildIntCommand(connection, storedProcName, parameters);
                rowsAffected = command.ExecuteNonQuery();
                result = (int)command.Parameters["ReturnValue"].Value;
                //Connection.Close();
                return result;
            }
        }

        /// <summary>
        /// 创建 OracleCommand 对象实例(用来返回一个整数值)	
        /// </summary>
        /// <param name="storedProcName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>OracleCommand 对象实例</returns>
        private static OracleCommand BuildIntCommand(OracleConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            OracleCommand command = BuildQueryCommand(connection, storedProcName, parameters);
            command.Parameters.Add(new OracleParameter("ReturnValue",
                OracleType.Int32, 4, ParameterDirection.ReturnValue,
                false, 0, 0, string.Empty, DataRowVersion.Default, null));
            return command;
        }
        #endregion

    }

    #endregion
}