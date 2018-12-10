using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EnumScene
{
    TITLE,
    GAME,
    TUTORIAL
}

namespace Managers
{
    public class PocketPlanetSceneManager : ManagerBase<PocketPlanetSceneManager>
    {
        private EnumScene currentScene;
        private EnumScene targetScene;

        public EnumScene CurrentScene { get { return currentScene; } }
        public EnumScene TargetScene { get { return targetScene; } }

        private void Awake()
        {
            currentScene = (EnumScene)SceneManager.GetActiveScene().buildIndex;
            targetScene = currentScene;
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