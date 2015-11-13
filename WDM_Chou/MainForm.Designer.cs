namespace WDM_Chou
{
    partial class MainForm
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
            this._btnChou = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this._ctlEditPhoneNumber = new System.Windows.Forms.TextBox();
            this._ctlRadio19 = new System.Windows.Forms.RadioButton();
            this._ctlRadio18 = new System.Windows.Forms.RadioButton();
            this._ctlRadio17 = new System.Windows.Forms.RadioButton();
            this._ctlRadio20 = new System.Windows.Forms.RadioButton();
            this._ctlRadio21 = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this._ctlTextCrumb = new System.Windows.Forms.Label();
            this._ctlEditLog = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this._ctlStatusCount = new System.Windows.Forms.ToolStripStatusLabel();
            this._ctlStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this._ctlStatusMobile = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this._ctlStatusBatch = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // _btnChou
            // 
            this._btnChou.Font = new System.Drawing.Font("Microsoft YaHei", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this._btnChou.Location = new System.Drawing.Point(277, 56);
            this._btnChou.Name = "_btnChou";
            this._btnChou.Size = new System.Drawing.Size(114, 173);
            this._btnChou.TabIndex = 0;
            this._btnChou.Text = "已停止";
            this._btnChou.UseVisualStyleBackColor = true;
            this._btnChou.Click += new System.EventHandler(this._btnChou_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 38);
            this.label1.TabIndex = 1;
            this.label1.Text = "手机号:";
            // 
            // _ctlEditPhoneNumber
            // 
            this._ctlEditPhoneNumber.Font = new System.Drawing.Font("Microsoft YaHei", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this._ctlEditPhoneNumber.Location = new System.Drawing.Point(129, 11);
            this._ctlEditPhoneNumber.Name = "_ctlEditPhoneNumber";
            this._ctlEditPhoneNumber.Size = new System.Drawing.Size(262, 39);
            this._ctlEditPhoneNumber.TabIndex = 2;
            // 
            // _ctlRadio19
            // 
            this._ctlRadio19.AutoSize = true;
            this._ctlRadio19.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this._ctlRadio19.Location = new System.Drawing.Point(28, 154);
            this._ctlRadio19.Name = "_ctlRadio19";
            this._ctlRadio19.Size = new System.Drawing.Size(204, 29);
            this._ctlRadio19.TabIndex = 3;
            this._ctlRadio19.TabStop = true;
            this._ctlRadio19.Tag = "19";
            this._ctlRadio19.Text = "10元月饼礼盒代金券";
            this._ctlRadio19.UseVisualStyleBackColor = true;
            // 
            // _ctlRadio18
            // 
            this._ctlRadio18.AutoSize = true;
            this._ctlRadio18.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this._ctlRadio18.Location = new System.Drawing.Point(28, 14);
            this._ctlRadio18.Name = "_ctlRadio18";
            this._ctlRadio18.Size = new System.Drawing.Size(101, 29);
            this._ctlRadio18.TabIndex = 3;
            this._ctlRadio18.TabStop = true;
            this._ctlRadio18.Tag = "18";
            this._ctlRadio18.Text = "288礼盒";
            this._ctlRadio18.UseVisualStyleBackColor = true;
            // 
            // _ctlRadio17
            // 
            this._ctlRadio17.AutoSize = true;
            this._ctlRadio17.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this._ctlRadio17.Location = new System.Drawing.Point(28, 49);
            this._ctlRadio17.Name = "_ctlRadio17";
            this._ctlRadio17.Size = new System.Drawing.Size(98, 29);
            this._ctlRadio17.TabIndex = 3;
            this._ctlRadio17.TabStop = true;
            this._ctlRadio17.Tag = "17";
            this._ctlRadio17.Text = "月饼1颗";
            this._ctlRadio17.UseVisualStyleBackColor = true;
            // 
            // _ctlRadio20
            // 
            this._ctlRadio20.AutoSize = true;
            this._ctlRadio20.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this._ctlRadio20.Location = new System.Drawing.Point(28, 119);
            this._ctlRadio20.Name = "_ctlRadio20";
            this._ctlRadio20.Size = new System.Drawing.Size(109, 29);
            this._ctlRadio20.TabIndex = 3;
            this._ctlRadio20.TabStop = true;
            this._ctlRadio20.Tag = "20";
            this._ctlRadio20.Text = "20元礼盒";
            this._ctlRadio20.UseVisualStyleBackColor = true;
            // 
            // _ctlRadio21
            // 
            this._ctlRadio21.AutoSize = true;
            this._ctlRadio21.Font = new System.Drawing.Font("Microsoft YaHei", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this._ctlRadio21.Location = new System.Drawing.Point(28, 84);
            this._ctlRadio21.Name = "_ctlRadio21";
            this._ctlRadio21.Size = new System.Drawing.Size(109, 29);
            this._ctlRadio21.TabIndex = 3;
            this._ctlRadio21.TabStop = true;
            this._ctlRadio21.Tag = "21";
            this._ctlRadio21.Text = "30元礼盒";
            this._ctlRadio21.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this._ctlRadio18);
            this.panel1.Controls.Add(this._ctlRadio21);
            this.panel1.Controls.Add(this._ctlRadio19);
            this.panel1.Controls.Add(this._ctlRadio20);
            this.panel1.Controls.Add(this._ctlRadio17);
            this.panel1.Location = new System.Drawing.Point(12, 56);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(259, 198);
            this.panel1.TabIndex = 4;
            // 
            // _ctlTextCrumb
            // 
            this._ctlTextCrumb.Location = new System.Drawing.Point(277, 232);
            this._ctlTextCrumb.Name = "_ctlTextCrumb";
            this._ctlTextCrumb.Size = new System.Drawing.Size(114, 22);
            this._ctlTextCrumb.TabIndex = 6;
            this._ctlTextCrumb.Text = "0";
            this._ctlTextCrumb.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // _ctlEditLog
            // 
            this._ctlEditLog.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this._ctlEditLog.Location = new System.Drawing.Point(397, 12);
            this._ctlEditLog.Multiline = true;
            this._ctlEditLog.Name = "_ctlEditLog";
            this._ctlEditLog.ReadOnly = true;
            this._ctlEditLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this._ctlEditLog.Size = new System.Drawing.Size(280, 242);
            this._ctlEditLog.TabIndex = 7;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._ctlStatusCount,
            this._ctlStatus,
            this.toolStripStatusLabel1,
            this._ctlStatusMobile,
            this.toolStripStatusLabel2,
            this._ctlStatusBatch});
            this.statusStrip1.Location = new System.Drawing.Point(0, 260);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(689, 22);
            this.statusStrip1.TabIndex = 8;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // _ctlStatusCount
            // 
            this._ctlStatusCount.Name = "_ctlStatusCount";
            this._ctlStatusCount.Size = new System.Drawing.Size(51, 17);
            this._ctlStatusCount.Text = "尝试0次";
            // 
            // _ctlStatus
            // 
            this._ctlStatus.Name = "_ctlStatus";
            this._ctlStatus.Size = new System.Drawing.Size(56, 17);
            this._ctlStatus.Text = "活动信息";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(47, 17);
            this.toolStripStatusLabel1.Text = "手机号:";
            // 
            // _ctlStatusMobile
            // 
            this._ctlStatusMobile.Name = "_ctlStatusMobile";
            this._ctlStatusMobile.Size = new System.Drawing.Size(85, 17);
            this._ctlStatusMobile.Text = "00000000000";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(43, 17);
            this.toolStripStatusLabel2.Text = "batch:";
            // 
            // _ctlStatusBatch
            // 
            this._ctlStatusBatch.Name = "_ctlStatusBatch";
            this._ctlStatusBatch.Size = new System.Drawing.Size(15, 17);
            this._ctlStatusBatch.Text = "0";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(689, 282);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this._ctlEditLog);
            this.Controls.Add(this._ctlTextCrumb);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this._ctlEditPhoneNumber);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._btnChou);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "味多美中秋抽奖";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _btnChou;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox _ctlEditPhoneNumber;
        private System.Windows.Forms.RadioButton _ctlRadio19;
        private System.Windows.Forms.RadioButton _ctlRadio18;
        private System.Windows.Forms.RadioButton _ctlRadio17;
        private System.Windows.Forms.RadioButton _ctlRadio20;
        private System.Windows.Forms.RadioButton _ctlRadio21;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label _ctlTextCrumb;
        private System.Windows.Forms.TextBox _ctlEditLog;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel _ctlStatusCount;
        private System.Windows.Forms.ToolStripStatusLabel _ctlStatus;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel _ctlStatusMobile;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel _ctlStatusBatch;
    }
}

