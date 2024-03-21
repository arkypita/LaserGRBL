namespace LaserGRBL.UserControls.NumericInput
{
    partial class NumericUpDown
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.mNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.SuspendLayout();
            // 
            // mTextBox
            // 
            this.mNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mNumericUpDown.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.mNumericUpDown.Location = new System.Drawing.Point(4, 2);
            this.mNumericUpDown.Name = "mTextBox";
            this.mNumericUpDown.Size = new System.Drawing.Size(95, 13);
            this.mNumericUpDown.TabIndex = 0;
            // 
            // TextInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mNumericUpDown);
            this.Name = "TextInput";
            this.Size = new System.Drawing.Size(100, 20);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
        private System.Windows.Forms.NumericUpDown mNumericUpDown;
    }
}
