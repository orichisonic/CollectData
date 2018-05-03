using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Diagnostics;
using System.Threading;

using C_Event;
using C_Global;
using C_Socket;
using Language;

using System.IO;
namespace DataCollectMonitor
{
    public partial class Main : Form
    {

       public Main()
        {
            InitializeComponent();
        }

        #region 变量
        string currSelectNodeTag = null;    //tree select node tag
        string lastSelectNodeTage = null;
        bool leftOpen = true;   //左边listview是否打开
        #endregion

        #region 方法

        #region 获取游戏菜单
        /// <summary>
        ///  获取游戏菜单
        /// </summary>
        /// <returns></returns>
        private DataSet LoadGameMenu()
        {
            try
            {
                string strSql = "select t.* from game_info t";
                DataSet dsGame = CommonCLS.RunOracle(strSql);                    //查询数据库
                return dsGame;
            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog(ex.ToString());
                return null;
            }
        }
        #endregion

        #region 建立菜单
        /// <summary>
        ///  建立菜单
        /// </summary>
        /// <returns></returns>
        private void BuildMenu()
        {
            try
            {
                //建立监控惨淡
                TrvMenu.Nodes.Add("游戏数据采集监控");
                DataSet dsGName = this.LoadGameMenu();
                if (dsGName != null)
                {
                    for (int i = 0; i < dsGName.Tables[0].Rows.Count; i++)
                    {
                        TrvMenu.Nodes[0].Nodes.Add(dsGName.Tables[0].Rows[i].ItemArray[1].ToString());

                    }
                }

                //系统设置菜单
                TrvMenu.Nodes.Add("系统设置");
                TrvMenu.Nodes[1].Nodes.Add("游戏浏览");
                TrvMenu.Nodes[1].Nodes.Add("采集任务浏览");
                if (CommonCLS.LoginUser.ToUpper().Trim() == "ADMIN")
                {
                    TrvMenu.Nodes[1].Nodes.Add("用户浏览");
                }
                else
                {
                    TrvMenu.Nodes[1].Nodes.Add("密码修改");
                }
            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog(ex.ToString());
            }
        }
        #endregion

        #region 获取画面名
        private string GetFormName(string strNode)
        {
                switch (strNode)
                {
                    case "游戏浏览":
                        {
                            return "frmGameBrowse";
                        }
                    case "用户设置":
                        {
                            return "frmUser";
                        }
                    case "游戏数据采集监控":
                        {
                            return "";
                        }
                    case "系统设置":
                        {
                            return "";
                        }
                    case "采集任务浏览":
                        {
                            return "frmImort_ExpressBrowse";
                        }
                    case "用户浏览":
                        {
                            return "frmUserBrowse";
                        }
                    case "密码修改":
                        {
                            return "frmUserEdit.cs";
                        }
                    case "":
                        {
                            return "";
                        }
                    default:
                        {
                            return "frmDataCollect";
                        }
                }
        }
        

        #endregion

        private void MdiChild_Closed(object sender, EventArgs e)
        {
            if (!(sender is System.Windows.Forms.Form))
                return;
            // remove tab
            this.tabControl_mdichild.TabPages.Remove((TabPage)((System.Windows.Forms.Form)sender).Tag);
            if (this.tabControl_mdichild.TabPages.Count == 0)
                //hide tab control
                this.panel_mdi_tab.Visible = false;
        }
        private void MdiChild_TextChanged(object sender, EventArgs e)
        {
            if (!(sender is System.Windows.Forms.Form))
                return;
            System.Windows.Forms.Form frm = (System.Windows.Forms.Form)sender;
            if (!(frm.Tag is TabPage))
                return;
            TabPage tp = (TabPage)frm.Tag;
            tp.Text = frm.Text;
        }

        private void tabControl_set_close_image_state(bool b_active)
        {
            if (this.tabControl_mdichild.SelectedTab == null)
                return;
            // if active state
            if (b_active)
            {
                if (this.tabControl_mdichild.SelectedTab.ImageIndex != 1)
                    this.tabControl_mdichild.SelectedTab.ImageIndex = 1;
            }
            // else
            else if (this.tabControl_mdichild.SelectedTab.ImageIndex != 0)
                this.tabControl_mdichild.SelectedTab.ImageIndex = 0;
        }
        #endregion

        #region 事件
        #region 画面读入
        /// <summary>
        /// 画面读入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_Load(object sender, EventArgs e)
        {
            //建立菜单
            this.BuildMenu();
        }
        #endregion

