using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Core;

namespace SharpGL.Serialization.Caligari
{
    internal class CaligariChunk
    {
        /// <summary>
        /// This is the one chunk that reads no scene object, use it to skip
        /// past unknown chunks.
        /// </summary>
        /// <param name="reader">The Reader to read from.</param>
        /// <returns>The object that has been read.</returns>
        public virtual object Read(BinaryReader reader)
        {
            //	Read the header.
            header.Read(reader);

            //	Return the data.
            return ReadData(reader);
        }

        /// <summary>
        /// This function writes an object to the stream.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="sceneObject">The object to write.</param>
        public virtual void Write(BinaryWriter writer, SceneElement sceneObject)
        {

        }

        /// <summary>
        /// This function reads the chunk header.
        /// </summary>
        /// <param name="reader">The Reader to read from.</param>
        protected virtual object ReadData(BinaryReader reader)
        {
            //	Skip the data, return nothing.
            data = reader.ReadBytes((int)header.dataBytes);
            return null;
        }

        /// <summary>
        /// Writes the data.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="sceneObject">The scene object.</param>
        protected virtual void WriteData(BinaryWriter writer, SceneElement sceneObject)
        {

        }

        public CaligariChunkHeader header = new CaligariChunkHeader();
        public byte[] data;
    }
}
