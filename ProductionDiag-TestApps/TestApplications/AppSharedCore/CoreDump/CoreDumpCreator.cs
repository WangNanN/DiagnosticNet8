using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace AppSharedCore.CoreDump
{
    public static class CoreDumpCreator
    {
        public static void CreateDump()
        {
            List<string> fruits = new List<string> { "apple", "passionfruit", "banana", "mango", "orange", "blueberry", "grape", "strawberry" };
            IEnumerable<string> query = fruits.Where(fruit => fruit.Length < 6);
            foreach (string fruit in query)
            {
                Console.WriteLine(fruit);
            }

            object[] pList = new object[] { 1, "one", 2, "two", 3, "three" };
            var query1 = pList.OfType<string>();
            foreach(var q in query1)
            {
                Console.WriteLine(q);
            }

            Dictionary<string, string> mygroup = new Dictionary<string, string>() { { "Hannah","Zhang"},{ "Alex","Yao"},{ "Alisa","Zhang"}
                ,{ "Nelson","Yan"},{ "Richard","Zeng"},{ "Clarie","Kang"},{ "Qian","Wang"},{ "Serena","Wang"},{ "Maggie","Zhang"},{ "Cherry","Wu"}
                ,{ "Lynn","Zhang"},{ "Grace","Dong"} };
            
            List<string> stringDates = new List<string> { "Friday, April 10, 2009", "not a day", "Saturday, August 29, 2020"};
            var dates = stringDates.Select(d => DateTime.TryParse(d, out var result) ? (DateTime?)result : null).ToArray();

            var text = "The quick brown fox jumps over the lazy dog";
            var word1 = text.AsSpan().Slice(0, 3);
            var word2 = text.AsSpan().Slice(4, 5);
            var word3 = text.AsSpan().Slice(10, 5);

            WriteDump();
        }

        public static void WriteDump()
        {
            try
            {
                var installDir = Path.GetDirectoryName(typeof(System.Object).Assembly.Location);
                var createDumpToolPath = Path.Combine(installDir, "createdump");

                var spanTest = createDumpToolPath.AsSpan();
                var spanTest2 = spanTest.Slice(3,2);

                var currentThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
                var nativeThreadId = -1;
                try
                {
                    nativeThreadId = syscall(SYS_gettid);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex);
                }

                var scenarioName = GetEnvVar("SCENARIO_NAME", "unknown").Replace('.', '_');
                var dumpType = GetEnvVar("DUMP_TYPE", "normal"); // normal withheap triage full
                bool skipDumpType = (string.CompareOrdinal(dumpType, "default") == 0);
                var coreDumpName = $"{scenarioName}-{dumpType}-{currentThreadId}-{nativeThreadId}";
                var coreDumpPath = $"/coredumps/{coreDumpName}";

                //https://github.com/dotnet/runtime/blob/master/docs/design/coreclr/botr/xplat-minidump-generation.md#configurationpolicy
                var args = skipDumpType ?  $"{Process.GetCurrentProcess().Id} -f {coreDumpPath}" : $"{Process.GetCurrentProcess().Id} -f {coreDumpPath} --{dumpType}";
                Console.WriteLine($"Starting {createDumpToolPath} {args}");

                using (var createDumpProc = Process.Start(new ProcessStartInfo(createDumpToolPath, args)))
                {
                    createDumpProc.WaitForExit();
                }

                Console.WriteLine($"Finished writing the dump to {coreDumpPath}");
                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                System.Threading.Thread.Sleep(TimeSpan.FromDays(10));
            }

        }

        private static string GetEnvVar(string varName, string defaultValue) => Environment.GetEnvironmentVariable(varName) ?? defaultValue;

        private const int SYS_gettid = 186;

        [DllImport("libc", SetLastError = true)]
        public static extern int syscall(int number);
    }
}