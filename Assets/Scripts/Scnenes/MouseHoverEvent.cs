using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace PunchGear.Scenes
{
    public class MouseHoverEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private AudioClip _clip;

        [SerializeField]
        private Sprite[] _titleButton = new Sprite[2]; // 요놈 배경따로 글씨 따로 있어야할듯?

        private Image _image;

        private string _originText;

        private Dictionary<string, int> _buttonMapping;

        private void Start()
        {
            _image = GetComponent<Image>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            AudioManager.Instance.Play(_clip);
            _image.sprite = _titleButton[1];
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _image.sprite = _titleButton[0];
        }
    }
}
