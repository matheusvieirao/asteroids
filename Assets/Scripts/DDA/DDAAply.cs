﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Emotion;

public class DDAAply : MonoBehaviour {

	public GameObject prefab;
	public static DDAAply instance;
		
	public GetPlayerData playerSignals;
	public int densityChange = 0; //the actual amout of change
	public int lastDensityChange = 0;//the last level amount of change
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
		DontDestroyOnLoad (gameObject);
		string sensor = PlayerPrefs.GetString ("Sensor");
		if (sensor == "EDA")
			IsEDA = true;
		else if (sensor == "ECG")
			IsECG = true;
	}

	public void BalanceWithEmotion(){
		GetPlayerData ();
	}	

	public void GetPlayerData(){
		playerSignals = GameObject.Find("Game Controller").GetComponent<GetPlayerData>();
		string playerData = File.ReadAllText("FisiologicalData.txt");
		playerSignals.BreakIntoLines (playerData);

		if (IsEDA) {
			Debug.Log ("balanceamento com EDA");
			emotion = playerSignals.GetEDAEmotion ();
		} else if (IsECG) {
			Debug.Log ("balanceamento com ECG");
			emotion = playerSignals.GetECGEmotion ();
		} else { //se o balanceamento é feito apenas com dados in-game
			Debug.Log ("balanceamento com dados in-game");
			emotion = PlayerState.NORMAL;
		}
	}
				
	public void SpeedBalanceNextLevel(){
		if (LowDeathLevel () && emotion == PlayerState.BORED) {
			lastSpeedChange += 0.8f;
		} else if (LowDeathLevel () && emotion == PlayerState.NORMAL) {
			Debug.Log ("Poucas mortes (" + DataColector.instance.Deaths + ") lastSpeedChange: " + lastSpeedChange);
			lastSpeedChange += 0.6f;
			Debug.Log ("lastSpeedChange: " + lastSpeedChange);
		} else if (LowDeathLevel () && emotion == PlayerState.STRESSED) {
			lastSpeedChange -= 0.2f;
		} else if (MediumDeathLevel () && emotion == PlayerState.STRESSED) {
			lastSpeedChange += signalChange * GradualSpeedChange ();
		} else if (MediumDeathLevel () && emotion == PlayerState.NORMAL) {
			Debug.Log ("Medias mortes (" + DataColector.instance.Deaths + ") lastSpeedChange: " + lastSpeedChange);
			lastSpeedChange += GradualSpeedChange ();
			Debug.Log ("lastSpeedChange: " + lastSpeedChange);
		} else if (MediumDeathLevel () && emotion == PlayerState.BORED) {
			lastSpeedChange += 0.2f;
		} else if (emotion == PlayerState.STRESSED) {
			lastSpeedChange += signalChange * GradualSpeedChange () - 0.2f;
		} else if (emotion == PlayerState.NORMAL) {
			Debug.Log ("Muitas mortes (" + DataColector.instance.Deaths + ") lastSpeedChange: " + lastSpeedChange);
			lastSpeedChange += signalChange * GradualSpeedChange ();
			Debug.Log ("lastSpeedChange: " + lastSpeedChange);
		} else if (emotion == PlayerState.BORED) {
			lastSpeedChange += GradualSpeedChange ();
		}
		speedChange = lastSpeedChange;
	}

	public void DensityBalanceNextLevel(){
		if (LowDeathLevel () && emotion == PlayerState.BORED) {
			lastDensityChange += 200;
		} else if (LowDeathLevel () && emotion == PlayerState.NORMAL) {
			Debug.Log ("Poucas mortes (" + DataColector.instance.Deaths + ") lastDensityChange: " + lastDensityChange);
			lastDensityChange += 150;
			Debug.Log ("lastDensityChange: " + lastDensityChange);
		} else if (MediumDeathLevel () && emotion == PlayerState.STRESSED) {
			lastDensityChange += signalChange * GradualDensityChange ();
		} else if (MediumDeathLevel () && emotion == PlayerState.NORMAL) {
			Debug.Log ("Medias mortes (" + DataColector.instance.Deaths + ") lastDensityChange: " + lastDensityChange);
			lastDensityChange += GradualDensityChange ();
			Debug.Log ("lastDensityChange: " + lastDensityChange);
		} else if (MediumDeathLevel () && emotion == PlayerState.BORED) {
			lastDensityChange += 50;
		} else if (emotion == PlayerState.STRESSED) {
			lastDensityChange += signalChange * GradualDensityChange () - 50;
		} else if (emotion == PlayerState.NORMAL) {
			Debug.Log ("Muitas mortes (" + DataColector.instance.Deaths + ") lastDensityChange: " + lastDensityChange);
			lastDensityChange += signalChange * GradualDensityChange ();
			Debug.Log ("lastDensityChange: " + lastDensityChange);
		}
		densityChange = lastDensityChange;
	}

	public void SpeedBalanceCurrentLevel(){
		Debug.Log ("speedChange antes: " + speedChange);
		speedChange = lastSpeedChange+ signalChange*GradualSpeedChange();
		Debug.Log ("speedChange depois: " + speedChange);

	}

	public void DensityBalanceCurrentLevel(){
		Debug.Log ("densityChange antes: " + densityChange);
		densityChange = lastDensityChange+signalChange*GradualDensityChange();
		Debug.Log ("densityChange depois: " + densityChange);
	}

		
	private float GradualSpeedChange(){
		return (float)DataColector.instance.Deaths/20;
	}

	private int GradualDensityChange(){
		return DataColector.instance.Deaths*20;
	}

	private bool LowDeathLevel(){
		return DataColector.instance.Deaths < 2;
	}

	private bool MediumDeathLevel(){
		return (DataColector.instance.Deaths < 4 && DataColector.instance.Deaths >= 2);
	}

}
