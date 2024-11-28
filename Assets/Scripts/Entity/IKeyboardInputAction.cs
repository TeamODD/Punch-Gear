using System.Collections.Generic;
using UnityEngine;

namespace PunchGear.Entity
{
    public interface IKeyboardInputAction
    {
        public void OnKeyDown(IList<KeyCode> keyCodes);
    }
}
