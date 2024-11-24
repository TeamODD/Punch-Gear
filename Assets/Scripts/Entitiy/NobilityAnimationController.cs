using System;
using System.Collections;
using UnityEngine;

namespace PunchGear.Entity
{
    public class NobilityAnimationController : MonoBehaviour
    {
        [SerializeField]
        private NobilityAnimationSpriteProfile _profile;

        [SerializeField]
        private SpriteRenderer _renderer;

        [SerializeField]
        private float _animationDuration;

        private void Awake()
        {
            _renderer.sprite = _profile.DefaultSprite;
        }

        public IEnumerator TransitAnimationRoutine()
        {
            _renderer.sprite = _profile.AttackSprite;
            yield return new WaitForSecondsRealtime(_animationDuration);
            _renderer.sprite = _profile.DefaultSprite;
        }
    }

    [Serializable]
    public class NobilityAnimationSpriteProfile
    {
        [field: SerializeField]
        public Sprite DefaultSprite { get; private set; }

        [field: SerializeField]
        public Sprite AttackSprite { get; private set; }
    }
}
