using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;
using Oracle.DataAccess.Client;
using System;

using Language;
namespace DataCollectMonitor
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }
        #region 变量

        #endregion

        #region 事件

        #region 画面LOAD
        /// <summary>
        /// 画面LOAD
        /// </summary>
        /// <returns></returns>
        private void frmLogin_Load(object sender, EventArgs e)
        {
          
            //画面初始化
            this.txtUser.Text = string.Empty;
            this.txtPwd.Text = string.Empty;
            this.txtUser.Focus();
            CommonCLS.BcpPath = CommonCLS.ReadINIValue(Application.StartupPath  + "\\ini\\Conf.INI", "PATH", "BcpPath");
            CommonCLS.CtlPath = CommonCLS.ReadINIValue(Application.StartupPath + "\\ini\\Conf.INI", "PATH", "CtlPath");

            CommonCLS.TNS = CommonCLS.ReadINIValue(Application.StartupPath + "\\ini\\Conf.INI", "SERVER", "TNS");
            CommonCLS.UserName = CommonCLS.ReadINIValue(Application.StartupPath + "\\ini\\Conf.INI", "SERVER", "DBUser");
            CommonCLS.PASSWORD = CommonCLS.ReadINIValue(Application.StartupPath + "\\ini\\Conf.INI", "SERVER", "DBPwd");

            CommonCLS.MangerUserName = CommonCLS.ReadINIValue(Application.StartupPath + "\\ini\\Conf.INI", "SERVER", "DBMangerUser");
            CommonCLS.MangerPASSWORD = CommonCLS.ReadINIValue(Application.StartupPath + "\\ini\\Conf.INI", "SERVER", "DBMangerPwd");

            //string strsql;
            //strsql = " select 'ip' as txtip,  Server_IP from gamedb_info where Game_id=1 ";
            //DataSet dste = CommonCLS.RunOracle(strsql);
            //CommonCLS.CreateTBL("ServerIP 50 1,ServerDB 50 0,famid 20 0,fampoint 20 0,mastersn 20 0,masternick 20 0,houseid 20 0,shortintro 100 0,longintro 200 0,secret 10 0,rank 20 0,membercount 10 0,createdate DATE 1,wincount 10 0,losecount 10 0",
            //                    "faminfo",
            //                    1,
            //                    dste.Tables[0], "COLLECTDATA");

        }
        #endregion

        #region 画面关闭按钮
        /// <summary>
        /// 画面关闭按钮
        /// </summary>
        /// <returns></returns>
        private void btnCancle_Click(object sender, EventArgs e)
        {
            Process.GetCurrentProcess().Kill();
        }
        #endregion

        #region 登录按钮
        /// <summary>
        /// 登录按钮
        /// </summary>
        /// <returns></returns>
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (blnChkInput() == false)
                {
                    return;
                }
                int iLoginResult = this.CheckLogin(this.txtUser.Text, this.txtPwd.Text);
                if (iLoginResult == 0)
                {              
                    //登录成功,显示数据采集画面
                    CommonCLS.LoginUser = this.txtUser.Text.Trim();
                    this.DialogResult = DialogResult.OK; 
                    this.Close(); 
                    //Main frmMenu = new Main();
                    //this.Hide();
                    //Thread.Sleep(1000);
                    //frmMenu.Show();
                }
                else if (iLoginResult == 1)
                {
                    //登录失败
                    MessageBox.Show("登录失败，请检查用户名或密码！");
                    this.DialogResult = DialogResult.None; 
                    this.txtPwd.Text = string.Empty;
                    this.txtUser.Focus();
                    this.txtUser.SelectAll();
                }
                else if (iLoginResult == 2)
                {
                    //登录失败
                    MessageBox.Show("该帐号已停止使用！");
                    this.DialogResult = DialogResult.None;
                    this.txtPwd.Text = string.Empty;
                    this.txtUser.Focus();
                    this.txtUser.SelectAll();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("服务器连接失败！");
                CommonCLS.SaveLog(ex.ToString());
            }
        }
        #endregion

        #endregion

        #region 方法

        #region 检查用户名和密码
        /// <summary>
        ///  检查用户名和密码
        /// </summary>
        ///  /// <param name="_strUserNm">账号</param>
        /// <param name="_strPwd">密码</param>
        /// <returns></returns>
        private int CheckLogin(string _strUserNm,string _strPwd)
        {
            try
            {
                //Modify 2009/09/07 wyh Begin *********************************
                this.ShowStatusControl();
                string strSql = "SELECT userid ,pwd,id,status FROM User_Info";
                strSql = strSql + " WHERE userid= '" + _strUserNm.Trim() + "'" + " AND  pwd='" + _strPwd.Trim() + "'"; //登录检查SQL语句

                this.Cursor = Cursors.WaitCursor;
                DataSet dsLogin = CommonCLS.RunOracle(strSql);                    //查询数据库


                
                //OracleParameter[] parameters = 
                //{
                //    new OracleParameter("v_userid", OracleDbType.Varchar2),
                //    new OracleParameter("v_pwd", OracleDbType.Varchar2),
                //    new OracleParameter("cur_Result", OracleDbType.RefCursor)
                //};
                //parameters[0].Direction = ParameterDirection.Input;
                //parameters[1].Direction = ParameterDirection.Input;
                //parameters[2].Direction = ParameterDirection.Output;

                //parameters[0].Value = _strUserNm.Trim();
                //parameters[1].Value = _strPwd.Trim();
                //DataSet dsLogin = CommonCLS.RunOracleSP("PD_GameInfo_Pack.PD_CheckLogin_QueryAll", parameters);
                //Modify 2009/09/07 End******************************************

                this.HideStatus();
                if (dsLogin != null && dsLogin.Tables[0].Rows.Count > 0)
                {

                    if (dsLogin.Tables[0].Rows[0].ItemArray[3].ToString() == "0")
                    {
                        return 2;        //帐号停止使用
                    }


                    if (dsLogin.Tables[0].Rows[0].ItemArray[0].ToString() == _strUserNm.Trim() && dsLogin.Tables[0].Rows[0].ItemArray[1].ToString() == _strPwd.Trim())
                    {
                        CommonCLS.LoginUserID = dsLogin.Tables[0].Rows[0].ItemArray[2].ToString();
     
                        return 0;        //登录成功
                        
                    }
                    else
                    {
                        return 1;       //登录失败
                    }
                }
                else
                {
                    return 1;
                }
            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog(ex.ToString());
                return 1;
            }
            finally
            {

                this.Cursor = Cursors.Default;
            }
        }
        #endregion

        #region 检查画面输入
        /// <summary>
        ///  检查画面输入
        /// </summary>
        /// <returns></returns>
        private bool blnChkInput()
        { 
            //用户名是否输入
            if (this.txtUser.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入用户名");
                this.DialogResult = DialogResult.None; 
                this.txtUser.Focus();
                return false;
            }

            //密码是否输入
            if (this.txtPwd.Text.Trim() == string.Empty)
            {
                MessageBox.Show("请输入密码");
                this.DialogResult = DialogResult.None; 
                this.txtPwd.Focus();
                return false;
            }
            return true;
        }
        #endregion

        #region 显示指定区域

        private void ShowStatusControl()
        {
            Application.DoEvents();
            this.lblStatusText.Text = "正在登陆..";
            this.Size = new Size(311, 163);
            dpStatus.Visible = true;
            this.Invalidate();
            Application.DoEvents();
        }
        #endregion

        #region 隐藏指定区域
        private void HideStatus()
        {
            this.Size = new Size(311, 131);
            dpStatus.Visible = false;
            this.Invalidate();
        }
        #endregion

        #region 执行Cmd命令
        /// <summary>
        /// 执行Cmd命令
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public static string ExeCommand(string commandText)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            string strOutput = null;
            try
            {
                p.Start();
                p.StandardInput.WriteLine(commandText);
                p.StandardInput.WriteLine("exit");
                strOutput = p.StandardOutput.ReadToEnd();
                p.WaitForExit();
                p.Close();
            }
            catch (Exception e)
            {
                strOutput = e.Message;
            }
            return strOutput;
        }
        #endregion

        private void RunSqlldr()
        { 
            string BcpExec = "";
                    BcpExec = @"sqlldr gmtools/gmtools123@test control=D:\";
                    BcpExec += "user.ctl";
                    //BcpExec += " -S\"222.73.118.13\" -U\"Nyreport\" -P\"PgQf7QxUfG\" -c -t,";//组合bcp命令
                    ExeCommand(BcpExec);//执行bcp命令并显示操作结果
        }
        #endregion

    }
}