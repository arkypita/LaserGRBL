//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;

namespace LaserGRBL.UserControls.NumericInput
{
	[DefaultEvent("CurrentValueChanged"), DefaultProperty("CurrentValue")]
	public class IntegerInputBase : ColoredBorderControl
	{

		//UserControl esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
		protected override void Dispose(bool disposing)
		{
			/*if (disposing) {
				if ((components != null)) {
					components.Dispose();
				}
			}*/
			base.Dispose(disposing);
		}

		//Richiesto da Progettazione Windows Form

		//private IContainer components;
		//NOTA: la procedura che segue è richiesta da Progettazione Windows Form
		//Può essere modificata in Progettazione Windows Form.  
		//Non modificarla nell'editor del codice.
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.TB = new System.Windows.Forms.TextBox();
			this.LB = new System.Windows.Forms.Label();
			this.SuspendLayout();
			//
			//TB
			//
			this.TB.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.TB.Dock = System.Windows.Forms.DockStyle.Fill;
			this.TB.Location = new System.Drawing.Point(1, 1);
			this.TB.Name = "TB";
			this.TB.Size = new System.Drawing.Size(77, 13);
			this.TB.TabIndex = 0;
			this.TB.KeyDown += TBKeyDown;
			this.TB.KeyPress += TBKeyPress;
			this.TB.GotFocus += FocusEvent;
			this.TB.LostFocus += FocusEvent;
			this.TB.TextChanged += TBTextChanged;
			//
			//LB
			//
			this.LB.BackColor = System.Drawing.SystemColors.Control;
			this.LB.Dock = System.Windows.Forms.DockStyle.Fill;
			this.LB.Location = new System.Drawing.Point(1, 1);
			this.LB.Name = "LB";
			this.LB.Size = new System.Drawing.Size(77, 13);
			this.LB.TabIndex = 1;
			this.LB.Text = "LB";
			//
			//IntegerInputBase
			//
			this.Controls.Add(this.TB);
			this.Controls.Add(this.LB);
			this.Name = "IntegerInputBase";
			this.Size = new System.Drawing.Size(79, 15);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		internal System.Windows.Forms.TextBox TB;
		internal System.Windows.Forms.Label LB;

		//ok
		public event CurrentValueChangedEventHandler OnTheFlyValueChanged;
		public event CurrentValueChangedEventHandler CurrentValueChanged;
		public delegate void CurrentValueChangedEventHandler(object sender, int OldValue, int NewValue, bool ByUser);
		//ok

		public event BeginEditEventHandler BeginEdit;
		public delegate void BeginEditEventHandler(object sender);
		//ok
		public event EndEditEventHandler EndEdit;
		public delegate void EndEditEventHandler(object sender);
		//ok

		private int _CurrentValue = 0;

		private string _ForcedText;
		private Color _inEditBorderColor = Color.Orange;
		private void Init()
		{
			LB.TabStop = false;
			UpdateStatus();
		}

