using System;
using System.Collections.Generic;
using Scrapyard.items.weapons;
using Scrapyard;
using Scrapyard.items;
using UnityEngine;
using System.Reflection;

namespace Scrapyard.services 
{
    public class WeaponBuilder : Service
    {
        protected override void Register()
        {
            ServiceLocator.Register<WeaponBuilder>(this);
        }

        public Weapon BuildWeapon(WeaponBase weaponBase, WeaponPart[] weaponParts)
        {
            if (!Validate(weaponBase, weaponParts))
                return null;

            Assembly asm = typeof(Weapon).Assembly;
            Type type = asm.GetType("Scrapyard.items.weapons." + weaponBase.type.ToString());
            Weapon weapon = (Weapon)Activator.CreateInstance(type, weaponBase, weaponParts);
            return weapon;
        }

        public bool Validate(WeaponBase weaponBase, WeaponPart[] weaponParts)
        {
            List<WeaponPartType> partTypes = new List<WeaponPartType>();

            foreach(WeaponPart part in weaponParts)
            {
                if (!part.baseTypes.Contains(weaponBase.baseType) || partTypes.Contains(part.type))
                    return false;

                partTypes.Add(part.type);
            }

            return true;
        }
    }
}

