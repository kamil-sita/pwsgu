using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoadMenuFinal : MonoBehaviour
{
    private bool buttonPressed;

    /// <summary>
    /// Change scene to main menu
    /// </summary>
    public void InvokeChange() 
    {
        buttonPressed = true;
    }

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
            SceneLoader.Load(SceneLoader.Scene.MainMenu);
        }
    }
}
