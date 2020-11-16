using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableElement : MonoBehaviour, IPointerClickHandler
{

    public Material defaultMaterial; //todo ensure material is selected
    public Material selectedMaterial; //todo ensure material is selected
    private Vector3 center;
    private Vector3 defaultPos;
    private float slide = 1.0f;

    private bool clicked = false;

    private ClickableManager watcherScript;

    private Material newMaterial;

    private Vector3 defaultScale;
    private bool defaultScalePresent = false; //Vector3 is non nullable

    public void SetScaleMultiplier(float value)
    {
        if (!defaultScalePresent)
        {
            defaultScale = this.transform.localScale;
            defaultScalePresent = true;
        }

        this.transform.localScale = defaultScale * value;

    }

    public void SetDefault(Vector3 vector3)
    {
        this.defaultPos = new Vector3(vector3.x, vector3.y, vector3.z);
        recalculatePosition();
    }

    public void SetCenter(Vector3 vector3)
    {
        this.center = new Vector3(vector3.x, vector3.y, vector3.z);
        recalculatePosition();
    }

    public void Slide(float slide)
    {
        this.slide = slide;
        recalculatePosition();
    }

    private void recalculatePosition()
    {
        if (center == null || defaultPos == null)
        {
            return; //not enough arguments yet
        }
        Vector3 diff = defaultPos - center;
        Vector3 scaledDiff = slide * diff;
        this.transform.position = center + scaledDiff;
    }

    public void SetSelectedMaterial()
    {
        newMaterial = selectedMaterial;
    }

    public void SetDefaultMaterial()
    {
        newMaterial = defaultMaterial;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        clicked = true;
    }

    public void SetWatcherScript(ClickableManager newWatcherScript)
    {
        watcherScript = newWatcherScript;
    }
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<MeshRenderer>().material = defaultMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        if (newMaterial != null)
        {
            gameObject.GetComponent<MeshRenderer>().material = newMaterial;
            newMaterial = null;
        }
        if (clicked)
        {
            clicked = false;
            watcherScript.BallClicked(this.gameObject);
        }
    }


}
