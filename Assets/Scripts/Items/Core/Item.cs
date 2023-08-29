using UnityEngine;

namespace Scrapyard.items
{
    [CreateAssetMenu(fileName = "Items", menuName = "Items/New Item", order = 2)]
    public class Item : ScriptableObject
    {
        [field: SerializeField] public Sprite sprite { get; private set; }
    }
}
