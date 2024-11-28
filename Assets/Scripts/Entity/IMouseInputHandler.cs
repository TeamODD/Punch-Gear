namespace PunchGear.Entity
{
    public interface IMouseInputHandler : IInputHandler
    {
        public void AddAction(IMouseInputAction action);

        public void RemoveAction(IMouseInputAction action);
    }
}
