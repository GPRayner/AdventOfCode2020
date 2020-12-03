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

        private static long GetCount(IEnumerable<string> lines, int width, int right, int down) => lines
            .Where((s, i) => i % down == 0)
            .Select((row, i) => row.ElementAt(PickElement(i, width, right)))
            .Count(c => c == '#');

        private static int PickElement(int row, int width, int right) => ((row * right) - ((row * right / width) * width));

        [Fact]
        public void RunPart1Example()
        {
            var lines = File.ReadAllLines("Day3/example.txt");

            var count = GetCount(lines, lines.First().Length, 3, 1);
            Assert.Equal(7, count);
        }

        [Fact]
        public void RunPart1()
        {
            var lines = File.ReadAllLines("Day3/input.txt");

            var count = GetCount(lines, lines.First().Length, 3, 1);
            _testOutputHelper.WriteLine($"Count: {count}");
        }

        [Fact]
        public void RunPart2Example()
        {
            var lines = File.ReadAllLines("Day3/example.txt");

            var width = lines.First().Length;
            var count1 = GetCount(lines, width, 1, 1);
            var count2 = GetCount(lines, width, 3, 1);
            var count3 = GetCount(lines, width, 5, 1);
            var count4 = GetCount(lines, width, 7, 1);
            var count5 = GetCount(lines, width, 1, 2);

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

            var width = lines.First().Length;
            var count1 = GetCount(lines, width, 1, 1);
            var count2 = GetCount(lines, width, 3, 1);
            var count3 = GetCount(lines, width, 5, 1);
            var count4 = GetCount(lines, width, 7, 1);
            var count5 = GetCount(lines, width, 1, 2);
            var product = count1 * count2 * count3 * count4 * count5;
            _testOutputHelper.WriteLine($"Product: {product}");
        }

        [Theory]
        [InlineData(0, 11, 0)]
        [InlineData(1, 11, 3)]
        [InlineData(2, 11, 6)]
        [InlineData(3, 11, 9)]
        [InlineData(4, 11, 1)]
        [InlineData(5, 11, 4)]
        [InlineData(6, 11, 7)]
        [InlineData(7, 11, 10)]
        [InlineData(8, 11, 2)]
        public void TestPickElement(int row, int width, int expected)
        {
            Assert.Equal(expected, PickElement(row, width, 3));
        }

        [Theory]
        [InlineData(0, 11, 0)]
        [InlineData(1, 11, 1)]
        [InlineData(2, 11, 2)]
        [InlineData(3, 11, 3)]
        [InlineData(4, 11, 4)]
        public void TestPickElementDown2(int row, int width, int expected)
        {
            Assert.Equal(expected, PickElement(row, width, 1));
        }
    }
}