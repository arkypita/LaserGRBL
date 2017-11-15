namespace LaserGRBL
{
	partial class IssueDetectorForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IssueDetectorForm));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.BtnOK = new System.Windows.Forms.Button();
			this.CbDoNotShow = new System.Windows.Forms.CheckBox();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
			this.label4 = new System.Windows.Forms.Label();
			this.TxtCause = new System.Windows.Forms.Label();
			this.LblDetected = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.LblBrief = new System.Windows.Forms.Label();
			this.LL = new System.Windows.Forms.LinkLabel();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.tableLayoutPanel5.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(427, 149);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.AutoSize = true;
			this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel2.ColumnCount = 2;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.Controls.Add(this.BtnOK, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.CbDoNotShow, 0, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 113);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.Size = new System.Drawing.Size(421, 33);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// BtnOK
			// 
			this.BtnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.BtnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.BtnOK.Location = new System.Drawing.Point(344, 3);
			this.BtnOK.Name = "BtnOK";
			this.BtnOK.Size = new System.Drawing.Size(74, 27);
			this.BtnOK.TabIndex = 14;
			this.BtnOK.Text = "OK";
			this.BtnOK.UseVisualStyleBackColor = true;
			this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
			// 
			// CbDoNotShow
			// 
			this.CbDoNotShow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.CbDoNotShow.AutoSize = true;
			this.CbDoNotShow.Location = new System.Drawing.Point(3, 13);
			this.CbDoNotShow.Name = "CbDoNotShow";
			this.CbDoNotShow.Size = new System.Drawing.Size(115, 17);
			this.CbDoNotShow.TabIndex = 15;
			this.CbDoNotShow.Text = "Do not show again";
			this.CbDoNotShow.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.AutoSize = true;
			this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel3.ColumnCount = 2;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel5, 1, 0);
			this.tableLayoutPanel3.Controls.Add(this.pictureBox1, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.LblBrief, 0, 1);
			this.tableLayoutPanel3.Controls.Add(this.LL, 0, 2);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 3;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(421, 104);
			this.tableLayoutPanel3.TabIndex = 1;
			// 
			// tableLayoutPanel5
			// 
			this.tableLayoutPanel5.AutoSize = true;
			this.tableLayoutPanel5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel5.ColumnCount = 2;
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel5.Controls.Add(this.label4, 0, 1);
			this.tableLayoutPanel5.Controls.Add(this.TxtCause, 0, 1);
			this.tableLayoutPanel5.Controls.Add(this.LblDetected, 0, 0);
			this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel5.Location = new System.Drawing.Point(41, 3);
			this.tableLayoutPanel5.Name = "tableLayoutPanel5";
			this.tableLayoutPanel5.RowCount = 2;
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel5.Size = new System.Drawing.Size(377, 34);
			this.tableLayoutPanel5.TabIndex = 6;
			// 
			// label4
			// 
			this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(2, 19);
			this.label4.Margin = new System.Windows.Forms.Padding(2);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(40, 13);
			this.label4.TabIndex = 5;
			this.label4.Text = "Cause:";
			// 
			// TxtCause
			// 
			this.TxtCause.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.TxtCause.AutoSize = true;
			this.TxtCause.ForeColor = System.Drawing.Color.Red;
			this.TxtCause.Location = new System.Drawing.Point(46, 19);
			this.TxtCause.Margin = new System.Windows.Forms.Padding(2);
			this.TxtCause.Name = "TxtCause";
			this.TxtCause.Size = new System.Drawing.Size(53, 13);
			this.TxtCause.TabIndex = 4;
			this.TxtCause.Text = "Unknown";
			// 
			// LblDetected
			// 
			this.LblDetected.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblDetected.AutoSize = true;
			this.tableLayoutPanel5.SetColumnSpan(this.LblDetected, 2);
			this.LblDetected.Location = new System.Drawing.Point(2, 2);
			this.LblDetected.Margin = new System.Windows.Forms.Padding(2);
			this.LblDetected.Name = "LblDetected";
			this.LblDetected.Size = new System.Drawing.Size(253, 13);
			this.LblDetected.TabIndex = 3;
			this.LblDetected.Text = "LaserGRBL detected some problem with your board!";
			// 
			// pictureBox1
			// 
			this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(3, 4);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(32, 32);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox1.TabIndex = 3;
			this.pictureBox1.TabStop = false;
			// 
			// LblBrief
			// 
			this.LblBrief.AutoSize = true;
			this.tableLayoutPanel3.SetColumnSpan(this.LblBrief, 2);
			this.LblBrief.Location = new System.Drawing.Point(3, 43);
			this.LblBrief.Margin = new System.Windows.Forms.Padding(3);
			this.LblBrief.MaximumSize = new System.Drawing.Size(400, 0);
			this.LblBrief.Name = "LblBrief";
			this.LblBrief.Size = new System.Drawing.Size(388, 26);
			this.LblBrief.TabIndex = 4;
			this.LblBrief.Text = "It would appear that grbl has problems executing this job. Causes of this situati" +
    "on can be multiple, please read the FAQ for information on how to resolve this i" +
    "ssue.\r\n";
			// 
			// LL
			// 
			this.LL.AutoSize = true;
			this.tableLayoutPanel3.SetColumnSpan(this.LL, 2);
			this.LL.Location = new System.Drawing.Point(3, 76);
			this.LL.Margin = new System.Windows.Forms.Padding(3, 4, 3, 0);
			this.LL.Name = "LL";
			this.LL.Size = new System.Drawing.Size(125, 13);
			this.LL.TabIndex = 5;
			this.LL.TabStop = true;
			this.LL.Text = "http://lasergrbl.com/faq/";
			this.LL.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LL_LinkClicked);
			// 
			// IssueDetectorForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.CancelButton = this.BtnOK;
			this.ClientSize = new System.Drawing.Size(427, 149);
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "IssueDetectorForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Problem detected!";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.tableLayoutPanel5.ResumeLayout(false);
			this.tableLayoutPanel5.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Button BtnOK;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label LblBrief;
		private System.Windows.Forms.LinkLabel LL;
		private System.Windows.Forms.CheckBox CbDoNotShow;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label TxtCause;
		private System.Windows.Forms.Label LblDetected;
	}
}