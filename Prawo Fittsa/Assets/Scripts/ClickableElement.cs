using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableElement : MonoBehaviour, IPointerClickHandler
{

    public Material defaultMaterial; //todo ensure material is selected
    public Material selectedMaterial; //todo ensure material is selected

    private bool clicked = false;

    private ClickableManager watcherScript;

    private Material newMaterial;

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
            Debug.Log("Material change");
            gameObject.GetComponent<MeshRenderer>().material = newMaterial;
            newMaterial = null;
        }
        if (clicked)
        {
            Debug.Log("Ball clicked");
            clicked = false;
            watcherScript.BallClicked(this.gameObject);
        }
    }


}
