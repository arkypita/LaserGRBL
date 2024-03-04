using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LaserGRBL.UserControls
{
    public interface IGrblPanel
    {
        void SetComProgram(GrblCore core);
        void TimerUpdate();
        void OnColorChange();

    }
}
