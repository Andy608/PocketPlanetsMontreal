using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Managers
{
    public class OrbitManager : ManagerBase<OrbitManager>
    {
        private float orbitCount;

        [SerializeField] private TextMeshProUGUI orbitLabel;

        private void OnEnable()
        {
            EventManager.OnOrbitOccurred += TrackOrbit;
        }

        private void OnDisable()
        {
            EventManager.OnOrbitOccurred -= TrackOrbit;
        }

        private void TrackOrbit(OrbitData orbitData)
        {
            //For now.
            ++orbitCount;
            orbitLabel.text = "Orbits: " + orbitCount.ToString();
        }
    }
}
