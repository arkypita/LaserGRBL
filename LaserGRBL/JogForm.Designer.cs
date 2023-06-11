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
            this.BtnFrameInf = new LaserGRBL.GCodeButton();
            this.BtnC = new LaserGRBL.GCodeButton();
            this.BtnFrame = new LaserGRBL.GCodeButton();
            this.BtnBR = new LaserGRBL.GCodeButton();
            this.BtnB = new LaserGRBL.GCodeButton();
            this.BtnR = new LaserGRBL.GCodeButton();
            this.BtnTR = new LaserGRBL.GCodeButton();
            this.BtnT = new LaserGRBL.GCodeButton();
            this.BtnBL = new LaserGRBL.GCodeButton();
            this.BtnL = new LaserGRBL.GCodeButton();
            this.BtnHome = new LaserGRBL.DirectionButton();
            this.BtnW = new LaserGRBL.DirectionButton();
            this.BtnN = new LaserGRBL.DirectionButton();
            this.BtnE = new LaserGRBL.DirectionButton();
            this.BtnNW = new LaserGRBL.DirectionButton();
            this.BtnNE = new LaserGRBL.DirectionButton();
            this.BtnS = new LaserGRBL.DirectionButton();
            this.BtnSW = new LaserGRBL.DirectionButton();
            this.BtnSE = new LaserGRBL.DirectionButton();
            this.TlpSpeedControl = new System.Windows.Forms.TableLayoutPanel();
            this.TbSpeed = new System.Windows.Forms.TrackBar();
            this.LblSpeed = new System.Windows.Forms.Label();
            this.TlpStepControl = new System.Windows.Forms.TableLayoutPanel();
            this.TbStep = new LaserGRBL.StepBar();
            this.LblStep = new System.Windows.Forms.Label();
            this.TlpZControl = new System.Windows.Forms.TableLayoutPanel();
            this.BtnZup01 = new LaserGRBL.DirectionStepButton();
            this.BtnZup1 = new LaserGRBL.DirectionStepButton();
            this.BtnZup10 = new LaserGRBL.DirectionStepButton();
            this.BtnZdown10 = new LaserGRBL.DirectionStepButton();
            this.BtnZdown1 = new LaserGRBL.DirectionStepButton();
            this.BtnZdown01 = new LaserGRBL.DirectionStepButton();
            this.label1 = new System.Windows.Forms.Label();
            this.BtnTL = new LaserGRBL.GCodeButton();
            this.BtnShape = new LaserGRBL.GCodeButton();
            this.BtnFocus = new LaserGRBL.TwoStatesGCodeButton();
            this.BtnClickNJog = new LaserGRBL.TwoStatesGCodeButton();
            this.TT = new System.Windows.Forms.ToolTip(this.components);
            this.UpdateFMax = new System.Windows.Forms.Timer(this.components);
            this.tlp.SuspendLayout();
            this.TlpSpeedControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TbSpeed)).BeginInit();
            this.TlpStepControl.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TbStep)).BeginInit();
            this.TlpZControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlp
            // 
            resources.ApplyResources(this.tlp, "tlp");
            this.tlp.Controls.Add(this.BtnFrameInf, 6, 6);
            this.tlp.Controls.Add(this.BtnC, 3, 6);
            this.tlp.Controls.Add(this.BtnFrame, 6, 5);
            this.tlp.Controls.Add(this.BtnBR, 4, 7);
            this.tlp.Controls.Add(this.BtnB, 3, 7);
            this.tlp.Controls.Add(this.BtnR, 4, 6);
            this.tlp.Controls.Add(this.BtnTR, 4, 5);
            this.tlp.Controls.Add(this.BtnT, 3, 5);
            this.tlp.Controls.Add(this.BtnBL, 2, 7);
            this.tlp.Controls.Add(this.BtnL, 2, 6);
            this.tlp.Controls.Add(this.BtnHome, 3, 2);
            this.tlp.Controls.Add(this.BtnW, 2, 2);
            this.tlp.Controls.Add(this.BtnN, 3, 1);
            this.tlp.Controls.Add(this.BtnE, 4, 2);
            this.tlp.Controls.Add(this.BtnNW, 2, 1);
            this.tlp.Controls.Add(this.BtnNE, 4, 1);
            this.tlp.Controls.Add(this.BtnS, 3, 3);
            this.tlp.Controls.Add(this.BtnSW, 2, 3);
            this.tlp.Controls.Add(this.BtnSE, 4, 3);
            this.tlp.Controls.Add(this.TlpSpeedControl, 1, 1);
            this.tlp.Controls.Add(this.TlpStepControl, 6, 1);
            this.tlp.Controls.Add(this.TlpZControl, 5, 1);
            this.tlp.Controls.Add(this.BtnTL, 2, 5);
            this.tlp.Controls.Add(this.BtnShape, 6, 7);
            this.tlp.Controls.Add(this.BtnFocus, 1, 7);
            this.tlp.Controls.Add(this.BtnClickNJog, 1, 5);
            this.tlp.Name = "tlp";
            this.TT.SetToolTip(this.tlp, resources.GetString("tlp.ToolTip"));
            // 
            // BtnFrameInf
            // 
            resources.ApplyResources(this.BtnFrameInf, "BtnFrameInf");
            this.BtnFrameInf.AltImage = null;
            this.BtnFrameInf.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnFrameInf.Caption = null;
            this.BtnFrameInf.Coloration = System.Drawing.Color.Empty;
            this.BtnFrameInf.GCode = "G0 X[left] Y[bottom]\r\nG{0} Y[top] {1}\r\nG{0} X[right]\r\nG{0} Y[bottom] \r\nG{0} X[lef" +
    "t]";
            this.BtnFrameInf.Image = ((System.Drawing.Image)(resources.GetObject("BtnFrameInf.Image")));
            this.BtnFrameInf.Name = "BtnFrameInf";
            this.BtnFrameInf.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.StretchImage;
            this.BtnFrameInf.TabStop = false;
            this.TT.SetToolTip(this.BtnFrameInf, resources.GetString("BtnFrameInf.ToolTip"));
            this.BtnFrameInf.UseAltImage = false;
            this.BtnFrameInf.Click += new System.EventHandler(this.BtnFrameInf_Click);
            // 
            // BtnC
            // 
            resources.ApplyResources(this.BtnC, "BtnC");
            this.BtnC.AltImage = null;
            this.BtnC.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnC.Caption = null;
            this.BtnC.Coloration = System.Drawing.Color.Empty;
            this.BtnC.GCode = "G{0} X[left+width/2] Y[bottom+height/2] {1}";
            this.BtnC.Image = ((System.Drawing.Image)(resources.GetObject("BtnC.Image")));
            this.BtnC.Name = "BtnC";
            this.BtnC.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.StretchImage;
            this.BtnC.TabStop = false;
            this.TT.SetToolTip(this.BtnC, resources.GetString("BtnC.ToolTip"));
            this.BtnC.UseAltImage = false;
            this.BtnC.Click += new System.EventHandler(this.BtnPosition_Click);
            // 
            // BtnFrame
            // 
            resources.ApplyResources(this.BtnFrame, "BtnFrame");
            this.BtnFrame.AltImage = null;
            this.BtnFrame.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnFrame.Caption = null;
            this.BtnFrame.Coloration = System.Drawing.Color.Empty;
            this.BtnFrame.GCode = "G0X[left]Y[bottom]\r\nG{0}Y[top]{1}\r\nG{0}X[right]\r\nG{0} Y[bottom] \r\nG{0}X[left]";
            this.BtnFrame.Image = ((System.Drawing.Image)(resources.GetObject("BtnFrame.Image")));
            this.BtnFrame.Name = "BtnFrame";
            this.BtnFrame.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.StretchImage;
            this.BtnFrame.TabStop = false;
            this.TT.SetToolTip(this.BtnFrame, resources.GetString("BtnFrame.ToolTip"));
            this.BtnFrame.UseAltImage = false;
            this.BtnFrame.Click += new System.EventHandler(this.BtnPosition_Click);
            // 
            // BtnBR
            // 
            resources.ApplyResources(this.BtnBR, "BtnBR");
            this.BtnBR.AltImage = null;
            this.BtnBR.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnBR.Caption = null;
            this.BtnBR.Coloration = System.Drawing.Color.Empty;
            this.BtnBR.GCode = "G{0} X[right] Y[bottom] {1}";
            this.BtnBR.Image = ((System.Drawing.Image)(resources.GetObject("BtnBR.Image")));
            this.BtnBR.Name = "BtnBR";
            this.BtnBR.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.StretchImage;
            this.BtnBR.TabStop = false;
            this.TT.SetToolTip(this.BtnBR, resources.GetString("BtnBR.ToolTip"));
            this.BtnBR.UseAltImage = false;
            this.BtnBR.Click += new System.EventHandler(this.BtnPosition_Click);
            // 
            // BtnB
            // 
            resources.ApplyResources(this.BtnB, "BtnB");
            this.BtnB.AltImage = null;
            this.BtnB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnB.Caption = null;
            this.BtnB.Coloration = System.Drawing.Color.Empty;
            this.BtnB.GCode = "G{0} Y[bottom] {1}";
            this.BtnB.Image = ((System.Drawing.Image)(resources.GetObject("BtnB.Image")));
            this.BtnB.Name = "BtnB";
            this.BtnB.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.StretchImage;
            this.BtnB.TabStop = false;
            this.TT.SetToolTip(this.BtnB, resources.GetString("BtnB.ToolTip"));
            this.BtnB.UseAltImage = false;
            this.BtnB.Click += new System.EventHandler(this.BtnPosition_Click);
            // 
            // BtnR
            // 
            resources.ApplyResources(this.BtnR, "BtnR");
            this.BtnR.AltImage = null;
            this.BtnR.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnR.Caption = null;
            this.BtnR.Coloration = System.Drawing.Color.Empty;
            this.BtnR.GCode = "G{0} X[right] {1}";
            this.BtnR.Image = ((System.Drawing.Image)(resources.GetObject("BtnR.Image")));
            this.BtnR.Name = "BtnR";
            this.BtnR.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.StretchImage;
            this.BtnR.TabStop = false;
            this.TT.SetToolTip(this.BtnR, resources.GetString("BtnR.ToolTip"));
            this.BtnR.UseAltImage = false;
            this.BtnR.Click += new System.EventHandler(this.BtnPosition_Click);
            // 
            // BtnTR
            // 
            resources.ApplyResources(this.BtnTR, "BtnTR");
            this.BtnTR.AltImage = null;
            this.BtnTR.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnTR.Caption = null;
            this.BtnTR.Coloration = System.Drawing.Color.Empty;
            this.BtnTR.GCode = "G{0} X[right] Y[top] {1}";
            this.BtnTR.Image = ((System.Drawing.Image)(resources.GetObject("BtnTR.Image")));
            this.BtnTR.Name = "BtnTR";
            this.BtnTR.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.StretchImage;
            this.BtnTR.TabStop = false;
            this.TT.SetToolTip(this.BtnTR, resources.GetString("BtnTR.ToolTip"));
            this.BtnTR.UseAltImage = false;
            this.BtnTR.Click += new System.EventHandler(this.BtnPosition_Click);
            // 
            // BtnT
            // 
            resources.ApplyResources(this.BtnT, "BtnT");
            this.BtnT.AltImage = null;
            this.BtnT.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnT.Caption = null;
            this.BtnT.Coloration = System.Drawing.Color.Empty;
            this.BtnT.GCode = "G{0} Y[top] {1}";
            this.BtnT.Image = ((System.Drawing.Image)(resources.GetObject("BtnT.Image")));
            this.BtnT.Name = "BtnT";
            this.BtnT.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.StretchImage;
            this.BtnT.TabStop = false;
            this.TT.SetToolTip(this.BtnT, resources.GetString("BtnT.ToolTip"));
            this.BtnT.UseAltImage = false;
            this.BtnT.Click += new System.EventHandler(this.BtnPosition_Click);
            // 
            // BtnBL
            // 
            resources.ApplyResources(this.BtnBL, "BtnBL");
            this.BtnBL.AltImage = null;
            this.BtnBL.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnBL.Caption = null;
            this.BtnBL.Coloration = System.Drawing.Color.Empty;
            this.BtnBL.GCode = "G{0} X[left] Y[bottom] {1}";
            this.BtnBL.Image = ((System.Drawing.Image)(resources.GetObject("BtnBL.Image")));
            this.BtnBL.Name = "BtnBL";
            this.BtnBL.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.StretchImage;
            this.BtnBL.TabStop = false;
            this.TT.SetToolTip(this.BtnBL, resources.GetString("BtnBL.ToolTip"));
            this.BtnBL.UseAltImage = false;
            this.BtnBL.Click += new System.EventHandler(this.BtnPosition_Click);
            // 
            // BtnL
            // 
            resources.ApplyResources(this.BtnL, "BtnL");
            this.BtnL.AltImage = null;
            this.BtnL.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnL.Caption = null;
            this.BtnL.Coloration = System.Drawing.Color.Empty;
            this.BtnL.GCode = "G{0} X[left] {1}";
            this.BtnL.Image = ((System.Drawing.Image)(resources.GetObject("BtnL.Image")));
            this.BtnL.Name = "BtnL";
            this.BtnL.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.StretchImage;
            this.BtnL.TabStop = false;
            this.TT.SetToolTip(this.BtnL, resources.GetString("BtnL.ToolTip"));
            this.BtnL.UseAltImage = false;
            this.BtnL.Click += new System.EventHandler(this.BtnPosition_Click);
            // 
            // BtnHome
            // 
            resources.ApplyResources(this.BtnHome, "BtnHome");
            this.BtnHome.AltImage = null;
            this.BtnHome.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnHome.Caption = null;
            this.BtnHome.Coloration = System.Drawing.Color.Empty;
            this.BtnHome.Image = ((System.Drawing.Image)(resources.GetObject("BtnHome.Image")));
            this.BtnHome.JogDirection = LaserGRBL.GrblCore.JogDirection.Home;
            this.BtnHome.Name = "BtnHome";
            this.BtnHome.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.StretchImage;
            this.BtnHome.TabStop = false;
            this.TT.SetToolTip(this.BtnHome, resources.GetString("BtnHome.ToolTip"));
            this.BtnHome.UseAltImage = false;
            this.BtnHome.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnJogButtonMouseDown);
            this.BtnHome.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnJogButtonMouseUp);
            // 
            // BtnW
            // 
            resources.ApplyResources(this.BtnW, "BtnW");
            this.BtnW.AltImage = null;
            this.BtnW.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnW.Caption = null;
            this.BtnW.Coloration = System.Drawing.Color.Empty;
            this.BtnW.Image = ((System.Drawing.Image)(resources.GetObject("BtnW.Image")));
            this.BtnW.JogDirection = LaserGRBL.GrblCore.JogDirection.W;
            this.BtnW.Name = "BtnW";
            this.BtnW.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.StretchImage;
            this.BtnW.TabStop = false;
            this.TT.SetToolTip(this.BtnW, resources.GetString("BtnW.ToolTip"));
            this.BtnW.UseAltImage = false;
            this.BtnW.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnJogButtonMouseDown);
            this.BtnW.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnJogButtonMouseUp);
            // 
            // BtnN
            // 
            resources.ApplyResources(this.BtnN, "BtnN");
            this.BtnN.AltImage = null;
            this.BtnN.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnN.Caption = null;
            this.BtnN.Coloration = System.Drawing.Color.Empty;
            this.BtnN.Image = ((System.Drawing.Image)(resources.GetObject("BtnN.Image")));
            this.BtnN.JogDirection = LaserGRBL.GrblCore.JogDirection.N;
            this.BtnN.Name = "BtnN";
            this.BtnN.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.StretchImage;
            this.BtnN.TabStop = false;
            this.TT.SetToolTip(this.BtnN, resources.GetString("BtnN.ToolTip"));
            this.BtnN.UseAltImage = false;
            this.BtnN.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnJogButtonMouseDown);
            this.BtnN.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnJogButtonMouseUp);
            // 
            // BtnE
            // 
            resources.ApplyResources(this.BtnE, "BtnE");
            this.BtnE.AltImage = null;
            this.BtnE.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnE.Caption = null;
            this.BtnE.Coloration = System.Drawing.Color.Empty;
            this.BtnE.Image = ((System.Drawing.Image)(resources.GetObject("BtnE.Image")));
            this.BtnE.JogDirection = LaserGRBL.GrblCore.JogDirection.E;
            this.BtnE.Name = "BtnE";
            this.BtnE.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.StretchImage;
            this.BtnE.TabStop = false;
            this.TT.SetToolTip(this.BtnE, resources.GetString("BtnE.ToolTip"));
            this.BtnE.UseAltImage = false;
            this.BtnE.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnJogButtonMouseDown);
            this.BtnE.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnJogButtonMouseUp);
            // 
            // BtnNW
            // 
            resources.ApplyResources(this.BtnNW, "BtnNW");
            this.BtnNW.AltImage = null;
            this.BtnNW.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnNW.Caption = null;
            this.BtnNW.Coloration = System.Drawing.Color.Empty;
            this.BtnNW.Image = ((System.Drawing.Image)(resources.GetObject("BtnNW.Image")));
            this.BtnNW.JogDirection = LaserGRBL.GrblCore.JogDirection.NW;
            this.BtnNW.Name = "BtnNW";
            this.BtnNW.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.StretchImage;
            this.BtnNW.TabStop = false;
            this.TT.SetToolTip(this.BtnNW, resources.GetString("BtnNW.ToolTip"));
            this.BtnNW.UseAltImage = false;
            this.BtnNW.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnJogButtonMouseDown);
            this.BtnNW.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnJogButtonMouseUp);
            // 
            // BtnNE
            // 
            resources.ApplyResources(this.BtnNE, "BtnNE");
            this.BtnNE.AltImage = null;
            this.BtnNE.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnNE.Caption = null;
            this.BtnNE.Coloration = System.Drawing.Color.Empty;
            this.BtnNE.Image = ((System.Drawing.Image)(resources.GetObject("BtnNE.Image")));
            this.BtnNE.JogDirection = LaserGRBL.GrblCore.JogDirection.NE;
            this.BtnNE.Name = "BtnNE";
            this.BtnNE.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.StretchImage;
            this.BtnNE.TabStop = false;
            this.TT.SetToolTip(this.BtnNE, resources.GetString("BtnNE.ToolTip"));
            this.BtnNE.UseAltImage = false;
            this.BtnNE.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnJogButtonMouseDown);
            this.BtnNE.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnJogButtonMouseUp);
            // 
            // BtnS
            // 
            resources.ApplyResources(this.BtnS, "BtnS");
            this.BtnS.AltImage = null;
            this.BtnS.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnS.Caption = null;
            this.BtnS.Coloration = System.Drawing.Color.Empty;
            this.BtnS.Image = ((System.Drawing.Image)(resources.GetObject("BtnS.Image")));
            this.BtnS.JogDirection = LaserGRBL.GrblCore.JogDirection.S;
            this.BtnS.Name = "BtnS";
            this.BtnS.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.StretchImage;
            this.BtnS.TabStop = false;
            this.TT.SetToolTip(this.BtnS, resources.GetString("BtnS.ToolTip"));
            this.BtnS.UseAltImage = false;
            this.BtnS.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnJogButtonMouseDown);
            this.BtnS.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnJogButtonMouseUp);
            // 
            // BtnSW
            // 
            resources.ApplyResources(this.BtnSW, "BtnSW");
            this.BtnSW.AltImage = null;
            this.BtnSW.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnSW.Caption = null;
            this.BtnSW.Coloration = System.Drawing.Color.Empty;
            this.BtnSW.Image = ((System.Drawing.Image)(resources.GetObject("BtnSW.Image")));
            this.BtnSW.JogDirection = LaserGRBL.GrblCore.JogDirection.SW;
            this.BtnSW.Name = "BtnSW";
            this.BtnSW.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.StretchImage;
            this.BtnSW.TabStop = false;
            this.TT.SetToolTip(this.BtnSW, resources.GetString("BtnSW.ToolTip"));
            this.BtnSW.UseAltImage = false;
            this.BtnSW.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnJogButtonMouseDown);
            this.BtnSW.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnJogButtonMouseUp);
            // 
            // BtnSE
            // 
            resources.ApplyResources(this.BtnSE, "BtnSE");
            this.BtnSE.AltImage = null;
            this.BtnSE.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnSE.Caption = null;
            this.BtnSE.Coloration = System.Drawing.Color.Empty;
            this.BtnSE.Image = ((System.Drawing.Image)(resources.GetObject("BtnSE.Image")));
            this.BtnSE.JogDirection = LaserGRBL.GrblCore.JogDirection.SE;
            this.BtnSE.Name = "BtnSE";
            this.BtnSE.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.StretchImage;
            this.BtnSE.TabStop = false;
            this.TT.SetToolTip(this.BtnSE, resources.GetString("BtnSE.ToolTip"));
            this.BtnSE.UseAltImage = false;
            this.BtnSE.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnJogButtonMouseDown);
            this.BtnSE.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnJogButtonMouseUp);
            // 
            // TlpSpeedControl
            // 
            resources.ApplyResources(this.TlpSpeedControl, "TlpSpeedControl");
            this.TlpSpeedControl.Controls.Add(this.TbSpeed, 0, 0);
            this.TlpSpeedControl.Controls.Add(this.LblSpeed, 0, 1);
            this.TlpSpeedControl.Name = "TlpSpeedControl";
            this.tlp.SetRowSpan(this.TlpSpeedControl, 3);
            this.TT.SetToolTip(this.TlpSpeedControl, resources.GetString("TlpSpeedControl.ToolTip"));
            // 
            // TbSpeed
            // 
            resources.ApplyResources(this.TbSpeed, "TbSpeed");
            this.TbSpeed.LargeChange = 100;
            this.TbSpeed.Maximum = 4000;
            this.TbSpeed.Minimum = 10;
            this.TbSpeed.Name = "TbSpeed";
            this.TbSpeed.SmallChange = 50;
            this.TbSpeed.TickFrequency = 200;
            this.TbSpeed.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.TT.SetToolTip(this.TbSpeed, resources.GetString("TbSpeed.ToolTip"));
            this.TbSpeed.Value = 1000;
            this.TbSpeed.ValueChanged += new System.EventHandler(this.TbSpeed_ValueChanged);
            // 
            // LblSpeed
            // 
            resources.ApplyResources(this.LblSpeed, "LblSpeed");
            this.LblSpeed.Name = "LblSpeed";
            this.TT.SetToolTip(this.LblSpeed, resources.GetString("LblSpeed.ToolTip"));
            // 
            // TlpStepControl
            // 
            resources.ApplyResources(this.TlpStepControl, "TlpStepControl");
            this.TlpStepControl.Controls.Add(this.TbStep, 0, 0);
            this.TlpStepControl.Controls.Add(this.LblStep, 0, 1);
            this.TlpStepControl.Name = "TlpStepControl";
            this.tlp.SetRowSpan(this.TlpStepControl, 3);
            this.TT.SetToolTip(this.TlpStepControl, resources.GetString("TlpStepControl.ToolTip"));
            // 
            // TbStep
            // 
            resources.ApplyResources(this.TbStep, "TbStep");
            this.TbStep.LargeChange = 1;
            this.TbStep.Name = "TbStep";
            this.TbStep.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.TT.SetToolTip(this.TbStep, resources.GetString("TbStep.ToolTip"));
            this.TbStep.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.TbStep.ValueChanged += new System.EventHandler(this.TbStep_ValueChanged);
            // 
            // LblStep
            // 
            resources.ApplyResources(this.LblStep, "LblStep");
            this.LblStep.Name = "LblStep";
            this.TT.SetToolTip(this.LblStep, resources.GetString("LblStep.ToolTip"));
            // 
            // TlpZControl
            // 
            resources.ApplyResources(this.TlpZControl, "TlpZControl");
            this.TlpZControl.Controls.Add(this.BtnZup01, 0, 3);
            this.TlpZControl.Controls.Add(this.BtnZup1, 0, 2);
            this.TlpZControl.Controls.Add(this.BtnZup10, 0, 1);
            this.TlpZControl.Controls.Add(this.BtnZdown10, 0, 6);
            this.TlpZControl.Controls.Add(this.BtnZdown1, 0, 5);
            this.TlpZControl.Controls.Add(this.BtnZdown01, 0, 5);
            this.TlpZControl.Controls.Add(this.label1, 0, 4);
            this.TlpZControl.Name = "TlpZControl";
            this.tlp.SetRowSpan(this.TlpZControl, 3);
            this.TT.SetToolTip(this.TlpZControl, resources.GetString("TlpZControl.ToolTip"));
            // 
            // BtnZup01
            // 
            resources.ApplyResources(this.BtnZup01, "BtnZup01");
            this.BtnZup01.AltImage = null;
            this.BtnZup01.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnZup01.Caption = null;
            this.BtnZup01.Coloration = System.Drawing.Color.Empty;
            this.BtnZup01.Image = ((System.Drawing.Image)(resources.GetObject("BtnZup01.Image")));
            this.BtnZup01.JogDirection = LaserGRBL.GrblCore.JogDirection.Zup;
            this.BtnZup01.JogStep = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.BtnZup01.Name = "BtnZup01";
            this.BtnZup01.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
            this.BtnZup01.TabStop = false;
            this.TT.SetToolTip(this.BtnZup01, resources.GetString("BtnZup01.ToolTip"));
            this.BtnZup01.UseAltImage = false;
            this.BtnZup01.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnZJogButtonMouseDown);
            // 
            // BtnZup1
            // 
            resources.ApplyResources(this.BtnZup1, "BtnZup1");
            this.BtnZup1.AltImage = null;
            this.BtnZup1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnZup1.Caption = null;
            this.BtnZup1.Coloration = System.Drawing.Color.Empty;
            this.BtnZup1.Image = ((System.Drawing.Image)(resources.GetObject("BtnZup1.Image")));
            this.BtnZup1.JogDirection = LaserGRBL.GrblCore.JogDirection.Zup;
            this.BtnZup1.JogStep = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.BtnZup1.Name = "BtnZup1";
            this.BtnZup1.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
            this.BtnZup1.TabStop = false;
            this.TT.SetToolTip(this.BtnZup1, resources.GetString("BtnZup1.ToolTip"));
            this.BtnZup1.UseAltImage = false;
            this.BtnZup1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnZJogButtonMouseDown);
            // 
            // BtnZup10
            // 
            resources.ApplyResources(this.BtnZup10, "BtnZup10");
            this.BtnZup10.AltImage = null;
            this.BtnZup10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnZup10.Caption = null;
            this.BtnZup10.Coloration = System.Drawing.Color.Empty;
            this.BtnZup10.Image = ((System.Drawing.Image)(resources.GetObject("BtnZup10.Image")));
            this.BtnZup10.JogDirection = LaserGRBL.GrblCore.JogDirection.Zup;
            this.BtnZup10.JogStep = new decimal(new int[] {
            100,
            0,
            0,
            65536});
            this.BtnZup10.Name = "BtnZup10";
            this.BtnZup10.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
            this.BtnZup10.TabStop = false;
            this.TT.SetToolTip(this.BtnZup10, resources.GetString("BtnZup10.ToolTip"));
            this.BtnZup10.UseAltImage = false;
            this.BtnZup10.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnZJogButtonMouseDown);
            // 
            // BtnZdown10
            // 
            resources.ApplyResources(this.BtnZdown10, "BtnZdown10");
            this.BtnZdown10.AltImage = null;
            this.BtnZdown10.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnZdown10.Caption = null;
            this.BtnZdown10.Coloration = System.Drawing.Color.Empty;
            this.BtnZdown10.Image = ((System.Drawing.Image)(resources.GetObject("BtnZdown10.Image")));
            this.BtnZdown10.JogDirection = LaserGRBL.GrblCore.JogDirection.Zdown;
            this.BtnZdown10.JogStep = new decimal(new int[] {
            100,
            0,
            0,
            65536});
            this.BtnZdown10.Name = "BtnZdown10";
            this.BtnZdown10.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
            this.BtnZdown10.TabStop = false;
            this.TT.SetToolTip(this.BtnZdown10, resources.GetString("BtnZdown10.ToolTip"));
            this.BtnZdown10.UseAltImage = false;
            this.BtnZdown10.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnZJogButtonMouseDown);
            // 
            // BtnZdown1
            // 
            resources.ApplyResources(this.BtnZdown1, "BtnZdown1");
            this.BtnZdown1.AltImage = null;
            this.BtnZdown1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnZdown1.Caption = null;
            this.BtnZdown1.Coloration = System.Drawing.Color.Empty;
            this.BtnZdown1.Image = ((System.Drawing.Image)(resources.GetObject("BtnZdown1.Image")));
            this.BtnZdown1.JogDirection = LaserGRBL.GrblCore.JogDirection.Zdown;
            this.BtnZdown1.JogStep = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.BtnZdown1.Name = "BtnZdown1";
            this.BtnZdown1.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
            this.BtnZdown1.TabStop = false;
            this.TT.SetToolTip(this.BtnZdown1, resources.GetString("BtnZdown1.ToolTip"));
            this.BtnZdown1.UseAltImage = false;
            this.BtnZdown1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnZJogButtonMouseDown);
            // 
            // BtnZdown01
            // 
            resources.ApplyResources(this.BtnZdown01, "BtnZdown01");
            this.BtnZdown01.AltImage = null;
            this.BtnZdown01.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnZdown01.Caption = null;
            this.BtnZdown01.Coloration = System.Drawing.Color.Empty;
            this.BtnZdown01.Image = ((System.Drawing.Image)(resources.GetObject("BtnZdown01.Image")));
            this.BtnZdown01.JogDirection = LaserGRBL.GrblCore.JogDirection.Zdown;
            this.BtnZdown01.JogStep = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.BtnZdown01.Name = "BtnZdown01";
            this.BtnZdown01.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.FixedSize;
            this.BtnZdown01.TabStop = false;
            this.TT.SetToolTip(this.BtnZdown01, resources.GetString("BtnZdown01.ToolTip"));
            this.BtnZdown01.UseAltImage = false;
            this.BtnZdown01.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnZJogButtonMouseDown);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            this.TT.SetToolTip(this.label1, resources.GetString("label1.ToolTip"));
            // 
            // BtnTL
            // 
            resources.ApplyResources(this.BtnTL, "BtnTL");
            this.BtnTL.AltImage = null;
            this.BtnTL.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnTL.Caption = null;
            this.BtnTL.Coloration = System.Drawing.Color.Empty;
            this.BtnTL.GCode = "G{0} X[left] Y[top] {1}";
            this.BtnTL.Image = ((System.Drawing.Image)(resources.GetObject("BtnTL.Image")));
            this.BtnTL.Name = "BtnTL";
            this.BtnTL.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.StretchImage;
            this.BtnTL.TabStop = false;
            this.TT.SetToolTip(this.BtnTL, resources.GetString("BtnTL.ToolTip"));
            this.BtnTL.UseAltImage = false;
            this.BtnTL.Click += new System.EventHandler(this.BtnPosition_Click);
            // 
            // BtnShape
            // 
            resources.ApplyResources(this.BtnShape, "BtnShape");
            this.BtnShape.AltImage = null;
            this.BtnShape.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnShape.Caption = null;
            this.BtnShape.Coloration = System.Drawing.Color.Empty;
            this.BtnShape.GCode = null;
            this.BtnShape.Image = ((System.Drawing.Image)(resources.GetObject("BtnShape.Image")));
            this.BtnShape.Name = "BtnShape";
            this.BtnShape.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.StretchImage;
            this.BtnShape.TabStop = false;
            this.TT.SetToolTip(this.BtnShape, resources.GetString("BtnShape.ToolTip"));
            this.BtnShape.UseAltImage = false;
            this.BtnShape.Click += new System.EventHandler(this.BtnShape_Click);
            // 
            // BtnFocus
            // 
            resources.ApplyResources(this.BtnFocus, "BtnFocus");
            this.BtnFocus.AltImage = null;
            this.BtnFocus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnFocus.Caption = null;
            this.BtnFocus.Coloration = System.Drawing.Color.Empty;
            this.BtnFocus.GCode = "M3 S[$30 * {0} / 1000]\r\nG1 F1000";
            this.BtnFocus.GCode2 = "M5 S0\r\nG0";
            this.BtnFocus.Image = ((System.Drawing.Image)(resources.GetObject("BtnFocus.Image")));
            this.BtnFocus.Name = "BtnFocus";
            this.BtnFocus.ON = false;
            this.BtnFocus.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.StretchImage;
            this.BtnFocus.TabStop = false;
            this.TT.SetToolTip(this.BtnFocus, resources.GetString("BtnFocus.ToolTip"));
            this.BtnFocus.UseAltImage = false;
            this.BtnFocus.Click += new System.EventHandler(this.BtnFocus_Click);
            // 
            // BtnClickNJog
            // 
            resources.ApplyResources(this.BtnClickNJog, "BtnClickNJog");
            this.BtnClickNJog.AltImage = null;
            this.BtnClickNJog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BtnClickNJog.Caption = null;
            this.BtnClickNJog.Coloration = System.Drawing.Color.Empty;
            this.BtnClickNJog.GCode = "M3 S[$30*3/100]\r\nG1 F1000";
            this.BtnClickNJog.GCode2 = "M5 S0\r\nG0";
            this.BtnClickNJog.Image = ((System.Drawing.Image)(resources.GetObject("BtnClickNJog.Image")));
            this.BtnClickNJog.Name = "BtnClickNJog";
            this.BtnClickNJog.ON = false;
            this.BtnClickNJog.SizingMode = LaserGRBL.UserControls.ImageButton.SizingModes.StretchImage;
            this.BtnClickNJog.TabStop = false;
            this.TT.SetToolTip(this.BtnClickNJog, resources.GetString("BtnClickNJog.ToolTip"));
            this.BtnClickNJog.UseAltImage = false;
            this.BtnClickNJog.Click += new System.EventHandler(this.BtnClickNJog_Click);
            // 
            // UpdateFMax
            // 
            this.UpdateFMax.Interval = 1000;
            this.UpdateFMax.Tick += new System.EventHandler(this.UpdateFMax_Tick);
            // 
            // JogForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlp);
            this.Name = "JogForm";
            this.TT.SetToolTip(this, resources.GetString("$this.ToolTip"));
            this.tlp.ResumeLayout(false);
            this.tlp.PerformLayout();
            this.TlpSpeedControl.ResumeLayout(false);
            this.TlpSpeedControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TbSpeed)).EndInit();
            this.TlpStepControl.ResumeLayout(false);
            this.TlpStepControl.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TbStep)).EndInit();
            this.TlpZControl.ResumeLayout(false);
            this.TlpZControl.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tlp;
		private DirectionButton BtnS;
		private DirectionButton BtnSW;
		private DirectionButton BtnHome;
		private DirectionButton BtnW;
		private DirectionButton BtnN;
		private DirectionButton BtnSE;
		private DirectionButton BtnE;
		private DirectionButton BtnNW;
		private DirectionButton BtnNE;
		private System.Windows.Forms.TrackBar TbSpeed;
		private StepBar TbStep;
		private System.Windows.Forms.ToolTip TT;
		private System.Windows.Forms.TableLayoutPanel TlpSpeedControl;
		private System.Windows.Forms.Label LblSpeed;
		private System.Windows.Forms.TableLayoutPanel TlpStepControl;
		private System.Windows.Forms.Label LblStep;
		private System.Windows.Forms.Timer UpdateFMax;
        private System.Windows.Forms.TableLayoutPanel TlpZControl;
        private DirectionStepButton BtnZdown10;
        private DirectionStepButton BtnZdown1;
        private DirectionStepButton BtnZdown01;
        private DirectionStepButton BtnZup01;
        private DirectionStepButton BtnZup1;
        private DirectionStepButton BtnZup10;
        private System.Windows.Forms.Label label1;
        private GCodeButton BtnBR;
        private GCodeButton BtnB;
        private GCodeButton BtnR;
        private GCodeButton BtnTR;
        private GCodeButton BtnT;
        private GCodeButton BtnBL;
        private GCodeButton BtnL;
        private GCodeButton BtnTL;
        private GCodeButton BtnShape;
        private GCodeButton BtnC;
        private TwoStatesGCodeButton BtnFocus;
        private GCodeButton BtnFrame;
        private TwoStatesGCodeButton BtnClickNJog;
        private GCodeButton BtnFrameInf;
    }
}
