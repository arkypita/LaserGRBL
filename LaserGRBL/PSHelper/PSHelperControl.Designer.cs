namespace LaserGRBL.PSHelper
{
	partial class PSHelperControl
	{
		/// <summary> 
		/// Variabile di progettazione necessaria.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Pulire le risorse in uso.
		/// </summary>
		/// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Codice generato da Progettazione componenti

		/// <summary> 
		/// Metodo necessario per il supporto della finestra di progettazione. Non modificare 
		/// il contenuto del metodo con l'editor di codice.
		/// </summary>
		private void InitializeComponent()
		{
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.LblModel = new System.Windows.Forms.Label();
			this.LblMaterial = new System.Windows.Forms.Label();
			this.LblThickness = new System.Windows.Forms.Label();
			this.LblAction = new System.Windows.Forms.Label();
			this.CbModel = new System.Windows.Forms.ComboBox();
			this.CbMaterial = new System.Windows.Forms.ComboBox();
			this.CbThickness = new System.Windows.Forms.ComboBox();
			this.CbAction = new System.Windows.Forms.ComboBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.LblPasses = new System.Windows.Forms.Label();
			this.LblSpeed = new System.Windows.Forms.Label();
			this.LblPower = new System.Windows.Forms.Label();
			this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
			this.TbPower = new System.Windows.Forms.TextBox();
			this.TbSpeed = new System.Windows.Forms.TextBox();
			this.TbPasses = new System.Windows.Forms.TextBox();
			this.BtnApply = new System.Windows.Forms.Button();
			this.tableLayoutPanel1.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.tableLayoutPanel3.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel1.ColumnCount = 2;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.CbAction, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.LblModel, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.LblMaterial, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.LblThickness, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.LblAction, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.CbModel, 1, 0);
			this.tableLayoutPanel1.Controls.Add(this.CbMaterial, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.CbThickness, 1, 2);
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
			this.LblThickness.Location = new System.Drawing.Point(3, 61);
			this.LblThickness.Name = "LblThickness";
			this.LblThickness.Size = new System.Drawing.Size(56, 13);
			this.LblThickness.TabIndex = 2;
			this.LblThickness.Text = "Thickness";
			// 
			// LblAction
			// 
			this.LblAction.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.LblAction.AutoSize = true;
			this.LblAction.Location = new System.Drawing.Point(3, 88);
			this.LblAction.Name = "LblAction";
			this.LblAction.Size = new System.Drawing.Size(37, 13);
			this.LblAction.TabIndex = 3;
			this.LblAction.Text = "Action";
			// 
			// CbModel
			// 
			this.CbModel.FormattingEnabled = true;
			this.CbModel.Location = new System.Drawing.Point(74, 3);
			this.CbModel.Name = "CbModel";
			this.CbModel.Size = new System.Drawing.Size(121, 21);
			this.CbModel.TabIndex = 5;
			// 
			// CbMaterial
			// 
			this.CbMaterial.FormattingEnabled = true;
			this.CbMaterial.Location = new System.Drawing.Point(74, 30);
			this.CbMaterial.Name = "CbMaterial";
			this.CbMaterial.Size = new System.Drawing.Size(121, 21);
			this.CbMaterial.TabIndex = 6;
			// 
			// CbThickness
			// 
			this.CbThickness.FormattingEnabled = true;
			this.CbThickness.Location = new System.Drawing.Point(74, 57);
			this.CbThickness.Name = "CbThickness";
			this.CbThickness.Size = new System.Drawing.Size(121, 21);
			this.CbThickness.TabIndex = 7;
			// 
			// CbAction
			// 
			this.CbAction.FormattingEnabled = true;
			this.CbAction.Location = new System.Drawing.Point(74, 84);
			this.CbAction.Name = "CbAction";
			this.CbAction.Size = new System.Drawing.Size(121, 21);
			this.CbAction.TabIndex = 8;
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
			// tableLayoutPanel3
			// 
			this.tableLayoutPanel3.AutoSize = true;
			this.tableLayoutPanel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel3.ColumnCount = 2;
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel1, 0, 0);
			this.tableLayoutPanel3.Controls.Add(this.groupBox1, 1, 0);
			this.tableLayoutPanel3.Controls.Add(this.BtnApply, 1, 1);
			this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel3.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel3.Name = "tableLayoutPanel3";
			this.tableLayoutPanel3.RowCount = 2;
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
			this.tableLayoutPanel3.Size = new System.Drawing.Size(369, 142);
			this.tableLayoutPanel3.TabIndex = 11;
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
			// TbSpeed
			// 
			this.TbSpeed.Anchor = System.Windows.Forms.AnchorStyles.Left;
			this.TbSpeed.Location = new System.Drawing.Point(50, 33);
			this.TbSpeed.Name = "TbSpeed";
			this.TbSpeed.ReadOnly = true;
			this.TbSpeed.Size = new System.Drawing.Size(100, 20);
			this.TbSpeed.TabIndex = 8;
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
			// BtnApply
			// 
			this.BtnApply.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.BtnApply.Location = new System.Drawing.Point(291, 117);
			this.BtnApply.Name = "BtnApply";
			this.BtnApply.Size = new System.Drawing.Size(75, 22);
			this.BtnApply.TabIndex = 11;
			this.BtnApply.Text = "Apply";
			this.BtnApply.UseVisualStyleBackColor = true;
			// 
			// PSHelperControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.Controls.Add(this.tableLayoutPanel3);
			this.Name = "PSHelperControl";
			this.Size = new System.Drawing.Size(369, 142);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.tableLayoutPanel3.ResumeLayout(false);
			this.tableLayoutPanel3.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label LblModel;
		private System.Windows.Forms.Label LblMaterial;
		private System.Windows.Forms.Label LblThickness;
		private System.Windows.Forms.Label LblAction;
		private System.Windows.Forms.ComboBox CbModel;
		private System.Windows.Forms.ComboBox CbMaterial;
		private System.Windows.Forms.ComboBox CbThickness;
		private System.Windows.Forms.ComboBox CbAction;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Label LblPasses;
		private System.Windows.Forms.Label LblSpeed;
		private System.Windows.Forms.Label LblPower;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.TextBox TbPasses;
		private System.Windows.Forms.TextBox TbSpeed;
		private System.Windows.Forms.TextBox TbPower;
		private System.Windows.Forms.Button BtnApply;
	}
}
