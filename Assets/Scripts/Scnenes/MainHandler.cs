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
            _option.SetActive(true);
        }

        public void QuitButton()
        {
            // 에디터에서 실행 중인지 확인 (에디터에서는 종료되지 않기 때문에 메시지를 띄움)
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 실제 빌드된 게임에서 실행 시 게임을 종료
#endif
        }
    }
}
