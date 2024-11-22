using System;
using UnityEngine;

namespace PunchGear.Entity
{
    public class Player : MonoBehaviour, IHealthHolder
    {
        [field: SerializeField]
        public Weapon Weapon { get; private set; }

        [SerializeField]
        private int _healthPoint;

        public int Health
        {
            get => _healthPoint;
            set
            {
                int previous = _healthPoint;
                _healthPoint = value;
                OnHealthChange?.Invoke(previous, _healthPoint);
            }
        }


        private void Awake()
        {
            if (Weapon == null)
            {
                throw new NullReferenceException("Weapon is not attached.");
            }
        }

        public event HealthChangeDelegate OnHealthChange;
    }
}
