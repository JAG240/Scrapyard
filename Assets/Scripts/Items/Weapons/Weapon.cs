using UnityEngine;

namespace Scrapyard.items.weapons
{
    public abstract class Weapon
    {
        public float bluntDamage { get; protected set; }
        public float sharpDamage { get; protected set; }
        public float accuracy { get; protected set; }
        public float reloadSpeed { get; protected set; }
        public bool isComplete { get; protected set; } = false;

        public WeaponBase weaponBase { get; protected set; }
        public GameObject model;

        public Weapon(WeaponBase weaponBase, WeaponPart[] weaponParts)
        {
            SetWeaponBase(weaponBase);
            SetWeaponParts(weaponParts);
            CalcuateStats();
        }

        protected void SetWeaponBase(WeaponBase weaponBase)
        {
            if (this.weaponBase != null)
                return;

            this.weaponBase = weaponBase;
        }

        protected abstract void SetWeaponParts(WeaponPart[] weaponParts);
        protected abstract void CalcuateStats();
    }
}
