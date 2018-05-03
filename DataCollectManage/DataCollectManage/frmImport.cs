using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Oracle.DataAccess.Client;
namespace DataCollectMonitor
{
    public partial class frmImport : Form
    {
        public frmImport()
        {
            InitializeComponent();
        }

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

        #region  变量
        //导入的excel数据
        DataSet _dsServer = null;
        DataSet _dsTable = null;

        DataSet dsGame = null;
        DataTable dtArea=new DataTable();
        #endregion

        #region 事件

        /// <summary>
        /// 导入按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            string strSQL = string.Empty;
            int game_id;  //游戏ID
            int result1 = 0;
            int result2 = 0;
            if (MessageBox.Show("确认导入？", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.Enabled = false;
                string[] sNM = CommonCLS.ExcelSheetNameToDS(txtFilePath.Text);
                _dsServer = CommonCLS.ExcelToDS(txtFilePath.Text, "SERVER");

                //_dsTable = CommonCLS.ExcelToDS(txtFilePath.Text, "TABLE");
                //if (this.txtDesTable.Text == "")
                //{
                //    MessageBox.Show("请先输入采集目的表名,再进行导入");
                //    return;
                //}

                //if (_dsServer != null)
                //{
                //    for (int iX = 0; iX < _dsServer.Tables[0].Rows.Count; iX++)
                //    {
                //        string sIpAddr = _dsServer.Tables[0].Rows[iX].ItemArray[0].ToString() + _dsServer.Tables[0].Rows[iX].ItemArray[2].ToString() + _dsServer.Tables[0].Rows[iX].ItemArray[3].ToString();
                //        for (int iY = 0; iY < _dsServer.Tables[0].Rows.Count; iY++)
                //        {
                //            if (iX != iY && _dsServer.Tables[0].Rows[iY].ItemArray[0].ToString() + _dsServer.Tables[0].Rows[iY].ItemArray[2].ToString() + _dsServer.Tables[0].Rows[iY].ItemArray[3].ToString() == sIpAddr)
                //            {
                //                MessageBox.Show("要导入的文件中， 有重复数据,IP:" + _dsServer.Tables[0].Rows[iY].ItemArray[0].ToString() + ".数据库名:" + _dsServer.Tables[0].Rows[iY].ItemArray[2].ToString() + ".数据库源表名:" + _dsServer.Tables[0].Rows[iY].ItemArray[3].ToString() + "，请修改！");
                //                return;
                //            }
                //        }
                //    }
                //}

                //if (_dsTable != null)
                //{
                //    for (int iX = 0; iX < _dsTable.Tables[0].Rows.Count; iX++)
                //    {
                //        string sField = _dsTable.Tables[0].Rows[iX].ItemArray[0].ToString();
                //        for (int iY = 0; iY < _dsTable.Tables[0].Rows.Count; iY++)
                //        {
                //            if (iX != iY && _dsTable.Tables[0].Rows[iY].ItemArray[0].ToString() == sField)
                //            {
                //                MessageBox.Show("要导入的文件中， 表结构字段名有重复:" + _dsTable.Tables[0].Rows[iY].ItemArray[0].ToString() + "，请修改！");
                //                return;
                //            }
                //        }
                //    }
                //}


             
                game_id = GetGameid(this.cbxGameName.Text.Trim());
                int iDbType = CommonCLS.GetGameDbType(this.cbxGameName.Text.Trim());
                string strTbSpace = CommonCLS.GetGameTbSpace(this.cbxGameName.Text.Trim());
                for (int iServer = 0; iServer < _dsServer.Tables[0].Rows.Count; iServer++)
                {
                    _dsTable = null;
                    _dsTable = CommonCLS.ExcelToDS(txtFilePath.Text, _dsServer.Tables[0].Rows[iServer].ItemArray[2].ToString());
                    strSQL = "";

                    //插入import_express
                    //gamedb_ID
                    string gamedb_ID = this.GetAreaID(); 
                    //bcp_name
                    string strBcpName = this.GetBcp_CTLNM("txt",_dsServer.Tables[0].Rows[iServer].ItemArray[1].ToString());
                    //bcp_sql
                    string strbcp_selectTMP = _dsServer.Tables[0].Rows[iServer].ItemArray[3].ToString();
                    strbcp_selectTMP = strbcp_selectTMP.ToUpper().Replace("SELECT", "SELECT  '%s' AS ServerIP , '%s' AS ServerDB,");
                    //ctl_name
                    string strCTLNAME = this.GetBcp_CTLNM("ctl", _dsServer.Tables[0].Rows[iServer].ItemArray[1].ToString());
                    //DBROW
                    string strDBrow = this.GetDBRow(_dsTable.Tables[0]);
                    //ctl_byte 生成CTL字节流
                    byte[] str = System.Text.Encoding.Default.GetBytes(this.BuildCTL(strBcpName, "APPEND", _dsTable.Tables[0], _dsServer.Tables[0].Rows[iServer].ItemArray[2].ToString()));

                    OracleParameter[] parameters1 = 
                    {
                        new OracleParameter("v_GameDBid", OracleDbType.Varchar2, ParameterDirection.Input),
                        new OracleParameter("v_bcpName", OracleDbType.Varchar2, ParameterDirection.Input),
                        new OracleParameter("v_bcpsql", OracleDbType.Varchar2, ParameterDirection.Input),
                        new OracleParameter("v_CTLNAME", OracleDbType.Varchar2, ParameterDirection.Input),
                        new OracleParameter("v_DBrow", OracleDbType.Varchar2, ParameterDirection.Input),
                        new OracleParameter("v_ctl", OracleDbType.Blob, ParameterDirection.Input),
                        new OracleParameter("v_SRCTABLE", OracleDbType.Varchar2, ParameterDirection.Input),
                        new OracleParameter("v_DESTABLE", OracleDbType.Varchar2, ParameterDirection.Input),
                        new OracleParameter("v_SERVER_DB", OracleDbType.Varchar2, ParameterDirection.Input),
                        new OracleParameter("v_GAME_ID", OracleDbType.Varchar2, ParameterDirection.Input),
                        new OracleParameter("v_result", OracleDbType.Decimal, ParameterDirection.Output)
                    };

                    parameters1[0].Value = gamedb_ID;                   //gamedb_ID  
                    parameters1[1].Value = strBcpName;                  //bcp_name
                    parameters1[2].Value = strbcp_selectTMP.ToUpper();  //bcp_sql
                    parameters1[3].Value = strCTLNAME;                  //ctl_name
                    parameters1[4].Value = strDBrow;                    //DBROW
                    parameters1[5].Value = str;                         //ctl_byte
                    parameters1[6].Value = _dsServer.Tables[0].Rows[iServer].ItemArray[1].ToString();       //源数据表
                    parameters1[7].Value = _dsServer.Tables[0].Rows[iServer].ItemArray[2].ToString();       //目的数据表
                    parameters1[8].Value = _dsServer.Tables[0].Rows[iServer].ItemArray[0].ToString();       //源库名
                    parameters1[9].Value = game_id;                     //游戏id
                    result2 = CommonCLS.RunOracleNonQuerySp("PD_Game_Admin.PD_IMPORTEXPRESS_Insert", parameters1);
                    if (result2 > 0)
                    {
                        //记录操作log
                        CommonCLS.WriteOperatelog("插入import_express表,内容: "
                                                + " BCP名字:" + strBcpName
                                                + " BCP SELECT 语句:" + strbcp_selectTMP
                                                + " CTl名字:" + strCTLNAME);
                    }
                    //检查表是否存在

                    OracleParameter[] parameters = 
                    {
                        new OracleParameter("v_TbNm", OracleDbType.Varchar2, ParameterDirection.Input),
                        new OracleParameter("cur_Result", OracleDbType.RefCursor, ParameterDirection.Output)
                    };
                    parameters[0].Value = _dsServer.Tables[0].Rows[iServer].ItemArray[2].ToString();
                    DataSet dsTable = CommonCLS.RunOracleSP("PD_GameInfo_Pack.PD_CheckTableExist_Query", parameters);
                    if (dsTable.Tables[0].Rows[0].ItemArray[0].ToString()== "0")
                    {
                        DataTable dtIP = GetCheckedIpLst();
                       // CommonCLS.CreateTBL(strDBrow, _dsServer.Tables[0].Rows[iServer].ItemArray[2].ToString(), 1, dtIP, strTbSpace);
                    }
                }
                this.Enabled = true;
                this.Cursor = Cursors.Default;
                if (result2 > 0)
                {
                    MessageBox.Show("导入成功！");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("导入失败！");
                }


            }
            catch (System.Exception ex)
            {

                CommonCLS.SaveLog("btnOk_Click->" + ex.ToString());
                MessageBox.Show(ex.ToString());
                this.Close();
            }
        }

