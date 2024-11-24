using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PunchGear.Scenes
{
    public class MouseHoverEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private AudioClip clip;
        private AudioSource _audioSource;

        private TextMeshProUGUI _textMeshProUGUI;
        private string _originText;

        private void Start()
        {
            _textMeshProUGUI = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            _originText = _textMeshProUGUI.text;

            _audioSource = GetComponent<AudioSource>();
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            _textMeshProUGUI.text = $"< {_originText} >";
            _audioSource.clip = clip;
            _audioSource.volume = 0.5f;
            _audioSource.Play();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _textMeshProUGUI.text = _originText;
        }
    }
}
