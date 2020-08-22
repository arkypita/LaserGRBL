//Copyright (c) 2016-2020 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.Collections.Generic;
using System.Drawing;

namespace LaserGRBL
{
	public partial class GrblCommand : ICloneable, IGrblRow
	{

		private string mLine;
		private string mCodedResult;
		private TimeSpan mTimeOffset;
		private Dictionary<char, GrblElement> mHelper;

		private int mRepeatCount;

		public GrblCommand(string line)
		{ mLine = line.ToUpper().Trim(); mRepeatCount = 0; }

		public GrblCommand(string line, int repeat)
		{ mLine = line.ToUpper().Trim(); mRepeatCount = repeat; }

		public GrblCommand(IEnumerable<GrblElement> elements)
		{
			mLine = "";
			foreach (GrblElement e in elements)
				mLine = mLine + e.ToString() + " ";
			mLine = mLine.TrimEnd().ToUpper();
		}

		public GrblCommand(GrblElement first, GrblCommand toappend)
		{
			mLine = string.Format("{0} {1}", first, toappend.mLine);
		}

		public bool JustBuilt
		{ get { return mHelper != null; } }

		public void BuildHelper()
		{
			if (JustBuilt) //just built!
				return;

			try
			{
				mHelper = new Dictionary<char, GrblElement>();
				char cmd = '\0';
				string num = "";
				bool comment = false;
				bool oldspace = false;
				System.Text.StringBuilder sb = new System.Text.StringBuilder();

				if (!IsGrblCommand) //do not parse grbl command set
				{
					foreach (char c in mLine)
					{
						if (c == ';') //dal punto e virgola fino a fine riga -> commento!
							break;

						if (c == '(')
							comment = true;

						bool space = c == ' ' ? true : false;
						if (!comment)
						{
							if (space && !oldspace)
								sb.Append(' ');
							else if (!space)
								sb.Append(c);
						}
						oldspace = space;

						if (!comment)
						{
							if (Char.IsLetter(c))
							{
								if (cmd != '\0') //chiudi il comando precedente
									Add(new GrblElement(cmd, Decimal.Parse(num, System.Globalization.NumberFormatInfo.InvariantInfo)));

								cmd = c; //apri il comando successivo
								num = "";
							}
							else if (Char.IsNumber(c) || c == '.' || c == '-')
							{
								num += c; //accumula il dato
							}
						}

						if (c == ')')
							comment = false;
					}

					mLine = sb.ToString();

					if (cmd != '\0')
						Add(new GrblElement(cmd, Decimal.Parse(num, System.Globalization.NumberFormatInfo.InvariantInfo))); //aggiungi l'ultimo
				}
			}
			catch { }
		}

		public int RepeatCount
		{ get { return mRepeatCount; } }

		public void DeleteHelper()
		{ mHelper = null; }

		public void SetOffset(TimeSpan Time)
		{ mTimeOffset = Time; }

		public TimeSpan TimeOffset
		{ get { return mTimeOffset; } }

		public object Clone()
		{ return MemberwiseClone(); }

		public string Command
		{ get { return mLine; } }

		private static char[] trimarray = new char[] { '\r', '\n', ' ' };

		public string SerialData
		{
			get
			{
				if (CanCompress)
					return mLine.Trim(trimarray).Replace(" ", "") + '\n';  //strip spaces
				else
					return mLine.Trim(trimarray) + '\n';  //send it "as is"
			}
		}

		private bool CanCompress
		{ get { return !IsGrblCommand; } }

		public string GetResult(bool decode, bool erroronly)
		{
			if (Status == CommandStatus.ResponseBad && decode)
			{
				try
				{
					string key = mCodedResult.Substring(mCodedResult.IndexOf(':') + 1);
					string brief = CSVD.Errors.GetItem(key, 0);
					if (brief != null) return brief;
				}
				catch { }

				return mCodedResult; //if ex or null
			}

			return erroronly ? null : mCodedResult;
		}

		public CommandStatus Status
		{
			get
			{
				if (mCodedResult == null)
					return CommandStatus.Queued;
				else if (mCodedResult.Length == 0)
					return CommandStatus.WaitingResponse;
				else if (mCodedResult.StartsWith("OK"))
					return CommandStatus.ResponseGood;
				else if (mCodedResult.StartsWith("ERROR"))
					return CommandStatus.ResponseBad;
				else
					return CommandStatus.InvalidResponse;
			}
		}


