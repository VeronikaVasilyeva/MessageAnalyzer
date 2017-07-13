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
        public void NullTest()
        {
            Assert.AreEqual(new Dictionary<char, double>(), TextStatisticService.GetLetterFrequency(null));
        }

        [Test]
        public void EmptyStringTest()
        {
            Assert.AreEqual(new Dictionary<char, double>(), TextStatisticService.GetLetterFrequency(""));
        }

        [Test]
        public void SimpleStringTest()
        {
            var actual = TextStatisticService.GetLetterFrequency("sdf ghjk dfgh jkl;");

            var expected = new Dictionary<char, double>
            {
                {'s', 0.071},
                {'d', 0.143},
                {'f', 0.143},
                {'g', 0.143},
                {'h', 0.143},
                {'j', 0.143},
                {'k', 0.143},
                {'l', 0.071}
            };

            AssertDictionary(expected, actual);
        }

        [Test]
        public void ManySpacesStringTest()
        {
            var actual = TextStatisticService.GetLetterFrequency("        to much          spaces                  ");

            var expected = new Dictionary<char, double>
            {
                {'t', 0.083},
                {'o', 0.083},
                {'m', 0.083},
                {'u', 0.083},
                {'c', 0.167},
                {'h', 0.083},
                {'s', 0.167},
                {'p', 0.083},
                {'a', 0.083},
                {'e', 0.083}
            };

            AssertDictionary(expected, actual);
        }

        [Test]
        public void StringWithDigitalTest()
        {
            var actual = TextStatisticService.GetLetterFrequency("3456789 string with digital 1234567890");

            var expected = new Dictionary<char, double>
            {
                {'s', 0.059},
                {'t', 0.176},
                {'r', 0.059},
                {'i', 0.235},
                {'n', 0.059},
                {'g', 0.118},
                {'w', 0.059},
                {'h', 0.059},
                {'d', 0.059},
                {'a', 0.059},
                {'l', 0.059}
            };

            AssertDictionary(expected, actual);
        }

        private void AssertDictionary(Dictionary<char, double> expected, Dictionary<char, double> actual)
        {
            Assert.AreEqual(expected.Keys, actual.Keys);

            foreach (var key in expected.Keys)
            {
                Assert.AreEqual(expected[key], actual[key], 0.001);
            }
        }
    }
}
