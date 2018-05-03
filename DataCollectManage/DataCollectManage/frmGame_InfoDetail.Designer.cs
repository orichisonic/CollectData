namespace DataCollectMonitor
{
    partial class frmGame_InfoDetail
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
            this.txtGameNM = new System.Windows.Forms.TextBox();
            this.chkStatus = new System.Windows.Forms.CheckBox();
            this.txtCollactUser = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCollactPwd = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtManagePwd = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtManageUser = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnCancle = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtSPACE_MODULUS = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cbxDbType = new System.Windows.Forms.ComboBox();
            this.txtTNS = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.cbxTbSpace = new System.Windows.Forms.ComboBox();
            this.txtTblSpace = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "游戏名";
            // 
            // txtGameNM
            // 
            this.txtGameNM.Location = new System.Drawing.Point(119, 8);
            this.txtGameNM.MaxLength = 20;
            this.txtGameNM.Name = "txtGameNM";
            this.txtGameNM.Size = new System.Drawing.Size(112, 21);
            this.txtGameNM.TabIndex = 1;
            // 
            // chkStatus
            // 
            this.chkStatus.AutoSize = true;
            this.chkStatus.Location = new System.Drawing.Point(260, 14);
            this.chkStatus.Name = "chkStatus";
            this.chkStatus.Size = new System.Drawing.Size(102, 16);
            this.chkStatus.TabIndex = 4;
            this.chkStatus.Text = "采集/停止采集";
            this.chkStatus.UseVisualStyleBackColor = true;
            // 
            // txtCollactUser
            // 
            this.txtCollactUser.Location = new System.Drawing.Point(119, 35);
            this.txtCollactUser.MaxLength = 30;
            this.txtCollactUser.Name = "txtCollactUser";
            this.txtCollactUser.Size = new System.Drawing.Size(112, 21);
            this.txtCollactUser.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "目标库用户名";
            // 
            // txtCollactPwd
            // 
            this.txtCollactPwd.Location = new System.Drawing.Point(119, 62);
            this.txtCollactPwd.MaxLength = 30;
            this.txtCollactPwd.Name = "txtCollactPwd";
            this.txtCollactPwd.PasswordChar = '*';
            this.txtCollactPwd.Size = new System.Drawing.Size(112, 21);
            this.txtCollactPwd.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "目标库密码";
            // 
            // txtManagePwd
            // 
            this.txtManagePwd.Enabled = false;
            this.txtManagePwd.Location = new System.Drawing.Point(119, 156);
            this.txtManagePwd.MaxLength = 30;
            this.txtManagePwd.Name = "txtManagePwd";
            this.txtManagePwd.PasswordChar = '*';
            this.txtManagePwd.Size = new System.Drawing.Size(112, 21);
            this.txtManagePwd.TabIndex = 6;
            this.txtManagePwd.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Enabled = false;
            this.label4.Location = new System.Drawing.Point(12, 162);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "目标库管理密码";
            this.label4.Visible = false;
            // 
            // txtManageUser
            // 
            this.txtManageUser.Enabled = false;
            this.txtManageUser.Location = new System.Drawing.Point(119, 126);
            this.txtManageUser.MaxLength = 30;
            this.txtManageUser.Name = "txtManageUser";
            this.txtManageUser.Size = new System.Drawing.Size(112, 21);
            this.txtManageUser.TabIndex = 5;
            this.txtManageUser.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Enabled = false;
            this.label5.Location = new System.Drawing.Point(12, 132);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 12);
            this.label5.TabIndex = 7;
            this.label5.Text = "目标库管理用户名";
            this.label5.Visible = false;
            // 
            // btnCancle
            // 
            this.btnCancle.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancle.Location = new System.Drawing.Point(406, 132);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(80, 21);
            this.btnCancle.TabIndex = 10;
            this.btnCancle.Text = "取消";
            this.btnCancle.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(307, 132);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(80, 21);
            this.btnOk.TabIndex = 9;
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 95);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 12);
            this.label6.TabIndex = 10;
            this.label6.Text = "采集缓存系数";
            // 
            // txtSPACE_MODULUS
            // 
            this.txtSPACE_MODULUS.Location = new System.Drawing.Point(119, 89);
            this.txtSPACE_MODULUS.MaxLength = 15;
            this.txtSPACE_MODULUS.Name = "txtSPACE_MODULUS";
            this.txtSPACE_MODULUS.Size = new System.Drawing.Size(112, 21);
            this.txtSPACE_MODULUS.TabIndex = 7;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(258, 92);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 12);
            this.label7.TabIndex = 12;
            this.label7.Text = "源数据库类型";
            // 
            // cbxDbType
            // 
            this.cbxDbType.FormattingEnabled = true;
            this.cbxDbType.Items.AddRange(new object[] {
            "SQL SERVER",
            "ORACLE",
            "MYSQL"});
            this.cbxDbType.Location = new System.Drawing.Point(376, 89);
            this.cbxDbType.Name = "cbxDbType";
            this.cbxDbType.Size = new System.Drawing.Size(112, 20);
            this.cbxDbType.TabIndex = 8;
            this.cbxDbType.SelectedIndexChanged += new System.EventHandler(this.cbxDbType_SelectedIndexChanged);
            // 
            // txtTNS
            // 
            this.txtTNS.Location = new System.Drawing.Point(375, 35);
            this.txtTNS.MaxLength = 15;
            this.txtTNS.Name = "txtTNS";
            this.txtTNS.Size = new System.Drawing.Size(112, 21);
            this.txtTNS.TabIndex = 13;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(258, 41);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 12);
            this.label8.TabIndex = 14;
            this.label8.Text = "目标库 TNS名";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(258, 65);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(41, 12);
            this.label13.TabIndex = 22;
            this.label13.Text = "表空间";
            // 
            // cbxTbSpace
            // 
            this.cbxTbSpace.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxTbSpace.Enabled = false;
            this.cbxTbSpace.FormattingEnabled = true;
            this.cbxTbSpace.Location = new System.Drawing.Point(376, 62);
            this.cbxTbSpace.Name = "cbxTbSpace";
            this.cbxTbSpace.Size = new System.Drawing.Size(110, 20);
            this.cbxTbSpace.TabIndex = 21;
            this.cbxTbSpace.Visible = false;
            // 
            // txtTblSpace
            // 
            this.txtTblSpace.Location = new System.Drawing.Point(376, 62);
            this.txtTblSpace.MaxLength = 15;
            this.txtTblSpace.Name = "txtTblSpace";
            this.txtTblSpace.Size = new System.Drawing.Size(112, 21);
            this.txtTblSpace.TabIndex = 23;
            // 
            // frmGame_InfoDetail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 164);
            this.Controls.Add(this.txtTblSpace);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.cbxTbSpace);
            this.Controls.Add(this.txtTNS);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cbxDbType);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtSPACE_MODULUS);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnCancle);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtManagePwd);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtManageUser);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtCollactPwd);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtCollactUser);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chkStatus);
            this.Controls.Add(this.txtGameNM);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmGame_InfoDetail";
            this.Load += new System.EventHandler(this.frmGame_InfoDetail_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtGameNM;
        private System.Windows.Forms.CheckBox chkStatus;
        private System.Windows.Forms.TextBox txtCollactUser;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCollactPwd;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtManagePwd;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtManageUser;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnCancle;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtSPACE_MODULUS;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbxDbType;
        private System.Windows.Forms.TextBox txtTNS;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox cbxTbSpace;
        private System.Windows.Forms.TextBox txtTblSpace;
    }
}