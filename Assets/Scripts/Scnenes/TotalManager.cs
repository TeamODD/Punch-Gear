using Unity.Properties;
using UnityEngine;

namespace PunchGear.Scenes
{
    public class TotalManager : MonoBehaviour
    {
        public Sprite[] resultBackground = new Sprite[2]; // 결과창 이미지
        
        public int playerHealth; // 플레이어 체력
        public int playerHealthMax = 3; // 플레이어 최대 체력

        public int enemyHealth = 8; // 적 체력
        public int enemyHealthMax = 10; // 적 최대 체력
        
        public string internalScene; // 인게임 씬

        private void Awake() // 배경화면으로 돌아올 일 없음 수구
        {
            DontDestroyOnLoad(this);
        }
    }
}
