using System;
using System.Globalization;
using System.IO;
using System.Linq;
using SharpGL.SceneGraph;
using SharpGL.SceneGraph.Assets;
using SharpGL.SceneGraph.Core;
using SharpGL.SceneGraph.Primitives;
using Index = SharpGL.SceneGraph.Index;

namespace SharpGL.Serialization.Wavefront
{
    public class ObjFileFormat : IFileFormat
    {
        public Scene LoadData(string path)
        {
            char[] split = new char[] { ' ' };

            //  Create a scene and polygon.
            Scene scene = new Scene();
            Polygon polygon = new Polygon();

            string mtlName = null;

            //  Create a stream reader.
            using (StreamReader reader = new StreamReader(path))
            {
                //  Read line by line.
                string line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    //  Skip any comments (lines that start with '#').
                    if (line.StartsWith("#"))
                        continue;

                    //  Do we have a texture coordinate?
                    if (line.StartsWith("vt"))
                    {
                        //  Get the texture coord strings.
                        string[] values = line.Substring(3).Split(split, StringSplitOptions.RemoveEmptyEntries);
                        float x = float.Parse(values[0], CultureInfo.InvariantCulture);
                        float y = float.Parse(values[1], CultureInfo.InvariantCulture);

                        //  Parse texture coordinates.
                        float u = x;
                        float v = 1.0f - y;

                        //  Add the texture coordinate.
                        polygon.UVs.Add(new UV(u, v));
                        continue;
                    }

                    //  Do we have a normal coordinate?
                    if (line.StartsWith("vn"))
                    {
                        //  Get the normal coord strings.
                        string[] values = line.Substring(3).Split(split, StringSplitOptions.RemoveEmptyEntries);

                        //  Parse normal coordinates.
                        float x = float.Parse(values[0], CultureInfo.InvariantCulture);
                        float y = float.Parse(values[1], CultureInfo.InvariantCulture);
                        float z = float.Parse(values[2], CultureInfo.InvariantCulture);

                        //  Add the normal.
                        polygon.Normals.Add(new Vertex(x, y, z));
                        continue;
                    }

                    //  Do we have a vertex?
                    if (line.StartsWith("v"))
                    {
                        //  Get the vertex coord strings.
                        string[] values = line.Substring(2).Split(split, StringSplitOptions.RemoveEmptyEntries);

                        //  Parse vertex coordinates.
                        float x = float.Parse(values[0], CultureInfo.InvariantCulture);
                        float y = float.Parse(values[1], CultureInfo.InvariantCulture);
                        float z = float.Parse(values[2], CultureInfo.InvariantCulture);

                        //   Add the vertices.
                        polygon.Vertices.Add(new Vertex(x, y, z));

                        continue;
                    }

                    //  Do we have a face?
                    if (line.StartsWith("f"))
                    {
                        Face face = new Face();

                        if (!string.IsNullOrWhiteSpace(mtlName))
                            face.Material = scene.Assets.FirstOrDefault(t => t.Name == mtlName) as Material;

                        //  Get the face indices
                        string[] indices = line.Substring(2).Split(split, StringSplitOptions.RemoveEmptyEntries);

                        //  Add each index.
                        foreach (var index in indices)
                        {
                            //  Split the parts.
                            string[] parts = index.Split(new char[] { '/' }, StringSplitOptions.None);

                            //  Add each part.
                            face.Indices.Add(new Index(
                                (parts.Length > 0 && parts[0].Length > 0) ? int.Parse(parts[0], CultureInfo.InvariantCulture) - 1 : -1,
                                (parts.Length > 1 && parts[1].Length > 0) ? int.Parse(parts[1], CultureInfo.InvariantCulture) - 1 : -1,
                                (parts.Length > 2 && parts[2].Length > 0) ? int.Parse(parts[2], CultureInfo.InvariantCulture) - 1 : -1));
                        }

                        //  Add the face.
                        polygon.Faces.Add(face);
                        continue;
                    }

                    if (line.StartsWith("mtllib"))
                    {
                        // Load materials file.
                        string mtlPath = ReadMaterialValue(line);
                        if (!Path.IsPathRooted(mtlPath))
                            // It's better to build an absolute path here, rather than
                            // messing with Environment.CurrentDirectory
                            mtlPath = Path.Combine(Path.GetDirectoryName(path), mtlPath);

                        LoadMaterials(mtlPath, scene);
                    }

                    if (line.StartsWith("usemtl"))
                        mtlName = ReadMaterialValue(line);
                }
            }

            scene.SceneContainer.AddChild(polygon);

            return scene;
        }

        public bool SaveData(Scene scene, string path)
        {
            throw new NotImplementedException("The SaveData method has not been implemented for .obj files.");
            //return SaveData(scene, scene.SceneContainer, path);
        }

