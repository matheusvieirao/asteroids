﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PassLevel : MonoBehaviour {

	public string nextLevel;

	private GameObject player;
	private Ship ship;

	void Start(){
		player = GameObject.FindGameObjectWithTag("Player");
		ship = player.GetComponent<Ship> ();
	}

	void Update()
    {
        if (ship.hasWon)
            PassToNextLevel ();
	}

	private bool HasPressedNext(){
		if (Input.GetKeyDown (KeyCode.Space))
			return true;
		return false;
	}

	public void PassToNextLevel(){
		//if (bitalino.IsRunning) {
		//	bitalino.Kill ();
		//	Invoke ("BalanceWithSignals", 2);
		//} else
			BalanceOnData ();
	}


	private void BalanceWithSignals(){
		DDAAply.instance.BalanceWithEmotion ();
		BalanceOnData ();
	}

	private void BalanceOnData(){
		DDAAply.instance.SpeedBalanceNextLevel ();
		DataColector.instance.ResetData (true);
        SceneManager.LoadScene(nextLevel);
    }

}
