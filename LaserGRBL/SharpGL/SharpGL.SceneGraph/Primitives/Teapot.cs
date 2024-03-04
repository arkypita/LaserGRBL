using System;
using System.Runtime.InteropServices;
using SharpGL.SceneGraph.Core;
using SharpGL.SceneGraph.Assets;

namespace SharpGL.SceneGraph.Primitives
{
    /// <summary>
    /// The standard OpenGL teapot.
    /// </summary>
    public class Teapot : 
        SceneElement, 
        IRenderable,
        IHasMaterial
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Teapot"/> class.
        /// </summary>
        public Teapot()
        {
            Name = "Teapot";
            Grid = 14;
            Scale = 1;
            FillType = OpenGL.GL_FILL;
        }

        /// <summary>
        /// Render to the provided instance of OpenGL.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        /// <param name="renderMode">The render mode.</param>
        public void Render(OpenGL gl, RenderMode renderMode)
        {
            Draw(gl, Grid, Scale, FillType);
        }

        /// <summary>
        /// Draws the specified gl.
        /// </summary>
        /// <param name="gl">The gl.</param>
        /// <param name="grid">The grid.</param>
        /// <param name="scale">The scale.</param>
        /// <param name="type">The type.</param>
        public void Draw(OpenGL gl, int grid, double scale, uint type)
        {
            float[, ,] p = new float[4, 4, 3];
            float[, ,] q = new float[4, 4, 3];
            float[, ,] r = new float[4, 4, 3];
            float[, ,] s = new float[4, 4, 3];

            gl.PushAttrib(OpenGL.GL_ENABLE_BIT | OpenGL.GL_EVAL_BIT);
            gl.Enable(OpenGL.GL_AUTO_NORMAL);
            gl.Enable(OpenGL.GL_NORMALIZE);
            gl.Enable(OpenGL.GL_MAP2_VERTEX_3);
            gl.Enable(OpenGL.GL_MAP2_TEXTURE_COORD_2);
            gl.PushMatrix();
            gl.Rotate(270.0f, 1.0f, 0.0f, 0.0f);
            gl.Scale(0.5f * scale, 0.5f * scale, 0.5f * scale);
            gl.Translate(0.0, 0.0, -1.5);

            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        for (int l = 0; l < 3; l++)
                        {
                            p[j,k,l] = cpdata[patchdata[i,(j * 4 + k)],l];
                            q[j,k,l] = cpdata[patchdata[i,(j * 4 + (3 - k))],l];
                            if (l == 1)
                                q[j,k,l] *= -1.0f;
                            if (i < 6)
                            {
                                r[j,k,l] =
                                  cpdata[patchdata[i,(j * 4 + (3 - k))],l];
                                if (l == 0)
                                    r[j,k,l] *= -1.0f;
                                s[j,k,l] = cpdata[patchdata[i,(j * 4 + k)],l];
                                if (l == 0)
                                    s[j,k,l] *= -1.0f;
                                if (l == 1)
                                    s[j,k,l] *= -1.0f;
                            }
                        }
                    }
                }
                gl.Map2(OpenGL.GL_MAP2_TEXTURE_COORD_2, 0.0f, 1.0f, 2, 2, 0.0f, 1.0f, 4, 2,
                  tex.Flatten());
                gl.Map2(OpenGL.GL_MAP2_VERTEX_3, 0, 1, 3, 4, 0, 1, 12, 4,
                  p.Flatten());
                gl.MapGrid2(grid, 0.0, 1.0, grid, 0.0, 1.0);
                gl.EvalMesh2(type, 0, grid, 0, grid);
                gl.Map2(OpenGL.GL_MAP2_VERTEX_3, 0, 1, 3, 4, 0, 1, 12, 4,
                  q.Flatten());
                gl.EvalMesh2(type, 0, grid, 0, grid);
                if (i < 6)
                {
                    gl.Map2(OpenGL.GL_MAP2_VERTEX_3, 0, 1, 3, 4, 0, 1, 12, 4,
                      r.Flatten());
                    gl.EvalMesh2(type, 0, grid, 0, grid);
                    gl.Map2(OpenGL.GL_MAP2_VERTEX_3, 0, 1, 3, 4, 0, 1, 12, 4,
                      s.Flatten());
                    gl.EvalMesh2(type, 0, grid, 0, grid);
                }
            }


            gl.PopMatrix();
            gl.PopAttrib();
        }

        int[,] patchdata =
  {
      /* rim */
    {102, 103, 104, 105, 4, 5, 6, 7, 8, 9, 10, 11,
      12, 13, 14, 15},
      /* body */
    {12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23,
      24, 25, 26, 27},
    {24, 25, 26, 27, 29, 30, 31, 32, 33, 34, 35, 36,
      37, 38, 39, 40},
      /* lid */
    {96, 96, 96, 96, 97, 98, 99, 100, 101, 101, 101,
      101, 0, 1, 2, 3,},
    {0, 1, 2, 3, 106, 107, 108, 109, 110, 111, 112,
      113, 114, 115, 116, 117},
      /* bottom */
    {118, 118, 118, 118, 124, 122, 119, 121, 123, 126,
      125, 120, 40, 39, 38, 37},
      /* handle */
    {41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52,
      53, 54, 55, 56},
    {53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64,
      28, 65, 66, 67},
      /* spout */
    {68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79,
      80, 81, 82, 83},
    {80, 81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 91,
      92, 93, 94, 95}
  };

        float[,] cpdata = 
  {
      {0.2f, 0f, 2.7f}, {0.2f, -0.112f, 2.7f}, {0.112f, -0.2f, 2.7f}, {0f,
      -0.2f, 2.7f}, {1.3375f, 0f, 2.53125f}, {1.3375f, -0.749f, 2.53125f},
      {0.749f, -1.3375f, 2.53125f}, {0f, -1.3375f, 2.53125f}, {1.4375f,
      0f, 2.53125f}, {1.4375f, -0.805f, 2.53125f}, {0.805f, -1.4375f,
      2.53125f}, {0f, -1.4375f, 2.53125f}, {1.5f, 0f, 2.4f}, {1.5f, -0.84f,
      2.4f}, {0.84f, -1.5f, 2.4f}, {0f, -1.5f, 2.4f}, {1.75f, 0f, 1.875f},
      {1.75f, -0.98f, 1.875f}, {0.98f, -1.75f, 1.875f}, {0f, -1.75f,
      1.875f}, {2f, 0f, 1.35f}, {2f, -1.12f, 1.35f}, {1.12f, -2f, 1.35f},
      {0f, -2f, 1.35f}, {2f, 0f, 0.9f}, {2f, -1.12f, 0.9f}, {1.12f, -2f,
      0.9f}, {0f, -2f, 0.9f}, {-2f, 0f, 0.9f}, {2f, 0f, 0.45f}, {2f, -1.12f,
      0.45f}, {1.12f, -2f, 0.45f}, {0f, -2f, 0.45f}, {1.5f, 0f, 0.225f},
      {1.5f, -0.84f, 0.225f}, {0.84f, -1.5f, 0.225f}, {0f, -1.5f, 0.225f},
      {1.5f, 0f, 0.15f}, {1.5f, -0.84f, 0.15f}, {0.84f, -1.5f, 0.15f}, {0f,
      -1.5f, 0.15f}, {-1.6f, 0f, 2.025f}, {-1.6f, -0.3f, 2.025f}, {-1.5f,
      -0.3f, 2.25f}, {-1.5f, 0f, 2.25f}, {-2.3f, 0f, 2.025f}, {-2.3f, -0.3f,
      2.025f}, {-2.5f, -0.3f, 2.25f}, {-2.5f, 0f, 2.25f}, {-2.7f, 0f,
      2.025f}, {-2.7f, -0.3f, 2.025f}, {-3f, -0.3f, 2.25f}, {-3f, 0f,
      2.25f}, {-2.7f, 0f, 1.8f}, {-2.7f, -0.3f, 1.8f}, {-3f, -0.3f, 1.8f},
      {-3f, 0f, 1.8f}, {-2.7f, 0f, 1.575f}, {-2.7f, -0.3f, 1.575f}, {-3f,
      -0.3f, 1.35f}, {-3f, 0f, 1.35f}, {-2.5f, 0f, 1.125f}, {-2.5f, -0.3f,
      1.125f}, {-2.65f, -0.3f, 0.9375f}, {-2.65f, 0f, 0.9375f}, {-2f,
      -0.3f, 0.9f}, {-1.9f, -0.3f, 0.6f}, {-1.9f, 0f, 0.6f}, {1.7f, 0f,
      1.425f}, {1.7f, -0.66f, 1.425f}, {1.7f, -0.66f, 0.6f}, {1.7f, 0f,
      0.6f}, {2.6f, 0f, 1.425f}, {2.6f, -0.66f, 1.425f}, {3.1f, -0.66f,
      0.825f}, {3.1f, 0f, 0.825f}, {2.3f, 0f, 2.1f}, {2.3f, -0.25f, 2.1f},
      {2.4f, -0.25f, 2.025f}, {2.4f, 0f, 2.025f}, {2.7f, 0f, 2.4f}, {2.7f,
      -0.25f, 2.4f}, {3.3f, -0.25f, 2.4f}, {3.3f, 0f, 2.4f}, {2.8f, 0f,
      2.475f}, {2.8f, -0.25f, 2.475f}, {3.525f, -0.25f, 2.49375f},
      {3.525f, 0f, 2.49375f}, {2.9f, 0f, 2.475f}, {2.9f, -0.15f, 2.475f},
      {3.45f, -0.15f, 2.5125f}, {3.45f, 0f, 2.5125f}, {2.8f, 0f, 2.4f},
      {2.8f, -0.15f, 2.4f}, {3.2f, -0.15f, 2.4f}, {3.2f, 0f, 2.4f}, {0f, 0f,
      3.15f}, {0.8f, 0f, 3.15f}, {0.8f, -0.45f, 3.15f}, {0.45f, -0.8f,
      3.15f}, {0f, -0.8f, 3.15f}, {0f, 0f, 2.85f}, {1.4f, 0f, 2.4f}, {1.4f,
      -0.784f, 2.4f}, {0.784f, -1.4f, 2.4f}, {0f, -1.4f, 2.4f}, {0.4f, 0f,
      2.55f}, {0.4f, -0.224f, 2.55f}, {0.224f, -0.4f, 2.55f}, {0f, -0.4f,
      2.55f}, {1.3f, 0f, 2.55f}, {1.3f, -0.728f, 2.55f}, {0.728f, -1.3f,
      2.55f}, {0f, -1.3f, 2.55f}, {1.3f, 0f, 2.4f}, {1.3f, -0.728f, 2.4f},
      {0.728f, -1.3f, 2.4f}, {0f, -1.3f, 2.4f}, {0f, 0f, 0f}, {1.425f,
      -0.798f, 0f}, {1.5f, 0f, 0.075f}, {1.425f, 0f, 0f}, {0.798f, -1.425f,
      0f}, {0f, -1.5f, 0.075f}, {0f, -1.425f, 0f}, {1.5f, -0.84f, 0.075f},
      {0.84f, -1.5f, 0.075f}
  };

        float[, ,] tex = 
      {
        { {0.0f, 0.0f},
          {1.0f, 0.0f}},
        { {0.0f, 1.0f},
          {1.0f, 1.0f}}
        };

        /// <summary>
        /// Gets or sets the grid.
        /// </summary>
        /// <value>
        /// The grid.
        /// </value>
        public int Grid
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the scale.
        /// </summary>
        /// <value>
        /// The scale.
        /// </value>
        public double Scale
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the type of the fill.
        /// </summary>
        /// <value>
        /// The type of the fill.
        /// </value>
        public uint FillType
        {
            get;
            set;
        }

        /// <summary>
        /// Material to be used when rendering the teapot in lighted mode.
        /// </summary>
        /// <value>
        /// The material.
        /// </value>
        public Material Material 
        { 
            get; 
            set; 
        }
    }

    /// <summary>
    /// Extensions for Array type.
    /// </summary>
    public static class ArrayExtensions
{
  /// <summary>
  /// Flattens the specified array.
  /// </summary>
  /// <typeparam name="T">The array type.</typeparam>
  /// <param name="array">The array.</param>
  /// <returns>The flattened array.</returns>
        public static T[] Flatten<T>(this T[,,] array)
  where T : struct
        {
            int size = Marshal.SizeOf(array[0, 0, 0]);
            int totalSize = Buffer.ByteLength(array);
            T[] result = new T[totalSize / size];
            Buffer.BlockCopy(array, 0, result, 0, totalSize);
            return result;
        }
}
}