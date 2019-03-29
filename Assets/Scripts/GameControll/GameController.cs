using System;
using System.IO;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour{
	
	private GameObject player;
	private ShipCollision shipCollisionStatus;

	public string actualScene;

	void Start (){
		TextEnable.Init ();
		player = GameObject.FindGameObjectWithTag("Player");
		shipCollisionStatus = player.GetComponent<ShipCollision> ();
	}

    void Update () {
		//Quit Game
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}
		//Restart Level
		if (shipCollisionStatus.isDead && Input.GetKeyDown (KeyCode.Space)) {
			//DDAAply.instance.DensityBalanceCurrentLevel ();
			//DDAAply.instance.SpeedBalanceCurrentLevel ();
			SceneManager.LoadScene (actualScene);
		}
    }
						
    public void SetGameWin(){
		TextEnable.EnableFaseCompleta ();
    }

	public void SetGameOver(){
		TextEnable.EnableGameLose ();
    }
 		
}	