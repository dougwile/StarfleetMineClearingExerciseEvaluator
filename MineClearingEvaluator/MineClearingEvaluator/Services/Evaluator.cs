using System;
using System.Collections.Generic;
using MineClearingEvaluator.Common;
using MineClearingEvaluator.Models;

namespace MineClearingEvaluator.Services
{
    public interface IEvaluator
    {
        string Evaluate(string field, string script);
    }

    /// <summary>
    /// The evaluator handles all of the logic in the project apart from dealing with the command line.
    /// The evaluator first parses the field and script into a digestible format,
    /// then runs the simulation, building the output at each step. I chose this design because I wanted a single
    /// entry point into project from the command line so that I could write tests more easily. By having this top level view,
    /// the entire flow is clear and relatively clean. Each injected service performs a single 
    /// function (single responsibility principle from SOLID), although the Evaluator itself must piece it all together at the end.
    /// </summary>
    public class Evaluator : IEvaluator
    {
        private readonly IFieldParser _fieldParser;
        private readonly IScriptParser _scriptParser;
        private readonly ISimulationFactory _simulationFactory;
        private readonly IFieldPrinter _fieldPrinter;

        public Evaluator(
            IFieldParser fieldParser,
            IScriptParser scriptParser,
            ISimulationFactory simulationFactory,
            IFieldPrinter fieldPrinter)
        {
            _fieldParser = fieldParser;
            _scriptParser = scriptParser;
            _simulationFactory = simulationFactory;
            _fieldPrinter = fieldPrinter;
        }

        public string Evaluate(string fieldText, string scriptText)
        {
            var output = "";

            try
            {
                Field field;
                Queue<Instruction> instructions;

                try
                {
                    field = _fieldParser.Parse(fieldText);
                    instructions = _scriptParser.Parse(scriptText);
                }
                catch (ValidationException)
                {
                    output = "fail (0)";
                    return output;
                }

                var simulation = _simulationFactory.CreateSimulation(field, instructions);
                while (!simulation.IsComplete)
                {
                    output += $"Step {simulation.StepCount + 1}\n\n";
                    output += $"{_fieldPrinter.Print(simulation.Field)}\n\n";
                    output += $"{simulation.NextInstruction.Text}\n\n";

                    simulation.Step();

                    output += $"{_fieldPrinter.Print(simulation.Field)}\n\n";
                }

                var scoreTerm = simulation.Passed ? "pass" : "fail";
                output += $"{scoreTerm} ({simulation.Score})";

            }
            catch (Exception)
            {
                output += "ERROR: simulation failed unexpectedly";
            }

            return output;
        }
    }

    
}