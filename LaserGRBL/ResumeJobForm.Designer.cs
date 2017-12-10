namespace LaserGRBL
{
	partial class ResumeJobForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResumeJobForm));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.CbRedoHoming = new System.Windows.Forms.CheckBox();
			this.BtnOK = new System.Windows.Forms.Button();
			this.BtnCancel = new System.Windows.Forms.Button();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.LblSomeLines = new System.Windows.Forms.Label();
			this.RbSomeLines = new System.Windows.Forms.RadioButton();
			this.label2 = new System.Windows.Forms.Label();
			this.RbFromSpecific = new System.Windows.Forms.RadioButton();
			this.RbFromBeginning = new System.Windows.Forms.RadioButton();
			this.label1 = new System.Windows.Forms.Label();
			this.LblBegin = new System.Windows.Forms.Label();
			this.UdSpecific = new System.Windows.Forms.NumericUpDown();
			this.RbFromSent = new System.Windows.Forms.RadioButton();
			this.LblSent = new System.Windows.Forms.Label();
			this.LblBrief = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
			this.label4 = new System.Windows.Forms.Label();
			this.TxtCause = new System.Windows.Forms.Label();
			this.LblDetected = new System.Windows.Forms.Label();
			this.CbRestoreWCO = new System.Windows.Forms.CheckBox();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.UdSpecific)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.tableLayoutPanel5.SuspendLayout();
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
			this.tableLayoutPanel1.Size = new System.Drawing.Size(490, 261);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.AutoSize = true;
			this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel2.ColumnCount = 4;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.Controls.Add(this.CbRestoreWCO, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.BtnOK, 3, 0);
			this.tableLayoutPanel2.Controls.Add(this.BtnCancel, 2, 0);
			this.tableLayoutPanel2.Controls.Add(this.CbRedoHoming, 0, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 225);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.Size = new System.Drawing.Size(484, 33);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// CbRedoHoming
			// 
			this.CbRedoHoming.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.CbRedoHoming.AutoSize = true;
			this.CbRedoHoming.Location = new System.Drawing.Point(3, 13);
			this.CbRedoHoming.Name = "CbRedoHoming";
			this.CbRedoHoming.Size = new System.Drawing.Size(114, 17);
			this.CbRedoHoming.TabIndex = 16;
			this.CbRedoHoming.Text = "Redo Homing [$H]";
			this.CbRedoHoming.UseVisualStyleBackColor = true;
			// 
			// BtnOK
			// 
			this.BtnOK.Enabled = false;
			this.BtnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.BtnOK.Location = new System.Drawing.Point(407, 3);
			this.BtnOK.Name = "BtnOK";
			this.BtnOK.Size = new System.Drawing.Size(74, 27);
			this.BtnOK.TabIndex = 14;
			this.BtnOK.Text = "OK";
			this.BtnOK.UseVisualStyleBackColor = true;
			this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
			// 
			// BtnCancel
			// 
			this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.BtnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.BtnCancel.Location = new System.Drawing.Point(327, 3);
			this.BtnCancel.Name = "BtnCancel";
			this.BtnCancel.Size = new System.Drawing.Size(74, 27);
			this.BtnCancel.TabIndex = 15;
			this.BtnCancel.Text = "Cancel";
			this.BtnCancel.UseVisualStyleBackColor = true;
			this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.AutoSize = true;
			this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel3.ColumnCount = 2;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 0, 2);
			this.tableLayoutPanel3.Controls.Add(this.LblBrief, 0, 1);
			this.tableLayoutPanel3.Controls.Add(this.pictureBox1, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel5, 1, 0);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 3;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.Size = new System.Drawing.Size(484, 216);
			this.tableLayoutPanel3.TabIndex = 1;
			// 
			// tableLayoutPanel4
			// 
			this.tableLayoutPanel4.AutoSize = true;
			this.tableLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel4.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
			this.tableLayoutPanel4.ColumnCount = 2;
			this.tableLayoutPanel3.SetColumnSpan(this.tableLayoutPanel4, 2);
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.Controls.Add(this.LblSomeLines, 1, 2);
			this.tableLayoutPanel4.Controls.Add(this.RbSomeLines, 0, 2);
			this.tableLayoutPanel4.Controls.Add(this.label2, 1, 0);
			this.tableLayoutPanel4.Controls.Add(this.RbFromSpecific, 0, 4);
			this.tableLayoutPanel4.Controls.Add(this.RbFromBeginning, 0, 1);
			this.tableLayoutPanel4.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel4.Controls.Add(this.LblBegin, 1, 1);
			this.tableLayoutPanel4.Controls.Add(this.UdSpecific, 1, 4);
			this.tableLayoutPanel4.Controls.Add(this.RbFromSent, 0, 3);
			this.tableLayoutPanel4.Controls.Add(this.LblSent, 1, 3);
			this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel4.Location = new System.Drawing.Point(5, 90);
			this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(5);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 5;
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel4.Size = new System.Drawing.Size(474, 121);
			this.tableLayoutPanel4.TabIndex = 0;
			// 
			// LblSomeLines
			// 
			this.LblSomeLines.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblSomeLines.AutoSize = true;
			this.LblSomeLines.Location = new System.Drawing.Point(384, 51);
			this.LblSomeLines.Name = "LblSomeLines";
			this.LblSomeLines.Size = new System.Drawing.Size(13, 13);
			this.LblSomeLines.TabIndex = 11;
			this.LblSomeLines.Text = "0";
			// 
			// RbSomeLines
			// 
			this.RbSomeLines.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.RbSomeLines.AutoSize = true;
			this.RbSomeLines.Location = new System.Drawing.Point(4, 49);
			this.RbSomeLines.Name = "RbSomeLines";
			this.RbSomeLines.Size = new System.Drawing.Size(190, 17);
			this.RbSomeLines.TabIndex = 10;
			this.RbSomeLines.Text = "Resume some lines before the stop";
			this.RbSomeLines.UseVisualStyleBackColor = true;
			this.RbSomeLines.CheckedChanged += new System.EventHandler(this.RbCheckedChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.BackColor = System.Drawing.Color.SteelBlue;
			this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label2.ForeColor = System.Drawing.Color.White;
			this.label2.Location = new System.Drawing.Point(381, 1);
			this.label2.Margin = new System.Windows.Forms.Padding(0);
			this.label2.MinimumSize = new System.Drawing.Size(0, 20);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(92, 20);
			this.label2.TabIndex = 5;
			this.label2.Text = "Line #";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// RbFromSpecific
			// 
			this.RbFromSpecific.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.RbFromSpecific.AutoSize = true;
			this.RbFromSpecific.Location = new System.Drawing.Point(4, 98);
			this.RbFromSpecific.Name = "RbFromSpecific";
			this.RbFromSpecific.Size = new System.Drawing.Size(145, 17);
			this.RbFromSpecific.TabIndex = 3;
			this.RbFromSpecific.Text = "Resume from specific line";
			this.RbFromSpecific.UseVisualStyleBackColor = true;
			this.RbFromSpecific.CheckedChanged += new System.EventHandler(this.RbCheckedChanged);
			// 
			// RbFromBeginning
			// 
			this.RbFromBeginning.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.RbFromBeginning.AutoSize = true;
			this.RbFromBeginning.Location = new System.Drawing.Point(4, 25);
			this.RbFromBeginning.Name = "RbFromBeginning";
			this.RbFromBeginning.Size = new System.Drawing.Size(177, 17);
			this.RbFromBeginning.TabIndex = 0;
			this.RbFromBeginning.Text = "Start again from beginning [safe]";
			this.RbFromBeginning.UseVisualStyleBackColor = true;
			this.RbFromBeginning.CheckedChanged += new System.EventHandler(this.RbCheckedChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.BackColor = System.Drawing.Color.SteelBlue;
			this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label1.ForeColor = System.Drawing.Color.White;
			this.label1.Location = new System.Drawing.Point(1, 1);
			this.label1.Margin = new System.Windows.Forms.Padding(0);
			this.label1.MinimumSize = new System.Drawing.Size(0, 20);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(379, 20);
			this.label1.TabIndex = 4;
			this.label1.Text = "Options";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// LblBegin
			// 
			this.LblBegin.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblBegin.AutoSize = true;
			this.LblBegin.Location = new System.Drawing.Point(384, 27);
			this.LblBegin.Name = "LblBegin";
			this.LblBegin.Size = new System.Drawing.Size(13, 13);
			this.LblBegin.TabIndex = 6;
			this.LblBegin.Text = "1";
			// 
			// UdSpecific
			// 
			this.UdSpecific.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.UdSpecific.Enabled = false;
			this.UdSpecific.Location = new System.Drawing.Point(384, 97);
			this.UdSpecific.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.UdSpecific.Name = "UdSpecific";
			this.UdSpecific.Size = new System.Drawing.Size(86, 20);
			this.UdSpecific.TabIndex = 9;
			this.UdSpecific.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// RbFromSent
			// 
			this.RbFromSent.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.RbFromSent.AutoSize = true;
			this.RbFromSent.Location = new System.Drawing.Point(4, 73);
			this.RbFromSent.Name = "RbFromSent";
			this.RbFromSent.Size = new System.Drawing.Size(178, 17);
			this.RbFromSent.TabIndex = 2;
			this.RbFromSent.Text = "Resume from last command sent";
			this.RbFromSent.UseVisualStyleBackColor = true;
			this.RbFromSent.CheckedChanged += new System.EventHandler(this.RbCheckedChanged);
			// 
			// LblSent
			// 
			this.LblSent.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblSent.AutoSize = true;
			this.LblSent.Location = new System.Drawing.Point(384, 75);
			this.LblSent.Name = "LblSent";
			this.LblSent.Size = new System.Drawing.Size(13, 13);
			this.LblSent.TabIndex = 7;
			this.LblSent.Text = "0";
			// 
			// LblBrief
			// 
			this.LblBrief.AutoSize = true;
			this.tableLayoutPanel3.SetColumnSpan(this.LblBrief, 2);
			this.LblBrief.Location = new System.Drawing.Point(3, 43);
			this.LblBrief.Margin = new System.Windows.Forms.Padding(3);
			this.LblBrief.Name = "LblBrief";
			this.LblBrief.Size = new System.Drawing.Size(476, 39);
			this.LblBrief.TabIndex = 4;
			this.LblBrief.Text = resources.GetString("LblBrief.Text");
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
			this.tableLayoutPanel5.Size = new System.Drawing.Size(440, 34);
			this.tableLayoutPanel5.TabIndex = 5;
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
			this.LblDetected.Size = new System.Drawing.Size(238, 13);
			this.LblDetected.TabIndex = 3;
			this.LblDetected.Text = "LaserGRBL detected last job was not completed!\r\n";
			// 
			// CbRestoreWCO
			// 
			this.CbRestoreWCO.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.CbRestoreWCO.AutoSize = true;
			this.CbRestoreWCO.Location = new System.Drawing.Point(123, 13);
			this.CbRestoreWCO.Name = "CbRestoreWCO";
			this.CbRestoreWCO.Size = new System.Drawing.Size(68, 17);
			this.CbRestoreWCO.TabIndex = 17;
			this.CbRestoreWCO.Text = "Home @";
			this.CbRestoreWCO.UseVisualStyleBackColor = true;
			// 
			// ResumeJobForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.CancelButton = this.BtnCancel;
			this.ClientSize = new System.Drawing.Size(490, 261);
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "ResumeJobForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Resume Job";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.tableLayoutPanel4.ResumeLayout(false);
			this.tableLayoutPanel4.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.UdSpecific)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.tableLayoutPanel5.ResumeLayout(false);
			this.tableLayoutPanel5.PerformLayout();
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
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private System.Windows.Forms.RadioButton RbFromBeginning;
		private System.Windows.Forms.RadioButton RbFromSent;
		private System.Windows.Forms.RadioButton RbFromSpecific;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label LblSent;
		private System.Windows.Forms.Label LblBegin;
		private System.Windows.Forms.NumericUpDown UdSpecific;
		private System.Windows.Forms.Button BtnCancel;
		private System.Windows.Forms.RadioButton RbSomeLines;
		private System.Windows.Forms.Label LblSomeLines;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
		private System.Windows.Forms.Label LblDetected;
		private System.Windows.Forms.Label TxtCause;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.CheckBox CbRedoHoming;
		private System.Windows.Forms.CheckBox CbRestoreWCO;
	}
}