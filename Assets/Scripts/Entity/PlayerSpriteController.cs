using UnityEngine;

namespace PunchGear.Entity
{
    public class PlayerSpriteController : MonoBehaviour
    {
        private SpriteRenderer _renderer;

        [Range(0.1f, 1f)]
        [SerializeField]
        private float blinkRate;

        [field: SerializeField]
        public Player Player { get; private set; }

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            if (Player == null)
            {
                throw new UnassignedReferenceException("Player is not attached");
            }
            Player.OnHealthChange += UpdateIndicator;
        }

        private void OnDisable()
        {
            Player.OnHealthChange -= UpdateIndicator;
        }

        private void UpdateIndicator(int previousHealth, int currentHealth)
        {
            if (currentHealth >= previousHealth || currentHealth < 0)
            {
                return;
            }
            for (int i = currentHealth; i < previousHealth; i++)
            {
                this.StartBlink(_renderer, 2, blinkRate, true);
            }
        }
    }
}
