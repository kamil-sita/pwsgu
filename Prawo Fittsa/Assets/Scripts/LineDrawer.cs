using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Responsible for drawing line between selected balls and computing amplitude
/// </summary>
public class LineDrawer : MonoBehaviour
{
    /// <summary>
    /// Contains computed amplitude between two objects
    /// </summary>    
    private float amplitude = 0;
    /// <summary>
    /// Contains vector of starting point
    /// </summary>    
    private Vector3? startPosition = null;
    /// <summary>
    /// Contains latest mouse position
    /// </summary>    
    private Vector3 endPosition;
    /// <summary>
    /// Contains mouse position necessary to compute amplitude
    /// </summary>    
    private Vector3 mousePosition;
    /// <summary>
    /// Contains class rendering line
    /// </summary>    
    private LineRenderer lineRenderer;
    /// <summary>
    /// Contains width of drawing line
    /// </summary>    
    private float lineWidth = 0.008f;
    /// <summary>
    /// Contains line for drawing
    /// </summary>    
    GameObject line;
    /// <summary>
    /// Structs contains position of line (startingPoint and endingPoint) and amplitude
    /// </summary>    
    struct LinePosition
    {
        public Vector3 startPos;
        public Vector3 endPos;
        public float amplitude;
    }
    /// <summary>
    /// Shader for amplitude calculations
    /// </summary>    
    public ComputeShader shader;

    /// <summary>
    /// Draws line between two object, the lastClicked and the selected one. Additionaly counts amplitude 
    /// </summary>    
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
    /// <summary>
    /// Renders line with starting parameter
    /// </summary>    
    public void renderLine()
    {
        lineRenderer = line.AddComponent<LineRenderer>();
        lineRenderer.material.color = Color.red;
        lineRenderer.SetPositions(new Vector3[] {(Vector3)startPosition, endPosition });
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
    }
    /// <summary>
    /// Removes line between two points before next iteration
    /// </summary>    
    public void removeLine()
    {
        GameObject.Destroy(line);
    }
    /// <summary>
    /// Counts ampltude using distance between first and second clics and compute shader
    /// </summary>    
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
    /// <summary>
    /// Gets mouse point necessary to compute amplitude
    /// </summary>    
    /// <returns>point where mouse is located</returns>
    private Vector3 GetMouseCameraPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return ray.origin + ray.direction;
    }

}