        #region 取得游戏id
        private int GetGameid(string strName)
        {
            int iId = 0;
            try
            {
                if (this.dsGame != null && this.dsGame.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < this.dsGame.Tables[0].Rows.Count; i++)
                    {
                        if (this.dsGame.Tables[0].Rows[i].ItemArray[1].ToString() == strName)
                        {
                            iId = Convert.ToInt32(this.dsGame.Tables[0].Rows[i].ItemArray[0]);
                            break;
                        }
                    }
                }
                return iId;
            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog("GetGameid->" + ex.Message);
                return 0;
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
                        if (strid.ToString() != string.Empty)
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
        #region 取得BCP_NAME
        private string GetBcp_CTLNM(string strType,string sTable)
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
                        strNM.Append(this.dgArea.Rows[i].Cells[2].Value + sTable + "." + strType);

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
        #region 取得DBROW
        private string GetDBRow(DataTable dtTable)
        {
            StringBuilder strDBrow = new StringBuilder();
            try
            {
                //取得表结构字段
                for (int iField = 0; iField < dtTable.Rows.Count; iField++)
                {
                    string strChk = string.Empty;
                    if (dtTable.Rows[iField].ItemArray[2].ToString() == "True" || dtTable.Rows[iField].ItemArray[2].ToString() == "1")
                    {
                        strChk = "1";
                    }
                    else
                    {
                        strChk = "0";
                    }

                    if (iField == 0)
                    {

                        strDBrow.Append(dtTable.Rows[iField].ItemArray[0].ToString() + " " + dtTable.Rows[iField].ItemArray[1].ToString() + " " + strChk);
                    }
                    else
                    {
                        strDBrow.Append("," + dtTable.Rows[iField].ItemArray[0].ToString() + " " + dtTable.Rows[iField].ItemArray[1].ToString() + " " + strChk);
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

        #region 取得购选大区的ip列表
        private DataTable GetCheckedIpLst()
        {
            DataTable dtIP = new DataTable();
            dtIP.Columns.Add("AreaNm");
            dtIP.Columns.Add("AreaIP");
            try
            {
                for (int i = 0; i < this.dgArea.Rows.Count; i++)
                {
                    if (Convert.ToString(this.dgArea.Rows[i].Cells[0].Value) == "1" || Convert.ToString(this.dgArea.Rows[i].Cells[0].Value) == "True")
                    {
                        DataRow dtRow = dtIP.NewRow();
                        dtRow[0] = dgArea.Rows[i].Cells[2].Value.ToString();
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
        #region 生成CTL语句
        private string BuildCTL(string strBcpName, string strMODE, DataTable dtTable,string strDestable)
        {
            try
            {
                string strCTL = string.Empty;
                string strField = string.Empty;
                string filelist = null;
                string rowlist = null;

                for (int iField = 0; iField < dtTable.Rows.Count; iField++)
                {
                    if (iField == 0)
                    {
                        if (dtTable.Rows[iField].ItemArray[1].ToString() == "DATE")
                        {
                            strField = strField + dtTable.Rows[iField].ItemArray[0].ToString() + " DATE \"YYYY-MM-DD HH24:MI:SS\"";
                        }
                        else
                        {
                            strField = strField + dtTable.Rows[iField].ItemArray[0].ToString();
                        }
                    }
                    else
                    {
                        if (dtTable.Rows[iField].ItemArray[1].ToString() == "DATE")
                        {
                            strField = strField + "," + dtTable.Rows[iField].ItemArray[0].ToString() + " DATE \"YYYY-MM-DD HH24:MI:SS\"";
                        }
                        else
                        {
                            strField = strField + "," + dtTable.Rows[iField].ItemArray[0].ToString();
                        }
                    }
                }

                // filelist = "INFILE '" + strBcpName.Replace(",", "'\r\nINFILE '") + "'\r\n";
                filelist = " Replace_Part ";
                rowlist = strField.Replace(",", ",\r\n");

                strCTL = "Load DATA\r\n";
                strCTL = strCTL + filelist;
                strCTL = strCTL + " INTO TABLE " + strDestable + "\r\n";
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

        #region 取得游戏LIST
        private void GetGameLIST()
        {
            try
            {
                //string strsql = " SELECT * FROM Game_Info ";
                //DataSet  dsGame = CommonCLS.RunOracle(strsql);
                OracleParameter[] parameters = 
                    {
                        new OracleParameter("cur_Result", OracleDbType.RefCursor, ParameterDirection.Output)
                    };
                dsGame = CommonCLS.RunOracleSP("PD_GameInfo_Pack.PD_GameList_Query", parameters);

                if (dsGame != null && dsGame.Tables[0].Rows.Count > 0)
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
                        dtRow[0] = false;
                        dtRow[1] = dsArea.Tables[0].Rows[j].ItemArray[0].ToString();
                        dtRow[2] = dsArea.Tables[0].Rows[j].ItemArray[2].ToString();
                        dtRow[3] = dsArea.Tables[0].Rows[j].ItemArray[1].ToString();
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

        private void btnCancle_Click(object sender, EventArgs e)
        {

        }

        #endregion

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.FileName = "";
                openFileDialog1.Filter = "Excel文件 | *.xls";
                openFileDialog1.Title = "请选择文件";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    this.txtFilePath.Text = openFileDialog1.FileName;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void frmImport_Load(object sender, EventArgs e)
        {
            GetGameLIST();
            dtArea.Columns.Add("chk");
            dtArea.Columns.Add("id");
            dtArea.Columns.Add("ServerNM");
            dtArea.Columns.Add("Server_Ip");
            //取得大区列表
            int igame_id = CommonCLS.GetGameID(this.cbxGameName.Text.Trim());
            GetAreaList(igame_id);
        }

        private void cbxGameName_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strsql = string.Empty;
            try
            {
                if (this.cbxGameName.Text.Trim() != string.Empty)
                {
                   
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

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                int iValue = 0;
                if (this.chkAll.Checked == true)
                {
                    iValue = 1;
                }
                else
                {
                    iValue = 0;
                }
                if (this.dgArea.Rows.Count > 0)
                {
                    for (int i = 0; i < this.dgArea.Rows.Count; i++)
                    {
                        this.dgArea.Rows[i].Cells[0].Value = iValue;
                    }
                }
            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog("chkAll_CheckedChanged->" + ex.Message);
            }
        }
    }
}