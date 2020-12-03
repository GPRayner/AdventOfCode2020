using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode2020.Day3
{
    public class Day3
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public Day3(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        private static long GetCount(IEnumerable<string> lines, int right, int down) => lines
            .Where((s, i) => i % down == 0)
            .Select((row, i) => row.ElementAt(PickElement(i, row.Length, right)))
            .Count(c => c == '#');

        private static int PickElement(int row, int width, int right) => row * right % width;

        [Fact]
        public void RunPart1Example()
        {
            var lines = File.ReadAllLines("Day3/example.txt");

            var count = GetCount(lines, 3, 1);
            Assert.Equal(7, count);
        }

        [Fact]
        public void RunPart1()
        {
            var lines = File.ReadAllLines("Day3/input.txt");

            var count = GetCount(lines, 3, 1);
            _testOutputHelper.WriteLine($"Count: {count}");
            Assert.Equal(162, count);
        }

        [Fact]
        public void RunPart2Example()
        {
            var lines = File.ReadAllLines("Day3/example.txt");

            var count1 = GetCount(lines, 1, 1);
            var count2 = GetCount(lines, 3, 1);
            var count3 = GetCount(lines, 5, 1);
            var count4 = GetCount(lines, 7, 1);
            var count5 = GetCount(lines, 1, 2);

            Assert.Equal(2, count1);
            Assert.Equal(7, count2);
            Assert.Equal(3, count3);
            Assert.Equal(4, count4);
            Assert.Equal(2, count5);
            Assert.Equal(336, count1 * count2 * count3 * count4 * count5);
        }

        [Fact]
        public void RunPart2()
        {
            var lines = File.ReadAllLines("Day3/input.txt");

            var product = new[]
            {
                GetCount(lines, 1, 1),
                GetCount(lines, 3, 1),
                GetCount(lines, 5, 1),
                GetCount(lines, 7, 1),
                GetCount(lines, 1, 2)
            }.Aggregate((a1, a2) => a1 * a2);

            _testOutputHelper.WriteLine($"Product: {product}");
            Assert.Equal(3064612320, product);
        }

        [Theory]
        [InlineData(0, 11, 3, 0)]
        [InlineData(1, 11, 3, 3)]
        [InlineData(2, 11, 3, 6)]
        [InlineData(3, 11, 3, 9)]
        [InlineData(4, 11, 3, 1)]
        [InlineData(5, 11, 3, 4)]
        [InlineData(6, 11, 3, 7)]
        [InlineData(7, 11, 3, 10)]
        [InlineData(8, 11, 3, 2)]
        [InlineData(0, 11, 1, 0)]
        [InlineData(1, 11, 1, 1)]
        [InlineData(2, 11, 1, 2)]
        [InlineData(3, 11, 1, 3)]
        [InlineData(4, 11, 1, 4)]
        public void TestPickElement(int row, int width, int right, int expected)
        {
            Assert.Equal(expected, PickElement(row, width, right));
        }
    }
}