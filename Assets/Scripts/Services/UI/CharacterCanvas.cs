using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Scrapyard.items.weapons;
using Scrapyard.services;

namespace Scrapyard.UI
{
    public class CharacterCanvas : MonoBehaviour
    {
        [SerializeField] private TMP_Text primaryAmmo;
        [SerializeField] private TMP_Text secondaryAmmo;
        [SerializeField] private Image primaryType;
        [SerializeField] private Image secondaryType;

        private Weapon primary;
        private Weapon secondary;

        public void ChangeWeapon(int slot, Weapon weapon)
        {
            //TODO: expand to allow more than 2 weapon slots
            Image updateType = slot == 0 ? primaryType : secondaryType;

            if (slot == 0)
                primary = weapon;
            else
                secondary = weapon;

            if (weapon == null)
            {
                updateType.sprite = null;
                updateType.color = Color.clear;
                UpdateAmmoCount();
                return;
            }

            BulletBase bulletBase = ServiceLocator.Resolve<ItemIndex>().Get<BulletBase>(weapon.weaponBase.ammoType.ToString().ToLower());
            updateType.sprite = bulletBase.bulletImage;
            updateType.color = Color.white;
        }

        private void Update()
        {
            UpdateAmmoCount();
        }


        private void UpdateAmmoCount()
        {
            if (primary != null && primary.GetType() != typeof(Melee))
                primaryAmmo.text = $"{primary.curMag}/{primary.magSize}";
            else
                primaryAmmo.text = string.Empty;

            if(secondary != null && secondary.GetType() != typeof(Melee))
                secondaryAmmo.text = $"{secondary.curMag}/{secondary.magSize}";
            else
                secondaryAmmo.text = string.Empty;
        }
    }
}
