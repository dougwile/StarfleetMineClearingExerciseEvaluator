using System;
using System.Collections.Generic;
using System.Linq;
using MineClearingEvaluator.Common;
using MineClearingEvaluator.Services;

namespace MineClearingEvaluator.Models
{
    public interface ISimulation
    {
        Field Field { get; }
        Instruction NextInstruction { get; }

        bool IsComplete { get; }
        int StepCount { get; }
        bool Passed { get; }
        int Score { get; }
        void Step();
    }

    public class Simulation : ISimulation
    {
        private static IDictionary<FiringPattern, IList<Offset>> _firingPatternOffsetMapping;
        private static IDictionary<Direction, Offset> _directionOffsetMapping;
        private int _distanceMoved;
        private readonly int _initialMineCount;
        private readonly Queue<Instruction> _instructions;
        private int _shotsFired;

        public Simulation(Field field, Queue<Instruction> instructions)
        {
            if (field == null || field.Ship == null || field.Mines == null)
            {
                throw new ArgumentException("Invalid field");
            }
            if (instructions == null)
            {
                throw new ArgumentException("Instructions must not be null");
            }

            Field = field;
            _instructions = instructions;
            StepCount = 0;
            _initialMineCount = field.Mines.Count;
            _shotsFired = 0;
            _distanceMoved = 0;

            IsComplete = false;
            Passed = false;
            Score = 0;

            UpdateSimulationResults();

            _firingPatternOffsetMapping = new Dictionary<FiringPattern, IList<Offset>>
            {
                {
                    FiringPattern.Alpha,
                    new List<Offset>
                    {
                        new Offset(-1, -1),
                        new Offset(-1, 1),
                        new Offset(1, -1),
                        new Offset(1, 1)
                    }
                },
                {
                    FiringPattern.Beta,
                    new List<Offset>
                    {
                        new Offset(-1, 0),
                        new Offset(0, -1),
                        new Offset(0, 1),
                        new Offset(1, 0)
                    }
                },
                {
                    FiringPattern.Gamma,
                    new List<Offset>
                    {
                        new Offset(-1, 0),
                        new Offset(0, 0),
                        new Offset(1, 0)
                    }
                },
                {
                    FiringPattern.Delta,
                    new List<Offset>
                    {
                        new Offset(0, -1),
                        new Offset(0, 0),
                        new Offset(0, 1)
                    }
                }
            };

            _directionOffsetMapping = new Dictionary<Direction, Offset>
            {
                {Direction.North, new Offset(0, -1)},
                {Direction.South, new Offset(0, 1)},
                {Direction.East, new Offset(1, 0)},
                {Direction.West, new Offset(-1, 0)}
            };
        }

        public Field Field { get; }
        public Instruction NextInstruction => _instructions.Any() ? _instructions.Peek() : null;

        public int StepCount { get; private set; }

        public bool IsComplete { get; private set; }
        public bool Passed { get; private set; }
        public int Score { get; private set; }

        public void Step()
        {
            if (NextInstruction == null)
            {
                throw new Exception("Simulation has already completed.");
            }

            var instruction = _instructions.Dequeue();

            if (instruction.FiringPattern != FiringPattern.None)
            {
                var torpedoesCoords = GetTorpedoesCoordinates(Field.Ship.Coordinates, instruction.FiringPattern);

                var remainingMines = new List<Mine>();
                foreach (var mine in Field.Mines)
                {
                    if (!torpedoesCoords.Any(t => t.X == mine.Coordinates.X && t.Y == mine.Coordinates.Y))
                    {
                        remainingMines.Add(mine);
                    }
                }

                Field.Mines = remainingMines;
                _shotsFired++;
            }

            if (instruction.Direction != Direction.None)
            {
                var shipOffset = _directionOffsetMapping[instruction.Direction];

                Field.Ship.Coordinates.X += shipOffset.X;
                Field.Ship.Coordinates.Y += shipOffset.Y;
                _distanceMoved++;
            }

            Field.Ship.Coordinates.Z++;

            StepCount++;
            UpdateSimulationResults();
        }

        private void UpdateSimulationResults()
        {
            if (Field.Mines.Any(x => x.Coordinates.Z <= Field.Ship.Coordinates.Z))
            {
                IsComplete = true;
                Passed = false;
                Score = 0;
                return;
            }

            if (NextInstruction == null && Field.Mines.Any())
            {
                IsComplete = true;
                Passed = false;
                Score = 0;
                return;
            }

            if (!Field.Mines.Any() && NextInstruction != null)
            {
                IsComplete = true;
                Passed = true;
                Score = 1;
                return;
            }

            if (!Field.Mines.Any() && NextInstruction == null)
            {
                IsComplete = true;
                Passed = true;

                Score = 10*_initialMineCount -
                        Math.Min(5*_shotsFired, 5*_initialMineCount) -
                        Math.Min(3*_distanceMoved, 3*_initialMineCount);
            }
        }

        private IList<Coordinates> GetTorpedoesCoordinates(Coordinates shipCoords, FiringPattern firingPattern)
        {
            var offsets = _firingPatternOffsetMapping[firingPattern];

            return
                offsets.Select(offset => new Coordinates(shipCoords.X + offset.X, shipCoords.Y + offset.Y, shipCoords.Z))
                    .ToList();
        }
    }
}