namespace DataCollectMonitor
{
    partial class frmGameEdit
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
            this.label1 = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.stbStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.label5 = new System.Windows.Forms.Label();
            this.txtTable = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtLength = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.dgTable = new System.Windows.Forms.DataGridView();
            this.btnRemove2 = new System.Windows.Forms.Button();
            this.btnAdd2 = new System.Windows.Forms.Button();
            this.txtField = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lblbcp = new System.Windows.Forms.Label();
            this.txtBcp = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancle = new System.Windows.Forms.Button();
            this.cbxGameName = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtDestable = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chkAll = new System.Windows.Forms.CheckBox();
            this.dgArea = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDB = new System.Windows.Forms.TextBox();
            this.cbxSwitch = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.rdb1 = new System.Windows.Forms.RadioButton();
            this.rdb2 = new System.Windows.Forms.RadioButton();
            this.txtTblSql = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.nudHour = new System.Windows.Forms.NumericUpDown();
            this.nudMinute = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label15 = new System.Windows.Forms.Label();
            this.nudDay = new System.Windows.Forms.NumericUpDown();
            this.dpStartTime = new System.Windows.Forms.DateTimePicker();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.fieldname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.length = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chkFLG = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.字段类型 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chkSelect = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtArea = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.areaip = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PORT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusStrip1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgTable)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgArea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHour)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinute)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDay)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "游戏名";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stbStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 851);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(681, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // stbStatus
            // 
            this.stbStatus.ForeColor = System.Drawing.Color.Red;
            this.stbStatus.Name = "stbStatus";
            this.stbStatus.Size = new System.Drawing.Size(0, 17);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 68);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "表字段结构";
            // 
            // txtTable
            // 
            this.txtTable.Location = new System.Drawing.Point(436, 28);
            this.txtTable.Name = "txtTable";
            this.txtTable.Size = new System.Drawing.Size(114, 21);
            this.txtTable.TabIndex = 3;
            this.txtTable.Leave += new System.EventHandler(this.txtTable_Leave);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(435, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 12;
            this.label6.Text = "源数据表名";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cmbType);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.txtLength);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.dgTable);
            this.groupBox2.Controls.Add(this.btnRemove2);
            this.groupBox2.Controls.Add(this.btnAdd2);
            this.groupBox2.Controls.Add(this.txtField);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(317, 55);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(359, 449);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "表结构";
            // 
            // cmbType
            // 
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Items.AddRange(new object[] {
            "VARCHAR2()",
            "CHAR()",
            "DATE",
            "NUMBER",
            "INTEGER",
            "FLOAT"});
            this.cmbType.Location = new System.Drawing.Point(119, 33);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(114, 20);
            this.cmbType.TabIndex = 18;
            this.cmbType.SelectedIndexChanged += new System.EventHandler(this.cmbType_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(122, 17);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 17;
            this.label8.Text = "字段类型";
            // 
            // txtLength
            // 
            this.txtLength.Location = new System.Drawing.Point(237, 32);
            this.txtLength.MaxLength = 15;
            this.txtLength.Name = "txtLength";
            this.txtLength.Size = new System.Drawing.Size(114, 21);
            this.txtLength.TabIndex = 6;
            this.txtLength.Leave += new System.EventHandler(this.txtLength_Leave);
            this.txtLength.Enter += new System.EventHandler(this.txtLength_Enter);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(239, 17);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 12);
            this.label11.TabIndex = 15;
            this.label11.Text = "字段长度";
            // 
            // dgTable
            // 
            this.dgTable.AllowUserToAddRows = false;
            this.dgTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.fieldname,
            this.length,
            this.chkFLG,
            this.字段类型});
            this.dgTable.Location = new System.Drawing.Point(5, 86);
            this.dgTable.MultiSelect = false;
            this.dgTable.Name = "dgTable";
            this.dgTable.RowHeadersVisible = false;
            this.dgTable.RowTemplate.Height = 23;
            this.dgTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgTable.Size = new System.Drawing.Size(348, 357);
            this.dgTable.TabIndex = 15;
            this.dgTable.TabStop = false;
            this.dgTable.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgTable_CellContentClick_1);
            // 
            // btnRemove2
            // 
            this.btnRemove2.Location = new System.Drawing.Point(300, 57);
            this.btnRemove2.Name = "btnRemove2";
            this.btnRemove2.Size = new System.Drawing.Size(53, 21);
            this.btnRemove2.TabIndex = 8;
            this.btnRemove2.Text = "去除";
            this.btnRemove2.UseVisualStyleBackColor = true;
            this.btnRemove2.Click += new System.EventHandler(this.btnRemove2_Click);
            // 
            // btnAdd2
            // 
            this.btnAdd2.Location = new System.Drawing.Point(241, 57);
            this.btnAdd2.Name = "btnAdd2";
            this.btnAdd2.Size = new System.Drawing.Size(53, 21);
            this.btnAdd2.TabIndex = 7;
            this.btnAdd2.Text = "添加";
            this.btnAdd2.UseVisualStyleBackColor = true;
            this.btnAdd2.Click += new System.EventHandler(this.btnAdd2_Click);
            // 
            // txtField
            // 
            this.txtField.Location = new System.Drawing.Point(4, 32);
            this.txtField.MaxLength = 500;
            this.txtField.Name = "txtField";
            this.txtField.Size = new System.Drawing.Size(113, 21);
            this.txtField.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 17);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 11;
            this.label7.Text = "字段名";
            // 
            // lblbcp
            // 
            this.lblbcp.AutoSize = true;
            this.lblbcp.Location = new System.Drawing.Point(10, 513);
            this.lblbcp.Name = "lblbcp";
            this.lblbcp.Size = new System.Drawing.Size(89, 12);
            this.lblbcp.TabIndex = 14;
            this.lblbcp.Text = "BCP SELECT语句";
            // 
            // txtBcp
            // 
            this.txtBcp.Location = new System.Drawing.Point(7, 528);
            this.txtBcp.Multiline = true;
            this.txtBcp.Name = "txtBcp";
            this.txtBcp.Size = new System.Drawing.Size(669, 56);
            this.txtBcp.TabIndex = 9;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOk.Location = new System.Drawing.Point(497, 821);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 21);
            this.btnOk.TabIndex = 10;
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancle
            // 
            this.btnCancle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancle.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancle.Location = new System.Drawing.Point(596, 821);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(80, 21);
            this.btnCancle.TabIndex = 11;
            this.btnCancle.Text = "取消";
            this.btnCancle.UseVisualStyleBackColor = true;
            // 
            // cbxGameName
            // 
            this.cbxGameName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxGameName.FormattingEnabled = true;
            this.cbxGameName.Location = new System.Drawing.Point(12, 24);
            this.cbxGameName.Name = "cbxGameName";
            this.cbxGameName.Size = new System.Drawing.Size(195, 20);
            this.cbxGameName.TabIndex = 0;
            this.cbxGameName.SelectedIndexChanged += new System.EventHandler(this.cbxGameName_SelectedIndexChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(551, 9);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(77, 12);
            this.label12.TabIndex = 18;
            this.label12.Text = "目的数据表名";
            // 
            // txtDestable
            // 
            this.txtDestable.Location = new System.Drawing.Point(553, 28);
            this.txtDestable.Name = "txtDestable";
            this.txtDestable.Size = new System.Drawing.Size(114, 21);
            this.txtDestable.TabIndex = 4;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chkAll);
            this.groupBox3.Controls.Add(this.dgArea);
            this.groupBox3.Location = new System.Drawing.Point(7, 55);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(304, 449);
            this.groupBox3.TabIndex = 19;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "游戏大区";
            // 
            // chkAll
            // 
            this.chkAll.AutoSize = true;
            this.chkAll.Location = new System.Drawing.Point(28, 19);
            this.chkAll.Name = "chkAll";
            this.chkAll.Size = new System.Drawing.Size(78, 16);
            this.chkAll.TabIndex = 1;
            this.chkAll.Text = "全选/取消";
            this.chkAll.UseVisualStyleBackColor = true;
            this.chkAll.CheckedChanged += new System.EventHandler(this.chkAll_CheckedChanged);
            // 
            // dgArea
            // 
            this.dgArea.AllowUserToAddRows = false;
            this.dgArea.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgArea.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.chkSelect,
            this.id,
            this.txtArea,
            this.areaip,
            this.PORT});
            this.dgArea.Location = new System.Drawing.Point(8, 41);
            this.dgArea.Name = "dgArea";
            this.dgArea.RowHeadersVisible = false;
            this.dgArea.RowTemplate.Height = 23;
            this.dgArea.Size = new System.Drawing.Size(290, 402);
            this.dgArea.TabIndex = 0;
            this.dgArea.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(319, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 21;
            this.label2.Text = "源数据库名";
            // 
            // txtDB
            // 
            this.txtDB.Location = new System.Drawing.Point(320, 28);
            this.txtDB.Name = "txtDB";
            this.txtDB.Size = new System.Drawing.Size(113, 21);
            this.txtDB.TabIndex = 2;
            this.txtDB.Leave += new System.EventHandler(this.txtTable_Leave);
            // 
            // cbxSwitch
            // 
            this.cbxSwitch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxSwitch.FormattingEnabled = true;
            this.cbxSwitch.Items.AddRange(new object[] {
            "采集",
            "停止采集"});
            this.cbxSwitch.Location = new System.Drawing.Point(213, 24);
            this.cbxSwitch.Name = "cbxSwitch";
            this.cbxSwitch.Size = new System.Drawing.Size(98, 20);
            this.cbxSwitch.TabIndex = 22;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(211, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 23;
            this.label3.Text = "采集开关";
            // 
            // rdb1
            // 
            this.rdb1.AutoSize = true;
            this.rdb1.Location = new System.Drawing.Point(7, 590);
            this.rdb1.Name = "rdb1";
            this.rdb1.Size = new System.Drawing.Size(107, 16);
            this.rdb1.TabIndex = 24;
            this.rdb1.TabStop = true;
            this.rdb1.Text = "自动建立目的表";
            this.rdb1.UseVisualStyleBackColor = true;
            this.rdb1.CheckedChanged += new System.EventHandler(this.rdb1_CheckedChanged);
            // 
            // rdb2
            // 
            this.rdb2.AutoSize = true;
            this.rdb2.Location = new System.Drawing.Point(120, 590);
            this.rdb2.Name = "rdb2";
            this.rdb2.Size = new System.Drawing.Size(107, 16);
            this.rdb2.TabIndex = 25;
            this.rdb2.TabStop = true;
            this.rdb2.Text = "手动建立目的表";
            this.rdb2.UseVisualStyleBackColor = true;
            this.rdb2.CheckedChanged += new System.EventHandler(this.rdb1_CheckedChanged);
            // 
            // txtTblSql
            // 
            this.txtTblSql.Enabled = false;
            this.txtTblSql.Location = new System.Drawing.Point(6, 625);
            this.txtTblSql.Multiline = true;
            this.txtTblSql.Name = "txtTblSql";
            this.txtTblSql.Size = new System.Drawing.Size(669, 121);
            this.txtTblSql.TabIndex = 26;
            this.txtTblSql.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 609);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 12);
            this.label4.TabIndex = 27;
            this.label4.Text = "建表SQL语句";
            this.label4.Visible = false;
            // 
            // nudHour
            // 
            this.nudHour.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nudHour.Location = new System.Drawing.Point(154, 20);
            this.nudHour.Maximum = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.nudHour.Name = "nudHour";
            this.nudHour.Size = new System.Drawing.Size(40, 21);
            this.nudHour.TabIndex = 28;
            // 
            // nudMinute
            // 
            this.nudMinute.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nudMinute.Location = new System.Drawing.Point(226, 20);
            this.nudMinute.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.nudMinute.Name = "nudMinute";
            this.nudMinute.Size = new System.Drawing.Size(40, 21);
            this.nudMinute.TabIndex = 29;
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(195, 27);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 12);
            this.label9.TabIndex = 30;
            this.label9.Text = "小时";
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(267, 27);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(29, 12);
            this.label10.TabIndex = 31;
            this.label10.Text = "分钟";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.nudDay);
            this.groupBox1.Controls.Add(this.dpStartTime);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.nudMinute);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.nudHour);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Location = new System.Drawing.Point(6, 761);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(305, 82);
            this.groupBox1.TabIndex = 32;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "采集时间设定";
            // 
            // label15
            // 
            this.label15.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(134, 27);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(17, 12);
            this.label15.TabIndex = 36;
            this.label15.Text = "天";
            // 
            // nudDay
            // 
            this.nudDay.Location = new System.Drawing.Point(94, 20);
            this.nudDay.Name = "nudDay";
            this.nudDay.Size = new System.Drawing.Size(40, 21);
            this.nudDay.TabIndex = 33;
            // 
            // dpStartTime
            // 
            this.dpStartTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dpStartTime.CustomFormat = "yyyy年MM月dd日 HH:mm:ss";
            this.dpStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dpStartTime.Location = new System.Drawing.Point(94, 52);
            this.dpStartTime.Name = "dpStartTime";
            this.dpStartTime.Size = new System.Drawing.Size(205, 21);
            this.dpStartTime.TabIndex = 35;
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(7, 57);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(77, 12);
            this.label14.TabIndex = 34;
            this.label14.Text = "采集开始时间";
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(7, 27);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(77, 12);
            this.label13.TabIndex = 33;
            this.label13.Text = "采集间隔时间";
            // 
            // fieldname
            // 
            this.fieldname.DataPropertyName = "字段名";
            this.fieldname.HeaderText = "字段名";
            this.fieldname.Name = "fieldname";
            this.fieldname.ReadOnly = true;
            this.fieldname.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.fieldname.Width = 90;
            // 
            // length
            // 
            this.length.DataPropertyName = "字段长度";
            this.length.HeaderText = "字段长度";
            this.length.Name = "length";
            this.length.ReadOnly = true;
            this.length.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.length.Width = 80;
            // 
            // chkFLG
            // 
            this.chkFLG.DataPropertyName = "表分区标示";
            this.chkFLG.FalseValue = "0";
            this.chkFLG.HeaderText = "表分区标示";
            this.chkFLG.Name = "chkFLG";
            this.chkFLG.ReadOnly = true;
            this.chkFLG.TrueValue = "1";
            this.chkFLG.Width = 75;
            // 
            // 字段类型
            // 
            this.字段类型.DataPropertyName = "字段类型";
            this.字段类型.HeaderText = "字段类型";
            this.字段类型.Name = "字段类型";
            this.字段类型.ReadOnly = true;
            this.字段类型.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // chkSelect
            // 
            this.chkSelect.DataPropertyName = "chk";
            this.chkSelect.FalseValue = "0";
            this.chkSelect.HeaderText = "";
            this.chkSelect.Name = "chkSelect";
            this.chkSelect.TrueValue = "1";
            this.chkSelect.Width = 50;
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            this.id.HeaderText = "id";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.id.Visible = false;
            // 
            // txtArea
            // 
            this.txtArea.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.txtArea.DataPropertyName = "ServerNM";
            this.txtArea.HeaderText = "大区名";
            this.txtArea.Name = "txtArea";
            this.txtArea.ReadOnly = true;
            this.txtArea.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.txtArea.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // areaip
            // 
            this.areaip.DataPropertyName = "Server_Ip";
            this.areaip.HeaderText = "IP";
            this.areaip.Name = "areaip";
            this.areaip.ReadOnly = true;
            this.areaip.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.areaip.Width = 80;
            // 
            // PORT
            // 
            this.PORT.DataPropertyName = "PORT";
            this.PORT.HeaderText = "端口";
            this.PORT.Name = "PORT";
            this.PORT.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.PORT.Width = 70;
            // 
            // frmGameEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 873);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtTblSql);
            this.Controls.Add(this.rdb2);
            this.Controls.Add(this.rdb1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbxSwitch);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtDB);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txtDestable);
            this.Controls.Add(this.cbxGameName);
            this.Controls.Add(this.btnCancle);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.lblbcp);
            this.Controls.Add(this.txtTable);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtBcp);
            this.Name = "frmGameEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "添加采集任务";
            this.Load += new System.EventHandler(this.frmGameEdit_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgTable)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgArea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHour)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinute)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel stbStatus;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtTable;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lblbcp;
        private System.Windows.Forms.TextBox txtBcp;
        private System.Windows.Forms.Button btnRemove2;
        private System.Windows.Forms.Button btnAdd2;
        private System.Windows.Forms.TextBox txtField;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancle;
        private System.Windows.Forms.ComboBox cbxGameName;
        private System.Windows.Forms.DataGridView dgTable;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtDestable;
        private System.Windows.Forms.TextBox txtLength;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView dgArea;
        private System.Windows.Forms.CheckBox chkAll;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDB;
        private System.Windows.Forms.ComboBox cbxSwitch;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rdb1;
        private System.Windows.Forms.RadioButton rdb2;
        private System.Windows.Forms.TextBox txtTblSql;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.NumericUpDown nudHour;
        private System.Windows.Forms.NumericUpDown nudMinute;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.DateTimePicker dpStartTime;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.NumericUpDown nudDay;
        private System.Windows.Forms.DataGridViewTextBoxColumn fieldname;
        private System.Windows.Forms.DataGridViewTextBoxColumn length;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chkFLG;
        private System.Windows.Forms.DataGridViewTextBoxColumn 字段类型;
        private System.Windows.Forms.DataGridViewCheckBoxColumn chkSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn txtArea;
        private System.Windows.Forms.DataGridViewTextBoxColumn areaip;
        private System.Windows.Forms.DataGridViewTextBoxColumn PORT;
    }
}