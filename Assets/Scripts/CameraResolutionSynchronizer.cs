using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.UI;

namespace PunchGear
{
    [DisallowMultipleComponent]
    public class CameraResolutionSynchronizer : MonoBehaviour
    {
        private const float TargetWidth = 1920;
        private const float TargetHeight = 1080;
        private const float TargetRatio = 1.7777778f;

        private Camera _camera;
        private Canvas _canvas;
        private CanvasScaler _scaler;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            if (!_camera)
            {
                throw new UnassignedReferenceException("This script requires a camera component to be attached to it.");
            }
            _canvas = FindFirstObjectByType<Canvas>();
            _scaler = _canvas.GetComponent<CanvasScaler>();
            if (!_scaler)
            {
                _scaler = _canvas.AddComponent<CanvasScaler>();
            }
            _canvas.renderMode = RenderMode.ScreenSpaceCamera;
            _canvas.worldCamera = _camera;
            _scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            _scaler.referenceResolution = new Vector2(TargetWidth, TargetHeight);
            _scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
        }

        private void OnEnable()
        {
            UpdateCameraRect();
        }

        private void OnPreCull()
        {
            GL.Clear(true, true, Color.black);
        }

        private void UpdateCameraRect()
        {
            Rect rect = _camera.rect;
            float screenRatio = Screen.width / (float) Screen.height;
            float scaleHeight = screenRatio / TargetRatio;
            float scaleWidth = 1 / scaleHeight;

            if (scaleHeight < 1)
            {
                rect.height = scaleHeight;
                rect.y = (1 - scaleHeight) * 0.5f;
            }
            else
            {
                rect.width = scaleWidth;
                rect.x = (1 - scaleWidth) * 0.5f;
            }
            _camera.rect = rect;
        }
    }
}
