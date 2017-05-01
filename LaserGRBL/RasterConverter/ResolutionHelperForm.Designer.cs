namespace LaserGRBL.RasterConverter
{
	partial class ResolutionHelperForm
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
			this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
			this.UDComputed = new System.Windows.Forms.NumericUpDown();
			this.UDHardware = new System.Windows.Forms.NumericUpDown();
			this.UDDesired = new System.Windows.Forms.NumericUpDown();
			this.LblBorderTracing = new System.Windows.Forms.Label();
			this.LblBorderTracingmm = new System.Windows.Forms.Label();
			this.label20 = new System.Windows.Forms.Label();
			this.label19 = new System.Windows.Forms.Label();
			this.LblLinearFillingmm = new System.Windows.Forms.Label();
			this.LblLinearFilling = new System.Windows.Forms.Label();
			this.linkLabel1 = new System.Windows.Forms.LinkLabel();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.BtnCancel = new System.Windows.Forms.Button();
			this.BtnCreate = new System.Windows.Forms.Button();
			this.tableLayoutPanel9.SuspendLayout();
			this.tableLayoutPanel6.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.UDComputed)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.UDHardware)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.UDDesired)).BeginInit();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel9
			// 
			this.tableLayoutPanel9.AutoSize = true;
			this.tableLayoutPanel9.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel9.ColumnCount = 1;
			this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel9.Controls.Add(this.tableLayoutPanel6, 0, 0);
			this.tableLayoutPanel9.Controls.Add(this.tableLayoutPanel1, 0, 1);
			this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel9.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel9.Name = "tableLayoutPanel9";
			this.tableLayoutPanel9.RowCount = 2;
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel9.Size = new System.Drawing.Size(304, 157);
			this.tableLayoutPanel9.TabIndex = 2;
			// 
			// tableLayoutPanel6
			// 
			this.tableLayoutPanel6.AutoSize = true;
			this.tableLayoutPanel6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel6.ColumnCount = 4;
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel6.Controls.Add(this.UDComputed, 1, 3);
			this.tableLayoutPanel6.Controls.Add(this.UDHardware, 1, 2);
			this.tableLayoutPanel6.Controls.Add(this.UDDesired, 1, 1);
			this.tableLayoutPanel6.Controls.Add(this.LblBorderTracing, 0, 1);
			this.tableLayoutPanel6.Controls.Add(this.LblBorderTracingmm, 2, 1);
			this.tableLayoutPanel6.Controls.Add(this.label20, 2, 3);
			this.tableLayoutPanel6.Controls.Add(this.label19, 0, 3);
			this.tableLayoutPanel6.Controls.Add(this.LblLinearFillingmm, 2, 2);
			this.tableLayoutPanel6.Controls.Add(this.LblLinearFilling, 0, 2);
			this.tableLayoutPanel6.Controls.Add(this.linkLabel1, 0, 0);
			this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel6.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel6.Name = "tableLayoutPanel6";
			this.tableLayoutPanel6.RowCount = 4;
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel6.Size = new System.Drawing.Size(298, 113);
			this.tableLayoutPanel6.TabIndex = 0;
			// 
			// UDComputed
			// 
			this.UDComputed.DecimalPlaces = 3;
			this.UDComputed.Enabled = false;
			this.UDComputed.Location = new System.Drawing.Point(141, 91);
			this.UDComputed.Margin = new System.Windows.Forms.Padding(2);
			this.UDComputed.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
			this.UDComputed.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.UDComputed.Name = "UDComputed";
			this.UDComputed.Size = new System.Drawing.Size(64, 20);
			this.UDComputed.TabIndex = 27;
			this.UDComputed.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
			// 
			// UDHardware
			// 
			this.UDHardware.DecimalPlaces = 3;
			this.UDHardware.Location = new System.Drawing.Point(141, 67);
			this.UDHardware.Margin = new System.Windows.Forms.Padding(2);
			this.UDHardware.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
			this.UDHardware.Name = "UDHardware";
			this.UDHardware.Size = new System.Drawing.Size(64, 20);
			this.UDHardware.TabIndex = 26;
			this.UDHardware.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
			this.UDHardware.ValueChanged += new System.EventHandler(this.Compute);
			// 
			// UDDesired
			// 
			this.UDDesired.DecimalPlaces = 3;
			this.UDDesired.Location = new System.Drawing.Point(141, 43);
			this.UDDesired.Margin = new System.Windows.Forms.Padding(2);
			this.UDDesired.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
			this.UDDesired.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.UDDesired.Name = "UDDesired";
			this.UDDesired.Size = new System.Drawing.Size(64, 20);
			this.UDDesired.TabIndex = 25;
			this.UDDesired.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
			this.UDDesired.ValueChanged += new System.EventHandler(this.Compute);
			// 
			// LblBorderTracing
			// 
			this.LblBorderTracing.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblBorderTracing.AutoSize = true;
			this.LblBorderTracing.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.LblBorderTracing.Location = new System.Drawing.Point(3, 46);
			this.LblBorderTracing.Name = "LblBorderTracing";
			this.LblBorderTracing.Size = new System.Drawing.Size(91, 13);
			this.LblBorderTracing.TabIndex = 23;
			this.LblBorderTracing.Text = "Desired resolution";
			// 
			// LblBorderTracingmm
			// 
			this.LblBorderTracingmm.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblBorderTracingmm.AutoSize = true;
			this.LblBorderTracingmm.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.LblBorderTracingmm.Location = new System.Drawing.Point(210, 46);
			this.LblBorderTracingmm.Name = "LblBorderTracingmm";
			this.LblBorderTracingmm.Size = new System.Drawing.Size(53, 13);
			this.LblBorderTracingmm.TabIndex = 22;
			this.LblBorderTracingmm.Text = "Lines/mm";
			// 
			// label20
			// 
			this.label20.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label20.AutoSize = true;
			this.label20.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label20.Location = new System.Drawing.Point(210, 94);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(53, 13);
			this.label20.TabIndex = 19;
			this.label20.Text = "Lines/min";
			// 
			// label19
			// 
			this.label19.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label19.AutoSize = true;
			this.label19.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.label19.Location = new System.Drawing.Point(3, 94);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(103, 13);
			this.label19.TabIndex = 17;
			this.label19.Text = "Computed resolution";
			// 
			// LblLinearFillingmm
			// 
			this.LblLinearFillingmm.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblLinearFillingmm.AutoSize = true;
			this.LblLinearFillingmm.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.LblLinearFillingmm.Location = new System.Drawing.Point(210, 70);
			this.LblLinearFillingmm.Name = "LblLinearFillingmm";
			this.LblLinearFillingmm.Size = new System.Drawing.Size(55, 13);
			this.LblLinearFillingmm.TabIndex = 15;
			this.LblLinearFillingmm.Text = "Steps/mm";
			// 
			// LblLinearFilling
			// 
			this.LblLinearFilling.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblLinearFilling.AutoSize = true;
			this.LblLinearFilling.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.LblLinearFilling.Location = new System.Drawing.Point(3, 70);
			this.LblLinearFilling.Name = "LblLinearFilling";
			this.LblLinearFilling.Size = new System.Drawing.Size(133, 13);
			this.LblLinearFilling.TabIndex = 13;
			this.LblLinearFilling.Text = "$100 Hardware Resolution";
			// 
			// linkLabel1
			// 
			this.linkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel6.SetColumnSpan(this.linkLabel1, 4);
			this.linkLabel1.Location = new System.Drawing.Point(3, 0);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.Size = new System.Drawing.Size(292, 41);
			this.linkLabel1.TabIndex = 24;
			this.linkLabel1.TabStop = true;
			this.linkLabel1.Text = "If resolution could not be achieved by full engine steps some artifacts may appea" +
    "r. Click here for more information...\r\n";
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.BtnCancel, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.BtnCreate, 2, 0);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 122);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(298, 32);
			this.tableLayoutPanel1.TabIndex = 2;
			// 
			// BtnCancel
			// 
			this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.BtnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.BtnCancel.Location = new System.Drawing.Point(141, 3);
			this.BtnCancel.Name = "BtnCancel";
			this.BtnCancel.Size = new System.Drawing.Size(74, 27);
			this.BtnCancel.TabIndex = 13;
			this.BtnCancel.Text = "Cancel";
			this.BtnCancel.UseVisualStyleBackColor = true;
			// 
			// BtnCreate
			// 
			this.BtnCreate.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.BtnCreate.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.BtnCreate.Location = new System.Drawing.Point(221, 3);
			this.BtnCreate.Name = "BtnCreate";
			this.BtnCreate.Size = new System.Drawing.Size(74, 27);
			this.BtnCreate.TabIndex = 5;
			this.BtnCreate.Text = "Set";
			this.BtnCreate.UseVisualStyleBackColor = true;
			this.BtnCreate.Click += new System.EventHandler(this.BtnCreate_Click);
			// 
			// ResolutionHelperForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(304, 157);
			this.Controls.Add(this.tableLayoutPanel9);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "ResolutionHelperForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Resolution Helper";
			this.tableLayoutPanel9.ResumeLayout(false);
			this.tableLayoutPanel9.PerformLayout();
			this.tableLayoutPanel6.ResumeLayout(false);
			this.tableLayoutPanel6.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.UDComputed)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.UDHardware)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.UDDesired)).EndInit();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
		private System.Windows.Forms.Label LblBorderTracing;
		private System.Windows.Forms.Label LblBorderTracingmm;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.Label LblLinearFillingmm;
		private System.Windows.Forms.Label LblLinearFilling;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Button BtnCancel;
		private System.Windows.Forms.Button BtnCreate;
		private System.Windows.Forms.LinkLabel linkLabel1;
		private System.Windows.Forms.NumericUpDown UDComputed;
		private System.Windows.Forms.NumericUpDown UDHardware;
		private System.Windows.Forms.NumericUpDown UDDesired;
	}
}