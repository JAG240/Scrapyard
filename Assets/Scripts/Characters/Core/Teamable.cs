using UnityEngine;

namespace Scrapyard.core 
{
    public abstract class Teamable : MonoBehaviour
    {
        [field: SerializeField] public Team team { get; protected set; }
        protected void Shoot(GameObject bullet, Vector3 origin, Vector3 direction)
        {
            GameObject newBullet = Instantiate(bullet, origin, Quaternion.identity);
            newBullet.GetComponent<Rigidbody>().AddForce(direction, ForceMode.Impulse);
        }
    }
}
