using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


/// <summary>
/// Represents a clickable element, is related to ClickableManager
/// </summary>
public class ClickableElement : MonoBehaviour, IPointerClickHandler, MaterialChangeListener
{
    /// <summary>
    /// center position - used when slide is equal to 0
    /// </summary>
    private Vector3 center;

    /// <summary>
    /// default (max) position for the element. User when slide is equal to 1
    /// </summary>
    private Vector3 defaultPos;

    /// <summary>
    /// Indicates whether center was set
    /// </summary>
    private bool centerSet = false;

    /// <summary>
    /// Indicates whether defaultPos was set
    /// </summary>
    private bool defaultPosSet = false;

    /// <summary>
    /// coefficient that decides on how close the position is to either "center" or "defaultPos". Scales linearly, with slide = 0 being center and slide = 1 being defaultPos
    /// </summary>
    private float slide = 1.0f;

    /// <summary>
    /// Reference to the ClickableManager that manages this clickable object
    /// </summary>
    private ClickableManager watcherScript;

    /// <summary>
    /// defines material that this object will change to on the next iteration
    /// </summary>
    private Material newMaterial;

    /// <summary>
    /// Default scale of this object, as set from Unity inspector
    /// </summary>
    private Vector3 defaultScale;

    /// <summary>
    /// Denotes whether defaultScale was set already. Used because Vector3 is not nullable
    /// </summary>
    private bool defaultScalePresent = false;

    /// <summary>
    /// Scales this object to the new size, without ignoring default scale that this object has
    /// </summary>
    /// <param name="value">Size to set for this object, relative to the default scale</param>
    public void SetScaleMultiplier(float value)
    {
        if (!defaultScalePresent)
        {
            defaultScale = this.transform.localScale;
            defaultScalePresent = true;
        }

        this.transform.localScale = defaultScale * value;
    }

    /// <summary>
    /// Sets default (max) position for this object
    /// </summary>
    /// <param name="vector3">default/max position</param>
    public void SetDefault(Vector3 vector3)
    {
        this.defaultPos = new Vector3(vector3.x, vector3.y, vector3.z);
        defaultPosSet = true;
        recalculatePosition();
    }

    /// <summary>
    /// Sets center position for this object
    /// </summary>
    /// <param name="vector3">center position</param>
    public void SetCenter(Vector3 vector3)
    {
        this.center = new Vector3(vector3.x, vector3.y, vector3.z);
        centerSet = true;
        recalculatePosition();
    }

    /// <summary>
    /// Sets slide parameter, that describes how far this object is between its center and default/max position. For value of 0 position is center, for value 1 position is default/max. Scales linearly.
    /// </summary>
    /// <param name="slide">slide coefficient</param>
    public void Slide(float slide)
    {
        this.slide = slide;
        recalculatePosition();
    }

    /// <summary>
    /// Recalculates position based on center, max/default and slide
    /// </summary>
    private void recalculatePosition()
    {
        if (!centerSet  || !defaultPosSet)
        {
            return; //not enough arguments yet
        }
        Vector3 diff = defaultPos - center;
        Vector3 scaledDiff = slide * diff;
        this.transform.position = center + scaledDiff;
    }

    /// <summary>
    /// Sets material of this object to "selected" material. Updates on next iteration
    /// </summary>
    public void SetSelectedMaterial()
    {
        var meshRender = gameObject.GetComponent<MeshRenderer>() as MeshRenderer;
        if (meshRender)
        {
            meshRender.material.SetInt("IsHighlighted", 1);
        }  
    }

    /// <summary>
    /// Sets material of this object to "default" material. Updates on next iteration
    /// </summary>
    public void SetDefaultMaterial()
    {
        var meshRender = gameObject.GetComponent<MeshRenderer>() as MeshRenderer;
        if (meshRender)
        {
            meshRender.material.SetInt("IsHighlighted", 0);
        }     
    }

    /// <summary>
    /// Interface implementation that is used to check whether this object has been clicked on by user
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        PointClickedData data = new PointClickedData(EventSystem.current, this);
        ExecuteEvents.Execute<ClickableListener>(
                            watcherScript.transform.gameObject,
                            data,
                            PointClickedData.clickedDelegate);
    }

    /// <summary>
    /// Sets manager script for this object
    /// </summary>
    /// <param name="managerScript">manager script</param>
    public void SetManagerScript(ClickableManager managerScript)
    {
        watcherScript = managerScript;
    }

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
    }


}
