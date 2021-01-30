namespace LaserGRBL.WiFiDiscovery
{
	partial class DiscoveryForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DiscoveryForm));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.BtnScan = new System.Windows.Forms.Button();
			this.BtnConnect = new System.Windows.Forms.Button();
			this.BtnStop = new System.Windows.Forms.Button();
			this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
			this.label1 = new System.Windows.Forms.Label();
			this.UdPort = new System.Windows.Forms.NumericUpDown();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.LblDescription = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.LblProgress = new System.Windows.Forms.Label();
			this.LV = new System.Windows.Forms.ListView();
			this.ChIP = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ChHostName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ChMAC = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ChPing = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.ChConnection = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.tableLayoutPanel5.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.UdPort)).BeginInit();
			this.tableLayoutPanel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.tableLayoutPanel4.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel4, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 3;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 46F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(551, 338);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 4;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.Controls.Add(this.BtnScan, 3, 0);
			this.tableLayoutPanel2.Controls.Add(this.BtnConnect, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.BtnStop, 2, 0);
			this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel5, 0, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 295);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(545, 40);
			this.tableLayoutPanel2.TabIndex = 1;
			// 
			// BtnScan
			// 
			this.BtnScan.Location = new System.Drawing.Point(462, 3);
			this.BtnScan.Name = "BtnScan";
			this.BtnScan.Size = new System.Drawing.Size(80, 34);
			this.BtnScan.TabIndex = 0;
			this.BtnScan.Text = "Scan";
			this.BtnScan.UseVisualStyleBackColor = true;
			this.BtnScan.Click += new System.EventHandler(this.BtnScan_Click);
			// 
			// BtnConnect
			// 
			this.BtnConnect.Enabled = false;
			this.BtnConnect.Location = new System.Drawing.Point(284, 3);
			this.BtnConnect.Name = "BtnConnect";
			this.BtnConnect.Size = new System.Drawing.Size(86, 34);
			this.BtnConnect.TabIndex = 1;
			this.BtnConnect.Text = "Connect";
			this.BtnConnect.UseVisualStyleBackColor = true;
			this.BtnConnect.Click += new System.EventHandler(this.BtnConnect_Click);
			// 
			// BtnStop
			// 
			this.BtnStop.Location = new System.Drawing.Point(376, 3);
			this.BtnStop.Name = "BtnStop";
			this.BtnStop.Size = new System.Drawing.Size(80, 34);
			this.BtnStop.TabIndex = 2;
			this.BtnStop.Text = "Stop";
			this.BtnStop.UseVisualStyleBackColor = true;
			this.BtnStop.Visible = false;
			this.BtnStop.Click += new System.EventHandler(this.BtnStop_Click);
			// 
			// tableLayoutPanel5
			// 
			this.tableLayoutPanel5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.tableLayoutPanel5.AutoSize = true;
			this.tableLayoutPanel5.ColumnCount = 2;
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel5.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel5.Controls.Add(this.UdPort, 1, 0);
			this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 11);
			this.tableLayoutPanel5.Name = "tableLayoutPanel5";
			this.tableLayoutPanel5.RowCount = 1;
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.Size = new System.Drawing.Size(118, 26);
			this.tableLayoutPanel5.TabIndex = 3;
			// 
			// label1
			// 
			this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(3, 6);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(53, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "TCP Port:";
			// 
			// UdPort
			// 
			this.UdPort.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.UdPort.Location = new System.Drawing.Point(62, 3);
			this.UdPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
			this.UdPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.UdPort.Name = "UdPort";
			this.UdPort.Size = new System.Drawing.Size(53, 20);
			this.UdPort.TabIndex = 1;
			this.UdPort.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.UdPort.ValueChanged += new System.EventHandler(this.UdPort_ValueChanged);
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.AutoSize = true;
			this.tableLayoutPanel3.ColumnCount = 2;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Controls.Add(this.LblDescription, 1, 0);
			this.tableLayoutPanel3.Controls.Add(this.pictureBox1, 0, 0);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 1;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.Size = new System.Drawing.Size(545, 83);
			this.tableLayoutPanel3.TabIndex = 2;
			// 
			// LblDescription
			// 
			this.LblDescription.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.LblDescription.AutoSize = true;
			this.LblDescription.Location = new System.Drawing.Point(99, 15);
			this.LblDescription.Name = "LblDescription";
			this.LblDescription.Size = new System.Drawing.Size(430, 52);
			this.LblDescription.TabIndex = 0;
			this.LblDescription.Text = resources.GetString("LblDescription.Text");
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(3, 3);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(77, 77);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox1.TabIndex = 1;
			this.pictureBox1.TabStop = false;
			// 
			// tableLayoutPanel4
			// 
			this.tableLayoutPanel4.ColumnCount = 1;
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel4.Controls.Add(this.LblProgress, 0, 1);
			this.tableLayoutPanel4.Controls.Add(this.LV, 0, 0);
			this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 92);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 2;
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.Size = new System.Drawing.Size(545, 197);
			this.tableLayoutPanel4.TabIndex = 3;
			// 
			// LblProgress
			// 
			this.LblProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.LblProgress.AutoSize = true;
			this.LblProgress.Location = new System.Drawing.Point(3, 181);
			this.LblProgress.Margin = new System.Windows.Forms.Padding(3);
			this.LblProgress.Name = "LblProgress";
			this.LblProgress.Size = new System.Drawing.Size(539, 13);
			this.LblProgress.TabIndex = 3;
			this.LblProgress.Text = "Progress:";
			// 
			// LV
			// 
			this.LV.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ChIP,
            this.ChPing,
            this.ChHostName,
            this.ChMAC,
            this.ChConnection});
			this.LV.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LV.FullRowSelect = true;
			this.LV.HideSelection = false;
			this.LV.Location = new System.Drawing.Point(3, 3);
			this.LV.MultiSelect = false;
			this.LV.Name = "LV";
			this.LV.Size = new System.Drawing.Size(539, 172);
			this.LV.TabIndex = 2;
			this.LV.UseCompatibleStateImageBehavior = false;
			this.LV.View = System.Windows.Forms.View.Details;
			this.LV.SelectedIndexChanged += new System.EventHandler(this.LV_SelectedIndexChanged);
			this.LV.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.LV_MouseDoubleClick);
			// 
			// ChIP
			// 
			this.ChIP.Text = "IP Address";
			this.ChIP.Width = 100;
			// 
			// ChHostName
			// 
			this.ChHostName.Text = "Host Name";
			this.ChHostName.Width = 120;
			// 
			// ChMAC
			// 
			this.ChMAC.Text = "MAC Address";
			this.ChMAC.Width = 100;
			// 
			// ChPing
			// 
			this.ChPing.Text = "Ping";
			// 
			// ChConnection
			// 
			this.ChConnection.Text = "Connection";
			this.ChConnection.Width = 130;
			// 
			// DiscoveryForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(551, 338);
			this.Controls.Add(this.tableLayoutPanel1);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "DiscoveryForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "WiFi Discovery";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DiscoveryForm_FormClosing);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.tableLayoutPanel5.ResumeLayout(false);
			this.tableLayoutPanel5.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.UdPort)).EndInit();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.tableLayoutPanel4.ResumeLayout(false);
			this.tableLayoutPanel4.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Button BtnScan;
		private System.Windows.Forms.Button BtnConnect;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.Label LblDescription;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Button BtnStop;
		private System.Windows.Forms.Label LblProgress;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private System.Windows.Forms.ListView LV;
		private System.Windows.Forms.ColumnHeader ChIP;
		private System.Windows.Forms.ColumnHeader ChMAC;
		private System.Windows.Forms.ColumnHeader ChPing;
		private System.Windows.Forms.ColumnHeader ChConnection;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown UdPort;
		private System.Windows.Forms.ColumnHeader ChHostName;
	}
}