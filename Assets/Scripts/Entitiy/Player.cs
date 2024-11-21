using System;
using UnityEngine;

namespace PunchGear.Entity
{
    public class Player : MonoBehaviour
    {
        [field: SerializeField]
        public Weapon Weapon { get; private set; }

        private void Awake()
        {
            if (Weapon == null)
            {
                throw new NullReferenceException("Weapon is not attached.");
            }
        }
    }
}
