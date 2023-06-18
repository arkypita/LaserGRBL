/*
 * Created by SharpDevelop.
 * User: Diego
 * Date: 15/01/2017
 * Time: 12:12
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
namespace LaserGRBL
{
	partial class ChangeScaleForm
	{
		/// <summary>
		/// Designer variable used to keep track of non-visual components.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel9;
		private System.Windows.Forms.GroupBox GbSpeed;
		private System.Windows.Forms.GroupBox GbLaser;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChangeScaleForm));
            this.tableLayoutPanel9 = new System.Windows.Forms.TableLayoutPanel();
            this.GbSpeed = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.DIscaleX = new LaserGRBL.UserControls.NumericInput.DecimalInputBase();
            this.DIscaleY = new LaserGRBL.UserControls.NumericInput.DecimalInputBase();
            this.GbLaser = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.DIsizeX = new LaserGRBL.UserControls.NumericInput.DecimalInputBase();
            this.DIsizeY = new LaserGRBL.UserControls.NumericInput.DecimalInputBase();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.BtnCreate = new System.Windows.Forms.Button();
            this.ChKeepRatio = new System.Windows.Forms.CheckBox();
            this.TT = new System.Windows.Forms.ToolTip(this.components);
            this.tableLayoutPanel9.SuspendLayout();
            this.GbSpeed.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.GbLaser.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel9
            // 
            resources.ApplyResources(this.tableLayoutPanel9, "tableLayoutPanel9");
            this.tableLayoutPanel9.Controls.Add(this.GbSpeed, 0, 0);
            this.tableLayoutPanel9.Controls.Add(this.GbLaser, 0, 1);
            this.tableLayoutPanel9.Controls.Add(this.tableLayoutPanel1, 0, 4);
            this.tableLayoutPanel9.Controls.Add(this.ChKeepRatio, 0, 2);
            this.tableLayoutPanel9.Name = "tableLayoutPanel9";
            this.TT.SetToolTip(this.tableLayoutPanel9, resources.GetString("tableLayoutPanel9.ToolTip"));
            // 
            // GbSpeed
            // 
            resources.ApplyResources(this.GbSpeed, "GbSpeed");
            this.GbSpeed.Controls.Add(this.tableLayoutPanel2);
            this.GbSpeed.Name = "GbSpeed";
            this.GbSpeed.TabStop = false;
            this.TT.SetToolTip(this.GbSpeed, resources.GetString("GbSpeed.ToolTip"));
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label2, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.DIscaleX, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.DIscaleY, 4, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.TT.SetToolTip(this.tableLayoutPanel2, resources.GetString("tableLayoutPanel2.ToolTip"));
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            this.TT.SetToolTip(this.label1, resources.GetString("label1.ToolTip"));
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            this.TT.SetToolTip(this.label2, resources.GetString("label2.ToolTip"));
            // 
            // DIscaleX
            // 
            resources.ApplyResources(this.DIscaleX, "DIscaleX");
            this.DIscaleX.CurrentValue = 0F;
            this.DIscaleX.Name = "DIscaleX";
            this.TT.SetToolTip(this.DIscaleX, resources.GetString("DIscaleX.ToolTip"));
            this.DIscaleX.CurrentValueChanged += new LaserGRBL.UserControls.NumericInput.DecimalInputBase.CurrentValueChangedDlg(this.DIscaleX_CurrentValueChanged);
            // 
            // DIscaleY
            // 
            resources.ApplyResources(this.DIscaleY, "DIscaleY");
            this.DIscaleY.CurrentValue = 0F;
            this.DIscaleY.Name = "DIscaleY";
            this.TT.SetToolTip(this.DIscaleY, resources.GetString("DIscaleY.ToolTip"));
            this.DIscaleY.CurrentValueChanged += new LaserGRBL.UserControls.NumericInput.DecimalInputBase.CurrentValueChangedDlg(this.DIscaleY_CurrentValueChanged);
            // 
            // GbLaser
            // 
            resources.ApplyResources(this.GbLaser, "GbLaser");
            this.GbLaser.Controls.Add(this.tableLayoutPanel3);
            this.GbLaser.Name = "GbLaser";
            this.GbLaser.TabStop = false;
            this.TT.SetToolTip(this.GbLaser, resources.GetString("GbLaser.ToolTip"));
            // 
            // tableLayoutPanel3
            // 
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.label4, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.DIsizeX, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.DIsizeY, 4, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.TT.SetToolTip(this.tableLayoutPanel3, resources.GetString("tableLayoutPanel3.ToolTip"));
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            this.TT.SetToolTip(this.label3, resources.GetString("label3.ToolTip"));
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            this.TT.SetToolTip(this.label4, resources.GetString("label4.ToolTip"));
            // 
            // DIsizeX
            // 
            resources.ApplyResources(this.DIsizeX, "DIsizeX");
            this.DIsizeX.CurrentValue = 0F;
            this.DIsizeX.Name = "DIsizeX";
            this.TT.SetToolTip(this.DIsizeX, resources.GetString("DIsizeX.ToolTip"));
            this.DIsizeX.CurrentValueChanged += new LaserGRBL.UserControls.NumericInput.DecimalInputBase.CurrentValueChangedDlg(this.DIsizeX_CurrentValueChanged);
            // 
            // DIsizeY
            // 
            resources.ApplyResources(this.DIsizeY, "DIsizeY");
            this.DIsizeY.CurrentValue = 0F;
            this.DIsizeY.Name = "DIsizeY";
            this.TT.SetToolTip(this.DIsizeY, resources.GetString("DIsizeY.ToolTip"));
            this.DIsizeY.CurrentValueChanged += new LaserGRBL.UserControls.NumericInput.DecimalInputBase.CurrentValueChangedDlg(this.DIsizeY_CurrentValueChanged);
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
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // BtnCreate
            // 
            resources.ApplyResources(this.BtnCreate, "BtnCreate");
            this.BtnCreate.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.BtnCreate.Name = "BtnCreate";
            this.TT.SetToolTip(this.BtnCreate, resources.GetString("BtnCreate.ToolTip"));
            this.BtnCreate.UseVisualStyleBackColor = true;
            this.BtnCreate.Click += new System.EventHandler(this.BtnCreate_Click);
            // 
            // ChKeepRatio
            // 
            resources.ApplyResources(this.ChKeepRatio, "ChKeepRatio");
            this.ChKeepRatio.Name = "ChKeepRatio";
            this.TT.SetToolTip(this.ChKeepRatio, resources.GetString("ChKeepRatio.ToolTip"));
            this.ChKeepRatio.UseVisualStyleBackColor = true;
            this.ChKeepRatio.CheckedChanged += new System.EventHandler(this.ChKeepRatio_CheckedChanged);
            // 
            // TT
            // 
            this.TT.AutoPopDelay = 10000;
            this.TT.InitialDelay = 500;
            this.TT.ReshowDelay = 100;
            // 
            // ChangeScaleForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel9);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ChangeScaleForm";
            this.TT.SetToolTip(this, resources.GetString("$this.ToolTip"));
            this.tableLayoutPanel9.ResumeLayout(false);
            this.tableLayoutPanel9.PerformLayout();
            this.GbSpeed.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.GbLaser.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		private System.Windows.Forms.ToolTip TT;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private UserControls.NumericInput.DecimalInputBase DIscaleX;
        private UserControls.NumericInput.DecimalInputBase DIscaleY;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private UserControls.NumericInput.DecimalInputBase DIsizeX;
        private UserControls.NumericInput.DecimalInputBase DIsizeY;
        private System.Windows.Forms.CheckBox ChKeepRatio;
    }
}
