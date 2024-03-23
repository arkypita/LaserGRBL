using LaserGRBL.UserControls.NumericInput;
using System.Drawing;

namespace LaserGRBL.UserControls
{
    public partial class TextInput : ColoredBorderControl
    {
        public TextInput()
        {
            InitializeComponent();
        }

        public new Color ForeColor
        {
            get => mTextBox.ForeColor;
            set => mTextBox.ForeColor = value;
        }

        public new Color BackColor
        {
            get => mTextBox.BackColor;
            set {
                base.BackColor = value;
                mTextBox.BackColor = value;
            }
        }

        public new string Text
        {
            get => mTextBox.Text;
            set => mTextBox.Text = value;
        }

        public bool ReadOnly
        {
            get => mTextBox.ReadOnly;
            set => mTextBox.ReadOnly = value;
        }

    }

}
