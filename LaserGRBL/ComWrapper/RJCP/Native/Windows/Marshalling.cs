namespace RJCP.IO.Ports.Native.Windows
{
    using System.Collections.Generic;

    internal static class Marshalling
    {
        /// <summary>
        /// Gets a list of strings from a Win32 API MULTI_SZ type.
        /// </summary>
        /// <param name="blob">The blob of character data to convert from.</param>
        /// <returns>A list of strings.</returns>
        /// <remarks>
        /// Getting the multiple strings is used in many different Win32 API, not just the registry. It iterates through
        /// the blob, separating strings by the NUL character. The end of the list has two NUL characters.
        /// </remarks>
        public static List<string> GetMultiSz(char[] blob)
        {
            return GetMultiSz(blob, blob.Length);
        }

        /// <summary>
        /// Gets a list of strings from a Win32 API MULTI_SZ type.
        /// </summary>
        /// <param name="blob">The blob of character data to convert from.</param>
        /// <param name="blobLen">The number of bytes returned for the blob.</param>
        /// <returns>A list of strings.</returns>
        /// <remarks>
        /// Getting the multiple strings is used in many different Win32 API, not just the registry. It iterates through
        /// the blob, separating strings by the NUL character. The end of the list has two NUL characters.
        /// </remarks>
        public static List<string> GetMultiSz(char[] blob, int blobLen)
        {
            List<string> strings = new List<string>();
            int cur = 0;

            while (cur < blobLen) {
                int nextNull = cur;
                while (nextNull < blobLen && blob[nextNull] != (char)0) {
                    nextNull++;
                }

                if (nextNull < blobLen) {
                    if (nextNull - cur > 0) {
                        strings.Add(new string(blob, cur, nextNull - cur));
                    } else {
                        // we found an empty string.  But if we're at the end of the data,
                        // it's just the extra null terminator.
                        if (nextNull != blobLen - 1)
                            strings.Add(string.Empty);
                    }
                } else {
                    strings.Add(new string(blob, cur, blobLen - cur));
                }
                cur = nextNull + 1;
            }
            return strings;
        }

        /// <summary>
        /// Gets a list of strings from a Win32 API MULTI_SZ type.
        /// </summary>
        /// <param name="blob">The blob of character data to convert from.</param>
        /// <param name="blobLen">The number of bytes returned for the blob.</param>
        /// <returns>A list of strings.</returns>
        /// <remarks>
        /// Getting the multiple strings is used in many different Win32 API, not just the registry. It iterates through
        /// the blob, separating strings by the NUL character. The end of the list has two NUL characters.
        /// </remarks>
        public static unsafe List<string> GetMultiSz(char* blob, int blobLen)
        {
            List<string> strings = new List<string>();
            int cur = 0;

            while (cur < blobLen) {
                int nextNull = cur;
                while (nextNull < blobLen && blob[nextNull] != (char)0) {
                    nextNull++;
                }

                if (nextNull < blobLen) {
                    if (nextNull - cur > 0) {
                        strings.Add(new string(blob, cur, nextNull - cur));
                    } else {
                        // we found an empty string.  But if we're at the end of the data,
                        // it's just the extra null terminator.
                        if (nextNull != blobLen - 1)
                            strings.Add(string.Empty);
                    }
                } else {
                    strings.Add(new string(blob, cur, blobLen - cur));
                }
                cur = nextNull + 1;
            }
            return strings;
        }
    }
}
