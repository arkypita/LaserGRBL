using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Svg.Text
{
    /// <summary>
    /// http://stackoverflow.com/questions/3633000/net-enumerate-winforms-font-styles
    /// </summary>
    public class FontFamily
    {
        #region InstalledFont Parameters

        string _fontName = string.Empty;
        string _fontSubFamily = string.Empty;
        string _fontPath = string.Empty;

        #endregion

        #region InstalledFont Constructor

        private FontFamily(string fontName, string fontSubFamily, string fontPath)
        {
            _fontName = fontName;
            _fontSubFamily = fontSubFamily;
            _fontPath = fontPath;
        }

        #endregion

        #region InstalledFont Properties

        public string FontName { get { return _fontName; } set { _fontName = value; } }
        public string FontSubFamily { get { return _fontSubFamily; } set { _fontSubFamily = value; } }
        public string FontPath { get { return _fontPath; } set { _fontPath = value; } }

        #endregion

        static public FontFamily FromPath(string fontFilePath)
        {
            string FontName = string.Empty;
            string FontSubFamily = string.Empty;
            string encStr = "UTF-8";
            string strRet = string.Empty;

            using (FileStream fs = new FileStream(fontFilePath, FileMode.Open, FileAccess.Read))
            {

                TT_OFFSET_TABLE ttOffsetTable = new TT_OFFSET_TABLE()
                {
                    uMajorVersion = ReadUShort(fs),
                    uMinorVersion = ReadUShort(fs),
                    uNumOfTables = ReadUShort(fs),
                    uSearchRange = ReadUShort(fs),
                    uEntrySelector = ReadUShort(fs),
                    uRangeShift = ReadUShort(fs),
                };

                TT_TABLE_DIRECTORY tblDir = new TT_TABLE_DIRECTORY();
                bool found = false;
                for (int i = 0; i <= ttOffsetTable.uNumOfTables; i++)
                {
                    tblDir = new TT_TABLE_DIRECTORY();
                    tblDir.Initialize();
                    fs.Read(tblDir.szTag, 0, tblDir.szTag.Length);
                    tblDir.uCheckSum = ReadULong(fs);
                    tblDir.uOffset = ReadULong(fs);
                    tblDir.uLength = ReadULong(fs);

                    Encoding enc = Encoding.GetEncoding(encStr);
                    string s = enc.GetString(tblDir.szTag);

                    if (s.CompareTo("name") == 0)
                    {
                        found = true;
                        break;
                    }
                }

                if (!found) return null;

                fs.Seek(tblDir.uOffset, SeekOrigin.Begin);

                TT_NAME_TABLE_HEADER ttNTHeader = new TT_NAME_TABLE_HEADER
                {
                    uFSelector = ReadUShort(fs),
                    uNRCount = ReadUShort(fs),
                    uStorageOffset = ReadUShort(fs)
                };

                TT_NAME_RECORD ttRecord = new TT_NAME_RECORD();

                for (int j = 0; j <= ttNTHeader.uNRCount; j++)
                {
                    ttRecord = new TT_NAME_RECORD()
                    {
                        uPlatformID = ReadUShort(fs),
                        uEncodingID = ReadUShort(fs),
                        uLanguageID = ReadUShort(fs),
                        uNameID = ReadUShort(fs),
                        uStringLength = ReadUShort(fs),
                        uStringOffset = ReadUShort(fs)
                    };

                    if (ttRecord.uNameID > 2) { break; }

                    long nPos = fs.Position;
                    fs.Seek(tblDir.uOffset + ttRecord.uStringOffset + ttNTHeader.uStorageOffset, SeekOrigin.Begin);

                    byte[] buf = new byte[ttRecord.uStringLength];
                    fs.Read(buf, 0, ttRecord.uStringLength);

                    Encoding enc;
                    if (ttRecord.uEncodingID == 3 || ttRecord.uEncodingID == 1)
                    {
                        enc = Encoding.BigEndianUnicode;
                    }
                    else
                    {
                        enc = Encoding.UTF8;
                    }

                    strRet = enc.GetString(buf);
                    if (ttRecord.uNameID == 1) { FontName = strRet; }
                    if (ttRecord.uNameID == 2) { FontSubFamily = strRet; }

                    fs.Seek(nPos, SeekOrigin.Begin);
                }

                return new FontFamily(FontName, FontSubFamily, fontFilePath);
            }
        }

        [CLSCompliant(false)]
        public struct TT_OFFSET_TABLE
        {
            public ushort uMajorVersion;
            public ushort uMinorVersion;
            public ushort uNumOfTables;
            public ushort uSearchRange;
            public ushort uEntrySelector;
            public ushort uRangeShift;
        }

        [CLSCompliant(false)]
        public struct TT_TABLE_DIRECTORY
        {
            public byte[] szTag;
            public UInt32 uCheckSum;
            public UInt32 uOffset;
            public UInt32 uLength;
            public void Initialize()
            {
                szTag = new byte[4];
            }
        }

        [CLSCompliant(false)]
        public struct TT_NAME_TABLE_HEADER
        {
            public ushort uFSelector;
            public ushort uNRCount;
            public ushort uStorageOffset;
        }

        [CLSCompliant(false)]
        public struct TT_NAME_RECORD
        {
            public ushort uPlatformID;
            public ushort uEncodingID;
            public ushort uLanguageID;
            public ushort uNameID;
            public ushort uStringLength;
            public ushort uStringOffset;
        }

        static private UInt16 ReadChar(FileStream fs, int characters)
        {
            string[] s = new string[characters];
            byte[] buf = new byte[Convert.ToByte(s.Length)];

            buf = ReadAndSwap(fs, buf.Length);
            return BitConverter.ToUInt16(buf, 0);
        }

        static private UInt16 ReadByte(FileStream fs)
        {
            byte[] buf = new byte[11];
            buf = ReadAndSwap(fs, buf.Length);
            return BitConverter.ToUInt16(buf, 0);
        }

        static private UInt16 ReadUShort(FileStream fs)
        {
            byte[] buf = new byte[2];
            buf = ReadAndSwap(fs, buf.Length);
            return BitConverter.ToUInt16(buf, 0);
        }

        static private UInt32 ReadULong(FileStream fs)
        {
            byte[] buf = new byte[4];
            buf = ReadAndSwap(fs, buf.Length);
            return BitConverter.ToUInt32(buf, 0);
        }

        static private byte[] ReadAndSwap(FileStream fs, int size)
        {
            byte[] buf = new byte[size];
            fs.Read(buf, 0, buf.Length);
            Array.Reverse(buf);
            return buf;
        }
    }

    class Program
    {
        //static void Main(string[] args)
        //{
        //    System.Drawing.FontFamily fam;
        //    var allInstalledFonts = from e in Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion\\Fonts", false).GetValueNames()
        //                            select Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion\\Fonts").GetValue(e);

        //    var ttfFonts = from e in allInstalledFonts.Where(e => (e.ToString().EndsWith(".ttf") || e.ToString().EndsWith(".otf"))) select e;
        //    var ttfFontsPaths = from e in ttfFonts.Select(e => (Path.GetPathRoot(e.ToString()) == "") ? Environment.GetFolderPath(Environment.SpecialFolder.Fonts) + "\\" + e.ToString() : e.ToString()) select e;
        //    var fonts = from e in ttfFontsPaths.Select(e => GetFontDetails(e.ToString())) select e;

        //    foreach (InstalledFont f in fonts)
        //    {
        //        if (f != null)
        //            Console.WriteLine("Name: " + f.FontName + ", SubFamily: " + f.FontSubFamily + ", Path: " + f.FontPath);
        //    }

        //    Console.ReadLine();
        //}

        
    }
}
