// Copyright © Jason Curl 2012-2018
// Sources at https://github.com/jcurl/SerialPortStream
// Licensed under the Microsoft Public License (Ms-PL)

namespace RJCP.IO.Ports.Trace
{
    using System;
    using System.Diagnostics;

    internal sealed class LogSource : IDisposable
    {
        private string m_Name;
        private TraceSource m_TraceSource;
        private long m_TraceLevels;

        public LogSource(string name)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Name may not be empty", "name");
            m_Name = name;

            m_TraceSource = new TraceSource(m_Name);
            m_TraceLevels = GetTraceLevels(m_TraceSource);
        }

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
#if !NETSTANDARD15
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
            return ((m_TraceLevels & (long)eventType) != 0);
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
