using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class GravityManager : ManagerBase<GravityManager>
    {
        public static readonly float G = 6.67408f * 1000.0f;//Heuristic
        private static readonly float MIN_THRESHOLD = 1.0f;

        private void Update()
        {
            if (GameStateManager.Instance.IsState(GameStateManager.EnumGameState.RUNNING))
            {
                List<Planet> planets = WorldPlanetTrackingManager.Instance.PlanetsInWorld;

                int i, j;
                for (i = 0; i < planets.Count; ++i)
                {
                    Planet attractee = planets[i];

                    if (attractee.PlanetState == EnumPlanetState.ALIVE && !attractee.PlanetProperties.IsAnchor)
                    {
                        for (j = 0; j < planets.Count; ++j)
                        {
                            Planet attractor = planets[j];

                            if (attractor == attractee)
                            {
                                continue;
                            }
                            else if (attractor.PlanetState == EnumPlanetState.ALIVE)
                            {
                                //Pull attractee
                                AttractPlanet(attractor, attractee);
                            }
                        }
                    }
                }
            }
        }

        private void AttractPlanet(Planet attractor, Planet attractee)
        {
            Vector2 direction = attractor.PhysicsIntegrator.Position - attractee.PhysicsIntegrator.Position;
            float distanceSquared = direction.sqrMagnitude;
            float distanceFromCenter = attractor.Radius + attractee.Radius;

            //Debug.Log("Attractor dist: " + attractor.PhysicsIntegrator.Position + " | Attractee dist: " + attractee.PhysicsIntegrator.Position);
            //Debug.Log("Distance Squared: " + distanceSquared + " | Distance From Center: " + distanceFromCenter);

            //If the planets are far enough away, then attract.
            if (distanceSquared > (distanceFromCenter * distanceFromCenter))
            {
                float gravitationScalar = (G * (attractor.CurrentMass) / distanceSquared);
                Vector2 gravitationalAcceleration = direction.normalized * gravitationScalar;

                attractee.PhysicsIntegrator.AddAcceleration(gravitationalAcceleration);

                if (gravitationalAcceleration.sqrMagnitude > MIN_THRESHOLD)
                {
                    if (!attractor.PlanetsImGravitating.Contains(attractee))
                    {
                        attractor.PlanetsImGravitating.Add(attractee);

                        if (attractor.CurrentMass >= attractee.CurrentMass)
                        {
                            if (EventManager.OnPlanetEnteredGravitationalPull != null)
                            {
                                Debug.Log("ENTER GRAVITATIONAL RANGE");
                                EventManager.OnPlanetEnteredGravitationalPull(attractor, attractee);
                            }
                        }
                    }
                }
                else if (attractor.PlanetsImGravitating.Contains(attractee))
                {
                    attractor.PlanetsImGravitating.Remove(attractee);

                    if (EventManager.OnPlanetExitedGravitationalPull != null)
                    {
                        if (attractor.CurrentMass >= attractee.CurrentMass)
                        {
                            Debug.Log("EXIT GRAVITATIONAL RANGE");
                            EventManager.OnPlanetExitedGravitationalPull(attractor, attractee);
                        }
                    }
                }
            }
            //Otherwise, absorb the smaller one!
            else
            {
                AbsorbObject(attractor, attractee);
            }
        }

        private void AbsorbObject(Planet attractor, Planet attractee)
        {
            Planet absorber;
            Planet absorbed;

            if (attractor.CurrentMass >= attractee.CurrentMass)
            {
                absorber = attractor;
                absorbed = attractee;
            }
            else
            {
                absorber = attractee;
                absorbed = attractor;
            }

            absorber.AbsorbPlanet(absorbed);
        }
    }
}
