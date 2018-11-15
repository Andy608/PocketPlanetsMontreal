using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class PhysicsIntegrationManager : ManagerBase<PhysicsIntegrationManager>
    {
        private float gameSpeed = 1.0f;
        private List<Planet> planetsInUniverse;

        private bool isTheoretical = false;
        public bool IsTheoretical { get { return isTheoretical; } set { isTheoretical = value; } }

        private void Start()
        {
            isTheoretical = true;
        }

        public float counter = 0.0f;
        public float ticker = 1.0f;

        private void FixedUpdate()
        {
            //Debug.Log("GAME STATE: " + GameStateManager.Instance.CurrentGameState);
            if (GameStateManager.Instance.IsState(GameStateManager.EnumGameState.RUNNING))
            {
                planetsInUniverse = WorldPlanetTrackingManager.Instance.PlanetsInWorld;
                //Debug.Log("Planets In Universe: " + planetsInUniverse.Count);

                if ((counter += ticker) >= 1.0f)
                {
                    while (counter > 1.0f)
                    {
                        GravityManager.Instance.IntegrateGravity();

                        counter -= 1.0f;
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

        public void SyncTheoretical()
        {
            planetsInUniverse = WorldPlanetTrackingManager.Instance.PlanetsInWorld;

            for (int i = 0; i < planetsInUniverse.Count; ++i)
            {
                if (planetsInUniverse[i].PlanetState != EnumPlanetState.DEAD)
                {
                    planetsInUniverse[i].PhysicsIntegrator.SyncTheoreticalData();
                }
            }
        }

        public void IntegrateTheoretical()
        {
            planetsInUniverse = WorldPlanetTrackingManager.Instance.PlanetsInWorld;

            GravityManager.Instance.IntegrateTheoreticalGravity();
            for (int i = 0; i < planetsInUniverse.Count; ++i)
            {
                if (planetsInUniverse[i].PlanetState != EnumPlanetState.DEAD)
                {
                    planetsInUniverse[i].PhysicsIntegrator.IntegrateTheoretical();
                }
            }
        }
    }
}