using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LaserGRBL.UserControls
{
    public interface IGrblPanel
    {
        void SetCore(GrblCore core);
        void TimerUpdate();
        void OnColorChange();

    }
}
