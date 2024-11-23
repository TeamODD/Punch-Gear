using UnityEngine;
using UnityEngine.SceneManagement;

namespace PunchGear.Scenes
{
    public class ResultHandler : MonoBehaviour
    {
        private TotalManager totalManager;
        public GameObject background;


        private void Start()
        {
            // �÷��̾� ü�°� ���� ü���� �����ͼ� ��� �̹��� �� ������� ����
            SpriteRenderer spriteRenderer = background.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = totalManager.resultBackground[totalManager.enemyHealth == 0 ? 1 : 0];
        }

        public void EndEvent() //���� ������ ���� 1ȣ�� �ڵ� �ܾ��
        {
            // �����Ϳ��� ���� ������ Ȯ�� (�����Ϳ����� ������� �ʱ� ������ �޽����� ���)
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit(); // ���� ����� ���ӿ��� ���� �� ������ ����
#endif
        }

        public void RetryEvent()
        {
            SceneManager.LoadScene(totalManager.internalScene);
        }
    }
}
