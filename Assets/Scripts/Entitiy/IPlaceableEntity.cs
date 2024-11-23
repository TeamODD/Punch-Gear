namespace PunchGear.Entity
{
    public interface IPlaceableEntity : IEntity
    {
        public EntityPosition Position { get; }
    }
}
