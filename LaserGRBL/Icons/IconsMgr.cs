using Base.Drawing;
using LaserGRBL.UserControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;

namespace LaserGRBL.Icons
{
    public static class IconsMgr
    {
        // buttons list
        private static Dictionary<string, ImageButton> mImageButtons = new Dictionary<string, ImageButton>();
        // light color set
        private static Dictionary<string, Color> mLightIconColors = new Dictionary<string, Color>
        {
            { "reset"     , Color.FromArgb(199,193, 13) },
            { "unlock"    , Color.FromArgb(230,138,  7) },
            { "zeroing"   , Color.FromArgb( 14,136,229) },
            { "center"    , Color.FromArgb( 19,169,142) },
            { "corner"    , Color.FromArgb( 19,169,142) },
            { "frame"     , Color.FromArgb(  0,173, 16) },
            { "focus"     , Color.FromArgb(197, 60,221) },
            { "blink"     , Color.FromArgb(231, 70,143) },
            { "home"      , Color.FromArgb( 79,188,243) },
            { "n"         , Color.FromArgb(  0,122,217) },
            { "ne"        , Color.FromArgb(  0,122,217) },
            { "e"         , Color.FromArgb(  0,122,217) },
            { "se"        , Color.FromArgb(  0,122,217) },
            { "s"         , Color.FromArgb(  0,122,217) },
            { "sw"        , Color.FromArgb(  0,122,217) },
            { "w"         , Color.FromArgb(  0,122,217) },
            { "nw"        , Color.FromArgb(  0,122,217) },
            { "resume"    , Color.FromArgb(  0,173, 16) },
            { "stop"      , Color.FromArgb(236, 58, 58) },
            { "run"       , Color.FromArgb(  0,173, 16) },
            { "abort"     , Color.FromArgb(236, 58, 58) },
            { "connect"   , Color.FromArgb(  0,173, 16) },
            { "disconnect", Color.FromArgb(236, 58, 58) },
            { "open"      , Color.FromArgb( 88,119,232) },
        };
        // dark color set
        private static Dictionary<string, Color> mDarkIconColors = new Dictionary<string, Color>
        {
            { "reset"     , Color.FromArgb(230,230, 20) },
            { "unlock"    , Color.FromArgb(246,163, 41) },
            { "zeroing"   , Color.FromArgb( 79,188,243) },
            { "center"    , Color.FromArgb( 85,231,192) },
            { "corner"    , Color.FromArgb( 85,231,192) },
            { "frame"     , Color.FromArgb( 71,200, 86) },
            { "focus"     , Color.FromArgb(193,114,235) },
            { "blink"     , Color.FromArgb(243, 79,133) },
            { "home"      , Color.FromArgb( 79,188,243) },
            { "n"         , Color.FromArgb(  0,122,217) },
            { "ne"        , Color.FromArgb(  0,122,217) },
            { "e"         , Color.FromArgb(  0,122,217) },
            { "se"        , Color.FromArgb(  0,122,217) },
            { "s"         , Color.FromArgb(  0,122,217) },
            { "sw"        , Color.FromArgb(  0,122,217) },
            { "w"         , Color.FromArgb(  0,122,217) },
            { "nw"        , Color.FromArgb(  0,122,217) },
            { "resume"    , Color.FromArgb( 71,200, 86) },
            { "stop"      , Color.FromArgb(228, 60, 60) },
            { "run"       , Color.FromArgb( 71,200, 86) },
            { "abort"     , Color.FromArgb(228, 60, 60) },
            { "connect"   , Color.FromArgb( 71,200, 86) },
            { "disconnect", Color.FromArgb(228, 60, 60) },
            { "open"      , Color.FromArgb( 85,231,192) },
        };
        // current icon colors
        private static Dictionary<string, Color> mIconColors = mLightIconColors;

        // reload image
        private static Image LoadImage(string resourceName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            Bitmap image = new Bitmap(assembly.GetManifestResourceStream($"LaserGRBL.Icons.{resourceName}.png"));
            if (mIconColors.TryGetValue(resourceName, out Color color))
            {
                return ImageTransform.SetColor(image, color);
            }
            return image;

        }

        // prepare button
        public static void PrepareButton(ImageButton button, string resourceName, string resourceNameAlt = null)
        {
            button.Image = LoadImage(resourceName);
            button.SizingMode = ImageButton.SizingModes.StretchImage;
            mImageButtons[resourceName] = button;
            if (!string.IsNullOrEmpty(resourceNameAlt))
            {
                button.AltImage = LoadImage(resourceNameAlt);
            }
            button.SizingMode = ImageButton.SizingModes.StretchImage;
        }

        internal static void OnColorChannge()
        {
            mIconColors = ColorScheme.DarkScheme ? mDarkIconColors : mLightIconColors;
            foreach (var button in mImageButtons)
            {
                button.Value.Image?.Dispose();
                button.Value.Image = LoadImage(button.Key);
            }

        }
    }

}
