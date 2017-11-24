#pragma warning disable 0618
namespace MyComponets.DBUtility
{
    using System;
    using System.Configuration;
    using System.Data;
    using System.Data.OracleClient;
    using System.Collections;


    #region OracleConnectionHelper ����
    internal static class OracleConnectionHelperDJ
    {
        /// <summary>
        /// �������ݿ�������ַ���
        /// </summary>
        /// <returns></returns>
        static internal string GetConnection()
        {
            return System.Configuration.ConfigurationManager.ConnectionStrings["OraConnStringDJ"].ConnectionString;
        }
    }
    #endregion

    #region OracleHelper���ඨ��

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
        /// �Զ���ֻ��SQL���Ͳ�����ֵ
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

    #region OracleDataBase���ඨ��

    /// <summary>
    /// ���ݿ��������������
    /// </summary>
    public abstract class OracleDataBaseDJ
    {
        //��ȡWeb.Config���ݿ������ַ���
        private readonly string OracleConnectionString = OracleConnectionHelperDJ.GetConnection();

        private OracleConnection cn;		//����Oracle����
        private OracleDataAdapter sda;		//����Oracle����������
        private OracleDataReader sdr;		//����Oracle���ݶ�ȡ��
        private OracleCommand cmd;			//����Oracle�������
        private OracleParameter param;      //����Oracle����
        private DataSet ds;				    //�������ݼ�
        private DataView dv;			    //������ͼ

        /// <summary>
        /// �����ݿ�����
        /// </summary>
        public void Open()
        {
            #region
            cn = new OracleConnection(OracleConnectionString);
            cn.Open();
            #endregion
        }

        /// <summary>
        /// �ر����ݿ�����
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
        /// ����DataSet���ݼ�
        /// </summary>
        /// <param name="strOracle">Oracle���</param>
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
        /// ����DataSet���ݼ�,�޴���ر����Ӳ���
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
        /// ���DataSet��
        /// </summary>
        /// <param name="ds">DataSet����</param>
        /// <param name="strOracle">Oracle���</param>
        /// <param name="strTableName">����</param>
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
        /// ����DataView������ͼ
        /// </summary>
        /// <param name="strOracle">Oracle���</param>
        public DataView GetDv(string strOracle)
        {
            #region
            dv = GetDs(strOracle).Tables[0].DefaultView;
            return dv;
            #endregion
        }

        /// <summary>
        /// ���DataTable����
        /// </summary>
        /// <param name="strOracle">Oracle���</param>
        /// <returns></returns>
        public DataTable GetTable(string strOracle)
        {
            #region
            return GetDs(strOracle).Tables[0];
            #endregion
        }

        /// <summary>
        /// ���DataTable����,����δ�ر�
        /// </summary>
        /// <param name="strOracle">Oracle���</param>
        /// <returns></returns>
        public DataTable GetNoTable(string strOracle)
        {
            #region
            return GetDsNo(strOracle).Tables[0];
            #endregion
        }

        /// <summary>
        /// ����DataTable����
        /// </summary>
        /// <param name="strOracle">Oracle���</param>
        /// <returns></returns>
        public void SetTable(DataTable dt, string strOracle)
        {
            #region
            sda = new OracleDataAdapter(strOracle, cn);
            sda.Fill(dt);
            #endregion
        }

        /// <summary>
        /// ���OracleDataReader���� ʹ������ر�DataReader,�ر����ݿ�����
        /// </summary>
        /// <param name="strOracle">Oracle���</param>
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
        /// ִ��Oracle���
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
        /// ִ�д洢���̷���OracleDataReader����
        /// </summary>
        /// <param name="procName">Oracle���</param>
        public OracleDataReader RunOracleNo(string strOracle)
        {
            #region
            cmd = new OracleCommand(strOracle, cn);
            return cmd.ExecuteReader();
            #endregion
        }

