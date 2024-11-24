namespace PunchGear.Entity
{
    public interface IProjectile
    {
        public bool Disassembled { get; }

        public bool Assembled { get; }

        public bool Assemble();

        public bool Disassemble();
    }
}
