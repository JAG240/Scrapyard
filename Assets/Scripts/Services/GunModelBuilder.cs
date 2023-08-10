using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scrapyard.items.weapons;

namespace Scrapyard.services.modelbuilders
{
    public class GunModelBuilder : ModelBuilder
    {
        private WeaponBuilder _weaponBuilder;

        public GunModelBuilder()
        {
            type = WeaponType.Gun;
        }

        public override GameObject Build(WeaponBuilder weaponBuilder, Weapon weapon)
        {
            _weaponBuilder = weaponBuilder;

            GameObject newBase = weapon.weaponBase.model;

            if (newBase == null) { }
                newBase = ServiceLocator.Resolve<ItemIndex>().Get<WeaponBase>("Default Gun").model;

            GameObject newWeaponBase = Object.Instantiate(newBase, Vector3.zero, Quaternion.identity);

            WeaponBaseModel baseModel = newWeaponBase.GetComponent<WeaponBaseModel>();

            Gun gun = weapon as Gun;

            GameObject grip = gun.grip.model;

            if (grip == null)
                grip = ServiceLocator.Resolve<ItemIndex>().Get<WeaponPart>("Default Grip").model;

            GameObject barrel = gun.barrel.model;

            if (barrel == null)
                barrel = ServiceLocator.Resolve<ItemIndex>().Get<WeaponPart>("Default Barrel").model;

            grip = Object.Instantiate(grip, Vector3.zero, Quaternion.identity);
            barrel = Object.Instantiate(barrel, Vector3.zero, Quaternion.identity);

            baseModel.EquipPart(grip, WeaponPartType.GRIP);
            baseModel.EquipPart(barrel, WeaponPartType.BARREL);

            return newWeaponBase;
        }

    }

}
