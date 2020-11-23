using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MaterialChangeData
{
    public static readonly ExecuteEvents.EventFunction<MaterialChangeListener> materialChangeDefaultDelegate
    = delegate (MaterialChangeListener handler, BaseEventData data)
    {
        handler.SetDefaultMaterial();
    };

    public static readonly ExecuteEvents.EventFunction<MaterialChangeListener> materialChangeSelectedDelegate
    = delegate (MaterialChangeListener handler, BaseEventData data)
    {
        handler.SetSelectedMaterial();
    };
}
