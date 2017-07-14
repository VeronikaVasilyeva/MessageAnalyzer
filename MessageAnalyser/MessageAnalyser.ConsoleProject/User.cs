using System.Collections.Generic;

namespace MessageAnalyzer.ConsoleProject
{
    internal class User
    {
        public string Name { get; set; }
        public List<string> Enties { get; set; }
        public Dictionary<char, double> Statistic { get; set; }
    }
}