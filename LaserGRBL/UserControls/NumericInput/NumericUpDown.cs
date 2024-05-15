using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LaserGRBL.UserControls.NumericInput
{
    public partial class NumericUpDown : ColoredBorderControl, ISupportInitialize
    {

        public event EventHandler ValueChanged;

        public NumericUpDown()
        {
            InitializeComponent();
            mNumericUpDown.ValueChanged += MNumericUpDown_ValueChanged;
        }

        public int DecimalPlaces
        {
            get => mNumericUpDown.DecimalPlaces;
            set => mNumericUpDown.DecimalPlaces = value;
        }

        public decimal Increment
        {
            get => mNumericUpDown.Increment;
            set => mNumericUpDown.Increment = value;
        }

        private void MNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            ValueChanged?.Invoke(sender, e);
        }

        public void BeginInit()
        {
            mNumericUpDown.BeginInit();
        }

        public void EndInit()
        {
            mNumericUpDown.EndInit();
        }

        public decimal Value
        {
            get => mNumericUpDown.Value;
            set => mNumericUpDown.Value = value;
        }

        public decimal Minimum
        {
            get => mNumericUpDown.Minimum;
            set => mNumericUpDown.Minimum = value;
        }

        public decimal Maximum
        {
            get => mNumericUpDown.Maximum;
            set => mNumericUpDown.Maximum = value;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            mNumericUpDown.BackColor = ColorScheme.LogBackColor;
            BackColor = ColorScheme.LogBackColor;
            mNumericUpDown.ForeColor = ColorScheme.FormForeColor;
            BorderColor = ColorScheme.ControlsBorder;
            base.OnPaint(e);
        }
        

    }
}
