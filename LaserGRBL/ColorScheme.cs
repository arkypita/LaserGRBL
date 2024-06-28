//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System.Collections.Generic;
using System.Drawing;
using static LaserGRBL.ColorScheme;

namespace LaserGRBL
{

	public interface IColorScheme
    {
        Scheme Scheme { get; }
        bool IsDark { get; }
        Color FormBackColor { get; }
        Color FormForeColor { get; }
        Color PreviewBackColor { get; }
        Color PreviewText { get; }
        Color PreviewRuler { get; }
        Color PreviewGrid { get; }
        Color PreviewGridMinor { get; }
        Color PreviewJobRange { get; }
        Color PreviewFirstMovement { get; }
        Color PreviewOtherMovement { get; }
        Color PreviewLaserPower { get; }
        Color PreviewCross { get; }
        Color PreviewCommandOK { get; }
        Color PreviewCommandKO { get; }
        Color PreviewCommandWait { get; }
        Color PreviewCrossCursor { get; }
        Color LogBackColor { get; }
        Color LogLeftCOMMAND { get; }
        Color LogLeftSTARTUP { get; }
        Color LogLeftALARM { get; }
        Color LogLeftCONFIG { get; }
        Color LogLeftFEEDBACK { get; }
        Color LogLeftPOSITION { get; }
        Color LogLeftOTHERS { get; }
        Color LogRightGOOD { get; }
        Color LogRightBAD { get; }
        Color LogRightOTHERS { get; }
        Color TextBoxColorOverride { get; }
        Color LinkColor { get; }
        Color VisitedLinkColor { get; }
        Color ControlsBorder { get; }
        Color ControlsBackDisabled { get; }
        Color DisabledButtons { get; }
        Color PressedButtons { get; }
    }

	// CAD style color scheme
    public class SchemeCADStyle: IColorScheme
    {
        public Scheme Scheme => Scheme.CADStyle;
        public bool IsDark => false;
        public Color FormBackColor => Color.FromArgb(248, 248, 248);
        public Color FormForeColor => SystemColors.ControlText;
        public Color PreviewBackColor => Color.FromArgb(33, 40, 48);
        public Color PreviewText => Color.FromArgb(220, 220, 220);
        public Color PreviewRuler => Color.FromArgb(69, 78, 101);
        public Color PreviewGrid => Color.FromArgb(49, 55, 70);
        public Color PreviewGridMinor => Color.FromArgb(38, 45, 55);
        public Color PreviewJobRange => Color.FromArgb(200, 200, 140);
        public Color PreviewFirstMovement => Color.Blue;
        public Color PreviewOtherMovement => Color.FromArgb(60, 82, 85);
        public Color PreviewLaserPower => Color.FromArgb(220, 220, 220);
        public Color PreviewCross => Color.FromArgb(230, 230, 20);
        public Color PreviewCommandOK => Color.FromArgb(55, 199, 116);
        public Color PreviewCommandKO => Color.FromArgb(240, 50, 50);
        public Color PreviewCommandWait => Color.LightPink;
        public Color PreviewCrossCursor => Color.FromArgb(103, 107, 117);
        public Color LogBackColor => Color.White;
        public Color LogLeftCOMMAND => Color.Black;
        public Color LogLeftSTARTUP => Color.DarkGreen;
        public Color LogLeftALARM => Color.Crimson;
        public Color LogLeftCONFIG => Color.DimGray;
        public Color LogLeftFEEDBACK => Color.DodgerBlue;
        public Color LogLeftPOSITION => Color.OrangeRed;
        public Color LogLeftOTHERS => Color.Purple;
        public Color LogRightGOOD => Color.DarkBlue;
        public Color LogRightBAD => Color.Red;
        public Color LogRightOTHERS => Color.Black;
        public Color TextBoxColorOverride => Color.Black;
        public Color LinkColor => Color.DodgerBlue;
        public Color VisitedLinkColor => Color.Purple;
        public Color ControlsBorder => Color.FromArgb(220, 220, 220);
        public Color ControlsBackDisabled => Color.FromArgb(180, 180, 180);
        public Color DisabledButtons => Color.FromArgb(140, 140, 140);
        public Color PressedButtons => Color.FromArgb(222, 230,  7);
    }

