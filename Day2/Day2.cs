using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode2020.Day2
{
    public class Day2
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public Day2(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void RunExample()
        {
            var validPasswords = ExtractValidPasswords("Day2/example.txt").ToList();

            Assert.Equal(2, validPasswords.Count);
            _testOutputHelper.WriteLine($"Valid passwords: {validPasswords.Count}");
        }

        [Fact]
        public void Run1()
        {
            var validPasswords = ExtractValidPasswords("Day2/input.txt");
            
            _testOutputHelper.WriteLine($"Valid passwords: {validPasswords.Count()}");
        }

        private static IEnumerable<string> ExtractValidPasswords(string file)
        {
            var example = File.ReadAllText(file);

            var r = new Regex("(?<from>[0-9]+)-(?<to>[0-9]+) (?<letter>[a-zA-Z]+): (?<input>[a-zA-Z]+)", RegexOptions.IgnoreCase);

            var validPasswords = r.Matches(example)
                .Where(row => row.Input()
                    .Count(c => c == row.Letter())
                    .IsBetween(row.From(), row.To()))
                .Select(s => s.Value);

            return validPasswords;
        }

        [Fact]
        public void RunExample2()
        {
            var validPasswords = ExtractValidPasswords2("Day2/example.txt").ToList();

            Assert.Equal(1, validPasswords.Count);
            _testOutputHelper.WriteLine($"Valid passwords: {validPasswords.Count}");
        }
        
        [Fact]
        public void Run2()
        {
            var validPasswords = ExtractValidPasswords2("Day2/input.txt");
            
            _testOutputHelper.WriteLine($"Valid passwords: {validPasswords.Count()}");
        }
        

        private static IEnumerable<string> ExtractValidPasswords2(string file)
        {
            var example = File.ReadAllText(file);

            var r = new Regex("(?<from>[0-9]+)-(?<to>[0-9]+) (?<letter>[a-zA-Z]+): (?<input>[a-zA-Z]+)", RegexOptions.IgnoreCase);

            var validPasswords = r.Matches(example)
                .Where(row => row.Input().ElementAt(row.From() - 1) == row.Letter() ^ 
                              row.Input().ElementAt(row.To() - 1) == row.Letter())
                .Select(s => s.Value);

            return validPasswords;
        }
    }

    public static class Extensions
    {
        public static char Letter(this Match row)
        {
            return char.Parse(row.Groups["letter"].Value);
        }
        
        public static string Input(this Match row)
        {
            return row.Groups["input"].Value;
        }

        public static int From(this Match row)
        {
            return int.Parse(row.Groups["from"].Value);
        }

        public static int To(this Match row)
        {
            return int.Parse(row.Groups["to"].Value);
        }
        
        public static bool IsBetween(this int value, int minimum, int maximum)
        {
            return value >= minimum && value <= maximum;
        }
    }
}