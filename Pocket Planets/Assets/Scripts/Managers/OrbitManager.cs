using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class OrbitManager : ManagerBase<OrbitManager>
    {
        private float orbitCount;

        [SerializeField] private Text orbitLabel;

        private void OnEnable()
        {
            EventManager.OnOrbitOccurred += TrackOrbit;
        }

        private void OnDisable()
        {
            EventManager.OnOrbitOccurred -= TrackOrbit;
        }

        private void TrackOrbit(Planet parent, Planet orbital)
        {
            //For now.
            ++orbitCount;
            orbitLabel.text = "Orbits: " + orbitCount.ToString();
        }
    }
}
