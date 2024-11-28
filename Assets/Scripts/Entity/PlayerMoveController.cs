using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace PunchGear.Entity
{
    public class PlayerMoveController : MonoBehaviour
    {
        private GloballyPlayerInputHandler _globallyPlayerInputHandler;
        private Player _player;

        private void Awake()
        {
            _player = GetComponent<Player>();
        }

        private void OnEnable()
        {
            _globallyPlayerInputHandler = GloballyPlayerInputHandler.Instance;
            _globallyPlayerInputHandler.AddAction(new KeyboardInputAction(this, _player));
            Debug.Log("Keyboard action attached");
        }

        private void Start()
        {
            EntityPositionHandler.Instance.SetPosition(this, _player.Position);
        }

        private class KeyboardInputAction : IKeyboardInputAction
        {
            private readonly PlayerMoveController _controller;
            private readonly Player _player;

            private Coroutine _smoothDampPositionCoroutine;

            public KeyboardInputAction(PlayerMoveController controller, Player player)
            {
                _controller = controller;
                _player = player;
            }

            public void OnKeyDown(IList<KeyCode> keyCodes)
            {
                EntityPosition lastPosition = _player.Position;
                if (keyCodes.Contains(KeyCode.W))
                {
                    if (_smoothDampPositionCoroutine == null && lastPosition == EntityPosition.Bottom)
                    {
                        _smoothDampPositionCoroutine = _controller.StartCoroutine(StartAnimation(EntityPosition.Top));
                        _player.Position = EntityPosition.Top;
                    }
                }
                if (keyCodes.Contains(KeyCode.S))
                {
                    if (_smoothDampPositionCoroutine == null && lastPosition == EntityPosition.Top)
                    {
                        _smoothDampPositionCoroutine =
                            _controller.StartCoroutine(StartAnimation(EntityPosition.Bottom));
                        _player.Position = EntityPosition.Bottom;
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
    }
}
