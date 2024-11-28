using System;

using UnityEngine;

using TMPro;

namespace PunchGear.Scenes
{
    [Obsolete]
    public class RewardHandler : MonoBehaviour
    {
        public TextMeshProUGUI textMeshProText;

        private int _minutes = 7;
        private int _seconds = 12;
        private int _milliseconds = 74;

        private void Start()
        {
            textMeshProText.text = $"<Size=25>{_minutes:00}:{_seconds:00}</Size><Size=15>.{_milliseconds:00}</Size>";
        }
    }
}
