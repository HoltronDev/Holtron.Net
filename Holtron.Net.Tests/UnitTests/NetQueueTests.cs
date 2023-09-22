using Holtron.Net.Network;

namespace Holtron.Net.Tests.UnitTests
{
    public class NetQueueTests
    {
        [Fact]
        public void TestQueuing()
        {
            var queue = new NetQueue<int>(4);

            queue.Enqueue(1);
            queue.Enqueue(2);
            queue.Enqueue(3);

            var array = queue.ToArray();

            Assert.Equal(3, array.Length);
            Assert.Equal(1, array[0]); 
            Assert.Equal(2, array[1]); 
            Assert.Equal(3, array[2]);
            Assert.False(queue.Contains(4));
            Assert.True(queue.Contains(2));
            Assert.Equal(3, queue.Count);

            int output;
            var ok = queue.TryDequeue(out output);

            Assert.True(ok);
            Assert.Equal(1, output);
            Assert.Equal(2, queue.Count);

            queue.EnqueueFirst(42);
            ok = queue.TryDequeue(out output);
            Assert.True(ok);
            Assert.Equal(42, output);

            ok = queue.TryDequeue(out output);
            Assert.True(ok);
            Assert.Equal(2, output);

            ok = queue.TryDequeue(out output);
            Assert.True(ok);
            Assert.Equal(3, output);

            ok = queue.TryDequeue(out output);
            Assert.False(ok);

            queue.Enqueue(24601);
            Assert.Equal(1, queue.Count);

            ok = queue.TryDequeue(out output);
            Assert.True(ok);
            Assert.Equal(24601, output);

            queue.Clear();
            Assert.Equal(0, queue.Count);

            var array2 = queue.ToArray();
            Assert.Empty(array2);
        }
    }
}
