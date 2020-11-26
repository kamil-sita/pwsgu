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
    struct LinePosition
    {
        public Vector3 startPos;
        public Vector3 endPos;
        public float amplitude;
    }
    public ComputeShader shader;
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
        LinePosition[] inputLine = new LinePosition[1];
        inputLine[0].startPos = (Vector3)startPosition;
        inputLine[0].endPos = endPosition;
        LinePosition[] outputLine = new LinePosition[1];
        ComputeBuffer buffer = new ComputeBuffer(1, 28);
        buffer.SetData(inputLine);
        int kernel = shader.FindKernel("ComputeAmplitude");
        shader.SetBuffer(kernel, "linePositionBuffer", buffer);
        shader.Dispatch(kernel, 1, 1, 1);
        buffer.GetData(outputLine);
        amplitude = outputLine[0].amplitude;
    }

    private Vector3 GetMouseCameraPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return ray.origin + ray.direction;
    }

}
