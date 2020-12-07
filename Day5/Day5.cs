using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode2020.Day5
{
    public class Day5
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public Day5(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        public class Seat
        {
            public Range Row { get; set; }
            public Range Col { get; set; }
            public int Number() => Row.End.Value * 8 + Col.End.Value;

            public Seat()
            {
                Row = new Range(0, 127);
                Col = new Range(0, 7);
            }
        }

        [Theory]
        [InlineData("FBFBBFFRLR", 44, 5, 357)]
        [InlineData("BFFFBBFRRR", 70, 7, 567)]
        [InlineData("FFFBBBFRRR", 14, 7, 119)]
        [InlineData("BBFFBBFRLL", 102, 4, 820)]
        public void ExamplePart1(string input, int expectedRow, int expectedCol, int expected)
        {
            var seat = GetSeat(input);

            Assert.Equal(expectedRow, seat.Row.End);
            Assert.Equal(expectedCol, seat.Col.End);
            Assert.Equal(expected, seat.Number());
        }

        [Fact]
        public void RunPart1()
        {
            var highestSeat = File.ReadLines("Day5/input.txt").Max(m => GetSeat(m).Number());
            _testOutputHelper.WriteLine($"Highest seat: {highestSeat}");
        }

        [Fact]
        public void RunPart2()
        {
            var allSeats = File.ReadLines("Day5/input.txt").Select(m => GetSeat(m).Number()).OrderBy(o => o).ToList();
            var mySeat = allSeats.Zip(allSeats.Skip(1)).Single(tuple => tuple.Second - tuple.First != 1);
            _testOutputHelper.WriteLine($"My seat: {mySeat.First + 1}");
        }

        private static Seat GetSeat(string input)
        {
            var seat = new Seat();
            foreach (var letter in input)
            {
                switch (letter)
                {
                    case 'F':
                        seat.Row = seat.Row.Front();
                        break;
                    case 'B':
                        seat.Row = seat.Row.Back();
                        break;
                    case 'L':
                        seat.Col = seat.Col.Front();
                        break;
                    case 'R':
                        seat.Col = seat.Col.Back();
                        break;
                }
            }
            return seat;
        }
    }

    public static class Extensions
    {
        public static int Middle(this Range range) => (range.End.Value + range.Start.Value) / 2;
        public static Range Front(this Range range) => new Range(range.Start, range.Middle());
        public static Range Back(this Range range) => new Range(range.Middle(), range.End);
        
    }
}