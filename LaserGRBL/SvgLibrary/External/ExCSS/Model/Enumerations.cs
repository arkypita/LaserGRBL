
// ReSharper disable once CheckNamespace
namespace ExCSS
{
    internal static class RuleTypes
    {
        internal const string CharacterSet = "charset";
        internal const string Keyframes = "keyframes";
        internal const string Media = "media";
        internal const string Page = "page";
        internal const string Import = "import";
        internal const string FontFace = "font-face";
        internal const string Namespace = "namespace";
        internal const string Supports = "supports";
        internal const string Document = "document";
    }

    internal static class PseudoSelectorPrefix
    {
        internal const string NthChildOdd = "odd";
        internal const string NthChildEven = "even";
        internal const string NthChildN = "n";
        internal const string PseudoFunctionNthchild = "nth-child";
        internal const string PseudoFunctionNthlastchild = "nth-last-child";
        internal const string PseudoFunctionNthOfType = "nth-of-type";
        internal const string PseudoFunctionNthLastOfType = "nth-last-of-type";
        internal const string PseudoRoot = "root";
        internal const string PseudoFirstOfType = "first-of-type";
        internal const string PseudoLastoftype = "last-of-type";
        internal const string PseudoOnlychild = "only-child";
        internal const string PseudoOnlyOfType = "only-of-type";
        internal const string PseudoFirstchild = "first-child";
        internal const string PseudoLastchild = "last-child";
        internal const string PseudoEmpty = "empty";
        internal const string PseudoLink = "link";
        internal const string PseudoVisited = "visited";
        internal const string PseudoActive = "active";
        internal const string PseudoHover = "hover";
        internal const string PseudoFocus = "focus";
        internal const string PseudoTarget = "target";
        internal const string PseudoEnabled = "enabled";
        internal const string PseudoDisabled = "disabled";
        internal const string PseudoChecked = "checked";
        internal const string PseudoUnchecked = "unchecked";
        internal const string PseudoIndeterminate = "indeterminate";
        internal const string PseudoDefault = "default";
        internal const string PseudoValid = "valid";
        internal const string PseudoInvalid = "invalid";
        internal const string PseudoRequired = "required";
        internal const string PseudoInrange = "in-range";
        internal const string PseudoOutofrange = "out-of-range";
        internal const string PseudoOptional = "optional";
        internal const string PseudoReadonly = "read-only";
        internal const string PseudoReadwrite = "read-write";
        internal const string PseudoFunctionDir = "dir";

        internal const string PseudoFunctionNot = "not";
        internal const string PseudoFunctionLang = "lang";
        internal const string PseudoFunctionContains = "contains";
        internal const string PseudoElementBefore = "before";
        internal const string PseudoElementAfter = "after";
        internal const string PseudoElementSelection = "selection";
        internal const string PseudoElementFirstline = "first-line";
        internal const string PseudoElementFirstletter = "first-letter";
    }

    internal static class ErrorMessages
    {
        internal const string InvalidCharacter = "Invalid character detected.";
        internal const string LineBreakEof = "Unexpected line break or EOF.";
        internal const string UnexpectedCommentToken = "The input element is unexpected and has been ignored.";
        internal const string DoubleQuotedString = "Expected double quoted string to terminate before form feed or line feed.";
        internal const string DoubleQuotedStringEof = "Expected double quoted string to terminate before end of file.";
        internal const string SingleQuotedString = "Expected single quoted string to terminate before form feed or line feed.";
        internal const string SingleQuotedStringEof = "Expected single quoted string to terminate before end of file.";
        internal const string InvalidCharacterAfterHash = "Invalid character after #.";
        internal const string InvalidIdentAfterHash = "Invalid character after #.";
        internal const string InvalidUrlEnd = "Expected URL to terminate before line break or end of file.";
        internal const string InvalidUrlQuote = "Expected quotation or open paren in URL.";
        internal const string InvalidUrlCharacter = "Invalid character in URL.";
        internal const string ExpectedCommentEnd = "Expected comment to close before end of file.";
        internal const string Default = "An unexpected error occurred.";
    }

    public enum Combinator
    {
        Child,
        Descendent,
        AdjacentSibling,
        Sibling,
        Namespace
    }

    internal enum GrammarSegment
    {
        String,
        Url,
        UrlPrefix,
        Domain,
        Hash,           //#
        AtRule,         //@
        Ident,
        Function,
        Number,
        Percentage,
        Dimension,
        Range,
        CommentOpen,
        CommentClose,
        Column,
        Delimiter,
        IncludeMatch,   //~=
        DashMatch,      // |=
        PrefixMatch,    // ^=
        SuffixMatch,    // $=
        SubstringMatch, // *=
        NegationMatch,  // !=
        ParenOpen,
        ParenClose,
        CurlyBraceOpen,
        CurlyBracketClose,
        SquareBraceOpen,
        SquareBracketClose,
        Colon,
        Comma,
        Semicolon,
        Whitespace
    }

    public enum RuleType
    {
        Unknown = 0,
        Style = 1,
        Charset = 2,
        Import = 3,
        Media = 4,
        FontFace = 5,
        Page = 6,
        Keyframes = 7,
        Keyframe = 8,
        Namespace = 10,
        CounterStyle = 11,
        Supports = 12,
        Document = 13,
        FontFeatureValues = 14,
        Viewport = 15,
        RegionStyle = 16
    }

    public enum UnitType
    {
        Unknown = 0,
        Number = 1,
        Percentage = 2,
        Ems = 3,
        Exs = 4,
        Pixel = 5,
        Centimeter = 6,
        Millimeter = 7,
        Inch = 8,
        Point = 9,
        Percent = 10,
        Degree = 11,
        Radian = 12,
        Grad = 13,
        Millisecond = 14,
        Second = 15,
        Hertz = 16,
        KiloHertz = 17,
        Dimension = 18,
        String = 19,
        Uri = 20,
        Ident = 21,
        Attribute = 22,
        Counter = 23,
        Rect = 24,
        RGB = 25,
        ViewportWidth = 26,
        ViewportHeight = 28,
        ViewportMin = 29,
        ViewportMax = 30,
        Turn = 31,
    }

    public enum DocumentFunction
    {
        Url,
        UrlPrefix,
        Domain,
        RegExp
    }

    public enum DirectionMode
    {
        LeftToRight,
        RightToLeft
    }

    public enum ParserError
    {
        EndOfFile,
        UnexpectedLineBreak,
        InvalidCharacter,
        UnexpectedCommentToken
    }

    internal enum SelectorOperation
    {
        Data,
        Attribute,
        AttributeOperator,
        AttributeValue,
        AttributeEnd,
        Class,
        PseudoClass,
        PseudoClassFunction,
        PseudoClassFunctionEnd,
        PseudoElement
    }

    internal enum ParsingContext
    {
        DataBlock,
        InSelector,
        InDeclaration,
        AfterProperty,
        BeforeValue,
        InValuePool,
        InValueList,
        InSingleValue,
        InMediaList,
        InMediaValue,
        BeforeImport,
        BeforeCharset,
        BeforeNamespacePrefix,
        AfterNamespacePrefix,
        BeforeFontFace,
        FontfaceData,
        FontfaceProperty,
        AfterInstruction,
        InCondition,
        BeforeKeyframesName,
        BeforeKeyframesData,
        KeyframesData,
        InKeyframeText,
        BeforePageSelector,
        BeforeDocumentFunction,
        InDocumentFunction,
        AfterDocumentFunction,
        BetweenDocumentFunctions,
        InUnknown,
        ValueImportant,
        AfterValue,
        InHexValue,
        InFunction
    }
}
