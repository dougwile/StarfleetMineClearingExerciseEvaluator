using MineClearingEvaluator.Models;

namespace MineClearingEvaluator.Services
{
    public interface IFieldPrinter
    {
        string Print(Field field);
    }

    public class FieldPrinter: IFieldPrinter
    {
        public string Print(Field field)
        {
            return "";
        }
    }
}