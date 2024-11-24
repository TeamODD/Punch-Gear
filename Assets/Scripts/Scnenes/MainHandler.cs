using UnityEngine;
using UnityEngine.SceneManagement;

namespace PunchGear.Scenes
{
    public class MainHandler : MonoBehaviour
    {
        [SerializeField]
        private GameObject _tutorial;
        [SerializeField]
        private GameObject _option;

        private string _internalScene = "Internal";
        

        private void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                _tutorial.SetActive(false);
                _option.SetActive(false);
            }
        }

        public void StartButton()
        {
            SceneManager.LoadScene(_internalScene);
        }

        public void TutorialButton()
        {
            _tutorial.SetActive(true);
        }

        public void OptionButton()
        {

        }

        public void QuitButton()
        {
            // 대충 복붙
        }
    }
}
