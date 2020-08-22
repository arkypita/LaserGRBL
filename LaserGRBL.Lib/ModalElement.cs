using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LaserGRBL
{
	public class ModalElement : GrblElement
	{
		List<GrblElement> mOptions = new List<GrblElement>();
		GrblElement mDefault = null;
		bool mSettled = false;


		public ModalElement(GrblElement defval, params GrblElement[] options) : base(defval.Command, defval.Number)
		{
			mDefault = defval;
			mOptions.Add(defval);
			foreach (GrblElement e in options)
				mOptions.Add(e);
		}

		public bool IsDefault
		{ get { return base.Equals(mDefault); } }

		public bool IsSettled
		{ get { return mSettled; } }

		public void Update(GrblElement e)
		{
			if (e != null && mOptions.Contains(e))
			{
				mCommand = e.Command;
				mNumber = e.Number;
				mSettled = true;
			}
		}
	}

}