	// CAD dark color scheme
	public class SchemeCADDark: IColorScheme
    {
        public Scheme Scheme => Scheme.CADDark;
        public bool IsDark => true;
        public Color FormBackColor => Color.FromArgb(35, 40, 51);
        public Color FormForeColor => Color.FromArgb(190, 190, 190);
        public Color PreviewBackColor => Color.FromArgb(23, 30, 38);
        public Color PreviewText => Color.FromArgb(220, 220, 220);
        public Color PreviewRuler => Color.FromArgb(69, 78, 101);
        public Color PreviewGrid => Color.FromArgb(39, 45, 60);
        public Color PreviewGridMinor => Color.FromArgb(28, 35, 45);
        public Color PreviewJobRange => Color.FromArgb(200, 200, 140);
        public Color PreviewFirstMovement => Color.Blue;
        public Color PreviewOtherMovement => Color.FromArgb(60, 82, 85);
        public Color PreviewLaserPower => Color.FromArgb(220, 220, 220);
        public Color PreviewCross => Color.FromArgb(230, 230, 20);
        public Color PreviewCommandOK => Color.FromArgb(55, 199, 116);
        public Color PreviewCommandKO => Color.FromArgb(240, 50, 50);
        public Color PreviewCommandWait => Color.LightPink;
        public Color PreviewCrossCursor => Color.FromArgb(103, 107, 117);
        public Color LogBackColor => Color.FromArgb(70, 79, 92);
        public Color LogLeftCOMMAND => Color.White;
        public Color LogLeftSTARTUP => Color.FromArgb(121, 214, 58);
        public Color LogLeftALARM => Color.Crimson;
        public Color LogLeftCONFIG => Color.LightGray;
        public Color LogLeftFEEDBACK => Color.DodgerBlue;
        public Color LogLeftPOSITION => Color.OrangeRed;
        public Color LogLeftOTHERS => Color.Pink;
        public Color LogRightGOOD => Color.DarkBlue;
        public Color LogRightBAD => Color.Red;
        public Color LogRightOTHERS => Color.Black;
        public Color TextBoxColorOverride => Color.Black;
        public Color LinkColor => Color.Pink;
        public Color VisitedLinkColor => Color.Purple;
        public Color ControlsBorder => Color.FromArgb(64, 67, 85);
        public Color ControlsBackDisabled => Color.FromArgb(180, 180, 180);
        public Color DisabledButtons => Color.FromArgb(85, 90, 105);
        public Color PressedButtons => Color.FromArgb(84, 96, 122);
    }

	// blue laser color scheme
	public class SchemeBlueLaser: IColorScheme
    {
        public Scheme Scheme => Scheme.BlueLaser;
        public bool IsDark => false;
        public Color FormBackColor => SystemColors.Control;
        public Color FormForeColor => SystemColors.ControlText;
        public Color PreviewBackColor => Color.LightYellow;
        public Color PreviewText => Color.Black;
        public Color PreviewRuler => Color.DarkGray;
        public Color PreviewGrid => Color.FromArgb(242, 242, 200);
		public Color PreviewGridMinor => Color.FromArgb(248, 248, 220);
		public Color PreviewJobRange => Color.DarkGray;
        public Color PreviewFirstMovement => Color.Blue;
        public Color PreviewOtherMovement => Color.LightGray;
        public Color PreviewLaserPower => Color.Red;
        public Color PreviewCross => Color.Blue;
        public Color PreviewCommandOK => Color.DarkGreen;
        public Color PreviewCommandKO => Color.DarkRed;
        public Color PreviewCommandWait => Color.LightPink;
        public Color PreviewCrossCursor => Color.FromArgb(184, 184, 184);
		public Color LogBackColor => Color.White;
        public Color LogLeftCOMMAND => Color.Black;
        public Color LogLeftSTARTUP => Color.DarkGreen;
        public Color LogLeftALARM => Color.Crimson;
        public Color LogLeftCONFIG => Color.DimGray;
        public Color LogLeftFEEDBACK => Color.DodgerBlue;
        public Color LogLeftPOSITION => Color.OrangeRed;
        public Color LogLeftOTHERS => Color.Purple;
        public Color LogRightGOOD => Color.DarkBlue;
        public Color LogRightBAD => Color.Red;
        public Color LogRightOTHERS => Color.Black;
        public Color TextBoxColorOverride => Color.Black;
        public Color LinkColor => Color.DodgerBlue;
        public Color VisitedLinkColor => Color.Purple;
        public Color ControlsBorder => Color.FromArgb(220, 220, 220);
		public Color ControlsBackDisabled => Color.FromArgb(180, 180, 180);
		public Color DisabledButtons => Color.FromArgb(70, 70, 70);
		public Color PressedButtons => Color.Crimson;
    }

