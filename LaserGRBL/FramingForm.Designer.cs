
using System;

namespace LaserGRBL
{
    partial class FramingForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FramingForm));
            this.XTextBox = new System.Windows.Forms.TextBox();
            this.YTextBox = new System.Windows.Forms.TextBox();
            this.RectangularFraming = new System.Windows.Forms.RadioButton();
            this.CircularFraming = new System.Windows.Forms.RadioButton();
            this.XLabel = new System.Windows.Forms.Label();
            this.YLabel = new System.Windows.Forms.Label();
            this.NPassesNum = new System.Windows.Forms.NumericUpDown();
            this.SPowerNum = new System.Windows.Forms.NumericUpDown();
            this.FeedrateNum = new System.Windows.Forms.NumericUpDown();
            this.StartButton = new System.Windows.Forms.Button();
            this.NPassesLabel = new System.Windows.Forms.Label();
            this.SPowerLabel = new System.Windows.Forms.Label();
            this.FeedrateLabel = new System.Windows.Forms.Label();
            this.EllipticFraming = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.NPassesNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SPowerNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FeedrateNum)).BeginInit();
            this.SuspendLayout();
            // 
            // XTextBox
            // 
            resources.ApplyResources(this.XTextBox, "XTextBox");
            this.XTextBox.Name = "XTextBox";
            this.XTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.NumbersValidation);
            this.XTextBox.Validated += new System.EventHandler(this.CircleValidation);
            // 
            // YTextBox
            // 
            resources.ApplyResources(this.YTextBox, "YTextBox");
            this.YTextBox.Name = "YTextBox";
            this.YTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.NumbersValidation);
            this.YTextBox.Validated += new System.EventHandler(this.CircleValidation);
            // 
            // RectangularFraming
            // 
            resources.ApplyResources(this.RectangularFraming, "RectangularFraming");
            this.RectangularFraming.Checked = true;
            this.RectangularFraming.Name = "RectangularFraming";
            this.RectangularFraming.TabStop = true;
            this.RectangularFraming.UseVisualStyleBackColor = true;
            // 
            // CircularFraming
            // 
            resources.ApplyResources(this.CircularFraming, "CircularFraming");
            this.CircularFraming.Name = "CircularFraming";
            this.CircularFraming.UseVisualStyleBackColor = true;
            this.CircularFraming.CheckedChanged += new System.EventHandler(this.CircularFramingChecked);
            // 
            // XLabel
            // 
            resources.ApplyResources(this.XLabel, "XLabel");
            this.XLabel.Name = "XLabel";
            // 
            // YLabel
            // 
            resources.ApplyResources(this.YLabel, "YLabel");
            this.YLabel.Name = "YLabel";
            // 
            // NPassesNum
            // 
            resources.ApplyResources(this.NPassesNum, "NPassesNum");
            this.NPassesNum.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.NPassesNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NPassesNum.Name = "NPassesNum";
            this.NPassesNum.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // SPowerNum
            // 
            resources.ApplyResources(this.SPowerNum, "SPowerNum");
            this.SPowerNum.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.SPowerNum.Name = "SPowerNum";
            this.SPowerNum.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // FeedrateNum
            // 
            this.FeedrateNum.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            resources.ApplyResources(this.FeedrateNum, "FeedrateNum");
            this.FeedrateNum.Maximum = new decimal(new int[] {
            9000,
            0,
            0,
            0});
            this.FeedrateNum.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.FeedrateNum.Name = "FeedrateNum";
            this.FeedrateNum.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // StartButton
            // 
            resources.ApplyResources(this.StartButton, "StartButton");
            this.StartButton.Name = "StartButton";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.FramingButton_Click);
            // 
            // NPassesLabel
            // 
            resources.ApplyResources(this.NPassesLabel, "NPassesLabel");
            this.NPassesLabel.Name = "NPassesLabel";
            // 
            // SPowerLabel
            // 
            resources.ApplyResources(this.SPowerLabel, "SPowerLabel");
            this.SPowerLabel.Name = "SPowerLabel";
            // 
            // FeedrateLabel
            // 
            resources.ApplyResources(this.FeedrateLabel, "FeedrateLabel");
            this.FeedrateLabel.Name = "FeedrateLabel";
            // 
            // EllipticFraming
            // 
            resources.ApplyResources(this.EllipticFraming, "EllipticFraming");
            this.EllipticFraming.Name = "EllipticFraming";
            this.EllipticFraming.UseVisualStyleBackColor = true;
            // 
            // FramingForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.EllipticFraming);
            this.Controls.Add(this.FeedrateLabel);
            this.Controls.Add(this.SPowerLabel);
            this.Controls.Add(this.NPassesLabel);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.FeedrateNum);
            this.Controls.Add(this.SPowerNum);
            this.Controls.Add(this.NPassesNum);
            this.Controls.Add(this.YLabel);
            this.Controls.Add(this.XLabel);
            this.Controls.Add(this.CircularFraming);
            this.Controls.Add(this.RectangularFraming);
            this.Controls.Add(this.YTextBox);
            this.Controls.Add(this.XTextBox);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FramingForm";
            this.Load += new System.EventHandler(this.FramingForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.NPassesNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SPowerNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FeedrateNum)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void FramingForm_Load(object sender, EventArgs e)
        {
            ;
        }

        #endregion

        private System.Windows.Forms.TextBox XTextBox;
        private System.Windows.Forms.TextBox YTextBox;
        private System.Windows.Forms.RadioButton RectangularFraming;
        private System.Windows.Forms.RadioButton CircularFraming;
        private System.Windows.Forms.Label XLabel;
        private System.Windows.Forms.Label YLabel;
        private System.Windows.Forms.NumericUpDown NPassesNum;
        private System.Windows.Forms.NumericUpDown SPowerNum;
        private System.Windows.Forms.NumericUpDown FeedrateNum;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.Label NPassesLabel;
        private System.Windows.Forms.Label SPowerLabel;
        private System.Windows.Forms.Label FeedrateLabel;
        private System.Windows.Forms.RadioButton EllipticFraming;
    }
}