using System.Collections;
using PunchGear.Entity;
using UnityEngine;

namespace PunchGear.Enemy
{
    public class AttackPattern4 : IAttackPattern
    {
        private readonly EnemyPattern _enemyPattern;

        public AttackPattern4(EnemyPattern enemyPattern)
        {
            _enemyPattern = enemyPattern;
        }

        public IEnumerator GetPatternCoroutine()
        {
            ProjectileLauncher launcher = ProjectileLauncher.Instance;
            yield return _enemyPattern.JoinCoroutines(_enemyPattern.MoveOppositePosition(), new WaitForSecondsRealtime(_enemyPattern.fast));
            yield return _enemyPattern.JoinCoroutines(launcher.Launch(0), new WaitForSecondsRealtime(_enemyPattern.slow));
            yield return launcher.Launch(_enemyPattern.fast);
        }
    }
}
