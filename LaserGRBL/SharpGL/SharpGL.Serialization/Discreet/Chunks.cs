using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpGL.SceneGraph;
using System.IO;
using SharpGL.SceneGraph.Collections;
using SharpGL.SceneGraph.Primitives;
using SharpGL.SceneGraph.Core;
using Index = SharpGL.SceneGraph.Index;

namespace SharpGL.Serialization.Discreet
{
    public enum ChunkType : ushort
    {
        CHUNK_RGBF = 0x0010,
        CHUNK_RGBB = 0x0011,
        CHUNK_MAIN = 0x4D4D,
        CHUNK_OBJMESH = 0x3D3D,
        CHUNK_BKGCOLOR = 0x1200,
        CHUNK_AMBCOLOR = 0x2100,
        CHUNK_OBJBLOCK = 0x4000,
        CHUNK_TRIMESH = 0x4100,
        CHUNK_VERTLIST = 0x4110,
        CHUNK_FACELIST = 0x4120,
        CHUNK_FACEMAT = 0x4130,
        CHUNK_MAPLIST = 0x4140,
        CHUNK_SMOOLIST = 0x4150,
        CHUNK_TRMATRIX = 0x4160,
        CHUNK_LIGHT = 0x4600,
        CHUNK_SPOTLIGHT = 0x4610,
        CHUNK_CAMERA = 0x4700,
        CHUNK_MATERIAL = 0xAFFF,
        CHUNK_MATNAME = 0xA000,
        CHUNK_AMBIENT = 0xA010,
        CHUNK_DIFFUSE = 0xA020,
        CHUNK_SPECULAR = 0xA030,
        CHUNK_TEXTURE = 0xA200,
        CHUNK_BUMPMAP = 0xA230,
        CHUNK_MAPFILE = 0xA300,
        CHUNK_KEYFRAMER = 0xB000,
        CHUNK_FRAMES = 0xB008
    };

    internal class MAXChunkHeader
    {
        public virtual void Read(BinaryReader stream)
        {
            //	Read the data.
            type = (ChunkType)stream.ReadInt16();
            allDataBytes = stream.ReadInt32();
            dataBytes = allDataBytes - 6; //	data - header
        }

        public static MAXChunkHeader Peep(BinaryReader stream)
        {
            //	Read a header.
            MAXChunkHeader header = new MAXChunkHeader();
            header.Read(stream);

            //	Go back.
            stream.BaseStream.Seek(-6, System.IO.SeekOrigin.Current);

            //	Return the header.
            return header;
        }

        public ChunkType type;
        public long allDataBytes;
        public long dataBytes;
    };

    internal class MAXChunk
    {
        public virtual void Read(Scene scene, BinaryReader stream)
        {
            //	Set the start position.
            startPosition = stream.BaseStream.Position;

            //	Read the header.
            ReadHeader(stream);

            //	Read the data itself.
            ReadData(scene, stream);
        }

        protected virtual void ReadHeader(BinaryReader stream)
        {
            //	Read the chunk header.
            chunkHeader.Read(stream);
        }

        /// <summary>
        /// This function reads the chunk and bangs the data in it into the scene.
        /// </summary>
        /// <param name="stream">The file stream to read from.</param>
        /// <param name="scene">The scene to put data into.</param>
        public virtual void ReadData(Scene scene, BinaryReader stream)
        {
            //	This is the code that is executed when an unknown chunk is read.

            //	Advance the stream.
            stream.BaseStream.Seek(chunkHeader.dataBytes, System.IO.SeekOrigin.Current);
        }

        public virtual bool MoreChunks(BinaryReader reader)
        {
            if (reader.BaseStream.Position < (startPosition + chunkHeader.allDataBytes))
                return true;
            return false;
        }

        public MAXChunkHeader chunkHeader = new MAXChunkHeader();
        long startPosition = 0;
    }

    internal class MainChunk : MAXChunk
    {
        public override void ReadData(Scene scene, BinaryReader stream)
        {
            do
            {
                //	Peep at the next chunk.
                MAXChunkHeader next = MAXChunkHeader.Peep(stream);

                //	If it's an Object Mesh, we can read that.
                if (next.type == ChunkType.CHUNK_OBJMESH)
                {
                    ObjectMeshChunk chunk = new ObjectMeshChunk();
                    chunk.Read(scene, stream);
                }
                else
                {
                    //	We don't know what this chunk is, so just read the generic one.
                    MAXChunk chunk = new MAXChunk();
                    chunk.Read(scene, stream);
                }

            } while (MoreChunks(stream));
        }
    }

    internal class ObjectMeshChunk : MAXChunk
    {
        public override void ReadData(Scene scene, BinaryReader stream)
        {
            do
            {
                //	Peep at the next chunk.
                MAXChunkHeader next = MAXChunkHeader.Peep(stream);

                //	If it's an Object Block, we can read that.
                if (next.type == ChunkType.CHUNK_OBJBLOCK)
                {
                    ObjectBlockChunk chunk = new ObjectBlockChunk();
                    chunk.Read(scene, stream);
                }
                else
                {
                    //	We don't know what this chunk is, so just read the generic one.
                    MAXChunk chunk = new MAXChunk();
                    chunk.Read(scene, stream);
                }

            } while (MoreChunks(stream));
        }
    }