        #region 点击树状菜单
        /// <summary>
        /// 点击树状菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TrvMenu_Click(object sender, EventArgs e)
        {
            while (this.check_if_tab_exist(currSelectNodeTag))
            {
                return;
            }
            if (lastSelectNodeTage == currSelectNodeTag)
            {
                if (currSelectNodeTag != null)
                {
                    switch (this.GetFormName(currSelectNodeTag))
                    {
                        case "frmGameBrowse":
                            {
                                frmGameBrowse frmBse = new frmGameBrowse();
                                frmBse.CreateModule(this, null, null);
                                break;
                            }
                        case "frmUser":
                            {
                                break;
                            }
                        case "frmImort_ExpressBrowse":
                            {
                                frmImort_ExpressBrowse frmJOB = new frmImort_ExpressBrowse();
                                frmJOB.CreateModule(this, null, null);
                                break;
                            }
                        case "frmDataCollect":
                            {
                                frmDataCollect frmClt = new frmDataCollect(currSelectNodeTag);
                                frmClt.CreateModule(this, null, null);
                                break;
                            }
                        case "frmUserBrowse":
                            {
                                frmUserBrowse frmUserBse = new frmUserBrowse();
                                frmUserBse.CreateModule(this, null, null);
                                break;
                            }
                        case "frmUserEdit.cs":
                            {
                                frmUserEdit frmUserEdt = new frmUserEdit(CommonCLS.LoginUserID, "1");
                                frmUserEdt.CreateModule(null, null, null);
                                break;
                            }
                    }
                }
            }
            else
            {
                lastSelectNodeTage = currSelectNodeTag;
            }
        }
        #endregion

        #region 树状菜单选择后
        /// <summary>
        /// 树状菜单选择后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TrvMenu_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                while (this.check_if_tab_exist(e.Node.Text))
                {
                    return;
                }
                currSelectNodeTag = Convert.ToString(e.Node.Text);

                if (lastSelectNodeTage == null)
                {
                    lastSelectNodeTage = e.Node.Text;
                }

