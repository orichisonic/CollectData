using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.IO;
using Oracle.DataAccess.Client;
namespace DataCollectMonitor
{
    public partial class frmDataCollect :Form
    {
        public frmDataCollect()
        {
            InitializeComponent();
        }
        public frmDataCollect(string strGameName)
        {
            this._strGameName=strGameName;      //游戏名

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
        private string _strGameName=string.Empty;       //游戏名
        private int _iGameID;    //游戏ID
        #endregion

        #region 事件
        #region 画面LOAD
        /// <summary>
        /// 画面LOAD 
        /// </summary>
        /// <returns></returns>
        private void frmDataCollect_Load(object sender, EventArgs e)
        {
            try
            {
                //设定标题栏文字
                this.Text=this._strGameName;

                //日期空间初始化
                this.dteFrom.Value = DateTime.Now;
                this.dteTo.Value = DateTime.Now;

                //读取log数据
                Thread tShowData = new Thread(GetMonitorData);
                tShowData.Start();
            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog(ex.ToString());
            }
        }
        #endregion
        /// <summary>
        /// 手动采集按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                //判断
                string strRowData = string.Empty;
                strRowData = this.dataGV.Rows[0].Cells[2].Value.ToString() + this.dataGV.Rows[0].Cells[3].Value.ToString()
                                       + this.dataGV.Rows[0].Cells[4].Value.ToString() + this.dataGV.Rows[0].Cells[5].Value.ToString();

                for (int i = 0; i < this.dataGV.Rows.Count; i++)
                {
                    if (this.dataGV.Rows[i].Cells["chkDoCollect"].Value.ToString() == "1")
                    { 
                        strRowData = this.dataGV.Rows[i].Cells[2].Value.ToString() + this.dataGV.Rows[i].Cells[3].Value.ToString()
                            + this.dataGV.Rows[i].Cells[4].Value.ToString() + this.dataGV.Rows[i].Cells[5].Value.ToString();
                        for (int j = 0; j < this.dataGV.Rows.Count; j++)
                        {
                            if (i != j && this.dataGV.Rows[j].Cells["chkDoCollect"].Value.ToString() == "1")
                            {
                                if (strRowData == this.dataGV.Rows[j].Cells[2].Value.ToString() + this.dataGV.Rows[j].Cells[3].Value.ToString()
                                        + this.dataGV.Rows[j].Cells[4].Value.ToString() + this.dataGV.Rows[j].Cells[5].Value.ToString())
                                {
                                    MessageBox.Show("已选择的手动采集对象中,有重复的采集对象 大区:" + this.dataGV.Rows[j].Cells[2].Value.ToString() + "IP:" + this.dataGV.Rows[j].Cells[3].Value.ToString()
                                                     + "采集源库:" + this.dataGV.Rows[j].Cells[4].Value.ToString() + "采集源表:" + this.dataGV.Rows[j].Cells[5].Value.ToString()+",请修改！");
                                    return;
                                }
                            }
                        }
                    }


                }
                //开启线程，开始采集
                ReadDate();
                //Thread RunCollect = new Thread(ReadDate);
                //RunCollect.Start();

               
            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog(ex.ToString());
            }
        }

       
        #endregion

        #region 方法