        /// <summary>
        /// ������������ִ�д洢���̻���SQL���;����ִ��
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
        /// ִ��Hashtable����Sql���
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
        /// ִ��Hashtable����Sql���,Υ��PK���򲻷��ش���
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
                if (!e.Message.ToUpper().Contains("PK"))//�������PK���򣬺��Դ��󣬷��򷵻ش���
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
        /// ִ�ж���Sql���,����������
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
        /// ����û���κη���ֵ
        /// </summary>
        /// <param name="strOracle">Oracle���</param>
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
        /// ִ��Oracle��䣬�����ص�һ�е�һ�н��
        /// </summary>
        /// <param name="strOracle">Oracle���</param>
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
        /// ִ�д洢����
        /// </summary>
        /// <param name="procName">�洢���̵�����</param>
        /// <returns>���ش洢���̷���ֵ</returns>
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
        /// ִ�д洢����
        /// </summary>
        /// <param name="procName">�洢��������</param>
        /// <param name="prams">�洢�����������</param>
        /// <returns>���ش洢���̷���ֵ</returns>
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
        /// ִ�д洢���̷���DataReader����
        /// </summary>
        /// <param name="procName">Oracle���</param>
        /// <param name="dataReader">DataReader����</param>
        public void RunProc(string procName, OracleDataReader dataReader)
        {
            #region
            cmd = CreateCommand(procName, null);
            dataReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            #endregion
        }

        /// <summary>
        /// ִ�д洢����
        /// </summary>
        /// <param name="procName">�洢���̵�����</param>
        /// <param name="prams">�洢�����������</param>
        /// <param name="dataReader">DataReader����</param>
        public void RunProc(string procName, OracleParameter[] prams, OracleDataReader dataReader)
        {
            #region
            cmd = CreateCommand(procName, prams);
            dataReader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            #endregion
        }

        /// <summary>
        /// ִ�д洢����(��������������Ϊnull)
        /// </summary>
        /// <param name="procName">�洢��������</param>
        /// <param name="prams">�洢�����������</param>
        /// <returns>���ش洢���̷���ֵ</returns>
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
        /// ��������
        /// </summary>
        /// <param name="procName">�洢���̵����ƻ���SQL���</param>
        /// <param name="cmdType">��������</param>
        /// <param name="state">״̬</param>
        /// <param name="paramObj">������ֵ</param>
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
        /// ����һ��OracleCommand�����Դ���ִ�д洢����
        /// </summary>
        /// <param name="procName">�洢���̵�����</param>
        /// <param name="prams">�洢�����������</param>
        /// <returns>����OracleCommand����</returns>
        private OracleCommand CreateCommand(string procName, OracleParameter[] prams)
        {
            #region
            // ȷ�ϴ�����
            Open();
            cmd = new OracleCommand(procName, cn);
            cmd.CommandType = CommandType.StoredProcedure;

            // ���ΰѲ�������洢����
            if (prams != null)
            {
                foreach (OracleParameter parameter in prams)
                    cmd.Parameters.Add(parameter);
            }

            // ���뷵�ز���
            //cmd.Parameters.Add(
            //    new OracleParameter("ReturnValue", OracleType.Int32, 4,
            //    ParameterDirection.ReturnValue, false, 0, 0,
            //    string.Empty, DataRowVersion.Default, null));

            return cmd;
            #endregion
        }

        /// <summary>
        /// ִ�д洢����
        /// </summary>
        /// <param name="procName">�洢��������</param>
        /// <param name="prams">�洢�����������</param>
        /// <returns>�޷���ֵ����</returns>
        public void RunProcNoResult(string procName, OracleParameter[] prams)
        {
            #region
            cmd = CreateCommand(procName, prams);
            cmd.ExecuteNonQuery();
            Close();
            #endregion
        }

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="ParamName">�洢��������</param>
        /// <param name="DbType">��������</param></param>
        /// <param name="Size">������С(��Ϊ0,���ʾ������)</param>
        /// <param name="Value">����ֵ</param>
        /// <returns>�µ� parameter ����</returns>
        public OracleParameter MakeInParam(string ParamName, OracleType DbType, int Size, object Value)
        {
            #region
            return MakeParam(ParamName, DbType, Size, ParameterDirection.Input, Value);

            #endregion
        }

        /// <summary>
        /// ���뷵��ֵ����
        /// </summary>
        /// <param name="ParamName">�洢��������</param>
        /// <param name="DbType">��������</param>
        /// <param name="Size">������С(��Ϊ0,���ʾ������)</param>
        /// <returns>�µ� parameter ����</returns>
        public OracleParameter MakeOutParam(string ParamName, OracleType DbType, int Size)
        {
            #region
            return MakeParam(ParamName, DbType, Size, ParameterDirection.Output, null);
            #endregion
        }

