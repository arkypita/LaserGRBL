using LaserGRBL.Icons;
using Svg;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace LaserGRBL.SvgIcons
{
    public class SvgIcons: IIconsLoader
    {
        private Dictionary<string, string> mIconList = new Dictionary<string, string>();

        public SvgIcons()
        {
            LoadSvg("Mdi");
            LoadSvg("Custom");
            LoadSvg("Flags");
        }

        private void LoadSvg(string fileName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream dataStream = assembly.GetManifestResourceStream($"LaserGRBL.SvgIcons.{fileName}.data"))
            using (Stream headerStream = assembly.GetManifestResourceStream($"LaserGRBL.SvgIcons.{fileName}.header"))
            using (StreamReader dataReader = new StreamReader(dataStream))
            using (StreamReader headerReader = new StreamReader(headerStream))
            {
                while (!headerReader.EndOfStream && !dataReader.EndOfStream)
                {
                    mIconList.Add($"{fileName.ToLower()}-{headerReader.ReadLine()}", dataReader.ReadLine());
                }
            }
        }

        public Bitmap LoadImage(string resourceName) {
            if (mIconList.TryGetValue(resourceName, out string svgData))
            {
                var doc = SvgDocument.FromSvg<SvgDocument>(svgData);
                Bitmap bmp = doc.Draw(256, 256);
                return bmp;
            }
            return null;
        }

        public bool Contains(string key) => mIconList.ContainsKey(key);

    }

}
