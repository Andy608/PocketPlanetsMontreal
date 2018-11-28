using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class CameraMovementManager : ManagerBase<CameraMovementManager>
    {
        private Vector3 UNIVERSE_CENTER = new Vector3(0, 0, -10);
        [SerializeField] private Camera gameCamera;

        private static Vector3 startPosition = new Vector3();
        private static Vector3 dragPosition = new Vector3();

        private void OnEnable()
        {
            EventManager.OnDragBegan += HandleDragBegan;
            EventManager.OnDragHeld += HandleDragHeld;
            EventManager.OnDragEnded += HandleDragEnded;
            EventManager.OnCameraCenterSelected += CenterCamera;
        }

        private void OnDisable()
        {
            EventManager.OnDragBegan -= HandleDragBegan;
            EventManager.OnDragHeld -= HandleDragHeld;
            EventManager.OnDragEnded -= HandleDragEnded;
            EventManager.OnCameraCenterSelected -= CenterCamera;
        }

        private void CenterCamera()
        {
            //Make this JUICY
            gameCamera.transform.position = UNIVERSE_CENTER;
        }

        private void HandleDragBegan(Touch touch)
        {
            if (CameraStateManager.Instance.CurrentCameraState == EnumCameraState.FREE_ROAM)
            {
                DisplayManager.TouchPositionToWorldVector3(touch, ref startPosition);
            }
        }

        private void HandleDragHeld(Touch touch)
        {
            if (CameraStateManager.Instance.CurrentCameraState == EnumCameraState.FREE_ROAM)
            {
                DragCamera(touch);
            }
        }

        private void HandleDragEnded(Touch touch)
        {
            if (CameraStateManager.Instance.CurrentCameraState == EnumCameraState.FREE_ROAM)
            {
                DragCamera(touch);
            }
        }

        private void DragCamera(Touch touch)
        {
            DisplayManager.TouchPositionToWorldVector3(touch, ref dragPosition);

            PanHorizontal(startPosition - dragPosition);
            PanVertical(startPosition - dragPosition);

            //Vector3 futurePos = gameCamera.transform.position + (startPosition - dragPosition);
            //Vector3 pos = Vector3.zero;
            //if (CanPanHorizontal(futurePos))
            //{
            //    pos = gameCamera.transform.position;
            //    pos.x = futurePos.x;
            //    gameCamera.transform.position = pos;
            //}

            //futurePos = gameCamera.transform.position + (startPosition - dragPosition);

            //if (CanPanVertical(futurePos))
            //{
            //    pos = gameCamera.transform.position;
            //    pos.y = futurePos.y;
            //    gameCamera.transform.position = pos;
            //}
        }

        //private bool CanPanVertical(Vector3 futurePosition)
        //{
        //    float topBorderY = futurePosition.y + DisplayManager.Instance.CurrentCameraHeight;
        //    float bottomBorderY = futurePosition.y - DisplayManager.Instance.CurrentCameraHeight;
        //    float borderY = WorldBoundsManager.Instance.VerticalRadius;

        //    if (topBorderY >= borderY || bottomBorderY <= -borderY)
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        //private bool CanPanHorizontal(Vector3 futurePosition)
        //{
        //    float rightBorderX = futurePosition.x + DisplayManager.Instance.CurrentCameraWidth;
        //    float leftBorderX = futurePosition.x - DisplayManager.Instance.CurrentCameraWidth;
        //    float borderX = WorldBoundsManager.Instance.HorizontalRadius;

        //    if (rightBorderX >= borderX)
        //    {
        //        return false;
        //    }
        //    else if (leftBorderX <= -borderX)
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        private void PanHorizontal(Vector3 dragDistance)
        {
            float rightBorderX = gameCamera.transform.position.x + dragDistance.x + DisplayManager.Instance.CurrentCameraWidth;
            float leftBorderX = gameCamera.transform.position.x + dragDistance.x - DisplayManager.Instance.CurrentCameraWidth;
            float borderX = WorldBoundsManager.Instance.HorizontalRadius;

            Vector3 cameraPosition = gameCamera.transform.position;

            if (rightBorderX >= borderX)
            {
                cameraPosition.x = borderX - DisplayManager.Instance.CurrentCameraWidth;
            }
            else if (leftBorderX <= -borderX)
            {
                cameraPosition.x = -borderX + DisplayManager.Instance.CurrentCameraWidth;
            }
            else
            {
                cameraPosition.x += dragDistance.x;
            }

            gameCamera.transform.position = cameraPosition;
            //return true;
        }

        private void PanVertical(Vector3 dragDistance)
        {
            float topBorderY = gameCamera.transform.position.y + dragDistance.y + DisplayManager.Instance.CurrentCameraHeight;
            float bottomBorderY = gameCamera.transform.position.y + dragDistance.y - DisplayManager.Instance.CurrentCameraHeight;
            float borderY = WorldBoundsManager.Instance.VerticalRadius;

            Vector3 cameraPosition = gameCamera.transform.position;

            if (topBorderY >= borderY)
            {
                cameraPosition.y = borderY - DisplayManager.Instance.CurrentCameraHeight;
            }
            else if (bottomBorderY <= -borderY)
            {
                cameraPosition.y = -borderY + DisplayManager.Instance.CurrentCameraHeight;
            }
            else
            {
                cameraPosition.y += dragDistance.y;
            }

            gameCamera.transform.position = cameraPosition;
            //return true;
        }
    }
}
