namespace LaserGRBL.Generator
{
	partial class CuttingTest
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CuttingTest));
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.BtnCancel = new LaserGRBL.UserControls.GrblButton();
			this.BtnCreate = new LaserGRBL.UserControls.GrblButton();
			this.label1 = new System.Windows.Forms.Label();
			this.GbParameters = new LaserGRBL.UserControls.GrblGroupBox();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.Ii_Sfixed = new LaserGRBL.UserControls.NumericInput.IntegerInputRanged();
			this.CBLaserON = new LaserGRBL.UserControls.FlatComboBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.Ii_Pmin = new LaserGRBL.UserControls.NumericInput.IntegerInputRanged();
			this.label4 = new System.Windows.Forms.Label();
			this.Ii_Pmax = new LaserGRBL.UserControls.NumericInput.IntegerInputRanged();
			this.label8 = new System.Windows.Forms.Label();
			this.Ii_Fmin = new LaserGRBL.UserControls.NumericInput.IntegerInputRanged();
			this.Ii_Fmax = new LaserGRBL.UserControls.NumericInput.IntegerInputRanged();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.Ii_Fsteps = new LaserGRBL.UserControls.NumericInput.IntegerInputRanged();
			this.label11 = new System.Windows.Forms.Label();
			this.LblLaserMode = new System.Windows.Forms.Label();
			this.BtnOnOffInfo = new LaserGRBL.UserControls.ImageButton();
			this.Ii_FStepSize = new LaserGRBL.UserControls.NumericInput.IntegerInputRanged();
			this.label12 = new System.Windows.Forms.Label();
			this.label15 = new System.Windows.Forms.Label();
			this.label18 = new System.Windows.Forms.Label();
			this.label16 = new System.Windows.Forms.Label();
			this.Ii_Ftext = new LaserGRBL.UserControls.NumericInput.IntegerInputRanged();
			this.IISizeW = new LaserGRBL.UserControls.NumericInput.DecimalInputRanged();
			this.label19 = new System.Windows.Forms.Label();
			this.label17 = new System.Windows.Forms.Label();
			this.Ii_Stext = new LaserGRBL.UserControls.NumericInput.IntegerInputRanged();
			this.IISizeH = new LaserGRBL.UserControls.NumericInput.DecimalInputRanged();
			this.PhTbLabel = new LaserGRBL.UserControls.PlaceholderTextBox();
			this.label13 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			this.GbParameters.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			resources.ApplyResources(this.pictureBox1, "pictureBox1");
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.TabStop = false;
			// 
			// tableLayoutPanel1
			// 
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 1, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			// 
			// tableLayoutPanel2
			// 
			resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
			this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel4, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.GbParameters, 0, 1);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
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
			// GbParameters
			// 
			resources.ApplyResources(this.GbParameters, "GbParameters");
			this.GbParameters.Controls.Add(this.tableLayoutPanel3);
			this.GbParameters.Name = "GbParameters";
			this.GbParameters.TabStop = false;
			// 
			// tableLayoutPanel3
			// 
			resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
			this.tableLayoutPanel3.Controls.Add(this.Ii_Sfixed, 2, 0);
			this.tableLayoutPanel3.Controls.Add(this.CBLaserON, 2, 4);
			this.tableLayoutPanel3.Controls.Add(this.label7, 0, 2);
			this.tableLayoutPanel3.Controls.Add(this.label2, 0, 1);
			this.tableLayoutPanel3.Controls.Add(this.label3, 1, 1);
			this.tableLayoutPanel3.Controls.Add(this.Ii_Pmin, 2, 1);
			this.tableLayoutPanel3.Controls.Add(this.label4, 3, 1);
			this.tableLayoutPanel3.Controls.Add(this.Ii_Pmax, 4, 1);
			this.tableLayoutPanel3.Controls.Add(this.label8, 1, 2);
			this.tableLayoutPanel3.Controls.Add(this.Ii_Fmin, 2, 2);
			this.tableLayoutPanel3.Controls.Add(this.Ii_Fmax, 4, 2);
			this.tableLayoutPanel3.Controls.Add(this.label9, 3, 2);
			this.tableLayoutPanel3.Controls.Add(this.label10, 5, 2);
			this.tableLayoutPanel3.Controls.Add(this.Ii_Fsteps, 6, 2);
			this.tableLayoutPanel3.Controls.Add(this.label11, 7, 2);
			this.tableLayoutPanel3.Controls.Add(this.LblLaserMode, 0, 4);
			this.tableLayoutPanel3.Controls.Add(this.BtnOnOffInfo, 4, 4);
			this.tableLayoutPanel3.Controls.Add(this.Ii_FStepSize, 8, 2);
			this.tableLayoutPanel3.Controls.Add(this.label12, 0, 6);
			this.tableLayoutPanel3.Controls.Add(this.label15, 0, 8);
			this.tableLayoutPanel3.Controls.Add(this.label18, 1, 6);
			this.tableLayoutPanel3.Controls.Add(this.label16, 1, 8);
			this.tableLayoutPanel3.Controls.Add(this.Ii_Ftext, 2, 6);
			this.tableLayoutPanel3.Controls.Add(this.IISizeW, 2, 8);
			this.tableLayoutPanel3.Controls.Add(this.label19, 3, 6);
			this.tableLayoutPanel3.Controls.Add(this.label17, 3, 8);
			this.tableLayoutPanel3.Controls.Add(this.Ii_Stext, 4, 6);
			this.tableLayoutPanel3.Controls.Add(this.IISizeH, 4, 8);
			this.tableLayoutPanel3.Controls.Add(this.PhTbLabel, 6, 6);
			this.tableLayoutPanel3.Controls.Add(this.label13, 0, 0);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			// 
			// Ii_Sfixed
			// 
			resources.ApplyResources(this.Ii_Sfixed, "Ii_Sfixed");
			this.Ii_Sfixed.ForcedText = null;
			this.Ii_Sfixed.ForceMinMax = false;
			this.Ii_Sfixed.MaxValue = 1000;
			this.Ii_Sfixed.MinValue = 1;
			this.Ii_Sfixed.Name = "Ii_Sfixed";
			// 
			// CBLaserON
			// 
			this.CBLaserON.BackColor = System.Drawing.Color.White;
			this.tableLayoutPanel3.SetColumnSpan(this.CBLaserON, 3);
			resources.ApplyResources(this.CBLaserON, "CBLaserON");
			this.CBLaserON.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CBLaserON.ForeColor = System.Drawing.SystemColors.ControlText;
			this.CBLaserON.FormattingEnabled = true;
			this.CBLaserON.Name = "CBLaserON";
			// 
			// label7
			// 
			resources.ApplyResources(this.label7, "label7");
			this.label7.Name = "label7";
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
			// Ii_Pmin
			// 
			resources.ApplyResources(this.Ii_Pmin, "Ii_Pmin");
			this.Ii_Pmin.ForcedText = null;
			this.Ii_Pmin.ForceMinMax = true;
			this.Ii_Pmin.MaxValue = 20;
			this.Ii_Pmin.MinValue = 1;
			this.Ii_Pmin.Name = "Ii_Pmin";
			this.Ii_Pmin.CurrentValueChanged += new LaserGRBL.UserControls.NumericInput.IntegerInputBase.CurrentValueChangedEventHandler(this.Ii_PCurrentValueChanged);
			// 
			// label4
			// 
			resources.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			// 
			// Ii_Pmax
			// 
			resources.ApplyResources(this.Ii_Pmax, "Ii_Pmax");
			this.Ii_Pmax.ForcedText = null;
			this.Ii_Pmax.ForceMinMax = true;
			this.Ii_Pmax.MaxValue = 20;
			this.Ii_Pmax.MinValue = 1;
			this.Ii_Pmax.Name = "Ii_Pmax";
			this.Ii_Pmax.CurrentValueChanged += new LaserGRBL.UserControls.NumericInput.IntegerInputBase.CurrentValueChangedEventHandler(this.Ii_PCurrentValueChanged);
			// 
			// label8
			// 
			resources.ApplyResources(this.label8, "label8");
			this.label8.Name = "label8";
			// 
			// Ii_Fmin
			// 
			resources.ApplyResources(this.Ii_Fmin, "Ii_Fmin");
			this.Ii_Fmin.ForcedText = null;
			this.Ii_Fmin.ForceMinMax = false;
			this.Ii_Fmin.MaxValue = 1000;
			this.Ii_Fmin.MinValue = 0;
			this.Ii_Fmin.Name = "Ii_Fmin";
			this.Ii_Fmin.CurrentValueChanged += new LaserGRBL.UserControls.NumericInput.IntegerInputBase.CurrentValueChangedEventHandler(this.IiF_CurrentValueChanged);
			// 
			// Ii_Fmax
			// 
			resources.ApplyResources(this.Ii_Fmax, "Ii_Fmax");
			this.Ii_Fmax.ForcedText = null;
			this.Ii_Fmax.ForceMinMax = false;
			this.Ii_Fmax.MaxValue = 1000;
			this.Ii_Fmax.MinValue = 0;
			this.Ii_Fmax.Name = "Ii_Fmax";
			this.Ii_Fmax.CurrentValueChanged += new LaserGRBL.UserControls.NumericInput.IntegerInputBase.CurrentValueChangedEventHandler(this.IiF_CurrentValueChanged);
			// 
			// label9
			// 
			resources.ApplyResources(this.label9, "label9");
			this.label9.Name = "label9";
			// 
			// label10
			// 
			resources.ApplyResources(this.label10, "label10");
			this.label10.Name = "label10";
			// 
			// Ii_Fsteps
			// 
			resources.ApplyResources(this.Ii_Fsteps, "Ii_Fsteps");
			this.Ii_Fsteps.CurrentValue = 11;
			this.Ii_Fsteps.ForcedText = null;
			this.Ii_Fsteps.ForceMinMax = true;
			this.Ii_Fsteps.MaxValue = 20;
			this.Ii_Fsteps.MinValue = 1;
			this.Ii_Fsteps.Name = "Ii_Fsteps";
			this.Ii_Fsteps.CurrentValueChanged += new LaserGRBL.UserControls.NumericInput.IntegerInputBase.CurrentValueChangedEventHandler(this.IiF_CurrentValueChanged);
			// 
			// label11
			// 
			resources.ApplyResources(this.label11, "label11");
			this.label11.Name = "label11";
			// 
			// LblLaserMode
			// 
			resources.ApplyResources(this.LblLaserMode, "LblLaserMode");
			this.LblLaserMode.Name = "LblLaserMode";
			// 
			// BtnOnOffInfo
			// 
			this.BtnOnOffInfo.AltImage = null;
			resources.ApplyResources(this.BtnOnOffInfo, "BtnOnOffInfo");
			this.BtnOnOffInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtnOnOffInfo.Caption = null;
			this.BtnOnOffInfo.Cursor = System.Windows.Forms.Cursors.Hand;
			this.BtnOnOffInfo.Image = ((System.Drawing.Image)(resources.GetObject("BtnOnOffInfo.Image")));
			this.BtnOnOffInfo.Name = "BtnOnOffInfo";
			this.BtnOnOffInfo.RoundedBorders = false;
			this.BtnOnOffInfo.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.BtnOnOffInfo.UseAltImage = false;
			this.BtnOnOffInfo.Click += new System.EventHandler(this.BtnOnOffInfo_Click);
			// 
			// Ii_FStepSize
			// 
			resources.ApplyResources(this.Ii_FStepSize, "Ii_FStepSize");
			this.Ii_FStepSize.ForcedText = null;
			this.Ii_FStepSize.ForceMinMax = false;
			this.Ii_FStepSize.MaxValue = 1000;
			this.Ii_FStepSize.MinValue = 0;
			this.Ii_FStepSize.Name = "Ii_FStepSize";
			// 
			// label12
			// 
			resources.ApplyResources(this.label12, "label12");
			this.label12.Name = "label12";
			// 
			// label15
			// 
			resources.ApplyResources(this.label15, "label15");
			this.label15.Name = "label15";
			// 
			// label18
			// 
			resources.ApplyResources(this.label18, "label18");
			this.label18.Name = "label18";
			// 
			// label16
			// 
			resources.ApplyResources(this.label16, "label16");
			this.label16.Name = "label16";
			// 
			// Ii_Ftext
			// 
			resources.ApplyResources(this.Ii_Ftext, "Ii_Ftext");
			this.Ii_Ftext.ForcedText = null;
			this.Ii_Ftext.ForceMinMax = false;
			this.Ii_Ftext.MaxValue = 1000;
			this.Ii_Ftext.MinValue = 0;
			this.Ii_Ftext.Name = "Ii_Ftext";
			// 
			// IISizeW
			// 
			this.IISizeW.CurrentValue = 100F;
			this.IISizeW.DecimalPositions = 1;
			resources.ApplyResources(this.IISizeW, "IISizeW");
			this.IISizeW.ForceMinMax = false;
			this.IISizeW.MaxValue = 1000F;
			this.IISizeW.MinValue = 0F;
			this.IISizeW.Name = "IISizeW";
			// 
			// label19
			// 
			resources.ApplyResources(this.label19, "label19");
			this.label19.Name = "label19";
			// 
			// label17
			// 
			resources.ApplyResources(this.label17, "label17");
			this.label17.Name = "label17";
			// 
			// Ii_Stext
			// 
			resources.ApplyResources(this.Ii_Stext, "Ii_Stext");
			this.Ii_Stext.ForcedText = null;
			this.Ii_Stext.ForceMinMax = false;
			this.Ii_Stext.MaxValue = 1000;
			this.Ii_Stext.MinValue = 0;
			this.Ii_Stext.Name = "Ii_Stext";
			// 
			// IISizeH
			// 
			this.IISizeH.CurrentValue = 100F;
			this.IISizeH.DecimalPositions = 1;
			resources.ApplyResources(this.IISizeH, "IISizeH");
			this.IISizeH.ForceMinMax = false;
			this.IISizeH.MaxValue = 1000F;
			this.IISizeH.MinValue = 0F;
			this.IISizeH.Name = "IISizeH";
			// 
			// PhTbLabel
			// 
			this.PhTbLabel.BackColor = System.Drawing.Color.White;
			this.PhTbLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.tableLayoutPanel3.SetColumnSpan(this.PhTbLabel, 3);
			resources.ApplyResources(this.PhTbLabel, "PhTbLabel");
			this.PhTbLabel.ForeColor = System.Drawing.SystemColors.ControlText;
			this.PhTbLabel.Name = "PhTbLabel";
			this.PhTbLabel.WaterMarkActiveColor = System.Drawing.Color.Gray;
			this.PhTbLabel.WaterMarkActiveForeColor = System.Drawing.Color.Gray;
			this.PhTbLabel.WaterMarkColor = System.Drawing.Color.LightGray;
			this.PhTbLabel.WaterMarkFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.PhTbLabel.WaterMarkForeColor = System.Drawing.Color.LightGray;
			// 
			// label13
			// 
			resources.ApplyResources(this.label13, "label13");
			this.label13.Name = "label13";
			// 
			// CuttingTest
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.BtnCancel;
			this.Controls.Add(this.tableLayoutPanel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "CuttingTest";
			this.Load += new System.EventHandler(this.GreyScaleForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.tableLayoutPanel4.ResumeLayout(false);
			this.GbParameters.ResumeLayout(false);
			this.GbParameters.PerformLayout();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Label label1;
		private LaserGRBL.UserControls.GrblGroupBox GbParameters;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.Label label2;
		private UserControls.NumericInput.IntegerInputRanged Ii_Pmin;
		private System.Windows.Forms.Label label4;
		private UserControls.NumericInput.IntegerInputRanged Ii_Pmax;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private UserControls.NumericInput.IntegerInputRanged Ii_Fmin;
		private UserControls.NumericInput.IntegerInputRanged Ii_Fmax;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private UserControls.NumericInput.IntegerInputRanged Ii_Fsteps;
		private System.Windows.Forms.Label label11;
		private LaserGRBL.UserControls.FlatComboBox CBLaserON;
		private System.Windows.Forms.Label LblLaserMode;
		private UserControls.ImageButton BtnOnOffInfo;
		private System.Windows.Forms.Label label15;
		private System.Windows.Forms.Label label16;
		private UserControls.NumericInput.DecimalInputRanged IISizeW;
		private UserControls.NumericInput.DecimalInputRanged IISizeH;
		private System.Windows.Forms.Label label17;
		private UserControls.NumericInput.IntegerInputRanged Ii_FStepSize;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private LaserGRBL.UserControls.GrblButton BtnCancel;
		private LaserGRBL.UserControls.GrblButton BtnCreate;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.Label label18;
		private UserControls.NumericInput.IntegerInputRanged Ii_Ftext;
		private System.Windows.Forms.Label label19;
		private UserControls.NumericInput.IntegerInputRanged Ii_Stext;
		private UserControls.PlaceholderTextBox PhTbLabel;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label13;
		private UserControls.NumericInput.IntegerInputRanged Ii_Sfixed;
	}
}