using System;
using System.Collections.Generic;

using UnityEngine;

namespace PunchGear.Entity
{
    public class GlobalEntityCollisionHandler : MonoBehaviour
    {
        private readonly Dictionary<(Type, Type), object> _collisionHandlers = new Dictionary<(Type, Type), object>();

        private void OnDisable()
        {
            _collisionHandlers.Clear();
        }
    }
}
