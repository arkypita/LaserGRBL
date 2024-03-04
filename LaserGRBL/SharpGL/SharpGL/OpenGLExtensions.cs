using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace SharpGL
{
    public partial class OpenGL
    {
        /// <summary>
        /// ftlPhysicsGuy - This should provide better performance than using a dictionary.
        ///                 Testing indicated that with only 15 items in extensionFunctions,
        ///                 accessing this static class averaged about 7 times faster than doing
        ///                 a lookup on the dictionary<para></para>
        /// A static class that holds a delegate for a given GL Extensions Function.  During
        /// runtime, the application will generate or retrieve the appropriate class for a given
        /// delegate type as needed when GetDelegateFor<typeparamref name="T"/>() is called.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        static class GlExtensionFunction<T> where T : class
        {
            private static T m_delegate = null;
            public static T Delegate
            {
                get
                {
                    if (m_delegate == null)
                    {
                        Delegate del = null;
                        Type delegateType = typeof(T);
                        //  Get the name of the extension function.
                        string name = delegateType.Name;
                        IntPtr proc = Win32.wglGetProcAddress(name);
                        if (proc == IntPtr.Zero)
                            throw new Exception("Extension function " + name + " not supported");

                        //  Get the delegate for the function pointer.
                        del = Marshal.GetDelegateForFunctionPointer(proc, delegateType);
                        if (del == null)
                            throw new Exception("Extension function " + name + " marshalled incorrectly");

                        m_delegate = del as T;
                    }
                    return m_delegate;
                }
            }
        }

        /// <summary>
        /// Determines whether a named extension function is supported.
        /// </summary>
        /// <param name="extensionFunctionName">Name of the extension function.</param>
        /// <returns>
        /// 	<c>true</c> if the extension function is supported; otherwise, <c>false</c>.
        /// </returns>
        public bool IsExtensionFunctionSupported(string extensionFunctionName)
        {
            //  Try and get the proc address for the function.
            IntPtr procAddress = Win32.wglGetProcAddress(extensionFunctionName);

            //  As long as the pointer is non-zero, we can invoke the extension function.
            return procAddress != IntPtr.Zero;
        }

        /// <summary>
        /// Returns a delegate for an extension function. This delegate  can be called to execute the extension function.
        /// </summary>
        /// <typeparam name="T">The extension delegate type.</typeparam>
        /// <returns>The delegate that points to the extension function.</returns>
        private T GetDelegateFor<T>() where T : class
        {
            // Retrieves or creates a static version of the GlExtensionFunction class to provide
            // the delegate for this method
            // ftlPhysicsGuy - See comments on GlExtensionFunction class above.
            return GlExtensionFunction<T>.Delegate;
        }

        #region OpenGL 1.2

        //  Methods
        public void BlendColor(float red, float green, float blue, float alpha)
        {
            GetDelegateFor<glBlendColor>()(red, green, blue, alpha);
        }
        public void BlendEquation(uint mode)
        {
            GetDelegateFor<glBlendEquation>()(mode);
        }
        public void DrawRangeElements(uint mode, uint start, uint end, int count, uint type, IntPtr indices)
        {
            GetDelegateFor<glDrawRangeElements>()(mode, start, end, count, type, indices);
        }
        public void TexImage3D(uint target, int level, int internalformat, int width, int height, int depth, int border, uint format, uint type, IntPtr pixels)
        {
            GetDelegateFor<glTexImage3D>()(target, level, internalformat, width, height, depth, border, format, type, pixels);
        }
        public void TexSubImage3D(uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, IntPtr pixels)
        {
            GetDelegateFor<glTexSubImage3D>()(target, level, xoffset, yoffset, zoffset, width, height, depth, format, type, pixels);
        }
        public void CopyTexSubImage3D(uint target, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height)
        {
            GetDelegateFor<glCopyTexSubImage3D>()(target, level, xoffset, yoffset, zoffset, x, y, width, height);
        }

        //  Deprecated Methods
        [Obsolete]
        public void ColorTable(uint target, uint internalformat, int width, uint format, uint type, IntPtr table)
        {
            GetDelegateFor<glColorTable>()(target, internalformat, width, format, type, table);
        }
        [Obsolete]
        public void ColorTableParameterfv(uint target, uint pname, float[] parameters)
        {
            GetDelegateFor<glColorTableParameterfv>()(target, pname, parameters);
        }
        [Obsolete]
        public void ColorTableParameteriv(uint target, uint pname, int[] parameters)
        {
            GetDelegateFor<glColorTableParameteriv>()(target, pname, parameters);
        }
        [Obsolete]
        public void CopyColorTable(uint target, uint internalformat, int x, int y, int width)
        {
            GetDelegateFor<glCopyColorTable>()(target, internalformat, x, y, width);
        }
        [Obsolete]
        public void GetColorTable(uint target, uint format, uint type, IntPtr table)
        {
            GetDelegateFor<glGetColorTable>()(target, format, type, table);
        }
        [Obsolete]
        public void GetColorTableParameter(uint target, uint pname, float[] parameters)
        {
            GetDelegateFor<glGetColorTableParameterfv>()(target, pname, parameters);
        }
        [Obsolete]
        public void GetColorTableParameter(uint target, uint pname, int[] parameters)
        {
            GetDelegateFor<glGetColorTableParameteriv>()(target, pname, parameters);
        }
        [Obsolete]
        public void ColorSubTable(uint target, int start, int count, uint format, uint type, IntPtr data)
        {
            GetDelegateFor<glColorSubTable>()(target, start, count, format, type, data);
        }
        [Obsolete]
        public void CopyColorSubTable(uint target, int start, int x, int y, int width)
        {
            GetDelegateFor<glCopyColorSubTable>()(target, start, x, y, width);
        }
        [Obsolete]
        public void ConvolutionFilter1D(uint target, uint internalformat, int width, uint format, uint type, IntPtr image)
        {
            GetDelegateFor<glConvolutionFilter1D>()(target, internalformat, width, format, type, image);
        }
        [Obsolete]
        public void ConvolutionFilter2D(uint target, uint internalformat, int width, int height, uint format, uint type, IntPtr image)
        {
            GetDelegateFor<glConvolutionFilter2D>()(target, internalformat, width, height, format, type, image);
        }
        [Obsolete]
        public void ConvolutionParameter(uint target, uint pname, float parameters)
        {
            GetDelegateFor<glConvolutionParameterf>()(target, pname, parameters);
        }
        [Obsolete]
        public void ConvolutionParameter(uint target, uint pname, float[] parameters)
        {
            GetDelegateFor<glConvolutionParameterfv>()(target, pname, parameters);
        }
        [Obsolete]
        public void ConvolutionParameter(uint target, uint pname, int parameters)
        {
            GetDelegateFor<glConvolutionParameteri>()(target, pname, parameters);
        }
        [Obsolete]
        public void ConvolutionParameter(uint target, uint pname, int[] parameters)
        {
            GetDelegateFor<glConvolutionParameteriv>()(target, pname, parameters);
        }
        [Obsolete]
        public void CopyConvolutionFilter1D(uint target, uint internalformat, int x, int y, int width)
        {
            GetDelegateFor<glCopyConvolutionFilter1D>()(target, internalformat, x, y, width);
        }
        [Obsolete]
        public void CopyConvolutionFilter2D(uint target, uint internalformat, int x, int y, int width, int height)
        {
            GetDelegateFor<glCopyConvolutionFilter2D>()(target, internalformat, x, y, width, height);
        }
        [Obsolete]
        public void GetConvolutionFilter(uint target, uint format, uint type, IntPtr image)
        {
            GetDelegateFor<glGetConvolutionFilter>()(target, format, type, image);
        }
        [Obsolete]
        public void GetConvolutionParameter(uint target, uint pname, float[] parameters)
        {
            GetDelegateFor<glGetConvolutionParameterfv>()(target, pname, parameters);
        }
        [Obsolete]
        public void GetConvolutionParameter(uint target, uint pname, int[] parameters)
        {
            GetDelegateFor<glGetConvolutionParameteriv>()(target, pname, parameters);
        }
        [Obsolete]
        public void GetSeparableFilter(uint target, uint format, uint type, IntPtr row, IntPtr column, IntPtr span)
        {
            GetDelegateFor<glGetSeparableFilter>()(target, format, type, row, column, span);
        }
        [Obsolete]
        public void SeparableFilter2D(uint target, uint internalformat, int width, int height, uint format, uint type, IntPtr row, IntPtr column)
        {
            GetDelegateFor<glSeparableFilter2D>()(target, internalformat, width, height, format, type, row, column);
        }
        [Obsolete]
        public void GetHistogram(uint target, bool reset, uint format, uint type, IntPtr values)
        {
            GetDelegateFor<glGetHistogram>()(target, reset, format, type, values);
        }
        [Obsolete]
        public void GetHistogramParameter(uint target, uint pname, float[] parameters)
        {
            GetDelegateFor<glGetHistogramParameterfv>()(target, pname, parameters);
        }
        [Obsolete]
        public void GetHistogramParameter(uint target, uint pname, int[] parameters)
        {
            GetDelegateFor<glGetHistogramParameteriv>()(target, pname, parameters);
        }
        [Obsolete]
        public void GetMinmax(uint target, bool reset, uint format, uint type, IntPtr values)
        {
            GetDelegateFor<glGetMinmax>()(target, reset, format, type, values);
        }
        [Obsolete]
        public void GetMinmaxParameter(uint target, uint pname, float[] parameters)
        {
            GetDelegateFor<glGetMinmaxParameterfv>()(target, pname, parameters);
        }
        [Obsolete]
        public void GetMinmaxParameter(uint target, uint pname, int[] parameters)
        {
            GetDelegateFor<glGetMinmaxParameteriv>()(target, pname, parameters);
        }
        [Obsolete]
        public void Histogram(uint target, int width, uint internalformat, bool sink)
        {
            GetDelegateFor<glHistogram>()(target, width, internalformat, sink);
        }
        [Obsolete]
        public void Minmax(uint target, uint internalformat, bool sink)
        {
            GetDelegateFor<glMinmax>()(target, internalformat, sink);
        }
        [Obsolete]
        public void ResetHistogram(uint target)
        {
            GetDelegateFor<glResetHistogram>()(target);
        }
        [Obsolete]
        public void ResetMinmax(uint target)
        {
            GetDelegateFor<glResetMinmax>()(target);
        }

        //  Delegates
        private delegate void glBlendColor (float red, float green, float blue, float alpha);
        private delegate void glBlendEquation (uint mode);
        private delegate void glDrawRangeElements (uint mode, uint start, uint end, int count, uint type, IntPtr indices);
        private delegate void glTexImage3D (uint target, int level, int internalformat, int width, int height, int depth, int border, uint format, uint type, IntPtr pixels);
        private delegate void glTexSubImage3D (uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, uint type, IntPtr pixels);
        private delegate void glCopyTexSubImage3D (uint target, int level, int xoffset, int yoffset, int zoffset, int x, int y, int width, int height);
        private delegate void glColorTable (uint target, uint internalformat, int width, uint format, uint type, IntPtr table);
        private delegate void glColorTableParameterfv (uint target, uint pname, float[] parameters);
        private delegate void glColorTableParameteriv (uint target, uint pname, int[] parameters);
        private delegate void glCopyColorTable (uint target, uint internalformat, int x, int y, int width);
        private delegate void glGetColorTable (uint target, uint format, uint type, IntPtr table);
        private delegate void glGetColorTableParameterfv (uint target, uint pname, float[] parameters);
        private delegate void glGetColorTableParameteriv (uint target, uint pname, int[] parameters);
        private delegate void glColorSubTable (uint target, int start, int count, uint format, uint type, IntPtr data);
        private delegate void glCopyColorSubTable (uint target, int start, int x, int y, int width);
        private delegate void glConvolutionFilter1D (uint target, uint internalformat, int width, uint format, uint type, IntPtr image);
        private delegate void glConvolutionFilter2D (uint target, uint internalformat, int width, int height, uint format, uint type, IntPtr image);
        private delegate void glConvolutionParameterf (uint target, uint pname, float parameters);
        private delegate void glConvolutionParameterfv (uint target, uint pname, float[] parameters);
        private delegate void glConvolutionParameteri (uint target, uint pname, int parameters);
        private delegate void glConvolutionParameteriv (uint target, uint pname, int[] parameters);
        private delegate void glCopyConvolutionFilter1D (uint target, uint internalformat, int x, int y, int width);
        private delegate void glCopyConvolutionFilter2D (uint target, uint internalformat, int x, int y, int width, int height);
        private delegate void glGetConvolutionFilter (uint target, uint format, uint type, IntPtr image);
        private delegate void glGetConvolutionParameterfv (uint target, uint pname, float[] parameters);
        private delegate void glGetConvolutionParameteriv (uint target, uint pname, int[] parameters);
        private delegate void glGetSeparableFilter (uint target, uint format, uint type, IntPtr row, IntPtr column, IntPtr span);
        private delegate void glSeparableFilter2D (uint target, uint internalformat, int width, int height, uint format, uint type, IntPtr row, IntPtr column);
        private delegate void glGetHistogram (uint target, bool reset, uint format, uint type, IntPtr values);
        private delegate void glGetHistogramParameterfv (uint target, uint pname, float[] parameters);
        private delegate void glGetHistogramParameteriv (uint target, uint pname, int[] parameters);
        private delegate void glGetMinmax (uint target, bool reset, uint format, uint type, IntPtr values);
        private delegate void glGetMinmaxParameterfv (uint target, uint pname, float[] parameters);
        private delegate void glGetMinmaxParameteriv (uint target, uint pname, int[] parameters);
        private delegate void glHistogram (uint target, int width, uint internalformat, bool sink);
        private delegate void glMinmax (uint target, uint internalformat, bool sink);
        private delegate void glResetHistogram (uint target);
        private delegate void glResetMinmax (uint target);

        //  Constants
        public const uint GL_UNSIGNED_BYTE_3_3_2             = 0x8032;
        public const uint GL_UNSIGNED_SHORT_4_4_4_4          = 0x8033;
        public const uint GL_UNSIGNED_SHORT_5_5_5_1          = 0x8034;
        public const uint GL_UNSIGNED_INT_8_8_8_8            = 0x8035;
        public const uint GL_UNSIGNED_INT_10_10_10_2         = 0x8036;
        public const uint GL_TEXTURE_BINDING_3D              = 0x806A;
        public const uint GL_PACK_SKIP_IMAGES                = 0x806B;
        public const uint GL_PACK_IMAGE_HEIGHT               = 0x806C;
        public const uint GL_UNPACK_SKIP_IMAGES              = 0x806D;
        public const uint GL_UNPACK_IMAGE_HEIGHT             = 0x806E;
        public const uint GL_TEXTURE_3D                      = 0x806F;
        public const uint GL_PROXY_TEXTURE_3D                = 0x8070;
        public const uint GL_TEXTURE_DEPTH                   = 0x8071;
        public const uint GL_TEXTURE_WRAP_R                  = 0x8072;
        public const uint GL_MAX_3D_TEXTURE_SIZE             = 0x8073;
        public const uint GL_UNSIGNED_BYTE_2_3_3_REV         = 0x8362;
        public const uint GL_UNSIGNED_SHORT_5_6_5            = 0x8363;
        public const uint GL_UNSIGNED_SHORT_5_6_5_REV        = 0x8364;
        public const uint GL_UNSIGNED_SHORT_4_4_4_4_REV      = 0x8365;
        public const uint GL_UNSIGNED_SHORT_1_5_5_5_REV      = 0x8366;
        public const uint GL_UNSIGNED_INT_8_8_8_8_REV        = 0x8367;
        public const uint GL_UNSIGNED_INT_2_10_10_10_REV     = 0x8368;
        public const uint GL_BGR                             = 0x80E0;
        public const uint GL_BGRA                            = 0x80E1;
        public const uint GL_MAX_ELEMENTS_VERTICES           = 0x80E8;
        public const uint GL_MAX_ELEMENTS_INDICES            = 0x80E9;
        public const uint GL_CLAMP_TO_EDGE                   = 0x812F;
        public const uint GL_TEXTURE_MIN_LOD                 = 0x813A;
        public const uint GL_TEXTURE_MAX_LOD                 = 0x813B;
        public const uint GL_TEXTURE_BASE_LEVEL              = 0x813C;
        public const uint GL_TEXTURE_MAX_LEVEL               = 0x813D;
        public const uint GL_SMOOTH_POINT_SIZE_RANGE         = 0x0B12;
        public const uint GL_SMOOTH_POINT_SIZE_GRANULARITY   = 0x0B13;
        public const uint GL_SMOOTH_LINE_WIDTH_RANGE         = 0x0B22;
        public const uint GL_SMOOTH_LINE_WIDTH_GRANULARITY   = 0x0B23;
        public const uint GL_ALIASED_LINE_WIDTH_RANGE        = 0x846E;

        #endregion

        #region OpenGL 1.3

        //  Methods
        
        
        public void ActiveTexture(uint texture)
        {
            GetDelegateFor<glActiveTexture>()(texture);
        }
        public void SampleCoverage(float value, bool invert)
        {
            GetDelegateFor<glSampleCoverage>()(value, invert);
        }
        public void CompressedTexImage3D(uint target, int level, uint internalformat, int width, int height, int depth, int border, int imageSize, IntPtr data)
        {
            GetDelegateFor<glCompressedTexImage3D>()(target, level, internalformat, width, height, depth, border, imageSize, data);
        }
        public void CompressedTexImage2D(uint target, int level, uint internalformat, int width, int height, int border, int imageSize, IntPtr data)
        {
            GetDelegateFor<glCompressedTexImage2D>()(target, level, internalformat, width, height, border, imageSize, data);
        }
        public void CompressedTexImage1D(uint target, int level, uint internalformat, int width, int border, int imageSize, IntPtr data)
        {
            GetDelegateFor<glCompressedTexImage1D>()(target, level, internalformat, width, border, imageSize, data);
        }
        public void CompressedTexSubImage3D(uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, int imageSize, IntPtr data)
        {
            GetDelegateFor<glCompressedTexSubImage3D>()(target, level, xoffset, yoffset, zoffset, width, height, depth, format, imageSize, data);
        }
        public void CompressedTexSubImage2D(uint target, int level, int xoffset, int yoffset, int width, int height, uint format, int imageSize, IntPtr data)
        {
            GetDelegateFor<glCompressedTexSubImage2D>()(target, level, xoffset, yoffset, width, height, format, imageSize, data);
        }
        public void CompressedTexSubImage1D(uint target, int level, int xoffset, int width, uint format, int imageSize, IntPtr data)
        {
            GetDelegateFor<glCompressedTexSubImage1D>()(target, level, xoffset, width, format, imageSize, data);
        }
        public void GetCompressedTexImage(uint target, int level, IntPtr img)
        {
            GetDelegateFor<glGetCompressedTexImage>()(target, level, img);
        }

        //  Deprecated Methods
        [Obsolete]
        public void ClientActiveTexture(uint texture)
        {
            GetDelegateFor<glClientActiveTexture>()(texture);
        }
        [Obsolete]
        public void MultiTexCoord1(uint target, double s)
        {
            GetDelegateFor<glMultiTexCoord1d>()(target, s);
        }
        [Obsolete]
        public void MultiTexCoord1(uint target, double[] v)
        {
            GetDelegateFor<glMultiTexCoord1dv>()(target, v);
        }
        [Obsolete]
        public void MultiTexCoord1(uint target, float s)
        {
            GetDelegateFor<glMultiTexCoord1f>()(target, s);
        }
        [Obsolete]
        public void MultiTexCoord1(uint target, float[] v)
        {
            GetDelegateFor<glMultiTexCoord1fv>()(target, v);
        }
        [Obsolete]
        public void MultiTexCoord1(uint target, int s)
        {
            GetDelegateFor<glMultiTexCoord1i>()(target, s);
        }
        [Obsolete]
        public void MultiTexCoord1(uint target, int[] v)
        {
            GetDelegateFor<glMultiTexCoord1iv>()(target, v);
        }
        [Obsolete]
        public void MultiTexCoord1(uint target, short s)
        {
            GetDelegateFor<glMultiTexCoord1s>()(target, s);
        }
        [Obsolete]
        public void MultiTexCoord1(uint target, short[] v)
        {
            GetDelegateFor<glMultiTexCoord1sv>()(target, v);
        }
        [Obsolete]
        public void MultiTexCoord2(uint target, double s, double t)
        {
            GetDelegateFor<glMultiTexCoord2d>()(target, s, t);
        }
        [Obsolete]
        public void MultiTexCoord2(uint target, double[] v)
        {
            GetDelegateFor<glMultiTexCoord2dv>()(target, v);
        }
        [Obsolete]
        public void MultiTexCoord2(uint target, float s, float t)
        {
            GetDelegateFor<glMultiTexCoord2f>()(target, s, t);
        }
        [Obsolete]
        public void MultiTexCoord2(uint target, float[] v)
        {
            GetDelegateFor<glMultiTexCoord2fv>()(target, v);
        }
        [Obsolete]
        public void MultiTexCoord2(uint target, int s, int t)
        {
            GetDelegateFor<glMultiTexCoord2i>()(target, s, t);
        }
        [Obsolete]
        public void MultiTexCoord2(uint target, int[] v)
        {
            GetDelegateFor<glMultiTexCoord2iv>()(target, v);
        }
        [Obsolete]
        public void MultiTexCoord2(uint target, short s, short t)
        {
            GetDelegateFor<glMultiTexCoord2s>()(target, s, t);
        }
        [Obsolete]
        public void MultiTexCoord2(uint target, short[] v)
        {
            GetDelegateFor<glMultiTexCoord2sv>()(target, v);
        }
        [Obsolete]
        public void MultiTexCoord3(uint target, double s, double t, double r)
        {
            GetDelegateFor<glMultiTexCoord3d>()(target, s, t, r);
        }
        [Obsolete]
        public void MultiTexCoord3(uint target, double[] v)
        {
            GetDelegateFor<glMultiTexCoord3dv>()(target, v);
        }
        [Obsolete]
        public void MultiTexCoord3(uint target, float s, float t, float r)
        {
            GetDelegateFor<glMultiTexCoord3f>()(target, s, t, r);
        }
        [Obsolete]
        public void MultiTexCoord3(uint target, float[] v)
        {
            GetDelegateFor<glMultiTexCoord3fv>()(target, v);
        }
        [Obsolete]
        public void MultiTexCoord3(uint target, int s, int t, int r)
        {
            GetDelegateFor<glMultiTexCoord3i>()(target, s, t, r);
        }
        [Obsolete]
        public void MultiTexCoord3(uint target, int[] v)
        {
            GetDelegateFor<glMultiTexCoord3iv>()(target, v);
        }
        [Obsolete]
        public void MultiTexCoord3(uint target, short s, short t, short r)
        {
            GetDelegateFor<glMultiTexCoord3s>()(target, s, t, r);
        }
        [Obsolete]
        public void MultiTexCoord3(uint target, short[] v)
        {
            GetDelegateFor<glMultiTexCoord3sv>()(target, v);
        }
        [Obsolete]
        public void MultiTexCoord4(uint target, double s, double t, double r, double q)
        {
            GetDelegateFor<glMultiTexCoord4d>()(target, s, t, r, q);
        }
        [Obsolete]
        public void MultiTexCoord4(uint target, double[] v)
        {
            GetDelegateFor<glMultiTexCoord4dv>()(target, v);
        }
        [Obsolete]
        public void MultiTexCoord4(uint target, float s, float t, float r, float q)
        {
            GetDelegateFor<glMultiTexCoord4f>()(target, s, t, r, q);
        }
        [Obsolete]
        public void MultiTexCoord4(uint target, float[] v)
        {
            GetDelegateFor<glMultiTexCoord4fv>()(target, v);
        }
        [Obsolete]
        public void MultiTexCoord4(uint target, int s, int t, int r, int q)
        {
            GetDelegateFor<glMultiTexCoord4i>()(target, s, t, r, q);
        }
        [Obsolete]
        public void MultiTexCoord4(uint target, int[] v)
        {
            GetDelegateFor<glMultiTexCoord4iv>()(target, v);
        }
        [Obsolete]
        public void MultiTexCoord4(uint target, short s, short t, short r, short q)
        {
            GetDelegateFor<glMultiTexCoord4s>()(target, s, t, r, q);
        }
        [Obsolete]
        public void MultiTexCoord4(uint target, short[] v)
        {
            GetDelegateFor<glMultiTexCoord4sv>()(target, v);
        }
        [Obsolete]
        public void LoadTransposeMatrix(float[] m)
        {
            GetDelegateFor<glLoadTransposeMatrixf>()(m);
        }
        [Obsolete]
        public void LoadTransposeMatrix(double[] m)
        {
            GetDelegateFor<glLoadTransposeMatrixd>()(m);
        }
        [Obsolete]
        public void MultTransposeMatrix(float[] m)
        {
            GetDelegateFor<glMultTransposeMatrixf>()(m);
        }
        [Obsolete]
        public void MultTransposeMatrix(double[] m)
        {
            GetDelegateFor<glMultTransposeMatrixd>()(m);
        }

        //  Delegates
        private delegate void glActiveTexture (uint texture);
        private delegate void glSampleCoverage (float value, bool invert);
        private delegate void glCompressedTexImage3D (uint target, int level, uint internalformat, int width, int height, int depth, int border, int imageSize, IntPtr data);
        private delegate void glCompressedTexImage2D (uint target, int level, uint internalformat, int width, int height, int border, int imageSize, IntPtr data);
        private delegate void glCompressedTexImage1D (uint target, int level, uint internalformat, int width, int border, int imageSize, IntPtr data);
        private delegate void glCompressedTexSubImage3D (uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, int imageSize, IntPtr data);
        private delegate void glCompressedTexSubImage2D (uint target, int level, int xoffset, int yoffset, int width, int height, uint format, int imageSize, IntPtr data);
        private delegate void glCompressedTexSubImage1D (uint target, int level, int xoffset, int width, uint format, int imageSize, IntPtr data);
        private delegate void glGetCompressedTexImage (uint target, int level, IntPtr img);

        private delegate void glClientActiveTexture (uint texture);
        private delegate void glMultiTexCoord1d (uint target, double s);
        private delegate void glMultiTexCoord1dv (uint target, double[] v);
        private delegate void glMultiTexCoord1f (uint target, float s);
        private delegate void glMultiTexCoord1fv (uint target, float[] v);
        private delegate void glMultiTexCoord1i (uint target, int s);
        private delegate void glMultiTexCoord1iv (uint target, int[] v);
        private delegate void glMultiTexCoord1s (uint target, short s);
        private delegate void glMultiTexCoord1sv (uint target, short[] v);
        private delegate void glMultiTexCoord2d (uint target, double s, double t);
        private delegate void glMultiTexCoord2dv (uint target, double[] v);
        private delegate void glMultiTexCoord2f (uint target, float s, float t);
        private delegate void glMultiTexCoord2fv (uint target, float[] v);
        private delegate void glMultiTexCoord2i (uint target, int s, int t);
        private delegate void glMultiTexCoord2iv (uint target, int[] v);
        private delegate void glMultiTexCoord2s (uint target, short s, short t);
        private delegate void glMultiTexCoord2sv (uint target, short[] v);
        private delegate void glMultiTexCoord3d (uint target, double s, double t, double r);
        private delegate void glMultiTexCoord3dv (uint target, double[] v);
        private delegate void glMultiTexCoord3f (uint target, float s, float t, float r);
        private delegate void glMultiTexCoord3fv (uint target, float[] v);
        private delegate void glMultiTexCoord3i (uint target, int s, int t, int r);
        private delegate void glMultiTexCoord3iv (uint target, int[] v);
        private delegate void glMultiTexCoord3s (uint target, short s, short t, short r);
        private delegate void glMultiTexCoord3sv (uint target, short[] v);
        private delegate void glMultiTexCoord4d (uint target, double s, double t, double r, double q);
        private delegate void glMultiTexCoord4dv (uint target, double[] v);
        private delegate void glMultiTexCoord4f (uint target, float s, float t, float r, float q);
        private delegate void glMultiTexCoord4fv (uint target, float[] v);
        private delegate void glMultiTexCoord4i (uint target, int s, int t, int r, int q);
        private delegate void glMultiTexCoord4iv (uint target, int[] v);
        private delegate void glMultiTexCoord4s (uint target, short s, short t, short r, short q);
        private delegate void glMultiTexCoord4sv (uint target, short[] v);
        private delegate void glLoadTransposeMatrixf (float[] m);
        private delegate void glLoadTransposeMatrixd (double[] m);
        private delegate void glMultTransposeMatrixf (float[] m);
        private delegate void glMultTransposeMatrixd (double[] m);

        //  Constants
        public const uint GL_TEXTURE0                        = 0x84C0;
        public const uint GL_TEXTURE1                        = 0x84C1;
        public const uint GL_TEXTURE2                        = 0x84C2;
        public const uint GL_TEXTURE3                        = 0x84C3;
        public const uint GL_TEXTURE4                        = 0x84C4;
        public const uint GL_TEXTURE5                        = 0x84C5;
        public const uint GL_TEXTURE6                        = 0x84C6;
        public const uint GL_TEXTURE7                        = 0x84C7;
        public const uint GL_TEXTURE8                        = 0x84C8;
        public const uint GL_TEXTURE9                        = 0x84C9;
        public const uint GL_TEXTURE10                       = 0x84CA;
        public const uint GL_TEXTURE11                       = 0x84CB;
        public const uint GL_TEXTURE12                       = 0x84CC;
        public const uint GL_TEXTURE13                       = 0x84CD;
        public const uint GL_TEXTURE14                       = 0x84CE;
        public const uint GL_TEXTURE15                       = 0x84CF;
        public const uint GL_TEXTURE16                       = 0x84D0;
        public const uint GL_TEXTURE17                       = 0x84D1;
        public const uint GL_TEXTURE18                       = 0x84D2;
        public const uint GL_TEXTURE19                       = 0x84D3;
        public const uint GL_TEXTURE20                       = 0x84D4;
        public const uint GL_TEXTURE21                       = 0x84D5;
        public const uint GL_TEXTURE22                       = 0x84D6;
        public const uint GL_TEXTURE23                       = 0x84D7;
        public const uint GL_TEXTURE24                       = 0x84D8;
        public const uint GL_TEXTURE25                       = 0x84D9;
        public const uint GL_TEXTURE26                       = 0x84DA;
        public const uint GL_TEXTURE27                       = 0x84DB;
        public const uint GL_TEXTURE28                       = 0x84DC;
        public const uint GL_TEXTURE29                       = 0x84DD;
        public const uint GL_TEXTURE30                       = 0x84DE;
        public const uint GL_TEXTURE31                       = 0x84DF;
        public const uint GL_ACTIVE_TEXTURE                  = 0x84E0;
        public const uint GL_MULTISAMPLE                     = 0x809D;
        public const uint GL_SAMPLE_ALPHA_TO_COVERAGE        = 0x809E;
        public const uint GL_SAMPLE_ALPHA_TO_ONE             = 0x809F;
        public const uint GL_SAMPLE_COVERAGE                 = 0x80A0;
        public const uint GL_SAMPLE_BUFFERS                  = 0x80A8;
        public const uint GL_SAMPLES                         = 0x80A9;
        public const uint GL_SAMPLE_COVERAGE_VALUE           = 0x80AA;
        public const uint GL_SAMPLE_COVERAGE_INVERT          = 0x80AB;
        public const uint GL_TEXTURE_CUBE_MAP                = 0x8513;
        public const uint GL_TEXTURE_BINDING_CUBE_MAP        = 0x8514;
        public const uint GL_TEXTURE_CUBE_MAP_POSITIVE_X     = 0x8515;
        public const uint GL_TEXTURE_CUBE_MAP_NEGATIVE_X     = 0x8516;
        public const uint GL_TEXTURE_CUBE_MAP_POSITIVE_Y     = 0x8517;
        public const uint GL_TEXTURE_CUBE_MAP_NEGATIVE_Y     = 0x8518;
        public const uint GL_TEXTURE_CUBE_MAP_POSITIVE_Z     = 0x8519;
        public const uint GL_TEXTURE_CUBE_MAP_NEGATIVE_Z     = 0x851A;
        public const uint GL_PROXY_TEXTURE_CUBE_MAP          = 0x851B;
        public const uint GL_MAX_CUBE_MAP_TEXTURE_SIZE       = 0x851C;
        public const uint GL_COMPRESSED_RGB                  = 0x84ED;
        public const uint GL_COMPRESSED_RGBA                 = 0x84EE;
        public const uint GL_TEXTURE_COMPRESSION_HINT        = 0x84EF;
        public const uint GL_TEXTURE_COMPRESSED_IMAGE_SIZE   = 0x86A0;
        public const uint GL_TEXTURE_COMPRESSED              = 0x86A1;
        public const uint GL_NUM_COMPRESSED_TEXTURE_FORMATS  = 0x86A2;
        public const uint GL_COMPRESSED_TEXTURE_FORMATS      = 0x86A3;
        public const uint GL_CLAMP_TO_BORDER                 = 0x812D;

        #endregion
        
        #region OpenGL 1.4

        //  Methods
        public void BlendFuncSeparate(uint sfactorRGB, uint dfactorRGB, uint sfactorAlpha, uint dfactorAlpha)
        {
            GetDelegateFor<glBlendFuncSeparate>()(sfactorRGB, dfactorRGB, sfactorAlpha, dfactorAlpha);
        }
        public void MultiDrawArrays(uint mode, int[] first, int[] count, int primcount)
        {
            GetDelegateFor<glMultiDrawArrays>()(mode, first, count, primcount);
        }
        public void MultiDrawElements(uint mode, int[] count, uint type, IntPtr indices, int primcount)
        {
            GetDelegateFor<glMultiDrawElements>()(mode, count, type, indices, primcount);
        }
        public void PointParameter(uint pname, float parameter)
        {
            GetDelegateFor<glPointParameterf>()(pname, parameter);
        }
        public void PointParameter(uint pname, float[] parameters)
        {
            GetDelegateFor<glPointParameterfv>()(pname, parameters);
        }
        public void PointParameter(uint pname, int parameter)
        {
            GetDelegateFor<glPointParameteri>()(pname, parameter);
        }
        public void PointParameter(uint pname, int[] parameters)
        {
            GetDelegateFor<glPointParameteriv>()(pname, parameters);
        }
        
        //  Deprecated Methods
        [Obsolete]
        public void FogCoord(float coord)
        {
            GetDelegateFor<glFogCoordf>()(coord);
        }
        [Obsolete]
        public void FogCoord(float[] coord)
        {
            GetDelegateFor<glFogCoordfv>()(coord);
        }
        [Obsolete]
        public void FogCoord(double coord)
        {
            GetDelegateFor<glFogCoordd>()(coord);
        }
        [Obsolete]
        public void FogCoord(double[] coord)
        {
            GetDelegateFor<glFogCoorddv>()(coord);
        }
        [Obsolete]
        public void FogCoordPointer(uint type, int stride, IntPtr pointer)
        {
            GetDelegateFor<glFogCoordPointer>()(type, stride, pointer);
        }
        [Obsolete]
        public void SecondaryColor3(sbyte red, sbyte green, sbyte blue)
        {
            GetDelegateFor<glSecondaryColor3b>()(red, green, blue);
        }
        [Obsolete]
        public void SecondaryColor3(sbyte[] v)
        {
            GetDelegateFor<glSecondaryColor3bv>()(v);
        }
        [Obsolete]
        public void SecondaryColor3(double red, double green, double blue)
        {
            GetDelegateFor<glSecondaryColor3d>()(red, green, blue);
        }
        [Obsolete]
        public void SecondaryColor3(double[] v)
        {
            GetDelegateFor<glSecondaryColor3dv>()(v);
        }
        [Obsolete]
        public void SecondaryColor3(float red, float green, float blue)
        {
            GetDelegateFor<glSecondaryColor3f>()(red, green, blue);
        }
        [Obsolete]
        public void SecondaryColor3(float[] v)
        {
            GetDelegateFor<glSecondaryColor3fv>()(v);
        }
        [Obsolete]
        public void SecondaryColor3(int red, int green, int blue)
        {
            GetDelegateFor<glSecondaryColor3i>()(red, green, blue);
        }
        [Obsolete]
        public void SecondaryColor3(int[] v)
        {
            GetDelegateFor<glSecondaryColor3iv>()(v);
        }
        [Obsolete]
        public void SecondaryColor3(short red, short green, short blue)
        {
            GetDelegateFor<glSecondaryColor3s>()(red, green, blue);
        }
        [Obsolete]
        public void SecondaryColor3(short[] v)
        {
            GetDelegateFor<glSecondaryColor3sv>()(v);
        }
        [Obsolete]
        public void SecondaryColor3(byte red, byte green, byte blue)
        {
            GetDelegateFor<glSecondaryColor3ub>()(red, green, blue);
        }
        [Obsolete]
        public void SecondaryColor3(byte[] v)
        {
            GetDelegateFor<glSecondaryColor3ubv>()(v);
        }
        [Obsolete]
        public void SecondaryColor3(uint red, uint green, uint blue)
        {
            GetDelegateFor<glSecondaryColor3ui>()(red, green, blue);
        }
        [Obsolete]
        public void SecondaryColor3(uint[] v)
        {
            GetDelegateFor<glSecondaryColor3uiv>()(v);
        }
        [Obsolete]
        public void SecondaryColor3(ushort red, ushort green, ushort blue)
        {
            GetDelegateFor<glSecondaryColor3us>()(red, green, blue);
        }
        [Obsolete]
        public void SecondaryColor3(ushort[] v)
        {
            GetDelegateFor<glSecondaryColor3usv>()(v);
        }
        [Obsolete]
        public void SecondaryColorPointer(int size, uint type, int stride, IntPtr pointer)
        {
            GetDelegateFor<glSecondaryColorPointer>()(size, type, stride, pointer);
        }
        [Obsolete]
        public void WindowPos2(double x, double y)
        {
            GetDelegateFor<glWindowPos2d>()(x, y);
        }
        [Obsolete]
        public void WindowPos2(double[] v)
        {
            GetDelegateFor<glWindowPos2dv>()(v);
        }
        [Obsolete]
        public void WindowPos2(float x, float y)
        {
            GetDelegateFor<glWindowPos2f>()(x, y);
        }
        [Obsolete]
        public void WindowPos2(float[] v)
        {
            GetDelegateFor<glWindowPos2fv>()(v);
        }
        [Obsolete]
        public void WindowPos2(int x, int y)
        {
            GetDelegateFor<glWindowPos2i>()(x, y);
        }
        [Obsolete]
        public void WindowPos2(int[] v)
        {
            GetDelegateFor<glWindowPos2iv>()(v);
        }
        [Obsolete]
        public void WindowPos2(short x, short y)
        {
            GetDelegateFor<glWindowPos2s>()(x, y);
        }
        [Obsolete]
        public void WindowPos2(short[] v)
        {
            GetDelegateFor<glWindowPos2sv>()(v);
        }
        [Obsolete]
        public void WindowPos3(double x, double y, double z)
        {
            GetDelegateFor<glWindowPos3d>()(x, y, z);
        }
        [Obsolete]
        public void WindowPos3(double[] v)
        {
            GetDelegateFor<glWindowPos3dv>()(v);
        }
        [Obsolete]
        public void WindowPos3(float x, float y, float z)
        {
            GetDelegateFor<glWindowPos3f>()(x, y, z);
        }
        [Obsolete]
        public void WindowPos3(float[] v)
        {
            GetDelegateFor<glWindowPos3fv>()(v);
        }
        [Obsolete]
        public void WindowPos3(int x, int y, int z)
        {
            GetDelegateFor<glWindowPos3i>()(x, y, z);
        }
        [Obsolete]
        public void WindowPos3(int[] v)
        {
            GetDelegateFor<glWindowPos3iv>()(v);
        }
        [Obsolete]
        public void WindowPos3(short x, short y, short z)
        {
            GetDelegateFor<glWindowPos3s>()(x, y, z);
        }
        [Obsolete]
        public void WindowPos3(short[] v)
        {
            GetDelegateFor<glWindowPos3sv>()(v);
        }

        //  Delegates
        private delegate void glBlendFuncSeparate (uint sfactorRGB, uint dfactorRGB, uint sfactorAlpha, uint dfactorAlpha);
        private delegate void glMultiDrawArrays (uint mode, int[] first, int[] count, int primcount);
        private delegate void glMultiDrawElements (uint mode, int[] count, uint type, IntPtr indices, int primcount);
        private delegate void glPointParameterf (uint pname, float parameter);
        private delegate void glPointParameterfv (uint pname, float[] parameters);
        private delegate void glPointParameteri (uint pname, int parameter);
        private delegate void glPointParameteriv (uint pname, int[] parameters);
        private delegate void glFogCoordf (float coord);
        private delegate void glFogCoordfv (float[] coord);
        private delegate void glFogCoordd (double coord);
        private delegate void glFogCoorddv (double[] coord);
        private delegate void glFogCoordPointer (uint type, int stride, IntPtr pointer);
        private delegate void glSecondaryColor3b (sbyte red, sbyte green, sbyte blue);
        private delegate void glSecondaryColor3bv (sbyte[] v);
        private delegate void glSecondaryColor3d (double red, double green, double blue);
        private delegate void glSecondaryColor3dv (double[] v);
        private delegate void glSecondaryColor3f (float red, float green, float blue);
        private delegate void glSecondaryColor3fv (float[] v);
        private delegate void glSecondaryColor3i (int red, int green, int blue);
        private delegate void glSecondaryColor3iv (int[] v);
        private delegate void glSecondaryColor3s (short red, short green, short blue);
        private delegate void glSecondaryColor3sv (short[] v);
        private delegate void glSecondaryColor3ub (byte red, byte green, byte blue);
        private delegate void glSecondaryColor3ubv (byte[] v);
        private delegate void glSecondaryColor3ui (uint red, uint green, uint blue);
        private delegate void glSecondaryColor3uiv (uint[] v);
        private delegate void glSecondaryColor3us (ushort red, ushort green, ushort blue);
        private delegate void glSecondaryColor3usv (ushort[] v);
        private delegate void glSecondaryColorPointer (int size, uint type, int stride, IntPtr pointer);
        private delegate void glWindowPos2d (double x, double y);
        private delegate void glWindowPos2dv (double[] v);
        private delegate void glWindowPos2f (float x, float y);
        private delegate void glWindowPos2fv (float[] v);
        private delegate void glWindowPos2i (int x, int y);
        private delegate void glWindowPos2iv (int[] v);
        private delegate void glWindowPos2s (short x, short y);
        private delegate void glWindowPos2sv (short[] v);
        private delegate void glWindowPos3d (double x, double y, double z);
        private delegate void glWindowPos3dv (double[] v);
        private delegate void glWindowPos3f (float x, float y, float z);
        private delegate void glWindowPos3fv (float[] v);
        private delegate void glWindowPos3i (int x, int y, int z);
        private delegate void glWindowPos3iv (int[] v);
        private delegate void glWindowPos3s (short x, short y, short z);
        private delegate void glWindowPos3sv (short[] v);

        //  Constants
        public const uint GL_BLEND_DST_RGB                   = 0x80C8;
        public const uint GL_BLEND_SRC_RGB                   = 0x80C9;
        public const uint GL_BLEND_DST_ALPHA                 = 0x80CA;
        public const uint GL_BLEND_SRC_ALPHA                 = 0x80CB;
        public const uint GL_POINT_FADE_THRESHOLD_SIZE       = 0x8128;
        public const uint GL_DEPTH_COMPONENT16               = 0x81A5;
        public const uint GL_DEPTH_COMPONENT24               = 0x81A6;
        public const uint GL_DEPTH_COMPONENT32               = 0x81A7;
        public const uint GL_MIRRORED_REPEAT                 = 0x8370;
        public const uint GL_MAX_TEXTURE_LOD_BIAS            = 0x84FD;
        public const uint GL_TEXTURE_LOD_BIAS                = 0x8501;
        public const uint GL_INCR_WRAP                       = 0x8507;
        public const uint GL_DECR_WRAP                       = 0x8508;
        public const uint GL_TEXTURE_DEPTH_SIZE              = 0x884A;
        public const uint GL_TEXTURE_COMPARE_MODE            = 0x884C;
        public const uint GL_TEXTURE_COMPARE_FUNC            = 0x884D;

        #endregion
        
        #region OpenGL 1.5

        //  Methods
        public void GenQueries(int n, uint[] ids)
        {
            GetDelegateFor<glGenQueries>()(n, ids);
        }
        public void DeleteQueries(int n, uint[] ids)
        {
            GetDelegateFor<glDeleteQueries>()(n, ids);
        }
        public bool IsQuery(uint id)
        {
            return (bool)GetDelegateFor<glIsQuery>()(id);
        }
        public void BeginQuery(uint target, uint id)
        {
            GetDelegateFor<glBeginQuery>()(target, id);
        }
        public void EndQuery(uint target)
        {
            GetDelegateFor<glEndQuery>()(target);
        }
        public void GetQuery(uint target, uint pname, int[] parameters)
        {
            GetDelegateFor<glGetQueryiv>()(target, pname, parameters);
        }
        public void GetQueryObject(uint id, uint pname, int[] parameters)
        {
            GetDelegateFor<glGetQueryObjectiv>()(id, pname, parameters);
        }
        public void GetQueryObject(uint id, uint pname, uint[] parameters)
        {
            GetDelegateFor<glGetQueryObjectuiv>()(id, pname, parameters);
        }
        public void BindBuffer(uint target, uint buffer)
        {
            GetDelegateFor<glBindBuffer>()(target, buffer);
        }
        public void DeleteBuffers(int n, uint[] buffers)
        {
            GetDelegateFor<glDeleteBuffers>()(n, buffers);
        }
        public void GenBuffers(int n, uint[] buffers)
        {
            GetDelegateFor<glGenBuffers>()(n, buffers);
        }
        public bool IsBuffer(uint buffer)
        {
            return (bool)GetDelegateFor<glIsBuffer>()(buffer);
        }
        public void BufferData(uint target, int size, IntPtr data, uint usage)
        {
            GetDelegateFor<glBufferData>()(target, size, data, usage);
        }
        public void BufferData(uint target, float[] data, uint usage)
        {
            IntPtr p = Marshal.AllocHGlobal(data.Length * sizeof(float));
            Marshal.Copy(data, 0, p, data.Length);
            GetDelegateFor<glBufferData>()(target, data.Length * sizeof(float), p, usage);
            Marshal.FreeHGlobal(p);
        }
        public void BufferData(uint target, ushort[] data, uint usage)
        {
            var dataSize = data.Length * sizeof(ushort);
            IntPtr p = Marshal.AllocHGlobal(dataSize);
            var shortData = new short[data.Length];
            Buffer.BlockCopy(data, 0, shortData, 0, dataSize);
            Marshal.Copy(shortData, 0, p, data.Length);
            GetDelegateFor<glBufferData>()(target, dataSize, p, usage);
            Marshal.FreeHGlobal(p);
        }
        public void BufferSubData(uint target, int offset, int size, IntPtr data)
        {
            GetDelegateFor<glBufferSubData>()(target, offset, size, data);
        }
        public void GetBufferSubData(uint target, int offset, int size, IntPtr data)
        {
            GetDelegateFor<glGetBufferSubData>()(target, offset, size, data);
        }
        public IntPtr MapBuffer(uint target, uint access)
        {
            return (IntPtr)GetDelegateFor<glMapBuffer>()(target, access);
        }
        public bool UnmapBuffer(uint target)
        {
            return (bool)GetDelegateFor<glUnmapBuffer>()(target);
        }
        public void GetBufferParameter(uint target, uint pname, int[] parameters)
        {
            GetDelegateFor<glGetBufferParameteriv>()(target, pname, parameters);
        }
        public void GetBufferPointer(uint target, uint pname, IntPtr[] parameters)
        {
            GetDelegateFor<glGetBufferPointerv>()(target, pname, parameters);
        }
        
        //  Delegates
        private delegate void glGenQueries (int n, uint[] ids);
        private delegate void glDeleteQueries (int n, uint[] ids);
        private delegate bool glIsQuery (uint id);
        private delegate void glBeginQuery (uint target, uint id);
        private delegate void glEndQuery (uint target);
        private delegate void glGetQueryiv (uint target, uint pname, int[] parameters);
        private delegate void glGetQueryObjectiv (uint id, uint pname, int[] parameters);
        private delegate void glGetQueryObjectuiv (uint id, uint pname, uint[] parameters);
        private delegate void glBindBuffer (uint target, uint buffer);
        private delegate void glDeleteBuffers (int n, uint[] buffers);
        private delegate void glGenBuffers (int n, uint[] buffers);
        private delegate bool glIsBuffer (uint buffer);
        private delegate void glBufferData(uint target, int size, IntPtr data, uint usage);
        private delegate void glBufferSubData (uint target, int offset, int size, IntPtr data);
        private delegate void glGetBufferSubData (uint target, int offset, int size, IntPtr data);
        private delegate IntPtr glMapBuffer (uint target, uint access);
        private delegate bool glUnmapBuffer (uint target);
        private delegate void glGetBufferParameteriv (uint target, uint pname, int[] parameters);
        private delegate void glGetBufferPointerv (uint target, uint pname, IntPtr[] parameters);

        //  Constants
        public const uint GL_BUFFER_SIZE                             = 0x8764;
        public const uint GL_BUFFER_USAGE                            = 0x8765;
        public const uint GL_QUERY_COUNTER_BITS                      = 0x8864;
        public const uint GL_CURRENT_QUERY                           = 0x8865;
        public const uint GL_QUERY_RESULT                            = 0x8866;
        public const uint GL_QUERY_RESULT_AVAILABLE                  = 0x8867;
        public const uint GL_ARRAY_BUFFER                            = 0x8892;
        public const uint GL_ELEMENT_ARRAY_BUFFER                    = 0x8893;
        public const uint GL_ARRAY_BUFFER_BINDING                    = 0x8894;
        public const uint GL_ELEMENT_ARRAY_BUFFER_BINDING            = 0x8895;
        public const uint GL_VERTEX_ATTRIB_ARRAY_BUFFER_BINDING      = 0x889F;
        public const uint GL_READ_ONLY                               = 0x88B8;
        public const uint GL_WRITE_ONLY                              = 0x88B9;
        public const uint GL_READ_WRITE                              = 0x88BA;
        public const uint GL_BUFFER_ACCESS                           = 0x88BB;
        public const uint GL_BUFFER_MAPPED                           = 0x88BC;
        public const uint GL_BUFFER_MAP_POINTER                      = 0x88BD;
        public const uint GL_STREAM_DRAW                             = 0x88E0;
        public const uint GL_STREAM_READ                             = 0x88E1;
        public const uint GL_STREAM_COPY                             = 0x88E2;
        public const uint GL_STATIC_DRAW                             = 0x88E4;
        public const uint GL_STATIC_READ                             = 0x88E5;
        public const uint GL_STATIC_COPY                             = 0x88E6;
        public const uint GL_DYNAMIC_DRAW                            = 0x88E8;
        public const uint GL_DYNAMIC_READ                            = 0x88E9;
        public const uint GL_DYNAMIC_COPY                            = 0x88EA;
        public const uint GL_SAMPLES_PASSED                          = 0x8914;

        #endregion
        
        #region OpenGL 2.0

        //  Methods
        public void BlendEquationSeparate (uint modeRGB, uint modeAlpha)
        {
            GetDelegateFor<glBlendEquationSeparate>()(modeRGB, modeAlpha);
        }
        public void DrawBuffers (int n, uint[] bufs)
        {
            GetDelegateFor<glDrawBuffers>()(n, bufs);
        }
        public void StencilOpSeparate (uint face, uint sfail, uint dpfail, uint dppass)
        {
            GetDelegateFor<glStencilOpSeparate>()(face, sfail, dpfail, dppass);
        }
        public void StencilFuncSeparate (uint face, uint func, int reference, uint mask)
        {
            GetDelegateFor<glStencilFuncSeparate>()(face, func, reference, mask);
        }
        public void StencilMaskSeparate (uint face, uint mask)
        {
            GetDelegateFor<glStencilMaskSeparate>()(face, mask);
        }
        public void AttachShader (uint program, uint shader)
        {
            GetDelegateFor<glAttachShader>()(program, shader);
        }
        public void BindAttribLocation (uint program, uint index, string name)
        {
            GetDelegateFor<glBindAttribLocation>()(program, index, name);
        }
        /// <summary>
        /// Compile a shader object
        /// </summary>
        /// <param name="shader">Specifies the shader object to be compiled.</param>
        public void CompileShader (uint shader)
        {
            GetDelegateFor<glCompileShader>()(shader);
        }
        public uint CreateProgram ()
        {
            return (uint)GetDelegateFor<glCreateProgram>()();
        }
        /// <summary>
        /// Create a shader object
        /// </summary>
        /// <param name="type">Specifies the type of shader to be created. Must be either GL_VERTEX_SHADER or GL_FRAGMENT_SHADER.</param>
        /// <returns>This function returns 0 if an error occurs creating the shader object. Otherwise the shader id is returned.</returns>
        public uint CreateShader (uint type)
        {
            return (uint)GetDelegateFor<glCreateShader>()(type);
        }
        public void DeleteProgram (uint program)
        {
            GetDelegateFor<glDeleteProgram>()(program);
        }
        public void DeleteShader (uint shader)
        {
            GetDelegateFor<glDeleteShader>()(shader);
        }
        public void DetachShader (uint program, uint shader)
        {
            GetDelegateFor<glDetachShader>()(program, shader);
        }
        public void DisableVertexAttribArray (uint index)
        {
            GetDelegateFor<glDisableVertexAttribArray>()(index);
        }
        public void EnableVertexAttribArray (uint index)
        {
            GetDelegateFor<glEnableVertexAttribArray>()(index);
        }


        /// <summary>
        /// Return information about an active attribute variable
        /// </summary>
        /// <param name="program">Specifies the program object to be queried.</param>
        /// <param name="index">Specifies the index of the attribute variable to be queried.</param>
        /// <param name="bufSize">Specifies the maximum number of characters OpenGL is allowed to write in the character buffer indicated by <paramref name="name"/>.</param>
        /// <param name="length">Returns the number of characters actually written by OpenGL in the string indicated by name (excluding the null terminator) if a value other than NULL is passed.</param>
        /// <param name="size">Returns the size of the attribute variable.</param>
        /// <param name="type">Returns the data type of the attribute variable.</param>
        /// <param name="name">Returns a null terminated string containing the name of the attribute variable.</param>
        public void GetActiveAttrib (uint program, uint index, int bufSize, out int length, out int size, out uint type, out string name)
        {
            var builder = new StringBuilder(bufSize);
            GetDelegateFor<glGetActiveAttrib>()(program, index, bufSize, out length, out size, out type, builder);
            name = builder.ToString();
        }

        /// <summary>
        /// Return information about an active uniform variable
        /// </summary>
        /// <param name="program">Specifies the program object to be queried.</param>
        /// <param name="index">Specifies the index of the uniform variable to be queried.</param>
        /// <param name="bufSize">Specifies the maximum number of characters OpenGL is allowed 
        /// to write in the character buffer indicated by <paramref name="name"/>.</param>
        /// <param name="length">Returns the number of characters actually written by OpenGL in the string indicated by name 
        /// (excluding the null terminator) if a value other than NULL is passed.</param>
        /// <param name="size">Returns the size of the uniform variable.</param>
        /// <param name="type">Returns the data type of the uniform variable.</param>
        /// <param name="name">Returns a null terminated string containing the name of the uniform variable.</param>
        public void GetActiveUniform (uint program, uint index, int bufSize, out int length, out int size, out uint type, out string name)
        {
            var builder = new StringBuilder(bufSize);
            GetDelegateFor<glGetActiveUniform>()(program, index, bufSize, out length, out size, out type, builder);
            name = builder.ToString();
        }

        public void GetAttachedShaders (uint program, int maxCount, int[] count, uint[] obj)
        {
            GetDelegateFor<glGetAttachedShaders>()(program, maxCount, count, obj);
        }
        public int GetAttribLocation (uint program, string name)
        {
            return (int)GetDelegateFor<glGetAttribLocation>()(program, name);
        }
        public void GetProgram (uint program, uint pname, int[] parameters)
        {
            GetDelegateFor<glGetProgramiv>()(program, pname, parameters);
        }
        public void GetProgramInfoLog(uint program, int bufSize, IntPtr length, StringBuilder infoLog)
        {
            GetDelegateFor<glGetProgramInfoLog>()(program, bufSize, length, infoLog);
        }
        public void GetShader (uint shader, uint pname, int[] parameters)
        {
            GetDelegateFor<glGetShaderiv>()(shader, pname, parameters);
        }
        public void GetShaderInfoLog (uint shader, int bufSize, IntPtr length, StringBuilder infoLog)
        {
            GetDelegateFor<glGetShaderInfoLog>()(shader, bufSize, length, infoLog);

        }
        public void GetShaderSource(uint shader, int bufSize, IntPtr length, StringBuilder source)
        {
            GetDelegateFor<glGetShaderSource>()(shader, bufSize, length, source);
        }
        /// <summary>
        /// Returns an integer that represents the location of a specific uniform variable within a program object. name must be a null terminated string that contains no white space. name must be an active uniform variable name in program that is not a structure, an array of structures, or a subcomponent of a vector or a matrix. This function returns -1 if name does not correspond to an active uniform variable in program, if name starts with the reserved prefix "gl_", or if name is associated with an atomic counter or a named uniform block.
        /// </summary>
        /// <param name="program">Specifies the program object to be queried.</param>
        /// <param name="name">Points to a null terminated string containing the name of the uniform variable whose location is to be queried.</param>
        /// <returns></returns>
        public int GetUniformLocation (uint program, string name)
        {
            return (int)GetDelegateFor<glGetUniformLocation>()(program, name);
        }
        public void GetUniform (uint program, int location, float[] parameters)
        {
            GetDelegateFor<glGetUniformfv>()(program, location, parameters);
        }
        public void GetUniform (uint program, int location, int[] parameters)
        {
            GetDelegateFor<glGetUniformiv>()(program, location, parameters);
        }
        public void GetVertexAttrib (uint index, uint pname, double[] parameters)
        {
            GetDelegateFor<glGetVertexAttribdv>()(index, pname, parameters);
        }
        public void GetVertexAttrib (uint index, uint pname, float[] parameters)
        {
            GetDelegateFor<glGetVertexAttribfv>()(index, pname, parameters);
        }
        public void GetVertexAttrib (uint index, uint pname, int[] parameters)
        {
            GetDelegateFor<glGetVertexAttribiv>()(index, pname, parameters);
        }
        public void GetVertexAttribPointer(uint index, uint pname, IntPtr pointer)
        {
            GetDelegateFor<glGetVertexAttribPointerv>()(index, pname, pointer);
        }
        public bool IsProgram (uint program)
        {
            return (bool)GetDelegateFor<glIsProgram>()(program);
        }
        public bool IsShader (uint shader)
        {
            return (bool)GetDelegateFor<glIsShader>()(shader);
        }
        public void LinkProgram (uint program)
        {
            GetDelegateFor<glLinkProgram>()(program);
        }

        /// <summary>
        /// Replace the source code in a shader object
        /// </summary>
        /// <param name="shader">Specifies the handle of the shader object whose source code is to be replaced.</param>
        /// <param name="source">The source.</param>
        public void ShaderSource (uint shader, string source)
        {
            //  Remember, the function takes an array of strings but concatenates them, so we should NOT split into lines!
            GetDelegateFor<glShaderSource>()(shader, 1, new[] { source }, new[] { source.Length });
        }

        public static IntPtr StringToPtrAnsi(string str)
        {
            if (string.IsNullOrEmpty(str))
                return IntPtr.Zero;

            byte[] bytes = Encoding.ASCII.GetBytes(str + '\0');
            IntPtr strPtr = Marshal.AllocHGlobal(bytes.Length);
            Marshal.Copy(bytes, 0, strPtr, bytes.Length);

            return strPtr;
        }
        public void UseProgram (uint program)
        {
            GetDelegateFor<glUseProgram>()(program);
        }
        public void Uniform1 (int location, float v0)
        {
            GetDelegateFor<glUniform1f>()(location, v0);
        }
        public void Uniform2 (int location, float v0, float v1)
        {
            GetDelegateFor<glUniform2f>()(location, v0, v1);
        }
        public void Uniform3 (int location, float v0, float v1, float v2)
        {
            GetDelegateFor<glUniform3f>()(location, v0, v1, v2);
        }
        public void Uniform4 (int location, float v0, float v1, float v2, float v3)
        {
            GetDelegateFor<glUniform4f>()(location, v0, v1, v2, v3);
        }
        public void Uniform1 (int location, int v0)
        {
            GetDelegateFor<glUniform1i>()(location, v0);
        }
        public void Uniform2 (int location, int v0, int v1)
        {
            GetDelegateFor<glUniform2i>()(location, v0, v1);
        }
        public void Uniform3(int location, int v0, int v1, int v2)
        {
            GetDelegateFor<glUniform3i>()(location, v0, v1, v2);
        }
        public void Uniform (int location, int v0, int v1, int v2, int v3)
        {
            GetDelegateFor<glUniform4i>()(location, v0, v1, v2, v3);
        }
        public void Uniform1 (int location, int count, float[] value)
        {
            GetDelegateFor<glUniform1fv>()(location, count, value);
        }
        public void Uniform2 (int location, int count, float[] value)
        {
            GetDelegateFor<glUniform2fv>()(location, count, value);
        }
        public void Uniform3 (int location, int count, float[] value)
        {
            GetDelegateFor<glUniform3fv>()(location, count, value);
        }
        public void Uniform4 (int location, int count, float[] value)
        {
            GetDelegateFor<glUniform4fv>()(location, count, value);
        }
        public void Uniform1 (int location, int count, int[] value)
        {
            GetDelegateFor<glUniform1iv>()(location, count, value);
        }
        public void Uniform2 (int location, int count, int[] value)
        {
            GetDelegateFor<glUniform2iv>()(location, count, value);
        }
        public void Uniform3 (int location, int count, int[] value)
        {
            GetDelegateFor<glUniform3iv>()(location, count, value);
        }
        public void Uniform4 (int location, int count, int[] value)
        {
            GetDelegateFor<glUniform4iv>()(location, count, value);
        }
        public void UniformMatrix2 (int location, int count, bool transpose, float[] value)
        {
            GetDelegateFor<glUniformMatrix2fv>()(location, count, transpose, value);
        }
        public void UniformMatrix3 (int location, int count, bool transpose, float[] value)
        {
            GetDelegateFor<glUniformMatrix3fv>()(location, count, transpose, value);
        }
        public void UniformMatrix4 (int location, int count, bool transpose, float[] value)
        {
            GetDelegateFor<glUniformMatrix4fv>()(location, count, transpose, value);
        }
        public void ValidateProgram (uint program)
        {
            GetDelegateFor<glValidateProgram>()(program);
        }
        public void VertexAttrib1 (uint index, double x)
        {
            GetDelegateFor<glVertexAttrib1d>()(index, x);
        }
        public void VertexAttrib1 (uint index, double[] v)
        {
            GetDelegateFor<glVertexAttrib1dv>()(index, v);
        }
        public void VertexAttrib (uint index, float x)
        {
            GetDelegateFor<glVertexAttrib1f>()(index, x);
        }
        public void VertexAttrib1 (uint index, float[] v)
        {
            GetDelegateFor<glVertexAttrib1fv>()(index, v);
        }
        public void VertexAttrib (uint index, short x)
        {
            GetDelegateFor<glVertexAttrib1s>()(index, x);
        }
        public void VertexAttrib1 (uint index, short[] v)
        {
            GetDelegateFor<glVertexAttrib1sv>()(index, v);
        }
        public void VertexAttrib2 (uint index, double x, double y)
        {
            GetDelegateFor<glVertexAttrib2d>()(index, x, y);
        }
        public void VertexAttrib2 (uint index, double[] v)
        {
            GetDelegateFor<glVertexAttrib2dv>()(index, v);
        }
        public void VertexAttrib2 (uint index, float x, float y)
        {
            GetDelegateFor<glVertexAttrib2f>()(index, x, y);
        }
        public void VertexAttrib2 (uint index, float[] v)
        {
            GetDelegateFor<glVertexAttrib2fv>()(index, v);
        }
        public void VertexAttrib2 (uint index, short x, short y)
        {
            GetDelegateFor<glVertexAttrib2s>()(index, x, y);
        }
        public void VertexAttrib2 (uint index, short[] v)
        {
            GetDelegateFor<glVertexAttrib2sv>()(index, v);
        }
        public void VertexAttrib3 (uint index, double x, double y, double z)
        {
            GetDelegateFor<glVertexAttrib3d>()(index, x, y, z);
        }
        public void VertexAttrib3 (uint index, double[] v)
        {
            GetDelegateFor<glVertexAttrib3dv>()(index, v);
        }
        public void VertexAttrib3 (uint index, float x, float y, float z)
        {
            GetDelegateFor<glVertexAttrib3f>()(index, x, y, z);
        }
        public void VertexAttrib3 (uint index, float[] v)
        {
            GetDelegateFor<glVertexAttrib3fv>()(index, v);
        }
        public void VertexAttrib3 (uint index, short x, short y, short z)
        {
            GetDelegateFor<glVertexAttrib3s>()(index, x, y, z);
        }
        public void VertexAttrib3 (uint index, short[] v)
        {
            GetDelegateFor<glVertexAttrib3sv>()(index, v);
        }
        public void VertexAttrib4N (uint index, sbyte[] v)
        {
            GetDelegateFor<glVertexAttrib4Nbv>()(index, v);
        }
        public void VertexAttrib4N (uint index, int[] v)
        {
            GetDelegateFor<glVertexAttrib4Niv>()(index, v);
        }
        public void VertexAttrib4N (uint index, short[] v)
        {
            GetDelegateFor<glVertexAttrib4Nsv>()(index, v);
        }
        public void VertexAttrib4N (uint index, byte x, byte y, byte z, byte w)
        {
            GetDelegateFor<glVertexAttrib4Nub>()(index, x, y, z, w);
        }
        public void VertexAttrib4N (uint index, byte[] v)
        {
            GetDelegateFor<glVertexAttrib4Nubv>()(index, v);
        }
        public void VertexAttrib4N (uint index, uint[] v)
        {
            GetDelegateFor<glVertexAttrib4Nuiv>()(index, v);
        }
        public void VertexAttrib4N (uint index, ushort[] v)
        {
            GetDelegateFor<glVertexAttrib4Nusv>()(index, v);
        }
        public void VertexAttrib4 (uint index, sbyte[] v)
        {
            GetDelegateFor<glVertexAttrib4bv>()(index, v);
        }
        public void VertexAttrib4 (uint index, double x, double y, double z, double w)
        {
            GetDelegateFor<glVertexAttrib4d>()(index, x, y, z, w);
        }
        public void VertexAttrib4 (uint index, double[] v)
        {
            GetDelegateFor<glVertexAttrib4dv>()(index, v);
        }
        public void VertexAttrib4 (uint index, float x, float y, float z, float w)
        {
            GetDelegateFor<glVertexAttrib4f>()(index, x, y, z, w);
        }
        public void VertexAttrib4 (uint index, float[] v)
        {
            GetDelegateFor<glVertexAttrib4fv>()(index, v);
        }
        public void VertexAttrib4 (uint index, int[] v)
        {
            GetDelegateFor<glVertexAttrib4iv>()(index, v);
        }
        public void VertexAttrib4 (uint index, short x, short y, short z, short w)
        {
            GetDelegateFor<glVertexAttrib4s>()(index, x, y, z, w);
        }
        public void VertexAttrib4 (uint index, short[] v)
        {
            GetDelegateFor<glVertexAttrib4sv>()(index, v);
        }
        public void VertexAttrib4 (uint index, byte[] v)
        {
            GetDelegateFor<glVertexAttrib4ubv>()(index, v);
        }
        public void VertexAttrib4 (uint index, uint[] v)
        {
            GetDelegateFor<glVertexAttrib4uiv>()(index, v);
        }
        public void VertexAttrib4 (uint index, ushort[] v)
        {
            GetDelegateFor<glVertexAttrib4usv>()(index, v);
        }
        public void VertexAttribPointer (uint index, int size, uint type, bool normalized, int stride, IntPtr pointer)
        {
            GetDelegateFor<glVertexAttribPointer>()(index, size, type, normalized, stride, pointer);
        }

        //  Delegates
        private delegate void glBlendEquationSeparate (uint modeRGB, uint modeAlpha);
        private delegate void glDrawBuffers (int n, uint[] bufs);
        private delegate void glStencilOpSeparate (uint face, uint sfail, uint dpfail, uint dppass);
        private delegate void glStencilFuncSeparate (uint face, uint func, int reference, uint mask);
        private delegate void glStencilMaskSeparate (uint face, uint mask);
        private delegate void glAttachShader (uint program, uint shader);
        private delegate void glBindAttribLocation (uint program, uint index, string name);
        private delegate void glCompileShader (uint shader);
        private delegate uint glCreateProgram ();
        private delegate uint glCreateShader (uint type);
        private delegate void glDeleteProgram (uint program);
        private delegate void glDeleteShader (uint shader);
        private delegate void glDetachShader (uint program, uint shader);
        private delegate void glDisableVertexAttribArray (uint index);
        private delegate void glEnableVertexAttribArray (uint index);
        private delegate void glGetActiveAttrib (uint program, uint index, int bufSize, out int length, out int size, out uint type, StringBuilder name);
        private delegate void glGetActiveUniform (uint program, uint index, int bufSize, out int length, out int size, out uint type, StringBuilder name);
        private delegate void glGetAttachedShaders (uint program, int maxCount, int[] count, uint[] obj);
        private delegate int glGetAttribLocation (uint program, string name);
        private delegate void glGetProgramiv (uint program, uint pname, int[] parameters);
        private delegate void glGetProgramInfoLog(uint program, int bufSize, IntPtr length, StringBuilder infoLog);
        private delegate void glGetShaderiv (uint shader, uint pname, int[] parameters);
        private delegate void glGetShaderInfoLog (uint shader, int bufSize, IntPtr length, StringBuilder infoLog);
        private delegate void glGetShaderSource (uint shader, int bufSize, IntPtr length, StringBuilder source);
        private delegate int glGetUniformLocation (uint program, string name);
        private delegate void glGetUniformfv (uint program, int location, float[] parameters);
        private delegate void glGetUniformiv (uint program, int location, int[] parameters);
        private delegate void glGetVertexAttribdv (uint index, uint pname, double[] parameters);
        private delegate void glGetVertexAttribfv (uint index, uint pname, float[] parameters);
        private delegate void glGetVertexAttribiv (uint index, uint pname, int[] parameters);
        private delegate void glGetVertexAttribPointerv (uint index, uint pname, IntPtr pointer);
        private delegate bool glIsProgram (uint program);
        private delegate bool glIsShader (uint shader);
        private delegate void glLinkProgram (uint program);
        //  By specifying 'ThrowOnUnmappableChar' we protect ourselves from inadvertantly using a unicode character
        //  in the source which the marshaller cannot map. Without this, it maps it to '?' leading to long and pointless
        //  sessions of trying to find bugs in the shader, which are most often just copied and pasted unicode characters!
        //  If you're getting exceptions here, remove all unicode crap from your input files (remember, some unicode 
        //  characters you can't even see).
        [UnmanagedFunctionPointer(CallingConvention.StdCall, ThrowOnUnmappableChar = true)]
        private delegate void glShaderSource (uint shader, int count, string[] source, int[] length);
        private delegate void glUseProgram (uint program);
        private delegate void glUniform1f (int location, float v0);
        private delegate void glUniform2f (int location, float v0, float v1);
        private delegate void glUniform3f (int location, float v0, float v1, float v2);
        private delegate void glUniform4f (int location, float v0, float v1, float v2, float v3);
        private delegate void glUniform1i (int location, int v0);
        private delegate void glUniform2i (int location, int v0, int v1);
        private delegate void glUniform3i (int location, int v0, int v1, int v2);
        private delegate void glUniform4i (int location, int v0, int v1, int v2, int v3);
        private delegate void glUniform1fv (int location, int count, float[] value);
        private delegate void glUniform2fv (int location, int count, float[] value);
        private delegate void glUniform3fv (int location, int count, float[] value);
        private delegate void glUniform4fv (int location, int count, float[] value);
        private delegate void glUniform1iv (int location, int count, int[] value);
        private delegate void glUniform2iv (int location, int count, int[] value);
        private delegate void glUniform3iv (int location, int count, int[] value);
        private delegate void glUniform4iv (int location, int count, int[] value);
        private delegate void glUniformMatrix2fv (int location, int count, bool transpose, float[] value);
        private delegate void glUniformMatrix3fv (int location, int count, bool transpose, float[] value);
        private delegate void glUniformMatrix4fv (int location, int count, bool transpose, float[] value);
        private delegate void glValidateProgram (uint program);
        private delegate void glVertexAttrib1d (uint index, double x);
        private delegate void glVertexAttrib1dv (uint index, double[] v);
        private delegate void glVertexAttrib1f (uint index, float x);
        private delegate void glVertexAttrib1fv (uint index, float[] v);
        private delegate void glVertexAttrib1s (uint index, short x);
        private delegate void glVertexAttrib1sv (uint index, short[] v);
        private delegate void glVertexAttrib2d (uint index, double x, double y);
        private delegate void glVertexAttrib2dv (uint index, double[] v);
        private delegate void glVertexAttrib2f (uint index, float x, float y);
        private delegate void glVertexAttrib2fv (uint index, float[] v);
        private delegate void glVertexAttrib2s (uint index, short x, short y);
        private delegate void glVertexAttrib2sv (uint index, short[] v);
        private delegate void glVertexAttrib3d (uint index, double x, double y, double z);
        private delegate void glVertexAttrib3dv (uint index, double[] v);
        private delegate void glVertexAttrib3f (uint index, float x, float y, float z);
        private delegate void glVertexAttrib3fv (uint index, float[] v);
        private delegate void glVertexAttrib3s (uint index, short x, short y, short z);
        private delegate void glVertexAttrib3sv (uint index, short[] v);
        private delegate void glVertexAttrib4Nbv (uint index, sbyte[] v);
        private delegate void glVertexAttrib4Niv (uint index, int[] v);
        private delegate void glVertexAttrib4Nsv (uint index, short[] v);
        private delegate void glVertexAttrib4Nub (uint index, byte x, byte y, byte z, byte w);
        private delegate void glVertexAttrib4Nubv (uint index, byte[] v);
        private delegate void glVertexAttrib4Nuiv (uint index, uint[] v);
        private delegate void glVertexAttrib4Nusv (uint index, ushort[] v);
        private delegate void glVertexAttrib4bv (uint index, sbyte[] v);
        private delegate void glVertexAttrib4d (uint index, double x, double y, double z, double w);
        private delegate void glVertexAttrib4dv (uint index, double[] v);
        private delegate void glVertexAttrib4f (uint index, float x, float y, float z, float w);
        private delegate void glVertexAttrib4fv (uint index, float[] v);
        private delegate void glVertexAttrib4iv (uint index, int[] v);
        private delegate void glVertexAttrib4s (uint index, short x, short y, short z, short w);
        private delegate void glVertexAttrib4sv (uint index, short[] v);
        private delegate void glVertexAttrib4ubv (uint index, byte[] v);
        private delegate void glVertexAttrib4uiv (uint index, uint[] v);
        private delegate void glVertexAttrib4usv (uint index, ushort[] v);
        private delegate void glVertexAttribPointer (uint index, int size, uint type, bool normalized, int stride, IntPtr pointer);

        //  Constants
        public const uint GL_BLEND_EQUATION_RGB                  = 0x8009;
        public const uint GL_VERTEX_ATTRIB_ARRAY_ENABLED         = 0x8622;
        public const uint GL_VERTEX_ATTRIB_ARRAY_SIZE            = 0x8623;
        public const uint GL_VERTEX_ATTRIB_ARRAY_STRIDE          = 0x8624;
        public const uint GL_VERTEX_ATTRIB_ARRAY_TYPE            = 0x8625;
        public const uint GL_CURRENT_VERTEX_ATTRIB               = 0x8626;
        public const uint GL_VERTEX_PROGRAM_POINT_SIZE           = 0x8642;
        public const uint GL_VERTEX_ATTRIB_ARRAY_POINTER         = 0x8645;
        public const uint GL_STENCIL_BACK_FUNC                   = 0x8800;
        public const uint GL_STENCIL_BACK_FAIL                   = 0x8801;
        public const uint GL_STENCIL_BACK_PASS_DEPTH_FAIL        = 0x8802;
        public const uint GL_STENCIL_BACK_PASS_DEPTH_PASS        = 0x8803;
        public const uint GL_MAX_DRAW_BUFFERS                    = 0x8824;
        public const uint GL_DRAW_BUFFER0                        = 0x8825;
        public const uint GL_DRAW_BUFFER1                        = 0x8826;
        public const uint GL_DRAW_BUFFER2                        = 0x8827;
        public const uint GL_DRAW_BUFFER3                        = 0x8828;
        public const uint GL_DRAW_BUFFER4                        = 0x8829;
        public const uint GL_DRAW_BUFFER5                        = 0x882A;
        public const uint GL_DRAW_BUFFER6                        = 0x882B;
        public const uint GL_DRAW_BUFFER7                        = 0x882C;
        public const uint GL_DRAW_BUFFER8                        = 0x882D;
        public const uint GL_DRAW_BUFFER9                        = 0x882E;
        public const uint GL_DRAW_BUFFER10                       = 0x882F;
        public const uint GL_DRAW_BUFFER11                       = 0x8830;
        public const uint GL_DRAW_BUFFER12                       = 0x8831;
        public const uint GL_DRAW_BUFFER13                       = 0x8832;
        public const uint GL_DRAW_BUFFER14                       = 0x8833;
        public const uint GL_DRAW_BUFFER15                       = 0x8834;
        public const uint GL_BLEND_EQUATION_ALPHA                = 0x883D;
        public const uint GL_MAX_VERTEX_ATTRIBS                  = 0x8869;
        public const uint GL_VERTEX_ATTRIB_ARRAY_NORMALIZED      = 0x886A;
        public const uint GL_MAX_TEXTURE_IMAGE_UNITS             = 0x8872;
        public const uint GL_FRAGMENT_SHADER                     = 0x8B30;
        public const uint GL_VERTEX_SHADER                       = 0x8B31;
        public const uint GL_MAX_FRAGMENT_UNIFORM_COMPONENTS     = 0x8B49;
        public const uint GL_MAX_VERTEX_UNIFORM_COMPONENTS       = 0x8B4A;
        public const uint GL_MAX_VARYING_FLOATS                  = 0x8B4B;
        public const uint GL_MAX_VERTEX_TEXTURE_IMAGE_UNITS      = 0x8B4C;
        public const uint GL_MAX_COMBINED_TEXTURE_IMAGE_UNITS    = 0x8B4D;
        public const uint GL_SHADER_TYPE                         = 0x8B4F;
        public const uint GL_FLOAT_VEC2                          = 0x8B50;
        public const uint GL_FLOAT_VEC3                          = 0x8B51;
        public const uint GL_FLOAT_VEC4                          = 0x8B52;
        public const uint GL_INT_VEC2                            = 0x8B53;
        public const uint GL_INT_VEC3                            = 0x8B54;
        public const uint GL_INT_VEC4                            = 0x8B55;
        public const uint GL_BOOL                                = 0x8B56;
        public const uint GL_BOOL_VEC2                           = 0x8B57;
        public const uint GL_BOOL_VEC3                           = 0x8B58;
        public const uint GL_BOOL_VEC4                           = 0x8B59;
        public const uint GL_FLOAT_MAT2                          = 0x8B5A;
        public const uint GL_FLOAT_MAT3                          = 0x8B5B;
        public const uint GL_FLOAT_MAT4                          = 0x8B5C;
        public const uint GL_SAMPLER_1D                          = 0x8B5D;
        public const uint GL_SAMPLER_2D                          = 0x8B5E;
        public const uint GL_SAMPLER_3D                          = 0x8B5F;
        public const uint GL_SAMPLER_CUBE                        = 0x8B60;
        public const uint GL_SAMPLER_1D_SHADOW                   = 0x8B61;
        public const uint GL_SAMPLER_2D_SHADOW                   = 0x8B62;
        public const uint GL_DELETE_STATUS                       = 0x8B80;
        public const uint GL_COMPILE_STATUS                      = 0x8B81;
        public const uint GL_LINK_STATUS                         = 0x8B82;
        public const uint GL_VALIDATE_STATUS                     = 0x8B83;
        public const uint GL_INFO_LOG_LENGTH                     = 0x8B84;
        public const uint GL_ATTACHED_SHADERS                    = 0x8B85;
        public const uint GL_ACTIVE_UNIFORMS                     = 0x8B86;
        public const uint GL_ACTIVE_UNIFORM_MAX_LENGTH           = 0x8B87;
        public const uint GL_SHADER_SOURCE_LENGTH                = 0x8B88;
        public const uint GL_ACTIVE_ATTRIBUTES                   = 0x8B89;
        public const uint GL_ACTIVE_ATTRIBUTE_MAX_LENGTH         = 0x8B8A;
        public const uint GL_FRAGMENT_SHADER_DERIVATIVE_HINT     = 0x8B8B;
        public const uint GL_SHADING_LANGUAGE_VERSION            = 0x8B8C;
        public const uint GL_CURRENT_PROGRAM                     = 0x8B8D;
        public const uint GL_POINT_SPRITE_COORD_ORIGIN           = 0x8CA0;
        public const uint GL_LOWER_LEFT                          = 0x8CA1;
        public const uint GL_UPPER_LEFT                          = 0x8CA2;
        public const uint GL_STENCIL_BACK_REF                    = 0x8CA3;
        public const uint GL_STENCIL_BACK_VALUE_MASK             = 0x8CA4;
        public const uint GL_STENCIL_BACK_WRITEMASK              = 0x8CA5;
        
        #endregion

        #region OpenGL 2.1

        //  Methods
        public void UniformMatrix2x3(int location, int count, bool transpose, float[] value)
        {
            GetDelegateFor<glUniformMatrix2x3fv>()(location, count, transpose, value);
        }
        public void UniformMatrix3x2(int location, int count, bool transpose, float[] value)
        {
            GetDelegateFor<glUniformMatrix3x2fv>()(location, count, transpose, value);
        }
        public void UniformMatrix2x4(int location, int count, bool transpose, float[] value)
        {
            GetDelegateFor<glUniformMatrix2x4fv>()(location, count, transpose, value);
        }
        public void UniformMatrix4x2(int location, int count, bool transpose, float[] value)
        {
            GetDelegateFor<glUniformMatrix4x2fv>()(location, count, transpose, value);
        }
        public void UniformMatrix3x4(int location, int count, bool transpose, float[] value)
        {
            GetDelegateFor<glUniformMatrix3x4fv>()(location, count, transpose, value);
        }
        public void UniformMatrix4x3(int location, int count, bool transpose, float[] value)
        {
            GetDelegateFor<glUniformMatrix4x3fv>()(location, count, transpose, value);
        }

        //  Delegates
        private delegate void glUniformMatrix2x3fv (int location, int count, bool transpose, float[] value);
        private delegate void glUniformMatrix3x2fv (int location, int count, bool transpose, float[] value);
        private delegate void glUniformMatrix2x4fv (int location, int count, bool transpose, float[] value);
        private delegate void glUniformMatrix4x2fv (int location, int count, bool transpose, float[] value);
        private delegate void glUniformMatrix3x4fv (int location, int count, bool transpose, float[] value);
        private delegate void glUniformMatrix4x3fv (int location, int count, bool transpose, float[] value);

        //  Constants
        public const uint GL_PIXEL_PACK_BUFFER                   = 0x88EB;
        public const uint GL_PIXEL_UNPACK_BUFFER                 = 0x88EC;
        public const uint GL_PIXEL_PACK_BUFFER_BINDING           = 0x88ED;
        public const uint GL_PIXEL_UNPACK_BUFFER_BINDING         = 0x88EF;
        public const uint GL_FLOAT_MAT2x3                        = 0x8B65;
        public const uint GL_FLOAT_MAT2x4                        = 0x8B66;
        public const uint GL_FLOAT_MAT3x2                        = 0x8B67;
        public const uint GL_FLOAT_MAT3x4                        = 0x8B68;
        public const uint GL_FLOAT_MAT4x2                        = 0x8B69;
        public const uint GL_FLOAT_MAT4x3                        = 0x8B6A;
        public const uint GL_SRGB                                = 0x8C40;
        public const uint GL_SRGB8                               = 0x8C41;
        public const uint GL_SRGB_ALPHA                          = 0x8C42;
        public const uint GL_SRGB8_ALPHA8                        = 0x8C43;
        public const uint GL_COMPRESSED_SRGB                     = 0x8C48;
        public const uint GL_COMPRESSED_SRGB_ALPHA               = 0x8C49;
       
        #endregion

        #region OpenGL 3.0
        
        //  Methods
        public void ColorMask(uint index, bool r, bool g, bool b, bool a)
        {
            GetDelegateFor<glColorMaski>()(index, r, g, b, a);
        }
        public void GetBoolean(uint target, uint index, bool[] data)
        {
            GetDelegateFor<glGetBooleani_v>()(target, index, data);
        }
        public void GetInteger(uint target, uint index, int[] data)
        {
            GetDelegateFor<glGetIntegeri_v>()(target, index, data);
        }
        public void Enable(uint target, uint index)
        {
            GetDelegateFor<glEnablei>()(target, index);
        }
        public void Disable(uint target, uint index)
        {
            GetDelegateFor<glDisablei>()(target, index);
        }
        public bool IsEnabled(uint target, uint index)
        {
            return (bool)GetDelegateFor<glIsEnabledi>()(target, index);
        }
        public void BeginTransformFeedback(uint primitiveMode)
        {
            GetDelegateFor<glBeginTransformFeedback>()(primitiveMode);
        }
        public void EndTransformFeedback()
        {
            GetDelegateFor<glEndTransformFeedback>()();
        }
        public void BindBufferRange(uint target, uint index, uint buffer, int offset, int size)
        {
            GetDelegateFor<glBindBufferRange>()(target, index, buffer, offset, size);
        }
        public void BindBufferBase(uint target, uint index, uint buffer)
        {
            GetDelegateFor<glBindBufferBase>()(target, index, buffer);
        }
        public void TransformFeedbackVaryings(uint program, int count, string[] varyings, uint bufferMode)
        {
            GetDelegateFor<glTransformFeedbackVaryings>()(program, count, varyings, bufferMode);
        }
        public void GetTransformFeedbackVarying(uint program, uint index, int bufSize, int[] length, int[] size, uint[] type, string name)
        {
            GetDelegateFor<glGetTransformFeedbackVarying>()(program, index, bufSize, length, size, type, name);
        }
        public void ClampColor(uint target, uint clamp)
        {
            GetDelegateFor<glClampColor>()(target, clamp);
        }
        public void BeginConditionalRender(uint id, uint mode)
        {
            GetDelegateFor<glBeginConditionalRender>()(id, mode);
        }
        public void EndConditionalRender()
        {
            GetDelegateFor<glEndConditionalRender>()();
        }
        public void VertexAttribIPointer(uint index, int size, uint type, int stride, IntPtr pointer)
        {
            GetDelegateFor<glVertexAttribIPointer>()(index, size, type, stride, pointer);
        }
        public void GetVertexAttribI(uint index, uint pname, int[] parameters)
        {
            GetDelegateFor<glGetVertexAttribIiv>()(index, pname, parameters);
        }
        public void GetVertexAttribI(uint index, uint pname, uint[] parameters)
        {
            GetDelegateFor<glGetVertexAttribIuiv>()(index, pname, parameters);
        }
        public void VertexAttribI1(uint index, int x)
        {
            GetDelegateFor<glVertexAttribI1i>()(index, x);
        }
        public void VertexAttribI2(uint index, int x, int y)
        {
            GetDelegateFor<glVertexAttribI2i>()(index, x, y);
        }
        public void VertexAttribI3(uint index, int x, int y, int z)
        {
            GetDelegateFor<glVertexAttribI3i>()(index, x, y, z);
        }
        public void VertexAttribI4(uint index, int x, int y, int z, int w)
        {
            GetDelegateFor<glVertexAttribI4i>()(index, x, y, z, w);
        }
        public void VertexAttribI1(uint index, uint x)
        {
            GetDelegateFor<glVertexAttribI1ui>()(index, x);
        }
        public void VertexAttribI2(uint index, uint x, uint y)
        {
            GetDelegateFor<glVertexAttribI2ui>()(index, x, y);
        }
        public void VertexAttribI3(uint index, uint x, uint y, uint z)
        {
            GetDelegateFor<glVertexAttribI3ui>()(index, x, y, z);
        }
        public void VertexAttribI4(uint index, uint x, uint y, uint z, uint w)
        {
            GetDelegateFor<glVertexAttribI4ui>()(index, x, y, z, w);
        }
        public void VertexAttribI1(uint index, int[] v)
        {
            GetDelegateFor<glVertexAttribI1iv>()(index, v);
        }
        public void VertexAttribI2(uint index, int[] v)
        {
            GetDelegateFor<glVertexAttribI2iv>()(index, v);
        }
        public void VertexAttribI3(uint index, int[] v)
        {
            GetDelegateFor<glVertexAttribI3iv>()(index, v);
        }
        public void VertexAttribI4(uint index, int[] v)
        {
            GetDelegateFor<glVertexAttribI4iv>()(index, v);
        }
        public void VertexAttribI1(uint index, uint[] v)
        {
            GetDelegateFor<glVertexAttribI1uiv>()(index, v);
        }
        public void VertexAttribI2(uint index, uint[] v)
        {
            GetDelegateFor<glVertexAttribI2uiv>()(index, v);
        }
        public void VertexAttribI3(uint index, uint[] v)
        {
            GetDelegateFor<glVertexAttribI3uiv>()(index, v);
        }
        public void VertexAttribI4(uint index, uint[] v)
        {
            GetDelegateFor<glVertexAttribI4uiv>()(index, v);
        }
        public void VertexAttribI4(uint index, sbyte[] v)
        {
            GetDelegateFor<glVertexAttribI4bv>()(index, v);
        }
        public void VertexAttribI4(uint index, short[] v)
        {
            GetDelegateFor<glVertexAttribI4sv>()(index, v);
        }
        public void VertexAttribI4(uint index, byte[] v)
        {
            GetDelegateFor<glVertexAttribI4ubv>()(index, v);
        }
        public void VertexAttribI4(uint index, ushort[] v)
        {
            GetDelegateFor<glVertexAttribI4usv>()(index, v);
        }
        public void GetUniform(uint program, int location, uint[] parameters)
        {
            GetDelegateFor<glGetUniformuiv>()(program, location, parameters);
        }
        public void BindFragDataLocation(uint program, uint color, string name)
        {
            GetDelegateFor<glBindFragDataLocation>()(program, color, name);
        }
        public int GetFragDataLocation(uint program, string name)
        {
            return (int)GetDelegateFor<glGetFragDataLocation>()(program, name);
        }
        public void Uniform1(int location, uint v0)
        {
            GetDelegateFor<glUniform1ui>()(location, v0);
        }
        public void Uniform2(int location, uint v0, uint v1)
        {
            GetDelegateFor<glUniform2ui>()(location, v0, v1);
        }
        public void Uniform3(int location, uint v0, uint v1, uint v2)
        {
            GetDelegateFor<glUniform3ui>()(location, v0, v1, v2);
        }
        public void Uniform4(int location, uint v0, uint v1, uint v2, uint v3)
        {
            GetDelegateFor<glUniform4ui>()(location, v0, v1, v2, v3);
        }
        public void Uniform1(int location, int count, uint[] value)
        {
            GetDelegateFor<glUniform1uiv>()(location, count, value);
        }
        public void Uniform2(int location, int count, uint[] value)
        {
            GetDelegateFor<glUniform2uiv>()(location, count, value);
        }
        public void Uniform3(int location, int count, uint[] value)
        {
            GetDelegateFor<glUniform3uiv>()(location, count, value);
        }
        public void Uniform4(int location, int count, uint[] value)
        {
            GetDelegateFor<glUniform4uiv>()(location, count, value);
        }
        public void TexParameterI(uint target, uint pname, int[] parameters)
        {
            GetDelegateFor<glTexParameterIiv>()(target, pname, parameters);
        }
        public void TexParameterI(uint target, uint pname, uint[] parameters)
        {
            GetDelegateFor<glTexParameterIuiv>()(target, pname, parameters);
        }
        public void GetTexParameterI(uint target, uint pname, int[] parameters)
        {
            GetDelegateFor<glGetTexParameterIiv>()(target, pname, parameters);
        }
        public void GetTexParameterI(uint target, uint pname, uint[] parameters)
        {
            GetDelegateFor<glGetTexParameterIuiv>()(target, pname, parameters);
        }
        public void ClearBuffer(uint buffer, int drawbuffer, int[] value)
        {
            GetDelegateFor<glClearBufferiv>()(buffer, drawbuffer, value);
        }
        public void ClearBuffer(uint buffer, int drawbuffer, uint[] value)
        {
            GetDelegateFor<glClearBufferuiv>()(buffer, drawbuffer, value);
        }
        public void ClearBuffer(uint buffer, int drawbuffer, float[] value)
        {
            GetDelegateFor<glClearBufferfv>()(buffer, drawbuffer, value);
        }
        public void ClearBuffer(uint buffer, int drawbuffer, float depth, int stencil)
        {
            GetDelegateFor<glClearBufferfi>()(buffer, drawbuffer, depth, stencil);
        }
        public unsafe string GetString(uint name, uint index)
        {
            sbyte* pStr = (sbyte*)GetDelegateFor<glGetStringi>()(name, index);
            var str = new string(pStr);
            return str;
        }

        //  Delegates
        private delegate void glColorMaski (uint index, bool r, bool g, bool b, bool a);
        private delegate void glGetBooleani_v (uint target, uint index, bool[] data);
        private delegate void glGetIntegeri_v (uint target, uint index, int[] data);
        private delegate void glEnablei (uint target, uint index);
        private delegate void glDisablei (uint target, uint index);
        private delegate bool glIsEnabledi (uint target, uint index);
        private delegate void glBeginTransformFeedback (uint primitiveMode);
        private delegate void glEndTransformFeedback ();
        private delegate void glBindBufferRange (uint target, uint index, uint buffer, int offset, int size);
        private delegate void glBindBufferBase (uint target, uint index, uint buffer);
        private delegate void glTransformFeedbackVaryings (uint program, int count, string[] varyings, uint bufferMode);
        private delegate void glGetTransformFeedbackVarying (uint program, uint index, int bufSize, int[] length, int[] size, uint[] type, string name);
        private delegate void glClampColor (uint target, uint clamp);
        private delegate void glBeginConditionalRender (uint id, uint mode);
        private delegate void glEndConditionalRender ();
        private delegate void glVertexAttribIPointer (uint index, int size, uint type, int stride, IntPtr pointer);
        private delegate void glGetVertexAttribIiv (uint index, uint pname, int[] parameters);
        private delegate void glGetVertexAttribIuiv (uint index, uint pname, uint[] parameters);
        private delegate void glVertexAttribI1i (uint index, int x);
        private delegate void glVertexAttribI2i (uint index, int x, int y);
        private delegate void glVertexAttribI3i (uint index, int x, int y, int z);
        private delegate void glVertexAttribI4i (uint index, int x, int y, int z, int w);
        private delegate void glVertexAttribI1ui (uint index, uint x);
        private delegate void glVertexAttribI2ui (uint index, uint x, uint y);
        private delegate void glVertexAttribI3ui (uint index, uint x, uint y, uint z);
        private delegate void glVertexAttribI4ui (uint index, uint x, uint y, uint z, uint w);
        private delegate void glVertexAttribI1iv (uint index, int[] v);
        private delegate void glVertexAttribI2iv (uint index, int[] v);
        private delegate void glVertexAttribI3iv (uint index, int[] v);
        private delegate void glVertexAttribI4iv (uint index, int[] v);
        private delegate void glVertexAttribI1uiv (uint index, uint[] v);
        private delegate void glVertexAttribI2uiv (uint index, uint[] v);
        private delegate void glVertexAttribI3uiv (uint index, uint[] v);
        private delegate void glVertexAttribI4uiv (uint index, uint[] v);
        private delegate void glVertexAttribI4bv (uint index, sbyte[] v);
        private delegate void glVertexAttribI4sv (uint index, short[] v);
        private delegate void glVertexAttribI4ubv (uint index, byte[] v);
        private delegate void glVertexAttribI4usv (uint index, ushort[] v);
        private delegate void glGetUniformuiv (uint program, int location, uint[] parameters);
        private delegate void glBindFragDataLocation (uint program, uint color, string name);
        private delegate int glGetFragDataLocation (uint program, string name);
        private delegate void glUniform1ui (int location, uint v0);
        private delegate void glUniform2ui (int location, uint v0, uint v1);
        private delegate void glUniform3ui (int location, uint v0, uint v1, uint v2);
        private delegate void glUniform4ui (int location, uint v0, uint v1, uint v2, uint v3);
        private delegate void glUniform1uiv (int location, int count, uint[] value);
        private delegate void glUniform2uiv (int location, int count, uint[] value);
        private delegate void glUniform3uiv (int location, int count, uint[] value);
        private delegate void glUniform4uiv (int location, int count, uint[] value);
        private delegate void glTexParameterIiv (uint target, uint pname, int[] parameters);
        private delegate void glTexParameterIuiv (uint target, uint pname, uint[] parameters);
        private delegate void glGetTexParameterIiv (uint target, uint pname, int[] parameters);
        private delegate void glGetTexParameterIuiv (uint target, uint pname, uint[] parameters);
        private delegate void glClearBufferiv (uint buffer, int drawbuffer, int[] value);
        private delegate void glClearBufferuiv (uint buffer, int drawbuffer, uint[] value);
        private delegate void glClearBufferfv (uint buffer, int drawbuffer, float[] value);
        private delegate void glClearBufferfi (uint buffer, int drawbuffer, float depth, int stencil);
        private delegate IntPtr glGetStringi (uint name, uint index);

        //  Constants
        public const uint GL_COMPARE_REF_TO_TEXTURE                        = 0x884E;
        public const uint GL_CLIP_DISTANCE0                                = 0x3000;
        public const uint GL_CLIP_DISTANCE1                                = 0x3001;
        public const uint GL_CLIP_DISTANCE2                                = 0x3002;
        public const uint GL_CLIP_DISTANCE3                                = 0x3003;
        public const uint GL_CLIP_DISTANCE4                                = 0x3004;
        public const uint GL_CLIP_DISTANCE5                                = 0x3005;
        public const uint GL_CLIP_DISTANCE6                                = 0x3006;
        public const uint GL_CLIP_DISTANCE7                                = 0x3007;
        public const uint GL_MAX_CLIP_DISTANCES                            = 0x0D32;
        public const uint GL_MAJOR_VERSION                                 = 0x821B;
        public const uint GL_MINOR_VERSION                                 = 0x821C;
        public const uint GL_NUM_EXTENSIONS                                = 0x821D;
        public const uint GL_CONTEXT_FLAGS                                 = 0x821E;
        public const uint GL_DEPTH_BUFFER                                  = 0x8223;
        public const uint GL_STENCIL_BUFFER                                = 0x8224;
        public const uint GL_COMPRESSED_RED                                = 0x8225;
        public const uint GL_COMPRESSED_RG                                 = 0x8226;
        public const uint GL_CONTEXT_FLAG_FORWARD_COMPATIBLE_BIT           = 0x0001;
        public const uint GL_RGBA32F                                       = 0x8814;
        public const uint GL_RGB32F                                        = 0x8815;
        public const uint GL_RGBA16F                                       = 0x881A;
        public const uint GL_RGB16F                                        = 0x881B;
        public const uint GL_VERTEX_ATTRIB_ARRAY_INTEGER                   = 0x88FD;
        public const uint GL_MAX_ARRAY_TEXTURE_LAYERS                      = 0x88FF;
        public const uint GL_MIN_PROGRAM_TEXEL_OFFSET                      = 0x8904;
        public const uint GL_MAX_PROGRAM_TEXEL_OFFSET                      = 0x8905;
        public const uint GL_CLAMP_READ_COLOR                              = 0x891C;
        public const uint GL_FIXED_ONLY                                    = 0x891D;
        public const uint GL_MAX_VARYING_COMPONENTS                        = 0x8B4B;
        public const uint GL_TEXTURE_1D_ARRAY                              = 0x8C18;
        public const uint GL_PROXY_TEXTURE_1D_ARRAY                        = 0x8C19;
        public const uint GL_TEXTURE_2D_ARRAY                              = 0x8C1A;
        public const uint GL_PROXY_TEXTURE_2D_ARRAY                        = 0x8C1B;
        public const uint GL_TEXTURE_BINDING_1D_ARRAY                      = 0x8C1C;
        public const uint GL_TEXTURE_BINDING_2D_ARRAY                      = 0x8C1D;
        public const uint GL_R11F_G11F_B10F                                = 0x8C3A;
        public const uint GL_UNSIGNED_INT_10F_11F_11F_REV                  = 0x8C3B;
        public const uint GL_RGB9_E5                                       = 0x8C3D;
        public const uint GL_UNSIGNED_INT_5_9_9_9_REV                      = 0x8C3E;
        public const uint GL_TEXTURE_SHARED_SIZE                           = 0x8C3F;
        public const uint GL_TRANSFORM_FEEDBACK_VARYING_MAX_LENGTH         = 0x8C76;
        public const uint GL_TRANSFORM_FEEDBACK_BUFFER_MODE                = 0x8C7F;
        public const uint GL_MAX_TRANSFORM_FEEDBACK_SEPARATE_COMPONENTS    = 0x8C80;
        public const uint GL_TRANSFORM_FEEDBACK_VARYINGS                   = 0x8C83;
        public const uint GL_TRANSFORM_FEEDBACK_BUFFER_START               = 0x8C84;
        public const uint GL_TRANSFORM_FEEDBACK_BUFFER_SIZE                = 0x8C85;
        public const uint GL_PRIMITIVES_GENERATED                          = 0x8C87;
        public const uint GL_TRANSFORM_FEEDBACK_PRIMITIVES_WRITTEN         = 0x8C88;
        public const uint GL_RASTERIZER_DISCARD                            = 0x8C89;
        public const uint GL_MAX_TRANSFORM_FEEDBACK_INTERLEAVED_COMPONENTS = 0x8C8A;
        public const uint GL_MAX_TRANSFORM_FEEDBACK_SEPARATE_ATTRIBS       = 0x8C8B;
        public const uint GL_INTERLEAVED_ATTRIBS                           = 0x8C8C;
        public const uint GL_SEPARATE_ATTRIBS                              = 0x8C8D;
        public const uint GL_TRANSFORM_FEEDBACK_BUFFER                     = 0x8C8E;
        public const uint GL_TRANSFORM_FEEDBACK_BUFFER_BINDING             = 0x8C8F;
        public const uint GL_RGBA32UI                                      = 0x8D70;
        public const uint GL_RGB32UI                                       = 0x8D71;
        public const uint GL_RGBA16UI                                      = 0x8D76;
        public const uint GL_RGB16UI                                       = 0x8D77;
        public const uint GL_RGBA8UI                                       = 0x8D7C;
        public const uint GL_RGB8UI                                        = 0x8D7D;
        public const uint GL_RGBA32I                                       = 0x8D82;
        public const uint GL_RGB32I                                        = 0x8D83;
        public const uint GL_RGBA16I                                       = 0x8D88;
        public const uint GL_RGB16I                                        = 0x8D89;
        public const uint GL_RGBA8I                                        = 0x8D8E;
        public const uint GL_RGB8I                                         = 0x8D8F;
        public const uint GL_RED_INTEGER                                   = 0x8D94;
        public const uint GL_GREEN_INTEGER                                 = 0x8D95;
        public const uint GL_BLUE_INTEGER                                  = 0x8D96;
        public const uint GL_RGB_INTEGER                                   = 0x8D98;
        public const uint GL_RGBA_INTEGER                                  = 0x8D99;
        public const uint GL_BGR_INTEGER                                   = 0x8D9A;
        public const uint GL_BGRA_INTEGER                                  = 0x8D9B;
        public const uint GL_SAMPLER_1D_ARRAY                              = 0x8DC0;
        public const uint GL_SAMPLER_2D_ARRAY                              = 0x8DC1;
        public const uint GL_SAMPLER_1D_ARRAY_SHADOW                       = 0x8DC3;
        public const uint GL_SAMPLER_2D_ARRAY_SHADOW                       = 0x8DC4;
        public const uint GL_SAMPLER_CUBE_SHADOW                           = 0x8DC5;
        public const uint GL_UNSIGNED_INT_VEC2                             = 0x8DC6;
        public const uint GL_UNSIGNED_INT_VEC3                             = 0x8DC7;
        public const uint GL_UNSIGNED_INT_VEC4                             = 0x8DC8;
        public const uint GL_INT_SAMPLER_1D                                = 0x8DC9;
        public const uint GL_INT_SAMPLER_2D                                = 0x8DCA;
        public const uint GL_INT_SAMPLER_3D                                = 0x8DCB;
        public const uint GL_INT_SAMPLER_CUBE                              = 0x8DCC;
        public const uint GL_INT_SAMPLER_1D_ARRAY                          = 0x8DCE;
        public const uint GL_INT_SAMPLER_2D_ARRAY                          = 0x8DCF;
        public const uint GL_UNSIGNED_INT_SAMPLER_1D                       = 0x8DD1;
        public const uint GL_UNSIGNED_INT_SAMPLER_2D                       = 0x8DD2;
        public const uint GL_UNSIGNED_INT_SAMPLER_3D                       = 0x8DD3;
        public const uint GL_UNSIGNED_INT_SAMPLER_CUBE                     = 0x8DD4;
        public const uint GL_UNSIGNED_INT_SAMPLER_1D_ARRAY                 = 0x8DD6;
        public const uint GL_UNSIGNED_INT_SAMPLER_2D_ARRAY                 = 0x8DD7;
        public const uint GL_QUERY_WAIT                                    = 0x8E13;
        public const uint GL_QUERY_NO_WAIT                                 = 0x8E14;
        public const uint GL_QUERY_BY_REGION_WAIT                          = 0x8E15;
        public const uint GL_QUERY_BY_REGION_NO_WAIT                       = 0x8E16;
        public const uint GL_BUFFER_ACCESS_FLAGS                           = 0x911F;
        public const uint GL_BUFFER_MAP_LENGTH                             = 0x9120;
        public const uint GL_BUFFER_MAP_OFFSET                             = 0x9121;
        public const uint GL_R8 = 0x8229;
        public const uint GL_R16 = 0x822A;
        public const uint GL_RG8 = 0x822B;
        public const uint GL_RG16 = 0x822C;
        public const uint GL_R16F = 0x822D;
        public const uint GL_R32F = 0x822E;
        public const uint GL_RG16F = 0x822F;
        public const uint GL_RG32F = 0x8230;
        public const uint GL_R8I = 0x8231;
        public const uint GL_R8UI = 0x8232;
        public const uint GL_R16I = 0x8233;
        public const uint GL_R16UI = 0x8234;
        public const uint GL_R32I = 0x8235;
        public const uint GL_R32UI = 0x8236;
        public const uint GL_RG8I = 0x8237;
        public const uint GL_RG8UI = 0x8238;
        public const uint GL_RG16I = 0x8239;
        public const uint GL_RG16UI = 0x823A;
        public const uint GL_RG32I = 0x823B;
        public const uint GL_RG32UI = 0x823C;
        public const uint GL_RG = 0x8227;
        public const uint GL_RG_INTEGER = 0x8228;
     
        #endregion

        #region OpenGL 3.1

        //  Methods
        public void DrawArraysInstanced(uint mode, int first, int count, int primcount)
        {
            GetDelegateFor<glDrawArraysInstanced>()(mode, first, count, primcount);
        }
        public void DrawElementsInstanced(uint mode, int count, uint type, IntPtr indices, int primcount)
        {
            GetDelegateFor<glDrawElementsInstanced>()(mode, count, type, indices, primcount);
        }
        public void TexBuffer(uint target, uint internalformat, uint buffer)
        {
            GetDelegateFor<glTexBuffer>()(target, internalformat, buffer);
        }
        public void PrimitiveRestartIndex(uint index)
        {
            GetDelegateFor<glPrimitiveRestartIndex>()(index);
        }

        //  Delegates
        private delegate void glDrawArraysInstanced (uint mode, int first, int count, int primcount);
        private delegate void glDrawElementsInstanced (uint mode, int count, uint type, IntPtr indices, int primcount);
        private delegate void glTexBuffer (uint target, uint internalformat, uint buffer);
        private delegate void glPrimitiveRestartIndex (uint index);

        //  Constants
        public const uint GL_SAMPLER_2D_RECT                       = 0x8B63;
        public const uint GL_SAMPLER_2D_RECT_SHADOW                = 0x8B64;
        public const uint GL_SAMPLER_BUFFER                        = 0x8DC2;
        public const uint GL_INT_SAMPLER_2D_RECT                   = 0x8DCD;
        public const uint GL_INT_SAMPLER_BUFFER                    = 0x8DD0;
        public const uint GL_UNSIGNED_INT_SAMPLER_2D_RECT          = 0x8DD5;
        public const uint GL_UNSIGNED_INT_SAMPLER_BUFFER           = 0x8DD8;
        public const uint GL_TEXTURE_BUFFER                        = 0x8C2A;
        public const uint GL_MAX_TEXTURE_BUFFER_SIZE               = 0x8C2B;
        public const uint GL_TEXTURE_BINDING_BUFFER                = 0x8C2C;
        public const uint GL_TEXTURE_BUFFER_DATA_STORE_BINDING     = 0x8C2D;
        public const uint GL_TEXTURE_BUFFER_FORMAT                 = 0x8C2E;
        public const uint GL_TEXTURE_RECTANGLE                     = 0x84F5;
        public const uint GL_TEXTURE_BINDING_RECTANGLE             = 0x84F6;
        public const uint GL_PROXY_TEXTURE_RECTANGLE               = 0x84F7;
        public const uint GL_MAX_RECTANGLE_TEXTURE_SIZE            = 0x84F8;
        public const uint GL_RED_SNORM                             = 0x8F90;
        public const uint GL_RG_SNORM                              = 0x8F91;
        public const uint GL_RGB_SNORM                             = 0x8F92;
        public const uint GL_RGBA_SNORM                            = 0x8F93;
        public const uint GL_R8_SNORM                              = 0x8F94;
        public const uint GL_RG8_SNORM                             = 0x8F95;
        public const uint GL_RGB8_SNORM                            = 0x8F96;
        public const uint GL_RGBA8_SNORM                           = 0x8F97;
        public const uint GL_R16_SNORM                             = 0x8F98;
        public const uint GL_RG16_SNORM                            = 0x8F99;
        public const uint GL_RGB16_SNORM                           = 0x8F9A;
        public const uint GL_RGBA16_SNORM                          = 0x8F9B;
        public const uint GL_SIGNED_NORMALIZED                     = 0x8F9C;
        public const uint GL_PRIMITIVE_RESTART                     = 0x8F9D;
        public const uint GL_PRIMITIVE_RESTART_INDEX               = 0x8F9E;
        
        #endregion

        #region OpenGL 3.2

        //  Methods
        public void GetInteger64(uint target, uint index, Int64[] data)
        {
            GetDelegateFor<glGetInteger64i_v>()(target, index, data);
        }
        public void GetBufferParameteri64(uint target, uint pname, Int64[] parameters)
        {
            GetDelegateFor<glGetBufferParameteri64v>()(target, pname, parameters);
        }
        public void FramebufferTexture(uint target, uint attachment, uint texture, int level)
        {
            GetDelegateFor<glFramebufferTexture>()(target, attachment, texture, level);
        }

        /// <summary>
        /// Render primitives from array data with a per-element offset.
        /// </summary>
        /// <param name="mode">Specifies what kind of primitives to render. Symbolic constants OpenGL.GL_POINTS, OpenGL.GL_LINE_STRIP, OpenGL.GL_LINE_LOOP, OpenGL.GL_LINES, OpenGL.GL_TRIANGLE_STRIP, OpenGL.GL_TRIANGLE_FAN, OpenGL.GL_TRIANGLES, OpenGL.GL_LINES_ADJACENCY, OpenGL.GL_LINE_STRIP_ADJACENCY, OpenGL.GL_TRIANGLES_ADJACENCY, OpenGL.GL_TRIANGLE_STRIP_ADJACENCY and OpenGL.GL_PATCHES are accepted.</param>
        /// <param name="count">Specifies the number of elements to be rendered.</param>
        /// <param name="type">Specifies the type of the values in indices. Must be one of GL_UNSIGNED_BYTE, GL_UNSIGNED_SHORT, or GL_UNSIGNED_INT.</param>
        /// <param name="indices">Specifies a pointer to the location where the indices are stored.</param>
        /// <param name="basevertex">Specifies a constant that should be added to each element of indices when chosing elements from the enabled vertex arrays.</param>
        public void DrawElementsBaseVertex(uint mode, int count, uint type, IntPtr indices, int basevertex)
        {
            GetDelegateFor<glDrawElementsBaseVertex>()(mode, count, type, indices, basevertex);
        }

        /// <summary>
        /// Render primitives from array data with a per-element offset. Uses OpenGL.GL_UNSIGNED_INT as the data type.
        /// </summary>
        /// <param name="mode">Specifies what kind of primitives to render. Symbolic constants OpenGL.GL_POINTS, OpenGL.GL_LINE_STRIP, OpenGL.GL_LINE_LOOP, OpenGL.GL_LINES, OpenGL.GL_TRIANGLE_STRIP, OpenGL.GL_TRIANGLE_FAN, OpenGL.GL_TRIANGLES, OpenGL.GL_LINES_ADJACENCY, OpenGL.GL_LINE_STRIP_ADJACENCY, OpenGL.GL_TRIANGLES_ADJACENCY, OpenGL.GL_TRIANGLE_STRIP_ADJACENCY and OpenGL.GL_PATCHES are accepted.</param>
        /// <param name="count">Specifies the number of elements to be rendered.</param>
        /// <param name="indices">Specifies a pointer to the location where the indices are stored.</param>
        /// <param name="basevertex">Specifies a constant that should be added to each element of indices when chosing elements from the enabled vertex arrays.</param>
        public void DrawElementsBaseVertex(uint mode, int count, uint[] indices, int basevertex)
        {
            //  This helper version of the the function handles the pinning of the memory for the caller.
            var pinnedIndices = GCHandle.Alloc(indices, GCHandleType.Pinned);
            var pointer = pinnedIndices.AddrOfPinnedObject();
            GetDelegateFor<glDrawElementsBaseVertex>()(mode, count, OpenGL.GL_UNSIGNED_INT, pointer, basevertex);
            pinnedIndices.Free();
        }

        //  Delegates
        private delegate void glGetInteger64i_v (uint target, uint index, Int64[] data);
        private delegate void glGetBufferParameteri64v (uint target, uint pname, Int64[] parameters);
        private delegate void glFramebufferTexture (uint target, uint attachment, uint texture, int level);
        private delegate void glDrawElementsBaseVertex(uint mode, int count, uint type, IntPtr indices, int basevertex);

        //  Constants
        public const uint GL_CONTEXT_CORE_PROFILE_BIT                  = 0x00000001;
        public const uint GL_CONTEXT_COMPATIBILITY_PROFILE_BIT         = 0x00000002;
        public const uint GL_LINES_ADJACENCY                           = 0x000A;
        public const uint GL_LINE_STRIP_ADJACENCY                      = 0x000B;
        public const uint GL_TRIANGLES_ADJACENCY                       = 0x000C;
        public const uint GL_TRIANGLE_STRIP_ADJACENCY                  = 0x000D;
        public const uint GL_PATCHES                                   = 0x000E;
        public const uint GL_PROGRAM_POINT_SIZE                        = 0x8642;
        public const uint GL_MAX_GEOMETRY_TEXTURE_IMAGE_UNITS          = 0x8C29;
        public const uint GL_FRAMEBUFFER_ATTACHMENT_LAYERED            = 0x8DA7;
        public const uint GL_FRAMEBUFFER_INCOMPLETE_LAYER_TARGETS      = 0x8DA8;
        public const uint GL_GEOMETRY_SHADER                           = 0x8DD9;
        public const uint GL_GEOMETRY_VERTICES_OUT                     = 0x8916;
        public const uint GL_GEOMETRY_INPUT_TYPE                       = 0x8917;
        public const uint GL_GEOMETRY_OUTPUT_TYPE                      = 0x8918;
        public const uint GL_MAX_GEOMETRY_UNIFORM_COMPONENTS           = 0x8DDF;
        public const uint GL_MAX_GEOMETRY_OUTPUT_VERTICES              = 0x8DE0;
        public const uint GL_MAX_GEOMETRY_TOTAL_OUTPUT_COMPONENTS      = 0x8DE1;
        public const uint GL_MAX_VERTEX_OUTPUT_COMPONENTS              = 0x9122;
        public const uint GL_MAX_GEOMETRY_INPUT_COMPONENTS             = 0x9123;
        public const uint GL_MAX_GEOMETRY_OUTPUT_COMPONENTS            = 0x9124;
        public const uint GL_MAX_FRAGMENT_INPUT_COMPONENTS             = 0x9125;
        public const uint GL_CONTEXT_PROFILE_MASK                      = 0x9126;

        #endregion

        #region OpenGL 3.3

        //  Methods
        public void VertexAttribDivisor(uint index, uint divisor)
        {
            GetDelegateFor<glVertexAttribDivisor>()(index, divisor);
        }
        
        //  Delegates
        private delegate void glVertexAttribDivisor (uint index, uint divisor);

        //  Constants
        public const uint GL_VERTEX_ATTRIB_ARRAY_DIVISOR             = 0x88FE;
        
        #endregion

        #region OpenGL 4.0

        //  Methods        
        public void MinSampleShading(float value)
        {
            GetDelegateFor<glMinSampleShading>()(value);
        }
        public void BlendEquation(uint buf, uint mode)
        {
            GetDelegateFor<glBlendEquationi>()(buf, mode);
        }
        public void BlendEquationSeparate(uint buf, uint modeRGB, uint modeAlpha)
        {
            GetDelegateFor<glBlendEquationSeparatei>()(buf, modeRGB, modeAlpha);
        }
        public void BlendFunc(uint buf, uint src, uint dst)
        {
            GetDelegateFor<glBlendFunci>()(buf, src, dst);
        }
        public void BlendFuncSeparate(uint buf, uint srcRGB, uint dstRGB, uint srcAlpha, uint dstAlpha)
        {
            GetDelegateFor<glBlendFuncSeparatei>()(buf, srcRGB, dstRGB, srcAlpha, dstAlpha);
        }

        //  Delegates        
        private delegate void glMinSampleShading (float value);
        private delegate void glBlendEquationi (uint buf, uint mode);
        private delegate void glBlendEquationSeparatei (uint buf, uint modeRGB, uint modeAlpha);
        private delegate void glBlendFunci (uint buf, uint src, uint dst);
        private delegate void glBlendFuncSeparatei (uint buf, uint srcRGB, uint dstRGB, uint srcAlpha, uint dstAlpha);

        //  Constants
        public const uint GL_SAMPLE_SHADING                        = 0x8C36;
        public const uint GL_MIN_SAMPLE_SHADING_VALUE              = 0x8C37;
        public const uint GL_MIN_PROGRAM_TEXTURE_GATHER_OFFSET     = 0x8E5E;
        public const uint GL_MAX_PROGRAM_TEXTURE_GATHER_OFFSET     = 0x8E5F;
        public const uint GL_TEXTURE_CUBE_MAP_ARRAY                = 0x9009;
        public const uint GL_TEXTURE_BINDING_CUBE_MAP_ARRAY        = 0x900A;
        public const uint GL_PROXY_TEXTURE_CUBE_MAP_ARRAY          = 0x900B;
        public const uint GL_SAMPLER_CUBE_MAP_ARRAY                = 0x900C;
        public const uint GL_SAMPLER_CUBE_MAP_ARRAY_SHADOW         = 0x900D;
        public const uint GL_INT_SAMPLER_CUBE_MAP_ARRAY            = 0x900E;
        public const uint GL_UNSIGNED_INT_SAMPLER_CUBE_MAP_ARRAY   = 0x900F;

        #endregion

        #region OpenGL 4.0 Additions (ftlPhysicsGuy)
        //  Methods        
        public void PatchParameter(uint pname, uint value)
        {
            GetDelegateFor<glPatchParameteri>()(pname, value);
        }

        private delegate void glPatchParameteri(uint pname, uint value);

        // Constants
        public const uint GL_TESS_EVALUATION_SHADER = 0x8e87;
        public const uint GL_TESS_CONTROL_SHADER = 0x8e88;
        public const uint GL_PATCH_VERTICES = 0x8E72; //36466;
        public const uint GL_PATCH_DEFAULT_INNER_LEVEL = 0x8E73;
        public const uint GL_PATCH_DEFAULT_OUTER_LEVEL = 0x8E74;
        #endregion

        #region GL_EXT_texture3D

        /// <summary>
        /// Specify a three-dimensional texture subimage.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="level">The level.</param>
        /// <param name="internalformat">The internalformat.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="depth">The depth.</param>
        /// <param name="border">The border.</param>
        /// <param name="format">The format.</param>
        /// <param name="type">The type.</param>
        /// <param name="pixels">The pixels.</param>
        public void TexImage3DEXT (uint target, int level, uint internalformat, uint width, 
            uint height, uint depth, int border, uint format, uint type, IntPtr pixels)
        {
            GetDelegateFor<glTexImage3DEXT>()(target, level, internalformat, width, height, depth, border, format, type, pixels);
        }

        /// <summary>
        /// Texes the sub image3 DEXT.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="level">The level.</param>
        /// <param name="xoffset">The xoffset.</param>
        /// <param name="yoffset">The yoffset.</param>
        /// <param name="zoffset">The zoffset.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="depth">The depth.</param>
        /// <param name="format">The format.</param>
        /// <param name="type">The type.</param>
        /// <param name="pixels">The pixels.</param>
        public void TexSubImage3DEXT(uint target, int level, int xoffset, int yoffset, int zoffset,
            uint width, uint height, uint depth, uint format, uint type, IntPtr pixels)
        {
            GetDelegateFor<glTexSubImage3DEXT>()(target, level, xoffset, yoffset, zoffset, width, height, depth, format, type, pixels);
        }

        private delegate void glTexImage3DEXT(uint target, int level, uint internalformat, uint width,
            uint height, uint depth, int border, uint format, uint type, IntPtr pixels);
        private delegate void glTexSubImage3DEXT(uint target, int level, int xoffset, int yoffset, int zoffset,
            uint width, uint height, uint depth, uint format, uint type, IntPtr pixels);

        #endregion

        #region GL_EXT_bgra

        public const uint GL_BGR_EXT = 0x80E0;
        public const uint GL_BGRA_EXT = 0x80E1;

        #endregion

        #region GL_EXT_packed_pixels

        public const uint GL_UNSIGNED_BYTE_3_3_2_EXT = 0x8032;
        public const uint GL_UNSIGNED_SHORT_4_4_4_4_EXT = 0x8033;
        public const uint GL_UNSIGNED_SHORT_5_5_5_1_EXT = 0x8034;
        public const uint GL_UNSIGNED_INT_8_8_8_8_EXT = 0x8035;
        public const uint GL_UNSIGNED_INT_10_10_10_2_EXT = 0x8036;

        #endregion

        #region GL_EXT_rescale_normal

        public const uint GL_RESCALE_NORMAL_EXT = 0x803A;

        #endregion

        #region GL_EXT_separate_specular_color

        public const uint GL_LIGHT_MODEL_COLOR_CONTROL_EXT = 0x81F8;
        public const uint GL_SINGLE_COLOR_EXT = 0x81F9;
        public const uint GL_SEPARATE_SPECULAR_COLOR_EXT = 0x81FA;

        #endregion

        #region GL_SGIS_texture_edge_clamp

        public const uint GL_CLAMP_TO_EDGE_SGIS = 0x812F;

        #endregion

        #region GL_SGIS_texture_lod

        public const uint GL_TEXTURE_MIN_LOD_SGIS = 0x813A;
        public const uint GL_TEXTURE_MAX_LOD_SGIS = 0x813B;
        public const uint GL_TEXTURE_BASE_LEVEL_SGIS = 0x813C;
        public const uint GL_TEXTURE_MAX_LEVEL_SGIS = 0x813D;

        #endregion

        #region GL_EXT_draw_range_elements

        /// <summary>
        /// Render primitives from array data.
        /// </summary>
        /// <param name="mode">The mode.</param>
        /// <param name="start">The start.</param>
        /// <param name="end">The end.</param>
        /// <param name="count">The count.</param>
        /// <param name="type">The type.</param>
        /// <param name="indices">The indices.</param>
        public void DrawRangeElementsEXT(uint mode, uint start, uint end, uint count, uint type, IntPtr indices)
        {
            GetDelegateFor<glDrawRangeElementsEXT>()(mode, start, end, count, type, indices);
        }

        private delegate void glDrawRangeElementsEXT(uint mode, uint start, uint end, uint count, uint type, IntPtr indices);

        public const uint GL_MAX_ELEMENTS_VERTICES_EXT = 0x80E8;
        public const uint GL_MAX_ELEMENTS_INDICES_EXT = 0x80E9;

        #endregion

        #region GL_SGI_color_table

        //  Delegates
        public void ColorTableSGI(uint target, uint internalformat, uint width, uint format, uint type, IntPtr table)
        {
            GetDelegateFor<glColorTableSGI>()(target, internalformat, width, format, type, table);
        }

        public void ColorTableParameterSGI(uint target, uint pname, float[] parameters)
        {
            GetDelegateFor<glColorTableParameterfvSGI>()(target, pname, parameters);
        }

        public void ColorTableParameterSGI(uint target, uint pname, int[] parameters)
        {
            GetDelegateFor<glColorTableParameterivSGI>()(target, pname, parameters);
        }

        public void CopyColorTableSGI(uint target, uint internalformat, int x, int y, uint width)
        {
            GetDelegateFor<glCopyColorTableSGI>()(target, internalformat, x, y, width);
        }

        public void GetColorTableSGI(uint target, uint format, uint type, IntPtr table)
        {
            GetDelegateFor<glGetColorTableSGI>()(target, format, type, table);
        }

        public void GetColorTableParameterSGI(uint target, uint pname, float[] parameters)
        {
            GetDelegateFor<glGetColorTableParameterfvSGI>()(target, pname, parameters);
        }

        public void GetColorTableParameterSGI(uint target, uint pname, int[] parameters)
        {
            GetDelegateFor<glGetColorTableParameterivSGI>()(target, pname, parameters);
        }

        //  Delegates
        private delegate void glColorTableSGI(uint target, uint internalformat, uint width, uint format, uint type, IntPtr table);
        private delegate void glColorTableParameterfvSGI(uint target, uint pname, float[] parameters);
        private delegate void glColorTableParameterivSGI(uint target, uint pname, int[] parameters);
        private delegate void glCopyColorTableSGI(uint target, uint internalformat, int x, int y, uint width);
        private delegate void glGetColorTableSGI(uint target, uint format, uint type, IntPtr table);
        private delegate void glGetColorTableParameterfvSGI(uint target, uint pname, float[] parameters);
        private delegate void glGetColorTableParameterivSGI(uint target, uint pname, int[] parameters);

        //  Constants
        public const uint GL_COLOR_TABLE_SGI = 0x80D0;
        public const uint GL_POST_CONVOLUTION_COLOR_TABLE_SGI = 0x80D1;
        public const uint GL_POST_COLOR_MATRIX_COLOR_TABLE_SGI = 0x80D2;
        public const uint GL_PROXY_COLOR_TABLE_SGI = 0x80D3;
        public const uint GL_PROXY_POST_CONVOLUTION_COLOR_TABLE_SGI = 0x80D4;
        public const uint GL_PROXY_POST_COLOR_MATRIX_COLOR_TABLE_SGI = 0x80D5;
        public const uint GL_COLOR_TABLE_SCALE_SGI = 0x80D6;
        public const uint GL_COLOR_TABLE_BIAS_SGI = 0x80D7;
        public const uint GL_COLOR_TABLE_FORMAT_SGI = 0x80D8;
        public const uint GL_COLOR_TABLE_WIDTH_SGI = 0x80D9;
        public const uint GL_COLOR_TABLE_RED_SIZE_SGI = 0x80DA;
        public const uint GL_COLOR_TABLE_GREEN_SIZE_SGI = 0x80DB;
        public const uint GL_COLOR_TABLE_BLUE_SIZE_SGI = 0x80DC;
        public const uint GL_COLOR_TABLE_ALPHA_SIZE_SGI = 0x80DD;
        public const uint GL_COLOR_TABLE_LUMINANCE_SIZE_SGI = 0x80DE;
        public const uint GL_COLOR_TABLE_INTENSITY_SIZE_SGI = 0x80DF;

        #endregion

        #region GL_EXT_convolution

        //  Methods.
        public void ConvolutionFilter1DEXT(uint target, uint internalformat, int width, uint format, uint type, IntPtr image)
        {
            GetDelegateFor<glConvolutionFilter1DEXT>()(target, internalformat, width, format, type, image);
        }

        public void ConvolutionFilter2DEXT(uint target, uint internalformat, int width, int height, uint format, uint type, IntPtr image)
        {
            GetDelegateFor<glConvolutionFilter2DEXT>()(target, internalformat, width, height, format, type, image);
        }

        public void ConvolutionParameterEXT(uint target, uint pname, float parameters)
        {
            GetDelegateFor<glConvolutionParameterfEXT>()(target, pname, parameters);
        }

        public void ConvolutionParameterEXT(uint target, uint pname, float[] parameters)
        {
            GetDelegateFor<glConvolutionParameterfvEXT>()(target, pname, parameters);
        }

        public void ConvolutionParameterEXT(uint target, uint pname, int parameter)
        {
            GetDelegateFor<glConvolutionParameteriEXT>()(target, pname, parameter);
        }

        public void ConvolutionParameterEXT(uint target, uint pname, int[] parameters)
        {
            GetDelegateFor<glConvolutionParameterivEXT>()(target, pname, parameters);
        }

        public void CopyConvolutionFilter1DEXT(uint target, uint internalformat, int x, int y, int width)
        {
            GetDelegateFor<glCopyConvolutionFilter1DEXT>()(target, internalformat, x, y, width);
        }

        public void CopyConvolutionFilter2DEXT(uint target, uint internalformat, int x, int y, int width, int height)
        {
            GetDelegateFor<glCopyConvolutionFilter2DEXT>()(target, internalformat, x, y, width, height);
        }

        public void GetConvolutionFilterEXT(uint target, uint format, uint type, IntPtr image)
        {
            GetDelegateFor<glGetConvolutionFilterEXT>()(target, format, type, image);
        }

        public void GetConvolutionParameterfvEXT(uint target, uint pname, float[] parameters)
        {
            GetDelegateFor<glGetConvolutionParameterfvEXT>()(target, pname, parameters);
        }

        public void GetConvolutionParameterivEXT(uint target, uint pname, int[] parameters)
        {
            GetDelegateFor<glGetConvolutionParameterivEXT>()(target, pname, parameters);
        }

        public void GetSeparableFilterEXT(uint target, uint format, uint type, IntPtr row, IntPtr column, IntPtr span)
        {
            GetDelegateFor<glGetSeparableFilterEXT>()(target, format, type, row, column, span);
        }

        public void SeparableFilter2DEXT(uint target, uint internalformat, int width, int height, uint format, uint type, IntPtr row, IntPtr column)
        {
            GetDelegateFor<glSeparableFilter2DEXT>()(target, internalformat, width, height, format, type, row, column);
        }

        //  Delegates
        private delegate void glConvolutionFilter1DEXT(uint target, uint internalformat, int width, uint format, uint type, IntPtr image);
        private delegate void glConvolutionFilter2DEXT(uint target, uint internalformat, int width, int height, uint format, uint type, IntPtr image);
        private delegate void glConvolutionParameterfEXT(uint target, uint pname, float parameters);
        private delegate void glConvolutionParameterfvEXT(uint target, uint pname, float[] parameters);
        private delegate void glConvolutionParameteriEXT(uint target, uint pname, int parameter);
        private delegate void glConvolutionParameterivEXT(uint target, uint pname, int[] parameters);
        private delegate void glCopyConvolutionFilter1DEXT(uint target, uint internalformat, int x, int y, int width);
        private delegate void glCopyConvolutionFilter2DEXT(uint target, uint internalformat, int x, int y, int width, int height);
        private delegate void glGetConvolutionFilterEXT(uint target, uint format, uint type, IntPtr image);
        private delegate void glGetConvolutionParameterfvEXT(uint target, uint pname, float[] parameters);
        private delegate void glGetConvolutionParameterivEXT(uint target, uint pname, int[] parameters);
        private delegate void glGetSeparableFilterEXT(uint target, uint format, uint type, IntPtr row, IntPtr column, IntPtr span);
        private delegate void glSeparableFilter2DEXT(uint target, uint internalformat, int width, int height, uint format, uint type, IntPtr row, IntPtr column);

        //  Constants        
        public static uint GL_CONVOLUTION_1D_EXT = 0x8010;
        public static uint GL_CONVOLUTION_2D_EXT = 0x8011;
        public static uint GL_SEPARABLE_2D_EXT = 0x8012;
        public static uint GL_CONVOLUTION_BORDER_MODE_EXT = 0x8013;
        public static uint GL_CONVOLUTION_FILTER_SCALE_EXT = 0x8014;
        public static uint GL_CONVOLUTION_FILTER_BIAS_EXT = 0x8015;
        public static uint GL_REDUCE_EXT = 0x8016;
        public static uint GL_CONVOLUTION_FORMAT_EXT = 0x8017;
        public static uint GL_CONVOLUTION_WIDTH_EXT = 0x8018;
        public static uint GL_CONVOLUTION_HEIGHT_EXT = 0x8019;
        public static uint GL_MAX_CONVOLUTION_WIDTH_EXT = 0x801A;
        public static uint GL_MAX_CONVOLUTION_HEIGHT_EXT = 0x801B;
        public static uint GL_POST_CONVOLUTION_RED_SCALE_EXT = 0x801C;
        public static uint GL_POST_CONVOLUTION_GREEN_SCALE_EXT = 0x801D;
        public static uint GL_POST_CONVOLUTION_BLUE_SCALE_EXT = 0x801E;
        public static uint GL_POST_CONVOLUTION_ALPHA_SCALE_EXT = 0x801F;
        public static uint GL_POST_CONVOLUTION_RED_BIAS_EXT = 0x8020;
        public static uint GL_POST_CONVOLUTION_GREEN_BIAS_EXT = 0x8021;
        public static uint GL_POST_CONVOLUTION_BLUE_BIAS_EXT = 0x8022;
        public static uint GL_POST_CONVOLUTION_ALPHA_BIAS_EXT = 0x8023;

        #endregion

        #region GL_SGI_color_matrix

        public const uint GL_COLOR_MATRIX_SGI = 0x80B1;
        public const uint GL_COLOR_MATRIX_STACK_DEPTH_SGI = 0x80B2;
        public const uint GL_MAX_COLOR_MATRIX_STACK_DEPTH_SGI = 0x80B3;
        public const uint GL_POST_COLOR_MATRIX_RED_SCALE_SGI = 0x80B4;
        public const uint GL_POST_COLOR_MATRIX_GREEN_SCALE_SGI = 0x80B5;
        public const uint GL_POST_COLOR_MATRIX_BLUE_SCALE_SGI = 0x80B6;
        public const uint GL_POST_COLOR_MATRIX_ALPHA_SCALE_SGI = 0x80B7;
        public const uint GL_POST_COLOR_MATRIX_RED_BIAS_SGI = 0x80B8;
        public const uint GL_POST_COLOR_MATRIX_GREEN_BIAS_SGI = 0x80B9;
        public const uint GL_POST_COLOR_MATRIX_BLUE_BIAS_SGI = 0x80BA;
        public const uint GL_POST_COLOR_MATRIX_ALPHA_BIAS_SGI = 0x80BB;

        #endregion

        #region GL_EXT_histogram

        //  Methods
        public void GetHistogramEXT(uint target, bool reset, uint format, uint type, IntPtr values)
        {
            GetDelegateFor<glGetHistogramEXT>()(target, reset, format, type, values);
        }

        public void GetHistogramParameterEXT(uint target, uint pname, float[] parameters)
        {
            GetDelegateFor<glGetHistogramParameterfvEXT>()(target, pname, parameters);
        }

        public void GetHistogramParameterEXT(uint target, uint pname, int[] parameters)
        {
            GetDelegateFor<glGetHistogramParameterivEXT>()(target, pname, parameters);
        }

        public void GetMinmaxEXT(uint target, bool reset, uint format, uint type, IntPtr values)
        {
            GetDelegateFor<glGetMinmaxEXT>()(target, reset, format, type, values);
        }

        public void GetMinmaxParameterfvEXT(uint target, uint pname, float[] parameters)
        {
            GetDelegateFor<glGetMinmaxParameterfvEXT>()(target, pname, parameters);
        }

        public void GetMinmaxParameterivEXT(uint target, uint pname, int[] parameters)
        {
            GetDelegateFor<glGetMinmaxParameterivEXT>()(target, pname, parameters);
        }

        public void HistogramEXT(uint target, int width, uint internalformat, bool sink)
        {
            GetDelegateFor<glHistogramEXT>()(target, width, internalformat, sink);
        }

        public void MinmaxEXT(uint target, uint internalformat, bool sink)
        {
            GetDelegateFor<glMinmaxEXT>()(target, internalformat, sink);
        }

        public void ResetHistogramEXT(uint target)
        {
            GetDelegateFor<glResetHistogramEXT>()(target);
        }

        public void ResetMinmaxEXT(uint target)
        {
            GetDelegateFor<glResetMinmaxEXT>()(target);
        }

        //  Delegates
        private delegate void glGetHistogramEXT(uint target, bool reset, uint format, uint type, IntPtr values);
        private delegate void glGetHistogramParameterfvEXT(uint target, uint pname, float[] parameters);
        private delegate void glGetHistogramParameterivEXT(uint target, uint pname, int[] parameters);
        private delegate void glGetMinmaxEXT(uint target, bool reset, uint format, uint type, IntPtr values);
        private delegate void glGetMinmaxParameterfvEXT(uint target, uint pname, float[] parameters);
        private delegate void glGetMinmaxParameterivEXT(uint target, uint pname, int[] parameters);
        private delegate void glHistogramEXT(uint target, int width, uint internalformat, bool sink);
        private delegate void glMinmaxEXT(uint target, uint internalformat, bool sink);
        private delegate void glResetHistogramEXT(uint target);
        private delegate void glResetMinmaxEXT(uint target);

        //  Constants
        public const uint GL_HISTOGRAM_EXT = 0x8024;
        public const uint GL_PROXY_HISTOGRAM_EXT = 0x8025;
        public const uint GL_HISTOGRAM_WIDTH_EXT = 0x8026;
        public const uint GL_HISTOGRAM_FORMAT_EXT = 0x8027;
        public const uint GL_HISTOGRAM_RED_SIZE_EXT = 0x8028;
        public const uint GL_HISTOGRAM_GREEN_SIZE_EXT = 0x8029;
        public const uint GL_HISTOGRAM_BLUE_SIZE_EXT = 0x802A;
        public const uint GL_HISTOGRAM_ALPHA_SIZE_EXT = 0x802B;
        public const uint GL_HISTOGRAM_LUMINANCE_SIZE_EXT = 0x802C;
        public const uint GL_HISTOGRAM_SINK_EXT = 0x802D;
        public const uint GL_MINMAX_EXT = 0x802E;
        public const uint GL_MINMAX_FORMAT_EXT = 0x802F;
        public const uint GL_MINMAX_SINK_EXT = 0x8030;
        public const uint GL_TABLE_TOO_LARGE_EXT = 0x8031;

        #endregion

        #region GL_EXT_blend_color

        //  Methods
        public void BlendColorEXT(float red, float green, float blue, float alpha)
        {
            GetDelegateFor<glBlendColorEXT>()(red, green, blue, alpha);
        }

        //  Delegates
        private delegate void glBlendColorEXT(float red, float green, float blue, float alpha);

        //  Constants        
        public const uint GL_CONSTANT_COLOR_EXT = 0x8001;
        public const uint GL_ONE_MINUS_CONSTANT_COLOR_EXT = 0x8002;
        public const uint GL_CONSTANT_ALPHA_EXT = 0x8003;
        public const uint GL_ONE_MINUS_CONSTANT_ALPHA_EXT = 0x8004;
        public const uint GL_BLEND_COLOR_EXT = 0x8005;

        #endregion

        #region GL_EXT_blend_minmax

        //  Methods
        public void BlendEquationEXT(uint mode)
        {
            GetDelegateFor<glBlendEquationEXT>()(mode);
        }

        //  Delegates
        private delegate void glBlendEquationEXT(uint mode);

        //  Constants        
        public const uint GL_FUNC_ADD_EXT = 0x8006;
        public const uint GL_MIN_EXT = 0x8007;
        public const uint GL_MAX_EXT = 0x8008;
        public const uint GL_FUNC_SUBTRACT_EXT = 0x800A;
        public const uint GL_FUNC_REVERSE_SUBTRACT_EXT = 0x800B;
        public const uint GL_BLEND_EQUATION_EXT = 0x8009;

        #endregion

        #region GL_ARB_multitexture

        //  Methods
        [Obsolete("Deprecated from OpenGL version 3.0")]
        public void ActiveTextureARB(uint texture)
        {
            GetDelegateFor<glActiveTextureARB>()(texture);
        }
        [Obsolete("Deprecated from OpenGL version 3.0")]
        public void ClientActiveTextureARB(uint texture)
        {
            GetDelegateFor<glClientActiveTextureARB>()(texture);
        }
        [Obsolete("Deprecated from OpenGL version 3.0")]
        public void MultiTexCoord1ARB(uint target, double s)
        {
            GetDelegateFor<glMultiTexCoord1dARB>()(target, s);
        }
        [Obsolete("Deprecated from OpenGL version 3.0")]
        public void MultiTexCoord1ARB(uint target, double[] v)
        {
            GetDelegateFor<glMultiTexCoord1dvARB>()(target, v);
        }
        [Obsolete("Deprecated from OpenGL version 3.0")]
        public void MultiTexCoord1ARB(uint target, float s)
        {
            GetDelegateFor<glMultiTexCoord1fARB>()(target, s);
        }
        [Obsolete("Deprecated from OpenGL version 3.0")]
        public void MultiTexCoord1ARB(uint target, float[] v)
        {
            GetDelegateFor<glMultiTexCoord1fvARB>()(target, v);
        }
        [Obsolete("Deprecated from OpenGL version 3.0")]
        public void MultiTexCoord1ARB(uint target, int s)
        {
            GetDelegateFor<glMultiTexCoord1iARB>()(target, s);
        }
        [Obsolete("Deprecated from OpenGL version 3.0")]
        public void MultiTexCoord1ARB(uint target, int[] v)
        {
            GetDelegateFor<glMultiTexCoord1ivARB>()(target, v);
        }
        [Obsolete("Deprecated from OpenGL version 3.0")]
        public void MultiTexCoord1ARB(uint target, short s)
        {
            GetDelegateFor<glMultiTexCoord1sARB>()(target, s);
        }
        [Obsolete("Deprecated from OpenGL version 3.0")]
        public void MultiTexCoord1ARB(uint target, short[] v)
        {
            GetDelegateFor<glMultiTexCoord1svARB>()(target, v);
        }
        [Obsolete("Deprecated from OpenGL version 3.0")]
        public void MultiTexCoord2ARB(uint target, double s, double t)
        {
            GetDelegateFor<glMultiTexCoord2dARB>()(target, s, t);
        }
        [Obsolete("Deprecated from OpenGL version 3.0")]
        public void MultiTexCoord2ARB(uint target, double[] v)
        {
            GetDelegateFor<glMultiTexCoord2dvARB>()(target, v);
        }
        [Obsolete("Deprecated from OpenGL version 3.0")]
        public void MultiTexCoord2ARB(uint target, float s, float t)
        {
            GetDelegateFor<glMultiTexCoord2fARB>()(target, s, t);
        }
        [Obsolete("Deprecated from OpenGL version 3.0")]
        public void MultiTexCoord2ARB(uint target, float[] v)
        {
            GetDelegateFor<glMultiTexCoord2fvARB>()(target, v);
        }
        [Obsolete("Deprecated from OpenGL version 3.0")]
        public void MultiTexCoord2ARB(uint target, int s, int t)
        {
            GetDelegateFor<glMultiTexCoord2iARB>()(target, s, t);
        }
        [Obsolete("Deprecated from OpenGL version 3.0")]
        public void MultiTexCoord2ARB(uint target, int[] v)
        {
            GetDelegateFor<glMultiTexCoord2ivARB>()(target, v);
        }
        [Obsolete("Deprecated from OpenGL version 3.0")]
        public void MultiTexCoord2ARB(uint target, short s, short t)
        {
            GetDelegateFor<glMultiTexCoord2sARB>()(target, s, t);
        }
        [Obsolete("Deprecated from OpenGL version 3.0")]
        public void MultiTexCoord2ARB(uint target, short[] v)
        {
            GetDelegateFor<glMultiTexCoord2svARB>()(target, v);
        }
        [Obsolete("Deprecated from OpenGL version 3.0")]
        public void MultiTexCoord3ARB(uint target, double s, double t, double r)
        {
            GetDelegateFor<glMultiTexCoord3dARB>()(target, s, t, r);
        }
        [Obsolete("Deprecated from OpenGL version 3.0")]
        public void MultiTexCoord3ARB(uint target, double[] v)
        {
            GetDelegateFor<glMultiTexCoord3dvARB>()(target, v);
        }
        [Obsolete("Deprecated from OpenGL version 3.0")]
        public void MultiTexCoord3ARB(uint target, float s, float t, float r)
        {
            GetDelegateFor<glMultiTexCoord3fARB>()(target, s, t, r);
        }
        [Obsolete("Deprecated from OpenGL version 3.0")]
        public void MultiTexCoord3ARB(uint target, float[] v)
        {
            GetDelegateFor<glMultiTexCoord3fvARB>()(target, v);
        }
        [Obsolete("Deprecated from OpenGL version 3.0")]
        public void MultiTexCoord3ARB(uint target, int s, int t, int r)
        {
            GetDelegateFor<glMultiTexCoord3iARB>()(target, s, t, r);
        }
        [Obsolete("Deprecated from OpenGL version 3.0")]
        public void MultiTexCoord3ARB(uint target, int[] v)
        {
            GetDelegateFor<glMultiTexCoord3ivARB>()(target, v);
        }
        [Obsolete("Deprecated from OpenGL version 3.0")]
        public void MultiTexCoord3ARB(uint target, short s, short t, short r)
        {
            GetDelegateFor<glMultiTexCoord3sARB>()(target, s, t, r);
        }
        [Obsolete("Deprecated from OpenGL version 3.0")]
        public void MultiTexCoord3ARB(uint target, short[] v)
        {
            GetDelegateFor<glMultiTexCoord3svARB>()(target, v);
        }
        [Obsolete("Deprecated from OpenGL version 3.0")]
        public void MultiTexCoord4ARB(uint target, double s, double t, double r, double q)
        {
            GetDelegateFor<glMultiTexCoord4dARB>()(target, s, t, r, q);
        }
        [Obsolete("Deprecated from OpenGL version 3.0")]
        public void MultiTexCoord4ARB(uint target, double[] v)
        {
            GetDelegateFor<glMultiTexCoord4dvARB>()(target, v);
        }
        [Obsolete("Deprecated from OpenGL version 3.0")]
        public void MultiTexCoord4ARB(uint target, float s, float t, float r, float q)
        {
            GetDelegateFor<glMultiTexCoord4fARB>()(target, s, t, r, q);
        }
        [Obsolete("Deprecated from OpenGL version 3.0")]
        public void MultiTexCoord4ARB(uint target, float[] v)
        {
            GetDelegateFor<glMultiTexCoord4fvARB>()(target, v);
        }
        [Obsolete("Deprecated from OpenGL version 3.0")]
        public void MultiTexCoord4ARB(uint target, int s, int t, int r, int q)
        {
            GetDelegateFor<glMultiTexCoord4iARB>()(target, s, t, r, q);
        }
        public void MultiTexCoord4ARB(uint target, int[] v)
        {
            GetDelegateFor<glMultiTexCoord4ivARB>()(target, v);
        }
        [Obsolete("Deprecated from OpenGL version 3.0")]
        public void MultiTexCoord4ARB(uint target, short s, short t, short r, short q)
        {
            GetDelegateFor<glMultiTexCoord4sARB>()(target, s, t, r, q);
        }
        [Obsolete("Deprecated from OpenGL version 3.0")]
        public void MultiTexCoord4ARB(uint target, short[] v)
        {
            GetDelegateFor<glMultiTexCoord4svARB>()(target, v);
        }

        //  Delegates 
        private delegate void glActiveTextureARB(uint texture);
        private delegate void glClientActiveTextureARB(uint texture);
        private delegate void glMultiTexCoord1dARB(uint target, double s);
        private delegate void glMultiTexCoord1dvARB(uint target, double[] v);
        private delegate void glMultiTexCoord1fARB(uint target, float s);
        private delegate void glMultiTexCoord1fvARB(uint target, float[] v);
        private delegate void glMultiTexCoord1iARB(uint target, int s);
        private delegate void glMultiTexCoord1ivARB(uint target, int[] v);
        private delegate void glMultiTexCoord1sARB(uint target, short s);
        private delegate void glMultiTexCoord1svARB(uint target, short[] v);
        private delegate void glMultiTexCoord2dARB(uint target, double s, double t);
        private delegate void glMultiTexCoord2dvARB(uint target, double[] v);
        private delegate void glMultiTexCoord2fARB(uint target, float s, float t);
        private delegate void glMultiTexCoord2fvARB(uint target, float[] v);
        private delegate void glMultiTexCoord2iARB(uint target, int s, int t);
        private delegate void glMultiTexCoord2ivARB(uint target, int[] v);
        private delegate void glMultiTexCoord2sARB(uint target, short s, short t);
        private delegate void glMultiTexCoord2svARB(uint target, short[] v);
        private delegate void glMultiTexCoord3dARB(uint target, double s, double t, double r);
        private delegate void glMultiTexCoord3dvARB(uint target, double[] v);
        private delegate void glMultiTexCoord3fARB(uint target, float s, float t, float r);
        private delegate void glMultiTexCoord3fvARB(uint target, float[] v);
        private delegate void glMultiTexCoord3iARB(uint target, int s, int t, int r);
        private delegate void glMultiTexCoord3ivARB(uint target, int[] v);
        private delegate void glMultiTexCoord3sARB(uint target, short s, short t, short r);
        private delegate void glMultiTexCoord3svARB(uint target, short[] v);
        private delegate void glMultiTexCoord4dARB(uint target, double s, double t, double r, double q);
        private delegate void glMultiTexCoord4dvARB(uint target, double[] v);
        private delegate void glMultiTexCoord4fARB(uint target, float s, float t, float r, float q);
        private delegate void glMultiTexCoord4fvARB(uint target, float[] v);
        private delegate void glMultiTexCoord4iARB(uint target, int s, int t, int r, int q);
        private delegate void glMultiTexCoord4ivARB(uint target, int[] v);
        private delegate void glMultiTexCoord4sARB(uint target, short s, short t, short r, short q);
        private delegate void glMultiTexCoord4svARB(uint target, short[] v);

        //  Constants
        public const uint GL_TEXTURE0_ARB = 0x84C0;
        public const uint GL_TEXTURE1_ARB = 0x84C1;
        public const uint GL_TEXTURE2_ARB = 0x84C2;
        public const uint GL_TEXTURE3_ARB = 0x84C3;
        public const uint GL_TEXTURE4_ARB = 0x84C4;
        public const uint GL_TEXTURE5_ARB = 0x84C5;
        public const uint GL_TEXTURE6_ARB = 0x84C6;
        public const uint GL_TEXTURE7_ARB = 0x84C7;
        public const uint GL_TEXTURE8_ARB = 0x84C8;
        public const uint GL_TEXTURE9_ARB = 0x84C9;
        public const uint GL_TEXTURE10_ARB = 0x84CA;
        public const uint GL_TEXTURE11_ARB = 0x84CB;
        public const uint GL_TEXTURE12_ARB = 0x84CC;
        public const uint GL_TEXTURE13_ARB = 0x84CD;
        public const uint GL_TEXTURE14_ARB = 0x84CE;
        public const uint GL_TEXTURE15_ARB = 0x84CF;
        public const uint GL_TEXTURE16_ARB = 0x84D0;
        public const uint GL_TEXTURE17_ARB = 0x84D1;
        public const uint GL_TEXTURE18_ARB = 0x84D2;
        public const uint GL_TEXTURE19_ARB = 0x84D3;
        public const uint GL_TEXTURE20_ARB = 0x84D4;
        public const uint GL_TEXTURE21_ARB = 0x84D5;
        public const uint GL_TEXTURE22_ARB = 0x84D6;
        public const uint GL_TEXTURE23_ARB = 0x84D7;
        public const uint GL_TEXTURE24_ARB = 0x84D8;
        public const uint GL_TEXTURE25_ARB = 0x84D9;
        public const uint GL_TEXTURE26_ARB = 0x84DA;
        public const uint GL_TEXTURE27_ARB = 0x84DB;
        public const uint GL_TEXTURE28_ARB = 0x84DC;
        public const uint GL_TEXTURE29_ARB = 0x84DD;
        public const uint GL_TEXTURE30_ARB = 0x84DE;
        public const uint GL_TEXTURE31_ARB = 0x84DF;
        public const uint GL_ACTIVE_TEXTURE_ARB = 0x84E0;
        public const uint GL_CLIENT_ACTIVE_TEXTURE_ARB = 0x84E1;
        public const uint GL_MAX_TEXTURE_UNITS_ARB = 0x84E2;

        #endregion

        #region GL_ARB_texture_compression

        //  Methods
        public void CompressedTexImage3DARB(uint target, int level, uint internalformat, int width, int height, int depth, int border, int imageSize, IntPtr data)
        {
            GetDelegateFor<glCompressedTexImage3DARB>()(target, level, internalformat, width, height, depth, border, imageSize, data);
        }
        public void CompressedTexImage2DARB(uint target, int level, uint internalformat, int width, int height, int border, int imageSize, IntPtr data)
        {
            GetDelegateFor<glCompressedTexImage2DARB>()(target, level, internalformat, width, height, border, imageSize, data);
        }
        public void CompressedTexImage1DARB(uint target, int level, uint internalformat, int width, int border, int imageSize, IntPtr data)
        {
            GetDelegateFor<glCompressedTexImage1DARB>()(target, level, internalformat, width, border, imageSize, data);
        }
        public void CompressedTexSubImage3DARB(uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, int imageSize, IntPtr data)
        {
            GetDelegateFor<glCompressedTexSubImage3DARB>()(target, level, xoffset, yoffset, zoffset, width, height, depth, format, imageSize, data);
        }
        public void CompressedTexSubImage2DARB(uint target, int level, int xoffset, int yoffset, int width, int height, uint format, int imageSize, IntPtr data)
        {
            GetDelegateFor<glCompressedTexSubImage2DARB>()(target, level, xoffset, yoffset, width, height, format, imageSize, data);
        }
        public void CompressedTexSubImage1DARB(uint target, int level, int xoffset, int width, uint format, int imageSize, IntPtr data)
        {
            GetDelegateFor<glCompressedTexSubImage1DARB>()(target, level, xoffset, width, format, imageSize, data);
        }

        //  Delegates
        private delegate void glCompressedTexImage3DARB(uint target, int level, uint internalformat, int width, int height, int depth, int border, int imageSize, IntPtr data);
        private delegate void glCompressedTexImage2DARB(uint target, int level, uint internalformat, int width, int height, int border, int imageSize, IntPtr data);
        private delegate void glCompressedTexImage1DARB(uint target, int level, uint internalformat, int width, int border, int imageSize, IntPtr data);
        private delegate void glCompressedTexSubImage3DARB(uint target, int level, int xoffset, int yoffset, int zoffset, int width, int height, int depth, uint format, int imageSize, IntPtr data);
        private delegate void glCompressedTexSubImage2DARB(uint target, int level, int xoffset, int yoffset, int width, int height, uint format, int imageSize, IntPtr data);
        private delegate void glCompressedTexSubImage1DARB(uint target, int level, int xoffset, int width, uint format, int imageSize, IntPtr data);
        private delegate void glGetCompressedTexImageARB(uint target, int level, IntPtr img);

        //  Constants
        public const uint GL_COMPRESSED_ALPHA_ARB = 0x84E9;
        public const uint GL_COMPRESSED_LUMINANCE_ARB = 0x84EA;
        public const uint GL_COMPRESSED_LUMINANCE_ALPHA_ARB = 0x84EB;
        public const uint GL_COMPRESSED_INTENSITY_ARB = 0x84EC;
        public const uint GL_COMPRESSED_RGB_ARB = 0x84ED;
        public const uint GL_COMPRESSED_RGBA_ARB = 0x84EE;
        public const uint GL_TEXTURE_COMPRESSION_HINT_ARB = 0x84EF;
        public const uint GL_TEXTURE_COMPRESSED_IMAGE_SIZE_ARB = 0x86A0;
        public const uint GL_TEXTURE_COMPRESSED_ARB = 0x86A1;
        public const uint GL_NUM_COMPRESSED_TEXTURE_FORMATS_ARB = 0x86A2;
        public const uint GL_COMPRESSED_TEXTURE_FORMATS_ARB = 0x86A3;

        #endregion

        #region GL_EXT_texture_cube_map

        //  Constants
        public const uint GL_NORMAL_MAP_EXT = 0x8511;
        public const uint GL_REFLECTION_MAP_EXT = 0x8512;
        public const uint GL_TEXTURE_CUBE_MAP_EXT = 0x8513;
        public const uint GL_TEXTURE_BINDING_CUBE_MAP_EXT = 0x8514;
        public const uint GL_TEXTURE_CUBE_MAP_POSITIVE_X_EXT = 0x8515;
        public const uint GL_TEXTURE_CUBE_MAP_NEGATIVE_X_EXT = 0x8516;
        public const uint GL_TEXTURE_CUBE_MAP_POSITIVE_Y_EXT = 0x8517;
        public const uint GL_TEXTURE_CUBE_MAP_NEGATIVE_Y_EXT = 0x8518;
        public const uint GL_TEXTURE_CUBE_MAP_POSITIVE_Z_EXT = 0x8519;
        public const uint GL_TEXTURE_CUBE_MAP_NEGATIVE_Z_EXT = 0x851A;
        public const uint GL_PROXY_TEXTURE_CUBE_MAP_EXT = 0x851B;
        public const uint GL_MAX_CUBE_MAP_TEXTURE_SIZE_EXT = 0x851C;

        #endregion

        #region GL_ARB_multisample

        //  Methods
        public void SampleCoverageARB(float value, bool invert)
        {
            GetDelegateFor<glSampleCoverageARB>()(value, invert);
        }

        //  Delegates
        private delegate void glSampleCoverageARB(float value, bool invert);

        //  Constants
        public const uint GL_MULTISAMPLE_ARB = 0x809D;
        public const uint GL_SAMPLE_ALPHA_TO_COVERAGE_ARB = 0x809E;
        public const uint GL_SAMPLE_ALPHA_TO_ONE_ARB = 0x809F;
        public const uint GL_SAMPLE_COVERAGE_ARB = 0x80A0;
        public const uint GL_SAMPLE_BUFFERS_ARB = 0x80A8;
        public const uint GL_SAMPLES_ARB = 0x80A9;
        public const uint GL_SAMPLE_COVERAGE_VALUE_ARB = 0x80AA;
        public const uint GL_SAMPLE_COVERAGE_INVERT_ARB = 0x80AB;
        public const uint GL_MULTISAMPLE_BIT_ARB = 0x20000000;

        #endregion

        #region GL_ARB_texture_env_add

        //  Appears to not have any functionality

        #endregion

        #region GL_ARB_texture_env_combine

        //  Constants
        public const uint GL_COMBINE_ARB = 0x8570;
        public const uint GL_COMBINE_RGB_ARB = 0x8571;
        public const uint GL_COMBINE_ALPHA_ARB = 0x8572;
        public const uint GL_SOURCE0_RGB_ARB = 0x8580;
        public const uint GL_SOURCE1_RGB_ARB = 0x8581;
        public const uint GL_SOURCE2_RGB_ARB = 0x8582;
        public const uint GL_SOURCE0_ALPHA_ARB = 0x8588;
        public const uint GL_SOURCE1_ALPHA_ARB = 0x8589;
        public const uint GL_SOURCE2_ALPHA_ARB = 0x858A;
        public const uint GL_OPERAND0_RGB_ARB = 0x8590;
        public const uint GL_OPERAND1_RGB_ARB = 0x8591;
        public const uint GL_OPERAND2_RGB_ARB = 0x8592;
        public const uint GL_OPERAND0_ALPHA_ARB = 0x8598;
        public const uint GL_OPERAND1_ALPHA_ARB = 0x8599;
        public const uint GL_OPERAND2_ALPHA_ARB = 0x859A;
        public const uint GL_RGB_SCALE_ARB = 0x8573;
        public const uint GL_ADD_SIGNED_ARB = 0x8574;
        public const uint GL_INTERPOLATE_ARB = 0x8575;
        public const uint GL_SUBTRACT_ARB = 0x84E7;
        public const uint GL_CONSTANT_ARB = 0x8576;
        public const uint GL_PRIMARY_COLOR_ARB = 0x8577;
        public const uint GL_PREVIOUS_ARB = 0x8578;

        #endregion

        #region GL_ARB_texture_env_dot3

        //  Constants
        public const uint GL_DOT3_RGB_ARB = 0x86AE;
        public const uint GL_DOT3_RGBA_ARB = 0x86AF;

        #endregion

        #region GL_ARB_texture_border_clamp

        //  Constants
        public const uint GL_CLAMP_TO_BORDER_ARB = 0x812D;

        #endregion

        #region GL_ARB_transpose_matrix

        //  Methods
        public void glLoadTransposeMatrixARB(float[] m)
        {
            GetDelegateFor<glLoadTransposeMatrixfARB>()(m);
        }
        public void glLoadTransposeMatrixARB(double[] m)
        {
            GetDelegateFor<glLoadTransposeMatrixdARB>()(m);
        }
        public void glMultTransposeMatrixARB(float[] m)
        {
            GetDelegateFor<glMultTransposeMatrixfARB>()(m);
        }
        public void glMultTransposeMatrixARB(double[] m)
        {
            GetDelegateFor<glMultTransposeMatrixdARB>()(m);
        }

        //  Delegates
        private delegate void glLoadTransposeMatrixfARB(float[] m);
        private delegate void glLoadTransposeMatrixdARB(double[] m);
        private delegate void glMultTransposeMatrixfARB(float[] m);
        private delegate void glMultTransposeMatrixdARB(double[] m);

        //  Constants
        public const uint GL_TRANSPOSE_MODELVIEW_MATRIX_ARB = 0x84E3;
        public const uint GL_TRANSPOSE_PROJECTION_MATRIX_ARB = 0x84E4;
        public const uint GL_TRANSPOSE_TEXTURE_MATRIX_ARB = 0x84E5;
        public const uint GL_TRANSPOSE_COLOR_MATRIX_ARB = 0x84E6;

        #endregion

        #region GL_SGIS_generate_mipmap

        //  Constants
        public const uint GL_GENERATE_MIPMAP_SGIS = 0x8191;
        public const uint GL_GENERATE_MIPMAP_HINT_SGIS = 0x8192;

        #endregion

        #region GL_NV_blend_square

        //  Appears to be empty.

        #endregion

        #region GL_ARB_depth_texture

        //  Constants
        public const uint GL_DEPTH_COMPONENT16_ARB = 0x81A5;
        public const uint GL_DEPTH_COMPONENT24_ARB = 0x81A6;
        public const uint GL_DEPTH_COMPONENT32_ARB = 0x81A7;
        public const uint GL_TEXTURE_DEPTH_SIZE_ARB = 0x884A;
        public const uint GL_DEPTH_TEXTURE_MODE_ARB = 0x884B;

        #endregion

        #region GL_ARB_shadow

        //  Constants
        public const uint GL_TEXTURE_COMPARE_MODE_ARB = 0x884C;
        public const uint GL_TEXTURE_COMPARE_FUNC_ARB = 0x884D;
        public const uint GL_COMPARE_R_TO_TEXTURE_ARB = 0x884E;

        #endregion

        #region GL_EXT_fog_coord

        //  Methods
        public void FogCoordEXT(float coord)
        {
            GetDelegateFor<glFogCoordfEXT>()(coord);
        }
        public void FogCoordEXT(float[] coord)
        {
            GetDelegateFor<glFogCoordfvEXT>()(coord);
        }
        public void FogCoordEXT(double coord)
        {
            GetDelegateFor<glFogCoorddEXT>()(coord);
        }
        public void FogCoordEXT(double[] coord)
        {
            GetDelegateFor<glFogCoorddvEXT>()(coord);
        }
        public void FogCoordPointerEXT(uint type, int stride, IntPtr pointer)
        {
            GetDelegateFor<glFogCoordPointerEXT>()(type, stride, pointer);
        }

        //  Delegates
        private delegate void glFogCoordfEXT(float coord);
        private delegate void glFogCoordfvEXT(float[] coord);
        private delegate void glFogCoorddEXT(double coord);
        private delegate void glFogCoorddvEXT(double[] coord);
        private delegate void glFogCoordPointerEXT(uint type, int stride, IntPtr pointer);

        //  Constants
        public const uint GL_FOG_COORDINATE_SOURCE_EXT = 0x8450;
        public const uint GL_FOG_COORDINATE_EXT = 0x8451;
        public const uint GL_FRAGMENT_DEPTH_EXT = 0x8452;
        public const uint GL_CURRENT_FOG_COORDINATE_EXT = 0x8453;
        public const uint GL_FOG_COORDINATE_ARRAY_TYPE_EXT = 0x8454;
        public const uint GL_FOG_COORDINATE_ARRAY_STRIDE_EXT = 0x8455;
        public const uint GL_FOG_COORDINATE_ARRAY_POINTER_EXT = 0x8456;
        public const uint GL_FOG_COORDINATE_ARRAY_EXT = 0x8457;

        #endregion

        #region GL_EXT_multi_draw_arrays

        //  Methods
        public void MultiDrawArraysEXT(uint mode, int[] first, int[] count, int primcount)
        {
            GetDelegateFor<glMultiDrawArraysEXT>()(mode, first, count, primcount);
        }
        public void MultiDrawElementsEXT(uint mode, int[] count, uint type, IntPtr indices, int primcount)
        {
            GetDelegateFor<glMultiDrawElementsEXT>()(mode, count, type, indices, primcount);
        }

        //  Delegates
        private delegate void glMultiDrawArraysEXT(uint mode, int[] first, int[] count, int primcount);
        private delegate void glMultiDrawElementsEXT(uint mode, int[] count, uint type, IntPtr indices, int primcount);

        #endregion

        #region GL_ARB_point_parameters

        //  Methods
        public void glPointParameterARB(uint pname, float parameter)
        {
            GetDelegateFor<glPointParameterfARB>()(pname, parameter);
        }
        public void glPointParameterARB(uint pname, float[] parameters)
        {
            GetDelegateFor<glPointParameterfvARB>()(pname, parameters);
        }

        //  Delegates
        private delegate void glPointParameterfARB(uint pname, float param);
        private delegate void glPointParameterfvARB(uint pname, float[] parameters);

        //  Constants
        public const uint GL_POINT_SIZE_MIN_ARB = 0x8126;
        public const uint GL_POINT_SIZE_MAX_ARB = 0x8127;
        public const uint GL_POINT_FADE_THRESHOLD_SIZE_ARB = 0x8128;
        public const uint GL_POINT_DISTANCE_ATTENUATION_ARB = 0x8129;

        #endregion

        #region GL_EXT_secondary_color

        //  Methods
        public void SecondaryColor3EXT(sbyte red, sbyte green, sbyte blue)
        {
            GetDelegateFor<glSecondaryColor3bEXT>()(red, green, blue);
        }
        public void SecondaryColor3EXT(sbyte[] v)
        {
            GetDelegateFor<glSecondaryColor3bvEXT>()(v);
        }
        public void SecondaryColor3EXT(double red, double green, double blue)
        {
            GetDelegateFor<glSecondaryColor3dEXT>()(red, green, blue);
        }
        public void SecondaryColor3EXT(double[] v)
        {
            GetDelegateFor<glSecondaryColor3dvEXT>()(v);
        }
        public void SecondaryColor3EXT(float red, float green, float blue)
        {
            GetDelegateFor<glSecondaryColor3fEXT>()(red, green, blue);
        }
        public void SecondaryColor3EXT(float[] v)
        {
            GetDelegateFor<glSecondaryColor3fvEXT>()(v);
        }
        public void SecondaryColor3EXT(int red, int green, int blue)
        {
            GetDelegateFor<glSecondaryColor3iEXT>()(red, green, blue);
        }
        public void SecondaryColor3EXT(int[] v)
        {
            GetDelegateFor<glSecondaryColor3ivEXT>()(v);
        }
        public void SecondaryColor3EXT(short red, short green, short blue)
        {
            GetDelegateFor<glSecondaryColor3sEXT>()(red, green, blue);
        }
        public void SecondaryColor3EXT(short[] v)
        {
            GetDelegateFor<glSecondaryColor3svEXT>()(v);
        }
        public void SecondaryColor3EXT(byte red, byte green, byte blue)
        {
            GetDelegateFor<glSecondaryColor3ubEXT>()(red, green, blue);
        }
        public void SecondaryColor3EXT(byte[] v)
        {
            GetDelegateFor<glSecondaryColor3ubvEXT>()(v);
        }
        public void SecondaryColor3EXT(uint red, uint green, uint blue)
        {
            GetDelegateFor<glSecondaryColor3uiEXT>()(red, green, blue);
        }
        public void SecondaryColor3EXT(uint[] v)
        {
            GetDelegateFor<glSecondaryColor3uivEXT>()(v);
        }
        public void SecondaryColor3EXT(ushort red, ushort green, ushort blue)
        {
            GetDelegateFor<glSecondaryColor3usEXT>()(red, green, blue);
        }
        public void SecondaryColor3EXT(ushort[] v)
        {
            GetDelegateFor<glSecondaryColor3usvEXT>()(v);
        }
        public void SecondaryColorPointerEXT(int size, uint type, int stride, IntPtr pointer)
        {
            GetDelegateFor<glSecondaryColorPointerEXT>()(size, type, stride, pointer);
        }

        //  Delegates
        private delegate void glSecondaryColor3bEXT(sbyte red, sbyte green, sbyte blue);
        private delegate void glSecondaryColor3bvEXT(sbyte[] v);
        private delegate void glSecondaryColor3dEXT(double red, double green, double blue);
        private delegate void glSecondaryColor3dvEXT(double[] v);
        private delegate void glSecondaryColor3fEXT(float red, float green, float blue);
        private delegate void glSecondaryColor3fvEXT(float[] v);
        private delegate void glSecondaryColor3iEXT(int red, int green, int blue);
        private delegate void glSecondaryColor3ivEXT(int[] v);
        private delegate void glSecondaryColor3sEXT(short red, short green, short blue);
        private delegate void glSecondaryColor3svEXT(short[] v);
        private delegate void glSecondaryColor3ubEXT(byte red, byte green, byte blue);
        private delegate void glSecondaryColor3ubvEXT(byte[] v);
        private delegate void glSecondaryColor3uiEXT(uint red, uint green, uint blue);
        private delegate void glSecondaryColor3uivEXT(uint[] v);
        private delegate void glSecondaryColor3usEXT(ushort red, ushort green, ushort blue);
        private delegate void glSecondaryColor3usvEXT(ushort[] v);
        private delegate void glSecondaryColorPointerEXT(int size, uint type, int stride, IntPtr pointer);

        //  Constants        
        public const uint GL_COLOR_SUM_EXT = 0x8458;
        public const uint GL_CURRENT_SECONDARY_COLOR_EXT = 0x8459;
        public const uint GL_SECONDARY_COLOR_ARRAY_SIZE_EXT = 0x845A;
        public const uint GL_SECONDARY_COLOR_ARRAY_TYPE_EXT = 0x845B;
        public const uint GL_SECONDARY_COLOR_ARRAY_STRIDE_EXT = 0x845C;
        public const uint GL_SECONDARY_COLOR_ARRAY_POINTER_EXT = 0x845D;
        public const uint GL_SECONDARY_COLOR_ARRAY_EXT = 0x845E;

        #endregion

        #region  GL_EXT_blend_func_separate

        //  Methods
        public void BlendFuncSeparateEXT(uint sfactorRGB, uint dfactorRGB, uint sfactorAlpha, uint dfactorAlpha)
        {
            GetDelegateFor<glBlendFuncSeparateEXT>()(sfactorRGB, dfactorRGB, sfactorAlpha, dfactorAlpha);
        }

        //  Delegates
        private delegate void glBlendFuncSeparateEXT(uint sfactorRGB, uint dfactorRGB, uint sfactorAlpha, uint dfactorAlpha);

        //  Constants
        public const uint GL_BLEND_DST_RGB_EXT = 0x80C8;
        public const uint GL_BLEND_SRC_RGB_EXT = 0x80C9;
        public const uint GL_BLEND_DST_ALPHA_EXT = 0x80CA;
        public const uint GL_BLEND_SRC_ALPHA_EXT = 0x80CB;

        #endregion

        #region GL_EXT_stencil_wrap

        //  Constants
        public const uint GL_INCR_WRAP_EXT = 0x8507;
        public const uint GL_DECR_WRAP_EXT = 0x8508;

        #endregion

        #region GL_ARB_texture_env_crossbar

        //  No methods or constants.

        #endregion

        #region GL_EXT_texture_lod_bias

        //  Constants
        public const uint GL_MAX_TEXTURE_LOD_BIAS_EXT = 0x84FD;
        public const uint GL_TEXTURE_FILTER_CONTROL_EXT = 0x8500;
        public const uint GL_TEXTURE_LOD_BIAS_EXT = 0x8501;

        #endregion

        #region GL_ARB_texture_mirrored_repeat

        //  Constants
        public const uint GL_MIRRORED_REPEAT_ARB = 0x8370;

        #endregion

        #region GL_ARB_window_pos

        //  Methods
        public void WindowPos2ARB(double x, double y)
        {
            GetDelegateFor<glWindowPos2dARB>()(x, y);
        }
        public void WindowPos2ARB(double[] v)
        {
            GetDelegateFor<glWindowPos2dvARB>()(v);
        }
        public void WindowPos2ARB(float x, float y)
        {
            GetDelegateFor<glWindowPos2fARB>()(x, y);
        }
        public void WindowPos2ARB(float[] v)
        {
            GetDelegateFor<glWindowPos2fvARB>()(v);
        }
        public void WindowPos2ARB(int x, int y)
        {
            GetDelegateFor<glWindowPos2iARB>()(x, y);
        }
        public void WindowPos2ARB(int[] v)
        {
            GetDelegateFor<glWindowPos2ivARB>()(v);
        }
        public void WindowPos2ARB(short x, short y)
        {
            GetDelegateFor<glWindowPos2sARB>()(x, y);
        }
        public void WindowPos2ARB(short[] v)
        {
            GetDelegateFor<glWindowPos2svARB>()(v);
        }
        public void WindowPos3ARB(double x, double y, double z)
        {
            GetDelegateFor<glWindowPos3dARB>()(x, y, z);
        }
        public void WindowPos3ARB(double[] v)
        {
            GetDelegateFor<glWindowPos3dvARB>()(v);
        }
        public void WindowPos3ARB(float x, float y, float z)
        {
            GetDelegateFor<glWindowPos3fARB>()(x, y, z);
        }
        public void WindowPos3ARB(float[] v)
        {
            GetDelegateFor<glWindowPos3fvARB>()(v);
        }
        public void WindowPos3ARB(int x, int y, int z)
        {
            GetDelegateFor<glWindowPos3iARB>()(x, y, z);
        }
        public void WindowPos3ARB(int[] v)
        {
            GetDelegateFor<glWindowPos3ivARB>()(v);
        }
        public void WindowPos3ARB(short x, short y, short z)
        {
            GetDelegateFor<glWindowPos3sARB>()(x, y, z);
        }
        public void WindowPos3ARB(short[] v)
        {
            GetDelegateFor<glWindowPos3svARB>()(v);
        }

        //  Delegates
        private delegate void glWindowPos2dARB(double x, double y);
        private delegate void glWindowPos2dvARB(double[] v);
        private delegate void glWindowPos2fARB(float x, float y);
        private delegate void glWindowPos2fvARB(float[] v);
        private delegate void glWindowPos2iARB(int x, int y);
        private delegate void glWindowPos2ivARB(int[] v);
        private delegate void glWindowPos2sARB(short x, short y);
        private delegate void glWindowPos2svARB(short[] v);
        private delegate void glWindowPos3dARB(double x, double y, double z);
        private delegate void glWindowPos3dvARB(double[] v);
        private delegate void glWindowPos3fARB(float x, float y, float z);
        private delegate void glWindowPos3fvARB(float[] v);
        private delegate void glWindowPos3iARB(int x, int y, int z);
        private delegate void glWindowPos3ivARB(int[] v);
        private delegate void glWindowPos3sARB(short x, short y, short z);
        private delegate void glWindowPos3svARB(short[] v);

        #endregion

        #region GL_ARB_vertex_buffer_object

        //  Methods
        public void BindBufferARB(uint target, uint buffer)
        {
            GetDelegateFor<glBindBufferARB>()(target, buffer);
        }
        public void DeleteBuffersARB(int n, uint[] buffers)
        {
            GetDelegateFor<glDeleteBuffersARB>()(n, buffers);
        }
        public void GenBuffersARB(int n, uint[] buffers)
        {
            GetDelegateFor<glGenBuffersARB>()(n, buffers);
        }
        public bool IsBufferARB(uint buffer)
        {
            return (bool)GetDelegateFor<glIsBufferARB>()(buffer);
        }
        public void BufferDataARB(uint target, uint size, IntPtr data, uint usage)
        {
            GetDelegateFor<glBufferDataARB>()(target, size, data, usage);
        }
        public void BufferSubDataARB(uint target, uint offset, uint size, IntPtr data)
        {
            GetDelegateFor<glBufferSubDataARB>()(target, offset, size, data);
        }
        public void GetBufferSubDataARB(uint target, uint offset, uint size, IntPtr data)
        {
            GetDelegateFor<glGetBufferSubDataARB>()(target, offset, size, data);
        }
        public IntPtr MapBufferARB(uint target, uint access)
        {
            return (IntPtr)GetDelegateFor<glMapBufferARB>()(target, access);
        }
        public bool UnmapBufferARB(uint target)
        {
            return (bool)GetDelegateFor<glUnmapBufferARB>()(target);
        }
        public void GetBufferParameterARB(uint target, uint pname, int[] parameters)
        {
            GetDelegateFor<glGetBufferParameterivARB>()(target, pname, parameters);
        }
        public void GetBufferPointerARB(uint target, uint pname, IntPtr parameters)
        {
            GetDelegateFor<glGetBufferPointervARB>()(target, pname, parameters);
        }

        //  Delegates
        private delegate void glBindBufferARB(uint target, uint buffer);
        private delegate void glDeleteBuffersARB(int n, uint[] buffers);
        private delegate void glGenBuffersARB(int n, uint[] buffers);
        private delegate bool glIsBufferARB(uint buffer);
        private delegate void glBufferDataARB(uint target, uint size, IntPtr data, uint usage);
        private delegate void glBufferSubDataARB(uint target, uint offset, uint size, IntPtr data);
        private delegate void glGetBufferSubDataARB(uint target, uint offset, uint size, IntPtr data);
        private delegate IntPtr glMapBufferARB(uint target, uint access);
        private delegate bool glUnmapBufferARB(uint target);
        private delegate void glGetBufferParameterivARB(uint target, uint pname, int[] parameters);
        private delegate void glGetBufferPointervARB(uint target, uint pname, IntPtr parameters);

        //  Constants
        public const uint GL_BUFFER_SIZE_ARB = 0x8764;
        public const uint GL_BUFFER_USAGE_ARB = 0x8765;
        public const uint GL_ARRAY_BUFFER_ARB = 0x8892;
        public const uint GL_ELEMENT_ARRAY_BUFFER_ARB = 0x8893;
        public const uint GL_ARRAY_BUFFER_BINDING_ARB = 0x8894;
        public const uint GL_ELEMENT_ARRAY_BUFFER_BINDING_ARB = 0x8895;
        public const uint GL_VERTEX_ARRAY_BUFFER_BINDING_ARB = 0x8896;
        public const uint GL_NORMAL_ARRAY_BUFFER_BINDING_ARB = 0x8897;
        public const uint GL_COLOR_ARRAY_BUFFER_BINDING_ARB = 0x8898;
        public const uint GL_INDEX_ARRAY_BUFFER_BINDING_ARB = 0x8899;
        public const uint GL_TEXTURE_COORD_ARRAY_BUFFER_BINDING_ARB = 0x889A;
        public const uint GL_EDGE_FLAG_ARRAY_BUFFER_BINDING_ARB = 0x889B;
        public const uint GL_SECONDARY_COLOR_ARRAY_BUFFER_BINDING_ARB = 0x889C;
        public const uint GL_FOG_COORDINATE_ARRAY_BUFFER_BINDING_ARB = 0x889D;
        public const uint GL_WEIGHT_ARRAY_BUFFER_BINDING_ARB = 0x889E;
        public const uint GL_VERTEX_ATTRIB_ARRAY_BUFFER_BINDING_ARB = 0x889F;
        public const uint GL_READ_ONLY_ARB = 0x88B8;
        public const uint GL_WRITE_ONLY_ARB = 0x88B9;
        public const uint GL_READ_WRITE_ARB = 0x88BA;
        public const uint GL_BUFFER_ACCESS_ARB = 0x88BB;
        public const uint GL_BUFFER_MAPPED_ARB = 0x88BC;
        public const uint GL_BUFFER_MAP_POINTER_ARB = 0x88BD;
        public const uint GL_STREAM_DRAW_ARB = 0x88E0;
        public const uint GL_STREAM_READ_ARB = 0x88E1;
        public const uint GL_STREAM_COPY_ARB = 0x88E2;
        public const uint GL_STATIC_DRAW_ARB = 0x88E4;
        public const uint GL_STATIC_READ_ARB = 0x88E5;
        public const uint GL_STATIC_COPY_ARB = 0x88E6;
        public const uint GL_DYNAMIC_DRAW_ARB = 0x88E8;
        public const uint GL_DYNAMIC_READ_ARB = 0x88E9;
        public const uint GL_DYNAMIC_COPY_ARB = 0x88EA;
        #endregion

        #region GL_ARB_occlusion_query

        //  Methods
        public void GenQueriesARB(int n, uint[] ids)
        {
            GetDelegateFor<glGenQueriesARB>()(n, ids);
        }
        public void DeleteQueriesARB(int n, uint[] ids)
        {
            GetDelegateFor<glDeleteQueriesARB>()(n, ids);
        }
        public bool IsQueryARB(uint id)
        {
            return (bool)GetDelegateFor<glIsQueryARB>()(id);
        }
        public void BeginQueryARB(uint target, uint id)
        {
            GetDelegateFor<glBeginQueryARB>()(target, id);
        }
        public void EndQueryARB(uint target)
        {
            GetDelegateFor<glEndQueryARB>()(target);
        }
        public void GetQueryARB(uint target, uint pname, int[] parameters)
        {
            GetDelegateFor<glGetQueryivARB>()(target, pname, parameters);
        }
        public void GetQueryObjectARB(uint id, uint pname, int[] parameters)
        {
            GetDelegateFor<glGetQueryObjectivARB>()(id, pname, parameters);
        }
        public void GetQueryObjectARB(uint id, uint pname, uint[] parameters)
        {
            GetDelegateFor<glGetQueryObjectuivARB>()(id, pname, parameters);
        }

        //  Delegates
        private delegate void glGenQueriesARB(int n, uint[] ids);
        private delegate void glDeleteQueriesARB(int n, uint[] ids);
        private delegate bool glIsQueryARB(uint id);
        private delegate void glBeginQueryARB(uint target, uint id);
        private delegate void glEndQueryARB(uint target);
        private delegate void glGetQueryivARB(uint target, uint pname, int[] parameters);
        private delegate void glGetQueryObjectivARB(uint id, uint pname, int[] parameters);
        private delegate void glGetQueryObjectuivARB(uint id, uint pname, uint[] parameters);

        //  Constants
        public const uint GL_QUERY_COUNTER_BITS_ARB = 0x8864;
        public const uint GL_CURRENT_QUERY_ARB = 0x8865;
        public const uint GL_QUERY_RESULT_ARB = 0x8866;
        public const uint GL_QUERY_RESULT_AVAILABLE_ARB = 0x8867;
        public const uint GL_SAMPLES_PASSED_ARB = 0x8914;
        public const uint GL_ANY_SAMPLES_PASSED = 0x8C2F;

        #endregion

        #region GL_ARB_shader_objects

        //  Methods
        public void DeleteObjectARB(uint obj)
        {
            GetDelegateFor<glDeleteObjectARB>()(obj);
        }
        public uint GetHandleARB(uint pname)
        {
            return (uint)GetDelegateFor<glGetHandleARB>()(pname);
        }
        public void DetachObjectARB(uint containerObj, uint attachedObj)
        {
            GetDelegateFor<glDetachObjectARB>()(containerObj, attachedObj);
        }
        public uint CreateShaderObjectARB(uint shaderType)
        {
            return (uint)GetDelegateFor<glCreateShaderObjectARB>()(shaderType);
        }
        public void ShaderSourceARB(uint shaderObj, int count, string[] source, ref int length)
        {
            GetDelegateFor<glShaderSourceARB>()(shaderObj, count, source, ref length);
        }
        public void CompileShaderARB(uint shaderObj)
        {
            GetDelegateFor<glCompileShaderARB>()(shaderObj);
        }
        public uint CreateProgramObjectARB()
        {
            return (uint)GetDelegateFor<glCreateProgramObjectARB>()();
        }
        public void AttachObjectARB(uint containerObj, uint obj)
        {
            GetDelegateFor<glAttachObjectARB>()(containerObj, obj);
        }
        public void LinkProgramARB(uint programObj)
        {
            GetDelegateFor<glLinkProgramARB>()(programObj);
        }
        public void UseProgramObjectARB(uint programObj)
        {
            GetDelegateFor<glUseProgramObjectARB>()(programObj);
        }
        public void ValidateProgramARB(uint programObj)
        {
            GetDelegateFor<glValidateProgramARB>()(programObj);
        }
        public void Uniform1ARB(int location, float v0)
        {
            GetDelegateFor<glUniform1fARB>()(location, v0);
        }
        public void Uniform2ARB(int location, float v0, float v1)
        {
            GetDelegateFor<glUniform2fARB>()(location, v0, v1);
        }
        public void Uniform3ARB(int location, float v0, float v1, float v2)
        {
            GetDelegateFor<glUniform3fARB>()(location, v0, v1, v2);
        }
        public void Uniform4ARB(int location, float v0, float v1, float v2, float v3)
        {
            GetDelegateFor<glUniform4fARB>()(location, v0, v1, v2, v3);
        }
        public void Uniform1ARB(int location, int v0)
        {
            GetDelegateFor<glUniform1iARB>()(location, v0);
        }
        public void Uniform2ARB(int location, int v0, int v1)
        {
            GetDelegateFor<glUniform2iARB>()(location, v0, v1);
        }
        public void Uniform3ARB(int location, int v0, int v1, int v2)
        {
            GetDelegateFor<glUniform3iARB>()(location, v0, v1, v2);
        }
        public void Uniform4ARB(int location, int v0, int v1, int v2, int v3)
        {
            GetDelegateFor<glUniform4iARB>()(location, v0, v1, v2, v3);
        }
        public void Uniform1ARB(int location, int count, float[] value)
        {
            GetDelegateFor<glUniform1fvARB>()(location, count, value);
        }
        public void Uniform2ARB(int location, int count, float[] value)
        {
            GetDelegateFor<glUniform2fvARB>()(location, count, value);
        }
        public void Uniform3ARB(int location, int count, float[] value)
        {
            GetDelegateFor<glUniform3fvARB>()(location, count, value);
        }
        public void Uniform4ARB(int location, int count, float[] value)
        {
            GetDelegateFor<glUniform4fvARB>()(location, count, value);
        }
        public void Uniform1ARB(int location, int count, int[] value)
        {
            GetDelegateFor<glUniform1ivARB>()(location, count, value);
        }
        public void Uniform2ARB(int location, int count, int[] value)
        {
            GetDelegateFor<glUniform2ivARB>()(location, count, value);
        }
        public void Uniform3ARB(int location, int count, int[] value)
        {
            GetDelegateFor<glUniform3ivARB>()(location, count, value);
        }
        public void Uniform4ARB(int location, int count, int[] value)
        {
            GetDelegateFor<glUniform4ivARB>()(location, count, value);
        }
        public void UniformMatrix2ARB(int location, int count, bool transpose, float[] value)
        {
            GetDelegateFor<glUniformMatrix2fvARB>()(location, count, transpose, value);
        }
        public void UniformMatrix3ARB(int location, int count, bool transpose, float[] value)
        {
            GetDelegateFor<glUniformMatrix3fvARB>()(location, count, transpose, value);
        }
        public void UniformMatrix4ARB(int location, int count, bool transpose, float[] value)
        {
            GetDelegateFor<glUniformMatrix4fvARB>()(location, count, transpose, value);
        }
        public void GetObjectParameterARB(uint obj, uint pname, float[] parameters)
        {
            GetDelegateFor<glGetObjectParameterfvARB>()(obj, pname, parameters);
        }
        public void GetObjectParameterARB(uint obj, uint pname, int[] parameters)
        {
            GetDelegateFor<glGetObjectParameterivARB>()(obj, pname, parameters);
        }
        public void GetInfoLogARB(uint obj, int maxLength, ref int length, string infoLog)
        {
            GetDelegateFor<glGetInfoLogARB>()(obj, maxLength, ref length, infoLog);
        }
        public void GetAttachedObjectsARB(uint containerObj, int maxCount, ref int count, ref uint obj)
        {
            GetDelegateFor<glGetAttachedObjectsARB>()(containerObj, maxCount, ref count, ref obj);
        }
        public int GetUniformLocationARB(uint programObj, string name)
        {
            return (int)GetDelegateFor<glGetUniformLocationARB>()(programObj, name);
        }
        public void GetActiveUniformARB(uint programObj, uint index, int maxLength, ref int length, ref int size, ref uint type, string name)
        {
            GetDelegateFor<glGetActiveUniformARB>()(programObj, index, maxLength, ref length, ref size, ref type, name);
        }
        public void GetUniformARB(uint programObj, int location, float[] parameters)
        {
            GetDelegateFor<glGetUniformfvARB>()(programObj, location, parameters);
        }
        public void GetUniformARB(uint programObj, int location, int[] parameters)
        {
            GetDelegateFor<glGetUniformivARB>()(programObj, location, parameters);
        }
        public void GetShaderSourceARB(uint obj, int maxLength, ref int length, string source)
        {
            GetDelegateFor<glGetShaderSourceARB>()(obj, maxLength, ref length, source);
        }

        //  Delegates
        private delegate void glDeleteObjectARB(uint obj);
        private delegate uint glGetHandleARB(uint pname);
        private delegate void glDetachObjectARB(uint containerObj, uint attachedObj);
        private delegate uint glCreateShaderObjectARB(uint shaderType);
        private delegate void glShaderSourceARB(uint shaderObj, int count, string[] source, ref int length);
        private delegate void glCompileShaderARB(uint shaderObj);
        private delegate uint glCreateProgramObjectARB();
        private delegate void glAttachObjectARB(uint containerObj, uint obj);
        private delegate void glLinkProgramARB(uint programObj);
        private delegate void glUseProgramObjectARB(uint programObj);
        private delegate void glValidateProgramARB(uint programObj);
        private delegate void glUniform1fARB(int location, float v0);
        private delegate void glUniform2fARB(int location, float v0, float v1);
        private delegate void glUniform3fARB(int location, float v0, float v1, float v2);
        private delegate void glUniform4fARB(int location, float v0, float v1, float v2, float v3);
        private delegate void glUniform1iARB(int location, int v0);
        private delegate void glUniform2iARB(int location, int v0, int v1);
        private delegate void glUniform3iARB(int location, int v0, int v1, int v2);
        private delegate void glUniform4iARB(int location, int v0, int v1, int v2, int v3);
        private delegate void glUniform1fvARB(int location, int count, float[] value);
        private delegate void glUniform2fvARB(int location, int count, float[] value);
        private delegate void glUniform3fvARB(int location, int count, float[] value);
        private delegate void glUniform4fvARB(int location, int count, float[] value);
        private delegate void glUniform1ivARB(int location, int count, int[] value);
        private delegate void glUniform2ivARB(int location, int count, int[] value);
        private delegate void glUniform3ivARB(int location, int count, int[] value);
        private delegate void glUniform4ivARB(int location, int count, int[] value);
        private delegate void glUniformMatrix2fvARB(int location, int count, bool transpose, float[] value);
        private delegate void glUniformMatrix3fvARB(int location, int count, bool transpose, float[] value);
        private delegate void glUniformMatrix4fvARB(int location, int count, bool transpose, float[] value);
        private delegate void glGetObjectParameterfvARB(uint obj, uint pname, float[] parameters);
        private delegate void glGetObjectParameterivARB(uint obj, uint pname, int[] parameters);
        private delegate void glGetInfoLogARB(uint obj, int maxLength, ref int length, string infoLog);
        private delegate void glGetAttachedObjectsARB(uint containerObj, int maxCount, ref int count, ref uint obj);
        private delegate int glGetUniformLocationARB(uint programObj, string name);
        private delegate void glGetActiveUniformARB(uint programObj, uint index, int maxLength, ref int length, ref int size, ref uint type, string name);
        private delegate void glGetUniformfvARB(uint programObj, int location, float[] parameters);
        private delegate void glGetUniformivARB(uint programObj, int location, int[] parameters);
        private delegate void glGetShaderSourceARB(uint obj, int maxLength, ref int length, string source);

        //  Constants
        public const uint GL_PROGRAM_OBJECT_ARB = 0x8B40;
        public const uint GL_SHADER_OBJECT_ARB = 0x8B48;
        public const uint GL_OBJECT_TYPE_ARB = 0x8B4E;
        public const uint GL_OBJECT_SUBTYPE_ARB = 0x8B4F;
        public const uint GL_FLOAT_VEC2_ARB = 0x8B50;
        public const uint GL_FLOAT_VEC3_ARB = 0x8B51;
        public const uint GL_FLOAT_VEC4_ARB = 0x8B52;
        public const uint GL_INT_VEC2_ARB = 0x8B53;
        public const uint GL_INT_VEC3_ARB = 0x8B54;
        public const uint GL_INT_VEC4_ARB = 0x8B55;
        public const uint GL_BOOL_ARB = 0x8B56;
        public const uint GL_BOOL_VEC2_ARB = 0x8B57;
        public const uint GL_BOOL_VEC3_ARB = 0x8B58;
        public const uint GL_BOOL_VEC4_ARB = 0x8B59;
        public const uint GL_FLOAT_MAT2_ARB = 0x8B5A;
        public const uint GL_FLOAT_MAT3_ARB = 0x8B5B;
        public const uint GL_FLOAT_MAT4_ARB = 0x8B5C;
        public const uint GL_SAMPLER_1D_ARB = 0x8B5D;
        public const uint GL_SAMPLER_2D_ARB = 0x8B5E;
        public const uint GL_SAMPLER_3D_ARB = 0x8B5F;
        public const uint GL_SAMPLER_CUBE_ARB = 0x8B60;
        public const uint GL_SAMPLER_1D_SHADOW_ARB = 0x8B61;
        public const uint GL_SAMPLER_2D_SHADOW_ARB = 0x8B62;
        public const uint GL_SAMPLER_2D_RECT_ARB = 0x8B63;
        public const uint GL_SAMPLER_2D_RECT_SHADOW_ARB = 0x8B64;
        public const uint GL_OBJECT_DELETE_STATUS_ARB = 0x8B80;
        public const uint GL_OBJECT_COMPILE_STATUS_ARB = 0x8B81;
        public const uint GL_OBJECT_LINK_STATUS_ARB = 0x8B82;
        public const uint GL_OBJECT_VALIDATE_STATUS_ARB = 0x8B83;
        public const uint GL_OBJECT_INFO_LOG_LENGTH_ARB = 0x8B84;
        public const uint GL_OBJECT_ATTACHED_OBJECTS_ARB = 0x8B85;
        public const uint GL_OBJECT_ACTIVE_UNIFORMS_ARB = 0x8B86;
        public const uint GL_OBJECT_ACTIVE_UNIFORM_MAX_LENGTH_ARB = 0x8B87;
        public const uint GL_OBJECT_SHADER_SOURCE_LENGTH_ARB = 0x8B88;

        #endregion

        #region GL_ARB_vertex_program

        //  Methods
        public void VertexAttrib1ARB(uint index, double x)
        {
            GetDelegateFor<glVertexAttrib1dARB>()(index, x);
        }
        public void VertexAttrib1ARB(uint index, double[] v)
        {
            GetDelegateFor<glVertexAttrib1dvARB>()(index, v);
        }
        public void VertexAttrib1ARB(uint index, float x)
        {
            GetDelegateFor<glVertexAttrib1fARB>()(index, x);
        }
        public void VertexAttrib1ARB(uint index, float[] v)
        {
            GetDelegateFor<glVertexAttrib1fvARB>()(index, v);
        }
        public void VertexAttrib1ARB(uint index, short x)
        {
            GetDelegateFor<glVertexAttrib1sARB>()(index, x);
        }
        public void VertexAttrib1ARB(uint index, short[] v)
        {
            GetDelegateFor<glVertexAttrib1svARB>()(index, v);
        }
        public void VertexAttrib2ARB(uint index, double x, double y)
        {
            GetDelegateFor<glVertexAttrib2dARB>()(index, x, y);
        }
        public void VertexAttrib2ARB(uint index, double[] v)
        {
            GetDelegateFor<glVertexAttrib2dvARB>()(index, v);
        }
        public void VertexAttrib2ARB(uint index, float x, float y)
        {
            GetDelegateFor<glVertexAttrib2fARB>()(index, x, y);
        }
        public void VertexAttrib2ARB(uint index, float[] v)
        {
            GetDelegateFor<glVertexAttrib2fvARB>()(index, v);
        }
        public void VertexAttrib2ARB(uint index, short x, short y)
        {
            GetDelegateFor<glVertexAttrib2sARB>()(index, x, y);
        }
        public void VertexAttrib2ARB(uint index, short[] v)
        {
            GetDelegateFor<glVertexAttrib2svARB>()(index, v);
        }
        public void VertexAttrib3ARB(uint index, double x, double y, double z)
        {
            GetDelegateFor<glVertexAttrib3dARB>()(index, x, y, z);
        }
        public void VertexAttrib3ARB(uint index, double[] v)
        {
            GetDelegateFor<glVertexAttrib3dvARB>()(index, v);
        }
        public void VertexAttrib3ARB(uint index, float x, float y, float z)
        {
            GetDelegateFor<glVertexAttrib3fARB>()(index, x, y, z);
        }
        public void VertexAttrib3ARB(uint index, float[] v)
        {
            GetDelegateFor<glVertexAttrib3fvARB>()(index, v);
        }
        public void VertexAttrib3ARB(uint index, short x, short y, short z)
        {
            GetDelegateFor<glVertexAttrib3sARB>()(index, x, y, z);
        }
        public void VertexAttrib3ARB(uint index, short[] v)
        {
            GetDelegateFor<glVertexAttrib3svARB>()(index, v);
        }
        public void VertexAttrib4NARB(uint index, sbyte[] v)
        {
            GetDelegateFor<glVertexAttrib4NbvARB>()(index, v);
        }
        public void VertexAttrib4NARB(uint index, int[] v)
        {
            GetDelegateFor<glVertexAttrib4NivARB>()(index, v);
        }
        public void VertexAttrib4NARB(uint index, short[] v)
        {
            GetDelegateFor<glVertexAttrib4NsvARB>()(index, v);
        }
        public void VertexAttrib4NARB(uint index, byte x, byte y, byte z, byte w)
        {
            GetDelegateFor<glVertexAttrib4NubARB>()(index, x, y, z, w);
        }
        public void VertexAttrib4NARB(uint index, byte[] v)
        {
            GetDelegateFor<glVertexAttrib4NubvARB>()(index, v);
        }
        public void VertexAttrib4NARB(uint index, uint[] v)
        {
            GetDelegateFor<glVertexAttrib4NuivARB>()(index, v);
        }
        public void VertexAttrib4NARB(uint index, ushort[] v)
        {
            GetDelegateFor<glVertexAttrib4NusvARB>()(index, v);
        }
        public void VertexAttrib4ARB(uint index, sbyte[] v)
        {
            GetDelegateFor<glVertexAttrib4bvARB>()(index, v);
        }
        public void VertexAttrib4ARB(uint index, double x, double y, double z, double w)
        {
            GetDelegateFor<glVertexAttrib4dARB>()(index, x, y, z, w);
        }
        public void VertexAttrib4ARB(uint index, double[] v)
        {
            GetDelegateFor<glVertexAttrib4dvARB>()(index, v);
        }
        public void VertexAttrib4ARB(uint index, float x, float y, float z, float w)
        {
            GetDelegateFor<glVertexAttrib4fARB>()(index, x, y, z, w);
        }
        public void VertexAttrib4ARB(uint index, float[] v)
        {
            GetDelegateFor<glVertexAttrib4fvARB>()(index, v);
        }
        public void VertexAttrib4ARB(uint index, int[] v)
        {
            GetDelegateFor<glVertexAttrib4ivARB>()(index, v);
        }
        public void VertexAttrib4ARB(uint index, short x, short y, short z, short w)
        {
            GetDelegateFor<glVertexAttrib4sARB>()(index, x, y, z, w);
        }
        public void VertexAttrib4ARB(uint index, short[] v)
        {
            GetDelegateFor<glVertexAttrib4svARB>()(index, v);
        }
        public void VertexAttrib4ARB(uint index, byte[] v)
        {
            GetDelegateFor<glVertexAttrib4ubvARB>()(index, v);
        }
        public void VertexAttrib4ARB(uint index, uint[] v)
        {
            GetDelegateFor<glVertexAttrib4uivARB>()(index, v);
        }
        public void VertexAttrib4ARB(uint index, ushort[] v)
        {
            GetDelegateFor<glVertexAttrib4usvARB>()(index, v);
        }
        public void VertexAttribPointerARB(uint index, int size, uint type, bool normalized, int stride, IntPtr pointer)
        {
            GetDelegateFor<glVertexAttribPointerARB>()(index, size, type, normalized, stride, pointer);
        }
        public void EnableVertexAttribArrayARB(uint index)
        {
            GetDelegateFor<glEnableVertexAttribArrayARB>()(index);
        }
        public void DisableVertexAttribArrayARB(uint index)
        {
            GetDelegateFor<glDisableVertexAttribArrayARB>()(index);
        }
        public void ProgramStringARB(uint target, uint format, int len, IntPtr str)
        {
            GetDelegateFor<glProgramStringARB>()(target, format, len, str);
        }
        public void BindProgramARB(uint target, uint program)
        {
            GetDelegateFor<glBindProgramARB>()(target, program);
        }
        public void DeleteProgramsARB(int n, uint[] programs)
        {
            GetDelegateFor<glDeleteProgramsARB>()(n, programs);
        }
        public void GenProgramsARB(int n, uint[] programs)
        {
            GetDelegateFor<glGenProgramsARB>()(n, programs);
        }
        public void ProgramEnvParameter4ARB(uint target, uint index, double x, double y, double z, double w)
        {
            GetDelegateFor<glProgramEnvParameter4dARB>()(target, index, x, y, z, w);
        }
        public void ProgramEnvParameter4ARB(uint target, uint index, double[] parameters)
        {
            GetDelegateFor<glProgramEnvParameter4dvARB>()(target, index, parameters);
        }
        public void ProgramEnvParameter4ARB(uint target, uint index, float x, float y, float z, float w)
        {
            GetDelegateFor<glProgramEnvParameter4fARB>()(target, index, x, y, z, w);
        }
        public void ProgramEnvParameter4ARB(uint target, uint index, float[] parameters)
        {
            GetDelegateFor<glProgramEnvParameter4fvARB>()(target, index, parameters);
        }
        public void ProgramLocalParameter4ARB(uint target, uint index, double x, double y, double z, double w)
        {
            GetDelegateFor<glProgramLocalParameter4dARB>()(target, index, x, y, z, w);
        }
        public void ProgramLocalParameter4ARB(uint target, uint index, double[] parameters)
        {
            GetDelegateFor<glProgramLocalParameter4dvARB>()(target, index, parameters);
        }
        public void ProgramLocalParameter4ARB(uint target, uint index, float x, float y, float z, float w)
        {
            GetDelegateFor<glProgramLocalParameter4fARB>()(target, index, x, y, z, w);
        }
        public void ProgramLocalParameter4ARB(uint target, uint index, float[] parameters)
        {
            GetDelegateFor<glProgramLocalParameter4fvARB>()(target, index, parameters);
        }
        public void GetProgramEnvParameterdARB(uint target, uint index, double[] parameters)
        {
            GetDelegateFor<glGetProgramEnvParameterdvARB>()(target, index, parameters);
        }
        public void GetProgramEnvParameterfARB(uint target, uint index, float[] parameters)
        {
            GetDelegateFor<glGetProgramEnvParameterfvARB>()(target, index, parameters);
        }
        public void GetProgramLocalParameterARB(uint target, uint index, double[] parameters)
        {
            GetDelegateFor<glGetProgramLocalParameterdvARB>()(target, index, parameters);
        }
        public void GetProgramLocalParameterARB(uint target, uint index, float[] parameters)
        {
            GetDelegateFor<glGetProgramLocalParameterfvARB>()(target, index, parameters);
        }
        public void GetProgramARB(uint target, uint pname, int[] parameters)
        {
            GetDelegateFor<glGetProgramivARB>()(target, pname, parameters);
        }
        public void GetProgramStringARB(uint target, uint pname, IntPtr str)
        {
            GetDelegateFor<glGetProgramStringARB>()(target, pname, str);
        }
        public void GetVertexAttribARB(uint index, uint pname, double[] parameters)
        {
            GetDelegateFor<glGetVertexAttribdvARB>()(index, pname, parameters);
        }
        public void GetVertexAttribARB(uint index, uint pname, float[] parameters)
        {
            GetDelegateFor<glGetVertexAttribfvARB>()(index, pname, parameters);
        }
        public void GetVertexAttribARB(uint index, uint pname, int[] parameters)
        {
            GetDelegateFor<glGetVertexAttribivARB>()(index, pname, parameters);
        }
        public void GetVertexAttribPointerARB(uint index, uint pname, IntPtr pointer)
        {
            GetDelegateFor<glGetVertexAttribPointervARB>()(index, pname, pointer);
        }

        //  Delegates
        private delegate void glVertexAttrib1dARB(uint index, double x);
        private delegate void glVertexAttrib1dvARB(uint index, double[] v);
        private delegate void glVertexAttrib1fARB(uint index, float x);
        private delegate void glVertexAttrib1fvARB(uint index, float[] v);
        private delegate void glVertexAttrib1sARB(uint index, short x);
        private delegate void glVertexAttrib1svARB(uint index, short[] v);
        private delegate void glVertexAttrib2dARB(uint index, double x, double y);
        private delegate void glVertexAttrib2dvARB(uint index, double[] v);
        private delegate void glVertexAttrib2fARB(uint index, float x, float y);
        private delegate void glVertexAttrib2fvARB(uint index, float[] v);
        private delegate void glVertexAttrib2sARB(uint index, short x, short y);
        private delegate void glVertexAttrib2svARB(uint index, short[] v);
        private delegate void glVertexAttrib3dARB(uint index, double x, double y, double z);
        private delegate void glVertexAttrib3dvARB(uint index, double[] v);
        private delegate void glVertexAttrib3fARB(uint index, float x, float y, float z);
        private delegate void glVertexAttrib3fvARB(uint index, float[] v);
        private delegate void glVertexAttrib3sARB(uint index, short x, short y, short z);
        private delegate void glVertexAttrib3svARB(uint index, short[] v);
        private delegate void glVertexAttrib4NbvARB(uint index, sbyte[] v);
        private delegate void glVertexAttrib4NivARB(uint index, int[] v);
        private delegate void glVertexAttrib4NsvARB(uint index, short[] v);
        private delegate void glVertexAttrib4NubARB(uint index, byte x, byte y, byte z, byte w);
        private delegate void glVertexAttrib4NubvARB(uint index, byte[] v);
        private delegate void glVertexAttrib4NuivARB(uint index, uint[] v);
        private delegate void glVertexAttrib4NusvARB(uint index, ushort[] v);
        private delegate void glVertexAttrib4bvARB(uint index, sbyte[] v);
        private delegate void glVertexAttrib4dARB(uint index, double x, double y, double z, double w);
        private delegate void glVertexAttrib4dvARB(uint index, double[] v);
        private delegate void glVertexAttrib4fARB(uint index, float x, float y, float z, float w);
        private delegate void glVertexAttrib4fvARB(uint index, float[] v);
        private delegate void glVertexAttrib4ivARB(uint index, int[] v);
        private delegate void glVertexAttrib4sARB(uint index, short x, short y, short z, short w);
        private delegate void glVertexAttrib4svARB(uint index, short[] v);
        private delegate void glVertexAttrib4ubvARB(uint index, byte[] v);
        private delegate void glVertexAttrib4uivARB(uint index, uint[] v);
        private delegate void glVertexAttrib4usvARB(uint index, ushort[] v);
        private delegate void glVertexAttribPointerARB(uint index, int size, uint type, bool normalized, int stride, IntPtr pointer);
        private delegate void glEnableVertexAttribArrayARB(uint index);
        private delegate void glDisableVertexAttribArrayARB(uint index);
        private delegate void glProgramStringARB(uint target, uint format, int len, IntPtr str);
        private delegate void glBindProgramARB(uint target, uint program);
        private delegate void glDeleteProgramsARB(int n, uint[] programs);
        private delegate void glGenProgramsARB(int n, uint[] programs);
        private delegate void glProgramEnvParameter4dARB(uint target, uint index, double x, double y, double z, double w);
        private delegate void glProgramEnvParameter4dvARB(uint target, uint index, double[] parameters);
        private delegate void glProgramEnvParameter4fARB(uint target, uint index, float x, float y, float z, float w);
        private delegate void glProgramEnvParameter4fvARB(uint target, uint index, float[] parameters);
        private delegate void glProgramLocalParameter4dARB(uint target, uint index, double x, double y, double z, double w);
        private delegate void glProgramLocalParameter4dvARB(uint target, uint index, double[] parameters);
        private delegate void glProgramLocalParameter4fARB(uint target, uint index, float x, float y, float z, float w);
        private delegate void glProgramLocalParameter4fvARB(uint target, uint index, float[] parameters);
        private delegate void glGetProgramEnvParameterdvARB(uint target, uint index, double[] parameters);
        private delegate void glGetProgramEnvParameterfvARB(uint target, uint index, float[] parameters);
        private delegate void glGetProgramLocalParameterdvARB(uint target, uint index, double[] parameters);
        private delegate void glGetProgramLocalParameterfvARB(uint target, uint index, float[] parameters);
        private delegate void glGetProgramivARB(uint target, uint pname, int[] parameters);
        private delegate void glGetProgramStringARB(uint target, uint pname, IntPtr str);
        private delegate void glGetVertexAttribdvARB(uint index, uint pname, double[] parameters);
        private delegate void glGetVertexAttribfvARB(uint index, uint pname, float[] parameters);
        private delegate void glGetVertexAttribivARB(uint index, uint pname, int[] parameters);
        private delegate void glGetVertexAttribPointervARB(uint index, uint pname, IntPtr pointer);

        //  Constants
        public const uint GL_COLOR_SUM_ARB = 0x8458;
        public const uint GL_VERTEX_PROGRAM_ARB = 0x8620;
        public const uint GL_VERTEX_ATTRIB_ARRAY_ENABLED_ARB = 0x8622;
        public const uint GL_VERTEX_ATTRIB_ARRAY_SIZE_ARB = 0x8623;
        public const uint GL_VERTEX_ATTRIB_ARRAY_STRIDE_ARB = 0x8624;
        public const uint GL_VERTEX_ATTRIB_ARRAY_TYPE_ARB = 0x8625;
        public const uint GL_CURRENT_VERTEX_ATTRIB_ARB = 0x8626;
        public const uint GL_PROGRAM_LENGTH_ARB = 0x8627;
        public const uint GL_PROGRAM_STRING_ARB = 0x8628;
        public const uint GL_MAX_PROGRAM_MATRIX_STACK_DEPTH_ARB = 0x862E;
        public const uint GL_MAX_PROGRAM_MATRICES_ARB = 0x862F;
        public const uint GL_CURRENT_MATRIX_STACK_DEPTH_ARB = 0x8640;
        public const uint GL_CURRENT_MATRIX_ARB = 0x8641;
        public const uint GL_VERTEX_PROGRAM_POINT_SIZE_ARB = 0x8642;
        public const uint GL_VERTEX_PROGRAM_TWO_SIDE_ARB = 0x8643;
        public const uint GL_VERTEX_ATTRIB_ARRAY_POINTER_ARB = 0x8645;
        public const uint GL_PROGRAM_ERROR_POSITION_ARB = 0x864B;
        public const uint GL_PROGRAM_BINDING_ARB = 0x8677;
        public const uint GL_MAX_VERTEX_ATTRIBS_ARB = 0x8869;
        public const uint GL_VERTEX_ATTRIB_ARRAY_NORMALIZED_ARB = 0x886A;
        public const uint GL_PROGRAM_ERROR_STRING_ARB = 0x8874;
        public const uint GL_PROGRAM_FORMAT_ASCII_ARB = 0x8875;
        public const uint GL_PROGRAM_FORMAT_ARB = 0x8876;
        public const uint GL_PROGRAM_INSTRUCTIONS_ARB = 0x88A0;
        public const uint GL_MAX_PROGRAM_INSTRUCTIONS_ARB = 0x88A1;
        public const uint GL_PROGRAM_NATIVE_INSTRUCTIONS_ARB = 0x88A2;
        public const uint GL_MAX_PROGRAM_NATIVE_INSTRUCTIONS_ARB = 0x88A3;
        public const uint GL_PROGRAM_TEMPORARIES_ARB = 0x88A4;
        public const uint GL_MAX_PROGRAM_TEMPORARIES_ARB = 0x88A5;
        public const uint GL_PROGRAM_NATIVE_TEMPORARIES_ARB = 0x88A6;
        public const uint GL_MAX_PROGRAM_NATIVE_TEMPORARIES_ARB = 0x88A7;
        public const uint GL_PROGRAM_PARAMETERS_ARB = 0x88A8;
        public const uint GL_MAX_PROGRAM_PARAMETERS_ARB = 0x88A9;
        public const uint GL_PROGRAM_NATIVE_PARAMETERS_ARB = 0x88AA;
        public const uint GL_MAX_PROGRAM_NATIVE_PARAMETERS_ARB = 0x88AB;
        public const uint GL_PROGRAM_ATTRIBS_ARB = 0x88AC;
        public const uint GL_MAX_PROGRAM_ATTRIBS_ARB = 0x88AD;
        public const uint GL_PROGRAM_NATIVE_ATTRIBS_ARB = 0x88AE;
        public const uint GL_MAX_PROGRAM_NATIVE_ATTRIBS_ARB = 0x88AF;
        public const uint GL_PROGRAM_ADDRESS_REGISTERS_ARB = 0x88B0;
        public const uint GL_MAX_PROGRAM_ADDRESS_REGISTERS_ARB = 0x88B1;
        public const uint GL_PROGRAM_NATIVE_ADDRESS_REGISTERS_ARB = 0x88B2;
        public const uint GL_MAX_PROGRAM_NATIVE_ADDRESS_REGISTERS_ARB = 0x88B3;
        public const uint GL_MAX_PROGRAM_LOCAL_PARAMETERS_ARB = 0x88B4;
        public const uint GL_MAX_PROGRAM_ENV_PARAMETERS_ARB = 0x88B5;
        public const uint GL_PROGRAM_UNDER_NATIVE_LIMITS_ARB = 0x88B6;
        public const uint GL_TRANSPOSE_CURRENT_MATRIX_ARB = 0x88B7;
        public const uint GL_MATRIX0_ARB = 0x88C0;
        public const uint GL_MATRIX1_ARB = 0x88C1;
        public const uint GL_MATRIX2_ARB = 0x88C2;
        public const uint GL_MATRIX3_ARB = 0x88C3;
        public const uint GL_MATRIX4_ARB = 0x88C4;
        public const uint GL_MATRIX5_ARB = 0x88C5;
        public const uint GL_MATRIX6_ARB = 0x88C6;
        public const uint GL_MATRIX7_ARB = 0x88C7;
        public const uint GL_MATRIX8_ARB = 0x88C8;
        public const uint GL_MATRIX9_ARB = 0x88C9;
        public const uint GL_MATRIX10_ARB = 0x88CA;
        public const uint GL_MATRIX11_ARB = 0x88CB;
        public const uint GL_MATRIX12_ARB = 0x88CC;
        public const uint GL_MATRIX13_ARB = 0x88CD;
        public const uint GL_MATRIX14_ARB = 0x88CE;
        public const uint GL_MATRIX15_ARB = 0x88CF;
        public const uint GL_MATRIX16_ARB = 0x88D0;
        public const uint GL_MATRIX17_ARB = 0x88D1;
        public const uint GL_MATRIX18_ARB = 0x88D2;
        public const uint GL_MATRIX19_ARB = 0x88D3;
        public const uint GL_MATRIX20_ARB = 0x88D4;
        public const uint GL_MATRIX21_ARB = 0x88D5;
        public const uint GL_MATRIX22_ARB = 0x88D6;
        public const uint GL_MATRIX23_ARB = 0x88D7;
        public const uint GL_MATRIX24_ARB = 0x88D8;
        public const uint GL_MATRIX25_ARB = 0x88D9;
        public const uint GL_MATRIX26_ARB = 0x88DA;
        public const uint GL_MATRIX27_ARB = 0x88DB;
        public const uint GL_MATRIX28_ARB = 0x88DC;
        public const uint GL_MATRIX29_ARB = 0x88DD;
        public const uint GL_MATRIX30_ARB = 0x88DE;
        public const uint GL_MATRIX31_ARB = 0x88DF;

        #endregion

        #region GL_ARB_vertex_shader

        //  Methods
        public void BindAttribLocationARB(uint programObj, uint index, string name)
        {
            GetDelegateFor<glBindAttribLocationARB>()(programObj, index, name);
        }
        public void GetActiveAttribARB(uint programObj, uint index, int maxLength, int[] length, int[] size, uint[] type, string name)
        {
            GetDelegateFor<glGetActiveAttribARB>()(programObj, index, maxLength, length, size, type, name);
        }
        public uint GetAttribLocationARB(uint programObj, string name)
        {
            return (uint)GetDelegateFor<glGetAttribLocationARB>()(programObj, name);
        }

        //  Delegates
        private delegate void glBindAttribLocationARB(uint programObj, uint index, string name);
        private delegate void glGetActiveAttribARB(uint programObj, uint index, int maxLength, int[] length, int[] size, uint[] type, string name);
        private delegate uint glGetAttribLocationARB(uint programObj, string name);

        //  Constants
        public const uint GL_VERTEX_SHADER_ARB = 0x8B31;
        public const uint GL_MAX_VERTEX_UNIFORM_COMPONENTS_ARB = 0x8B4A;
        public const uint GL_MAX_VARYING_FLOATS_ARB = 0x8B4B;
        public const uint GL_MAX_VERTEX_TEXTURE_IMAGE_UNITS_ARB = 0x8B4C;
        public const uint GL_MAX_COMBINED_TEXTURE_IMAGE_UNITS_ARB = 0x8B4D;
        public const uint GL_OBJECT_ACTIVE_ATTRIBUTES_ARB = 0x8B89;
        public const uint GL_OBJECT_ACTIVE_ATTRIBUTE_MAX_LENGTH_ARB = 0x8B8A;

        #endregion

        #region GL_ARB_fragment_shader

        public const uint GL_FRAGMENT_SHADER_ARB = 0x8B30;
        public const uint GL_MAX_FRAGMENT_UNIFORM_COMPONENTS_ARB = 0x8B49;
        public const uint GL_FRAGMENT_SHADER_DERIVATIVE_HINT_ARB = 0x8B8B;

        #endregion

        #region GL_ARB_draw_buffers

        //  Methods
        public void DrawBuffersARB(int n, uint[] bufs)
        {
            GetDelegateFor<glDrawBuffersARB>()(n, bufs);
        }

        //  Delegates
        private delegate void glDrawBuffersARB(int n, uint[] bufs);

        //  Constants        
        public const uint GL_MAX_DRAW_BUFFERS_ARB = 0x8824;
        public const uint GL_DRAW_BUFFER0_ARB = 0x8825;
        public const uint GL_DRAW_BUFFER1_ARB = 0x8826;
        public const uint GL_DRAW_BUFFER2_ARB = 0x8827;
        public const uint GL_DRAW_BUFFER3_ARB = 0x8828;
        public const uint GL_DRAW_BUFFER4_ARB = 0x8829;
        public const uint GL_DRAW_BUFFER5_ARB = 0x882A;
        public const uint GL_DRAW_BUFFER6_ARB = 0x882B;
        public const uint GL_DRAW_BUFFER7_ARB = 0x882C;
        public const uint GL_DRAW_BUFFER8_ARB = 0x882D;
        public const uint GL_DRAW_BUFFER9_ARB = 0x882E;
        public const uint GL_DRAW_BUFFER10_ARB = 0x882F;
        public const uint GL_DRAW_BUFFER11_ARB = 0x8830;
        public const uint GL_DRAW_BUFFER12_ARB = 0x8831;
        public const uint GL_DRAW_BUFFER13_ARB = 0x8832;
        public const uint GL_DRAW_BUFFER14_ARB = 0x8833;
        public const uint GL_DRAW_BUFFER15_ARB = 0x8834;

        #endregion

        #region GL_ARB_texture_non_power_of_two

        //  No methods or constants

        #endregion

        #region GL_ARB_texture_rectangle

        //  Constants
        public const uint GL_TEXTURE_RECTANGLE_ARB = 0x84F5;
        public const uint GL_TEXTURE_BINDING_RECTANGLE_ARB = 0x84F6;
        public const uint GL_PROXY_TEXTURE_RECTANGLE_ARB = 0x84F7;
        public const uint GL_MAX_RECTANGLE_TEXTURE_SIZE_ARB = 0x84F8;

        #endregion

        #region GL_ARB_point_sprite

        //  Constants
        public const uint GL_POINT_SPRITE_ARB = 0x8861;
        public const uint GL_COORD_REPLACE_ARB = 0x8862;

        #endregion

        #region GL_ARB_texture_float

        //  Constants
        public const uint GL_TEXTURE_RED_TYPE_ARB = 0x8C10;
        public const uint GL_TEXTURE_GREEN_TYPE_ARB = 0x8C11;
        public const uint GL_TEXTURE_BLUE_TYPE_ARB = 0x8C12;
        public const uint GL_TEXTURE_ALPHA_TYPE_ARB = 0x8C13;
        public const uint GL_TEXTURE_LUMINANCE_TYPE_ARB = 0x8C14;
        public const uint GL_TEXTURE_INTENSITY_TYPE_ARB = 0x8C15;
        public const uint GL_TEXTURE_DEPTH_TYPE_ARB = 0x8C16;
        public const uint GL_UNSIGNED_NORMALIZED_ARB = 0x8C17;
        public const uint GL_RGBA32F_ARB = 0x8814;
        public const uint GL_RGB32F_ARB = 0x8815;
        public const uint GL_ALPHA32F_ARB = 0x8816;
        public const uint GL_INTENSITY32F_ARB = 0x8817;
        public const uint GL_LUMINANCE32F_ARB = 0x8818;
        public const uint GL_LUMINANCE_ALPHA32F_ARB = 0x8819;
        public const uint GL_RGBA16F_ARB = 0x881A;
        public const uint GL_RGB16F_ARB = 0x881B;
        public const uint GL_ALPHA16F_ARB = 0x881C;
        public const uint GL_INTENSITY16F_ARB = 0x881D;
        public const uint GL_LUMINANCE16F_ARB = 0x881E;
        public const uint GL_LUMINANCE_ALPHA16F_ARB = 0x881F;

        #endregion

        #region GL_EXT_blend_equation_separate

        //  Methods
        public void BlendEquationSeparateEXT(uint modeRGB, uint modeAlpha)
        {
            // GetDelegateFor<glBlendEquationEXT>()(modeRGB, modeAlpha);
            GetDelegateFor<glBlendEquationEXT>()(modeRGB);
        }

        //  Delegates
        private delegate void glBlendEquationSeparateEXT(uint modeRGB, uint modeAlpha);

        //  Constants
        public const uint GL_BLEND_EQUATION_RGB_EXT = 0x8009;
        public const uint GL_BLEND_EQUATION_ALPHA_EXT = 0x883D;

        #endregion

        #region GL_EXT_stencil_two_side

        //  Methods
        public void ActiveStencilFaceEXT(uint face)
        {
            GetDelegateFor<glActiveStencilFaceEXT>()(face);
        }

        //  Delegates
        private delegate void glActiveStencilFaceEXT(uint face);

        //  Constants
        public const uint GL_STENCIL_TEST_TWO_SIDE_EXT = 0x8009;
        public const uint GL_ACTIVE_STENCIL_FACE_EXT = 0x883D;

        #endregion

        #region GL_ARB_pixel_buffer_object

        public const uint GL_PIXEL_PACK_BUFFER_ARB = 0x88EB;
        public const uint GL_PIXEL_UNPACK_BUFFER_ARB = 0x88EC;
        public const uint GL_PIXEL_PACK_BUFFER_BINDING_ARB = 0x88ED;
        public const uint GL_PIXEL_UNPACK_BUFFER_BINDING_ARB = 0x88EF;

        #endregion

        #region GL_EXT_texture_sRGB

        public const uint GL_SRGB_EXT = 0x8C40;
        public const uint GL_SRGB8_EXT = 0x8C41;
        public const uint GL_SRGB_ALPHA_EXT = 0x8C42;
        public const uint GL_SRGB8_ALPHA8_EXT = 0x8C43;
        public const uint GL_SLUMINANCE_ALPHA_EXT = 0x8C44;
        public const uint GL_SLUMINANCE8_ALPHA8_EXT = 0x8C45;
        public const uint GL_SLUMINANCE_EXT = 0x8C46;
        public const uint GL_SLUMINANCE8_EXT = 0x8C47;
        public const uint GL_COMPRESSED_SRGB_EXT = 0x8C48;
        public const uint GL_COMPRESSED_SRGB_ALPHA_EXT = 0x8C49;
        public const uint GL_COMPRESSED_SLUMINANCE_EXT = 0x8C4A;
        public const uint GL_COMPRESSED_SLUMINANCE_ALPHA_EXT = 0x8C4B;
        public const uint GL_COMPRESSED_SRGB_S3TC_DXT1_EXT = 0x8C4C;
        public const uint GL_COMPRESSED_SRGB_ALPHA_S3TC_DXT1_EXT = 0x8C4D;
        public const uint GL_COMPRESSED_SRGB_ALPHA_S3TC_DXT3_EXT = 0x8C4E;
        public const uint GL_COMPRESSED_SRGB_ALPHA_S3TC_DXT5_EXT = 0x8C4F;

        #endregion

        #region GL_EXT_framebuffer_object

        //  Methods
        public bool IsRenderbufferEXT(uint renderbuffer)
        {
            return (bool)GetDelegateFor<glIsRenderbufferEXT>()(renderbuffer);
        }

        public void BindRenderbufferEXT(uint target, uint renderbuffer)
        {
            GetDelegateFor<glBindRenderbufferEXT>()(target, renderbuffer);
        }

        public void DeleteRenderbuffersEXT(uint n, uint[] renderbuffers)
        {
            GetDelegateFor<glDeleteRenderbuffersEXT>()(n, renderbuffers);
        }

        public void GenRenderbuffersEXT(uint n, uint[] renderbuffers)
        {
            GetDelegateFor<glGenRenderbuffersEXT>()(n, renderbuffers);
        }

        public void RenderbufferStorageEXT(uint target, uint internalformat, int width, int height)
        {
            GetDelegateFor<glRenderbufferStorageEXT>()(target, internalformat, width, height);
        }

        public void GetRenderbufferParameterivEXT(uint target, uint pname, int[] parameters)
        {
            GetDelegateFor<glGetRenderbufferParameterivEXT>()(target, pname, parameters);
        }

        public bool IsFramebufferEXT(uint framebuffer)
        {
            return (bool)GetDelegateFor<glIsFramebufferEXT>()(framebuffer);
        }

        public void BindFramebufferEXT(uint target, uint framebuffer)
        {
            GetDelegateFor<glBindFramebufferEXT>()(target, framebuffer);
        }

        public void DeleteFramebuffersEXT(uint n, uint[] framebuffers)
        {
            GetDelegateFor<glDeleteFramebuffersEXT>()(n, framebuffers);
        }

        public void GenFramebuffersEXT(uint n, uint[] framebuffers)
        {
            GetDelegateFor<glGenFramebuffersEXT>()(n, framebuffers);
        }

        public uint CheckFramebufferStatusEXT(uint target)
        {
            return (uint)GetDelegateFor<glCheckFramebufferStatusEXT>()(target);
        }

        public void FramebufferTexture1DEXT(uint target, uint attachment, uint textarget, uint texture, int level)
        {
            GetDelegateFor<glFramebufferTexture1DEXT>()(target, attachment, textarget, texture, level);
        }

        public void FramebufferTexture2DEXT(uint target, uint attachment, uint textarget, uint texture, int level)
        {
            GetDelegateFor<glFramebufferTexture2DEXT>()(target, attachment, textarget, texture, level);
        }

        public void FramebufferTexture3DEXT(uint target, uint attachment, uint textarget, uint texture, int level, int zoffset)
        {
            GetDelegateFor<glFramebufferTexture3DEXT>()(target, attachment, textarget, texture, level, zoffset);
        }

        public void FramebufferRenderbufferEXT(uint target, uint attachment, uint renderbuffertarget, uint renderbuffer)
        {
            GetDelegateFor<glFramebufferRenderbufferEXT>()(target, attachment, renderbuffertarget, renderbuffer);
        }

        public void GetFramebufferAttachmentParameterivEXT(uint target, uint attachment, uint pname, int[] parameters)
        {
            GetDelegateFor<glGetFramebufferAttachmentParameterivEXT>()(target, attachment, pname, parameters);
        }

        public void GenerateMipmapEXT(uint target)
        {
            GetDelegateFor<glGenerateMipmapEXT>()(target);
        }

        //  Delegates
        private delegate bool glIsRenderbufferEXT(uint renderbuffer);
        private delegate void glBindRenderbufferEXT(uint target, uint renderbuffer);
        private delegate void glDeleteRenderbuffersEXT(uint n, uint[] renderbuffers);
        private delegate void glGenRenderbuffersEXT(uint n, uint[] renderbuffers);
        private delegate void glRenderbufferStorageEXT(uint target, uint internalformat, int width, int height);
        private delegate void glGetRenderbufferParameterivEXT(uint target, uint pname, int[] parameters);
        private delegate bool glIsFramebufferEXT(uint framebuffer);
        private delegate void glBindFramebufferEXT(uint target, uint framebuffer);
        private delegate void glDeleteFramebuffersEXT(uint n, uint[] framebuffers);
        private delegate void glGenFramebuffersEXT(uint n, uint[] framebuffers);
        private delegate uint glCheckFramebufferStatusEXT(uint target);
        private delegate void glFramebufferTexture1DEXT(uint target, uint attachment, uint textarget, uint texture, int level);
        private delegate void glFramebufferTexture2DEXT(uint target, uint attachment, uint textarget, uint texture, int level);
        private delegate void glFramebufferTexture3DEXT(uint target, uint attachment, uint textarget, uint texture, int level, int zoffset);
        private delegate void glFramebufferRenderbufferEXT(uint target, uint attachment, uint renderbuffertarget, uint renderbuffer);
        private delegate void glGetFramebufferAttachmentParameterivEXT(uint target, uint attachment, uint pname, int[] parameters);
        private delegate void glGenerateMipmapEXT(uint target);

        //  Constants
        public const uint GL_INVALID_FRAMEBUFFER_OPERATION_EXT = 0x0506;
        public const uint GL_MAX_RENDERBUFFER_SIZE_EXT = 0x84E8;
        public const uint GL_FRAMEBUFFER_BINDING_EXT = 0x8CA6;
        public const uint GL_RENDERBUFFER_BINDING_EXT = 0x8CA7;
        public const uint GL_FRAMEBUFFER_ATTACHMENT_OBJECT_TYPE_EXT = 0x8CD0;
        public const uint GL_FRAMEBUFFER_ATTACHMENT_OBJECT_NAME_EXT = 0x8CD1;
        public const uint GL_FRAMEBUFFER_ATTACHMENT_TEXTURE_LEVEL_EXT = 0x8CD2;
        public const uint GL_FRAMEBUFFER_ATTACHMENT_TEXTURE_CUBE_MAP_FACE_EXT = 0x8CD3;
        public const uint GL_FRAMEBUFFER_ATTACHMENT_TEXTURE_3D_ZOFFSET_EXT = 0x8CD4;
        public const uint GL_FRAMEBUFFER_COMPLETE_EXT = 0x8CD5;
        public const uint GL_FRAMEBUFFER_INCOMPLETE_ATTACHMENT_EXT = 0x8CD6;
        public const uint GL_FRAMEBUFFER_INCOMPLETE_MISSING_ATTACHMENT_EXT = 0x8CD7;
        public const uint GL_FRAMEBUFFER_INCOMPLETE_DIMENSIONS_EXT = 0x8CD9;
        public const uint GL_FRAMEBUFFER_INCOMPLETE_FORMATS_EXT = 0x8CDA;
        public const uint GL_FRAMEBUFFER_INCOMPLETE_DRAW_BUFFER_EXT = 0x8CDB;
        public const uint GL_FRAMEBUFFER_INCOMPLETE_READ_BUFFER_EXT = 0x8CDC;
        public const uint GL_FRAMEBUFFER_UNSUPPORTED_EXT = 0x8CDD;
        public const uint GL_MAX_COLOR_ATTACHMENTS_EXT = 0x8CDF;
        public const uint GL_COLOR_ATTACHMENT0_EXT = 0x8CE0;
        public const uint GL_COLOR_ATTACHMENT1_EXT = 0x8CE1;
        public const uint GL_COLOR_ATTACHMENT2_EXT = 0x8CE2;
        public const uint GL_COLOR_ATTACHMENT3_EXT = 0x8CE3;
        public const uint GL_COLOR_ATTACHMENT4_EXT = 0x8CE4;
        public const uint GL_COLOR_ATTACHMENT5_EXT = 0x8CE5;
        public const uint GL_COLOR_ATTACHMENT6_EXT = 0x8CE6;
        public const uint GL_COLOR_ATTACHMENT7_EXT = 0x8CE7;
        public const uint GL_COLOR_ATTACHMENT8_EXT = 0x8CE8;
        public const uint GL_COLOR_ATTACHMENT9_EXT = 0x8CE9;
        public const uint GL_COLOR_ATTACHMENT10_EXT = 0x8CEA;
        public const uint GL_COLOR_ATTACHMENT11_EXT = 0x8CEB;
        public const uint GL_COLOR_ATTACHMENT12_EXT = 0x8CEC;
        public const uint GL_COLOR_ATTACHMENT13_EXT = 0x8CED;
        public const uint GL_COLOR_ATTACHMENT14_EXT = 0x8CEE;
        public const uint GL_COLOR_ATTACHMENT15_EXT = 0x8CEF;
        public const uint GL_DEPTH_ATTACHMENT_EXT = 0x8D00;
        public const uint GL_STENCIL_ATTACHMENT_EXT = 0x8D20;
        public const uint GL_FRAMEBUFFER_EXT = 0x8D40;
        public const uint GL_RENDERBUFFER_EXT = 0x8D41;
        public const uint GL_RENDERBUFFER_WIDTH_EXT = 0x8D42;
        public const uint GL_RENDERBUFFER_HEIGHT_EXT = 0x8D43;
        public const uint GL_RENDERBUFFER_INTERNAL_FORMAT_EXT = 0x8D44;
        public const uint GL_STENCIL_INDEX1_EXT = 0x8D46;
        public const uint GL_STENCIL_INDEX4_EXT = 0x8D47;
        public const uint GL_STENCIL_INDEX8_EXT = 0x8D48;
        public const uint GL_STENCIL_INDEX16_EXT = 0x8D49;
        public const uint GL_RENDERBUFFER_RED_SIZE_EXT = 0x8D50;
        public const uint GL_RENDERBUFFER_GREEN_SIZE_EXT = 0x8D51;
        public const uint GL_RENDERBUFFER_BLUE_SIZE_EXT = 0x8D52;
        public const uint GL_RENDERBUFFER_ALPHA_SIZE_EXT = 0x8D53;
        public const uint GL_RENDERBUFFER_DEPTH_SIZE_EXT = 0x8D54;
        public const uint GL_RENDERBUFFER_STENCIL_SIZE_EXT = 0x8D55;

        #endregion

        #region GL_EXT_framebuffer_multisample

        //  Methods
        public void RenderbufferStorageMultisampleEXT(uint target, int samples, uint internalformat, int width, int height)
        {
            GetDelegateFor<glRenderbufferStorageMultisampleEXT>()(target, samples, internalformat, width, height);
        }

        //  Delegates
        private delegate void glRenderbufferStorageMultisampleEXT(uint target, int samples, uint internalformat, int width, int height);

        //  Constants
        public const uint GL_RENDERBUFFER_SAMPLES_EXT = 0x8CAB;
        public const uint GL_FRAMEBUFFER_INCOMPLETE_MULTISAMPLE_EXT = 0x8D56;
        public const uint GL_MAX_SAMPLES_EXT = 0x8D57;

        #endregion

        #region GL_EXT_draw_instanced

        //  Methods
        public void DrawArraysInstancedEXT(uint mode, int start, int count, int primcount)
        {
            GetDelegateFor<glDrawArraysInstancedEXT>()(mode, start, count, primcount);
        }
        public void DrawElementsInstancedEXT(uint mode, int count, uint type, IntPtr indices, int primcount)
        {
            GetDelegateFor<glDrawElementsInstancedEXT>()(mode, count, type, indices, primcount);
        }

        //  Delegates
        private delegate void glDrawArraysInstancedEXT(uint mode, int start, int count, int primcount);
        private delegate void glDrawElementsInstancedEXT(uint mode, int count, uint type, IntPtr indices, int primcount);

        #endregion

        #region GL_ARB_vertex_array_object

        //  Methods
        public void BindVertexArray(uint array)
        {
            GetDelegateFor<glBindVertexArray>()(array);
        }
        public void DeleteVertexArrays(int n, uint[] arrays)
        {
            GetDelegateFor<glDeleteVertexArrays>()(n, arrays);
        }
        public void GenVertexArrays(int n, uint[] arrays)
        {
            GetDelegateFor<glGenVertexArrays>()(n, arrays);
        }
        public bool IsVertexArray(uint array)
        {
            return (bool)GetDelegateFor<glIsVertexArray>()(array);
        }

        //  Delegates
        private delegate void glBindVertexArray(uint array);
        private delegate void glDeleteVertexArrays(int n, uint[] arrays);
        private delegate void glGenVertexArrays(int n, uint[] arrays);
        private delegate bool glIsVertexArray(uint array);

        //  Constants
        public const uint GL_VERTEX_ARRAY_BINDING = 0x85B5;

        #endregion

        #region GL_EXT_framebuffer_sRGB

        //  Constants
        public const uint GL_FRAMEBUFFER_SRGB_EXT = 0x8DB9;
        public const uint GL_FRAMEBUFFER_SRGB_CAPABLE_EXT = 0x8DBA;

        #endregion

        #region GGL_EXT_transform_feedback

        //  Methods
        public void BeginTransformFeedbackEXT(uint primitiveMode)
        {
            GetDelegateFor<glBeginTransformFeedbackEXT>()(primitiveMode);
        }
        public void EndTransformFeedbackEXT()
        {
            GetDelegateFor<glEndTransformFeedbackEXT>()();
        }
        public void BindBufferRangeEXT(uint target, uint index, uint buffer, int offset, int size)
        {
            GetDelegateFor<glBindBufferRangeEXT>()(target, index, buffer, offset, size);
        }
        public void BindBufferOffsetEXT(uint target, uint index, uint buffer, int offset)
        {
            GetDelegateFor<glBindBufferOffsetEXT>()(target, index, buffer, offset);
        }
        public void BindBufferBaseEXT(uint target, uint index, uint buffer)
        {
            GetDelegateFor<glBindBufferBaseEXT>()(target, index, buffer);
        }
        public void TransformFeedbackVaryingsEXT(uint program, int count, string[] varyings, uint bufferMode)
        {
            GetDelegateFor<glTransformFeedbackVaryingsEXT>()(program, count, varyings, bufferMode);
        }
        public void GetTransformFeedbackVaryingEXT(uint program, uint index, int bufSize, int[] length, int[] size, uint[] type, string name)
        {
            GetDelegateFor<glGetTransformFeedbackVaryingEXT>()(program, index, bufSize, length, size, type, name);
        }

        //  Delegates
        private delegate void glBeginTransformFeedbackEXT(uint primitiveMode);
        private delegate void glEndTransformFeedbackEXT ();
        private delegate void glBindBufferRangeEXT (uint target, uint index, uint buffer, int offset, int size);
        private delegate void glBindBufferOffsetEXT (uint target, uint index, uint buffer, int offset);
        private delegate void glBindBufferBaseEXT (uint target, uint index, uint buffer);
        private delegate void glTransformFeedbackVaryingsEXT (uint program, int count, string[] varyings, uint bufferMode);
        private delegate void glGetTransformFeedbackVaryingEXT (uint program, uint index, int bufSize, int[] length, int[] size, uint[] type, string name);

        //  Constants
        public const uint GL_TRANSFORM_FEEDBACK_BUFFER_EXT                       = 0x8C8E;
        public const uint GL_TRANSFORM_FEEDBACK_BUFFER_START_EXT                 = 0x8C84;
        public const uint GL_TRANSFORM_FEEDBACK_BUFFER_SIZE_EXT                  = 0x8C85;
        public const uint GL_TRANSFORM_FEEDBACK_BUFFER_BINDING_EXT               = 0x8C8F;
        public const uint GL_INTERLEAVED_ATTRIBS_EXT                             = 0x8C8C;
        public const uint GL_SEPARATE_ATTRIBS_EXT                                = 0x8C8D;
        public const uint GL_PRIMITIVES_GENERATED_EXT                            = 0x8C87;
        public const uint GL_TRANSFORM_FEEDBACK_PRIMITIVES_WRITTEN_EXT           = 0x8C88;
        public const uint GL_RASTERIZER_DISCARD_EXT                              = 0x8C89;
        public const uint GL_MAX_TRANSFORM_FEEDBACK_INTERLEAVED_COMPONENTS_EXT   = 0x8C8A;
        public const uint GL_MAX_TRANSFORM_FEEDBACK_SEPARATE_ATTRIBS_EXT         = 0x8C8B;
        public const uint GL_MAX_TRANSFORM_FEEDBACK_SEPARATE_COMPONENTS_EXT      = 0x8C80;
        public const uint GL_TRANSFORM_FEEDBACK_VARYINGS_EXT                     = 0x8C83;
        public const uint GL_TRANSFORM_FEEDBACK_BUFFER_MODE_EXT                  = 0x8C7F;
        public const uint GL_TRANSFORM_FEEDBACK_VARYING_MAX_LENGTH_EXT           = 0x8C76;

        #endregion

        #region WGL_ARB_extensions_string

        /// <summary>
        /// Gets the ARB extensions string.
        /// </summary>
        public string GetExtensionsStringARB()
        {
            return (string)GetDelegateFor<wglGetExtensionsStringARB>()(RenderContextProvider.DeviceContextHandle);
        }

        //  Delegates
        private delegate string wglGetExtensionsStringARB(IntPtr hdc);

        #endregion

        #region WGL_ARB_create_context
        
        //  Methods

        /// <summary>
        /// Creates a render context with the specified attributes.
        /// </summary>
        /// <param name="hShareContext">
        /// If is not null, then all shareable data (excluding
        /// OpenGL texture objects named 0) will be shared by hShareContext,
        /// all other contexts hShareContext already shares with, and the
        /// newly created context. An arbitrary number of contexts can share
        /// data in this fashion.</param>
        /// <param name="attribList">
        /// specifies a list of attributes for the context. The
        /// list consists of a sequence of (name,value) pairs terminated by the
        /// value 0. If an attribute is not specified in attribList, then the
        /// default value specified below is used instead. If an attribute is
        /// specified more than once, then the last value specified is used.
        /// </param>
        public IntPtr CreateContextAttribsARB(IntPtr hShareContext, int[] attribList)
        {
            return (IntPtr)GetDelegateFor<wglCreateContextAttribsARB>()(RenderContextProvider.DeviceContextHandle, hShareContext, attribList);
        }

        //  Delegates
        private delegate IntPtr wglCreateContextAttribsARB(IntPtr hDC, IntPtr hShareContext, int[] attribList);
    
        //  Constants
        public const int WGL_CONTEXT_MAJOR_VERSION_ARB            = 0x2091;
        public const int WGL_CONTEXT_MINOR_VERSION_ARB            = 0x2092;
        public const int WGL_CONTEXT_LAYER_PLANE_ARB              = 0x2093;
        public const int WGL_CONTEXT_FLAGS_ARB                    = 0x2094;
        public const int WGL_CONTEXT_PROFILE_MASK_ARB             = 0x9126;
        public const int WGL_CONTEXT_DEBUG_BIT_ARB                = 0x0001;
        public const int WGL_CONTEXT_FORWARD_COMPATIBLE_BIT_ARB   = 0x0002;
        public const int WGL_CONTEXT_CORE_PROFILE_BIT_ARB         = 0x00000001;
        public const int WGL_CONTEXT_COMPATIBILITY_PROFILE_BIT_ARB = 0x00000002;
        public const int ERROR_INVALID_VERSION_ARB = 0x2095;
        public const int ERROR_INVALID_PROFILE_ARB                = 0x2096;

        #endregion

        #region GL_ARB_explicit_uniform_location

        //  Constants

        /// <summary>
        /// The number of available pre-assigned uniform locations to that can default be 
        /// allocated in the default uniform block.
        /// </summary>
        public const int GL_MAX_UNIFORM_LOCATIONS = 0x826E;

        #endregion

        #region GL_ARB_clear_buffer_object

        /// <summary>
        /// Fill a buffer object's data store with a fixed value
        /// </summary>
        /// <param name="target">Specifies the target buffer object. The symbolic constant must be GL_ARRAY_BUFFER​, GL_ATOMIC_COUNTER_BUFFER​, GL_COPY_READ_BUFFER​, GL_COPY_WRITE_BUFFER​, GL_DRAW_INDIRECT_BUFFER​, GL_DISPATCH_INDIRECT_BUFFER​, GL_ELEMENT_ARRAY_BUFFER​, GL_PIXEL_PACK_BUFFER​, GL_PIXEL_UNPACK_BUFFER​, GL_QUERY_BUFFER​, GL_SHADER_STORAGE_BUFFER​, GL_TEXTURE_BUFFER​, GL_TRANSFORM_FEEDBACK_BUFFER​, or GL_UNIFORM_BUFFER​.</param>
        /// <param name="internalformat">The sized internal format with which the data will be stored in the buffer object.</param>
        /// <param name="format">Specifies the format of the pixel data. For transfers of depth, stencil, or depth/stencil data, you must use GL_DEPTH_COMPONENT​, GL_STENCIL_INDEX​, or GL_DEPTH_STENCIL​, where appropriate. For transfers of normalized integer or floating-point color image data, you must use one of the following: GL_RED​, GL_GREEN​, GL_BLUE​, GL_RG​, GL_RGB​, GL_BGR​, GL_RGBA​, and GL_BGRA​. For transfers of non-normalized integer data, you must use one of the following: GL_RED_INTEGER​, GL_GREEN_INTEGER​, GL_BLUE_INTEGER​, GL_RG_INTEGER​, GL_RGB_INTEGER​, GL_BGR_INTEGER​, GL_RGBA_INTEGER​, and GL_BGRA_INTEGER​.</param>
        /// <param name="type">Specifies the data type of the pixel data. The following symbolic values are accepted: GL_UNSIGNED_BYTE​, GL_BYTE​, GL_UNSIGNED_SHORT​, GL_SHORT​, GL_UNSIGNED_INT​, GL_INT​, GL_FLOAT​, GL_UNSIGNED_BYTE_3_3_2​, GL_UNSIGNED_BYTE_2_3_3_REV​, GL_UNSIGNED_SHORT_5_6_5​, GL_UNSIGNED_SHORT_5_6_5_REV​, GL_UNSIGNED_SHORT_4_4_4_4​, GL_UNSIGNED_SHORT_4_4_4_4_REV​, GL_UNSIGNED_SHORT_5_5_5_1​, GL_UNSIGNED_SHORT_1_5_5_5_REV​, GL_UNSIGNED_INT_8_8_8_8​, GL_UNSIGNED_INT_8_8_8_8_REV​, GL_UNSIGNED_INT_10_10_10_2​, and GL_UNSIGNED_INT_2_10_10_10_REV​.</param>
        /// <param name="data">Specifies a pointer to a single pixel of data to upload. This parameter may not be NULL.</param>
        public void ClearBufferData(uint target, uint internalformat, uint format, uint type, IntPtr data)
        {
            GetDelegateFor<glClearBufferData>()(target, internalformat, format, type, data);
        }

        /// <summary>
        /// Fill all or part of buffer object's data store with a fixed value
        /// </summary>
        /// <param name="target">Specifies the target buffer object. The symbolic constant must be GL_ARRAY_BUFFER​, GL_ATOMIC_COUNTER_BUFFER​, GL_COPY_READ_BUFFER​, GL_COPY_WRITE_BUFFER​, GL_DRAW_INDIRECT_BUFFER​, GL_DISPATCH_INDIRECT_BUFFER​, GL_ELEMENT_ARRAY_BUFFER​, GL_PIXEL_PACK_BUFFER​, GL_PIXEL_UNPACK_BUFFER​, GL_QUERY_BUFFER​, GL_SHADER_STORAGE_BUFFER​, GL_TEXTURE_BUFFER​, GL_TRANSFORM_FEEDBACK_BUFFER​, or GL_UNIFORM_BUFFER​.</param>
        /// <param name="internalformat">The sized internal format with which the data will be stored in the buffer object.</param>
        /// <param name="offset">The offset, in basic machine units into the buffer object's data store at which to start filling.</param>
        /// <param name="size">The size, in basic machine units of the range of the data store to fill.</param>
        /// <param name="format">Specifies the format of the pixel data. For transfers of depth, stencil, or depth/stencil data, you must use GL_DEPTH_COMPONENT​, GL_STENCIL_INDEX​, or GL_DEPTH_STENCIL​, where appropriate. For transfers of normalized integer or floating-point color image data, you must use one of the following: GL_RED​, GL_GREEN​, GL_BLUE​, GL_RG​, GL_RGB​, GL_BGR​, GL_RGBA​, and GL_BGRA​. For transfers of non-normalized integer data, you must use one of the following: GL_RED_INTEGER​, GL_GREEN_INTEGER​, GL_BLUE_INTEGER​, GL_RG_INTEGER​, GL_RGB_INTEGER​, GL_BGR_INTEGER​, GL_RGBA_INTEGER​, and GL_BGRA_INTEGER​.</param>
        /// <param name="type">Specifies the data type of the pixel data. The following symbolic values are accepted: GL_UNSIGNED_BYTE​, GL_BYTE​, GL_UNSIGNED_SHORT​, GL_SHORT​, GL_UNSIGNED_INT​, GL_INT​, GL_FLOAT​, GL_UNSIGNED_BYTE_3_3_2​, GL_UNSIGNED_BYTE_2_3_3_REV​, GL_UNSIGNED_SHORT_5_6_5​, GL_UNSIGNED_SHORT_5_6_5_REV​, GL_UNSIGNED_SHORT_4_4_4_4​, GL_UNSIGNED_SHORT_4_4_4_4_REV​, GL_UNSIGNED_SHORT_5_5_5_1​, GL_UNSIGNED_SHORT_1_5_5_5_REV​, GL_UNSIGNED_INT_8_8_8_8​, GL_UNSIGNED_INT_8_8_8_8_REV​, GL_UNSIGNED_INT_10_10_10_2​, and GL_UNSIGNED_INT_2_10_10_10_REV​.</param>
        /// <param name="data">Specifies a pointer to a single pixel of data to upload. This parameter may not be NULL.</param>
        public void ClearBufferSubData(uint target, uint internalformat, IntPtr offset, uint size, uint format, uint type, IntPtr data)
        {
            GetDelegateFor<glClearBufferSubData>()(target, internalformat, offset, size, format, type, data);
        }

        public void ClearNamedBufferDataEXT(uint buffer, uint internalformat, uint format, uint type, IntPtr data)
        {
            GetDelegateFor<glClearNamedBufferDataEXT>()(buffer, internalformat, format, type, data);
        }
        public void ClearNamedBufferSubDataEXT(uint buffer, uint internalformat, IntPtr offset, uint size, uint format, uint type, IntPtr data)
        {
            GetDelegateFor<glClearNamedBufferSubDataEXT>()(buffer, internalformat, offset, size, format, type, data);
        }
        
        //  Delegates
        private delegate void glClearBufferData(uint target, uint internalformat, uint format, uint type, IntPtr data);
        private delegate void glClearBufferSubData(uint target, uint internalformat, IntPtr offset, uint size, uint format, uint type, IntPtr data);
        private delegate void glClearNamedBufferDataEXT(uint buffer, uint internalformat, uint format, uint type, IntPtr data);
        private delegate void glClearNamedBufferSubDataEXT(uint buffer, uint internalformat, IntPtr offset, uint size, uint format, uint type, IntPtr data);

        #endregion

        #region GL_ARB_compute_shader

        /// <summary>
        /// Launch one or more compute work groups
        /// </summary>
        /// <param name="num_groups_x">The number of work groups to be launched in the X dimension.</param>
        /// <param name="num_groups_y">The number of work groups to be launched in the Y dimension.</param>
        /// <param name="num_groups_z">The number of work groups to be launched in the Z dimension.</param>
        public void DispatchCompute(uint num_groups_x, uint num_groups_y, uint num_groups_z)
        {
            GetDelegateFor<glDispatchCompute>()(num_groups_x, num_groups_y, num_groups_z);
        }

        /// <summary>
        /// Launch one or more compute work groups using parameters stored in a buffer
        /// </summary>
        /// <param name="indirect">The offset into the buffer object currently bound to the GL_DISPATCH_INDIRECT_BUFFER​ buffer target at which the dispatch parameters are stored.</param>
        public void DispatchComputeIndirect(IntPtr indirect)
        {
            GetDelegateFor<glDispatchComputeIndirect>()(indirect);
        }

        //  Delegates
        private delegate void glDispatchCompute(uint num_groups_x, uint num_groups_y, uint num_groups_z);
        private delegate void glDispatchComputeIndirect(IntPtr indirect);

        // Constants
        public const uint GL_COMPUTE_SHADER = 0x91B9;
        public const uint GL_MAX_COMPUTE_UNIFORM_BLOCKS = 0x91BB;
        public const uint GL_MAX_COMPUTE_TEXTURE_IMAGE_UNITS = 0x91BC;
        public const uint GL_MAX_COMPUTE_IMAGE_UNIFORMS = 0x91BD;
        public const uint GL_MAX_COMPUTE_SHARED_MEMORY_SIZE = 0x8262;
        public const uint GL_MAX_COMPUTE_UNIFORM_COMPONENTS = 0x8263;
        public const uint GL_MAX_COMPUTE_ATOMIC_COUNTER_BUFFERS = 0x8264;
        public const uint GL_MAX_COMPUTE_ATOMIC_COUNTERS = 0x8265;
        public const uint GL_MAX_COMBINED_COMPUTE_UNIFORM_COMPONENTS = 0x8266;
        public const uint GL_MAX_COMPUTE_WORK_GROUP_INVOCATIONS = 0x90EB;
        public const uint GL_MAX_COMPUTE_WORK_GROUP_COUNT = 0x91BE;
        public const uint GL_MAX_COMPUTE_WORK_GROUP_SIZE = 0x91BF;
        public const uint GL_COMPUTE_WORK_GROUP_SIZE = 0x8267;
        public const uint GL_UNIFORM_BLOCK_REFERENCED_BY_COMPUTE_SHADER = 0x90EC;
        public const uint GL_ATOMIC_COUNTER_BUFFER_REFERENCED_BY_COMPUTE_SHADER = 0x90ED;
        public const uint GL_DISPATCH_INDIRECT_BUFFER = 0x90EE;
        public const uint GL_DISPATCH_INDIRECT_BUFFER_BINDING = 0x90EF;
        public const uint GL_COMPUTE_SHADER_BIT = 0x00000020;

        #endregion

        #region GL_ARB_copy_image

        /// <summary>
        /// Perform a raw data copy between two images
        /// </summary>
        /// <param name="srcName">The name of a texture or renderbuffer object from which to copy.</param>
        /// <param name="srcTarget">The target representing the namespace of the source name srcName​.</param>
        /// <param name="srcLevel">The mipmap level to read from the source.</param>
        /// <param name="srcX">The X coordinate of the left edge of the souce region to copy.</param>
        /// <param name="srcY">The Y coordinate of the top edge of the souce region to copy.</param>
        /// <param name="srcZ">The Z coordinate of the near edge of the souce region to copy.</param>
        /// <param name="dstName">The name of a texture or renderbuffer object to which to copy.</param>
        /// <param name="dstTarget">The target representing the namespace of the destination name dstName​.</param>
        /// <param name="dstLevel">The desination mipmap level.</param>
        /// <param name="dstX">The X coordinate of the left edge of the destination region.</param>
        /// <param name="dstY">The Y coordinate of the top edge of the destination region.</param>
        /// <param name="dstZ">The Z coordinate of the near edge of the destination region.</param>
        /// <param name="srcWidth">The width of the region to be copied.</param>
        /// <param name="srcHeight">The height of the region to be copied.</param>
        /// <param name="srcDepth">The depth of the region to be copied.</param>
        public void CopyImageSubData(uint srcName, uint srcTarget, int srcLevel, int srcX, int srcY, int srcZ, uint dstName,
            uint dstTarget, int dstLevel, int dstX, int dstY, int dstZ, uint srcWidth, uint srcHeight, uint srcDepth)
        {
            GetDelegateFor<glCopyImageSubData>()(srcName, srcTarget, srcLevel, srcX, srcY, srcZ, dstName, 
            dstTarget, dstLevel, dstX, dstY, dstZ, srcWidth, srcHeight, srcDepth);
        }

        //  Delegates
        private delegate void glCopyImageSubData(uint srcName, uint srcTarget, int srcLevel, int srcX, int srcY, int srcZ, uint dstName, 
            uint dstTarget, int dstLevel, int dstX, int dstY, int dstZ, uint srcWidth, uint srcHeight, uint srcDepth);

        #endregion

        #region GL_ARB_ES3_compatibility
        
        public const uint GL_COMPRESSED_RGB8_ETC2                          = 0x9274;
        public const uint GL_COMPRESSED_SRGB8_ETC2                         = 0x9275;
        public const uint GL_COMPRESSED_RGB8_PUNCHTHROUGH_ALPHA1_ETC2      = 0x9276;
        public const uint GL_COMPRESSED_SRGB8_PUNCHTHROUGH_ALPHA1_ETC2     = 0x9277;
        public const uint GL_COMPRESSED_RGBA8_ETC2_EAC                     = 0x9278;
        public const uint GL_COMPRESSED_SRGB8_ALPHA8_ETC2_EAC              = 0x9279;
        public const uint GL_COMPRESSED_R11_EAC                            = 0x9270;
        public const uint GL_COMPRESSED_SIGNED_R11_EAC                     = 0x9271;
        public const uint GL_COMPRESSED_RG11_EAC                           = 0x9272;
        public const uint GL_COMPRESSED_SIGNED_RG11_EAC                    = 0x9273;
        public const uint GL_PRIMITIVE_RESTART_FIXED_INDEX                 = 0x8D69;
        public const uint GL_ANY_SAMPLES_PASSED_CONSERVATIVE               = 0x8D6A;
        public const uint GL_MAX_ELEMENT_INDEX                             = 0x8D6B;
        public const uint GL_TEXTURE_IMMUTABLE_LEVELS                      = 0x82DF;

        #endregion

        #region GL_ARB_framebuffer_no_attachments

        //  Methods

        /// <summary>
        /// Set a named parameter of a framebuffer.
        /// </summary>
        /// <param name="target">The target of the operation, which must be GL_READ_FRAMEBUFFER​, GL_DRAW_FRAMEBUFFER​ or GL_FRAMEBUFFER​.</param>
        /// <param name="pname">A token indicating the parameter to be modified.</param>
        /// <param name="param">The new value for the parameter named pname​.</param>
        public void FramebufferParameter(uint target, uint pname, int param)
        {
            GetDelegateFor<glFramebufferParameteri>()(target, pname, param);
        }

        /// <summary>
        /// Retrieve a named parameter from a framebuffer
        /// </summary>
        /// <param name="target">The target of the operation, which must be GL_READ_FRAMEBUFFER​, GL_DRAW_FRAMEBUFFER​ or GL_FRAMEBUFFER​.</param>
        /// <param name="pname">A token indicating the parameter to be retrieved.</param>
        /// <param name="parameters">The address of a variable to receive the value of the parameter named pname​.</param>
        public void GetFramebufferParameter(uint target, uint pname, int[] parameters)
        {
            GetDelegateFor<glGetFramebufferParameteriv>()(target, pname, parameters);
        }

        public void NamedFramebufferParameterEXT(uint framebuffer, uint pname, int param)
        {
            GetDelegateFor<glNamedFramebufferParameteriEXT>()(framebuffer, pname, param);
        }

        public void GetNamedFramebufferParameterEXT(uint framebuffer, uint pname, int[] parameters)
        {
            GetDelegateFor<glGetNamedFramebufferParameterivEXT>()(framebuffer, pname, parameters);
        }

        //  Delegates
        private delegate void glFramebufferParameteri(uint target, uint pname, int param);
        private delegate void glGetFramebufferParameteriv(uint target, uint pname, int[] parameters);
        private delegate void glNamedFramebufferParameteriEXT(uint framebuffer, uint pname, int param);
        private delegate void glGetNamedFramebufferParameterivEXT(uint framebuffer, uint pname, int[] parameters);
        
        #endregion

        #region GL_ARB_internalformat_query2

        /// <summary>
        /// Retrieve information about implementation-dependent support for internal formats
        /// </summary>
        /// <param name="target">Indicates the usage of the internal format. target​ must be GL_TEXTURE_1D​, GL_TEXTURE_1D_ARRAY​, GL_TEXTURE_2D​, GL_TEXTURE_2D_ARRAY​, GL_TEXTURE_3D​, GL_TEXTURE_CUBE_MAP​, GL_TEXTURE_CUBE_MAP_ARRAY​, GL_TEXTURE_RECTANGLE​, GL_TEXTURE_BUFFER​, GL_RENDERBUFFER​, GL_TEXTURE_2D_MULTISAMPLE​ or GL_TEXTURE_2D_MULTISAMPLE_ARRAY​.</param>
        /// <param name="internalformat">Specifies the internal format about which to retrieve information.</param>
        /// <param name="pname">Specifies the type of information to query.</param>
        /// <param name="bufSize">Specifies the maximum number of basic machine units that may be written to params​ by the function.</param>
        /// <param name="parameters">Specifies the address of a variable into which to write the retrieved information.</param>
        public void GetInternalformat(uint target, uint internalformat, uint pname, uint bufSize, int[] parameters)
        {
            GetDelegateFor<glGetInternalformativ>()(target, internalformat, pname, bufSize, parameters);
        }

        /// <summary>
        /// Retrieve information about implementation-dependent support for internal formats
        /// </summary>
        /// <param name="target">Indicates the usage of the internal format. target​ must be GL_TEXTURE_1D​, GL_TEXTURE_1D_ARRAY​, GL_TEXTURE_2D​, GL_TEXTURE_2D_ARRAY​, GL_TEXTURE_3D​, GL_TEXTURE_CUBE_MAP​, GL_TEXTURE_CUBE_MAP_ARRAY​, GL_TEXTURE_RECTANGLE​, GL_TEXTURE_BUFFER​, GL_RENDERBUFFER​, GL_TEXTURE_2D_MULTISAMPLE​ or GL_TEXTURE_2D_MULTISAMPLE_ARRAY​.</param>
        /// <param name="internalformat">Specifies the internal format about which to retrieve information.</param>
        /// <param name="pname">Specifies the type of information to query.</param>
        /// <param name="bufSize">Specifies the maximum number of basic machine units that may be written to params​ by the function.</param>
        /// <param name="parameters">Specifies the address of a variable into which to write the retrieved information.</param>
        public void GetInternalformat(uint target, uint internalformat, uint pname, uint bufSize, Int64[] parameters)
        {
            GetDelegateFor<glGetInternalformati64v>()(target, internalformat, pname, bufSize, parameters);
        }

        //  Delegates
        private delegate void glGetInternalformativ(uint target, uint internalformat, uint pname, uint bufSize, int[] parameters);
        private delegate void glGetInternalformati64v(uint target, uint internalformat, uint pname, uint bufSize, Int64[] parameters);

        //  Constants
        public const uint GL_RENDERBUFFER                                  =   0x8D41;
        public const uint GL_TEXTURE_2D_MULTISAMPLE                        =   0x9100;
        public const uint GL_TEXTURE_2D_MULTISAMPLE_ARRAY                  =   0x9102;
        public const uint GL_NUM_SAMPLE_COUNTS                              =  0x9380;
        public const uint GL_INTERNALFORMAT_SUPPORTED                       =  0x826F;
        public const uint GL_INTERNALFORMAT_PREFERRED                       =  0x8270;
        public const uint GL_INTERNALFORMAT_RED_SIZE                        =  0x8271;
        public const uint GL_INTERNALFORMAT_GREEN_SIZE                      =  0x8272;
        public const uint GL_INTERNALFORMAT_BLUE_SIZE                       =  0x8273;
        public const uint GL_INTERNALFORMAT_ALPHA_SIZE                      =  0x8274;
        public const uint GL_INTERNALFORMAT_DEPTH_SIZE                      =  0x8275;
        public const uint GL_INTERNALFORMAT_STENCIL_SIZE                    =  0x8276;
        public const uint GL_INTERNALFORMAT_SHARED_SIZE                     =  0x8277;
        public const uint GL_INTERNALFORMAT_RED_TYPE                        =  0x8278;
        public const uint GL_INTERNALFORMAT_GREEN_TYPE                      =  0x8279;
        public const uint GL_INTERNALFORMAT_BLUE_TYPE                       =  0x827A;
        public const uint GL_INTERNALFORMAT_ALPHA_TYPE                      =  0x827B;
        public const uint GL_INTERNALFORMAT_DEPTH_TYPE                      =  0x827C;
        public const uint GL_INTERNALFORMAT_STENCIL_TYPE                    =  0x827D;
        public const uint GL_MAX_WIDTH                                      =  0x827E;
        public const uint GL_MAX_HEIGHT                                     =  0x827F;
        public const uint GL_MAX_DEPTH                                      =  0x8280;
        public const uint GL_MAX_LAYERS                                     =  0x8281;
        public const uint GL_MAX_COMBINED_DIMENSIONS                        =  0x8282;
        public const uint GL_COLOR_COMPONENTS                               =  0x8283;
        public const uint GL_DEPTH_COMPONENTS                               =  0x8284;
        public const uint GL_STENCIL_COMPONENTS                             =  0x8285;
        public const uint GL_COLOR_RENDERABLE                               =  0x8286;
        public const uint GL_DEPTH_RENDERABLE                               =  0x8287;
        public const uint GL_STENCIL_RENDERABLE                             =  0x8288;
        public const uint GL_FRAMEBUFFER_RENDERABLE                         =  0x8289;
        public const uint GL_FRAMEBUFFER_RENDERABLE_LAYERED                 =  0x828A;
        public const uint GL_FRAMEBUFFER_BLEND                              =  0x828B;
        public const uint GL_READ_PIXELS                                    =  0x828C;
        public const uint GL_READ_PIXELS_FORMAT                             =  0x828D;
        public const uint GL_READ_PIXELS_TYPE                               =  0x828E;
        public const uint GL_TEXTURE_IMAGE_FORMAT                           =  0x828F;
        public const uint GL_TEXTURE_IMAGE_TYPE                             =  0x8290;
        public const uint GL_GET_TEXTURE_IMAGE_FORMAT                       =  0x8291;
        public const uint GL_GET_TEXTURE_IMAGE_TYPE                         =  0x8292;
        public const uint GL_MIPMAP                                         =  0x8293;
        public const uint GL_MANUAL_GENERATE_MIPMAP                         =  0x8294;
        public const uint GL_AUTO_GENERATE_MIPMAP                           =  0x8295;
        public const uint GL_COLOR_ENCODING                                 =  0x8296;
        public const uint GL_SRGB_READ                                      =  0x8297;
        public const uint GL_SRGB_WRITE                                     =  0x8298;
        public const uint GL_SRGB_DECODE_ARB                                =  0x8299;
        public const uint GL_FILTER                                         =  0x829A;
        public const uint GL_VERTEX_TEXTURE                                 =  0x829B;
        public const uint GL_TESS_CONTROL_TEXTURE                           =  0x829C;
        public const uint GL_TESS_EVALUATION_TEXTURE                        =  0x829D;
        public const uint GL_GEOMETRY_TEXTURE                               =  0x829E;
        public const uint GL_FRAGMENT_TEXTURE                               =  0x829F;
        public const uint GL_COMPUTE_TEXTURE                                =  0x82A0;
        public const uint GL_TEXTURE_SHADOW                                 =  0x82A1;
        public const uint GL_TEXTURE_GATHER                                 =  0x82A2;
        public const uint GL_TEXTURE_GATHER_SHADOW                          =  0x82A3;
        public const uint GL_SHADER_IMAGE_LOAD                              =  0x82A4;
        public const uint GL_SHADER_IMAGE_STORE                             =  0x82A5;
        public const uint GL_SHADER_IMAGE_ATOMIC                            =  0x82A6;
        public const uint GL_IMAGE_TEXEL_SIZE                               =  0x82A7;
        public const uint GL_IMAGE_COMPATIBILITY_CLASS                      =  0x82A8;
        public const uint GL_IMAGE_PIXEL_FORMAT                             =  0x82A9;
        public const uint GL_IMAGE_PIXEL_TYPE                               =  0x82AA;
        public const uint GL_IMAGE_FORMAT_COMPATIBILITY_TYPE                =  0x90C7;
        public const uint GL_SIMULTANEOUS_TEXTURE_AND_DEPTH_TEST            =  0x82AC;
        public const uint GL_SIMULTANEOUS_TEXTURE_AND_STENCIL_TEST          =  0x82AD;
        public const uint GL_SIMULTANEOUS_TEXTURE_AND_DEPTH_WRITE           =  0x82AE;
        public const uint GL_SIMULTANEOUS_TEXTURE_AND_STENCIL_WRITE         =  0x82AF;
        public const uint GL_TEXTURE_COMPRESSED_BLOCK_WIDTH                 =  0x82B1;
        public const uint GL_TEXTURE_COMPRESSED_BLOCK_HEIGHT                =  0x82B2;
        public const uint GL_TEXTURE_COMPRESSED_BLOCK_SIZE                  =  0x82B3;
        public const uint GL_CLEAR_BUFFER                                   =  0x82B4;
        public const uint GL_TEXTURE_VIEW                                   =  0x82B5;
        public const uint GL_VIEW_COMPATIBILITY_CLASS                       =  0x82B6;
        public const uint GL_FULL_SUPPORT                                   =  0x82B7;
        public const uint GL_CAVEAT_SUPPORT                                 =  0x82B8;
        public const uint GL_IMAGE_CLASS_4_X_32                             =  0x82B9;
        public const uint GL_IMAGE_CLASS_2_X_32                             =  0x82BA;
        public const uint GL_IMAGE_CLASS_1_X_32                             =  0x82BB;
        public const uint GL_IMAGE_CLASS_4_X_16                             =  0x82BC;
        public const uint GL_IMAGE_CLASS_2_X_16                             =  0x82BD;
        public const uint GL_IMAGE_CLASS_1_X_16                             =  0x82BE;
        public const uint GL_IMAGE_CLASS_4_X_8                              =  0x82BF;
        public const uint GL_IMAGE_CLASS_2_X_8                              =  0x82C0;
        public const uint GL_IMAGE_CLASS_1_X_8                              =  0x82C1;
        public const uint GL_IMAGE_CLASS_11_11_10                           =  0x82C2;
        public const uint GL_IMAGE_CLASS_10_10_10_2                         =  0x82C3;
        public const uint GL_VIEW_CLASS_128_BITS                            =  0x82C4;
        public const uint GL_VIEW_CLASS_96_BITS                             =  0x82C5;
        public const uint GL_VIEW_CLASS_64_BITS                             =  0x82C6;
        public const uint GL_VIEW_CLASS_48_BITS                             =  0x82C7;
        public const uint GL_VIEW_CLASS_32_BITS                             =  0x82C8;
        public const uint GL_VIEW_CLASS_24_BITS                             =  0x82C9;
        public const uint GL_VIEW_CLASS_16_BITS                             =  0x82CA;
        public const uint GL_VIEW_CLASS_8_BITS                              =  0x82CB;
        public const uint GL_VIEW_CLASS_S3TC_DXT1_RGB                       =  0x82CC;
        public const uint GL_VIEW_CLASS_S3TC_DXT1_RGBA                      =  0x82CD;
        public const uint GL_VIEW_CLASS_S3TC_DXT3_RGBA                      =  0x82CE;
        public const uint GL_VIEW_CLASS_S3TC_DXT5_RGBA                      =  0x82CF;
        public const uint GL_VIEW_CLASS_RGTC1_RED                           =  0x82D0;
        public const uint GL_VIEW_CLASS_RGTC2_RG                            =  0x82D1;
        public const uint GL_VIEW_CLASS_BPTC_UNORM                          =  0x82D2;
        public const uint GL_VIEW_CLASS_BPTC_FLOAT                          =  0x82D3;
        
        #endregion

        #region GL_ARB_invalidate_subdata

        /// <summary>
        /// Invalidate a region of a texture image
        /// </summary>
        /// <param name="texture">The name of a texture object a subregion of which to invalidate.</param>
        /// <param name="level">The level of detail of the texture object within which the region resides.</param>
        /// <param name="xoffset">The X offset of the region to be invalidated.</param>
        /// <param name="yoffset">The Y offset of the region to be invalidated.</param>
        /// <param name="zoffset">The Z offset of the region to be invalidated.</param>
        /// <param name="width">The width of the region to be invalidated.</param>
        /// <param name="height">The height of the region to be invalidated.</param>
        /// <param name="depth">The depth of the region to be invalidated.</param>
        public void InvalidateTexSubImage(uint texture, int level, int xoffset, int yoffset, int zoffset,
            uint width, uint height, uint depth)
        {
            GetDelegateFor<glInvalidateTexSubImage>()(texture, level, xoffset, yoffset, zoffset, width, height, depth);
        }

        /// <summary>
        /// Invalidate the entirety a texture image
        /// </summary>
        /// <param name="texture">The name of a texture object to invalidate.</param>
        /// <param name="level">The level of detail of the texture object to invalidate.</param>
        public void InvalidateTexImage(uint texture, int level)
        {
            GetDelegateFor<glInvalidateTexImage>()(texture, level);
        }

        /// <summary>
        /// Invalidate a region of a buffer object's data store
        /// </summary>
        /// <param name="buffer">The name of a buffer object, a subrange of whose data store to invalidate.</param>
        /// <param name="offset">The offset within the buffer's data store of the start of the range to be invalidated.</param>
        /// <param name="length">The length of the range within the buffer's data store to be invalidated.</param>
        public void InvalidateBufferSubData(uint buffer, IntPtr offset, IntPtr length)
        {
            GetDelegateFor<glInvalidateBufferSubData>()(buffer, offset, length);
        }

        /// <summary>
        /// Invalidate the content of a buffer object's data store
        /// </summary>
        /// <param name="buffer">The name of a buffer object whose data store to invalidate.</param>
        public void InvalidateBufferData(uint buffer)
        {
            GetDelegateFor<glInvalidateBufferData>()(buffer);
        }

        /// <summary>
        /// Invalidate the content some or all of a framebuffer object's attachments
        /// </summary>
        /// <param name="target">The target to which the framebuffer is attached. target​ must be GL_FRAMEBUFFER​, GL_DRAW_FRAMEBUFFER​, or GL_READ_FRAMEBUFFER​.</param>
        /// <param name="numAttachments">The number of entries in the attachments​ array.</param>
        /// <param name="attachments">The address of an array identifying the attachments to be invalidated.</param>
        public void InvalidateFramebuffer(uint target, uint numAttachments, uint[] attachments)
        {
            GetDelegateFor<glInvalidateFramebuffer>()(target, numAttachments, attachments);
        }

        /// <summary>
        /// Invalidate the content of a region of some or all of a framebuffer object's attachments
        /// </summary>
        /// <param name="target">The target to which the framebuffer is attached. target​ must be GL_FRAMEBUFFER​, GL_DRAW_FRAMEBUFFER​, or GL_READ_FRAMEBUFFER​.</param>
        /// <param name="numAttachments">The number of entries in the attachments​ array.</param>
        /// <param name="attachments">The address of an array identifying the attachments to be invalidated.</param>
        /// <param name="x">The X offset of the region to be invalidated.</param>
        /// <param name="y">The Y offset of the region to be invalidated.</param>
        /// <param name="width">The width of the region to be invalidated.</param>
        /// <param name="height">The height of the region to be invalidated.</param>
        public void InvalidateSubFramebuffer(uint target, uint numAttachments, uint[] attachments,
            int x, int y, uint width, uint height)
        {
            GetDelegateFor<glInvalidateSubFramebuffer>()(target, numAttachments, attachments, x, y, width, height);
        }

        //  Delegates
        private delegate void glInvalidateTexSubImage(uint texture, int level, int xoffset, 
            int yoffset, int zoffset, uint width, uint height, uint depth);
        private delegate void glInvalidateTexImage(uint texture, int level);
        private delegate void glInvalidateBufferSubData(uint buffer, IntPtr offset, IntPtr length);
        private delegate void glInvalidateBufferData(uint buffer);
        private delegate void glInvalidateFramebuffer(uint target, uint numAttachments, uint[] attachments);
        private delegate void glInvalidateSubFramebuffer(uint target, uint numAttachments, uint[] attachments,
            int x, int y, uint width, uint height);

        #endregion

        #region ARB_multi_draw_indirect

        /// <summary>
        /// Render multiple sets of primitives from array data, taking parameters from memory
        /// </summary>
        /// <param name="mode">Specifies what kind of primitives to render. Symbolic constants GL_POINTS​, GL_LINE_STRIP​, GL_LINE_LOOP​, GL_LINES​, GL_LINE_STRIP_ADJACENCY​, GL_LINES_ADJACENCY​, GL_TRIANGLE_STRIP​, GL_TRIANGLE_FAN​, GL_TRIANGLES​, GL_TRIANGLE_STRIP_ADJACENCY​, GL_TRIANGLES_ADJACENCY​, and GL_PATCHES​ are accepted.</param>
        /// <param name="indirect">Specifies the address of an array of structures containing the draw parameters.</param>
        /// <param name="primcount">Specifies the the number of elements in the array of draw parameter structures.</param>
        /// <param name="stride">Specifies the distance in basic machine units between elements of the draw parameter array.</param>
        public void MultiDrawArraysIndirect(uint mode, IntPtr indirect, uint primcount, uint stride)
        {
            GetDelegateFor<glMultiDrawArraysIndirect>()(mode, indirect, primcount, stride);
        }

        /// <summary>
        /// Render indexed primitives from array data, taking parameters from memory
        /// </summary>
        /// <param name="mode">Specifies what kind of primitives to render. Symbolic constants GL_POINTS​, GL_LINE_STRIP​, GL_LINE_LOOP​, GL_LINES​, GL_LINE_STRIP_ADJACENCY​, GL_LINES_ADJACENCY​, GL_TRIANGLE_STRIP​, GL_TRIANGLE_FAN​, GL_TRIANGLES​, GL_TRIANGLE_STRIP_ADJACENCY​, GL_TRIANGLES_ADJACENCY​, and GL_PATCHES​ are accepted.</param>
        /// <param name="type">Specifies the type of data in the buffer bound to the GL_ELEMENT_ARRAY_BUFFER​ binding.</param>
        /// <param name="indirect">Specifies a byte offset (cast to a pointer type) into the buffer bound to GL_DRAW_INDIRECT_BUFFER​, which designates the starting point of the structure containing the draw parameters.</param>
        /// <param name="primcount">Specifies the number of elements in the array addressed by indirect​.</param>
        /// <param name="stride">Specifies the distance in basic machine units between elements of the draw parameter array.</param>
        public void MultiDrawElementsIndirect(uint mode, uint type, IntPtr indirect, uint primcount, uint stride)
        {
            GetDelegateFor<glMultiDrawElementsIndirect>()(mode, type, indirect, primcount, stride);
        }

        private delegate void glMultiDrawArraysIndirect(uint mode, IntPtr indirect, uint primcount, uint stride);
        private delegate void glMultiDrawElementsIndirect(uint mode, uint type, IntPtr indirect, uint primcount, uint stride);

        #endregion

        #region GL_ARB_program_interface_query

        /// <summary>
        /// Query a property of an interface in a program
        /// </summary>
        /// <param name="program">The name of a program object whose interface to query.</param>
        /// <param name="programInterface">A token identifying the interface within program​ to query.</param>
        /// <param name="pname">The name of the parameter within programInterface​ to query.</param>
        /// <param name="parameters">The address of a variable to retrieve the value of pname​ for the program interface..</param>
        public void GetProgramInterface(uint program, uint programInterface, uint pname, int[] parameters)
        {
            GetDelegateFor<glGetProgramInterfaceiv>()(program, programInterface, pname, parameters);
        }

        /// <summary>
        /// Query the index of a named resource within a program
        /// </summary>
        /// <param name="program">The name of a program object whose resources to query.</param>
        /// <param name="programInterface">A token identifying the interface within program​ containing the resource named name​.</param>
        /// <param name="name">The name of the resource to query the index of.</param>
        public void GetProgramResourceIndex(uint program, uint programInterface, string name)
        {
            GetDelegateFor<glGetProgramResourceIndex>()(program, programInterface, name);
        }

        /// <summary>
        /// Query the name of an indexed resource within a program
        /// </summary>
        /// <param name="program">The name of a program object whose resources to query.</param>
        /// <param name="programInterface">A token identifying the interface within program​ containing the indexed resource.</param>
        /// <param name="index">The index of the resource within programInterface​ of program​.</param>
        /// <param name="bufSize">The size of the character array whose address is given by name​.</param>
        /// <param name="length">The address of a variable which will receive the length of the resource name.</param>
        /// <param name="name">The address of a character array into which will be written the name of the resource.</param>
        public void GetProgramResourceName(uint program, uint programInterface, uint index, uint bufSize, out uint length, out string name)
        {
            var lengthParameter = new uint[1];
            var nameParameter = new string[1];
            GetDelegateFor<glGetProgramResourceName>()(program, programInterface, index, bufSize, lengthParameter, nameParameter);
            length = lengthParameter[0];
            name = nameParameter[0];
        }

        /// <summary>
        /// Retrieve values for multiple properties of a single active resource within a program object
        /// </summary>
        /// <param name="program">The name of a program object whose resources to query.</param>
        /// <param name="programInterface">A token identifying the interface within program​ containing the resource named name​.</param>
        /// <param name="index">The index within the programInterface​ to query information about.</param>
        /// <param name="propCount">The number of properties being queried.</param>
        /// <param name="props">An array of properties of length propCount​ to query.</param>
        /// <param name="bufSize">The number of GLint values in the params​ array.</param>
        /// <param name="length">If not NULL, then this value will be filled in with the number of actual parameters written to params​.</param>
        /// <param name="parameters">The output array of parameters to write.</param>
        public void GetProgramResource(uint program, uint programInterface, uint index, uint propCount, uint[] props, uint bufSize, out uint length, out int[] parameters)
        {
            var lengthParameter = new uint[1];
            var parametersParameter = new int[bufSize];

            GetDelegateFor<glGetProgramResourceiv>()(program, programInterface, index, propCount, props, bufSize, lengthParameter, parametersParameter);
            length = lengthParameter[0];
            parameters = parametersParameter;
        }

        /// <summary>
        /// Query the location of a named resource within a program.
        /// </summary>
        /// <param name="program">The name of a program object whose resources to query.</param>
        /// <param name="programInterface">A token identifying the interface within program​ containing the resource named name​.</param>
        /// <param name="name">The name of the resource to query the location of.</param>
        public void GetProgramResourceLocation(uint program, uint programInterface, string name)
        {
            GetDelegateFor<glGetProgramResourceLocation>()(program, programInterface, name);
        }

        /// <summary>
        /// Query the fragment color index of a named variable within a program.
        /// </summary>
        /// <param name="program">The name of a program object whose resources to query.</param>
        /// <param name="programInterface">A token identifying the interface within program​ containing the resource named name​.</param>
        /// <param name="name">The name of the resource to query the location of.</param>
        public void GetProgramResourceLocationIndex(uint program, uint programInterface, string name)
        {
            GetDelegateFor<glGetProgramResourceLocationIndex>()(program, programInterface, name);
        }

        private delegate void glGetProgramInterfaceiv(uint program, uint programInterface, uint pname, int[] parameters);
        private delegate uint glGetProgramResourceIndex(uint program, uint programInterface, string name);
        private delegate void glGetProgramResourceName(uint program, uint programInterface, uint index, uint bufSize, uint[] length, string[] name);
        private delegate void glGetProgramResourceiv(uint program, uint programInterface, uint index, uint propCount, uint[] props, uint bufSize, uint[] length, int[] parameters);
        private delegate int glGetProgramResourceLocation(uint program, uint programInterface, string name);
        private delegate int glGetProgramResourceLocationIndex(uint program, uint programInterface, string name);

        #endregion

        #region GL_ARB_shader_storage_buffer_object

        /// <summary>
        /// Change an active shader storage block binding.
        /// </summary>
        /// <param name="program">The name of the program containing the block whose binding to change.</param>
        /// <param name="storageBlockIndex">The index storage block within the program.</param>
        /// <param name="storageBlockBinding">The index storage block binding to associate with the specified storage block.</param>
        public void ShaderStorageBlockBinding(uint program, uint storageBlockIndex, uint storageBlockBinding)
        {
            GetDelegateFor<glShaderStorageBlockBinding>()(program, storageBlockIndex, storageBlockBinding);
        }

        private delegate void glShaderStorageBlockBinding(uint program, uint storageBlockIndex, uint storageBlockBinding);

        //  Constants
        public const uint GL_SHADER_STORAGE_BUFFER                         = 0x90D2;
        public const uint GL_SHADER_STORAGE_BUFFER_BINDING                 = 0x90D3;
        public const uint GL_SHADER_STORAGE_BUFFER_START                   = 0x90D4;
        public const uint GL_SHADER_STORAGE_BUFFER_SIZE                    = 0x90D5;
        public const uint GL_MAX_VERTEX_SHADER_STORAGE_BLOCKS              = 0x90D6;
        public const uint GL_MAX_GEOMETRY_SHADER_STORAGE_BLOCKS            = 0x90D7;
        public const uint GL_MAX_TESS_CONTROL_SHADER_STORAGE_BLOCKS        = 0x90D8;
        public const uint GL_MAX_TESS_EVALUATION_SHADER_STORAGE_BLOCKS     = 0x90D9;
        public const uint GL_MAX_FRAGMENT_SHADER_STORAGE_BLOCKS            = 0x90DA;
        public const uint GL_MAX_COMPUTE_SHADER_STORAGE_BLOCKS             = 0x90DB;
        public const uint GL_MAX_COMBINED_SHADER_STORAGE_BLOCKS            = 0x90DC;
        public const uint GL_MAX_SHADER_STORAGE_BUFFER_BINDINGS            = 0x90DD;
        public const uint GL_MAX_SHADER_STORAGE_BLOCK_SIZE                 = 0x90DE;
        public const uint GL_SHADER_STORAGE_BUFFER_OFFSET_ALIGNMENT        = 0x90DF;
        public const uint GL_SHADER_STORAGE_BARRIER_BIT                    = 0x2000;       
        public const uint GL_MAX_COMBINED_SHADER_OUTPUT_RESOURCES          = 0x8F39;

        #endregion

        #region GL_ARB_stencil_texturing

        //  Constants
        public const uint GL_DEPTH_STENCIL_TEXTURE_MODE = 0x90EA;

        #endregion

        #region GL_ARB_texture_buffer_range

        /// <summary>
        /// Bind a range of a buffer's data store to a buffer texture
        /// </summary>
        /// <param name="target">Specifies the target of the operation and must be GL_TEXTURE_BUFFER​.</param>
        /// <param name="internalformat">Specifies the internal format of the data in the store belonging to buffer​.</param>
        /// <param name="buffer">Specifies the name of the buffer object whose storage to attach to the active buffer texture.</param>
        /// <param name="offset">Specifies the offset of the start of the range of the buffer's data store to attach.</param>
        /// <param name="size">Specifies the size of the range of the buffer's data store to attach.</param>
        public void TexBufferRange(uint target, uint internalformat, uint buffer, IntPtr offset, IntPtr size)
        {
            GetDelegateFor<glTexBufferRange>()(target, internalformat, buffer, offset, size);
        }

        /// <summary>
        /// Bind a range of a buffer's data store to a buffer texture
        /// </summary>
        /// <param name="texture">The texture.</param>
        /// <param name="target">Specifies the target of the operation and must be GL_TEXTURE_BUFFER​.</param>
        /// <param name="internalformat">Specifies the internal format of the data in the store belonging to buffer​.</param>
        /// <param name="buffer">Specifies the name of the buffer object whose storage to attach to the active buffer texture.</param>
        /// <param name="offset">Specifies the offset of the start of the range of the buffer's data store to attach.</param>
        /// <param name="size">Specifies the size of the range of the buffer's data store to attach.</param>
        public void TextureBufferRangeEXT(uint texture, uint target, uint internalformat, uint buffer, IntPtr offset, IntPtr size)
        {
            GetDelegateFor<glTextureBufferRangeEXT>()(texture, target, internalformat, buffer, offset, size);
        }

        private delegate void glTexBufferRange(uint target, uint internalformat, uint buffer, IntPtr offset, IntPtr size);
        private delegate void glTextureBufferRangeEXT(uint texture, uint target, uint internalformat, uint buffer, IntPtr offset, IntPtr size);

        #endregion

        #region GL_ARB_texture_storage_multisample

        /// <summary>
        /// Specify storage for a two-dimensional multisample texture.
        /// </summary>
        /// <param name="target">Specify the target of the operation. target​ must be GL_TEXTURE_2D_MULTISAMPLE​ or GL_PROXY_TEXTURE_2D_MULTISAMPLE​.</param>
        /// <param name="samples">Specify the number of samples in the texture.</param>
        /// <param name="internalformat">Specifies the sized internal format to be used to store texture image data.</param>
        /// <param name="width">Specifies the width of the texture, in texels.</param>
        /// <param name="height">Specifies the height of the texture, in texels.</param>
        /// <param name="fixedsamplelocations">Specifies whether the image will use identical sample locations and the same number of samples for all texels in the image, and the sample locations will not depend on the internal format or size of the image.</param>
        public void TexStorage2DMultisample(uint target, uint samples, uint internalformat, uint width, uint height, bool fixedsamplelocations)
        {
            GetDelegateFor<glTexStorage2DMultisample>()(target, samples, internalformat, width, height, fixedsamplelocations);
        }

        /// <summary>
        /// Specify storage for a three-dimensional multisample array texture
        /// </summary>
        /// <param name="target">Specify the target of the operation. target​ must be GL_TEXTURE_3D_MULTISAMPLE_ARRAY​ or GL_PROXY_TEXTURE_3D_MULTISAMPLE_ARRAY​.</param>
        /// <param name="samples">Specify the number of samples in the texture.</param>
        /// <param name="internalformat">Specifies the sized internal format to be used to store texture image data.</param>
        /// <param name="width">Specifies the width of the texture, in texels.</param>
        /// <param name="height">Specifies the height of the texture, in texels.</param>
        /// <param name="depth">Specifies the depth of the texture, in layers.</param>
        /// <param name="fixedsamplelocations">Specifies the depth of the texture, in layers.</param>
        public void TexStorage3DMultisample(uint target, uint samples, uint internalformat, uint width, uint height, uint depth, bool fixedsamplelocations)
        {
            GetDelegateFor<glTexStorage3DMultisample>()(target, samples, internalformat, width, height, depth, fixedsamplelocations);
        }

        /// <summary>
        /// Specify storage for a two-dimensional multisample texture.
        /// </summary>
        /// <param name="texture">The texture.</param>
        /// <param name="target">Specify the target of the operation. target​ must be GL_TEXTURE_2D_MULTISAMPLE​ or GL_PROXY_TEXTURE_2D_MULTISAMPLE​.</param>
        /// <param name="samples">Specify the number of samples in the texture.</param>
        /// <param name="internalformat">Specifies the sized internal format to be used to store texture image data.</param>
        /// <param name="width">Specifies the width of the texture, in texels.</param>
        /// <param name="height">Specifies the height of the texture, in texels.</param>
        /// <param name="fixedsamplelocations">Specifies whether the image will use identical sample locations and the same number of samples for all texels in the image, and the sample locations will not depend on the internal format or size of the image.</param>
        public void TexStorage2DMultisampleEXT(uint texture, uint target, uint samples, uint internalformat, uint width, uint height, bool fixedsamplelocations)
        {
            GetDelegateFor<glTexStorage2DMultisampleEXT>()(texture, target, samples, internalformat, width, height, fixedsamplelocations);
        }

        /// <summary>
        /// Specify storage for a three-dimensional multisample array texture
        /// </summary>
        /// <param name="texture">The texture.</param>
        /// <param name="target">Specify the target of the operation. target​ must be GL_TEXTURE_3D_MULTISAMPLE_ARRAY​ or GL_PROXY_TEXTURE_3D_MULTISAMPLE_ARRAY​.</param>
        /// <param name="samples">Specify the number of samples in the texture.</param>
        /// <param name="internalformat">Specifies the sized internal format to be used to store texture image data.</param>
        /// <param name="width">Specifies the width of the texture, in texels.</param>
        /// <param name="height">Specifies the height of the texture, in texels.</param>
        /// <param name="depth">Specifies the depth of the texture, in layers.</param>
        /// <param name="fixedsamplelocations">Specifies the depth of the texture, in layers.</param>
        public void TexStorage3DMultisampleEXT(uint texture, uint target, uint samples, uint internalformat, uint width, uint height, uint depth, bool fixedsamplelocations)
        {
            GetDelegateFor<glTexStorage3DMultisampleEXT>()(texture, target, samples, internalformat, width, height, depth, fixedsamplelocations);
        }

        //  Delegates
        private delegate void glTexStorage2DMultisample(uint target, uint samples, uint internalformat, uint width, uint height, bool fixedsamplelocations);
        private delegate void glTexStorage3DMultisample(uint target, uint samples, uint internalformat, uint width, uint height, uint depth, bool fixedsamplelocations);
        private delegate void glTexStorage2DMultisampleEXT(uint texture, uint target, uint samples, uint internalformat, uint width, uint height, bool fixedsamplelocations);
        private delegate void glTexStorage3DMultisampleEXT(uint texture, uint target, uint samples, uint internalformat, uint width, uint height, uint depth, bool fixedsamplelocations);

        #endregion

        #region GL_ARB_texture_view

        /// <summary>
        /// Initialize a texture as a data alias of another texture's data store.
        /// </summary>
        /// <param name="texture">Specifies the texture object to be initialized as a view.</param>
        /// <param name="target">Specifies the target to be used for the newly initialized texture.</param>
        /// <param name="origtexture">Specifies the name of a texture object of which to make a view.</param>
        /// <param name="internalformat">Specifies the internal format for the newly created view.</param>
        /// <param name="minlevel">Specifies lowest level of detail of the view.</param>
        /// <param name="numlevels">Specifies the number of levels of detail to include in the view.</param>
        /// <param name="minlayer">Specifies the index of the first layer to include in the view.</param>
        /// <param name="numlayers">Specifies the number of layers to include in the view.</param>
        public void TextureView(uint texture, uint target, uint origtexture, uint internalformat, uint minlevel, uint numlevels, uint minlayer, uint numlayers)
        {
            GetDelegateFor<glTextureView>()(texture, target, origtexture, internalformat, minlevel, numlevels, minlayer, numlayers);
        }

        //  Delegates
        private delegate void glTextureView(uint texture, uint target, uint origtexture, uint internalformat, uint minlevel, uint numlevels, uint minlayer, uint numlayers);

        //  Constants
        public const uint GL_TEXTURE_VIEW_MIN_LEVEL = 0x82DB;
        public const uint GL_TEXTURE_VIEW_NUM_LEVELS = 0x82DC;
        public const uint GL_TEXTURE_VIEW_MIN_LAYER = 0x82DD;
        public const uint GL_TEXTURE_VIEW_NUM_LAYERS = 0x82DE;

        #endregion

        #region GL_ARB_vertex_attrib_binding

        /// <summary>
        /// Bind a buffer to a vertex buffer bind point.
        /// </summary>
        /// <param name="bindingindex">The index of the vertex buffer binding point to which to bind the buffer.</param>
        /// <param name="buffer">The name of an existing buffer to bind to the vertex buffer binding point.</param>
        /// <param name="offset">The offset of the first element of the buffer.</param>
        /// <param name="stride">The distance between elements within the buffer.</param>
        public void BindVertexBuffer(uint bindingindex, uint buffer, IntPtr offset, uint stride)
        {
            GetDelegateFor<glBindVertexBuffer>()(bindingindex, buffer, offset, stride);
        }
        
        /// <summary>
        /// Specify the organization of vertex arrays.
        /// </summary>
        /// <param name="attribindex">The generic vertex attribute array being described.</param>
        /// <param name="size">The number of values per vertex that are stored in the array.</param>
        /// <param name="type">The type of the data stored in the array.</param>
        /// <param name="normalized">GL_TRUE​ if the parameter represents a normalized integer (type​ must be an integer type). GL_FALSE​ otherwise.</param>
        /// <param name="relativeoffset">The offset, measured in basic machine units of the first element relative to the start of the vertex buffer binding this attribute fetches from.</param>
        public void VertexAttribFormat(uint attribindex, int size, uint type, bool normalized, uint relativeoffset)
        {
            GetDelegateFor<glVertexAttribFormat>()(attribindex, size, type, normalized, relativeoffset);
        }

        /// <summary>
        /// Specify the organization of vertex arrays.
        /// </summary>
        /// <param name="attribindex">The generic vertex attribute array being described.</param>
        /// <param name="size">The number of values per vertex that are stored in the array.</param>
        /// <param name="type">The type of the data stored in the array.</param>
        /// <param name="relativeoffset">The offset, measured in basic machine units of the first element relative to the start of the vertex buffer binding this attribute fetches from.</param>
        public void VertexAttribIFormat(uint attribindex, int size, uint type, uint relativeoffset)
        {
            GetDelegateFor<glVertexAttribIFormat>()(attribindex, size, type, relativeoffset);
        }

        /// <summary>
        /// Specify the organization of vertex arrays.
        /// </summary>
        /// <param name="attribindex">The generic vertex attribute array being described.</param>
        /// <param name="size">The number of values per vertex that are stored in the array.</param>
        /// <param name="type">The type of the data stored in the array.</param>
        /// <param name="relativeoffset">The offset, measured in basic machine units of the first element relative to the start of the vertex buffer binding this attribute fetches from.</param>
        public void VertexAttribLFormat(uint attribindex, int size, uint type, uint relativeoffset)
        {
            GetDelegateFor<glVertexAttribLFormat>()(attribindex, size, type, relativeoffset);
        }
        
        /// <summary>
        /// Associate a vertex attribute and a vertex buffer binding.
        /// </summary>
        /// <param name="attribindex">The index of the attribute to associate with a vertex buffer binding.</param>
        /// <param name="bindingindex">The index of the vertex buffer binding with which to associate the generic vertex attribute.</param>
        public void VertexAttribBinding(uint attribindex, uint bindingindex)
        {
            GetDelegateFor<glVertexAttribBinding>()(attribindex, bindingindex);
        }
        
        /// <summary>
        /// Modify the rate at which generic vertex attributes advance.
        /// </summary>
        /// <param name="bindingindex">The index of the binding whose divisor to modify.</param>
        /// <param name="divisor">The new value for the instance step rate to apply.</param>
        public void VertexBindingDivisor(uint bindingindex, uint divisor)
        {
            GetDelegateFor<glVertexBindingDivisor>()(bindingindex, divisor);
        }

        /// <summary>
        /// Bind a buffer to a vertex buffer bind point.
        /// Available only when When EXT_direct_state_access is present.
        /// </summary>
        /// <param name="vaobj">The vertex array object.</param>
        /// <param name="bindingindex">The index of the vertex buffer binding point to which to bind the buffer.</param>
        /// <param name="buffer">The name of an existing buffer to bind to the vertex buffer binding point.</param>
        /// <param name="offset">The offset of the first element of the buffer.</param>
        /// <param name="stride">The distance between elements within the buffer.</param>
        public void VertexArrayBindVertexBufferEXT(uint vaobj, uint bindingindex, uint buffer, IntPtr offset, uint stride)
        {
            GetDelegateFor<glVertexArrayBindVertexBufferEXT>()(vaobj, bindingindex, buffer, offset, stride);
        }

        /// <summary>
        /// Specify the organization of vertex arrays.
        /// Available only when When EXT_direct_state_access is present.
        /// </summary>
        /// <param name="vaobj">The vertex array object.</param>
        /// <param name="attribindex">The generic vertex attribute array being described.</param>
        /// <param name="size">The number of values per vertex that are stored in the array.</param>
        /// <param name="type">The type of the data stored in the array.</param>
        /// <param name="normalized">GL_TRUE​ if the parameter represents a normalized integer (type​ must be an integer type). GL_FALSE​ otherwise.</param>
        /// <param name="relativeoffset">The offset, measured in basic machine units of the first element relative to the start of the vertex buffer binding this attribute fetches from.</param>
        public void VertexArrayVertexAttribFormatEXT(uint vaobj, uint attribindex, int size, uint type, bool normalized, uint relativeoffset)
        {
            GetDelegateFor<glVertexArrayVertexAttribFormatEXT>()(vaobj, attribindex, size, type, normalized, relativeoffset);
        }

        /// <summary>
        /// Specify the organization of vertex arrays.
        /// Available only when When EXT_direct_state_access is present.
        /// </summary>
        /// <param name="vaobj">The vertex array object.</param>
        /// <param name="attribindex">The generic vertex attribute array being described.</param>
        /// <param name="size">The number of values per vertex that are stored in the array.</param>
        /// <param name="type">The type of the data stored in the array.</param>
        /// <param name="relativeoffset">The offset, measured in basic machine units of the first element relative to the start of the vertex buffer binding this attribute fetches from.</param>
        public void VertexArrayVertexAttribIFormatEXT(uint vaobj, uint attribindex, int size, uint type, uint relativeoffset)
        {
            GetDelegateFor<glVertexArrayVertexAttribIFormatEXT>()(vaobj, attribindex, size, type, relativeoffset);
        }

        /// <summary>
        /// Specify the organization of vertex arrays.
        /// Available only when When EXT_direct_state_access is present.
        /// </summary>
        /// <param name="vaobj">The vertex array object.</param>
        /// <param name="attribindex">The generic vertex attribute array being described.</param>
        /// <param name="size">The number of values per vertex that are stored in the array.</param>
        /// <param name="type">The type of the data stored in the array.</param>
        /// <param name="relativeoffset">The offset, measured in basic machine units of the first element relative to the start of the vertex buffer binding this attribute fetches from.</param>
        public void VertexArrayVertexAttribLFormatEXT(uint vaobj, uint attribindex, int size, uint type, uint relativeoffset)
        {
            GetDelegateFor<glVertexArrayVertexAttribLFormatEXT>()(vaobj, attribindex, size, type, relativeoffset);
        }

        /// <summary>
        /// Associate a vertex attribute and a vertex buffer binding.
        /// Available only when When EXT_direct_state_access is present.
        /// </summary>
        /// <param name="vaobj">The vertex array object.</param>
        /// <param name="attribindex">The index of the attribute to associate with a vertex buffer binding.</param>
        /// <param name="bindingindex">The index of the vertex buffer binding with which to associate the generic vertex attribute.</param>
        public void VertexArrayVertexAttribBindingEXT(uint vaobj, uint attribindex, uint bindingindex)
        {
            GetDelegateFor<glVertexArrayVertexAttribBindingEXT>()(vaobj, attribindex, bindingindex);
        }

        /// <summary>
        /// Modify the rate at which generic vertex attributes advance.
        /// Available only when When EXT_direct_state_access is present.
        /// </summary>
        /// <param name="vaobj">The vertex array object.</param>
        /// <param name="bindingindex">The index of the binding whose divisor to modify.</param>
        /// <param name="divisor">The new value for the instance step rate to apply.</param>
        public void VertexArrayVertexBindingDivisorEXT(uint vaobj, uint bindingindex, uint divisor)
        {
            GetDelegateFor<glVertexArrayVertexBindingDivisorEXT>()(vaobj, bindingindex, divisor);
        }

        //  Delegates
        private delegate void glBindVertexBuffer(uint bindingindex, uint buffer, IntPtr offset, uint stride);
        private delegate void glVertexAttribFormat(uint attribindex, int size, uint type, bool normalized, uint relativeoffset);
        private delegate void glVertexAttribIFormat(uint attribindex, int size, uint type, uint relativeoffset);
        private delegate void glVertexAttribLFormat(uint attribindex, int size, uint type, uint relativeoffset);
        private delegate void glVertexAttribBinding(uint attribindex, uint bindingindex);
        private delegate void glVertexBindingDivisor(uint bindingindex, uint divisor);
        private delegate void glVertexArrayBindVertexBufferEXT(uint vaobj, uint bindingindex, uint buffer, IntPtr offset, uint stride);
        private delegate void glVertexArrayVertexAttribFormatEXT(uint vaobj, uint attribindex, int size, uint type, bool normalized, uint relativeoffset);
        private delegate void glVertexArrayVertexAttribIFormatEXT(uint vaobj, uint attribindex, int size, uint type, uint relativeoffset);
        private delegate void glVertexArrayVertexAttribLFormatEXT(uint vaobj, uint attribindex, int size, uint type, uint relativeoffset);
        private delegate void glVertexArrayVertexAttribBindingEXT(uint vaobj, uint attribindex, uint bindingindex);
        private delegate void glVertexArrayVertexBindingDivisorEXT(uint vaobj, uint bindingindex, uint divisor);

        //  Constants
        public const uint GL_VERTEX_ATTRIB_BINDING                          = 0x82D4;  
        public const uint GL_VERTEX_ATTRIB_RELATIVE_OFFSET                  = 0x82D5;  
        public const uint GL_VERTEX_BINDING_DIVISOR                         = 0x82D6;  
        public const uint GL_VERTEX_BINDING_OFFSET                          = 0x82D7;  
        public const uint GL_VERTEX_BINDING_STRIDE                          = 0x82D8;  
        public const uint GL_VERTEX_BINDING_BUFFER                          = 0x8F4F;
        public const uint GL_MAX_VERTEX_ATTRIB_RELATIVE_OFFSET              = 0x82D9;  
        public const uint GL_MAX_VERTEX_ATTRIB_BINDINGS                     = 0x82DA;  

        #endregion

        #region WGL_NV_DX_interop / WGL_NV_DX_interop2

        public bool DXSetResourceShareHandleNV(IntPtr dxObject, IntPtr shareHandle)
        {
            return (bool)GetDelegateFor<wglDXSetResourceShareHandleNV>()(dxObject, shareHandle);
        }

        public IntPtr DXOpenDeviceNV(IntPtr dxDevice)
        {
            return (IntPtr)GetDelegateFor<wglDXOpenDeviceNV>()(dxDevice);
        }

        public bool DXCloseDeviceNV(IntPtr hDevice)
        {
            return (bool)GetDelegateFor<wglDXCloseDeviceNV>()(hDevice);
        }

        public IntPtr DXRegisterObjectNV(IntPtr hDevice, IntPtr dxObject, uint name, uint type, uint access)
        {
            return (IntPtr)GetDelegateFor<wglDXRegisterObjectNV>()(hDevice, dxObject, name, type, access);
        }

        public bool DXUnregisterObjectNV(IntPtr hDevice, IntPtr hObject)
        {
            return (bool)GetDelegateFor<wglDXUnregisterObjectNV>()(hDevice, hObject);
        }

        public bool DXObjectAccessNV(IntPtr hObject, uint access)
        {
            return (bool)GetDelegateFor<wglDXObjectAccessNV>()(hObject, access);
        }

        public bool DXLockObjectsNV(IntPtr hDevice, IntPtr[] hObjects)
        {
            unsafe
            {
                void** objects = stackalloc void*[hObjects.Length];

                for (int i = 0; i<hObjects.Length; i++)
                {
                    objects[i] = hObjects[i].ToPointer();
                }

                return (bool)GetDelegateFor<wglDXLockObjectsNV>()(hDevice, hObjects.Length, objects);
            }
        }

        public bool DXUnlockObjectsNV(IntPtr hDevice, IntPtr[] hObjects)
        {
            unsafe
            {
                void** objects = stackalloc void*[hObjects.Length];

                for (int i = 0; i<hObjects.Length; i++)
                {
                    objects[i] = hObjects[i].ToPointer();
                }

                return (bool)GetDelegateFor<wglDXUnlockObjectsNV>()(hDevice, hObjects.Length, objects);
            }
        }

        private delegate bool wglDXSetResourceShareHandleNV(IntPtr dxObject, IntPtr shareHandle);
        private delegate IntPtr wglDXOpenDeviceNV(IntPtr dxDevice);
        private delegate bool wglDXCloseDeviceNV(IntPtr hDevice);
        private delegate IntPtr wglDXRegisterObjectNV(IntPtr hDevice, IntPtr dxObject, uint name, uint type, uint access);
        private delegate bool wglDXUnregisterObjectNV(IntPtr hDevice, IntPtr hObject);
        private delegate bool wglDXObjectAccessNV(IntPtr hObject, uint access);
        private unsafe delegate bool wglDXLockObjectsNV(IntPtr hDevice, int count, void** hObjects);
        private unsafe delegate bool wglDXUnlockObjectsNV(IntPtr hDevice, int count, void** hObjects);

        public const uint WGL_ACCESS_READ_ONLY_NV = 0x0000;
        public const uint WGL_ACCESS_READ_WRITE_NV = 0x0001;
        public const uint WGL_ACCESS_WRITE_DISCARD_NV = 0x0002;

        #endregion
    }
}
