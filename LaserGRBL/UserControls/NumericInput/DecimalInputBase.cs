//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System.ComponentModel;
using System.Drawing;
using System;
using System.Windows.Forms;

namespace LaserGRBL.UserControls.NumericInput
{

    [DefaultEvent("CurrentValueChanged")]
    [DefaultProperty("CurrentValue")]
    public partial class DecimalInputBase : ColoredBorderControl
    {

        public delegate void CurrentValueChangedDlg(object sender, float OldValue, float NewValue, bool ByUser);
        public event CurrentValueChangedDlg CurrentValueChanged;
        public event CurrentValueChangedDlg OnTheFlyValueChanged;

        public delegate void DecimalPositionsChangedDlg(object sender, int NewValue);
        public event DecimalPositionsChangedDlg DecimalPositionsChanged;

        public delegate void BeginEditDlg(object sender);
        public event BeginEditDlg BeginEdit;


        public delegate void EndEditDlg(object sender);
        public event EndEditDlg EndEdit;

        // ok
        private float _CurrentValue = 0;
        private int _DecimalPositions = 3;
        private Color _inEditBorderColor = Color.Orange;
        private Color _NormalBorderColor = Color.DodgerBlue;
        private float OldEditValue;
        private float NewEditValue;
        private bool _enabled = true;

        private void Init()
        {
            LB.TabStop = false;
            this.UpdateStatus();
        }

        public DecimalInputBase()
        {
            //  Chiamata richiesta da Progettazione Windows Form.
            this.InitializeComponent();
            //  Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent().
            this.Init();

            TB.GotFocus += FocusEvent;
            TB.LostFocus += FocusEvent;
            TB.KeyDown += TBKeyDown;
            TB.KeyPress += TBKeyPress;
            TB.MouseWheel += TBWheel;
            TB.TextChanged += TBTextChanged;
        }

