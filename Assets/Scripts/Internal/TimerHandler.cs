using UnityEngine;

using TMPro;

namespace PunchGear
{
    public class TimerHandler : MonoBehaviour
    {
        public TextMeshProUGUI textMeshProText;

        [field: SerializeField]
        public float StartTime { get; set; }

        private bool _enabled;

        private void Awake()
        {
            _enabled = false;
        }

        private void FixedUpdate()
        {
            if (!_enabled)
            {
                return;
            }
            float currentTime = Time.time;
            float deltaTime = currentTime - StartTime;
            int seconds = (int) Mathf.Floor(deltaTime);
            int displayMinutes = seconds / 60;
            int displaySeconds = seconds % 60;
            int underSeconds = (int) ((deltaTime - seconds) * 100f);
            textMeshProText.text =
                $"<Size=25>{displayMinutes:00}:{displaySeconds:00}</Size><Size=15>.{underSeconds:00}</Size>";
        }

        public void Enable()
        {
            _enabled = true;
        }

        public void Disable()
        {
            _enabled = false;
        }
    }
}
