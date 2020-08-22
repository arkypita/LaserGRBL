using System;
using System.Text;

namespace ExCSS.Model.Extensions
{
    public static class StringExtensions
    {
        public static string Indent(this string value, bool friendlyForamt, int indentation)
        {
            if (!friendlyForamt)
            {
                return value;
            }

            var tabs = new StringBuilder();
            for (var i = 0; i < indentation; i++)
            {
                tabs.Append("\t");
            }

            return string.Format("{0}{1}", tabs, value);
        }

        public static string NewLineIndent(this string value, bool friendlyFormat, int indentation)
        {
            if (!friendlyFormat)
            {
                return value;
            }

            return Environment.NewLine + value.Indent(true, indentation);
        }

        public static string TrimFirstLine(this string value)
        {
            return new StringBuilder(value).TrimFirstLine().ToString();
        }

        public static StringBuilder TrimLastLine(this StringBuilder builder)
        {
            while (builder[builder.Length-1] == '\r' || builder[builder.Length-1] == '\n' || builder[builder.Length-1] == '\t')
            {
                builder.Remove(builder.Length - 1, 1);
            }

            return builder;
        }

        public static StringBuilder TrimFirstLine(this StringBuilder builder)
        {
            while (builder[0] == '\r' || builder[0] == '\n' || builder[0] == '\t')
            {
                builder.Remove(0, 1);
            }

            return builder;
        }
    }
}
