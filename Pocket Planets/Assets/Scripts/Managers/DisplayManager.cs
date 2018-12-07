using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class DisplayManager : ManagerBase<DisplayManager>
    {
        public static int TARGET_SCREEN_DENSITY = 96;
        private static float scale = 1.0f;

        //Designed for iPhone X resolution.
        [SerializeField] private Vector2 nativeResolution = new Vector2(1125, 2436);
        [SerializeField] private Camera gameCamera;

        private float zoomSpeed;

        private float defaultSize;
        private float currentSize;
        private float minimumSize;
        private float maximumSize;

        [SerializeField] private float zoomScale = 1.0f;
        [SerializeField] private bool touchToZoomEnabled = false;

        private float zoomDist = 0;
        private float prevZoomDist = 0;
        private float zoomOffset = 0;

        private float futureZoomDist = 0;
        private float futurePrevZoomDist = 0;
        private float futureZoomOffset = 0;

        public Color BackgroundColor { get { return gameCamera.backgroundColor; } }

        public float CameraOrthoSize { get { return gameCamera.orthographicSize; } }
        public float CurrentCameraHeight { get { return currentSize; } }
        public float CurrentCameraWidth { get { return currentSize * Screen.width / Screen.height; } }
        public float DefaultCameraSize { get { return defaultSize; } }

        public float MaxCameraHeight { get { return maximumSize; } }
        public float MaxCameraWidth { get { return maximumSize * Screen.width / Screen.height; } }

        public float Scale { get { return scale; } }

        private void Awake()
        {
            OnValidate();
        }

        private void OnEnable()
        {
            if (touchToZoomEnabled)
            {
                EventManager.OnPinchBegan += ZoomBegan;
                EventManager.OnPinchHeld += ZoomHeld;
                EventManager.OnPinchEnded += ZoomEnded;
            }
        }

        private void OnDisable()
        {
            if (touchToZoomEnabled)
            {
                EventManager.OnPinchBegan -= ZoomBegan;
                EventManager.OnPinchHeld -= ZoomHeld;
                EventManager.OnPinchEnded -= ZoomEnded;
            }

            //UpdateCameraSize(defaultSize);
        }

        private void OnValidate()
        {
            if (gameCamera.orthographic)
            {
                scale = Screen.height / nativeResolution.y;
                defaultSize = currentSize = (zoomScale * (Screen.height / 2.0f) / scale);
                maximumSize = defaultSize * 2.0f;
                minimumSize = maximumSize / 8.0f;
            }

            zoomSpeed = TARGET_SCREEN_DENSITY;

            UpdateCameraSize(currentSize);
        }

        private void ZoomBegan(Touch first, Touch second)
        {
            currentSize = gameCamera.orthographicSize;
            futureZoomDist = (first.position - second.position).magnitude;
            futurePrevZoomDist = futureZoomDist;

            futureZoomOffset = futurePrevZoomDist - futureZoomDist;

            if (!CanZoom(currentSize + (futureZoomOffset * Time.deltaTime * zoomSpeed), 
                currentSize + (futureZoomOffset * Time.deltaTime * zoomSpeed) * Screen.width / Screen.height))
            {
                futureZoomDist = zoomDist;
                futurePrevZoomDist = prevZoomDist;
                futureZoomOffset = zoomOffset;
            }
            else
            {
                zoomDist = futureZoomDist;
                prevZoomDist = futurePrevZoomDist;
                Zoom();
            }
        }

        private void ZoomHeld(Touch first, Touch second)
        {
            //if (!CanZoom()) return;

            //prevZoomDist = zoomDist;
            //zoomDist = (first.position - second.position).magnitude;

            //Zoom();

            futurePrevZoomDist = zoomDist;
            futureZoomDist = (first.position - second.position).magnitude;

            futureZoomOffset = futurePrevZoomDist - futureZoomDist;

            if (!CanZoom(currentSize + (futureZoomOffset * Time.deltaTime * zoomSpeed),
                currentSize + (futureZoomOffset * Time.deltaTime * zoomSpeed) * Screen.width / Screen.height))
            {
                futureZoomDist = zoomDist;
                futurePrevZoomDist = prevZoomDist;
                futureZoomOffset = zoomOffset;
            }
            else
            {
                zoomDist = futureZoomDist;
                prevZoomDist = futurePrevZoomDist;
                Zoom();
            }
        }

        private void ZoomEnded(Touch first, Touch second)
        {
            //if (!CanZoom()) return;

            //Zoom();

            //prevZoomDist = 0;
            //zoomDist = 0;

            if (CanZoom(currentSize + (futureZoomOffset * Time.deltaTime * zoomSpeed),
                currentSize + (futureZoomOffset * Time.deltaTime * zoomSpeed) * Screen.width / Screen.height))
            {
                Zoom();
            }

            prevZoomDist = 0;
            zoomDist = 0;
        }

        private void Zoom()
        {
            zoomOffset = prevZoomDist - zoomDist;
            UpdateCameraSize(currentSize + (zoomOffset * Time.deltaTime * zoomSpeed));
        }

        private bool CanZoom(float futureWidth, float futureHeight)
        {
            float topBorderY = gameCamera.transform.position.y + futureHeight;
            float bottomBorderY = gameCamera.transform.position.y - futureHeight;
            float rightBorderX = gameCamera.transform.position.x + futureWidth;
            float leftBorderX = gameCamera.transform.position.x - futureWidth;

            float borderX = WorldBoundsManager.Instance.HorizontalRadius;
            float borderY = WorldBoundsManager.Instance.VerticalRadius;

            if (topBorderY >= borderY ||
                bottomBorderY <= -borderY ||
                rightBorderX >= borderX ||
                leftBorderX <= -borderX)
            {
                return false;
            }

            return true;
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

        public bool IsInView(Vector2 position)
        {
            float yRadius = gameCamera.orthographicSize;
            float xRadius = gameCamera.orthographicSize * Screen.width / Screen.height;

            if (position.x < xRadius && position.x > -xRadius &&
                position.y < yRadius && position.y > -yRadius)
            {
                return true;
            }

            return false;
        }
    }
}
