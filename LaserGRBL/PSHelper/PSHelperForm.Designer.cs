namespace LaserGRBL.PSHelper
{
	partial class PSHelperForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PSHelperForm));
			this.BtnApply = new System.Windows.Forms.Button();
			this.LblPower = new System.Windows.Forms.Label();
			this.CbAction = new System.Windows.Forms.ComboBox();
			this.LblModel = new System.Windows.Forms.Label();
			this.LblMaterial = new System.Windows.Forms.Label();
			this.LblThickness = new System.Windows.Forms.Label();
			this.LblAction = new System.Windows.Forms.Label();
			this.CbModel = new System.Windows.Forms.ComboBox();
			this.CbMaterial = new System.Windows.Forms.ComboBox();
			this.TbSpeed = new System.Windows.Forms.TextBox();
			this.LblPasses = new System.Windows.Forms.Label();
			this.LblSpeed = new System.Windows.Forms.Label();
			this.CbThickness = new System.Windows.Forms.ComboBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.TbPasses = new System.Windows.Forms.TextBox();
			this.TbPower = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
			this.BtnCancel = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.tableLayoutPanel2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.tableLayoutPanel4.SuspendLayout();
			this.tableLayoutPanel5.SuspendLayout();
			this.SuspendLayout();
			// 
			// BtnApply
			// 
			resources.ApplyResources(this.BtnApply, "BtnApply");
			this.BtnApply.Name = "BtnApply";
			this.BtnApply.UseVisualStyleBackColor = true;
			this.BtnApply.Click += new System.EventHandler(this.BtnApply_Click);
			// 
			// LblPower
			// 
			resources.ApplyResources(this.LblPower, "LblPower");
			this.LblPower.Name = "LblPower";
			// 
			// CbAction
			// 
			this.CbAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CbAction.FormattingEnabled = true;
			resources.ApplyResources(this.CbAction, "CbAction");
			this.CbAction.Name = "CbAction";
			this.CbAction.SelectedIndexChanged += new System.EventHandler(this.CbAction_SelectedIndexChanged);
			// 
			// LblModel
			// 
			resources.ApplyResources(this.LblModel, "LblModel");
			this.LblModel.Name = "LblModel";
			// 
			// LblMaterial
			// 
			resources.ApplyResources(this.LblMaterial, "LblMaterial");
			this.LblMaterial.Name = "LblMaterial";
			// 
			// LblThickness
			// 
			resources.ApplyResources(this.LblThickness, "LblThickness");
			this.LblThickness.Name = "LblThickness";
			// 
			// LblAction
			// 
			resources.ApplyResources(this.LblAction, "LblAction");
			this.LblAction.Name = "LblAction";
			// 
			// CbModel
			// 
			this.CbModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CbModel.FormattingEnabled = true;
			resources.ApplyResources(this.CbModel, "CbModel");
			this.CbModel.Name = "CbModel";
			this.CbModel.SelectedIndexChanged += new System.EventHandler(this.CbModel_SelectedIndexChanged);
			// 
			// CbMaterial
			// 
			this.CbMaterial.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CbMaterial.FormattingEnabled = true;
			resources.ApplyResources(this.CbMaterial, "CbMaterial");
			this.CbMaterial.Name = "CbMaterial";
			this.CbMaterial.SelectedIndexChanged += new System.EventHandler(this.CbMaterial_SelectedIndexChanged);
			// 
			// TbSpeed
			// 
			resources.ApplyResources(this.TbSpeed, "TbSpeed");
			this.TbSpeed.Name = "TbSpeed";
			this.TbSpeed.ReadOnly = true;
			// 
			// LblPasses
			// 
			resources.ApplyResources(this.LblPasses, "LblPasses");
			this.LblPasses.Name = "LblPasses";
			// 
			// LblSpeed
			// 
			resources.ApplyResources(this.LblSpeed, "LblSpeed");
			this.LblSpeed.Name = "LblSpeed";
			// 
			// CbThickness
			// 
			this.CbThickness.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CbThickness.FormattingEnabled = true;
			resources.ApplyResources(this.CbThickness, "CbThickness");
			this.CbThickness.Name = "CbThickness";
			this.CbThickness.SelectedIndexChanged += new System.EventHandler(this.CbThickness_SelectedIndexChanged);
			// 
			// tableLayoutPanel2
			// 
			resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
			this.tableLayoutPanel2.Controls.Add(this.TbPasses, 1, 2);
			this.tableLayoutPanel2.Controls.Add(this.TbSpeed, 1, 1);
			this.tableLayoutPanel2.Controls.Add(this.LblPasses, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this.LblSpeed, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.LblPower, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.TbPower, 1, 0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			// 
			// TbPasses
			// 
			resources.ApplyResources(this.TbPasses, "TbPasses");
			this.TbPasses.Name = "TbPasses";
			this.TbPasses.ReadOnly = true;
			// 
			// TbPower
			// 
			resources.ApplyResources(this.TbPower, "TbPower");
			this.TbPower.Name = "TbPower";
			this.TbPower.ReadOnly = true;
			// 
			// groupBox1
			// 
			resources.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Controls.Add(this.tableLayoutPanel2);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			// 
			// tableLayoutPanel1
			// 
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.LblModel, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.LblMaterial, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.CbModel, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.CbMaterial, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.LblThickness, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.LblAction, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.CbThickness, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.CbAction, 1, 2);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			// 
			// tableLayoutPanel3
			// 
			resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
			this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel1, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.groupBox1, 1, 0);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			// 
			// tableLayoutPanel4
			// 
			resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
			this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel3, 0, 0);
			this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel5, 0, 1);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			// 
			// tableLayoutPanel5
			// 
			resources.ApplyResources(this.tableLayoutPanel5, "tableLayoutPanel5");
			this.tableLayoutPanel5.Controls.Add(this.BtnApply, 2, 1);
			this.tableLayoutPanel5.Controls.Add(this.BtnCancel, 1, 1);
			this.tableLayoutPanel5.Controls.Add(this.label1, 0, 0);
			this.tableLayoutPanel5.Name = "tableLayoutPanel5";
			// 
			// BtnCancel
			// 
			resources.ApplyResources(this.BtnCancel, "BtnCancel");
			this.BtnCancel.Name = "BtnCancel";
			this.BtnCancel.UseVisualStyleBackColor = true;
			this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
			// 
			// label1
			// 
			this.tableLayoutPanel5.SetColumnSpan(this.label1, 3);
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// PSHelperForm
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel4);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "PSHelperForm";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.tableLayoutPanel4.ResumeLayout(false);
			this.tableLayoutPanel4.PerformLayout();
			this.tableLayoutPanel5.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button BtnApply;
		private System.Windows.Forms.Label LblPower;
		private System.Windows.Forms.ComboBox CbAction;
		private System.Windows.Forms.Label LblModel;
		private System.Windows.Forms.Label LblMaterial;
		private System.Windows.Forms.Label LblThickness;
		private System.Windows.Forms.Label LblAction;
		private System.Windows.Forms.ComboBox CbModel;
		private System.Windows.Forms.ComboBox CbMaterial;
		private System.Windows.Forms.TextBox TbSpeed;
		private System.Windows.Forms.Label LblPasses;
		private System.Windows.Forms.Label LblSpeed;
		private System.Windows.Forms.ComboBox CbThickness;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.TextBox TbPasses;
		private System.Windows.Forms.TextBox TbPower;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
		private System.Windows.Forms.Button BtnCancel;
		private System.Windows.Forms.Label label1;
	}
}