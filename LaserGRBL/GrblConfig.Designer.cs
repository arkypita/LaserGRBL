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
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.BtnWrite = new LaserGRBL.UserControls.GrblButton();
			this.BtnRead = new LaserGRBL.UserControls.GrblButton();
			this.BtnCancel = new LaserGRBL.UserControls.GrblButton();
			this.LblConnect = new System.Windows.Forms.Label();
			this.BtnExport = new LaserGRBL.UserControls.GrblButton();
			this.BtnImport = new LaserGRBL.UserControls.GrblButton();
			this.GB = new LaserGRBL.UserControls.GrblGroupBox();
			this.LblAction = new System.Windows.Forms.Label();
			this.DGV = new LaserGRBL.MyDatagridView();
			this.DollarNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Parameter = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.Number = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.ActionTimer = new System.Windows.Forms.Timer(this.components);
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
			this.GB.Controls.Add(this.LblAction);
			this.GB.Controls.Add(this.DGV);
			resources.ApplyResources(this.GB, "GB");
			this.GB.Name = "GB";
			this.GB.TabStop = false;
			// 
			// LblAction
			// 
			resources.ApplyResources(this.LblAction, "LblAction");
			this.LblAction.BackColor = System.Drawing.Color.White;
			this.LblAction.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.LblAction.ForeColor = System.Drawing.Color.Red;
			this.LblAction.Name = "LblAction";
			// 
			// DGV
			// 
			this.DGV.AllowUserToAddRows = false;
			this.DGV.AllowUserToDeleteRows = false;
			this.DGV.AllowUserToResizeRows = false;
			this.DGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.DGV.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DollarNumber,
            this.Parameter,
            this.Value,
            this.Unit,
            this.Description,
            this.Number});
			resources.ApplyResources(this.DGV, "DGV");
			this.DGV.Name = "DGV";
			this.DGV.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.DGV_CellValidating);
			this.DGV.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.DGV_DataError);
			// 
			// DollarNumber
			// 
			this.DollarNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.DollarNumber.DataPropertyName = "DollarNumber";
			resources.ApplyResources(this.DollarNumber, "DollarNumber");
			this.DollarNumber.Name = "DollarNumber";
			this.DollarNumber.ReadOnly = true;
			// 
			// Parameter
			// 
			this.Parameter.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.Parameter.DataPropertyName = "Parameter";
			resources.ApplyResources(this.Parameter, "Parameter");
			this.Parameter.Name = "Parameter";
			this.Parameter.ReadOnly = true;
			// 
			// Value
			// 
			this.Value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.Value.DataPropertyName = "Value";
			dataGridViewCellStyle1.BackColor = System.Drawing.Color.WhiteSmoke;
			dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
			this.Value.DefaultCellStyle = dataGridViewCellStyle1;
			resources.ApplyResources(this.Value, "Value");
			this.Value.Name = "Value";
			// 
			// Unit
			// 
			this.Unit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
			this.Unit.DataPropertyName = "Unit";
			resources.ApplyResources(this.Unit, "Unit");
			this.Unit.Name = "Unit";
			this.Unit.ReadOnly = true;
			// 
			// Description
			// 
			this.Description.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.Description.DataPropertyName = "Description";
			resources.ApplyResources(this.Description, "Description");
			this.Description.Name = "Description";
			this.Description.ReadOnly = true;
			// 
			// Number
			// 
			this.Number.DataPropertyName = "Number";
			resources.ApplyResources(this.Number, "Number");
			this.Number.Name = "Number";
			this.Number.ReadOnly = true;
			// 
			// ActionTimer
			// 
			this.ActionTimer.Interval = 5000;
			this.ActionTimer.Tick += new System.EventHandler(this.ActionTimer_Tick);
			// 
			// GrblConfig
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.BtnCancel;
			this.Controls.Add(this.tableLayoutPanel1);
			this.Name = "GrblConfig";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GrblConfig_FormClosing);
			this.Load += new System.EventHandler(this.GrblConfig_Load);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.GB.ResumeLayout(false);
			this.GB.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.DGV)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private LaserGRBL.UserControls.GrblButton BtnCancel;
		private LaserGRBL.UserControls.GrblGroupBox GB;
		private MyDatagridView DGV;
		private LaserGRBL.UserControls.GrblButton BtnWrite;
		private LaserGRBL.UserControls.GrblButton BtnRead;
		private LaserGRBL.UserControls.GrblButton BtnImport;
		private LaserGRBL.UserControls.GrblButton BtnExport;
		private System.Windows.Forms.Label LblConnect;
		private System.Windows.Forms.Timer ActionTimer;
		private System.Windows.Forms.Label LblAction;
		//private System.Windows.Forms.DataGridViewTextBoxColumn numberDataGridViewTextBoxColumn;
		//private System.Windows.Forms.DataGridViewTextBoxColumn parameterDataGridViewTextBoxColumn;
		//private System.Windows.Forms.DataGridViewTextBoxColumn valueDataGridViewTextBoxColumn;
		//private System.Windows.Forms.DataGridViewTextBoxColumn unitDataGridViewTextBoxColumn;
		//private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
		private System.Windows.Forms.DataGridViewTextBoxColumn DollarNumber;
		private System.Windows.Forms.DataGridViewTextBoxColumn Parameter;
		private System.Windows.Forms.DataGridViewTextBoxColumn Value;
		private System.Windows.Forms.DataGridViewTextBoxColumn Unit;
		private System.Windows.Forms.DataGridViewTextBoxColumn Description;
		private System.Windows.Forms.DataGridViewTextBoxColumn Number;
    }
}