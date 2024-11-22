using System;
using UnityEngine;

namespace PunchGear
{
    public class EntityPositionHandler : MonoBehaviour
    {
        private static EntityPositionHandler _instance;

        [field: SerializeField]
        public EntityPositionProfile TopPositionProfile { get; private set; }

        [field: SerializeField]
        public EntityPositionProfile BottomPositionProfile { get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            if (_instance)
            {
                return;
            }
            _instance = FindFirstObjectByType<EntityPositionHandler>();
            if (!_instance)
            {
                throw new NullReferenceException("Cannot find any Entity position handler");
            }
            DontDestroyOnLoad(_instance);
        }

        public static EntityPositionHandler Instance => _instance;

        private void Awake()
        {
            if (TopPositionProfile == null)
            {
                throw new NullReferenceException("Top position profile is not provided");
            }
            if (TopPositionProfile.Position != EntityPosition.Top)
            {
                throw new InvalidOperationException("Top position profile's position must be top");
            }
            if (BottomPositionProfile == null)
            {
                throw new NullReferenceException("Bottom position profile is not provided");
            }
            if (BottomPositionProfile.Position != EntityPosition.Bottom)
            {
                throw new InvalidOperationException("Bottom position profile's position must be bottom");
            }
        }

        public void SetPosition(MonoBehaviour monoBehaviour, EntityPosition position)
        {
            SetPosition(monoBehaviour.transform, position);
        }

        public void SetPosition(Transform transform, EntityPosition position)
        {
            EntityPositionProfile profile = GetProfileByPosition(position);
            Vector2 currentPosition = transform.position;
            currentPosition.y = profile.Height;
            transform.position = currentPosition;
        }

        public void SetPosition(Rigidbody rigidbody, EntityPosition position)
        {
            EntityPositionProfile profile = GetProfileByPosition(position);
            Vector2 currentPosition = rigidbody.position;
            currentPosition.y = profile.Height;
            rigidbody.position = currentPosition;
        }

        private EntityPositionProfile GetProfileByPosition(EntityPosition position)
        {
            return position switch
            {
                EntityPosition.Bottom => TopPositionProfile,
                EntityPosition.Top => BottomPositionProfile,
                _ => throw new InvalidOperationException("Undefined value"),
            };
        }
    }

    [Serializable]
    public class EntityPositionProfile
    {
        [field: SerializeField]
        public EntityPosition Position { get; private set; }

        [field: SerializeField]
        public float Height { get; private set; }

        public Vector2 Vector => new Vector2(0, Height);
    }

    public enum EntityPosition
    {
        Bottom,
        Top
    }
}
