using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public enum EnumCameraState
    {
        CENTERED,       //Allows for planet spawning, but camera is anchored to center of universe
        FREE_ROAM,      //Allows for camera movement, but not planet spawning.
        ANCHORED        //Allows for planet spawning, but camera is anchored where it is.
    }
    
    public class CameraStateManager : ManagerBase<CameraStateManager>
    {
        [SerializeField] private EnumCameraState defaultCameraState;
        private EnumCameraState currentCameraState;

        public EnumCameraState CurrentCameraState { get { return currentCameraState; } }

        private void OnEnable()
        {
            currentCameraState = defaultCameraState;
        }

        //When the button is pressed, this function gets called
        private void SetStateToCameraCenter()
        {
            currentCameraState = EnumCameraState.CENTERED;

            if (EventManager.OnCameraCenterSelected != null)
            {
                EventManager.OnCameraCenterSelected();
            }
        }

        //When the button is pressed, this function gets called
        private void SetStateToCameraFreeroam()
        {
            currentCameraState = EnumCameraState.FREE_ROAM;

            if (EventManager.OnCameraFreeroamSelected != null)
            {
                EventManager.OnCameraFreeroamSelected();
            }
        }

        private void SetStateToCameraAnchored()
        {
            currentCameraState = EnumCameraState.ANCHORED;

            if (EventManager.OnCameraAnchoredSelected != null)
            {
                EventManager.OnCameraAnchoredSelected();
            }
        }
    }
}
