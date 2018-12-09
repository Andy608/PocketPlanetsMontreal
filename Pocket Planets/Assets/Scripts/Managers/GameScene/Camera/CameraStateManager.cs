using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public enum EnumCameraState
    {
        FREE_ROAM,      //Allows for camera movement, but not planet spawning.
        ANCHORED,       //Allows for planet spawning, but camera is anchored where it is.
        //FOLLOW          //Follows planet the player clicks on
    }
    
    public class CameraStateManager : ManagerBase<CameraStateManager>
    {
        [SerializeField] private EnumCameraState defaultCameraState;
        private EnumCameraState currentCameraState;

        public EnumCameraState CurrentCameraState { get { return currentCameraState; } }

        private void OnEnable()
        {
            currentCameraState = defaultCameraState;
            EventManager.OnCameraFreeroamSelected += SetStateToCameraFreeroam;
            EventManager.OnCameraAnchoredSelected += SetStateToCameraAnchored;
        }

        //When the button is pressed, this function gets called
        private void SetStateToCameraFreeroam()
        {
            currentCameraState = EnumCameraState.FREE_ROAM;
        }

        private void SetStateToCameraAnchored()
        {
            currentCameraState = EnumCameraState.ANCHORED;
        }

        //public void SetStateToCameraFollow()
        //{
        //    currentCameraState = EnumCameraState.FOLLOW;

        //    if (EventManager.OnCameraFollowSelected != null)
        //    {
        //        EventManager.OnCameraFollowSelected();
        //    }
        //}
    }
}
