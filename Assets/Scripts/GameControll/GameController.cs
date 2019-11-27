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
		LoadResourcers ();
        DataColector.instance.SetTempoInicial(); //grava o tempo inicial do nível
	}

	private void LoadResourcers(){
		TextEnable.Init ();
		player = GameObject.FindGameObjectWithTag("Player");
		shipCollisionStatus = player.GetComponent<ShipCollision> ();
	}


    void Update () {
		if(HasPressedExitGame())
			ExitGame();
		if (shipCollisionStatus.isDead) {
            TextEnable.SetHiperspaceText("Pressione espaço");
            if (Input.GetKeyDown(KeyCode.Space)) {
                RestartLevel ();
            }
        }
    }

	private bool HasPressedExitGame(){
		if (Input.GetKeyDown (KeyCode.Escape))
			return true;
		return false;
	}

	private void RestartLevel(){
		DDAAply.instance.BalanceAtDeath();
		SceneManager.LoadScene (actualScene);
	}

	private void ExitGame(){
		Application.Quit ();
	}
						
    public void SetGameWin(){
		TextEnable.EnableFaseCompleta ();
    }

	public void SetGameOver(){
		TextEnable.EnableGameLose ();
    }
 		
}	