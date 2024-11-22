using UnityEngine;

namespace PunchGear
{
    public class AssemblyPoint : MonoBehaviour
    {
        [field: SerializeField]
        public EntityPosition Position { get; private set; }

        private void Start()
        {
            EntityPositionHandler.Instance.SetPosition(this, Position);
        }
    }
}
