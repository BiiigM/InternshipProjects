using System;
using System.Collections.Generic;

namespace Schulaufgabe
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            int cache = 100;
            int pin19 = 181;
            
            cache &= ~pin19;
            
            Console.WriteLine(ToBinäry(cache));
            Console.ReadLine();
        }

        private static string ToBinäry(int nr)
        {
            List<string> tmpBinär = new List<string>();
            
            int tmpNr = nr;

            while (tmpNr != 0)
            {
                tmpBinär.Add(tmpNr % 2 + "");
                tmpNr /= 2;
            }
            
            tmpBinär.Reverse();
            string output = String.Join("", tmpBinär);
            
            return output;
        }
    }
}