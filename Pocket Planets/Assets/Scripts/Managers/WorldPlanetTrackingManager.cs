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

        //private List<Planet> pausedPlanetsInWorld = new List<Planet>();
        //private List<Planet> unpausedPlanetsInWorld = new List<Planet>();

        private void OnEnable()
        {
            EventManager.OnGamePaused += HandlePause;
            EventManager.OnGameUnpaused += HandleUnpause;
        }

        private void OnDisable()
        {
            EventManager.OnGamePaused -= HandlePause;
            EventManager.OnGameUnpaused -= HandleUnpause;
        }

        public void RegisterPlanet(Planet planet)
        {
            if (planetsInWorld != null)
            {
                Debug.Log("ADDED PLANET");
                planetsInWorld.Add(planet);

                //if (planet.PlanetState == EnumPlanetState.PAUSED)
                //{
                //    pausedPlanetsInWorld.Add(planet);
                //}
                //else
                //{
                //    unpausedPlanetsInWorld.Add(planet);
                //}
            }
        }

        public void UnregisterPlanet(Planet planet)
        {
            Debug.Log("REMOVED PLANET");

            if (planetsInWorld != null)
            {
                planetsInWorld.Remove(planet);
            }

            //if (planet.PlanetState == EnumPlanetState.PAUSED)
            //{
            //    pausedPlanetsInWorld.Remove(planet);
            //}
            //else
            //{
            //    unpausedPlanetsInWorld.Remove(planet);
            //}
        }

        private void HandlePause()
        {
            foreach(Planet planet in planetsInWorld)
            {
                PausePlanet(planet);
            }
        }

        private void HandleUnpause()
        {
            foreach (Planet planet in planetsInWorld)
            {
                UnpausePlanet(planet);
            }
        }

        private void PausePlanet(Planet planet)
        {
            if (planet.PlanetState != EnumPlanetState.PAUSED)
            {
                //planet.PlanetRigidbody.simulated = false;

                planet.PlanetTrail.Pause();

                //pausedPlanetsInWorld.Add(planet);
                //unpausedPlanetsInWorld.Remove(planet);

                planet.PlanetState = EnumPlanetState.PAUSED;
            }
        }

        private void UnpausePlanet(Planet planet)
        {
            if (planet.PlanetState == EnumPlanetState.PAUSED)
            {
                //planet.PlanetRigidbody.simulated = true;
                planet.PlanetTrail.Unpause();
                Debug.Log("UNPAUSE");

                //pausedPlanetsInWorld.Remove(planet);
                //unpausedPlanetsInWorld.Add(planet);

                planet.PlanetState = planet.PreviousPlanetState;
            }
        }

        public Planet GetPlanetAtPosition(Vector2 worldPosition)
        {
            foreach (Planet planet in PlanetsInWorld)
            {
                if ((planet.PhysicsIntegrator.Position - worldPosition).sqrMagnitude <= planet.Circumference * planet.Circumference)
                {
                    return planet;
                }
            }

            return null;
        }
    }
}
