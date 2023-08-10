using System;
using System.Collections.Generic;
using Scrapyard.items.weapons;
using Scrapyard;
using Scrapyard.items;
using UnityEngine;
using System.Reflection;
using Scrapyard.services.modelbuilders;

namespace Scrapyard.services 
{
    public class WeaponBuilder : Service
    {
        [field:SerializeField] public GameObject defaultGunBase {get; private set;}
        [field:SerializeField] public GameObject defaultMeleeBase {get; private set;}
        [field:SerializeField] public GameObject defaultGrip {get; private set;}
        [field:SerializeField] public GameObject defaultBarrel {get; private set;}
        [field: SerializeField] public GameObject defaultBlade { get; private set; }

        private GunModelBuilder _gunModelBuilder;
        private MeleeModelBuilder _meleeModelBuilder;

        protected override void Register()
        {
            _gunModelBuilder = new GunModelBuilder(this);
            _meleeModelBuilder = new MeleeModelBuilder(this);

            ServiceLocator.Register<WeaponBuilder>(this);
        }

        public GameObject BuildWeaponModel(Weapon weapon)
        {
            if (weapon.weaponBase.type == WeaponType.Gun)
                return _gunModelBuilder.Build(weapon);
            else if (weapon.weaponBase.type == WeaponType.Melee)
                return _meleeModelBuilder.Build(weapon);

            return null;
        }

        public void DestroyModel(Weapon weapon)
        {
            Destroy(weapon.model);
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

