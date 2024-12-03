// Copyright © Jason Curl 2012-2023
// Sources at https://github.com/jcurl/SerialPortStream
// Licensed under the Microsoft Public License (Ms-PL)

namespace RJCP.IO.Ports.Trace
{
    using System;
    using System.Diagnostics;

#if NETSTANDARD1_5
    using Microsoft.Extensions.Logging;
#endif

    /// <summary>
    /// Provide an abstraction over a <see cref="TraceSource"/> object.
    /// </summary>
    /// <remarks>
    /// On .NET Framework, it wraps around a <see cref="TraceSource"/> and provides that to the user. THe method
    /// <see cref="ShouldTrace"/> converts enums to integers first so that no boxing is needed.
    /// <para>On .NET Core, the <see cref="TraceSource"/> but doesn't initialize based on a configuration file. This
    /// means tracing doesn't work. The user can set the <see cref="T:LogSourceFactory.LoggerFactory"/> for a singleton
    /// object to generate an <see cref="T:Microsoft.Extensions.Logging.ILogger"/> object. The <see cref="LogSource"/>
    /// class then intantiates from this method, and wraps it inside a custom <see cref="TraceListener"/> used by
    /// <see cref="SerialPortStream"/>.</para>
    /// </remarks>
    internal sealed class LogSource : IDisposable
    {
        private readonly string m_Name;
        private TraceSource m_TraceSource;
#if NETSTANDARD1_5
        private long m_TraceLevels;
#else
        private readonly long m_TraceLevels;
#endif

        public LogSource()
        {
            m_TraceSource = new TraceSource("null", SourceLevels.Off);
            m_TraceSource.Listeners.Clear();
        }

        /// <summary>
        /// Initialize the internal <see cref="TraceSource"/> object.
        /// </summary>
        /// <remarks>
        /// On .NET Framework, initializes the <see cref="TraceSource"/> object, which loads from the <c>app.config</c>
        /// file. On .NET Core, looks to <see cref="T:LogSourceFactory.LoggerFactory"/> to instantiate a
        /// <see cref="T:Microsoft.Extensions.Logging.ILogger"/>, which is then wrapped around an internal
        /// <see cref="TraceListener"/> for compatibility to .NET Framework. This means on .NET Core, your application
        /// must first set <see cref="T:LogSourceFactory.LoggerFactory"/>, which will be queried with the name
        /// <paramref name="name"/>.
        /// </remarks>
        public LogSource(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name may not be empty", nameof(name));
            m_Name = name;

//#if NETFRAMEWORK
            m_TraceSource = new TraceSource(m_Name);
            m_TraceLevels = GetTraceLevels(m_TraceSource);
//#else
//            SetLoggerTraceListener(name, LogSourceFactory.CreateLogger(name));
//#endif
        }

#if NETSTANDARD1_5
        /// <summary>
        /// Initialize the internal <see cref="TraceSource"/> object.
        /// </summary>
        /// <remarks>
        /// Instantiates a <see cref="TraceSource"/> object with the name given. All listeners are removed from the
        /// collection, and a new listener specific for this <paramref name="logger"/> is added.
        /// </remarks>
        public LogSource(string name, ILogger logger)
        {
            SetLoggerTraceListener(name, logger);
        }

        private void SetLoggerTraceListener(string name, ILogger logger)
        {
            if (logger == null) {
                m_TraceSource = new TraceSource(name, SourceLevels.Off);
                m_TraceSource.Listeners.Clear();
                return;
            }

            m_TraceSource = new TraceSource(name, GetSourceLevels(logger));
            m_TraceSource.Listeners.Clear();

            TraceListener listener = new LoggerTraceListener(name, logger);
            m_TraceSource.Listeners.Add(listener);
            m_TraceLevels = GetTraceLevels(m_TraceSource);
        }

        private SourceLevels GetSourceLevels(ILogger logger)
        {
            if (logger == null) return SourceLevels.Off;

            if (logger.IsEnabled(LogLevel.Trace)) return SourceLevels.All;
            if (logger.IsEnabled(LogLevel.Debug)) return SourceLevels.Verbose;
            if (logger.IsEnabled(LogLevel.Information)) return SourceLevels.Information;
            if (logger.IsEnabled(LogLevel.Warning)) return SourceLevels.Warning;
            if (logger.IsEnabled(LogLevel.Error)) return SourceLevels.Error;
            if (logger.IsEnabled(LogLevel.Critical)) return SourceLevels.Critical;
            return SourceLevels.Off;
        }
#endif

        public TraceSource TraceSource
        {
            get
            {
                if (m_TraceSource != null) return m_TraceSource;
                throw new ObjectDisposedException(m_Name);
            }
        }

        private long GetTraceLevels(TraceSource source)
        {
            long levels = 0;
            if (source.Switch.ShouldTrace(TraceEventType.Critical)) levels |= (long)TraceEventType.Critical;
            if (source.Switch.ShouldTrace(TraceEventType.Error)) levels |= (long)TraceEventType.Error;
            if (source.Switch.ShouldTrace(TraceEventType.Warning)) levels |= (long)TraceEventType.Warning;
            if (source.Switch.ShouldTrace(TraceEventType.Information)) levels |= (long)TraceEventType.Information;
            if (source.Switch.ShouldTrace(TraceEventType.Verbose)) levels |= (long)TraceEventType.Verbose;
#if NETFRAMEWORK
            if (source.Switch.ShouldTrace(TraceEventType.Start)) levels |= (long)TraceEventType.Start;
            if (source.Switch.ShouldTrace(TraceEventType.Stop)) levels |= (long)TraceEventType.Stop;
            if (source.Switch.ShouldTrace(TraceEventType.Suspend)) levels |= (long)TraceEventType.Suspend;
            if (source.Switch.ShouldTrace(TraceEventType.Resume)) levels |= (long)TraceEventType.Resume;
            if (source.Switch.ShouldTrace(TraceEventType.Transfer)) levels |= (long)TraceEventType.Transfer;
#endif

            return levels;
        }

        public bool ShouldTrace(TraceEventType eventType)
        {
            return (m_TraceLevels & (long)eventType) != 0;
        }

        /// <summary>
        /// Writes a trace event message to the trace listeners in the <see cref="TraceSource.Listeners"/> collection.
        /// </summary>
        /// <param name="eventType">
        /// One of the enumeration values that specifies the event type of the trace data.
        /// </param>
        /// <param name="message">The trace message to write.</param>
        public void TraceEvent(TraceEventType eventType, string message)
        {
            TraceSource.TraceEvent(eventType, 0, message);
        }

        /// <summary>
        /// Writes a trace event message to the trace listeners in the <see cref="TraceSource.Listeners"/> collection.
        /// </summary>
        /// <param name="eventType">Type of the event.</param>
        /// <param name="format">
        /// A composite format string that contains text intermixed with zero or more format items, which correspond to
        /// objects in the <paramref name="args"/> array.
        /// </param>
        /// <param name="args">An object array containing zero or more objects to format.</param>
        public void TraceEvent(TraceEventType eventType, string format, params object[] args)
        {
            TraceSource.TraceEvent(eventType, 0, format, args);
        }

        private bool m_IsDisposed;

        public void Dispose()
        {
            if (m_IsDisposed) return;

            m_TraceSource.Flush();
            m_TraceSource.Close();
            m_TraceSource = null;
            m_IsDisposed = true;
        }
    }
}
