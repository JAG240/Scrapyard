using Scrapyard.core.character;
using Scrapyard.services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Scrapyard.items;

namespace Scrapyard.UI 
{
    public class InventoryController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform panel;
        [SerializeField] private RectTransform InvGrid;

        [Header("Prefabs")]
        [SerializeField] private GameObject InvSlot;
        [SerializeField] private GameObject InvItemDisplay;

        private CharacterInventory _characterInventory;
        private GameEvent toggleInventory;
        private bool _isVisible = false;
        private object _dragging;
        private UIManager uiManager;

        private void Start()
        {
            _characterInventory = GameObject.Find("Player").GetComponent<Character>().inventory;
            toggleInventory = ServiceLocator.Resolve<GameEvents>().Get("ToggleInventory");
            uiManager = ServiceLocator.Resolve<UIManager>();
            toggleInventory.gameEvent += Toggle;
        }

        private void Toggle()
        {
            if (uiManager.inConsole)
                return;

            _isVisible = !_isVisible;

            if (_isVisible)
            {
                LoadItems();
            }
            else
            {
                UnloadItems();
            }

            panel.gameObject.SetActive(_isVisible);
        }

        private void LoadItems()
        {
            int size = _characterInventory.inventory.Length;

            for(int i = 0; i < size; i++)
            {
                GameObject newSlot = Instantiate(InvSlot, InvGrid);
                InvItemSlot slot = newSlot.GetComponent<InvItemSlot>();

                slot.inventoryController = this;
                slot.slotID = i;

                if(_characterInventory.inventory[i] != null)
                {
                    GameObject newItem = Instantiate(InvItemDisplay, newSlot.transform);
                    InvItemDisplay item = newItem.GetComponent<InvItemDisplay>();
                    Sprite sprite = _characterInventory.GetSprite(i);

                    item.slot = i;
                    item.inventoryController = this;

                    if(sprite != null)
                        newItem.GetComponent<Image>().sprite = sprite;
                }
            }
        }

        private void UnloadItems()
        {
            foreach(Transform slot in InvGrid.transform)
                Destroy(slot.gameObject);
        }

        public bool AddItem(int slot)
        {
            return _characterInventory.AddToInventorySlot(_dragging, slot);
        }

        public bool RemoveItem(int slot)
        {
            _dragging = _characterInventory.inventory[slot];
            _characterInventory.inventory[slot] = null;

            if(_dragging != null)
                return true;

            return false;
        }
    }
}
