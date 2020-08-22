//Copyright (c) 2016-2020 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System.Drawing;

namespace LaserGRBL
{
    public class GrblMessage : IGrblRow
	{
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

		public string Message
		{get { return mMessage; ; }}
		
		public string GetMessage()
		{return mMessage; }

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
		{ get { return 3; } }
	}
}
