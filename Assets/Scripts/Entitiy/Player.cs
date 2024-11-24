using System.Collections;
using UnityEngine;

namespace PunchGear.Entity
{
    [RequireComponent(typeof(PlayerMoveController))]
    [RequireComponent(typeof(PlayerAssembleController))]
    public class Player : MonoBehaviour, IHealthHolder, IPlaceableEntity
    {
        private static AudioClip ExplosionAudioClip;

        [SerializeField]
        private int _healthPoint;

        [SerializeField]
        private GameObject _explosionPrefab;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Initialize()
        {
            ExplosionAudioClip = Resources.Load<AudioClip>("Sound/explosion");
        }

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
            OnHealthChange += HandleExplosion;
        }

        private void OnDisable()
        {
            OnHealthChange -= HandleExplosion;
        }

        public event HealthChangeDelegate OnHealthChange;

        private void HandleExplosion(int previousHealth, int currentHealth)
        {
            if (previousHealth <= currentHealth)
            {
                return;
            }
            _explosionPrefab.SetActive(true);
            AudioManager.Instance.Play(ExplosionAudioClip);
            StartCoroutine(InactiveAfter(0.8f));
        }

        private IEnumerator InactiveAfter(float seconds)
        {
            yield return new WaitForSecondsRealtime(seconds);
            _explosionPrefab.SetActive(false);
        }
    }
}
