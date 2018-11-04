using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class PlanetUnlockManager : ManagerBase<PlanetUnlockManager>
    {
        //Unlocked planets
        private List<Planet> unlockedPlanetPrefabs = new List<Planet>();

        private void Start()
        {
            InitStartingUnlockedPlanets();
        }

        private void UnlockPlanet(Planet planetPrefab)
        {
            if (!unlockedPlanetPrefabs.Contains(planetPrefab))
            {
                Debug.Log("Unlocking planet type: " + planetPrefab.PlanetProperties.PlanetName);
                unlockedPlanetPrefabs.Add(planetPrefab);

                if (EventManager.OnNewPlanetUnlocked != null)
                {
                    EventManager.OnNewPlanetUnlocked(planetPrefab.PlanetProperties.PlanetType);
                }
            }
        }

        private void InitStartingUnlockedPlanets()
        {
            Dictionary<Planet, PlanetProperties> allPlanetPrefabs = PlanetStoreManager.Instance.PlanetPrefabs;

            foreach (KeyValuePair<Planet, PlanetProperties> planetIndex in allPlanetPrefabs)
            {
                if (planetIndex.Value.IsUnlocked)
                {
                    UnlockPlanet(planetIndex.Key);
                }
            }
        }
    }
}
