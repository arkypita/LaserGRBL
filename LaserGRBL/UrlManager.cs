
//Usage statistics and Update are for LaserGRBL official version
//Unofficial/fork versions of LaserGRBL should use their own url for stats and update (if they need the feature)

//to leave the file in the repo but ignore future changes to it:
//git update-index --skip-worktree LaserGRBL\UrlManager.cs

//https://fallengamer.livejournal.com/93321.html
//https://stackoverflow.com/questions/13630849/git-difference-between-assume-unchanged-and-skip-worktree

namespace LaserGRBL
{
	public static class UrlManager
	{
		public static string UpdateMain = null;		//@"https://api.github.com/repos/arkypita/LaserGRBL/releases/latest";
		public static string UpdateMirror = null;	//@"http://lasergrbl.com/latest.php";
		public static string Statistics = null;		//@"http://stats.lasergrbl.com/handler.php";
	}
}
