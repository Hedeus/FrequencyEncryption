using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            string str = "aboba";
            Dictionary<string, int> abetka = new Dictionary<string, int>();
            string s;
            int n;
            foreach (var c in str)
            {
                s = "" + c;
                if (!abetka.TryGetValue(s, out n))
                    n = 0;
                abetka[s] = n + 1;
            }

            foreach (var pair in abetka.OrderBy(pair => pair.Value))
            {
                Console.WriteLine("{0} - {1}", pair.Key, pair.Value);
            }
            Console.WriteLine();

            abetka = abetka.OrderBy(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);

            Console.WriteLine("0 - {0} - {1}", abetka.ElementAt(0).Key, abetka.ElementAt(0).Value);
            Console.WriteLine("1 - {0} - {1}", abetka.ElementAt(1).Key, abetka.ElementAt(1).Value);
            Console.WriteLine("2 - {0} - {1}", abetka.ElementAt(2).Key, abetka.ElementAt(2).Value);

            Console.ReadLine();
        }
    }
}
