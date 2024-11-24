using UnityEngine;
using System.Collections;
using TMPro;

namespace PunchGear
{
    public class TimerHandler : MonoBehaviour
    {
        public TextMeshProUGUI textMeshProText;

        private int _minutes = 0;
        private int _seconds = 0;
        private int _milliseconds = 0;

        void Start()
        {
            StartCoroutine(TimerCoroutine());
        }

        private IEnumerator TimerCoroutine()
        {
            while (true)
            {
                yield return new WaitForSecondsRealtime(0.01f); // 0.01초 대기

                _milliseconds++;
                if (_milliseconds == 100)
                {
                    _milliseconds = 0;
                    _seconds++;

                    if (_seconds == 60)
                    {
                        _seconds = 0;
                        _minutes++;
                    }
                }

                textMeshProText.text = $"<Size=25>{_minutes:00}:{_seconds:00}</Size><Size=15>.{_milliseconds:00}</Size>";
            }
        }
    }
}
