using UnityEngine;

namespace PunchGear
{
    public class TotalManager : MonoBehaviour
    {
        public Sprite[] resultBackground = new Sprite[2]; // 결과창 이미지
        public int playerHealth; // 플레이어 체력
        public int enemyHealth; // 적 체력
        public string internalScene;
    }
}