        protected override void OnSizeChanged(System.EventArgs e)
        {
            base.OnSizeChanged(e);
            if (!(TB == null))
            {
                this.Height = (TB.Height + 2);
                TB.Location = new Point(1, 1);
                TB.Width = (this.Width - 2);
                LB.Location = TB.Location;
                LB.Width = TB.Width;
            }

        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Color BorderColor
        {
            get { return base.BorderColor; }
            set { base.BorderColor = value; }
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Color BackColor
        {
            get { return base.BackColor; }
            set { base.BackColor = value; }
        }

        [Category("Apparence")]
        public Color InEditBorderColor
        {
            get { return _inEditBorderColor; }
            set
            {
                _inEditBorderColor = value;
                this.UpdateStatus();
            }
        }

        private void ResetInEditBorderColor()
        { InEditBorderColor = Color.Orange; }

        private bool ShouldSerializeInEditBorderColor()
        { return !InEditBorderColor.Equals(Color.Orange); }

        [Category("Numeric Info")]
        [DefaultValue(0)]
        public virtual float CurrentValue
        {
            get { return _CurrentValue; }
            set { this.SetCurrentValue(value, false); }
        }

        private void SetCurrentValue(float value, bool byuser)
        {
            value = (float)Math.Round(value, DecimalPositions);
            if (value != _CurrentValue)
            {
                bool cancel = false;
                this.OnValueChanging(_CurrentValue, value, ref cancel);
                if (!cancel)
                {
                    float oldvalue = _CurrentValue;
                    _CurrentValue = value;
                    CurrentValueChanged?.Invoke(this, oldvalue, value, byuser);
                }

            }

            this.UpdateStatus();
        }

        public void SetCurrentValueAsUser(float value)
        {
            this.SetCurrentValue(value, true);
        }

        [Category("Numeric Info")]
        [DefaultValue(0)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        [Browsable(false)]
        public virtual int CurrentIntValue
        {
            get { return (int)(_CurrentValue * Math.Pow(10, DecimalPositions)); }
            set { CurrentValue = (float)value / (float)Math.Pow(10, DecimalPositions); }
        }

        [Category("Numeric Info")]
        [DefaultValue(3)]
        public int DecimalPositions
        {
            get
            {
                return _DecimalPositions;
            }
            set
            {
                if ((value < 0))
                {
                    throw new ArgumentException();
                }
                else if (value != _DecimalPositions)
                {
                    _DecimalPositions = value;
                    CurrentValue = (float)Math.Round(CurrentValue, value);
                    this.UpdateStatus();
                    DecimalPositionsChanged?.Invoke(this, value);
                }

            }
        }

        [Category("Runtime Property")]
        [Browsable(false)]
        public bool InEdit
        {
            get
            {
                return TB.ContainsFocus;
            }
        }

        private void TBKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if ((e.KeyCode == System.Windows.Forms.Keys.Enter))
            {
                this.ParentForm.ActiveControl = null;
            }

            if ((e.KeyCode == System.Windows.Forms.Keys.Escape))
            {
                this.TB.Text = null;
                this.ParentForm.ActiveControl = null;
            }

        }

        protected virtual void TBKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            e.Handled = !this.AcceptKeys(e.KeyChar);
        }

        private void TBWheel(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            OnMouseWheel(e.Delta / System.Windows.Forms.SystemInformation.MouseWheelScrollDelta);
        }

        protected void OnMouseWheel(int delta)
        {
        }

        void TBTextChanged(object sender, EventArgs e)
        {
            try
            {
                float newValue = ParseEditValue();
                OnTheFlyValueChanged?.Invoke(this, OldEditValue, newValue, InEdit);
            }
            catch { }
        }

        protected virtual bool AcceptKeys(char key)
        {
            System.Globalization.CultureInfo CI = System.Globalization.CultureInfo.CurrentCulture;
            return char.IsDigit(key) || char.IsControl(key) || new string(key,1) == CI.NumberFormat.NegativeSign || new string(key, 1) == CI.NumberFormat.NumberDecimalSeparator;
        }

        private void FocusEvent(object sender, System.EventArgs e)
        {
            if (InEdit) OnBeginEdit();
            if (!InEdit) OnEndEdit();
        }

        void OnBeginEdit()
        {
            // OldEditValue = ParseEditValue() 'sostituito con riga successiva xè aveva eccezioni in uscita
            OldEditValue = CurrentValue;
            this.UpdateStatus();
            BeginEdit?.Invoke(this);
        }



        protected virtual void OnEndEdit()
        {
            try
            {
                NewEditValue = this.ParseEditValue();
                if (!(NewEditValue == OldEditValue))
                {
                    this.SetCurrentValue(NewEditValue, true);
                }
                else
                {
                    this.UpdateStatus();
                }

            }
            catch (Exception)
            {
                this.UpdateStatus();
            }

            EndEdit?.Invoke(this);
        }

        protected virtual float ParseEditValue()
        {
            return float.Parse(TB.Text);
        }

        protected virtual void OnValueChanging(float oldvalue, float newvalue, ref bool cancel)
        {
        }

        protected virtual void UpdateStatus()
        {
            if (this.InEdit)
            {
                // TB.Text = CurrentValue.ToString
            }
            else
            {
                TB.Text = CurrentValue.ToString(GetFormatString());
            }

            LB.Text = TB.Text;
            if (this.InEdit)
            {
                this.BorderColor = InEditBorderColor;
            }
            else
            {
                this.BorderColor = ColorScheme.ControlsBorder;
            }

        }

        protected virtual string GetFormatString()
        {
            if (DecimalPositions > 0)
                return "0." + new string('0', DecimalPositions);
            else
                return "0";
        }

        

        public new bool Enabled
        {
            get
            {
                return _enabled;
            }
            set
            {
                if (!(value == _enabled))
                {
                    _enabled = value;
                    this.OnEnabledChange();
                }

            }
        }

        private void OnEnabledChange()
        {
            LB.Visible = !Enabled;
            TB.Visible = Enabled;
            this.TabStop = Enabled;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            BackColor = ColorScheme.LogBackColor;
            LB.BackColor = ColorScheme.LogBackColor;
            TB.BackColor = ColorScheme.LogBackColor;
            TB.ForeColor = ColorScheme.FormForeColor;
            base.OnPaint(e);
        }


    }
}