using Scrapyard.items.weapons;
using UnityEngine;

namespace Scrapyard.core 
{
    public abstract class Teamable : MonoBehaviour
    {
        [field: SerializeField] public Team team { get; protected set; }

        protected void Shoot(Weapon weapon, Vector3 origin, Vector3 direction)
        {
            if (weapon.onCooldown || weapon.isReloading)
                return;

            GameObject newBullet = Instantiate(weapon.bullet, origin, Quaternion.identity);
            newBullet.GetComponent<BulletBehavior>().Init(weapon, direction, team);
            weapon.Fire();
        }
    }
}
