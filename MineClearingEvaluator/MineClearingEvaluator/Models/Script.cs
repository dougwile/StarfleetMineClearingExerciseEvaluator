using System.Collections.Generic;

namespace MineClearingEvaluator.Models
{
    public class Script
    {
        public Script(IList<Instruction> instructions)
        {
            Instructions = instructions;
        }

        public IList<Instruction> Instructions { get; private set; }
    }
}