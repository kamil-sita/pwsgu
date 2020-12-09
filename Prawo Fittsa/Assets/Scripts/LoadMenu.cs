using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoadMenu : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
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
            List<float> areas = cm.getAreas();
            List<float> amplitudes = cm.getAmplitudes();
            List<float> times = cm.getTimes();

            //linear regression
            List<float> id = new List<float>();

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


            //saving csv data
            StoreCSV.saveCSV(
                areas,
                amplitudes,
                times,
                id
            );
            SceneLoader.Load(SceneLoader.Scene.MainMenu);
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

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
    }
}
