using UnityEngine;

namespace PunchGear.Internal
{
    public class OptionButton : MonoBehaviour
    {
        public GameObject gameManager;

        private void OnMouseDown()
        {
            gameManager.GetComponent<OpenOption>().OpenEvent();
        }
    }
}
