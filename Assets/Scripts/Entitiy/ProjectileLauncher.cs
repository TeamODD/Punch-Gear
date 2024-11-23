using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace PunchGear.Entity
{
    public class ProjectileLauncher : MonoBehaviour
    {
        public static ProjectileLauncher Instance { get; private set; }

        [field: SerializeField]
        private GameObject _bulletLauncherOrigin;

        [field: SerializeField]
        public GameObject BulletPrefab { get; private set; }

        [field: SerializeField]
        public UnityEvent<Projectile> OnProjectileCreated { get; private set; }

        [field: SerializeField]
        public UnityEvent<Projectile> OnProjectileDestroyed { get; private set; }

        public GameObject BulletLauncherOrigin
        {
            get => _bulletLauncherOrigin;
            set
            {
                _bulletLauncherOrigin = value;
                _placeableEntity = _bulletLauncherOrigin.GetComponent<IPlaceableEntity>();
                if (_placeableEntity == null)
                {
                    throw new NullReferenceException("This object does not contain IPlaceableEntity component");
                }
            }
        }

        private IPlaceableEntity _placeableEntity;
        private Player _player;

        private void Awake()
        {
            if (Instance)
            {
                Destroy(this);
                throw new InvalidOperationException("Bullet launcher component already exists");
            }
            if (BulletPrefab == null)
            {
                throw new NullReferenceException("Bullet prefab is not attached");
            }
            Instance = this;
        }

        private void Start()
        {
            _player = FindFirstObjectByType<Player>();
            if (_player == null)
            {
                throw new NullReferenceException("Cannot find any player component");
            }
            Debug.Log("Player detected");
        }

        public IEnumerator Launch(float launcherCooldown)
        {
            if (BulletLauncherOrigin == null)
            {
                throw new NullReferenceException("Bullet launcher origin is not set");
            }
            GameObject bulletObject = Instantiate(BulletPrefab, BulletLauncherOrigin.transform.position, Quaternion.identity);
            bulletObject.name = "Bullet";
            Projectile projectile = bulletObject.GetComponent<Projectile>();
            projectile.Position = _placeableEntity.Position;
            projectile.EnemyOrigin = BulletLauncherOrigin;
            projectile.Player = _player;
            yield return null;
            OnProjectileCreated.Invoke(projectile);
            yield return new WaitForSeconds(launcherCooldown); // 시간 지연
        }
    }
}
