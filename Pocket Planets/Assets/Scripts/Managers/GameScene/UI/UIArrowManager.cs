using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class UIArrowManager : ManagerBase<UIArrowManager>
    {
        enum EnumDirection
        {
            NORTH,
            SOUTH,
            EAST,
            WEST
        }

        [SerializeField] private RectTransform uiArrowContainer;
        [SerializeField] private Image arrowPrefab;

        //Grows with how many planets there are.
        private List<Image> arrowPool = new List<Image>();

        //The arrows actually showing right now.
        private List<Image> activeArrows = new List<Image>();

        //This list gets repopulated every frame.
        private List<Planet> planetsOutsideBounds = new List<Planet>();


        private float leftBounds;
        private float rightBounds;
        private float topBounds;
        private float bottomBounds;

        //Is this world space or pixel space?
        private float PADDING = 0;


        private void FixedUpdate()
        {
            PADDING = (arrowPrefab.preferredWidth / 2.0f) * DisplayManager.Instance.ZoomRatio;

            //Update bounds
            UpdateBounds();

            //Clear planetlist and active arrows list.
            planetsOutsideBounds.Clear();
            ResetActiveArrows();

            //if arrowpool size is less than planet size, instantiate more
            UpdatePoolSize();

            //Check all the planets if they're outside the bounds
            List<Planet> planetsInWorld = WorldPlanetTrackingManager.Instance.PlanetsInWorld;
            foreach (Planet planet in planetsInWorld)
            {
                //If yes, add them to the list.
                if (IsPositionOutOfBounds(planet.PhysicsIntegrator.Position))
                {
                    planetsOutsideBounds.Add(planet);
                    //Debug.Log("WE'RE OUT OF BOUNDS");
                }
            }

            //Go through planetsOutsideBounds and add that amount of arrows from the pool into the active arrows.
            //Active the arrows and set the positions
            for (int i = 0; i < planetsOutsideBounds.Count; ++i)
            {
                Planet planet = planetsOutsideBounds[i];
                Image arrow = arrowPool[i];
                SetArrowPosAndRot(planet, ref arrow);
                arrow.gameObject.SetActive(true);
                activeArrows.Add(arrow);
            }
        }

        private void UpdatePoolSize()
        {
            int planetCount = WorldPlanetTrackingManager.Instance.PlanetsInWorld.Count;
            while (arrowPool.Count < planetCount)
            {
                Image newArrow = Instantiate(arrowPrefab, uiArrowContainer) as Image;
                newArrow.gameObject.SetActive(false);
                arrowPool.Add(newArrow);
            }

            //Debug.Log("Finished updating ArrowPool size: " + arrowPool.Count);
        }

        private void UpdateBounds()
        {
            Vector3 cameraPos = DisplayManager.Instance.CameraPosition;
            leftBounds = -DisplayManager.Instance.CurrentCameraWidth + cameraPos.x;
            rightBounds = DisplayManager.Instance.CurrentCameraWidth + cameraPos.x;
            topBounds = DisplayManager.Instance.CurrentCameraHeight + cameraPos.y;
            bottomBounds = -DisplayManager.Instance.CurrentCameraHeight + cameraPos.y;
        }

        private bool IsPositionOutOfBounds(Vector2 position)
        {
            if (position.x < leftBounds || position.x > rightBounds ||
                position.y > topBounds || position.y < bottomBounds)
            {
                return true;
            }

            return false;
        }

        private void SetArrowPosAndRot(Planet planet, ref Image arrow)
        {
            //Get direction towards arrow from center of screen
            //Get direction angle and clamp to one of the four orientations

            //Vector2 direction = planet.PhysicsIntegrator.Position - DisplayManager.Instance.CameraPosition;
            float angle = 0; /*= Vector2.SignedAngle(Vector2.right, direction);*/
            //Debug.Log(angle);

            Vector3 rot = arrow.transform.rotation.eulerAngles;
            //EnumDirection dir = ClampAngle(ref angle);
            //Depending on the direction, find the position of where the arrow should be
            EnumDirection dir = SetArrowPosition(ref arrow, planet.PhysicsIntegrator.Position);

            switch (dir)
            {
                case EnumDirection.EAST:
                    angle = 0;
                    break;
                case EnumDirection.NORTH:
                    angle = 90;
                    break;
                case EnumDirection.WEST:
                    angle = 180;
                    break;
                case EnumDirection.SOUTH:
                    angle = 270;
                    break;
            }

            rot.z = angle;
            arrow.transform.rotation = Quaternion.Euler(rot);
        }

        private void ResetActiveArrows()
        {
            foreach (Image arrow in activeArrows)
            {
                arrow.gameObject.SetActive(false);
            }

            activeArrows.Clear();
        }

        private EnumDirection ClampAngle(ref float angle)
        {
            if (angle <= 45 && angle >= -45)
            {
                angle = 0;
                return EnumDirection.EAST;
            }
            else if (angle <= 135 && angle >= 45)
            {
                angle = 90;
                return EnumDirection.NORTH;
            }
            else if (angle <= 215 && angle >= 135)
            {
                angle = 180;
                return EnumDirection.WEST;
            }
            else
            {
                angle = 270;
                return EnumDirection.SOUTH;
            }
        }

        private EnumDirection SetArrowPosition(ref Image arrow, Vector2 planetPosition)
        {
            bool isFound = false;
            Vector2 intersectionTop = GetIntersectionPointCoordinates(DisplayManager.Instance.CameraPosition, planetPosition, new Vector2(leftBounds, topBounds), new Vector2(rightBounds, topBounds), out isFound);

            if (isFound && intersectionTop.x > leftBounds && intersectionTop.x < rightBounds && 
                planetPosition.y > topBounds)
            {
                Vector2 pos = Camera.main.WorldToViewportPoint(new Vector3(intersectionTop.x, intersectionTop.y - PADDING, 0.0f));
                arrow.rectTransform.anchorMin = pos;
                arrow.rectTransform.anchorMax = pos;
                return EnumDirection.NORTH;
            }

            isFound = false;
            Vector2 intersectionBottom = GetIntersectionPointCoordinates(DisplayManager.Instance.CameraPosition, planetPosition, new Vector2(leftBounds, bottomBounds), new Vector2(rightBounds, bottomBounds), out isFound);

            if (isFound && intersectionBottom.x > leftBounds && intersectionBottom.x < rightBounds && planetPosition.y < bottomBounds)
            {
                Vector2 pos = Camera.main.WorldToViewportPoint(new Vector3(intersectionBottom.x, intersectionBottom.y + PADDING, 0.0f));
                arrow.rectTransform.anchorMin = pos;
                arrow.rectTransform.anchorMax = pos;
                return EnumDirection.SOUTH;
            }

            isFound = false;
            Vector2 intersectionLeft = GetIntersectionPointCoordinates(DisplayManager.Instance.CameraPosition, planetPosition, new Vector2(leftBounds, topBounds), new Vector2(leftBounds, bottomBounds), out isFound);

            if (isFound && intersectionLeft.y < topBounds && intersectionLeft.y > bottomBounds && planetPosition.x < leftBounds)
            {
                Vector2 pos = Camera.main.WorldToViewportPoint(new Vector3(intersectionLeft.x + PADDING, intersectionLeft.y, 0.0f));
                arrow.rectTransform.anchorMin = pos;
                arrow.rectTransform.anchorMax = pos;
                return EnumDirection.WEST;
            }

            isFound = false;
            Vector2 intersectionRight = GetIntersectionPointCoordinates(DisplayManager.Instance.CameraPosition, planetPosition, new Vector2(rightBounds, topBounds), new Vector2(rightBounds, bottomBounds), out isFound);

            if (isFound && intersectionRight.y < topBounds && intersectionRight.y > bottomBounds && planetPosition.x > rightBounds)
            {
                Vector2 pos = Camera.main.WorldToViewportPoint(new Vector3(intersectionRight.x - PADDING, intersectionRight.y, 0.0f));
                arrow.rectTransform.anchorMin = pos;
                arrow.rectTransform.anchorMax = pos;
                return EnumDirection.EAST;
            }

            //Default image direction
            return EnumDirection.EAST;
        }

        //https://blog.dakwamine.fr/?p=1943
        public Vector2 GetIntersectionPointCoordinates(Vector2 A1, Vector2 A2, Vector2 B1, Vector2 B2, out bool found)
        {
            float tmp = (B2.x - B1.x) * (A2.y - A1.y) - (B2.y - B1.y) * (A2.x - A1.x);

            if (tmp == 0)
            {
                // No solution!
                found = false;
                return Vector2.zero;
            }

            float mu = ((A1.x - B1.x) * (A2.y - A1.y) - (A1.y - B1.y) * (A2.x - A1.x)) / tmp;

            found = true;

            return new Vector2(
                B1.x + (B2.x - B1.x) * mu,
                B1.y + (B2.y - B1.y) * mu
            );
        }
    }
}
