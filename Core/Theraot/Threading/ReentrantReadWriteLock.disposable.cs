#if FAT

using System;
using System.Threading;

namespace Theraot.Threading
{
    internal sealed partial class ReentrantReadWriteLock : IExtendedDisposable
    {
        private int _disposeStatus;

        [System.Diagnostics.DebuggerNonUserCode]
        ~ReentrantReadWriteLock()
        {
            try
            {
                // Empty
            }
            finally
            {
                Dispose(false);
            }
        }

        public bool IsDisposed
        {
            get { return _disposeStatus == -1; }
        }

        [System.Diagnostics.DebuggerNonUserCode]
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

        [System.Diagnostics.DebuggerNonUserCode]
        public void DisposedConditional(Action whenDisposed, Action whenNotDisposed)
        {
            if (_disposeStatus == -1)
            {
                if (whenDisposed != null)
                {
                    whenDisposed.Invoke();
                }
            }
            else
            {
                if (whenNotDisposed != null)
                {
                    if (ThreadingHelper.SpinWaitRelativeSet(ref _disposeStatus, 1, -1))
                    {
                        try
                        {
                            whenNotDisposed.Invoke();
                        }
                        finally
                        {
                            Interlocked.Decrement(ref _disposeStatus);
                        }
                    }
                    else
                    {
                        if (whenDisposed != null)
                        {
                            whenDisposed.Invoke();
                        }
                    }
                }
            }
        }

        [System.Diagnostics.DebuggerNonUserCode]
        public TReturn DisposedConditional<TReturn>(Func<TReturn> whenDisposed, Func<TReturn> whenNotDisposed)
        {
            if (_disposeStatus == -1)
            {
                if (whenDisposed == null)
                {
                    return default(TReturn);
                }
                return whenDisposed.Invoke();
            }
            if (whenNotDisposed == null)
            {
                return default(TReturn);
            }
            if (ThreadingHelper.SpinWaitRelativeSet(ref _disposeStatus, 1, -1))
            {
                try
                {
                    return whenNotDisposed.Invoke();
                }
                finally
                {
                    Interlocked.Decrement(ref _disposeStatus);
                }
            }
            if (whenDisposed == null)
            {
                return default(TReturn);
            }
            return whenDisposed.Invoke();
        }

        [System.Diagnostics.DebuggerNonUserCode]
        private void Dispose(bool disposeManagedResources)
        {
            if (TakeDisposalExecution())
            {
                try
                {
                    if (disposeManagedResources)
                    {
                        _freeToRead.Dispose();
                        _freeToWrite.Dispose();
                        _currentReadingCount.Dispose();
                    }
                }
                finally
                {
                    _freeToRead = null;
                    _freeToWrite = null;
                    _currentReadingCount = null;
                }
            }
        }

        private bool TakeDisposalExecution()
        {
            if (_disposeStatus == -1)
            {
                return false;
            }
            return ThreadingHelper.SpinWaitSetUnless(ref _disposeStatus, -1, 0, -1);
        }
    }
}

#endif