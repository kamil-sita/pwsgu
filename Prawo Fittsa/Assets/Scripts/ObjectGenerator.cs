using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Enum that describes available generation strategies
/// </summary>
public enum GenerationStrategy
{
    GENERATE_RANDOMLY,
    GENERATE_CIRCLE,
    GENERATE_SPHERE
}

/// <summary>
/// Script that can be used to genrate objects, accordingly to settings
/// </summary>
public class ObjectGenerator : IObjectGenerator
{
    /// <summary>
    /// Hierarchy that contains objects that will be used for generation
    /// </summary>
    public GameObject templateHierarchy;


    [Header("Amount of objects to generate")]
    public int countToGenerate = 15;

    [Header("Generation strategy.")]
    public GenerationStrategy generationStrategy = GenerationStrategy.GENERATE_CIRCLE;


    [Header("Center position for cloned object")]
    public float xCenter = 0;
    public float yCenter = 0;
    public float zCenter = 0;

    [Header("Position of cloned object")]
    public float xMin;
    public float xMax;
    public float yMin;
    public float yMax;
    public float zMin;
    public float zMax;

    /// <summary>
    /// Reference to ClickableManager
    /// </summary>
    private ClickableManager cm;

    /// <summary>
    /// If of object in template hierarchy that will be used to generate new object
    /// </summary>
    private int idOfTemplateToPut = 0;

    // Start is called before the first frame update
    void Start()
    {
        variableCheck();
    }

    // Update is called once per frame
    void Update()
    {
        float tmpFloat;
        if (xMin > xMax)
        {
            tmpFloat = xMin;
            xMin = xMax;
            xMax = tmpFloat;
            Debug.Log("xMin > xMax!");
        }
        if (yMin > yMax)
        {
            tmpFloat = yMin;
            yMin = yMax;
            yMax = tmpFloat;
            Debug.Log("yMin > yMax!");
        }
        if (zMin > zMax)
        {
            tmpFloat = zMin;
            zMin = zMax;
            zMax = tmpFloat;
            Debug.Log("zMin > zMax!");
        }
        if (countToGenerate < 0)
        {
            Debug.Log("Count to generate < 0!");
            countToGenerate = 0;
        }
    }

    /// <summary>
    /// Checks all variables for correctness
    /// </summary>
    private void variableCheck()
    {

    }

    public override void SetManager(ClickableManager cm)
    {
        this.cm = cm;
    }


    public override void GenerateObjects()
    {
        if (generationStrategy == GenerationStrategy.GENERATE_CIRCLE)
        {
            for (int i = 0; i < countToGenerate; i++)
            {
                float iAsRadian = (1.0f * i / countToGenerate) * 2 * Mathf.PI;

                putObjectAt(new Vector3((Mathf.Sin(iAsRadian) + 1) * 0.5f * (xMax - xMin) + xMin, UnityEngine.Random.Range(yMin, yMax), (Mathf.Cos(iAsRadian) + 1) * 0.5f * (zMax - zMin) + zMin)); ;
            }
        }
        if (generationStrategy == GenerationStrategy.GENERATE_RANDOMLY)
        {
            for (int i = 0; i < countToGenerate; i++)
            {
                putObjectAt(new Vector3(UnityEngine.Random.Range(xMin, xMax), UnityEngine.Random.Range(yMin, yMax), UnityEngine.Random.Range(zMin, zMax)));
            }
        }
        if (generationStrategy == GenerationStrategy.GENERATE_SPHERE)
        {

            for (int i = 0; i < countToGenerate; i++)
            {
                float theta = 2 * Mathf.PI * UnityEngine.Random.Range(0f, 1f);
                float phi = Mathf.Acos(1 - 2 * UnityEngine.Random.Range(0f, 1f));

                float x = Mathf.Sin(phi) * Mathf.Cos(theta);
                float y = Mathf.Sin(phi) * Mathf.Sin(theta);
                float z = Mathf.Cos(phi);

                Debug.Log("theta:" + theta);
                Debug.Log("phi:" + phi);
                Debug.Log("x:" + x);
                Debug.Log("y:" + y);
                Debug.Log("z:" + z);

                x = (x + 1) / 2.0f * (xMax - xMin) + xMin;
                y = (y + 1) / 2.0f * (yMax - yMin) + yMin;
                z = (z + 1) / 2.0f * (zMax - zMin) + zMin;

                putObjectAt(new Vector3(x, y, z));
            }
        }
    }

    /// <summary>
    /// Prepares new object according to all rules (hierarchy, positions) and puts it in a given position
    /// </summary>
    /// <param name="targetPosition">default position for a new object</param>
    private void putObjectAt(Vector3 targetPosition)
    {
        if (cm == null)
        {
            Debug.Log("Can't generate object without ClickableManager set correctly. This generator must be referenced by ClickableManager.");
            return;
        }
        GameObject element = createGameObjectFromTemplate();
        element.transform.parent = cm.GetHierarchy().transform;
        ClickableElement clickable = element.GetComponent<ClickableElement>();
        clickable.SetCenter(new Vector3(xCenter, yCenter, zCenter));
        clickable.SetDefault(targetPosition);
        element.GetComponent<ClickableElement>().SetManagerScript(cm);
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
}
