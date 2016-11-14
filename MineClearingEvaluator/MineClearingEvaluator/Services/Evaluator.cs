using MineClearingEvaluator.Models;

namespace MineClearingEvaluator.Services
{
    public interface IEvaluator
    {
        string Evaluate(string field, string script);
    }

    public class Evaluator : IEvaluator
    {
        private readonly IFieldParser _fieldParser;
        private readonly IScriptParser _scriptParser;
        private readonly ISimulator _simulator;
        private readonly IFieldPrinter _fieldPrinter;

        public Evaluator(
            IFieldParser fieldParser,
            IScriptParser scriptParser,
            ISimulator simulator,
            IFieldPrinter fieldPrinter)
        {
            _fieldParser = fieldParser;
            _scriptParser = scriptParser;
            _simulator = simulator;
            _fieldPrinter = fieldPrinter;
        }

        public string Evaluate(string fieldText, string scriptText)
        {
            var field = _fieldParser.Parse(fieldText);
            var script = _scriptParser.Parse(scriptText);
            var output = "";

            for (var i = 0; i < script.Instructions.Count; i++)
            {
                var instruction = script.Instructions[i];

                output += $"Step {i + 1}\n\n";
                output += $"{_fieldPrinter.Print(field)}\n\n";
                output += $"{instruction.Text}";

                field = _simulator.Simulate(field, instruction);

                output += $"{_fieldPrinter.Print(field)}\n\n";
            }

            return output;
        }
    }

    
}