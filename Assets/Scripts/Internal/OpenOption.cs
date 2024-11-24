using UnityEngine;

namespace PunchGear.Internal
{
    public class OpenOption : MonoBehaviour
    {
        // 게임이 멈췄는지 확인하는 변수
        public bool isPaused;
        public GameObject Panel;

        private void Start()
        {
            isPaused = false;
            Time.timeScale = 1f;
        }

        private void Update()
        {
            // ESC 키를 눌렀을 때
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OpenEvent();
            }
        }

        public void OpenEvent()
        {
            Panel.SetActive(!isPaused);
            Time.timeScale = isPaused ? 1f : 0f; // 게임을 재개하거나 멈춤
            isPaused = !isPaused; // 일시 정지 상태를 토글
        }
    }
}
