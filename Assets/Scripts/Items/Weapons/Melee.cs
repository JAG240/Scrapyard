namespace Scrapyard.items.weapons
{
    public class Melee : Weapon
    {
        public WeaponPart meleeEnd { get; private set; }

        public Melee(WeaponBase weaponBase, WeaponPart[] weaponParts) : base(weaponBase, weaponParts)
        {

        }

        protected override void SetComplete()
        {
            if (weaponBase && meleeEnd)
                isComplete = true;
            else
                isComplete = false;
        }

        protected override void SetWeaponParts(WeaponPart[] weaponParts)
        {
            foreach(WeaponPart part in weaponParts)
            {
                if(part.type == WeaponPartType.BLADE || part.type == WeaponPartType.BARREL)
                {
                    meleeEnd = part;
                }
            }
        }
    }
}
