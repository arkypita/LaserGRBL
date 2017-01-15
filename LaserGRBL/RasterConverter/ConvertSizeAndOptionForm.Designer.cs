/*
 * Created by SharpDevelop.
 * User: Diego
 * Date: 15/01/2017
 * Time: 12:12
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace LaserGRBL.RasterConverter
{
	partial class ConvertSizeAndOptionForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label4;
		private LaserGRBL.UserControls.IntegerInput.IntegerInputRanged IIOffsetX;
		private LaserGRBL.UserControls.IntegerInput.IntegerInputRanged IIOffsetY;
		private LaserGRBL.UserControls.IntegerInput.IntegerInputRanged IISizeH;
		private LaserGRBL.UserControls.IntegerInput.IntegerInputRanged IISizeW;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
		private System.Windows.Forms.Label LblBorderTracing;
		private System.Windows.Forms.Label LblBorderTracingmm;
		private LaserGRBL.UserControls.IntegerInput.IntegerInputRanged IIBorderTracing;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.Label label19;
		private LaserGRBL.UserControls.IntegerInput.IntegerInputRanged IILinearFilling;
		private LaserGRBL.UserControls.IntegerInput.IntegerInputRanged IITravelSpeed;
		private System.Windows.Forms.Label LblLinearFillingmm;
		private System.Windows.Forms.Label LblLinearFilling;
		private System.Windows.Forms.GroupBox groupBox5;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
		private System.Windows.Forms.TextBox TxtLaserOff;
		private System.Windows.Forms.Label label26;
		private System.Windows.Forms.Label label21;
		private LaserGRBL.UserControls.IntegerInput.IntegerInputRanged IIMinPower;
		private System.Windows.Forms.Label label25;
		private LaserGRBL.UserControls.IntegerInput.IntegerInputRanged IIMaxPower;
		private System.Windows.Forms.Label label18;
		private System.Windows.Forms.TextBox TxtLaserOn;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Button BtnCreate;
		private System.Windows.Forms.Button BtnCancel;
		
		/// <summary>
		/// Disposes resources used by the form.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if (components != null) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}
		
		/// <summary>
		/// This method is required for Windows Forms designer support.
		/// Do not change the method contents inside the source code editor. The Forms designer might
		/// not be able to load this method if it was changed manually.
		/// </summary>
		private void InitializeComponent()
		{
			this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.label9 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.IIOffsetX = new LaserGRBL.UserControls.IntegerInput.IntegerInputRanged();
			this.IIOffsetY = new LaserGRBL.UserControls.IntegerInput.IntegerInputRanged();
			this.IISizeH = new LaserGRBL.UserControls.IntegerInput.IntegerInputRanged();
			this.IISizeW = new LaserGRBL.UserControls.IntegerInput.IntegerInputRanged();
			this.label6 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label11 = new System.Windows.Forms.Label();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
			this.LblBorderTracing = new System.Windows.Forms.Label();
			this.LblBorderTracingmm = new System.Windows.Forms.Label();
			this.IIBorderTracing = new LaserGRBL.UserControls.IntegerInput.IntegerInputRanged();
			this.label20 = new System.Windows.Forms.Label();
			this.label19 = new System.Windows.Forms.Label();
			this.IILinearFilling = new LaserGRBL.UserControls.IntegerInput.IntegerInputRanged();
			this.IITravelSpeed = new LaserGRBL.UserControls.IntegerInput.IntegerInputRanged();
			this.LblLinearFillingmm = new System.Windows.Forms.Label();
			this.LblLinearFilling = new System.Windows.Forms.Label();
			this.groupBox5 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
			this.TxtLaserOff = new System.Windows.Forms.TextBox();
			this.label26 = new System.Windows.Forms.Label();
			this.label21 = new System.Windows.Forms.Label();
			this.IIMinPower = new LaserGRBL.UserControls.IntegerInput.IntegerInputRanged();
			this.label25 = new System.Windows.Forms.Label();
			this.IIMaxPower = new LaserGRBL.UserControls.IntegerInput.IntegerInputRanged();
			this.label18 = new System.Windows.Forms.Label();
			this.TxtLaserOn = new System.Windows.Forms.TextBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.BtnCreate = new System.Windows.Forms.Button();
			this.BtnCancel = new System.Windows.Forms.Button();
			this.tableLayoutPanel9.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.tableLayoutPanel6.SuspendLayout();
			this.groupBox5.SuspendLayout();
			this.tableLayoutPanel7.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel9
			// 
			this.tableLayoutPanel9.AutoSize = true;
			this.tableLayoutPanel9.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel9.ColumnCount = 1;
			this.tableLayoutPanel9.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel9.Controls.Add(this.groupBox2, 0, 2);
			this.tableLayoutPanel9.Controls.Add(this.groupBox3, 0, 0);
			this.tableLayoutPanel9.Controls.Add(this.groupBox5, 0, 1);
			this.tableLayoutPanel9.Controls.Add(this.tableLayoutPanel1, 0, 3);
			this.tableLayoutPanel9.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel9.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel9.Name = "tableLayoutPanel9";
			this.tableLayoutPanel9.RowCount = 4;
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel9.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel9.Size = new System.Drawing.Size(265, 266);
			this.tableLayoutPanel9.TabIndex = 1;
			// 
			// groupBox2
			// 
			this.groupBox2.AutoSize = true;
			this.groupBox2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.groupBox2.Controls.Add(this.tableLayoutPanel3);
			this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox2.Location = new System.Drawing.Point(3, 163);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(259, 61);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Image Size and Position [mm]";
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.AutoSize = true;
			this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel3.ColumnCount = 5;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.Controls.Add(this.label9, 0, 1);
			this.tableLayoutPanel3.Controls.Add(this.label4, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.IIOffsetX, 2, 1);
			this.tableLayoutPanel3.Controls.Add(this.IIOffsetY, 4, 1);
			this.tableLayoutPanel3.Controls.Add(this.IISizeH, 4, 0);
			this.tableLayoutPanel3.Controls.Add(this.IISizeW, 2, 0);
			this.tableLayoutPanel3.Controls.Add(this.label6, 1, 0);
			this.tableLayoutPanel3.Controls.Add(this.label10, 1, 1);
			this.tableLayoutPanel3.Controls.Add(this.label7, 3, 0);
			this.tableLayoutPanel3.Controls.Add(this.label11, 3, 1);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 2;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(253, 42);
			this.tableLayoutPanel3.TabIndex = 0;
			// 
			// label9
			// 
			this.label9.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(3, 25);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(35, 13);
			this.label9.TabIndex = 9;
			this.label9.Text = "Offset";
			// 
			// label4
			// 
			this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(3, 4);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(27, 13);
			this.label4.TabIndex = 1;
			this.label4.Text = "Size";
			// 
			// IIOffsetX
			// 
			this.IIOffsetX.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.IIOffsetX.ForcedText = null;
			this.IIOffsetX.ForceMinMax = false;
			this.IIOffsetX.Location = new System.Drawing.Point(61, 24);
			this.IIOffsetX.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this.IIOffsetX.MaxValue = 1000;
			this.IIOffsetX.MinValue = 0;
			this.IIOffsetX.Name = "IIOffsetX";
			this.IIOffsetX.NormalBorderColor = System.Drawing.SystemColors.ActiveBorder;
			this.IIOffsetX.Size = new System.Drawing.Size(55, 15);
			this.IIOffsetX.TabIndex = 17;
			this.IIOffsetX.CurrentValueChanged += new LaserGRBL.UserControls.IntegerInput.IntegerInputBase.CurrentValueChangedEventHandler(this.IIOffsetXYCurrentValueChanged);
			// 
			// IIOffsetY
			// 
			this.IIOffsetY.ForcedText = null;
			this.IIOffsetY.ForceMinMax = false;
			this.IIOffsetY.Location = new System.Drawing.Point(139, 24);
			this.IIOffsetY.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this.IIOffsetY.MaxValue = 1000;
			this.IIOffsetY.MinValue = 0;
			this.IIOffsetY.Name = "IIOffsetY";
			this.IIOffsetY.NormalBorderColor = System.Drawing.SystemColors.ActiveBorder;
			this.IIOffsetY.Size = new System.Drawing.Size(55, 15);
			this.IIOffsetY.TabIndex = 18;
			this.IIOffsetY.CurrentValueChanged += new LaserGRBL.UserControls.IntegerInput.IntegerInputBase.CurrentValueChangedEventHandler(this.IIOffsetXYCurrentValueChanged);
			// 
			// IISizeH
			// 
			this.IISizeH.CurrentValue = 100;
			this.IISizeH.ForcedText = null;
			this.IISizeH.ForceMinMax = false;
			this.IISizeH.Location = new System.Drawing.Point(139, 3);
			this.IISizeH.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this.IISizeH.MaxValue = 1000;
			this.IISizeH.MinValue = 10;
			this.IISizeH.Name = "IISizeH";
			this.IISizeH.NormalBorderColor = System.Drawing.SystemColors.ActiveBorder;
			this.IISizeH.Size = new System.Drawing.Size(55, 15);
			this.IISizeH.TabIndex = 20;
			this.IISizeH.CurrentValueChanged += new LaserGRBL.UserControls.IntegerInput.IntegerInputBase.CurrentValueChangedEventHandler(this.IISizeH_CurrentValueChanged);
			// 
			// IISizeW
			// 
			this.IISizeW.CurrentValue = 100;
			this.IISizeW.ForcedText = null;
			this.IISizeW.ForceMinMax = false;
			this.IISizeW.Location = new System.Drawing.Point(61, 3);
			this.IISizeW.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this.IISizeW.MaxValue = 1000;
			this.IISizeW.MinValue = 10;
			this.IISizeW.Name = "IISizeW";
			this.IISizeW.NormalBorderColor = System.Drawing.SystemColors.ActiveBorder;
			this.IISizeW.Size = new System.Drawing.Size(55, 15);
			this.IISizeW.TabIndex = 19;
			this.IISizeW.CurrentValueChanged += new LaserGRBL.UserControls.IntegerInput.IntegerInputBase.CurrentValueChangedEventHandler(this.IISizeW_CurrentValueChanged);
			// 
			// label6
			// 
			this.label6.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(44, 4);
			this.label6.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(17, 13);
			this.label6.TabIndex = 5;
			this.label6.Text = "W";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label10
			// 
			this.label10.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(44, 25);
			this.label10.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(14, 13);
			this.label10.TabIndex = 11;
			this.label10.Text = "X";
			this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label7
			// 
			this.label7.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(122, 4);
			this.label7.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(15, 13);
			this.label7.TabIndex = 6;
			this.label7.Text = "H";
			this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label11
			// 
			this.label11.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(122, 25);
			this.label11.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(14, 13);
			this.label11.TabIndex = 12;
			this.label11.Text = "Y";
			this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// groupBox3
			// 
			this.groupBox3.AutoSize = true;
			this.groupBox3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.groupBox3.Controls.Add(this.tableLayoutPanel6);
			this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox3.Location = new System.Drawing.Point(3, 3);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(259, 82);
			this.groupBox3.TabIndex = 5;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Speed";
			// 
			// tableLayoutPanel6
			// 
			this.tableLayoutPanel6.AutoSize = true;
			this.tableLayoutPanel6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel6.ColumnCount = 3;
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel6.Controls.Add(this.LblBorderTracing, 0, 0);
			this.tableLayoutPanel6.Controls.Add(this.LblBorderTracingmm, 2, 0);
			this.tableLayoutPanel6.Controls.Add(this.IIBorderTracing, 1, 0);
			this.tableLayoutPanel6.Controls.Add(this.label20, 2, 2);
			this.tableLayoutPanel6.Controls.Add(this.label19, 0, 2);
			this.tableLayoutPanel6.Controls.Add(this.IILinearFilling, 1, 1);
			this.tableLayoutPanel6.Controls.Add(this.IITravelSpeed, 1, 2);
			this.tableLayoutPanel6.Controls.Add(this.LblLinearFillingmm, 2, 1);
			this.tableLayoutPanel6.Controls.Add(this.LblLinearFilling, 0, 1);
			this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel6.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel6.Name = "tableLayoutPanel6";
			this.tableLayoutPanel6.RowCount = 3;
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel6.Size = new System.Drawing.Size(253, 63);
			this.tableLayoutPanel6.TabIndex = 0;
			// 
			// LblBorderTracing
			// 
			this.LblBorderTracing.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblBorderTracing.AutoSize = true;
			this.LblBorderTracing.Location = new System.Drawing.Point(3, 4);
			this.LblBorderTracing.Name = "LblBorderTracing";
			this.LblBorderTracing.Size = new System.Drawing.Size(72, 13);
			this.LblBorderTracing.TabIndex = 23;
			this.LblBorderTracing.Text = "Border Speed";
			// 
			// LblBorderTracingmm
			// 
			this.LblBorderTracingmm.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblBorderTracingmm.AutoSize = true;
			this.LblBorderTracingmm.Location = new System.Drawing.Point(145, 4);
			this.LblBorderTracingmm.Name = "LblBorderTracingmm";
			this.LblBorderTracingmm.Size = new System.Drawing.Size(44, 13);
			this.LblBorderTracingmm.TabIndex = 22;
			this.LblBorderTracingmm.Text = "mm/min";
			// 
			// IIBorderTracing
			// 
			this.IIBorderTracing.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.IIBorderTracing.CurrentValue = 1000;
			this.IIBorderTracing.ForcedText = null;
			this.IIBorderTracing.ForceMinMax = false;
			this.IIBorderTracing.Location = new System.Drawing.Point(84, 3);
			this.IIBorderTracing.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this.IIBorderTracing.MaxValue = 4000;
			this.IIBorderTracing.MinValue = 1;
			this.IIBorderTracing.Name = "IIBorderTracing";
			this.IIBorderTracing.NormalBorderColor = System.Drawing.SystemColors.ActiveBorder;
			this.IIBorderTracing.Size = new System.Drawing.Size(55, 15);
			this.IIBorderTracing.TabIndex = 21;
			this.IIBorderTracing.CurrentValueChanged += new LaserGRBL.UserControls.IntegerInput.IntegerInputBase.CurrentValueChangedEventHandler(this.IIBorderTracingCurrentValueChanged);
			// 
			// label20
			// 
			this.label20.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label20.AutoSize = true;
			this.label20.Location = new System.Drawing.Point(145, 46);
			this.label20.Name = "label20";
			this.label20.Size = new System.Drawing.Size(44, 13);
			this.label20.TabIndex = 19;
			this.label20.Text = "mm/min";
			// 
			// label19
			// 
			this.label19.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label19.AutoSize = true;
			this.label19.Location = new System.Drawing.Point(3, 46);
			this.label19.Name = "label19";
			this.label19.Size = new System.Drawing.Size(78, 13);
			this.label19.TabIndex = 17;
			this.label19.Text = "Jogging Speed";
			// 
			// IILinearFilling
			// 
			this.IILinearFilling.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.IILinearFilling.CurrentValue = 1000;
			this.IILinearFilling.ForcedText = null;
			this.IILinearFilling.ForceMinMax = false;
			this.IILinearFilling.Location = new System.Drawing.Point(84, 24);
			this.IILinearFilling.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this.IILinearFilling.MaxValue = 4000;
			this.IILinearFilling.MinValue = 1;
			this.IILinearFilling.Name = "IILinearFilling";
			this.IILinearFilling.NormalBorderColor = System.Drawing.SystemColors.ActiveBorder;
			this.IILinearFilling.Size = new System.Drawing.Size(55, 15);
			this.IILinearFilling.TabIndex = 16;
			this.IILinearFilling.CurrentValueChanged += new LaserGRBL.UserControls.IntegerInput.IntegerInputBase.CurrentValueChangedEventHandler(this.IIMarkSpeedCurrentValueChanged);
			// 
			// IITravelSpeed
			// 
			this.IITravelSpeed.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.IITravelSpeed.CurrentValue = 4000;
			this.IITravelSpeed.ForcedText = null;
			this.IITravelSpeed.ForceMinMax = false;
			this.IITravelSpeed.Location = new System.Drawing.Point(84, 45);
			this.IITravelSpeed.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
			this.IITravelSpeed.MaxValue = 4000;
			this.IITravelSpeed.MinValue = 1;
			this.IITravelSpeed.Name = "IITravelSpeed";
			this.IITravelSpeed.NormalBorderColor = System.Drawing.SystemColors.ActiveBorder;
			this.IITravelSpeed.Size = new System.Drawing.Size(55, 15);
			this.IITravelSpeed.TabIndex = 18;
			this.IITravelSpeed.CurrentValueChanged += new LaserGRBL.UserControls.IntegerInput.IntegerInputBase.CurrentValueChangedEventHandler(this.IITravelSpeedCurrentValueChanged);
			// 
			// LblLinearFillingmm
			// 
			this.LblLinearFillingmm.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblLinearFillingmm.AutoSize = true;
			this.LblLinearFillingmm.Location = new System.Drawing.Point(145, 25);
			this.LblLinearFillingmm.Name = "LblLinearFillingmm";
			this.LblLinearFillingmm.Size = new System.Drawing.Size(44, 13);
			this.LblLinearFillingmm.TabIndex = 15;
			this.LblLinearFillingmm.Text = "mm/min";
			// 
			// LblLinearFilling
			// 
			this.LblLinearFilling.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblLinearFilling.AutoSize = true;
			this.LblLinearFilling.Location = new System.Drawing.Point(3, 25);
			this.LblLinearFilling.Name = "LblLinearFilling";
			this.LblLinearFilling.Size = new System.Drawing.Size(67, 13);
			this.LblLinearFilling.TabIndex = 13;
			this.LblLinearFilling.Text = "Filling Speed";
			// 
			// groupBox5
			// 
			this.groupBox5.AutoSize = true;
			this.groupBox5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.groupBox5.Controls.Add(this.tableLayoutPanel7);
			this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox5.Location = new System.Drawing.Point(3, 91);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.Size = new System.Drawing.Size(259, 66);
			this.groupBox5.TabIndex = 6;
			this.groupBox5.TabStop = false;
			this.groupBox5.Text = "Laser Options";
			// 
			// tableLayoutPanel7
			// 
			this.tableLayoutPanel7.AutoSize = true;
			this.tableLayoutPanel7.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel7.ColumnCount = 4;
			this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel7.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel7.Controls.Add(this.TxtLaserOff, 3, 0);
			this.tableLayoutPanel7.Controls.Add(this.label26, 2, 0);
			this.tableLayoutPanel7.Controls.Add(this.label21, 0, 1);
			this.tableLayoutPanel7.Controls.Add(this.IIMinPower, 1, 1);
			this.tableLayoutPanel7.Controls.Add(this.label25, 2, 1);
			this.tableLayoutPanel7.Controls.Add(this.IIMaxPower, 3, 1);
			this.tableLayoutPanel7.Controls.Add(this.label18, 0, 0);
			this.tableLayoutPanel7.Controls.Add(this.TxtLaserOn, 1, 0);
			this.tableLayoutPanel7.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel7.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel7.Name = "tableLayoutPanel7";
			this.tableLayoutPanel7.RowCount = 2;
			this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel7.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel7.Size = new System.Drawing.Size(253, 47);
			this.tableLayoutPanel7.TabIndex = 0;
			// 
			// TxtLaserOff
			// 
			this.TxtLaserOff.Location = new System.Drawing.Point(166, 3);
			this.TxtLaserOff.MaxLength = 3;
			this.TxtLaserOff.Name = "TxtLaserOff";
			this.TxtLaserOff.Size = new System.Drawing.Size(35, 20);
			this.TxtLaserOff.TabIndex = 22;
			this.TxtLaserOff.Text = "M5";
			this.TxtLaserOff.TextChanged += new System.EventHandler(this.TxtLaserOffTextChanged);
			// 
			// label26
			// 
			this.label26.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label26.AutoSize = true;
			this.label26.Location = new System.Drawing.Point(104, 6);
			this.label26.Name = "label26";
			this.label26.Size = new System.Drawing.Size(56, 13);
			this.label26.TabIndex = 21;
			this.label26.Text = "Laser OFF";
			// 
			// label21
			// 
			this.label21.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label21.AutoSize = true;
			this.label21.Location = new System.Drawing.Point(3, 30);
			this.label21.Name = "label21";
			this.label21.Size = new System.Drawing.Size(27, 13);
			this.label21.TabIndex = 13;
			this.label21.Text = "MIN";
			// 
			// IIMinPower
			// 
			this.IIMinPower.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.IIMinPower.ForcedText = null;
			this.IIMinPower.ForceMinMax = false;
			this.IIMinPower.Location = new System.Drawing.Point(61, 29);
			this.IIMinPower.MaxValue = 1000;
			this.IIMinPower.MinValue = 0;
			this.IIMinPower.Name = "IIMinPower";
			this.IIMinPower.NormalBorderColor = System.Drawing.SystemColors.ActiveBorder;
			this.IIMinPower.Size = new System.Drawing.Size(37, 15);
			this.IIMinPower.TabIndex = 16;
			this.IIMinPower.CurrentValueChanged += new LaserGRBL.UserControls.IntegerInput.IntegerInputBase.CurrentValueChangedEventHandler(this.IIMinPowerCurrentValueChanged);
			// 
			// label25
			// 
			this.label25.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label25.AutoSize = true;
			this.label25.Location = new System.Drawing.Point(104, 30);
			this.label25.Name = "label25";
			this.label25.Size = new System.Drawing.Size(30, 13);
			this.label25.TabIndex = 17;
			this.label25.Text = "MAX";
			// 
			// IIMaxPower
			// 
			this.IIMaxPower.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.IIMaxPower.CurrentValue = 255;
			this.IIMaxPower.ForcedText = null;
			this.IIMaxPower.ForceMinMax = false;
			this.IIMaxPower.Location = new System.Drawing.Point(166, 29);
			this.IIMaxPower.MaxValue = 1000;
			this.IIMaxPower.MinValue = 1;
			this.IIMaxPower.Name = "IIMaxPower";
			this.IIMaxPower.NormalBorderColor = System.Drawing.SystemColors.ActiveBorder;
			this.IIMaxPower.Size = new System.Drawing.Size(37, 15);
			this.IIMaxPower.TabIndex = 18;
			this.IIMaxPower.CurrentValueChanged += new LaserGRBL.UserControls.IntegerInput.IntegerInputBase.CurrentValueChangedEventHandler(this.IIMaxPowerCurrentValueChanged);
			// 
			// label18
			// 
			this.label18.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.label18.AutoSize = true;
			this.label18.Location = new System.Drawing.Point(3, 6);
			this.label18.Name = "label18";
			this.label18.Size = new System.Drawing.Size(52, 13);
			this.label18.TabIndex = 19;
			this.label18.Text = "Laser ON";
			// 
			// TxtLaserOn
			// 
			this.TxtLaserOn.Location = new System.Drawing.Point(61, 3);
			this.TxtLaserOn.MaxLength = 3;
			this.TxtLaserOn.Name = "TxtLaserOn";
			this.TxtLaserOn.Size = new System.Drawing.Size(37, 20);
			this.TxtLaserOn.TabIndex = 20;
			this.TxtLaserOn.Text = "M3";
			this.TxtLaserOn.TextChanged += new System.EventHandler(this.TxtLaserOnTextChanged);
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
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 230);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 1;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(259, 33);
			this.tableLayoutPanel1.TabIndex = 7;
			// 
			// BtnCreate
			// 
			this.BtnCreate.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.BtnCreate.Location = new System.Drawing.Point(182, 3);
			this.BtnCreate.Name = "BtnCreate";
			this.BtnCreate.Size = new System.Drawing.Size(74, 27);
			this.BtnCreate.TabIndex = 5;
			this.BtnCreate.Text = "Create!";
			this.BtnCreate.UseVisualStyleBackColor = true;
			// 
			// BtnCancel
			// 
			this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.BtnCancel.Location = new System.Drawing.Point(102, 3);
			this.BtnCancel.Name = "BtnCancel";
			this.BtnCancel.Size = new System.Drawing.Size(74, 27);
			this.BtnCancel.TabIndex = 6;
			this.BtnCancel.Text = "Cancel";
			this.BtnCancel.UseVisualStyleBackColor = true;
			// 
			// ConvertSizeAndOptionForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ClientSize = new System.Drawing.Size(265, 266);
			this.Controls.Add(this.tableLayoutPanel9);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "ConvertSizeAndOptionForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Target image";
			this.tableLayoutPanel9.ResumeLayout(false);
			this.tableLayoutPanel9.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.tableLayoutPanel6.ResumeLayout(false);
			this.tableLayoutPanel6.PerformLayout();
			this.groupBox5.ResumeLayout(false);
			this.groupBox5.PerformLayout();
			this.tableLayoutPanel7.ResumeLayout(false);
			this.tableLayoutPanel7.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
	}
}
