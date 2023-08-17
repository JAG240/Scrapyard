using UnityEngine;

namespace Scrapyard.services 
{
    public abstract class Service : MonoBehaviour
    {
        protected virtual void Awake()
        {
            Register();
            DontDestroyOnLoad(this);
        }

        protected abstract void Register();
    }
}
