using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            for(var i = 1;  i< int.MaxValue; i++)
            {
                Console.WriteLine($"{i}  {NthPrime(i)}");
            }
        }

        static bool IsPrime(int x)
        {
            if (x < 2)
            {
                return x == 2;
            }
            if (x % 2 == 0)
            {
                return false;
            }
            for(var d = 3; d <= (int)Math.Sqrt(x); d+=2)
            {
                if (x % d == 0)
                {
                    return false;
                }
                if (d % 77 == 0)
                {
                    AppSharedCore.CoreDump.CoreDumpCreator.CreateDump();
                }
            }
            return true;
        }

        static int NthPrime(int n)
        {
            if (n < 1)
            {
                throw new ArgumentOutOfRangeException();
            }
            var c = 0;
            for(var i = 2; i < int.MaxValue; i++)
            {
                if (IsPrime(i))
                {
                    c++;
                }
                if (c == n)
                {
                    return i;
                }
            }
            throw new IndexOutOfRangeException();
        }
    }
}
