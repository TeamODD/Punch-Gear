using UnityEngine;

namespace PunchGear.Entity
{
    [DisallowMultipleComponent]
    public abstract class HealthHolderBase : MonoBehaviour, IHealthHolder
    {
        [SerializeField]
        protected int health;

        public int Health
        {
            get => health;
            set
            {
                int previous = health;
                health = value;
                OnHealthChange?.Invoke(previous, health);
            }
        }

        public event HealthChangeDelegate OnHealthChange;
    }
}
