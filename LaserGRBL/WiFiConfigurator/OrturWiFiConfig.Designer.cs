namespace LaserGRBL.WiFiConfigurator
{
	partial class OrturWiFiConfig
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OrturWiFiConfig));
			this.TxtSSID = new System.Windows.Forms.TextBox();
			this.TxtPassword = new System.Windows.Forms.TextBox();
			this.LblSSID = new System.Windows.Forms.Label();
			this.LblPassword = new System.Windows.Forms.Label();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.PbRevealPwd = new System.Windows.Forms.PictureBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.BtnHelp = new System.Windows.Forms.Button();
			this.BtnWrite = new System.Windows.Forms.Button();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.LblDescription = new System.Windows.Forms.Label();
			this.LblWaitConnection = new System.Windows.Forms.Label();
			this.TimWait = new System.Windows.Forms.Timer(this.components);
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.PbRevealPwd)).BeginInit();
			this.tableLayoutPanel2.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// TxtSSID
			// 
			this.TxtSSID.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.TxtSSID.Location = new System.Drawing.Point(117, 3);
			this.TxtSSID.Name = "TxtSSID";
			this.TxtSSID.Size = new System.Drawing.Size(276, 20);
			this.TxtSSID.TabIndex = 0;
			this.TxtSSID.TextChanged += new System.EventHandler(this.OnTextChanges);
			// 
			// TxtPassword
			// 
			this.TxtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
			this.TxtPassword.Location = new System.Drawing.Point(117, 29);
			this.TxtPassword.Name = "TxtPassword";
			this.TxtPassword.Size = new System.Drawing.Size(276, 20);
			this.TxtPassword.TabIndex = 1;
			this.TxtPassword.UseSystemPasswordChar = true;
			this.TxtPassword.TextChanged += new System.EventHandler(this.OnTextChanges);
			// 
			// LblSSID
			// 
			this.LblSSID.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblSSID.AutoSize = true;
			this.LblSSID.Location = new System.Drawing.Point(3, 6);
			this.LblSSID.Name = "LblSSID";
			this.LblSSID.Size = new System.Drawing.Size(108, 13);
			this.LblSSID.TabIndex = 2;
			this.LblSSID.Text = "SSID (network name)";
			// 
			// LblPassword
			// 
			this.LblPassword.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblPassword.AutoSize = true;
			this.LblPassword.Location = new System.Drawing.Point(3, 32);
			this.LblPassword.Name = "LblPassword";
			this.LblPassword.Size = new System.Drawing.Size(53, 13);
			this.LblPassword.TabIndex = 3;
			this.LblPassword.Text = "Password";
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 27F));
			this.tableLayoutPanel1.Controls.Add(this.LblPassword, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.TxtSSID, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.LblSSID, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.TxtPassword, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.PbRevealPwd, 2, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(73, 39);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(423, 52);
			this.tableLayoutPanel1.TabIndex = 4;
			// 
			// PbRevealPwd
			// 
			this.PbRevealPwd.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.PbRevealPwd.Image = ((System.Drawing.Image)(resources.GetObject("PbRevealPwd.Image")));
			this.PbRevealPwd.Location = new System.Drawing.Point(399, 29);
			this.PbRevealPwd.Name = "PbRevealPwd";
			this.PbRevealPwd.Size = new System.Drawing.Size(21, 20);
			this.PbRevealPwd.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.PbRevealPwd.TabIndex = 4;
			this.PbRevealPwd.TabStop = false;
			this.PbRevealPwd.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PbRevealPwd_MouseDown);
			this.PbRevealPwd.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PbRevealPwd_MouseUp);
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.AutoSize = true;
			this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel2.ColumnCount = 3;
			this.tableLayoutPanel3.SetColumnSpan(this.tableLayoutPanel2, 2);
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.Controls.Add(this.BtnHelp, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.BtnWrite, 2, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 119);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.Size = new System.Drawing.Size(493, 48);
			this.tableLayoutPanel2.TabIndex = 5;
			// 
			// BtnHelp
			// 
			this.BtnHelp.Image = ((System.Drawing.Image)(resources.GetObject("BtnHelp.Image")));
			this.BtnHelp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.BtnHelp.Location = new System.Drawing.Point(3, 3);
			this.BtnHelp.Name = "BtnHelp";
			this.BtnHelp.Size = new System.Drawing.Size(107, 42);
			this.BtnHelp.TabIndex = 17;
			this.BtnHelp.Text = "Help";
			this.BtnHelp.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.BtnHelp.UseVisualStyleBackColor = true;
			this.BtnHelp.Click += new System.EventHandler(this.BtnHelp_Click);
			// 
			// BtnWrite
			// 
			this.BtnWrite.Enabled = false;
			this.BtnWrite.Image = ((System.Drawing.Image)(resources.GetObject("BtnWrite.Image")));
			this.BtnWrite.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.BtnWrite.Location = new System.Drawing.Point(383, 3);
			this.BtnWrite.Name = "BtnWrite";
			this.BtnWrite.Size = new System.Drawing.Size(107, 42);
			this.BtnWrite.TabIndex = 16;
			this.BtnWrite.Text = "Write";
			this.BtnWrite.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
			this.BtnWrite.UseVisualStyleBackColor = true;
			this.BtnWrite.Click += new System.EventHandler(this.BtnWrite_Click);
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.ColumnCount = 2;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.Controls.Add(this.pictureBox1, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel2, 0, 3);
			this.tableLayoutPanel3.Controls.Add(this.LblDescription, 1, 0);
			this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel1, 1, 1);
			this.tableLayoutPanel3.Controls.Add(this.LblWaitConnection, 1, 2);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 4;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(499, 170);
			this.tableLayoutPanel3.TabIndex = 6;
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.pictureBox1.Location = new System.Drawing.Point(3, 3);
			this.pictureBox1.Name = "pictureBox1";
			this.tableLayoutPanel3.SetRowSpan(this.pictureBox1, 2);
			this.pictureBox1.Size = new System.Drawing.Size(64, 64);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox1.TabIndex = 5;
			this.pictureBox1.TabStop = false;
			// 
			// LblDescription
			// 
			this.LblDescription.AutoSize = true;
			this.LblDescription.Location = new System.Drawing.Point(75, 5);
			this.LblDescription.Margin = new System.Windows.Forms.Padding(5);
			this.LblDescription.Name = "LblDescription";
			this.LblDescription.Size = new System.Drawing.Size(410, 26);
			this.LblDescription.TabIndex = 0;
			this.LblDescription.Text = "In order to activate WiFi, SSID name and WiFi password must be programmed on the " +
    "machine. Please enter these details.";
			// 
			// LblWaitConnection
			// 
			this.LblWaitConnection.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblWaitConnection.AutoSize = true;
			this.LblWaitConnection.ForeColor = System.Drawing.Color.Red;
			this.LblWaitConnection.Location = new System.Drawing.Point(76, 98);
			this.LblWaitConnection.Margin = new System.Windows.Forms.Padding(6, 0, 3, 0);
			this.LblWaitConnection.Name = "LblWaitConnection";
			this.LblWaitConnection.Size = new System.Drawing.Size(249, 13);
			this.LblWaitConnection.TabIndex = 6;
			this.LblWaitConnection.Text = "Engraver is trying to connect to WiFi, please wait ...";
			this.LblWaitConnection.Visible = false;
			// 
			// TimWait
			// 
			this.TimWait.Tick += new System.EventHandler(this.TimWait_Tick);
			// 
			// OrturWiFiConfig
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(499, 170);
			this.Controls.Add(this.tableLayoutPanel3);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "OrturWiFiConfig";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "WiFi Configuration";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OrturWiFiConfig_FormClosing);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.PbRevealPwd)).EndInit();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TextBox TxtSSID;
		private System.Windows.Forms.TextBox TxtPassword;
		private System.Windows.Forms.Label LblSSID;
		private System.Windows.Forms.Label LblPassword;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.Label LblDescription;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Button BtnWrite;
		private System.Windows.Forms.PictureBox PbRevealPwd;
		private System.Windows.Forms.Timer TimWait;
		private System.Windows.Forms.Label LblWaitConnection;
		private System.Windows.Forms.Button BtnHelp;
	}
}