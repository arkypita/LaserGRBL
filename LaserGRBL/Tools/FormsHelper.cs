using System.Windows.Forms;

namespace LaserGRBL
{
    public static class FormsHelper
    {
        public static Form MainForm => Application.OpenForms[0];
    }
}
