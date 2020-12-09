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
            StoreCSV.saveCSV(GameObject.Find("Watcher").GetComponent<ClickableManager>().getAreas(), GameObject.Find("Watcher").GetComponent<ClickableManager>().getAmplitudes());
            SceneLoader.Load(SceneLoader.Scene.MainMenu);
        }
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
