using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class WorldBoundsManager : ManagerBase<WorldBoundsManager>
    {
        private const float MAX_HORIZONTAL_RADIUS = 5000;
        private const float MAX_VERTICAL_RADIUS = 5000;

        private float xRadius;
        private float yRadius;

        public float HorizontalRadius { get { return xRadius; } }
        public float VerticalRadius { get { return yRadius; } }

        //If there is an object outside of the bounds, delete it.

        private void Start()
        {
            xRadius = MAX_HORIZONTAL_RADIUS;
            yRadius = MAX_VERTICAL_RADIUS;
        }

        private void Update()
        {
            List<Planet> activePlanets = WorldPlanetTrackingManager.Instance.PlanetsInWorld;
            Vector3 currentPlanetPosition;

            for (int i = 0; i < activePlanets.Count; ++i)
            {
                currentPlanetPosition = activePlanets[i].transform.position;

                if (currentPlanetPosition.x > xRadius || currentPlanetPosition.x < -xRadius ||
                    currentPlanetPosition.y > yRadius || currentPlanetPosition.y < -yRadius)
                {
                    Debug.Log("Outside boundry!");

                    if (activePlanets[i].PlanetProperties.PlanetType == EnumPlanetType.COMET && !PlanetStoreManager.Instance.GetPlanetProperty(EnumPlanetType.COMET).IsUnlocked)
                    {
                        continue;
                    }

                    if (EventManager.OnPlanetDestroyed != null)
                    {
                        EventManager.OnPlanetDestroyed(activePlanets[i]);
                    }

                    Destroy(activePlanets[i].gameObject);
                    --i;
                }
            }
        }
    }
}
