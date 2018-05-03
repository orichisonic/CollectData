using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Oracle.DataAccess.Client;

namespace DataCollectMonitor
{
    public partial class frmGame_InfoDetail : Form
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
        public frmGame_InfoDetail()
        {
            InitializeComponent();
        }

        public frmGame_InfoDetail(string sID,int mode)
        {
            InitializeComponent();
            this._sID = sID;
            this._mode = mode;
        }
        #endregion

        #region 变量
        private string _sID = string.Empty;     //游戏ID
        private int _mode = 0;                  //模式0:新规,1:修改
        #endregion

        #region 方法
        #region 保存数据
        /// <summary>
        /// 保存数据
        /// </summary>
        private bool SaveData()
        {
            string strSQL = string.Empty;
            string strLogResult = string.Empty;
            int iFlag = 0;
            int result = 0;
            try
            {
                if (this._mode == 0)
                { 
                    //新规
                    
                    //strSQL = " INSERT INTO GAME_INFO VALUES(GAME_ID_SEQ.Nextval,";              //游戏ID ,自增长
                    //strSQL = strSQL + "'" + this.txtGameNM.Text.Trim() + "',";                  //游戏名
                    //if (this.chkStatus.Checked == true)
                    //{
                    //    strSQL = strSQL + " 1,";    //采集标志
                    //}
                    //else
                    //{ 
                    //    strSQL = strSQL + " 0,'";   //采集标志
                    //}
                    //strSQL = strSQL + "'" + this.txtTNS.Text.Trim() + "',";                     //TNS  
                    //strSQL = strSQL + "'" + this.txtCollactUser.Text.Trim() + "',";             //采集用户  
                    //strSQL = strSQL + "'" + this.txtCollactPwd.Text.Trim() + "',";              //采集密码
                    //strSQL = strSQL + "'" + this.txtManageUser.Text.Trim() + "',";              //管理用户
                    //strSQL = strSQL + "'" + this.txtManagePwd.Text.Trim() + "',";               //管理密码
                    //strSQL = strSQL + "'" + this.txtSPACE_MODULUS.Text.Trim() + "',";           //缓存系数
                    //strSQL = strSQL + this.cbxDbType.SelectedIndex+1 + ",";                     //数据库类型
                    //strSQL = strSQL + this.txtCount.Text+",";         //SQL_COUNT
                    //strSQL = strSQL + this.cbxTbSpace.Text + ")";         //表空间

                    OracleParameter[] parameters = 
                    {
                        new OracleParameter("v_GameName", OracleDbType.Varchar2),
                        new OracleParameter("v_flg", OracleDbType.Int32),
                        new OracleParameter("v_TNS", OracleDbType.Varchar2),
                        new OracleParameter("v_CollectUser", OracleDbType.Varchar2),
                        new OracleParameter("v_CollectPwd", OracleDbType.Varchar2),
                        new OracleParameter("v_ManageUser", OracleDbType.Varchar2),
                        new OracleParameter("v_ManagerPwd", OracleDbType.Varchar2),
                        new OracleParameter("v_SpaceModule", OracleDbType.Int32),
                        new OracleParameter("v_dbtype", OracleDbType.Int32),
                        new OracleParameter("v_sqlCnt", OracleDbType.Varchar2),
                        new OracleParameter("v_tableSpace", OracleDbType.Varchar2),
                        new OracleParameter("v_result", OracleDbType.Int32)
                    };

                    parameters[0].Direction = ParameterDirection.Input;
                    parameters[1].Direction = ParameterDirection.Input;
                    parameters[2].Direction = ParameterDirection.Input;
                    parameters[3].Direction = ParameterDirection.Input;
                    parameters[4].Direction = ParameterDirection.Input;
                    parameters[5].Direction = ParameterDirection.Input;
                    parameters[6].Direction = ParameterDirection.Input;
                    parameters[7].Direction = ParameterDirection.Input;
                    parameters[8].Direction = ParameterDirection.Input;
                    parameters[9].Direction = ParameterDirection.Input;
                    parameters[10].Direction = ParameterDirection.Input;
                    parameters[11].Direction = ParameterDirection.Output;

                    parameters[0].Value = this.txtGameNM.Text.Trim();

                    if (this.chkStatus.Checked == true)         //游戏名
                    {
                        parameters[1].Value = 1;    //采集标志
                    }
                    else
                    {
                        parameters[1].Value = 0;   //采集标志
                    }

                    if (this.txtTNS.Text.Trim() != string.Empty)
                    {
                        parameters[2].Value = this.txtTNS.Text.Trim();            //TNS  
                    }
                    else
                    {
                        parameters[2].Value = "empty";                      //TNS  
                    }
                    parameters[3].Value = this.txtCollactUser.Text.Trim();    //采集用户
                    parameters[4].Value = this.txtCollactPwd.Text.Trim();     //采集密码
                    parameters[5].Value = this.txtManageUser.Text.Trim();     //管理用户
                    parameters[6].Value = this.txtManagePwd.Text.Trim();      //管理密码
                    parameters[7].Value = this.txtSPACE_MODULUS.Text.Trim();  //缓存系数
                    parameters[8].Value = this.cbxDbType.SelectedIndex + 1;   //数据库类型
                    parameters[9].Value = "not use";                 //SQL_COUNT
                    //parameters[10].Value = this.cbxTbSpace.Text;              //表空间
                    parameters[10].Value = this.txtTblSpace.Text;              //表空间
                    result = CommonCLS.RunOracleNonQuerySp("PD_Game_Admin.PD_GameInfo_Insert", parameters);
                    strLogResult = "新建游戏 ,游戏名:" + this.txtGameNM.Text.Trim();
                
                }
                else
                { 
                    //修改
                    //strSQL = " UPDATE GAME_INFO SET ";     
                    //strSQL = strSQL + " GAME_NAME= '" +this.txtGameNM.Text.Trim() + "',";               //游戏名
                    //if (this.chkStatus.Checked == true)
                    //{
                    //    strSQL = strSQL + " FLAG=1,";   //采集标志
                    //}
                    //else
                    //{
                    //    strSQL = strSQL + " FLAG=0,";   //采集标志
                    //}
                    //int iGameDB = this.cbxDbType.SelectedIndex + 1;
                    //strSQL = strSQL + " ORA_TNS='" + this.txtTNS.Text.Trim() + "',";                    //TNS  
                    //strSQL = strSQL + " COLLECT_ID='"+this.txtCollactUser.Text.Trim() + "',";           //采集用户  
                    //strSQL = strSQL + " COLLECT_PWD='" +this.txtCollactPwd.Text.Trim() + "',";          //采集密码
                    //strSQL = strSQL + " MANAGE_ID='"  +this.txtManageUser.Text.Trim() + "',";           //管理用户
                    //strSQL = strSQL + " MANAGE_PWD='" +this.txtManagePwd.Text.Trim() + "',";            //管理密码
                    //strSQL = strSQL + " SPACE_MODULUS=" +this.txtSPACE_MODULUS.Text.Trim() + ",";       //缓存系数
                    //strSQL = strSQL + " TYPE_DB=" + iGameDB + ",";              //数据库类型
                    //strSQL = strSQL + " SQL_COLLECT='" + this.txtCount.Text + "',";              //数据库类型
                    //strSQL = strSQL + " TABLE_SPACE_NAME='" + this.cbxTbSpace.Text + "'";              //表空间

                    //strSQL = strSQL + " WHERE GAME_ID=" + this._sID ;
                    OracleParameter[] parameters = 
                    {
                        new OracleParameter("v_GameName", OracleDbType.Varchar2),
                        new OracleParameter("v_flg", OracleDbType.Int32),
                        new OracleParameter("v_TNS", OracleDbType.Varchar2),
                        new OracleParameter("v_CollectUser", OracleDbType.Varchar2),
                        new OracleParameter("v_CollectPwd", OracleDbType.Varchar2),
                        new OracleParameter("v_ManageUser", OracleDbType.Varchar2),
                        new OracleParameter("v_ManagerPwd", OracleDbType.Varchar2),
                        new OracleParameter("v_SpaceModule", OracleDbType.Int32),
                        new OracleParameter("v_dbtype", OracleDbType.Int32),
                        new OracleParameter("v_sqlCnt", OracleDbType.Varchar2),
                        new OracleParameter("v_tableSpace", OracleDbType.Varchar2),
                        new OracleParameter("v_GameID", OracleDbType.Int32),
                        new OracleParameter("v_result", OracleDbType.Decimal)
                    };
                    
                    parameters[0].Direction = ParameterDirection.Input;
                    parameters[1].Direction = ParameterDirection.Input;
                    parameters[2].Direction = ParameterDirection.Input;
                    parameters[3].Direction = ParameterDirection.Input;
                    parameters[4].Direction = ParameterDirection.Input;
                    parameters[5].Direction = ParameterDirection.Input;
                    parameters[6].Direction = ParameterDirection.Input;
                    parameters[7].Direction = ParameterDirection.Input;
                    parameters[8].Direction = ParameterDirection.Input;
                    parameters[9].Direction = ParameterDirection.Input;
                    parameters[10].Direction = ParameterDirection.Input;
                    parameters[11].Direction = ParameterDirection.Input;
                    parameters[12].Direction = ParameterDirection.Output;

                    parameters[0].Value = this.txtGameNM.Text.Trim();

                    if (this.chkStatus.Checked == true)         //游戏名
                    {
                        parameters[1].Value = 1;    //采集标志
                    }
                    else
                    {
                        parameters[1].Value = 0;   //采集标志
                    }
                    if (this.txtTNS.Text.Trim() != string.Empty)
                    {
                        parameters[2].Value = this.txtTNS.Text.Trim();            //TNS  
                    }
                    else
                    {
                        parameters[2].Value = "empty";                      //TNS  
                    }
                    parameters[3].Value = this.txtCollactUser.Text.Trim();    //采集用户
                    parameters[4].Value = this.txtCollactPwd.Text.Trim();     //采集密码
                    parameters[5].Value = this.txtManageUser.Text.Trim();     //管理用户
                    parameters[6].Value = this.txtManagePwd.Text.Trim();      //管理密码
                    parameters[7].Value = this.txtSPACE_MODULUS.Text.Trim();  //缓存系数
                    parameters[8].Value = this.cbxDbType.SelectedIndex + 1;   //数据库类型
                    parameters[9].Value ="not use";                 //SQL_COUNT
                    //parameters[10].Value = this.cbxTbSpace.Text;              //表空间
                    parameters[10].Value = this.txtTblSpace.Text;              //表空间
                    parameters[11].Value = this._sID;              
                    result = CommonCLS.RunOracleNonQuerySp("PD_Game_Admin.PD_GameInfo_Update", parameters);
                    strLogResult = "新建游戏 ,游戏id:" + this._sID + "游戏名:" + this.txtGameNM.Text.Trim();
                }

                if (result > 0)
                {
                    //记录操作log
                    CommonCLS.WriteOperatelog(strLogResult);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (System.Exception ex)
            { 
                CommonCLS.SaveLog("SaveData->" + ex.Message);
                return false;
            }
        }
        #endregion

        #region  取得表空间列表
        private void GetTableSpaceList()
        {
            string strSql = string.Empty;
            try
            {
                strSql = "select TABLESPACE_NAME  from   dba_data_files";
                DataSet dsTbSpace = CommonCLS.RunOracleByManager(strSql);
                if (dsTbSpace != null && dsTbSpace.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsTbSpace.Tables[0].Rows.Count; i++)
                    {
                        this.cbxTbSpace.Items.Add(dsTbSpace.Tables[0].Rows[i].ItemArray[0].ToString());
                    }
                }
            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog("GetTableSpaceList->" + ex.Message);
            }
        }
        #endregion
        #endregion


        #region 事件

        private void cbxDbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ////this.txtTNS.Text=string.Empty;
            //if (this.cbxDbType.Text == "ORACLE")
            //{
            //    this.txtTNS.Visible = true;
            //    this.label8.Visible = true;
            //    this.txtTNS.Enabled = true;
            //}
            //else
            //{
            //    this.txtTNS.Visible = false;
            //    this.label8.Visible = false;
            //    this.txtTNS.Enabled = false;
            //}
        }
     

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (txtGameNM.Text == string.Empty)
            {
                MessageBox.Show("请输入游戏名！");
                txtGameNM.Focus();
                return;
            }

