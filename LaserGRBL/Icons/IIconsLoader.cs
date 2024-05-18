using System.Drawing;

namespace LaserGRBL.Icons
{
    public interface IIconsLoader
    {
        Bitmap LoadImage(string resourceName);
        bool Contains(string key);

    }
}
