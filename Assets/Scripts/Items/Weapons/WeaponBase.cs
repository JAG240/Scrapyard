using UnityEngine;

namespace Scrapyard.items.weapons
{
    [CreateAssetMenu(fileName = "WeaponBase", menuName = "Items/New Weapon Base", order = 2)]
    public class WeaponBase: WeaponStats
    {
        [field: SerializeField] public WeaponType type { get; private set; }
        [field: SerializeField] public WeaponBaseType baseType { get; private set; }
        [field: SerializeField] public AmmoType ammoType { get; private set; }
        [field: SerializeField] public GameObject model { get; private set; }
        [field: SerializeField] public FireMode fireMode { get; private set; }
        [field: SerializeField] public WeaponUseAnimation playerAnimation {  get; private set; }
    }
}
