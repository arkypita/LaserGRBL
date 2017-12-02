namespace LaserGRBL
{
	partial class JogForm
	{
		/// <summary> 
		/// Variabile di progettazione necessaria.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Liberare le risorse in uso.
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JogForm));
			this.tlp = new System.Windows.Forms.TableLayoutPanel();
			this.BtnHome = new LaserGRBL.DirectionButton();
			this.TbStep = new System.Windows.Forms.TrackBar();
			this.TbSpeed = new System.Windows.Forms.TrackBar();
			this.imageButton1 = new LaserGRBL.DirectionButton();
			this.imageButton2 = new LaserGRBL.DirectionButton();
			this.imageButton4 = new LaserGRBL.DirectionButton();
			this.imageButton6 = new LaserGRBL.DirectionButton();
			this.imageButton5 = new LaserGRBL.DirectionButton();
			this.imageButton8 = new LaserGRBL.DirectionButton();
			this.imageButton7 = new LaserGRBL.DirectionButton();
			this.imageButton3 = new LaserGRBL.DirectionButton();
			this.TT = new System.Windows.Forms.ToolTip(this.components);
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.LblSpeed = new System.Windows.Forms.Label();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.LblStep = new System.Windows.Forms.Label();
			this.tlp.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.TbStep)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.TbSpeed)).BeginInit();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// tlp
			// 
			this.tlp.ColumnCount = 7;
			this.tlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tlp.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlp.Controls.Add(this.BtnHome, 3, 2);
			this.tlp.Controls.Add(this.imageButton1, 2, 2);
			this.tlp.Controls.Add(this.imageButton2, 3, 1);
			this.tlp.Controls.Add(this.imageButton4, 4, 2);
			this.tlp.Controls.Add(this.imageButton6, 2, 1);
			this.tlp.Controls.Add(this.imageButton5, 4, 1);
			this.tlp.Controls.Add(this.imageButton8, 3, 3);
			this.tlp.Controls.Add(this.imageButton7, 2, 3);
			this.tlp.Controls.Add(this.imageButton3, 4, 3);
			this.tlp.Controls.Add(this.tableLayoutPanel1, 1, 1);
			this.tlp.Controls.Add(this.tableLayoutPanel2, 5, 1);
			this.tlp.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlp.Location = new System.Drawing.Point(0, 0);
			this.tlp.Name = "tlp";
			this.tlp.RowCount = 5;
			this.tlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlp.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlp.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlp.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tlp.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tlp.Size = new System.Drawing.Size(234, 125);
			this.tlp.TabIndex = 0;
			// 
			// BtnHome
			// 
			this.BtnHome.AltImage = null;
			this.BtnHome.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.BtnHome.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.BtnHome.Coloration = System.Drawing.Color.Empty;
			this.BtnHome.Image = ((System.Drawing.Image)(resources.GetObject("BtnHome.Image")));
			this.BtnHome.JogDirection = LaserGRBL.GrblCore.JogDirection.N;
			this.BtnHome.Location = new System.Drawing.Point(100, 45);
			this.BtnHome.MaximumSize = new System.Drawing.Size(48, 48);
			this.BtnHome.Name = "BtnHome";
			this.BtnHome.Size = new System.Drawing.Size(33, 33);
			this.BtnHome.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.StretchImage;
			this.BtnHome.TabIndex = 7;
			this.BtnHome.UseAltImage = false;
			this.BtnHome.Click += new System.EventHandler(this.BtnHome_Click);
			// 
			// TbStep
			// 
			this.TbStep.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TbStep.LargeChange = 10;
			this.TbStep.Location = new System.Drawing.Point(3, 3);
			this.TbStep.Maximum = 200;
			this.TbStep.Minimum = 1;
			this.TbStep.Name = "TbStep";
			this.TbStep.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.TbStep.Size = new System.Drawing.Size(45, 100);
			this.TbStep.TabIndex = 17;
			this.TbStep.TickFrequency = 10;
			this.TbStep.TickStyle = System.Windows.Forms.TickStyle.Both;
			this.TbStep.Value = 10;
			this.TbStep.ValueChanged += new System.EventHandler(this.TbStep_ValueChanged);
			this.TbStep.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnSliderMouseUP);
			// 
			// TbSpeed
			// 
			this.TbSpeed.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TbSpeed.LargeChange = 100;
			this.TbSpeed.Location = new System.Drawing.Point(3, 3);
			this.TbSpeed.Maximum = 4000;
			this.TbSpeed.Minimum = 10;
			this.TbSpeed.Name = "TbSpeed";
			this.TbSpeed.Orientation = System.Windows.Forms.Orientation.Vertical;
			this.TbSpeed.Size = new System.Drawing.Size(45, 100);
			this.TbSpeed.SmallChange = 50;
			this.TbSpeed.TabIndex = 16;
			this.TbSpeed.TickFrequency = 100;
			this.TbSpeed.TickStyle = System.Windows.Forms.TickStyle.Both;
			this.TbSpeed.Value = 1000;
			this.TbSpeed.ValueChanged += new System.EventHandler(this.TbSpeed_ValueChanged);
			this.TbSpeed.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnSliderMouseUP);
			// 
			// imageButton1
			// 
			this.imageButton1.AltImage = null;
			this.imageButton1.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.imageButton1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.imageButton1.Coloration = System.Drawing.Color.Empty;
			this.imageButton1.Image = ((System.Drawing.Image)(resources.GetObject("imageButton1.Image")));
			this.imageButton1.JogDirection = LaserGRBL.GrblCore.JogDirection.W;
			this.imageButton1.Location = new System.Drawing.Point(59, 45);
			this.imageButton1.MaximumSize = new System.Drawing.Size(48, 48);
			this.imageButton1.Name = "imageButton1";
			this.imageButton1.Size = new System.Drawing.Size(33, 33);
			this.imageButton1.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.StretchImage;
			this.imageButton1.TabIndex = 8;
			this.imageButton1.UseAltImage = false;
			this.imageButton1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnJogButtonMouseDown);
			// 
			// imageButton2
			// 
			this.imageButton2.AltImage = null;
			this.imageButton2.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.imageButton2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.imageButton2.Coloration = System.Drawing.Color.Empty;
			this.imageButton2.Image = ((System.Drawing.Image)(resources.GetObject("imageButton2.Image")));
			this.imageButton2.JogDirection = LaserGRBL.GrblCore.JogDirection.N;
			this.imageButton2.Location = new System.Drawing.Point(100, 6);
			this.imageButton2.MaximumSize = new System.Drawing.Size(48, 48);
			this.imageButton2.Name = "imageButton2";
			this.imageButton2.Size = new System.Drawing.Size(33, 33);
			this.imageButton2.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.StretchImage;
			this.imageButton2.TabIndex = 9;
			this.imageButton2.UseAltImage = false;
			this.imageButton2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnJogButtonMouseDown);
			// 
			// imageButton4
			// 
			this.imageButton4.AltImage = null;
			this.imageButton4.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.imageButton4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.imageButton4.Coloration = System.Drawing.Color.Empty;
			this.imageButton4.Image = ((System.Drawing.Image)(resources.GetObject("imageButton4.Image")));
			this.imageButton4.JogDirection = LaserGRBL.GrblCore.JogDirection.E;
			this.imageButton4.Location = new System.Drawing.Point(141, 45);
			this.imageButton4.MaximumSize = new System.Drawing.Size(48, 48);
			this.imageButton4.Name = "imageButton4";
			this.imageButton4.Size = new System.Drawing.Size(33, 33);
			this.imageButton4.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.StretchImage;
			this.imageButton4.TabIndex = 11;
			this.imageButton4.UseAltImage = false;
			this.imageButton4.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnJogButtonMouseDown);
			// 
			// imageButton6
			// 
			this.imageButton6.AltImage = null;
			this.imageButton6.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.imageButton6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.imageButton6.Coloration = System.Drawing.Color.Empty;
			this.imageButton6.Image = ((System.Drawing.Image)(resources.GetObject("imageButton6.Image")));
			this.imageButton6.JogDirection = LaserGRBL.GrblCore.JogDirection.NW;
			this.imageButton6.Location = new System.Drawing.Point(59, 6);
			this.imageButton6.MaximumSize = new System.Drawing.Size(48, 48);
			this.imageButton6.Name = "imageButton6";
			this.imageButton6.Size = new System.Drawing.Size(33, 33);
			this.imageButton6.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.StretchImage;
			this.imageButton6.TabIndex = 13;
			this.imageButton6.UseAltImage = false;
			this.imageButton6.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnJogButtonMouseDown);
			// 
			// imageButton5
			// 
			this.imageButton5.AltImage = null;
			this.imageButton5.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.imageButton5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.imageButton5.Coloration = System.Drawing.Color.Empty;
			this.imageButton5.Image = ((System.Drawing.Image)(resources.GetObject("imageButton5.Image")));
			this.imageButton5.JogDirection = LaserGRBL.GrblCore.JogDirection.NE;
			this.imageButton5.Location = new System.Drawing.Point(141, 6);
			this.imageButton5.MaximumSize = new System.Drawing.Size(48, 48);
			this.imageButton5.Name = "imageButton5";
			this.imageButton5.Size = new System.Drawing.Size(33, 33);
			this.imageButton5.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.StretchImage;
			this.imageButton5.TabIndex = 12;
			this.imageButton5.UseAltImage = false;
			this.imageButton5.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnJogButtonMouseDown);
			// 
			// imageButton8
			// 
			this.imageButton8.AltImage = null;
			this.imageButton8.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.imageButton8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.imageButton8.Coloration = System.Drawing.Color.Empty;
			this.imageButton8.Image = ((System.Drawing.Image)(resources.GetObject("imageButton8.Image")));
			this.imageButton8.JogDirection = LaserGRBL.GrblCore.JogDirection.S;
			this.imageButton8.Location = new System.Drawing.Point(99, 84);
			this.imageButton8.MaximumSize = new System.Drawing.Size(48, 48);
			this.imageButton8.Name = "imageButton8";
			this.imageButton8.Size = new System.Drawing.Size(35, 35);
			this.imageButton8.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.StretchImage;
			this.imageButton8.TabIndex = 15;
			this.imageButton8.UseAltImage = false;
			this.imageButton8.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnJogButtonMouseDown);
			// 
			// imageButton7
			// 
			this.imageButton7.AltImage = null;
			this.imageButton7.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.imageButton7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.imageButton7.Coloration = System.Drawing.Color.Empty;
			this.imageButton7.Image = ((System.Drawing.Image)(resources.GetObject("imageButton7.Image")));
			this.imageButton7.JogDirection = LaserGRBL.GrblCore.JogDirection.SW;
			this.imageButton7.Location = new System.Drawing.Point(58, 84);
			this.imageButton7.MaximumSize = new System.Drawing.Size(48, 48);
			this.imageButton7.Name = "imageButton7";
			this.imageButton7.Size = new System.Drawing.Size(35, 35);
			this.imageButton7.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.StretchImage;
			this.imageButton7.TabIndex = 14;
			this.imageButton7.UseAltImage = false;
			this.imageButton7.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnJogButtonMouseDown);
			// 
			// imageButton3
			// 
			this.imageButton3.AltImage = null;
			this.imageButton3.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.imageButton3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.imageButton3.Coloration = System.Drawing.Color.Empty;
			this.imageButton3.Image = ((System.Drawing.Image)(resources.GetObject("imageButton3.Image")));
			this.imageButton3.JogDirection = LaserGRBL.GrblCore.JogDirection.SE;
			this.imageButton3.Location = new System.Drawing.Point(140, 84);
			this.imageButton3.MaximumSize = new System.Drawing.Size(48, 48);
			this.imageButton3.Name = "imageButton3";
			this.imageButton3.Size = new System.Drawing.Size(35, 35);
			this.imageButton3.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.StretchImage;
			this.imageButton3.TabIndex = 10;
			this.imageButton3.UseAltImage = false;
			this.imageButton3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnJogButtonMouseDown);
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.AutoSize = true;
			this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel1.Controls.Add(this.TbSpeed, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.LblSpeed, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 3);
			this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tlp.SetRowSpan(this.tableLayoutPanel1, 3);
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel1.Size = new System.Drawing.Size(51, 119);
			this.tableLayoutPanel1.TabIndex = 18;
			// 
			// LblSpeed
			// 
			this.LblSpeed.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.LblSpeed.AutoSize = true;
			this.LblSpeed.Location = new System.Drawing.Point(7, 106);
			this.LblSpeed.Name = "LblSpeed";
			this.LblSpeed.Size = new System.Drawing.Size(37, 13);
			this.LblSpeed.TabIndex = 17;
			this.LblSpeed.Text = "F1000";
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.AutoSize = true;
			this.tableLayoutPanel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.tableLayoutPanel2.ColumnCount = 1;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
			this.tableLayoutPanel2.Controls.Add(this.TbStep, 0, 0);
			this.tableLayoutPanel2.Controls.Add(this.LblStep, 0, 1);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(178, 3);
			this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 2;
			this.tlp.SetRowSpan(this.tableLayoutPanel2, 3);
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
			this.tableLayoutPanel2.Size = new System.Drawing.Size(51, 119);
			this.tableLayoutPanel2.TabIndex = 19;
			// 
			// LblStep
			// 
			this.LblStep.Anchor = System.Windows.Forms.AnchorStyles.None;
			this.LblStep.AutoSize = true;
			this.LblStep.Location = new System.Drawing.Point(16, 106);
			this.LblStep.Name = "LblStep";
			this.LblStep.Size = new System.Drawing.Size(19, 13);
			this.LblStep.TabIndex = 18;
			this.LblStep.Text = "10";
			// 
			// JogForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tlp);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "JogForm";
			this.Size = new System.Drawing.Size(234, 125);
			this.tlp.ResumeLayout(false);
			this.tlp.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.TbStep)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.TbSpeed)).EndInit();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tlp;
		private DirectionButton imageButton8;
		private DirectionButton imageButton7;
		private DirectionButton BtnHome;
		private DirectionButton imageButton1;
		private DirectionButton imageButton2;
		private DirectionButton imageButton3;
		private DirectionButton imageButton4;
		private DirectionButton imageButton6;
		private DirectionButton imageButton5;
		private System.Windows.Forms.TrackBar TbSpeed;
		private System.Windows.Forms.TrackBar TbStep;
		private System.Windows.Forms.ToolTip TT;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Label LblSpeed;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Label LblStep;
	}
}
