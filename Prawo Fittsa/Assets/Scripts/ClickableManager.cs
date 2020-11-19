using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages behavior and updates of managed ClickableElements
/// </summary>
public class ClickableManager : MonoBehaviour
{

    [Header("Hierarchy, in which objects are put when generated.")]

    /// <summary>
    /// Hierarchy in which new, spawned objects will be put. Can also be used to independently add object for this Manager
    /// </summary>
    public GameObject hierarchy; //todo if missing, generate it

    [Header("Selection strategy. 0 = random, 1 = other side")]

    /// <summary>
    /// Manages behavior and updates of managed ClickableElements
    /// </summary>
    public int selectionStrategy = 0; //todo enum or lambda


    [Header("Cycles to change slide and size (0 = never)")]
    public int cycleCount = 5; //todo ensure >= 0 
    public float minSlide = 0.3f; // todo ensure min < max
    public float maxSlide = 1.0f;
    public float minSize = 0.1f; // min < max
    public float maxSize = 2.0f;

    //start of todo - move methods below to object responsible for generating;
    [Header("Template of cloned object")]
    [Header("=========Object generation========")] //those two labels seem to be inverted in Editor
    /// <summary>
    /// Hierarchy that contains objects that will be used for generation
    /// </summary>
    public GameObject templateHierarchy;

    [Header("Amount of objects to generate")]
    public int countToGenerate = 15; //todo ensure makes sense

    [Header("Generation strategy. 0 = random, 1 = circle")]
    public int generationStrategy = 0; //todo enum or lambda


    [Header("Center position for cloned object")] //todo might be some helper object
    public float xCenter = 0;
    public float yCenter = 0;
    public float zCenter = 0;

    [Header("Position of cloned object")]
    public float xMin; //todo  check if min < max
    public float xMax;
    public float yMin;
    public float yMax;
    public float zMin;
    public float zMax;
    //end of todo

    /// <summary>
    /// If of object in template hierarchy that will be used to generate new object
    /// </summary>
    private int idOfTemplateToPut = 0;

    /// <summary>
    /// Contains reference to object selected by ClickableManager as selection target
    /// </summary>
    private GameObject selectedGameObject = null;

    /// <summary>
    /// Contains reference to object clicked by user at last iteration
    /// </summary>
    private GameObject clickedAtLastIteration = null;

    /// <summary>
    /// Contains reference to object selected by random generator in order to not select the same object twice in a row
    /// </summary>
    private GameObject lastSelected = null;

    /// <summary>
    /// Contains cycle, that is number of times an object has been successfully clicked on, up to size of hierarchy, in which objects are put
    /// </summary>
    private int clickedCycle = 0;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {

        Initialize();
        selectNext();
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        handleClick();
        if (selectedGameObject == null)
        {
            selectNext();
        }
        
    }

    /// <summary>
    /// Initializes this object according to selected strategy
    /// </summary>
    private void Initialize()
    {
        if (generationStrategy == 0)
        {
            for (int i = 0; i < countToGenerate; i++)
            {
                putObjectAt(new Vector3(UnityEngine.Random.Range(xMin, xMax), UnityEngine.Random.Range(yMin, yMax), UnityEngine.Random.Range(zMin, zMax)));
            }
        }
        if (generationStrategy == 1)
        {
            for (int i = 0; i < countToGenerate; i++)
            {
                float iAsRadian = (1.0f * i / countToGenerate) * 2 * Mathf.PI;

                putObjectAt(new Vector3((Mathf.Sin(iAsRadian) + 1) * 0.5f * (xMax - xMin) + xMin, UnityEngine.Random.Range(yMin, yMax), (Mathf.Cos(iAsRadian) + 1) * 0.5f * (zMax - zMin) + zMin));;
            }
        }
        setRandomSlideScale();
    }

    /// <summary>
    /// Prepares new object according to all rules (hierarchy, positions) and puts it in a given position
    /// </summary>
    /// <param name="targetPosition">default position for a new object</param>
    private void putObjectAt(Vector3 targetPosition)
    {
        GameObject element = createGameObjectFromTemplate();
        element.transform.parent = hierarchy.transform;
        ClickableElement clickable = element.GetComponent<ClickableElement>();
        clickable.SetCenter(new Vector3(xCenter, yCenter, zCenter));
        clickable.SetDefault(targetPosition);
        element.GetComponent<ClickableElement>().SetWatcherScript(this);
    }

    /// <summary>
    /// Creates game object using template hierarchy
    /// </summary>
    /// <returns>newly created game object</returns>
    private GameObject createGameObjectFromTemplate()
    {
        //todo ensure selectedGameObject contains script "ClickableElement"
        GameObject selectedGameObject = templateHierarchy.transform.GetChild(idOfTemplateToPut).gameObject;
        GameObject element = Instantiate(selectedGameObject);
        idOfTemplateToPut++;
        idOfTemplateToPut = idOfTemplateToPut % templateHierarchy.transform.childCount;
        return element;
    }

