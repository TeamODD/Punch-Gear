using UnityEngine;

namespace PunchGear
{
    public class Main : MonoBehaviour
    {
        private Camera _camera;

        private static float _targetWidth = 1920;
        private static float _targetHeight = 1080;

        private void Awake()
        {
            _camera = GetComponent<Camera>();

            Rect rect = _camera.rect;
            float scaleHeight = ((float)Screen.width / Screen.height) / (_targetWidth / _targetHeight);
            float scaleWidth = 1 / scaleHeight;

            if (scaleHeight < 1f)
            {
                rect.height = scaleHeight;
                rect.y = (1f - scaleHeight) / 2;
            }
            else
            {
                rect.width = scaleWidth;
                rect.x = (1 - scaleWidth) / 2;
            }

            _camera.rect = rect;
        }

        private void OnPreCull()
        {
            GL.Clear(true, true, Color.black);
        }
    }
}
