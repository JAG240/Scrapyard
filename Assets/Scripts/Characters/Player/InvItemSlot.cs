using Scrapyard.core;
using Scrapyard.items.weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scrapyard.UI
{
    public class InvItemSlot : MonoBehaviour, IDropHandler
    {
        public int slotID;
        public SlotType slotType;
        
        public InvItemSlot(SlotType slotType)
        {
            this.slotType = slotType;
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (transform.childCount > 0)
                return;

            GameObject dropped = eventData.pointerDrag;
            InvItemDisplay item = dropped.GetComponent<InvItemDisplay>();

            if (!IsValidSlot(item))
                return;

            item.slot = this;
            item.currentParent = transform;
        }

        private bool IsValidSlot(InvItemDisplay item)
        {
            if(slotType == SlotType.Item)
                return true;
            else if(slotType == SlotType.Weapon)
                return item.inventoryController.GetItemType() == typeof(Gun) || item.inventoryController.GetItemType() == typeof(Melee);

            return false;
        }

        public enum SlotType
        {
            Item,
            Weapon,
            Gear
        }
    }
}