        /// <summary>
        /// ���ɴ洢���̲���
        /// </summary>
        /// <param name="ParamName">�洢��������</param>
        /// <param name="DbType">��������</param>
        /// <param name="Size">������С(��Ϊ0,���ʾ������)</param>
        /// <param name="Direction">��������</param>
        /// <param name="Value">����ֵ</param>
        /// <returns>�µ� parameter ����</returns>
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

    #region OracleDbHelper���ඨ��

    /// <summary>
    /// ���ݷ��ʳ��������
    /// Copyright (C) 2004-2008 Xia Wengang
    /// All rights reserved
    /// </summary>
    public abstract class OracleDbHelperDJ
    {
        private static readonly string connectionString = OracleConnectionHelperDJ.GetConnection();

        public static readonly string conString = connectionString;

        public OracleDbHelperDJ() { }

        #region ���÷���

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

        // ժҪ:
        //      ȡ�ö����ID��ֵ.
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

        #region  ִ�м�Oracle���

        /// <summary>
        /// ִ��Oracle��䣬����Ӱ��ļ�¼��
        /// </summary>
        /// <param name="OracleString">Oracle���</param>
        /// <returns>Ӱ��ļ�¼��</returns>
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
        /// ִ��Oracle��䣬����Ӱ��ļ�¼��
        /// </summary>
        /// <param name="OracleString">Oracle���</param>
        /// <returns>Ӱ��ļ�¼��</returns>
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
        /// ִ��Oracle��䣬����Ӱ���һ��Object���͵�ֵ
        /// </summary>
        /// <param name="OracleString"></param>
        /// <param name="ReturnObject">������Ĳ���</param>
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
        /// ִ��Oracle��䣬����Ӱ��ļ�¼��
        /// </summary>
        /// <param name="OracleString">Oracle���</param>
        /// <returns>Ӱ��ļ�¼��</returns>
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
        /// ִ��Oracle��䣬���������ִ�еȴ�ʱ��
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
        /// ִ�ж���Oracle��䣬ʵ�����ݿ�����
        /// </summary>
        /// <param name="OracleStringList">����Oracle���</param>		
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
        /// ִ�д�һ���洢���̲����ĵ�Oracle��䡣
        /// </summary>
        /// <param name="OracleString">Oracle���</param>
        /// <param name="content">��������,����һ���ֶ��Ǹ�ʽ���ӵ����£���������ţ�����ͨ�������ʽ���</param>
        /// <returns>Ӱ��ļ�¼��</returns>
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
        /// ִ�д�һ���洢���̲����ĵ�Oracle��䡣
        /// </summary>
        /// <param name="OracleString">Oracle���</param>
        /// <param name="content">��������,����һ���ֶ��Ǹ�ʽ���ӵ����£���������ţ�����ͨ�������ʽ���</param>
        /// <returns>Ӱ��ļ�¼��</returns>
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
        /// �����ݿ������ͼ���ʽ���ֶ�(������������Ƶ���һ��ʵ��)
        /// </summary>
        /// <param name="strOracle">Oracle���</param>
        /// <param name="fs">ͼ���ֽ�,���ݿ���ֶ�����Ϊimage�����</param>
        /// <returns>Ӱ��ļ�¼��</returns>
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
        /// ִ��һ�������ѯ�����䣬���ز�ѯ�����object����
        /// </summary>
        /// <param name="OracleString">�����ѯ������</param>
        /// <returns>��ѯ�����object��</returns>
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
        /// ִ�в�ѯ��䣬����OracleDataReader(ʹ�ø÷����м�Ҫ�ֹ��ر�OracleDataReader������)
        /// </summary>
        /// <param name="strOracle">��ѯ���</param>
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
            //finally //�����ڴ˹رգ����򣬷��صĶ����޷�ʹ��
            //{
            //	cmd.Dispose();
            //	connection.Close();
            //}
        }

        /// <summary>
        /// ִ�в�ѯ��䣬����OracleDataReader (ʹ�ø÷����м�Ҫ�ֹ��ر�OracleDataReader������)
        /// </summary>
        /// <param name="strOracle">��ѯ���</param>
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
            //finally //�����ڴ˹رգ����򣬷��صĶ����޷�ʹ��
            //{
            //	cmd.Dispose();
            //	connection.Close();
            //}
        }

