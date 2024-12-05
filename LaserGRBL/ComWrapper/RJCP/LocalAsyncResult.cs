// Copyright © Jason Curl 2012-2016
// Sources at https://github.com/jcurl/SerialPortStream
// Licensed under the Microsoft Public License (Ms-PL)

namespace RJCP.IO
{
    using System;
    using System.Threading;

    /// <summary>
    /// Provides a local implementation of an IAsyncResult
    /// </summary>
    internal class LocalAsync : IAsyncResult, IDisposable
    {
        readonly object m_State;
        private ManualResetEvent m_Handle;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalAsync"/> class.
        /// </summary>
        /// <param name="state">The state object given by the user in a BeginWrite/EndWrite method.</param>
        /// <remarks>
        /// When your asynchronous operation is finished, you should set the <see cref="IsCompleted"/>
        /// property, which will automatically trigger the <see cref="AsyncWaitHandle"/> if the user is
        /// waiting on this. When you're finished, be sure to call the <see cref="Dispose()"/> method in
        /// your EndXXX() method.
        /// </remarks>
        public LocalAsync(object state)
        {
            m_State = state;
        }

        /// <summary>
        /// Gets a user-defined object that qualifies or contains information
        /// about an asynchronous operation.
        /// </summary>
        /// <returns>A user-defined object that qualifies or contains
        /// information about an asynchronous operation.</returns>
        public object AsyncState
        {
            get { return m_State; }
        }

        /// <summary>
        /// Gets a <see cref="T:System.Threading.WaitHandle" /> that
        /// is used to wait for an asynchronous operation to complete.
        /// </summary>
        /// <returns>A <see cref="T:System.Threading.WaitHandle" /> that is
        /// used to wait for an asynchronous operation to complete.</returns>
        public WaitHandle AsyncWaitHandle
        {
            get
            {
                if (m_Handle == null) {
                    bool done = IsCompleted;
                    ManualResetEvent mre = new ManualResetEvent(done);
                    if (Interlocked.CompareExchange(ref m_Handle,
                        mre, null) != null) {
                        // Another thread created this object's event; dispose
                        // the event we just created
                        mre.Dispose();
                    } else {
                        if (!done && IsCompleted) {
                            // If the operation wasn't done when we created
                            // the event but now it is done, set the event
                            m_Handle.Set();
                        }
                    }
                }
                return m_Handle;
            }
        }

        /// <summary>
        /// Gets a value that indicates whether the asynchronous
        /// operation completed synchronously.
        /// </summary>
        /// <returns><see langword="true"/> if the asynchronous operation completed synchronously;
        /// otherwise, <see langword="false"/>.</returns>

        public bool CompletedSynchronously { get; internal set; }

        private volatile bool m_IsCompleted;

        /// <summary>
        /// Gets a value that indicates whether the asynchronous operation has completed.
        /// </summary>
        /// <returns><see langword="true"/> if the operation is complete; otherwise, <see langword="false"/>.</returns>
        public bool IsCompleted
        {
            get { return m_IsCompleted; }
            set
            {
                if (value) {
                    m_IsCompleted = true;
                    if (m_Handle != null) m_Handle.Set();
                }
            }
        }

        /// <summary>
        /// Gets or sets the exception if one occurred.
        /// </summary>
        /// <value>The exception.</value>
        public Exception Exception { get; set; }

        /// <summary>
        /// Performs application-defined tasks associated with freeing,
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources;
        /// <see langword="false"/> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;

            if (m_Handle != null) {
                m_Handle.Dispose();
                m_Handle = null;
            }
        }
    }

    internal class LocalAsync<T> : LocalAsync
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalAsync"/> class.
        /// </summary>
        /// <param name="state">The state object given by the user in a BeginWrite/EndWrite method.</param>
        /// <remarks>
        /// When your asynchronous operation is finished, you should set the <see cref="LocalAsync.IsCompleted"/>
        /// property, which will automatically trigger the <see cref="LocalAsync.AsyncWaitHandle"/> if the user is
        /// waiting on this. When you're finished, be sure to call the <see cref="LocalAsync.Dispose()"/> method in
        /// your EndXXX() method.
        /// </remarks>
        public LocalAsync(object state) : base(state) { }

        /// <summary>
        /// Gets or sets the result.
        /// </summary>
        /// <value>
        /// The result.
        /// </value>
        internal T Result { get; set; }
    }
}