	// red laser color scheme
	public class SchemeRedLaser: IColorScheme
    {
        public Scheme Scheme => Scheme.RedLaser;
        public bool IsDark => false;
        public Color FormBackColor => SystemColors.Control;
        public Color FormForeColor => SystemColors.ControlText;
        public Color PreviewBackColor => Color.LightYellow;
        public Color PreviewText => Color.Black;
        public Color PreviewRuler => Color.DarkGray;
        public Color PreviewGrid => Color.FromArgb(242, 242, 200);
		public Color PreviewGridMinor => Color.FromArgb(248, 248, 220);
		public Color PreviewJobRange => Color.DarkGray;
        public Color PreviewFirstMovement => Color.Blue;
        public Color PreviewOtherMovement => Color.LightGray;
        public Color PreviewLaserPower => Color.DarkBlue;
        public Color PreviewCross => Color.DarkViolet;
        public Color PreviewCommandOK => Color.DarkGreen;
        public Color PreviewCommandKO => Color.DarkRed;
        public Color PreviewCommandWait => Color.LightBlue;
        public Color PreviewCrossCursor => Color.FromArgb(184, 184, 184);
		public Color LogBackColor => Color.White;
        public Color LogLeftCOMMAND => Color.Black;
        public Color LogLeftSTARTUP => Color.DarkGreen;
        public Color LogLeftALARM => Color.Crimson;
        public Color LogLeftCONFIG => Color.DimGray;
        public Color LogLeftFEEDBACK => Color.DodgerBlue;
        public Color LogLeftPOSITION => Color.OrangeRed;
        public Color LogLeftOTHERS => Color.Purple;
        public Color LogRightGOOD => Color.DarkGreen;
        public Color LogRightBAD => Color.Red;
        public Color LogRightOTHERS => Color.Black;
        public Color TextBoxColorOverride => Color.Black;
        public Color LinkColor => Color.DodgerBlue;
        public Color VisitedLinkColor => Color.Purple;
        public Color ControlsBorder => Color.FromArgb(220, 220, 220);
		public Color ControlsBackDisabled => Color.FromArgb(180, 180, 180);
		public Color DisabledButtons => Color.FromArgb(100, 100, 100);
		public Color PressedButtons => Color.Crimson;
    }

	// dark color scheme
	public class SchemeDark: IColorScheme
    {
        public Scheme Scheme => Scheme.Dark;
        public bool IsDark => true;
        public Color FormBackColor => Color.FromArgb(29, 44, 75);
        public Color FormForeColor => Color.White;
        public Color PreviewBackColor => Color.FromArgb(220, 220, 220);
		public Color PreviewText => Color.Black;
        public Color PreviewRuler => Color.DarkGray;
        public Color PreviewGrid => Color.FromArgb(210, 210, 210);
		public Color PreviewGridMinor => Color.FromArgb(220, 220, 220);
		public Color PreviewJobRange => Color.DimGray;
        public Color PreviewFirstMovement => Color.Blue;
        public Color PreviewOtherMovement => Color.FromArgb(180, 118, 0);
		public Color PreviewLaserPower => Color.Red;
        public Color PreviewCross => Color.DarkMagenta;
        public Color PreviewCommandOK => Color.DarkGreen;
        public Color PreviewCommandKO => Color.DarkRed;
        public Color PreviewCommandWait => Color.LightBlue;
        public Color PreviewCrossCursor => Color.Gray;
        public Color LogBackColor => Color.FromArgb(220, 220, 220);
		public Color LogLeftCOMMAND => Color.Black;
        public Color LogLeftSTARTUP => Color.DarkGreen;
        public Color LogLeftALARM => Color.DarkRed;
        public Color LogLeftCONFIG => Color.DarkSlateGray;
        public Color LogLeftFEEDBACK => Color.DarkBlue;
        public Color LogLeftPOSITION => Color.OrangeRed;
        public Color LogLeftOTHERS => Color.Purple;
        public Color LogRightGOOD => Color.Lime;
        public Color LogRightBAD => Color.OrangeRed;
        public Color LogRightOTHERS => Color.White;
        public Color TextBoxColorOverride => Color.White;
        public Color LinkColor => Color.Yellow;
        public Color VisitedLinkColor => Color.Violet;
        public Color ControlsBorder => Color.FromArgb(39, 54, 85);
		public Color ControlsBackDisabled => Color.FromArgb(180, 180, 180);
		public Color DisabledButtons => Color.FromArgb(100, 100, 100);
		public Color PressedButtons => Color.Crimson;
    }

