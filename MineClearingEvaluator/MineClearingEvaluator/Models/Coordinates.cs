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

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
    }
}