            if (txtCollactUser.Text == string.Empty)
            {
                MessageBox.Show("请输入目标库用户名！");
                txtCollactUser.Focus();
                return;
            }

            if (txtCollactPwd.Text == string.Empty)
            {
                MessageBox.Show("请输入目标库密码！");
                txtCollactPwd.Focus();
                return;
            }

            //if (txtManageUser.Text == string.Empty)
            //{
            //    MessageBox.Show("请输入目标库管理用户名！");
            //    txtManageUser.Focus();
            //    return;
            //}

            //if (txtManagePwd.Text == string.Empty)
            //{
            //    MessageBox.Show("请输入目标库管理密码！");
            //    txtManagePwd.Focus();
            //    return;
            //}

            if (txtSPACE_MODULUS.Text == string.Empty)
            {
                MessageBox.Show("请输入采集缓存系数！");
                txtSPACE_MODULUS.Focus();
                return;
            }

            if (cbxDbType.Text == string.Empty)
            {
                MessageBox.Show("请选择源数据库类型！");
                cbxDbType.Focus();
                return;
            }


            if (cbxDbType.SelectedIndex==1)
            {
                if (txtTNS.Text == string.Empty)
                {
                    MessageBox.Show("请输入TNS！");
                    txtTNS.Focus();
                    return;
                }
            }