    /// <summary>
    /// Handles all the functions related to feedback from the clicked elements
    /// </summary>
    private void handleClick()
    {
        if (clickedAtLastIteration != null)
        {
            if (clickedAtLastIteration == selectedGameObject) //clicked element was the one that we looked for
            {
                cycleIteration();
                selectedGameObject = null;
                clickedAtLastIteration.GetComponent<ClickableElement>().SetDefaultMaterial();
            }
        }
        clickedAtLastIteration = null;
    }

    /// <summary>
    /// Notifies this manager that the passed object has been clicked
    /// </summary>
    /// <param name="clickedBall">Reference to clicked object</param>
    public void BallClicked(GameObject clickedBall)
    {
        clickedAtLastIteration = clickedBall;
    }

    /// <summary>
    /// Keeps count of cycle iterations between changing managed objects positions and scales
    /// </summary>
    private void cycleIteration()
    {
        clickedCycle++;
        if (cycleCount != 0)
        {
            if (clickedCycle > cycleCount)
            {
                clickedCycle = 0;
                setRandomSlideScale();
            }
        }
    }

    /// <summary>
    /// Selects random position and scale modifier and applies it to all managed objects
    /// </summary>
    private void setRandomSlideScale()
    {

        float scale = UnityEngine.Random.Range(minSize, maxSize);
        float slide = UnityEngine.Random.Range(minSlide, maxSlide);
        setSlideScale(slide, scale);
    }

    /// <summary>
    /// Sets given position (slide) and scale modifier and applies it to all managed objects
    /// </summary>
    /// <param name="slide">position modifier</param>
    /// <param name="scale">sclae modifier</param>    
    private void setSlideScale(float slide, float scale)
    {
        foreach (Transform child in hierarchy.transform)
        {
            GameObject gameObject = child.gameObject;
            //todo check if is ClickableElement
            ClickableElement clickableElement = gameObject.GetComponent<ClickableElement>();
            clickableElement.SetScaleMultiplier(scale);
            clickableElement.Slide(slide);
        }
    }

    /// <summary>
    /// Selects next selectable object, according to selection strategy
    /// </summary>
    private void selectNext()
    {
        if (selectionStrategy == 0)
        {
            selectRandomElement();
        }

        if (selectionStrategy == 1)
        {
            selectOnTheOtherSide();
        }

    }

    private bool skip = false;

    /// <summary>
    /// Selects element on the other side of imaginary circle
    /// </summary>
    private void selectOnTheOtherSide()
    {
        int currentIndex = indexOfLastSelectedDefault0();
        int length = hierarchy.transform.childCount;
        int nextToSelect;
        if (length % 2 == 0)
        {
            if (skip)
            {
                nextToSelect = (currentIndex - 1 + length / 2) % length;
            } else
            {
                nextToSelect = (currentIndex + length / 2) % length;
            }
            skip = !skip;
        } else
        {
            nextToSelect = (currentIndex + length / 2) % length;
        }

        int id = 0;

        foreach (Transform child in hierarchy.transform) //todo maybe children in hierarchy have some kind of index? Would be faster
        {
            if (id == nextToSelect)
            {
                selectGameObject(child.gameObject);
                return;
            }
            id++;
        }

        Debug.Log("Object to select has not been found in the hierarchy, but it should have been");
    }

    /// <summary>
    /// Finds id of lastly selected object, defaults to 0
    /// </summary>
    /// <returns>id of last selected object, or 0</returns>
    private int indexOfLastSelectedDefault0()
    {
        if (lastSelected == null) return 0;

        int id = 0;

        foreach (Transform child in hierarchy.transform) //todo possibly could be optimized to something like IndexOf
        {
            GameObject gameObject = child.gameObject;
            if (gameObject != lastSelected)
            {
                id++;
            } else
            {
                return id;
            }
        }

        //object was removed, defaults to 0
        return 0;
    }

    /// <summary>
    /// Selects object at random, exluding the one that was lastly selected if possible
    /// </summary>
    private void selectRandomElement()
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
        selectGameObject(toSelect);
    }

    /// <summary>
    /// Select given game object for selection
    /// </summary>
    /// <param name="gameObject"></param>
    private void selectGameObject(GameObject gameObject)
    {
        if (selectedGameObject != null)
        {
            Debug.Log("Selecting one game object without deselecting the others! This might lead to errors");
        }
        selectedGameObject = gameObject;
        lastSelected = gameObject;
        gameObject.GetComponent<ClickableElement>().SetSelectedMaterial();
    }
}