                if (e.Node.Text != null)
                {
                    switch (this.GetFormName(e.Node.Text))
                    {
                        case "frmGameBrowse":
                            {
                                frmGameBrowse frmBse = new frmGameBrowse();
                                frmBse.CreateModule(this, null, null);
                                break;
                            }
                        case "frmUser":
                            {
                                break;
                            }
                        case "frmImort_ExpressBrowse":
                            {
                                frmImort_ExpressBrowse frmJOB= new frmImort_ExpressBrowse();
                                frmJOB.CreateModule(this, null, null);
                                break;
                            }
                        case "frmDataCollect":
                            {
                                frmDataCollect frmClt = new frmDataCollect(currSelectNodeTag);
                                frmClt.CreateModule(this, null, null);
                                break;
                            }
                        case "frmUserBrowse":
                            {
                                frmUserBrowse frmUserBse = new frmUserBrowse();
                                frmUserBse.CreateModule(this, null, null);
                                break;
                            }
                        case "frmUserEdit.cs":
                            {
                                frmUserEdit frmUserEdt = new  frmUserEdit(CommonCLS.LoginUserID,"1");
                                frmUserEdt.CreateModule(null, null, null);
                                break;
                            }
                    }
                }
                if (e.Node.Tag != null)
                {

                    //this.lblStatus.Text = "正在打开窗体\"" + e.Node.Text.ToString() + "\",请稍等...";
                }

            }
            catch(System.Exception ex)
            {
            }

        }
        #endregion

        #region 画面关闭
        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        #endregion

        private void Main_MdiChildActivate(object sender, EventArgs e)
        {

            System.Windows.Forms.Form frm = this.MdiChildren[this.MdiChildren.Length - 1];
            // System.Windows.Forms.Form frm =new frmddd();
            // if Mdiform creation
            if (frm.Tag == null)// add_Tab
            {
                frm.WindowState = FormWindowState.Maximized;
                // search an unique caption for the form and the tabpage
                string frm_caption = frm.Text;
                int cpt = 2;
                while (this.check_if_tab_exist(frm_caption))
                {
                    //frm_caption = frm.Text + " (" + cpt + ")";
                    //cpt++;
                    return;
                }
                frm.Text = frm_caption;
                TabPage tp = new TabPage(frm.Text);
                // add close image on the tabpage
                tp.ImageIndex = 0;// unselected state
                frm.Tag = tp;// associate mdi child window and tabpage

                // add tab page
                this.tabControl_mdichild.TabPages.Add(tp);
                // add event handler on the mdi child
                frm.Closed += new EventHandler(MdiChild_Closed);
                frm.TextChanged += new EventHandler(MdiChild_TextChanged);
                // check if it is the first mdichild
                if (this.MdiChildren.Length == 1)
                {
                    // show tabcontrol
                    this.panel_mdi_tab.Visible = true;
                }
                this.tabControl_mdichild.SelectedTab = tp;
            }
            else // activate corresponding tabpage
            {
                if (this.ActiveMdiChild == null)
                    return;
                frm = this.ActiveMdiChild;
                if (!(frm.Tag is TabPage))
                    return;
                // unactivate old tabpage image selection
                this.tabControl_set_close_image_state(false);
                // activate associated tabpage
                TabPage tp = (TabPage)frm.Tag;
                this.tabControl_mdichild.SelectedTab = tp;
            }
            //this.lblStatus.Text = "完毕";
        }
        #endregion

        private void picOpenClose_Click(object sender, EventArgs e)
        {
            if (this.leftOpen)
            {
                leftOpen = false;
                this.picOpenClose.Image = global::DataCollectMonitor.Properties.Resources.left_right;
            }
            else
            {
                leftOpen = true;
                this.picOpenClose.Image = global::DataCollectMonitor.Properties.Resources.left_right2;
            }
            this.Invalidate();
        }

        private void tabControl_mdichild_Click(object sender, EventArgs e)
        {
            TabPage tp = this.tabControl_mdichild.SelectedTab;
            System.Windows.Forms.Form mdic = this.tabControl_get_associated_form(tp);
            if (mdic == null)
                return;
            mdic.Activate();
        }

        private void tabControl_mdichild_MouseLeave(object sender, EventArgs e)
        {
            this.tabControl_set_close_image_state(false);
        }

        private void tabControl_mdichild_MouseMove(object sender, MouseEventArgs e)
        {
            this.tabControl_set_close_image_state(this.tabControl_is_mouse_on_close_image(e));
        }

      
        private void tabControl_mdichild_MouseUp(object sender, MouseEventArgs e)
        {
            if (!this.tabControl_is_mouse_on_close_image(e))
                return;
            System.Windows.Forms.Form mdic = this.tabControl_get_associated_form(this.tabControl_mdichild.SelectedTab);
            if (mdic == null)
                return;
            mdic.Close();
        }

        /// <summary>
        /// get the mdi child form associated with the TabPage
        /// </summary>
        /// <param name="tp"></param>
        /// <returns></returns>
        private System.Windows.Forms.Form tabControl_get_associated_form(TabPage tp)
        {
            for (int cpt = 0; cpt < this.MdiChildren.Length; cpt++)
            {
                if ((TabPage)this.MdiChildren[cpt].Tag == tp)
                {
                    return this.MdiChildren[cpt];
                }
            }
            return null;
        }
        private bool tabControl_is_mouse_on_close_image(System.Windows.Forms.MouseEventArgs e)
        {
            // position of image in tab control
            byte image_left = 6;
            byte image_top = 1;
            // check if mouse is other the image
            if (
                (e.X < image_left + this.imageList_tabpage_icons.ImageSize.Width + this.tabControl_mdichild.GetTabRect(this.tabControl_mdichild.SelectedIndex).X)
                && (e.X > image_left + this.tabControl_mdichild.GetTabRect(this.tabControl_mdichild.SelectedIndex).X)
                && (e.Y < image_top + this.imageList_tabpage_icons.ImageSize.Height + this.tabControl_mdichild.GetTabRect(this.tabControl_mdichild.SelectedIndex).Y)
                && (e.Y > image_top + this.tabControl_mdichild.GetTabRect(this.tabControl_mdichild.SelectedIndex).Y)
                )
                return true;
            else
                return false;
        }
        /// <summary>
        /// check if caption already exist for another tab/mdiChildForm
        /// </summary>
        /// <param name="caption"></param>
        /// <returns></returns>
        private bool check_if_tab_exist(string caption)
        {
            for (int cpt = 0; cpt < this.tabControl_mdichild.TabPages.Count; cpt++)
                if (this.tabControl_mdichild.TabPages[cpt].Text == caption)
                    return true;
            return false;
        }

    }

}