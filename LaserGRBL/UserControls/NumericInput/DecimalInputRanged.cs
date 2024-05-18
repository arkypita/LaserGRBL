//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.Drawing;
using System.ComponentModel;

namespace LaserGRBL.UserControls.NumericInput
{
    public class DecimalInputRanged : DecimalInputBase
    {

        // Form esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
        protected override void Dispose(bool disposing)
        {
            /*if (disposing)
            {
                if (!(components == null))
                    components.Dispose();
            }*/
            base.Dispose(disposing);
        }

        // Richiesto da Progettazione Windows Form
        //private IContainer components;

        // NOTA: la procedura che segue è richiesta da Progettazione Windows Form
        // Può essere modificata in Progettazione Windows Form.  
        // Non modificarla nell'editor del codice.
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // DecimalInputRanged
            // 
            // Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            this.Name = "DecimalInputRanged";
            this.Size = new System.Drawing.Size(78, 15);
            this.ResumeLayout(false);
            this.PerformLayout();
        }


        public event MaxValueChangedEventHandler MaxValueChanged;        // ok

        public delegate void MaxValueChangedEventHandler(object sender, float NewValue);

        public event MinValueChangedEventHandler MinValueChanged;        // ok

        public delegate void MinValueChangedEventHandler(object sender, float NewValue);

        public event IsInErrorChangedEventHandler IsInErrorChanged;      // ok

        public delegate void IsInErrorChangedEventHandler(object sender, bool NewValue);

        private bool _ValidationEnabled = true;
        private float _MaxValue = 100;
        private float _MinValue = -100;

        private Color _inErrorBorderColor = Color.Red;

        [Category("Behaviour")]
        [DefaultValue(true)]
        public bool ValidationEnabled
        {
            get
            {
                return _ValidationEnabled;
            }
            set
            {
                _ValidationEnabled = value;
                UpdateStatus();
                CheckInError();
            }
        }

        [Category("Apparence")]
        public Color InErrorBorderColor
        {
            get
            {
                return _inErrorBorderColor;
            }
            set
            {
                _inErrorBorderColor = value;
                UpdateStatus();
            }
        }

        private void ResetInErrorBorderColor()
        {
            InErrorBorderColor = Color.Red;
        }

        private bool ShouldSerializeInErrorBorderColor()
        {
            return !InErrorBorderColor.Equals(Color.Red);
        }

        [Category("Numeric Info")]
        [DefaultValue(100)]
        public float MaxValue
        {
            get
            {
                return _MaxValue;
            }
            set
            {
                if (value != _MaxValue)
                {
                    _MaxValue = value;
                    UpdateStatus();
                    MaxValueChanged?.Invoke(this, value);
                    CheckInError();
                }
            }
        }

        [Category("Numeric Info")]
        [DefaultValue(-100)]
        public float MinValue
        {
            get
            {
                return _MinValue;
            }
            set
            {
                if (value != _MinValue)
                {
                    _MinValue = value;
                    UpdateStatus();
                    MinValueChanged?.Invoke(this, value);
                    CheckInError();
                }
            }
        }

        [Category("Numeric Info")]
        [DefaultValue(0)]
        public override float CurrentValue
        {
            get
            {
                return base.CurrentValue;
            }
            set
            {
                base.CurrentValue = value;
                CheckInError();
            }
        }

        [Category("Runtime Property")]
        [System.ComponentModel.Browsable(false)]
        public bool InError
        {
            get
            {
                return ValidationEnabled && (CurrentValue > MaxValue || CurrentValue < MinValue);
            }
        }

        private bool OldErrorState = false;
        private void CheckInError()
        {
			if (this.InError != OldErrorState)
            {
                OldErrorState = this.InError;
                IsInErrorChanged?.Invoke(this, InError);
            }
        }

        protected override void UpdateStatus()
        {
            if (this.InEdit)
            {
            }
            else
                TB.Text = CurrentValue.ToString(GetFormatString());
            LB.Text = TB.Text;

            if (this.InEdit)
                this.BorderColor = InEditBorderColor;
            else if (this.InError)
                this.BorderColor = InErrorBorderColor;
            else
                this.BorderColor = ColorScheme.ControlsBorder;
        }

        protected override bool AcceptKeys(char key)
        {
            System.Globalization.CultureInfo CI = System.Globalization.CultureInfo.CurrentCulture;
            return char.IsDigit(key) | char.IsControl(key) | key.ToString() == CI.NumberFormat.NumberDecimalSeparator | (key.ToString() == CI.NumberFormat.NegativeSign & MinValue < 0);
        }

        private bool _ForceMinMax;
        public bool ForceMinMax
        {
            get
            {
                return _ForceMinMax;
            }
            set
            {
                _ForceMinMax = value;
            }
        }

        protected override void OnValueChanging(float oldvalue, float newvalue, ref bool cancel)
        {
            if (ForceMinMax & (newvalue < MinValue | newvalue > MaxValue))
                cancel = true;
        }
    }
}
