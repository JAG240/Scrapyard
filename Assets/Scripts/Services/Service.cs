using UnityEngine;

namespace Scrapyard.services 
{
    public abstract class Service : MonoBehaviour
    {
        [field: SerializeField] public bool Registered { get; protected set; } = false;

        protected virtual void Awake()
        {
            Register();
            
            if(transform.parent == null)
                DontDestroyOnLoad(this);
        }

        protected abstract void Register();
    }
}
