using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.EventSystems;

public class OnClick : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    public Entry trigger;
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (trigger.eventID == EventTriggerType.PointerClick && trigger.callback != null)
            trigger.callback.Invoke(eventData);
    }
}
