using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SharpGL.Enumerations
{
    /// <summary>
    ///  AccumOp
    /// </summary>
    public enum AccumOperation : uint
    {
        Accum = OpenGL.GL_ACCUM,
        Load = OpenGL.GL_LOAD,
        Return = OpenGL.GL_RETURN,
        Multiple = OpenGL.GL_MULT,
        Add = OpenGL.GL_ADD
    }

    /// <summary>
    /// The alpha function
    /// </summary>
    public enum AlphaTestFunction : uint
    {
        Never = OpenGL.GL_NEVER,
        Less = OpenGL.GL_LESS,
        Equal = OpenGL.GL_EQUAL,
        LessThanOrEqual = OpenGL.GL_LEQUAL,
        Great = OpenGL.GL_GREATER,
        NotEqual = OpenGL.GL_NOTEQUAL,
        GreaterThanOrEqual = OpenGL.GL_GEQUAL,
        Always = OpenGL.GL_ALWAYS,
    }
    
    /// <summary>
    /// The OpenGL Attribute flags.
    /// </summary>
    [Flags]
    public enum AttributeMask : uint
    {
        None = 0,
        Current = OpenGL.GL_CURRENT_BIT,
        Point = OpenGL.GL_POINT_BIT,
        Line = OpenGL.GL_LINE_BIT,
        Polygon = OpenGL.GL_POLYGON_BIT,
        PolygonStipple = OpenGL.GL_POLYGON_STIPPLE_BIT,
        PixelMode = OpenGL.GL_PIXEL_MODE_BIT,
        Lighting = OpenGL.GL_LIGHTING_BIT,
        Fog = OpenGL.GL_FOG_BIT,
        DepthBuffer = OpenGL.GL_DEPTH_BUFFER_BIT,
        AccumBuffer = OpenGL.GL_ACCUM_BUFFER_BIT,
        StencilBuffer = OpenGL.GL_STENCIL_BUFFER_BIT,
        Viewport = OpenGL.GL_VIEWPORT_BIT,
        Transform = OpenGL.GL_TRANSFORM_BIT,
        Enable = OpenGL.GL_ENABLE_BIT,
        ColorBuffer = OpenGL.GL_COLOR_BUFFER_BIT,
        Hint = OpenGL.GL_HINT_BIT,
        Eval = OpenGL.GL_EVAL_BIT,
        List = OpenGL.GL_LIST_BIT,
        Texture = OpenGL.GL_TEXTURE_BIT,
        Scissor = OpenGL.GL_SCISSOR_BIT,
        All = OpenGL.GL_ALL_ATTRIB_BITS,
    }

    /// <summary>
    /// The begin mode.
    /// </summary>
    public enum BeginMode : uint
    {
        Points = OpenGL.GL_POINTS,
        Lines = OpenGL.GL_LINES,
        LineLoop = OpenGL.GL_LINE_LOOP,
        LineStrip = OpenGL.GL_LINE_STRIP,
        Triangles = OpenGL.GL_TRIANGLES,
        TriangleString = OpenGL.GL_TRIANGLE_STRIP,
        TriangleFan = OpenGL.GL_TRIANGLE_FAN,
        Quads= OpenGL.GL_QUADS,
        QuadStrip = OpenGL.GL_QUAD_STRIP,
        Polygon = OpenGL.GL_POLYGON
    }
    
    /// <summary>
    /// BlendingDestinationFactor
    /// </summary>
    public enum BlendingDestinationFactor : uint
    {
        Zero = OpenGL.GL_ZERO,
        One = OpenGL.GL_ONE,
        SourceColor = OpenGL.GL_SRC_COLOR,
        OneMinusSourceColor = OpenGL.GL_ONE_MINUS_SRC_COLOR,
        SourceAlpha = OpenGL.GL_SRC_ALPHA,
        OneMinusSourceAlpha = OpenGL.GL_ONE_MINUS_SRC_ALPHA,
        DestinationAlpha = OpenGL.GL_DST_ALPHA,
        OneMinusDestinationAlpha = OpenGL.GL_ONE_MINUS_DST_ALPHA,
    }

    /// <summary>
    /// The blending source factor.
    /// </summary>
    public enum BlendingSourceFactor : uint
    {
        DestinationColor = OpenGL.GL_DST_COLOR,
        OneMinusDestinationColor = OpenGL.GL_ONE_MINUS_DST_COLOR,
        SourceAlphaSaturate = OpenGL.GL_SRC_ALPHA_SATURATE,
        /// <summary>
        /// 
        /// </summary>
        SourceAlpha = OpenGL.GL_SRC_ALPHA
    }
    
    /// <summary>
    /// The Clip Plane Name
    /// </summary>
    public enum ClipPlaneName : uint
    {
        ClipPlane0 = OpenGL.GL_CLIP_PLANE0,
        ClipPlane1 = OpenGL.GL_CLIP_PLANE1,
        ClipPlane2 = OpenGL.GL_CLIP_PLANE2,
        ClipPlane3 = OpenGL.GL_CLIP_PLANE3,
        ClipPlane4 = OpenGL.GL_CLIP_PLANE4,
        ClipPlane5 = OpenGL.GL_CLIP_PLANE5
    }

    /// <summary>
    /// The Cull Face mode.
    /// </summary>
    public enum FaceMode : uint
    {
        /// <summary>
        /// 
        /// </summary>
        Front = OpenGL.GL_FRONT,
        FrontAndBack = OpenGL.GL_FRONT_AND_BACK,
        Back = OpenGL.GL_BACK,
    }

    /// <summary>
    /// The Data Type.
    /// </summary>
    public enum DataType : uint
    {
        Byte = OpenGL.GL_BYTE,
        UnsignedByte = OpenGL.GL_UNSIGNED_BYTE,
        Short = OpenGL.GL_SHORT,
        UnsignedShort = OpenGL.GL_UNSIGNED_SHORT,
        Int = OpenGL.GL_INT,
        UnsignedInt = OpenGL.GL_UNSIGNED_INT,
        Float = OpenGL.GL_FLOAT,
        TwoBytes = OpenGL.GL_2_BYTES,
        ThreeBytes = OpenGL.GL_3_BYTES,
        FourBytes = OpenGL.GL_4_BYTES,
        /// <summary>
        /// 
        /// </summary>
        Double= OpenGL.GL_DOUBLE
    }

    /// <summary>
    /// The depth function
    /// </summary>
    public enum DepthFunction : uint
    {
        Never = OpenGL.GL_NEVER,
        Less = OpenGL.GL_LESS,
        Equal = OpenGL.GL_EQUAL,
        LessThanOrEqual = OpenGL.GL_LEQUAL,
        Great = OpenGL.GL_GREATER,
        NotEqual = OpenGL.GL_NOTEQUAL,
        GreaterThanOrEqual = OpenGL.GL_GEQUAL,
        Always = OpenGL.GL_ALWAYS,
    }

    /// <summary>
    /// The Draw Buffer Mode
    /// </summary>
    public enum DrawBufferMode : uint
    {
        None = OpenGL.GL_NONE,
        FrontLeft = OpenGL.GL_FRONT_LEFT,
        FrontRight = OpenGL.GL_FRONT_RIGHT,
        BackLeft = OpenGL.GL_BACK_LEFT,
        BackRight = OpenGL.GL_BACK_RIGHT,
        Front = OpenGL.GL_FRONT,
        Back = OpenGL.GL_BACK,
        Left = OpenGL.GL_LEFT,
        Right = OpenGL.GL_RIGHT,
        FrontAndBack = OpenGL.GL_FRONT_AND_BACK,
        Auxilliary0= OpenGL.GL_AUX0,
        Auxilliary1 = OpenGL.GL_AUX1,
        Auxilliary2 = OpenGL.GL_AUX2,
        Auxilliary3 = OpenGL.GL_AUX3,
    }
    
    /// <summary>
    /// Error Code
    /// </summary>
    public enum ErrorCode : uint
    {
        NoError = OpenGL.GL_NO_ERROR,
        InvalidEnum = OpenGL.GL_INVALID_ENUM,
        InvalidValue = OpenGL.GL_INVALID_VALUE,
        InvalidOperation = OpenGL.GL_INVALID_OPERATION,
        StackOverflow = OpenGL.GL_STACK_OVERFLOW,
        StackUnderflow = OpenGL.GL_STACK_UNDERFLOW,
        OutOfMemory = OpenGL.GL_OUT_OF_MEMORY
    }

    /// <summary>
    /// FeedBackMode
    /// </summary>
    public enum FeedbackMode : uint
    {
        TwoD = OpenGL.GL_2D,
        ThreeD = OpenGL.GL_3D,
        FourD = OpenGL.GL_4D_COLOR,
        ThreeDColorTexture = OpenGL.GL_3D_COLOR_TEXTURE,
        FourDColorTexture = OpenGL.GL_4D_COLOR_TEXTURE
    }

    /// <summary>
    /// The Feedback Token
    /// </summary>
    public enum FeedbackToken : uint
    {
        PassThroughToken = OpenGL.GL_PASS_THROUGH_TOKEN,
        PointToken = OpenGL.GL_POINT_TOKEN,
        LineToken = OpenGL.GL_LINE_TOKEN,
        PolygonToken = OpenGL.GL_POLYGON_TOKEN,
        BitmapToken = OpenGL.GL_BITMAP_TOKEN,
        DrawPixelToken = OpenGL.GL_DRAW_PIXEL_TOKEN,
        CopyPixelToken = OpenGL.GL_COPY_PIXEL_TOKEN,
        LineResetToken = OpenGL.GL_LINE_RESET_TOKEN
    }

    /// <summary>
    /// The Fog Mode.
    /// </summary>
    public enum FogMode : uint
    {
	   	Exp = OpenGL.GL_EXP,

        /// <summary>
        /// 
        /// </summary>
		Exp2 = OpenGL.GL_EXP2,
	}
	
    /// <summary>
    /// GetMapTarget 
    /// </summary>
    public enum GetMapTarget : uint
    {
        Coeff = OpenGL.GL_COEFF,
        Order = OpenGL.GL_ORDER,
        Domain = OpenGL.GL_DOMAIN
    }

    public enum GetTarget : uint
    {
        CurrentColor = OpenGL.GL_CURRENT_COLOR,
        CurrentIndex = OpenGL.GL_CURRENT_INDEX,
        CurrentNormal = OpenGL.GL_CURRENT_NORMAL,
        CurrentTextureCoords = OpenGL.GL_CURRENT_TEXTURE_COORDS,
        CurrentRasterColor = OpenGL.GL_CURRENT_RASTER_COLOR,
        CurrentRasterIndex = OpenGL.GL_CURRENT_RASTER_INDEX,
        CurrentRasterTextureCoords = OpenGL.GL_CURRENT_RASTER_TEXTURE_COORDS,
        CurrentRasterPosition = OpenGL.GL_CURRENT_RASTER_POSITION,
        CurrentRasterPositionValid = OpenGL.GL_CURRENT_RASTER_POSITION_VALID,
        CurrentRasterDistance = OpenGL.GL_CURRENT_RASTER_DISTANCE,
        PointSmooth = OpenGL.GL_POINT_SMOOTH,
        PointSize = OpenGL.GL_POINT_SIZE,
        PointSizeRange = OpenGL.GL_POINT_SIZE_RANGE,
        PointSizeGranularity = OpenGL.GL_POINT_SIZE_GRANULARITY,
        LineSmooth = OpenGL.GL_LINE_SMOOTH,
        LineWidth = OpenGL.GL_LINE_WIDTH,
        LineWidthRange = OpenGL.GL_LINE_WIDTH_RANGE,
        LineWidthGranularity = OpenGL.GL_LINE_WIDTH_GRANULARITY,
        LineStipple = OpenGL.GL_LINE_STIPPLE,
        LineStipplePattern = OpenGL.GL_LINE_STIPPLE_PATTERN,
        LineStippleRepeat = OpenGL.GL_LINE_STIPPLE_REPEAT,
        ListMode = OpenGL.GL_LIST_MODE,
        MaxListNesting = OpenGL.GL_MAX_LIST_NESTING,
        ListBase = OpenGL.GL_LIST_BASE,
        ListIndex = OpenGL.GL_LIST_INDEX,
        PolygonMode = OpenGL.GL_POLYGON_MODE,
        PolygonSmooth = OpenGL.GL_POLYGON_SMOOTH,
        PolygonStipple = OpenGL.GL_POLYGON_STIPPLE,
        EdgeFlag = OpenGL.GL_EDGE_FLAG,
        CullFace = OpenGL.GL_CULL_FACE,
        CullFaceMode = OpenGL.GL_CULL_FACE_MODE,
        FrontFace = OpenGL.GL_FRONT_FACE,
        Lighting = OpenGL.GL_LIGHTING,
        LightModelLocalViewer = OpenGL.GL_LIGHT_MODEL_LOCAL_VIEWER,
        LightModelTwoSide = OpenGL.GL_LIGHT_MODEL_TWO_SIDE,
        LightModelAmbient = OpenGL.GL_LIGHT_MODEL_AMBIENT,
        ShadeModel = OpenGL.GL_SHADE_MODEL,
        ColorMaterialFace = OpenGL.GL_COLOR_MATERIAL_FACE,
        ColorMaterialParameter = OpenGL.GL_COLOR_MATERIAL_PARAMETER,
        ColorMaterial = OpenGL.GL_COLOR_MATERIAL,
        Fog = OpenGL.GL_FOG,
        FogIndex = OpenGL.GL_FOG_INDEX,
        FogDensity = OpenGL.GL_FOG_DENSITY,
        FogStart = OpenGL.GL_FOG_START,
        FogEnd = OpenGL.GL_FOG_END,
        FogMode = OpenGL.GL_FOG_MODE,
        FogColor = OpenGL.GL_FOG_COLOR,
        DepthRange = OpenGL.GL_DEPTH_RANGE,
        DepthTest = OpenGL.GL_DEPTH_TEST,
        DepthWritemask = OpenGL.GL_DEPTH_WRITEMASK,
        DepthClearValue = OpenGL.GL_DEPTH_CLEAR_VALUE,
        DepthFunc = OpenGL.GL_DEPTH_FUNC,
        AccumClearValue = OpenGL.GL_ACCUM_CLEAR_VALUE,
        StencilTest = OpenGL.GL_STENCIL_TEST,
        StencilClearValue = OpenGL.GL_STENCIL_CLEAR_VALUE,
        StencilFunc = OpenGL.GL_STENCIL_FUNC,
        StencilValueMask = OpenGL.GL_STENCIL_VALUE_MASK,
        StencilFail = OpenGL.GL_STENCIL_FAIL,
        StencilPassDepthFail = OpenGL.GL_STENCIL_PASS_DEPTH_FAIL,
        StencilPassDepthPass = OpenGL.GL_STENCIL_PASS_DEPTH_PASS,
        StencilRef = OpenGL.GL_STENCIL_REF,
        StencilWritemask = OpenGL.GL_STENCIL_WRITEMASK,
        MatrixMode = OpenGL.GL_MATRIX_MODE,
        Normalize = OpenGL.GL_NORMALIZE,
        Viewport = OpenGL.GL_VIEWPORT,
        ModelviewStackDepth = OpenGL.GL_MODELVIEW_STACK_DEPTH,
        ProjectionStackDepth = OpenGL.GL_PROJECTION_STACK_DEPTH,
        TextureStackDepth = OpenGL.GL_TEXTURE_STACK_DEPTH,
        ModelviewMatix = OpenGL.GL_MODELVIEW_MATRIX,
        ProjectionMatrix = OpenGL.GL_PROJECTION_MATRIX,
        TextureMatrix = OpenGL.GL_TEXTURE_MATRIX,
        AttribStackDepth = OpenGL.GL_ATTRIB_STACK_DEPTH,
        ClientAttribStackDepth = OpenGL.GL_CLIENT_ATTRIB_STACK_DEPTH,
        AlphaTest = OpenGL.GL_ALPHA_TEST,
        AlphaTestFunc = OpenGL.GL_ALPHA_TEST_FUNC,
        AlphaTestRef = OpenGL.GL_ALPHA_TEST_REF,
        Dither = OpenGL.GL_DITHER,
        BlendDst = OpenGL.GL_BLEND_DST,
        BlendSrc = OpenGL.GL_BLEND_SRC,
        Blend = OpenGL.GL_BLEND,
        LogicOpMode = OpenGL.GL_LOGIC_OP_MODE,
        IndexLogicOp = OpenGL.GL_INDEX_LOGIC_OP,
        ColorLogicOp = OpenGL.GL_COLOR_LOGIC_OP,
        AuxBuffers = OpenGL.GL_AUX_BUFFERS,
        DrawBuffer = OpenGL.GL_DRAW_BUFFER,
        ReadBuffer = OpenGL.GL_READ_BUFFER,
        ScissorBox = OpenGL.GL_SCISSOR_BOX,
        ScissorTest = OpenGL.GL_SCISSOR_TEST,
        IndexClearValue = OpenGL.GL_INDEX_CLEAR_VALUE,
        IndexWritemask = OpenGL.GL_INDEX_WRITEMASK,
        ColorClearValue = OpenGL.GL_COLOR_CLEAR_VALUE,
        ColorWritemask = OpenGL.GL_COLOR_WRITEMASK,
        IndexMode = OpenGL.GL_INDEX_MODE,
        RgbaMode = OpenGL.GL_RGBA_MODE,
        DoubleBuffer = OpenGL.GL_DOUBLEBUFFER,
        Stereo = OpenGL.GL_STEREO,
        RenderMode = OpenGL.GL_RENDER_MODE,
        PerspectiveCorrectionHint = OpenGL.GL_PERSPECTIVE_CORRECTION_HINT,
        PointSmoothHint = OpenGL.GL_POINT_SMOOTH_HINT,
        LineSmoothHint = OpenGL.GL_LINE_SMOOTH_HINT,
        PolygonSmoothHint = OpenGL.GL_POLYGON_SMOOTH_HINT,
        FogHint = OpenGL.GL_FOG_HINT,
        TextureGenS = OpenGL.GL_TEXTURE_GEN_S,
        TextureGenT = OpenGL.GL_TEXTURE_GEN_T,
        TextureGenR = OpenGL.GL_TEXTURE_GEN_R,
        TextureGenQ = OpenGL.GL_TEXTURE_GEN_Q,
        PixelMapItoI = OpenGL.GL_PIXEL_MAP_I_TO_I,
        PixelMapStoS = OpenGL.GL_PIXEL_MAP_S_TO_S,
        PixelMapItoR = OpenGL.GL_PIXEL_MAP_I_TO_R,
        PixelMapItoG = OpenGL.GL_PIXEL_MAP_I_TO_G,
        PixelMapItoB = OpenGL.GL_PIXEL_MAP_I_TO_B,
        PixelMapItoA = OpenGL.GL_PIXEL_MAP_I_TO_A,
        PixelMapRtoR = OpenGL.GL_PIXEL_MAP_R_TO_R,
        PixelMapGtoG = OpenGL.GL_PIXEL_MAP_G_TO_G,
        PixelMapBtoB = OpenGL.GL_PIXEL_MAP_B_TO_B,
        PixelMapAtoA = OpenGL.GL_PIXEL_MAP_A_TO_A,
        PixelMapItoISize = OpenGL.GL_PIXEL_MAP_I_TO_I_SIZE,
        PixelMapStoSSize = OpenGL.GL_PIXEL_MAP_S_TO_S_SIZE,
        PixelMapItoRSize = OpenGL.GL_PIXEL_MAP_I_TO_R_SIZE,
        PixelMapItoGSize = OpenGL.GL_PIXEL_MAP_I_TO_G_SIZE,
        PixelMapItoBSize = OpenGL.GL_PIXEL_MAP_I_TO_B_SIZE,
        PixelMapItoASize = OpenGL.GL_PIXEL_MAP_I_TO_A_SIZE,
        PixelMapRtoRSize = OpenGL.GL_PIXEL_MAP_R_TO_R_SIZE,
        PixelMapGtoGSize = OpenGL.GL_PIXEL_MAP_G_TO_G_SIZE,
        PixelMapBtoBSize = OpenGL.GL_PIXEL_MAP_B_TO_B_SIZE,
        PixelMapAtoASize = OpenGL.GL_PIXEL_MAP_A_TO_A_SIZE,
        UnpackSwapBytes = OpenGL.GL_UNPACK_SWAP_BYTES,
        LsbFirst = OpenGL.GL_UNPACK_LSB_FIRST,
        UnpackRowLength = OpenGL.GL_UNPACK_ROW_LENGTH,
        UnpackSkipRows = OpenGL.GL_UNPACK_SKIP_ROWS,
        UnpackSkipPixels = OpenGL.GL_UNPACK_SKIP_PIXELS,
        UnpackAlignment = OpenGL.GL_UNPACK_ALIGNMENT,
        PackSwapBytes = OpenGL.GL_PACK_SWAP_BYTES,
        PackLsbFirst = OpenGL.GL_PACK_LSB_FIRST,
        PackRowLength = OpenGL.GL_PACK_ROW_LENGTH,
        PackSkipRows = OpenGL.GL_PACK_SKIP_ROWS,
        PackSkipPixels = OpenGL.GL_PACK_SKIP_PIXELS,
        PackAlignment = OpenGL.GL_PACK_ALIGNMENT,
        MapColor = OpenGL.GL_MAP_COLOR,
        MapStencil = OpenGL.GL_MAP_STENCIL,
        IndexShift = OpenGL.GL_INDEX_SHIFT,
        IndexOffset = OpenGL.GL_INDEX_OFFSET,
        RedScale = OpenGL.GL_RED_SCALE,
        RedBias = OpenGL.GL_RED_BIAS,
        ZoomX = OpenGL.GL_ZOOM_X,
        ZoomY = OpenGL.GL_ZOOM_Y,
        GreenScale = OpenGL.GL_GREEN_SCALE,
        GreenBias = OpenGL.GL_GREEN_BIAS,
        BlueScale = OpenGL.GL_BLUE_SCALE,
        BlueBias = OpenGL.GL_BLUE_BIAS,
        AlphaScale = OpenGL.GL_ALPHA_SCALE,
        AlphaBias = OpenGL.GL_ALPHA_BIAS,
        DepthScale = OpenGL.GL_DEPTH_SCALE,
        DepthBias = OpenGL.GL_DEPTH_BIAS,
        MapEvalOrder = OpenGL.GL_MAX_EVAL_ORDER,
        MaxLights = OpenGL.GL_MAX_LIGHTS,
        MaxClipPlanes = OpenGL.GL_MAX_CLIP_PLANES,
        MaxTextureSize = OpenGL.GL_MAX_TEXTURE_SIZE,
        MapPixelMapTable = OpenGL.GL_MAX_PIXEL_MAP_TABLE,
        MaxAttribStackDepth = OpenGL.GL_MAX_ATTRIB_STACK_DEPTH,
        MaxModelviewStackDepth = OpenGL.GL_MAX_MODELVIEW_STACK_DEPTH,
        MaxNameStackDepth = OpenGL.GL_MAX_NAME_STACK_DEPTH,
        MaxProjectionStackDepth = OpenGL.GL_MAX_PROJECTION_STACK_DEPTH,
        MaxTextureStackDepth = OpenGL.GL_MAX_TEXTURE_STACK_DEPTH,
        MaxViewportDims = OpenGL.GL_MAX_VIEWPORT_DIMS,
        MaxClientAttribStackDepth = OpenGL.GL_MAX_CLIENT_ATTRIB_STACK_DEPTH,
        SubpixelBits = OpenGL.GL_SUBPIXEL_BITS,
        IndexBits = OpenGL.GL_INDEX_BITS,
        RedBits = OpenGL.GL_RED_BITS,
        GreenBits = OpenGL.GL_GREEN_BITS,
        BlueBits = OpenGL.GL_BLUE_BITS,
        AlphaBits = OpenGL.GL_ALPHA_BITS,
        DepthBits = OpenGL.GL_DEPTH_BITS,
        StencilBits = OpenGL.GL_STENCIL_BITS,
        AccumRedBits = OpenGL.GL_ACCUM_RED_BITS,
        AccumGreenBits = OpenGL.GL_ACCUM_GREEN_BITS,
        AccumBlueBits = OpenGL.GL_ACCUM_BLUE_BITS,
        AccumAlphaBits = OpenGL.GL_ACCUM_ALPHA_BITS,
        NameStackDepth = OpenGL.GL_NAME_STACK_DEPTH,
        AutoNormal = OpenGL.GL_AUTO_NORMAL,
        Map1Color4 = OpenGL.GL_MAP1_COLOR_4,
        Map1Index = OpenGL.GL_MAP1_INDEX,
        Map1Normal = OpenGL.GL_MAP1_NORMAL,
        Map1TextureCoord1 = OpenGL.GL_MAP1_TEXTURE_COORD_1,
        Map1TextureCoord2 = OpenGL.GL_MAP1_TEXTURE_COORD_2,
        Map1TextureCoord3 = OpenGL.GL_MAP1_TEXTURE_COORD_3,
        Map1TextureCoord4 = OpenGL.GL_MAP1_TEXTURE_COORD_4,
        Map1Vertex3 = OpenGL.GL_MAP1_VERTEX_3,
        Map1Vertex4 = OpenGL.GL_MAP1_VERTEX_4,
        Map2Color4 = OpenGL.GL_MAP2_COLOR_4,
        Map2Index = OpenGL.GL_MAP2_INDEX,
        Map2Normal = OpenGL.GL_MAP2_NORMAL,
        Map2TextureCoord1 = OpenGL.GL_MAP2_TEXTURE_COORD_1,
        Map2TextureCoord2 = OpenGL.GL_MAP2_TEXTURE_COORD_2,
        Map2TextureCoord3 = OpenGL.GL_MAP2_TEXTURE_COORD_3,
        Map2TextureCoord4 = OpenGL.GL_MAP2_TEXTURE_COORD_4,
        Map2Vertex3 = OpenGL.GL_MAP2_VERTEX_3,
        Map2Vertex4 = OpenGL.GL_MAP2_VERTEX_4,
        Map1GridDomain = OpenGL.GL_MAP1_GRID_DOMAIN,
        Map1GridSegments = OpenGL.GL_MAP1_GRID_SEGMENTS,
        Map2GridDomain = OpenGL.GL_MAP2_GRID_DOMAIN,
        Map2GridSegments = OpenGL.GL_MAP2_GRID_SEGMENTS,
        Texture1D = OpenGL.GL_TEXTURE_1D,
        Texture2D = OpenGL.GL_TEXTURE_2D,
        FeedbackBufferPointer = OpenGL.GL_FEEDBACK_BUFFER_POINTER,
        FeedbackBufferSize = OpenGL.GL_FEEDBACK_BUFFER_SIZE,
        FeedbackBufferType = OpenGL.GL_FEEDBACK_BUFFER_TYPE,
        SelectionBufferPointer = OpenGL.GL_SELECTION_BUFFER_POINTER,
        SelectionBufferSize = OpenGL.GL_SELECTION_BUFFER_SIZE
    }	

    /// <summary>
    /// The Front Face Mode.
    /// </summary>
    public enum FrontFaceMode : uint
    {
        ClockWise = OpenGL.GL_CW,
        CounterClockWise = OpenGL.GL_CCW,
    }


    /// <summary>
    /// The hint mode.
    /// </summary>
	public enum HintMode : uint
    {
		DontCare = OpenGL.GL_DONT_CARE,
		Fastest = OpenGL.GL_FASTEST,
        /// <summary>
        /// The 
        /// </summary>
        Nicest = OpenGL.GL_NICEST
    }

    /// <summary>
    /// The hint target.
    /// </summary>
    public enum HintTarget : uint
    {
        PerspectiveCorrection = OpenGL.GL_PERSPECTIVE_CORRECTION_HINT,
        PointSmooth = OpenGL.GL_POINT_SMOOTH_HINT,
        LineSmooth = OpenGL.GL_LINE_SMOOTH_HINT,
        PolygonSmooth = OpenGL.GL_POLYGON_SMOOTH_HINT,
        Fog = OpenGL.GL_FOG_HINT
    }
     
    /// <summary>
    /// LightName
    /// </summary>
    public enum LightName : uint
    {
		Light0 = OpenGL.GL_LIGHT0  ,
        Light1 = OpenGL.GL_LIGHT1,
        Light2 = OpenGL.GL_LIGHT2,
        Light3 = OpenGL.GL_LIGHT3,
        Light4 = OpenGL.GL_LIGHT4,
        Light5 = OpenGL.GL_LIGHT5,
        Light6 = OpenGL.GL_LIGHT6,
        Light7 = OpenGL.GL_LIGHT7  
    }
	
    /// <summary>
    /// LightParameter
    /// </summary>
    public enum LightParameter : uint
    {
        Ambient = OpenGL.GL_AMBIENT,
        Diffuse = OpenGL.GL_DIFFUSE,
        Specular = OpenGL.GL_SPECULAR,
        Position = OpenGL.GL_POSITION,
        SpotDirection = OpenGL.GL_SPOT_DIRECTION,
        SpotExponent = OpenGL.GL_SPOT_EXPONENT,
        SpotCutoff = OpenGL.GL_SPOT_CUTOFF,
        ConstantAttenuatio = OpenGL.GL_CONSTANT_ATTENUATION,
        LinearAttenuation = OpenGL.GL_LINEAR_ATTENUATION,
        QuadraticAttenuation = OpenGL.GL_QUADRATIC_ATTENUATION
    }

    /// <summary>
    /// The Light Model Parameter.
    /// </summary>
    public enum LightModelParameter : uint
    {
        LocalViewer = OpenGL.GL_LIGHT_MODEL_LOCAL_VIEWER,
        TwoSide = OpenGL.GL_LIGHT_MODEL_TWO_SIDE,
        Ambient = OpenGL.GL_LIGHT_MODEL_AMBIENT
    }

    /// <summary>
    /// The Logic Op
    /// </summary>
    public enum LogicOp : uint
    {
        Clear = OpenGL.GL_CLEAR,
        And = OpenGL.GL_AND,
        AndReverse  = OpenGL.GL_AND_REVERSE,
        Copy = OpenGL.GL_COPY,
        AndInverted = OpenGL.GL_AND_INVERTED,
        NoOp= OpenGL.GL_NOOP,
        XOr = OpenGL.GL_XOR,
        Or = OpenGL.GL_OR,
        NOr= OpenGL.GL_NOR,
        Equiv = OpenGL.GL_EQUIV,
        Invert = OpenGL.GL_INVERT,
        OrReverse = OpenGL.GL_OR_REVERSE,
        CopyInverted = OpenGL.GL_COPY_INVERTED,
        OrInverted = OpenGL.GL_OR_INVERTED,
        NAnd= OpenGL.GL_NAND,
        Set = OpenGL.GL_SET,
    }

    /// <summary>
    /// The matrix mode.
    /// </summary>
    public enum MatrixMode : uint
    {
        Modelview = OpenGL.GL_MODELVIEW,
        Projection = OpenGL.GL_PROJECTION,
        Texture = OpenGL.GL_TEXTURE
    }

    /// <summary>
    /// The pixel transfer parameter name
    /// </summary>
    public enum PixelTransferParameterName : uint
    {
        MapColor = OpenGL.GL_MAP_COLOR,
        MapStencil = OpenGL.GL_MAP_STENCIL,
        IndexShift = OpenGL.GL_INDEX_SHIFT,
        IndexOffset = OpenGL.GL_INDEX_OFFSET,
        RedScale = OpenGL.GL_RED_SCALE,
        RedBias = OpenGL.GL_RED_BIAS,
        ZoomX = OpenGL.GL_ZOOM_X,
        ZoomY = OpenGL.GL_ZOOM_Y,
        GreenScale = OpenGL.GL_GREEN_SCALE,
        GreenBias = OpenGL.GL_GREEN_BIAS,
        BlueScale = OpenGL.GL_BLUE_SCALE,
        BlueBias = OpenGL.GL_BLUE_BIAS,
        AlphaScale = OpenGL.GL_ALPHA_SCALE,
        AlphaBias = OpenGL.GL_ALPHA_BIAS,
        DepthScale = OpenGL.GL_DEPTH_SCALE,
        DepthBias = OpenGL.GL_DEPTH_BIAS
    }

    /// <summary>
    /// The Polygon mode.
    /// </summary>
    public enum PolygonMode : uint
    {
        /// <summary>
        /// Render as points.
        /// </summary>
        Points = OpenGL.GL_POINT,

        /// <summary>
        /// Render as lines.
        /// </summary>
        Lines = OpenGL.GL_LINE,

        /// <summary>
        /// Render as filled.
        /// </summary>
        Filled = OpenGL.GL_FILL
    }
    
    /// <summary>
    /// Rendering Mode 
    /// </summary>
    public enum RenderingMode: uint
    {
        Render = OpenGL.GL_RENDER,
        Feedback = OpenGL.GL_FEEDBACK,
        Select = OpenGL.GL_SELECT
    }

    /// <summary>
    /// ShadingModel
    /// </summary>
	public enum ShadeModel : uint
    {
        Flat = OpenGL.GL_FLAT,
        Smooth = OpenGL.GL_SMOOTH
    }

    /// <summary>
    /// The stencil function
    /// </summary>
    public enum StencilFunction : uint
    {
        Never = OpenGL.GL_NEVER,
        Less = OpenGL.GL_LESS,
        Equal = OpenGL.GL_EQUAL,
        LessThanOrEqual = OpenGL.GL_LEQUAL,
        Great = OpenGL.GL_GREATER,
        NotEqual = OpenGL.GL_NOTEQUAL,
        GreaterThanOrEqual = OpenGL.GL_GEQUAL,
        Always = OpenGL.GL_ALWAYS,
    }

    /// <summary>
    /// The stencil operation.
    /// </summary>
    public enum StencilOperation : uint
    {
        Keep = OpenGL.GL_KEEP,
        Replace = OpenGL.GL_REPLACE,
        Increase = OpenGL.GL_INCR,
        Decrease = OpenGL.GL_DECR,
        Zero = OpenGL.GL_ZERO,
        IncreaseWrap = OpenGL.GL_INCR_WRAP,
        DecreaseWrap = OpenGL.GL_DECR_WRAP,
        Invert = OpenGL.GL_INVERT
    }    
    
    /// <summary>
    /// GetTextureParameter
    /// </summary>
    public enum TextureParameter : uint
    {
        TextureWidth = OpenGL.GL_TEXTURE_WIDTH,
        TextureHeight = OpenGL.GL_TEXTURE_HEIGHT,
        TextureInternalFormat = OpenGL.GL_TEXTURE_INTERNAL_FORMAT,
        TextureBorderColor = OpenGL.GL_TEXTURE_BORDER_COLOR,
        TextureBorder = OpenGL.GL_TEXTURE_BORDER
    }

    /// <summary>
    /// Texture target.
    /// </summary>
    public enum TextureTarget : uint
    {
        Texture1D = OpenGL.GL_TEXTURE_1D,
        Texture2D = OpenGL.GL_TEXTURE_2D,
        Texture3D = OpenGL.GL_TEXTURE_3D
    }
}
