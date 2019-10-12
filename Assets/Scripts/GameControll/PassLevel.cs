using System.Collections;
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
        if (Input.GetKeyDown(KeyCode.P))
            Jump();
    }

    private void Jump() {
        /*if (bitalino.IsRunning) {
			bitalino.Kill ();
			Invoke ("BalanceWithSignals", 2);
		} else*/
            BalanceOnData(false);
    }

	public void PassToNextLevel(){
		//if (bitalino.IsRunning) {
		//	bitalino.Kill ();
		//	Invoke ("BalanceWithSignals", 2);
		//} else
		    BalanceOnData (true);
	}


	private void BalanceWithSignals(){
		//DDAAply.instance.BalanceWithEmotion ();
		//BalanceOnData ();
	}

	private void BalanceOnData(bool venceu){
		DDAAply.instance.SpeedBalanceNextLevel ();
		DataColector.instance.AddToOutputLevel(venceu);
        SceneManager.LoadScene(nextLevel);
    }

    void OnApplicationQuit() {
        if (SceneManager.GetActiveScene().name.Equals("Level")) {
            DataColector.instance.SetTempoFinal();
            DataColector.instance.AddToOutputLevel(false);
            DataColector.instance.AddLevelToJson();
            DataColector.instance.Write();
        }
    }
}
