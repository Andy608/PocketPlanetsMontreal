using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class PlanetUnlockManager : ManagerBase<PlanetUnlockManager>
    {
        //Unlocked planets
        private List<Planet> unlockedPlanetPrefabs = new List<Planet>();

        private static int TERRESTRIAL_PLANET_ORBIT_COUNT = 1;
        private static int SECONDS_BEFORE_COMET_SPAWNS = 10;
        private float cometSecondsCounter = 0;
        private bool cometUnlocked = false;

        private Planet cometSpawn = null;

        private IEnumerator cometSpawnCoroutine;

        private void Start()
        {
            InitStartingUnlockedPlanets();
        }

        private void OnEnable()
        {
            EventManager.OnPlanetAbsorbed += HandlePlanetAbsorb;
            EventManager.OnOrbitOccurred += HandleOrbit;
        }

        private void OnDisable()
        {
            EventManager.OnPlanetAbsorbed -= HandlePlanetAbsorb;
            EventManager.OnOrbitOccurred -= HandleOrbit;
        }

        private IEnumerator CometSpawn()
        {
            while (cometSecondsCounter < SECONDS_BEFORE_COMET_SPAWNS || GameStateManager.Instance.CurrentGameState == GameStateManager.EnumGameState.PAUSED)
            {
                yield return new WaitForFixedUpdate();
                cometSecondsCounter += Time.fixedDeltaTime;
            }

            //Spawn comet and fly it across the screen
            //Debug.Log("SPAWN COMET");

            Vector2 spawnPosition = new Vector2(DisplayManager.Instance.MaxCameraWidth, Random.Range(-DisplayManager.Instance.MaxCameraHeight / 2.0f, DisplayManager.Instance.MaxCameraHeight / 2.0f));
            cometSpawn = PlanetSpawnManager.Instance.SpawnPlanet(EnumPlanetType.COMET, spawnPosition);
            cometSpawn.InitialVelocity = new Vector2(-200.0f, 0.0f);
            cometSpawn.PhysicsIntegrator.InitialVelocity = cometSpawn.InitialVelocity;
            cometSpawnCoroutine = null;
        }

        private void Update()
        {
            if (!cometUnlocked)
            {
                if (cometSpawn && DisplayManager.Instance.IsInView(cometSpawn.transform.position))
                {
                    Planet comet = PlanetStoreManager.Instance.GetPlanetPrefab(EnumPlanetType.COMET);

                    //Unlock comet!
                    if (!comet.PlanetProperties.IsUnlocked && !unlockedPlanetPrefabs.Contains(comet))
                    {
                        UnlockPlanet(comet);
                    }

                    cometUnlocked = true;
                }
            }
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

                if (cometSpawnCoroutine != null)
                {
                    StopCoroutine(cometSpawnCoroutine);
                    cometSpawnCoroutine = null;
                }

                if (!unlockedPlanetPrefabs.Contains(PlanetStoreManager.Instance.GetPlanetPrefab(EnumPlanetType.COMET)))
                {
                    cometSpawnCoroutine = CometSpawn();
                    StartCoroutine(cometSpawnCoroutine);
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

        private void HandlePlanetAbsorb(Planet absorber, Planet absorbee)
        {
            //Unlock Terrestrial Planet
            if (absorber.PlanetProperties.PlanetType == EnumPlanetType.ASTEROID)
            {
                Planet terrestrialPlanet = PlanetStoreManager.Instance.GetPlanetPrefab(EnumPlanetType.TERRESTRIAL_PLANET);

                if (absorber.CurrentMass >= terrestrialPlanet.PlanetProperties.DefaultMass)
                {
                    if (!terrestrialPlanet.PlanetProperties.IsUnlocked && !unlockedPlanetPrefabs.Contains(terrestrialPlanet))
                    {
                        UnlockPlanet(terrestrialPlanet);
                    }
                }
            }
            //Unlock Star
            else if (absorber.PlanetProperties.PlanetType == EnumPlanetType.GAS_PLANET)
            {
                Planet star = PlanetStoreManager.Instance.GetPlanetPrefab(EnumPlanetType.STAR);

                if (absorber.CurrentMass >= star.PlanetProperties.DefaultMass)
                {
                    if (!star.PlanetProperties.IsUnlocked && !unlockedPlanetPrefabs.Contains(star))
                    {
                        UnlockPlanet(star);
                    }
                }
            }
            //Unlock Super Giant
            else if (absorber.PlanetProperties.PlanetType == EnumPlanetType.STAR)
            {
                Planet superGiant = PlanetStoreManager.Instance.GetPlanetPrefab(EnumPlanetType.SUPERGIANT);

                if (absorber.CurrentMass >= superGiant.PlanetProperties.DefaultMass)
                {
                    if (!superGiant.PlanetProperties.IsUnlocked && !unlockedPlanetPrefabs.Contains(superGiant))
                    {
                        UnlockPlanet(superGiant);
                    }
                }
            }
            else if (absorber.PlanetProperties.PlanetType == EnumPlanetType.TERRESTRIAL_PLANET)
            {
                Planet ringPlanet = PlanetStoreManager.Instance.GetPlanetPrefab(EnumPlanetType.RING_PLANET);

                if (absorber.AsteroidCollisionCounter >= 5)
                {
                    if (!unlockedPlanetPrefabs.Contains(ringPlanet))
                    {
                        if (!ringPlanet.PlanetProperties.IsUnlocked)
                        {
                            UnlockPlanet(ringPlanet);
                        }

                        absorber.UpgradePlanet(EnumPlanetType.RING_PLANET);
                    }
                }
            }
        }
        
        private void HandleOrbit(OrbitData orbitData)
        {
            Planet parent = orbitData.OrbitParent;
            Planet child = orbitData.OrbitChild;

            //Unlock Gas Planet
            if (child.PlanetProperties.PlanetType == EnumPlanetType.TERRESTRIAL_PLANET ||
                parent.PlanetProperties.PlanetType == EnumPlanetType.TERRESTRIAL_PLANET)
            {
                if (orbitData.OrbitCount >= TERRESTRIAL_PLANET_ORBIT_COUNT)
                {
                    Planet gasPlanet = PlanetStoreManager.Instance.GetPlanetPrefab(EnumPlanetType.GAS_PLANET);

                    if (!gasPlanet.PlanetProperties.IsUnlocked && !unlockedPlanetPrefabs.Contains(gasPlanet))
                    {
                        UnlockPlanet(gasPlanet);
                    }
                }
            }
        }
    }
}
