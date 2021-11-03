using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LauncherSceneController : MonoBehaviour {

    [SerializeField] private Button SettingsButton;

    void Awake() {
        SettingsButton.onClick.AddListener(OpenSettings);
    }

    private void OpenSettings() {
        SceneManager.LoadSceneAsync(GameScene.LauncherSettings, LoadSceneMode.Additive);
    }

    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}
