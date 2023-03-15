namespace LaserGRBL
{
	partial class SaveOptionForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SaveOptionForm));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.CBLFLineEndings = new System.Windows.Forms.CheckBox();
            this.CBHeader = new System.Windows.Forms.CheckBox();
            this.CBFooter = new System.Windows.Forms.CheckBox();
            this.UDCount = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.CBBetween = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.BtnSave = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UDCount)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel3, 0, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.tableLayoutPanel2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.CBLFLineEndings, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.CBHeader, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.CBFooter, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.UDCount, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.CBBetween, 2, 1);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // CBLFLineEndings
            // 
            resources.ApplyResources(this.CBLFLineEndings, "CBLFLineEndings");
            this.tableLayoutPanel2.SetColumnSpan(this.CBLFLineEndings, 2);
            this.CBLFLineEndings.Name = "CBLFLineEndings";
            this.CBLFLineEndings.UseVisualStyleBackColor = true;
            // 
            // CBHeader
            // 
            resources.ApplyResources(this.CBHeader, "CBHeader");
            this.CBHeader.Checked = true;
            this.CBHeader.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tableLayoutPanel2.SetColumnSpan(this.CBHeader, 2);
            this.CBHeader.Name = "CBHeader";
            this.CBHeader.UseVisualStyleBackColor = true;
            // 
            // CBFooter
            // 
            resources.ApplyResources(this.CBFooter, "CBFooter");
            this.CBFooter.Checked = true;
            this.CBFooter.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tableLayoutPanel2.SetColumnSpan(this.CBFooter, 2);
            this.CBFooter.Name = "CBFooter";
            this.CBFooter.UseVisualStyleBackColor = true;
            // 
            // UDCount
            // 
            resources.ApplyResources(this.UDCount, "UDCount");
            this.UDCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.UDCount.Name = "UDCount";
            this.UDCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // CBBetween
            // 
            resources.ApplyResources(this.CBBetween, "CBBetween");
            this.CBBetween.Checked = true;
            this.CBBetween.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CBBetween.Name = "CBBetween";
            this.CBBetween.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // tableLayoutPanel3
            // 
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.Controls.Add(this.BtnSave, 1, 0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            // 
            // BtnSave
            // 
            this.BtnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.BtnSave, "BtnSave");
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.UseVisualStyleBackColor = true;
            // 
            // SaveOptionForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SaveOptionForm";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UDCount)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
		private System.Windows.Forms.Button BtnSave;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.CheckBox CBHeader;
		private System.Windows.Forms.CheckBox CBFooter;
		private System.Windows.Forms.NumericUpDown UDCount;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox CBBetween;
        private System.Windows.Forms.CheckBox CBLFLineEndings;
    }
}