        #region 取得数据采集结果
        /// <summary>
        /// 取得数据采集结果
        /// </summary>
        /// <returns></returns>
        private void GetMonitorData()
        {
            DataSet dsGV = null;
            try
            {
                Invoke(new EventHandler(delegate(object sender, EventArgs e)
                                        {
                                            CommonCLS.WriteStatusText(this.ParentForm, "正在读入数据..");
                                        }
                                )
                );

                //string strID = "SELECT game_id FROM Game_Info WHERE game_name='" + this._strGameName + "'";

                //DataSet dsGid = CommonCLS.RunOracle(strID);
                //this._iGameID = Convert.ToInt32(dsGid.Tables[0].Rows[0].ItemArray[0].ToString());
                //取得游戏id
                OracleParameter[] parameters = 
                    {
                        new OracleParameter("v_GameName", OracleDbType.Varchar2),
                        new OracleParameter("v_result", OracleDbType.RefCursor)
                    };
                parameters[0].Direction = ParameterDirection.Input;
                parameters[1].Direction = ParameterDirection.Output;

                parameters[0].Value = this._strGameName;
                DataSet dsGid = CommonCLS.RunOracleSP("PD_GameInfo_Pack.PD_Game_id_QueryAll", parameters);
                this._iGameID = Convert.ToInt32(dsGid.Tables[0].Rows[0].ItemArray[0].ToString());

                //取得expresslog
                //string strSq = " SELECT 0 AS chkbox,TA.id,TA.server_name,TA.server_ip ,TA.server_db,TA.srctable,TB.log_date,TB.log_source,TB.log_result,decode(TB.log_detail,'','尚未采集',TB.log_detail) as log_detail,TB.id as log_id FROM ";
                //strSq = strSq + "  gamedb_info TA LEFT JOIN express_log TB ON TA.game_id =TB.log_game and TA.server_db =TB.log_db and TA.SERVER_IP =TB.LOG_SERVER ";
                //strSq = strSq + "  WHERE TA.game_id=" + this._iGameID ;
                //strSq = strSq + "  AND TB.log_date >='" + this.dteFrom.Value.ToString("yyyy/MM/dd").Replace("-", "/") +"' "
                //                + "  AND TB.log_date <='" + this.dteTo.Value.ToString("yyyy/MM/dd").Replace("-", "/") + "' "
                //                + "  Order by server_name , TA.server_db,TB.log_date ";
                //dsGV = CommonCLS.RunOracle(strSq);

                OracleParameter[] parameters2 = 
                    {
                        new OracleParameter("v_GameID", OracleDbType.Int32, ParameterDirection.Input),
                        new OracleParameter("v_dteFrom", OracleDbType.Date, ParameterDirection.Input),
                        new OracleParameter("v_dteTo", OracleDbType.Date, ParameterDirection.Input),
                        new OracleParameter("v_result", OracleDbType.RefCursor, ParameterDirection.Output)
                    };
                parameters2[0].Value = this._iGameID;
                parameters2[1].Value = this.dteFrom.Value;
                parameters2[2].Value = this.dteTo.Value;
                dsGV = CommonCLS.RunOracleSP("PD_GameInfo_Pack.PD_EXPRESSLOG_Query", parameters2);
                this._iGameID = Convert.ToInt32(dsGid.Tables[0].Rows[0].ItemArray[0].ToString());

                Invoke(new EventHandler(delegate(object sender, EventArgs e)
                                   {
                                       if (dsGV != null)
                                       {
                                           if (dsGV.Tables.Count > 0)
                                           {
                                               if (dsGV.Tables[0].Rows.Count > 0)
                                               {
                                                   this.dataGV.DataSource = dsGV.Tables[0];
                                                   //this.dataGV.Columns[1].Visible = false;
                                                   for (int iRow = 0; iRow < this.dataGV.Rows.Count; iRow++)
                                                   {
                                                       ////非当前日期的纪录无法采集
                                                       //if (dataGV.Rows[iRow].Cells[6].Value.ToString() != DateTime.Now.ToString("yyyy/MM/dd").Replace("-", "/"))
                                                       //{
                                                       //    this.dataGV.Rows[iRow].Cells[0].ReadOnly = true;
                                                       //    this.dataGV.Rows[iRow].Cells[0].Style.BackColor = this.BackColor;
                                                       //    continue;
                                                       //}
                                                       //else if (dataGV.Rows[iRow].Cells[6].Value.ToString() == DateTime.Now.ToString("yyyy/MM/dd").Replace("-", "/"))
                                                       //{
                                                       //    this.dataGV.Rows[iRow].Cells[0].ReadOnly = false;
                                                       //    this.dataGV.Rows[iRow].Cells[0].Style.BackColor = Color.White;
                                                           
                                                       //}

                                                       //采集成功的数据无法手动采集
                                                       if (dataGV.Rows[iRow].Cells[7].Value.ToString() == dataGV.Rows[iRow].Cells[8].Value.ToString())
                                                       {
                                                           this.dataGV.Rows[iRow].Cells["chkDoCollect"].ReadOnly = true;
                                                           this.dataGV.Rows[iRow].Cells["chkDoCollect"].Style.BackColor = this.BackColor;
                                                      
                                                       }
                                                       else if (dataGV.Rows[iRow].Cells[7].Value.ToString() != dataGV.Rows[iRow].Cells[8].Value.ToString())
                                                       {
                                                           this.dataGV.Rows[iRow].Cells["chkDoCollect"].ReadOnly = false;
                                                           this.dataGV.Rows[iRow].Cells["chkDoCollect"].Style.BackColor = Color.White;
                                                           
                                                       }
                                                   }
                                                   CommonCLS.WriteStatusText(this.ParentForm, "");
                                               }
                                               else
                                               {
                                                   CommonCLS.WriteStatusText(this.ParentForm, "暂无数据");
                                               }
                                           }
                                           else
                                           {
                                               CommonCLS.WriteStatusText(this.ParentForm, "暂无数据");
                                           }
                                       }
                                       else
                                       {
                                           CommonCLS.WriteStatusText(this.ParentForm, "暂无数据");
                                       }
                                   }
                                )
                             );

            }
            catch (System.Exception ex)
            {
                CommonCLS.SaveLog(ex.ToString());
               // return null;
              
            }
            //return dsGV;
        }
        #endregion

