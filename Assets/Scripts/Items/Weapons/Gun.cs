using System.Reflection;
using UnityEngine;

namespace Scrapyard.items.weapons
{
    public class Gun : Weapon
    {
        public WeaponPart grip { get; private set; }
        public WeaponPart barrel { get; private set; }

        public Gun(WeaponBase weaponBase, WeaponPart[] weaponParts) : base(weaponBase, weaponParts)
        {

        }

        protected override void SetWeaponParts(WeaponPart[] weaponParts)
        {
            if (weaponParts == null)
                return;

            foreach(WeaponPart part in weaponParts)
            {
                if (part.type == WeaponPartType.GRIP)
                    grip = part;
                else if (part.type == WeaponPartType.BARREL)
                {
                    barrel = part;
                }
            }
        }

        protected override void SetComplete()
        {
            if (weaponBase && grip && barrel)
                isComplete = true;
            else
                isComplete = false;
        }
    }
}
