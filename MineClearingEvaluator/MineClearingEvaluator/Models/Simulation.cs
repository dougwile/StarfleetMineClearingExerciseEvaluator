using System;
using System.Collections.Generic;
using System.Linq;
using MineClearingEvaluator.Common;

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

    /// <summary>
    ///     The Simulation contains the initial state of the field and the instructions to be executed.
    ///     At each call of the Step method, the simulation runs one instruction and updates the field.
    ///     At the same time, the relevant information about the state of the field is updated, including
    ///     whether the simulation is complete, whether it is passed or failed, and the score.
    ///     I modeled this class loosely on the concept of a game loop for video games. The game contains
    ///     some initial state, performs some logic each "frame", and then updates the state for the next
    ///     iteration.
    ///     This design could be improved by further breaking off the responsibilities of the simulation into
    ///     other classes. For example, the firing pattern offset mapping could be handled by a class dedicated
    ///     too translating the firing pattern into torpeo coordinates. I chose to keep the class as it is because
    ///     it is readable and maintainable enough, has few dependencies, and adheres to a fairly simple interface.
    ///     In the wise words of Donald Knuth, "premature optimization is the root of all evil".
    /// </summary>
    public class Simulation : ISimulation
    {
        private static IDictionary<FiringPattern, IList<Offset>> _firingPatternOffsetMapping;
        private static IDictionary<Direction, Offset> _directionOffsetMapping;
        private readonly int _initialMineCount;
        private readonly Queue<Instruction> _instructions;
        private int _distanceMoved;
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

                // The remaining mines are the ones where no torpedoes were fired in their XY lane
                var remainingMines = Field.Mines.Where(mine =>
                    torpedoesCoords.All(t =>
                        !(t.X == mine.Coordinates.X && t.Y == mine.Coordinates.Y)))
                    .ToList();

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

            // The ship descends one Z level each iteration
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