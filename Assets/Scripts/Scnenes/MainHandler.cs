using System.Collections;
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
        [SerializeField]
        private AudioClip _mouseDownClip;
        [SerializeField]
        private AudioClip _gameStartClip;

        [SerializeField]
        private AudioClip _clip;

        private string _internalScene = "MainStage";

        private void Start()
        {
            AudioManager.Instance.PlayLoop(_clip);
        }

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
            AudioManager.Instance.Play(_gameStartClip);
            SceneManager.LoadScene(_internalScene);
        }

        public void TutorialButton()
        {
            AudioManager.Instance.Play(_mouseDownClip);
            _tutorial.SetActive(true);
        }

        public void OptionButton()
        {
            AudioManager.Instance.Play(_mouseDownClip);
            _option.SetActive(true);
        }

        public void QuitButton()
        {
            AudioManager.Instance.Play(_mouseDownClip);
            StartCoroutine(DelayAndQuit());
        }

        public void CloseButton()
        {
            AudioManager.Instance.Play(_mouseDownClip);
            _option.SetActive(false);
        }

        IEnumerator DelayAndQuit()
        {
            yield return new WaitForSeconds(1f);

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 실제 빌드된 게임에서 실행 시 게임을 종료
#endif
        }
    }
}
