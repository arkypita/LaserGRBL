//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.Collections.Generic;
using System.Drawing;
using LaserGRBL.CSV;
using LaserGRBL.Obj3D;

namespace LaserGRBL
{
	public interface IGrblRow
	{
		string GetDecodedMessage();

		string GetResult(bool decode, bool erroronly);
		string GetToolTip(bool decode);

		Color LeftColor { get; }
		Color RightColor { get; }

		int ImageIndex { get; }
	}
	
	public static class CSVD
	{
		public static CsvDictionary Settings = new CsvDictionary("LaserGRBL.CSV.setting_codes.v1.1.csv", 3);
		public static CsvDictionary Alarms = new CsvDictionary("LaserGRBL.CSV.alarm_codes.csv", 2);
		public static CsvDictionary Errors = new CsvDictionary("LaserGRBL.CSV.error_codes.csv", 2);

		public static void LoadAppropriateCSV(GrblCore.GrblVersionInfo value)
		{
			LoadAppropriateSettings(value);
			LoadAppropriateAlarms(value);
			LoadAppropriateErrors(value);
		}

		private static void LoadAppropriateSettings(GrblCore.GrblVersionInfo value)
		{
			try
			{
				string ResourceName;
				if (value.IsOrtur && value.IsHAL)
					ResourceName = String.Format("LaserGRBL.CSV.setting_codes.ortur.GrblHal.csv");
				else if (value.IsOrtur && value.OrturFWVersionNumber >= 170)
					ResourceName = String.Format("LaserGRBL.CSV.setting_codes.ortur.v1.7.x.csv");
				else if (value.IsOrtur && value.OrturFWVersionNumber >= 150)
					ResourceName = String.Format("LaserGRBL.CSV.setting_codes.ortur.v1.5.x.csv");
				else if (value.IsOrtur)
					ResourceName = String.Format("LaserGRBL.CSV.setting_codes.ortur.v1.4.x.csv"); 
				else
					ResourceName = String.Format("LaserGRBL.CSV.setting_codes.v{0}.{1}.csv", value.Major, value.Minor);

				Settings = new CsvDictionary(ResourceName, 3);
			}
			catch { Settings = new CsvDictionary("LaserGRBL.CSV.setting_codes.v1.1.csv", 3); }
		}

		private static void LoadAppropriateAlarms(GrblCore.GrblVersionInfo value)
		{
			try
			{
				string ResourceName;
				if (value.IsOrtur && value.IsHAL)
					ResourceName = String.Format("LaserGRBL.CSV.alarm_codes.ortur.GrblHal.csv");
				else
					ResourceName = String.Format("LaserGRBL.CSV.alarm_codes.csv");

				Alarms = new CsvDictionary(ResourceName, 2);
			}
			catch { Alarms = new CsvDictionary("LaserGRBL.CSV.alarm_codes.csv", 2); }
		}

		private static void LoadAppropriateErrors(GrblCore.GrblVersionInfo value)
		{
			try
			{
				string ResourceName;
				if (value.IsOrtur && value.IsHAL)
					ResourceName = String.Format("LaserGRBL.CSV.error_codes.ortur.GrblHal.csv");
				else
					ResourceName = String.Format("LaserGRBL.CSV.error_codes.csv");

				Errors = new CsvDictionary(ResourceName, 2);
			}
			catch { Errors = new CsvDictionary("LaserGRBL.CSV.error_codes.csv", 2); }
		}
	}

	public partial class GrblCommand : ICloneable, IGrblRow, IDisposable
	{
		public class Element
		{
			protected Char mCommand;
			protected Decimal mNumber;

			public static implicit operator Element(string value)
			{return new Element(value[0], decimal.Parse(value.Substring(1), System.Globalization.CultureInfo.InvariantCulture));}

			public Element(Char Command, Decimal Number)
			{
				mCommand = Command;
				mNumber = Number;
			}

			public Char Command
			{ get { return mCommand; } }

			public Decimal Number
			{ get { return mNumber; } }

			public override string ToString()
			{ return Command + Number.ToString(System.Globalization.CultureInfo.InvariantCulture); }

			public override bool Equals(object obj)
			{
				Element o = obj as Element;
				return o != null && o.mCommand == mCommand && o.mNumber == mNumber;
			}

			public override int GetHashCode()
			{return mCommand.GetHashCode() ^ mNumber.GetHashCode();}

			//internal void SetNumber(decimal p)
			//{mNumber = p;}
		}

        private class ResultWrapper
        {
            public string CodedResult;
        }

