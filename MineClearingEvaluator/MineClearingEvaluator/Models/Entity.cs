namespace MineClearingEvaluator.Models
{
    /// <summary>
    /// An entity is an object in the cuboid of space with a coordinate.
    /// An entity is not an object by itself and must be subclassed to be instantiated.
    /// </summary>
    public abstract class Entity
    {
        protected Entity(int x, int y, int z)
        {
            Coordinates = new Coordinates(x, y, z);
        }

        public Coordinates Coordinates { get; private set; }
    }
}