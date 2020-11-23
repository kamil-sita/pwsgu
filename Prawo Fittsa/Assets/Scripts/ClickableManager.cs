using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Enum that describes available selection strategies
/// </summary>
public enum SelectionStrategy
{
    SELECT_RANDOM,
    SELECT_OTHER_SIDE
}


/// <summary>
/// Manages behavior and updates of managed ClickableElements
/// </summary>
public class ClickableManager : MonoBehaviour, ClickableListener
{

    [Header("Hierarchy, in which objects are put when generated.")]
    /// <summary>
    /// Hierarchy in which new, spawned objects will be put. Can also be used to independently add object for this Manager
    /// </summary>
    public GameObject hierarchy; 

    [Header("Selection strategy.")]
    /// <summary>
    /// Manages behavior and updates of managed ClickableElements
    /// </summary>
    public SelectionStrategy selectionStrategy = SelectionStrategy.SELECT_OTHER_SIDE;


    [Header("Cycles to change slide and size (0 = never)")]
    public int cycleCount = 5;
    public float minSlide = 0.3f;
    public float maxSlide = 1.0f;
    public float minSize = 0.1f;
    public float maxSize = 2.0f;

    [Header("Template of cloned object")]
    public IObjectGenerator objectGenerator;

    /// <summary>
    /// Contains reference to object selected by ClickableManager as selection target
    /// </summary>
    private GameObject selectedGameObject = null;

    /// <summary>
    /// Contains reference to object selected by random generator in order to not select the same object twice in a row
    /// </summary>
    private GameObject lastSelected = null;

    /// <summary>
    /// Contains cycle, that is number of times an object has been successfully clicked on, up to size of hierarchy, in which objects are put
    /// </summary>
    private int clickedCycle = 0;

    private LineDrawer lineDrawer = new LineDrawer(); 



    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        variableCheck();
        Initialize();
        selectNext();
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        if (selectedGameObject == null)
        {
            selectNext();
        }
    }

    /// <summary>
    /// Returns hierarchy, that should contain generated objects
    /// </summary>
    /// <returns> hierarchy, that should contain generated objects</returns>
    public GameObject GetHierarchy()
    {
        return hierarchy;
    }

    /// <summary>
    /// Checks all variables for correctness
    /// </summary>
    private void variableCheck()
    {
        float tmpFloat;
        if (minSlide > maxSlide)
        {
            tmpFloat = minSlide;
            minSlide = maxSlide;
            maxSlide = tmpFloat;
            Debug.Log("minSlide > maxSlide!");
        }
        if (minSize > maxSize)
        {
            tmpFloat = minSize;
            minSize = maxSize;
            maxSize = tmpFloat;
            Debug.Log("minSize > maxSize!");
        }
        if (cycleCount < 0)
        {
            cycleCount = 0;
            Debug.Log("cycleCount < 0!");
        }
        if (hierarchy == null)
        {
            Debug.Log("Hierarchy is missing!");
            hierarchy = new GameObject("Generated selectables hierarchy (default is missing)");
        }
        if (objectGenerator == null)
        {
            Debug.Log("Object generator is null!");
        }
    }

    /// <summary>
    /// Initializes this object according to selected strategy
    /// </summary>
    private void Initialize()
    {
        if (objectGenerator != null)
        {
            objectGenerator.SetManager(this);
            objectGenerator.GenerateObjects();
        }
        setRandomSlideScale();
    }

    /// <summary>
    /// Event information for clicked elements
    /// </summary>
    /// <param name="clickable"></param>
    public void ObjectClicked(PointClickedData clickable)
    {
        var clickedAtLastIteration = clickable.selectedElement;
        if (clickedAtLastIteration.transform.gameObject == selectedGameObject) //clicked element was the one that we looked for
        {
            lineDrawer.removeLine();
            lineDrawer.drawLine();
            cycleIteration();
            selectedGameObject = null;
            ExecuteEvents.Execute<MaterialChangeListener>(
                                clickedAtLastIteration.transform.gameObject,
                                null,
                                (handler, data) => clickedAtLastIteration.SetDefaultMaterial()
                                );
        }
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
        if (selectionStrategy == SelectionStrategy.SELECT_RANDOM)
        {
            selectRandomElement();
        }

        if (selectionStrategy == SelectionStrategy.SELECT_OTHER_SIDE)
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
        if (length == 0)
        {
            return;
        }
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
        var clickableElement = gameObject.GetComponent<ClickableElement>();
        //selected ball might not have this script selected as manager, if it was not created by ObjectGenerator. Because of that, we make sure it knows about this script
        clickableElement.SetManagerScript(this);
        selectedGameObject = gameObject;
        lastSelected = gameObject;
        ExecuteEvents.Execute<MaterialChangeListener>(
                            gameObject,
                            null,
                            (handler, data) => clickableElement.SetSelectedMaterial()
                            );
    }
}
