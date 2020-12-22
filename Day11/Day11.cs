using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode2020.Day11
{
    public class Day11
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public Day11(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Part1Example()
        {
            var actual = GetSeats("Day11/example.txt", 4, GetAdjacent);
            Assert.Equal(37, actual);
        }

        [Fact]
        public void Part1()
        {
            var actual = GetSeats("Day11/input.txt", 4, GetAdjacent);
            Assert.Equal(2249, actual);
            _testOutputHelper.WriteLine($"Number of full seats: {actual}");
        }

        [Fact]
        public void Part2Example()
        {
            var actual = GetSeats("Day11/example.txt", 5, GetLineOfSightAdjacent);
            Assert.Equal(26, actual);
        }
        
        [Fact]
        public void Part2()
        {
            var actual = GetSeats("Day11/input.txt", 5, GetLineOfSightAdjacent);
            Assert.Equal(2023, actual);
            _testOutputHelper.WriteLine($"Number of full seats: {actual}");
        }

        private int GetSeats(string file, int tooManyCount, Func<char[][], int, int, IEnumerable<char>> getAdjacentSeats)
        {
            var initialState = File.ReadLines(file).Select(s => s.ToArray()).ToArray();

            var previousState = CopyState(initialState);
            while (true)
            {
                var newState = CopyState(previousState);
                var row = 0;
                foreach (var rowList in previousState)
                {
                    var col = 0;
                    foreach (var seat in rowList)
                    {
                        switch (seat)
                        {
                            case 'L':
                            {
                                var adjacent = getAdjacentSeats(previousState, row, col);
                                if (adjacent.All(a => a == '.' || a == 'L'))
                                    newState[row][col] = '#';
                                break;
                            }
                            case '#':
                            {
                                var adjacent = getAdjacentSeats(previousState, row, col);
                                if (adjacent.Count(a => a == '#') >= tooManyCount)
                                    newState[row][col] = 'L';
                                break;
                            }
                        }

                        col++;
                    }

                    row++;
                }
                
                if (GetString(newState) == GetString(previousState))
                    break;
                previousState = CopyState(newState);
            }

            return previousState.SelectMany(m => m.Where(s => s == '#')).Count();
        }

        private static string GetString(char[][] newState)
        {
            return string.Join("", newState.SelectMany(m => m.Select(s => s)));
        }

        private static char[][] CopyState(char[][] initialState)
        {
            return initialState.Select(s => s.Select(s2 => s2).ToArray()).ToArray();
        }

        private IEnumerable<char> GetAdjacent(char[][] state, int row, int col)
        {
            if (TryGetValue(state, row - 1, col - 1, out var topLeft))
                yield return topLeft;
            if (TryGetValue(state, row - 1, col, out var topMid))
                yield return topMid;
            if (TryGetValue(state, row - 1, col + 1, out var topRight))
                yield return topRight;

            if (TryGetValue(state, row, col - 1, out var left))
                yield return left;
            if (TryGetValue(state, row, col + 1, out var right))
                yield return right;

            if (TryGetValue(state, row + 1, col - 1, out var bottomLeft))
                yield return bottomLeft;
            if (TryGetValue(state, row + 1, col, out var bottomMid))
                yield return bottomMid;
            if (TryGetValue(state, row + 1, col + 1, out var bottomRight))
                yield return bottomRight;
        }

        private IEnumerable<char> GetLineOfSightAdjacent(char[][] state, int row, int col)
        {
            if (TryFindSeat(state, row, col, r => r - 1, c => c - 1, out var topLeft))
                yield return topLeft;
            if (TryFindSeat(state, row, col, r => r - 1, c => c, out var topMid))
                yield return topMid;
            if (TryFindSeat(state, row, col, r => r - 1, c => c + 1, out var topRight))
                yield return topRight;

            if (TryFindSeat(state, row, col, r => r, c => c - 1, out var left))
                yield return left;
            if (TryFindSeat(state, row, col, r => r, c => c + 1, out var right))
                yield return right;

            if (TryFindSeat(state, row, col, r => r + 1, c => c - 1, out var bottomLeft))
                yield return bottomLeft;
            if (TryFindSeat(state, row, col, r => r + 1, c => c, out var bottomMid))
                yield return bottomMid;
            if (TryFindSeat(state, row, col, r => r + 1, c => c + 1, out var bottomRight))
                yield return bottomRight;
        }

        private bool TryFindSeat(char[][] state, int row, int col, Func<int, int> rowFunc, Func<int, int> colFunc,
            out char value)
        {
            while (true)
            {
                row = rowFunc(row);
                col = colFunc(col);
                if (!TryGetValue(state, row, col, out var thisValue))
                {
                    value = default;
                    return false;
                }

                if (thisValue == '.')
                    continue;

                value = thisValue;
                return true;
            }
        }

        private bool TryGetValue(char[][] state, int row, int col, out char value)
        {
            var rowList = state.ElementAtOrDefault(row);
            if (rowList == null)
            {
                value = default;
                return false;
            }

            value = rowList.ElementAtOrDefault(col);
            return value != default;
        }
    }
}