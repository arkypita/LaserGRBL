using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.ComponentModel;

namespace LaserGRBL.UserControls.IntegerInput
{

	public class IntegerInputRanged : IntegerInputBase
	{

		//Form esegue l'override del metodo Dispose per pulire l'elenco dei componenti.
		protected override void Dispose(bool disposing)
		{
			if (disposing) {
				if ((components != null)) {
					components.Dispose();
				}
			}
			base.Dispose(disposing);
		}

		//Richiesto da Progettazione Windows Form

		private System.ComponentModel.IContainer components;
		//NOTA: la procedura che segue è richiesta da Progettazione Windows Form
		//Può essere modificata in Progettazione Windows Form.  
		//Non modificarla nell'editor del codice.
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			this.SuspendLayout();
			//
			//IntegerInputRanged
			//
			//Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
			this.Name = "IntegerInputRanged";
			this.Size = new System.Drawing.Size(78, 15);
			this.ResumeLayout(false);
			this.PerformLayout();

		}


		public event MaxValueChangedEventHandler MaxValueChanged;
		public delegate void MaxValueChangedEventHandler(object sender, int NewValue);
		//ok
		public event MinValueChangedEventHandler MinValueChanged;
		public delegate void MinValueChangedEventHandler(object sender, int NewValue);
		//ok

		public event IsInErrorChangedEventHandler IsInErrorChanged;
		public delegate void IsInErrorChangedEventHandler(object sender, bool NewValue);
		//ok

		private bool _ValidationEnabled = true;
		private int _MaxValue = 100;

		private int _MinValue = -100;

		private Color _inErrorBorderColor = Color.Red;
		[Category("Behaviour"), DefaultValue(true)]
		public bool ValidationEnabled {
			get { return _ValidationEnabled; }
			set {
				_ValidationEnabled = value;
				UpdateStatus();
				CheckInError();
			}
		}

		[Category("Apparence")]
		public Color InErrorBorderColor {
			get { return _inErrorBorderColor; }
			set {
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

		[Category("Numeric Info"), DefaultValue(100)]
		public int MaxValue {
			get { return _MaxValue; }
			set {
				if (!(value == _MaxValue)) {
					_MaxValue = value;
					UpdateStatus();
					if (MaxValueChanged != null) {
						MaxValueChanged(this, value);
					}
					CheckInError();
				}
			}
		}

		[Category("Numeric Info"), DefaultValue(-100)]
		public int MinValue {
			get { return _MinValue; }
			set {
				if (!(value == _MinValue)) {
					_MinValue = value;
					UpdateStatus();
					if (MinValueChanged != null) {
						MinValueChanged(this, value);
					}
					CheckInError();
				}
			}
		}

		[Category("Numeric Info"), DefaultValue(0)]
		public override int CurrentValue {
			get { return base.CurrentValue; }
			set {
				base.CurrentValue = value;
				CheckInError();
			}
		}

		[Category("Runtime Property"), System.ComponentModel.Browsable(false)]
		public bool InError {
			get { return ValidationEnabled && (CurrentValue > MaxValue || CurrentValue < MinValue); }
		}

		static bool OldErrorState = false;
		private void CheckInError()
		{
			if (InError != OldErrorState)
			{
				OldErrorState = InError;

				if (IsInErrorChanged != null) 
					IsInErrorChanged(this, InError);
			}
		}

		protected override void UpdateStatus()
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
				if (this.InError && ForcedText == null) {
					this.BorderColor = InErrorBorderColor;
				} else {
					this.BorderColor = NormalBorderColor;
				}
			}
		}

		protected override bool AcceptKeys(char key)
		{
			System.Globalization.CultureInfo CI = System.Globalization.CultureInfo.CurrentCulture;
			return char.IsDigit(key) || char.IsControl(key) || key.ToString() == CI.NumberFormat.NumberDecimalSeparator || (key.ToString() == CI.NumberFormat.NegativeSign & MinValue < 0);
		}


		private bool _ForceMinMax;
		public bool ForceMinMax {
			get { return _ForceMinMax; }
			set { _ForceMinMax = value; }
		}

		protected override void OnValueChanging(double oldvalue, double newvalue, ref bool cancel)
		{
			if (ForceMinMax & (newvalue < MinValue | newvalue > MaxValue)) {
				cancel = true;
			}
		}

	}


}
