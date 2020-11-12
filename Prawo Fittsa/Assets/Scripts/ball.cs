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
    public void OnPointerClick(PointerEventData eventData)
    {
        clicked = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        watcher = GameObject.Find("Watcher");
        watcherScript = watcher.GetComponent<ballBehavior>();
    }

    // Update is called once per frame
    void Update()
    {
        if (clicked)
        {
            Debug.Log("Ball clicked");
            clicked = false;
            watcherScript.BallClicked(this.gameObject);
        }
    }


}
