namespace PunchGear.Entity
{
    public interface IKeyboardInputHandler : IInputHandler
    {
        public void AddAction(IKeyboardInputAction action);

        public void RemoveAction(IKeyboardInputAction action);
    }
}
