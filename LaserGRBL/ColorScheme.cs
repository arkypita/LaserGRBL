using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace LaserGRBL
{
	class ColorScheme
	{
		public enum Scheme
		{ BlueLaser, RedLaser, Dark, Hacker }

		public static Dictionary<Scheme, Color[]> mData;

		static ColorScheme()
		{
			mData = new Dictionary<Scheme, Color[]>();
			mData.Add(Scheme.BlueLaser, new Color[] 
			{
				SystemColors.Control,		//form backcolor
				SystemColors.ControlText,	//form forecolor

				Color.LightYellow,			//preview background
				Color.Black,				//preview text
				Color.Empty,				//preview grid?
				Color.LightGray,			//preview reference line
				Color.Blue,					//preview first line
				Color.LightGray,			//preview other line
				Color.Red,					//preview laser
				Color.Blue,					//preview cross position

				Color.White,				//log background
				Color.Black,				//command text
				Color.DarkGreen,			//startup
				Color.Crimson,				//alarm
				Color.DimGray,				//config
				Color.DodgerBlue,			//feedback
				Color.OrangeRed,			//position
				Color.Purple,				//others
				
				Color.DarkBlue,				//response good
				Color.Red,					//response bad
				Color.Black,				//response others
			});
			mData.Add(Scheme.RedLaser, new Color[] 
			{
				SystemColors.Control,		//form backcolor
				SystemColors.ControlText,	//form forecolor

				Color.LightYellow,			//preview background
				Color.Black,				//preview text
				Color.Empty,				//preview grid?
				Color.LightGray,			//preview reference line
				Color.Blue,					//preview first line
				Color.LightGray,			//preview other line
				Color.DarkBlue,				//preview laser
				Color.DarkViolet,			//preview cross position

				Color.White,				//log background
				Color.Black,				//command text
				Color.DarkGreen,			//startup
				Color.Crimson,				//alarm
				Color.DimGray,				//config
				Color.DodgerBlue,			//feedback
				Color.OrangeRed,			//position
				Color.Purple,				//others
				
				Color.DarkGreen,				//response good
				Color.Red,					//response bad
				Color.Black,				//response others
			});
			mData.Add(Scheme.Dark, new Color[] 
			{
				Color.FromArgb(29,44,75),	//form backcolor
				Color.White,				//form forecolor

				Color.FromArgb(220,220,220),//preview background
				Color.Black,				//preview text
				Color.Empty,				//preview grid?
				Color.DimGray,				//preview reference line
				Color.Blue,					//preview first line
				Color.FromArgb(180,118,0),  //preview other line
				Color.Red,					//preview laser
				Color.DarkMagenta,			//preview cross position

				Color.DimGray,				//log background
				Color.White,				//command text
				Color.Lime,					//startup
				Color.Red,					//alarm
				Color.LightGray,			//config
				Color.LightBlue,			//feedback
				Color.OrangeRed,			//position
				Color.Pink,					//others
				
				Color.Lime,					//response good
				Color.OrangeRed,			//response bad
				Color.White,				//response others
			});
			mData.Add(Scheme.Hacker, new Color[] 
			{
				Color.FromArgb(0,10,35),	//form backcolor
				Color.LimeGreen,			//form forecolor

				Color.FromArgb(220,220,220),//preview background
				Color.Black,				//preview text
				Color.Empty,				//preview grid?
				Color.DimGray,				//preview reference line
				Color.Blue,					//preview first line
				Color.FromArgb(180,118,0),  //preview other line
				Color.DarkGreen,			//preview laser
				Color.DarkMagenta,			//preview cross position

				Color.FromArgb(20,20,20),	//log background
				Color.LimeGreen,			//command text
				Color.Pink,					//startup
				Color.Red,					//alarm
				Color.LightGray,			//config
				Color.LightBlue,			//feedback
				Color.OrangeRed,			//position
				Color.Pink,					//others
				
				Color.LightBlue,			//response good
				Color.Red,					//response bad
				Color.White,				//response others
			});
			
			CurrentScheme = Scheme.RedLaser;
		}

		public static bool DarkScheme
		{ get { return CurrentScheme == Scheme.Dark || CurrentScheme == Scheme.Hacker; } }

		public static Scheme CurrentScheme { get; set; }

		public static Color FormBackColor
		{ get { return GetColor(0); } }
		public static Color FormForeColor
		{ get { return GetColor(1); } }

		public static Color FormButtonsColor
		{ 
			get 
			{
				if (DarkScheme)
					return ChangeColorBrightness(FormBackColor, +0.1f);
				else
					return ChangeColorBrightness(FormBackColor, -0.1f); 
			} 
		}

		public static Color MenuHighlightColor
		{ 
			get 
			{
				if (DarkScheme)
					return ChangeColorBrightness(FormBackColor, +0.2f); 
				else
					return ChangeColorBrightness(FormBackColor, -0.1f); 
			} 
		}

		public static Color MenuSeparatorColor
		{
			get 
			{
				if (DarkScheme)
					return ChangeColorBrightness(FormBackColor, +0.15f);
				else
					return ChangeColorBrightness(FormBackColor, -0.1f); 
			} 
		}

		public static Color PreviewBackColor
		{ get { return GetColor(2); } }
		public static Color PreviewText
		{ get { return GetColor(3); } }
		public static Color PreviewGrid
		{ get { return GetColor(4); } }
		public static Color PreviewJobRange
		{ get { return GetColor(5); } }
		public static Color PreviewFirstMovement
		{ get { return GetColor(6); } }
		public static Color PreviewOtherMovement
		{ get { return GetColor(7); } }
		public static Color PreviewLaserPower
		{ get { return GetColor(8); } }
		public static Color PreviewCross
		{ get { return GetColor(9); } }

		public static Color LogBackColor
		{ get { return GetColor(10); } }
		public static Color LogLeftCOMMAND
		{ get { return GetColor(11); } }
		public static Color LogLeftSTARTUP
		{ get { return GetColor(12); } }
		public static Color LogLeftALARM
		{ get { return GetColor(13); } }
		public static Color LogLeftCONFIG
		{ get { return GetColor(14); } }
		public static Color LogLeftFEEDBACK
		{ get { return GetColor(15); } }
		public static Color LogLeftPOSITION
		{ get { return GetColor(16); } }
		public static Color LogLeftOTHERS
		{ get { return GetColor(17); } }
		public static Color LogRightGOOD
		{ get { return GetColor(18); } }
		public static Color LogRightBAD
		{ get { return GetColor(19); } }
		public static Color LogRightOTHERS
		{ get { return GetColor(20); } }
		

		private static Color GetColor(int index)
		{return mData[CurrentScheme][index];}

		private static Color ChangeColorBrightness(Color color, float correctionFactor)
		{
			float red = (float)color.R;
			float green = (float)color.G;
			float blue = (float)color.B;

			if (correctionFactor < 0)
			{
				correctionFactor = 1 + correctionFactor;
				red *= correctionFactor;
				green *= correctionFactor;
				blue *= correctionFactor;
			}
			else
			{
				red = (255 - red) * correctionFactor + red;
				green = (255 - green) * correctionFactor + green;
				blue = (255 - blue) * correctionFactor + blue;
			}

			return Color.FromArgb(color.A, (int)red, (int)green, (int)blue);
		}


	}
}


		//public Color LeftColor
		//{ get { return Color.Black; } }

		//public Color RightColor
		//{ get { return Status == CommandStatus.ResponseGood ? Color.DarkBlue : Status == CommandStatus.ResponseBad ? Color.Red : Color.Black; } }


		//public Color LeftColor
		//{
		//	get 
		//	{
		//		if (mType == MessageType.Startup)
		//			return Color.DarkGreen;
		//		else if (mType == MessageType.Alarm)
		//			return Color.Crimson;
		//		else if (mType == MessageType.Config)
		//			return Color.DimGray;
		//		else if (mType == MessageType.Feedback)
		//			return Color.DodgerBlue;
		//		else if (mType == MessageType.Position)
		//			return Color.OrangeRed;
		//		else if (mType == MessageType.Others)
		//			return Color.Purple;
		//		else
		//			return Color.Purple;
		//	}
		//}