using System;
using System.IO;
using SharpGL.SceneGraph;

namespace SharpGL.Serialization.Discreet
{
	public class Discreet3dsFormat : IFileFormat
	{
        /*
		protected override object LoadData(Stream stream)
		{
			
		}

		protected override bool SaveData(object data, Stream stream)
		{
			//	Haven't yet created this code.
			return false;
		}

		public override string[] FileTypes
		{
			
		}

		public override string Filter
		{
			
		}

		public override Type[] DataTypes
		{
			get {return new Type[] {typeof(Scene)};}
		}*/
        public Scene LoadData(string path)
        {
            //  Create a null scene.
            Scene scene = null;

            //  Open the file stream.
            using (var fileStream = new FileStream(path, FileMode.Open))
            {
                using (var reader = new BinaryReader(fileStream, System.Text.Encoding.ASCII))
                {
                    //	Create a new scene to load to.
                    scene = new Scene();

                    //	Peep the first chunk to make sure it's a 'main' chunk.
                    if (MAXChunkHeader.Peep(reader).type != ChunkType.CHUNK_MAIN)
                        return null;

                    //	The first chunk is always the main chunk, so read it.
                    MainChunk main = new MainChunk();
                    main.Read(scene, reader);
                }
            }            

            //  Return the scene.
            return scene;
        }

        public bool SaveData(Scene scene, string path)
        {
            throw new NotImplementedException();
        }

        public string[] FileTypes
        {
            get { return new string[] { "3ds" }; }
        }

        public string Filter
        {
            get { return "3D Studio MAX Files (*.3ds)|*.3ds"; }
        }
    }
}