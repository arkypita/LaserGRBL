namespace LaserGRBL
{
	partial class GrblConfig
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;


		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GrblConfig));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.BtnWrite = new System.Windows.Forms.Button();
			this.BtnRead = new System.Windows.Forms.Button();
			this.BtnCancel = new System.Windows.Forms.Button();
			this.LblConnect = new System.Windows.Forms.Label();
			this.BtnExport = new System.Windows.Forms.Button();
			this.BtnImport = new System.Windows.Forms.Button();
			this.GB = new System.Windows.Forms.GroupBox();
			this.DGV = new System.Windows.Forms.DataGridView();
			this.numberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.parameterDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.valueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.unitDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.BS = new System.Windows.Forms.BindingSource(this.components);
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.GB.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.DGV)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.BS)).BeginInit();
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
			this.tableLayoutPanel2.Controls.Add(this.BtnWrite, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.BtnRead, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.BtnCancel, 6, 0);
			this.tableLayoutPanel2.Controls.Add(this.LblConnect, 5, 0);
			this.tableLayoutPanel2.Controls.Add(this.BtnExport, 3, 0);
			this.tableLayoutPanel2.Controls.Add(this.BtnImport, 4, 0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			// 
			// BtnWrite
			// 
			resources.ApplyResources(this.BtnWrite, "BtnWrite");
			this.BtnWrite.Name = "BtnWrite";
			this.BtnWrite.UseVisualStyleBackColor = true;
			this.BtnWrite.Click += new System.EventHandler(this.BtnWrite_Click);
			// 
			// BtnRead
			// 
			resources.ApplyResources(this.BtnRead, "BtnRead");
			this.BtnRead.Name = "BtnRead";
			this.BtnRead.UseVisualStyleBackColor = true;
			this.BtnRead.Click += new System.EventHandler(this.BtnRead_Click);
			// 
			// BtnCancel
			// 
			this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			resources.ApplyResources(this.BtnCancel, "BtnCancel");
			this.BtnCancel.Name = "BtnCancel";
			this.BtnCancel.UseVisualStyleBackColor = true;
			this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
			// 
			// LblConnect
			// 
			resources.ApplyResources(this.LblConnect, "LblConnect");
			this.LblConnect.ForeColor = System.Drawing.Color.Red;
			this.LblConnect.Name = "LblConnect";
			// 
			// BtnExport
			// 
			resources.ApplyResources(this.BtnExport, "BtnExport");
			this.BtnExport.Name = "BtnExport";
			this.BtnExport.UseVisualStyleBackColor = true;
			this.BtnExport.Click += new System.EventHandler(this.BtnExport_Click);
			// 
			// BtnImport
			// 
			resources.ApplyResources(this.BtnImport, "BtnImport");
			this.BtnImport.Name = "BtnImport";
			this.BtnImport.UseVisualStyleBackColor = true;
			this.BtnImport.Click += new System.EventHandler(this.BtnImport_Click);
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
			this.DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.DGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.numberDataGridViewTextBoxColumn,
            this.parameterDataGridViewTextBoxColumn,
            this.valueDataGridViewTextBoxColumn,
            this.unitDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn});
			resources.ApplyResources(this.DGV, "DGV");
			this.DGV.Name = "DGV";
			this.DGV.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.DGV_DataError);
			// 
			// numberDataGridViewTextBoxColumn
			// 
			this.numberDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.numberDataGridViewTextBoxColumn.DataPropertyName = "Number";
			resources.ApplyResources(this.numberDataGridViewTextBoxColumn, "numberDataGridViewTextBoxColumn");
			this.numberDataGridViewTextBoxColumn.Name = "numberDataGridViewTextBoxColumn";
			this.numberDataGridViewTextBoxColumn.ReadOnly = true;
			// 
			// parameterDataGridViewTextBoxColumn
			// 
			this.parameterDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.parameterDataGridViewTextBoxColumn.DataPropertyName = "Parameter";
			resources.ApplyResources(this.parameterDataGridViewTextBoxColumn, "parameterDataGridViewTextBoxColumn");
			this.parameterDataGridViewTextBoxColumn.Name = "parameterDataGridViewTextBoxColumn";
			this.parameterDataGridViewTextBoxColumn.ReadOnly = true;
			// 
			// valueDataGridViewTextBoxColumn
			// 
			this.valueDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.valueDataGridViewTextBoxColumn.DataPropertyName = "Value";
			resources.ApplyResources(this.valueDataGridViewTextBoxColumn, "valueDataGridViewTextBoxColumn");
			this.valueDataGridViewTextBoxColumn.Name = "valueDataGridViewTextBoxColumn";
			// 
			// unitDataGridViewTextBoxColumn
			// 
			this.unitDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.unitDataGridViewTextBoxColumn.DataPropertyName = "Unit";
			resources.ApplyResources(this.unitDataGridViewTextBoxColumn, "unitDataGridViewTextBoxColumn");
			this.unitDataGridViewTextBoxColumn.Name = "unitDataGridViewTextBoxColumn";
			this.unitDataGridViewTextBoxColumn.ReadOnly = true;
			// 
			// descriptionDataGridViewTextBoxColumn
			// 
			this.descriptionDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
			resources.ApplyResources(this.descriptionDataGridViewTextBoxColumn, "descriptionDataGridViewTextBoxColumn");
			this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
			this.descriptionDataGridViewTextBoxColumn.ReadOnly = true;
			// 
			// BS
			// 
			this.BS.DataSource = typeof(LaserGRBL.GrblConf);
			// 
			// GrblConfig
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.BtnCancel;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "GrblConfig";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.GB.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.DGV)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.BS)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Button BtnCancel;
		private System.Windows.Forms.GroupBox GB;
		private System.Windows.Forms.DataGridView DGV;
		private System.Windows.Forms.Button BtnWrite;
		private System.Windows.Forms.Button BtnRead;
		private System.Windows.Forms.Button BtnImport;
		private System.Windows.Forms.Button BtnExport;
		private System.Windows.Forms.DataGridViewTextBoxColumn numberDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn parameterDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn valueDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn unitDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
		private System.Windows.Forms.BindingSource BS;
		private System.Windows.Forms.Label LblConnect;
    }
}