using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EnumScene
{
    TITLE,
    GAME
}

namespace Managers
{
    public class PocketPlanetSceneManager : ManagerBase<PocketPlanetSceneManager>
    {
        private EnumScene currentScene;
        private EnumScene targetScene;

        public EnumScene CurrentScene { get { return currentScene; } }

        private void Awake()
        {
            currentScene = (EnumScene)SceneManager.GetActiveScene().buildIndex;
            Debug.Log("CURRENT SCENE: " + currentScene);
        }

        private void OnEnable()
        {
            EventManager.OnSetTargetScene += SetTargetScene;
            EventManager.OnRequestSceneChange += ChangeScene;
        }

        private void OnDisable()
        {
            EventManager.OnSetTargetScene -= SetTargetScene;
            EventManager.OnRequestSceneChange -= ChangeScene;
        }

        private void SetTargetScene(EnumScene scene)
        {
            targetScene = scene;
        }

        private void ChangeScene()
        {
            if (currentScene == targetScene) return;
            SceneManager.LoadScene((int)targetScene);
        }
    }
}