
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;




public class OpenCVUni : MonoBehaviour
{ 

    [DllImport("CVLIB", EntryPoint = "detectFace", CallingConvention = CallingConvention.StdCall)] // deklaracja importu
    public static extern bool detectFace(long length, int width, int height, byte[] data, out float tlx, out float tly, out float brx, out float bry); //deklaracja funkcji

    [DllImport("CVLIB", EntryPoint = "loadClassifier", CallingConvention = CallingConvention.StdCall)]
    public static extern bool loadClassifier();

    // Start is called before the first frame update
    void Start()
    {
        loadClassifier();
    }

// Update is called once per frame
    void Update()
    {
        Classify();
    }

    private void Classify()
    {
        Renderer renderer;
        renderer = GetComponent<Renderer>() as Renderer;
        
        Texture2D t = new Texture2D(renderer.material.mainTexture.width, renderer.material.mainTexture.height, TextureFormat.RGB24, false);
        t.SetPixels((renderer.material.mainTexture as WebCamTexture).GetPixels());
        Debug.Log(" Renderer2: " + renderer);
        t.Apply();
        byte[] bytes = t.GetRawTextureData();
        float tlx = 0, tly = 0, brx = 0, bry = 0;
        detectFace(bytes.Length, t.width, t.height, bytes, out tlx, out tly, out brx, out bry);
        Debug.Log(" FaceCoords :(" + tlx + ", " + tly + ", " + brx + ", " + bry + " ,) ");
        renderer.sharedMaterial.SetFloat("CX", 1 - (tlx + brx / 2) / t.width);
        renderer.sharedMaterial.SetFloat("CY", (tly + bry / 2) / t.height);
        renderer.sharedMaterial.SetFloat("R", bry / (2 * t.height));
    }

}