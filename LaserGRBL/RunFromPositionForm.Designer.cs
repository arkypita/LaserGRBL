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
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			// 
			// tableLayoutPanel2
			// 
			resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
			this.tableLayoutPanel2.Controls.Add(this.BtnOK, 3, 0);
			this.tableLayoutPanel2.Controls.Add(this.BtnCancel, 2, 0);
			this.tableLayoutPanel2.Controls.Add(this.CbRedoHoming, 0, 0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			// 
			// BtnOK
			// 
			resources.ApplyResources(this.BtnOK, "BtnOK");
			this.BtnOK.Name = "BtnOK";
			this.BtnOK.UseVisualStyleBackColor = true;
			this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
			// 
			// BtnCancel
			// 
			this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			resources.ApplyResources(this.BtnCancel, "BtnCancel");
			this.BtnCancel.Name = "BtnCancel";
			this.BtnCancel.UseVisualStyleBackColor = true;
			this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
			// 
			// CbRedoHoming
			// 
			resources.ApplyResources(this.CbRedoHoming, "CbRedoHoming");
			this.CbRedoHoming.Name = "CbRedoHoming";
			this.CbRedoHoming.UseVisualStyleBackColor = true;
			// 
			// tableLayoutPanel3
			// 
			resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
			this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 0, 1);
			this.tableLayoutPanel3.Controls.Add(this.LblBrief, 1, 0);
			this.tableLayoutPanel3.Controls.Add(this.PBWarn, 0, 0);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			// 
			// tableLayoutPanel4
			// 
			resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
			this.tableLayoutPanel3.SetColumnSpan(this.tableLayoutPanel4, 2);
			this.tableLayoutPanel4.Controls.Add(this.label2, 1, 0);
			this.tableLayoutPanel4.Controls.Add(this.RbFromSpecific, 0, 1);
			this.tableLayoutPanel4.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel4.Controls.Add(this.UdSpecific, 1, 1);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.BackColor = System.Drawing.Color.SteelBlue;
			this.label2.ForeColor = System.Drawing.Color.White;
			this.label2.Name = "label2";
			// 
			// RbFromSpecific
			// 
			resources.ApplyResources(this.RbFromSpecific, "RbFromSpecific");
			this.RbFromSpecific.Checked = true;
			this.RbFromSpecific.Name = "RbFromSpecific";
			this.RbFromSpecific.TabStop = true;
			this.RbFromSpecific.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.BackColor = System.Drawing.Color.SteelBlue;
			this.label1.ForeColor = System.Drawing.Color.White;
			this.label1.Name = "label1";
			// 
			// UdSpecific
			// 
			resources.ApplyResources(this.UdSpecific, "UdSpecific");
			this.UdSpecific.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.UdSpecific.Name = "UdSpecific";
			this.UdSpecific.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// LblBrief
			// 
			resources.ApplyResources(this.LblBrief, "LblBrief");
			this.LblBrief.Name = "LblBrief";
			// 
			// PBWarn
			// 
			resources.ApplyResources(this.PBWarn, "PBWarn");
			this.PBWarn.Name = "PBWarn";
			this.PBWarn.TabStop = false;
			// 
			// RunFromPositionForm
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.BtnCancel;
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "RunFromPositionForm";
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