        private string mLine;
        private ResultWrapper mResultWrapper;
        private TimeSpan mTimeOffset;
        private Dictionary<char, GrblCommand.Element> mHelper;
        private int mRepeatCount;
        public Object3DDisplayList LinkedDisplayList { get; internal set; } = null;


        public string mCodedResult
        {
            get { return mResultWrapper.CodedResult; }
            set { mResultWrapper.CodedResult = value; }
        }

		public GrblCommand(string line, int repeat = 0, bool preservecase = false)
		{
			ClearResult();
			mLine = line.Trim();
			if (!preservecase) mLine = mLine.ToUpper();
			mRepeatCount = repeat;
		}

		public void ClearResult()
		{
            mResultWrapper = new ResultWrapper();
			LinkedDisplayList?.Invalidate();
        }

		public GrblCommand(IEnumerable<Element> elements)
		{
			ClearResult();
			mLine = "";
			foreach (GrblCommand.Element e in elements)
				mLine = mLine + e.ToString() + " ";
			mLine = mLine.ToUpper().Trim();
		}

		public GrblCommand(Element first, GrblCommand toappend)
		{
			ClearResult();
			mLine = string.Format("{0} {1}", first, toappend.mLine).ToUpper().Trim();
		}

		public bool JustBuilt
		{ get { return mHelper != null; } }

		public void BuildHelper()
		{
			if (JustBuilt) //just built!
				return;

			try
			{
				mHelper = new Dictionary<char, Element>();
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
									Add(new Element(cmd, Decimal.Parse(num, System.Globalization.NumberFormatInfo.InvariantInfo)));

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
						Add(new Element(cmd, Decimal.Parse(num, System.Globalization.NumberFormatInfo.InvariantInfo))); //aggiungi l'ultimo
				}
			}
			catch { }
		}

		public int RepeatCount
		{ get { return mRepeatCount; } }

		public void DeleteHelper()
		{mHelper = null;}
		
		public void SetOffset(TimeSpan Time)
		{mTimeOffset = Time;}
		
		public TimeSpan TimeOffset
		{get {return mTimeOffset;}}
		
		public enum CommandStatus
		{ Queued, WaitingResponse, ResponseGood, ResponseBad, InvalidResponse }

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
					return mLine.Trim(trimarray).Replace(" ","") + '\n';  //strip spaces
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



		private void Add(Element element)
		{ mHelper.Add(element.Command, element); }

		public void SetResult(string result, bool decode) //ERROR:NUM
		{
			mCodedResult = result.ToUpper().Trim();
            LinkedDisplayList?.Invalidate();
        }

		public bool IsGrblCommand
		{ get { return mLine.StartsWith("$"); } }
		
		public bool IsEmpty
		{get{return mLine.Length == 0;}}

		public bool IsWriteEEPROM
		{ get { return IsGrblCommand && IsSetConf; } } //maybe need to add G10/G28.1/G30.1 ?

		private bool IsSetConf
		{ get { return GrblConfST.IsSetConf(mLine); } }

		
		#region G Codes

		public Element G
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

		public bool IsPause
		{ get { return G != null && G.Number == 4; } }

		//G90 

		public bool IsAbsoluteCoord
		{ get { return G != null && G.Number == 90; } }

		public bool IsRelativeCoord
		{ get { return G != null && G.Number == 91; } }

		#endregion

		#region M Codes

		public Element M
		{ get { return GetElement('M'); } }

		public bool IsLaserON
		{get {return IsM3 || IsM4;}}
		
		public bool IsM3
		{ get { return M != null && M.Number == 3; } }

		public bool IsM4
		{ get { return M != null && M.Number == 4; } }
		
		public bool IsLaserOFF
		{get {return IsM5;}}
		
		public bool IsM5
		{ get { return M != null && M.Number == 5; } }

		#endregion

		#region Parameters
	
		public Element T
		{ get { return GetElement('T'); } }

		public Element S
		{ get { return GetElement('S'); } }

		public Element P
		{ get { return GetElement('P'); } }

		public Element X
		{ get { return GetElement('X'); } }

		public Element Y
		{ get { return GetElement('Y'); } }

		public Element Z
		{ get { return GetElement('Z'); } }

		public Element I
		{ get { return GetElement('I'); } }

		public Element J
		{ get { return GetElement('J'); } }

		public Element F
		{ get { return GetElement('F'); } }

		public Element R
		{ get { return GetElement('R'); } }

		#endregion

