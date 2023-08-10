using UnityEngine;
using Scrapyard.items.weapons;

namespace Scrapyard.services 
{
    public class BulletInfo : Service
    {
        [SerializeField] private Mesh Nut;
        [SerializeField] private Mesh Screw;

        protected override void Register()
        {
            ServiceLocator.Register<BulletInfo>(this);
        }

        public Mesh GetBulletMesh(AmmoType ammoType)
        {
            switch (ammoType)
            {
                case AmmoType.NUT:
                    return Nut;
                case AmmoType.SCREW:
                    return Screw;
                case AmmoType.NONE:
                    return null;
                default:
                    return Nut;
            }
        }
    }
}