		private void Add(GrblElement element)
		{ mHelper.Add(element.Command, element); }

		public void SetResult(string result, bool decode) //ERROR:NUM
		{
			mCodedResult = result.ToUpper().Trim();
		}

		public bool IsGrblCommand
		{ get { return mLine.StartsWith("$"); } }

		public bool IsEmpty
		{ get { return mLine.Length == 0; } }

		public bool IsWriteEEPROM
		{ get { return IsGrblCommand && IsSetConf; } } //maybe need to add G10/G28.1/G30.1 ?

		private bool IsSetConf
		{ get { return GrblConf.IsSetConf(mLine); } }


		#region G Codes

		public GrblElement G
		{ get { return GetElement('G'); } }

		public bool IsMovement
		{ get { return IsLinearMovement || IsArcMovement; } }

		public bool IsLinearMovement
		{ get { return !IsSetWCO && (X != null || Y != null || Z != null) && (I == null && J == null && R == null); } }

		//public bool IsRapidMovement
		//{
		//	get
		//	{
		//		return (G != null && G.Number == 0);
		//	}
		//}

		public bool IsArcMovement
		{ get { return !IsSetWCO && (I != null || J != null || R != null); } }

		public bool IsCW(bool prev)
		{
			if (G != null && G.Number == 2)
				return true;
			else if (G != null && G.Number == 3)
				return false;
			else
				return prev;
		}

		public bool IsSetWCO
		{ get { return G != null && G.Number == 92; } }

		public bool IsPause
		{ get { return G != null && G.Number == 4; } }

		//G90 

		public bool IsAbsoluteCoord
		{ get { return G != null && G.Number == 90; } }

		public bool IsRelativeCoord
		{ get { return G != null && G.Number == 91; } }

		#endregion

		#region M Codes

		public GrblElement M
		{ get { return GetElement('M'); } }

		public bool IsLaserON
		{ get { return IsM3 || IsM4; } }

		public bool IsM3
		{ get { return M != null && M.Number == 3; } }

		public bool IsM4
		{ get { return M != null && M.Number == 4; } }

		public bool IsLaserOFF
		{ get { return IsM5; } }

		public bool IsM5
		{ get { return M != null && M.Number == 5; } }

		#endregion

		#region Parameters

		public GrblElement T
		{ get { return GetElement('T'); } }

		public GrblElement S
		{ get { return GetElement('S'); } }

		public GrblElement P
		{ get { return GetElement('P'); } }

		public GrblElement X
		{ get { return GetElement('X'); } }

		public GrblElement Y
		{ get { return GetElement('Y'); } }

		public GrblElement Z
		{ get { return GetElement('Z'); } }

		public GrblElement I
		{ get { return GetElement('I'); } }

		public GrblElement J
		{ get { return GetElement('J'); } }

		public GrblElement F
		{ get { return GetElement('F'); } }

		public GrblElement R
		{ get { return GetElement('R'); } }

		#endregion

		private GrblElement GetElement(char key)
		{ return mHelper.ContainsKey(key) ? mHelper[key] : null; }

		public string GetMessage() //per la visualizzazione
		{ return mRepeatCount == 0 ? Command : String.Format("{0} (Retry {1})", Command, mRepeatCount); }

		public string GetToolTip(bool decode)
		{
			if (Status == CommandStatus.ResponseBad && decode)
			{
				try
				{
					string key = mCodedResult.Substring(mCodedResult.IndexOf(':') + 1);
					string tooltip = CSVD.Errors.GetItem(key, 1);
					if (tooltip != null) return tooltip;
				}
				catch { }
			}
			return "";
		}

		public Color LeftColor
		{ get { return ColorScheme.LogLeftCOMMAND; } }

		public Color RightColor
		{ get { return Status == CommandStatus.ResponseGood ? ColorScheme.LogRightGOOD : Status == CommandStatus.ResponseBad ? ColorScheme.LogRightBAD : ColorScheme.LogRightOTHERS; } }

		public void SetSending()
		{ mCodedResult = ""; }

		public int ImageIndex
		{ get { return Status == CommandStatus.Queued || Status == CommandStatus.WaitingResponse ? 0 : Status == CommandStatus.ResponseGood ? 1 : 2; } }

		public override string ToString()
		{ return this.mLine; }
	}
}
