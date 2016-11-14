using System.Collections.Generic;
using System.Linq;
using MineClearingEvaluator.Common;
using MineClearingEvaluator.Models;

namespace MineClearingEvaluator.Services
{
    public interface ISimulator
    {
        Field Simulate(Field field, Instruction instruction);
    }

    public class Simulator : ISimulator
    {
        public Simulator()
        {
            _firingPatternOffsetMapping = new Dictionary<FiringPattern, IList<Offset>>()
            {
                {
                    FiringPattern.Alpha,
                    new List<Offset>()
                    {
                        new Offset(-1, -1),
                        new Offset(-1, 1),
                        new Offset(1, -1),
                        new Offset(1, 1)
                    }
                },
                {
                    FiringPattern.Beta,
                    new List<Offset>()
                    {
                        new Offset(-1, 0),
                        new Offset(0, -1),
                        new Offset(0, 1),
                        new Offset(1, 0)
                    }
                },
                {
                    FiringPattern.Gamma,
                    new List<Offset>()
                    {
                        new Offset(-1, 0),
                        new Offset(0, 0),
                        new Offset(1, 0),
                    }
                },
                {
                    FiringPattern.Delta,
                    new List<Offset>()
                    {
                        new Offset(0, -1),
                        new Offset(0, 0),
                        new Offset(0, 1),
                    }
                },
            };

            _directionOffsetMapping = new Dictionary<Direction, Offset>()
            {
                {Direction.North, new Offset(0, -1) },
                {Direction.South, new Offset(0, 1) },
                {Direction.East, new Offset(1, 0) },
                {Direction.West, new Offset(-1, 0) },
            };
        }

        private static IDictionary<FiringPattern, IList<Offset>> _firingPatternOffsetMapping;
        private static IDictionary<Direction, Offset> _directionOffsetMapping;

        public Field Simulate(Field field, Instruction instruction)
        {
            var ship = field.Ship;

            if (instruction.FiringPattern != FiringPattern.None)
            {
                
                var torpedoesCoords = GetTorpedoesCoordinates(field.Ship.Coordinates, instruction.FiringPattern);

                var remainingMines = new List<Mine>();
                foreach (var mine in field.Mines)
                {
                    if (!torpedoesCoords.Any(t => t.X == mine.Coordinates.X && t.Y == mine.Coordinates.Y))
                    {
                        remainingMines.Add(mine);
                    }
                }

                field.Mines = remainingMines;
            }

            if (instruction.Direction != Direction.None)
            {
                var shipOffset = _directionOffsetMapping[instruction.Direction];

                ship.Coordinates.X += shipOffset.X;
                ship.Coordinates.Y += shipOffset.Y;
            }

            return field;
        }

        private IList<Coordinates> GetTorpedoesCoordinates(Coordinates shipCoords, FiringPattern firingPattern)
        {
            var offsets = _firingPatternOffsetMapping[firingPattern];

            return
                offsets.Select(offset => new Coordinates(shipCoords.X + offset.X, shipCoords.Y + offset.Y, shipCoords.Z))
                    .ToList();
        }
    }

    public class Offset
    {
        public Offset(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; private set; }
        public int Y { get; private set; }
    }
}