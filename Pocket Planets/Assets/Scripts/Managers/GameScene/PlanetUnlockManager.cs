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
                child.PlanetProperties.PlanetType == EnumPlanetType.RING_PLANET)
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
