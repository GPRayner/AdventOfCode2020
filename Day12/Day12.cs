using System;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace AdventOfCode2020.Day12
{
    public class Day12
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public Day12(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Part1Example()
        {
            var actual = GetPosition("Day12/example.txt");
            Assert.Equal(17 + 8, actual);
        }
        
        [Fact]
        public void Part1()
        {
            var actual = GetPosition("Day12/input.txt");
            _testOutputHelper.WriteLine($"Manhattan distance: {actual}");
            Assert.Equal(364, actual);
        }
        
        [Fact]
        public void Part2Example()
        {
            var actual = GetPositionWithWP("Day12/example.txt");
            Assert.Equal(214 + 72, actual);
        }

        [Fact]
        public void Part2()
        {
            var actual = GetPositionWithWP("Day12/input.txt");
            _testOutputHelper.WriteLine($"Manhattan distance: {actual}");
            Assert.Equal(39518, actual);
        }

        [Fact]
        public void RotationTest()
        {
            var x = 10;
            var y = 4;
            var (newX, newY) = GetNewLocation(x , y, -90);

            Assert.Equal(4, newX);
            Assert.Equal(-10, newY);
        }

        private static (int, int) GetNewLocation(int x, int y, double rotation)
        {
            return rotation switch
            {
                90 => (-y, x),
                180 => (-x, -y),
                270 => (y, -x),
                _ => throw new NotSupportedException($"Rotation {rotation} not supported")
            };
        }

        private static int GetPosition(string file)
        {
            var boatActions = File.ReadLines(file).Select(s => new BoatAction(s[0], int.Parse(s.Substring(1))));
            int NS = 0;
            int EW = 0;
            int direction = 0;
            foreach (var boatAction in boatActions)
            {
                switch (boatAction.Action)
                {
                    //  Action N means to move north by the given value.
                    case 'N':
                        NS += boatAction.Amount;
                        break;
                    //  Action S means to move south by the given value.
                    case 'S':
                        NS -= boatAction.Amount;
                        break;
                    //  Action E means to move east by the given value.
                    case 'E':
                        EW += boatAction.Amount;
                        break;
                    //  Action W means to move west by the given value.
                    case 'W':
                        EW -= boatAction.Amount;
                        break;
                    //  Action L means to turn left the given number of degrees.
                    case 'L':
                        direction -= boatAction.Amount;
                        if (direction <= -360)
                            direction += 360;
                        break;
                    //  Action R means to turn right the given number of degrees.
                    case 'R':
                        direction += boatAction.Amount;
                        if (direction >= 360)
                            direction -= 360;
                        break;
                    //  Action F means to move forward by the given value in the direction the ship is currently facing.
                    case 'F':
                        switch (direction)
                        {
                            case 0:
                                EW += boatAction.Amount;
                                break;
                            case 180:
                            case -180:
                                EW -= boatAction.Amount;
                                break;
                            case 90:
                            case -270:
                                NS -= boatAction.Amount;
                                break;
                            case -90:
                            case 270:
                                NS += boatAction.Amount;
                                break;
                            default:
                                throw new NotSupportedException($"Direction {direction} not supported");
                        }

                        break;
                }
            }
            var actual = Math.Abs(NS) + Math.Abs(EW);
            return actual;
        }

        private static int GetPositionWithWP(string file)
        {
            var boatActions = File.ReadLines(file).Select(s => new BoatAction(s[0], int.Parse(s.Substring(1))));
            int NS = 0;
            int WPNS = 1;
            int EW = 0;
            int WPEW = 10;
            foreach (var boatAction in boatActions)
            {
                switch (boatAction.Action)
                {
                    case 'N':
                        WPNS += boatAction.Amount;
                        break;
                    case 'S':
                        WPNS -= boatAction.Amount;
                        break;
                    case 'E':
                        WPEW += boatAction.Amount;
                        break;
                    case 'W':
                        WPEW -= boatAction.Amount;
                        break;
                    case 'L':
                        (WPEW, WPNS) = GetNewLocation(WPEW, WPNS, boatAction.Amount);
                        break;
                    case 'R':
                        (WPEW, WPNS) = GetNewLocation(WPEW, WPNS, 360 - boatAction.Amount);
                        break;
                    case 'F':
                        NS += boatAction.Amount * WPNS;
                        EW += boatAction.Amount * WPEW;
                        break;
                    default:
                        throw new NotSupportedException($"Action {boatAction.Action} not supported");
                }
            }
            return Math.Abs(NS) + Math.Abs(EW);
        }
    }

    public record BoatAction
    {
        public char Action { get; }
        public int Amount { get; }

        public BoatAction(char action, int amount)
        {
            Action = action;
            Amount = amount;
        }
    }
}