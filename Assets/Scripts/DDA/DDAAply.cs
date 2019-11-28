using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using State;

public class DDAAply : MonoBehaviour {

	public GameObject prefab;
	public static DDAAply instance;
		
    public float asteroidSpeed = 1f; //velocidade dos asteroids

    public PlayerState excitacao; //LOW, NORMAL ou HIGH
    public PlayerState desempenho; //LOW, NORMAL ou HIGH
    public PlayerState zona; //LOW(amena), NORMAL(otima) ou HIGH(intensa)

	//private float EDA = 0; //eda values
    
	
    private bool IsEDA = false;
    private bool IsDesempenho = false;
    private bool IsHibrido = false;

    void Awake () {
		if (instance == null) {
			instance = prefab.GetComponent<DDAAply> ();
            excitacao = PlayerState.NORMAL;
            desempenho = PlayerState.NORMAL;
            zona = PlayerState.NORMAL;
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

        CalculaDesempenho();
        CalculaZona();

        if (IsDesempenho) {
            if (desempenho == PlayerState.LOW) {
                if (zona == PlayerState.LOW) {
                    Debug.Log("desL zonaL");
                    asteroidSpeed += 1f;
                }
                else if (zona == PlayerState.NORMAL) {
                    Debug.Log("desL zonaN");
                    asteroidSpeed += +0.5f;
                }
                else { //(zona == PlayerState.HIGH) 
                    Debug.Log("desL zonaH");
                    asteroidSpeed += 0;
                }
            }
            else if (desempenho == PlayerState.NORMAL) {
                if (zona == PlayerState.LOW) {
                    Debug.Log("desN zonaL");
                    asteroidSpeed += 1.5f;
                }
                else if (zona == PlayerState.NORMAL) {
                    Debug.Log("desN zonaN");
                    asteroidSpeed += 1;
                }
                else { //(zona == PlayerState.HIGH) 
                    Debug.Log("desN zonaH");
                    asteroidSpeed += 0.5f;
                }
            }
            else { //(desempenho == PlayerState.HIGH) 
                if (zona == PlayerState.LOW) {
                    Debug.Log("desH zonaL");
                    asteroidSpeed += 2f;
                }
                else if (zona == PlayerState.NORMAL) {
                    Debug.Log("desHzonaN");
                    asteroidSpeed += 1.5f;
                }
                else { //(zona == PlayerState.HIGH) 
                    Debug.Log("desH zonaH");
                    asteroidSpeed += 1f;
                }
            }
        }
        else if (IsEDA) {
            if (excitacao == PlayerState.LOW) {
                if (zona == PlayerState.LOW) {
                    asteroidSpeed += 1f;
                }
                else if (zona == PlayerState.NORMAL) {
                    asteroidSpeed += +0.5f;
                }
                else { //(zona == PlayerState.HIGH) 
                    asteroidSpeed += 0;
                }
            }
            else if (excitacao == PlayerState.NORMAL) {
                if (zona == PlayerState.LOW) {
                    asteroidSpeed += 1.5f;
                }
                else if (zona == PlayerState.NORMAL) {
                    asteroidSpeed += 1;
                }
                else { //(zona == PlayerState.HIGH) 
                    asteroidSpeed += 0.5f;
                }
            }
            else { //(excitacao == PlayerState.HIGH) 
                if (zona == PlayerState.LOW) {
                    asteroidSpeed += 2f;
                }
                else if (zona == PlayerState.NORMAL) {
                    asteroidSpeed += 1.5f;
                }
                else { //(zona == PlayerState.HIGH) 
                    asteroidSpeed += 1f;
                }
            }
        }
        else {
            Debug.Log("O jogo não está sendo balanceado");
        }
    }

    //ajusta o nível quando morre
    public void BalanceAtDeath() {

        CalculaZona();

        if (IsDesempenho) {
            if (desempenho == PlayerState.LOW) {
                if(zona == PlayerState.LOW) {
                    asteroidSpeed += -0.25f;
                }
                else if(zona == PlayerState.NORMAL) {
                    asteroidSpeed += -0.375f;
                }
                else { //(zona == PlayerState.HIGH) 
                    asteroidSpeed += -0.5f;
                }
            }
            else if (desempenho == PlayerState.NORMAL) {
                if (zona == PlayerState.LOW) {
                    asteroidSpeed += -0.125f;
                }
                else if (zona == PlayerState.NORMAL) {
                    asteroidSpeed += -0.25f;
                }
                else { //(zona == PlayerState.HIGH) 
                    asteroidSpeed += -0.375f;
                }
            }
            else { //(desempenho == PlayerState.HIGH) 
                if (zona == PlayerState.LOW) {
                    asteroidSpeed += 0f;
                }
                else if (zona == PlayerState.NORMAL) {
                    asteroidSpeed += -0.125f;
                }
                else { //(zona == PlayerState.HIGH) 
                    asteroidSpeed += -0.25f;
                }
            }
        }
        else if (IsEDA) {
            if (excitacao == PlayerState.HIGH) {
                if (zona == PlayerState.LOW) {
                    asteroidSpeed += -0.25f;
                }
                else if (zona == PlayerState.NORMAL) {
                    asteroidSpeed += -0.375f;
                }
                else { //(zona == PlayerState.HIGH) 
                    asteroidSpeed += -0.5f;
                }
            }
            else if (excitacao == PlayerState.NORMAL) {
                if (zona == PlayerState.LOW) {
                    asteroidSpeed += -0.125f;
                }
                else if (zona == PlayerState.NORMAL) {
                    asteroidSpeed += -0.25f;
                }
                else { //(zona == PlayerState.HIGH) 
                    asteroidSpeed += -0.375f;
                }
            }
            else { //(excitacao == PlayerState.LOW) 
                if (zona == PlayerState.LOW) {
                    asteroidSpeed += 0f;
                }
                else if (zona == PlayerState.NORMAL) {
                    asteroidSpeed += -0.125f;
                }
                else { //(zona == PlayerState.HIGH) 
                    asteroidSpeed += -0.25f;
                }
            }
        }
        else {
            Debug.Log("O jogo não está sendo balanceado");
        }
    }

    public void CalculaDesempenho() {
        float mortes = (float) DataCenter.instance.numberOfLevelDeaths;
        float duracao = DataCenter.instance.GetDuracao();
        //float velocidade = ;
        float velocidade = asteroidSpeed;

        float limiarMortesAltoDesempenho = 0.03619733f * Mathf.Exp(0.43041275f * velocidade); //se tiver menos mortes que isso, é alto desempenho
        float limiarDuracaoAltoDesempenho =  24.12764375f * Mathf.Exp(0.08573745f * velocidade); //se durar menos tempo que isso, é alto desemepnho
        float limiarMortesBaixoDesempenho = 0.11379691f * Mathf.Exp(0.49376684f * velocidade); //se tiver mais mortes que isso, é baixo desempenho
        float limiarDuracaoBaixoDesempenho = 35.66058598f * Mathf.Exp(0.15140602f * velocidade); //se durar mais tempo que isso, é baixo desempenho
        
        Debug.Log("v:" + velocidade);

        if (mortes < limiarMortesAltoDesempenho && duracao < limiarDuracaoAltoDesempenho) {
            desempenho = PlayerState.HIGH;
        }
        else if(mortes > limiarMortesBaixoDesempenho && duracao > limiarDuracaoBaixoDesempenho) {
            desempenho = PlayerState.LOW;
        }
        else {
            desempenho = PlayerState.NORMAL;
        }

        Mathf.Exp(5);

    }

    public void CalculaZona() {
        int mortes = DataCenter.instance.numberOfLevelDeaths;
        float duracao = DataCenter.instance.GetDuracao();

        if (mortes < 4 && duracao < 67f) {
            zona = PlayerState.LOW;
        }
        else if (mortes > 4 && duracao > 77f) {
            zona = PlayerState.HIGH;
        }
        else {
            zona = PlayerState.NORMAL;
        }
    }

    public float getAsteroidSpeed() {
        return asteroidSpeed;
    }

}
