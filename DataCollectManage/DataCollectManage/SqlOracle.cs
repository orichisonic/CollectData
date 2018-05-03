﻿using System;
using System.Data;
using System.Web;
using System.Text;
using System.Collections;
using System.Configuration;
using Oracle.DataAccess.Client;
using System.Windows.Forms;
using System.Threading;
//using System.Data.OracleClient;

namespace DataCollectMonitor
{
    /// <summary>
    /// 数据库通用操作类
    /// </summary>
    public class SqlOracle
    {
        protected OracleConnection con;//连接对象
        protected OracleTransaction myOracleTransaction;


        public SqlOracle()
        {

        }
        public SqlOracle(string constr)
        {
            //System.Data.OracleClient.OracleCommand aa = new System.Data.OracleClient.OracleCommand()
            con = new OracleConnection(constr);
        }

        #region 打开数据库连接
        /// <summary>
        /// 打开数据库连接
        /// </summary>
        private void Open()
        {
            //打开数据库连接
            if (con.State == ConnectionState.Closed)
            {
                try
                {
                    //打开数据库连接
                    con.Open();
                    
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
        #endregion
        #region 关闭数据库连接
        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        private void Close()
        {
            //判断连接的状态是否已经打开
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }

        }
        #endregion

        #region 执行操作语句
        /// <summary>
        /// 执行操作语句
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <returns>int</returns>   
        public int ExecuteNonquery(string sql)
        {
            int iEffecienRows;
            try
            {
                Open();
                this.BeginTran();
                OracleCommand cmd = new OracleCommand(sql, con);
                iEffecienRows= cmd.ExecuteNonQuery();
                this.CommitTran();
                return iEffecienRows;
            }
            catch (Oracle.DataAccess.Client.OracleException ex)
            {
                Console.WriteLine(ex.Message);
                this.RollbackTran();
                return 0;
            }
             finally
            {
                Close();//关闭数据库连接
            }
        }
        #endregion
        #region 执行查询语句，返回OracleDataReader ( 注意：调用该方法后，一定要对OracleDataReader进行Close )
        /// <summary>
        /// 执行查询语句，返回OracleDataReader ( 注意：调用该方法后，一定要对OracleDataReader进行Close )
        /// </summary>
        /// <param name="sql">查询语句</param>
        /// <returns>OracleDataReader</returns>   
        public OracleDataReader ExecuteReader(string sql)
        {
            OracleDataReader myReader;
            Open();
            OracleCommand cmd = new OracleCommand(sql, con);
            myReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            return myReader;
        }
        #endregion
        #region 建表
        public int CreateTable(string fields, string tableName, int iflg, DataTable dtIP, string strTableSpace)
        {
            int iEffecienRows;
            DataSet ds = new DataSet();
            string strPartion = string.Empty;
            try
            {
                Open();
               // this.BeginTran();
                tableName = tableName.ToUpper();
                string sql = "SELECT * FROM all_objects WHERE OBJECT_TYPE='TABLE' AND OBJECT_NAME='"+tableName+"'";
                OracleDataAdapter adapter = new OracleDataAdapter(sql, con);
                adapter.Fill(ds);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (iflg == 1)
                    {
                        //if (MessageBox.Show("目的数据表已存在，是否要删除重新建立？（若不进行此操作，则有可能导致采集失败）", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        //{
                            sql = "Drop Table " + tableName;
                            OracleCommand cmd = new OracleCommand(sql, con);
                            iEffecienRows = cmd.ExecuteNonQuery();
                        //}
                        //else
                        //{
                        //    return 0;
                        //}
                    }
                    else
                    {
                        sql = "Drop Table " + tableName;
                        OracleCommand cmd = new OracleCommand(sql, con);
                        iEffecienRows = cmd.ExecuteNonQuery();
                    }
                }
                string[] row = fields.Split(',');
                sql = "create table " + tableName + "(";
                for (int i = 0; i < row.Length; i++)
                {
                    string[] strTmp = row[i].Split(' ');
                    if (i == 0)
                    {

                        if (strTmp[1].ToUpper().ToString() == "DATE")
                        {
                            //如果字段名为DATE
                            if (strTmp[0].ToUpper() == "DATE")
                            {
                                sql = sql + "\"" + strTmp[0].ToUpper() + "\" " + strTmp[1];
                            }
                            else
                            {
                                sql = sql + strTmp[0] + " " + strTmp[2];
                            }
                        }
                        else if (strTmp[0].ToUpper().ToString() == "LEVEL")
                        {
                            //如果字段名为level
                            if (strTmp[0].ToUpper() == "LEVEL")
                            {
                                sql = sql + "\"" + strTmp[0].ToUpper() + "\" " + strTmp[1];
                            }
                            else
                            {
                                sql = sql + strTmp[0] + " " + strTmp[1];
                            }
                        }
                        else
                        {
                            sql = sql + strTmp[0] + " " + strTmp[1];
                        }
                    }
                    else
                    {
                        if (strTmp[1].ToUpper().ToString() == "DATE")
                        {
                            //如果字段名为DATE
                            if (strTmp[0].ToUpper() == "DATE")
                            {
                                sql = sql + "," + "\"" + strTmp[0].ToUpper() + "\" " + strTmp[1];
                            }
                            else
                            {
                                sql = sql + "," + strTmp[0] + " " + strTmp[1];
                            }
                        }
                        else if (strTmp[1].ToUpper().ToString() == "LEVEL")
                        {
                            //如果字段名为level
                            if (strTmp[0].ToUpper() == "LEVEL")
                            {
                                sql = sql + "\"" + strTmp[0].ToUpper() + "\" " + strTmp[1];
                            }
                            else
                            {
                                sql = sql + strTmp[0] + " " + strTmp[1];
                            }
                        }
                        else
                        {
                            sql = sql + "," + strTmp[0] + " " + strTmp[1];
                        }
                    }
                }
                sql = sql + ")";
                strPartion = CommonCLS.GetPartition(fields, dtIP, strTableSpace);
                OracleCommand cmd2 = new OracleCommand(sql + " " + strPartion, con);

                //ParameterizedThreadStart ParStart = new System.Threading.ParameterizedThreadStart(runCMD);
                //Thread myThread = new Thread(runCMD);
                //myThread.Start(sql + " " + strPartion);
                iEffecienRows = cmd2.ExecuteNonQuery();
               // this.CommitTran();
               
                return 1;
               
              
            }
            catch (Oracle.DataAccess.Client.OracleException ex) 
            {
                Console.WriteLine(ex.Message);
                this.RollbackTran();
                return 0;
            }
            finally
            {
                Close();//关闭数据库连接
            }
        }

        public string  CreateTableString(string fields, string tableName, int iflg, DataTable dtIP, string strTableSpace)
        {
            int iEffecienRows;
            DataSet ds = new DataSet();
            string strPartion = string.Empty;
            try
            {
                Open();
                // this.BeginTran();
                tableName = tableName.ToUpper();
                string sql = "SELECT * FROM all_objects WHERE OBJECT_TYPE='TABLE' AND OBJECT_NAME='" + tableName + "'";
                OracleDataAdapter adapter = new OracleDataAdapter(sql, con);
                adapter.Fill(ds);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    if (iflg == 1)
                    {
                        //if (MessageBox.Show("目的数据表已存在，是否要删除重新建立？（若不进行此操作，则有可能导致采集失败）", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        //{
                        sql = "Drop Table " + tableName;
                        OracleCommand cmd = new OracleCommand(sql, con);
                        iEffecienRows = cmd.ExecuteNonQuery();
                        //}
                        //else
                        //{
                        //    return 0;
                        //}
                    }
                    else
                    {
                        sql = "Drop Table " + tableName;
                        OracleCommand cmd = new OracleCommand(sql, con);
                        iEffecienRows = cmd.ExecuteNonQuery();
                    }
                }
                string[] row = fields.Split(',');
                sql = "create table " + tableName + "(";
                for (int i = 0; i < row.Length; i++)
                {
                    string[] strTmp = row[i].Split(' ');
                    if (i == 0)
                    {

                        if (strTmp[1].ToUpper().ToString() == "DATE")
                        {
                            //如果字段名为DATE
                            if (strTmp[0].ToUpper() == "DATE")
                            {
                                sql = sql + "\"" + strTmp[0].ToUpper() + "\" " + strTmp[1];
                            }
                            else
                            {
                                sql = sql + strTmp[0] + " " + strTmp[2];
                            }
                        }
                        else if (strTmp[0].ToUpper().ToString() == "LEVEL")
                        {
                            //如果字段名为level
                            if (strTmp[0].ToUpper() == "LEVEL")
                            {
                                sql = sql + "\"" + strTmp[0].ToUpper() + "\" " + strTmp[1];
                            }
                            else
                            {
                                sql = sql + strTmp[0] + " " + strTmp[1];
                            }
                        }
                        else
                        {
                            sql = sql + strTmp[0] + " " + strTmp[1];
                        }
                    }
                    else
                    {
                        if (strTmp[1].ToUpper().ToString() == "DATE")
                        {
                            //如果字段名为DATE
                            if (strTmp[0].ToUpper() == "DATE")
                            {
                                sql = sql + "," + "\"" + strTmp[0].ToUpper() + "\" " + strTmp[1];
                            }
                            else
                            {
                                sql = sql + "," + strTmp[0] + " " + strTmp[1];
                            }
                        }
                        else if (strTmp[1].ToUpper().ToString() == "LEVEL")
                        {
                            //如果字段名为level
                            if (strTmp[0].ToUpper() == "LEVEL")
                            {
                                sql = sql + "\"" + strTmp[0].ToUpper() + "\" " + strTmp[1];
                            }
                            else
                            {
                                sql = sql + strTmp[0] + " " + strTmp[1];
                            }
                        }
                        else
                        {
                            sql = sql + "," + strTmp[0] + " " + strTmp[1];
                        }
                    }
                }
                sql = sql + ")";
                strPartion = CommonCLS.GetPartition(fields, dtIP, strTableSpace);

                return sql + " " + strPartion;


            }
            catch (Oracle.DataAccess.Client.OracleException ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
            finally
            {

            }
        }
        private void runCMD(object txtCMD)
        {
            OracleCommand cmd2 = new OracleCommand((string)txtCMD,con);
            cmd2.ExecuteNonQuery();
        }
        #endregion
        #region 执行SQL语句，返回数据到DataSet中
        /// <summary>
        /// 执行SQL语句，返回数据到DataSet中
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>返回DataSet</returns>
        public DataSet GetDataSet(string sql)
        {
            DataSet ds = new DataSet();
            try
            {
                Open();//打开数据连接
                OracleDataAdapter adapter = new OracleDataAdapter(sql, con);
                adapter.Fill(ds);
            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog(ex.ToString());
            }
            finally
            {
                Close();//关闭数据库连接
            }
            return ds;
        }
        #endregion
        #region 执行存储过程，返回数据到DataSet中

        /// <summary>
        /// 执行SQL语句，返回数据到DataSet中
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>返回DataSet</returns>
        public DataSet GetDataSetBySP(string storedProcName, OracleParameter[] parameters)
        {
            DataSet ds = new DataSet();
            try
            {
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = this.con; 
                Open();//打开数据连接

                cmd.CommandText = storedProcName;
                cmd.CommandType = CommandType.StoredProcedure;
                foreach (OracleParameter parameter in parameters)
                {
                    cmd.Parameters.Add(parameter);
                }
                OracleDataAdapter da = new OracleDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Oracle.DataAccess.Client.OracleException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Close();//关闭数据库连接

            }
            return ds;
        }

        /// <summary>
        /// 执行SQL语句，返回数据到DataSet中
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>返回DataSet</returns>
        public DataSet GetDataSetBySP(string storedProcName)
        {
            DataSet ds = new DataSet();
            try
            {
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = this.con;
                Open();//打开数据连接
                cmd.CommandText = storedProcName;
                cmd.CommandType = CommandType.StoredProcedure;
                OracleParameter p1 = new OracleParameter("mycs", OracleDbType.RefCursor);
                p1.Direction =  ParameterDirection.Output;
                cmd.Parameters.Add(p1);
                OracleDataAdapter da = new OracleDataAdapter(cmd);
                da.Fill(ds);
            }
            catch (Oracle.DataAccess.Client.OracleException ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Close();//关闭数据库连接
            }
            return ds;
        }

        /// <summary>
        /// 执行SQL语句，返回数据到DataSet中
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>返回DataSet</returns>
        public int ExecuteNonqueryBySP(string storedProcName, OracleParameter[] parameters)
        {
            DataSet ds = new DataSet();
            try
            {
                OracleCommand cmd = new OracleCommand();
                cmd.Connection = this.con;
                Open();//打开数据连接
                cmd.CommandText = storedProcName;
                cmd.CommandType = CommandType.StoredProcedure;
                foreach (OracleParameter parameter in parameters)
                {
                    cmd.Parameters.Add(parameter);
                }
                cmd.ExecuteNonQuery();
                if (cmd.Parameters[parameters.Length - 1].Value.ToString()=="null")
                {
                    return 1;
                }
                else
                {
                    return Convert.ToInt32(cmd.Parameters[parameters.Length - 1].Value);
                }
            }
            catch (Oracle.DataAccess.Client.OracleException ex)
            {
                Console.WriteLine(ex.Message);
                return 0;
            }
            finally
            {
                Close();//关闭数据库连接

            }
        }
        #endregion
        #region 删除数据
        public int DelData(string id)
        {
            int iEffecienRows;
            DataSet ds = new DataSet();
            try
            {
                Open();
                this.BeginTran();
                string sql = "";
                string sql2 = "";

               
                //删除import_express
                //sql2 = "Delete FROM import_express where gamedb_id =" + id;
                //cmd.CommandText = sql2;
                //iEffecienRows = iEffecienRows +cmd.ExecuteNonQuery();

                OracleParameter[] parameters1 = 
                    {
                        new OracleParameter("v_id", OracleDbType.Int32),
                        new OracleParameter("v_result", OracleDbType.Decimal)
                    };
                parameters1[0].Direction = ParameterDirection.Input;
                parameters1[1].Direction = ParameterDirection.Output;
                parameters1[0].Value = id;
                iEffecienRows =  CommonCLS.RunOracleNonQuerySp("PD_Game_Admin.PD_Import_express_Delete", parameters1);

                this.CommitTran();
                return iEffecienRows;


            }
            catch (Oracle.DataAccess.Client.OracleException ex)
            {
                Console.WriteLine(ex.Message);
                this.RollbackTran();
                return 0;
            }
            finally
            {
                Close();//关闭数据库连接
            }
        }
        #endregion

        #region 插入BLOB数据
        public int BLOBOPERATE(string strSQL, byte[] bFile)
        {

            int iEffecienRows;
            try
            {
                Open();
                this.BeginTran();
                OracleCommand cmd = new OracleCommand(strSQL, con);
                cmd.Parameters.Add("txtctl", OracleDbType.Blob, bFile.Length).Value = bFile ;
               // cmd.Parameters[0].Value = bFile; 
                iEffecienRows = cmd.ExecuteNonQuery();
                this.CommitTran();
                return iEffecienRows;
            }
            catch (Oracle.DataAccess.Client.OracleException ex)
            {
                Console.WriteLine(ex.Message);
                this.RollbackTran();
                return 0;
            }
            finally
            {
                Close();//关闭数据库连接
            }
            return 0;
        }
        #endregion
        #region 执行SQL语句，返回数据到自定义DataSet中
        /// <summary>
        /// 执行SQL语句，返回数据到DataSet中
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="DataSetName">自定义返回的DataSet表名</param>
        /// <returns>返回DataSet</returns>
        public DataSet GetDataSet(string sql, string DataSetName)
        {
            DataSet ds = new DataSet();
            Open();//打开数据连接
            OracleDataAdapter adapter = new OracleDataAdapter(sql, con);
            adapter.Fill(ds, DataSetName);
            Close();//关闭数据库连接
            return ds;
        }
        #endregion
        #region 执行Sql语句,返回带分页功能的自定义dataset
        /// <summary>
        /// 执行Sql语句,返回带分页功能的自定义dataset
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <param name="PageSize">每页显示记录数</param>
        /// <param name="CurrPageIndex">当前页</param>
        /// <param name="DataSetName">返回dataset表名</param>
        /// <returns>返回DataSet</returns>
        public DataSet GetDataSet(string sql, int PageSize, int CurrPageIndex, string DataSetName)
        {
            DataSet ds = new DataSet();
            Open();//打开数据连接
            OracleDataAdapter adapter = new OracleDataAdapter(sql, con);
            adapter.Fill(ds, PageSize * (CurrPageIndex - 1), PageSize, DataSetName);
            Close();//关闭数据库连接
            return ds;
        }
        #endregion
        #region 执行SQL语句，返回记录总数
        /// <summary>
        /// 执行SQL语句，返回记录总数
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>返回记录总条数</returns>
        public int GetRecordCount(string sql)
        {
            int recordCount = 0;
            Open();//打开数据连接
            OracleCommand command = new OracleCommand(sql, con);
            OracleDataReader dataReader = command.ExecuteReader();
            while (dataReader.Read())
            {
                recordCount++;
            }
            dataReader.Close();
            Close();//关闭数据库连接
            return recordCount;
        }
        #endregion

        #region 统计某表记录总数
        /// <summary>
        /// 统计某表记录总数
        /// </summary>
        /// <param name="KeyField">主键/索引键</param>
        /// <param name="TableName">数据库.用户名.表名</param>
        /// <param name="Condition">查询条件</param>
        /// <returns>返回记录总数</returns> 
        public int GetRecordCount(string keyField, string tableName, string condition)
        {
            int RecordCount = 0;
            string sql = "select count(" + keyField + ") as count from " + tableName + " " + condition;
            DataSet ds = GetDataSet(sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                RecordCount = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
            }
            ds.Clear();
            ds.Dispose();
            return RecordCount;
        }
        /// <summary>
        /// 统计某表记录总数
        /// </summary>
        /// <param name="Field">可重复的字段</param>
        /// <param name="tableName">数据库.用户名.表名</param>
        /// <param name="condition">查询条件</param>
        /// <param name="flag">字段是否主键</param>
        /// <returns>返回记录总数</returns> 
        public int GetRecordCount(string Field, string tableName, string condition, bool flag)
        {
            int RecordCount = 0;
            if (flag)
            {
                RecordCount = GetRecordCount(Field, tableName, condition);
            }
            else
            {
                string sql = "select count(distinct(" + Field + ")) as count from " + tableName + " " + condition;
                DataSet ds = GetDataSet(sql);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    RecordCount = Convert.ToInt32(ds.Tables[0].Rows[0][0]);
                }
                ds.Clear();
                ds.Dispose();
            }
            return RecordCount;
        }
        #endregion
        #region 统计某表分页总数
        /// <summary>
        /// 统计某表分页总数
        /// </summary>
        /// <param name="keyField">主键/索引键</param>
        /// <param name="tableName">表名</param>
        /// <param name="condition">查询条件</param>
        /// <param name="pageSize">页宽</param>
        /// <param name="RecordCount">记录总数</param>
        /// <returns>返回分页总数</returns> 
        public int GetPageCount(string keyField, string tableName, string condition, int pageSize, int RecordCount)
        {
            int PageCount = 0;
            PageCount = (RecordCount % pageSize) > 0 ? (RecordCount / pageSize) + 1 : RecordCount / pageSize;
            if (PageCount < 1) PageCount = 1;
            return PageCount;
        }
        /// <summary>
        /// 统计某表分页总数
        /// </summary>
        /// <param name="keyField">主键/索引键</param>
        /// <param name="tableName">表名</param>
        /// <param name="condition">查询条件</param>
        /// <param name="pageSize">页宽</param>
        /// <returns>返回页面总数</returns> 
        public int GetPageCount(string keyField, string tableName, string condition, int pageSize, ref int RecordCount)
        {
            RecordCount = GetRecordCount(keyField, tableName, condition);
            return GetPageCount(keyField, tableName, condition, pageSize, RecordCount);
        }
        /// <summary>
        /// 统计某表分页总数
        /// </summary>
        /// <param name="Field">可重复的字段</param>
        /// <param name="tableName">表名</param>
        /// <param name="condition">查询条件</param>
        /// <param name="pageSize">页宽</param>
        /// <param name="flag">是否主键</param>
        /// <returns>返回页页总数</returns> 
        public int GetPageCount(string Field, string tableName, string condition, ref int RecordCount, int pageSize, bool flag)
        {
            RecordCount = GetRecordCount(Field, tableName, condition, flag);
            return GetPageCount(Field, tableName, condition, pageSize, ref RecordCount);
        }
        #endregion

