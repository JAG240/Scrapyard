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

        protected override void Register()
        {
            ServiceLocator.Register<UIManager>(this);
            Registered = true;
        }
    }
}
