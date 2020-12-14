using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode2020.Day10
{
    public class Day10
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public Day10(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void RunPart1Example()
        {
            var tracker = NumberOfJolts("Day10/example.txt");

            Assert.Equal(22, tracker.NumberOfOneJolts);
            Assert.Equal(10, tracker.NumberOfThreeJolts);
        }

        [Fact]
        public void RunPart1()
        {
            var tracker = NumberOfJolts("Day10/input.txt");

            _testOutputHelper.WriteLine($"Number {tracker.NumberOfOneJolts * tracker.NumberOfThreeJolts}");
            Assert.Equal(2210, tracker.NumberOfOneJolts * tracker.NumberOfThreeJolts);
        }

        [Fact]
        public void RunPart2Example()
        {
            var actual = GetResult("Day10/example.txt");
            Assert.Equal(19208, actual);
        }

        [Fact]
        public void RunPart2()
        {
            var result = GetResult("Day10/input.txt");

            _testOutputHelper.WriteLine($"Count: {result}");
        }

        private static long GetResult(string file)
        {
            var lines = File.ReadAllLines(file).Select(int.Parse).OrderBy(o => o).ToList();
            lines.Insert(0, 0);
            lines.Add(lines.Last() + 3);

            var routesCount = new Dictionary<int, long>
            {
                [0] = 1
            };

            foreach (var jolt in lines.Skip(1))
            {
                routesCount[jolt] = routesCount.GetValueOrDefault(jolt - 1) + 
                                    routesCount.GetValueOrDefault(jolt - 2) +
                                    routesCount.GetValueOrDefault(jolt - 3);
            }
            
            return routesCount[lines.Last()];
        }

        private static JoltTracker NumberOfJolts(string file) => File.ReadAllLines(file).Select(int.Parse)
            .OrderBy(o => o)
            .Aggregate(new JoltTracker(), (tracker, i) => tracker.Next(i));

        private class JoltTracker
        {
            private int? _previous = null;

            public JoltTracker Next(int current)
            {
                if (_previous.HasValue)
                {
                    var jump = current - _previous;
                    if (jump == 1)
                        NumberOfOneJolts++;
                    if (jump == 3)
                        NumberOfThreeJolts++;
                }

                _previous = current;
                return this;
            }

            public int NumberOfOneJolts { get; private set; } = 1;
            public int NumberOfThreeJolts { get; private set; } = 1;
        }
    }
};