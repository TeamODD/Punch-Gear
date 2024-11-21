using UnityEngine;
using System.Collections;

namespace PunchGear.Enemy
{
    public class AttackPattern5 : IAttackPattern
    {
        private readonly EnemyPattern _enemyPattern;

        public AttackPattern5(EnemyPattern enemyPattern)
        {
            _enemyPattern = enemyPattern;
        }

        public IEnumerator GetPatternCoroutine()
        {
            for (int i = 0; i < 5; i++)
            {
                int randomInt = Random.Range(0, 4);

                switch (randomInt)
                {
                    case 0: yield return _enemyPattern.TransPos(); break;
                    case 1: yield return _enemyPattern.Launch(_enemyPattern.slow); break;
                    case 2: yield return _enemyPattern.Launch(_enemyPattern.normal); break;
                    case 3: yield return _enemyPattern.Launch(_enemyPattern.fast); break;
                }
            }
        }
    }
}
