using UnityEngine;

namespace PunchGear
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
