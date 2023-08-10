using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scrapyard.items.weapons;

namespace Scrapyard.services.modelbuilders
{
    public class MeleeModelBuilder
    {
        private WeaponBuilder _weaponBuilder;

        public MeleeModelBuilder(WeaponBuilder weaponBuilder)
        {
            _weaponBuilder = weaponBuilder;
        }

        public GameObject Build(Weapon weapon)
        {
            GameObject newBase = weapon.weaponBase.model;

            if (newBase == null)
                newBase = _weaponBuilder.defaultMeleeBase;

            GameObject newWeaponBase = Object.Instantiate(newBase, Vector3.zero, Quaternion.identity);

            WeaponBaseModel baseModel = newWeaponBase.GetComponent<WeaponBaseModel>();

            Melee melee = weapon as Melee;

            GameObject end = melee.end.model;

            if (end == null)
                end = _weaponBuilder.defaultBlade;

            end = Object.Instantiate(end, Vector3.zero, Quaternion.identity);

            baseModel.EquipPart(end, WeaponPartType.BLADE);

            return newWeaponBase;
        }
    }

}
