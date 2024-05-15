using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Xml.Serialization;

namespace SharpGL.SceneGraph.Transformations
{
    /// <summary>
    /// This enumeration describes the linear transformation order.
    /// </summary>
    public enum LinearTransformationOrder
    {
        /// <summary>
        /// Translate > Rotate > Scale
        /// </summary>
        TranslateRotateScale = 0,

        /// <summary>
        /// Rotate > Translate > Scale
        /// </summary>
        RotateTranslateScale = 1,
    }

    /// <summary>
    /// The LinearTransformation class represents a linear transformation, such
    /// as a transformation that moves us from world space into object space.
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class LinearTransformation : Transformation, IDeepCloneable<LinearTransformation>
    {
        /// <summary>
        /// Performs the transformation on the current matrix.
        /// </summary>
        /// <param name="gl">The OpenGL instance.</param>
        public override void Transform(OpenGL gl)
        {
            //  Perform the transformation in the specified order.
            switch (transformationOrder)
            {
                case LinearTransformationOrder.TranslateRotateScale:
                    gl.Translate(translateX, translateY, translateZ);
                    gl.Rotate(rotateX, rotateY, rotateZ);
                    gl.Scale(scaleX, scaleY, scaleZ);
                    break;
                case LinearTransformationOrder.RotateTranslateScale:
                    gl.Rotate(rotateX, rotateY, rotateZ);
                    gl.Translate(translateX, translateY, translateZ);
                    gl.Scale(scaleX, scaleY, scaleZ);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public LinearTransformation DeepClone()
        {
            //  Clone the object.
            return new LinearTransformation()
            {
                translateX = this.translateX,
                translateY = this.translateY,
                translateZ = this.translateZ,
                rotateX = this.rotateX,
                rotateY = this.rotateY,
                rotateZ = this.rotateZ,
                scaleX = this.scaleX,
                scaleY = this.scaleY,
                scaleZ = this.scaleZ,
                transformationOrder = this.transformationOrder
            };
        }

        /// <summary>
        /// X Component of the Translation.
        /// </summary>
        private float translateX = 0;

        /// <summary>
        /// Y Component of the Translation.
        /// </summary>
        private float translateY = 0;

        /// <summary>
        /// Z Component of the Translation.
        /// </summary>
        private float translateZ = 0;

        /// <summary>
        /// X Component of the Rotation.
        /// </summary>
        private float rotateX = 0;

        /// <summary>
        /// Y Component of the Rotation.
        /// </summary>
        private float rotateY = 0;

        /// <summary>
        /// Z Component of the Rotation.
        /// </summary>
        private float rotateZ = 0;

        /// <summary>
        /// X Component of the Scale.
        /// </summary>
        private float scaleX = 1;
        /// <summary>
        /// Y Component of the Scale.
        /// </summary>
        private float scaleY = 1;

        /// <summary>
        /// Z Component of the Scale.
        /// </summary>
        private float scaleZ = 1;

        /// <summary>
        /// The order of the linear transformation.
        /// </summary>
        private LinearTransformationOrder transformationOrder = LinearTransformationOrder.TranslateRotateScale;

        /// <summary>
        /// Gets the translation vertex.
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public Vertex TranslationVertex
        {
            get { return new Vertex(translateX, translateY, translateZ); }
        }

        /// <summary>
        /// Gets or sets the x component of the translation.
        /// </summary>
        /// <value>
        /// The x component of the translation.
        /// </value>
        [Category("Transformation"), Description("The X Component of the Translation")]
        [XmlAttribute]
        public float TranslateX
        {
            get { return translateX; }
            set { translateX = value; }
        }

        /// <summary>
        /// Gets or sets the y component of the translation.
        /// </summary>
        /// <value>
        /// The y component of the translation.
        /// </value>
        [Category("Transformation"), Description("The Y Component of the Translation")]
        public float TranslateY
        {
            get { return translateY; }
            set { translateY = value; }
        }

        /// <summary>
        /// Gets or sets the z component of the translation.
        /// </summary>
        /// <value>
        /// The z component of the translation.
        /// </value>
        [Category("Transformation"), Description("The Z Component of the Translation")]
        [XmlAttribute]
        public float TranslateZ
        {
            get { return translateZ; }
            set { translateZ = value; }
        }

        /// <summary>
        /// Gets or sets the x component of the rotation.
        /// </summary>
        /// <value>
        /// The x component of the rotation.
        /// </value>
        [Category("Transformation"), Description("Rotation around the X axis (in degrees)")]
        [XmlAttribute]
        public float RotateX
        {
            get { return rotateX; }
            set { rotateX = value; }
        }

        /// <summary>
        /// Gets or sets the y component of the rotation.
        /// </summary>
        /// <value>
        /// The y component of the rotation.
        /// </value>
        [Category("Transformation"), Description("Rotation around the Y axis (in degrees)")]
        [XmlAttribute]
        public float RotateY
        {
            get { return rotateY; }
            set { rotateY = value; }
        }

        /// <summary>
        /// Gets or sets the z component of the rotation.
        /// </summary>
        /// <value>
        /// The z component of the rotation.
        /// </value>
        [Category("Transformation"), Description("Rotation around the Z axis (in degrees)")]
        [XmlAttribute]
        public float RotateZ
        {
            get { return rotateZ; }
            set { rotateZ = value; }
        }

        /// <summary>
        /// Gets or sets the x component of the scale.
        /// </summary>
        /// <value>
        /// The x component of the scale.
        /// </value>
        [Category("Transformation"), Description("X Component of the Scaling")]
        [XmlAttribute]
        public float ScaleX
        {
            get { return scaleX; }
            set { scaleX = value; }
        }

        /// <summary>
        /// Gets or sets the y component of the scale.
        /// </summary>
        /// <value>
        /// The y component of the scale.
        /// </value>
        [Category("Transformation"), Description("Y Component of the Scaling")]
        [XmlAttribute]
        public float ScaleY
        {
            get { return scaleY; }
            set { scaleY = value; }
        }

        /// <summary>
        /// Gets or sets the z component of the scale.
        /// </summary>
        /// <value>
        /// The z component of the scale.
        /// </value>
        [Category("Transformation"), Description("Z Component of the Scaling")]
        [XmlAttribute]
        public float ScaleZ
        {
            get { return scaleZ; }
            set { scaleZ = value; }
        }

        /// <summary>
        /// Gets or sets the transformation order.
        /// </summary>
        /// <value>
        /// The transformation order.
        /// </value>
        [XmlAttribute]
        public LinearTransformationOrder TransformationOrder
        {
            get { return transformationOrder; }
            set { transformationOrder = value; }
        }
    }
}
