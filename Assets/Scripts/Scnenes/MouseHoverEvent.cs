using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PunchGear.Scenes
{
    public class MouseHoverEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private TextMeshProUGUI _textMeshProUGUI;
        private string _originText;

        private void Start()
        {
            _textMeshProUGUI = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            _originText = _textMeshProUGUI.text;
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            _textMeshProUGUI.text = $"< {_originText} >";
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _textMeshProUGUI.text = _originText;
        }
    }
}
