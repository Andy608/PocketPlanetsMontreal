using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class PhysicsIntegrationManager : ManagerBase<PhysicsIntegrationManager>
    {
        private float gameSpeed = 1.0f;
        private List<Planet> planetsInUniverse;

        private void FixedUpdate()
        {
            if (GameStateManager.Instance.IsState(GameStateManager.EnumGameState.RUNNING))
            {
                planetsInUniverse = WorldPlanetTrackingManager.Instance.PlanetsInWorld;

                for (int i = 0; i < planetsInUniverse.Count; ++i)
                {
                    if (planetsInUniverse[i].PlanetState == EnumPlanetState.ALIVE)
                    {
                        planetsInUniverse[i].PhysicsIntegrator.Integrate();
                    }
                }
            }
        }
    }
}