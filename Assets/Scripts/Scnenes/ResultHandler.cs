﻿using UnityEngine;
using UnityEngine.SceneManagement;

namespace PunchGear.Scenes
{
    public class ResultHandler : MonoBehaviour
    {
        private TotalManager totalManager;
        public GameObject background;


        private void Start()
        {
            // 플레이어 체력과 보스 체력을 가져와서 배경 이미지 뭐 출력할지 결정
            SpriteRenderer spriteRenderer = background.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = totalManager.resultBackground[totalManager.enemyHealth == 0 ? 1 : 0];
        }

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
            SceneManager.LoadScene(totalManager.internalScene);
        }
    }
}
