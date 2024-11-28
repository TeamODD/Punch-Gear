using System;

using UnityEngine;

namespace PunchGear
{
    [Serializable]
    public class EntityPositionProfile
    {
        [field: SerializeField]
        public EntityPosition Position { get; private set; }

        [field: SerializeField]
        public float Height { get; private set; }

        public Vector2 Vector
        {
            get
            {
                return new Vector2(0, Height);
            }
        }
    }
}
