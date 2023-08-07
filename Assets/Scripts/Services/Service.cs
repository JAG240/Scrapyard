using UnityEngine;

namespace Scrapyard.services 
{
    public abstract class Service : MonoBehaviour
    {
        protected virtual void Awake()
        {
            Register();
        }

        protected abstract void Register();
    }
}
