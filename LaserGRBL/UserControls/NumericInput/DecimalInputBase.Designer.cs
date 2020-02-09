namespace LaserGRBL.UserControls.NumericInput
{
    public partial class DecimalInputBase
    {

        protected System.Windows.Forms.TextBox TB;
        internal System.Windows.Forms.Label LB;

        // UserControl esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
        protected override void Dispose(bool disposing)
        {
            /*if (disposing)
            {
                if (!(components == null))
                {
                    components.Dispose();
                }

            }*/

            base.Dispose(disposing);
        }

        // Richiesto da Progettazione Windows Form
        //private System.ComponentModel.IContainer components;

        // NOTA: la procedura che segue � richiesta da Progettazione Windows Form
        // Pu� essere modificata in Progettazione Windows Form.  
        // Non modificarla nell'editor del codice.
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            this.TB = new System.Windows.Forms.TextBox();
            this.LB = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // TB
            // 
            this.TB.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TB.Location = new System.Drawing.Point(1, 1);
            this.TB.Name = "TB";
            this.TB.Size = new System.Drawing.Size(77, 13);
            this.TB.TabIndex = 0;
            // 
            // LB
            // 
            this.LB.BackColor = System.Drawing.SystemColors.Control;
            this.LB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LB.Location = new System.Drawing.Point(1, 1);
            this.LB.Name = "LB";
            this.LB.Size = new System.Drawing.Size(77, 13);
            this.LB.TabIndex = 1;
            this.LB.Text = "LB";
            this.LB.Visible = false;
            this.Controls.Add(this.TB);
            this.Controls.Add(this.LB);
            this.Name = "DecimalInputBase";
            this.Size = new System.Drawing.Size(79, 15);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
