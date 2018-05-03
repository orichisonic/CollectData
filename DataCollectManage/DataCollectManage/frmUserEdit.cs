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
    public partial class frmUserEdit : Form
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

        #region 变量
        private string _mode = "0";//模式,0新规,1修改
        private string _iUid;
        private DataSet _dsUser;
        #endregion

        #region 构造函数
        public frmUserEdit()
        {
            InitializeComponent();
        }

        public frmUserEdit(string iUid, string sMode)
        {
            InitializeComponent();
            this._mode = sMode;
            this._iUid = iUid;
        }

        #endregion

        #region 事件
        #region 画面读入
        /// <summary>
        /// 画面读入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmUser_Load(object sender, EventArgs e)
        {
            try
            {
                
                this.GetData();
            }
            catch (System.Exception ex)
            {
            }
        }
        #endregion

        /// <summary>
        /// 确认按钮按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.ChkInput() == false)
                {
                    return;
                }
                else
                {
                    if (this.SaveData() == true)
                    {
                        MessageBox.Show("操作成功");
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("操作失败");
                        this.DialogResult = DialogResult.Cancel;
                    }
                }

            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog("GetData->" + ex.ToString());
            } 
        }
        #endregion

        #region 方法
        /// <summary>
        /// 取得用户数据
        /// </summary>
        private void GetData()
        {
            string strsql = string.Empty;
            try
            {
                //修改模式
                if (this._mode=="1")
                {
                    this.txtUserNM.Enabled = false;
                    if (CommonCLS.LoginUserID != "0")
                    {
                        this.chkStatus.Enabled = false;
                    }
                    //strsql = "SELECT * FROM user_info where id=" + this._iUid;
                    //this._dsUser = CommonCLS.RunOracle(strsql);
                    OracleParameter[] parameters = 
                    {
                        new OracleParameter("v_id", OracleDbType.Int32),
                        new OracleParameter("cur_Result", OracleDbType.RefCursor)
                    };
                    parameters[0].Direction = ParameterDirection.Input;
                    parameters[1].Direction = ParameterDirection.Output;

                    parameters[0].Value = this._iUid;

                    this._dsUser = CommonCLS.RunOracleSP("PD_GameInfo_Pack.PD_UserInfo_QueryAll", parameters);

                    if (_dsUser != null && _dsUser.Tables[0].Rows.Count > 0)
                    {
                        //用户名
                        this.txtUserNM.Text = _dsUser.Tables[0].Rows[0].ItemArray[1].ToString();
                        //密码
                        this.txtPwd.Text = _dsUser.Tables[0].Rows[0].ItemArray[2].ToString();
                        this.txtConfirmPwd.Text = _dsUser.Tables[0].Rows[0].ItemArray[2].ToString();
                        //状态
                        if (_dsUser.Tables[0].Rows[0].ItemArray[3].ToString()=="0")
                        {
                            this.chkStatus.Checked=false;
                        }
                        else
                        {
                            this.chkStatus.Checked=true;
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog("GetData->" + ex.ToString());
            } 
        }

        /// <summary>
        /// 检查画面输入内容
        /// </summary>
        private bool ChkInput()
        {
            try
            {
                if (this.txtUserNM.Text == string.Empty)
                {
                    MessageBox.Show("请输入用户名");
                    this.txtUserNM.Focus();
                    return false;
                }

                if (this.txtPwd.Text == string.Empty)
                {
                    MessageBox.Show("请输入用户密码");
                    this.txtPwd.Focus();
                    return false;
                }

                if (this.txtPwd.Text != this.txtConfirmPwd.Text )
                {
                    MessageBox.Show("两次输入的密码不一致，请重新输入");
                    this.txtPwd.Focus();
                    return false;
                }
                return true;
            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog("ChkInput->" + ex.ToString());
                return false;
            } 
        }
        /// <summary>
        /// 保存数据
        /// </summary>
        /// <returns></returns>
        private bool SaveData()
        {
            string strsql = string.Empty;
            string strLOG=string.Empty;
            try
            {
                int sStatus = 0;
                if (this.chkStatus.Checked == true)
                {
                    sStatus = 1;
                }
                else
                {
                    sStatus = 0;
                }

                if (this._mode == "0")
                {
                    strLOG="新建用户,用户名:"+txtUserNM;
                    //新规模式
                    //strsql = @"INSERT INTO　user_info VALUES(";
                    //strsql = strsql + @"USER_ID_SEQ.Nextval,'" + this.txtUserNM.Text.Trim() + "','" + this.txtPwd.Text.Trim() + "'," + sStatus + ")";

                    OracleParameter[] parameters = 
                    {
                        new OracleParameter("v_UserName", OracleDbType.Varchar2),
                        new OracleParameter("v_pwd", OracleDbType.Varchar2),
                        new OracleParameter("v_status", OracleDbType.Varchar2),
                        new OracleParameter("v_result", OracleDbType.Int32)
                    };
                    parameters[0].Direction = ParameterDirection.Input;
                    parameters[1].Direction = ParameterDirection.Input;
                    parameters[2].Direction = ParameterDirection.Input;
                    parameters[3].Direction = ParameterDirection.Output;

                    parameters[0].Value = this.txtUserNM.Text.Trim();
                    parameters[1].Value = this.txtPwd.Text.Trim();
                    parameters[2].Value = sStatus;
                    int result = CommonCLS.RunOracleNonQuerySp("PD_Game_Admin.PD_UserInfo_Insert", parameters);
                    //if (CommonCLS.RunOracleNonQuery(strsql)<=0)
                    if (result!=1)
                    {
                        return false;
                    }
                }
                else
                { 
                    //修改模式
                    //strLOG = "修改用户,用户名:"+this.txtUserNM.Text;
                    //strsql = @"UPDATE user_info SET ";
                    //strsql = strsql + @"pwd='" + this.txtPwd.Text.Trim() + "',status=" + sStatus + " Where id="+this._iUid;
                    OracleParameter[] parameters = 
                    {
                        new OracleParameter("v_UserID", OracleDbType.Int32),
                        new OracleParameter("v_status", OracleDbType.Varchar2),
                        new OracleParameter("v_pwd", OracleDbType.Varchar2),
                        new OracleParameter("v_result", OracleDbType.Int32)
                    };
                    parameters[0].Direction = ParameterDirection.Input;
                    parameters[1].Direction = ParameterDirection.Input;
                    parameters[2].Direction = ParameterDirection.Input;
                    parameters[3].Direction = ParameterDirection.Output;

                    parameters[0].Value = this._iUid;
                    parameters[1].Value = sStatus;
                    parameters[2].Value = this.txtPwd.Text.Trim();
                    int result = CommonCLS.RunOracleNonQuerySp("PD_Game_Admin.PD_UserInfo_Update", parameters);

                    //if (CommonCLS.RunOracleNonQuery(strsql) <= 0)
                    if (result != 1) 
                    {
                        return false;
                    }

                }
                //记录操作log
                CommonCLS.WriteOperatelog(strLOG);
                return true;
            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog("ChkInput->" + ex.ToString());
                return false;
            }
        }
        #endregion

    }
}