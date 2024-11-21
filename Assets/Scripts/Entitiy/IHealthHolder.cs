namespace PunchGear.Entity
{
    public delegate void HealthChangeDelegate(int previousHealthPoint, int currentHealthPoint);


    public interface IHealthHolder : IEntity
    {
        public int Health { get; set; }

        public event HealthChangeDelegate OnHealthChange;
    }
}
