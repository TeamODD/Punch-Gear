using System.Collections.Generic;

using UnityEngine;

namespace PunchGear.Enemy
{
    [CreateAssetMenu(
        fileName = "Nobility Attack Pattern Profile",
        menuName = "Scriptable Objects/Create Nobility Attack Pattern Profile")]
    public class NobilityAttackPatternProfile : ScriptableObject
    {
        [field: SerializeField]
        public List<float> LaunchSpeeds { get; private set; }

        [field: SerializeField]
        public float PatternDelay { get; private set; }
    }
}
