using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Windows.Forms;

namespace LaserGRBL
{
	public partial class LegalDisclaimer : Form
	{
		public bool accepted = false;
		public LegalDisclaimer()
		{
			InitializeComponent();
		}

		private void LlTranslate_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			string text = HttpUtility.UrlEncode($"{LblNotToy.Text}\r\n\r\n{LblFree.Text}\r\n\r\n{CbIHaveRead.Text}");
			string lng = System.Threading.Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName;
			Tools.Utils.OpenLink($"https://translate.google.it/?sl=en&tl={lng}&text={text}&op=translate");
		}

		private void BtnAccept_Click(object sender, EventArgs e)
		{
			accepted = true;
			Close();
		}

		private void CbIHaveRead_CheckedChanged(object sender, EventArgs e)
		{
			BtnAccept.Enabled = CbIHaveRead.Checked;
		}

	}
}
