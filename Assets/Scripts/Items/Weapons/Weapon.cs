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
        public float magSize { get; protected set; }
        public float curMag { get; protected set; }
        public bool isComplete { get; protected set; } = false;
        public bool onCooldown { get; protected set; } = false;
        public bool isReloading { get; protected set; } = false;

        public WeaponBase weaponBase { get; protected set; }
        public GameObject bullet { get; set; }
        public Transform end { get; set; }
        public GameObject model;

        private float maxFirerate = 10f;
        private float maxFireTime = 2f;

        private float cooldownTimer = 0f;
        private float reloadTimer = 0f;

        public Weapon(WeaponBase weaponBase, WeaponPart[] weaponParts)
        {
            SetWeaponBase(weaponBase);
            SetWeaponParts(weaponParts);
            CalcuateStats(weaponParts);
            SetComplete();

            curMag = magSize;
        }

        public void Update()
        {
            UpdateFirerate();
            UpdateReload();
        }

        public void Fire()
        {
            cooldownTimer = 0f;
            onCooldown = true;

            curMag -= 1;

            if(magSize > 0 && curMag <= 0)
                Reload();
        }

        public void Reload()
        {
            if(magSize > 0 && curMag < magSize)
            {
                reloadTimer = 0f;
                isReloading = true;
            }
        }

        private void UpdateReload()
        {
            if (isReloading)
            {
                reloadTimer += Time.deltaTime;
                
                if(reloadTimer >= reloadSpeed)
                {
                    curMag = magSize;
                    isReloading = false;
                }
            }
        }

        private void UpdateFirerate()
        {
            if (onCooldown)
            {
                cooldownTimer += Time.deltaTime;
                float firerateTime = CustomFunctions.remap(0f, maxFirerate, maxFireTime, 0f, firerate);

                if (cooldownTimer >= firerateTime)
                {
                    onCooldown = false;
                }
            }
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
