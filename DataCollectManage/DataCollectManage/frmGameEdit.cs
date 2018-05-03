using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using Oracle.DataAccess.Client;
namespace DataCollectMonitor
{
    public partial class frmGameEdit : Form
    {
        #region 调用函数
        /// <summary>
        /// 创建类库中的窗体
        /// </summary>
        /// <param name="oParent">MDI 程序的父窗体</param>
        /// <param name="oSocket">Socket</param>
        /// <returns>类库中的窗体</returns>
        public Form CreateModule(object oParent, object oEvent, object oReturnValue)
        {
        

            if (oParent != null)
            {
                _parent = (Form)oParent;
                this.MdiParent = (Form)oParent;
                this.Show();
            }
            else
            {
                if (oReturnValue != null)
                {
                    this._returnValue = (DataCollectMonitor.Betweenness)oReturnValue;
                }
                this.FormBorderStyle = FormBorderStyle.FixedDialog;
                this.StartPosition = FormStartPosition.CenterParent;
                this.MaximizeBox = false;
                this.MinimizeBox = false;
                this.ShowInTaskbar = false;
                this.ShowDialog();
            }

            return this;
        }

        private Form _parent = null;
        private DataCollectMonitor.Betweenness _returnValue = null;
        #endregion

        #region 构造函数
        public frmGameEdit()
        {
            InitializeComponent();
        }

        public frmGameEdit(int MODE,string strGameName,string strID)
        {
            InitializeComponent();
            this._mode = MODE;
            this._game_name = strGameName;
            this._Task_id = strID;
        }
        #endregion

        #region 变量
        private int _mode;  //0:新建 //1:修改  
        private string _game_name=string.Empty;//游戏名 ，新建时为空，修改模式时传入
        private string _gameID = string.Empty;
        private string _Task_id = string.Empty;//游戏ID ，新建时为空，修改模式时传入
        private DataTable dtServer= new DataTable();        //服务器列表
        private DataTable dtTable= new DataTable();         //字段列表
        private DataTable dtArea = new DataTable();         //大区列表
        private DataSet dsGame = new DataSet();             //游戏列表
        private string strCount = string.Empty;
        string strTbSpace = string.Empty;
        #endregion

        #region 方法

