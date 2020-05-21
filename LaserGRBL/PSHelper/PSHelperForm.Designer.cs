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
			this.BtnApply.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.BtnApply.Location = new System.Drawing.Point(293, 4);
			this.BtnApply.Name = "BtnApply";
			this.BtnApply.Size = new System.Drawing.Size(75, 22);
			this.BtnApply.TabIndex = 11;
			this.BtnApply.Text = "Apply";
			this.BtnApply.UseVisualStyleBackColor = true;
			this.BtnApply.Click += new System.EventHandler(this.BtnApply_Click);
			// 
			// LblPower
			// 
			this.LblPower.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblPower.AutoSize = true;
			this.LblPower.Location = new System.Drawing.Point(3, 8);
			this.LblPower.Name = "LblPower";
			this.LblPower.Size = new System.Drawing.Size(37, 13);
			this.LblPower.TabIndex = 4;
			this.LblPower.Text = "Power";
			// 
			// CbAction
			// 
			this.CbAction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CbAction.FormattingEnabled = true;
			this.CbAction.Location = new System.Drawing.Point(74, 57);
			this.CbAction.Name = "CbAction";
			this.CbAction.Size = new System.Drawing.Size(121, 21);
			this.CbAction.TabIndex = 8;
			this.CbAction.SelectedIndexChanged += new System.EventHandler(this.CbAction_SelectedIndexChanged);
			// 
			// LblModel
			// 
			this.LblModel.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblModel.AutoSize = true;
			this.LblModel.Location = new System.Drawing.Point(3, 7);
			this.LblModel.Name = "LblModel";
			this.LblModel.Size = new System.Drawing.Size(65, 13);
			this.LblModel.TabIndex = 0;
			this.LblModel.Text = "Laser Model";
			// 
			// LblMaterial
			// 
			this.LblMaterial.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblMaterial.AutoSize = true;
			this.LblMaterial.Location = new System.Drawing.Point(3, 34);
			this.LblMaterial.Name = "LblMaterial";
			this.LblMaterial.Size = new System.Drawing.Size(44, 13);
			this.LblMaterial.TabIndex = 1;
			this.LblMaterial.Text = "Material";
			// 
			// LblThickness
			// 
			this.LblThickness.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblThickness.AutoSize = true;
			this.LblThickness.Location = new System.Drawing.Point(3, 88);
			this.LblThickness.Name = "LblThickness";
			this.LblThickness.Size = new System.Drawing.Size(56, 13);
			this.LblThickness.TabIndex = 2;
			this.LblThickness.Text = "Thickness";
			// 
			// LblAction
			// 
			this.LblAction.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblAction.AutoSize = true;
			this.LblAction.Location = new System.Drawing.Point(3, 61);
			this.LblAction.Name = "LblAction";
			this.LblAction.Size = new System.Drawing.Size(37, 13);
			this.LblAction.TabIndex = 3;
			this.LblAction.Text = "Action";
			// 
			// CbModel
			// 
			this.CbModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CbModel.FormattingEnabled = true;
			this.CbModel.Location = new System.Drawing.Point(74, 3);
			this.CbModel.Name = "CbModel";
			this.CbModel.Size = new System.Drawing.Size(121, 21);
			this.CbModel.TabIndex = 5;
			this.CbModel.SelectedIndexChanged += new System.EventHandler(this.CbModel_SelectedIndexChanged);
			// 
			// CbMaterial
			// 
			this.CbMaterial.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CbMaterial.FormattingEnabled = true;
			this.CbMaterial.Location = new System.Drawing.Point(74, 30);
			this.CbMaterial.Name = "CbMaterial";
			this.CbMaterial.Size = new System.Drawing.Size(121, 21);
			this.CbMaterial.TabIndex = 6;
			this.CbMaterial.SelectedIndexChanged += new System.EventHandler(this.CbMaterial_SelectedIndexChanged);
			// 
			// TbSpeed
			// 
			this.TbSpeed.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.TbSpeed.Location = new System.Drawing.Point(50, 33);
			this.TbSpeed.Name = "TbSpeed";
			this.TbSpeed.ReadOnly = true;
			this.TbSpeed.Size = new System.Drawing.Size(100, 20);
			this.TbSpeed.TabIndex = 8;
			// 
			// LblPasses
			// 
			this.LblPasses.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblPasses.AutoSize = true;
			this.LblPasses.Location = new System.Drawing.Point(3, 67);
			this.LblPasses.Name = "LblPasses";
			this.LblPasses.Size = new System.Drawing.Size(41, 13);
			this.LblPasses.TabIndex = 6;
			this.LblPasses.Text = "Passes";
			// 
			// LblSpeed
			// 
			this.LblSpeed.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblSpeed.AutoSize = true;
			this.LblSpeed.Location = new System.Drawing.Point(3, 37);
			this.LblSpeed.Name = "LblSpeed";
			this.LblSpeed.Size = new System.Drawing.Size(38, 13);
			this.LblSpeed.TabIndex = 5;
			this.LblSpeed.Text = "Speed";
			// 
			// CbThickness
			// 
			this.CbThickness.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CbThickness.FormattingEnabled = true;
			this.CbThickness.Location = new System.Drawing.Point(74, 84);
			this.CbThickness.Name = "CbThickness";
			this.CbThickness.Size = new System.Drawing.Size(121, 21);
			this.CbThickness.TabIndex = 7;
			this.CbThickness.SelectedIndexChanged += new System.EventHandler(this.CbThickness_SelectedIndexChanged);
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.AutoSize = true;
			this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel2.ColumnCount = 2;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.Controls.Add(this.TbPasses, 1, 2);
			this.tableLayoutPanel2.Controls.Add(this.TbSpeed, 1, 1);
			this.tableLayoutPanel2.Controls.Add(this.LblPasses, 0, 2);
			this.tableLayoutPanel2.Controls.Add(this.LblSpeed, 0, 1);
			this.tableLayoutPanel2.Controls.Add(this.LblPower, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.TbPower, 1, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 3;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(153, 89);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// TbPasses
			// 
			this.TbPasses.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.TbPasses.Location = new System.Drawing.Point(50, 63);
			this.TbPasses.Name = "TbPasses";
			this.TbPasses.ReadOnly = true;
			this.TbPasses.Size = new System.Drawing.Size(100, 20);
			this.TbPasses.TabIndex = 9;
			// 
			// TbPower
			// 
			this.TbPower.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.TbPower.Location = new System.Drawing.Point(50, 4);
			this.TbPower.Name = "TbPower";
			this.TbPower.ReadOnly = true;
			this.TbPower.Size = new System.Drawing.Size(100, 20);
			this.TbPower.TabIndex = 7;
			// 
			// groupBox1
			// 
			this.groupBox1.AutoSize = true;
			this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.groupBox1.Controls.Add(this.tableLayoutPanel2);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox1.Location = new System.Drawing.Point(207, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(159, 108);
			this.groupBox1.TabIndex = 10;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Suggested settings";
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.LblModel, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.LblMaterial, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.CbModel, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.CbMaterial, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.LblThickness, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.LblAction, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.CbThickness, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.CbAction, 1, 2);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(198, 108);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.AutoSize = true;
			this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel3.ColumnCount = 2;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel1, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.groupBox1, 1, 0);
			this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 1;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(369, 114);
			this.tableLayoutPanel3.TabIndex = 12;
			// 
			// tableLayoutPanel4
			// 
			this.tableLayoutPanel4.AutoSize = true;
			this.tableLayoutPanel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel4.ColumnCount = 1;
			this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel3, 0, 0);
			this.tableLayoutPanel4.Controls.Add(this.tableLayoutPanel5, 0, 1);
			this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel4.Name = "tableLayoutPanel4";
			this.tableLayoutPanel4.RowCount = 2;
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel4.Size = new System.Drawing.Size(377, 157);
			this.tableLayoutPanel4.TabIndex = 13;
			// 
			// tableLayoutPanel5
			// 
			this.tableLayoutPanel5.AutoSize = true;
			this.tableLayoutPanel5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel5.ColumnCount = 3;
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel5.Controls.Add(this.BtnApply, 2, 0);
			this.tableLayoutPanel5.Controls.Add(this.BtnCancel, 1, 0);
			this.tableLayoutPanel5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel5.Location = new System.Drawing.Point(3, 123);
			this.tableLayoutPanel5.Name = "tableLayoutPanel5";
			this.tableLayoutPanel5.RowCount = 1;
			this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel5.Size = new System.Drawing.Size(371, 31);
			this.tableLayoutPanel5.TabIndex = 13;
			// 
			// BtnCancel
			// 
			this.BtnCancel.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.BtnCancel.Location = new System.Drawing.Point(212, 4);
			this.BtnCancel.Name = "BtnCancel";
			this.BtnCancel.Size = new System.Drawing.Size(75, 22);
			this.BtnCancel.TabIndex = 12;
			this.BtnCancel.Text = "Cancel";
			this.BtnCancel.UseVisualStyleBackColor = true;
			this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
			// 
			// PSHelperForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.ClientSize = new System.Drawing.Size(377, 157);
			this.Controls.Add(this.tableLayoutPanel4);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
			this.Name = "PSHelperForm";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Select your configuration";
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
	}
}