using UnityEngine;
using System.Collections.Generic;
using System;

namespace Scrapyard.items.weapons
{
    public abstract class Weapon
    {
        public float bluntDamage { get; protected set; }
        public float sharpDamage { get; protected set; }
        public float accuracy { get; protected set; }
        public float reloadSpeed { get; protected set; }
        public float range { get; protected set; } 
        public float bulletSpeed { get; protected set; } 
        public float firerate { get; protected set; }
        public float modSlots { get; protected set; }
        public bool isComplete { get; protected set; } = false;

        public WeaponBase weaponBase { get; protected set; }
        public GameObject bullet { get; set; }
        public Transform end { get; set; }
        public GameObject model;

        public Weapon(WeaponBase weaponBase, WeaponPart[] weaponParts)
        {
            SetWeaponBase(weaponBase);
            SetWeaponParts(weaponParts);
            CalcuateStats(weaponParts);
            SetComplete();
        }

        protected void SetWeaponBase(WeaponBase weaponBase)
        {
            if (this.weaponBase != null)
                return;

            this.weaponBase = weaponBase;
        }

        protected abstract void SetWeaponParts(WeaponPart[] weaponParts);
        protected abstract void SetComplete();

        protected void CalcuateStats(WeaponPart[] weaponParts)
        {
            foreach(KeyValuePair<string, float> prop in weaponBase.GetStats())
            {
                GetType().GetProperty(prop.Key).SetValue(this, prop.Value);
            }

            foreach(WeaponPart part in weaponParts)
            {
                foreach(KeyValuePair<string, float> prop in part.GetStats())
                {
                    float curPropValue =  (float) Convert.ChangeType(GetType().GetProperty(prop.Key).GetValue(this), typeof(float));
                    GetType().GetProperty(prop.Key).SetValue(this, curPropValue + prop.Value);
                }
            }
        }
    }
}
