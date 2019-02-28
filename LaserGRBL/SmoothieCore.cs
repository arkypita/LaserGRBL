using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LaserGRBL
{
    public class SmoothieCore : GrblCore
    {
        public SmoothieCore(System.Windows.Forms.Control syncroObject, PreviewForm cbform) : base(syncroObject, cbform)
        {
        }

        protected override void InitializeBoard()
        {
            SendImmediate(10); //send a new line
            base.InitializeBoard();
        }

        protected override void ParseF(string p)
        {
            string sfs = p.Substring(2, p.Length - 2);
            string[] fs = sfs.Split(",".ToCharArray());
            SetFS(ParseFloat(fs[0]), ParseFloat(fs[1]));
        }

        public override bool SupportJogging => false;
    }

}
