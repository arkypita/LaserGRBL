using System;
using SharpGL.SceneGraph;
using System.IO;
using SharpGL.SceneGraph.Primitives;

namespace SharpGL.Serialization.Caligari
{
    public class CaligariFileFormat : IFileFormat
    {
        protected virtual string Peep(BinaryReader reader)
        {
            //	Get the current position.
            long pos = reader.BaseStream.Position;

            //	Read a header.
            CaligariChunkHeader header = new CaligariChunkHeader();
            header.Read(reader);

            //	Move the file to it's original position.
            reader.BaseStream.Position = pos;

            //	Return the type.
            return header.chunkType;
        }


        public Scene LoadData(string path)
        {
            //  Create the scene.
            Scene scene = null;

            //  Open a file stream.
            using (var fileStream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                //  Open a binary reader.
                using (var reader = new BinaryReader(fileStream))
                {
                    //	Create a scene.
                    scene = new Scene();

                    //	First, read the header.
                    CaligariFileHeader header = new CaligariFileHeader();
                    header.Read(reader);

                    //	Here we can make sure that it really is a Caligari File.
                    if (header.id != "Caligari " || header.dataMode != 'B')
                    {
                        System.Diagnostics.Debugger.Log(1, "File I/O", "File is not internally compatible.\n");
                        return null;
                    }

                    //	Now we go through the file, peeping at chunks.
                    while (true)
                    {
                        //	Peep at the next chunk.
                        string type = Peep(reader);

                        //	Check for every type of chunk.
                        if (type == "PolH")
                        {
                            //	Read a polygon into the scene.
                            PolygonChunk polyChunk = new PolygonChunk();
                            scene.SceneContainer.AddChild((Polygon)polyChunk.Read(reader));
                        }
                        else if (type == "END ")
                        {
                            //	It's the end of the file, so we may as well break.
                            break;
                        }
                        else
                        {
                            //	Well we don't know what type it is, so just read the generic chunk.
                            CaligariChunk chunk = new CaligariChunk();
                            chunk.Read(reader);
                        }

                    }
                }
            }
            
            //  Return the scene.
            return scene;
        }

        public bool SaveData(Scene scene, string path)
        {
            throw new NotImplementedException("Cannot save to Caligari format");
        }

        /// <summary>
        /// This property returns an array of file types that can be used with this
        /// format, e.g the CaligariFormat would return "cob", "scn".
        /// </summary>
        public string[] FileTypes
        {
            get { return new string[] { "scn", "cob" }; }
        }

        /// <summary>
        /// This gets a filter suitable for a file open/save dialog, e.g
        /// "Caligari trueSpace Files (*.cob, *.scn)|*.cob;*.scn".
        /// </summary>
        public string Filter
        {
            get { return "Caligari trueSpace Scenes (*.scn)|*.scn|Caligari trueSpace Objects (*.cob)|*.cob|All Caliari Files (*.cob, *.scn)|*.cob;*.scn"; }
        }
    }
}
