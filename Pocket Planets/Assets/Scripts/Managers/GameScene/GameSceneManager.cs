using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class GameSceneManager : ManagerBase<GameSceneManager>
    {
        private void OnEnable()
        {
            //PlanetSpawnManager.Instance.SpawnPlanet(EnumPlanetType.BLACKHOLE, Vector2.zero);
        }

        public void OnExitClicked()
        {
            if (EventManager.OnStartFadeOut != null)
            {
                EventManager.OnStartFadeOut();
            }

            if (EventManager.OnSetTargetScene != null)
            {
                EventManager.OnSetTargetScene(EnumScene.TITLE);
            }
        }
    }
}