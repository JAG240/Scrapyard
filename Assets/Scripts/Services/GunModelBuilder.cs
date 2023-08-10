using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scrapyard.items.weapons;

namespace Scrapyard.services.modelbuilders
{
    public class GunModelBuilder
    {
        private WeaponBuilder _weaponBuilder;

        public GunModelBuilder(WeaponBuilder weaponBuilder)
        {
            _weaponBuilder = weaponBuilder;
        }

        public GameObject Build(Weapon weapon)
        {
            GameObject newBase = weapon.weaponBase.model;

            if (newBase == null)
                newBase = _weaponBuilder.defaultGunBase;

            GameObject newWeaponBase = Object.Instantiate(newBase, Vector3.zero, Quaternion.identity);

            WeaponBaseModel baseModel = newWeaponBase.GetComponent<WeaponBaseModel>();

            Gun gun = weapon as Gun;

            GameObject grip = gun.grip.model;

            if (grip == null)
                grip = _weaponBuilder.defaultGrip;

            GameObject barrel = gun.barrel.model;

            if (barrel == null)
                barrel = _weaponBuilder.defaultBarrel;

            grip = Object.Instantiate(grip, Vector3.zero, Quaternion.identity);
            barrel = Object.Instantiate(barrel, Vector3.zero, Quaternion.identity);

            baseModel.EquipPart(grip, WeaponPartType.GRIP);
            baseModel.EquipPart(barrel, WeaponPartType.BARREL);

            return newWeaponBase;
        }
    }

}
