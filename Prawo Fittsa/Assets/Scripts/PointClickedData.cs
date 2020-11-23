using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointClickedData : BaseEventData
{
    public static readonly ExecuteEvents.EventFunction<ClickableListener> clickedDelegate
    = delegate (ClickableListener handler, BaseEventData data)
    {
        var casted = ExecuteEvents.ValidateEventData<PointClickedData>(data);
        handler.ObjectClicked(casted);
    };

    public ClickableElement selectedElement;

    public PointClickedData(EventSystem eventSystem,
                           ClickableElement selectedElement
                           ) : base(eventSystem)
    {
        this.selectedElement = selectedElement;
    }
}
