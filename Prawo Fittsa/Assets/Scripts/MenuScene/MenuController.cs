using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class for managing menu
/// </summary>
public class MenuController : MonoBehaviour
{
    [SerializeField]
    private GameObject MenuPanel, OptionPanel;
    [SerializeField]
    private Button PlayBtn, DetectFaceBtn, OptionsBtn, ExitBtn, ReturnBtn;
    /// <summary>
    /// Adding listeners on buttons
    /// </summary>
    private void Start() {
        CurrActivePanel(true, false);
        PlayBtn.onClick.AddListener(LoadMarblesScene);
        DetectFaceBtn.onClick.AddListener(LoadFaceScene);
        OptionsBtn.onClick.AddListener(setActiveOptionsPanel);
        ExitBtn.onClick.AddListener(ExitApplication);
        ReturnBtn.onClick.AddListener(setActiveMenuPanel);
    }
    /// <summary>
    /// Loading Marble Scenes
    /// </summary>
    private void LoadMarblesScene() {
        SceneLoader.Load(SceneLoader.Scene.Play);
    }
    /// <summary>
    /// Loading Face Scenes
    /// </summary>
    private void LoadFaceScene() {
        SceneLoader.Load(SceneLoader.Scene.FaceDetect);
    }
    /// <summary>
    /// Exit app
    /// </summary>
    private void ExitApplication() {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
     #else
         Application.Quit();
     #endif
    }
    /// <summary>
    ///  Activate options panel
    /// </summary>
    private void setActiveOptionsPanel() {
        CurrActivePanel();
    }
    /// <summary>
    /// Activate menu panel
    /// </summary>
    private void setActiveMenuPanel() {
        CurrActivePanel(true, false);
    }
    /// <summary>
    /// Currently active panel
    /// </summary>
    /// <param name="menuPanel"></param>
    /// <param name="optionPanel"></param>
    private void CurrActivePanel(bool menuPanel = false, bool optionPanel = true) {
        MenuPanel.SetActive(menuPanel);
        OptionPanel.SetActive(optionPanel);
    }
}
