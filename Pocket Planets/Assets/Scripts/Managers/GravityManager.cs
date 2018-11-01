using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class GravityManager : ManagerBase<GravityManager>
    {
        public static readonly float G = 66.7408f * 100.0f;//Heuristic
        private static readonly float MIN_THRESHOLD = 1.0f;

        private void Update()
        {
            List<Planet> planets = WorldPlanetTrackingManager.Instance.PlanetsInWorld;

            int i, j;
            for (i = 0; i < planets.Count; ++i)
            {
                Planet attractee = planets[i];

                if (attractee.PlanetState == EnumPlanetState.ALIVE)
                {
                    for (j = 0; j < planets.Count; ++j)
                    {
                        Planet attractor = planets[j];

                        if (attractor == attractee)
                        {
                            continue;
                        }
                        else if (attractor.PlanetState == EnumPlanetState.ANCHOR || 
                            attractor.PlanetState == EnumPlanetState.ALIVE)
                        {
                            //Pull attractee
                            AttractPlanet(attractor, attractee);
                        }
                    }
                }
            }
        }

        private void AttractPlanet(Planet attractor, Planet attractee)
        {
            Rigidbody2D attractorRB = attractor.PlanetRigidbody;
            Rigidbody2D attracteeRB = attractee.PlanetRigidbody;

            Vector2 direction = attractorRB.position - attracteeRB.position;
            float distanceSquared = direction.sqrMagnitude;
            float distanceFromCenter = attractor.Radius + attractee.Radius;

            //Debug.Log("Attracting");

            //If the planets are far enough away, then attract.
            if (distanceSquared > (distanceFromCenter * distanceFromCenter))
            {
                float gravitationScalar = (G * (attractorRB.mass * attracteeRB.mass) / distanceSquared);
                Vector2 gravitationalForce = direction.normalized * gravitationScalar;
                attracteeRB.AddForce(gravitationalForce);

                if (gravitationalForce.sqrMagnitude > MIN_THRESHOLD)
                {
                    if (!attractor.PlanetsImGravitating.Contains(attractee))
                    {
                        attractor.PlanetsImGravitating.Add(attractee);

                        if (attractor.PlanetRigidbody.mass >= attractee.PlanetRigidbody.mass)
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
                        if (attractor.PlanetRigidbody.mass >= attractee.PlanetRigidbody.mass)
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

            if (attractor.PlanetRigidbody.mass >= attractee.PlanetRigidbody.mass)
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