        #region Sql分页函数
        /// <summary>
        /// 构造分页查询SQL语句
        /// </summary>
        /// <param name="KeyField">主键</param>
        /// <param name="FieldStr">所有需要查询的字段(field1,field2...)</param>
        /// <param name="TableName">库名.拥有者.表名</param>
        /// <param name="where">查询条件1(where ...)</param>
        /// <param name="order">排序条件2(order by ...)</param>
        /// <param name="CurrentPage">当前页号</param>
        /// <param name="PageSize">页宽</param>
        /// <returns>SQL语句</returns> 
        public string JoinPageSQL(string KeyField, string FieldStr, string TableName, string Where, string Order, int CurrentPage, int PageSize)
        {
            string sql = null;
            if (CurrentPage == 1)
            {
                sql = "select  " + CurrentPage * PageSize + " " + FieldStr + " from " + TableName + " " + Where + " " + Order + " ";
            }
            else
            {
                sql = "select * from (";
                sql += "select  " + CurrentPage * PageSize + " " + FieldStr + " from " + TableName + " " + Where + " " + Order + ") a ";
                sql += "where " + KeyField + " not in (";
                sql += "select  " + (CurrentPage - 1) * PageSize + " " + KeyField + " from " + TableName + " " + Where + " " + Order + ")";
            }
            return sql;
        }
        /// <summary>
        /// 构造分页查询SQL语句
        /// </summary>
        /// <param name="Field">字段名(非主键)</param>
        /// <param name="TableName">库名.拥有者.表名</param>
        /// <param name="where">查询条件1(where ...)</param>
        /// <param name="order">排序条件2(order by ...)</param>
        /// <param name="CurrentPage">当前页号</param>
        /// <param name="PageSize">页宽</param>
        /// <returns>SQL语句</returns> 
        public string JoinPageSQL(string Field, string TableName, string Where, string Order, int CurrentPage, int PageSize)
        {
            string sql = null;
            if (CurrentPage == 1)
            {
                sql = "select rownum " + CurrentPage * PageSize + " " + Field + " from " + TableName + " " + Where + " " + Order + " group by " + Field;
            }
            else
            {
                sql = "select * from (";
                sql += "select rownum " + CurrentPage * PageSize + " " + Field + " from " + TableName + " " + Where + " " + Order + " group by " + Field + " ) a ";
                sql += "where " + Field + " not in (";
                sql += "select rownum " + (CurrentPage - 1) * PageSize + " " + Field + " from " + TableName + " " + Where + " " + Order + " group by " + Field + ")";
            }
            return sql;
        }
        #endregion

        #region 开始事务
        private void BeginTran()
        {
            try
            {
                this.myOracleTransaction = this.con.BeginTransaction();
            }
            catch (System.Exception ex)
            { 
            
            }
        }
        #endregion

        #region 提交事务
        private void CommitTran()
        {
            try
            {
                this.myOracleTransaction.Commit();
            }
            catch (System.Exception ex)
            {

            }
        }
        #endregion

        #region 回滚事务
        private  void RollbackTran()
        {
            try
            {
                this.myOracleTransaction.Rollback();
            }
            catch (System.Exception ex)
            {

            }
        }
        #endregion




    }
}