        private System.Drawing.Color ReadMaterialColor(string line, float alpha)
        {
            string[] lineParts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (lineParts.Length >= 4)
            {
                // Convert float a,r,g,b values to byte values.  Make sure they fall in 0-255 range.
                int a = Convert.ToInt32(255 * alpha);
                a = a < 0 ? 0 : a > 255 ? 255 : a;
                int r = Convert.ToInt32(255 * float.Parse(lineParts[1], CultureInfo.InvariantCulture));
                r = r < 0 ? 0 : r > 255 ? 255 : r;
                int g = Convert.ToInt32(255 * float.Parse(lineParts[2], CultureInfo.InvariantCulture));
                g = g < 0 ? 0 : g > 255 ? 255 : g;
                int b = Convert.ToInt32(255 * float.Parse(lineParts[3], CultureInfo.InvariantCulture));
                b = b < 0 ? 0 : b > 255 ? 255 : b;

                return System.Drawing.Color.FromArgb(a, r, g, b);
            }
            else
                return System.Drawing.Color.White;
        }

        private void SetAlphaForMaterial(Material material, float alpha)
        {
            int a = Convert.ToInt32(255 * alpha);
            material.Ambient = System.Drawing.Color.FromArgb(a, material.Ambient);
            material.Diffuse = System.Drawing.Color.FromArgb(a, material.Diffuse);
            material.Specular = System.Drawing.Color.FromArgb(a, material.Specular);
            material.Emission = System.Drawing.Color.FromArgb(a, material.Emission);
        }

        private string ReadMaterialValue(string line)
        {
            //  The material is everything after the first space.
            int spacePos = line.IndexOf(' ');
            if (spacePos == -1 || (spacePos + 1) >= line.Length)
                return null;

            //  Return the material path.
            return line.Substring(spacePos + 1).Trim();
        }

