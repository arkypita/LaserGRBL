/*
 * Created by SharpDevelop.
 * User: Diego
 * Date: 15/01/2017
 * Time: 12:12
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace LaserGRBL.SvgConverter
{
	partial class SvgToGCodeForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
		private System.Windows.Forms.GroupBox GbSpeed;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
		private System.Windows.Forms.Label LblBorderTracing;
		private LaserGRBL.UserControls.NumericInput.IntegerInputRanged IIBorderTracing;
		private System.Windows.Forms.GroupBox GbLaser;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel7;
		private System.Windows.Forms.Label LblSmin;
		private LaserGRBL.UserControls.NumericInput.IntegerInputRanged IIMinPower;
		private System.Windows.Forms.Label LblSmax;
		private LaserGRBL.UserControls.NumericInput.IntegerInputRanged IIMaxPower;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SvgToGCodeForm));
			this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
			this.GbSpeed = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
			this.LblBorderTracing = new System.Windows.Forms.Label();
			this.LblBorderTracingmm = new System.Windows.Forms.Label();
			this.IIBorderTracing = new LaserGRBL.UserControls.NumericInput.IntegerInputRanged();
			this.BtnPSHelper = new LaserGRBL.UserControls.ImageButton();
			this.GbLaser = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel7 = new System.Windows.Forms.TableLayoutPanel();
			this.BtnModulationInfo = new LaserGRBL.UserControls.ImageButton();
			this.LblSmin = new System.Windows.Forms.Label();
			this.IIMinPower = new LaserGRBL.UserControls.NumericInput.IntegerInputRanged();
			this.label18 = new System.Windows.Forms.Label();
			this.BtnOnOffInfo = new LaserGRBL.UserControls.ImageButton();
			this.CBLaserON = new System.Windows.Forms.ComboBox();
			this.LblSmax = new System.Windows.Forms.Label();
			this.IIMaxPower = new LaserGRBL.UserControls.NumericInput.IntegerInputRanged();
			this.LblMinPerc = new System.Windows.Forms.Label();
			this.LblMaxPerc = new System.Windows.Forms.Label();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.BtnCancel = new System.Windows.Forms.Button();
			this.BtnCreate = new System.Windows.Forms.Button();
			this.TT = new System.Windows.Forms.ToolTip(this.components);
			this.tableLayoutPanel9.SuspendLayout();
			this.GbSpeed.SuspendLayout();
			this.tableLayoutPanel6.SuspendLayout();
			this.GbLaser.SuspendLayout();
			this.tableLayoutPanel7.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel9
			// 
			resources.ApplyResources(this.tableLayoutPanel9, "tableLayoutPanel9");
			this.tableLayoutPanel9.Controls.Add(this.GbSpeed, 0, 0);
			this.tableLayoutPanel9.Controls.Add(this.GbLaser, 0, 1);
			this.tableLayoutPanel9.Controls.Add(this.tableLayoutPanel1, 0, 3);
			this.tableLayoutPanel9.Name = "tableLayoutPanel9";
			// 
			// GbSpeed
			// 
			resources.ApplyResources(this.GbSpeed, "GbSpeed");
			this.GbSpeed.Controls.Add(this.tableLayoutPanel6);
			this.GbSpeed.Name = "GbSpeed";
			this.GbSpeed.TabStop = false;
			// 
			// tableLayoutPanel6
			// 
			resources.ApplyResources(this.tableLayoutPanel6, "tableLayoutPanel6");
			this.tableLayoutPanel6.Controls.Add(this.LblBorderTracing, 0, 0);
			this.tableLayoutPanel6.Controls.Add(this.LblBorderTracingmm, 2, 0);
			this.tableLayoutPanel6.Controls.Add(this.IIBorderTracing, 1, 0);
			this.tableLayoutPanel6.Controls.Add(this.BtnPSHelper, 3, 0);
			this.tableLayoutPanel6.Name = "tableLayoutPanel6";
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
			this.IIBorderTracing.CurrentValueChanged += new LaserGRBL.UserControls.NumericInput.IntegerInputBase.CurrentValueChangedEventHandler(this.IIBorderTracingCurrentValueChanged);
			// 
			// BtnPSHelper
			// 
			this.BtnPSHelper.AltImage = null;
			resources.ApplyResources(this.BtnPSHelper, "BtnPSHelper");
			this.BtnPSHelper.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtnPSHelper.Caption = null;
			this.BtnPSHelper.Coloration = System.Drawing.Color.Empty;
			this.BtnPSHelper.Image = ((System.Drawing.Image)(resources.GetObject("BtnPSHelper.Image")));
			this.BtnPSHelper.Name = "BtnPSHelper";
			this.BtnPSHelper.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.TT.SetToolTip(this.BtnPSHelper, resources.GetString("BtnPSHelper.ToolTip"));
			this.BtnPSHelper.UseAltImage = false;
			this.BtnPSHelper.Click += new System.EventHandler(this.BtnPSHelper_Click);
			// 
			// GbLaser
			// 
			resources.ApplyResources(this.GbLaser, "GbLaser");
			this.GbLaser.Controls.Add(this.tableLayoutPanel7);
			this.GbLaser.Name = "GbLaser";
			this.GbLaser.TabStop = false;
			// 
			// tableLayoutPanel7
			// 
			resources.ApplyResources(this.tableLayoutPanel7, "tableLayoutPanel7");
			this.tableLayoutPanel7.Controls.Add(this.BtnModulationInfo, 3, 1);
			this.tableLayoutPanel7.Controls.Add(this.LblSmin, 0, 1);
			this.tableLayoutPanel7.Controls.Add(this.IIMinPower, 1, 1);
			this.tableLayoutPanel7.Controls.Add(this.label18, 0, 0);
			this.tableLayoutPanel7.Controls.Add(this.BtnOnOffInfo, 3, 0);
			this.tableLayoutPanel7.Controls.Add(this.CBLaserON, 1, 0);
			this.tableLayoutPanel7.Controls.Add(this.LblSmax, 0, 2);
			this.tableLayoutPanel7.Controls.Add(this.IIMaxPower, 1, 2);
			this.tableLayoutPanel7.Controls.Add(this.LblMinPerc, 2, 1);
			this.tableLayoutPanel7.Controls.Add(this.LblMaxPerc, 2, 2);
			this.tableLayoutPanel7.Name = "tableLayoutPanel7";
			// 
			// BtnModulationInfo
			// 
			this.BtnModulationInfo.AltImage = null;
			resources.ApplyResources(this.BtnModulationInfo, "BtnModulationInfo");
			this.BtnModulationInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtnModulationInfo.Caption = null;
			this.BtnModulationInfo.Coloration = System.Drawing.Color.Empty;
			this.BtnModulationInfo.Image = ((System.Drawing.Image)(resources.GetObject("BtnModulationInfo.Image")));
			this.BtnModulationInfo.Name = "BtnModulationInfo";
			this.tableLayoutPanel7.SetRowSpan(this.BtnModulationInfo, 2);
			this.BtnModulationInfo.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
			this.TT.SetToolTip(this.BtnModulationInfo, resources.GetString("BtnModulationInfo.ToolTip"));
			this.BtnModulationInfo.UseAltImage = false;
			this.BtnModulationInfo.Click += new System.EventHandler(this.BtnModulationInfo_Click);
			// 
			// LblSmin
			// 
			resources.ApplyResources(this.LblSmin, "LblSmin");
			this.LblSmin.Name = "LblSmin";
			// 
			// IIMinPower
			// 
			resources.ApplyResources(this.IIMinPower, "IIMinPower");
			this.IIMinPower.ForcedText = null;
			this.IIMinPower.ForceMinMax = false;
			this.IIMinPower.MaxValue = 999;
			this.IIMinPower.MinValue = 0;
			this.IIMinPower.Name = "IIMinPower";
			this.IIMinPower.NormalBorderColor = System.Drawing.SystemColors.ActiveBorder;
			this.IIMinPower.CurrentValueChanged += new LaserGRBL.UserControls.NumericInput.IntegerInputBase.CurrentValueChangedEventHandler(this.IIMinPowerCurrentValueChanged);
			// 
			// label18
			// 
			resources.ApplyResources(this.label18, "label18");
			this.label18.Name = "label18";
			// 
			// BtnOnOffInfo
			// 
			this.BtnOnOffInfo.AltImage = null;
			resources.ApplyResources(this.BtnOnOffInfo, "BtnOnOffInfo");
			this.BtnOnOffInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtnOnOffInfo.Caption = null;
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
			this.tableLayoutPanel7.SetColumnSpan(this.CBLaserON, 2);
			resources.ApplyResources(this.CBLaserON, "CBLaserON");
			this.CBLaserON.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CBLaserON.FormattingEnabled = true;
			this.CBLaserON.Name = "CBLaserON";
			this.CBLaserON.SelectedIndexChanged += new System.EventHandler(this.CBLaserON_SelectedIndexChanged);
			// 
			// LblSmax
			// 
			resources.ApplyResources(this.LblSmax, "LblSmax");
			this.LblSmax.Name = "LblSmax";
			// 
			// IIMaxPower
			// 
			resources.ApplyResources(this.IIMaxPower, "IIMaxPower");
			this.IIMaxPower.CurrentValue = 1000;
			this.IIMaxPower.ForcedText = null;
			this.IIMaxPower.ForceMinMax = false;
			this.IIMaxPower.MaxValue = 1000;
			this.IIMaxPower.MinValue = 1;
			this.IIMaxPower.Name = "IIMaxPower";
			this.IIMaxPower.NormalBorderColor = System.Drawing.SystemColors.ActiveBorder;
			this.IIMaxPower.CurrentValueChanged += new LaserGRBL.UserControls.NumericInput.IntegerInputBase.CurrentValueChangedEventHandler(this.IIMaxPowerCurrentValueChanged);
			// 
			// LblMinPerc
			// 
			resources.ApplyResources(this.LblMinPerc, "LblMinPerc");
			this.LblMinPerc.Name = "LblMinPerc";
			// 
			// LblMaxPerc
			// 
			resources.ApplyResources(this.LblMaxPerc, "LblMaxPerc");
			this.LblMaxPerc.Name = "LblMaxPerc";
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
			// 
			// TT
			// 
			this.TT.AutoPopDelay = 10000;
			this.TT.InitialDelay = 500;
			this.TT.ReshowDelay = 100;
			// 
			// SvgToGCodeForm
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel9);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "SvgToGCodeForm";
			this.tableLayoutPanel9.ResumeLayout(false);
			this.tableLayoutPanel9.PerformLayout();
			this.GbSpeed.ResumeLayout(false);
			this.GbSpeed.PerformLayout();
			this.tableLayoutPanel6.ResumeLayout(false);
			this.tableLayoutPanel6.PerformLayout();
			this.GbLaser.ResumeLayout(false);
			this.GbLaser.PerformLayout();
			this.tableLayoutPanel7.ResumeLayout(false);
			this.tableLayoutPanel7.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		private UserControls.ImageButton BtnModulationInfo;
		private UserControls.ImageButton BtnOnOffInfo;
		private System.Windows.Forms.ComboBox CBLaserON;
		private System.Windows.Forms.ToolTip TT;
		private System.Windows.Forms.Label LblBorderTracingmm;
		private UserControls.ImageButton BtnPSHelper;
		private System.Windows.Forms.Label LblMinPerc;
		private System.Windows.Forms.Label LblMaxPerc;
	}
}
