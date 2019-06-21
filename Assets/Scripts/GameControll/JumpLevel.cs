using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JumpLevel : MonoBehaviour {

	//public BitalinoOpen bitalino;

	void Update () {
		if(HasPressedJump())
			Jump();
	}

	private bool HasPressedJump(){
		if (Input.GetKeyDown (KeyCode.P))
			return true;
		return false;
	}

	private void Jump(){
		/*if (bitalino.IsRunning) {
			bitalino.Kill ();
			Invoke ("BalanceWithSignals", 2);
		} else*/
			BalanceOnData ();
	}


	private void BalanceWithSignals(){
		DDAAply.instance.BalanceWithEmotion ();
		BalanceOnData ();
	}

	private void BalanceOnData(){
		DDAAply.instance.DensityBalanceNextLevel ();
		DDAAply.instance.SpeedBalanceNextLevel ();
		DataColector.instance.ResetData (false);

        string nextScene = gameObject.GetComponent<PassLevel>().nextLevel;
        SceneManager.LoadScene(nextScene);
	}



}