        private void LoadMaterials(string path, Scene scene)
        {
            //  Create a stream reader.
            using (StreamReader reader = new StreamReader(path))
            {
                Material mtl = null;
                float alpha = 1f;

                //  Read line by line.
                string line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();

                    //  Skip any comments (lines that start with '#').
                    if (line.StartsWith("#"))
                        continue;

                    // newmtl indicates start of material definition.
                    if (line.StartsWith("newmtl"))
                    {
                        // Add new material to scene's assets.
                        mtl = new Material();
                        scene.Assets.Add(mtl);

                        // Name of material is on same line, immediately follows newmatl.
                        mtl.Name = ReadMaterialValue(line);

                        // Reset assumed alpha.
                        alpha = 1f;
                    }

                    // Read properties of material.
                    if (mtl != null)
                    {
                        if (line.StartsWith("Ka"))
                            mtl.Ambient = ReadMaterialColor(line, alpha);
                        else if (line.StartsWith("Kd"))
                            mtl.Diffuse = ReadMaterialColor(line, alpha);
                        else if (line.StartsWith("Ks"))
                            mtl.Specular = ReadMaterialColor(line, alpha);
                        else if (line.StartsWith("Ke"))
                            mtl.Emission = ReadMaterialColor(line, alpha);
                        else if (line.StartsWith("Ns"))
                            mtl.Shininess = float.Parse(ReadMaterialValue(line), CultureInfo.InvariantCulture);
                        else if (line.StartsWith("map_Ka") ||
                            line.StartsWith("map_Kd") ||
                            line.StartsWith("map_Ks"))
                        {
                            // Get texture map.                    		
                            string textureFile = ReadMaterialValue(line);

                            // Check for existing textures.  Create if does not exist.
                            Texture theTexture = null;
                            var existingTextures = scene.Assets.Where(t => t is Texture && t.Name == textureFile);
                            if (existingTextures.Count() >= 1)
                                theTexture = existingTextures.FirstOrDefault() as Texture;
                            else
                            {
                                //  Does the texture file exist?
                                if (File.Exists(textureFile) == false)
                                {
                                    //  It doesn't, assume its in the same location
                                    //  as the mtl file.
                                    textureFile = Path.Combine(Path.GetDirectoryName(path),
                                        Path.GetFileName(textureFile));
                                }

                                // Create/load texture.
                                theTexture = new Texture();
                                theTexture.Create(scene.CurrentOpenGLContext, textureFile);
                            }

                            // Set texture for material.
                            mtl.Texture = theTexture;
                        }
                        else if (line.StartsWith("d") || line.StartsWith("Tr"))
                        {
                            alpha = float.Parse(ReadMaterialValue(line), CultureInfo.InvariantCulture);
                            SetAlphaForMaterial(mtl, alpha);
                        }

                        // TODO: Handle illumination mode (illum)                    	                    
                    }
                }
            }
        }
                
        //public bool SaveData(Scene scene, SceneElement element, string path)
        //{
        //    string mtlPath = Path.ChangeExtension(path, ".mtl");
        //    string shortMtlPath = Path.GetFileName(mtlPath);
        //    SaveMaterials(mtlPath, scene);
        //    using (StreamWriter writer = new StreamWriter(path))
        //    {
        //        writer.WriteLine("mtllib {0}", shortMtlPath);
        //        string objName = "";
        //        string mtlName = "";
        //        WriteSceneElement(writer, element, ref objName, ref mtlName);
        //        writer.Flush();
        //        writer.Close();
        //    }

        //    return true;
        //}

        //private void WriteSceneElement(StreamWriter writer, SceneElement element, ref string currentObjectName, ref string currentMaterialName)
        //{
        //    // If object name different than for last element processed, write a g(roup) statement.
        //    if (!string.IsNullOrWhiteSpace(element.Name) &&
        //        element.Name != currentObjectName)
        //    {
        //        currentObjectName = element.Name;
        //        writer.WriteLine("g {0}", currentObjectName);
        //    }

        //    // If material name different than for last element processed, write a usemtl statement.
        //    if (element is IHasMaterial)
        //    {
        //        IHasMaterial hasMaterial = (IHasMaterial)element;
        //        if (hasMaterial.Material != null &&
        //            !string.IsNullOrWhiteSpace(hasMaterial.Material.Name) &&
        //            hasMaterial.Material.Name != currentMaterialName)
        //        {
        //            currentMaterialName = hasMaterial.Material.Name;
        //            writer.WriteLine("usemtl {0}", currentMaterialName);
        //        }
        //    }

        //    // Write out this element.
        //    if (element is Polygon)
        //    {
        //        Polygon poly = (Polygon)element;
        //        foreach (Face face in poly.Faces)
        //        {
        //            // If material name different than for last face processed, write a usemtl statement.
        //            if (face.Material != null &&
        //                !string.IsNullOrWhiteSpace(face.Material.Name) &&
        //                face.Material.Name != currentMaterialName)
        //            {
        //                currentMaterialName = face.Material.Name;
        //                writer.WriteLine("usemtl {0}", currentMaterialName);
        //            }

        //            // Write out the vertices.
        //            foreach (Index i in face.Indices)
        //            {
        //                Vertex v = poly.Vertices[i.Vertex];
        //                writer.WriteLine("v {0} {1} {2}",
        //                    v.X.ToString("F6", CultureInfo.InvariantCulture),
        //                    v.Y.ToString("F6", CultureInfo.InvariantCulture),
        //                    v.Z.ToString("F6", CultureInfo.InvariantCulture));
        //            }
        //        }
        //    }

        //    // TODO: Handle shapes other than polygons.

        //    // Write out any child elements.
        //    foreach (SceneElement child in element.Children)
        //        WriteSceneElement(writer, child, ref currentObjectName, ref currentMaterialName);

        //    writer.WriteLine();
        //}

        //private void WriteMaterialColor(StreamWriter writer, string name, System.Drawing.Color color)
        //{
        //    float r = color.R / 255F;
        //    float g = color.G / 255F;
        //    float b = color.B / 255F;
        //    writer.WriteLine("{0} {1} {2} {3}", name,
        //        r.ToString("F6", CultureInfo.InvariantCulture),
        //        g.ToString("F6", CultureInfo.InvariantCulture),
        //        b.ToString("F6", CultureInfo.InvariantCulture));
        //}

        //private void WriteMaterialValue(StreamWriter writer, string name, string value)
        //{
        //    writer.WriteLine("{0} {1}", name, value);
        //}

        //private void SaveMaterials(string path, Scene scene)
        //{
        //    using (StreamWriter writer = new StreamWriter(path))
        //    {
        //        foreach (Material mtl in scene.Assets.Where(asset => asset is Material))
        //        {
        //            if (mtl == null) continue;

        //            writer.WriteLine("newmtl {0}", mtl.Name);
        //            WriteMaterialColor(writer, "Ka", mtl.Ambient);
        //            WriteMaterialColor(writer, "Kd", mtl.Diffuse);
        //            WriteMaterialColor(writer, "Ks", mtl.Specular);
        //            // TODO: Shininess	        		
        //            WriteMaterialValue(writer, "Tr", (mtl.Diffuse.A / 255f).ToString("F6", CultureInfo.InvariantCulture));
        //            // TODO: Illumination model
        //            // TODO: Textures

        //            writer.WriteLine();
        //        }

        //        writer.Flush();
        //        writer.Close();
        //    }
        //}

        public string[] FileTypes
        {
            get { return new string[] { "obj" }; }
        }

        public string Filter
        {
            get { return "Wavefont Obj Files (*.obj)|*.obj"; }
        }
    }
}
