using UnityEngine;
using UnityEngine.UI;

namespace PunchGear.Internal
{
    public class SliderEventBGM : MonoBehaviour
    {
        private Slider volumeSlider;
        private AudioManager audioManager;

        private void Start()
        {
            audioManager = FindFirstObjectByType<AudioManager>();
            volumeSlider = GetComponent<Slider>();

            // 오디오 마네자 알아서 만드셈
            volumeSlider.value = AudioManager.Instance.Volume;
            volumeSlider.onValueChanged.AddListener(value => { AudioManager.Instance.Volume = value; });
        }
    }
}
