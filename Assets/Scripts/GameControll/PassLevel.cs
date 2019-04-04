﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PassLevel : MonoBehaviour {

	public string nextLevel;

	private GameObject player;
	private Ship ship;
	//public BitalinoOpen bitalino;

	void Start(){
		player = GameObject.FindGameObjectWithTag("Player");
		ship = player.GetComponent<Ship> ();
    }

	void Update(){
		//if (ship.hasWon && Input.GetKeyDown (KeyCode.Space))
		//	PassToNextLevel ();
		if (DataColector.instance.levelTimer.GetElapsedTime () > 2)
			PassToNextLevel ();
	}

	public void PassToNextLevel(){
//		if (bitalino.IsRunning) {
//			bitalino.Kill ();
//			Invoke ("BalanceWithSignals", 2);
//		} else
			BalanceOnData ();
	}


//	private void BalanceWithSignals(){
//		DDAAply.instance.BalanceWithEmotion ();
//		BalanceOnData ();
//	}

	private void BalanceOnData(){
        //		DDAAply.instance.DensityBalanceNextLevel ();
        //		DDAAply.instance.SpeedBalanceNextLevel ();
        DataColector.instance.ResetData();
        SceneManager.LoadScene (nextLevel);
	}

}
