using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scrapyard.UI
{
    public class InvItemSlot : MonoBehaviour, IDropHandler
    {
        public int slotID;

        public void OnDrop(PointerEventData eventData)
        {
            if (transform.childCount > 0)
                return;

            GameObject dropped = eventData.pointerDrag;
            InvItemDisplay item = dropped.GetComponent<InvItemDisplay>();
            item.slot = slotID;
            item.currentParent = transform;
        }
    }
}
