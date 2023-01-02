using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerDownHandler
{
    public static event Action<int, PointerEventData.InputButton> OnClick = delegate { };

    public int index;

    public void OnPointerDown(PointerEventData eventData)
    {
        OnClick(index, eventData.button);
    }
}