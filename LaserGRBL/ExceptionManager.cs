//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace LaserGRBL
{
	public partial class ExceptionManager : Form
	{
		public static GrblCore Core;
		public static Form ParentMain;
		public bool UserClose = false;

		private ExceptionManager()
		{
			InitializeComponent();
		}

		public static void RegisterHandler()
		{
			AppDomain.CurrentDomain.UnhandledException += OnUnhandledThreadException;
			Application.ThreadException += OnUnhandledMainException;
		}

		private static void OnUnhandledMainException(object sender, ThreadExceptionEventArgs e)
		{
			CreateAndShow(e?.Exception, true, false);
		}

		private static void OnUnhandledThreadException(object sender, UnhandledExceptionEventArgs e)
		{
			CreateAndShow(e?.ExceptionObject as Exception, false, false);
		}

		public static void OnHandledException(Exception ex, bool cancontinue)
		{
			CreateAndShow(ex, cancontinue, true);
		}

		public delegate void CreateAndShowDlg(Exception ex, bool cancontinue, bool manual);
		public static void CreateAndShow(Exception ex, bool cancontinue, bool manual)
		{
			if (Program.DesignTime)
				return;

			if (ParentMain != null && ParentMain.InvokeRequired)
			{
				ParentMain.Invoke(new CreateAndShowDlg(CreateAndShow), ex, cancontinue, manual);
			}
			else
			{
				bool close;
				using (ExceptionManager f = new ExceptionManager())
				{
					StringBuilder sb = new StringBuilder();

					try
					{
						sb.AppendFormat("LaserGrbl v{0}", Program.CurrentVersion);
						sb.AppendLine();
						sb.AppendFormat("{0} v{1}", Core?.Type, GrblCore.Configuration?.GrblVersion);
						sb.AppendLine();
						sb.AppendFormat("Wrapper: {0}", Settings.GetObject("ComWrapper Protocol", ComWrapper.WrapperType.UsbSerial));
						sb.AppendLine();
						sb.AppendFormat("{0} ({1})", Tools.OSHelper.GetOSInfo()?.Replace("|", ", "), Tools.OSHelper.GetBitFlag());
						sb.AppendLine();
						sb.AppendFormat("CLR: {0}", Tools.OSHelper.GetClrInfo());
						sb.AppendLine();
						sb.AppendLine();
					}
					catch { }

					AppendExceptionData(sb, ex);

					f.TbExMessage.Text = sb.ToString();
					f.BtnContinue.Visible = cancontinue;
					f.ShowDialog(ParentMain);
					close = f.UserClose;
				}

				if (close)
					Application.Exit();
			}
		}

		private void BtnAbort_Click(object sender, EventArgs e)
		{
			UserClose = true;
			Close();
		}

		private void BtnContinue_Click(object sender, EventArgs e)
		{
			Close();
		}

		private static void AppendExceptionData(StringBuilder sb, Exception e)
		{
			if (e != null)
			{
				try
				{
					sb.AppendFormat("TypeOf exception  [{0}]", e.GetType());
					sb.AppendLine();
					sb.AppendFormat("Exception message [{0}]", e.Message);
					sb.AppendLine();
					sb.AppendFormat("Exception source  [{0}], thread [{1}]", e.Source, Thread.CurrentThread.Name);
					sb.AppendLine();
					sb.AppendFormat("Exception method  [{0}]", e.TargetSite);
					sb.AppendLine();
					sb.AppendFormat("");
					sb.AppendLine();
				}
				catch { }

				try
				{
					if (e.StackTrace != null)
					{
						sb.AppendFormat("   ----------- stack trace -----------");
						sb.AppendLine();
						sb.AppendFormat(e.StackTrace);
						sb.AppendLine();
						sb.AppendFormat("");
					}
				}
				catch { }

				try
				{
					sb.AppendLine();
					if (e.InnerException != null)
					{
						sb.AppendFormat("Inner exception data");
						sb.AppendLine();
						AppendExceptionData(sb, e.InnerException);
						sb.AppendLine();
					}
				}
				catch { }
			}
		}

		private void LblFormDescription_LinkClicked(object sender, LinkClickedEventArgs e)
		{Tools.Utils.OpenLink(e.LinkText);}

		private void ExceptionManager_Load(object sender, EventArgs e)
		{
			BringToFront();
		}

		private void BtnClipboardCopy_Click(object sender, EventArgs e)
		{
			Clipboard.SetText(TbExMessage.Text);
		}
	}
}
