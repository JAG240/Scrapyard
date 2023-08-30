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

            toggleInventory.gameEvent += ToggleInventory;
            toggleConsole.gameEvent += ToggleConsole;

            ServiceLocator.Register<UIManager>(this);
            Registered = true;
        }

        private void ToggleInventory()
        {
            if (inConsole)
                return;

            inInventory = !inInventory;
        }

        private void ToggleConsole()
        {
            if (inInventory)
                return;

            inConsole = !inConsole;
        }
    }
}
