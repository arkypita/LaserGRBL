using System;
using System.Collections.Generic;
using System.Drawing;
using LaserGRBL.CSV;

namespace LaserGRBL
{
	public interface IGrblRow
	{
		string LeftString { get; }
		string RightString { get; }
		string ToolTip { get ;}

		Color LeftColor { get; }
		Color RightColor { get; }
	}
	
	public class CSVD
	{
		public static CsvDictionary Settings = new CSV.CsvDictionary("LaserGRBL.CSV.setting_codes.csv", 3);
		public static CsvDictionary Alarms = new CSV.CsvDictionary("LaserGRBL.CSV.alarm_codes.csv", 2);
		public static CsvDictionary Errors = new CSV.CsvDictionary("LaserGRBL.CSV.error_codes.csv", 2);
	}

	public class GrblCommand : ICloneable, IGrblRow
	{
		private string mLine;
		private string mResult;
		private CommandStatus mStatus;
		private Dictionary<char, GrblCommand.Element> list;
		private string mToolTip;
		
		public class Element
		{
			private Char mCommand;
			private Decimal mNumber;
			private string mString;

			public Element(Char Command, Decimal Number)
			{
				mCommand = Command;
				mNumber = Number;
				mString = Command + Number.ToString();
			}

			public Char Command
			{ get { return mCommand; } }

			public Decimal Number
			{ get { return mNumber; } }

		}

		public enum CommandStatus
		{ Queued, WaitingResponse, ResponseGood, ResponseBad }


		public object Clone()
		{ return MemberwiseClone(); }

		public string Command
		{ get { return mLine; } }

		public string Result
		{ get { return mResult; } }

		public CommandStatus Status
		{ get { return mStatus; } }

		public GrblCommand(string line)
		{
			list = new Dictionary<char, Element>();
			mStatus = CommandStatus.Queued;

			try
			{
				mLine = line.ToUpper();

				char cmd = '\0';
				string num = "";
				bool comment = false;
				bool oldspace = false;
				System.Text.StringBuilder sb = new System.Text.StringBuilder();

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
				Add(new Element(cmd, Decimal.Parse(num, System.Globalization.NumberFormatInfo.InvariantInfo))); //aggiungi l'ultimo
			}
			catch {}
		}

		private void Add(Element element)
		{ list.Add(element.Command, element); }

		public void SetResult(string result, bool decode) //ERROR:NUM
		{
			mResult = result.ToUpper().Trim();

			if (mResult.Length == 0)
				mStatus = CommandStatus.WaitingResponse;
			else if (mResult.StartsWith("OK"))
				mStatus = CommandStatus.ResponseGood;
			else if (mResult.StartsWith("ERROR"))
				mStatus = CommandStatus.ResponseBad;
			
			if (decode && mStatus == CommandStatus.ResponseBad)
			{
				try
				{
					string key = result.Substring(result.IndexOf(':') + 1);
					string brief = CSVD.Errors.GetItem(key, 0);
					string desc = CSVD.Errors.GetItem(key, 1);
					
					mResult = brief;
					mToolTip = desc;
				} catch {}
			}
		}

		#region G Codes

		public Element G
		{ get { return GetElement('G'); } }

		public bool IsMovement
		{ get { return G != null && G.Number >= 0 && G.Number <= 3; } }

		public bool IsLinearMovement
		{ get { return G != null && (G.Number == 0 || G.Number == 1); } }

		public bool IsArcMovement
		{ get { return G != null && (G.Number == 2 || G.Number == 3); } }

		public bool IsG0
		{ get { return G != null && G.Number == 0; } }

		public bool IsG1
		{ get { return G != null && G.Number == 1; } }

		public bool IsG2
		{ get { return G != null && G.Number == 2; } }

		public bool IsG3
		{ get { return G != null && G.Number == 3; } }

		public bool IsG4
		{ get { return G != null && G.Number == 4; } }

		#endregion

		#region M Codes

		public Element M
		{ get { return GetElement('M'); } }

		public bool IsM3
		{ get { return M != null && M.Number == 3; } }

		public bool IsM5
		{ get { return M != null && M.Number == 5; } }

		#endregion

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

		private Element GetElement(char key)
		{ return list.ContainsKey(key) ? list[key] : null; }

		public double GetArcRadius()
		{
			double oX = (double)(I != null ? I.Number : 0);
			double oY = (double)(J != null ? J.Number : 0);
			return Math.Sqrt(oX * oX + oY * oY);
		}
		public PointF GetCenter(float curX, float curY)
		{
			float oX = I != null ? (float)I.Number : 0;
			float oY = J != null ? (float)J.Number : 0;

			return new PointF(curX + oX, curY + oY);
		}

		public bool TrueMovement(decimal curX, decimal curY)
		{ return (X != null || Y != null) && ((X == null || X.Number != curX) || (Y == null || Y.Number != curY)); }

		public string LeftString
		{ get { return Command; } }

		public string RightString
		{ get { return Result; } }

		public string ToolTip
		{ get { return mToolTip; } }
		
		public Color LeftColor
		{ get { return Color.Black; } }

		public Color RightColor
		{ get { return Status == CommandStatus.ResponseGood ? Color.DarkBlue : Status == CommandStatus.ResponseBad ? Color.Red : Color.Black; } }

		internal void SetSending()
		{mStatus = CommandStatus.WaitingResponse;}
	}

	public class GrblMessage : IGrblRow
	{
		public enum MessageType
		{Startup, Config, Alarm, Feedback, Position, Others}

		private string mMessage;
		private string mToolTip;
		private MessageType mType;
		
		public GrblMessage(string message, bool decode)
		{
			mMessage = message.Trim();

			if (mMessage.ToLower().StartsWith("$") || mMessage.ToLower().StartsWith("~") || mMessage.ToLower().StartsWith("!") || mMessage.ToLower().StartsWith("?") || mMessage.ToLower().StartsWith("ctrl"))
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
						
						mMessage = string.Format("{0} ({1})", message, brief);
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

		public string Message
		{get { return mMessage; ; }}
		
		public string LeftString
		{get { return mMessage; ; }}

		public string RightString
		{get { return ""; }}
		
		public string ToolTip
		{get { return mToolTip; }}
		
		public Color LeftColor
		{
			get 
			{
				if (mType == MessageType.Startup)
					return Color.DarkGreen;
				else if (mType == MessageType.Alarm)
					return Color.Crimson;
				else if (mType == MessageType.Config)
					return Color.DimGray;
				else if (mType == MessageType.Feedback)
					return Color.DodgerBlue;
				else if (mType == MessageType.Position)
					return Color.OrangeRed;
				else if (mType == MessageType.Others)
					return Color.Purple;
				else
					return Color.Purple;
			}
		}

		public Color RightColor
		{get { return Color.Black; }}
	}
}
