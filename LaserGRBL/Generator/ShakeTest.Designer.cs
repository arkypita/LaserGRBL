namespace LaserGRBL.Generator
{
	partial class ShakeTest
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShakeTest));
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.BtnCancel = new System.Windows.Forms.Button();
			this.BtnCreate = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
			this.label8 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.IiCrossSpeed = new LaserGRBL.UserControls.NumericInput.IntegerInputRanged();
			this.label4 = new System.Windows.Forms.Label();
			this.CbLimit = new System.Windows.Forms.ComboBox();
			this.label6 = new System.Windows.Forms.Label();
			this.IiCrossPower = new LaserGRBL.UserControls.NumericInput.IntegerInputRanged();
			this.CbAxis = new System.Windows.Forms.ComboBox();
			this.label7 = new System.Windows.Forms.Label();
			this.IiAxisLen = new LaserGRBL.UserControls.NumericInput.IntegerInputRanged();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.tableLayoutPanel4.SuspendLayout();
			this.tableLayoutPanel5.SuspendLayout();
			this.tableLayoutPanel6.SuspendLayout();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.BackColor = System.Drawing.Color.White;
			this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(3, 3);
			this.pictureBox1.Name = "pictureBox1";
			this.tableLayoutPanel5.SetRowSpan(this.pictureBox1, 4);
			this.pictureBox1.Size = new System.Drawing.Size(300, 366);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// tableLayoutPanel4
			// 
			this.tableLayoutPanel4.AutoSize = true;
			this.tableLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel4.ColumnCount = 3;
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.Controls.Add(this.BtnCancel, 1, 0);
			this.tableLayoutPanel4.Controls.Add(this.BtnCreate, 2, 0);
			this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel4.Location = new System.Drawing.Point(309, 317);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 1;
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.Size = new System.Drawing.Size(609, 52);
			this.tableLayoutPanel4.TabIndex = 6;
			// 
			// BtnCancel
			// 
			this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.BtnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.BtnCancel.Location = new System.Drawing.Point(452, 3);
			this.BtnCancel.Name = "BtnCancel";
			this.BtnCancel.Size = new System.Drawing.Size(74, 27);
			this.BtnCancel.TabIndex = 6;
			this.BtnCancel.Text = "Cancel";
			this.BtnCancel.UseVisualStyleBackColor = true;
			// 
			// BtnCreate
			// 
			this.BtnCreate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.BtnCreate.Location = new System.Drawing.Point(532, 3);
			this.BtnCreate.Name = "BtnCreate";
			this.BtnCreate.Size = new System.Drawing.Size(74, 27);
			this.BtnCreate.TabIndex = 7;
			this.BtnCreate.Text = "Create!";
			this.BtnCreate.UseVisualStyleBackColor = true;
			this.BtnCreate.Click += new System.EventHandler(this.BtnCreate_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(310, 4);
			this.label1.Margin = new System.Windows.Forms.Padding(4);
			this.label1.MaximumSize = new System.Drawing.Size(600, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(598, 247);
			this.label1.TabIndex = 0;
			this.label1.Text = resources.GetString("label1.Text");
			// 
			// tableLayoutPanel5
			// 
			this.tableLayoutPanel5.AutoSize = true;
			this.tableLayoutPanel5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel5.ColumnCount = 2;
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel4, 1, 3);
			this.tableLayoutPanel5.Controls.Add(this.label1, 1, 0);
			this.tableLayoutPanel5.Controls.Add(this.pictureBox1, 0, 0);
			this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel6, 1, 2);
			this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel5.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel5.Name = "tableLayoutPanel5";
			this.tableLayoutPanel5.RowCount = 4;
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5F));
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel5.Size = new System.Drawing.Size(921, 372);
			this.tableLayoutPanel5.TabIndex = 1;
			// 
			// tableLayoutPanel6
			// 
			this.tableLayoutPanel6.AutoSize = true;
			this.tableLayoutPanel6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel6.ColumnCount = 7;
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel6.Controls.Add(this.label8, 3, 0);
			this.tableLayoutPanel6.Controls.Add(this.label2, 0, 0);
			this.tableLayoutPanel6.Controls.Add(this.label3, 0, 1);
			this.tableLayoutPanel6.Controls.Add(this.label5, 1, 1);
			this.tableLayoutPanel6.Controls.Add(this.IiCrossSpeed, 2, 1);
			this.tableLayoutPanel6.Controls.Add(this.label4, 5, 0);
			this.tableLayoutPanel6.Controls.Add(this.CbLimit, 6, 0);
			this.tableLayoutPanel6.Controls.Add(this.label6, 3, 1);
			this.tableLayoutPanel6.Controls.Add(this.IiCrossPower, 4, 1);
			this.tableLayoutPanel6.Controls.Add(this.CbAxis, 2, 0);
			this.tableLayoutPanel6.Controls.Add(this.label7, 1, 0);
			this.tableLayoutPanel6.Controls.Add(this.IiAxisLen, 4, 0);
			this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel6.Location = new System.Drawing.Point(309, 263);
			this.tableLayoutPanel6.Name = "tableLayoutPanel6";
			this.tableLayoutPanel6.RowCount = 2;
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel6.Size = new System.Drawing.Size(609, 48);
			this.tableLayoutPanel6.TabIndex = 1;
			// 
			// label8
			// 
			this.label8.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(202, 7);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(40, 13);
			this.label8.TabIndex = 10;
			this.label8.Text = "Lenght";
			// 
			// label2
			// 
			this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 7);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(51, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "Test type";
			// 
			// label3
			// 
			this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(3, 31);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(83, 13);
			this.label3.TabIndex = 1;
			this.label3.Text = "Cross engraving";
			// 
			// label5
			// 
			this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(92, 31);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(38, 13);
			this.label5.TabIndex = 4;
			this.label5.Text = "Speed";
			// 
			// IiCrossSpeed
			// 
			this.IiCrossSpeed.CurrentValue = 1000;
			this.IiCrossSpeed.ForcedText = null;
			this.IiCrossSpeed.ForceMinMax = false;
			this.IiCrossSpeed.Location = new System.Drawing.Point(136, 30);
			this.IiCrossSpeed.MaxValue = 10000;
			this.IiCrossSpeed.MinValue = 10;
			this.IiCrossSpeed.Name = "IiCrossSpeed";
			this.IiCrossSpeed.Size = new System.Drawing.Size(60, 15);
			this.IiCrossSpeed.TabIndex = 4;
			// 
			// label4
			// 
			this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(314, 7);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(60, 13);
			this.label4.TabIndex = 2;
			this.label4.Text = "Limit speed";
			// 
			// CbLimit
			// 
			this.CbLimit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CbLimit.FormattingEnabled = true;
			this.CbLimit.Location = new System.Drawing.Point(380, 3);
			this.CbLimit.MaxDropDownItems = 6;
			this.CbLimit.Name = "CbLimit";
			this.CbLimit.Size = new System.Drawing.Size(146, 21);
			this.CbLimit.TabIndex = 3;
			// 
			// label6
			// 
			this.label6.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(202, 31);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(37, 13);
			this.label6.TabIndex = 7;
			this.label6.Text = "Power";
			// 
			// IiCrossPower
			// 
			this.IiCrossPower.CurrentValue = 100;
			this.IiCrossPower.ForcedText = null;
			this.IiCrossPower.ForceMinMax = false;
			this.IiCrossPower.Location = new System.Drawing.Point(248, 30);
			this.IiCrossPower.MaxValue = 1000;
			this.IiCrossPower.MinValue = 1;
			this.IiCrossPower.Name = "IiCrossPower";
			this.IiCrossPower.Size = new System.Drawing.Size(60, 15);
			this.IiCrossPower.TabIndex = 5;
			// 
			// CbAxis
			// 
			this.CbAxis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CbAxis.FormattingEnabled = true;
			this.CbAxis.Items.AddRange(new object[] {
            "X",
            "Y"});
			this.CbAxis.Location = new System.Drawing.Point(136, 3);
			this.CbAxis.Name = "CbAxis";
			this.CbAxis.Size = new System.Drawing.Size(60, 21);
			this.CbAxis.TabIndex = 1;
			this.CbAxis.SelectedIndexChanged += new System.EventHandler(this.CbAxis_SelectedIndexChanged);
			// 
			// label7
			// 
			this.label7.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(92, 7);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(26, 13);
			this.label7.TabIndex = 9;
			this.label7.Text = "Axis";
			// 
			// IiAxisLen
			// 
			this.IiAxisLen.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.IiAxisLen.CurrentValue = 100;
			this.IiAxisLen.ForcedText = null;
			this.IiAxisLen.ForceMinMax = false;
			this.IiAxisLen.Location = new System.Drawing.Point(248, 6);
			this.IiAxisLen.MinValue = 50;
			this.IiAxisLen.Name = "IiAxisLen";
			this.IiAxisLen.Size = new System.Drawing.Size(60, 15);
			this.IiAxisLen.TabIndex = 2;
			// 
			// ShakeTest
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.CancelButton = this.BtnCancel;
			this.ClientSize = new System.Drawing.Size(921, 372);
			this.Controls.Add(this.tableLayoutPanel5);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "ShakeTest";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Shake Test";
			this.Load += new System.EventHandler(this.ShakeTest_Load);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.tableLayoutPanel4.ResumeLayout(false);
			this.tableLayoutPanel5.ResumeLayout(false);
			this.tableLayoutPanel5.PerformLayout();
			this.tableLayoutPanel6.ResumeLayout(false);
			this.tableLayoutPanel6.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private System.Windows.Forms.Button BtnCancel;
		private System.Windows.Forms.Button BtnCreate;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ComboBox CbAxis;
		private System.Windows.Forms.ComboBox CbLimit;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label5;
		private UserControls.NumericInput.IntegerInputRanged IiCrossSpeed;
		private System.Windows.Forms.Label label6;
		private UserControls.NumericInput.IntegerInputRanged IiCrossPower;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private UserControls.NumericInput.IntegerInputRanged IiAxisLen;
	}
}