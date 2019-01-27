﻿using System;
using System.Diagnostics;
using System.Threading;

namespace Theraot.Threading
{
    [DebuggerNonUserCode]
    public sealed class DisposableAkin :
#if TARGETS_NET || GREATERTHAN_NETCOREAPP11
        System.Runtime.ConstrainedExecution.CriticalFinalizerObject,
#endif
        IDisposable
    {
        private Action _release;
        private Thread _thread;

        private DisposableAkin(Action release, Thread thread)
        {
            _release = release ?? throw new ArgumentNullException(nameof(release));
            _thread = thread ?? throw new ArgumentNullException(nameof(thread));
        }

        ~DisposableAkin()
        {
            try
            {
                // Empty
            }
            finally
            {
                try
                {
                    Dispose(false);
                }
                catch (Exception exception)
                {
                    // Catch them all - fields may be partially collected.
                    No.Op(exception);
                }
            }
        }

        public bool IsDisposed => _thread == null;

        public static DisposableAkin Create(Action release)
        {
            return new DisposableAkin(release, Thread.CurrentThread);
        }

        public bool Dispose(Func<bool> condition)
        {
            if (condition == null)
            {
                throw new ArgumentNullException(nameof(condition));
            }

            if (Interlocked.CompareExchange(ref _thread, null, Thread.CurrentThread) != Thread.CurrentThread)
            {
                return false;
            }

            if (!condition.Invoke())
            {
                return false;
            }

            try
            {
                _release.Invoke();
                return true;
            }
            finally
            {
                _release = null;
            }
        }

        [DebuggerNonUserCode]
        public void Dispose()
        {
            try
            {
                Dispose(true);
            }
            finally
            {
                GC.SuppressFinalize(this);
            }
        }

        private void Dispose(bool disposeManagedResources)
        {
            if (disposeManagedResources && Interlocked.CompareExchange(ref _thread, null, Thread.CurrentThread) != Thread.CurrentThread)
            {
                return;
            }

            try
            {
                _release.Invoke();
            }
            finally
            {
                _release = null;
            }
        }
    }
}