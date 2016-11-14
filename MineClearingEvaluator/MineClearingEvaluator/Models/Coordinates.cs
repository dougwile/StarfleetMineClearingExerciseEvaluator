namespace MineClearingEvaluator.Models
{
    public class Coordinates
    {
        public Coordinates(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public int X { get; protected set; }
        public int Y { get; protected set; }
        public int Z { get; protected set; }
    }
}