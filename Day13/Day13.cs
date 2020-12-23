using System;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode2020.Day13
{
    public class Day13
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public Day13(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void RunPart1Example()
        {
            var firstBus = GetFirstBus("Day13/example.txt");
            Assert.Equal(295, firstBus);
        }

        [Fact]
        public void RunPart1()
        {
            var firstBus = GetFirstBus("Day13/input.txt");
            _testOutputHelper.WriteLine($"Result: {firstBus}");
            Assert.Equal(3385, firstBus);
        }

        [Fact]
        public void RunPart2Example()
        {
            var count = GetTimeTable(File.ReadLines("Day13/example.txt").Skip(1).Single());
            _testOutputHelper.WriteLine($"Result: {count}");
            Assert.Equal(1068781u, count);
        }

        [Fact]
        public void RunPart2()
        {
            var count = GetTimeTable(File.ReadLines("Day13/input.txt").Skip(1).Single());
            _testOutputHelper.WriteLine($"Result: {count}");
        }
        
        [Theory]
        [InlineData("17,x,13,19", 3417)]
        [InlineData("67,7,59,61", 754018)]
        [InlineData("67,x,7,59,61", 779210)]
        [InlineData("67,7,x,59,61", 1261476)]
        [InlineData("1789,37,47,1889", 1202161486)]
        public void Tests(string input, ulong result)
        {
            Assert.Equal(result, GetTimeTable(input));
        }
        
        private static ulong GetTimeTable(string input)
        {
            var buses = input.Split(",")
                .Select((s, i) => s == "x" ? null : new BusTime(ulong.Parse(s), (ulong)i))
                .Where(w => w != null).Select(s => s!).ToList();

            var firstBus = buses.First();
            ulong count = firstBus.Bus;
            ulong skip = firstBus.Bus;
            foreach (var busTime in buses.Skip(1))
            {
                while ((count + busTime.Interval) % busTime.Bus != 0)
                {
                    count += skip;
                }

                skip *= busTime.Bus;
            }
            return count;
        }
        
        public record BusTime
        {
            public ulong Bus { get; }
            public ulong Interval { get; }

            public BusTime(ulong bus, ulong interval)
            {
                Bus = bus;
                Interval = interval;
            }
        }

        private static double GetFirstBus(string file)
        {
            var startTime = double.Parse(File.ReadLines(file).First());
            var buses = File.ReadLines(file).Skip(1).Single().Split(",").Where(w => w != "x").Select(double.Parse);

            var firstBus = buses
                .Select(bus => new {waitTime = (Math.Ceiling(startTime / bus) * bus) - startTime, bus})
                .OrderBy(o => o.waitTime)
                .First();

            return firstBus.bus * firstBus.waitTime;
        }
    }
}