using Svg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;

namespace LaserGRBL.Icons
{
    public class LegacyIcons: IIconsLoader
    {
        private Dictionary<string, string> mIconList = new Dictionary<string, string>();

        public LegacyIcons()
        {
            LoadBase64();
        }

        private void LoadBase64()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            using (Stream dataStream = assembly.GetManifestResourceStream($"LaserGRBL.Icons.LegacyIcons.data"))
            using (Stream headerStream = assembly.GetManifestResourceStream($"LaserGRBL.Icons.LegacyIcons.header"))
            using (StreamReader dataReader = new StreamReader(dataStream))
            using (StreamReader headerReader = new StreamReader(headerStream))
            {
                while (!headerReader.EndOfStream && !dataReader.EndOfStream)
                {
                    mIconList.Add($"{headerReader.ReadLine()}", dataReader.ReadLine());
                }
            }
        }

        public Bitmap LoadImage(string resourceName)
        {
            if (mIconList.TryGetValue(resourceName, out string base64))
            {
                byte[] imageBytes = Convert.FromBase64String(base64);
                MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
                Bitmap bmp = new Bitmap(ms);
                return bmp;
            }
            return new Bitmap(1, 1);
        }

        public bool Contains(string key) => mIconList.ContainsKey(key);

    }
}
