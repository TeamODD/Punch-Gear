using UnityEngine;
using UnityEngine.SceneManagement;

namespace PunchGear.Scenes
{
    public class ResultHandler : MonoBehaviour
    {
        public string internalScene;

        public void EndEvent() //뭔진 모르지만 꿀잼 1호기 코드 긁어옴
        {
            // 에디터에서 실행 중인지 확인 (에디터에서는 종료되지 않기 때문에 메시지를 띄움)
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // 실제 빌드된 게임에서 실행 시 게임을 종료
#endif
        }

        public void RetryEvent()
        {
            SceneManager.LoadScene(internalScene);
        }
    }
}
