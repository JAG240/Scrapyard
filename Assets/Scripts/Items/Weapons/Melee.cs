namespace Scrapyard.items.weapons
{
    public class Melee : Weapon
    {
        public WeaponPart meleeEnd { get; private set; }

        public Melee(WeaponBase weaponBase, WeaponPart[] weaponParts) : base(weaponBase, weaponParts)
        {

        }

        protected override void CalcuateStats()
        {
            if (weaponBase)
            {
                bluntDamage += weaponBase.bluntDamage;
                sharpDamage += weaponBase.sharpDamage;
                accuracy += weaponBase.accuracy;
                reloadSpeed += weaponBase.reloadSpeed;
                range += weaponBase.range;
            }

            if (meleeEnd)
            {
                bluntDamage += meleeEnd.bluntDamage;
                sharpDamage += meleeEnd.sharpDamage;
                accuracy += meleeEnd.accuracy;
                reloadSpeed += meleeEnd.reloadSpeed;
                range += meleeEnd.range;
            }

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
