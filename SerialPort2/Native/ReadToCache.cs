// Copyright © Jason Curl 2012-2018
// Sources at https://github.com/jcurl/SerialPortStream
// Licensed under the Microsoft Public License (Ms-PL)

namespace RJCP.IO.Ports.Native
{
    using System;
    using System.Text;
    using Datastructures;
    using Trace;

    internal class ReadToCache
    {
        // Buffers for reading characters. We reserve one "char" at the end for 2-byte UTF16 sequences, to guarantee
        // guarantee we can always read into the cache. So you'll also see free space checks that we always need at
        // least two chars before conversion.
        private const int c_MaxLine = 1024;
        private readonly CircularBuffer<char> m_ReadCache = new CircularBuffer<char>(c_MaxLine + 1);
        private readonly CircularBuffer<int> m_ReadOffsets = new CircularBuffer<int>(c_MaxLine + 1);

        private int m_ReadOffset;    // Offset into byte buffer for next character
        private int m_LastChar;      // Position of m_ReadOffset on last successful character read

        // Overflow of the first character is used to know the precise state of the decoder in case we
        // need to discard the cache. We can reset the decoder, and know what the first byte was that we
        // read to ensure consistent behaviour, because after we read the first byte, we know the
        // decoder doesn't have any internal data cached any more.
        private int m_ReadOverflow = -1;         // Number of bytes to discard due to overflow
        private readonly char[] m_ReadOverflowChar = new char[2];   // First character that was lost
        private bool m_ReadOverflowUtf32;        // Indicates if two UTF16 overflowed or not.

        private Encoding m_Encoding = Encoding.GetEncoding("UTF-8");
        private Decoder m_Decoder;

        /// <summary>
        /// Gets or sets the byte encoding for pre- and post-transmission conversion of text.
        /// </summary>
        /// <remarks>
        /// The encoding is used for encoding string information to byte format when sending
        /// over the serial port, or receiving data via the serial port. It is only used
        /// with the read/write functions that accept strings (and not used for byte based
        /// reading and writing).
        /// </remarks>
        public Encoding Encoding
        {
            get { return m_Encoding; }
            set
            {
                if (value != null) {
                    m_Encoding = value;
                    m_Decoder = null;
                } else {
                    throw new ArgumentNullException("value", "Encoding may not be null");
                }
            }
        }

        private Decoder Decoder
        {
            get
            {
                if (m_Encoding != null) {
                    if (m_Decoder == null) m_Decoder = m_Encoding.GetDecoder();
                    return m_Decoder;
                } else {
                    return null;
                }
            }
        }

        /// <summary>
        /// Reads a single character from the byte buffer provided, putting it into the internal cache
        /// </summary>
        /// <param name="sbuffer">The buffer to read.</param>
        /// <returns><c>true</c> if a character was found; <c>false</c> otherwise.</returns>
        /// <exception cref="System.ArgumentNullException">buffer may not be null.</exception>
        /// <remarks>
        /// It is expected that this method be used iteratively to find a single character. Data is not consumed
        /// from the <paramref name="sbuffer"/>, but state is updated in this class to advance the byte offset
        /// into the buffer to the next character. Should data be consumed from the internal buffer, you will
        /// need to reset the state of this object.
        /// </remarks>
        private bool PeekChar(SerialBuffer sbuffer)
        {
            // Once the bug from Mono is fixed, we just drop the code above.
            if (sbuffer == null) throw new ArgumentNullException("sbuffer");
            int readLen = sbuffer.Serial.ReadBuffer.Length;
            if (Log.ReadToTrace(System.Diagnostics.TraceEventType.Verbose))
                Log.ReadTo.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0,
                    "PeekChar: readlen={0}; m_ReadOffset={1}; m_ReadCache.Free={2}", readLen, m_ReadOffset, m_ReadCache.Free);
            if (m_ReadOffset >= readLen) return false;

            if (m_ReadCache.Free <= 1) Overflow();

