using System.Collections.Generic;

namespace MineClearingEvaluator.Models
{
    public class Field
    {
        public Field(IList<Mine> mines, Ship ship)
        {
            Mines = mines;
            Ship = ship;
        }

        public Ship Ship { get; set; }

        public IList<Mine> Mines { get; set; }
    }
}