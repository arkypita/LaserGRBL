//// Copyright © Jason Curl 2021
//// Sources at https://github.com/jcurl/SerialPortStream
//// Licensed under the Microsoft Public License (Ms-PL)

//namespace RJCP.IO.Ports.Trace
//{
//    using System.Diagnostics;
//    using Microsoft.Extensions.Logging;

//    /// <summary>
//    /// A <see cref="TraceListener"/> wrapper for a .NET Core <see cref="ILogger"/>.
//    /// </summary>
//    /// <remarks>
//    /// This class is not intended to be instantiated within a <c>app.config</c> file. It is provided so that a user of
//    /// .NET Core can provide an <see cref="ILogger"/> object, and the code in this library can continue to use the
//    /// <see cref="TraceSource"/> classes for logging.
//    /// </remarks>
//    internal class LoggerTraceListener : TraceListener
//    {
//        private readonly ILogger m_Logger;
//        private readonly LineSplitter m_Lines = new LineSplitter();

//        public LoggerTraceListener(string name, ILogger logger) : base(name)
//        {
//            m_Logger = logger;
//        }

//        public override void Fail(string message, string detailMessage)
//        {
//            if (m_Lines.IsCached) m_Lines.NewLine();

//            string logMessage;
//            if (detailMessage == null) {
//                logMessage = message;
//            } else {
//                logMessage = string.Format("{0}: {1}", message, detailMessage);
//            }
//            m_Lines.AppendLine(logMessage);

//            foreach (string line in m_Lines) {
//                m_Logger.LogInformation(line);
//            }
//        }

//        public override void Write(string message)
//        {
//            m_Lines.Append(message);
//            foreach (string line in m_Lines) {
//                m_Logger.LogInformation(line);
//            }
//        }

//        public override void WriteLine(string message)
//        {
//            m_Lines.AppendLine(message);
//            foreach (string line in m_Lines) {
//                m_Logger.LogInformation(line);
//            }
//        }

//        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
//        {
//            TraceEvent(eventCache, source, eventType, id, string.Empty);
//        }

//        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
//        {
//            string message = string.Format(format, args);
//            TraceEvent(eventCache, source, eventType, id, message);
//        }

//        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
//        {
//            if (m_Lines.IsCached) m_Lines.NewLine();
//            m_Lines.AppendLine(message);

//            switch (eventType) {
//            case TraceEventType.Critical:
//                foreach (string line in m_Lines) {
//                    string fLine = string.Format("{0}: {1}", source, line);
//                    m_Logger.LogCritical(id, fLine);
//                }
//                break;
//            case TraceEventType.Error:
//                foreach (string line in m_Lines) {
//                    string fLine = string.Format("{0}: {1}", source, line);
//                    m_Logger.LogError(id, fLine);
//                }
//                break;
//            case TraceEventType.Warning:
//                foreach (string line in m_Lines) {
//                    string fLine = string.Format("{0}: {1}", source, line);
//                    m_Logger.LogWarning(id, fLine);
//                }
//                break;
//            case TraceEventType.Information:
//                foreach (string line in m_Lines) {
//                    string fLine = string.Format("{0}: {1}", source, line);
//                    m_Logger.LogInformation(id, fLine);
//                }
//                break;
//            case TraceEventType.Verbose:
//                foreach (string line in m_Lines) {
//                    string fLine = string.Format("{0}: {1}", source, line);
//                    m_Logger.LogDebug(id, fLine);
//                }
//                break;
//            default:
//                foreach (string line in m_Lines) {
//                    string fLine = string.Format("{0}: {1}-{2}", source, eventType.ToString(), line);
//                    m_Logger.LogTrace(id, fLine);
//                }
//                break;
//            }
//        }

//        public override void Flush()
//        {
//            if (m_Lines.IsCached) m_Lines.NewLine();
//            foreach (string line in m_Lines) {
//                m_Logger.LogInformation(line);
//            }
//        }
//    }
//}