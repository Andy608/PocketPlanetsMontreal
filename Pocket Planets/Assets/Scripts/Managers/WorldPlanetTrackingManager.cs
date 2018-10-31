using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class WorldPlanetTrackingManager : ManagerBase<WorldPlanetTrackingManager>
    {
        private List<Planet> objectsInWorld = new List<Planet>();

        public void RegisterObject(Planet planet)
        {
            if (objectsInWorld != null)
            {
                objectsInWorld.Add(planet);
            }
        }

        public void UnregisterObject(Planet planet)
        {
            if (objectsInWorld != null)
            {
                objectsInWorld.Remove(planet);
            }
        }
    }
}
