using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode2020.Day4
{
    public class Day4
    {
        private readonly ITestOutputHelper _testOutputHelper;

        private static readonly HashSet<string> EyeColours = new HashSet<string>()
        {
            "amb", "blu", "brn", "gry", "grn", "hzl", "oth"
        };

        private static readonly IDictionary<string, Func<string, bool>> RequiredFieldsValidation = new Dictionary<string, Func<string, bool>>
        {
            {"byr", s => int.TryParse(s, out var year) && year >= 1920 && year <= 2002},
            {"iyr", s => int.TryParse(s, out var year) && year >= 2010 && year <= 2020},
            {"eyr", s => int.TryParse(s, out var year) && year >= 2020 && year <= 2030},
            {"hgt", s =>
                {
                    if (s.EndsWith("cm"))
                        return int.TryParse(s.TrimEnd("cm".ToCharArray()), out var cm) && cm >= 150 && cm <= 193;
                    if (s.EndsWith("in"))
                        return int.TryParse(s.TrimEnd("in".ToCharArray()), out var inch) && inch >= 59 && inch <= 76;
                    return false;
                }
            },
            {"hcl", s => s.StartsWith('#') && s.Length == 7 && int.TryParse(s.Trim('#'), NumberStyles.HexNumber, CultureInfo.InvariantCulture, out _)},
            {"ecl", s => EyeColours.Contains(s)},
            {"pid", s => s.Length == 9 && s.All(char.IsDigit)}
        };

        private readonly Func<Dictionary<string, string>, bool> _part1Predicate = w => RequiredFieldsValidation.Keys.All(a => w.Keys.Contains(a));

        private readonly Func<Dictionary<string, string>, bool> _part2Predicate = p =>
            RequiredFieldsValidation.All(a => p.TryGetValue(a.Key, out var value) && a.Value(value));

        private IEnumerable<Dictionary<string, string>> ValidPassports(string file, Func<Dictionary<string, string>, bool> predicate) =>
            File.ReadAllText(file).Split("\r\n\r\n")
                .Select(s => s.Replace("\r\n", " ")
                    .Split(" ").ToDictionary(k => k.Split(":").First(), v => v.Split(":")[1]))
                .Where(predicate);

        [Fact]
        public void RunPart1Example()
        {
            var validPassports = ValidPassports("Day4/example.txt", _part1Predicate);
            Assert.Equal(2, validPassports.Count());
        }

        [Fact]
        public void RunPart1()
        {
            var validPassports = ValidPassports("Day4/input.txt", _part1Predicate).ToList();
            Assert.Equal(235, validPassports.Count());
            _testOutputHelper.WriteLine($"Valid passports: {validPassports.Count()}");
        }

        [Theory]
        [InlineData("byr", true, "2002")]
        [InlineData("byr", false, "2003")]
        [InlineData("hgt", true, "60in")]
        [InlineData("hgt", true, "190cm")]
        [InlineData("hgt", false, "190in")]
        [InlineData("hgt", false, "190")]
        [InlineData("hcl", true, "#123abc")]
        [InlineData("hcl", false, "#123abz")]
        [InlineData("hcl", false, "123abc")]
        [InlineData("ecl", true, "brn")]
        [InlineData("ecl", false, "wat")]
        [InlineData("pid", true, "000000001")]
        [InlineData("pid", false, "0123456789")]
        public void RunPart2Validation(string key, bool expected, string input) => Assert.Equal(expected, RequiredFieldsValidation[key](input));

        [Fact]
        public void RunPart2ValidExample()
        {
            Assert.Equal(4, ValidPassports("Day4/valid.txt", _part2Predicate).Count());
        }

        [Fact]
        public void RunPart2InvalidExample()
        {
            Assert.Empty(ValidPassports("Day4/invalid.txt", _part2Predicate));
        }

        [Fact]
        public void RunPart2()
        {
            var validPassports = ValidPassports("Day4/input.txt", _part2Predicate).ToList();
            Assert.Equal(194, validPassports.Count());
            _testOutputHelper.WriteLine($"Valid passports: {validPassports.Count()}");
        }

        public Day4(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }
    }
}