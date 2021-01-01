//Copyright (c) 2016-2021 Diego Settimi - https://github.com/arkypita/

// This program is free software; you can redistribute it and/or modify  it under the terms of the GPLv3 General Public License as published by  the Free Software Foundation; either version 3 of the License, or (at  your option) any later version.
// This program is distributed in the hope that it will be useful, but  WITHOUT ANY WARRANTY; without even the implied warranty of  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GPLv3  General Public License for more details.
// You should have received a copy of the GPLv3 General Public License  along with this program; if not, write to the Free Software  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307,  USA. using System;

using System;
using System.Collections.Generic;
using System.Text;

namespace LaserGRBL.CSV
{
	public class FormattedMessage
	{
		public enum DTFormat
		{ Clear, UnixDT }

		System.Globalization.CultureInfo dtf = new System.Globalization.CultureInfo("it-IT", false);
		System.Globalization.CultureInfo nf = new System.Globalization.CultureInfo("en-US", false);

		DTFormat defDTFormat = DTFormat.UnixDT;

		private StringList list;
		private int i = 0;

		public FormattedMessage()
		{ list = new StringList(); }

		private FormattedMessage(StringList message)
		{ list = new StringList(message); }

		public StringList ToStringList()
		{ return new StringList(list); }

		public string ToMessage()
		{ return list.ToMessage(); }

		public string EscapeSymbol
		{
			get { return list.EscapeSymbol; }
			set { list.EscapeSymbol = value; }
		}

		public string Separator
		{
			get { return list.Separator; }
			set { list.Separator = value; }
		}

		public string NullPlaceholder
		{
			get { return list.NullPlaceholder; }
			set { list.NullPlaceholder = value; }
		}

		public DTFormat DefaultTimeFormat
		{
			get { return defDTFormat; }
			set { defDTFormat = value; }
		}

		public static FormattedMessage FromStringList(StringList msg)
		{ return msg != null ? new FormattedMessage(msg) : null; }

		public string OriginalMessage
		{ get { return list.OriginalMessage; } }

		public void Append(bool value)
		{ list.Add(Convert.ToString(value, nf)); }
		public void Append(int value)
		{ list.Add(Convert.ToString(value, nf)); }
		public void Append(long value)
		{ list.Add(Convert.ToString(value, nf)); }
		public void Append(decimal value)
		{ list.Add(Convert.ToString(value, nf)); }
		public void Append(float value)
		{ list.Add(Convert.ToString(value, nf)); }
		public void Append(double value)
		{ list.Add(Convert.ToString(value, nf)); }
		public void Append(string value)
		{ list.Add(Convert.ToString(value)); }
		public void Append(TimeSpan value)
		{ list.Add(Convert.ToString(value)); }
		public void Append(DateTime value)
		{ Append(value, DefaultTimeFormat); }
		public void Append(DateTime value, DTFormat format)
		{
			if (format == DTFormat.Clear)
				list.Add(Convert.ToString(value, dtf));
			else
				list.Add(Convert.ToString(DotNetToUnix(value)));
		}

		public void BeginRead()
		{ i = 0; }

		public bool ReadBool()
		{ return ReadBool(i++); }
		public bool ReadBool(int index)
		{ return Convert.ToBoolean(list[index], nf); }
		public int ReadInt()
		{ return ReadInt(i++); }
		public int ReadInt(int index)
		{ return Convert.ToInt32(list[index], nf); }
		public long ReadLong()
		{ return ReadLong(i++); }
		public long ReadLong(int index)
		{ return Convert.ToInt64(list[index], nf); }
		public decimal ReadDecimal()
		{ return ReadDecimal(i++); }
		public decimal ReadDecimal(int index)
		{ return Convert.ToDecimal(list[index], nf); }
		public float ReadFloat()
		{ return ReadFloat(i++); }
		public float ReadFloat(int index)
		{ return Convert.ToSingle(list[index], nf); }
		public double ReadDouble()
		{ return ReadDouble(i++); }
		public double ReadDouble(int index)
		{ return Convert.ToDouble(list[index], nf); }
		public string ReadString()
		{ return ReadString(i++); }
		public string ReadString(int index)
		{ return list[index]; }
		//public string ReadString(System.Data.DataColumn dest)
		//{ return Truncate(ReadString(i++), dest); }
		//public string ReadString(int index, System.Data.DataColumn dest)
		//{ return Truncate(list[index], dest); }
		public TimeSpan ReadTimeSpan()
		{ return ReadTimeSpan(i++); }
		public TimeSpan ReadTimeSpan(int index)
		{ return TimeSpan.Parse(list[index]); }
		public DateTime ReadDateTime()
		{ return ReadDateTime(i++); }
		public DateTime ReadDateTime(int index)
		{ return ReadDateTime(index, DTFormat.UnixDT); }
		public DateTime ReadDateTime(DTFormat format)
		{ return ReadDateTime(i++, format); }
		public DateTime ReadDateTime(int index, DTFormat format)
		{
			if (format == DTFormat.Clear)
				return Convert.ToDateTime(list[index], dtf);
			else
				return UnixToDotNet(Convert.ToInt32(list[index], dtf));
		}

		//private string Truncate(string source, System.Data.DataColumn dest)
		//{
		//	if (source.Length > dest.MaxLength)
		//	{
		//		source = source.Substring(0, dest.MaxLength);
		//		//TODO: log the event
		//		System.Diagnostics.Debug.WriteLine(System.String.Format("Tronco il campo {0}", dest.ColumnName));
		//	}

