﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LevelManager : MonoBehaviour {
    public int currentStage = 0;

    private WwiseInterface wwInterface;

    void Awake() {
        DontDestroyOnLoad(transform.gameObject);

        wwInterface = GetComponent<WwiseInterface>();
    }
    
    public void LoadNextStage() {
        if (currentStage < 5) {
            SceneManager.LoadScene("Stage" + ++currentStage);

            wwInterface.setState("Game_Start", "Gameplay");
            wwInterface.callEvent("Play_Music");

            GameObject.FindObjectOfType<DataBank>().stage = currentStage;
        } else
            SceneManager.LoadScene("Win");
    }
    
    // Loads the scene with the given name
    public void LoadLevel(string level) {
        SceneManager.LoadScene(level);
    }

    // Quit game
    public void Quit() {
        Application.Quit();
    }
}