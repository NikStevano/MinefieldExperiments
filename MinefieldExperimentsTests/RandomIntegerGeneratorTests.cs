using MinefieldExperiments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinefieldExperiments.Tests
{
    [TestClass()]
    public class RandomIntegerServiceTests
    {
        [TestMethod()]
        public void NextValueTest()
        {
            // this will make the initial value 1665
            RandomIntegerGenerator r = new RandomIntegerGenerator(13, 1);

            // next random will be ((1665*13)+1) % 65536 = 21646 and return should be 21646 % 64 = 14
            int firstValue = r.NextValue(0, 64);
            // next random will be ((21646*13)+1) % 65536 = 19255 and return should be 19255 % 64 = 55
            int secondValue = r.NextValue(0, 64);

            Assert.AreEqual(firstValue, 14);
            Assert.AreEqual(secondValue, 55);
        }

        [TestMethod()]
        public void Sequence1Test()
        {
            // this will make the initial value 1665
            RandomIntegerGenerator r = new RandomIntegerGenerator(13, 1);

            // expected sequence for range 1-64 should be:
            // 38 41 33 6 2 45 37 28 6 4 24 49 57 36 46 35 63 50 9 27
            int count6 = 0;
            for (int i = 0; i < 20; i++)
            {
                if (r.NextValue(1, 64) == 6)
                    count6++;
            }

            Assert.AreEqual(count6, 2);
        }

        [TestMethod()]
        public void Sequence2Test()
        {
            // this will make the initial value 1665
            RandomIntegerGenerator r = new RandomIntegerGenerator(13, 1);

            // expected sequence for range 0-64 should be (no duplicates!):
            // 14 55 12 29 58 51 24 57 38 47 36 21 18 43 48 49 62 39 60 13 42 35 8 41 22 31 20 5 2 27
            var values = new Dictionary<int, int>();
            for (int i = 0; i < 30; i++)
            {
                int value = r.NextValue(0, 64);
                if (values.ContainsKey(value))
                {
                    values[value]++;
                }
                else
                {
                    values[value] = 1;
                }
            }

            // verify no duplicates in the first 30 numbers returned
            bool hasDuplicates = false;
            foreach (var keyValue in values)
            {
                if (keyValue.Value > 1)
                {
                    hasDuplicates = true;
                    break;
                }
            }
            Assert.IsFalse(hasDuplicates);
        }

        [TestMethod()]
        public void SequenceBy1Test()
        {
            // this will make the initial value 128*1+1 = 129
            RandomIntegerGenerator r = new RandomIntegerGenerator(1, 1);

            // expected sequence should start at 129 and incease by 1 every time
            int value = 0;
            for (int i = 0; i < 10; i++)
            {
                value = r.NextValue();
            }

            Assert.AreEqual(value, 129 + 10);
        }

        [TestMethod()]
        public void RangeTest()
        {
            RandomIntegerGenerator r = new RandomIntegerGenerator();

            for (int i = 0; i < 1024; i++)
            {
                int value = r.NextValue(0, 4);
                // has to be within range
                Assert.IsTrue(value >= 0 && value < 4);
            }
        }

    }
}
