using System;
using System.Xml.Serialization;

namespace SharpGL.SceneGraph
{
    /// <summary>
    /// The Vertex class represents a 3D point in space.
    /// </summary>
    public struct Vertex
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Vertex"/> struct.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <param name="z">The z.</param>
        public Vertex(float x, float y, float z)
        {
            this.x = x; 
            this.y = y; 
            this.z = z;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vertex"/> struct.
        /// </summary>
        /// <param name="vertex">The vertex.</param>
        public Vertex(Vertex vertex)
        {
            this.x = vertex.X;
            this.y = vertex.Y;
            this.z = vertex.Z;
        }

        /// <summary>
        /// Sets the specified X.
        /// </summary>
        /// <param name="X">The X.</param>
        /// <param name="Y">The Y.</param>
        /// <param name="Z">The Z.</param>
        public void Set(float X, float Y, float Z)
        {
            this.X = X; this.Y = Y; this.Z = Z;
        }

        public void Push(float X, float Y, float Z) { this.X += X; this.Y += Y; this.Z += Z; }

        public override string ToString()
        {
            return "(" + X + ", " + Y + ", " + Z + ")";
        }

        /// <summary>
        /// Implements the operator +.
        /// </summary>
        /// <param name="lhs">The LHS.</param>
        /// <param name="rhs">The RHS.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Vertex operator +(Vertex lhs, Vertex rhs)
        {
            return new Vertex(lhs.X + rhs.X, lhs.Y + rhs.Y, lhs.Z + rhs.Z);
        }

        /// <summary>
        /// Implements the operator -.
        /// </summary>
        /// <param name="lhs">The LHS.</param>
        /// <param name="rhs">The RHS.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Vertex operator -(Vertex lhs, Vertex rhs)
        {
            return new Vertex(lhs.X - rhs.X, lhs.Y - rhs.Y, lhs.Z - rhs.Z);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="lhs">The LHS.</param>
        /// <param name="rhs">The RHS.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Vertex operator *(Vertex lhs, Vertex rhs)
        {
            return new Vertex(lhs.X * rhs.X, lhs.Y * rhs.Y, lhs.Z * rhs.Z);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="lhs">The LHS.</param>
        /// <param name="rhs">The RHS.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Vertex operator *(Vertex lhs, float rhs)
        {
            return new Vertex(lhs.X * rhs, lhs.Y * rhs, lhs.Z * rhs);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="lhs">The LHS.</param>
        /// <param name="rhs">The RHS.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Vertex operator * (Vertex lhs, Matrix rhs)
        {
            float X = lhs.X * (float)rhs[0,0] + lhs.Y * (float)rhs[1,0] + lhs.Z * (float)rhs[2,0];
            float Y = lhs.X * (float)rhs[0,1] + lhs.Y * (float)rhs[1,1] + lhs.Z * (float)rhs[2,1];
            float Z = lhs.X * (float)rhs[0,2] + lhs.Y * (float)rhs[1,2] + lhs.Z * (float)rhs[2,2];

            return new Vertex(X, Y, Z);
        }

        /// <summary>
        /// Implements the operator *.
        /// </summary>
        /// <param name="lhs">The LHS.</param>
        /// <param name="rhs">The RHS.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Vertex operator *(Matrix lhs, Vertex rhs)
        {
            float X = rhs.X * (float)lhs[0, 0] + rhs.Y * (float)lhs[1, 0] + rhs.Z * (float)lhs[2, 0];
            float Y = rhs.X * (float)lhs[0, 1] + rhs.Y * (float)lhs[1, 1] + rhs.Z * (float)lhs[2, 1];
            float Z = rhs.X * (float)lhs[0, 2] + rhs.Y * (float)lhs[1, 2] + rhs.Z * (float)lhs[2, 2];

            return new Vertex(X, Y, Z);
        }

        /// <summary>
        /// Implements the operator /.
        /// </summary>
        /// <param name="lhs">The LHS.</param>
        /// <param name="rhs">The RHS.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
        public static Vertex operator /(Vertex lhs, float rhs)
        {
            return new Vertex(lhs.X / rhs, lhs.Y / rhs, lhs.Z / rhs);
        }

        /// <summarY>
        /// This finds the Scalar Product (Dot Product) of two vectors.
        /// </summarY>
        /// <param name="rhs">The right hand side of the equation.</param>
        /// <returns>A Scalar Representing the Dot-Product.</returns>
        public float ScalarProduct(Vertex rhs)
        {
            return X * rhs.X + Y * rhs.Y + Z * rhs.Z;
        }

        /// <summarY>
        /// Find the Vector product (cross product) of two vectors.
        /// </summarY>
        /// <param name="rhs">The right hand side of the equation.</param>
        /// <returns>The Cross Product.</returns>
        public Vertex VectorProduct(Vertex rhs)
        {
            return new Vertex((Y * rhs.Z) - (Z * rhs.Y), (Z * rhs.X) - (X * rhs.Z),
                (X * rhs.Y) - (Y * rhs.X));
        }

        /// <summarY>
        /// If You use this as a Vector, then call this function to get the vector
        /// magnitude.
        /// </summarY>
        /// <returns></returns>
        public double Magnitude()
        {
            return System.Math.Sqrt(X * X + Y * Y + Z * Z);
        }

        /// <summarY>
        /// Make this vector unit length.
        /// </summarY>
        public void UnitLength()
        {
            //  Zero length vertexes are always normalized to zero.
            if (x == 0f && y == 0f && z == 0f) return;

            float f = X * X + Y * Y + Z * Z;
            float frt = (float)Math.Sqrt(f);
            X /= frt;
            Y /= frt;
            Z /= frt;
        }

        /// <summary>
        /// Normalizes this instance.
        /// </summary>
        public void Normalize()
        {
            UnitLength();
        }

        public static implicit operator float[](Vertex rhs)
        {
            return new float[] { rhs.X, rhs.Y, rhs.Z };
        }

        private float x;
        private float y;
        private float z;
        
        /// <summary>
        /// The X coordinate.
        /// </summary>
        [XmlAttribute]
        public float X
        {
            get { return x; }
            set { x = value; }
        }

        /// <summary>
        /// The Y coordinate.
        /// </summary>
        [XmlAttribute]
        public float Y
        {
            get { return y; }
            set { y = value; }
        }

        /// <summary>
        /// The Z coordinate.
        /// </summary>
        [XmlAttribute]
        public float Z
        {
            get { return z; }
            set { z = value; }
        }
    }
}
