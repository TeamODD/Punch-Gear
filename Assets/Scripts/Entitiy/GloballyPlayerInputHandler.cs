using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.InputSystem;

namespace PunchGear.Entity
{
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(-50)]
    public class GloballyPlayerInputHandler : MonoBehaviour, IMouseInputHandler, IKeyboardInputHandler
    {
        private static readonly KeyCode[] KeyCodes = Enum.GetValues(typeof(KeyCode)).Cast<KeyCode>().ToArray();

        public static GloballyPlayerInputHandler Instance { get; private set; }

        private readonly KeyCodeInputPool _keyCodeInputPool = new KeyCodeInputPool();
        private readonly List<IKeyboardInputAction> _keyboardInputActions = new List<IKeyboardInputAction>();
        private readonly List<IMouseInputAction> _mouseInputActions = new List<IMouseInputAction>();

        private void Awake()
        {
            if (!Input.mousePresent)
            {
                throw new InvalidOperationException("Mouse not detected.");
            }
            Instance = this;
            Debug.Log("Player Input Handler initialized");
        }

        private void OnDisable()
        {
            _keyboardInputActions.Clear();
            _mouseInputActions.Clear();
            Instance = null;
        }

        private void FixedUpdate()
        {
            _keyCodeInputPool.Update();
            MouseInputs inputs = FetchMouseInputs();
            if (_keyCodeInputPool.Count != 0)
            {
                foreach (IKeyboardInputAction action in _keyboardInputActions)
                {
                    action.OnKeyDown(_keyCodeInputPool);
                }
            }
            if (inputs != MouseInputs.None)
            {
                foreach (IMouseInputAction action in _mouseInputActions)
                {
                    action.OnMouseDown(inputs);
                }
            }
        }

        private MouseInputs FetchMouseInputs()
        {
            MouseInputs inputs = MouseInputs.None;
            Mouse mouse = Mouse.current;
            mouse.leftButton.pressPoint = 0.01f;
            mouse.rightButton.pressPoint = 0.01f;
            if (mouse.leftButton.isPressed)
            {
                inputs |= MouseInputs.Left;
            }
            if (mouse.rightButton.isPressed)
            {
                inputs |= MouseInputs.Right;
            }
            if (mouse.middleButton.isPressed)
            {
                inputs |= MouseInputs.Scroll;
            }
            return inputs;
        }

        public void AddAction(IKeyboardInputAction action)
        {
            if (_keyboardInputActions.Contains(action))
            {
                return;
            }
            _keyboardInputActions.Add(action);
        }

        public void RemoveAction(IKeyboardInputAction action)
        {
            _keyboardInputActions.Remove(action);
        }

        public void AddAction(IMouseInputAction action)
        {
            if (_mouseInputActions.Contains(action))
            {
                return;
            }
            _mouseInputActions.Add(action);
        }

        public void RemoveAction(IMouseInputAction action)
        {
            _mouseInputActions.Remove(action);
        }

        private class KeyCodeInputPool : IList<KeyCode>
        {
            private readonly KeyCode[] _inputEvents = new KeyCode[KeyCodes.Length];
            private int _position;

            public int Count
            {
                get
                {
                    return _position;
                }
            }

            public bool IsReadOnly
            {
                get
                {
                    return false;
                }
            }

            public KeyCode this[int index]
            {
                get
                {
                    if (index < 0 || index >= _position)
                    {
                        throw new IndexOutOfRangeException(nameof(index));
                    }
                    return _inputEvents[index];
                }
                set
                {
                    if (index < 0 || index > _position || index >= KeyCodes.Length)
                    {
                        throw new IndexOutOfRangeException(nameof(index));
                    }
                    _inputEvents[index] = value;
                    if (index == _position)
                    {
                        _position++;
                    }
                }
            }

            public int IndexOf(KeyCode item)
            {
                for (int i = 0; i < _position; i++)
                {
                    if (_inputEvents[i] == item)
                    {
                        return i;
                    }
                }
                return -1;
            }

            public void Insert(int index, KeyCode item)
            {
                if (index < 0 || index > _position || index >= KeyCodes.Length)
                {
                    throw new IndexOutOfRangeException(nameof(index));
                }
                for (int i = _position; i > index; i--)
                {
                    _inputEvents[i] = _inputEvents[i - 1];
                }
                _inputEvents[index] = item;
            }

            public void RemoveAt(int index)
            {
                if (index < 0 || index >= _position)
                {
                    throw new IndexOutOfRangeException(nameof(index));
                }
                _position--;
                for (int i = index; i < _position; i++)
                {
                    _inputEvents[i] = _inputEvents[i + 1];
                }
                _inputEvents[_position] = KeyCode.None;
            }

            public void Add(KeyCode item)
            {
                Insert(_position, item);
            }

            public void Clear()
            {
                for (int i = 0; i < _position; i++)
                {
                    _inputEvents[i] = KeyCode.None;
                }
                _position = 0;
            }

            public bool Contains(KeyCode item)
            {
                return IndexOf(item) >= 0;
            }

            public void CopyTo(KeyCode[] array, int arrayIndex)
            {
                throw new NotImplementedException();
            }

            public bool Remove(KeyCode item)
            {
                for (int i = 0; i < _position; i++)
                {
                    if (_inputEvents[i] != item)
                    {
                        continue;
                    }
                    RemoveAt(i);
                    return true;
                }
                return false;
            }

            public void Update()
            {
                _position = 0;
                for (int i = 0; i < KeyCodes.Length; i++)
                {
                    KeyCode keyCode = (KeyCode) i;
                    if (Input.GetKey(keyCode))
                    {
                        _inputEvents[_position] = keyCode;
                        _position++;
                    }
                }
            }

            public IEnumerator<KeyCode> GetEnumerator()
            {
                for (int i = 0; i < _position; i++)
                {
                    yield return _inputEvents[i];
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}
