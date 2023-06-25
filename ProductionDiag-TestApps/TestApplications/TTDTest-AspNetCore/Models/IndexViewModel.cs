using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TTDTest_AspNetCore.Models
{
    public class IndexViewModel
    {
        private readonly DateTime generatedTime;
        public DateTime GeneratedTime
        {
            get
            {
                return generatedTime;
            }
        }

        public int BitsSelected
        {
            get
            {
                return (generatedTime.Millisecond % 8) + 3;
            }
        }
        public string MathFunction
        {
            get
            {
                if (BitsSelected % 3 == 0)
                {
                    return "DoFun1";
                }
                else if (BitsSelected % 3 == 1)
                {
                    return "DoTheMath2";
                }
                else if (BitsSelected % 3 == 2)
                {
                    return "DoMyHash3";
                }
                return "[Out Of Range]";
            }
        }

        public int CheesecakeCount
        {
            get
            {
                return Cheesecakes.Count();
            }
        }

        public IEnumerable<string> CheesecakeNames { get; set; }

        public IEnumerable<Cheesecake> Cheesecakes
        {
            get
            {
                return CheesecakeNames.Select(ccn => new Cheesecake(ccn, BitsSelected));
            }
        }

        public IndexViewModel()
        {
            generatedTime = DateTime.UtcNow;
        }
    }

    public class Cheesecake
    {
        public readonly string Name;

        public readonly bool IsChocolate, IsCheesecake, IsCake, IsCitrus, IsFruit;
        public readonly string RegisteredTrademark, RegisteredCopyright;

        public readonly List<int> magicNumbers;
        public IEnumerable<int> MagicNumbers
        {
            get
            {
                return magicNumbers;
            }
        }

        public Cheesecake(string name, int bitsSelected)
        {
            List<int> tempMagicNums = new List<int>();

            this.Name = name;
            this.IsChocolate = name.Contains("chocolate", StringComparison.InvariantCultureIgnoreCase);
            this.IsCheesecake = name.Contains("cheesecake", StringComparison.InvariantCultureIgnoreCase);

            string[] citrus = new string[] { "lime", "lemon" };
            this.IsCitrus = citrus.Select(f => name.Contains(f, StringComparison.InvariantCultureIgnoreCase)).Where(r => r).Any();

            string[] fruits = new string[] { "raspberry", "mango", "strawberry", "pumpkin", "banana", "pineapple" };
            this.IsFruit = fruits.Select(f => name.Contains(f, StringComparison.InvariantCultureIgnoreCase)).Where(r => r).Any();

            string tnp = $" {name} ";
            this.IsCake = tnp.Contains(" cake ", StringComparison.InvariantCultureIgnoreCase);

            if (name.Contains("®"))
            {
                int locCopy = tnp.IndexOf("®");
                int spStart = tnp.LastIndexOf(' ', locCopy, locCopy + 1);
                int spEnd = tnp.IndexOf(' ', locCopy);
                this.RegisteredCopyright =  tnp.Substring(spStart + 1, spEnd - spStart - 1);
                tempMagicNums.AddRange(DoDecomposition(this.RegisteredCopyright, bitsSelected));
            }
            else
            {
                this.RegisteredCopyright = string.Empty;
            }

            if (name.Contains("™"))
            {
                int locTrade = tnp.IndexOf("™");
                int spStart = tnp.LastIndexOf(' ', locTrade, locTrade + 1);
                int spEnd = tnp.IndexOf(' ', locTrade);
                this.RegisteredTrademark = tnp.Substring(spStart + 1, spEnd - spStart - 1);
                tempMagicNums.AddRange(DoDecomposition(this.RegisteredTrademark, bitsSelected));
            }
            else
            {
                this.RegisteredTrademark = string.Empty;
            }

            tempMagicNums.AddRange(DoDecomposition(this.Name, bitsSelected));

            this.magicNumbers = tempMagicNums.Distinct().OrderBy(i => i).ToList();
        }

        public IEnumerable<int> DoDecomposition(string str, int bitsPicked)
        {
            long number = 0;
            if (bitsPicked % 3 == 0)
            {
                number = DoFun1(str);
            }
            else if (bitsPicked % 3 == 1)
            {
                number = DoTheMath2(str);
            }
            else if (bitsPicked % 3 == 2)
            {
                number = DoMyHash3(str);
            }

            IEnumerable<int> decomposition = DecomposeNumber(number, bitsPicked);

            return decomposition;
        }

        public long DoFun1(string str)
        {
            List<int> list = new List<int>();
            for (int i = 0; i < str.Length; i++)
            {
                list.Add(str[i] - 'a');
            }

            long result = 0;
            result += str.Length;
            for (int i = 0; i < list.Count; i++)
            {
                result = result * 8 + (list[i] ^ (i * 3));
            }

            result -= str.Length;
            return result;
        }

        public long DoTheMath2(string str)
        {
            List<int> list = new List<int>();
            for (int i = 0; i < str.Length; i++)
            {
                list.Add(str[i] - 'A');
            }

            long result = 0;
            result += str.Length;
            for (int i = 0; i < list.Count; i++)
            {
                result = result * 4 + (list[i] ^ (i * 7));
            }

            result -= str.Length;
            return result;
        }

        public long DoMyHash3(string str)
        {
            List<int> list = new List<int>();
            for (int i = 0; i < str.Length; i++)
            {
                list.Add(str[i] - ' ');
            }

            long result = 0;
            result += str.Length;
            for (int i = 0; i < list.Count; i++)
            {
                result = result * 16 + (list[i] ^ (i * 5));
            }

            result -= str.Length;
            return result;
        }

        public IEnumerable<int> DecomposeNumber(long number, int bitsPicked)
        {
            for(int i = 0; i < (sizeof(long)*8) - bitsPicked; i += bitsPicked)
            {
                long num = number >> i;
                long numMask = (0x01 << bitsPicked) - 1;
                num &= numMask;
                if (num > 0 && num < numMask)
                {
                    int intNum = (int)num;
                    yield return intNum; 
                }
            }
        }
    }
}