    internal class ObjectBlockChunk : MAXChunk
    {
        public override void ReadData(Scene scene, BinaryReader stream)
        {
            do
            {
                //	Peep at the next chunk.
                MAXChunkHeader next = MAXChunkHeader.Peep(stream);

                //	If it's an Trimesh Block, we can read that.
                if (next.type == ChunkType.CHUNK_TRIMESH)
                {
                    TriangleMeshChunk chunk = new TriangleMeshChunk();
                    chunk.Read(scene, stream);
                }
                else
                {
                    //	We don't know what this chunk is, so just read the generic one.
                    MAXChunk chunk = new MAXChunk();
                    chunk.Read(scene, stream);
                }

            } while (MoreChunks(stream));
        }
    }

    internal class TriangleMeshChunk : MAXChunk
    {
        public override void ReadData(Scene scene, BinaryReader stream)
        {
            //	A triangle mesh is basicly a Polygon, so create it.
            Polygon poly = new Polygon();
            Matrix matrix = new Matrix(Matrix.Identity(4));

            do
            {
                //	Peep at the next chunk.
                MAXChunkHeader next = MAXChunkHeader.Peep(stream);

                if (next.type == ChunkType.CHUNK_VERTLIST)
                {
                    //	Read the vertices.
                    VertexListChunk chunk = new VertexListChunk();
                    chunk.Read(scene, stream);

                    //	Set them into the polygon.
                    poly.Vertices = chunk.vertices;
                }
                else if (next.type == ChunkType.CHUNK_FACELIST)
                {
                    //	Read the faces.
                    FaceListChunk chunk = new FaceListChunk();
                    chunk.Read(scene, stream);

                    //	Set them into the polygon.
                    poly.Faces = chunk.faces;
                }
                else if (next.type == ChunkType.CHUNK_MAPLIST)
                {
                    //	Read the uvs.
                    MapListChunk chunk = new MapListChunk();
                    chunk.Read(scene, stream);

                    //	Set them into the polygon.
                    poly.UVs = chunk.uvs;
                }
                else if (next.type == ChunkType.CHUNK_TRMATRIX)
                {
                    //	Here we just read the matrix (we'll use it later).
                    TrMatrixChunk chunk = new TrMatrixChunk();
                    chunk.Read(scene, stream);

                    matrix = chunk.matrix;
                }
                else
                {
                    //	We don't know what this chunk is, so just read the generic one.
                    MAXChunk chunk = new MAXChunk();
                    chunk.Read(scene, stream);
                }

            } while (MoreChunks(stream));

            //	Now we multiply each vertex by the matrix.
            for (int i = 0; i < poly.Vertices.Count; i++)
                poly.Vertices[i] *= matrix;

            //	Add the poly to the scene.
            scene.SceneContainer.AddChild(poly);
        }
    }

    internal class VertexListChunk : MAXChunk
    {
        public override void ReadData(Scene scene, BinaryReader stream)
        {
            //	Read number of vertices.
            short vertexCount = 0;
            vertexCount = stream.ReadInt16();

            //	Read each vertex and add it.
            for (short i = 0; i < vertexCount; i++)
            {
                Vertex v = new Vertex();
                v.X = stream.ReadSingle();
                v.Y = stream.ReadSingle();
                v.Z = stream.ReadSingle();
                vertices.Add(v);
            }
        }

        public List<Vertex> vertices = new List<Vertex>();
    }

    internal class FaceListChunk : MAXChunk
    {
        public override void ReadData(Scene scene, BinaryReader stream)
        {
            //	Note: A max face is three indices and
            //	a flag short.

            //	Read number of faces.
            short faceCount = 0;
            faceCount = stream.ReadInt16();

            //	Read each face and add it.
            for (short i = 0; i < faceCount; i++)
            {
                Face f = new Face();
                Index index = new Index(stream.ReadInt16());
                f.Indices.Add(index);
                index = new Index(stream.ReadInt16());
                f.Indices.Add(index);
                index = new Index(stream.ReadInt16());
                f.Indices.Add(index);
                stream.ReadInt16();
                faces.Add(f);
            }
        }

        public List<Face> faces = new List<Face>();
    }

    internal class MapListChunk : MAXChunk
    {
        public override void ReadData(Scene scene, BinaryReader stream)
        {
            //	Read number of uvs.
            short uvCount = 0;
            uvCount = stream.ReadInt16();

            //	Read each uv and add it.
            for (short i = 0; i < uvCount; i++)
            {
                UV uv = new UV();
                uv.U = stream.ReadSingle();
                uv.V = stream.ReadSingle();
                uvs.Add(uv);
            }
        }

        public List<UV> uvs = new List<UV>();
    }

    internal class TrMatrixChunk : MAXChunk
    {
        public override void ReadData(Scene scene, BinaryReader stream)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 4; j++)
                    matrix[i,j] = stream.ReadSingle();
            }

            matrix.Transpose();
        }

        public Matrix matrix = new Matrix(4,4);
    }
}
