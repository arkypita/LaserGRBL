using Svg;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;

namespace LaserGRBL.SvgIcons
{
    public static class SvgIcons
    {
        private static Dictionary<string, string> mIconList = new Dictionary<string, string>();

        public static void Initialize()
        {
            LoadSvg("Mdi");
            LoadSvg("Custom");
            LoadSvg("Flags");
        }

        private static void LoadSvg(string fileName)
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


        public static Bitmap LoadImage(string resourceName, int width = 0, int height = 0) {
            if (mIconList.TryGetValue(resourceName, out string svgData))
            {
                var doc = SvgDocument.FromSvg<Svg.SvgDocument>(svgData);
                Bitmap bmp = doc.Draw(width, height);
                return bmp;
            }
            return null;
        }

        public static bool Contains(string key) => mIconList.ContainsKey(key);

    }

}
