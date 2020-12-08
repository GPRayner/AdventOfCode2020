using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode2020.Day8
{
    public class Day8
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public Day8(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void RunPart1Example()
        {
            var commands = GetCommands("Day8/example.txt");
            TryGetAccumulator(i => commands.ElementAtOrDefault(i), out var accumulator);

            Assert.Equal(5, accumulator);
        }

        [Fact]
        public void RunPart1()
        {
            var commands = GetCommands("Day8/input.txt");
            TryGetAccumulator(i => commands.ElementAtOrDefault(i), out var accumulator);

            _testOutputHelper.WriteLine($"Accumulator: {accumulator}");
        }

        [Fact]
        public void RunPart2Example()
        {
            var accumulator = BruteForce("Day8/examplePart2.txt");

            Assert.Equal(8, accumulator);
        }

        [Fact]
        public void RunPart2()
        {
            var accumulator = BruteForce("Day8/input.txt");
            
            _testOutputHelper.WriteLine($"Accumulator: {accumulator}");
            Assert.Equal(1688, accumulator);
        }
        
        private static int BruteForce(string file)
        {
            var commands = GetCommands(file);
            var commandsToSwitch = commands.Where(w => w.Command == "nop" || w.Command == "jmp");
            var accumulator = 0;
            foreach (var cmd in commandsToSwitch)
            {
                if (!TryGetAccumulator(i => cmd.Index == i ? cmd.SwitchedCommand() : commands.ElementAtOrDefault(i), out accumulator))
                {
                    break;
                }
            }

            return accumulator;
        }

        private static bool TryGetAccumulator(Func<int, Cmd> getCommandAt, out int outAccumulator)
        {
            var visited = new HashSet<int>();
            var nextCommand = getCommandAt(0);
            var errored = false;
            var accumulator = 0;
            while (true)
            {
                if (visited.Contains(nextCommand.Index))
                {
                    errored = true;
                    break;
                }

                visited.Add(nextCommand.Index);

                switch (nextCommand.Command)
                {
                    case "nop":
                        nextCommand = GetNext(getCommandAt, nextCommand);
                        break;
                    case "jmp":
                        nextCommand = GetJump(getCommandAt, nextCommand);
                        break;
                    case "acc":
                        accumulator += nextCommand.Value;
                        nextCommand = GetNext(getCommandAt, nextCommand);
                        break;
                }

                if (nextCommand == null)
                    break;
            }

            outAccumulator = accumulator;
            return errored;
        }

        private static Cmd[] GetCommands(string file)
        {
            return File.ReadLines(file).Select((s, i) => new Cmd(s, i)).ToArray();
        }

        private static Cmd GetJump(Func<int, Cmd> getCommandAt, Cmd nextCommand) => getCommandAt(nextCommand.Index + nextCommand.Value);
        private static Cmd GetNext(Func<int, Cmd> getCommandAt, Cmd nextCommand) => getCommandAt(nextCommand.Index + 1);
    }

    public class Cmd
    {
        public Cmd SwitchedCommand() => new Cmd() {Command = Command == "nop" ? "jmp" : "nop", Value = this.Value, Index = this.Index};

        private Cmd()
        {
        }

        public Cmd(string input, int index)
        {
            Index = index;
            var parts = input.Split(" ");
            Command = parts.First();
            Value = int.Parse(parts[1]);
        }

        public int Index { get; set; }

        public int Value { get; set; }
        public string Command { get; set; }
    }
}