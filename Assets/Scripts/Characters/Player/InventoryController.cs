using Scrapyard.core.character;
using Scrapyard.services;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Scrapyard.items;
using Scrapyard.items.weapons;

namespace Scrapyard.UI 
{
    public class InventoryController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform panel;
        [SerializeField] private RectTransform InvGrid;
        [SerializeField] private RectTransform WeaponGrid;
        [SerializeField] private RectTransform GearGrid;

        [Header("Prefabs")]
        [SerializeField] private GameObject InvSlot;
        [SerializeField] private GameObject InvItemDisplay;
        [SerializeField] private Sprite weaponIcon;

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
                LoadEquipment();
            }
            else
            {
                UnloadItems();
                UnloadEquipment();
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
                slot.slotID = i;

                if(_characterInventory.inventory[i] != null)
                {
                    GameObject newItem = Instantiate(InvItemDisplay, newSlot.transform);
                    InvItemDisplay item = newItem.GetComponent<InvItemDisplay>();
                    Sprite sprite = GetInventorySprite(i);

                    item.slot = i;
                    item.inventoryController = this;
                    item.invType = InvItemDisplayType.Item;

                    if(sprite != null)
                        newItem.GetComponent<Image>().sprite = sprite;
                }
            }
        }

        private void LoadEquipment()
        {
            int weaponCount = 0;

            //Load weapons 
            foreach(Weapon weapon in _characterInventory.equippedWeapons)
            {
                GameObject newSlot = Instantiate(InvSlot, WeaponGrid);
                InvItemSlot slot = newSlot.GetComponent<InvItemSlot>();
                slot.slotID = weaponCount;

                if (_characterInventory.equippedWeapons[weaponCount] != null)
                {
                    GameObject newItem = Instantiate(InvItemDisplay, newSlot.transform);
                    InvItemDisplay item = newItem.GetComponent<InvItemDisplay>();

                    item.slot = weaponCount;
                    item.inventoryController = this;
                    item.invType = InvItemDisplayType.Weapon;

                    newItem.GetComponent<Image>().sprite = weaponIcon;
                }

                weaponCount++;
            }

            //Load Gear
        }

        private Sprite GetInventorySprite(int i)
        {
            object[] inventory = _characterInventory.inventory;

            if (inventory[i].GetType() == typeof(Item))
            {
                Item item = (Item)inventory[i];
                return item.sprite;
            }
            else if (inventory[i].GetType() == typeof(Weapon))
            {
                return weaponIcon;
            }

            return null;
        }

        private void UnloadItems()
        {
            foreach(Transform slot in InvGrid.transform)
                Destroy(slot.gameObject);
        }

        private void UnloadEquipment()
        {
            foreach(Transform slot in WeaponGrid.transform)
                Destroy(slot.gameObject);
        }

        public bool EndDrag(int slot, InvItemDisplayType type)
        {
            return _characterInventory.AddToInventorySlot(_dragging, slot);
        }

        public bool StartDrag(int slot, InvItemDisplayType type)
        {
            if (_characterInventory.inventory[slot] == null)
                return false;

            _dragging = _characterInventory.inventory[slot];
            _characterInventory.inventory[slot] = null;

            if(_dragging != null)
                return true;

            return false;
        }
    }
}
