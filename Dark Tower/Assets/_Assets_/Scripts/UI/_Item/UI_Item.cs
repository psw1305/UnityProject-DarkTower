using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Item : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Action ClickFunc = null;
    public Action MouseRightClickFunc = null;
    
    private ItemData _item;

    public ItemData GetItem()
    {
        return _item;
    }

    public virtual void SetItem(ItemData item)
    {
        _item = item;
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            //if (ClickFunc != null) ClickFunc();
        }

        if (eventData.button == PointerEventData.InputButton.Right)
        {
            //if (MouseRightClickFunc != null) MouseRightClickFunc();
        }
    }

    public virtual void OnPointerEnter(PointerEventData eventData) {}
    public virtual void OnPointerExit(PointerEventData eventData) {}
}
