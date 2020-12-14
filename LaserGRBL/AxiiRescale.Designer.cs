
namespace LaserGRBL
{
    partial class AxiiRescale
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
            this.XTextBox = new System.Windows.Forms.TextBox();
            this.YTextBox = new System.Windows.Forms.TextBox();
            this.dimensionsLabel = new System.Windows.Forms.Label();
            this.RescaleButton = new System.Windows.Forms.Button();
            this.RatioLockCheckBox = new System.Windows.Forms.CheckBox();
            this.xLabel = new System.Windows.Forms.Label();
            this.yLabel = new System.Windows.Forms.Label();
            this.HalfSizeButton = new System.Windows.Forms.Button();
            this.OriginalSizeButton = new System.Windows.Forms.Button();
            this.DoubleSizeButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // XTextBox
            // 
            this.XTextBox.Location = new System.Drawing.Point(84, 85);
            this.XTextBox.Name = "XTextBox";
            this.XTextBox.Size = new System.Drawing.Size(78, 20);
            this.XTextBox.TabIndex = 4;
            this.XTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.XTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyUp);
            this.XTextBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.TextBox_MouseDoubleClick);
            this.XTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.NumbersValidation);
            // 
            // YTextBox
            // 
            this.YTextBox.Enabled = false;
            this.YTextBox.Location = new System.Drawing.Point(84, 111);
            this.YTextBox.Name = "YTextBox";
            this.YTextBox.Size = new System.Drawing.Size(78, 20);
            this.YTextBox.TabIndex = 5;
            this.YTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.YTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.TextBox_KeyUp);
            this.YTextBox.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.TextBox_MouseDoubleClick);
            this.YTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.NumbersValidation);
            // 
            // dimensionsLabel
            // 
            this.dimensionsLabel.AutoSize = true;
            this.dimensionsLabel.Location = new System.Drawing.Point(12, 9);
            this.dimensionsLabel.Name = "dimensionsLabel";
            this.dimensionsLabel.Size = new System.Drawing.Size(64, 13);
            this.dimensionsLabel.TabIndex = 2;
            this.dimensionsLabel.Text = "Dimensions:";
            // 
            // RescaleButton
            // 
            this.RescaleButton.Location = new System.Drawing.Point(12, 138);
            this.RescaleButton.Name = "RescaleButton";
            this.RescaleButton.Size = new System.Drawing.Size(171, 36);
            this.RescaleButton.TabIndex = 6;
            this.RescaleButton.Text = "Rescale";
            this.RescaleButton.UseVisualStyleBackColor = true;
            this.RescaleButton.Click += new System.EventHandler(this.RescaleButton_Click);
            // 
            // RatioLockCheckBox
            // 
            this.RatioLockCheckBox.AutoSize = true;
            this.RatioLockCheckBox.Checked = true;
            this.RatioLockCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.RatioLockCheckBox.Location = new System.Drawing.Point(42, 63);
            this.RatioLockCheckBox.Name = "RatioLockCheckBox";
            this.RatioLockCheckBox.Size = new System.Drawing.Size(114, 17);
            this.RatioLockCheckBox.TabIndex = 3;
            this.RatioLockCheckBox.Text = "Aspect Ratio Lock";
            this.RatioLockCheckBox.UseVisualStyleBackColor = true;
            this.RatioLockCheckBox.CheckedChanged += new System.EventHandler(this.RatioLockCheckBox_CheckedChanged);
            // 
            // xLabel
            // 
            this.xLabel.AutoSize = true;
            this.xLabel.Location = new System.Drawing.Point(39, 92);
            this.xLabel.Name = "xLabel";
            this.xLabel.Size = new System.Drawing.Size(39, 13);
            this.xLabel.TabIndex = 6;
            this.xLabel.Text = "X (mm)";
            // 
            // yLabel
            // 
            this.yLabel.AutoSize = true;
            this.yLabel.Location = new System.Drawing.Point(39, 118);
            this.yLabel.Name = "yLabel";
            this.yLabel.Size = new System.Drawing.Size(39, 13);
            this.yLabel.TabIndex = 7;
            this.yLabel.Text = "Y (mm)";
            // 
            // HalfSizeButton
            // 
            this.HalfSizeButton.Location = new System.Drawing.Point(15, 25);
            this.HalfSizeButton.Name = "HalfSizeButton";
            this.HalfSizeButton.Size = new System.Drawing.Size(54, 32);
            this.HalfSizeButton.TabIndex = 0;
            this.HalfSizeButton.Text = "0.5 X";
            this.HalfSizeButton.UseVisualStyleBackColor = true;
            this.HalfSizeButton.Click += new System.EventHandler(this.HalfSizeButton_Click);
            // 
            // OriginalSizeButton
            // 
            this.OriginalSizeButton.Location = new System.Drawing.Point(72, 25);
            this.OriginalSizeButton.Name = "OriginalSizeButton";
            this.OriginalSizeButton.Size = new System.Drawing.Size(54, 32);
            this.OriginalSizeButton.TabIndex = 1;
            this.OriginalSizeButton.Text = "1 X";
            this.OriginalSizeButton.UseVisualStyleBackColor = true;
            this.OriginalSizeButton.Click += new System.EventHandler(this.OriginalSizeButton_Click);
            // 
            // DoubleSizeButton
            // 
            this.DoubleSizeButton.Location = new System.Drawing.Point(129, 25);
            this.DoubleSizeButton.Name = "DoubleSizeButton";
            this.DoubleSizeButton.Size = new System.Drawing.Size(54, 32);
            this.DoubleSizeButton.TabIndex = 2;
            this.DoubleSizeButton.Text = "2 X";
            this.DoubleSizeButton.UseVisualStyleBackColor = true;
            this.DoubleSizeButton.Click += new System.EventHandler(this.DoubleSizeButton_Click);
            // 
            // AxiiRescale
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(195, 189);
            this.Controls.Add(this.DoubleSizeButton);
            this.Controls.Add(this.OriginalSizeButton);
            this.Controls.Add(this.HalfSizeButton);
            this.Controls.Add(this.yLabel);
            this.Controls.Add(this.xLabel);
            this.Controls.Add(this.RatioLockCheckBox);
            this.Controls.Add(this.RescaleButton);
            this.Controls.Add(this.dimensionsLabel);
            this.Controls.Add(this.YTextBox);
            this.Controls.Add(this.XTextBox);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AxiiRescale";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "File Axii Rescale";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox XTextBox;
        private System.Windows.Forms.TextBox YTextBox;
        private System.Windows.Forms.Label dimensionsLabel;
        private System.Windows.Forms.Button RescaleButton;
        private System.Windows.Forms.CheckBox RatioLockCheckBox;
        private System.Windows.Forms.Label xLabel;
        private System.Windows.Forms.Label yLabel;
        private System.Windows.Forms.Button HalfSizeButton;
        private System.Windows.Forms.Button OriginalSizeButton;
        private System.Windows.Forms.Button DoubleSizeButton;
    }
}