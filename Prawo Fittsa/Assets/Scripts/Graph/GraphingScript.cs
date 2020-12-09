using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphingScript : MonoBehaviour
{

    public float zPos;
    public float xBegin;
    public float yBegin;
    public float xAxisScale;
    public float yAxisScale;
    public GameObject template1;
    public GameObject template2;
    public LineRenderer lineRenderer;
    public Material lineMaterial;


    void Start()
    {
        for (int i = -20; i < 40; i++)
        {
            putAt(i * 0.5f, 0, template2);
            putAt(0, i * 0.5f, template2);
        }

        List<float> x = LoadMenu.id;
        List<float> y = LoadMenu.times;

        if (x == null)
        {
            Debug.Log("Objects not set");
            return;
        }


        for (int i = 0; i < x.Count; i++)
        {
            putAt(x[i], y[i], template1);
        }

        float b = LoadMenu.a;
        float a = LoadMenu.b;
        float max = 10;

        lineRenderer.SetVertexCount(2);

        lineRenderer.SetPosition(0, new Vector3(xBegin, yBegin + b * yAxisScale, zPos));
        lineRenderer.SetPosition(1, new Vector3(xBegin + max * xAxisScale, yBegin + (a * max + b) * yAxisScale, zPos));
        lineRenderer.material = lineMaterial;

    }

    private void putAt(float x, float y, GameObject template)
    {
        GameObject go = Instantiate(template);
        go.transform.position = new Vector3(xBegin + x * xAxisScale, yBegin + y * yAxisScale, zPos);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
