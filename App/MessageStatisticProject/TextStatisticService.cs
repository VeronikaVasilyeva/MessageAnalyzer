using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace MessageAnalyzer.StatisticService
{
    public class TextStatisticService
    {
        public static Dictionary<char, double> GetLetterFrequency(string text)
        {
            if (string.IsNullOrEmpty(text))
                return new Dictionary<char, double>();

            var regex = new Regex("[\\d\\W_]"); //регулярка для удаления цифр и всех не буквенных символов
            var pureText = regex.Replace(text, string.Empty);

            var pureTextLength = (double) pureText.Length;

            return pureText.ToCharArray()
                .GroupBy(i => i)
                .ToDictionary(i => i.Key, j => Math.Round(j.Count() / pureTextLength, 3));
        }
    }
}