	// hacker color scheme
	public class SchemeHacker: IColorScheme
    {
        public Scheme Scheme => Scheme.Hacker;
        public bool IsDark => true;
        public Color FormBackColor => Color.FromArgb(0, 10, 35);
        public Color FormForeColor => Color.LimeGreen;
        public Color PreviewBackColor => Color.FromArgb(220, 220, 220);
        public Color PreviewText => Color.Black;
        public Color PreviewRuler => Color.DarkGray;
        public Color PreviewGrid => Color.FromArgb(210, 210, 210);
        public Color PreviewGridMinor => Color.FromArgb(220, 220, 220);
        public Color PreviewJobRange => Color.DimGray;
        public Color PreviewFirstMovement => Color.Blue;
        public Color PreviewOtherMovement => Color.FromArgb(180, 118, 0);
        public Color PreviewLaserPower => Color.DarkGreen;
        public Color PreviewCross => Color.DarkMagenta;
        public Color PreviewCommandOK => Color.DarkBlue;
        public Color PreviewCommandKO => Color.OrangeRed;
        public Color PreviewCommandWait => Color.Pink;
        public Color PreviewCrossCursor => Color.Gray;
        public Color LogBackColor => Color.FromArgb(20, 20, 20);
        public Color LogLeftCOMMAND => Color.LimeGreen;
        public Color LogLeftSTARTUP => Color.Pink;
        public Color LogLeftALARM => Color.Red;
        public Color LogLeftCONFIG => Color.LightGray;
        public Color LogLeftFEEDBACK => Color.LightBlue;
        public Color LogLeftPOSITION => Color.OrangeRed;
        public Color LogLeftOTHERS => Color.Pink;
        public Color LogRightGOOD => Color.LightBlue;
        public Color LogRightBAD => Color.Red;
        public Color LogRightOTHERS => Color.White;
        public Color TextBoxColorOverride => Color.White;
        public Color LinkColor => Color.Yellow;
        public Color VisitedLinkColor => Color.Violet;
        public Color ControlsBorder => Color.FromArgb(15, 25, 50);
        public Color ControlsBackDisabled => Color.FromArgb(180, 180, 180);
        public Color DisabledButtons => Color.FromArgb(100, 100, 100);
        public Color PressedButtons => Color.Crimson;
    }

    // nighty color scheme
    public class SchemeNighty : IColorScheme
    {
        public Scheme Scheme => Scheme.Nighty;
        public bool IsDark => true;
        public Color FormBackColor => Color.FromArgb(25, 25, 25);
        public Color FormForeColor => Color.Aqua;
        public Color PreviewBackColor => Color.FromArgb(25, 25, 25);
        public Color PreviewText => Color.Aqua;
        public Color PreviewRuler => Color.Aqua;
        public Color PreviewGrid => Color.FromArgb(34, 34, 34);
        public Color PreviewGridMinor => Color.FromArgb(28, 28, 28);
        public Color PreviewJobRange => Color.LightPink;
        public Color PreviewFirstMovement => Color.DarkOrange;
        public Color PreviewOtherMovement => Color.FromArgb(150, 0, 120);
        public Color PreviewLaserPower => Color.FromArgb(0, 125, 140);
        public Color PreviewCross => Color.Pink;
        public Color PreviewCommandOK => Color.DarkGreen;
        public Color PreviewCommandKO => Color.DarkRed;
        public Color PreviewCommandWait => Color.LightBlue;
        public Color PreviewCrossCursor => Color.Gray;
        public Color LogBackColor => Color.FromArgb(25, 25, 25);
        public Color LogLeftCOMMAND => Color.Aqua;
        public Color LogLeftSTARTUP => Color.FromArgb(220, 30, 220);
        public Color LogLeftALARM => Color.FromArgb(0, 127, 139);
        public Color LogLeftCONFIG => Color.MediumVioletRed;
        public Color LogLeftFEEDBACK => Color.MediumVioletRed;
        public Color LogLeftPOSITION => Color.MediumPurple;
        public Color LogLeftOTHERS => Color.Purple;
        public Color LogRightGOOD => Color.DarkGreen;
        public Color LogRightBAD => Color.Red;
        public Color LogRightOTHERS => Color.LimeGreen;
        public Color TextBoxColorOverride => Color.LimeGreen;
        public Color LinkColor => Color.Yellow;
        public Color VisitedLinkColor => Color.Violet;
        public Color ControlsBorder => Color.FromArgb(40, 40, 40);
        public Color ControlsBackDisabled => Color.FromArgb(180, 180, 180);
        public Color DisabledButtons => Color.FromArgb(100, 100, 100);
        public Color PressedButtons => Color.Crimson;
    }

