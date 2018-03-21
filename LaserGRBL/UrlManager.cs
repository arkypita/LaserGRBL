
//Usage statistics and Update are for LaserGRBL official version
//Unofficial/fork versions of LaserGRBL should use their own url for stats and update (if they need the feature)

namespace LaserGRBL
{
	public static class UrlManager
	{
		public static string UpdateMain = @"https://api.github.com/repos/arkypita/LaserGRBL/releases/latest";
		public static string UpdateMirror = @"http://lasergrbl.com/latest.php";
		public static string Statistics = @"http://stats.lasergrbl.com/handler.php";
	}
}
