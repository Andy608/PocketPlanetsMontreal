using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class DisplayManager : ManagerBase<DisplayManager>
    {
        private static float pixelsToUnits = 1.0f;
        private static float scale = 1.0f;

        //Designed for iPhone X resolution.
        [SerializeField] private Vector2 nativeResolution = new Vector2(1125, 2436);
        [SerializeField] private Camera gameCamera;

        public Color BackgroundColor { get { return gameCamera.backgroundColor; } }
        public float CurrentCameraSize { get { return currentSize; } }
        public float DefaultCameraSize { get { return defaultSize; } }

        private float zoomSpeed;

        private float defaultSize;
        private float currentSize;
        private float minimumSize;
        private float maximumSize;

        private float zoomDist = 0;
        private float prevZoomDist = 0;
        private float zoomOffset = 0;

        public float CameraOrthoSize { get { return gameCamera.orthographicSize; } }

        public float Scale { get { return scale; } }

        private void Awake()
        {
            if (gameCamera.orthographic)
            {
                scale = Screen.height / nativeResolution.y;
                pixelsToUnits *= scale;
                defaultSize = currentSize = gameCamera.orthographicSize = (Screen.height / 2.0f) / pixelsToUnits;
                maximumSize = gameCamera.orthographicSize * 2.0f;
                minimumSize = maximumSize / 8.0f;
            }

            zoomSpeed = Screen.dpi;
        }

        private void OnEnable()
        {
            EventManager.OnPinchBegan += ZoomBegan;
            EventManager.OnPinchHeld += ZoomHeld;
            EventManager.OnPinchEnded += ZoomEnded;
        }

        private void OnDisable()
        {
            EventManager.OnPinchBegan -= ZoomBegan;
            EventManager.OnPinchHeld -= ZoomHeld;
            EventManager.OnPinchEnded -= ZoomEnded;
        }

        private void ZoomBegan(Touch first, Touch second)
        {
            currentSize = gameCamera.orthographicSize;
            zoomDist = (first.position - second.position).magnitude;
            prevZoomDist = zoomDist;

            Zoom();
        }

        private void ZoomHeld(Touch first, Touch second)
        {
            prevZoomDist = zoomDist;
            zoomDist = (first.position - second.position).magnitude;

            Zoom();
        }

        private void ZoomEnded(Touch first, Touch second)
        {
            Zoom();

            prevZoomDist = 0;
            zoomDist = 0;
        }

        private void Zoom()
        {
            //Zoom offset in world units
            zoomOffset = prevZoomDist - zoomDist;
            UpdateCameraSize(currentSize + (zoomOffset * Time.deltaTime * zoomSpeed));
        }

        private void UpdateCameraSize(float size)
        {
            currentSize = Mathf.Clamp(size, minimumSize, maximumSize);
            gameCamera.orthographicSize = currentSize;
        }

        public static void TouchPositionToWorldVector3(Touch touch, ref Vector3 position)
        {
            position = Camera.main.ScreenToWorldPoint(touch.position);
            position.z = 0;
        }
    }
}
