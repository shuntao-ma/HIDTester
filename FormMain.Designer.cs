namespace HIDTester
{
    partial class FormMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.fraDeviceIdentifiers = new System.Windows.Forms.GroupBox();
            this.txtProductID = new System.Windows.Forms.TextBox();
            this.lblProductID = new System.Windows.Forms.Label();
            this.txtVendorID = new System.Windows.Forms.TextBox();
            this.lblVendorID = new System.Windows.Forms.Label();
            this.StateLabel = new System.Windows.Forms.Label();
            this.fdToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OutputListBox = new System.Windows.Forms.RichTextBox();
            this.SendDataSousse = new System.Windows.Forms.TextBox();
            this.DevListCombox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.DataTypeCombox = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.PoweZ = new System.Windows.Forms.MenuStrip();
            this.DeviceOperate = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenDevice = new System.Windows.Forms.ToolStripMenuItem();
            this.CloseDeviceBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitSoft = new System.Windows.Forms.ToolStripMenuItem();
            this.GetDeviceInfoBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.SendDataBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.ClearRecDataBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.GetPowrZValue = new System.Windows.Forms.ToolStripMenuItem();
            this.fraDeviceIdentifiers.SuspendLayout();
            this.PoweZ.SuspendLayout();
            this.SuspendLayout();
            // 
            // fraDeviceIdentifiers
            // 
            this.fraDeviceIdentifiers.Controls.Add(this.txtProductID);
            this.fraDeviceIdentifiers.Controls.Add(this.lblProductID);
            this.fraDeviceIdentifiers.Controls.Add(this.txtVendorID);
            this.fraDeviceIdentifiers.Controls.Add(this.lblVendorID);
            this.fraDeviceIdentifiers.Controls.Add(this.StateLabel);
            this.fraDeviceIdentifiers.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.fraDeviceIdentifiers.Location = new System.Drawing.Point(672, 30);
            this.fraDeviceIdentifiers.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.fraDeviceIdentifiers.Name = "fraDeviceIdentifiers";
            this.fraDeviceIdentifiers.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.fraDeviceIdentifiers.Size = new System.Drawing.Size(175, 148);
            this.fraDeviceIdentifiers.TabIndex = 11;
            this.fraDeviceIdentifiers.TabStop = false;
            this.fraDeviceIdentifiers.Text = "设备ID";
            // 
            // txtProductID
            // 
            this.txtProductID.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtProductID.Enabled = false;
            this.txtProductID.Location = new System.Drawing.Point(118, 117);
            this.txtProductID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtProductID.Name = "txtProductID";
            this.txtProductID.Size = new System.Drawing.Size(49, 23);
            this.txtProductID.TabIndex = 3;
            // 
            // lblProductID
            // 
            this.lblProductID.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.lblProductID.Location = new System.Drawing.Point(5, 120);
            this.lblProductID.Name = "lblProductID";
            this.lblProductID.Size = new System.Drawing.Size(107, 21);
            this.lblProductID.TabIndex = 2;
            this.lblProductID.Text = "Product ID (hex):";
            // 
            // txtVendorID
            // 
            this.txtVendorID.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtVendorID.Enabled = false;
            this.txtVendorID.Location = new System.Drawing.Point(118, 84);
            this.txtVendorID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtVendorID.Name = "txtVendorID";
            this.txtVendorID.Size = new System.Drawing.Size(49, 23);
            this.txtVendorID.TabIndex = 1;
            // 
            // lblVendorID
            // 
            this.lblVendorID.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.lblVendorID.Location = new System.Drawing.Point(5, 87);
            this.lblVendorID.Name = "lblVendorID";
            this.lblVendorID.Size = new System.Drawing.Size(107, 19);
            this.lblVendorID.TabIndex = 0;
            this.lblVendorID.Text = "Vendor ID (hex):";
            // 
            // StateLabel
            // 
            this.StateLabel.AutoSize = true;
            this.StateLabel.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.StateLabel.Font = new System.Drawing.Font("微软雅黑", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.StateLabel.Location = new System.Drawing.Point(9, 20);
            this.StateLabel.Name = "StateLabel";
            this.StateLabel.Size = new System.Drawing.Size(160, 46);
            this.StateLabel.TabIndex = 20;
            this.StateLabel.Text = "设备关闭";
            // 
            // fdToolStripMenuItem
            // 
            this.fdToolStripMenuItem.Name = "fdToolStripMenuItem";
            this.fdToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // OutputListBox
            // 
            this.OutputListBox.Location = new System.Drawing.Point(5, 204);
            this.OutputListBox.Name = "OutputListBox";
            this.OutputListBox.Size = new System.Drawing.Size(842, 353);
            this.OutputListBox.TabIndex = 26;
            this.OutputListBox.Text = "";
            // 
            // SendDataSousse
            // 
            this.SendDataSousse.Location = new System.Drawing.Point(5, 114);
            this.SendDataSousse.Multiline = true;
            this.SendDataSousse.Name = "SendDataSousse";
            this.SendDataSousse.Size = new System.Drawing.Size(661, 64);
            this.SendDataSousse.TabIndex = 27;
            // 
            // DevListCombox
            // 
            this.DevListCombox.BackColor = System.Drawing.Color.White;
            this.DevListCombox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DevListCombox.Enabled = false;
            this.DevListCombox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.DevListCombox.FormattingEnabled = true;
            this.DevListCombox.Location = new System.Drawing.Point(79, 64);
            this.DevListCombox.Name = "DevListCombox";
            this.DevListCombox.Size = new System.Drawing.Size(579, 25);
            this.DevListCombox.TabIndex = 28;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(8, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 20);
            this.label2.TabIndex = 25;
            this.label2.Text = "设备列表";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(8, 91);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 20);
            this.label3.TabIndex = 25;
            this.label3.Text = "发送";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(8, 181);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 20);
            this.label4.TabIndex = 25;
            this.label4.Text = "接收";
            // 
            // DataTypeCombox
            // 
            this.DataTypeCombox.FormattingEnabled = true;
            this.DataTypeCombox.Items.AddRange(new object[] {
            "String",
            "Byte"});
            this.DataTypeCombox.Location = new System.Drawing.Point(79, 28);
            this.DataTypeCombox.Name = "DataTypeCombox";
            this.DataTypeCombox.Size = new System.Drawing.Size(104, 25);
            this.DataTypeCombox.TabIndex = 29;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(8, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 20);
            this.label5.TabIndex = 25;
            this.label5.Text = "数据类型";
            // 
            // PoweZ
            // 
            this.PoweZ.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DeviceOperate,
            this.GetDeviceInfoBtn,
            this.SendDataBtn,
            this.GetPowrZValue,
            this.ClearRecDataBtn});
            this.PoweZ.Location = new System.Drawing.Point(0, 0);
            this.PoweZ.Name = "PoweZ";
            this.PoweZ.Size = new System.Drawing.Size(851, 25);
            this.PoweZ.TabIndex = 30;
            this.PoweZ.Text = "PoweZ";
            // 
            // DeviceOperate
            // 
            this.DeviceOperate.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenDevice,
            this.CloseDeviceBtn,
            this.ExitSoft});
            this.DeviceOperate.Name = "DeviceOperate";
            this.DeviceOperate.Size = new System.Drawing.Size(68, 21);
            this.DeviceOperate.Text = "设备操作";
            // 
            // OpenDevice
            // 
            this.OpenDevice.Name = "OpenDevice";
            this.OpenDevice.Size = new System.Drawing.Size(124, 22);
            this.OpenDevice.Text = "打开设备";
            this.OpenDevice.Click += new System.EventHandler(this.OpenDevice_Click);
            // 
            // CloseDeviceBtn
            // 
            this.CloseDeviceBtn.Name = "CloseDeviceBtn";
            this.CloseDeviceBtn.Size = new System.Drawing.Size(124, 22);
            this.CloseDeviceBtn.Text = "关闭设备";
            this.CloseDeviceBtn.Click += new System.EventHandler(this.CloseDevice_Click);
            // 
            // ExitSoft
            // 
            this.ExitSoft.Name = "ExitSoft";
            this.ExitSoft.Size = new System.Drawing.Size(124, 22);
            this.ExitSoft.Text = "退出软件";
            this.ExitSoft.Click += new System.EventHandler(this.ExitSoftWare_Click);
            // 
            // GetDeviceInfoBtn
            // 
            this.GetDeviceInfoBtn.Name = "GetDeviceInfoBtn";
            this.GetDeviceInfoBtn.Size = new System.Drawing.Size(92, 21);
            this.GetDeviceInfoBtn.Text = "获取设备信息";
            this.GetDeviceInfoBtn.Click += new System.EventHandler(this.GetDeviceInfo_Click);
            // 
            // SendDataBtn
            // 
            this.SendDataBtn.Name = "SendDataBtn";
            this.SendDataBtn.Size = new System.Drawing.Size(68, 21);
            this.SendDataBtn.Text = "发送数据";
            this.SendDataBtn.Click += new System.EventHandler(this.SendData_Click);
            // 
            // ClearRecDataBtn
            // 
            this.ClearRecDataBtn.Name = "ClearRecDataBtn";
            this.ClearRecDataBtn.Size = new System.Drawing.Size(68, 21);
            this.ClearRecDataBtn.Text = "清除信息";
            this.ClearRecDataBtn.Click += new System.EventHandler(this.ClearRecData_Click);
            // 
            // GetPowrZValue
            // 
            this.GetPowrZValue.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.GetPowrZValue.Name = "GetPowrZValue";
            this.GetPowrZValue.Size = new System.Drawing.Size(106, 21);
            this.GetPowrZValue.Text = "获取PoweZ数据";
            this.GetPowrZValue.Click += new System.EventHandler(this.GetPowrZValue_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(851, 574);
            this.Controls.Add(this.fraDeviceIdentifiers);
            this.Controls.Add(this.DataTypeCombox);
            this.Controls.Add(this.DevListCombox);
            this.Controls.Add(this.SendDataSousse);
            this.Controls.Add(this.OutputListBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.PoweZ);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MainMenuStrip = this.PoweZ;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "USB调试----默默的ID";
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.fraDeviceIdentifiers.ResumeLayout(false);
            this.fraDeviceIdentifiers.PerformLayout();
            this.PoweZ.ResumeLayout(false);
            this.PoweZ.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        internal System.Windows.Forms.GroupBox fraDeviceIdentifiers;
        internal System.Windows.Forms.TextBox txtProductID;
        internal System.Windows.Forms.Label lblProductID;
        internal System.Windows.Forms.TextBox txtVendorID;
        internal System.Windows.Forms.Label lblVendorID;
        private System.Windows.Forms.Label StateLabel;
        private System.Windows.Forms.ToolStripMenuItem fdToolStripMenuItem;
        private System.Windows.Forms.RichTextBox OutputListBox;
        private System.Windows.Forms.TextBox SendDataSousse;
        private System.Windows.Forms.ComboBox DevListCombox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox DataTypeCombox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.MenuStrip PoweZ;
        private System.Windows.Forms.ToolStripMenuItem GetPowrZValue;
        private System.Windows.Forms.ToolStripMenuItem DeviceOperate;
        private System.Windows.Forms.ToolStripMenuItem OpenDevice;
        private System.Windows.Forms.ToolStripMenuItem CloseDeviceBtn;
        private System.Windows.Forms.ToolStripMenuItem ExitSoft;
        private System.Windows.Forms.ToolStripMenuItem GetDeviceInfoBtn;
        private System.Windows.Forms.ToolStripMenuItem SendDataBtn;
        private System.Windows.Forms.ToolStripMenuItem ClearRecDataBtn;
    }
}

