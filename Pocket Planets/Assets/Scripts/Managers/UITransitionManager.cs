using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class UITransitionManager : ManagerBase<UITransitionManager>
    {
        [SerializeField] private Animator uiAnimator;

        private void OnEnable()
        {
            EventManager.OnStartFadeOut += FadeOut;
        }

        private void OnDisable()
        {
            EventManager.OnStartFadeOut -= FadeOut;
        }

        public void OnFadeOutComplete()
        {
            Debug.Log("CHANGE SCENE!");

            if (EventManager.OnRequestSceneChange != null)
            {
                EventManager.OnRequestSceneChange();
            }
        }

        private void FadeOut()
        {
            //Trigger fade out.
            uiAnimator.SetTrigger("FadeOut");
        }
    }
}