		private Element GetElement(char key)
		{ return mHelper.ContainsKey(key) ? mHelper[key] : null; }

		public string GetDecodedMessage() //per la visualizzazione
		{  return mRepeatCount == 0 ? Command : String.Format("{0} (Retry {1})", Command, mRepeatCount); } 

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

		internal void SetSending()
		{mCodedResult = "";}

		public int ImageIndex
		{ get { return Status == CommandStatus.Queued || Status == CommandStatus.WaitingResponse ? 0 : Status == CommandStatus.ResponseGood ? 1 : 2; } }

		public override string ToString()
		{ return this.mLine; }

        public void Dispose()
        {
			LinkedDisplayList = null;
        }
    }

	public class GrblMessage : IGrblRow
	{
		public enum MessageType
		{
			Startup,	//from grbl
			Config,		//from grbl
			Alarm,		//from grbl
			Feedback,   //from grbl
			Position,   //from grbl
			Others,		//from grbl

			Warning,	//from LaserGRBL
			Diagnostic, //from LaserGRBL
		}


		private string mNativeMessage;
		private string mMessage;
		private string mToolTip;
		private MessageType mType;

		public GrblMessage(string message, MessageType type)
		{
			mMessage = message.Trim();
			mNativeMessage = mMessage;
			mType = type;
		}

		public GrblMessage(string message, bool decode)
		{
			mMessage = message.Trim();
			mNativeMessage = mMessage;

			if (mMessage.ToLower().StartsWith("$") && mMessage.Contains("=")) //if (mMessage.ToLower().StartsWith("$") || mMessage.ToLower().StartsWith("~") || mMessage.ToLower().StartsWith("!") || mMessage.ToLower().StartsWith("?") || mMessage.ToLower().StartsWith("ctrl"))
				mType = MessageType.Config;
			else if (mMessage.ToLower().StartsWith("grbl"))
				mType = MessageType.Startup;
			else if (mMessage.ToLower().StartsWith("alarm"))
				mType = MessageType.Alarm;
			else if (mMessage.StartsWith("<") && mMessage.EndsWith(">"))
				mType = MessageType.Position;
			else if (mMessage.StartsWith("[") && mMessage.EndsWith("]"))
				mType = MessageType.Feedback;
			else
				mType = MessageType.Others;
			
			if (decode)
			{
				try
				{
					if (mType == MessageType.Config) //$NUM=VAL
					{
						string key = message.Substring(1,message.IndexOf('=')-1);
						string brief = CSVD.Settings.GetItem(key, 0);
						string unit = CSVD.Settings.GetItem(key, 1);
						string desc = CSVD.Settings.GetItem(key, 2);

						if (brief != null)
							mMessage = string.Format("{0} ({1})", message, brief);
						
						if (desc != null)
							mToolTip = string.Format("{0} [{1}]",desc ,unit);
					}
					else if (mType == MessageType.Alarm) //ALARM:NUM
					{
						string key = message.Substring(message.IndexOf(':') + 1);
						string brief = CSVD.Alarms.GetItem(key, 0);
						string desc = CSVD.Alarms.GetItem(key, 1);
						
						mMessage = brief;
						mToolTip = desc;
					}

					
				}catch{}
			}
		}

		public string GetNativeMessage() => mNativeMessage;
		public string GetDecodedMessage() => mMessage;

		public string GetResult(bool decode, bool erroronly)
		{return null; }
		
		public string GetToolTip(bool decode) //already decoded on build
		{ return mToolTip; }
		
		public Color LeftColor
		{
			get 
			{
				if (mType == MessageType.Startup)
					return ColorScheme.LogLeftSTARTUP;
				else if (mType == MessageType.Alarm)
					return ColorScheme.LogLeftALARM;
				else if (mType == MessageType.Config)
					return ColorScheme.LogLeftCONFIG;
				else if (mType == MessageType.Feedback)
					return ColorScheme.LogLeftFEEDBACK;
				else if (mType == MessageType.Position)
					return ColorScheme.LogLeftPOSITION;
				else if (mType == MessageType.Others)
					return ColorScheme.LogLeftOTHERS;
				else
					return ColorScheme.LogLeftOTHERS;
			}
		}

		public Color RightColor
		{get { return Color.Black; }} //normalmente per questi messaggi non c'è un right

		public int ImageIndex
		{ get 
			{
				if (mType == MessageType.Warning)
					return 4;
				else if (mType == MessageType.Diagnostic)
					return 5;
				else
					return 3; 
			}
		}
	}
}
