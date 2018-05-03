namespace DataCollectMonitor
{
    partial class frmDataCollect
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnOk = new System.Windows.Forms.Button();
            this.btrRefresh = new System.Windows.Forms.Button();
            this.dataGV = new System.Windows.Forms.DataGridView();
            this.chkDoCollect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.server_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ip = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.log_db = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.log_table = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CollectTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.log_source = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.log_result = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Desc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.log_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.express_id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.User_Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.User_Pwd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblLoading = new System.Windows.Forms.Label();
            this.dteFrom = new System.Windows.Forms.DateTimePicker();
            this.dteTo = new System.Windows.Forms.DateTimePicker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGV)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(12, 9);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(83, 23);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "手动采集";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btrRefresh
            // 
            this.btrRefresh.Location = new System.Drawing.Point(274, 10);
            this.btrRefresh.Name = "btrRefresh";
            this.btrRefresh.Size = new System.Drawing.Size(83, 23);
            this.btrRefresh.TabIndex = 3;
            this.btrRefresh.Text = "刷新";
            this.btrRefresh.UseVisualStyleBackColor = true;
            this.btrRefresh.Click += new System.EventHandler(this.btrRefresh_Click);
            // 
            // dataGV
            // 
            this.dataGV.AllowUserToAddRows = false;
            this.dataGV.AllowUserToDeleteRows = false;
            this.dataGV.AllowUserToOrderColumns = true;
            this.dataGV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGV.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.dataGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.chkDoCollect,
            this.id,
            this.server_Name,
            this.ip,
            this.log_db,
            this.log_table,
            this.CollectTime,
            this.log_source,
            this.log_result,
            this.Desc,
            this.log_id,
            this.express_id,
            this.User_Id,
            this.User_Pwd});
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGV.DefaultCellStyle = dataGridViewCellStyle11;
            this.dataGV.Location = new System.Drawing.Point(0, 39);
            this.dataGV.MultiSelect = false;
            this.dataGV.Name = "dataGV";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGV.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.dataGV.RowTemplate.Height = 23;
            this.dataGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGV.Size = new System.Drawing.Size(1215, 684);
            this.dataGV.TabIndex = 4;
            this.dataGV.Sorted += new System.EventHandler(this.dataGV_Sorted);
            // 
            // chkDoCollect
            // 
            this.chkDoCollect.DataPropertyName = "chkbox";
            this.chkDoCollect.FillWeight = 60.9137F;
            this.chkDoCollect.HeaderText = "";
            this.chkDoCollect.Name = "chkDoCollect";
            this.chkDoCollect.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.chkDoCollect.Width = 40;
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            this.id.HeaderText = "id";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Visible = false;
            // 
            // server_Name
            // 
            this.server_Name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.server_Name.DataPropertyName = "server_name";
            this.server_Name.HeaderText = "区域";
            this.server_Name.Name = "server_Name";
            this.server_Name.ReadOnly = true;
            this.server_Name.Width = 51;
            // 
            // ip
            // 
            this.ip.DataPropertyName = "Log_Server";
            this.ip.HeaderText = "服务器IP";
            this.ip.Name = "ip";
            this.ip.Width = 150;
            // 
            // log_db
            // 
            this.log_db.DataPropertyName = "log_db";
            this.log_db.HeaderText = "采集源库";
            this.log_db.Name = "log_db";
            this.log_db.ReadOnly = true;
            // 
            // log_table
            // 
            this.log_table.DataPropertyName = "Log_Table";
            this.log_table.HeaderText = "采集表";
            this.log_table.Name = "log_table";
            this.log_table.ReadOnly = true;
            // 
            // CollectTime
            // 
            this.CollectTime.DataPropertyName = "log_date";
            this.CollectTime.FillWeight = 113.0288F;
            this.CollectTime.HeaderText = "采集时间";
            this.CollectTime.Name = "CollectTime";
            this.CollectTime.ReadOnly = true;
            this.CollectTime.Width = 150;
            // 
            // log_source
            // 
            this.log_source.DataPropertyName = "log_source";
            this.log_source.HeaderText = "源记录数";
            this.log_source.Name = "log_source";
            this.log_source.ReadOnly = true;
            this.log_source.Visible = false;
            // 
            // log_result
            // 
            this.log_result.DataPropertyName = "log_result";
            this.log_result.HeaderText = "已保存记录数";
            this.log_result.Name = "log_result";
            this.log_result.ReadOnly = true;
            this.log_result.Visible = false;
            this.log_result.Width = 120;
            // 
            // Desc
            // 
            this.Desc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Desc.DataPropertyName = "log_detail";
            this.Desc.FillWeight = 113.0288F;
            this.Desc.HeaderText = "采集情况描述";
            this.Desc.Name = "Desc";
            this.Desc.ReadOnly = true;
            // 
            // log_id
            // 
            this.log_id.DataPropertyName = "log_id";
            this.log_id.HeaderText = "记录id";
            this.log_id.Name = "log_id";
            this.log_id.ReadOnly = true;
            this.log_id.Visible = false;
            // 
            // express_id
            // 
            this.express_id.DataPropertyName = "Log_Express";
            this.express_id.HeaderText = "任务id";
            this.express_id.Name = "express_id";
            this.express_id.ReadOnly = true;
            this.express_id.Visible = false;
            // 
            // User_Id
            // 
            this.User_Id.DataPropertyName = "User_Id";
            this.User_Id.HeaderText = "User_Id";
            this.User_Id.Name = "User_Id";
            this.User_Id.ReadOnly = true;
            this.User_Id.Visible = false;
            // 
            // User_Pwd
            // 
            this.User_Pwd.DataPropertyName = "User_Pwd";
            this.User_Pwd.HeaderText = "User_Pwd";
            this.User_Pwd.Name = "User_Pwd";
            this.User_Pwd.ReadOnly = true;
            this.User_Pwd.Visible = false;
            // 
            // lblLoading
            // 
            this.lblLoading.Location = new System.Drawing.Point(362, 282);
            this.lblLoading.Name = "lblLoading";
            this.lblLoading.Size = new System.Drawing.Size(504, 117);
            this.lblLoading.TabIndex = 5;
            this.lblLoading.Text = "loading..";
            this.lblLoading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblLoading.Visible = false;
            // 
            // dteFrom
            // 
            this.dteFrom.Location = new System.Drawing.Point(3, 11);
            this.dteFrom.Name = "dteFrom";
            this.dteFrom.Size = new System.Drawing.Size(117, 21);
            this.dteFrom.TabIndex = 1;
            // 
            // dteTo
            // 
            this.dteTo.Location = new System.Drawing.Point(147, 11);
            this.dteTo.Name = "dteTo";
            this.dteTo.Size = new System.Drawing.Size(117, 21);
            this.dteTo.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btrRefresh);
            this.groupBox1.Controls.Add(this.dteFrom);
            this.groupBox1.Controls.Add(this.dteTo);
            this.groupBox1.Location = new System.Drawing.Point(190, -1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(361, 37);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(124, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "～";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(101, 9);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(83, 23);
            this.btnClear.TabIndex = 9;
            this.btnClear.Text = "清除目的表";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // frmDataCollect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1216, 735);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblLoading);
            this.Controls.Add(this.dataGV);
            this.Controls.Add(this.btnOk);
            this.Name = "frmDataCollect";
            this.Text = "游戏数据采集状态浏览";
            this.Load += new System.EventHandler(this.frmDataCollect_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGV)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btrRefresh;
        private System.Windows.Forms.DataGridView dataGV;
        private System.Windows.Forms.Label lblLoading;
        private System.Windows.Forms.DateTimePicker dteFrom;
        private System.Windows.Forms.DateTimePicker dteTo;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chkDoCollect;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn server_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn ip;
        private System.Windows.Forms.DataGridViewTextBoxColumn log_db;
        private System.Windows.Forms.DataGridViewTextBoxColumn log_table;
        private System.Windows.Forms.DataGridViewTextBoxColumn CollectTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn log_source;
        private System.Windows.Forms.DataGridViewTextBoxColumn log_result;
        private System.Windows.Forms.DataGridViewTextBoxColumn Desc;
        private System.Windows.Forms.DataGridViewTextBoxColumn log_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn express_id;
        private System.Windows.Forms.DataGridViewTextBoxColumn User_Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn User_Pwd;
        private System.Windows.Forms.Button btnClear;
    }
}