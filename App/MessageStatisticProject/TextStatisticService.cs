using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace App.StatisticService
{
    class TextStatisticService
    {
        public static Dictionary<char, float> GetLetterFrequency(string text)
        {
            if (text == null || text.Equals(String.Empty)) return null;

            var result = new Dictionary<char, float>();

            var regex = new Regex("[\\d\\W]");
            var chars = regex.Replace(text, string.Empty).ToCharArray();

            float addition = 1 / (float)chars.Length;

            foreach (var character in chars)
            {
                if (!result.ContainsKey(character))
                {
                    result[character] = 0;
                }

                result[character] += addition;
            }

            return result;
        }
    }
}
