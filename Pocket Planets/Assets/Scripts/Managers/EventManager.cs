using System;
using UnityEngine;

namespace Managers
{
    public class EventManager : ManagerBase<EventManager>
    {
        //  Input Events
        //////////////////////////////////////////////////////////////
        public delegate void TapOccurred(Touch t);
        public static TapOccurred OnTapOccurred;

        public delegate void DragBegan(Touch t);
        public static DragBegan OnDragBegan;

        public delegate void DragHeld(Touch t);
        public static DragHeld OnDragHeld;

        public delegate void DragEnd(Touch t);
        public static DragEnd OnDragEnded;

        public delegate void PinchBegan(Touch first, Touch second);
        public static PinchBegan OnPinchBegan;

        public delegate void PinchHeld(Touch first, Touch second);
        public static PinchHeld OnPinchHeld;

        public delegate void PinchEnd(Touch first, Touch second);
        public static PinchEnd OnPinchEnded;
        //////////////////////////////////////////////////////////////

        
        //  Planet Lifetime Events
        //////////////////////////////////////////////////////////////
        public delegate void PlanetSpawned(Planet spawnedPlanet);
        public static PlanetSpawned OnPlanetSpawned;

        public delegate void PlanetDestroyed(Planet destroyedPlanet);
        public static PlanetDestroyed OnPlanetDestroyed;
        //////////////////////////////////////////////////////////////


        //  Special Game Events
        //////////////////////////////////////////////////////////////
        public delegate void PlanetAbsorbed(Planet absorber, Planet absorbed);
        public static PlanetAbsorbed OnPlanetAbsorbed;

        public delegate void OrbitOccurred(OrbitData orbitData);
        public static OrbitOccurred OnOrbitOccurred;

        public delegate void PlanetEnteredGravitationalPull(Planet absorber, Planet absorbed);
        public static PlanetEnteredGravitationalPull OnPlanetEnteredGravitationalPull;

        public delegate void PlanetExitedGravitationalPull(Planet parent, Planet orbital);
        public static PlanetExitedGravitationalPull OnPlanetExitedGravitationalPull;

        public delegate void PlanetCollapsed(Planet planet);
        public static PlanetCollapsed OnPlanetCollapsed;

        public delegate void NewPlanetUnlocked(EnumPlanetType planetType);
        public static NewPlanetUnlocked OnNewPlanetUnlocked;

        public delegate void PlanetToSpawnChanged(EnumPlanetType planetType);
        public static PlanetToSpawnChanged OnPlanetToSpawnChanged;
        //////////////////////////////////////////////////////////////


        //  Scene Events
        //////////////////////////////////////////////////////////////
        public delegate void SetTargetScene(EnumScene targetScene);
        public static SetTargetScene OnSetTargetScene;

        public delegate void RequestSceneChange();
        public static RequestSceneChange OnRequestSceneChange;

        public delegate void StartFadeOut();
        public static StartFadeOut OnStartFadeOut;
        //////////////////////////////////////////////////////////////

        //  Game State Events
        //////////////////////////////////////////////////////////////
        public delegate void GamePaused();
        public static GamePaused OnGamePaused;

        public delegate void GameUnpaused();
        public static GameUnpaused OnGameUnpaused;
        //////////////////////////////////////////////////////////////


        //  Money Events
        //////////////////////////////////////////////////////////////
        public delegate void MoneyChanged(Money money);
        public static MoneyChanged OnMoneyChanged;
        //////////////////////////////////////////////////////////////


        //  UI Events
        //////////////////////////////////////////////////////////////
        public delegate void CloseUnlockNotification();
        public static CloseUnlockNotification OnCloseUnlockNotification;

        public delegate void OpenPlanetUIList();
        public static OpenPlanetUIList OnOpenPlanetUIList;
        //////////////////////////////////////////////////////////////
    }
}