            if (this.SaveData() == true)
            {
                MessageBox.Show("操作成功");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("操作失败");
            }
        }
        #endregion

        private void frmGame_InfoDetail_Load(object sender, EventArgs e)
        {
            try
            {
                //GetTableSpaceList();
                string strSql = string.Empty;
                if (this._mode == 0)
                {
                    //新规模式

                }
                else
                {
                    //修改模式
                    //Modify 2009/09/07 wyh Begin *********************************
                    //strSql = "SELECT * FROM GAME_INFO WHERE GAME_ID=" + this._sID;
                    //DataSet dsFRM = CommonCLS.RunOracle(strSql);

                    OracleParameter[] parameters = 
                    {
                        new OracleParameter("v_GameID", OracleDbType.Int32),
                        new OracleParameter("cur_Result", OracleDbType.RefCursor)
                    };
                    parameters[0].Direction = ParameterDirection.Input;
                    parameters[1].Direction = ParameterDirection.Output;

                    parameters[0].Value = this._sID;
                    DataSet dsFRM = CommonCLS.RunOracleSP("PD_GameInfo_Pack.PD_GameInfo_Query", parameters);
                    //Modify 2009/09/07 End******************************************

                    
                    this.txtGameNM.Text = dsFRM.Tables[0].Rows[0].ItemArray[1].ToString();                 //游戏名
                    if (dsFRM.Tables[0].Rows[0].ItemArray[2].ToString() == "1")
                    {
                        this.chkStatus.Checked = true;                //采集标志
                    }
                    else
                    {
                        this.chkStatus.Checked = false;                 //采集标志
                    }
                    this.txtTNS.Text = dsFRM.Tables[0].Rows[0].ItemArray[3].ToString();               //TNS  
                    this.txtCollactUser.Text = dsFRM.Tables[0].Rows[0].ItemArray[4].ToString();      //采集用户  
                    this.txtCollactPwd.Text = dsFRM.Tables[0].Rows[0].ItemArray[5].ToString();       //采集密码
                    this.txtManageUser.Text = dsFRM.Tables[0].Rows[0].ItemArray[6].ToString();       //管理用户
                    this.txtManagePwd.Text = dsFRM.Tables[0].Rows[0].ItemArray[7].ToString();        //管理密码
                    this.txtSPACE_MODULUS.Text = dsFRM.Tables[0].Rows[0].ItemArray[8].ToString();     //缓存系数


                    this.cbxDbType.SelectedIndex = Convert.ToInt32(dsFRM.Tables[0].Rows[0].ItemArray[9])-1 ;         //数据库类型
                    //this.txtCount.Text = dsFRM.Tables[0].Rows[0].ItemArray[10].ToString();

                    //this.cbxTbSpace.Text = dsFRM.Tables[0].Rows[0].ItemArray[11].ToString();//表空间
                    this.txtTblSpace.Text = dsFRM.Tables[0].Rows[0].ItemArray[11].ToString();//表空间
                }
                //this.txtTNS.Text = string.Empty;

                //if (this.cbxDbType.Text == "ORACLE")
                //{
                //    this.txtTNS.Visible = true;
                //    this.txtTNS.Enabled = true;
                //}
                //else
                //{
                //    this.txtTNS.Visible = false;
                //    this.txtTNS.Enabled = false;
                //}
                this.cbxDbType.DropDownStyle = ComboBoxStyle.DropDownList;
                
            }
            catch (System.Exception ex)
            {
            }
        }

    }
}