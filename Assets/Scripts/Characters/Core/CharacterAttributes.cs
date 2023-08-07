using UnityEngine;

namespace Scrapyard.core.character
{
    [CreateAssetMenu(fileName = "CharacterAttributes", menuName = "Characters/New Character Attributes", order = 1)]
    public class CharacterAttributes : ScriptableObject
    {
        [field: SerializeField] public float maxHealth { get; private set; }
        [field: SerializeField] public float maxStamina { get; private set; }
        [field: SerializeField] public float bluntDefense { get; private set; }
        [field: SerializeField] public float sharpDefense { get; private set; }
        [field: SerializeField] public float speed { get; private set; }
    }
}
