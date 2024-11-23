using UnityEngine;

namespace PunchGear.Internal
{
    public class OptionButton : MonoBehaviour
    {
        public GameObject gameManager;

        private void OnMouseDown()
        {

            if (!gameManager.GetComponent<OpenOption>().isPaused)
            {
                gameManager.GetComponent<OpenOption>().OpenEvent();
            }
        }
    }
}
