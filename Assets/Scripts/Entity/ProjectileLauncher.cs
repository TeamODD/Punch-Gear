using System;
using System.Collections;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

namespace PunchGear.Entity
{
    public class ProjectileLauncher : MonoBehaviour, IProjectileLauncher
    {
        public static ProjectileLauncher Instance { get; private set; }

        [field: SerializeField]
        private GameObject _bulletLauncherOrigin;

        [field: SerializeField]
        public GameObject BulletPrefab { get; private set; }

        [field: SerializeField]
        public UnityEvent<IProjectile> OnProjectileCreated { get; private set; }

        [field: SerializeField]
        public UnityEvent<IProjectile> OnProjectileDestroyed { get; private set; }

        public GameObject BulletLauncherOrigin
        {
            get
            {
                return _bulletLauncherOrigin;
            }
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
        private IObjectPool<IProjectile> _projectilePool;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                throw new InvalidOperationException("Bullet launcher component already exists");
            }
            if (BulletPrefab == null)
            {
                throw new UnassignedReferenceException("Bullet prefab is not attached");
            }
            Instance = this;
            _projectilePool = new LinkedPool<IProjectile>(
                CreateProjectile,
                OnProjectileGet,
                OnProjectileRelease,
                OnProjectileDestroy,
                true,
                100);
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
            _projectilePool.Get();
            yield return new WaitForSeconds(launcherCooldown); // 시간 지연
        }

        private IProjectile CreateProjectile()
        {
            GameObject bulletObject = Instantiate(
                BulletPrefab,
                BulletLauncherOrigin.transform.position,
                Quaternion.identity);
            bulletObject.transform.SetParent(transform, true);
            bulletObject.name = "Projectile";
            Projectile projectileImpl = bulletObject.GetComponent<Projectile>();
            projectileImpl.ProjectilePool = _projectilePool;
            return projectileImpl;
        }

        private void OnProjectileGet(IProjectile projectile)
        {
            Projectile projectileImpl = (Projectile) projectile;
            projectileImpl.Position = _placeableEntity.Position;
            projectileImpl.EnemyOrigin = BulletLauncherOrigin;
            projectileImpl.gameObject.SetActive(true);
            OnProjectileCreated.Invoke(projectileImpl);
        }

        private void OnProjectileRelease(IProjectile projectile)
        {
            Projectile projectileImpl = (Projectile) projectile;
            projectileImpl.gameObject.SetActive(false);
            OnProjectileDestroyed.Invoke(projectileImpl);
        }

        private void OnProjectileDestroy(IProjectile projectile)
        {
            Projectile projectileImpl = (Projectile) projectile;
            Destroy(projectileImpl.gameObject);
        }
    }
}
