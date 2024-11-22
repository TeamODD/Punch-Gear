using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PunchGear.Entity
{
    public class PlayerMoveController : MonoBehaviour
    {
        private static PlayerMoveController _instance;

        private GloballyPlayerInputHandler _globallyPlayerInputHandler;

        [field: SerializeField]
        public EntityPosition Position { get; internal set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            if (_instance)
            {
                return;
            }
            GameObject gameObject = new GameObject("Player Move Controller", typeof(PlayerMoveController));
            _instance = gameObject.GetComponent<PlayerMoveController>();
            Debug.Log("Player Move Controller initialized");
        }

        private void Awake()
        {
            _globallyPlayerInputHandler = GloballyPlayerInputHandler.Instance;
            _globallyPlayerInputHandler.AddAction(new KeyboardInputAction(this));
            _globallyPlayerInputHandler.AddAction(new MouseInputAction());
            Debug.Log("Keyboard action attached");
        }

        private void Start()
        {
            Position = EntityPosition.Bottom;
            EntityPositionHandler.Instance.SetPosition(this, Position);
        }

        private class KeyboardInputAction : IKeyboardInputAction
        {
            private readonly PlayerMoveController _controller;

            private Coroutine _smoothDampPositionCoroutine;

            public KeyboardInputAction(PlayerMoveController controller)
            {
                _controller = controller;
            }

            public void OnKeyDown(IList<KeyCode> keyCodes)
            {
                EntityPosition lastPosition = _controller.Position;
                if (keyCodes.Contains(KeyCode.W))
                {
                    if (_smoothDampPositionCoroutine == null && lastPosition == EntityPosition.Bottom)
                    {
                        _smoothDampPositionCoroutine = _controller.StartCoroutine(StartAnimation(EntityPosition.Top));
                        _controller.Position = EntityPosition.Top;
                    }
                }
                if (keyCodes.Contains(KeyCode.S))
                {
                    if (_smoothDampPositionCoroutine == null && lastPosition == EntityPosition.Top)
                    {
                        _smoothDampPositionCoroutine = _controller.StartCoroutine(StartAnimation(EntityPosition.Bottom));
                        _controller.Position = EntityPosition.Bottom;
                    }
                }
            }

            private IEnumerator StartAnimation(EntityPosition position)
            {
                yield return EntityPositionHandler.Instance.SmoothDampPosition(
                    _controller.transform,
                    position,
                    0.4f,
                    0.1f);
                _smoothDampPositionCoroutine = null;
            }
        }

        private class MouseInputAction : IMouseInputAction
        {
            public void OnMouseDown(MouseInputs inputs)
            {
                if (inputs.HasFlag(MouseInputs.Left))
                {
                    Debug.Log("Left button clicked");
                }
                if (inputs.HasFlag(MouseInputs.Right))
                {
                    Debug.Log("Right button clicked");
                }
            }
        }
    }
}
