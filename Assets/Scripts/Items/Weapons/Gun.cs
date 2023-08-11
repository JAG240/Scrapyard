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

            if (grip)
            {
                bluntDamage += grip.bluntDamage;
                sharpDamage += grip.sharpDamage;
                accuracy += grip.accuracy;
                reloadSpeed += grip.reloadSpeed;
                range += grip.range;
            }

            if (barrel)
            {
                bluntDamage += barrel.bluntDamage;
                sharpDamage += barrel.sharpDamage;
                accuracy += barrel.accuracy;
                reloadSpeed += barrel.reloadSpeed;
                range += barrel.range;
            }

            if (weaponBase && grip && barrel)
                isComplete = true;
            else
                isComplete = false;
        }
    }
}
