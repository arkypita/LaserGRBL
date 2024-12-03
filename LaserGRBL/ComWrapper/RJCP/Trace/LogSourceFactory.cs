//// Copyright © Jason Curl 2021
//// Sources at https://github.com/jcurl/SerialPortStream
//// Licensed under the Microsoft Public License (Ms-PL)

//// This file is only for .NET Standard 1.5

//namespace RJCP.IO.Ports.Trace
//{
//    using System;
//    using Microsoft.Extensions.Logging;

//    /// <summary>
//    /// A class that the <see cref="SerialPortStream"/> library can refer to for obtaining a logger, instead of through
//    /// dependency injection.
//    /// </summary>
//    /// <remarks>
//    /// .NET Framework uses <see cref="System.Diagnostics.TraceSource"/> which reads the application configuration file
//    /// and instantiates the logging infrastructure, which is a static instance. Since .NET Core, this doesn't work and
//    /// the alternative method is to instantiate the objects through dependency injection. People upgrading or
//    /// supporting multiple frameworks (.NET Framework and .NET Core) have to provide a lot of effort in separating the
//    /// two. This library makes it easier by optionally providing the logger factory as a static instance to this class.
//    /// <para>Note, reading configuration files, is still the responsibility of the client application. Refer to the
//    /// package <c>Microsoft.Extensions.Configuration</c>.</para>
//    /// </remarks>
//    [CLSCompliant(false)]
//    public static class LogSourceFactory
//    {
//        private static readonly object s_Lock = new object();
//        private static ILoggerFactory m_LoggerFactory;

//        /// <summary>
//        /// The <see cref="ILoggerFactory"/> object that will be queried to obtain a logger from
//        /// <see cref="SerialPortStream"/>.
//        /// </summary>
//        /// <remarks>
//        /// Your application code can set this property globally so that when the Serial Port Stream requests a logger,
//        /// it can get one globally. The alternative is to provide the logger directly to the constructor of
//        /// <see cref="SerialPortStream(ILogger)"/>.
//        /// </remarks>
//        public static ILoggerFactory LoggerFactory
//        {
//            get { return m_LoggerFactory; }
//            set
//            {
//                lock (s_Lock) {
//                    m_LoggerFactory = value;
//                }
//            }
//        }

//        internal static ILogger CreateLogger(string categoryName)
//        {
//            lock (s_Lock) {
//                if (m_LoggerFactory == null)
//                    return null;

//                return m_LoggerFactory.CreateLogger(categoryName);
//            }
//        }
//    }
//}