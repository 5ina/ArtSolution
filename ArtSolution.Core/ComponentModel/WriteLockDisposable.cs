using System;
using System.Threading;

namespace ArtSolution.ComponentModel
{
    /// <summary>
    /// 提供了一种方便的方法来实现锁定访问资源
    /// </summary>
    /// <remarks>
    /// 作为一个基础设施类
    /// </remarks>
    public class WriteLockDisposable : IDisposable
    {
        private readonly ReaderWriterLockSlim _rwLock;

        /// <summary>
        /// 初始化新的实例 <see cref="WriteLockDisposable"/> class.
        /// </summary>
        /// <param name="rwLock">The rw lock.</param>
        public WriteLockDisposable(ReaderWriterLockSlim rwLock)
        {
            _rwLock = rwLock;
            _rwLock.EnterWriteLock();
        }

        void IDisposable.Dispose()
        {
            _rwLock.ExitWriteLock();
        }
    }
}
