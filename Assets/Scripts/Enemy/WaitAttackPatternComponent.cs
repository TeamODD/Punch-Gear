using System.Collections;

using UnityEngine;

namespace PunchGear.Enemy
{
    public class WaitAttackPatternComponent : IAttackPatternComponent
    {
        private readonly float _seconds;

        public WaitAttackPatternComponent(float seconds)
        {
            _seconds = seconds;
        }

        public IEnumerator GetPatternComponentCoroutine()
        {
            yield return new WaitForSecondsRealtime(_seconds);
        }
    }
}
