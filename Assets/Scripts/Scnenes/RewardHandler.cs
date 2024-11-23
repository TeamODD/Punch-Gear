using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

namespace PunchGear.Scenes
{
    public class RewardHandler : MonoBehaviour
    {
        public TextMeshProUGUI textMeshProText;

        public int _minutes = 7, _seconds = 12, _milliseconds = 74;

        void Start()
        {
            textMeshProText.text = $"<Size=25>{_minutes:00}:{_seconds:00}</Size><Size=15>.{_milliseconds:00}</Size>";
        }
    }
}
