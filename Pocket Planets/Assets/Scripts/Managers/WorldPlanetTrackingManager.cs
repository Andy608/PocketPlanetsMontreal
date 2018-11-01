using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class WorldPlanetTrackingManager : ManagerBase<WorldPlanetTrackingManager>
    {
        //Holds all objects, even objects that are spawned but haven't been released yet.
        private List<Planet> planetsInWorld = new List<Planet>();

        public List<Planet> PlanetsInWorld { get { return planetsInWorld; } }

        public void RegisterPlanet(Planet planet)
        {
            if (planetsInWorld != null)
            {
                Debug.Log("ADDED PLANET");
                planetsInWorld.Add(planet);
            }
        }

        public void UnregisterPlanet(Planet planet)
        {
            if (planetsInWorld != null)
            {
                planetsInWorld.Remove(planet);
            }
        }
    }
}
