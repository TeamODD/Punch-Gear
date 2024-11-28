using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace PunchGear.Enemy
{
    public static class AttackPatterns
    {
        public static IEnumerator JoinCoroutines(
            this MonoBehaviour monoBehaviour,
            params IEnumerator[] routines)
        {
            IEnumerable<Coroutine> coroutines = routines.Select(monoBehaviour.StartCoroutine);
            foreach (Coroutine coroutine in coroutines)
            {
                yield return coroutine;
            }
        }
    }
}
