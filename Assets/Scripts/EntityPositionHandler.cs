using System;
using System.Collections;

using UnityEngine;

namespace PunchGear
{
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(-50)]
    public class EntityPositionHandler : MonoBehaviour
    {
        public static EntityPositionHandler Instance { get; private set; }

        [field: SerializeField]
        public EntityPositionProfile TopPositionProfile { get; private set; }

        [field: SerializeField]
        public EntityPositionProfile BottomPositionProfile { get; private set; }

        private void Awake()
        {
            if (Instance)
            {
                throw new NullReferenceException("Entity position handler already exists");
            }
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
            Instance = this;
        }

        private void OnDisable()
        {
            Instance = null;
        }

        public EntityPositionProfile this[EntityPosition position]
        {
            get
            {
                return position switch
                {
                    EntityPosition.Top => TopPositionProfile,
                    EntityPosition.Bottom => BottomPositionProfile,
                    var _ => throw new InvalidOperationException("Undefined value")
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

        public IEnumerator SmoothDampPosition(
            Transform transform,
            EntityPosition targetPosition,
            float duration,
            float smoothTime)
        {
            Vector2 targetVector = this[targetPosition].Vector;
            targetVector.x = transform.position.x;
            float elapsedTime = 0f;
            Vector2 velocityVector = Vector2.zero;
            while (elapsedTime < duration)
            {
                transform.position = Vector2.SmoothDamp(
                    transform.position,
                    targetVector,
                    ref velocityVector,
                    smoothTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            SetPosition(transform, targetPosition);
        }

        private EntityPositionProfile GetProfileByPosition(EntityPosition position)
        {
            return position switch
            {
                EntityPosition.Bottom => BottomPositionProfile,
                EntityPosition.Top => TopPositionProfile,
                var _ => throw new InvalidOperationException("Undefined value")
            };
        }
    }
}
