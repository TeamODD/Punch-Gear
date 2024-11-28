using UnityEngine;

namespace PunchGear.Internal
{
    public class OptionButton : MonoBehaviour
    {
        public GameObject gameManager;

        private OpenOption _option;

        private void Awake()
        {
            _option = gameManager.GetComponent<OpenOption>();
        }

        private void OnMouseDown()
        {
            if (!_option.isPaused)
            {
                _option.OpenEvent();
            }
        }
    }
}
