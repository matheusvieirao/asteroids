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
                EDAStart.instance.LerEDACalculaExcitacao(false); //Descarta os sinais eda lidos no questionario (calcularExcitacao=false)
            }
        }
    }


    void Update () {
		if(HasPressedExitGame())
            Application.Quit();
        if (shipCollisionStatus.isDead) {
            SetTextDesExtZon();
            
            if (Input.GetKeyDown(KeyCode.Space)) {
                RestartLevel ();
            }
        }
    }

    private void SetTextDesExtZon() {
        DDAAply inst = DDAAply.instance;
        string desempenho, excitacao, zona;


        if (inst.desempenho == State.PlayerState.HIGH) {
            desempenho = "h";
        }
        else if (inst.desempenho == State.PlayerState.NORMAL) {
            desempenho = "n";
        }
        else if (inst.desempenho == State.PlayerState.LOW) {
            desempenho = "l";
        }
        else {
            desempenho = "-";
        }

        if (inst.excitacao == State.PlayerState.HIGH) {
            excitacao = "h";
        }
        else if (inst.excitacao == State.PlayerState.NORMAL) {
            excitacao = "n";
        }
        else if (inst.excitacao == State.PlayerState.LOW) {
            excitacao = "l";
        }
        else {
            excitacao = "-";
        }

        if (inst.zona == State.PlayerState.HIGH) {
            zona = "h";
        }
        else if (inst.zona == State.PlayerState.NORMAL) {
            zona = "n";
        }
        else if (inst.zona == State.PlayerState.LOW) {
            zona = "l";
        }
        else {
            zona = "-";
        }
        
        if (inst.IsDesempenho) {
            string s = "d" + desempenho + "z" + zona;
            TextEnable.SetHiperspaceText(s);
            TextEnable.SetHiperspaceText("Pressione Espaço");
        }
        else if (inst.IsAfetivo) {
            string s = "e" + excitacao+ "z" + zona;
            TextEnable.SetHiperspaceText(s);
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