using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    private GameObject MenuPanel, OptionPanel;
    private Button PlayBtn, DetectFaceBtn, OptionsBtn, ExitBtn, ReturnBtn, PlayAroundBtn;
    private void Awake() {
        PlayBtn = GameObject.Find("Play_Btn").GetComponent<Button>();
        PlayAroundBtn = GameObject.Find("Play_Around").GetComponent<Button>();
        DetectFaceBtn = GameObject.Find("FaceDetect_Btn").GetComponent<Button>();
        OptionsBtn = GameObject.Find("Option_Btn").GetComponent<Button>();
        ExitBtn = GameObject.Find("Exit_Btn").GetComponent<Button>();
        ReturnBtn = GameObject.Find("Return_Btn").GetComponent<Button>();
        MenuPanel = GameObject.Find("Main_Panel");
        OptionPanel = GameObject.Find("Options_Panel");

    }

    private void Start() {
        CurrActivePanel(true, false);
        PlayBtn.onClick.AddListener(LoadMarblesScene);
        PlayAroundBtn.onClick.AddListener(LoadPlayAroundScene);
        DetectFaceBtn.onClick.AddListener(LoadFaceScene);
        OptionsBtn.onClick.AddListener(setActiveOptionsPanel);
        ExitBtn.onClick.AddListener(ExitApplication);
        ReturnBtn.onClick.AddListener(setActiveMenuPanel);
    }

    private void LoadMarblesScene() {
        SceneLoader.Load(SceneLoader.Scene.Play);
    }
    private void LoadPlayAroundScene()
    {
        SceneLoader.Load(SceneLoader.Scene.PlayAround);
    }
    private void LoadFaceScene() {
        SceneLoader.Load(SceneLoader.Scene.FaceDetect);
    }

    private void ExitApplication() {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
     #else
         Application.Quit();
     #endif
    }
    private void setActiveOptionsPanel() {
        CurrActivePanel();
    }
    private void setActiveMenuPanel() {
        CurrActivePanel(true, false);
    }
    private void CurrActivePanel(bool menuPanel = false, bool optionPanel = true) {
        MenuPanel.SetActive(menuPanel);
        OptionPanel.SetActive(optionPanel);
    }
}
