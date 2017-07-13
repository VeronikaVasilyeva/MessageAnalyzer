using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App
{
    class User
    {
        public string Name { get; set; }
        public List<string> Enties { get; set; }
        public Dictionary<char, double> Statistic { get; set; }
    }
}
