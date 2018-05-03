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
    public partial class frmImort_ExpressBrowse : Form
    {
        public frmImort_ExpressBrowse()
        {
            InitializeComponent();
        }

        #region 变量
        private DataSet dsFrm;
        #endregion

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

        #region 方法
        #region 取得任务列表
        /// <summary>
        ///取得任务列表
        /// </summary>
        private void GetData()
        {
            try
            {
                //string strSQL = "select ta.id, tb.game_name as 游戏 ,ta.server_name as 大区名 ,ta.server_db 数据库名 ,ta.srctable 源数据表名 ,ta.destable 目的数据表名 from gamedb_info ta, game_info tb where ta.game_id=tb.game_id";
                //dsFrm = CommonCLS.RunOracle(strSQL);

                OracleParameter[] parameters = 
                    {
                        new OracleParameter("cur_Result", OracleDbType.RefCursor)
                    };
                parameters[0].Direction = ParameterDirection.Output;

                dsFrm = CommonCLS.RunOracleSP("PD_GameInfo_Pack.PD_ImportList_QueryAll", parameters);

                if (this.dsFrm.Tables[0].Rows.Count > 0)
                {
                    this.dataGV.DataSource = dsFrm.Tables[0];
                    this.dataGV.Columns[0].Visible = false;
                }
                else
                {
                    MessageBox.Show("暂无数据");
                    this.dataGV.DataSource = null;
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
        #region 添加按钮按下
        /// <summary>
        /// 添加按钮按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                Betweenness sysRv = new Betweenness();
                frmGameEdit frmEdit = new frmGameEdit(0, "", "");
                frmEdit.CreateModule(null, null, null);
                if (frmEdit.DialogResult == DialogResult.OK)
                {
                    this.GetData();
                }
                //if (sysRv.RESULT == BetweennessValue.SUCESS)
                //{
                //}

            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog("btnNew_Click->" + ex.ToString());
            }
        }
        #endregion

        #region 画面读入
        private void frmImort_ExpressBrowse_Load(object sender, EventArgs e)
        {
            this.GetData();
        }
        #endregion

        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dataGV.SelectedRows.Count >= 0)
                {
                    Betweenness sysRv = new Betweenness();
                    frmGameEdit frmEdit = new frmGameEdit(1, this.dataGV.SelectedRows[0].Cells[1].Value.ToString(), this.dataGV.SelectedRows[0].Cells[0].Value.ToString());
                    frmEdit.CreateModule(null, null, null);
                    if (frmEdit.DialogResult == DialogResult.OK)
                    {
                        this.GetData();
                    }
                }
                else
                {
                    MessageBox.Show("请选择要操作的数据行"); 
                }
            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog("btnNew_Click->" + ex.ToString());
            }
        }
        private void txtImport_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dataGV.SelectedRows.Count >= 0)
                {
                    Betweenness sysRv = new Betweenness();
                    frmImport import = new frmImport();
                    import.CreateModule(null, null, null);
                    if (import.DialogResult == DialogResult.OK)
                    {
                        this.GetData();
                    }
                }
                else
                {
                    MessageBox.Show("请选择要操作的数据行");
                }
            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog("btnNew_Click->" + ex.ToString());
            }
        }
        #endregion

        #region 删除按钮
        private void btnDel_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("确定删除?", "", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

             if (this.dataGV.SelectedRows.Count >= 0)
                {
                    Betweenness sysRv = new Betweenness();
                    frmGameEdit frmEdit = new frmGameEdit(1, this.dataGV.SelectedRows[0].Cells[1].Value.ToString(), this.dataGV.SelectedRows[0].Cells[0].Value.ToString());

                    if (CommonCLS.DelData(this.dataGV.SelectedRows[0].Cells[0].Value.ToString()) > 0)
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
                    MessageBox.Show("请选择要操作的数据行"); 
                }
        }
        #endregion

        private void btnSwitch_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dataGV.SelectedRows.Count > 0)
                {
                    this.SetStatus(this.dataGV.SelectedRows[0].Cells[5].Value.ToString(), this.dataGV.SelectedRows[0].Cells[0].Value.ToString());
                    this.GetData();
                }
                else
                {
                    MessageBox.Show("请选择要操作的行");
                }
            }
            catch (System.Exception ex)
            {
            }
        }
        #region 采集/停止采集
        private bool SetStatus(string strStatus, string id)
        {

            try
            {
                if (strStatus == "采集")
                {
                    strStatus = " 0";
                }
                else
                {
                    strStatus = "1";
                }
                //string strSQL = "UPDATE game_info SET flag=" + strStatus + "WHERE game_id='" + id  + "'";

                OracleParameter[] parameters = 
                {
                    new OracleParameter("v_id", OracleDbType.Int32),
                     new OracleParameter("v_status", OracleDbType.Int32),
                    new OracleParameter("v_Result", OracleDbType.Decimal)
                };
                parameters[0].Direction = ParameterDirection.Input;
                parameters[1].Direction = ParameterDirection.Input;
                parameters[2].Direction = ParameterDirection.Output;

                parameters[0].Value = id;
                parameters[1].Value = strStatus;
                int result = CommonCLS.RunOracleNonQuerySp("PD_Game_Admin.PD_ImportSwitch_Update", parameters);
                if (result == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog("SetStatus->" + ex.ToString());
                return false;
            }
        }
        #endregion
      
    }
}