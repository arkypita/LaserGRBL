using Base.Drawing;
using LaserGRBL.UserControls;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace LaserGRBL.Icons
{

    public class LoadedImageTag
    {
        public Color Color { get; set; }
        public bool Colorize { get; set; }
        public string ResourceName { get; set; }
        public Size Size { get; set; }

    }

    public static class IconsMgr
    {
        // icons loader
        private static IIconsLoader mIconsLoader = null;
        // buttons list
        private static List<object> mControls = new List<object>();
        // light color set
        private static Dictionary<string, Color> mLightIconColors = new Dictionary<string, Color>
        {
            { string.Empty                          , Color.FromArgb( 70,  70,  70) },
            { "custom-reset"                        , Color.FromArgb(242, 165,   1) },
            { "mdi-lightning-bolt"                  , Color.FromArgb(242, 165,   1) },
            { "mdi-arrow-up-bold-outline"           , Color.FromArgb(  0, 122, 217) },
            { "mdi-arrow-top-right-bold-outline"    , Color.FromArgb(  0, 122, 217) },
            { "mdi-arrow-right-bold-outline"        , Color.FromArgb(  0, 122, 217) },
            { "mdi-arrow-bottom-right-bold-outline" , Color.FromArgb(  0, 122, 217) },
            { "mdi-arrow-down-bold-outline"         , Color.FromArgb(  0, 122, 217) },
            { "mdi-arrow-bottom-left-bold-outline"  , Color.FromArgb(  0, 122, 217) },
            { "mdi-arrow-left-bold-outline"         , Color.FromArgb(  0, 122, 217) },
            { "mdi-arrow-top-left-bold-outline"     , Color.FromArgb(  0, 122, 217) },
            { "custom-zup01"                        , Color.FromArgb( 79, 188, 243) },
            { "custom-zup1"                         , Color.FromArgb( 79, 188, 243) },
            { "custom-zup10"                        , Color.FromArgb( 79, 188, 243) },
            { "custom-zdown01"                      , Color.FromArgb( 79, 188, 243) },
            { "custom-zdown1"                       , Color.FromArgb( 79, 188, 243) },
            { "custom-zdown10"                      , Color.FromArgb( 79, 188, 243) },
            { "custom-home"                         , Color.FromArgb( 79, 188, 243) },
            { "mdi-information-slab-box"            , Color.FromArgb( 79, 188, 243) },
            { "mdi-safety-goggles"                  , Color.FromArgb( 79, 188, 243) },
            { "mdi-play"                            , Color.FromArgb( 17, 169,  38) },
            { "mdi-power-plug"                      , Color.FromArgb( 17, 169,  38) },
            { "mdi-checkbox-marked"                 , Color.FromArgb( 17, 169,  38) },
            { "mdi-play-circle"                     , Color.FromArgb( 17, 169,  38) },
            { "custom-resume"                       , Color.FromArgb( 17, 169,  38) },
            { "mdi-heart"                           , Color.FromArgb(219,  20,  43) },
            { "mdi-stop"                            , Color.FromArgb(219,  20,  43) },
            { "mdi-power-plug-off"                  , Color.FromArgb(219,  20,  43) },
            { "mdi-close-box"                       , Color.FromArgb(219,  20,  43) },
            { "mdi-stop-circle"                     , Color.FromArgb(219,  20,  43) },
            { "custom-stop"                         , Color.FromArgb(219,  20,  43) },
        };
        // dark color set
        private static Dictionary<string, Color> mDarkIconColors = new Dictionary<string, Color>
        {
            { string.Empty                          , Color.FromArgb(210, 210, 210) },
            { "custom-reset"                        , Color.FromArgb(230, 230,  20) },
            { "mdi-lightning-bolt"                  , Color.FromArgb(230, 230,  20) },
            { "mdi-arrow-up-bold-outline"           , Color.FromArgb(  0, 122, 217) },
            { "mdi-arrow-top-right-bold-outline"    , Color.FromArgb(  0, 122, 217) },
            { "mdi-arrow-right-bold-outline"        , Color.FromArgb(  0, 122, 217) },
            { "mdi-arrow-bottom-right-bold-outline" , Color.FromArgb(  0, 122, 217) },
            { "mdi-arrow-down-bold-outline"         , Color.FromArgb(  0, 122, 217) },
            { "mdi-arrow-bottom-left-bold-outline"  , Color.FromArgb(  0, 122, 217) },
            { "mdi-arrow-left-bold-outline"         , Color.FromArgb(  0, 122, 217) },
            { "mdi-arrow-top-left-bold-outline"     , Color.FromArgb(  0, 122, 217) },
            { "custom-zup01"                        , Color.FromArgb( 79, 188, 243) },
            { "custom-zup1"                         , Color.FromArgb( 79, 188, 243) },
            { "custom-zup10"                        , Color.FromArgb( 79, 188, 243) },
            { "custom-zdown01"                      , Color.FromArgb( 79, 188, 243) },
            { "custom-zdown1"                       , Color.FromArgb( 79, 188, 243) },
            { "custom-zdown10"                      , Color.FromArgb( 79, 188, 243) },
            { "custom-home"                         , Color.FromArgb( 79, 188, 243) },
            { "mdi-information-slab-box"            , Color.FromArgb( 79, 188, 243) },
            { "mdi-safety-goggles"                  , Color.FromArgb( 79, 188, 243) },
            { "mdi-play"                            , Color.FromArgb( 71, 200,  86) },
            { "mdi-power-plug"                      , Color.FromArgb( 71, 200,  86) },
            { "mdi-checkbox-marked"                 , Color.FromArgb( 71, 200,  86) },
            { "mdi-play-circle"                     , Color.FromArgb( 71, 200,  86) },
            { "custom-resume"                       , Color.FromArgb( 71, 200,  86) },
            { "mdi-heart"                           , Color.FromArgb(248,  90,  90) },
            { "mdi-stop"                            , Color.FromArgb(248,  90,  90) },
            { "mdi-power-plug-off"                  , Color.FromArgb(248,  90,  90) },
            { "mdi-close-box"                       , Color.FromArgb(248,  90,  90) },
            { "mdi-stop-circle"                     , Color.FromArgb(248,  90,  90) },
            { "custom-stop"                         , Color.FromArgb(248,  90,  90) },
        };
        // current icon colors
        private static Dictionary<string, Color> mIconColors = mLightIconColors;

        public static bool LegacyIcons { get; private set; }
        public static IIconsLoader IconsLoader => mIconsLoader;

        public static void Initialize()
        {
            LegacyIcons = Settings.GetObject("LegacyIcons", false);
            if (LegacyIcons)
            {
                mIconsLoader = new LegacyIcons();
            }
            else
            {
                mIconsLoader = new SvgIcons.SvgIcons();
            }
        }


        // reload image
        private static Image LoadImage(object resourceName, Size size, bool colorize = true)
        {
            if (mIconsLoader == null) return null;
            string strResourceName = (string)resourceName;
            Bitmap image;
            if (mIconsLoader.Contains(strResourceName))
            {
                image = mIconsLoader.LoadImage(strResourceName);
            }
            else
            {
                if (mIconsLoader.Contains(strResourceName))
                {
                    image = mIconsLoader.LoadImage(strResourceName);
                }
                else
                {
                    image = mIconsLoader.LoadImage("mdi-square-rounded");
                }
            }
            Image result = image;
            // if resize needed
            if (!size.IsEmpty)
            {
                Bitmap newImage = new Bitmap(size.Width, size.Height);
                using (Graphics g = Graphics.FromImage(newImage))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.DrawImage(image, 0, 0, size.Width, size.Height);
                }
                image.Dispose();
                result = newImage;
            }
            // colorize
            if (colorize && mIconsLoader is SvgIcons.SvgIcons)
            {
                Color iconColor;
                if (mIconColors.TryGetValue(strResourceName, out Color color))
                {
                    result = ImageTransform.SetColor(result, color);
                    iconColor = color;
                }
                else
                {
                    result = ImageTransform.SetColor(result, mIconColors[string.Empty]);
                    iconColor = mIconColors[string.Empty];
                }
                // set resource link
                result.Tag = new LoadedImageTag
                {
                    Color = iconColor,
                    Colorize = colorize,
                    ResourceName = strResourceName,
                    Size = size
                };
            }
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
        public static void PrepareButton(ImageButton button, string resourceName, Size size, string resourceNameAlt = null)
        {
            button.Image = LoadImage(resourceName, size);
            button.SizingMode = ImageButton.SizingModes.StretchImage;
            if (!string.IsNullOrEmpty(resourceNameAlt))
            {
                button.AltImage = LoadImage(resourceNameAlt, size);
            }
            button.SizingMode = ImageButton.SizingModes.StretchImage;
            AddControl(button);
        }
        // prepare button
        public static void PrepareButton(ImageButton button, string resourceName, string resourceNameAlt = null)
        {
            PrepareButton(button, resourceName, Size.Empty, resourceNameAlt);
        }

        // prepare button
        public static void PrepareButton(GrblButton button, string resourceName)
        {
            PrepareButton(button, resourceName, Size.Empty);
        }

        // prepare button
        public static void PrepareButton(GrblButton button, string resourceName, Size size)
        {
            button.Image = LoadImage(resourceName, size);
            AddControl(button);
        }

        // prepare picturebox
        public static void PreparePictureBox(PictureBox pictureBox, string resourceName)
        {
            pictureBox.Image = LoadImage(resourceName, pictureBox.Size);
            AddControl(pictureBox);
        }

        // prepare menu item
        public static void PrepareMenuItem(ToolStripMenuItem item, string resourceName, bool colorize = true)
        {
            item.Image = LoadImage(resourceName, new Size(16, 16), colorize);
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
                if (control is Control ctrl)
                {
                    ctrl.Invalidate();
                }
                if (control is ImageButton imageBtn)
                {
                    imageBtn.Image = ReloadLoadImage(imageBtn.Image);
                    imageBtn.AltImage = ReloadLoadImage(imageBtn.AltImage);
                }
                else if (control is GrblButton grblBtn)
                {
                    grblBtn.Image = ReloadLoadImage(grblBtn.Image);
                }
                else if (control is PictureBox pictureBox)
                {
                    pictureBox.Image = ReloadLoadImage(pictureBox.Image);
                }
                else if (control is ToolStripMenuItem item)
                {
                    item.Image = ReloadLoadImage(item.Image);
                }
            }
        }

        private static Image ReloadLoadImage(Image image)
        {
            if (image != null)
            {
                LoadedImageTag tag = image.Tag as LoadedImageTag;
                if (tag != null)
                {
                    image.Dispose();
                    image = LoadImage(tag.ResourceName, tag.Size, tag.Colorize);
                }
            }
            return image;
        }
    }

}
