using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scrapyard.services;

namespace Scrapyard.items.weapons
{
    public class Weapon
    {
        public float damage { get; private set; }
        public float accuracy { get; private set; }
        public float reloadSpeed { get; private set; }

        private object weapon;
        private List<WeaponBaseType> gunTypes = new List<WeaponBaseType>() { WeaponBaseType.AUTORIFLE, WeaponBaseType.RIFLE, WeaponBaseType.SHOTGUN, WeaponBaseType.PISTOL };
        private List<WeaponBaseType> meleeTypes = new List<WeaponBaseType>() { WeaponBaseType.HILT };

        public Weapon(WeaponBase weaponBase, WeaponPart[] weaponParts)
        {
            if (gunTypes.Contains(weaponBase.type))
                weapon = CreateWeapon<Gun>(weaponBase, weaponParts);
            else if (meleeTypes.Contains(weaponBase.type))
                weapon = CreateWeapon<Melee>(weaponBase, weaponParts);

            CalculateStats();
        }

        private void CalculateStats()
        {
            if(weapon.GetType() == typeof(Gun))
            {
                Gun gun = (Gun)weapon;
                damage += gun.gunBase.damage + gun.grip.damage + gun.barrel.damage;
            }
            else if(weapon.GetType() == typeof(Melee))
            {
                Melee melee = (Melee)weapon;
                damage += melee.hilt.damage + melee.blade.damage;
            }
        }

        private T CreateWeapon<T>(WeaponBase weaponBase, WeaponPart[] weaponParts)
        {
            const string filled = "filled";
            const string missing = "missing";

            if (typeof(T) == typeof(Gun))
            {
                WeaponPart grip = null;
                WeaponPart barell = null;

                foreach (WeaponPart part in weaponParts)
                {
                    if (part.type == WeaponPartType.GRIP)
                        grip = part;
                    else if (part.type == WeaponPartType.BARREL)
                        barell = part;
                    else
                        ServiceLocator.Resolve<services.Console>().Log(services.LogType.ERROR, $"Attempted to create Gun with weapon part type {part.type} which is not supported!");
                }

                if (weaponBase && grip && barell)
                    return (T)Convert.ChangeType(new Gun(gunTypes, weaponBase, grip, barell), typeof(T));
                else
                    ServiceLocator.Resolve<services.Console>().Log(services.LogType.FATAL, $" Cannot create gun, missing parts. Base: {(weaponBase ? filled : missing)} Grip: {(grip ? filled : missing)} Barrel: {(barell ? filled : missing)}");
            }
            else if (typeof(T) == typeof(Melee))
            {
                WeaponPart blade = null;

                foreach (WeaponPart part in weaponParts)
                {
                    if (part.type == WeaponPartType.BLADE)
                        blade = part;
                    else
                        ServiceLocator.Resolve<services.Console>().Log(services.LogType.ERROR, $"Attempted to create Melee with weapon part type {part.type} which is not supported!");
                }

                if (weaponBase && blade)
                    return (T)Convert.ChangeType(new Melee(meleeTypes, weaponBase, blade), typeof(T));
                else
                    ServiceLocator.Resolve<services.Console>().Log(services.LogType.FATAL, $" Cannot create melee, missing parts. Base: {(weaponBase ? filled : missing)} Blade: {(blade ? filled : missing)}");
            }
            
            return (T)Convert.ChangeType(null, typeof(T));
        }

        private struct Gun
        {
            public WeaponBase gunBase { get; set; }
            public WeaponPart grip { get; set; }
            public WeaponPart barrel { get; set; }
            public bool isComplete { get; set; }

            public Gun(List<WeaponBaseType> gunTypes, WeaponBase gunBase, WeaponPart grip, WeaponPart barrel)
            {
                this.gunBase = gunTypes.Contains(gunBase.type) ? gunBase : null;
                this.grip = grip.type == WeaponPartType.GRIP ? grip : null;
                this.barrel = barrel.type == WeaponPartType.BARREL ? barrel : null;

                if (!this.gunBase || !this.grip || !this.barrel)
                    isComplete = false;
                else
                    isComplete = true;
            }
        }

        private struct Melee
        {
            public WeaponBase hilt { get; set; }
            public WeaponPart blade { get; set; }
            public bool isComplete { get; set; }

            public Melee(List<WeaponBaseType> meleeTypes, WeaponBase hilt, WeaponPart blade)
            {
                this.hilt = meleeTypes.Contains(hilt.type) ? hilt : null;
                this.blade = blade.type == WeaponPartType.BLADE ? blade : null;

                if (!this.hilt || !this.blade)
                    isComplete = false;
                else
                    isComplete = true;
            }
        }
    }
}
