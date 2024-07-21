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
			this.BtnCancel = new LaserGRBL.UserControls.GrblButton();
			this.BtnCreate = new LaserGRBL.UserControls.GrblButton();
			this.label1 = new System.Windows.Forms.Label();
			this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
			this.label8 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.IiCrossSpeed = new LaserGRBL.UserControls.NumericInput.IntegerInputRanged();
			this.label4 = new System.Windows.Forms.Label();
			this.CbLimit = new LaserGRBL.UserControls.FlatComboBox();
			this.label6 = new System.Windows.Forms.Label();
			this.IiCrossPower = new LaserGRBL.UserControls.NumericInput.IntegerInputRanged();
			this.CbAxis = new LaserGRBL.UserControls.FlatComboBox();
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
			resources.ApplyResources(this.pictureBox1, "pictureBox1");
			this.pictureBox1.Name = "pictureBox1";
			this.tableLayoutPanel5.SetRowSpan(this.pictureBox1, 4);
			this.pictureBox1.TabStop = false;
			// 
			// tableLayoutPanel4
			// 
			resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
			this.tableLayoutPanel4.Controls.Add(this.BtnCancel, 1, 0);
			this.tableLayoutPanel4.Controls.Add(this.BtnCreate, 2, 0);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
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
			resources.ApplyResources(this.BtnCreate, "BtnCreate");
			this.BtnCreate.Name = "BtnCreate";
			this.BtnCreate.UseVisualStyleBackColor = true;
			this.BtnCreate.Click += new System.EventHandler(this.BtnCreate_Click);
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// tableLayoutPanel5
			// 
			resources.ApplyResources(this.tableLayoutPanel5, "tableLayoutPanel5");
			this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel4, 1, 3);
			this.tableLayoutPanel5.Controls.Add(this.label1, 1, 0);
			this.tableLayoutPanel5.Controls.Add(this.pictureBox1, 0, 0);
			this.tableLayoutPanel5.Controls.Add(this.tableLayoutPanel6, 1, 2);
			this.tableLayoutPanel5.Name = "tableLayoutPanel5";
			// 
			// tableLayoutPanel6
			// 
			resources.ApplyResources(this.tableLayoutPanel6, "tableLayoutPanel6");
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
			this.tableLayoutPanel6.Name = "tableLayoutPanel6";
			// 
			// label8
			// 
			resources.ApplyResources(this.label8, "label8");
			this.label8.Name = "label8";
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// label3
			// 
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			// 
			// label5
			// 
			resources.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			// 
			// IiCrossSpeed
			// 
			this.IiCrossSpeed.CurrentValue = 1000;
			this.IiCrossSpeed.ForcedText = null;
			this.IiCrossSpeed.ForceMinMax = false;
			resources.ApplyResources(this.IiCrossSpeed, "IiCrossSpeed");
			this.IiCrossSpeed.MaxValue = 10000;
			this.IiCrossSpeed.MinValue = 10;
			this.IiCrossSpeed.Name = "IiCrossSpeed";
			// 
			// label4
			// 
			resources.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			// 
			// CbLimit
			// 
			this.CbLimit.BackColor = System.Drawing.Color.White;
			this.CbLimit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CbLimit.ForeColor = System.Drawing.SystemColors.ControlText;
			this.CbLimit.FormattingEnabled = true;
			resources.ApplyResources(this.CbLimit, "CbLimit");
			this.CbLimit.Name = "CbLimit";
			// 
			// label6
			// 
			resources.ApplyResources(this.label6, "label6");
			this.label6.Name = "label6";
			// 
			// IiCrossPower
			// 
			this.IiCrossPower.CurrentValue = 100;
			this.IiCrossPower.ForcedText = null;
			this.IiCrossPower.ForceMinMax = false;
			resources.ApplyResources(this.IiCrossPower, "IiCrossPower");
			this.IiCrossPower.MaxValue = 1000;
			this.IiCrossPower.MinValue = 1;
			this.IiCrossPower.Name = "IiCrossPower";
			// 
			// CbAxis
			// 
			this.CbAxis.BackColor = System.Drawing.Color.White;
			this.CbAxis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CbAxis.ForeColor = System.Drawing.SystemColors.ControlText;
			this.CbAxis.FormattingEnabled = true;
			this.CbAxis.Items.AddRange(new object[] {
            resources.GetString("CbAxis.Items"),
            resources.GetString("CbAxis.Items1")});
			resources.ApplyResources(this.CbAxis, "CbAxis");
			this.CbAxis.Name = "CbAxis";
			this.CbAxis.SelectedIndexChanged += new System.EventHandler(this.CbAxis_SelectedIndexChanged);
			// 
			// label7
			// 
			resources.ApplyResources(this.label7, "label7");
			this.label7.Name = "label7";
			// 
			// IiAxisLen
			// 
			resources.ApplyResources(this.IiAxisLen, "IiAxisLen");
			this.IiAxisLen.CurrentValue = 100;
			this.IiAxisLen.ForcedText = null;
			this.IiAxisLen.ForceMinMax = false;
			this.IiAxisLen.MinValue = 50;
			this.IiAxisLen.Name = "IiAxisLen";
			// 
			// ShakeTest
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.BtnCancel;
			this.Controls.Add(this.tableLayoutPanel5);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "ShakeTest";
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
		private LaserGRBL.UserControls.GrblButton BtnCancel;
		private LaserGRBL.UserControls.GrblButton BtnCreate;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label4;
		private LaserGRBL.UserControls.FlatComboBox CbAxis;
		private LaserGRBL.UserControls.FlatComboBox CbLimit;
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