        #region 保存数据
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns></returns>
        private bool SaveData()
        {
            string strSQL=string.Empty;     //SQL语句
            int game_id;  //游戏ID
            string gamedb_ID;//游戏DB_ID
            string strDBrow = string.Empty;
            string strbcp_selectTMP=String.Empty;
            int result2 = 0;
            int iDbType = 0;
            int iSwitch = 0;        //采集开关
            string strDtbl_Account = string.Empty;
            string strDtbl_Pwd = string.Empty;
            string strDtbl_Tns = string.Empty;
            try
            {
                if (this.cbxSwitch.SelectedIndex == 0)
                {
                    iSwitch = 1;
                }
                else
                {
                    iSwitch = 0;
                }
               
                if (this._mode == 0)    //新建模式
                {
                    game_id = GetGameid(this.cbxGameName.Text.Trim(), out strDtbl_Account, out strDtbl_Pwd, out strDtbl_Tns);
                    iDbType = CommonCLS.GetGameDbType(this.cbxGameName.Text.Trim());
                    strTbSpace = CommonCLS.GetGameTbSpace(this.cbxGameName.Text.Trim());

                    strSQL = "";
                                 
                    //插入import_express
                    //gamedb_ID
                    gamedb_ID = this.GetAreaID(); ;
                    string strBcpName = string.Empty;
                    //bcp_name
                    strBcpName = this.GetBcp_CTLNM("txt");
                    //bcp_sql
                    strbcp_selectTMP = this.GetBcpSql();
                    //ctl_name
                    string strCTLNAME = this.GetBcp_CTLNM("ctl");
                    //DBROW
                    strDBrow = this.GetDBRow();
                    //ctl_byte 生成CTL字节流
                    byte[] str = System.Text.Encoding.Default.GetBytes( this.BuildCTL(strBcpName,"APPEND"));

                    OracleParameter[] parameters1 = 
                    {
                        new OracleParameter("v_GameDBid", OracleDbType.Varchar2, ParameterDirection.Input),
                        new OracleParameter("v_bcpName", OracleDbType.Varchar2, ParameterDirection.Input),
                        new OracleParameter("v_bcpsql", OracleDbType.Varchar2, ParameterDirection.Input),
                        new OracleParameter("v_CTLNAME", OracleDbType.Varchar2, ParameterDirection.Input),
                        new OracleParameter("v_DBrow", OracleDbType.Varchar2, ParameterDirection.Input),
                        new OracleParameter("v_SRCTABLE", OracleDbType.Varchar2, ParameterDirection.Input),
                        new OracleParameter("v_DESTABLE", OracleDbType.Varchar2, ParameterDirection.Input),
                        new OracleParameter("v_SERVER_DB", OracleDbType.Varchar2, ParameterDirection.Input),
                        new OracleParameter("v_GAME_ID", OracleDbType.Varchar2, ParameterDirection.Input),
                        new OracleParameter("v_switch", OracleDbType.Int32, ParameterDirection.Input),
                        new OracleParameter("v_BEGINTIME ", OracleDbType.Date, ParameterDirection.Input),
                        new OracleParameter("v_INTERVAL_COLLECT", OracleDbType.Varchar2, ParameterDirection.Input),
                        new OracleParameter("v_CURRENTTIME", OracleDbType.Date, ParameterDirection.Input),
                        new OracleParameter("v_COUNT", OracleDbType.Decimal, ParameterDirection.Input),

                        new OracleParameter("v_result", OracleDbType.Decimal, ParameterDirection.Output)
                    };

                    parameters1[0].Value = gamedb_ID;                   //gamedb_ID  
                    parameters1[1].Value = strBcpName;                  //bcp_name
                    parameters1[2].Value = strbcp_selectTMP.Replace("%S","%s");  //bcp_sql
                    parameters1[3].Value = strCTLNAME;                  //ctl_name
                    parameters1[4].Value = strDBrow;                    //DBROW
                    parameters1[5].Value = this.txtTable.Text;          //源数据表
                    parameters1[6].Value = this.txtDestable .Text;      //目的数据表
                    parameters1[7].Value = this.txtDB.Text;             //源库名
                    parameters1[8].Value = game_id;                     //游戏id
                    parameters1[9].Value = iSwitch;                     //采集开关

                    parameters1[10].Value = this.dpStartTime.Value;                      //采集开始时间
                    parameters1[11].Value = getInterVal();                                                        //采集间隔时间
                    parameters1[12].Value = this.dpStartTime.Value;                      //当前时间
                    parameters1[13].Value = 0;                     //count

                    result2 = CommonCLS.RunOracleNonQuerySp("PD_Game_Admin.PD_IMPORTEXPRESS_Insert", parameters1);
                    if (result2 > 0)
                    {
                        //记录操作log
                        CommonCLS.WriteOperatelog("插入import_express表,内容: "
                                                + " BCP名字:" + strBcpName
                                                + " BCP SELECT 语句:" + strbcp_selectTMP
                                                + " CTl名字:" + strCTLNAME);
                    }
                    DataTable dtIP = GetCheckedIpLst();

                    //建立目的表
                    if (this.rdb2.Checked == true && this.txtTblSql.Text!=string.Empty)
                    {
                        //手动建立目的表
                        CommonCLS.CreateTBL(this.txtTblSql.Text,strDtbl_Account ,strDtbl_Pwd ,strDtbl_Tns);
                    }
                    else
                    {
                        //自动建立目的表

                        CommonCLS.CreateTBL(strDBrow, this.txtDestable.Text.Trim(), 1, dtIP, strTbSpace, strDtbl_Account, strDtbl_Pwd, strDtbl_Tns);
                    }
                }
                else if (this._mode == 1)  //修改模式
                {
                    game_id = GetGameid(this.cbxGameName.Text.Trim(), out strDtbl_Account, out strDtbl_Pwd, out strDtbl_Tns);
                    iDbType = CommonCLS.GetGameDbTypeById(game_id);
                    strTbSpace = CommonCLS.GetGameTbSpaceById(game_id);

                    //插入import_express
                    //gamedb_ID
                    gamedb_ID = this.GetAreaID(); ;
                    string strBcpName = string.Empty;
                    //bcp_name
                    strBcpName = this.GetBcp_CTLNM("txt");
                    //bcp_sql
                    strbcp_selectTMP = this.txtBcp.Text.Trim();
                    //ctl_name
                    string strCTLNAME = this.GetBcp_CTLNM("ctl");
                    //DBROW
                    strDBrow = this.GetDBRow();
                    //ctl_byte 生成CTL字节流
                    byte[] str = System.Text.Encoding.Default.GetBytes(this.BuildCTL(strBcpName, "APPEND"));


                    OracleParameter[] parameters3 = 
                    {
                        new OracleParameter("v_ID", OracleDbType.Int32, ParameterDirection.Input),
                        new OracleParameter("v_bcpName", OracleDbType.Varchar2, ParameterDirection.Input),
                        new OracleParameter("v_bcpsql", OracleDbType.Varchar2, ParameterDirection.Input),
                        new OracleParameter("v_ctlName", OracleDbType.Varchar2, ParameterDirection.Input),
                        new OracleParameter("v_dbrow", OracleDbType.Varchar2, ParameterDirection.Input),
                        new OracleParameter("v_Gamedb_id", OracleDbType.Varchar2, ParameterDirection.Input),
                        new OracleParameter("v_srctable", OracleDbType.Varchar2, ParameterDirection.Input),
                        new OracleParameter("v_destable", OracleDbType.Varchar2, ParameterDirection.Input),
                        new OracleParameter("v_server_db", OracleDbType.Varchar2, ParameterDirection.Input),
                        new OracleParameter("v_switch", OracleDbType.Int32, ParameterDirection.Input),
                        new OracleParameter("v_BEGINTIME ", OracleDbType.Date, ParameterDirection.Input),
                        new OracleParameter("v_INTERVAL_COLLECT", OracleDbType.Varchar2, ParameterDirection.Input),
                        new OracleParameter("v_CURRENTTIME", OracleDbType.Date, ParameterDirection.Input),
                        new OracleParameter("v_COUNT", OracleDbType.Decimal, ParameterDirection.Input),
                        new OracleParameter("v_result", OracleDbType.Decimal, ParameterDirection.Output)
                    };

                    parameters3[0].Value = _Task_id;                //gamedb_ID  
                    parameters3[1].Value = strBcpName;              //bcp_name
                    parameters3[2].Value = this.txtBcp.Text.Replace("%S", "%s");     //bcp_sql
                    parameters3[3].Value = strCTLNAME;              //ctl_name
                    parameters3[4].Value = strDBrow;               //DBROW
                    parameters3[5].Value = gamedb_ID;                //Gamedb_id
                    parameters3[6].Value = this.txtTable.Text;          //源数据表
                    parameters3[7].Value = this.txtDestable.Text;      //目的数据表
                    parameters3[8].Value = this.txtDB.Text;             //源库名
                    parameters3[9].Value = iSwitch;                     //采集开关
                    parameters3[10].Value = this.dpStartTime.Value;                      //采集开始时间
                    parameters3[11].Value = getInterVal();                                                        //采集间隔时间
                    parameters3[12].Value = this.dpStartTime.Value;                      //当前时间
                    parameters3[13].Value = 0;                     //count
                    result2 = CommonCLS.RunOracleNonQuerySp("PD_Game_Admin.PD_ImportExpress_Update", parameters3);
                    if (result2 > 0)
                    {
                        //记录操作log
                        CommonCLS.WriteOperatelog("更新import_express表,内容: "
                                                + " BCP名字:" + strBcpName
                                                + " BCP SELECT 语句:" + strbcp_selectTMP
                                                + " CTl名字:" + strCTLNAME);
                    }
                    DataTable dtIP = GetCheckedIpLst();


                    //建立目的表
                    if (this.rdb2.Checked == true && this.txtTblSql.Text != string.Empty)
                    {
                        //手动建立目的表

                        CommonCLS.CreateTBL(this.txtTblSql.Text, strDtbl_Account, strDtbl_Pwd, strDtbl_Tns);
                    }
                    else
                    {
                        //自动建立目的表

                        CommonCLS.CreateTBL(strDBrow, this.txtDestable.Text.Trim(), 1, dtIP, strTbSpace, strDtbl_Account, strDtbl_Pwd, strDtbl_Tns);
                    }
                }
            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog("SaveData->" + ex.Message);
                return false;
            }

            if (result2 > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 读出数据
        /// <summary>
        /// 读出数据
        /// </summary>
        private void LoadData()
        {
            string strSQL=string.Empty;
            try
            {

                this.cbxGameName.Enabled = false;

                OracleParameter[] parameters = 
                    {
                        new OracleParameter("v_ID", OracleDbType.Int32, ParameterDirection.Input),
                        new OracleParameter("cur_Result", OracleDbType.RefCursor, ParameterDirection.Output)
                    };
                parameters[0].Value = Convert.ToInt32(this._Task_id);
                DataSet dsTable = CommonCLS.RunOracleSP("PD_GameInfo_Pack.PD_ImportExpress_Query", parameters);

                if (dsTable != null && dsTable.Tables[0].Rows.Count > 0)
                {
                    //BCP语句
                    this.txtBcp.Text = dsTable.Tables[0].Rows[0].ItemArray[2].ToString();

                    //字段结构DataGrid
                    string[] strFieldRow = dsTable.Tables[0].Rows[0].ItemArray[4].ToString().Split(',');
                    if (strFieldRow.Length > 0)
                    {
                        for (int j = 0; j < strFieldRow.Length; j++)
                        {
                            string strLen = "";
                            string strType = "";
                            string[] strRows = strFieldRow[j].Split(' ');
                            DataRow dtRow = dtTable.NewRow();

                            if (strRows[1].ToString().IndexOf("(") == -1)
                            {
                                strLen = string.Empty;
                                strType = strRows[1];
                            }
                            else
                            {
                                strLen = strRows[1].Substring(strRows[1].IndexOf("(") + 1, strRows[1].IndexOf(")") - strRows[1].IndexOf("(") - 1);
                                strType = strRows[1].Substring(0,strRows[1].IndexOf("("))+"()";
                            }

                            //字段名
                            dtRow[0] = strRows[0];
                            //字段长度
                            dtRow[1] = strLen;
                            //表分区标示
                            if (strRows[2] == "1")
                            {
                                dtRow[2] = true;
                            }
                            else
                            {
                                dtRow[2] = false;
                            }
                            //字段类型
                            dtRow[3] = strType;
                            dtTable.Rows.Add(dtRow);

                        }
                        this.dgTable.DataSource = this.dtTable;
                    }
                    //已选择大区
                    string strAreaIDs = dsTable.Tables[0].Rows[0].ItemArray[6].ToString();
                    if (this.dgArea.Rows.Count > 0 && strAreaIDs!=string.Empty)
                    {
                        string[] strIdArr = strAreaIDs.Split(',');
                        for (int iArea = 0; iArea< this.dgArea.Rows.Count; iArea++)
                        {
                            for (int iArr = 0; iArr < strIdArr.Length; iArr++)
                            {
                                if (strIdArr[iArr]==this.dgArea.Rows[iArea].Cells[1].Value.ToString())
                                {
                                    this.dgArea.Rows[iArea].Cells[0].Value = 1;
                                }
                            }
                         
                        }
                    }
                    //源数据库
                    this.txtDB.Text = dsTable.Tables[0].Rows[0].ItemArray[10].ToString();
                    //gameid
                    this._gameID = dsTable.Tables[0].Rows[0].ItemArray[11].ToString();
                    //源数据表
                    this.txtTable.Text = dsTable.Tables[0].Rows[0].ItemArray[8].ToString();
                    //目的数据表
                    this.txtDestable.Text = dsTable.Tables[0].Rows[0].ItemArray[9].ToString();

                    //采集开关
                    if (dsTable.Tables[0].Rows[0].ItemArray[12].ToString()=="0")
                    {
                        this.cbxSwitch.SelectedIndex=1;
                    }
                    else
                    {
                        this.cbxSwitch.SelectedIndex = 0;
                    }

                    this.dpStartTime.Value = Convert.ToDateTime(dsTable.Tables[0].Rows[0].ItemArray[13].ToString());
                   
                    string[] day = dsTable.Tables[0].Rows[0].ItemArray[14].ToString().Split('.');
                    string[] HourMinute = day[1].Split(':');
                    this.nudHour.Value = Convert.ToDecimal(HourMinute[0]);
                    this.nudMinute.Value = Convert.ToDecimal(HourMinute[1]);
                    this.nudDay.Value = Convert.ToDecimal(day[0]);
                }
            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog("LoadData->" + ex.Message);
            }
        }
        #endregion

        #region 检查画面输入
        private bool blnChkSave()
        {
            try
            {
                if (this.dgArea.Rows.Count > 0)
                {
                    int iHaschecked = 0;
                    for (int i = 0; i < this.dgArea.Rows.Count; i++)
                    {
                        if (this.dgArea.Rows[i].Cells[0].Value.ToString() == "1" || this.dgArea.Rows[i].Cells[0].Value.ToString() == "True")
                        {
                            iHaschecked = 1;
                            break;
                        }
                    }
                    if (iHaschecked == 0)
                    {
                        MessageBox.Show("请选择要操作的大区");
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("请选择要操作的大区");
                    return false;
                }

                if (cbxGameName.Text == string.Empty)
                {
                    MessageBox.Show("请输入游戏名");
                    cbxGameName.Focus();
                    return false;
                }

                if (this.txtDB.Text == string.Empty)
                {
                    MessageBox.Show("请输入数据源库名");
                    txtDB.Focus();
                    return false;
                }

                if (this.txtTable.Text == string.Empty)
                {
                    MessageBox.Show("请输入数据源表名");
                    txtTable.Focus();
                    return false;
                }

                if (this.txtDestable.Text == string.Empty)
                {
                    MessageBox.Show("请输入目的表名");
                    this.txtDestable.Focus();
                    return false;
                }

                if (this.dgTable.Rows.Count == 0)
                {
                    MessageBox.Show("请添加采集表的结构");
                    return false;
                }

                if (this.txtBcp.Text == string.Empty)
                {
                    MessageBox.Show("请输入BCP语句");
                    return false;
                }

                if (this.rdb2.Checked == true)
                {
                    if(this.txtTblSql.Text.Trim()==string.Empty)
                    {
                        MessageBox.Show("请输入手动建表的SQL语句");
                        this.txtTblSql.Focus();
                        return false;
                    }
                }

                //判断间隔时间是否为输入
                if (this.nudDay.Value==0 && this.nudHour.Value == 0 && this.nudMinute.Value == 0)
                {
                    MessageBox.Show("请输入采集间隔时间");
                    this.nudHour.Focus();
                    return false;
                }
                //
                return true;
            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog("blnChkSave->" + ex.Message);
                return false;
            }
        }

        #endregion

        #region 取得游戏LIST
        private void GetGameLIST()
        {
            try
            {
                //string strsql = " SELECT * FROM Game_Info ";
                //this.dsGame = CommonCLS.RunOracle(strsql);
                OracleParameter[] parameters = 
                    {
                        new OracleParameter("cur_Result", OracleDbType.RefCursor, ParameterDirection.Output)
                    };
                this.dsGame = CommonCLS.RunOracleSP("PD_GameInfo_Pack.PD_GameList_Query", parameters);
                if (dsGame != null && this.dsGame.Tables[0].Rows.Count > 0)
                {
                    this.cbxGameName.Items.Clear();                  
                        for (int i = 0; i < dsGame.Tables[0].Rows.Count; i++)
                        {
                            //添加到COMBOX
                            cbxGameName.Items.Add(dsGame.Tables[0].Rows[i].ItemArray[1].ToString());
                        }
                        cbxGameName.SelectedIndex = 0;
                    cbxGameName.DropDownStyle = ComboBoxStyle.DropDownList;
                }
                else
                {
                    MessageBox.Show("暂无游戏");
                }
            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog("frmGameEdit_Load->" + ex.Message); 
            }
        }

        #endregion

        #region 画面初始化
        private void initForm()
        {
            try
            {
                this.Size = new Size(689, 763);
                this.rdb1.Checked = true;
                //初始化datatable
                dtServer.Columns.Add("大区名");
                dtServer.Columns.Add("服务器IP");
                dtServer.Columns.Add("数据库名");
                dtServer.Columns.Add("用户名");
                dtServer.Columns.Add("密码");

                dtTable.Columns.Add("字段名");
                dtTable.Columns.Add("字段长度");
                dtTable.Columns.Add("表分区标示");
                dtTable.Columns.Add("字段类型");

                dtArea.Columns.Add("chk");
                dtArea.Columns.Add("id");
                dtArea.Columns.Add("ServerNM");
                dtArea.Columns.Add("Server_Ip");
                dtArea.Columns.Add("PORT");
                //清除表中所有行
                this.dgTable.DataSource = dtTable;
               
                //获得游戏名COMBOX
                this.GetGameLIST();

                if (this._mode == 0)
                {
                    //对表结构中加入固定列
                    DataRow dtRowIP = dtTable.NewRow();
                    dtRowIP[0] = "ServerIP";
                    dtRowIP[1] = "200";
                    dtRowIP[2] = true;
                    dtRowIP[3] = "NVARCHAR2()";
                    dtTable.Rows.Add(dtRowIP);

                    DataRow dtRowDB = dtTable.NewRow();
                    dtRowDB[0] = "ServerDB";
                    dtRowDB[1] = "500";
                    dtRowDB[2] = false;
                    dtRowDB[3] = "NVARCHAR2()";
                    dtTable.Rows.Add(dtRowDB);
                }
                this.dgTable.DataSource = this.dtTable;

                //取得大区列表
               int  igame_id = CommonCLS.GetGameID(this.cbxGameName.Text.Trim());
               GetAreaList(igame_id);

            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog("frmGameEdit_Load->" + ex.Message);
            }
        }
        #endregion

        #region 生成CTL语句
        private string BuildCTL(string strBcpName,string strMODE)
        {
            try
            {
                string strCTL = string.Empty;
                string strField = string.Empty;
                string filelist = null;
                string rowlist = null;

                for (int iField = 0; iField < this.dtTable.Rows.Count; iField++)
                {
                    if (iField == 0)
                    {
                        if (this.dtTable.Rows[iField].ItemArray[1].ToString() == "DATE")
                        {
                            strField = strField + this.dtTable.Rows[iField].ItemArray[0].ToString() + " DATE \"YYYY-MM-DD HH24:MI:SS\""; 
                        }
                        else 
                        {
                            strField = strField + this.dtTable.Rows[iField].ItemArray[0].ToString(); 
                        }
                    }
                    else
                    {
                        if (this.dtTable.Rows[iField].ItemArray[1].ToString() == "DATE")
                        {
                            strField = strField + ","  + this.dtTable.Rows[iField].ItemArray[0].ToString() + " DATE \"YYYY-MM-DD HH24:MI:SS\""; 
                        }
                        else 
                        {
                            strField = strField + ","  + this.dtTable.Rows[iField].ItemArray[0].ToString(); 
                        }
                    }
                }

               // filelist = "INFILE '" + strBcpName.Replace(",", "'\r\nINFILE '") + "'\r\n";
                filelist = " Replace_Part ";
                rowlist = strField.Replace(",", ",\r\n");

                strCTL = "Load DATA\r\n";
                strCTL = strCTL + filelist;
                strCTL = strCTL + " INTO TABLE " + this.txtDestable.Text.Trim() + "\r\n";
                strCTL = strCTL + " APPEND\r\n";
                strCTL = strCTL + "FIELDS TERMINATED by X'09'\r\n TRAILING NULLCOLS \r\n";
                strCTL = strCTL + "(\r\n" + rowlist + "\r\n)";
                return strCTL;
            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog("BuildCTL->" + ex.Message);
                return string.Empty;
            }
        }
        #endregion


        #region 取得大区列表
        private void GetAreaList(int iGameID)
        {
            try
            {
                OracleParameter[] parameters = 
                    {
                        new OracleParameter("v_GameID", OracleDbType.Int32, ParameterDirection.Input),
                        new OracleParameter("v_result", OracleDbType.RefCursor, ParameterDirection.Output)
                    };
                parameters[0].Value = iGameID;
                DataSet dsArea = CommonCLS.RunOracleSP("PD_GameInfo_Pack.PD_AreaList_Query", parameters);
                this.dtArea.Rows.Clear();
                if (dsArea != null && dsArea.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < dsArea.Tables[0].Rows.Count; j++)
                    {
                        DataRow dtRow = dtArea.NewRow();
                        dtRow[0] = false ;
                        dtRow[1] = dsArea.Tables[0].Rows[j].ItemArray[0].ToString();
                        dtRow[2] = dsArea.Tables[0].Rows[j].ItemArray[2].ToString();
                        dtRow[3] = dsArea.Tables[0].Rows[j].ItemArray[1].ToString();
                        dtRow[4] = dsArea.Tables[0].Rows[j].ItemArray[7].ToString();
                        dtArea.Rows.Add(dtRow);
                    }
                }
                this.dgArea.DataSource = this.dtArea;
            }      
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog("GetAreaList->" + ex.Message);
            }
        }
        #endregion

        #region 取得BCP_NAME
        private string GetBcp_CTLNM(string strType)
        {
            StringBuilder strNM = new StringBuilder();
            try
            {
                //根据钩选的大区和采集对象表名，生成bcp/ctl文件名字
                for (int i = 0; i < this.dgArea.Rows.Count; i++)
                {
                    if (Convert.ToString(this.dgArea.Rows[i].Cells[0].Value) == "1" || Convert.ToString(this.dgArea.Rows[i].Cells[0].Value) == "True")
                    {
                        if (strNM.ToString() != string.Empty)
                        {
                            strNM.Append(",");
                        }
                        strNM.Append(this.dgArea.Rows[i].Cells[2].Value+this.txtTable.Text.Trim()+"."+strType);
                       
                    }
                   
                }
                return strNM.ToString();
                
            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog("GetBcp_Name->" + ex.Message);
                return string.Empty;
            }
        }
        #endregion

        #region 取得大区id字段
        private string GetAreaID()
        {
            StringBuilder strid = new StringBuilder();
            try
            {
                //根据钩选的大区和采集对象表名，生成bcp文件名字
                for (int i = 0; i < this.dgArea.Rows.Count; i++)
                {
                    if (Convert.ToString(this.dgArea.Rows[i].Cells[0].Value) == "1" || Convert.ToString(this.dgArea.Rows[i].Cells[0].Value) == "True")
                    {
                        if (strid.ToString() != string.Empty )
                        {
                            strid.Append(",");
                        }
                        strid.Append(this.dgArea.Rows[i].Cells[1].Value);

                    }

                }
                return strid.ToString();
            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog("GetAreaID->" + ex.Message);
                return string.Empty;
            }
        }
        #endregion

        #region 取得bcp_sql
        private string GetBcpSql()
        {
            string strBcpSql = string.Empty;
            try
            {
                strBcpSql = this.txtBcp.Text;
                string strSubSelect = strBcpSql.Substring(strBcpSql.ToUpper().IndexOf("SELECT"),6);
                strBcpSql = strBcpSql.Replace(strSubSelect, @"SELECT '%s' AS ServerIP , '%s' AS ServerDB,");
                return strBcpSql;
            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog("GetBcpSql->" + ex.Message);
                return string.Empty;
            }
        }
        #endregion

        #region 取得DBROW
        private string GetDBRow()
        {
            StringBuilder strDBrow = new StringBuilder();
            try
            {
                //类型与长度 
                string strItem2 = string.Empty;
                
                //取得表结构字段
                for (int iField = 0; iField < this.dtTable.Rows.Count; iField++)
                {
                    if (this.dtTable.Rows[iField].ItemArray[3].ToString().IndexOf("()") != -1)
                    {
                        strItem2 = this.dtTable.Rows[iField].ItemArray[3].ToString().Replace("()", "(" + this.dtTable.Rows[iField].ItemArray[1].ToString() + ")");
                    }
                    else
                    {
                        strItem2 =this.dtTable.Rows[iField].ItemArray[3].ToString();
                    }
                    string strChk = string.Empty;
                    if (this.dtTable.Rows[iField].ItemArray[2].ToString() == "True" || this.dtTable.Rows[iField].ItemArray[2].ToString() == "1")
                    {
                        strChk = "1";
                    }
                    else
                    {
                        strChk = "0";
                    }

                    if (iField == 0)
                    {

                        strDBrow.Append(this.dtTable.Rows[iField].ItemArray[0].ToString() 
                            + " " + strItem2
                            + " " + strChk);
                    }
                    else
                    {
                        strDBrow.Append("," + this.dtTable.Rows[iField].ItemArray[0].ToString() 
                            + " " + strItem2
                            + " " + strChk) ;
                    }
                }
                return strDBrow.ToString();
            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog("SaveData->" + ex.Message);
                return string.Empty;
            }
        }
        #endregion

        #region 取得游戏id
        private int GetGameid(string strName, out string straccount,out string strpwd,out string strtns)
        {
            int iId = 0;
            straccount = "";
            strpwd = "";
            strtns = "";
            try
            {
                if (this.dsGame!=null && this.dsGame.Tables[0].Rows.Count>0)
                {
                for (int i = 0; i < this.dsGame.Tables[0].Rows.Count; i++)
                {
                    if (this.dsGame.Tables[0].Rows[i].ItemArray[1].ToString() == strName)
                    {
                        iId = Convert.ToInt32(this.dsGame.Tables[0].Rows[i].ItemArray[0]);
                        straccount = Convert.ToString(this.dsGame.Tables[0].Rows[i].ItemArray[4]);
                        strpwd = Convert.ToString(this.dsGame.Tables[0].Rows[i].ItemArray[5]);
                        strtns = Convert.ToString(this.dsGame.Tables[0].Rows[i].ItemArray[3]);
                        break;
                    }
                }
                }
                return iId ;
            }
            catch(System.Exception ex)
            {
                CommonCLS.SaveLog("GetGameid->" + ex.Message);
                return 0;
            }
        }
        #endregion

        #region 取得购选大区的ip列表
        private DataTable GetCheckedIpLst()
        {
            DataTable dtIP=new DataTable();
            dtIP.Columns.Add("AreaNm");
            dtIP.Columns.Add("AreaIP");
            try
            {
                for (int i = 0; i < this.dgArea.Rows.Count; i++)
                {
                    if (Convert.ToString(this.dgArea.Rows[i].Cells[0].Value) == "1" || Convert.ToString(this.dgArea.Rows[i].Cells[0].Value) == "True")
                    {
                        DataRow dtRow = dtIP.NewRow();
                        dtRow[0] = dgArea.Rows[i].Cells[2].Value.ToString() ;
                        dtRow[1] = dgArea.Rows[i].Cells[3].Value.ToString();
                        dtIP.Rows.Add(dtRow);
                    }

                }
                return dtIP;
            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog("GetCheckedIpLst->" + ex.Message);
                return null;
            }
        }
        #endregion
        #endregion
        #region 事件
        #region 字段添加按钮按下
        /// <summary>
        /// 字段添加按钮按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd2_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < this.dgTable.Rows.Count; i++)
                {
                    if (dgTable.Rows[i].Cells[1].Value.ToString() == this.txtField.Text.Trim())
                    {
                        MessageBox.Show("该字段已添加");
                        return;
                    }
                }
                if (this.cmbType.Text != "NUMBER")
                {
                    if (this.txtField.Text == string.Empty || this.txtLength.Text == string.Empty)
                    {
                        MessageBox.Show("请输入字段名和长度");
                        return;
                    }
                }
                else
                {
                    if (this.txtField.Text == string.Empty)
                    {
                        MessageBox.Show("请输入字段名");
                        return;
                    }
                }

                DataRow dtRow = dtTable.NewRow();
                //字段名
                dtRow[0] = this.txtField.Text.Trim();
                if (this.txtLength.Text.Trim() != "0")
                {
                    dtRow[1] = this.txtLength.Text.Trim();
                }
                else
                {
                    dtRow[1] = "";
                }
                dtRow[3] = this.cmbType.Text.Trim();
                dtTable.Rows.Add(dtRow);
                //this.dgTable .DataSource = null;
                this.dgTable.DataSource = this.dtTable;
                //清空控件
                this.txtField.Text = string.Empty;
                this.txtLength.Text = string.Empty;
                this.txtField.Focus();
                
            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog("btnAdd2_Click->" + ex.Message);
            }
            finally
            {
                this.dgTable.Columns[2].ReadOnly = false;
            }
        }
        #endregion

