using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Data.OleDb;
using Oracle.DataAccess.Client;
namespace DataCollectMonitor
{
    class CommonCLS
    {
        #region 内部变量
            private static string _ip = "60.206.14.24"; //服务器ip
            private static string _TNS = "test";        //TNS
            private static string _User = "test";       //服务器用户名
            private static string _pwd = "test";        //服务器密码
            private static string _ManageUser = "Gmtools";      //服务器用户名
            private static string _ManagePwd = "SHIRU79";       //服务器密码

            public static string LoginUser=string.Empty;
            public static string LoginUserID = string.Empty;
            public static string BcpPath = string.Empty;    //BCP保存路径
            public static string CtlPath = string.Empty;    //CTL保存路径
        //public static SqlOracle sqlOrl = null;

        #region 引入 Win32 API 函数
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        #endregion
        #endregion

        #region 执行Oracle
        public static DataSet RunOracle(string sql)
        {
            try
            {
                string user = CommonCLS.UserName;
                string pwd = CommonCLS.PASSWORD;
                DataSet result = new DataSet();
                SqlOracle sqlOrl = null;
                sqlOrl = new SqlOracle("Data Source=" + CommonCLS.TNS +"; User Id=" + user + ";Password=" + pwd + ";");
                result = sqlOrl.GetDataSet(sql);
                return result;
            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog(ex.ToString());
                return null;
            }
        }
        #endregion        

        #region 执行RunOracleNonQuery
        public static int RunOracleNonQuery(string sql)
        {
            try
            {

                string user = CommonCLS.UserName;
                string pwd = CommonCLS.PASSWORD;
                int result;
                SqlOracle sqlOrl = null;
                sqlOrl = new SqlOracle("Data Source=" + CommonCLS.TNS + "; User Id=" + user + ";Password=" + pwd + ";");
                result = sqlOrl.ExecuteNonquery(sql);
                return result;
            }
            catch (System.Exception ex)
            {
                return 0;
            }
        }
        #endregion        

        #region 执行RunOracleNonQuery
        public static int RunOracleNonQuery(string sql,byte[] bFile)
        {
            try
            {

                string user = CommonCLS.UserName;
                string pwd = CommonCLS.PASSWORD;
                int result;
                SqlOracle sqlOrl = null;
                sqlOrl = new SqlOracle("Data Source=" + CommonCLS.TNS + "; User Id=" + user + ";Password=" + pwd + ";");
                result = sqlOrl.BLOBOPERATE(sql,bFile);
                return result;
            }
            catch (System.Exception ex)
            {
                return 0;
            }
        }
        #endregion

        #region 执行RunOracleNonQuerySp
        public static int RunOracleNonQuerySp(string storedProcName, OracleParameter[] parameters)
        {
            try
            {
                string user = CommonCLS.UserName;
                string pwd = CommonCLS.PASSWORD;
                int result;
                SqlOracle sqlOrl = null;
                sqlOrl = new SqlOracle("Data Source=" + CommonCLS.TNS + "; User Id=" + user + ";Password=" + pwd + ";");
                result = sqlOrl.ExecuteNonqueryBySP(storedProcName, parameters);
                return result;
            }
            catch (System.Exception ex)
            {
                return 0;
            }
        }
        #endregion        

