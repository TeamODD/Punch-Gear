using System.Collections;

namespace PunchGear.Enemy
{
    public class AttackPattern3 : IAttackPattern
    {
        private readonly EnemyPattern _enemyPattern;

        public AttackPattern3(EnemyPattern enemyPattern)
        {
            _enemyPattern = enemyPattern;
        }

        public IEnumerator GetPatternCoroutine()
        {
            yield return _enemyPattern.StartCoroutine(_enemyPattern.Launch(_enemyPattern.normal));
            yield return _enemyPattern.StartCoroutine(_enemyPattern.Launch(_enemyPattern.fast));
        }
    }
}
