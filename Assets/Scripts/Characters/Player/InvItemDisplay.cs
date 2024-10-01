using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scrapyard.UI
{
    public class InvItemDisplay : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private Image image;
        private bool draggable = false;

        [HideInInspector] public Transform currentParent;
        public InventoryController inventoryController;
        public int slot;
        public InvItemDisplayType invType;

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!inventoryController.StartDrag(slot, invType))
                return;

            draggable = true;
            image.raycastTarget = false;
            currentParent = transform.parent;
            transform.SetParent(transform.root);
            transform.SetAsLastSibling();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if(draggable)
                transform.position = Input.mousePosition;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            draggable = false;
            transform.SetParent(currentParent);
            inventoryController.EndDrag(slot, currentParent.GetComponent<InvItemDisplay>().invType);
            image.raycastTarget = true;
        }

        private void Start()
        {
            image = GetComponent<Image>();
        }

        //TODO: Determine how we can check valid end drag before assigning new parent in InvItemSlot script
        public bool ValidEndDrag()
        {
            return inventoryController.EndDrag(slot, currentParent.GetComponent<InvItemDisplay>().invType);
        }
    }

    public enum InvItemDisplayType
    {
        Item, 
        Weapon,
        Gear
    }
}
