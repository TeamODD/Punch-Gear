using PunchGear.Entity;
using UnityEngine;

namespace PunchGear.Enemy
{
    public class EnemyObject : MonoBehaviour, IHealthHolder
    {
        [SerializeField]
        private int _healthPoint;

        public event HealthChangeDelegate OnHealthChange;

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
    }
}
