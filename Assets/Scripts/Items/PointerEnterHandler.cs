using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerEnterHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public InventoryController inventoryController;
    public GameObject item;

    public void OnPointerEnter(PointerEventData eventData)
    {
        inventoryController.changeItemVisualizer(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        inventoryController.removeItemVisualizer();
    }
}
