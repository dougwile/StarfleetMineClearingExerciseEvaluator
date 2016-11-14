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

        public Evaluator(
            IFieldParser fieldParser,
            IScriptParser scriptParser)
        {
            _fieldParser = fieldParser;
            _scriptParser = scriptParser;
        }

        public string Evaluate(string fieldText, string scriptText)
        {
            var field = _fieldParser.Parse(fieldText);
            var script = _scriptParser.Parse(scriptText);



            return "Step 1\r\n\r\nz\r\n\r\ngamma\r\n\r\n.\r\n\r\npass (5)";
        }
    }
}