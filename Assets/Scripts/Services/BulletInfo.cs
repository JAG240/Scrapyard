using UnityEngine;
using Scrapyard.items.weapons;

namespace Scrapyard.services 
{
    public class BulletInfo : Service
    {
        [SerializeField] private GameObject Nut;
        [SerializeField] private GameObject Screw;

        protected override void Register()
        {
            ServiceLocator.Register<BulletInfo>(this);
        }

        public GameObject GetBulletMesh(AmmoType ammoType)
        {
            switch (ammoType)
            {
                case AmmoType.Nut:
                    return Nut;
                case AmmoType.Screw:
                    return Screw;
                case AmmoType.None:
                    return null;
                default:
                    return Nut;
            }
        }
    }
}
