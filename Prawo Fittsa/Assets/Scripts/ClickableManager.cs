using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ClickableManager : MonoBehaviour
{

    [Header("Hierarchy, in which objects are put")]
    public GameObject hierarchy; //todo if missing, generate it
  

    //todo - move methods below to object responsible for generating;
    [Header("=========Object generation========")]
    [Header("Template of cloned object")]
    public GameObject templateHierarchy;
    [Header("Amount of objects to generate")]
    public int countToGenerate = 15; //todo ensure makes sense
    [Header("Position of cloned object")]
    public float xMin; //todo  check if min < max
    public float xMax;
    public float yMin;
    public float yMax;
    public float zMin;
    public float zMax;
    private int idOfTemplateToPut = 0;
    //end of todo


    private bool initialized = false;
    private List<GameObject> elements = new List<GameObject>();
    private GameObject selectedGameObject = null;
    private GameObject clickedAtLastIteration = null;
    private GameObject lastSelected = null;
    private int previousSelectedIndex = -1;

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
            GameObject element = createGameObjectFromTemplate();
            element.transform.parent = hierarchy.transform;
            element.transform.position = new Vector3(UnityEngine.Random.Range(xMin, xMax), 0.05f, UnityEngine.Random.Range(-0.8f, 0.8f));
            //I can't find a way to modify prefab field in runtime in such way it is cloned, dirty way for now:
            element.GetComponent<ClickableElement>().SetWatcherScript(this);
            elements.Add(element);
        }
        previousSelectedIndex = -1;
    }

    private GameObject createGameObjectFromTemplate()
    {
        //todo ensure selectedGameObject contains script "ClickableElement"
        GameObject selectedGameObject = templateHierarchy.transform.GetChild(idOfTemplateToPut).gameObject;
        GameObject element = Instantiate(selectedGameObject);
        idOfTemplateToPut++;
        idOfTemplateToPut = idOfTemplateToPut % templateHierarchy.transform.childCount;
        return element;
    }

    private void HandleClick()
    {
        if (clickedAtLastIteration != null)
        {
            if (clickedAtLastIteration == selectedGameObject)
            {
                //success!!!!!!
                selectedGameObject = null;
                clickedAtLastIteration.GetComponent<ClickableElement>().SetDefaultMaterial();
            }
        }
        clickedAtLastIteration = null;
    }

    public void BallClicked(GameObject clickedBall)
    {
        clickedAtLastIteration = clickedBall;
    }

    private void SelectRandomElement()
    {
        //todo ensure at least two objects exists!
        //todo ensure objects are of correct type!

        List<GameObject> possibleChildrenToSelect = new List<GameObject>();
        foreach (Transform child in hierarchy.transform)
        {
            GameObject gameObject = child.gameObject;
            if (gameObject != lastSelected)
            {
                possibleChildrenToSelect.Add(gameObject);
            }
        }


        GameObject toSelect = possibleChildrenToSelect[UnityEngine.Random.Range(0, possibleChildrenToSelect.Count)];
        Select(toSelect);
    }

    private void Select(GameObject gameObject)
    {
        if (selectedGameObject != null)
        {
            //TODO warn or throw exception - there is already one object selected, and probably has not been deselected
        }
        Debug.Log("here");
        selectedGameObject = gameObject;
        lastSelected = gameObject;
        gameObject.GetComponent<ClickableElement>().SetSelectedMaterial();
    }
}