        #region 字段去除按钮按下
        /// <summary>
        /// 字段去除按钮按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemove2_Click(object sender, EventArgs e)
        {
            try
            {
                //行未选择
                if (this.dgTable.SelectedRows.Count == 0)
                {
                    MessageBox.Show("请选择要去除的字段");
                    return;
                }

                //判断固定列
                if (this.dgTable.SelectedRows[0].Cells[0].Value.ToString() == "id" ||
                    this.dgTable.SelectedRows[0].Cells[0].Value.ToString() == "ServerIP" ||
                    this.dgTable.SelectedRows[0].Cells[0].Value.ToString() == "ServerDB")
                {
                    MessageBox.Show("该字段为固定列，无法删除");
                    return;
                }
                //进行去除
                if (this.dgTable.Rows.Count > 0)
                {
                    this.dgTable.Rows.RemoveAt(this.dgTable.SelectedRows[0].Index);
                }
                else
                {
                    MessageBox.Show("字段列表中无数据");
                    return;
                }

            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog("btnRemove2_Click->" + ex.Message);
            }
            finally
            {
                this.dgTable.Columns[2].ReadOnly = false;
            }
        }
        #endregion

        #region 确定按钮按下
        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                
                if (blnChkSave() == true)
                {
                    //if (this.rdb2.Checked == true && this.txtTblSql.Text != string.Empty)
                    //{
                    //    string strStmp = this.txtTblSql.Text.Substring(13, this.txtTblSql.Text.Length - this.txtTblSql.Text.IndexOf('('));
                    //    strStmp = strStmp.Substring(0, strStmp.IndexOf('('));
                    //    this.txtDestable.Text = strStmp;
                    //}
                   
                    this.Cursor = Cursors.WaitCursor;
                    if (this.SaveData() == true)
                    {
                        if (this._mode == 0)
                        {
                            MessageBox.Show("添加成功");
                        }
                        else
                        {
                            MessageBox.Show("更新成功");
                        }
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        if (this._mode == 0)
                        {
                            MessageBox.Show("添加失败");
                        }
                        else
                        {
                            MessageBox.Show("更新失败");
                        }

                    }
                }
            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog("btnOk_Click->" + ex.Message);
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        #endregion

