using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode2020.Day6
{
    public class Day6
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public Day6(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void ExampleFilePart1()
        {
            var sumOfAnswers = SumOfAllAnswers("Day6/example.txt");

            Assert.Equal(11, sumOfAnswers.Sum());
        }

        private static IEnumerable<int> SumOfAllAnswers(string file) =>
            File.ReadAllText(file).Split("\r\n\r\n").Select(s => s.Split("\r\n").Aggregate((s1, s2) => new string(s1.Union(s2).ToArray())).Length);

        private static IEnumerable<int> SumOfCommonAnswers(string file) =>
            File.ReadAllText(file).Split("\r\n\r\n").Select(s => s.Split("\r\n").Aggregate((s1, s2) => new string(s1.Intersect(s2).ToArray())).Length);

        [Fact]
        public void RunPart1()
        {
            var sumOfAnswers = SumOfAllAnswers("Day6/input.txt");
            _testOutputHelper.WriteLine($"Sum of Questions: {sumOfAnswers.Sum()}");
        }

        [Fact]
        public void ExampleFilePart2()
        {
            var sumOfAnswers = SumOfCommonAnswers("Day6/example.txt");

            Assert.Equal(6, sumOfAnswers.Sum());
        }

        [Fact]
        public void RunPart2()
        {
            var sumOfAnswers = SumOfCommonAnswers("Day6/input.txt");
            _testOutputHelper.WriteLine($"Sum of Questions: {sumOfAnswers.Sum()}");
        }
    }
}