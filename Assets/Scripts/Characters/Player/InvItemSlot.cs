using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scrapyard.UI
{
    public class InvItemSlot : MonoBehaviour, IDropHandler
    {
        public InventoryController inventoryController;
        public int slotID;

        public void OnDrop(PointerEventData eventData)
        {
            GameObject dropped = eventData.pointerDrag;
            InvItemDisplay item = dropped.GetComponent<InvItemDisplay>();

            if (inventoryController.AddItem(slotID))
            {
                item.slot = slotID;
                item.currentParent = transform;
            }
            else
                inventoryController.AddItem(item.slot);
        }
    }
}
