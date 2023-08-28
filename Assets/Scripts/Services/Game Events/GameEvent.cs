using System;
using UnityEngine;

namespace Scrapyard.services
{
    [CreateAssetMenu(fileName = "GameEvent", menuName = "Game Events/New Game Event", order = 1)]
    public class GameEvent : ScriptableObject
    {
        public event Action gameEvent;

        public void Trigger()
        {
            gameEvent?.Invoke();
        }
    }
}
