namespace Scrapyard.items.weapons
{
    public class Melee : Weapon
    {
        public WeaponPart end { get; private set; }

        public Melee(WeaponBase weaponBase, WeaponPart[] weaponParts) : base(weaponBase, weaponParts)
        {

        }

        protected override void CalcuateStats()
        {
            bluntDamage = weaponBase.bluntDamage + end.bluntDamage;
            sharpDamage = weaponBase.sharpDamage + end.sharpDamage;
            accuracy = weaponBase.accuracy + end.accuracy;
            reloadSpeed = weaponBase.reloadSpeed + end.reloadSpeed;

            if (weaponBase && end)
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
                    end = part;
                }
            }
        }
    }
}
