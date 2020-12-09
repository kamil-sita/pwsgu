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

    /// <summary>
    /// Draws line and computes amplitude between balls
    /// </summary>
    [Header("Line Drawer class")]
    public LineDrawer lineDrawer;

    /// <summary>
    /// Used for computing ball area, contains object mesh
    /// </summary>
    public static Mesh viewedModel;

    /// <summary>
    /// List of amplitudes (distances between items clicked)
    /// </summary>
    private List<float> amplitudes = new List<float>();
    
    /// <summary>
    /// List of areas (areas of clicked objects)
    /// </summary>
    private List<float> areas = new List<float>();

    /// <summary>
    /// Contains a list of times, between succeeding clicks
    /// </summary>
    private List<float> times = new List<float>();

    /// <summary>
    /// Time of last click
    /// </summary>
    private float lastClickTime = -1;
    
    /// <summary>
    /// First area is ignored, this flag is true, and on first calculation of the area, it is changed
    /// </summary>
    private bool isFirstArea = true;


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
        Debug.Log(amplitudes.Count + ", " + areas.Count);
        var clickedAtLastIteration = clickable.selectedElement;
        if (clickedAtLastIteration.transform.gameObject == selectedGameObject) //clicked element was the one that we looked for
        {
            //calculating time between clicks
            float lastClickTimeTmp = lastClickTime;
            float thisClickTimeTmp = Time.time;

            if (lastClickTimeTmp != -1)
            {
                float diff = thisClickTimeTmp - lastClickTimeTmp;
                times.Add(diff);
            }

            lastClickTime = thisClickTimeTmp;

            //line drawing
            lineDrawer.removeLine();
            lineDrawer.drawLine();

            //calculating area of an object
            MeshFilter viewedModelFilter = (MeshFilter)selectedGameObject.GetComponent("MeshFilter");
            viewedModel = viewedModelFilter.sharedMesh;
            float objectArea = (CalculateSurfaceArea(viewedModel, selectedGameObject.transform.localScale)) / CalculateCameraObjectDistance(); //
            Debug.Log("Object area: " + objectArea);
            //we are only interested in object area, if it is not the first one calculated (we are interested in target, not source object)
            if (isFirstArea)
            {
                isFirstArea = false;
            } else
            {
                areas.Add(objectArea);
            }

            //iterating
            cycleIteration();
            selectedGameObject = null;

            //sending message to reset material
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
            var clickableElement = gameObject.GetComponent<ClickableElement>() as ClickableElement;
            if (clickableElement)
            {
                clickableElement.SetScaleMultiplier(scale);
                clickableElement.Slide(slide);
            } else
            {
                Debug.Log(gameObject + " does not contain ClickableElement component!");
            }
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

        foreach (Transform child in hierarchy.transform) //possibly replaceable by lookup, instead of iterating over it
        {
            if (id == nextToSelect)
            {
                var clickableElement = child.gameObject.GetComponent<ClickableElement>() as ClickableElement;
                if (clickableElement)
                {
                    selectGameObject(clickableElement);
                } else
                {
                    Debug.Log(child + " does not contain component ClickableElement"); 
                }
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

        foreach (Transform child in hierarchy.transform) //possibly replaceable by lookup, instead of iterating over it
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

        List<ClickableElement> possibleChildrenToSelect = new List<ClickableElement>();
        foreach (Transform child in hierarchy.transform)
        {
            GameObject gameObject = child.gameObject;
            if (gameObject != lastSelected)
            {
                var clickable = gameObject.GetComponent<ClickableElement>() as ClickableElement;
                if (clickable)
                {
                    possibleChildrenToSelect.Add(clickable);
                } else
                {
                    Debug.Log(gameObject + " does not contain component ClickableElement");
                }
            }
        }

        if (possibleChildrenToSelect.Count == 0)
        {
            var clickable = lastSelected.GetComponent<ClickableElement>() as ClickableElement;
            if (clickable)
            {
                selectGameObject(clickable);
            }
            else
            {
                Debug.Log(gameObject + " does not contain component ClickableElement");
            }
            return;
        }


        ClickableElement toSelect = possibleChildrenToSelect[UnityEngine.Random.Range(0, possibleChildrenToSelect.Count)];

        selectGameObject(toSelect);
    }

    /// <summary>
    /// Select given  clickable element for selection
    /// </summary>
    private void selectGameObject(ClickableElement clickableElement)
    {
        if (clickableElement == null)
        {
            return;
        }

        if (selectedGameObject != null)
        { 
            Debug.Log("Selecting one game object without deselecting the others! This might lead to errors");
        }
        //selected ball might not have this script selected as manager, if it was not created by ObjectGenerator. Because of that, we make sure it knows about this script
        clickableElement.SetManagerScript(this);
        selectedGameObject = clickableElement.transform.gameObject;
        lastSelected = clickableElement.transform.gameObject;
        ExecuteEvents.Execute<MaterialChangeListener>(
                            clickableElement.transform.gameObject,
                            null,
                            (handler, data) => clickableElement.SetSelectedMaterial()
                            );
    }
    /// <summary>
    /// Calculates area of the selected ball
    /// </summary>
    /// <param name="mesh">ball mesh</param>
    /// <param name="scale">sclae modifier</param>    
    /// <returns>ball area, which is computed using triangles from mesh</returns>
    float CalculateSurfaceArea(Mesh mesh, Vector3 scale)
    {
        var triangles = mesh.triangles;
        var vertices = mesh.vertices;

        double sum = 0.0;

        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 corner = vertices[triangles[i]];
            Vector3 a = vertices[triangles[i + 1]] - corner;
            Vector3 b = vertices[triangles[i + 2]] - corner;

            sum += Vector3.Cross(a, b).magnitude;
        }

        
        return (float)(sum / 2.0) * scale.x;
    }
    /// <summary>
    /// Calculates distance between camera and the clicked object
    /// </summary> 
    /// <returns>distance between camera and the object</returns>
    float CalculateCameraObjectDistance()
    {
        Vector3 heading = selectedGameObject.transform.position - Camera.main.transform.position;
        float distance = Vector3.Dot(heading, Camera.main.transform.forward);
        return distance;
    }

    /// <summary>
    /// Reports amplitude to this ClickableManager
    /// </summary>
    /// <param name="amplitude"></param>
    public void ReportAmplitude(float amplitude)
    {
        amplitudes.Add(amplitude);
    }

    /// <summary>
    /// Returns list of areas, as calculated by this ClickableManager. First area is automatically ignored
    /// </summary>
    /// <returns></returns>
    public List<float> getAreas()
    {
        return areas;
    }

    /// <summary>
    /// Returns list of amplitudes, as reported to this ClickableManager
    /// </summary>
    /// <returns></returns>
    public List<float> getAmplitudes()
    {
        return amplitudes;
    }

    /// <summary>
    /// Returns list of times, between succeeding clicks
    /// </summary>
    /// <returns></returns>
    public List<float> getTimes()
    {
        return times;
    }
}
