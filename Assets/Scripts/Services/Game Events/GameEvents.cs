using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scrapyard.services
{
    public class GameEvents : Service
    {
        private Dictionary<string, GameEvent> events = new Dictionary<string, GameEvent>();

        protected override void Register()
        {
            Index();
            ServiceLocator.Register<GameEvents>(this);
            Registered = true;
        }

        protected void Index()
        {
            object[] e = Resources.LoadAll("GameEvents");
            LoadEventDictionary<GameEvent>(e, events);
        }

        private void LoadEventDictionary<T>(object[] objs, Dictionary<string, T> list)
        {
            foreach (object obj in objs)
            {
                T o = (T)obj;
                ScriptableObject scrO = (ScriptableObject)obj;
                list.Add(scrO.name, o);
            }
        }

        public GameEvent Get(string name)
        {
            GameEvent gameEvent;
            events.TryGetValue(name, out gameEvent);
            return gameEvent;
        }

        public void OnConsole()
        {
            Get("ToggleConsole").Trigger();
        }

        public void OnInventoryToggle()
        {
            Get("ToggleInventory").Trigger();
        }
    }
}
