using System;
using System.ComponentModel;
using SharpGL.SceneGraph.Core;
using SharpGL.SceneGraph.Lighting;

namespace SharpGL.SceneGraph.Core
{
    /// <summary>
    /// The ArcBall camera supports arcball projection, making it ideal for use with a mouse.
    /// </summary>
    [Serializable()]
    public class ArcBall
    {
        /// <summary>
        /// Initializes a new instance 
        /// </summary>
        public ArcBall()
        {
            //  Set the identity matrices.
            transformMatrix.SetIdentity();
            lastRotationMatrix.SetIdentity();
            thisRotationMatrix.SetIdentity();
        }

        /// <summary>
        /// This is the class' main function, to override this function and perform a 
        /// perspective transformation.
        /// </summary>
        public void TransformMatrix(OpenGL gl)
        {
            gl.MultMatrix(transformMatrix.AsColumnMajorArray);
        }

        public void MouseDown(int x, int y)
        {
            //  Map the start vertex.
            //MapToSphere((float)x, (float)y, out startVector);

            startVector = MapToSphere((float)x, (float)y);
        }



        public void MouseMove(int x, int y)
        {
            currentVector = MapToSphere((float)x, (float)y);

            //  todo need solid tuple types.
            //  Calculate the quaternion.
            float[] quaternion = CalculateQuaternion();

            thisRotationMatrix = Matrix3fSetRotationFromQuat4f(quaternion);
            thisRotationMatrix = thisRotationMatrix * lastRotationMatrix;
            Matrix4fSetRotationFromMatrix3f(ref transformMatrix, thisRotationMatrix);          // Set Our Final Transform's Rotation From This One
        }

        public void MouseUp(int x, int y)
        {
            lastRotationMatrix.FromOtherMatrix(thisRotationMatrix, 3, 3);
            thisRotationMatrix.SetIdentity();

            startVector = new Vertex(0, 0, 0);
        }

        private Matrix Matrix3fSetRotationFromQuat4f(float[] q1)
        {
            float n, s;
            float xs, ys, zs;
            float wx, wy, wz;
            float xx, xy, xz;
            float yy, yz, zz;
            n = (q1[0] * q1[0]) + (q1[1] * q1[1]) + (q1[2] * q1[2]) + (q1[3] * q1[3]);
            s = (n > 0.0f) ? (2.0f / n) : 0.0f;

            xs = q1[0] * s; ys = q1[1] * s; zs = q1[2] * s;
            wx = q1[3] * xs; wy = q1[3] * ys; wz = q1[3] * zs;
            xx = q1[0] * xs; xy = q1[0] * ys; xz = q1[0] * zs;
            yy = q1[1] * ys; yz = q1[1] * zs; zz = q1[2] * zs;

            Matrix matrix = new Matrix(3, 3);

            matrix[0, 0] = 1.0f - (yy + zz); matrix[1, 0] = xy - wz; matrix[2, 0] = xz + wy;
            matrix[0, 1] = xy + wz; matrix[1, 1] = 1.0f - (xx + zz); matrix[2, 1] = yz - wx;
            matrix[0, 2] = xz - wy; matrix[1, 2] = yz + wx; matrix[2, 2] = 1.0f - (xx + yy);

            return matrix;
        }

        private void Matrix4fSetRotationFromMatrix3f(ref Matrix transform, Matrix matrix)
        {
            float scale = transform.TempSVD();
            transform.FromOtherMatrix(matrix, 3, 3);
            transform.Multiply(scale, 3, 3);
        }

        private float[] CalculateQuaternion()
        {
            //  Compute the cross product of the begin and end vectors.
            Vertex cross = startVector.VectorProduct(currentVector);

            //  Is the perpendicular length essentially non-zero?
            if (cross.Magnitude() > 1.0e-5)
            {
                //  The quaternion is the transform.
                return new float[] { cross.X, cross.Y, cross.Z, startVector.ScalarProduct(currentVector) };
            }
            else
            {
                //  Begin and end coincide, return identity.
                return new float[] { 0, 0, 0, 0 };
            }
        }

        public Vertex MapToSphere(float x, float y)
        {
            //hyperboloid mapping taken from https://www.opengl.org/wiki/Object_Mouse_Trackball

            float pX = x * adjustWidth - 1.0f;
            float pY = y * adjustHeight - 1.0f;

            Vertex P = new Vertex(pX, -pY, 0);

            //sphere radius
            const float radius = .5f;
            const float radius_squared = radius * radius;

            float XY_squared = P.X * P.X + P.Y * P.Y;

            if (XY_squared <= .5f * radius_squared)
                P.Z = (float)Math.Sqrt(radius_squared - XY_squared);  // Pythagore
            else
                P.Z = (float)(0.5f * (radius_squared)) / (float)Math.Sqrt(XY_squared);  // hyperboloid

            return P;
        }


        public void SetBounds(float width, float height)
        {
            SetBounds(width, height, 1.0f);
        }

        /// <summary>
        /// set screen boundaried bounds 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="sphereRadius">arcball radius [0.0-1.0f]</param>
        public void SetBounds(float width, float height, float sphereRadius)
        {
            //  Set the adjust width and height 
            //  normalize x,y to [-1,1] and center to screen center
            adjustWidth = 2.0f / width;
            adjustHeight = 2.0f / height;

        }

        private float adjustWidth = 1.0f;
        private float adjustHeight = 1.0f;

        public Vertex startVector = new Vertex(0, 0, 0);
        public Vertex currentVector = new Vertex(0, 0, 0);

        Matrix transformMatrix = new Matrix(4, 4);

        Matrix lastRotationMatrix = new Matrix(3, 3);

        Matrix thisRotationMatrix = new Matrix(3, 3);
    }


}
