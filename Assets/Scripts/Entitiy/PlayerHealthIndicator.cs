using System;
using System.Collections;
using PunchGear.Entity;
using UnityEngine;

namespace PunchGear
{
    public class PlayerHealthIndicator : MonoBehaviour
    {
        [SerializeField]
        private SpriteRenderer[] _healthIndicatorItems;
        [SerializeField]
        private SpriteRenderer _indicatorHolderSprite;
        [SerializeField]
        private PlayerHealthIndicatorHolderSpriteProfile _profile;

        [Range(0.1f, 1f)]
        [SerializeField]
        private float _blinkRate;

        [field: SerializeField]
        public Player Player { get; private set; }

        private void Awake()
        {
            if (Player == null)
            {
                throw new NullReferenceException("Player is not attached");
            }

            Player.OnHealthChange += UpdateIndicator;
        }

        private void Start()
        {
            _indicatorHolderSprite.sprite = _profile.DefaultSprite;
            Initialize();
        }

        private void OnDisable()
        {
            Player.OnHealthChange -= UpdateIndicator;
        }

        private void Initialize()
        {
            int health = Player.Health;
            if (health > _healthIndicatorItems.Length)
            {
                throw new IndexOutOfRangeException("Health value is out of range: " + health);
            }
            for (int i = 0; i < _healthIndicatorItems.Length; i++)
            {
                SpriteRenderer renderer = _healthIndicatorItems[i];
                Material material = renderer.material;
                Color color = material.color;
                bool indicates = i < health;
                color.a = indicates ? 1 : 0;
                material.color = color;
            }
        }

        private void UpdateIndicator(int previousHealth, int currentHealth)
        {
            if (currentHealth >= previousHealth || currentHealth < 0)
            {
                return;
            }
            Debug.Log("Update indicator");
            for (int i = currentHealth; i < previousHealth; i++)
            {
                SpriteRenderer renderer = _healthIndicatorItems[i];
                this.StartBlink(renderer, 2, _blinkRate);
            }
            if (currentHealth == 1)
            {
                _indicatorHolderSprite.sprite = _profile.BrokenSprite;
            }
        }
    }

    [Serializable]
    public class PlayerHealthIndicatorHolderSpriteProfile
    {
        [field: SerializeField]
        public Sprite DefaultSprite { get; private set; }

        [field: SerializeField]
        public Sprite BrokenSprite { get; private set; }
    }
}
