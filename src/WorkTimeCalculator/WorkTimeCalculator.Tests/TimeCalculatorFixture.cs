using System;
using Xunit;

namespace WorkTimeCalculator.Tests
{
    public class TimeCalculatorFixture
    {
        [Fact]
        public void Add_10122018And4900_Returns121120180100()
        {
            var timeCalculator = new TimeCalculator();
            var expected = new DateTime(2018, 12, 12, 1, 0, 0);
            var actual = timeCalculator.Add(new DateTime(2018, 12, 10), new TimeSpan(49, 0, 0));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Add_10122018And2359_Returns101220182359()
        {
            var timeCalculator = new TimeCalculator();
            var expected = new DateTime(2018, 10, 12, 23, 59, 0);
            var actual = timeCalculator.Add(new DateTime(2018, 10, 12), new TimeSpan(23, 59, 0));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Add_101220181400And201600_Returns311220180600()
        {
            var timeCalculator = new TimeCalculator();
            var expected = new DateTime(2018, 12, 31, 6, 0, 0);
            var actual = timeCalculator.Add(new DateTime(2018, 12, 10, 14, 0, 0), new TimeSpan(20, 16, 0, 0));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Add_4900And10122018_Returns121120180100()
        {
            var timeCalculator = new TimeCalculator();
            var expected = new DateTime(2018, 12, 12, 1, 0, 0);
            var actual = timeCalculator.Add(new TimeSpan(49, 0, 0), new DateTime(2018, 12, 10));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Add_2359And10122018_Returns101220182359()
        {
            var timeCalculator = new TimeCalculator();
            var expected = new DateTime(2018, 10, 12, 23, 59, 0);
            var actual = timeCalculator.Add(new TimeSpan(23, 59, 0), new DateTime(2018, 10, 12));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Add_201600And101220181400_Returns311220180600()
        {
            var timeCalculator = new TimeCalculator();
            var expected = new DateTime(2018, 12, 31, 6, 0, 0);
            var actual = timeCalculator.Add(new TimeSpan(20, 16, 0, 0), new DateTime(2018, 12, 10, 14, 0, 0));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Subtract_4900From121120180100_Returns10122018()
        {
            var timeCalculator = new TimeCalculator();
            var expected = new DateTime(2018, 12, 10);
            var actual = timeCalculator.Subtract(new DateTime(2018, 12, 12, 1, 0, 0), new TimeSpan(49, 0, 0));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Subtract_2359From101220182359_Returns10122018()
        {
            var timeCalculator = new TimeCalculator();
            var expected = new DateTime(2018, 10, 12);
            var actual = timeCalculator.Subtract(new DateTime(2018, 10, 12, 23, 59, 0), new TimeSpan(23, 59, 0));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Subtract_201600From311220180600_Returns101220181400()
        {
            var timeCalculator = new TimeCalculator();
            var expected = new DateTime(2018, 12, 10, 14, 0, 0);
            var actual = timeCalculator.Subtract(new DateTime(2018, 12, 31, 6, 0, 0), new TimeSpan(20, 16, 0, 0));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Subtract_10122018From121120180100_Returns4900()
        {
            var timeCalculator = new TimeCalculator();
            var expected = new TimeSpan(49, 0, 0);
            var actual = timeCalculator.Subtract(new DateTime(2018, 12, 12, 1, 0, 0), new DateTime(2018, 12, 10));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Subtract_10122018From101220182359_Returns2359()
        {
            var timeCalculator = new TimeCalculator();
            var expected = new TimeSpan(23, 59, 0);
            var actual = timeCalculator.Subtract(new DateTime(2018, 10, 12, 23, 59, 0), new DateTime(2018, 10, 12));
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void Subtract_101220181400From311220180600_Returns201600()
        {
            var timeCalculator = new TimeCalculator();
            var expected = new TimeSpan(20, 16, 0, 0);
            var actual = timeCalculator.Subtract(new DateTime(2018, 12, 31, 6, 0, 0), new DateTime(2018, 12, 10, 14, 0, 0));
            Assert.Equal(expected, actual);
        }
    }
}
