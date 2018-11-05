using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class DrawOrderManager : ManagerBase<DrawOrderManager>
    {
        private const string PLANET_SORTING_LAYER = "Planet";
        private const string TRAIL_SORTING_LAYER = "Trail";

        private void OnEnable()
        {
            EventManager.OnPlanetSpawned += UpdateDrawOrder;
        }

        private void OnDisable()
        {
            EventManager.OnPlanetSpawned -= UpdateDrawOrder;
        }

        private void UpdateDrawOrder(Planet newPlanet)
        {
            //Set the new objects draw layer to the space object draw layer
            newPlanet.PlanetObject.GetComponent<SpriteRenderer>().sortingLayerName = PLANET_SORTING_LAYER;
            newPlanet.PlanetTrail.PlanetTrailerRenderer.sortingLayerName = TRAIL_SORTING_LAYER;

            //Get list of active objects in universe.
            List<Planet> activePlanets = WorldPlanetTrackingManager.Instance.PlanetsInWorld;

            //Sort by max mass first
            SortObjectsByMass(ref activePlanets);

            //Testing sort
            printArray(activePlanets);

            //set the draw order to the index of the list
            int currentIndex = 0;
            for (; currentIndex < activePlanets.Count; ++currentIndex)
            {
                Planet currentPlanet = activePlanets[currentIndex];
                currentPlanet.PlanetObject.GetComponent<SpriteRenderer>().sortingOrder = currentIndex;
                currentPlanet.PlanetTrail.PlanetTrailerRenderer.sortingOrder = activePlanets.Count - 1 - currentIndex;
            }
        }

        private void SortObjectsByMass(ref List<Planet> planets)
        {
            //If we run into performance issues, then we'll look to optimize this algorithm.

            if (planets.Count > 1)
            {
                //Debug.Log("SORTING");
                int currentIndex;
                int otherIndex;
                int maxIndex;

                for (currentIndex = 0; currentIndex < planets.Count - 1; ++currentIndex)
                {
                    maxIndex = currentIndex;

                    for (otherIndex = currentIndex + 1; otherIndex < planets.Count; ++otherIndex)
                    {
                        if (planets[otherIndex].PlanetRigidbody.mass < planets[currentIndex].PlanetRigidbody.mass)
                        {
                            maxIndex = otherIndex;
                        }
                    }

                    //Swap
                    //Debug.Log("SWAPPING");
                    Planet tempIndex = planets[maxIndex];
                    planets[maxIndex] = planets[currentIndex];
                    planets[currentIndex] = tempIndex;
                }
            }
        }

        // Prints the array 
        static void printArray(List<Planet> arr)
        {
            string s = "";
            int n = arr.Count;
            for (int i = 0; i < n; ++i)
                s += ("[" + arr[i].PlanetRigidbody.mass.ToString() + "] ");

            //Debug.Log(s);
        }
    }
}

