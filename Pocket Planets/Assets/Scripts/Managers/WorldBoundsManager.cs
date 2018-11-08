using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class WorldBoundsManager : ManagerBase<WorldBoundsManager>
    {
        private float xRadius;
        private float yRadius;

        //If there is an object outside of the bounds, delete it.

        private void Start()
        {
            xRadius = DisplayManager.Instance.MaxCameraWidth;
            yRadius = DisplayManager.Instance.MaxCameraHeight;
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
