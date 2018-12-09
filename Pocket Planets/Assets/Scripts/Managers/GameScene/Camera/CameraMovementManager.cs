using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class CameraMovementManager : ManagerBase<CameraMovementManager>
    {
        public static Vector3 UNIVERSE_CENTER = new Vector3(0, 0, -10);
        [SerializeField] private Camera gameCamera;

        private static Vector3 startPosition = new Vector3();
        private static Vector3 dragPosition = new Vector3();
        private static Vector3 touchPosition = new Vector3();

        private Planet targetPlanet = null;
        private Vector3 targetCameraPosition = Vector3.zero;

        public Planet TargetPlanet { get { return targetPlanet; } }

        private void OnEnable()
        {
            EventManager.OnDragBegan += HandleDragBegan;
            EventManager.OnDragHeld += HandleDragHeld;
            EventManager.OnDragEnded += HandleDragEnded;

            EventManager.OnTapOccurred += HandleTapOccurred;
            EventManager.OnPlanetAbsorbed += HandlePlanetAbsorbed;
            EventManager.OnPlanetUpgraded += HandlePlanetAbsorbed;

            EventManager.OnGoToNextPlanetSelected += CenterCamera;
            EventManager.OnCameraAnchoredSelected += CameraAchoredSelected;
            EventManager.OnCameraFreeroamSelected += CameraFreeroamSelected;
        }

        private void OnDisable()
        {
            EventManager.OnDragBegan -= HandleDragBegan;
            EventManager.OnDragHeld -= HandleDragHeld;
            EventManager.OnDragEnded -= HandleDragEnded;

            EventManager.OnTapOccurred -= HandleTapOccurred;
            EventManager.OnPlanetAbsorbed -= HandlePlanetAbsorbed;
            EventManager.OnPlanetUpgraded -= HandlePlanetAbsorbed;

            EventManager.OnGoToNextPlanetSelected -= CenterCamera;
            EventManager.OnCameraAnchoredSelected -= CameraAchoredSelected;
            EventManager.OnCameraFreeroamSelected -= CameraFreeroamSelected;
        }

        public void DestroyTargetPlanet()
        {
            if (targetPlanet)
            {
                if (EventManager.OnPlanetDestroyed != null)
                {
                    EventManager.OnPlanetDestroyed(targetPlanet);
                }

                Destroy(targetPlanet.gameObject);
                targetPlanet = null;
            }
        }

        private void CenterCamera()
        {
            List<Planet> planets = WorldPlanetTrackingManager.Instance.PlanetsInWorld;
            if (targetPlanet)
            {
                for (int i = 0; i < planets.Count; ++i)
                {
                    if (planets[i] == targetPlanet)
                    {
                        targetPlanet = planets[(i + 1) % planets.Count];
                        break;
                    }
                }
            }
            else if (planets.Count > 0)
            {
                targetPlanet = planets[0];
            }
            else
            {
                //Make this JUICY
                gameCamera.transform.position = UNIVERSE_CENTER;
                targetPlanet = null;
            }
        }

        private void CameraFreeroamSelected()
        {
            //targetPlanet = null;
        }

        private void CameraAchoredSelected()
        {
            //targetPlanet = null;
        }

        private void HandlePlanetAbsorbed(Planet absorber, Planet absorbed)
        {
            if (absorbed == targetPlanet)
            {
                targetPlanet = absorber;
            }
        }

        private void HandleTapOccurred(Touch touch)
        {
            if (InputManager.IsPointerOverUIObject()) return;

            if (CameraStateManager.Instance.CurrentCameraState == EnumCameraState.FREE_ROAM)
            {
                //Get planet at touch position.
                DisplayManager.TouchPositionToWorldVector3(touch, ref touchPosition);
                targetPlanet = WorldPlanetTrackingManager.Instance.GetPlanetAtPosition(touchPosition);

                Debug.Log("TARGET PLANET: " + targetPlanet);

                //if (targetPlanet)
                //{
                //    if (EventManager.OnCameraFollowSelected != null)
                //    {
                //        EventManager.OnCameraFollowSelected();
                //    }
                //}
            }
            //else if (CameraStateManager.Instance.CurrentCameraState == EnumCameraState.FOLLOW)
            //{
            //    //Get planet at touch position.
            //    DisplayManager.TouchPositionToWorldVector3(touch, ref touchPosition);
            //    targetPlanet = WorldPlanetTrackingManager.Instance.GetPlanetAtPosition(touchPosition);
            //}
        }

        private void FixedUpdate()
        {
            if (targetPlanet)
            {
                targetCameraPosition = targetPlanet.PhysicsIntegrator.Position;
                targetCameraPosition.z = UNIVERSE_CENTER.z;
                gameCamera.transform.position = targetCameraPosition;
            }
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
            if (InputManager.IsPointerOverUIObject()) return;

            DisplayManager.TouchPositionToWorldVector3(touch, ref dragPosition);

            //PanHorizontal(startPosition - dragPosition);
            //PanVertical(startPosition - dragPosition);
            PanCamera(startPosition - dragPosition);
            targetPlanet = null;
        }

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
        }
        
        private void PanCamera(Vector3 dragDistance)
        {
            Vector3 cameraPosition = gameCamera.transform.position;
            cameraPosition += dragDistance;

            if (EventManager.OnPanCamera != null)
            {
                EventManager.OnPanCamera(dragDistance);
            }

            gameCamera.transform.position = cameraPosition;
        }
    }
}
