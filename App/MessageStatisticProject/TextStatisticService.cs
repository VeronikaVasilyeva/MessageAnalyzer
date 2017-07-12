using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace App.StatisticService
{
    public class TextStatisticService
    {
        public static Dictionary<char, double> GetLetterFrequency(string text)
        {
            if (text == null || text.Equals(String.Empty)) return null;

            var result = new Dictionary<char, double>();

            var regex = new Regex("[\\d\\W]");
            var chars = regex.Replace(text, string.Empty).ToCharArray();

            double addition = 1 / (double) chars.Length;

            foreach (var character in chars)
            {
                if (!result.ContainsKey(character))
                {
                    result[character] = 0;
                }

                result[character] = Math.Round(result[character] + addition, 4);
            }

            return result;
        }
    }
}
