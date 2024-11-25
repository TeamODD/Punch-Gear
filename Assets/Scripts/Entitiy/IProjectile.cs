namespace PunchGear.Entity
{
    public interface IProjectile : IColliderHolder
    {
        public ProjectileState State { get; }

        public bool Disassembled { get; }

        public bool Assembled { get; }

        public void Assemble();

        public void Disassemble();
    }
}
