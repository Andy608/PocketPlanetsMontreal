using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class GravityManager : ManagerBase<GravityManager>
    {
        public static readonly float G = 6.67408f * 1000.0f;//Heuristic
        private static readonly float MIN_THRESHOLD = 1.0f;

        public void IntegrateGravity()
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

        public void IntegrateTheoreticalGravity()
        {
            List<Planet> planets = WorldPlanetTrackingManager.Instance.PlanetsInWorld;

            int i, j;
            for (i = 0; i < planets.Count; ++i)
            {
                Planet attractee = planets[i];

                if (attractee.PlanetState != EnumPlanetState.DEAD && !attractee.PlanetProperties.IsAnchor)
                {
                    for (j = 0; j < planets.Count; ++j)
                    {
                        Planet attractor = planets[j];

                        if (attractor == attractee)
                        {
                            continue;
                        }
                        else if (attractor.PlanetState != EnumPlanetState.DEAD)
                        {
                            //Pull attractee
                            AttractTheoreticalPlanet(attractor, attractee);
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

        private void AttractTheoreticalPlanet(Planet attractor, Planet attractee)
        {
            Vector2 direction = attractor.PhysicsIntegrator.TheoreticalPosition - attractee.PhysicsIntegrator.TheoreticalPosition;
            float distanceSquared = direction.sqrMagnitude;
            float distanceFromCenter = attractor.Radius + attractee.Radius;

            //If the planets are far enough away, then attract.
            if (distanceSquared > (distanceFromCenter * distanceFromCenter))
            {
                float gravitationScalar = (G * (attractor.CurrentMass) / distanceSquared);
                Vector2 gravitationalAcceleration = direction.normalized * gravitationScalar;

                attractee.PhysicsIntegrator.AddTheoreticalAcceleration(gravitationalAcceleration);
            }
            //Otherwise, absorb the smaller one!
            else
            {
                TheoreticallyAbsorbObject(attractor, attractee);
            }
        }

        private void TheoreticallyAbsorbObject(Planet absorber, Planet absorbed)
        {
            GetAbsorberAndAbsorbed(ref absorber, ref absorbed);

            if (EventManager.OnPlanetTheoreticallyAbsorbed != null)
            {
                EventManager.OnPlanetTheoreticallyAbsorbed(absorber, absorbed);
            }
        }

        private void AbsorbObject(Planet absorber, Planet absorbed)
        {
            //Planet absorber;
            //Planet absorbed;

            //if (attractor.CurrentMass >= attractee.CurrentMass)
            //{
            //    absorber = attractor;
            //    absorbed = attractee;
            //}
            //else
            //{
            //    absorber = attractee;
            //    absorbed = attractor;
            //}

            GetAbsorberAndAbsorbed(ref absorber, ref absorbed);
            absorber.AbsorbPlanet(absorbed);
        }

        private void GetAbsorberAndAbsorbed(ref Planet absorber, ref Planet absorbed)
        {
            if (absorber.CurrentMass < absorbed.CurrentMass)
            {
                Planet temp = absorber;
                absorber = absorbed;
                absorbed = temp;
            }
        }
    }
}
