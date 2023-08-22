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
            if (slot == 0)
                primary = weapon;
            else
                secondary = weapon;

            BulletBase bulletBase = ServiceLocator.Resolve<ItemIndex>().Get<BulletBase>(weapon.weaponBase.ammoType.ToString());
            primaryType.sprite = bulletBase.bulletImage;
            primaryType.color = Color.white;
        }

        private void Update()
        {
            UpdateAmmoCount();
        }


        private void UpdateAmmoCount()
        {
            if(primary != null)
            {
                primaryAmmo.text = $"{primary.curMag}/{primary.magSize}";
            }

            if(secondary != null)
            {
                secondaryAmmo.text = $"{secondary.curMag}/{secondary.magSize}";
            }
        }
    }
}
