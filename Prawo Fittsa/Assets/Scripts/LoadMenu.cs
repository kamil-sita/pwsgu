using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoadMenu : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    public static List<float> areas { get; internal set; }
    public static List<float> amplitudes { get; internal set; }
    public static List<float> times { get; internal set; }
    public static List<float> id { get; internal set; }
    public static float a { get; internal set; }
    public static float b { get; internal set; }



    [SerializeField] private Button returnButton;
    private bool buttonPressed;
    void Start()
    {
        buttonPressed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonPressed)
        {
            buttonPressed = false;

            ClickableManager cm = GameObject.Find("Watcher").GetComponent<ClickableManager>();
            areas = cm.getAreas();
            amplitudes = cm.getAmplitudes();
            times = cm.getTimes();

            //linear regression
            id = new List<float>();

            for (int i = 0; i < areas.Count; i++)
            {
                float log = Mathf.Log(amplitudes[i]/areas[i] + 1, 2);
                id.Add(log);
            }

            List<float> x = id;
            List<float> y = times;

            Vector2 coords = calculateRegression(x, y);

            Debug.Log("A:" + coords.x);
            Debug.Log("B:" + coords.y);

            a = coords.x;
            b = coords.y;


            //saving csv data
            StoreCSV.saveCSV(
                areas,
                amplitudes,
                times,
                id
            );
            SceneLoader.Load(SceneLoader.Scene.Graphs);
        }
    }

    private Vector2 calculateRegression(List<float> x, List<float> y)
    {
        float sumx = 0;
        float sumy = 0;
        float sumxy = 0;
        float sumxsquared = 0;
        int count = x.Count;

        for (int i = 0; i < count; i++)
        {
            sumx += x[i];
            sumy += y[i];
            sumxy += x[i] * y[i];
            sumxsquared += x[i] * x[i];
        }

        float a =
            ((sumy * sumxsquared) - (sumx * sumxy))
            /
            ((count * sumxsquared) - (sumx * sumx));

        float b =
            ((count * sumxy) - (sumx * sumy))
            /
            ((count * sumxsquared) - (sumx * sumx));

        return new Vector2(a, b);
    }
    /// <summary>
    /// Check whether mouse button is pressed down
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
    }
    /// <summary>
    /// Check whether mouse button is pressed up
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
    }
}
