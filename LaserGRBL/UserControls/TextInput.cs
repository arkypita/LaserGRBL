using LaserGRBL.UserControls.NumericInput;
using System.Windows.Forms;

namespace LaserGRBL.UserControls
{
    public partial class TextInput : ColoredBorderControl
    {
        public TextInput()
        {
            InitializeComponent();
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

        protected override void OnPaint(PaintEventArgs e)
        {
            BackColor = ColorScheme.LogBackColor;
            mTextBox.BackColor = ColorScheme.LogBackColor;
            mTextBox.ForeColor = ColorScheme.FormForeColor;
            BorderColor = ColorScheme.ControlsBorder;
            base.OnPaint(e);
        }

    }

}
