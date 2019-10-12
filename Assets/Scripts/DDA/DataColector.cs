﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class DataColector : MonoBehaviour {

    public GameObject prefab;

    public string outputFileName;

    static public DataColector instance = null;
    public static int currentLevel;
    public int numberOfLevelDeaths;

    void Start() {

        if (instance == null)
        {
            instance = prefab.GetComponent<DataColector>();
            DataFile.SetFileName(outputFileName);
            DataFile.Init();
            currentLevel = 1;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }


    void Update() {
        long agora = System.DateTime.Now.Ticks;

        if (Input.GetKey(KeyCode.Q)){
            if (Input.GetKeyDown(KeyCode.W)) {
                DataFile.addFlagEmpatica(agora);
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            DataFile.addApertouUp(agora);
        }
        if (Input.GetKeyUp(KeyCode.UpArrow)) {
            DataFile.addSoltouUp(agora);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            DataFile.addApertouDown(agora);
        }
        if (Input.GetKeyUp(KeyCode.DownArrow)) {
            DataFile.addSoltouDown(agora);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            DataFile.addApertouLeft(agora);
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow)) {
            DataFile.addSoltouLeft(agora);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            DataFile.addApertouRight(agora);
        }
        if (Input.GetKeyUp(KeyCode.RightArrow)) {
            DataFile.addSoltouRight(agora);
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            DataFile.addTiro(agora);
        }
	}

	public void AddDeath(){
		numberOfLevelDeaths++;
        DataFile.addMorte(System.DateTime.Now.Ticks);
	}

	public void ResetData(bool venceu) {
        int AsteroidCount = GameObject.FindGameObjectWithTag("LevelController").GetComponent<CreateAsteroids>().GetAsteroidCount();
        float minSpeed = GameObject.FindGameObjectWithTag("LevelController").GetComponent<CreateAsteroids>().GetMinSpeed();
        float maxSpeed = GameObject.FindGameObjectWithTag("LevelController").GetComponent<CreateAsteroids>().GetMaxSpeed();
        long tempoFinal = System.DateTime.Now.Ticks;
        DataFile.addTempoFinal(tempoFinal);
        DataFile.AddToTxtLevel (AsteroidCount, minSpeed, maxSpeed, venceu);
		numberOfLevelDeaths = 0;
	}

	void OnApplicationQuit() {
        if (SceneManager.GetActiveScene().name.Equals("Level")) {
            ResetData(false);
            DataFile.WriteFile();
        }
    }

    public int GetCurrentLevel() {
        return currentLevel;
    }

    public void AddLevel() {
        currentLevel++;
    }

}
