using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class RelativeGravityManager : ManagerBase<RelativeGravityManager>
    {
        //We take the velocity of the target planet and subtract it from all the planets in the universe including the target planet.

        public void FixedUpdate()
        {
            MakeRelative();
        }

        private void MakeRelative()
        {
            Planet targetPlanet = CameraMovementManager.Instance.TargetPlanet;

            if (targetPlanet)
            {
                List<Planet> planets = WorldPlanetTrackingManager.Instance.PlanetsInWorld;
                foreach (Planet planet in planets)
                {
                    planet.PhysicsIntegrator.RelativeVelocity = -targetPlanet.PhysicsIntegrator.Velocity;
                }
            }
            else
            {
                List<Planet> planets = WorldPlanetTrackingManager.Instance.PlanetsInWorld;
                foreach (Planet planet in planets)
                {
                    planet.PhysicsIntegrator.RelativeVelocity = Vector2.zero;
                }
            }
        }
    }
}
