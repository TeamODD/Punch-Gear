using UnityEngine;
using System.Collections;
using PunchGear.Entity;

namespace PunchGear.Enemy
{
    public class AttackPattern5 : IAttackPattern
    {
        private readonly EnemyPattern _enemyPattern;
        private readonly float _projectileSlowSpeed;
        private readonly float _projectileNormalSpeed;
        private readonly float _projectileFastSpeed;

        public AttackPattern5(
            EnemyPattern enemyPattern,
            float projectileSlowSpeed,
            float projectileNormalSpeed,
            float projectileFastSpeed)
        {
            _enemyPattern = enemyPattern;
            _projectileSlowSpeed = projectileSlowSpeed;
            _projectileNormalSpeed = projectileNormalSpeed;
            _projectileFastSpeed = projectileFastSpeed;
        }

        public IEnumerator GetPatternCoroutine()
        {
            ProjectileLauncher launcher = ProjectileLauncher.Instance;
            for (int i = 0; i < 5; i++)
            {
                int randomInt = Random.Range(0, 4);

                switch (randomInt)
                {
                    case 0: yield return _enemyPattern.MoveOppositePosition(); break;
                    case 1: yield return launcher.Launch(_projectileSlowSpeed); break;
                    case 2: yield return launcher.Launch(_projectileNormalSpeed); break;
                    case 3: yield return launcher.Launch(_projectileFastSpeed); break;
                }
            }
        }
    }
}
