using BenchmarkDotNet.Attributes;
using Holtron.Net.Network;
using Holtron.Net.Network.Encryption;

namespace Holtron.Net.Benchmarks.Encryption
{
    public class EncryptionBenchmarks
    {
        private const string testKey = "testKey";
        private const string testString = "This is a test string for benchmarking encryption on messages.";
        private const int testInteger = int.MaxValue;
        private readonly byte[] testBytes = new byte[50];
        private readonly NetPeer peer;
        private readonly NetEncryptionAESGCM aesGcmEncryption;

        public EncryptionBenchmarks()
        {
            var config = new NetPeerConfiguration("benchmarkTesting");
            peer = new NetPeer(config);
            peer.Start();
            new Random(50).NextBytes(testBytes);
            aesGcmEncryption = new NetEncryptionAESGCM(testKey);
        }

        [Benchmark(Baseline = true)]
        public void NoEncryption()
        {
            var _ = CreateNetOutgoingMessage();
        }

        [Benchmark]
        public void AesGcmEncryption()
        {
            var msg = CreateNetOutgoingMessage();
            aesGcmEncryption.Encrypt(msg);
        }

        private NetOutgoingMessage CreateNetOutgoingMessage()
        {
            var msg = peer.CreateMessage();
            msg.Write(testString);
            msg.Write(testInteger);
            msg.Write(testBytes.Length);
            msg.Write(testBytes);
            return msg;
        }
    }
}
