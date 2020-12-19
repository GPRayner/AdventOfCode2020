using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Sdk;

namespace AdventOfCode2020.Day7
{
    public class Day7
    {
        [Fact]
        public void Part1Example()
        {
            var bags = CreateLookup("Day7/example.txt");

            var countGoldBags = bags.Count(m => m.Value.ContainsGoldBag(bags) == true);
            Assert.Equal(4, countGoldBags);
        }
        
        [Fact]
        public void RunPart1()
        {
            var bags = CreateLookup("Day7/input.txt");

            var countGoldBags = bags.Count(m => m.Value.ContainsGoldBag(bags) == true);
            Assert.Equal(177, countGoldBags);
        }

        [Fact]
        public void Part2Example()
        {
            var bags = CreateLookup("Day7/example.txt");

            var countGoldBags = bags["shiny_gold"].CountBags(bags);
            Assert.Equal(32, countGoldBags - 1);
        }
        
        [Fact]
        public void Part2Example2()
        {
            var bags = CreateLookup("Day7/examplePart2.txt");

            var countGoldBags = bags["shiny_gold"].CountBags(bags);
            Assert.Equal(126, countGoldBags - 1);
        }
        
        [Fact]
        public void RunPart2()
        {
            var bags = CreateLookup("Day7/input.txt");

            var countGoldBags = bags["shiny_gold"].CountBags(bags);
            Assert.Equal(34988, countGoldBags - 1);
        }
        
        private static Dictionary<string, BagContents> CreateLookup(string file) =>
            File.ReadLines(file).Select(s => 
                new BagContents(s.Replace("bags", string.Empty)
                    .Replace("bag", string.Empty)
                    .Replace("no other", string.Empty)
                    .Trim('.').Split("contain"))).ToDictionary(k => k.Name);
    }

    public class BagContents
    {
        private string _shinyGold = "shiny_gold";

        public BagContents(string[] strings)
        {
            Name = strings.First().Trim().Replace(" ","_" );
            Contains = strings.ElementAt(1).Split(',').Select(s => s.Trim()).Where(w => !string.IsNullOrWhiteSpace(w))
                .ToDictionary(k => new string(k.Skip(2).ToArray()).Replace(" ", "_"), v => int.Parse(v.FirstOrDefault().ToString()));
        }

        public bool ContainsGoldBag(Dictionary<string, BagContents> lookup)
        {
            var containsGoldBag = false;
            foreach (var (key, _) in Contains)
            {
                if (key == _shinyGold)
                    containsGoldBag = true;
                containsGoldBag = containsGoldBag || lookup[key].ContainsGoldBag(lookup);
            }

            return containsGoldBag;
        }
        
        public int CountBags(Dictionary<string, BagContents> lookup)
        {
            var count = 1;
            foreach (var (key, value) in Contains)
            {
                count += value * lookup[key].CountBags(lookup);
            }
            return count;
        }
        
        public Dictionary<string, int> Contains { get; set; }
        public string Name { get; set; }
    }
}