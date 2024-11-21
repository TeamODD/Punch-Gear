using System.Collections;

namespace PunchGear.Enemy
{
    public class AttackPattern2 : IAttackPattern
    {
        private readonly EnemyPattern _enemyPattern;

        public AttackPattern2(EnemyPattern enemyPattern)
        {
            _enemyPattern = enemyPattern;
        }

        public IEnumerator GetPatternCoroutine()
        {
            yield return _enemyPattern.StartCoroutine(_enemyPattern.Launch(_enemyPattern.fast));
            yield return _enemyPattern.StartCoroutine(_enemyPattern.Launch(_enemyPattern.fast));
            yield return _enemyPattern.StartCoroutine(_enemyPattern.Launch(_enemyPattern.fast));
            yield return _enemyPattern.StartCoroutine(_enemyPattern.Launch(_enemyPattern.fast));
            yield return _enemyPattern.StartCoroutine(_enemyPattern.TransPos());
            yield return _enemyPattern.StartCoroutine(_enemyPattern.Launch(_enemyPattern.slow));
            yield return _enemyPattern.StartCoroutine(_enemyPattern.Launch(_enemyPattern.slow));
        }
    }
}
