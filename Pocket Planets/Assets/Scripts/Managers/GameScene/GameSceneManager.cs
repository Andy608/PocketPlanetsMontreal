using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class GameSceneManager : ManagerBase<GameSceneManager>
    {
        private void OnEnable()
        {
            PlanetSpawnManager.Instance.SpawnPlanet(EnumPlanetType.BLACKHOLE, Vector2.zero);
        }
    }
}