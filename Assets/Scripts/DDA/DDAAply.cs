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
    
	
    private bool IsAfetivo = false;
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
	}
				
    //chamada quando se passa de nível (PassLevel)
	public void BalanceAtPassLevel(){
        CalculaZona();
        if (IsDesempenho) {
            CalculaDesempenho();
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
        else if (IsAfetivo) {
            CalculaExcitacao();
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

    //ajusta o nível quando morre (GameController)
    public void BalanceAtDeath() {

        CalculaZona();

        if (IsDesempenho) {
            if (desempenho == PlayerState.LOW) {
                if(zona == PlayerState.LOW) {
                    asteroidSpeed += -0.5f;
                }
                else if(zona == PlayerState.NORMAL) {
                    asteroidSpeed += -0.75f;
                }
                else { //(zona == PlayerState.HIGH) 
                    asteroidSpeed += -1f;
                }
            }
            else if (desempenho == PlayerState.NORMAL) {
                if (zona == PlayerState.LOW) {
                    asteroidSpeed += -0.25f;
                }
                else if (zona == PlayerState.NORMAL) {
                    asteroidSpeed += -0.5f;
                }
                else { //(zona == PlayerState.HIGH) 
                    asteroidSpeed += -0.75f;
                }
            }
            else { //(desempenho == PlayerState.HIGH) 
                if (zona == PlayerState.LOW) {
                    asteroidSpeed += 0f;
                }
                else if (zona == PlayerState.NORMAL) {
                    asteroidSpeed += -0.25f;
                }
                else { //(zona == PlayerState.HIGH) 
                    asteroidSpeed += -0.5f;
                }
            }
        }
        else if (IsAfetivo) {
            CalculaExcitacao();
            if (excitacao == PlayerState.HIGH) {
                if (zona == PlayerState.LOW) {
                    asteroidSpeed += -0.5f;
                }
                else if (zona == PlayerState.NORMAL) {
                    asteroidSpeed += -0.75f;
                }
                else { //(zona == PlayerState.HIGH) 
                    asteroidSpeed += -1f;
                }
            }
            else if (excitacao == PlayerState.NORMAL) {
                if (zona == PlayerState.LOW) {
                    asteroidSpeed += -0.25f;
                }
                else if (zona == PlayerState.NORMAL) {
                    asteroidSpeed += -0.5f;
                }
                else { //(zona == PlayerState.HIGH) 
                    asteroidSpeed += -0.75f;
                }
            }
            else { //(excitacao == PlayerState.LOW) 
                if (zona == PlayerState.LOW) {
                    asteroidSpeed += 0f;
                }
                else if (zona == PlayerState.NORMAL) {
                    asteroidSpeed += -0.25f;
                }
                else { //(zona == PlayerState.HIGH) 
                    asteroidSpeed += -0.5f;
                }
            }
        }
        else {
            Debug.Log("O jogo não está sendo balanceado...");
        }
    }

    public void CalculaDesempenho() {
        float mortes = (float) DataCenter.instance.numberOfLevelDeaths;
        float duracao = DataCenter.instance.GetDuracao();

        float limiarMortesAltoDesempenho = 0.03619733f * Mathf.Exp(0.43041275f * asteroidSpeed); //se tiver menos mortes que isso, é alto desempenho
        float limiarDuracaoAltoDesempenho =  24.12764375f * Mathf.Exp(0.08573745f * asteroidSpeed); //se durar menos tempo que isso, é alto desemepnho
        float limiarMortesBaixoDesempenho = 0.11379691f * Mathf.Exp(0.49376684f * asteroidSpeed); //se tiver mais mortes que isso, é baixo desempenho
        float limiarDuracaoBaixoDesempenho = 35.66058598f * Mathf.Exp(0.15140602f * asteroidSpeed); //se durar mais tempo que isso, é baixo desempenho
        
        Debug.Log("v" + asteroidSpeed);

        if (mortes < limiarMortesAltoDesempenho && duracao < limiarDuracaoAltoDesempenho) {
            desempenho = PlayerState.HIGH;
        }
        else if(mortes > limiarMortesBaixoDesempenho && duracao > limiarDuracaoBaixoDesempenho) {
            desempenho = PlayerState.LOW;
        }
        else {
            desempenho = PlayerState.NORMAL;
        }
    }

    public void CalculaExcitacao() {
        EDAStart.instance.callGetReadBigger(); //le os sinais e os salva em EDAStart.instance.sinais
        //printar aqui os sinais
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

    public string getTipoJogo() {
        if (IsDesempenho) {
            return "Desempenho";
        }
        else if (IsAfetivo) {
            return "Afetivo";
        }
        else if (IsHibrido) {
            return "Hibrido";
        }
        else {
            return "Erro ao selecionar o tipo de jogo";
        }
    }

    public void ChooseSensor(string sensor) {
        DataCenter.instance.setSensor(sensor);
        if (sensor == "AFT") {
            IsAfetivo = true;
            IsDesempenho = false;
            IsHibrido = false;
        }
        else if (sensor == "DSP") {
            IsAfetivo = false;
            IsDesempenho = true;
            IsHibrido = false;
        }
        else if (sensor == "HIB") {
            IsAfetivo = false;
            IsDesempenho = false;
            IsHibrido = true;
        }
    }

    public string getStringPlayerState(PlayerState ps) {
        if(ps == PlayerState.HIGH) {
            return "HIGH";
        }
        else if(ps == PlayerState.LOW) {
            return "LOW";
        }
        else {
            return "NORMAL";
        }
    }
}
