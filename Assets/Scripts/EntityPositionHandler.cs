using System;
using System.Collections;
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

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Initialize()
        {
            _instance = FindFirstObjectByType<EntityPositionHandler>();
            if (!_instance)
            {
                throw new NullReferenceException("Cannot find any Entity position handler");
            }
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

        public EntityPositionProfile this[EntityPosition position]
        {
            get
            {
                return position switch
                {
                    EntityPosition.Top => TopPositionProfile,
                    EntityPosition.Bottom => BottomPositionProfile,
                    _ => throw new InvalidOperationException("Undefined value")
                };
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

        public IEnumerator SmoothDampPosition(Transform transform, EntityPosition targetPosition, float duration, float smoothTime)
        {
            Vector2 targetVector = this[targetPosition].Vector;
            targetVector.x = transform.position.x;
            float elapsedTime = 0f;
            Vector2 velocityVector = Vector2.zero;
            while (elapsedTime < duration)
            {
                // SmoothDamp를 통해 부드럽게 이동
                transform.position = Vector2.SmoothDamp(
                    transform.position,
                    targetVector,
                    ref velocityVector,
                    smoothTime // 감속 시간
                );
                elapsedTime += Time.deltaTime; // 경과 시간 증가
                yield return null; // 다음 프레임까지 대기
            }

            // 이동 완료 후 정확히 목표 위치로 설정
            SetPosition(transform, targetPosition);
        }

        private EntityPositionProfile GetProfileByPosition(EntityPosition position)
        {
            return position switch
            {
                EntityPosition.Bottom => BottomPositionProfile,
                EntityPosition.Top => TopPositionProfile,
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
