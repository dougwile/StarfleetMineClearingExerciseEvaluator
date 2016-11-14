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
            var instructions = _scriptParser.Parse(scriptText);
            var output = "";

            var simulation = _simulator.CreateSimulation(field, instructions);
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

            return output;
        }
    }

    
}