        public  void ReadDate()
        {
            string strBCP = string.Empty;       //BCP语句
            string strBcpPatch = string.Empty;  //BCP路径
            string strCtlPatch = string.Empty;  //CTL路径
            int log_source =0; //源记录数
            int log_result=0;//录入记录数
            string error = string.Empty;
            int log_ExistNum = 0;//目的表中以存在的记录数
            string sql = string.Empty;
            int log_ExistNumAfter = 0;//目的表中导入后的记录数
            int iHasChecked = 0;//时候有行选中
            string strBcpFileNM;
            FileInfo fiTmp = null;
            try
            {
                for (int j = 0; j < this.dataGV.Rows.Count; j++)
                {
                    if (this.dataGV.Rows[j].Cells["chkDoCollect"].Value.ToString() == "1")
                    {
                        iHasChecked = 1;
                        break;
                    }
                }
                if (iHasChecked == 0)
                {
                    MessageBox.Show("请选择要手动采集的对象");
                    return;
                }
                for (int i = 0; i < this.dataGV.Rows.Count; i++)
                {
                    if (this.dataGV.Rows[i].Cells["chkDoCollect"].Value.ToString() == "1")
                    {

                        //建立游戏名字目录
                        if (System.IO.Directory.Exists(Application.StartupPath + @"\" + this.Text) == false)
                        {
                            System.IO.Directory.CreateDirectory(Application.StartupPath + @"\" + this.Text);
                        }

                        //读取BCP 
                        OracleParameter[] parameters = 
                        {
                            new OracleParameter("v_ExpressID", OracleDbType.Int32),
                            new OracleParameter("cur_Result", OracleDbType.RefCursor)
                        };
                        parameters[0].Direction = ParameterDirection.Input;
                        parameters[1].Direction = ParameterDirection.Output;

                        parameters[0].Value = this.dataGV.Rows[i].Cells["express_id"].Value.ToString();
                        DataSet dsBCP = CommonCLS.RunOracleSP("PD_GameInfo_Pack.PD_BCP_Query", parameters);


                        if (dsBCP != null && dsBCP.Tables[0].Rows.Count > 0)
                        {
                            //目的表中以存在的记录数
                            sql = " SELECT count(*) FROM " + dsBCP.Tables[0].Rows[0].ItemArray[4].ToString().Trim()
                                    + " Where TO_CHAR(SCN_TO_TIMESTAMP(ora_rowscn),'YYYY/MM/DD')='" + Convert.ToDateTime(this.dataGV.Rows[i].Cells["CollectTime"].Value.ToString()).ToShortDateString().Replace("-", "/")+"'";
                            DataSet dslog_ExistNum = CommonCLS.RunOracle(sql);
                            log_ExistNum = Convert.ToInt32(dslog_ExistNum.Tables[0].Rows[0].ItemArray[0]);
                            //执行BCP 
                            try
                            {
                                strBcpFileNM = this.dataGV.Rows[i].Cells["server_Name"].Value.ToString() + this.dataGV.Rows[i].Cells["log_table"].Value.ToString() + ".txt";
                                fiTmp = new FileInfo(strBcpFileNM);
                                //判断采集时间是否当天的日期
                                string strBcpTmp = string.Empty;
                                strBcpTmp = dsBCP.Tables[0].Rows[0].ItemArray[1].ToString().ToUpper().Replace("'%S' AS SERVERIP , '%S' AS SERVERDB",
                                            "'" + this.dataGV.Rows[i].Cells["ip"].Value.ToString() + "' AS ServerIP,'" + this.dataGV.Rows[i].Cells["log_table"].Value.ToString() + "' AS ServerDB");
                                if (this.dataGV.Rows[i].Cells[6].Value.ToString() != CommonCLS.GetSysDate().ToString("yyyy/MM/dd").Replace("-", "/"))
                                {

                                    //不是当天日期则替换bcp sql中的getdate或now函数
                                    if (strBcpTmp.ToUpper().Contains("GETDATE()") == true || strBcpTmp.ToUpper().Contains("NOW()") == true)
                                    {
                                        strBcpTmp = strBcpTmp.Replace("GETDATE()", "CONVERT(DATETIME,'" + this.dataGV.Rows[i].Cells[6].Value.ToString() + "')");
                                    }
                                    else
                                    {
                                        strBcpTmp = strBcpTmp.Replace("NOW()", "CONVERT(DATETIME,'" + this.dataGV.Rows[i].Cells[6].Value.ToString() + "')");
                                    }
                                }
                                //删除采集时间的错误数据
                                DelFailedData(Convert.ToDateTime(this.dataGV.Rows[i].Cells["CollectTime"].Value.ToString()).ToShortDateString().Replace("-", "/"), dsBCP.Tables[0].Rows[0].ItemArray[4].ToString().Trim());
                                strBCP = " BCP \"" + strBcpTmp + " \" queryout " + Application.StartupPath + @"\" + this.Text + @"\" + fiTmp.Name
                                           + " -c -S\"" + this.dataGV.Rows[i].Cells["ip"].Value.ToString() + "\""
                                           + " -U\"" + this.dataGV.Rows[i].Cells["User_Id"].Value.ToString() + "\""
                                           + " -P\"" + this.dataGV.Rows[i].Cells["User_Pwd"].Value.ToString() + "\"";
                                strBcpPatch = dsBCP.Tables[0].Rows[0].ItemArray[0].ToString().Trim();
                                Invoke(new EventHandler(delegate(object sender, EventArgs e)
                                           {
                                               this.lblLoading.Visible = true;
                                               this.lblLoading.Text = "执行BCP," +
                                                                        "IP:" + this.dataGV.Rows[i].Cells[3].Value.ToString() +
                                                                        ".大区:" + this.dataGV.Rows[i].Cells[2].Value.ToString() +
                                                                        ".库:" + this.dataGV.Rows[i].Cells[4].Value.ToString() +
                                                                        ".表:" + this.dataGV.Rows[i].Cells[5].Value.ToString() +
                                                                        ".采集数据中..";
                                               this.btnOk.Enabled = false;
                                               this.btrRefresh.Enabled = false;
                                               this.dataGV.Enabled = false;
                                           }
                                   )
                                    );
                                ExeCommand(strBCP);//执行bcp命令并显示操作结果

                                //源记录数
                                strBCP = strBCP.ToUpper();
                                string[] line = File.ReadAllLines(Application.StartupPath + @"\" + this.Text + @"\" + fiTmp.Name);
                                log_source = line.Length;
                            }
                            catch (System.Exception ex)
                            {
                                error = "执行BCP，下载数据失败";
                            }
                            try
                            {
                                //执行CTL
                                Invoke(new EventHandler(delegate(object sender, EventArgs e)
                                          {
                                              this.lblLoading.Text = "执行CTL,保存采集数据中,共有" + log_source + "条数据,处理时间较长.请稍候..";
                                          }
                                  )
                                   );
                                //strCtlPatch = System.Text.Encoding.Default.GetString((byte[])dsBCP.Tables[0].Rows[0].ItemArray[4]);
                                //strCtlPatch = strCtlPatch.Replace(strCtlPatch.Substring(strCtlPatch.IndexOf("INFILE") + 7, strCtlPatch.IndexOf(".txt") - 13), "'" + Application.StartupPath + @"\" + this.Text + @"\" + fiTmp.Name + "'");
                                strCtlPatch=this.BuildCTL(this.dataGV.Rows[i].Cells["server_Name"].Value.ToString() + this.dataGV.Rows[i].Cells["log_table"].Value.ToString()+".ctl",
                                                "Append", dsBCP.Tables[0].Rows[0].ItemArray[7].ToString(), dsBCP.Tables[0].Rows[0].ItemArray[4].ToString());

                                FileStream fs = new FileStream(Application.StartupPath + @"\" + this.Text + @"\" + this.dataGV.Rows[i].Cells["server_Name"].Value.ToString() + this.dataGV.Rows[i].Cells["log_table"].Value.ToString() + ".ctl", FileMode.Append, FileAccess.Write);
                                byte[] str = System.Text.Encoding.Default.GetBytes(strCtlPatch);
                                fs.Write(str, 0, str.Length);
                                fs.Close();
                                string sCtlNm = strBcpFileNM = this.dataGV.Rows[i].Cells["server_Name"].Value.ToString() + this.dataGV.Rows[i].Cells["log_table"].Value.ToString() + ".CTL";
                                ExeCommand("sqlldr " + CommonCLS.UserName  + "/" + CommonCLS.PASSWORD  + "@" + CommonCLS.TNS + " control=" + Application.StartupPath + @"\" + this.Text + @"\" + sCtlNm);//执行bcp命令并显示操作结果

                                System.IO.File.Delete(Application.StartupPath + @"\" + this.Text + @"\" + fiTmp.Name);
                            }
                            catch (System.Exception ex2)
                            {
                                error = "执行CTL,导入数据失败";
                            }

                            //读取写入记录数

                            //执行CTL
                            Invoke(new EventHandler(delegate(object sender, EventArgs e)
                                      {
                                          this.lblLoading.Text = "采集结束！..";
                                          this.lblLoading.Visible = false;
                                          this.btnOk.Enabled = true;
                                          this.btrRefresh.Enabled = true;
                                          this.dataGV.Enabled = true;
                                      }
                              )
                               );

                            sql = " SELECT count(*) FROM " + dsBCP.Tables[0].Rows[0].ItemArray[3].ToString().Trim();
                            DataSet ExistNumAfter = CommonCLS.RunOracle(sql);
                            log_ExistNumAfter = Convert.ToInt32(ExistNumAfter.Tables[0].Rows[0].ItemArray[0]);
                            //sql = " SELECT count(*) FROM " + dsBCP.Tables[0].Rows[0].ItemArray[3].ToString().Trim();
                            //DataSet dsCountResult = CommonCLS.RunOracle(sql);
                            //log_result = Convert.ToInt32(dsCountResult.Tables[0].Rows[0].ItemArray[0]);
                            log_result = log_ExistNumAfter - log_ExistNum;
                            string log_detail = "";
                            if (error == string.Empty)
                            {
                                log_detail = "[" + this.dataGV.Rows[i].Cells[3].Value.ToString() + "]执行[" + this._strGameName + "--" + this.dataGV.Rows[i].Cells[4].Value.ToString() + "]成功，数据源[" + log_source + "]条记录，保存[" + log_result + "]条记录";
                            }
                            else
                            {
                                log_detail = "[" + this.dataGV.Rows[i].Cells[3].Value.ToString() + "]执行[" + this._strGameName + "--" + this.dataGV.Rows[i].Cells[4].Value.ToString() + "]失败，失败原因：[" + error + "]";

                            }
                            //写操作LOG
                            OracleParameter[] parametersOpLog = 
                        {
                            new OracleParameter("v_LogSource", OracleDbType.Varchar2),
                            new OracleParameter("v_LogResult", OracleDbType.Varchar2),
                            new OracleParameter("v_Detail", OracleDbType.Varchar2),
                            new OracleParameter("v_id", OracleDbType.Int32),
                            new OracleParameter("v_Result", OracleDbType.Decimal)
                        };
                            parametersOpLog[0].Direction = ParameterDirection.Input;
                            parametersOpLog[1].Direction = ParameterDirection.Input;
                            parametersOpLog[2].Direction = ParameterDirection.Input;
                            parametersOpLog[3].Direction = ParameterDirection.Input;
                            parameters[4].Direction = ParameterDirection.Output;

                            parametersOpLog[0].Value = log_source;
                            parametersOpLog[1].Value = log_result;
                            parametersOpLog[2].Value = log_detail;
                            parametersOpLog[3].Value = this.dataGV.Rows[i].Cells[10].Value.ToString();
                            int resultOpLog = CommonCLS.RunOracleNonQuerySp("PD_Game_Admin.PD_ExpressLOG_Update", parametersOpLog);



                            //sql = " UPDATE EXPRESS_LOG SET ";
                            ////sql = sql + " LOG_DATE=(select to_char(sysdate,'YYYY/MM/DD')  from dual), ";
                            //sql = sql + " LOG_SOURCE=" + log_source +", ";
                            //sql = sql + " LOG_RESULT=" + log_result + ", ";
                            //sql = sql + " LOG_DETAIL='" + log_detail + "' ";
                            //sql = sql + " WHERE id=" + this.dataGV.Rows[i].Cells[10].Value.ToString();
                            //CommonCLS.RunOracleNonQuery(sql);

                        }
                    }
                }

                Invoke(new EventHandler(delegate(object sender, EventArgs e)
                                  {
                                      this.GetMonitorData();
                                      MessageBox.Show("执行完毕");
                                  }
                          )
                  );
            }
            catch (System.Exception ex)
            {
            }
        }
        
        /**/
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
            finally
            {
                p.Close();
                p.Dispose();
            }
            return strOutput;
        }
        #endregion

        #region 刷新按钮
        private void btrRefresh_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            Thread tShowData = new Thread(GetMonitorData);
            tShowData.Start();
            this.Enabled = true;
        }
        #endregion

        #region DataGrid排序
        private void dataGV_Sorted(object sender, EventArgs e)
        {
            for (int iRow = 0; iRow < this.dataGV.Rows.Count; iRow++)
            {
                ////非当前日期的纪录无法采集
                //if (dataGV.Rows[iRow].Cells[6].Value.ToString() != DateTime.Now.ToString("yyyy/MM/dd").Replace("-", "/"))
                //{
                //    this.dataGV.Rows[iRow].Cells[0].ReadOnly = true;
                //    this.dataGV.Rows[iRow].Cells[0].Style.BackColor = this.BackColor;
                //    continue;
                //}
                //else if (dataGV.Rows[iRow].Cells[6].Value.ToString() == DateTime.Now.ToString("yyyy/MM/dd").Replace("-", "/"))
                //{
                //    this.dataGV.Rows[iRow].Cells[0].ReadOnly = false;
                //    this.dataGV.Rows[iRow].Cells[0].Style.BackColor = Color.White;

                //}

                //采集成功的数据无法手动采集
                if (dataGV.Rows[iRow].Cells[7].Value.ToString() == dataGV.Rows[iRow].Cells[8].Value.ToString())
                {
                    this.dataGV.Rows[iRow].Cells["chkDoCollect"].ReadOnly = true;
                    this.dataGV.Rows[iRow].Cells["chkDoCollect"].Style.BackColor = this.BackColor;

                }
                else if (dataGV.Rows[iRow].Cells[7].Value.ToString() != dataGV.Rows[iRow].Cells[8].Value.ToString())
                {
                    this.dataGV.Rows[iRow].Cells["chkDoCollect"].ReadOnly = false;
                    this.dataGV.Rows[iRow].Cells["chkDoCollect"].Style.BackColor = Color.White;

                }
            }
        }
        #endregion

        #region 生成CTL语句
        private string BuildCTL(string strBcpName, string strMODE,string strDbRow,string strDestable)
        {
            try
            {
                string strCTL = string.Empty;
                string strField = string.Empty;
                string filelist = null;
                string rowlist = null;
                string[] strAll = strDbRow.Split(',');      //字段结构
                for (int iField = 0; iField < strAll.Length; iField++)
                {
                    string[] strRow = strAll[iField].Split(' ');
                    if (iField == 0)
                    {
                        if (strRow[1] == "DATE")
                        {
                            strField = strField + strRow[0].ToString() + " DATE \"YYYY-MM-DD HH24:MI:SS\"";
                        }
                        else
                        {
                            strField = strField + strRow[0] + " " + strRow[1];
                        }
                    }
                    else
                    {
                        if (strRow[1]== "DATE")
                        {
                            strField = strField + "," + strRow[0] + " DATE \"YYYY-MM-DD HH24:MI:SS\"";
                        }
                        else
                        {
                            strField = strField + "," + strRow[0]+" " + strRow[1];
                        }
                    }
                }

                filelist = "INFILE '" + Application.StartupPath + @"\" + this.Text + @"\" + strBcpName.Replace(",", "'\r\nINFILE '") + "'\r\n";
               //filelist = " Replace_Part ";
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

        #region 清除按钮
        private void btnClear_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认要清空选中采集任务的目的表(将删除目的表中,从今日起7天以前的数据)?", "", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                return;
            }
            for (int i = 0; i < this.dataGV.Rows.Count; i++)
            {
                if (this.dataGV.Rows[i].Cells["chkDoCollect"].Value.ToString() == "1")
                {
                    OracleParameter[] parameters2 = 
                    {
                        new OracleParameter("v_ID", OracleDbType.Int32, ParameterDirection.Input),
                        new OracleParameter("v_result", OracleDbType.RefCursor, ParameterDirection.Output)
                    };
                    parameters2[0].Value = this.dataGV.Rows[i].Cells["express_id"].Value;
                    DataSet dsDel = CommonCLS.RunOracleSP("PD_GameInfo_Pack.PD_ImportExpress_Query", parameters2);

                    if (dsDel != null && dsDel.Tables[0].Rows.Count > 0)
                    {
                        string strDestable = dsDel.Tables[0].Rows[0].ItemArray[9].ToString();
                        DateTime dteCollectTime=Convert.ToDateTime(this.dataGV.Rows[i].Cells["CollectTime"].Value);
                        string sWhere=" Where to_char(scn_to_timestamp(ORA_ROWSCN),'yyyy-mm-dd hh24:mi:ss')<='" + dteCollectTime.ToString("yyyy-MM-dd hh:mm:ss") + "' AND "
                                        + " to_char(scn_to_timestamp(ORA_ROWSCN),'yyyy-mm-dd hh24:mi:ss')>='"+ dteCollectTime.AddDays(-7).ToString("yyyy-MM-dd hh:mm:ss") + "' ";

                        string strsql = "Delete From " + strDestable + sWhere;
                        CommonCLS.RunOracleNonQuery(strsql);

                    }
                }
                else
                {
                    MessageBox.Show("请选择要操作的行");
                    return;
                }
            }
        }
        #endregion

        #region 根据时间删除采集失败的数据
        public static void DelFailedData(string strDate,string strTblName)
        {
            try
            {
                string strSql = "Delete From " + strTblName + " Where TO_CHAR(SCN_TO_TIMESTAMP(ora_rowscn),'YYYY/MM/DD')='" + strDate + "'";

                CommonCLS.RunOracleNonQuery(strSql);
            }
            catch (System.Exception ex)
            {
            }
        }
        #endregion
    }
}