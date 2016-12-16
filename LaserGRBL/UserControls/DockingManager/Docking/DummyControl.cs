using System;
using System.Windows.Forms;

namespace LaserGRBL.UserControls.DockingManager
{
    internal class DummyControl : Control
    {
        public DummyControl()
        {
            SetStyle(ControlStyles.Selectable, false);
        }
    }
}