        #region 画面读入
        private void frmGameEdit_Load(object sender, EventArgs e)
        {
            try
            {
                //调用画面初始化方法
                this.initForm();
                if (this._mode == 0)    //新建模式
                {
                    
                }
                else if(this._mode==1)  //修改模式
                {

                    if (this._game_name != string.Empty)
                    {
                        cbxGameName.SelectedIndex = cbxGameName.Items.IndexOf(this._game_name);
                        //读取数据
                        this.LoadData();
                        this.dgTable.Columns[2].ReadOnly = false;
                    }
                }
                
            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog("frmGameEdit_Load->" + ex.Message); 
            }
        }
        #endregion

        private void txtTable_Leave(object sender, EventArgs e)
        {
            string strtmp=string.Empty;     //返回用变量
            string strsql = string.Empty;
            string strTmpSplit = string.Empty;
            try
            {
                if (this._mode == 1)
                {
                    //如果是修改模式则推出
                    return;
                }
                else
                {
                    //如果源数据库名和源表名不为空
                    if (this.txtDB.Text != string.Empty && this.txtTable.Text != string.Empty)
                    {
                        int iDbType = 0;
                        if (this._mode == 0)
                        {
                            iDbType = CommonCLS.GetGameDbType(this.cbxGameName.Text.Trim());
                        }
                        else
                        {
                            iDbType = CommonCLS.GetGameDbTypeById(Convert.ToInt32(this._gameID));
                        }
                        if (iDbType == 3)
                        {
                            strTmpSplit = ".";
                        }
                        else
                        {
                            strTmpSplit = "..";
                        }
                        strtmp = "SELECT t.* FROM " +this.txtDB.Text.Trim()+strTmpSplit+ this.txtTable.Text.Trim() + " t";
                    }
                }

                 this.txtBcp.Text = strtmp;
            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog("txtTable_Leave->" + ex.Message);
            }     
        }

