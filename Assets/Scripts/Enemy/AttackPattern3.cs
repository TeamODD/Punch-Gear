using System.Collections;

namespace PunchGear.Enemy
{
    public class AttackPattern3 : IAttackPattern
    {
        private readonly EnemyPattern _enemyPattern;
        private readonly float _projectileSlowSpeed;
        private readonly float _projectileFastSpeed;

        public AttackPattern3(EnemyPattern enemyPattern, float projectileSlowSpeed, float projectileFastSpeed)
        {
            _enemyPattern = enemyPattern;
            _projectileSlowSpeed = projectileSlowSpeed;
            _projectileFastSpeed = projectileFastSpeed;
        }

        public IEnumerator GetPatternCoroutine()
        {
            yield return _enemyPattern.StartCoroutine(_enemyPattern.Launch(_projectileSlowSpeed));
            yield return _enemyPattern.StartCoroutine(_enemyPattern.Launch(_projectileFastSpeed));
        }
    }
}
