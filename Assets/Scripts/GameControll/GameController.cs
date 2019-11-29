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

    void Start () {
        TextEnable.Init();
        player = GameObject.FindGameObjectWithTag("Player");
        shipCollisionStatus = player.GetComponent<ShipCollision>();
        if(DataCenter.instance.numberOfLevelDeaths == 0) {
            DataCenter.instance.SetTempoInicial(); //grava o tempo inicial do nível
            if (DDAAply.instance.IsAfetivo) {
                EDAStart.instance.callGetReadBigger(); //le os sinais coletados no questionario até agora só pra descartar eles.
            }
        }
    }


    void Update () {
		if(HasPressedExitGame())
            Application.Quit();
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
	
    public void SetGameWin(){
		TextEnable.EnableFaseCompleta ();
    }

	public void SetGameOver(){
		TextEnable.EnableGameLose ();
    }
 		
}	