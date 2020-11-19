using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    private float amplitude = 0;
    private Vector3? startPosition = null;
    private Vector3 endPosition;
    private Vector3 mousePosition;
    private LineRenderer lineRenderer;
    private float lineWidth = 0.008f;
    GameObject line;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void drawLine()
    {
        line = new GameObject();
        line.name = "Line";
        if (startPosition == null)
            startPosition = GetMouseCameraPoint();
        else
        {
            endPosition = GetMouseCameraPoint();
            renderLine();
            countAmplitude();
            Debug.Log("amplituda =  " + amplitude.ToString());
            startPosition = endPosition;
        }
    }

    public void renderLine()
    {
        lineRenderer = line.AddComponent<LineRenderer>();
        lineRenderer.material.color = Color.red;
        lineRenderer.SetPositions(new Vector3[] {(Vector3)startPosition, endPosition });
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
    }

    public void removeLine()
    {
        GameObject.Destroy(line);
    }

    public void countAmplitude()
    {
         amplitude = (endPosition - (Vector3)startPosition).magnitude;
    }

    private Vector3 GetMouseCameraPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return ray.origin + ray.direction;
    }

}