		protected override void OnSizeChanged(System.EventArgs e)
		{
			base.OnSizeChanged(e);

			if ((TB != null)) {
				this.Height = TB.Height + 2;
				TB.Location = new Point(1, 1);
				TB.Width = this.Width - 2;
				LB.Location = TB.Location;
				LB.Width = TB.Width;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Color BorderColor {
			get { return base.BorderColor; }
			set { base.BorderColor = value; }
		}

		[EditorBrowsable(EditorBrowsableState.Never), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new Color BackColor {
			get { return base.BackColor; }
			set { base.BackColor = value; }
		}

		[Category("Apparence")]
		public Color InEditBorderColor {
			get { return _inEditBorderColor; }
			set {
				_inEditBorderColor = value;
				UpdateStatus();
			}
		}

		private void ResetInEditBorderColor()
		{
			InEditBorderColor = Color.Orange;
		}

		private bool ShouldSerializeInEditBorderColor()
		{
			return !InEditBorderColor.Equals(Color.Orange);
		}

		[Category("Numeric Info"), DefaultValue(0)]
		public virtual int CurrentValue {
			get { return _CurrentValue; }
			set { SetCurrentValue(value, false); }
		}

		private void SetCurrentValue(int value, bool byuser)
		{
			if (!(value == _CurrentValue)) {
				bool cancel = false;
				OnValueChanging(_CurrentValue, value, ref cancel);
				if (!cancel) {
					int O = _CurrentValue;
					_CurrentValue = value;
					_ForcedText = null;
					UpdateStatus();
					if (CurrentValueChanged != null)
						CurrentValueChanged(this, O, value, byuser);
				}
			}
		}

		public void SetCurrentValueAsUser(int value)
		{
			SetCurrentValue(value, true);
		}


		protected virtual void OnValueChanging(double oldvalue, double newvalue, ref bool cancel)
		{
		}

		[Category("Runtime Property"), System.ComponentModel.Browsable(false)]
		public bool InEdit {
			get { return TB.ContainsFocus; }
		}

		private void TBKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if (e.KeyCode == System.Windows.Forms.Keys.Enter)
			{
				this.ParentForm.ActiveControl = null;
			}
			if (e.KeyCode == System.Windows.Forms.Keys.Escape)
			{
				this.TB.Text = null;
				this.ParentForm.ActiveControl = null;
			}
		}

		protected virtual void TBKeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
		{
			e.Handled = !AcceptKeys(e.KeyChar);
		}

		void TBTextChanged(object sender, EventArgs e)
		{
			try
			{
				int newValue = ParseEditValue();
                OnTheFlyValueChanged?.Invoke(this, OldEditValue, newValue, InEdit);
            }
			catch { }
		}


		protected virtual bool AcceptKeys(char key)
		{
			System.Globalization.CultureInfo CI = System.Globalization.CultureInfo.CurrentCulture;
			return char.IsDigit(key) || char.IsControl(key) || key.ToString() == CI.NumberFormat.NegativeSign;
		}

		private void FocusEvent(object sender, System.EventArgs e)
		{
			if (this.InEdit)
				OnBeginEdit();
			if (!this.InEdit)
				OnEndEdit();
		}

		private int OldEditValue;
		private void OnBeginEdit()
		{
			if (ForcedText != null)
				TB.Clear();

			try {OldEditValue = ParseEditValue();}
			catch {OldEditValue = _CurrentValue;}

			UpdateStatus();
			if (BeginEdit != null) {
				BeginEdit(this);
			}
		}

		private int NewEditValue;
		protected virtual void OnEndEdit()
		{
			try 
			{
				NewEditValue = ParseEditValue();
				if ((!(NewEditValue == OldEditValue))) {
					SetCurrentValue(NewEditValue, true);
				}
				UpdateStatus();
			}
			catch {UpdateStatus();}

			if (EndEdit != null) {
				EndEdit(this);
			}
		}

		protected virtual int ParseEditValue()
		{
			return int.Parse(TB.Text);
		}



		protected virtual void UpdateStatus()
		{
			if (this.InEdit) {
				//TB.Text = CurrentValue.ToString
			} else {
				if (ForcedText != null) {
					TB.Text = ForcedText;
				} else {
					TB.Text = CurrentValue.ToString(GetFormatString());
				}
			}
			LB.Text = TB.Text;

			if (this.InEdit) {
				this.BorderColor = InEditBorderColor;
			} else {
				this.BorderColor = ColorScheme.ControlsBorder;
			}

		}

		protected string GetFormatString()
		{
			return "0";
		}

		private bool _enabled = true;
		public new bool Enabled {
			get { return _enabled; }
			set {
				if (!(value == _enabled)) {
					_enabled = value;
					OnEnabledChange();
				}
			}
		}

		public string ForcedText {
			get { return _ForcedText; }
			set {
				_ForcedText = value;
				UpdateStatus();
			}
		}

		private void OnEnabledChange()
		{
			LB.Visible = !Enabled;
			TB.Visible = Enabled;
			this.TabStop = Enabled;
		}


		public IntegerInputBase()
		{
			// Chiamata richiesta da Progettazione Windows Form.
			InitializeComponent();

			// Aggiungere le eventuali istruzioni di inizializzazione dopo la chiamata a InitializeComponent().
			Init();
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
