using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace App.StatisticService.UnitTest
{
    [TestFixture]
    class TextStatisticServiceTest
    {
        [Test]
        public void EmptyStringTest()
        {
            Assert.AreEqual(null, TextStatisticService.GetLetterFrequency(""));
        }

        [Test]
        public void StringTest()
        {
            var expected = new Dictionary<char, double>
            {
                {'s', 0.0714},
                {'d', 0.1428},
                {'f', 0.1428},
                {'g', 0.1428},
                {'h', 0.1428},
                {'j', 0.1428},
                {'k', 0.1428},
                {'l', 0.0714}
            };

            var actual = TextStatisticService.GetLetterFrequency("sdf ghjk dfgh jkl;");

            Assert.AreEqual(expected.Keys, actual.Keys);

            foreach (var key in expected.Keys)
            {
                Assert.AreEqual(expected[key], actual[key]);
            }
        }
    }
}
