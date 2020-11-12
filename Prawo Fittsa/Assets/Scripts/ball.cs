using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ball : MonoBehaviour, IPointerClickHandler
{
    private bool clicked = false;
    private GameObject watcher;
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

    // Start is called before the first frame update
    void Start()
    {
        watcher = GameObject.Find("Watcher"); //todo remove hardcode
        watcherScript = watcher.GetComponent<ballBehavior>();
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
