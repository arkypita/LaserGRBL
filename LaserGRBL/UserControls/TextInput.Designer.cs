namespace LaserGRBL.UserControls
{
    partial class TextInput
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
			this.mTextBox = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// mTextBox
			// 
			this.mTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.mTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.mTextBox.Location = new System.Drawing.Point(4, 4);
			this.mTextBox.Name = "mTextBox";
			this.mTextBox.Size = new System.Drawing.Size(92, 13);
			this.mTextBox.TabIndex = 0;
			this.mTextBox.TextChanged += new System.EventHandler(this.mTextBox_TextChanged);
			// 
			// TextInput
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.mTextBox);
			this.Name = "TextInput";
			this.Size = new System.Drawing.Size(100, 20);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox mTextBox;
    }
}
