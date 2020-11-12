using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballBehavior : MonoBehaviour
{

    public Material defaultMaterial;

    public Material highlightedMaterial;

    public GameObject templateClickable;

    public int countToGenerate = 15;

    private bool initialized = false;
    private List<GameObject> elements = new List<GameObject>();
    private GameObject selectedGameObject = null;
    private GameObject clickedAtLastIteration = null;
    


    void Start()
    {

    }

    void Update()
    {
        if (!initialized)
        {
            Initialize();
            initialized = true;
            SelectRandomElement();
        }
        HandleClick();
        if (selectedGameObject == null)
        {
            SelectRandomElement();
        }
        
    }

    private void Initialize()
    {
        for (int i = 0; i < countToGenerate; i++)
        {
            GameObject element = Instantiate(templateClickable);
            element.transform.position = new Vector3(UnityEngine.Random.Range(-0.8f, 0.8f), 0.05f, UnityEngine.Random.Range(-0.8f, 0.8f));
            elements.Add(element);
        }
    }

    private void HandleClick()
    {
        if (clickedAtLastIteration != null)
        {
            if (clickedAtLastIteration == selectedGameObject)
            {
                //success!!!!!!
                selectedGameObject = null;
                clickedAtLastIteration.GetComponent<ball>().SetMaterial(defaultMaterial);
            }
        }
        clickedAtLastIteration = null;
    }

    public void BallClicked(GameObject clickedBall)
    {
        //clickedBall.GetComponent<ball>().SetMaterial(defaultMaterial);
        clickedAtLastIteration = clickedBall;
    }

    private void SelectRandomElement()
    {
        GameObject toSelect = elements[UnityEngine.Random.Range(0, countToGenerate)];
        Select(toSelect);
    }

    private void Select(GameObject gameObject)
    {
        if (selectedGameObject != null)
        {
            //TODO warn, exception
        }
        Debug.Log("here");
        selectedGameObject = gameObject;
        gameObject.GetComponent<ball>().SetMaterial(highlightedMaterial);
    }
}
