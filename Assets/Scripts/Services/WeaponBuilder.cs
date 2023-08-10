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
        [SerializeField] private GameObject defaultGunBase;
        [SerializeField] private GameObject defaultMeleeBase;

        [SerializeField] private GameObject defaultGrip;
        [SerializeField] private GameObject defaultBarrel;
        [SerializeField] private GameObject defaultBlade;

        protected override void Register()
        {
            ServiceLocator.Register<WeaponBuilder>(this);
        }

        public GameObject BuildWeaponModel(Weapon weapon)
        {
            GameObject newBase = weapon.weaponBase.model;

            if (newBase == null)
                newBase = GetDefault(weapon.weaponBase.type);

            GameObject newWeaponBase = Instantiate(newBase, Vector3.zero, Quaternion.identity);

            WeaponBaseModel baseModel = newWeaponBase.GetComponent<WeaponBaseModel>();

            if(weapon.weaponBase.type == WeaponType.Gun)
            {
                Gun gun = weapon as Gun;

                GameObject grip = gun.grip.model;

                if (grip == null)
                    grip = defaultGrip;

                GameObject barrel = gun.barrel.model;

                if (barrel == null)
                    barrel = defaultBarrel;

                grip = Instantiate(grip, Vector3.zero, Quaternion.identity);
                barrel = Instantiate(barrel, Vector3.zero, Quaternion.identity);

                baseModel.EquipPart(grip, WeaponPartType.GRIP);
                baseModel.EquipPart(barrel, WeaponPartType.BARREL);
            }
            else if(weapon.weaponBase.type == WeaponType.Melee)
            {
                Melee melee = weapon as Melee;

                GameObject end = melee.end.model;

                if (end == null)
                    end = defaultBlade;

                end = Instantiate(end, Vector3.zero, Quaternion.identity);

                baseModel.EquipPart(end, WeaponPartType.BLADE);
            }

            return newWeaponBase;
        }

        public void DestroyModel(Weapon weapon)
        {
            Destroy(weapon.model);
        }

        private GameObject GetDefault(WeaponType type)
        {
            switch (type)
            {
                case WeaponType.Gun:
                    return defaultGunBase;
                case WeaponType.Melee:
                    return defaultMeleeBase;
                default:
                    return null;
            }
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

