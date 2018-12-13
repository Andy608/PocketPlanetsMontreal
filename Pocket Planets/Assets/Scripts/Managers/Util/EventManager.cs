using System;
using UnityEngine;

namespace Managers
{
    public class EventManager : ManagerBase<EventManager>
    {
        //  Input Events
        //////////////////////////////////////////////////////////////
        public delegate void TapOccurred(Vector3 t);
        public static TapOccurred OnTapOccurred;

        public delegate void ScrollOccurred(float direction);
        public static ScrollOccurred OnScrollOccurred;

        public delegate void DragBegan(Vector3 t);
        public static DragBegan OnDragBegan;

        public delegate void DragHeld(Vector3 t);
        public static DragHeld OnDragHeld;

        public delegate void DragEnd(Vector3 t);
        public static DragEnd OnDragEnded;

        public delegate void MiddleMouseDragBegan(Vector3 t);
        public static MiddleMouseDragBegan OnMiddleMouseDragBegan;

        public delegate void MiddleMouseDragHeld(Vector3 t);
        public static MiddleMouseDragHeld OnMiddleMouseDragHeld;

        public delegate void MiddleMouseDragEnd(Vector3 t);
        public static MiddleMouseDragEnd OnMiddleMouseDragEnded;

        public delegate void PinchBegan(Touch first, Touch second);
        public static PinchBegan OnPinchBegan;

        public delegate void PinchHeld(Touch first, Touch second);
        public static PinchHeld OnPinchHeld;

        public delegate void PinchEnd(Touch first, Touch second);
        public static PinchEnd OnPinchEnded;
        //////////////////////////////////////////////////////////////

        //  Camera Events
        //////////////////////////////////////////////////////////////
        public delegate void PanCamera(Vector3 dragDistance);
        public static PanCamera OnPanCamera;

        //////////////////////////////////////////////////////////////

        //  Planet Lifetime Events
        //////////////////////////////////////////////////////////////
        public delegate void PlanetSpawning(Planet spawningPlanet);
        public static PlanetSpawning OnPlanetSpawning;

        public delegate void PlanetAlive(Planet alivePlanet);
        public static PlanetAlive OnPlanetAlive;

        public delegate void PlanetSpawned(Planet spawnedPlanet);
        public static PlanetSpawned OnPlanetSpawned;

        public delegate void PlanetUpgraded(Planet upgradedPlanet, Planet originalPlanet);
        public static PlanetUpgraded OnPlanetUpgraded;

        public delegate void PlanetDestroyed(Planet destroyedPlanet);
        public static PlanetDestroyed OnPlanetDestroyed;
        //////////////////////////////////////////////////////////////


        //  Special Game Events
        //////////////////////////////////////////////////////////////
        public delegate void PlanetAbsorbed(Planet absorber, Planet absorbed);
        public static PlanetAbsorbed OnPlanetAbsorbed;

        public delegate void PlanetTheoreticallyAbsorbed(Planet absorber, Planet absorbed);
        public static PlanetTheoreticallyAbsorbed OnPlanetTheoreticallyAbsorbed;

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

        public delegate void PlanetSpawnDenied(Vector3 touchPos);
        public static PlanetSpawnDenied OnPlanetSpawnDenied;
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

        public delegate void GoToNextPlanetSelected();
        public static GoToNextPlanetSelected OnGoToNextPlanetSelected;

        public delegate void CameraFreeroamSelected();
        public static CameraFreeroamSelected OnCameraFreeroamSelected;

        public delegate void CameraAnchoredSelected();
        public static CameraAnchoredSelected OnCameraAnchoredSelected;

        public delegate void CameraFollowSelected();
        public static CameraFollowSelected OnCameraFollowSelected;

        public delegate void ButtonPressed();
        public static ButtonPressed OnButtonPressed;

        public delegate void DeletePlanetButtonSelected();
        public static DeletePlanetButtonSelected OnDeletePlanetButtonSelected;

        public delegate void SettingsButtonSelected();
        public static SettingsButtonSelected OnSettingsButtonSelected;
        //////////////////////////////////////////////////////////////

        //  Audio Events
        //////////////////////////////////////////////////////////////
        public delegate void PlayMusicRequested();
        public static PlayMusicRequested OnPlayMusicRequested;

        public delegate void StopMusicRequested();
        public static StopMusicRequested OnStopMusicRequested;

        public delegate void MusicVolumeChanged(float value);
        public static MusicVolumeChanged OnMusicVolumeChanged;

        public delegate void SoundVolumeChanged(float value);
        public static SoundVolumeChanged OnSoundVolumeChanged;

        public delegate void PlanetTapOccured();
        public static PlanetTapOccured OnPlanetTapOccured;

        public delegate void PlanetSwipeOccured();
        public static PlanetSwipeOccured OnPlanetSwipeOccured;
        //////////////////////////////////////////////////////////////

        public void OnButtonClicked()
        {
            if (OnButtonPressed != null)
            {
                OnButtonPressed();
            }
        }
    }
}
