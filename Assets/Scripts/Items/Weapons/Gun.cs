namespace Scrapyard.items.weapons
{
    public class Gun : Weapon
    {
        protected WeaponPart grip { get; private set; }
        protected WeaponPart barrel { get; private set; }

        public Gun(WeaponBase weaponBase, WeaponPart[] weaponParts) : base(weaponBase, weaponParts)
        {

        }

        protected override void SetWeaponParts(WeaponPart[] weaponParts)
        {
            foreach(WeaponPart part in weaponParts)
            {
                if (part.type == WeaponPartType.GRIP)
                    grip = part;
                else if (part.type == WeaponPartType.BARREL)
                    barrel = part;
            }
        }

        protected override void CalcuateStats()
        {
            bluntDamage = weaponBase.bluntDamage + grip.bluntDamage + barrel.bluntDamage;
            sharpDamage = weaponBase.sharpDamage + grip.sharpDamage + barrel.sharpDamage;
            accuracy = weaponBase.accuracy + grip.accuracy + barrel.accuracy;
            reloadSpeed = weaponBase.reloadSpeed + grip.reloadSpeed + barrel.reloadSpeed;

            if (weaponBase && grip && barrel)
                isComplete = true;
            else
                isComplete = false;
        }
    }
}
