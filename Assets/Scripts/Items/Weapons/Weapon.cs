using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scrapyard.items.weapons
{
    public class Weapon
    {
        public WeaponBaseType weaponType { get; private set; }
        public float damage { get; private set; }
        public float accuracy { get; private set; }
        public float reloadSpeed { get; private set; }
        public WeaponPart[] parts;

        public Weapon(WeaponBase weaponBase, WeaponPart[] weaponParts)
        {

        }
    }
}
