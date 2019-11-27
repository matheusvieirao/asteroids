using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using State;

public class DDAAply : MonoBehaviour {

	public GameObject prefab;
	public static DDAAply instance;
		
	public float speedChange = 0;//idem for speed
	public float lastSpeedChange = 0;

	//private float EDA = 0; //eda values
    
	
    private bool IsEDA = false;
    private bool IsDesempenho = false;
    private bool IsHibrido = false;

    void Awake () {
		if (instance == null) {
			instance = prefab.GetComponent<DDAAply> ();
		}
        else if (instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad (gameObject);
		string sensor = PlayerPrefs.GetString ("Sensor");
        if (sensor == "EDA") {
            IsEDA = true;
            IsDesempenho = false;
            IsHibrido = false;
        }
        else if (sensor == "DSP") {
            IsEDA = false;
            IsDesempenho = true;
            IsHibrido = false;
        }
        else if (sensor == "HIB") {
            IsEDA = false;
            IsDesempenho = false;
            IsHibrido = true;
        }
	}
				
    //chamada quando se passa de nível (PassLevel)
	public void BalanceAtPassLevel(){
        //a velocidade no nivel 1 é de 1-2, no nivel 2 de 2-3 e no nivel 10 de 10-11 (tudo em float).
        int mortes = DataCenter.instance.numberOfLevelDeaths;
        int duracao = (int) (DataCenter.instance.finalLevelTime - DataCenter.instance.initialLevelTime);
        PlayerState excitacao = DataCenter.instance.excitacao;
        PlayerState desempenho = DataCenter.instance.desempenho;

        //Descobrir, para se usar no próximo nível, se o desempenho do jogador foi alto, médio ou baixo.
        if (IsDesempenho) {
            //ver como a velocidade é alterada ao iniciar o nivel.
            //alterar DataCenter.instance.desempenho
            //alterar o speedChange
        }
        //Descobrir, para se usar no próximo nível, se a excitação do jogador foi alta, média ou baixa
        else if (IsEDA) {

            //alterar DataCenter.instance.excitacao
            //alterar o speedChange. 
        }
        else {
            Debug.Log("O jogo não estã sendo balanceado");
        }



        /*
        float mudanca_gradual = mortes / 20;
        if (mortes < 2) {
            if (excitacao == PlayerState.HIGH)
                lastSpeedChange -= 0.2f;
            else if (excitacao == PlayerState.NORMAL)
                lastSpeedChange += 0;
            else if (excitacao == PlayerState.LOW)
                lastSpeedChange += 0.8f;
        }
        else if (mortes < 4) {
            if (excitacao == PlayerState.HIGH)
                lastSpeedChange -= mudanca_gradual + 0.2f; // se morreu 3, -0,35
            else if (excitacao == PlayerState.NORMAL)
                lastSpeedChange += 0;
            else if (excitacao == PlayerState.LOW)
                lastSpeedChange += 0.2f;
        }
        else { //se morreu 5
		    if(excitacao == PlayerState.HIGH)
			    lastSpeedChange -= mudanca_gradual + 0.3f; //-0,55
		    else if(excitacao == PlayerState.NORMAL)
			    lastSpeedChange -= mudanca_gradual; //-0,25
		    else if(excitacao == PlayerState.LOW)
			    lastSpeedChange -= 0.1f;
        }

		speedChange = lastSpeedChange;*/
	}

    //ajusta o nível quando morre
	public void BalanceAtDeath() {
        int mortes = DataCenter.instance.numberOfLevelDeaths;
        float mudanca_gradual = mortes / 20;
        speedChange = lastSpeedChange+ (-1)* mudanca_gradual;
	}

}
