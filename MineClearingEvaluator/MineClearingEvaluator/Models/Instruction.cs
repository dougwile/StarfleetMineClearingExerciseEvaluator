using MineClearingEvaluator.Common;

namespace MineClearingEvaluator.Models
{
    public class Instruction
    {
        public Instruction(FiringPattern firingPattern, Direction direction)
        {
            Direction = direction;
            FiringPattern = firingPattern;
        }
        public Direction Direction { get; private set; }
        public FiringPattern FiringPattern { get; private set; }
    }
}