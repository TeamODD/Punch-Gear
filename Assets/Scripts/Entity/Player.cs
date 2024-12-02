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

        [SerializeField]
        private Collider2D collider;

        [field: SerializeField]
        public PlayerAssemblyCooldownIndicator DisassemblyCooldownIndicator { get; private set; }

        [field: SerializeField]
        public PlayerAssemblyCooldownIndicator AssemblyCooldownIndicator { get; private set; }

        private void Awake()
        {
            Position = EntityPosition.Bottom;
            OnHealthChange += HandleExplosion;
        }

        private void Start()
        {
            ProjectileLauncher.Instance.OnProjectileCreated.AddListener(
                projectile => projectile.OnCollisionEnter += HandleHealth);
            ProjectileLauncher.Instance.OnProjectileDestroyed.AddListener(
                projectile => projectile.OnCollisionEnter -= HandleHealth);
        }

        private void OnDisable()
        {
            OnHealthChange -= HandleExplosion;
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

        public event HealthChangeDelegate OnHealthChange;

        [field: SerializeField]
        public EntityPosition Position { get; set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Initialize()
        {
            ExplosionAudioClip = Resources.Load<AudioClip>("Sound/explosion");
        }

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

        private void HandleHealth(GameObject origin, Collider2D collider)
        {
            if (collider.gameObject != this.collider.gameObject)
            {
                return;
            }
            Projectile projectileImpl = origin.GetComponent<Projectile>();
            if (projectileImpl.State != ProjectileState.Launched)
            {
                return;
            }
            projectileImpl.ProjectilePool.Release(projectileImpl);
            Health -= 1;
        }
    }
}
