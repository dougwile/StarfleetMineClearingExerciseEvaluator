using System.Collections.Generic;
using MineClearingEvaluator.Common;
using MineClearingEvaluator.Models;

namespace MineClearingEvaluator.Services
{
    public interface IScriptParser
    {
        Script Parse(string scriptText);
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

        public Script Parse(string scriptText)
        {
            var instructions = new List<Instruction>();

            var lines = scriptText?.Trim().Split(new char[] {'\r', '\n'}) ?? new string[0];

            foreach (var line in lines)
            {
                var firingPattern = FiringPattern.None;
                var direction = Direction.None;

                var lineInstructions = line.Split();

                if (lineInstructions.Length == 2)
                {
                    if (!_firingPatternMapping.ContainsKey(lineInstructions[0]))
                    {
                        throw new ValidationException($"Invalid script: {lineInstructions[0]} is not a valid firing pattern");
                    }
                    if (!_directionMapping.ContainsKey(lineInstructions[1]))
                    {
                        throw new ValidationException($"Invalid script: {lineInstructions[1]} is not a valid direction");
                    }
                    firingPattern = _firingPatternMapping[lineInstructions[0]];
                    direction = _directionMapping[lineInstructions[1]];
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

                instructions.Add(new Instruction(firingPattern, direction));
            }


            return new Script(instructions);
        }
    }
}