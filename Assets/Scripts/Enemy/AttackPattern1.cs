using System.Collections;

namespace PunchGear.Enemy
{
    public class AttackPattern1 : IAttackPattern
    {
        private readonly EnemyPattern _enemyPattern;

        public AttackPattern1(EnemyPattern enemyPattern)
        {
            _enemyPattern = enemyPattern;
        }

        public IEnumerator GetPatternCoroutine()
        {
            yield return _enemyPattern.StartCoroutine(_enemyPattern.Launch(_enemyPattern.normal));
            yield return _enemyPattern.StartCoroutine(_enemyPattern.TransPos());
            yield return _enemyPattern.StartCoroutine(_enemyPattern.Launch(_enemyPattern.normal));
            yield return _enemyPattern.StartCoroutine(_enemyPattern.TransPos());
            yield return _enemyPattern.StartCoroutine(_enemyPattern.Launch(_enemyPattern.normal));
        }
    }
}
