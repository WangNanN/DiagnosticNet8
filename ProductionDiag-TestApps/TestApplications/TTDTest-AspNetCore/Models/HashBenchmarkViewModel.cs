using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace TTDTest_AspNetCore.Models
{
    public class HashBenchmarkViewModel
    {
        public DateTime GeneratedTime { get; }

        public int[] ByteCounts { get; }

        public Dictionary<string, Dictionary<int, HashBenchmarkResult>> Results { get; }

        public HashBenchmarkViewModel(int[] byteCounts)
        {
            GeneratedTime = DateTime.UtcNow;

            Results = new Dictionary<string, Dictionary<int, HashBenchmarkResult>>();

            Results["MD5"] = TestAlgorithm(() => MD5.Create(), byteCounts);
            Results["SHA1"] = TestAlgorithm(() => SHA1.Create(), byteCounts);
            Results["SHA256"] = TestAlgorithm(() => SHA256.Create(), byteCounts);
            Results["SHA384"] = TestAlgorithm(() => SHA384.Create(), byteCounts);
            Results["SHA512"] = TestAlgorithm(() => SHA512.Create(), byteCounts);

            ByteCounts = new int[byteCounts.Length];
            Array.Copy(byteCounts, ByteCounts, byteCounts.Length);
        }

        private Dictionary<int, HashBenchmarkResult> TestAlgorithm(Func<HashAlgorithm> algorithmFactory, int[] byteCounts)
        {
            Dictionary<int, HashBenchmarkResult> result = new Dictionary<int, HashBenchmarkResult>();

            for (int i = 0; i < byteCounts.Length; i++)
            {
                result[byteCounts[i]] = new HashBenchmarkResult(algorithmFactory, byteCounts[i]);
            }

            return result;
        }
    }

    public class HashBenchmarkResult
    {
        public readonly string Name;

        public readonly int ByteCount;

        public readonly long Ticks, Milliseconds;

        public const int WarmUpCount = 5, Iterations = 20;

        public HashBenchmarkResult(Func<HashAlgorithm> algorithmFactory, int byteCount)
        {
            ByteCount = byteCount;
            Name = algorithmFactory().GetType().Name;

            long tickSum = 0, millisecondSum = 0;
            int myIterations = byteCount <= 1024 * 1024 ? Iterations : Math.Max(Iterations / 4, 0);

            for (int i = 0; i < myIterations; i++)
            {
                Stopwatch benchResult = RunBenchmark(algorithmFactory, byteCount);

                tickSum += benchResult.ElapsedTicks;
                millisecondSum += benchResult.ElapsedMilliseconds;
            }

            Ticks = tickSum / myIterations;
            Milliseconds = millisecondSum / myIterations;
        }

        public Stopwatch RunBenchmark(Func<HashAlgorithm> algorithmFactory, int byteCount)
        {
            byte[] data = new byte[byteCount];
            using (RNGCryptoServiceProvider rando = new RNGCryptoServiceProvider())
            {
                rando.GetBytes(data);
            }

            Stopwatch sw = new Stopwatch();

            using (HashAlgorithm algo = algorithmFactory())
            {
                for (int i = 0; i < WarmUpCount; i++)
                {
                    algo.ComputeHash(data);
                }

                sw.Reset();

                sw.Start();
                byte[] result = algo.ComputeHash(data);
                sw.Stop();
            }

            return sw;
        }
    }
}
