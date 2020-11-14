using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ball : MonoBehaviour, IPointerClickHandler
{
    private bool clicked = false;

    private ballBehavior watcherScript;

    private bool changeMaterial = false;

    private Material newMaterial;

    public void SetMaterial(Material newMat){
        newMaterial = newMat;
        changeMaterial = true;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        clicked = true;
    }

    public void SetWatcherScript(ballBehavior newWatcherScript)
    {
        watcherScript = newWatcherScript;
    }
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (changeMaterial)
        {
            changeMaterial = false;
            gameObject.GetComponent<MeshRenderer>().material = newMaterial;
        }
        if (clicked)
        {
            Debug.Log("Ball clicked");
            clicked = false;
            watcherScript.BallClicked(this.gameObject);
        }
    }


}
