using System.Collections;

using PunchGear.Utility;

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

        private Nobility _nobility;

        private void Awake()
        {
            _renderer.sprite = _profile.DefaultSprite;
            _nobility = GetComponent<Nobility>();
        }

        private void OnEnable()
        {
            _nobility.OnHealthChange += BlinkSpriteHandler;
        }

        private void OnDisable()
        {
            _nobility.OnHealthChange -= BlinkSpriteHandler;
        }

        public IEnumerator TransitAnimationRoutine()
        {
            _renderer.sprite = _profile.AttackSprite;
            yield return new WaitForSecondsRealtime(_animationDuration);
            _renderer.sprite = _profile.DefaultSprite;
        }

        private void BlinkSpriteHandler(int previous, int current)
        {
            if (previous < current || current < 0)
            {
                return;
            }
            this.StartBlink(_renderer, 3, 0.1f, true);
        }
    }
}
