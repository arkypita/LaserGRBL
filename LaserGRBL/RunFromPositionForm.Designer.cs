namespace LaserGRBL
{
	partial class RunFromPositionForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RunFromPositionForm));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.BtnOK = new System.Windows.Forms.Button();
			this.BtnCancel = new System.Windows.Forms.Button();
			this.CbRedoHoming = new System.Windows.Forms.CheckBox();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.label2 = new System.Windows.Forms.Label();
			this.RbFromSpecific = new System.Windows.Forms.RadioButton();
			this.label1 = new System.Windows.Forms.Label();
			this.UdSpecific = new System.Windows.Forms.NumericUpDown();
			this.LblBrief = new System.Windows.Forms.Label();
			this.PBWarn = new System.Windows.Forms.PictureBox();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.UdSpecific)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.PBWarn)).BeginInit();
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
			this.tableLayoutPanel1.Size = new System.Drawing.Size(518, 142);
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
			this.tableLayoutPanel2.Controls.Add(this.BtnOK, 3, 0);
			this.tableLayoutPanel2.Controls.Add(this.BtnCancel, 2, 0);
			this.tableLayoutPanel2.Controls.Add(this.CbRedoHoming, 0, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 106);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.Size = new System.Drawing.Size(512, 33);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// BtnOK
			// 
			this.BtnOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.BtnOK.Location = new System.Drawing.Point(435, 3);
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
			this.BtnCancel.Location = new System.Drawing.Point(355, 3);
			this.BtnCancel.Name = "BtnCancel";
			this.BtnCancel.Size = new System.Drawing.Size(74, 27);
			this.BtnCancel.TabIndex = 15;
			this.BtnCancel.Text = "Cancel";
			this.BtnCancel.UseVisualStyleBackColor = true;
			this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
			// 
			// CbRedoHoming
			// 
			this.CbRedoHoming.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.CbRedoHoming.AutoSize = true;
			this.CbRedoHoming.Location = new System.Drawing.Point(3, 13);
			this.CbRedoHoming.Name = "CbRedoHoming";
			this.CbRedoHoming.Size = new System.Drawing.Size(102, 17);
			this.CbRedoHoming.TabIndex = 16;
			this.CbRedoHoming.Text = "Do Homing [$H]";
			this.CbRedoHoming.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.AutoSize = true;
			this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel3.ColumnCount = 2;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 0, 1);
			this.tableLayoutPanel3.Controls.Add(this.LblBrief, 1, 0);
			this.tableLayoutPanel3.Controls.Add(this.PBWarn, 0, 0);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 2;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.Size = new System.Drawing.Size(512, 97);
			this.tableLayoutPanel3.TabIndex = 1;
			// 
			// tableLayoutPanel4
			// 
			this.tableLayoutPanel4.AutoSize = true;
			this.tableLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel4.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
			this.tableLayoutPanel4.ColumnCount = 2;
			this.tableLayoutPanel3.SetColumnSpan(this.tableLayoutPanel4, 2);
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.Controls.Add(this.label2, 1, 0);
			this.tableLayoutPanel4.Controls.Add(this.RbFromSpecific, 0, 1);
			this.tableLayoutPanel4.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel4.Controls.Add(this.UdSpecific, 1, 1);
			this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel4.Location = new System.Drawing.Point(5, 43);
			this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(5);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 2;
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel4.Size = new System.Drawing.Size(502, 49);
			this.tableLayoutPanel4.TabIndex = 0;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.BackColor = System.Drawing.Color.SteelBlue;
			this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.label2.ForeColor = System.Drawing.Color.White;
			this.label2.Location = new System.Drawing.Point(409, 1);
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
			this.RbFromSpecific.Checked = true;
			this.RbFromSpecific.Location = new System.Drawing.Point(4, 26);
			this.RbFromSpecific.Name = "RbFromSpecific";
			this.RbFromSpecific.Size = new System.Drawing.Size(126, 17);
			this.RbFromSpecific.TabIndex = 3;
			this.RbFromSpecific.TabStop = true;
			this.RbFromSpecific.Text = "Run from specific line";
			this.RbFromSpecific.UseVisualStyleBackColor = true;
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
			this.label1.Size = new System.Drawing.Size(407, 20);
			this.label1.TabIndex = 4;
			this.label1.Text = "Options";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// UdSpecific
			// 
			this.UdSpecific.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.UdSpecific.Location = new System.Drawing.Point(412, 25);
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
			// LblBrief
			// 
			this.LblBrief.AutoSize = true;
			this.LblBrief.Location = new System.Drawing.Point(41, 3);
			this.LblBrief.Margin = new System.Windows.Forms.Padding(3);
			this.LblBrief.Name = "LblBrief";
			this.LblBrief.Size = new System.Drawing.Size(450, 26);
			this.LblBrief.TabIndex = 4;
			this.LblBrief.Text = "This feature allow to run your job starting from a specific line, skipping any pr" +
    "evious movement.\r\nLaserGRBL compute the exact position and state and put the eng" +
    "raver in the correct state.";
			// 
			// PBWarn
			// 
			this.PBWarn.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.PBWarn.Image = ((System.Drawing.Image)(resources.GetObject("PBWarn.Image")));
			this.PBWarn.Location = new System.Drawing.Point(3, 3);
			this.PBWarn.Name = "PBWarn";
			this.PBWarn.Size = new System.Drawing.Size(32, 32);
			this.PBWarn.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.PBWarn.TabIndex = 3;
			this.PBWarn.TabStop = false;
			// 
			// RunFromPositionForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.CancelButton = this.BtnCancel;
			this.ClientSize = new System.Drawing.Size(518, 142);
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "RunFromPositionForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Run From Position";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.tableLayoutPanel4.ResumeLayout(false);
			this.tableLayoutPanel4.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.UdSpecific)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.PBWarn)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Button BtnOK;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private System.Windows.Forms.RadioButton RbFromSpecific;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.NumericUpDown UdSpecific;
		private System.Windows.Forms.Button BtnCancel;
		private System.Windows.Forms.CheckBox CbRedoHoming;
		private System.Windows.Forms.Label LblBrief;
		private System.Windows.Forms.PictureBox PBWarn;
	}
}