    public class ColorScheme
    {
        public enum Scheme
        {
            CADStyle,
            CADDark,
            BlueLaser,
            RedLaser,
            Dark,
            Hacker,
            Nighty
        }

        private static Dictionary<Scheme, IColorScheme> mDefaultSchemas;

		static ColorScheme()
        {
            mDefaultSchemas = new Dictionary<Scheme, IColorScheme>();
            AddSchema(new SchemeCADStyle());
            AddSchema(new SchemeCADDark());
            AddSchema(new SchemeBlueLaser());
            AddSchema(new SchemeRedLaser());
            AddSchema(new SchemeDark());
            AddSchema(new SchemeHacker());
            AddSchema(new SchemeNighty());
            CurrentScheme = Scheme.RedLaser;
		}

        public static void AddSchema(IColorScheme colorSchema)
        {
            mDefaultSchemas.Add(colorSchema.Scheme, colorSchema);
        }

		public static Scheme CurrentScheme { get; set; }

        private static IColorScheme CurrentSchemeColors => mDefaultSchemas[CurrentScheme];

        public static bool DarkScheme => CurrentSchemeColors.IsDark;

        public static Color FormBackColor => CurrentSchemeColors.FormBackColor;
        public static Color FormForeColor => CurrentSchemeColors.FormForeColor;
        public static Color PreviewBackColor => CurrentSchemeColors.PreviewBackColor;
        public static Color PreviewText => CurrentSchemeColors.PreviewText;
        public static Color PreviewRuler => CurrentSchemeColors.PreviewRuler;
        public static Color PreviewGrid => CurrentSchemeColors.PreviewGrid;
        public static Color PreviewGridMinor => CurrentSchemeColors.PreviewGridMinor;
        public static Color PreviewJobRange => CurrentSchemeColors.PreviewJobRange;
        public static Color PreviewFirstMovement => CurrentSchemeColors.PreviewFirstMovement;
        public static Color PreviewOtherMovement => CurrentSchemeColors.PreviewOtherMovement;
        public static Color PreviewLaserPower => CurrentSchemeColors.PreviewLaserPower;
        public static Color PreviewCross => CurrentSchemeColors.PreviewCross;
        public static Color PreviewCommandOK => CurrentSchemeColors.PreviewCommandOK;
        public static Color PreviewCommandKO => CurrentSchemeColors.PreviewCommandKO;
        public static Color PreviewCommandWait => CurrentSchemeColors.PreviewCommandWait;
        public static Color PreviewCrossCursor => CurrentSchemeColors.PreviewCrossCursor;
        public static Color LogBackColor => CurrentSchemeColors.LogBackColor;
        public static Color LogLeftCOMMAND => CurrentSchemeColors.LogLeftCOMMAND;
        public static Color LogLeftSTARTUP => CurrentSchemeColors.LogLeftSTARTUP;
        public static Color LogLeftALARM => CurrentSchemeColors.LogLeftALARM;
        public static Color LogLeftCONFIG => CurrentSchemeColors.LogLeftCONFIG;
        public static Color LogLeftFEEDBACK => CurrentSchemeColors.LogLeftFEEDBACK;
        public static Color LogLeftPOSITION => CurrentSchemeColors.LogLeftPOSITION;
        public static Color LogLeftOTHERS => CurrentSchemeColors.LogLeftOTHERS;
        public static Color LogRightGOOD => CurrentSchemeColors.LogRightGOOD;
        public static Color LogRightBAD => CurrentSchemeColors.LogRightBAD;
        public static Color LogRightOTHERS => CurrentSchemeColors.LogRightOTHERS;
        public static Color TextBoxColorOverride => CurrentSchemeColors.TextBoxColorOverride;
		public static Color LinkColor => CurrentSchemeColors.LinkColor;
        public static Color VisitedLinkColor => CurrentSchemeColors.VisitedLinkColor;
        public static Color ControlsBorder => CurrentSchemeColors.ControlsBorder;
        public static Color ControlsBackDisabled => CurrentSchemeColors.ControlsBackDisabled;
        public static Color DisabledButtons => CurrentSchemeColors.DisabledButtons;
        public static Color PressedButtons => CurrentSchemeColors.PressedButtons;

		public static Color ChangeColorBrightness(Color color, float correctionFactor)
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

    }
}
