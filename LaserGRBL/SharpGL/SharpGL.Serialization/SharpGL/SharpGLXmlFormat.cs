using System;
using System.IO;
using System.Xml.Serialization;
using SharpGL.SceneGraph;

namespace SharpGL.Serialization.SharpGL
{
    /// <summary>
    /// The SharpGL XML format.
    /// </summary>
    public class SharpGLXmlFormat : IFileFormat
    {
        /// <summary>
        /// Load the data from the specified file stream. The data
        /// should be loaded into a scene object. Also, for consistency
        /// the ObjectLoaded event should be fired every time an object
        /// (such as a polygon or material) is loaded.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        /// The scene or null if loading failed.
        /// </returns>
        public SceneGraph.Scene LoadData(string path)
        {
            Scene scene = null;

            try
            {
                using (FileStream stream = new FileStream(path, FileMode.Open))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Scene));
                    scene = (Scene)serializer.Deserialize(stream);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Failed to load Scene from XML data.", e);
            }

            return scene;
        }

        /// <summary>
        /// Saves the scene to the specified stream.
        /// </summary>
        /// <param name="scene">The scene.</param>
        /// <param name="path">The path.</param>
        /// <returns>
        /// True if saved correctly.
        /// </returns>
        public bool SaveData(Scene scene, string path)
        {
            try
            {
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(Scene));
                    serializer.Serialize(stream, scene);
                    stream.Flush();
                }
            }
            catch (Exception e)
            {
                throw new Exception("Failed to load Scene from XML data.", e);
            }
               
            return true;
        }
        
        /// <summary>
        /// This property returns an array of file types that can be used with this
        /// format, e.g the CaligariFormat would return "cob", "scn".
        /// </summary>
        public string[] FileTypes
        {
            get { return new string[] { "sglsx" }; }
        }

        /// <summary>
        /// This gets a filter suitable for a file open/save dialog, e.g
        /// "Caligari trueSpace Files (*.cob, *.scn)|*.cob;*.scn".
        /// </summary>
        public string Filter
        {
            get { return "SharpGL XML Scenes (*.sglsx)|*.sglsx"; }
        }
    }
}
