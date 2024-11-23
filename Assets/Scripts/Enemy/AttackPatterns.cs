using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PunchGear.Enemy
{
    public static class AttackPatterns
    {
        public static IEnumerator JoinCoroutines(this MonoBehaviour monoBehaviour, params IEnumerator[] routines)
        {
            IEnumerable<Coroutine> coroutines = routines.Select(routine => monoBehaviour.StartCoroutine(routine));
            foreach (Coroutine coroutine in coroutines)
            {
                yield return coroutine;
            }
        }
    }
}
