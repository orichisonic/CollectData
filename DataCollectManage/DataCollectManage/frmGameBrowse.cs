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
    public partial class frmGameBrowse : Form
    {
        public frmGameBrowse()
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
        private DataSet dsGameAll = new DataSet();
        #endregion

        #region 方法
        #region 取得游戏列表
        /// <summary>
        /// 取得游戏列表
        /// </summary>
        private void GetData()
        {
            try 
            {

                //string strSQL = "SELECT distinct Ta.game_id as 游戏ID,Ta.game_name as 游戏名, decode(Ta.flag,1,'采集','停止采集')as 状态 From Game_Info Ta order by Ta.game_id";
                //dsGameAll = CommonCLS.RunOracle(strSQL);

                OracleParameter[] parameters = 
                {
                    new OracleParameter("cur_Result", OracleDbType.RefCursor)
                };
                parameters[0].Direction = ParameterDirection.Output;
                dsGameAll = CommonCLS.RunOracleSP("PD_GameInfo_Pack.PD_GameInfo_QueryALL", parameters);
                
                if (this.dsGameAll.Tables[0].Rows.Count > 0)
                {
                    this.dataGV.DataSource = dsGameAll.Tables[0];
                }
                else
                {
                    MessageBox.Show("暂无数据");
                }
               
            }
            catch (System.Exception ex) 
            {
                CommonCLS.SaveLog("GetData->"+ex.ToString());
            }
        }
        #endregion

        #region 删除游戏
        private bool DelCollectJob(string id)
        {
            try
            {
                string strSQL = "DELETE FROM game_info WHERE game_id="+ id;
                CommonCLS.RunOracleNonQuery(strSQL);

                    //strSQL = "DELETE FROM import_express WHERE gamedb_id=" + id;
                    //CommonCLS.RunOracleNonQuery(strSQL);

                    //strSQL = "DELETE FROM import_express WHERE gamedb_id=" + id;
                    //CommonCLS.RunOracleNonQuery(strSQL);
            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog("DelCollectJob->" + ex.ToString());
                return false;
            }
            return true;
        }
        #endregion

        #region 采集/停止采集
        private bool SetStatus(string   strStatus,string id)
        {

            try
            {
               if (strStatus=="采集")
               {
                   strStatus =" 0";
               }
               else
               {
                   strStatus = "1";
               }
                //string strSQL = "UPDATE game_info SET flag=" + strStatus + "WHERE game_id='" + id  + "'";

                OracleParameter[] parameters = 
                {
                    new OracleParameter("v_gameID", OracleDbType.Int32),
                     new OracleParameter("v_status", OracleDbType.Int32),
                    new OracleParameter("v_Result", OracleDbType.Decimal)
                };
                parameters[0].Direction = ParameterDirection.Input;
                parameters[1].Direction = ParameterDirection.Input;
                parameters[2].Direction = ParameterDirection.Output;

                parameters[0].Value = id;
                parameters[1].Value = strStatus;
                int result = CommonCLS.RunOracleNonQuerySp("PD_Game_Admin.PD_GameInfoFlag_Update", parameters);
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
        #endregion

        #region 事件

        #region 画面读入
        /// <summary>
        /// 画面读入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void frmGameBrowse_Load(object sender, EventArgs e)
        {
            this.GetData();
        }
        #endregion

        #region 添加新游戏采集任务按下
        /// <summary>
        /// 添加新游戏采集任务按下
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNew_Click(object sender, EventArgs e)
        {
            try
            {
                //Betweenness sysRv = new Betweenness();
                //frmGameEdit frmEdit = new frmGameEdit(0, "", "");
                //frmEdit.CreateModule(null, null, null);
                //if (sysRv.RESULT == BetweennessValue.SUCESS)
                //{
                //}

                if (this.dataGV.SelectedRows.Count > 0)
                { 
                    this.SetStatus(this.dataGV.SelectedRows[0].Cells[2].Value.ToString(),this.dataGV.SelectedRows[0].Cells[0].Value.ToString());
                    this.GetData();
                }
                else
                {
                    MessageBox.Show("请选择要操作的行");
                }
            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog("btnNew_Click->" + ex.ToString());
            }
        }
        #endregion

        private void btnDel_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("是否要删除?","", MessageBoxButtons.YesNo)== DialogResult.No)
                {
                    return;
                }

                if (this.dataGV.SelectedRows.Count > 0)
                { 
                    if (this.DelCollectJob(this.dataGV.SelectedRows[0].Cells[0].Value.ToString())==true  )
                    {
                        MessageBox.Show("删除成功");
                        this.GetData();
                    }
                }
                else
                {
                    MessageBox.Show("请选择要删除的行");
                }
            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog("btnDel_Click->" + ex.ToString());
            }
        }
        #endregion

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmGame_InfoDetail frmEdit = new frmGame_InfoDetail("",0);
            frmEdit.CreateModule(null, null, null);
            if (frmEdit.DialogResult == DialogResult.OK)
            {
                this.GetData();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (this.dataGV.SelectedRows.Count >= 0)
            {
                frmGame_InfoDetail frmEdit = new frmGame_InfoDetail(this.dataGV.SelectedRows[0].Cells[0].Value.ToString(),1);
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
    }
}