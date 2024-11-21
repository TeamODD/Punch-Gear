using UnityEngine;

namespace PunchGear
{
    public class Bullet : MonoBehaviour
    {
        private Rigidbody2D rb;
        public float power = 1000f;
        public float removeTime = 3f;
        void Start()
        {
            rb = this.GetComponent<Rigidbody2D>();
            rb.AddForce(Vector2.left * power); //�ƴ� ���� ����
            // ���� ��ӵ��� �߻�Ǳ�
            Destroy(this.gameObject, removeTime);
        }
    }
}
