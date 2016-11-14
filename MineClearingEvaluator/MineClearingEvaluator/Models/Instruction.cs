using MineClearingEvaluator.Common;

namespace MineClearingEvaluator.Models
{
    public class Instruction
    {
        public Instruction(FiringPattern firingPattern, Direction direction, string text)
        {
            Direction = direction;
            FiringPattern = firingPattern;
            Text = text;
        }
        public Direction Direction { get; private set; }
        public FiringPattern FiringPattern { get; private set; }
        public string Text { get; private set; }
    }
}