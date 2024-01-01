using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static CW.Common.CwInputManager;
using static UnityEngine.EventSystems.EventTrigger;

public class MenuItem : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    public Entry trigger;
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (trigger.eventID == EventTriggerType.PointerClick && trigger.callback != null)
            trigger.callback.Invoke(eventData);
    }
}