        #region 使用管理帐号执行Oracle
        public static DataSet RunOracleByManager(string sql)
        {
            try
            {
                string user = CommonCLS.MangerUserName ;
                string pwd = CommonCLS.MangerPASSWORD;
                DataSet result = new DataSet();
                SqlOracle sqlOrl = null;
                sqlOrl = new SqlOracle("Data Source=" + CommonCLS.TNS + "; User Id=" + user + ";Password=" + pwd + ";");
                result = sqlOrl.GetDataSet(sql);
                return result;
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }
        #endregion  

        #region 执行CreateTBL
        /// <summary>
        /// 自动建立目的表
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="tablename"></param>
        /// <param name="iFLG"></param>
        /// <param name="dtTable"></param>
        /// <param name="strTableSpace"></param>
        /// <returns></returns>
        public static int CreateTBL(string sql, string tablename, int iFLG,DataTable dtTable,string strTableSpace,string account,string pwd,string tns)
        {
            try
            {

                //string user = CommonCLS.UserName;
               // string pwd = CommonCLS.PASSWORD;
                int result;
                SqlOracle sqlOrl = null;
                sqlOrl = new SqlOracle("Data Source=" + tns + "; User Id=" + account + ";Password=" + pwd + ";");
               result = sqlOrl.CreateTable(sql, tablename, iFLG, dtTable, strTableSpace);
                return result;
            }
            catch (System.Exception ex)
            {
                return 0;
            }
        }
     
        /// <summary>
        /// 手动建立目的表
        /// </summary>
        /// <param name="strSql"></param>
        /// <returns></returns>
        public static int CreateTBL(string strSql, string account, string pwd, string tns)
        {
            try
            {
                //string user = CommonCLS.UserName;
                //string pwd = CommonCLS.PASSWORD;
                int result;
                SqlOracle sqlOrl = null;
                sqlOrl = new SqlOracle("Data Source=" + tns + "; User Id=" + account + ";Password=" + pwd + ";");
                result = sqlOrl.ExecuteNonquery(strSql);
                return result;
            }
            catch (System.Exception ex)
            {
                return 0;
            }
        }

        #endregion     
       
        #region 执行存储过程 
        public static DataSet RunOracleSP(string storedProcName, OracleParameter[] parameters)
        {
            try
            {
                string user = CommonCLS.UserName;
                string pwd = CommonCLS.PASSWORD;
                DataSet result = new DataSet();
                SqlOracle sqlOrl = null;
                sqlOrl = new SqlOracle("Data Source=" + CommonCLS.TNS +"; User Id=" + user + ";Password=" + pwd + ";");
                result = sqlOrl.GetDataSetBySP(storedProcName,parameters);
                return result;
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }

        public static DataSet RunOracleSP(string storedProcName)
        {
            try
            {
                string user = CommonCLS.UserName;
                string pwd = CommonCLS.PASSWORD;
                DataSet result = new DataSet();
                SqlOracle sqlOrl = null;
                sqlOrl = new SqlOracle("Data Source=" + CommonCLS.TNS + "; User Id=" + user + ";Password=" + pwd + ";");
                result = sqlOrl.GetDataSetBySP(storedProcName);
                return result;
            }
            catch (System.Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region 取得当前服务器时间
        public static DateTime GetSysDate()
        {
            try
            {
                string user = CommonCLS.UserName;
                string pwd = CommonCLS.PASSWORD;
                DataSet result = new DataSet();
                SqlOracle sqlOrl = null;
                sqlOrl = new SqlOracle("Data Source=" + CommonCLS.TNS + "; User Id=" + user + ";Password=" + pwd + ";");
                result = sqlOrl.GetDataSet("select to_char(sysdate,'YYYY/MM/DD HH:mi:SS') from dual");
                if (result != null && result.Tables[0].Rows.Count>0)
                {
                    return Convert.ToDateTime(result.Tables[0].Rows[0].ItemArray[0].ToString());
                }
                else
                {
                    return Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/ss HH:MI:SS").Replace("-","/"));
                }
            }
            catch (System.Exception ex)
            {
                return Convert.ToDateTime(DateTime.Now.ToString("yyyy/MM/ss HH:MI:SS").Replace("-", "/"));
            }
        }
        #endregion

        #region 执行删除
        public static int DelData(string id)
        {
            try
            {

                string user = CommonCLS.UserName;
                string pwd = CommonCLS.PASSWORD;
                int result;
                SqlOracle sqlOrl = null;
                sqlOrl = new SqlOracle("Data Source=" + CommonCLS.TNS + "; User Id=" + user + ";Password=" + pwd + ";");
                result = sqlOrl.DelData(id);
                return result;
            }
            catch (System.Exception ex)
            {
                return 0;
            }
        }
        #endregion     

        #region 读取INI指定键变量
        // <summary>
        /// ReadValue 读取指定键变量
        /// </summary>
        /// <param name="strSection">片断名称</param>
        /// <param name="strKey">键名称</param>
        /// <returns>键变量</returns>
        public static  string ReadINIValue(string strPath, string strSection, string strKey)
        {
            try
            {
                StringBuilder strValue = new StringBuilder(255);
                GetPrivateProfileString(strSection, strKey, "", strValue, 255, strPath);

                return strValue.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return null;
            }
        }
        #endregion

        #region 错误日志
        public static void  SaveLog(string LogStr)
        {
            StreamWriter sw = null;
            string strPath = System.IO.Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath);
            
            try
            {
                LogStr = DateTime.Now.ToLocalTime().ToString()  +"\n :" + LogStr;
                sw = new StreamWriter(strPath + "\\errlog\\Log"+DateTime.Now.ToShortDateString() +".txt", true);
                sw.WriteLine(LogStr);
            }
            catch
            {
            }
            finally
            {
                if (sw != null)
                {
                    sw.Close();
                }
            }
        }

        #endregion

        #region 属性
        /// <summary>
        /// 服务器IP
        /// </summary>
        public static string SERVER_IP
        {
            set
            {
                _ip = value;
            }
            get
            {
                return _ip;
            }
        }

        /// <summary>
        /// TNS
        /// </summary>
        public static string TNS
        {
            set
            {
                _TNS = value;
            }
            get
            {
                return _TNS;
            }
        }


        /// <summary>
        /// 用户名
        /// </summary>
        public static string UserName
        {
            set
            {
                _User = value;
            }
            get
            {
                return _User;
            }
        }



        /// <summary>
        /// 密码
        /// </summary>
        public static string PASSWORD
        {
            set
            {
                _pwd = value;
            }
            get
            {
                return _pwd;
            }
        }

        /// <summary>
        /// 管理用户名

        /// </summary>
        public static string MangerUserName
        {
            set
            {
                _ManageUser = value;
            }
            get
            {
                return _ManageUser;
            }
        }

        /// <summary>
        /// 管理密码
        /// </summary>
        public static string MangerPASSWORD
        {
            set
            {
                _ManagePwd = value;
            }
            get
            {
                return _ManagePwd;
            }
        }
        #endregion

        #region 状态栏信息
        /// <summary>
        /// 在主界面中写入状态信息

        /// </summary>
        /// <param name="form"></param>
        /// <param name="statusString"></param>
        public static void WriteStatusText(Form form, string statusString)
        {
            string DavidPanelName = "dpStatus";
            string LabelName = "lblStatus";

            for (int i = 0; i < form.Controls.Count; i++)
            {
                if (form.Controls[i].Name.Equals(DavidPanelName))
                {
                    for (int j = 0; j < form.Controls[i].Controls.Count; j++)
                    {
                        if (form.Controls[i].Controls[j].Name.Equals(LabelName))
                        {
                            form.Controls[i].Controls[j].Text = statusString;
                        }
                    }
                }
            }
        }
        #endregion

        #region 取得游戏ID
        public static int GetGameID(string strName)
        {
            string strSQL;
            try
            {
                strSQL = "SELECT game_id From Game_Info WHERE game_name ='" + strName.Trim() + "'";
                DataSet dGameNm = CommonCLS.RunOracle(strSQL);
                if (dGameNm !=null && dGameNm.Tables[0].Rows.Count>0)
                {
                    return Convert.ToInt32(dGameNm.Tables[0].Rows[0].ItemArray[0]);
                }
                else
                {
                    return 0;
                }
            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog("GetGameID->" + ex.Message);
                  return 0;
            }
        }
        #endregion

        #region 取得游戏db 类型
        public static int GetGameDbType(string strName)
        {
            string strSQL;
            try
            {
                strSQL = "SELECT TYPE_DB From Game_Info WHERE game_name ='" + strName.Trim() + "'";
                DataSet dGameNm = CommonCLS.RunOracle(strSQL);
                if (dGameNm != null && dGameNm.Tables[0].Rows.Count > 0)
                {
                    return Convert.ToInt32(dGameNm.Tables[0].Rows[0].ItemArray[0]);
                }
                else
                {
                    return 0;
                }
            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog("GetGameDbType->" + ex.Message);
                return 0;
            }
        }
        public static int GetGameDbTypeById(int iID)
        {
            string strSQL;
            try
            {
                strSQL = "SELECT TYPE_DB From Game_Info WHERE game_id =" + iID ;
                DataSet dGameNm = CommonCLS.RunOracle(strSQL);
                if (dGameNm != null && dGameNm.Tables[0].Rows.Count > 0)
                {
                    return Convert.ToInt32(dGameNm.Tables[0].Rows[0].ItemArray[0]);
                }
                else
                {
                    return 0;
                }
            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog("GetGameDbType->" + ex.Message);
                return 0;
            }
        }
        #endregion

        #region 取得游戏表空间
        public static string GetGameTbSpace(string strName)
        {
            string strSQL;
            try
            {
                strSQL = "SELECT TABLE_SPACE_NAME From Game_Info WHERE game_name ='" + strName.Trim() + "'";
                DataSet dGameNm = CommonCLS.RunOracle(strSQL);
                if (dGameNm != null && dGameNm.Tables[0].Rows.Count > 0)
                {
                    return dGameNm.Tables[0].Rows[0].ItemArray[0].ToString();
                }
                else
                {
                    return "";
                }
            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog("GetGameTbSpace->" + ex.Message);
                return "";
            }
        }
        public static string GetGameTbSpaceById(int iID)
        {
            string strSQL;
            try
            {
                strSQL = "SELECT TABLE_SPACE_NAME From Game_Info WHERE game_id =" + iID ;
                DataSet dGameNm = CommonCLS.RunOracle(strSQL);
                if (dGameNm != null && dGameNm.Tables[0].Rows.Count > 0)
                {
                    return dGameNm.Tables[0].Rows[0].ItemArray[0].ToString();
                }
                else
                {
                    return "";
                }
            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog("GetGameTbSpace->" + ex.Message);
                return "";
            }
        }
        #endregion

        #region 取得游戏DB ID
        public static int GetGameDBID(string sIP,string sServerNM,string sServerDB,string sSrcTbl,string sDesTbl,string sUser,string sPwd,int iGameID)
        {
            string strSQL;
            try
            {
                strSQL = "SELECT id From gamedb_info WHERE server_ip  ='" + sIP + "'";
                strSQL = strSQL + " AND server_name='" + sServerNM + "'";
                strSQL = strSQL + " AND server_db='" + sServerDB + "'";
                strSQL = strSQL + " AND srctable='" + sSrcTbl + "'";
                strSQL = strSQL + " AND destable='" + sDesTbl + "'";
                strSQL = strSQL + " AND user_id='" + sUser + "'";
                strSQL = strSQL + " AND user_pwd='" + sPwd + "'";
                strSQL = strSQL + " AND game_id=" + iGameID + "";
                DataSet dGameNm = CommonCLS.RunOracle(strSQL);
                if (dGameNm != null && dGameNm.Tables[0].Rows.Count > 0)
                {
                    return Convert.ToInt32(dGameNm.Tables[0].Rows[0].ItemArray[0]);
                }
                else
                {
                    return 0;
                }
            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog("SaveData->" + ex.Message);
                return 0;
            }
        }
        #endregion

        #region 发送文件 字节
        public static  void sendByteFile(string sPath,string sIP,int sPort)
        {
            Socket sendsocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //实例化socket
            IPEndPoint ipendpiont = new IPEndPoint(IPAddress.Parse(sIP), sPort);//建立终结点
            FileStream fs = new FileStream(sPath, FileMode.OpenOrCreate, FileAccess.Read);//要传输的文件
            byte[] fssize = new byte[fs.Length - 1];
            BinaryReader strread = new BinaryReader(fs);//流处理要传输的文件
            strread.Read(fssize, 0, fssize.Length - 1);
            sendsocket.Connect(ipendpiont);//连接远程计算机
            sendsocket.Send(fssize);//发送文件
            fs.Close();
            sendsocket.Shutdown(SocketShutdown.Send);
            //关闭发送连接
            sendsocket.Close();//关闭本机socket
        }
        #endregion

        #region
        /// <summary>
        /// 读取excel信息
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Sheet"></param>
        /// <returns></returns>
        public static  DataSet ExcelToDS(string Path, string Sheet)
        {
            DataSet ds = null;
            try
            {
                string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + Path + ";" + "Extended Properties=Excel 8.0;";
                OleDbConnection conn = new OleDbConnection(strConn);
                conn.Open();
                //DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" });//获取excel文件的表名
                string strExcel = "";
                OleDbDataAdapter myCommand = null;

                //if (Sheet == "")
                //{
                //    strExcel = "select * from [" + dt.Rows[0][2].ToString().Trim() + "]";
                //}
                //else
                //{
                strExcel = "select * from [" + Sheet + "$]";
                //}
               
                myCommand = new OleDbDataAdapter(strExcel, strConn);
               
                ds = new DataSet();
                myCommand.Fill(ds, "table1");

                return ds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return ds;
        }

        /// <summary>
        /// 读取excel sheet信息
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="Sheet"></param>
        /// <returns></returns>
        public static string[] ExcelSheetNameToDS(string Path)
        {
            string[] sSheetName=  new string[10];
            try
            {
                string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + Path + ";" + "Extended Properties=Excel 8.0;";
                OleDbConnection conn = new OleDbConnection(strConn);
                conn.Open();
                //DataTable dt = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" });//获取excel文件的表名

                string strExcel = "";
                OleDbDataAdapter myCommand = null;

                DataTable dtSheetName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "Table" });

                sSheetName = new string[dtSheetName.Select("TABLE_NAME <> 'SERVER$'").Length];

                for (int i = 0; i < dtSheetName.Rows.Count; i++)
                {
                    if (dtSheetName.Rows[i].ItemArray[2].ToString().Replace("$", "").ToUpper() != "SERVER")
                    {
                        sSheetName[i] = dtSheetName.Rows[i].ItemArray[2].ToString().Replace("$", "");
                    }
                }



                return sSheetName;
            }
            catch (Exception ex)
            {
              //  throw new Exception(ex.dtSheetNameMessage);
            }

            return sSheetName;
        }
        #endregion

        #region 表分区

        public static string GetPartition(string sFields, DataTable dtIP, string strTableSpace)
        {
            try
            {
                if (strTableSpace == string.Empty)
                {
                    return string.Empty;
                }
                string sIpName = string.Empty;      //IP字段名  
                string sDateName = string.Empty;    //日期字段名
                string sTableSpace = string.Empty;  //表空间语句
                //string sReslut=string.Empty;        //返回字符串
                StringBuilder sReslut=new StringBuilder();        //返回字符串
                sTableSpace = "Tablespace " + strTableSpace  + Environment.NewLine;
                sTableSpace = sTableSpace + " pctfree 10 " + Environment.NewLine;
                sTableSpace = sTableSpace + " initrans 1 " + Environment.NewLine;
                sTableSpace = sTableSpace + " maxtrans 255 " + Environment.NewLine;
                sTableSpace = sTableSpace + " storage " + Environment.NewLine;
                sTableSpace = sTableSpace + " (initial 64 " + Environment.NewLine;
                sTableSpace = sTableSpace + "   minextents 1 " + Environment.NewLine;
                sTableSpace = sTableSpace + "   maxextents unlimited) " + Environment.NewLine;
                //判断表结构中是否存在DATE类型和ip字段
                string[] sFld = sFields.Split(',');
                for (int i = 0; i < sFld.Length; i++)
                { 
                    string[] sFldRow=sFld[i].Split(' ');
                    //判断ip字段
                    //if (sFldRow[0].ToUpper().Contains("IP") == true && sIpName==string.Empty )
                    //{
                    //    sIpName = sFldRow[0];
                    //}
                    if (sFldRow[2] == "1" && sFldRow[1].ToUpper()  != "DATE" && sIpName==string.Empty)
                    {
                        sIpName = sFldRow[0];
                    }


                    //判断日期字段
                    if (sFldRow[2] == "1" && sFldRow[1].ToUpper() == "DATE" && sDateName == string.Empty)
                    {
                        sDateName = sFldRow[0];
                    }
                }
                if (sDateName == string.Empty && sIpName == string.Empty)
                {
                    return sTableSpace;
                }

                if (sDateName == string.Empty && sIpName != string.Empty)
                { 
                    //只有ip
                    sReslut.Append("PARTITION BY LIST (" + sIpName + ") " + Environment.NewLine);
                    string strIPTmp1 = string.Empty + Environment.NewLine;
                    strIPTmp1 = dtIP.Rows[0].ItemArray[1].ToString();
                    sReslut.Append( " ( " + Environment.NewLine);
                    for (int k1 = 0; k1 < dtIP.Rows.Count; k1++)
                    {
                        if (k1 != 0 && strIPTmp1 == dtIP.Rows[k1].ItemArray[1].ToString())
                        {
                            strIPTmp1 = dtIP.Rows[k1].ItemArray[1].ToString();
                            continue;
                        }
                        if (k1 != 0)
                        {
                            sReslut.Append( ",");
                        }
                        sReslut.Append( "PARTITION P" + "IP" + (k1 + 1) + " VALUES ('" + dtIP.Rows[k1].ItemArray[1].ToString() + "') TABLESPACE " + strTableSpace);

                        sReslut.Append( Environment.NewLine);
                    }
                    sReslut.Append( " ) " + Environment.NewLine);
                }
                else if (sDateName != string.Empty && sIpName != string.Empty)
                {
                    //有日期有ip
                    sReslut.Append("PARTITION BY RANGE(\"" + sDateName.ToUpper() + "\") SUBPARTITION BY LIST (" + sIpName + ") " + Environment.NewLine);
                    sReslut.Append( " ( " + Environment.NewLine);
                    Application.DoEvents();
                    for (int j = 0; j < 400; j++)
                    {
                        sReslut.Append( "PARTITION P" + (j + 1) + " VALUES LESS THAN(TO_DATE('" + System.DateTime.Now.AddDays(j * 7).ToShortDateString() + "','YYYY-MM-DD'))TABLESPACE " + strTableSpace + Environment.NewLine);
                        sReslut.Append( "     ( " + Environment.NewLine);

                        string strIPTmp = string.Empty;

                        strIPTmp = dtIP.Rows[0].ItemArray[1].ToString();
                        for (int k = 0; k < dtIP.Rows.Count; k++)
                        {
                            if (k != 0 && strIPTmp == dtIP.Rows[k].ItemArray[1].ToString())
                            {
                                strIPTmp = dtIP.Rows[k].ItemArray[1].ToString();
                                continue;
                            }
                            if (k != 0)
                            {
                                sReslut.Append( ",");
                            }
                            sReslut.Append( "       SUBPARTITION P" + (j + 1) + "IP" + (k + 1) + " VALUES ('" + dtIP.Rows[k].ItemArray[1].ToString() + "') TABLESPACE " + strTableSpace);

                            sReslut.Append( Environment.NewLine);
                        }
                        sReslut.Append( "     ) ");
                        if (j != 399)
                        {
                            sReslut.Append( ",");
                        }
                        sReslut.Append( Environment.NewLine);
                    }
                    sReslut.Append( " ) " + Environment.NewLine);
                }
                else if (sDateName != string.Empty && sIpName == string.Empty)
                {
                    //只有日期
                    sReslut.Append("PARTITION BY RANGE(\"" + sDateName.ToUpper() + "\") " + Environment.NewLine);
                    sReslut.Append( " ( " + Environment.NewLine);
                    for (int j = 0; j < 523; j++)
                    {
                        sReslut.Append( "PARTITION P" + (j + 1) + " VALUES LESS THAN(TO_DATE('" + System.DateTime.Now.AddDays(j * 7).ToShortDateString() + "','YYYY-MM-DD'))TABLESPACE " + strTableSpace + Environment.NewLine);
                        if (j != 522)
                        {
                            sReslut.Append( ",");
                        }
                        sReslut.Append( Environment.NewLine);
                    }
                    sReslut.Append( " ) " + Environment.NewLine);


                }
                return sReslut.ToString();
            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog("GetPartition->" + ex.ToString());
                return string.Empty;
            }
        }
        #endregion

        #region 操作log
        public static void WriteOperatelog(string strLogResult)
        {
            string strsql = string.Empty;
            try
            {
                //插入操作记录
                strsql = @"INSERT INTO Operate_Log VALUES(OPERATELOG_ID_SEQ.NEXTVAL,";                  //ID
                strsql=strsql +"sysdate, "; //操作时间
                strsql = strsql + "'" + strLogResult +"',";                                             //操作内容
                strsql = strsql + "'" +CommonCLS.LoginUser +"')";                                       //操作者
                CommonCLS.RunOracleNonQuery(strsql);

                OracleParameter[] parameters = 
                    {
                        new OracleParameter("v_LogResult", OracleDbType.Varchar2),
                        new OracleParameter("v_LoginUser", OracleDbType.Varchar2),
                        new OracleParameter("v_result", OracleDbType.Decimal)
                    };
                parameters[0].Direction = ParameterDirection.Input;
                parameters[1].Direction = ParameterDirection.Input;
                parameters[2].Direction = ParameterDirection.Output;

                parameters[0].Value = strLogResult;
                parameters[1].Value = CommonCLS.LoginUser;
                CommonCLS.RunOracleNonQuerySp("PD_Game_Admin.PD_OperateLog_Insert", parameters);

            }
            catch (System.Exception ex)
            {
            }
        }
        #endregion
    }
  
}
