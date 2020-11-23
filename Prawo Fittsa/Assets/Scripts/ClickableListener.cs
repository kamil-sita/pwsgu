using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public interface ClickableListener : IEventSystemHandler
{
    void ObjectClicked(PointClickedData clickable);
}
