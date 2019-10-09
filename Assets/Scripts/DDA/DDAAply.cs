﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Emotion;

public class DDAAply : MonoBehaviour {

	public GameObject prefab;
	public static DDAAply instance;
		
	public GetPlayerData playerSignals;
	public float speedChange = 0;//idem for speed
	public float lastSpeedChange = 0;

	private float EDA = 0; //eda values
	private float ECG = 0;//ecg values

	public bool isFirstLevel = false;

	private PlayerState emotion;
	private bool IsEDA;
	private bool IsECG;

	private const int signalChange = -1;

	void Awake () {
		if (instance == null) {
			instance = prefab.GetComponent<DDAAply> ();
			instance.emotion = PlayerState.NORMAL;
		}
        else if (instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad (gameObject);
		string sensor = PlayerPrefs.GetString ("Sensor");
		if (sensor == "EDA")
			IsEDA = true;
		else if (sensor == "ECG")
			IsECG = true;
	}

	public void BalanceWithEmotion() {
        playerSignals = GameObject.Find("Game Controller").GetComponent<GetPlayerData>();
        string playerData = File.ReadAllText("FisiologicalData.txt");
        playerSignals.BreakIntoLines(playerData);

        if (IsEDA)
            emotion = playerSignals.GetEDAEmotion();
        else if (IsECG)
            emotion = playerSignals.GetECGEmotion();
        else
            emotion = PlayerState.NORMAL;
    }
				
	public void SpeedBalanceNextLevel(){
		if (LowDeathLevel () && emotion==PlayerState.BORED)
			lastSpeedChange += 0.8f;
		else if (LowDeathLevel () && emotion==PlayerState.NORMAL)
			lastSpeedChange += 0.6f;
		else if(LowDeathLevel () && emotion==PlayerState.STRESSED)
			lastSpeedChange -= 0.2f;
		else if (MediumDeathLevel () && emotion==PlayerState.STRESSED)
			lastSpeedChange += signalChange*GradualSpeedChange();
		else if(MediumDeathLevel () && emotion==PlayerState.NORMAL)
			lastSpeedChange += GradualSpeedChange();
		else if(MediumDeathLevel () && emotion==PlayerState.BORED)
			lastSpeedChange += 0.2f;
		else if(emotion==PlayerState.STRESSED)
			lastSpeedChange += signalChange*GradualSpeedChange() -0.2f;
		else if(emotion==PlayerState.NORMAL)
			lastSpeedChange += signalChange*GradualSpeedChange();
		else if(emotion==PlayerState.BORED)
			lastSpeedChange += GradualSpeedChange();
		speedChange = lastSpeedChange;
	}

	public void SpeedBalanceCurrentLevel(){
		speedChange = lastSpeedChange+ signalChange*GradualSpeedChange();

	}

		
	private float GradualSpeedChange(){
		return (float)DataColector.instance.numberOfLevelDeaths/20;
	}

	private bool LowDeathLevel(){
		return DataColector.instance.numberOfLevelDeaths < 2;
	}

	private bool MediumDeathLevel(){
		return DataColector.instance.numberOfLevelDeaths < 4;
	}

}
