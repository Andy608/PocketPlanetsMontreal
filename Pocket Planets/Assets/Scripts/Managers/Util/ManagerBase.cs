using UnityEngine;

namespace Managers
{
    public class ManagerBase<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                T objofType = FindObjectOfType<T>();

                if (!instance)
                {
                    instance = objofType;
                }
                else if (instance != objofType)
                {
                    Destroy(objofType);
                }

                return instance;
            }
        }
    }
}
