﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class PlanetSpawnManager : ManagerBase<PlanetSpawnManager>
    {
        //The currently seleced planet to spawn.
        private Planet planetToSpawnPrefab;
        [SerializeField] private Transform worldParent;

        private static Vector3 spawnPosition = new Vector3();
        private static Vector3 dragPosition = new Vector3();

        private Planet currentSpawningPlanet;

        private void OnEnable()
        {
            EventManager.OnTapOccurred += SpawnTapOccurred;
            EventManager.OnDragBegan += SpawnDragBegan;
            EventManager.OnDragHeld += SpawnDragHeld;
            EventManager.OnDragEnded += SpawnDragEnded;
        }

        private void OnDisable()
        {
            EventManager.OnTapOccurred -= SpawnTapOccurred;
            EventManager.OnDragBegan -= SpawnDragBegan;
            EventManager.OnDragHeld -= SpawnDragHeld;
            EventManager.OnDragEnded -= SpawnDragEnded;
        }

        private void SpawnTapOccurred(Touch touch)
        {
            currentSpawningPlanet = SpawnPlanet(touch);

            if (currentSpawningPlanet)
            {
                currentSpawningPlanet.SetPlanetState(EnumPlanetState.ALIVE);
            }

            currentSpawningPlanet = null;
        }

        private void SpawnDragBegan(Touch touch)
        {
            currentSpawningPlanet = SpawnPlanet(touch);

            if (currentSpawningPlanet)
            {
                DisplayManager.TouchPositionToWorldVector3(touch, ref dragPosition);
                currentSpawningPlanet.SetPlanetState(EnumPlanetState.SPAWNING);
                currentSpawningPlanet.PlanetTrajectory.Show();
            }
        }

        private void SpawnDragHeld(Touch touch)
        {
            if (currentSpawningPlanet)
            {
                DisplayManager.TouchPositionToWorldVector3(touch, ref dragPosition);
            }
        }

        private void SpawnDragEnded(Touch touch)
        {
            if (currentSpawningPlanet)
            {
                DisplayManager.TouchPositionToWorldVector3(touch, ref dragPosition);
                currentSpawningPlanet.SetPlanetState(EnumPlanetState.ALIVE);

                currentSpawningPlanet.InitialVelocity = (currentSpawningPlanet.transform.position - dragPosition) * (DisplayManager.Instance.DefaultCameraSize / DisplayManager.Instance.CurrentCameraSize);
                //Remove the lines.
                //Set the velocity to the distance multiplied by a scale factor that works for the game.
                currentSpawningPlanet.PlanetRigidbody.velocity += currentSpawningPlanet.InitialVelocity;
                currentSpawningPlanet.PlanetTrajectory.Hide();
            }

            currentSpawningPlanet = null;
        }

        private Planet SpawnPlanet(Touch touch)
        {
            if (InputManager.IsPointerOverUIObject()) return null;
            Debug.Log("SPAWNING");

            DisplayManager.TouchPositionToWorldVector3(touch, ref spawnPosition);

            GameObject spawnedPlanet = Instantiate(planetToSpawnPrefab.gameObject, spawnPosition, Quaternion.identity);

            if (spawnedPlanet)
            {
                spawnedPlanet.transform.SetParent(worldParent);

                Planet newPlanet = spawnedPlanet.GetComponent<Planet>();
                newPlanet.SetPlanetState(EnumPlanetState.SPAWNING);

                if (EventManager.OnPlanetSpawned != null && newPlanet)
                {
                    EventManager.OnPlanetSpawned(newPlanet);
                }

                return newPlanet;
            }

            return null;
        }

        public void SpawnPlanetUpgrade(Planet planet)
        {
            Planet upgradedPrefab = PlanetStoreManager.Instance.GetPlanetPrefab(planet.PlanetProperties.UpgradedPlanetType);

            //The upgrade is not in the list.
            if (!upgradedPrefab)
            {
                return;
            }

            GameObject upgradedPlanet = Instantiate(upgradedPrefab.gameObject, planet.transform);

            if (upgradedPlanet)
            {
                upgradedPlanet.transform.SetParent(worldParent);

                //Spawn in the new planet
                Planet newPlanet = upgradedPlanet.GetComponent<Planet>();
                newPlanet.InitialVelocity = planet.PlanetRigidbody.velocity;
                newPlanet.PlanetRigidbody.velocity = newPlanet.InitialVelocity;
                newPlanet.SetPlanetState(EnumPlanetState.ALIVE);

                if (EventManager.OnPlanetSpawned != null && newPlanet)
                {
                    EventManager.OnPlanetSpawned(newPlanet);
                }

                if (EventManager.OnPlanetUpgraded != null)
                {
                    EventManager.OnPlanetUpgraded(newPlanet);
                }

                //Destroy the old planet
                if (EventManager.OnPlanetDestroyed != null)
                {
                    EventManager.OnPlanetDestroyed(planet);
                }

                Destroy(planet.gameObject);
            }
        }

        public Planet SpawnPlanet(EnumPlanetType planetType, Vector2 position)
        {
            Planet prefab = PlanetStoreManager.Instance.GetPlanetPrefab(planetType);

            GameObject spawnedPlanet = Instantiate(prefab.gameObject, position, Quaternion.identity);

            if (spawnedPlanet)
            {
                spawnedPlanet.transform.SetParent(worldParent);

                Planet newPlanet = spawnedPlanet.GetComponent<Planet>();
                newPlanet.SetPlanetState(EnumPlanetState.ALIVE);

                if (EventManager.OnPlanetSpawned != null && newPlanet)
                {
                    EventManager.OnPlanetSpawned(newPlanet);
                }

                return newPlanet;
            }

            return null;
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