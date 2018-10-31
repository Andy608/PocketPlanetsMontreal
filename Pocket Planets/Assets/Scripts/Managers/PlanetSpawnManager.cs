using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class PlanetSpawnManager : ManagerBase<PlanetSpawnManager>
    {
        //The currently seleced planet to spawn.
        private Planet planetToSpawnPrefab;

        private void OnEnable()
        {
            EventManager.OnTapOccurred += SpawnPlanet;
        }

        private void OnDisable()
        {
            EventManager.OnTapOccurred -= SpawnPlanet;
        }

        private void SpawnPlanet(Touch touch)
        {
            if (InputManager.IsPointerOverUIObject()) return;

            Vector3 spawnPosition = new Vector3();
            DisplayManager.TouchPositionToWorldVector3(touch, ref spawnPosition);

            GameObject spawnedPlanet = Instantiate(planetToSpawnPrefab.gameObject, spawnPosition, Quaternion.identity);

            if (spawnedPlanet)
            {
                Planet newPlanet = spawnedPlanet.GetComponent<Planet>();

                if (EventManager.OnPlanetSpawned != null && newPlanet)
                {
                    EventManager.OnPlanetSpawned(newPlanet);
                }
            }
        }

        //Call this from the awake method in the PlanetStore with the default planet to spawn for now
        //Call this when a player selects a new planet from the gui menu
        public void SetPlanetToSpawn(EnumPlanetType planetType)
        {
            //Ask planet store for planet prefab by type
            planetToSpawnPrefab = PlanetStoreManager.Instance.GetPlanetPrefab(planetType);
        }

        public void SetPlanetToSpawn(Planet planetPrefab)
        {
            //Ask planet store for planet prefab by type
            planetToSpawnPrefab = planetPrefab;
        }
    }
}