using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode2020.Day9
{
    public class Day9
    {
        private static IEnumerable<long> GetSuspects(IReadOnlyCollection<long> collection, int offSet) =>
            collection.Skip(offSet)
                .Where((s, i) => collection.Skip(i).Take(i + offSet)
                    .CrossJoin(collection.Skip(i + 1).Take(i + offSet))
                    .All(a => a.Item1 + a.Item2 != s));

        private static IEnumerable<Block> GetBlocks(IReadOnlyCollection<long> collection, int sumLookingFor)
        {
            return collection
                .Select((s, i) =>
                    collection.Skip(i).Aggregate(new Block(sumLookingFor), (b, a) => b.Add(a)))
                .Where(w => w.IsSumLooking());
        }

        private class Block
        {
            private readonly long _sumLookingFor;
            private long _min = long.MaxValue;
            private long _max = long.MinValue;
            private long _sum;

            public Block(long sumLookingFor)
            {
                _sumLookingFor = sumLookingFor;
            }

            public bool IsSumLooking() => _sumLookingFor == _sum && (_min != _max);

            public Block Add(long number)
            {
                if (_sum >= _sumLookingFor)
                    return this;
                _sum += number;
                _max = _max > number ? _max : number;
                _min = _min < number ? _min : number;
                return this;
            }

            public long Answer => _min + _max;
        }

        [Fact]
        public void RulePart1Example()
        {
            var collection = File.ReadLines("Day9/example.txt").Select(long.Parse).ToList();

            var suspects = GetSuspects(collection, 5);
            Assert.Equal(127, suspects.Single());
        }

        [Fact]
        public void RulePart1()
        {
            var collection = File.ReadLines("Day9/input.txt").Select(long.Parse).ToList();

            var suspects = GetSuspects(collection, 25).ToList();
            _testOutputHelper.WriteLine($"suspect: {suspects.First()}");
            Assert.Equal(105950735, suspects.First());
        }

        [Fact]
        public void RulePart2Example()
        {
            var collection = File.ReadLines("Day9/example.txt").Select(long.Parse).ToList();
            var blocks = GetBlocks(collection, 127);

            Assert.Equal(15 + 47, blocks.Single().Answer);
        }

        [Fact]
        public void RulePart2()
        {
            var collection = File.ReadLines("Day9/input.txt").Select(long.Parse).ToList();
            var blocks = GetBlocks(collection, 105950735);

            _testOutputHelper.WriteLine($"Answer: {blocks.Single().Answer}");
        }

        private readonly ITestOutputHelper _testOutputHelper;

        public Day9(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }
    }

    public static class Ext
    {
        public static IEnumerable<Tuple<T1, T2>> CrossJoin<T1, T2>(this IEnumerable<T1> s1, IEnumerable<T2> s2)
        {
            return s1.SelectMany(t1 => s2.Select(t2 => Tuple.Create(t1, t2)));
        }
    }
}