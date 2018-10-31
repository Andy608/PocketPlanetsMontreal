using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class PlanetStoreManager : ManagerBase<PlanetStoreManager>
    {
        //List of all the planet prefabs
        [SerializeField] private Planet unknownPlanetPrefab;
        [SerializeField] private List<Planet> planetPrefabs = new List<Planet>();

        private void Awake()
        {
            if (planetPrefabs.Count > 0)
            {
                PlanetSpawnManager.Instance.SetPlanetToSpawn(planetPrefabs[0]);
            }
            else
            {
                Debug.Log("ERROR. PLANET PREFAB LIST IS EMPTY.");
                PlanetSpawnManager.Instance.SetPlanetToSpawn(null);
            }
        }

        public Planet GetPlanetPrefab(EnumPlanetType planetType)
        {
            foreach (Planet planet in planetPrefabs)
            {
                if (planet.PlanetProperties.PlanetType == planetType)
                {
                    return planet;
                }
            }

            Debug.Log("ERROR. NO PLANET WITH TYPE: " + planetType.ToString());
            return null;
        }
    }
}