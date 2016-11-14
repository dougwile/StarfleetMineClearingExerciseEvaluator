namespace MineClearingEvaluator.Models
{
    public class Entity
    {
        public Entity(int x, int y, int z)
        {
            Coordinates = new Coordinates(x, y, z);
        }

        public Coordinates Coordinates { get; private set; }
    }
}