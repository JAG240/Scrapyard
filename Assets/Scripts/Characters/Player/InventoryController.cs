using Scrapyard.core.character;
using Scrapyard.services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Scrapyard.UI 
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private RectTransform panel;

        private CharacterInventory _characterInventory;
        private GameEvent toggleInventory;
        private GameEvent toggleConsole;
        private bool _isVisible = false;
        private bool _inConsole = false;

        //TODO: Explore option of creating a serivice that tracks the state of the player menus. Reduce boolean storage like ^ and make it global to reduce syncing failures

        private void Start()
        {
            _characterInventory = GameObject.Find("Player").GetComponent<Character>().inventory;
            toggleInventory = ServiceLocator.Resolve<GameEvents>().Get("ToggleInventory");
            toggleConsole = ServiceLocator.Resolve<GameEvents>().Get("ToggleConsole");
            toggleInventory.gameEvent += Toggle;
            toggleConsole.gameEvent += () => { _inConsole = !_inConsole; };
        }

        private void Toggle()
        {
            if (_inConsole)
                return;

            _isVisible = !_isVisible;

            panel.gameObject.SetActive(_isVisible);
        }
    }
}
