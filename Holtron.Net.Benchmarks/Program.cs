using BenchmarkDotNet.Running;
using Holtron.Net.Benchmarks.Components;
using Holtron.Net.Benchmarks.Encryption;

var encryptionSummary = BenchmarkRunner.Run<EncryptionBenchmarks>();
var bigIntSummary = BenchmarkRunner.Run<BigIntBenchmarks>();
