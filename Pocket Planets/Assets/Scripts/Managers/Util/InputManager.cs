using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Managers
{
    public class InputManager : ManagerBase<InputManager>
    {
        //The maximum pixels a tap can be dragged to count as a tap.
        private float maxTapMovement = 50.0f;
        private float sqrMaxTapMovement;

        private Vector2 dragMovement;
        private Vector3 startMousePosition;

        private float dragMinTime = 0.1f;
        private float pinchMinTime = 0.1f;
        private float startTime;

        //If movement is greated than maxTapMovement, the tap failed.
        private bool tapFailed = false;
        private bool dragRecognized = false;
        private bool pinchRecognized = false;
        private bool middleMouseDrag = false;

        private void Start()
        {
            sqrMaxTapMovement = maxTapMovement * maxTapMovement;
        }

        private void Update()
        {
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    GameStateManager.Instance.TogglePause();
            //}

            if (!IsPointerOverUIObject())
            {
                CheckForMouseScroll();
            }

            if (Input.touchCount > 0)
            {
                if (Input.touchCount == 2)
                {
                    CheckForPinch();
                }

                CheckForDrag();
            }
            else if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0) || Input.GetMouseButtonUp(0))
            {
                CheckForMouseDrag(0);
            }
            else if (Input.GetMouseButtonDown(2) || Input.GetMouseButton(2) || Input.GetMouseButtonUp(2))
            {
                CheckForMiddleMouseDrag();
            }
            else
            {
                dragRecognized = false;
                pinchRecognized = false;
                tapFailed = false;
                middleMouseDrag = false;
            }
        }

        private void CheckForMouseScroll()
        {
            //Update the bool variables too dragRecognized etc
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                if (EventManager.OnScrollOccurred != null)
                {
                    EventManager.OnScrollOccurred(Input.GetAxis("Mouse ScrollWheel"));
                }

                pinchRecognized = true;
                dragRecognized = false;
                tapFailed = true;
            }
        }

        private void CheckForMouseDrag(int button)
        {
            if (pinchRecognized) return;

            if (Input.GetMouseButtonDown(0))
            {
                dragMovement = Vector2.zero;
                startTime = Time.time;
                startMousePosition = Input.mousePosition;
            }
            else if (Input.GetMouseButton(0))
            {
                dragMovement = (Input.mousePosition - startMousePosition);

                if (!dragRecognized && Time.time - startTime > dragMinTime)
                {
                    dragRecognized = true;
                    pinchRecognized = false;
                    tapFailed = true;

                    if (EventManager.OnDragBegan != null)
                    {
                        EventManager.OnDragBegan(Input.mousePosition);
                    }
                }
                else if (dragRecognized)
                {
                    if (EventManager.OnDragHeld != null)
                    {
                        EventManager.OnDragHeld(Input.mousePosition);
                    }
                }
                else if (dragMovement.sqrMagnitude > sqrMaxTapMovement)
                {
                    tapFailed = true;
                }
            }
            else
            {
                if (dragRecognized)
                {
                    if (EventManager.OnDragEnded != null)
                    {
                        EventManager.OnDragEnded(Input.mousePosition);
                    }
                }
                else
                {
                    CheckForMouseTap();
                }
            }
        }

        private void CheckForMiddleMouseDrag()
        {
            if (Input.GetMouseButtonDown(2))
            {
                dragMovement = Vector2.zero;
                startTime = Time.time;
                startMousePosition = Input.mousePosition;
            }
            else if (Input.GetMouseButton(2))
            {
                dragMovement = (Input.mousePosition - startMousePosition);

                if (!middleMouseDrag && Time.time - startTime > dragMinTime)
                {
                    middleMouseDrag = true;

                    if (EventManager.OnMiddleMouseDragBegan != null)
                    {
                        EventManager.OnMiddleMouseDragBegan(Input.mousePosition);
                    }
                }
                else if (middleMouseDrag)
                {
                    if (EventManager.OnMiddleMouseDragHeld != null)
                    {
                        EventManager.OnMiddleMouseDragHeld(Input.mousePosition);
                    }
                }
                //else if (dragMovement.sqrMagnitude > sqrMaxTapMovement)
                //{
                //    tapFailed = true;
                //}
            }
            else
            {
                if (middleMouseDrag)
                {
                    if (EventManager.OnMiddleMouseDragEnded != null)
                    {
                        EventManager.OnMiddleMouseDragEnded(Input.mousePosition);
                    }
                }
            }
        }

        //First Priority
        private void CheckForPinch()
        {
            if (dragRecognized) return;

            Touch firstTouch = Input.touches[0];
            Touch secondTouch = Input.touches[1];

            if (firstTouch.phase == TouchPhase.Began && secondTouch.phase == TouchPhase.Began)
            {
                dragMovement = Vector2.zero;
                startTime = Time.time;
            }
            else if ((firstTouch.phase == TouchPhase.Moved || firstTouch.phase == TouchPhase.Stationary) && (secondTouch.phase == TouchPhase.Moved || secondTouch.phase == TouchPhase.Stationary))
            {
                if (!pinchRecognized && Time.time - startTime > pinchMinTime)
                {
                    pinchRecognized = true;
                    dragRecognized = false;
                    tapFailed = true;

                    //Debug.Log("PINCH BEGAN");
                    if (EventManager.OnPinchBegan != null)
                    {
                        EventManager.OnPinchBegan(firstTouch, secondTouch);
                    }
                }
                else if (pinchRecognized)
                {
                    //Debug.Log("PINCH HELD");
                    if (EventManager.OnPinchHeld != null)
                    {
                        EventManager.OnPinchHeld(firstTouch, secondTouch);
                    }
                }
                else if (dragMovement.sqrMagnitude > sqrMaxTapMovement)
                {
                    tapFailed = true;
                }
            }
            else
            {
                if (pinchRecognized)
                {
                    //Debug.Log("PINCH ENDED");
                    if (EventManager.OnPinchEnded != null)
                    {
                        EventManager.OnPinchEnded(firstTouch, secondTouch);
                    }
                }
            }
        }

        //Second Priority
        private void CheckForDrag()
        {
            if (pinchRecognized) return;

            Touch touch = Input.touches[0];

            if (touch.phase == TouchPhase.Began)
            {
                dragMovement = Vector2.zero;
                startTime = Time.time;
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                dragMovement += touch.deltaPosition;

                if (!dragRecognized && Time.time - startTime > dragMinTime)
                {
                    dragRecognized = true;
                    pinchRecognized = false;
                    tapFailed = true;

                    //Debug.Log("DRAG BEGAN");
                    if (EventManager.OnDragBegan != null)
                    {
                        EventManager.OnDragBegan(touch.position);
                    }
                }
                else if (dragRecognized)
                {
                    //Debug.Log("DRAG HELD");
                    if (EventManager.OnDragHeld != null)
                    {
                        EventManager.OnDragHeld(touch.position);
                    }
                }
                else if (dragMovement.sqrMagnitude > sqrMaxTapMovement)
                {
                    tapFailed = true;
                }
            }
            else
            {
                if (dragRecognized)
                {
                    //Debug.Log("DRAG ENDED");
                    if (EventManager.OnDragEnded != null)
                    {
                        EventManager.OnDragEnded(touch.position);
                    }
                }
                else
                {
                    CheckForTap();
                }
            }
        }

        //Last Priority
        private void CheckForTap()
        {
            if (dragRecognized) return;
            if (pinchRecognized) return;

            if (!tapFailed)
            {
                if (EventManager.OnTapOccurred != null)
                {
                    EventManager.OnTapOccurred(Input.touches[0].position);
                }
            }
        }

        private void CheckForMouseTap()
        {
            if (dragRecognized) return;
            if (pinchRecognized) return;

            if (!tapFailed)
            {
                if (EventManager.OnTapOccurred != null)
                {
                    EventManager.OnTapOccurred(Input.mousePosition);
                }
            }
        }

        //Found on https://answers.unity.com/questions/1073979/android-touches-pass-through-ui-elements.html
        public static bool IsPointerOverUIObject()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

            //Debug.Log("Raycasts: " + results.Count);

            foreach (RaycastResult raycast in results)
            {
                //Debug.Log("Raycast Layer: " + raycast.gameObject.layer.ToString());
                if (raycast.gameObject.layer == LayerMask.NameToLayer("UI"))
                {
                    return true;
                }
            }

            return false;
        }
    }
}