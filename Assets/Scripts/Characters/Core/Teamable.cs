using UnityEngine;

namespace Scrapyard.core 
{
    public abstract class Teamable : MonoBehaviour
    {
        [SerializeField] private Team team;
    }
}