        private void cbxGameName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strsql = string.Empty;
            try
            {
                if (this.cbxGameName.Text.Trim()!=string.Empty )
                {
                    strsql = "SELECT SQL_COLLECT FROM GAME_INFO WHERE GAME_NAME='"+ this.cbxGameName.Text.Trim() +"'";
                    DataSet dsgm = CommonCLS.RunOracle(strsql);
                    if (dsgm != null && dsgm.Tables.Count > 0 && dsgm.Tables[0].Rows.Count > 0)
                    {
                        this.strCount = dsgm.Tables[0].Rows[0].ItemArray[0].ToString();
                    }
                    //取得大区列表
                    int igame_id = CommonCLS.GetGameID(this.cbxGameName.Text.Trim());
                    GetAreaList(igame_id);
                }

            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog("cbxGameName_SelectedIndexChanged->" + ex.Message);
            }
        }

        private void txtLength_Enter(object sender, EventArgs e)
        {
            this.stbStatus.Text = "若字段的类型为日期类型，请在长度输入栏里输入\"DATE\"";
        }

        private void txtLength_Leave(object sender, EventArgs e)
        {
            this.stbStatus.Text = "";
        }

        private void dgTable_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 2 && e.RowIndex != -1)
            {

                //获取控件的值
                int iCheckedRowNum = 0;
                for (int i = 0; i < this.dgTable.Rows.Count; i++)
                {

                    if (Convert.ToBoolean(this.dgTable.Rows[i].Cells[2].EditedFormattedValue) == true)
                    {
                        iCheckedRowNum++;
                    }
                }

                if (iCheckedRowNum > 2)
                {

                    MessageBox.Show("表分组字段不能超过两个");
                    this.dtTable.Rows[e.RowIndex][2] = false;
                    this.dgTable.DataSource = this.dtTable;
                    return;
                }
            }
        }

        #region 全选checkbox
        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                int iValue=0;
                if (this.chkAll.Checked==true)
                {
                    iValue=1;
                }
                else
                {
                    iValue = 0;
                }
                if (this.dgArea.Rows.Count > 0)
                {
                    for (int i = 0; i < this.dgArea.Rows.Count; i++)
                    {
                        this.dgArea.Rows[i].Cells[0].Value =iValue ;
                    }
                }
            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog("chkAll_CheckedChanged->" + ex.Message);
            }
        }
        #endregion

        private void rdb1_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rdb1.Checked == true)
            {
                this.label4.Visible = false;
                this.txtTblSql.Enabled = false;
                this.txtTblSql.Visible = false;
                this.Size = new Size(689, 763);
            }

            if (this.rdb2.Checked == true)
            {
                this.label4.Visible = true;
                this.txtTblSql.Enabled = true;
                this.txtTblSql.Visible = true;
                this.Size = new Size(689, 900);
            }
        }
        #endregion

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.cmbType.Text.IndexOf("()") == -1)
                {
                    this.txtLength.Enabled = false;
                    this.txtLength.Text = "0";
                }
                else
                {
                    this.txtLength.Enabled = true;
                    this.txtLength.Text = string.Empty;
                }
            }
            catch (System.Exception ex)
            {
            }
        }


        private string getInterVal()
        {
            string strtmp = "";
            //00 01:30:00.000000
            decimal hour = this.nudHour.Value;
            decimal day = this.nudDay.Value;
            decimal minute = this.nudMinute.Value;

            //天数
            if (day < 10)
            {
                strtmp = strtmp + "0" + day;
            }
            else
            {
                strtmp = strtmp + day;
            }

            //小时数
            if (hour < 10)
            {
                strtmp = strtmp + " " +"0" +hour;
            }
            else
            {
                strtmp = strtmp + " " + hour;
            }

            //分钟数
            if (minute < 10)
            {
                strtmp = strtmp + ":" + "0" + minute;
            }
            else
            {
                strtmp = strtmp + ":" + minute;
            }
            strtmp = strtmp + ":00.000000";
            return strtmp;
        }
    }
}