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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResolutionHelperForm));
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
			resources.ApplyResources(this.tableLayoutPanel9, "tableLayoutPanel9");
			this.tableLayoutPanel9.Controls.Add(this.tableLayoutPanel6, 0, 0);
			this.tableLayoutPanel9.Controls.Add(this.tableLayoutPanel1, 0, 1);
			this.tableLayoutPanel9.Name = "tableLayoutPanel9";
			// 
			// tableLayoutPanel6
			// 
			resources.ApplyResources(this.tableLayoutPanel6, "tableLayoutPanel6");
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
			this.tableLayoutPanel6.Name = "tableLayoutPanel6";
			// 
			// UDComputed
			// 
			this.UDComputed.DecimalPlaces = 3;
			resources.ApplyResources(this.UDComputed, "UDComputed");
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
			this.UDComputed.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
			// 
			// UDHardware
			// 
			this.UDHardware.DecimalPlaces = 3;
			resources.ApplyResources(this.UDHardware, "UDHardware");
			this.UDHardware.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
			this.UDHardware.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
			this.UDHardware.Name = "UDHardware";
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
			resources.ApplyResources(this.UDDesired, "UDDesired");
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
			this.UDDesired.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
			this.UDDesired.ValueChanged += new System.EventHandler(this.Compute);
			// 
			// LblBorderTracing
			// 
			resources.ApplyResources(this.LblBorderTracing, "LblBorderTracing");
			this.LblBorderTracing.Name = "LblBorderTracing";
			// 
			// LblBorderTracingmm
			// 
			resources.ApplyResources(this.LblBorderTracingmm, "LblBorderTracingmm");
			this.LblBorderTracingmm.Name = "LblBorderTracingmm";
			// 
			// label20
			// 
			resources.ApplyResources(this.label20, "label20");
			this.label20.Name = "label20";
			// 
			// label19
			// 
			resources.ApplyResources(this.label19, "label19");
			this.label19.Name = "label19";
			// 
			// LblLinearFillingmm
			// 
			resources.ApplyResources(this.LblLinearFillingmm, "LblLinearFillingmm");
			this.LblLinearFillingmm.Name = "LblLinearFillingmm";
			// 
			// LblLinearFilling
			// 
			resources.ApplyResources(this.LblLinearFilling, "LblLinearFilling");
			this.LblLinearFilling.Name = "LblLinearFilling";
			// 
			// linkLabel1
			// 
			resources.ApplyResources(this.linkLabel1, "linkLabel1");
			this.tableLayoutPanel6.SetColumnSpan(this.linkLabel1, 4);
			this.linkLabel1.Name = "linkLabel1";
			this.linkLabel1.TabStop = true;
			this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
			// 
			// tableLayoutPanel1
			// 
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.BtnCancel, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.BtnCreate, 2, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			// 
			// BtnCancel
			// 
			this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			resources.ApplyResources(this.BtnCancel, "BtnCancel");
			this.BtnCancel.Name = "BtnCancel";
			this.BtnCancel.UseVisualStyleBackColor = true;
			// 
			// BtnCreate
			// 
			this.BtnCreate.DialogResult = System.Windows.Forms.DialogResult.OK;
			resources.ApplyResources(this.BtnCreate, "BtnCreate");
			this.BtnCreate.Name = "BtnCreate";
			this.BtnCreate.UseVisualStyleBackColor = true;
			this.BtnCreate.Click += new System.EventHandler(this.BtnCreate_Click);
			// 
			// ResolutionHelperForm
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel9);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "ResolutionHelperForm";
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