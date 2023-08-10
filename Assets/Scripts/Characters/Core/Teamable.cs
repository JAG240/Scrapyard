using UnityEngine;

namespace Scrapyard.core 
{
    public abstract class Teamable : MonoBehaviour
    {
        [field: SerializeField] public Team team { get; private set; }
        protected void Shoot(Vector3 origin, Vector3 direction)
        {

        }
    }
}
