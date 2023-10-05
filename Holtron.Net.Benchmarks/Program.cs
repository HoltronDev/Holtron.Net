using BenchmarkDotNet.Running;
using Holtron.Net.Benchmarks.Encryption;

var encryptionSummary = BenchmarkRunner.Run<EncryptionBenchmarks>();
