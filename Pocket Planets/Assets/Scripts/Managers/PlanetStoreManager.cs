using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class PlanetStoreManager : ManagerBase<PlanetStoreManager>
    {
        //List of all the planet prefabs
        [SerializeField] private Planet unknownPlanetPrefab;
        [SerializeField] private List<Planet> userPlanetPrefabs = new List<Planet>();
        private Dictionary<Planet, PlanetProperties> planetPrefabList = new Dictionary<Planet, PlanetProperties>();

        public Dictionary<Planet, PlanetProperties> PlanetPrefabs { get { return planetPrefabList; } }

        private void OnEnable()
        {
            PopulatePlanetList();

            PlanetSpawnManager.Instance.SetPlanetToSpawn(EnumPlanetType.ASTEROID);
            //if (planetPrefabs.Count > 0)
            //{
            //    PlanetSpawnManager.Instance.SetPlanetToSpawn(planetPrefabs[0]);
            //}
            //else
            //{
            //    Debug.Log("ERROR. PLANET PREFAB LIST IS EMPTY.");
            //    PlanetSpawnManager.Instance.SetPlanetToSpawn(null);
            //}
        }

        public Planet GetPlanetPrefab(EnumPlanetType planetType)
        {
            foreach (KeyValuePair<Planet, PlanetProperties> planetIndex in planetPrefabList)
            {
                if (planetIndex.Value.PlanetType == planetType)
                {
                    return planetIndex.Key;
                }
            }

            Debug.Log("ERROR. NO PLANET WITH TYPE: " + planetType.ToString());
            return null;
        }

        private void PopulatePlanetList()
        {
            Debug.Log("POPULATING PREFAB LIST");
            foreach (Planet planet in userPlanetPrefabs)
            {
                PlanetProperties newProperties = Instantiate(planet.PlanetProperties);
                planetPrefabList.Add(planet, newProperties);
            }
        }
    }
}