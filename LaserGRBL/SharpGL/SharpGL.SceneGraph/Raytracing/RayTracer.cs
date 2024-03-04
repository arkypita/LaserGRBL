using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using SharpGL.SceneGraph.Cameras;
using SharpGL.SceneGraph.Core;

namespace SharpGL.SceneGraph.Raytracing
{
    /// <summary>
    /// The Ray Tracer is an engine that renders a scene using the raytracing mechanism.
    /// </summary>
    public class RayTracer
    {
        /// <summary>
        /// Renders the specified scene.
        /// </summary>
        /// <param name="scene">The scene.</param>
        /// <param name="camera">The camera.</param>
        /// <returns>
        /// The scene rendered with raytracing.
        /// </returns>
        public Image Render(Scene scene, Camera camera)
        {
            //  Useful references.
            OpenGL gl = scene.CurrentOpenGLContext;

            //	First, we need the matricies and viewport.
            double[] modelview = new double[16];
            double[] projection = new double[16];
            int[] viewport = new int[4];
            gl.GetDouble(OpenGL.GL_MODELVIEW_MATRIX, modelview);
            gl.GetDouble(OpenGL.GL_PROJECTION_MATRIX, projection);
            gl.GetInteger(OpenGL.GL_VIEWPORT, viewport);
            int screenwidth = viewport[2];
            int screenheight = viewport[3];

            //	From frustum data, we make a screen origin, and s/t vectors.
            Vertex s = new Vertex(0, 0.03f, 0);
            Vertex t = new Vertex(0, 0, 0.05f);
            Vertex vScreenOrigin = new Vertex(0, 0, 5);

            //	Go through every pixel we have, and convert it into a screen pixel.
            ScreenPixel[] pixels = new ScreenPixel[viewport[2] * viewport[3]];

            for (int y = 0; y < screenheight; y++)
            {
                for (int x = 0; x < screenwidth; x++)
                {
                    //	Get plane coordinates first of all.
                    int planeX = x - (screenwidth / 2);
                    int planeY = y - (screenwidth / 2);

                    float worldX = vScreenOrigin.X + (planeX * t.X) + (planeY * s.X);
                    float worldY = vScreenOrigin.Y + (planeX * t.Y) + (planeY * s.Y);
                    float worldZ = vScreenOrigin.Z + (planeX * t.Z) + (planeY * s.Z);

                    //	Finally, pack all that data into a ScreenPixel.
                    ScreenPixel pixel = new ScreenPixel();
                    pixel.x = x;
                    pixel.y = y;
                    pixel.worldpos = new Vertex(worldX, worldY, worldZ);
                    pixel.ray.origin = camera.Position;
                    pixel.ray.direction = pixel.worldpos - camera.Position;

                    pixels[(y * viewport[2]) + x] = pixel;
                }
            }

            //  Create the resulting bitmap.
            System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(viewport[2], viewport[3]);

            //	Now go through every ray and test for intersections.
            int pixelcounter = 0;
            int pixelcount = viewport[2] * viewport[3];
            foreach (ScreenPixel pix in pixels)
            {
                //	Raytrace the polygons.
                Intersection closest = new Intersection();
                foreach (var raytracable in scene.SceneContainer.Traverse(se => se is IRayTracable))
                {
                    Intersection i = ((IRayTracable)raytracable).Raytrace(pix.ray, scene);
                    if (i.intersected && (closest.intersected == false || i.closeness < closest.closeness))
                        closest = i;
                }

                if (closest.intersected == true)
                {
                    System.Console.WriteLine("i = {0}, only {1} left!\n",
                        closest.closeness, pixelcount - pixelcounter);
                }
                bmp.SetPixel(pix.x, pix.y, pix.ray.light);
                pixelcounter++;


            }

            //  Return the ray traced imag.
            return bmp;
        }
    }
}
