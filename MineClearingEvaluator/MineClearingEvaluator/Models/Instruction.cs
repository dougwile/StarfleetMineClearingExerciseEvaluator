using MineClearingEvaluator.Common;

namespace MineClearingEvaluator.Models
{
    /// <summary>
    /// An instruction determines which direction and firing pattern to execute, if any, as well as the 
    /// order to execute them in. It also contains the original text from the script for later use in the output.
    /// </summary>
    public class Instruction
    {
        public Instruction(FiringPattern firingPattern, Direction direction, string text, bool shootFirst)
        {
            Direction = direction;
            FiringPattern = firingPattern;
            Text = text;
            ShootFirst = shootFirst;
        }
        public Direction Direction { get; private set; }
        public FiringPattern FiringPattern { get; private set; }
        public string Text { get; private set; }
        public bool ShootFirst { get; private set; }
    }
}