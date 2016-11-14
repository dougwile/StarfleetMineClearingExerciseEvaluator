using System;
using System.Collections.Generic;
using MineClearingEvaluator.Common;
using MineClearingEvaluator.Models;

namespace MineClearingEvaluator.Services
{
    public interface IScriptParser
    {
        Queue<Instruction> Parse(string scriptText);
    }

    public class ScriptParser : IScriptParser
    {
        private readonly IDictionary<string, FiringPattern> _firingPatternMapping = new Dictionary<string, FiringPattern>()
        {
            {"alpha", FiringPattern.Alpha },
            {"beta", FiringPattern.Beta },
            {"gamma", FiringPattern.Gamma },
            {"delta", FiringPattern.Delta },
        };

        private readonly IDictionary<string, Direction> _directionMapping = new Dictionary<string, Direction>()
        {
            {"north", Direction.North },
            {"south", Direction.South },
            {"east", Direction.East },
            {"west", Direction.West },
        };

        public Queue<Instruction> Parse(string scriptText)
        {
            var instructions = new Queue<Instruction>();

            var lines = scriptText?.Trim().Replace("\r", "").Split(new string[] {"\n"}, StringSplitOptions.RemoveEmptyEntries) ?? new string[0];

            foreach (var line in lines)
            {
                var firingPattern = FiringPattern.None;
                var direction = Direction.None;
                var shootFirst = true;

                var lineInstructions = line.Split();

                if (lineInstructions.Length == 2)
                {
                    if (_firingPatternMapping.ContainsKey(lineInstructions[0]) &&
                        _directionMapping.ContainsKey(lineInstructions[1]))
                    {
                        firingPattern = _firingPatternMapping[lineInstructions[0]];
                        direction = _directionMapping[lineInstructions[1]];
                    }
                    else if (_firingPatternMapping.ContainsKey(lineInstructions[1]) &&
                             _directionMapping.ContainsKey(lineInstructions[0]))
                    {
                        firingPattern = _firingPatternMapping[lineInstructions[1]];
                        direction = _directionMapping[lineInstructions[0]];
                        shootFirst = false;
                    }
                    else
                    {
                        throw new ValidationException($"Invalid script: {lineInstructions} is not a valid instruction");
                    }
                }
                else if (lineInstructions.Length == 1)
                {
                    if (_firingPatternMapping.ContainsKey(lineInstructions[0]))
                    {
                        firingPattern = _firingPatternMapping[lineInstructions[0]];
                    }
                    else if (_directionMapping.ContainsKey(lineInstructions[0]))
                    {
                        direction = _directionMapping[lineInstructions[0]];
                    }
                    else
                    {
                        throw new ValidationException(
                            $"Invalid script: {lineInstructions[0]} is not a valid firing pattern or direction");
                    }
                }
                else
                {
                    throw new ValidationException($"Invalid script: {lineInstructions} is not a valid instruction");
                }

                instructions.Enqueue(new Instruction(firingPattern, direction, line, shootFirst));
            }


            return instructions;
        }
    }
}