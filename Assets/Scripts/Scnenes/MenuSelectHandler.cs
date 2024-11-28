using UnityEngine;

using TMPro;

namespace PunchGear.Scenes
{
    public class MenuSelectHandler : MonoBehaviour
    {
        public TextMeshProUGUI textMeshProText;
        private string _originText;

        private void Start()
        {
            textMeshProText = GetComponent<TextMeshProUGUI>();
            _originText = textMeshProText.text;
        }

        private void OnMouseOver()
        {
            Debug.Log(1);
            textMeshProText.text = $"< {_originText} >";
        }

        private void OnMouseExit()
        {
            textMeshProText.text = _originText;
        }
    }
}
