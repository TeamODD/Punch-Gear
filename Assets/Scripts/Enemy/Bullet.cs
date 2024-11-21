using UnityEngine;

namespace PunchGear.Enemy
{
    public class Bullet : MonoBehaviour
    {
        private Rigidbody2D rb;
        public float power = 1000f;
        public float removeTime = 3f;
        void Start()
        {
            rb = this.GetComponent<Rigidbody2D>();
            rb.AddForce(Vector2.left * power); //아닐 수도 있음
            // 대충 등속도로 발사되기
            Destroy(this.gameObject, removeTime);
        }
    }
}
