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
    public partial class frmUserBrowse : Form
    {
        public frmUserBrowse()
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

        #region 变量
        private DataSet dsUserAll = new DataSet();
        private int _moda = 0;
        #endregion

        #region 方法
        #region 取得游戏列表
        /// <summary>
        /// 取得用户列表
        /// </summary>
        private void GetData()
        {
            try
            {
                //string strSQL = "SELECT id,userid,decode(status,1,'使用','未使用') as status From user_info order by id";
                //dsUserAll = CommonCLS.RunOracle(strSQL);

                OracleParameter[] parameters = 
                {
                    new OracleParameter("cur_Result", OracleDbType.RefCursor)
                };
                parameters[0].Direction = ParameterDirection.Output;
                dsUserAll = CommonCLS.RunOracleSP("PD_GameInfo_Pack.PD_UserList_QueryAll", parameters);

                if (this.dsUserAll.Tables[0].Rows.Count > 0)
                {
                    this.dataGV.DataSource = dsUserAll.Tables[0];
                }
                else
                {
                    MessageBox.Show("暂无数据");
                }

            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog("GetData->" + ex.ToString());
            }
        }
        #endregion
        #endregion

        #region 事件
        /// <summary>
        /// 添加按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                frmUserEdit frmEdit = new frmUserEdit(this.dataGV.SelectedRows[0].Cells[0].Value.ToString(),"0");
                frmEdit.CreateModule(null, null, null);
                if (frmEdit.DialogResult == DialogResult.OK)
                {
                    this.GetData();
                }
            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog("btnAdd_Click->" + ex.ToString());
            }
        }

        /// <summary>
        /// 编辑按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dataGV.SelectedRows.Count <= 0)
                {
                    MessageBox.Show("请选择要操作的行");
                    return;
                }
                frmUserEdit frmEdit = new frmUserEdit(this.dataGV.SelectedRows[0].Cells[0].Value.ToString(), "1");
                frmEdit.CreateModule(null, null, null);
                if (frmEdit.DialogResult == DialogResult.OK)
                {
                    this.GetData();
                }
            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog("btnEdit_Click->" + ex.ToString());
            }
        }

        /// <summary>
        /// 删除按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, EventArgs e)
        {
            string strsql = string.Empty;
            try
            {
                if (this.dataGV.SelectedRows.Count <= 0)
                {
                    MessageBox.Show("请选择要操作的行");
                    return;
                }

                if (MessageBox.Show("确认删除用户？", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    if (this.dataGV.SelectedRows[0].Cells[0].Value.ToString() != "0")
                    {
                        strsql = "　DELETE FROM user_info Where id=" + this.dataGV.SelectedRows[0].Cells[0].Value.ToString();
                  
                        //OracleParameter[] parameters = 
                        //{
                        //    new OracleParameter("v_id", OracleDbType.Int32),
                        //    new OracleParameter("v_Result", OracleDbType.Decimal)
                        //};
                        //parameters[0].Direction = ParameterDirection.Input;
                        //parameters[1].Direction = ParameterDirection.Output;

                        //parameters[0].Value = this.dataGV.SelectedRows[0].Cells[0].Value.ToString();
                        //int result = CommonCLS.RunOracleNonQuerySp("PD_Game_Admin.PD_UserInfo_Del", parameters);

                        int result = CommonCLS.RunOracleNonQuery(strsql);
                        if (result ==1)
                        {
                            MessageBox.Show("删除成功");
                            this.GetData();
                        }
                        else
                        {
                            MessageBox.Show("删除失败");
                        }
                    }
                    else
                    {
                        MessageBox.Show("admin管理帐号无法删除！");
                    }
                }
            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog("btnDel_Click->" + ex.ToString());
            }
        }

        /// <summary>
        /// 画面读取
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmUserBrowse_Load(object sender, EventArgs e)
        {
            try
            {
                this.GetData();
            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog("frmUserBrowse_Load->" + ex.ToString());
            }
        }
        #endregion

      
    }
}