        /// <summary>
        /// ִ�в�ѯ��䣬����DataSet
        /// </summary>
        /// <param name="OracleString">��ѯ���</param>
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
        /// ִ�в�ѯ��䣬����DataSet,���������ִ�еȴ�ʱ��
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
        /// ִ�в�ѯ��䣬����DataTable����
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

        #region ִ�д�������Oracle���

        /// <summary>
        /// ִ��Oracle��䣬����Ӱ��ļ�¼��
        /// </summary>
        /// <param name="OracleString">Oracle���</param>
        /// <returns>Ӱ��ļ�¼��</returns>
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
        /// ִ��Oracle��䣬����Ӱ��ļ�¼��,����ֵ����ΪObject
        /// </summary>
        /// <param name="OracleString">Oracle���</param>
        /// <param name="returnObject">������Ĳ���,����Ϊnull</param>
        /// <param name="cmdParms">����ֵ</param>
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
        /// ִ�ж���Oracle��䣬ʵ�����ݿ�����
        /// </summary>
        /// <param name="OracleStringList">Oracle���Ĺ�ϣ��keyΪOracle��䣬value�Ǹ�����OracleParameter[]��</param>
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
                        //ѭ��
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
        /// ִ��һ�������ѯ�����䣬���ز�ѯ�����object����
        /// </summary>
        /// <param name="OracleString">�����ѯ������</param>
        /// <returns>��ѯ�����object��</returns>
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
        /// ִ�в�ѯ��䣬����DataSet
        /// </summary>
        /// <param name="OracleString">��ѯ���</param>
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

        #region �洢���̲���

        ///// <summary>
        ///// ִ�д洢����
        ///// </summary>
        ///// <param name="storedProcName">�洢������</param>
        ///// <param name="parameters">�洢���̲���</param>
        ///// <returns>string</returns>
        //public static string RunProcedure(string storedProcName, OracleParameter[] parameters)
        //{
        //    OracleConnection con = new OracleConnection(connectionString);
        //    OracleCommand cmd = new OracleCommand(storedProcName, con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //}

        /// <summary>
        /// ִ�д洢����  (ʹ�ø÷����м�Ҫ�ֹ��ر�OracleDataReader������)
        /// </summary>
        /// <param name="storedProcName">�洢������</param>
        /// <param name="parameters">�洢���̲���</param>
        /// <returns>OracleDataReader</returns>
        public static OracleDataReader RunProcedure(string storedProcName, IDataParameter[] parameters)
        {
            OracleConnection connection = new OracleConnection(connectionString);
            OracleDataReader returnReader;
            connection.Open();
            OracleCommand command = BuildQueryCommand(connection, storedProcName, parameters);
            command.CommandType = CommandType.StoredProcedure;
            returnReader = command.ExecuteReader();
            //Connection.Close(); �����ڴ˹رգ����򣬷��صĶ����޷�ʹ��            
            return returnReader;

        }

        /// <summary>
        /// ִ�д洢����
        /// </summary>
        /// <param name="storedProcName">�洢������</param>
        /// <param name="parameters">�洢���̲���</param>
        /// <param name="tableName">DataSet����еı���</param>
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
        /// ���� OracleCommand ����(��������һ���������������һ������ֵ)
        /// </summary>
        /// <param name="connection">���ݿ�����</param>
        /// <param name="storedProcName">�洢������</param>
        /// <param name="parameters">�洢���̲���</param>
        /// <returns>OracleCommand</returns>
        private static OracleCommand BuildQueryCommand(OracleConnection connection, string storedProcName, IDataParameter[] parameters)
        {
            OracleCommand command = new OracleCommand(storedProcName, connection);
            command.CommandType = CommandType.StoredProcedure;
            foreach (OracleParameter parameter in parameters)
            {
                if (parameter != null)
                {
                    // ���δ����ֵ���������,���������DBNull.Value.
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
        /// ִ�д洢���̣�����Ӱ�������		
        /// </summary>
        /// <param name="storedProcName">�洢������</param>
        /// <param name="parameters">�洢���̲���</param>
        /// <param name="rowsAffected">Ӱ�������</param>
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
        /// ���� OracleCommand ����ʵ��(��������һ������ֵ)	
        /// </summary>
        /// <param name="storedProcName">�洢������</param>
        /// <param name="parameters">�洢���̲���</param>
        /// <returns>OracleCommand ����ʵ��</returns>
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