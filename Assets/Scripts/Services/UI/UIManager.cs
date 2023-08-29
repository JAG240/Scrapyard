using Scrapyard.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scrapyard.services
{
    public class UIManager : Service
    {
        [field: SerializeField] public DamageSplashController splashController { get; private set; }
        [field: SerializeField] public CharacterCanvas characterCanvas { get; private set; }

        private GameEvent toggleInventory;
        private GameEvent toggleConsole;

        public bool inInventory { get; private set; } = false;
        public bool inConsole { get; private set; } = false;

        protected override void Register()
        {
            toggleInventory = ServiceLocator.Resolve<GameEvents>().Get("ToggleInventory");
            toggleConsole = ServiceLocator.Resolve<GameEvents>().Get("ToggleConsole");

            toggleInventory.gameEvent += () => { inInventory = !inInventory; };
            toggleConsole.gameEvent += () => { inConsole = !inConsole; };

            ServiceLocator.Register<UIManager>(this);
            Registered = true;
        }
    }
}
