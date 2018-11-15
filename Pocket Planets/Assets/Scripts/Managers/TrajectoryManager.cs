using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class TrajectoryManager : ManagerBase<TrajectoryManager>
    {
        private Planet currentPlanet;
        private Trajectory currentTrajectory;

        //Amount of time to look ahead.
        [SerializeField] private float offsetTime = 1.0f;

        private void Start()
        {
            currentTrajectory = null;
        }

        private void OnEnable()
        {
            EventManager.OnPlanetSpawning += SetCurrentPlanet;
            EventManager.OnPlanetAlive += RemoveCurrentPlanet;

            EventManager.OnDragBegan += UpdateStartingVelocity;
            EventManager.OnDragHeld += UpdateStartingVelocity;
            EventManager.OnDragEnded += UpdateStartingVelocity;
        }

        private void OnDisable()
        {
            EventManager.OnPlanetSpawning -= SetCurrentPlanet;
            EventManager.OnPlanetAlive -= RemoveCurrentPlanet;

            EventManager.OnDragBegan -= UpdateStartingVelocity;
            EventManager.OnDragHeld -= UpdateStartingVelocity;
            EventManager.OnDragEnded -= UpdateStartingVelocity;
        }

        private void SetCurrentPlanet(Planet planet)
        {
            if (!planet.PlanetProperties.IsAnchor)
            {
                //Debug.Log("Setting Current Planet: " + planet + " ID: " + planet.GetInstanceID());
                currentPlanet = planet;
                currentTrajectory = currentPlanet.GetComponent<Trajectory>();
            }
        }

        private void RemoveCurrentPlanet(Planet planet)
        {
            if (currentPlanet == planet)
            {
                RemoveCurrentPlanet();
            }
        }

        private void RemoveCurrentPlanet()
        {
            //Debug.Log("Removing Current Planet");
            Hide();
            currentPlanet = null;
            currentTrajectory = null;
        }

        public void Show()
        {
            if (currentTrajectory)
            {
                //Debug.Log("Showing Trajectory");
                currentTrajectory.Show();
            }
        }

        public void Hide()
        {
            if (currentTrajectory)
            {
                //Debug.Log("Hiding Trajectory");
                currentTrajectory.Hide();
            }
        }

        private void UpdateStartingVelocity(Touch touch)
        {
            if (currentPlanet)
            {
                Vector3 worldTouchPos = Vector3.zero;
                DisplayManager.TouchPositionToWorldVector3(touch, ref worldTouchPos);

                currentTrajectory.ClearTrajectory();
                currentTrajectory.CreateTrajectory(PlanetSpawnManager.Instance.CurrentSpawningPlanet.PhysicsIntegrator.InitialVelocity, offsetTime);
            }
        }
    }
}