using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scrapyard.items.weapons;

namespace Scrapyard.services.modelbuilders
{
    public class MeleeModelBuilder : ModelBuilder
    {
        private WeaponBuilder _weaponBuilder;

        public MeleeModelBuilder()
        {
            type = WeaponType.Melee;
        }

        public override GameObject Build(WeaponBuilder weaponBuilder, Weapon weapon)
        {
            _weaponBuilder = weaponBuilder;

            GameObject newBase = weapon.weaponBase.model;

            if (newBase == null)
                newBase = ServiceLocator.Resolve<ItemIndex>().Get<WeaponBase>("Default Melee").model;

            GameObject newWeaponBase = Object.Instantiate(newBase, Vector3.zero, Quaternion.identity);

            WeaponBaseModel baseModel = newWeaponBase.GetComponent<WeaponBaseModel>();

            Melee melee = weapon as Melee;

            GameObject end = melee.meleeEnd.model;

            if (end == null)
                end = ServiceLocator.Resolve<ItemIndex>().Get<WeaponPart>("Default Blade").model;

            end = Object.Instantiate(end, Vector3.zero, Quaternion.identity);

            baseModel.EquipPart(end, WeaponPartType.BLADE);

            return newWeaponBase;
        }
    }

}
