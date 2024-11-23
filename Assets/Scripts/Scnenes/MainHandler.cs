using UnityEngine;
using UnityEngine.SceneManagement;

namespace PunchGear.Scenes
{
    public class MainHandler : MonoBehaviour
    {
        private string internalScene = "Internal";

        void Update()
        {
            if (Input.anyKeyDown)
            {
                SceneManager.LoadScene(internalScene);
            }

        }
    }
}
