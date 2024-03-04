using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SharpGL.Serialization.Caligari
{
    internal class CaligariFileHeader
    {
        public virtual void Read(BinaryReader stream)
        {
            //	Read the data.
            id = new string(stream.ReadChars(9));
            version = new string(stream.ReadChars(6));
            dataMode = stream.ReadChar();
            bitFormat = new string(stream.ReadChars(2));

            //	Skip past the pad and newline, 14 chars.
            stream.ReadChars(14);
        }

        public virtual void Write(BinaryWriter writer)
        {
            //	Write the data.
            writer.Write(id.ToCharArray(), 0, id.Length);
            writer.Write(version.ToCharArray(), 0, version.Length);
            writer.Write(dataMode);
            writer.Write(bitFormat.ToCharArray(), 0, bitFormat.Length);

            //	Skip past the pad and newline, 14 chars.
            writer.Write(new byte[14], 0, 14);
        }

        public string id = "Caligari ";
        public string version = "V04.00";
        public char dataMode = 'B';		//	'A' = ASCII, 'B' = Binary
        public string bitFormat = "HL";	//	'LH' = Little Endian, 'HL' = High Endian
    }

    internal class CaligariChunkHeader
    {
        public virtual void Read(BinaryReader stream)
        {
            //	Read the data.
            chunkType = new string(stream.ReadChars(4));
            majorVersion = stream.ReadInt16();
            minorVersion = stream.ReadInt16();
            chunkID = stream.ReadInt32();
            parentID = stream.ReadInt32();
            dataBytes = stream.ReadInt32();
        }

        public string chunkType;
        public short majorVersion;
        public short minorVersion;
        public long chunkID;
        public long parentID;
        public long dataBytes;
    }
}
