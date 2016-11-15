namespace MineClearingEvaluator.Models
{
    public class Offset
    {
        public Offset(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; private set; }
        public int Y { get; private set; }
    }
}