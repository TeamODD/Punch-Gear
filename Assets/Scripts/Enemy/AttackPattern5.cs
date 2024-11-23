using UnityEngine;
using System.Collections;
using PunchGear.Entity;
using System.Collections.Generic;
using System;

namespace PunchGear.Enemy
{
    public class AttackPattern5 : IAttackPattern
    {
        private readonly EnemyPattern _enemyPattern;
        private List<Func<IEnumerator>> _actions;

        public AttackPattern5(EnemyPattern enemyPattern)
        {
            _enemyPattern = enemyPattern;
            _actions = new List<Func<IEnumerator>>
            {
                () => _enemyPattern.JoinCoroutines(_enemyPattern.MoveOppositePosition(), new WaitForSecondsRealtime(_enemyPattern.fast)),
                () =>
                {
                    ProjectileLauncher launcher = ProjectileLauncher.Instance;
                    return launcher.Launch(enemyPattern.slow);
                },
                () =>
                {
                    ProjectileLauncher launcher = ProjectileLauncher.Instance;
                    return launcher.Launch(enemyPattern.normal);
                },
                () =>
                {
                    ProjectileLauncher launcher = ProjectileLauncher.Instance;
                    return launcher.Launch(_enemyPattern.fast);
                }
            };
        }

        public IEnumerator GetPatternCoroutine()
        {
            ProjectileLauncher launcher = ProjectileLauncher.Instance;
            for (int i = 0; i < 5; i++)
            {
                int index = UnityEngine.Random.Range(0, _actions.Count);
                Func<IEnumerator> routine = _actions[index];
                yield return routine();
            }
        }
    }
}
