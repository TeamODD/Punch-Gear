using UnityEngine;

namespace PunchGear.Entity
{
    [RequireComponent(typeof(PlayerMoveController))]
    [RequireComponent(typeof(PlayerAssembleController))]
    public class Player : MonoBehaviour, IHealthHolder, IPlaceableEntity
    {
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

        [field: SerializeField]
        public EntityPosition Position { get; set; }

        [field: SerializeField]
        public PlayerAssemblyCooldownIndicator DisassemblyCooldownIndicator { get; private set; }
        [field: SerializeField]
        public PlayerAssemblyCooldownIndicator AssemblyCooldownIndicator { get; private set; }

        private void Awake()
        {
            Position = EntityPosition.Bottom;
        }

        public event HealthChangeDelegate OnHealthChange;
    }
}
