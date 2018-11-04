using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Managers
{
    public class TitleSceneManager : ManagerBase<TitleSceneManager>
    {
        //Once a planet is absorbed into this, the game starts.
        private Planet anchorGoal;

        private void OnEnable()
        {
            anchorGoal = PlanetSpawnManager.Instance.SpawnPlanet(EnumPlanetType.BLACKHOLE, new Vector2(0, -50));

            EventManager.OnPlanetAbsorbed += HandleAbsorbed;
        }

        private void OnDisable()
        {
            EventManager.OnPlanetAbsorbed -= HandleAbsorbed;
        }

        private void HandleAbsorbed(Planet absorber, Planet absorbed)
        {
            if (absorber == anchorGoal)
            {
                if (EventManager.OnStartFadeOut != null)
                {
                    EventManager.OnStartFadeOut();
                }

                if (EventManager.OnSetTargetScene != null)
                {
                    EventManager.OnSetTargetScene(EnumScene.GAME);
                }
            }
        }
    }
}