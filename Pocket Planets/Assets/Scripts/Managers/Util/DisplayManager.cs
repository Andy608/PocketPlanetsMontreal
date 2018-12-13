using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class DisplayManager : ManagerBase<DisplayManager>
    {
        public static int TARGET_SCREEN_DENSITY = 96;
        private const float MOUSE_SENSITIVITY = 4.0f;
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

        public Color BackgroundColor { get { return gameCamera.backgroundColor; } }

        public Vector2 CameraPosition { get { return gameCamera.transform.position; } }
        public float CameraOrthoSize { get { return gameCamera.orthographicSize; } }
        public float CurrentCameraHeight { get { return currentSize; } }
        public float CurrentCameraWidth { get { return currentSize * Screen.width / Screen.height; } }
        public float DefaultCameraSize { get { return defaultSize; } }

        public float MaxCameraHeight { get { return maximumSize; } }
        public float MaxCameraWidth { get { return maximumSize * Screen.width / Screen.height; } }

        public float ZoomOffset { get { return zoomOffset; } }
        public float ZoomRatio { get { return currentSize / defaultSize; } }

        public float Scale { get { return scale; } }

        private void Awake()
        {
            OnValidate();
        }

        private void Start()
        {
            gameCamera.transform.position = CameraMovementManager.UNIVERSE_CENTER;
        }

        private void OnEnable()
        {
            if (touchToZoomEnabled)
            {
                EventManager.OnScrollOccurred += MouseZoom;
                EventManager.OnPinchBegan += ZoomBegan;
                EventManager.OnPinchHeld += ZoomHeld;
                EventManager.OnPinchEnded += ZoomEnded;
            }
        }

        private void OnDisable()
        {
            if (touchToZoomEnabled)
            {
                EventManager.OnScrollOccurred -= MouseZoom;
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
                maximumSize = defaultSize * 8.0f;
                minimumSize = defaultSize / 4.0f;
            }

            zoomSpeed = TARGET_SCREEN_DENSITY;

            UpdateCameraSize(currentSize);
        }

        private void MouseZoom(float direction)
        {
            prevZoomDist = zoomDist;
            zoomDist += direction * zoomSpeed * MOUSE_SENSITIVITY;

            Zoom();
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
            zoomOffset = prevZoomDist - zoomDist;
            UpdateCameraSize(currentSize + (zoomOffset * Time.deltaTime * zoomSpeed));
        }

        private void UpdateCameraSize(float size)
        {
            currentSize = Mathf.Clamp(size, minimumSize, maximumSize);
            gameCamera.orthographicSize = currentSize;

            //if (touchToZoomEnabled)
            //{
            //    KeepCameraInBounds();
            //}
        }

        public static void TouchPositionToWorldVector3(Vector3 touchPos, ref Vector3 position)
        {
            position = Camera.main.ScreenToWorldPoint(touchPos);
            position.z = 0;
        }

        public bool IsInView(Vector2 position)
        {
            //float yRadius = gameCamera.orthographicSize;
            //float xRadius = gameCamera.orthographicSize * Screen.width / Screen.height;

            float leftBounds = -CurrentCameraWidth + gameCamera.transform.position.x;
            float rightBounds = CurrentCameraWidth + gameCamera.transform.position.x;
            float topBounds = CurrentCameraHeight + gameCamera.transform.position.y;
            float bottomBounds = -CurrentCameraHeight + gameCamera.transform.position.y;

            if (position.x < rightBounds && position.x > leftBounds &&
                position.y < topBounds && position.y > bottomBounds)
            {
                return true;
            }

            return false;
        }

        //private void KeepCameraInBounds()
        //{
        //    float borderX = WorldBoundsManager.Instance.HorizontalRadius;
        //    float borderY = WorldBoundsManager.Instance.VerticalRadius;
        //    float cameraBorderTop = gameCamera.transform.position.y + CurrentCameraHeight;
        //    float cameraBorderBottom = gameCamera.transform.position.y - CurrentCameraHeight;
        //    float cameraBorderRight = gameCamera.transform.position.x + CurrentCameraWidth;
        //    float cameraBorderLeft = gameCamera.transform.position.x - CurrentCameraWidth;

        //    Vector3 clampedPosition = Vector3.zero;

        //    if (cameraBorderLeft <= -borderX)
        //    {
        //        clampedPosition.x += (-borderX - cameraBorderLeft);
        //    }
        //    else if (cameraBorderRight >= borderX)
        //    {
        //        clampedPosition.x -= cameraBorderRight - borderX;
        //    }

        //    if (cameraBorderBottom <= -borderY)
        //    {
        //        clampedPosition.y += -borderY - cameraBorderBottom;
        //    }
        //    else if (cameraBorderTop >= borderY)
        //    {
        //        clampedPosition.y -= cameraBorderTop - borderY;
        //    }

        //    gameCamera.transform.position += clampedPosition;
        //}
    }
}
