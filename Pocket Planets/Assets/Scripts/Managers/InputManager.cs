using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Managers
{
    public class InputManager : ManagerBase<InputManager>
    {
        private bool dragRecognized = false;
        private bool pinchRecognized = false;
        private bool tapRecognized = false;

        //Max time for one finger to be on the screen before a second one is.
        //If there is only one finger on the screen afterthis time, or the touch is removed before this time, it is a tap.
        //If there is two fingers on the screen after this time, it is a pinch.
        private float minThresholdTime = 0.1f;
        private float minThresholdCounter = 0.0f;
        private bool startMinThresholdCount = false;

        //If drag movement is greater than this, then drag event.
        private float minDragMovement = 20.0f;
        private Vector2 dragMovement;

        private void Update()
        {
            Touch firstTouch;
            Touch secondTouch;

            //Possible drag/pinch/tap
            if (Input.touchCount > 0)
            {
                firstTouch = Input.touches[0];

                if (firstTouch.phase == TouchPhase.Began)
                {
                    startMinThresholdCount = true;
                    dragMovement = Vector2.zero;
                }

                if (startMinThresholdCount)
                {
                    //Increment timer for possible pinch began
                    minThresholdCounter += Time.deltaTime;
                }

                //Possible pinch begin/held
                if (Input.touchCount > 1)
                {
                    secondTouch = Input.touches[1];

                    startMinThresholdCount = false;

                    //Debug.Log(minThresholdCounter + " | " + minThresholdTime);

                    //Possible pinch began
                    if (minThresholdCounter <= minThresholdTime)
                    {
                        //Pinch began
                        if (secondTouch.phase == TouchPhase.Began)
                        {
                            //Reset the threshold count, and fire the began event.
                            minThresholdCounter = 0.0f;
                            if (EventManager.OnPinchBegan != null)
                            {
                                EventManager.OnPinchBegan(firstTouch, secondTouch);
                            }

                            Debug.Log("PINCH BEGAN");
                            pinchRecognized = true;
                        }
                        //Pinch held
                        else if (pinchRecognized && (firstTouch.phase == TouchPhase.Stationary || firstTouch.phase == TouchPhase.Moved) && 
                                (secondTouch.phase == TouchPhase.Stationary || secondTouch.phase == TouchPhase.Moved))
                        {
                            if (EventManager.OnPinchHeld != null)
                            {
                                EventManager.OnPinchHeld(firstTouch, secondTouch);
                            }

                            Debug.Log("PINCH HELD");
                        }
                        //Pinch ended
                        else
                        {
                            pinchRecognized = false;

                            if (EventManager.OnPinchEnded != null)
                            {
                                EventManager.OnPinchEnded(firstTouch, secondTouch);
                            }

                            Debug.Log("PINCH ENDED");
                        }
                    }
                }
                else if (!pinchRecognized)
                {
                    ////Possible drag began/held
                    if (firstTouch.phase == TouchPhase.Moved || firstTouch.phase == TouchPhase.Stationary)
                    {
                        minThresholdCounter = 0.0f;
                        dragMovement += firstTouch.deltaPosition;

                        if (!dragRecognized && dragMovement.sqrMagnitude > minDragMovement * minDragMovement)
                        {
                            dragRecognized = true;

                            if (EventManager.OnDragBegan != null)
                            {
                                EventManager.OnDragBegan(firstTouch);
                            }
                            Debug.Log("DRAG BEGAN");
                        }
                        else if (dragRecognized)
                        {
                            if (EventManager.OnDragHeld != null)
                            {
                                EventManager.OnDragHeld(firstTouch);
                            }
                            Debug.Log("DRAG HELD");
                        }
                    }
                    //Possible tap or drag ended
                    else
                    {
                        minThresholdCounter = 0.0f;

                        if (dragRecognized)
                        {
                            dragRecognized = false;

                            if (EventManager.OnDragEnded != null)
                            {
                                EventManager.OnDragEnded(firstTouch);
                            }
                            Debug.Log("DRAG ENDED");
                        }
                        else if (!tapRecognized)
                        {
                            tapRecognized = true;

                            if (EventManager.OnTapOccurred != null)
                            {
                                EventManager.OnTapOccurred(firstTouch);
                            }

                            Debug.Log("TAP OCCURRED");
                        }
                    }
                }
            }
            else
            {
                minThresholdCounter = 0.0f;
                ResetRecognizedValues();
            }
        }

        private void ResetRecognizedValues()
        {
            dragRecognized = false;
            pinchRecognized = false;
            tapRecognized = false;
        }

        //Found on https://answers.unity.com/questions/1073979/android-touches-pass-through-ui-elements.html
        public static bool IsPointerOverUIObject()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }
    }
}