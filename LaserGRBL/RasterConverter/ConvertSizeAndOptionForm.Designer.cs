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
		private System.Windows.Forms.Label label26;
		private System.Windows.Forms.Label LblSmin;
		private LaserGRBL.UserControls.IntegerInput.IntegerInputRanged IIMinPower;
		private System.Windows.Forms.Label LblSmax;
		private LaserGRBL.UserControls.IntegerInput.IntegerInputRanged IIMaxPower;
		private System.Windows.Forms.Label label18;
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConvertSizeAndOptionForm));
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
			this.BtnModulationInfo = new LaserGRBL.UserControls.ImageButton();
			this.label26 = new System.Windows.Forms.Label();
			this.LblSmin = new System.Windows.Forms.Label();
			this.IIMinPower = new LaserGRBL.UserControls.IntegerInput.IntegerInputRanged();
			this.LblSmax = new System.Windows.Forms.Label();
			this.IIMaxPower = new LaserGRBL.UserControls.IntegerInput.IntegerInputRanged();
			this.label18 = new System.Windows.Forms.Label();
			this.BtnOnOffInfo = new LaserGRBL.UserControls.ImageButton();
			this.CBLaserON = new System.Windows.Forms.ComboBox();
			this.CBLaserOFF = new System.Windows.Forms.ComboBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.BtnCancel = new System.Windows.Forms.Button();
			this.BtnCreate = new System.Windows.Forms.Button();
			this.TT = new System.Windows.Forms.ToolTip(this.components);
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
			resources.ApplyResources(this.tableLayoutPanel9, "tableLayoutPanel9");
			this.tableLayoutPanel9.Controls.Add(this.groupBox2, 0, 2);
			this.tableLayoutPanel9.Controls.Add(this.groupBox3, 0, 0);
			this.tableLayoutPanel9.Controls.Add(this.groupBox5, 0, 1);
			this.tableLayoutPanel9.Controls.Add(this.tableLayoutPanel1, 0, 3);
			this.tableLayoutPanel9.Name = "tableLayoutPanel9";
			this.TT.SetToolTip(this.tableLayoutPanel9, resources.GetString("tableLayoutPanel9.ToolTip"));
			// 
			// groupBox2
			// 
			resources.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.Controls.Add(this.tableLayoutPanel3);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			this.TT.SetToolTip(this.groupBox2, resources.GetString("groupBox2.ToolTip"));
			// 
			// tableLayoutPanel3
			// 
			resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
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
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.TT.SetToolTip(this.tableLayoutPanel3, resources.GetString("tableLayoutPanel3.ToolTip"));
			// 
			// label9
			// 
			resources.ApplyResources(this.label9, "label9");
			this.label9.Name = "label9";
			this.TT.SetToolTip(this.label9, resources.GetString("label9.ToolTip"));
			// 
			// label4
			// 
			resources.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			this.TT.SetToolTip(this.label4, resources.GetString("label4.ToolTip"));
			// 
			// IIOffsetX
			// 
			resources.ApplyResources(this.IIOffsetX, "IIOffsetX");
			this.IIOffsetX.ForcedText = null;
			this.IIOffsetX.ForceMinMax = false;
			this.IIOffsetX.MaxValue = 1000;
			this.IIOffsetX.MinValue = 0;
			this.IIOffsetX.Name = "IIOffsetX";
			this.IIOffsetX.NormalBorderColor = System.Drawing.SystemColors.ActiveBorder;
			this.TT.SetToolTip(this.IIOffsetX, resources.GetString("IIOffsetX.ToolTip"));
			this.IIOffsetX.CurrentValueChanged += new LaserGRBL.UserControls.IntegerInput.IntegerInputBase.CurrentValueChangedEventHandler(this.IIOffsetXYCurrentValueChanged);
			// 
			// IIOffsetY
			// 
			resources.ApplyResources(this.IIOffsetY, "IIOffsetY");
			this.IIOffsetY.ForcedText = null;
			this.IIOffsetY.ForceMinMax = false;
			this.IIOffsetY.MaxValue = 1000;
			this.IIOffsetY.MinValue = 0;
			this.IIOffsetY.Name = "IIOffsetY";
			this.IIOffsetY.NormalBorderColor = System.Drawing.SystemColors.ActiveBorder;
			this.TT.SetToolTip(this.IIOffsetY, resources.GetString("IIOffsetY.ToolTip"));
			this.IIOffsetY.CurrentValueChanged += new LaserGRBL.UserControls.IntegerInput.IntegerInputBase.CurrentValueChangedEventHandler(this.IIOffsetXYCurrentValueChanged);
			// 
			// IISizeH
			// 
			resources.ApplyResources(this.IISizeH, "IISizeH");
			this.IISizeH.ForcedText = null;
			this.IISizeH.ForceMinMax = false;
			this.IISizeH.MaxValue = 1000;
			this.IISizeH.MinValue = 10;
			this.IISizeH.Name = "IISizeH";
			this.IISizeH.NormalBorderColor = System.Drawing.SystemColors.ActiveBorder;
			this.TT.SetToolTip(this.IISizeH, resources.GetString("IISizeH.ToolTip"));
			this.IISizeH.CurrentValueChanged += new LaserGRBL.UserControls.IntegerInput.IntegerInputBase.CurrentValueChangedEventHandler(this.IISizeH_CurrentValueChanged);
			// 
			// IISizeW
			// 
			resources.ApplyResources(this.IISizeW, "IISizeW");
			this.IISizeW.ForcedText = null;
			this.IISizeW.ForceMinMax = false;
			this.IISizeW.MaxValue = 1000;
			this.IISizeW.MinValue = 10;
			this.IISizeW.Name = "IISizeW";
			this.IISizeW.NormalBorderColor = System.Drawing.SystemColors.ActiveBorder;
			this.TT.SetToolTip(this.IISizeW, resources.GetString("IISizeW.ToolTip"));
			this.IISizeW.CurrentValueChanged += new LaserGRBL.UserControls.IntegerInput.IntegerInputBase.CurrentValueChangedEventHandler(this.IISizeW_CurrentValueChanged);
			// 
			// label6
			// 
			resources.ApplyResources(this.label6, "label6");
			this.label6.Name = "label6";
			this.TT.SetToolTip(this.label6, resources.GetString("label6.ToolTip"));
			// 
			// label10
			// 
			resources.ApplyResources(this.label10, "label10");
			this.label10.Name = "label10";
			this.TT.SetToolTip(this.label10, resources.GetString("label10.ToolTip"));
			// 
			// label7
			// 
			resources.ApplyResources(this.label7, "label7");
			this.label7.Name = "label7";
			this.TT.SetToolTip(this.label7, resources.GetString("label7.ToolTip"));
			// 
			// label11
			// 
			resources.ApplyResources(this.label11, "label11");
			this.label11.Name = "label11";
			this.TT.SetToolTip(this.label11, resources.GetString("label11.ToolTip"));
			// 
			// groupBox3
			// 
			resources.ApplyResources(this.groupBox3, "groupBox3");
			this.groupBox3.Controls.Add(this.tableLayoutPanel6);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.TabStop = false;
			this.TT.SetToolTip(this.groupBox3, resources.GetString("groupBox3.ToolTip"));
			// 
			// tableLayoutPanel6
			// 
			resources.ApplyResources(this.tableLayoutPanel6, "tableLayoutPanel6");
			this.tableLayoutPanel6.Controls.Add(this.LblBorderTracing, 0, 0);
			this.tableLayoutPanel6.Controls.Add(this.LblBorderTracingmm, 2, 0);
			this.tableLayoutPanel6.Controls.Add(this.IIBorderTracing, 1, 0);
			this.tableLayoutPanel6.Controls.Add(this.label20, 2, 2);
			this.tableLayoutPanel6.Controls.Add(this.label19, 0, 2);
			this.tableLayoutPanel6.Controls.Add(this.IILinearFilling, 1, 1);
			this.tableLayoutPanel6.Controls.Add(this.IITravelSpeed, 1, 2);
			this.tableLayoutPanel6.Controls.Add(this.LblLinearFillingmm, 2, 1);
			this.tableLayoutPanel6.Controls.Add(this.LblLinearFilling, 0, 1);
			this.tableLayoutPanel6.Name = "tableLayoutPanel6";
			this.TT.SetToolTip(this.tableLayoutPanel6, resources.GetString("tableLayoutPanel6.ToolTip"));
			// 
			// LblBorderTracing
			// 
			resources.ApplyResources(this.LblBorderTracing, "LblBorderTracing");
			this.LblBorderTracing.Name = "LblBorderTracing";
			this.TT.SetToolTip(this.LblBorderTracing, resources.GetString("LblBorderTracing.ToolTip"));
			// 
			// LblBorderTracingmm
			// 
			resources.ApplyResources(this.LblBorderTracingmm, "LblBorderTracingmm");
			this.LblBorderTracingmm.Name = "LblBorderTracingmm";
			this.TT.SetToolTip(this.LblBorderTracingmm, resources.GetString("LblBorderTracingmm.ToolTip"));
			// 
			// IIBorderTracing
			// 
			resources.ApplyResources(this.IIBorderTracing, "IIBorderTracing");
			this.IIBorderTracing.CurrentValue = 1000;
			this.IIBorderTracing.ForcedText = null;
			this.IIBorderTracing.ForceMinMax = false;
			this.IIBorderTracing.MaxValue = 4000;
			this.IIBorderTracing.MinValue = 1;
			this.IIBorderTracing.Name = "IIBorderTracing";
			this.IIBorderTracing.NormalBorderColor = System.Drawing.SystemColors.ActiveBorder;
			this.TT.SetToolTip(this.IIBorderTracing, resources.GetString("IIBorderTracing.ToolTip"));
			this.IIBorderTracing.CurrentValueChanged += new LaserGRBL.UserControls.IntegerInput.IntegerInputBase.CurrentValueChangedEventHandler(this.IIBorderTracingCurrentValueChanged);
			// 
			// label20
			// 
			resources.ApplyResources(this.label20, "label20");
			this.label20.Name = "label20";
			this.TT.SetToolTip(this.label20, resources.GetString("label20.ToolTip"));
			// 
			// label19
			// 
			resources.ApplyResources(this.label19, "label19");
			this.label19.Name = "label19";
			this.TT.SetToolTip(this.label19, resources.GetString("label19.ToolTip"));
			// 
			// IILinearFilling
			// 
			resources.ApplyResources(this.IILinearFilling, "IILinearFilling");
			this.IILinearFilling.CurrentValue = 1000;
			this.IILinearFilling.ForcedText = null;
			this.IILinearFilling.ForceMinMax = false;
			this.IILinearFilling.MaxValue = 4000;
			this.IILinearFilling.MinValue = 1;
			this.IILinearFilling.Name = "IILinearFilling";
			this.IILinearFilling.NormalBorderColor = System.Drawing.SystemColors.ActiveBorder;
			this.TT.SetToolTip(this.IILinearFilling, resources.GetString("IILinearFilling.ToolTip"));
			this.IILinearFilling.CurrentValueChanged += new LaserGRBL.UserControls.IntegerInput.IntegerInputBase.CurrentValueChangedEventHandler(this.IIMarkSpeedCurrentValueChanged);
			// 
			// IITravelSpeed
			// 
			resources.ApplyResources(this.IITravelSpeed, "IITravelSpeed");
			this.IITravelSpeed.CurrentValue = 4000;
			this.IITravelSpeed.ForcedText = null;
			this.IITravelSpeed.ForceMinMax = false;
			this.IITravelSpeed.MaxValue = 4000;
			this.IITravelSpeed.MinValue = 1;
			this.IITravelSpeed.Name = "IITravelSpeed";
			this.IITravelSpeed.NormalBorderColor = System.Drawing.SystemColors.ActiveBorder;
			this.TT.SetToolTip(this.IITravelSpeed, resources.GetString("IITravelSpeed.ToolTip"));
			this.IITravelSpeed.CurrentValueChanged += new LaserGRBL.UserControls.IntegerInput.IntegerInputBase.CurrentValueChangedEventHandler(this.IITravelSpeedCurrentValueChanged);
			// 
			// LblLinearFillingmm
			// 
			resources.ApplyResources(this.LblLinearFillingmm, "LblLinearFillingmm");
			this.LblLinearFillingmm.Name = "LblLinearFillingmm";
			this.TT.SetToolTip(this.LblLinearFillingmm, resources.GetString("LblLinearFillingmm.ToolTip"));
			// 
			// LblLinearFilling
			// 
			resources.ApplyResources(this.LblLinearFilling, "LblLinearFilling");
			this.LblLinearFilling.Name = "LblLinearFilling";
			this.TT.SetToolTip(this.LblLinearFilling, resources.GetString("LblLinearFilling.ToolTip"));
			// 
			// groupBox5
			// 
			resources.ApplyResources(this.groupBox5, "groupBox5");
			this.groupBox5.Controls.Add(this.tableLayoutPanel7);
			this.groupBox5.Name = "groupBox5";
			this.groupBox5.TabStop = false;
			this.TT.SetToolTip(this.groupBox5, resources.GetString("groupBox5.ToolTip"));
			// 
			// tableLayoutPanel7
			// 
			resources.ApplyResources(this.tableLayoutPanel7, "tableLayoutPanel7");
			this.tableLayoutPanel7.Controls.Add(this.BtnModulationInfo, 4, 1);
			this.tableLayoutPanel7.Controls.Add(this.label26, 2, 0);
			this.tableLayoutPanel7.Controls.Add(this.LblSmin, 0, 1);
			this.tableLayoutPanel7.Controls.Add(this.IIMinPower, 1, 1);
			this.tableLayoutPanel7.Controls.Add(this.LblSmax, 2, 1);
			this.tableLayoutPanel7.Controls.Add(this.IIMaxPower, 3, 1);
			this.tableLayoutPanel7.Controls.Add(this.label18, 0, 0);
			this.tableLayoutPanel7.Controls.Add(this.BtnOnOffInfo, 4, 0);
			this.tableLayoutPanel7.Controls.Add(this.CBLaserON, 1, 0);
			this.tableLayoutPanel7.Controls.Add(this.CBLaserOFF, 3, 0);
			this.tableLayoutPanel7.Name = "tableLayoutPanel7";
			this.TT.SetToolTip(this.tableLayoutPanel7, resources.GetString("tableLayoutPanel7.ToolTip"));
			// 
			// BtnModulationInfo
			// 
			resources.ApplyResources(this.BtnModulationInfo, "BtnModulationInfo");
			this.BtnModulationInfo.AltImage = null;
			this.BtnModulationInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtnModulationInfo.Coloration = System.Drawing.Color.Empty;
			this.BtnModulationInfo.Image = ((System.Drawing.Image)(resources.GetObject("BtnModulationInfo.Image")));
			this.BtnModulationInfo.Name = "BtnModulationInfo";
			this.BtnModulationInfo.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.TT.SetToolTip(this.BtnModulationInfo, resources.GetString("BtnModulationInfo.ToolTip"));
			this.BtnModulationInfo.UseAltImage = false;
			this.BtnModulationInfo.Click += new System.EventHandler(this.BtnModulationInfo_Click);
			// 
			// label26
			// 
			resources.ApplyResources(this.label26, "label26");
			this.label26.Name = "label26";
			this.TT.SetToolTip(this.label26, resources.GetString("label26.ToolTip"));
			// 
			// LblSmin
			// 
			resources.ApplyResources(this.LblSmin, "LblSmin");
			this.LblSmin.Name = "LblSmin";
			this.TT.SetToolTip(this.LblSmin, resources.GetString("LblSmin.ToolTip"));
			// 
			// IIMinPower
			// 
			resources.ApplyResources(this.IIMinPower, "IIMinPower");
			this.IIMinPower.ForcedText = null;
			this.IIMinPower.ForceMinMax = false;
			this.IIMinPower.MaxValue = 1000;
			this.IIMinPower.MinValue = 0;
			this.IIMinPower.Name = "IIMinPower";
			this.IIMinPower.NormalBorderColor = System.Drawing.SystemColors.ActiveBorder;
			this.TT.SetToolTip(this.IIMinPower, resources.GetString("IIMinPower.ToolTip"));
			this.IIMinPower.CurrentValueChanged += new LaserGRBL.UserControls.IntegerInput.IntegerInputBase.CurrentValueChangedEventHandler(this.IIMinPowerCurrentValueChanged);
			// 
			// LblSmax
			// 
			resources.ApplyResources(this.LblSmax, "LblSmax");
			this.LblSmax.Name = "LblSmax";
			this.TT.SetToolTip(this.LblSmax, resources.GetString("LblSmax.ToolTip"));
			// 
			// IIMaxPower
			// 
			resources.ApplyResources(this.IIMaxPower, "IIMaxPower");
			this.IIMaxPower.CurrentValue = 255;
			this.IIMaxPower.ForcedText = null;
			this.IIMaxPower.ForceMinMax = false;
			this.IIMaxPower.MaxValue = 1000;
			this.IIMaxPower.MinValue = 1;
			this.IIMaxPower.Name = "IIMaxPower";
			this.IIMaxPower.NormalBorderColor = System.Drawing.SystemColors.ActiveBorder;
			this.TT.SetToolTip(this.IIMaxPower, resources.GetString("IIMaxPower.ToolTip"));
			this.IIMaxPower.CurrentValueChanged += new LaserGRBL.UserControls.IntegerInput.IntegerInputBase.CurrentValueChangedEventHandler(this.IIMaxPowerCurrentValueChanged);
			// 
			// label18
			// 
			resources.ApplyResources(this.label18, "label18");
			this.label18.Name = "label18";
			this.TT.SetToolTip(this.label18, resources.GetString("label18.ToolTip"));
			// 
			// BtnOnOffInfo
			// 
			resources.ApplyResources(this.BtnOnOffInfo, "BtnOnOffInfo");
			this.BtnOnOffInfo.AltImage = null;
			this.BtnOnOffInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtnOnOffInfo.Coloration = System.Drawing.Color.Empty;
			this.BtnOnOffInfo.Image = ((System.Drawing.Image)(resources.GetObject("BtnOnOffInfo.Image")));
			this.BtnOnOffInfo.Name = "BtnOnOffInfo";
			this.BtnOnOffInfo.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.TT.SetToolTip(this.BtnOnOffInfo, resources.GetString("BtnOnOffInfo.ToolTip"));
			this.BtnOnOffInfo.UseAltImage = false;
			this.BtnOnOffInfo.Click += new System.EventHandler(this.BtnOnOffInfo_Click);
			// 
			// CBLaserON
			// 
			resources.ApplyResources(this.CBLaserON, "CBLaserON");
			this.CBLaserON.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CBLaserON.FormattingEnabled = true;
			this.CBLaserON.Items.AddRange(new object[] {
            resources.GetString("CBLaserON.Items"),
            resources.GetString("CBLaserON.Items1")});
			this.CBLaserON.Name = "CBLaserON";
			this.TT.SetToolTip(this.CBLaserON, resources.GetString("CBLaserON.ToolTip"));
			this.CBLaserON.SelectedIndexChanged += new System.EventHandler(this.CBLaserON_SelectedIndexChanged);
			// 
			// CBLaserOFF
			// 
			resources.ApplyResources(this.CBLaserOFF, "CBLaserOFF");
			this.CBLaserOFF.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CBLaserOFF.FormattingEnabled = true;
			this.CBLaserOFF.Items.AddRange(new object[] {
            resources.GetString("CBLaserOFF.Items")});
			this.CBLaserOFF.Name = "CBLaserOFF";
			this.TT.SetToolTip(this.CBLaserOFF, resources.GetString("CBLaserOFF.ToolTip"));
			this.CBLaserOFF.SelectedIndexChanged += new System.EventHandler(this.CBLaserOFF_SelectedIndexChanged);
			// 
			// tableLayoutPanel1
			// 
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.BtnCancel, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.BtnCreate, 2, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.TT.SetToolTip(this.tableLayoutPanel1, resources.GetString("tableLayoutPanel1.ToolTip"));
			// 
			// BtnCancel
			// 
			resources.ApplyResources(this.BtnCancel, "BtnCancel");
			this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.BtnCancel.Name = "BtnCancel";
			this.TT.SetToolTip(this.BtnCancel, resources.GetString("BtnCancel.ToolTip"));
			this.BtnCancel.UseVisualStyleBackColor = true;
			// 
			// BtnCreate
			// 
			resources.ApplyResources(this.BtnCreate, "BtnCreate");
			this.BtnCreate.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.BtnCreate.Name = "BtnCreate";
			this.TT.SetToolTip(this.BtnCreate, resources.GetString("BtnCreate.ToolTip"));
			this.BtnCreate.UseVisualStyleBackColor = true;
			// 
			// TT
			// 
			this.TT.AutoPopDelay = 10000;
			this.TT.InitialDelay = 500;
			this.TT.ReshowDelay = 100;
			// 
			// ConvertSizeAndOptionForm
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel9);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "ConvertSizeAndOptionForm";
			this.TT.SetToolTip(this, resources.GetString("$this.ToolTip"));
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

		private UserControls.ImageButton BtnModulationInfo;
		private UserControls.ImageButton BtnOnOffInfo;
		private System.Windows.Forms.ComboBox CBLaserON;
		private System.Windows.Forms.ComboBox CBLaserOFF;
		private System.Windows.Forms.ToolTip TT;
	}
}
