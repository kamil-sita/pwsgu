using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ballBehavior : MonoBehaviour
{

    public Material defaultMaterial;

    public Material highlightedMaterial;

    private GameObject ball;

    // Start is called before the first frame update


    void Start()
    {
        ball = GameObject.Find("selectBall_1");
        if (ball)
        {
            Debug.Log(ball.name);
            ball.GetComponent<MeshRenderer>().material = highlightedMaterial;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BallClicked(GameObject clickedBall)
    {
        if (clickedBall == ball)
        {

        }
    }
}
