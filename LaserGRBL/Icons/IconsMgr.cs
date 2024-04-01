using Base.Drawing;
using LaserGRBL.UserControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace LaserGRBL.Icons
{
    public static class IconsMgr
    {
        // buttons list
        private static List<object> mControls = new List<object>();
        // light color set
        private static Dictionary<string, Color> mLightIconColors = new Dictionary<string, Color>
        {
            { "custom-reset"            , Color.FromArgb(199,193, 13) },
            { "unlock"           , Color.FromArgb(230,138,  7) },
            { "zeroing"          , Color.FromArgb( 14,136,229) },
            { "center"           , Color.FromArgb( 19,169,142) },
            { "corner"           , Color.FromArgb( 19,169,142) },
            { "frame"            , Color.FromArgb(  0,173, 16) },
            { "focus"            , Color.FromArgb(197, 60,221) },
            { "blink"            , Color.FromArgb(231, 70,143) },
            { "mdi-home"             , Color.FromArgb( 79,188,243) },
            { "mdi-arrow-up-bold-outline"    , Color.FromArgb(  0,122,217) },
            { "mdi-arrow-top-right-bold-outline", Color.FromArgb(  0,122,217) },
            { "mdi-arrow-right-bold-outline" , Color.FromArgb(  0,122,217) },
            { "mdi-arrow-bottom-right-bold-outline"               , Color.FromArgb(  0,122,217) },
            { "mdi-arrow-down-bold-outline"  , Color.FromArgb(  0,122,217) },
            { "mdi-arrow-bottom-left-bold-outline"               , Color.FromArgb(  0,122,217) },
            { "mdi-arrow-left-bold-outline"  , Color.FromArgb(  0,122,217) },
            { "mdi-arrow-top-left-bold-outline"               , Color.FromArgb(  0,122,217) },
            { "resume"           , Color.FromArgb(  0,173, 16) },
            { "stop"             , Color.FromArgb(236, 58, 58) },
            { "mdi-play-box"              , Color.FromArgb(  0,173, 16) },
            { "custom-abort"     , Color.FromArgb(236, 58, 58) },
            { "connect"          , Color.FromArgb(  0,173, 16) },
            { "disconnect"       , Color.FromArgb(236, 58, 58) },
            { "open"             , Color.FromArgb( 88,119,232) },
            { "append"           , Color.FromArgb(  0,173, 16) },
            { "mdi-checkbox-marked"               , Color.FromArgb(  0,173, 16) },
            { "mdi-close-box"           , Color.FromArgb(236, 58, 58) },
            { "info"             , Color.FromArgb(  0,122,217) },
            { "book"             , Color.FromArgb( 19,169,142) },
            { "target"           , Color.FromArgb(  0,173, 16) },
            { "lockproportions"  , Color.FromArgb( 79,188,243) },
            { "unlockproportions", Color.FromArgb(230,138,  7) },
            { "exif"             , Color.FromArgb(  0,173, 16) },
            { "resetcenter"      , Color.FromArgb(199,193, 13) },
            { "fliphorizontal"   , Color.FromArgb( 19,169,142) },
            { "flipvertical"     , Color.FromArgb( 19,169,142) },
            { "rotateleft"       , Color.FromArgb( 19,169,142) },
            { "rotateright"      , Color.FromArgb( 19,169,142) },
            { "crop"             , Color.FromArgb( 19,169,142) },
            { "autotrim"         , Color.FromArgb( 19,169,142) },
            { "fill"             , Color.FromArgb( 19,169,142) },
            { "invert"           , Color.FromArgb( 19,169,142) },
            { "magicwand"        , Color.FromArgb( 19,169,142) },
            { "revert"           , Color.FromArgb(  0,173, 16) },
            { "mdi-heart-box"        , Color.FromArgb(236, 58, 58) },
            { "safety"           , Color.FromArgb( 79,188,243) },
            { "config"           , Color.FromArgb( 79,188,243) },
            { "settings"         , Color.FromArgb(  0,122,217) },
            { "mdi-chart-bar"            , Color.FromArgb(231, 70,143) },
            { "mdi-keyboard"         , Color.FromArgb(230,138,  7) },
            { "mdi-content-save"             , Color.FromArgb(230,138,  7) },
            { "mdi-content-save-edit"     , Color.FromArgb(230,138,  7) },
            { "mdi-content-save-cog"      , Color.FromArgb(230,138,  7) },
        };
        // dark color set
        private static Dictionary<string, Color> mDarkIconColors = new Dictionary<string, Color>
        {
            { "custom-reset"            , Color.FromArgb(230,230, 20) },
            { "custom-unlock"           , Color.FromArgb(246,163, 41) },
            { "zeroing"          , Color.FromArgb( 79,188,243) },
            { "center"           , Color.FromArgb( 85,231,192) },
            { "corner"           , Color.FromArgb( 85,231,192) },
            { "frame"            , Color.FromArgb( 71,200, 86) },
            { "focus"            , Color.FromArgb(193,114,235) },
            { "blink"            , Color.FromArgb(243, 79,133) },
            { "mdi-home"             , Color.FromArgb( 79,188,243) },
            { "mdi-arrow-up-bold-outline"    , Color.FromArgb(  0,122,217) },
            { "mdi-arrow-top-right-bold-outline", Color.FromArgb(  0,122,217) },
            { "mdi-arrow-right-bold-outline" , Color.FromArgb(  0,122,217) },
            { "mdi-arrow-bottom-right-bold-outline"               , Color.FromArgb(  0,122,217) },
            { "mdi-arrow-down-bold-outline"  , Color.FromArgb(  0,122,217) },
            { "mdi-arrow-bottom-left-bold-outline"               , Color.FromArgb(  0,122,217) },
            { "mdi-arrow-left-bold-outline"  , Color.FromArgb(  0,122,217) },
            { "mdi-arrow-top-left-bold-outline"               , Color.FromArgb(  0,122,217) },
            { "resume"           , Color.FromArgb( 71,200, 86) },
            { "custom-stop"             , Color.FromArgb(228, 60, 60) },
            { "mdi-play-box"              , Color.FromArgb( 71,200, 86) },
            { "custom-abort"     , Color.FromArgb(228, 60, 60) },
            { "connect"          , Color.FromArgb( 71,200, 86) },
            { "disconnect"       , Color.FromArgb(228, 60, 60) },
            { "open"             , Color.FromArgb( 85,231,192) },
            { "append"           , Color.FromArgb( 71,200, 86) },
            { "mdi-checkbox-marked"               , Color.FromArgb( 71,200, 86) },
            { "mdi-close-box"           , Color.FromArgb(228, 60, 60) },
            { "info"             , Color.FromArgb(  0,122,217) },
            { "book"             , Color.FromArgb( 85,231,192) },
            { "target"           , Color.FromArgb( 71,200, 86) },
            { "lockproportions"  , Color.FromArgb( 79,188,243) },
            { "unlockproportions", Color.FromArgb(246,163, 41) },
            { "exif"             , Color.FromArgb( 71,200, 86) },
            { "resetcenter"      , Color.FromArgb(230,230, 20) },
            { "fliphorizontal"   , Color.FromArgb( 85,231,192) },
            { "flipvertical"     , Color.FromArgb( 85,231,192) },
            { "rotateleft"       , Color.FromArgb( 85,231,192) },
            { "rotateright"      , Color.FromArgb( 85,231,192) },
            { "crop"             , Color.FromArgb( 85,231,192) },
            { "autotrim"         , Color.FromArgb( 85,231,192) },
            { "fill"             , Color.FromArgb( 85,231,192) },
            { "invert"           , Color.FromArgb( 85,231,192) },
            { "magicwand"        , Color.FromArgb( 85,231,192) },
            { "revert"           , Color.FromArgb( 71,200, 86) },
            { "mdi-heart-box"        , Color.FromArgb(228, 60, 60) },
            { "safety"           , Color.FromArgb( 79,188,243) },
            { "config"           , Color.FromArgb( 79,188,243) },
            { "settings"         , Color.FromArgb(  0,122,217) },
            { "mdi-chart-bar"            , Color.FromArgb(243, 79,133) },
            { "mdi-keyboard"         , Color.FromArgb(246,163, 41) },
            { "mdi-content-save"             , Color.FromArgb(246,163, 41) },
            { "mdi-content-save-edit"     , Color.FromArgb(246,163, 41) },
            { "mdi-content-save-cog"      , Color.FromArgb(246,163, 41) },
        };
        // current icon colors
        private static Dictionary<string, Color> mIconColors = mLightIconColors;

        // reload image
        private static Image LoadImage(object resourceName, int width = 0, int height = 0)
        {
            Bitmap image = null;
            if (SvgIcons.SvgIcons.Contains((string)resourceName))
            {
                image = SvgIcons.SvgIcons.LoadImage((string)resourceName, 256, 256);
            }
            else
            {
                // load image
                //Assembly assembly = Assembly.GetExecutingAssembly();
                //image = new Bitmap(assembly.GetManifestResourceStream($"LaserGRBL.Icons.{resourceName}.png"));
                image = SvgIcons.SvgIcons.LoadImage("mdi-square-rounded", 256, 256);
            }
            Image result = image;
            // if resize needed
            if (width > 0 && height > 0)
            {
                Bitmap newImage = new Bitmap(16, 16);
                using (Graphics g = Graphics.FromImage(newImage))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.DrawImage(image, 0, 0, width, height);
                }
                image.Dispose();
                result = newImage;
            }
            // colorize
            if (mIconColors.TryGetValue((string)resourceName, out Color color))
            {
                result = ImageTransform.SetColor(result, color);
            }
            // set resource link
            result.Tag = resourceName;
            return result;
        }

        // add control to list
        private static void AddControl(object control)
        {
            mControls.Add(control);
            if (control is Control ctrl)
            {
                ctrl.Disposed += ControlDisposed;
            }
        }

        // prepare button
        public static void PrepareButton(ImageButton button, string resourceName, string resourceNameAlt = null)
        {
            button.Image = LoadImage(resourceName);
            button.SizingMode = ImageButton.SizingModes.StretchImage;
            if (!string.IsNullOrEmpty(resourceNameAlt))
            {
                button.AltImage = LoadImage(resourceNameAlt);
            }
            button.SizingMode = ImageButton.SizingModes.StretchImage;
            AddControl(button);
        }

        // prepare button
        public static void PrepareButton(GrblButton button, string resourceName)
        {
            button.Image = LoadImage(resourceName);
            AddControl(button);
        }

        // prepare picturebox
        public static void PreparePictureBox(PictureBox pictureBox, string resourceName)
        {
            pictureBox.Image = LoadImage(resourceName);
            AddControl(pictureBox);
        }

        // prepare menu item
        public static void PrepareMenuItem(ToolStripMenuItem item, string resourceName)
        {
            item.Image = LoadImage(resourceName, 16, 16);
            AddControl(item);
        }

        private static void ControlDisposed(object sender, EventArgs e)
        {
            mControls.RemoveAt(mControls.IndexOf(sender as Control));
        }

        internal static void OnColorChannge()
        {
            mIconColors = ColorScheme.DarkScheme ? mDarkIconColors : mLightIconColors;
            foreach (object control in mControls)
            {
                if (control is ImageButton imageBtn)
                {
                    if (imageBtn.Image?.Tag != null) imageBtn.Image = LoadImage(imageBtn.Image.Tag);
                    if (imageBtn.AltImage?.Tag != null) imageBtn.AltImage = LoadImage(imageBtn.AltImage.Tag);
                }
                if (control is GrblButton grblBtn)
                {
                    if (grblBtn.Image != null) grblBtn.Image = LoadImage(grblBtn.Image.Tag);
                }
                if (control is PictureBox pictureBox)
                {
                    if (pictureBox.Image != null) pictureBox.Image = LoadImage(pictureBox.Image.Tag);
                }
                if (control is ToolStripMenuItem item)
                {
                    if (item.Image != null) item.Image = LoadImage(item.Image.Tag, item.Image.Width, item.Image.Height);
                }
            }

        }
    }

}
