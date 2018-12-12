using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class UINotificationsManager : ManagerBase<UINotificationsManager>
    {
        //Responsible for keeping track of all the notifications that may popup.

        [SerializeField] private RectTransform notificationsParent;
        [SerializeField] private UIUnlockNotification unlockNotificationPrefab;
        [SerializeField] private FloatAwayText notEnoughMoneyPrefab;
        [SerializeField] private RectTransform touchBlocker;

        //Holds a queue of notifications that need to be displayed.
        //Displays them in order:
        //When one is closed, it waits x seconds and then shows the next one in the queue.

        //The reason this isnt a list of planet properties and we arent just using one notification object
        //is because in the future we might have different notification types all subclassing a parent Notification
        //script. For now we only have one type but in the future this will be easier to handle multiple types.

        //Ideally we would have a super class "Notification" that would have subclasses, but we don't have time
        //for that... oh well!
        private Queue<UIUnlockNotification> pendingNotifications = new Queue<UIUnlockNotification>();

        private UIUnlockNotification currentShowingNotification = null;

        private void OnEnable()
        {
            EventManager.OnNewPlanetUnlocked += AddNewNotification;
            EventManager.OnCloseUnlockNotification += UpdateCurrentNotification;
            EventManager.OnPlanetSpawnDenied += SpawnBadFundsNotification;
            touchBlocker.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            EventManager.OnNewPlanetUnlocked -= AddNewNotification;
            EventManager.OnCloseUnlockNotification -= UpdateCurrentNotification;
            EventManager.OnPlanetSpawnDenied -= SpawnBadFundsNotification;
        }

        private void AddNewNotification(EnumPlanetType planetType)
        {
            Planet planet = PlanetStoreManager.Instance.GetPlanetPrefab(planetType);

            if (planet != null)
            {
                UIUnlockNotification planetUnlockNotification = Instantiate(unlockNotificationPrefab, notificationsParent, false);
                planetUnlockNotification.PlanetProperties = planet.PlanetProperties;
                planetUnlockNotification.gameObject.SetActive(false);
                EnqueueNotification(ref planetUnlockNotification);
            }
        }

        private void EnqueueNotification(ref UIUnlockNotification planetUnlockNotification)
        {
            pendingNotifications.Enqueue(planetUnlockNotification);
            ShowNextNotification();
        }

        private void UpdateCurrentNotification()
        {
            //Debug.Log("UPDATE");
            if (currentShowingNotification != null)
            {
                //Debug.Log("DELETE");
                Destroy(currentShowingNotification.gameObject);
                currentShowingNotification = null;
                touchBlocker.gameObject.SetActive(false);

                if (pendingNotifications.Count > 0)
                {
                    StartCoroutine(ShowNextNotificationWithPause());
                }
            }
        }

        private IEnumerator ShowNextNotificationWithPause()
        {
            //Debug.Log("SHOW NEXT");
            yield return new WaitForSeconds(0.2f);
            ShowNextNotification();
        }

        private void ShowNextNotification()
        {
            if (currentShowingNotification == null)
            {
                //Debug.Log("SHOW");
                currentShowingNotification = pendingNotifications.Dequeue();
                currentShowingNotification.gameObject.SetActive(true);
                touchBlocker.gameObject.SetActive(true);
            }
        }

        private void SpawnBadFundsNotification(Touch touch)
        {
            FloatAwayText text = Instantiate(notEnoughMoneyPrefab, notificationsParent, false);
            text.transform.position = touch.position;
        }
    }
}