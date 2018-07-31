using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CrossSolar.Tests.Providers
{
    internal class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
    {
        private readonly IEnumerator<T> _inner;

        public TestAsyncEnumerator(IEnumerator<T> inner) 
            => _inner = inner;

        public void Dispose() 
            => _inner.Dispose();

        public T Current 
            => _inner.Current;

        public Task<bool> MoveNext(CancellationToken cancellationToken) 
            => Task.FromResult(_inner.MoveNext());
    }
}