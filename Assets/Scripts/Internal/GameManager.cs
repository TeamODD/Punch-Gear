using UnityEngine;

namespace PunchGear.Internal
{
    public class GameManager : MonoBehaviour
    {
        // 집행유예
        public int playerHealth = 3; // 플레이어 체력
        public int playerHealthMax = 3; // 플레이어 최대 체력

        public int enemyHealth = 8; // 적 체력
        public int enemyHealthMax = 10; // 적 최대 체력
    }
}
