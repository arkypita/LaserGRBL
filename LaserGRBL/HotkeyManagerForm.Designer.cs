namespace LaserGRBL
{
	partial class HotkeyManagerForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HotkeyManagerForm));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.LblConnect = new System.Windows.Forms.Label();
			this.BtnCancel = new LaserGRBL.UserControls.GrblButton();
			this.BtnSave = new LaserGRBL.UserControls.GrblButton();
			this.GB = new LaserGRBL.UserControls.GrblGroupBox();
			this.DGV = new LaserGRBL.MyDatagridView();
			this.Action = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Shortcut = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.GB.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.DGV)).BeginInit();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.GB, 0, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			// 
			// tableLayoutPanel2
			// 
			resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
			this.tableLayoutPanel2.Controls.Add(this.LblConnect, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.BtnCancel, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.BtnSave, 2, 0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			// 
			// LblConnect
			// 
			resources.ApplyResources(this.LblConnect, "LblConnect");
			this.LblConnect.ForeColor = System.Drawing.Color.Red;
			this.LblConnect.Name = "LblConnect";
			// 
			// BtnCancel
			// 
			this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			resources.ApplyResources(this.BtnCancel, "BtnCancel");
			this.BtnCancel.Name = "BtnCancel";
			this.BtnCancel.UseVisualStyleBackColor = true;
			// 
			// BtnSave
			// 
			this.BtnSave.DialogResult = System.Windows.Forms.DialogResult.OK;
			resources.ApplyResources(this.BtnSave, "BtnSave");
			this.BtnSave.Name = "BtnSave";
			this.BtnSave.UseVisualStyleBackColor = true;
			this.BtnSave.Click += new System.EventHandler(this.BtnSave_Click);
			// 
			// GB
			// 
			this.GB.Controls.Add(this.DGV);
			resources.ApplyResources(this.GB, "GB");
			this.GB.Name = "GB";
			this.GB.TabStop = false;
			// 
			// DGV
			// 
			this.DGV.AllowUserToAddRows = false;
			this.DGV.AllowUserToDeleteRows = false;
			this.DGV.AllowUserToResizeRows = false;
			this.DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.DGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Action,
            this.Shortcut});
			resources.ApplyResources(this.DGV, "DGV");
			this.DGV.Name = "DGV";
			this.DGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			// 
			// Action
			// 
			this.Action.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.Action.DataPropertyName = "ActionString";
			resources.ApplyResources(this.Action, "Action");
			this.Action.Name = "Action";
			this.Action.ReadOnly = true;
			// 
			// Shortcut
			// 
			this.Shortcut.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.Shortcut.DataPropertyName = "Combination";
			resources.ApplyResources(this.Shortcut, "Shortcut");
			this.Shortcut.Name = "Shortcut";
			this.Shortcut.ReadOnly = true;
			// 
			// HotkeyManagerForm
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tableLayoutPanel1);
			this.KeyPreview = true;
			this.Name = "HotkeyManagerForm";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.GB.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.DGV)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private LaserGRBL.UserControls.GrblGroupBox GB;
		private LaserGRBL.UserControls.GrblButton BtnCancel;
		private MyDatagridView DGV;
		private LaserGRBL.UserControls.GrblButton BtnSave;
		private System.Windows.Forms.Label LblConnect;
		private System.Windows.Forms.DataGridViewTextBoxColumn Action;
		private System.Windows.Forms.DataGridViewTextBoxColumn Shortcut;
	}
}