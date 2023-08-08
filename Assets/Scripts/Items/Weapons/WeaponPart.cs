using UnityEngine;

namespace Scrapyard.items.weapons
{
    [CreateAssetMenu(fileName = "WeaponPart", menuName = "Items/New Weapon Part", order = 2)]
    public class WeaponPart : WeaponStats
    {
        [field:SerializeField] public WeaponPartType type { get; private set; }
    }
}