		//	return source;
		//}


		//**************************************************************

		private static DateTime UnixToDotNet(int unixTimestamp)
		{
			DateTime dt = DateTime.Parse("1/1/1970");
			return dt.AddSeconds(unixTimestamp);
		}

		private static int DotNetToUnix(DateTime dotNetTimestamp)
		{
			TimeSpan span = new TimeSpan(DateTime.Parse("1/1/1970").Ticks);
			DateTime time = dotNetTimestamp.Subtract(span);
			int t = (int)(time.Ticks / 10000000);
			return t;
		}
	}

	public class StringList : System.Collections.Generic.IList<string>
	{
		private string esc = "\\"; //escape symbol
		private string sep = ";";  //separator
		private string phl = "#";  //null placeholder
		private string orig;
		private List<string> list;

		public StringList()
		{
			list = new List<string>();
			orig = null;
		}

		public StringList(StringList source)
		{
			list = new List<string>(source);
			orig = source.orig;
		}

		private StringList(string message, string sep)
		{
			this.sep = sep;
			list = SplitMessage(message);
			orig = message;
		}

		public string EscapeSymbol
		{
			get { return esc; }
			set { esc = value; }
		}

		public string Separator
		{
			get { return sep; }
			set { sep = value; }
		}

		public string NullPlaceholder
		{
			get { return phl; }
			set { phl = value; }
		}

		public string ToMessage()
		{ return BuildMessage(); }

		public static StringList FromMessage(string msg, string sep)
		{ return msg != null ? new StringList(msg, sep) : null; }

		private List<string> SplitMessage(string msg)
		{
			if (msg == null)
				return null;

			List<string> rv = new List<string>();
			int last = 0;

			for (int i = 0; i < msg.Length + 1; )
			{
				bool fend = EndReached(msg, i);
				bool fsep = !fend && SeparatorFound(msg, i);
				if (fend || fsep)
				{
					int from = last;
					int len = i - from;
					string part = msg.Substring(from, len);
					part = sget(part);
					rv.Add(part);

					i += sep.Length; //skip separator
					last = i;
				}
				else
					i++;
			}

			return rv;
		}

		public string OriginalMessage
		{ get { return orig; } }

		private static bool EndReached(string msg, int i) //sono oltre la fine della stringa
		{
			return i == msg.Length;
		}

		private bool SeparatorFound(string msg, int i)
		{
			return (substring(msg, i, sep.Length) == sep && substring(msg, i, -esc.Length) != esc);
		}

		private string substring(string s, int start, int len)
		{
			if (len < 0)
			{
				len = -len;
				start -= len;
			}
			if (start < 0)
				start = 0;

			len = Math.Min(len, s.Length - start);

			string rv = s.Substring(start, len);

			return rv;
		}

		private string BuildMessage()
		{
			StringBuilder SB = new StringBuilder();

			if (list.Count == 0)
				throw new Exception("Il protocollo non gestisce l'invio di messaggi vuoti"); //questo per l'impossibilità di distinguere un messaggio vuoto da un messaggio contenente una singola stringa vuota

			for (int i = 0; i < list.Count; i++)
			{
				SB.Append(sput(list[i]));

				if (i < list.Count - 1)
					SB.Append(sep); //non appendere il separatore dopo l'ultimo campo
			}

			return SB.ToString();
		}

		private string sput(string s)
		{
			string rv = s;
			rv = escape(rv, esc, esc);
			rv = escape(rv, sep, esc);
			rv = escape(rv, phl, esc);
			rv = nullify(rv, phl);
			return rv;
		}

		private string sget(string s)
		{
			string rv = s;
			rv = unnullify(rv, phl);
			rv = unescape(rv, phl, esc);
			rv = unescape(rv, sep, esc);
			rv = unescape(rv, esc, esc);
			return rv;
		}

		private string nullify(string s, string placeholder)
		{ return (s == null ? placeholder : s); }

		private string unnullify(string s, string placeholder)
		{ return (s == placeholder ? null : s); }


		private string escape(string s, string token, string escape)
		{
			if (s == null)
				return null;

			StringBuilder SB = new StringBuilder(s);
			SB.Replace(token, escape + token);
			return SB.ToString();
		}

		private string unescape(string s, string token, string escape)
		{
			if (s == null)
				return null;

			StringBuilder SB = new StringBuilder(s);
			SB.Replace(escape + token, token);
			return SB.ToString();
		}


		#region Membri di IList<string>

		public int IndexOf(string item)
		{
			return list.IndexOf(item);
		}

		public void Insert(int index, string item)
		{
			list.Insert(index, item);
		}

		public void RemoveAt(int index)
		{
			list.RemoveAt(index);
		}

		public string this[int index]
		{
			get { return list[index]; }
			set { list[index] = value; }
		}

		#endregion

		#region ICollection<string> Membri di

		public void Add(string item)
		{
			list.Add(item);
		}

		public void Clear()
		{
			list.Clear();
		}

		public bool Contains(string item)
		{
			return list.Contains(item);
		}

		public void CopyTo(string[] array, int arrayIndex)
		{
			list.CopyTo(array, arrayIndex);
		}

		public int Count
		{
			get { return list.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public bool Remove(string item)
		{
			return list.Remove(item);
		}

		#endregion

		#region IEnumerable<string> Membri di

		public IEnumerator<string> GetEnumerator()
		{
			return list.GetEnumerator();
		}

		#endregion

		#region IEnumerable Membri di

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return list.GetEnumerator();
		}

		#endregion
	}

}