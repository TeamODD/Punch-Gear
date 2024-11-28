using System;

using UnityEngine;

namespace PunchGear.Entity
{
    [Serializable]
    public class NobilityAnimationSpriteProfile
    {
        [field: SerializeField]
        public Sprite DefaultSprite { get; private set; }

        [field: SerializeField]
        public Sprite AttackSprite { get; private set; }
    }
}
