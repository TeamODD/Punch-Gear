using System.Collections;

namespace PunchGear.Enemy
{
    public class AttackPattern4 : IAttackPattern
    {
        private readonly EnemyPattern _enemyPattern;
        private readonly float _projectileSlowSpeed;
        private readonly float _projectileFastSpeed;

        public AttackPattern4(EnemyPattern enemyPattern, float projectileSlowSpeed, float projectileFastSpeed)
        {
            _enemyPattern = enemyPattern;
            _projectileSlowSpeed = projectileSlowSpeed;
            _projectileFastSpeed = projectileFastSpeed;
        }

        public IEnumerator GetPatternCoroutine()
        {
            yield return _enemyPattern.StartCoroutine(_enemyPattern.MoveOppositePosition());
            yield return _enemyPattern.StartCoroutine(_enemyPattern.Launch(_projectileFastSpeed));
            yield return _enemyPattern.StartCoroutine(_enemyPattern.Launch(_projectileSlowSpeed));
        }
    }
}