            char[] oneChar = new char[2];
            int cu = 0;
            while (cu == 0 && m_ReadOffset < readLen) {
                int bu;
                try {
                    // Some UTF8 sequences may result in two UTF16 characters being generated.
                    Decoder.Convert(sbuffer.Serial.ReadBuffer.Array,
                        sbuffer.Serial.ReadBuffer.ToArrayIndex(m_ReadOffset),
                        1, oneChar, 0, 1, false, out bu, out cu, out _);
                } catch (ArgumentException ex) {
                    if (!ex.ParamName.Equals("chars")) throw;
                    Decoder.Convert(sbuffer.Serial.ReadBuffer.Array,
                        sbuffer.Serial.ReadBuffer.ToArrayIndex(m_ReadOffset),
                        1, oneChar, 0, 2, false, out bu, out cu, out _);
                }
                m_ReadOffset += bu;
            }
            if (cu == 0) return false;

            m_ReadCache.Append(oneChar, 0, cu);
            m_ReadOffsets.Append(m_ReadOffset - m_LastChar);
            if (cu > 1) m_ReadOffsets.Append(0);
            m_LastChar = m_ReadOffset;
            return true;
        }

        private void Overflow()
        {
            // If we haven't overflowed our m_ReadCache yet, then we record the first character and its
            // offset. So if we have to reset the cache, we can reset the decoder and put that first
            // character into the read buffer.
            //
            // If we have already overflowed, then we just continue doing so. m_ReadOffset is the number
            // of bytes we've consumed independent of if we've overflowed or not.

            int consume = 1;
            if (m_ReadOverflow == -1) {
                m_ReadOverflowChar[0] = m_ReadCache[0];
                m_ReadOverflow = m_ReadOffsets[0];
                if (m_ReadOffsets[1] == 0) {
                    m_ReadOverflowUtf32 = true;
                    m_ReadOverflowChar[1] = m_ReadCache[1];
                    consume = 2;
                }
                if (Log.ReadToTrace(System.Diagnostics.TraceEventType.Verbose))
                    Log.ReadTo.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0,
                        "Overflow: Capture UTF32={0}; Consumed{1}", m_ReadOverflowUtf32, consume);
            }
            m_ReadCache.Consume(consume);
            m_ReadOffsets.Consume(consume);
        }

        /// <summary>
        /// Reads characters from the byte buffer into the character buffer, consuming data from the byte buffer.
        /// </summary>
        /// <param name="sbuffer">The byte buffer to read from.</param>
        /// <param name="cbuffer">The character buffer to write to.</param>
        /// <param name="offset">The offset to write to in <paramref name="cbuffer"/>.</param>
        /// <param name="count">The number of characters to read into <paramref name="cbuffer"/>.</param>
        /// <returns>The number of bytes read into <paramref name="cbuffer"/>.</returns>
        /// <remarks>
        /// Data is read from the read byte buffer kept in <paramref name="sbuffer"/>, converted using the decoder into
        /// <paramref name="cbuffer"/>. As data is read from sbuffer, it is consumed.
        /// </remarks>
        public int Read(SerialBuffer sbuffer, char[] cbuffer, int offset, int count)
        {
            int chars = 0;
            if (IsOverflowed) Reset(true);
            if (IsCached) {
                chars = m_ReadCache.CopyTo(cbuffer, offset, count);
                if (Log.ReadToTrace(System.Diagnostics.TraceEventType.Verbose))
                    Log.ReadTo.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0,
                        "Read: Got {0} chars, need {1} count - From Cache", chars, count);
                ReadToConsume(sbuffer, chars);
                if (chars == count) return chars;
            }

            chars += sbuffer.Stream.Read(cbuffer, offset + chars, count - chars, Decoder);
            if (Log.ReadToTrace(System.Diagnostics.TraceEventType.Verbose))
                Log.ReadTo.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0,
                    "Read: Got {0} chars, need {1} count", chars, count);
            return chars;
        }

        /// <summary>
        /// Synchronously reads one character from the SerialPortStream input buffer.
        /// </summary>
        /// <param name="sbuffer">The byte buffer to read from.</param>
        /// <returns>The character that was read. -1 indicates no data was available
        /// within the time out.</returns>
        public int ReadChar(SerialBuffer sbuffer)
        {
            if (sbuffer == null) throw new ArgumentNullException("sbuffer", "NULL buffer provided");
            char[] schar = new char[1];
            if (IsOverflowed) Reset(true);

            if (Log.ReadToTrace(System.Diagnostics.TraceEventType.Verbose))
                Log.ReadTo.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0,
                    "ReadChar: IsCached {0}", IsCached);
            bool dataAvailable = IsCached;
            if (!IsCached) {
                lock (sbuffer.ReadLock) {
                    dataAvailable = PeekChar(sbuffer);
                }
            }

            // Get the next byte from the cache, or put the next byte in the cache
            if (dataAvailable) {
                m_ReadCache.CopyTo(schar, 0, 1);
                ReadToConsume(sbuffer, 1);
                return schar[0];
            }
            return -1;
        }

        private string m_ReadToString;

        /// <summary>
        /// Reads from the cached and byte stream looking for the text specified.
        /// </summary>
        /// <param name="sbuffer">The byte buffer to read from.</param>
        /// <param name="text">The text to indicate where the read operation stops.</param>
        /// <param name="line">On success, contains the line up to the text string requested.</param>
        /// <returns><c>true</c> if a line was found; <c>false</c> otherwise.</returns>
        public bool ReadTo(SerialBuffer sbuffer, string text, out string line)
        {
            bool changedText = !text.Equals(m_ReadToString);
            if (changedText) {
                if (Log.ReadToTrace(System.Diagnostics.TraceEventType.Verbose))
                    Log.ReadTo.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0,
                        "ReadTo: Text changed!");
                m_ReadToString = text;
                if (IsOverflowed) Reset(true);

                int readLen = m_ReadCache.Length;
                if (readLen >= text.Length) {
                    // Check if the text already exists
                    string lbuffer = m_ReadCache.GetString();
                    int p = lbuffer.IndexOf(text, StringComparison.Ordinal);
                    if (p != -1) {
                        if (Log.ReadToTrace(System.Diagnostics.TraceEventType.Verbose))
                            Log.ReadTo.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0,
                                "ReadTo: Text changed! And Found!");
                        // It does exist, so consume up to the buffered portion
                        line = lbuffer.Substring(0, p);
                        int l = p + text.Length;
                        ReadToConsume(sbuffer, l);
                        return true;
                    }
                }
            } else {
                if (Log.ReadToTrace(System.Diagnostics.TraceEventType.Verbose))
                    Log.ReadTo.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0,
                        "ReadTo: No reset, text the same.");
            }

            lock (sbuffer.ReadLock) {
                while (!ReadToMatch(text)) {

                    // Decoders in .NET are designed for streams and not really for reading
                    // a little bit of data. By design, they are "greedy", they consume as much
                    // byte data as possible. The data that they consume is cached internally.
                    // Because it's not possible to ask the decoder only to decode if there
                    // is sufficient bytes, we have to keep account of how many bytes are
                    // consumed for each character.

                    bool newChar = PeekChar(sbuffer);
                    if (!newChar) {
                        // Didn't find the string and there's no new data.
                        line = null;
                        return false;
                    }
                }

                // Found the string
                if (Log.ReadToTrace(System.Diagnostics.TraceEventType.Verbose))
                    Log.ReadTo.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0,
                        "ReadTo: Found! Discarding {0} bytes", m_ReadOffset);
                sbuffer.Serial.ReadBuffer.Consume(m_ReadOffset);
            }
            line = m_ReadCache.GetString(m_ReadCache.Length - text.Length);
            Reset(false);
            return true;
        }

        /// <summary>
        /// Waits for new data to arrive that can be used to recheck for new data.
        /// </summary>
        /// <param name="sbuffer">The byte buffer to read from.</param>
        /// <param name="timeout">The time out in milliseconds.</param>
        /// <returns><c>true</c> if one more byte is available since the last <see cref="ReadTo"/>
        /// call; <c>false</c> otherwise</returns>
        public bool ReadToWaitForNewData(SerialBuffer sbuffer, int timeout)
        {
            return sbuffer.Stream.WaitForRead(m_ReadOffset + 1, timeout);
        }

        /// <summary>
        /// Reads all immediately available bytes.
        /// </summary>
        /// <param name="sbuffer">The byte buffer to read from.</param>
        /// <returns>The contents of the stream and the input buffer of the SerialPortStream.</returns>
        public string ReadExisting(SerialBuffer sbuffer)
        {
            StringBuilder sb = new StringBuilder();
            if (IsOverflowed) Reset(true);
            sb.Append(m_ReadCache.GetString());
            Reset(false);

            lock (sbuffer.ReadLock) {
                do {
                    char[] c = new char[2048];
                    Decoder.Convert(sbuffer.Serial.ReadBuffer, c, 0, c.Length, false, out int bu, out int cu, out bool complete);
                    sb.Append(c, 0, cu);
                    if (Log.ReadToTrace(System.Diagnostics.TraceEventType.Verbose))
                        Log.ReadTo.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0,
                            "ReadExisting: Decoder.Convert(out bu={0}, out cu={1}); Remaining: {2}", bu, cu, sbuffer.Serial.ReadBuffer.Length);
                } while (sbuffer.Serial.ReadBuffer.Length > 0);
            }
            return sb.ToString();
        }

        private void ReadToConsume(SerialBuffer sbuffer, int chars)
        {
            int bytesRead = 0;
            for (int i = 0; i < chars; i++) {
                bytesRead += m_ReadOffsets[i];
            }
            m_LastChar -= bytesRead;
            m_ReadOffset -= bytesRead;
            m_ReadCache.Consume(chars);
            m_ReadOffsets.Consume(chars);
            sbuffer.Stream.ReadConsume(bytesRead);
            if (Log.ReadToTrace(System.Diagnostics.TraceEventType.Verbose)) {
                Log.ReadTo.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0, "ReadToConsume(chars={0}) = {1} bytes", chars, bytesRead);
                Log.ReadTo.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0, "ReadToConsume: m_ReadOffset={0}; m_ReadCache.Free={1}", m_ReadOffset, m_ReadCache.Free);
            }
        }

        /// <summary>
        /// Resets the cache, taking optionally into account a previous overflow.
        /// </summary>
        /// <param name="withOverflow">if set to <c>true</c> a previous overflow is taken into account and reinserted into
        /// the buffer, as if the first character had already been read.</param>
        public void Reset(bool withOverflow)
        {
            if (Log.ReadToTrace(System.Diagnostics.TraceEventType.Verbose))
                Log.ReadTo.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0, "Reset({0})", withOverflow);
            m_ReadCache.Reset();
            m_ReadOffsets.Reset();
            if (m_Decoder != null) m_Decoder.Reset();
            if (withOverflow) {
                m_ReadCache.Append(m_ReadOverflowChar, 0, m_ReadOverflowUtf32 ? 2 : 1);
                m_ReadOffsets.Append(m_ReadOverflow);
                if (m_ReadOverflowUtf32) m_ReadOffsets.Append(0);
                m_ReadOffset = m_ReadOverflow;
            } else {
                m_ReadOffset = 0;
            }
            m_LastChar = m_ReadOffset;
            m_ReadOverflow = -1;
            m_ReadOverflowUtf32 = false;
        }

        private bool ReadToMatch(string text)
        {
            int bl = m_ReadCache.Length;
            int offset = bl - text.Length;
            if (offset < 0) return false;

            for (int i = 0; i < text.Length; i++) {
                if (m_ReadCache[i + offset] != text[i]) return false;
            }
            if (Log.ReadToTrace(System.Diagnostics.TraceEventType.Verbose))
                Log.ReadTo.TraceEvent(System.Diagnostics.TraceEventType.Verbose, 0, "ReadToMatch: Found!");
            return true;
        }

        /// <summary>
        /// Indicates if the ReadTo buffer has cached data.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has cached data; otherwise, <c>false</c>.
        /// </value>
        private bool IsCached
        {
            get { return m_ReadCache.Length != 0; }
        }

        /// <summary>
        /// Indicates if the amount of data read exceeds the line length.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is overflowed; otherwise, <c>false</c>.
        /// </value>
        private bool IsOverflowed
        {
            get { return m_ReadOverflow != -1; }
        }
    }
}
