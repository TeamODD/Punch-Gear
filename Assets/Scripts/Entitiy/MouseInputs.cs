using System;

namespace PunchGear.Entity
{
    [Flags]
    public enum MouseInputs
    {
        None = 0,
        Left = 1,
        Right = 2,
        Scroll = 4
    }
}
