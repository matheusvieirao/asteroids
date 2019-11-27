using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Emotion;

public class DDAAply : MonoBehaviour {

	public GameObject prefab;
	public static DDAAply instance;
		
	public GetPlayerData playerSignals;
	public float speedChange = 0;//idem for speed
	public float lastSpeedChange = 0;

	//private float EDA = 0; //eda values
    
	private PlayerExcitement excitement; //LOW, NORMAL ou HIGH
    private bool IsEDA = false;
    private bool IsDesempenho = false;
    private bool IsHibrido = false;

    void Awake () {
		if (instance == null) {
			instance = prefab.GetComponent<DDAAply> ();
			instance.excitement = PlayerExcitement.NORMAL;
		}
        else if (instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad (gameObject);
		string sensor = PlayerPrefs.GetString ("Sensor");
        if (sensor == "EDA")
            IsEDA = true;
        else if (sensor == "DSP")
            IsDesempenho = true;
        else if (sensor == "HIB")
            IsHibrido = true;
	}
				
    //chamada quando se passa de nível (PassLevel)
	public void BalanceAtPassLevel(){
        //a velocidade no nivel 1 é de 1-2, no nivel 2 de 2-3 e no nivel 10 de 10-11 (tudo em float).
        int mortes = DataColector.instance.numberOfLevelDeaths;
        float mudanca_gradual = mortes / 20;
        if (mortes < 2) {
            if (excitement == PlayerExcitement.HIGH)
                lastSpeedChange -= 0.2f;
            else if (excitement == PlayerExcitement.NORMAL)
                lastSpeedChange += 0;
            else if (excitement == PlayerExcitement.LOW)
                lastSpeedChange += 0.8f;
        }
        else if (mortes < 4) {
            if (excitement == PlayerExcitement.HIGH)
                lastSpeedChange -= mudanca_gradual + 0.2f; // se morreu 3, -0,35
            else if (excitement == PlayerExcitement.NORMAL)
                lastSpeedChange += 0;
            else if (excitement == PlayerExcitement.LOW)
                lastSpeedChange += 0.2f;
        }
        else { //se morreu 5
		    if(excitement==PlayerExcitement.HIGH)
			    lastSpeedChange -= mudanca_gradual + 0.3f; //-0,55
		    else if(excitement==PlayerExcitement.NORMAL)
			    lastSpeedChange -= mudanca_gradual; //-0,25
		    else if(excitement==PlayerExcitement.LOW)
			    lastSpeedChange -= 0.1f;
        }

		speedChange = lastSpeedChange;
	}

    //ajusta o nível quando morre
	public void BalanceAtDeath() {
        int mortes = DataColector.instance.numberOfLevelDeaths;
        float mudanca_gradual = mortes / 20;
        speedChange = lastSpeedChange+ (-1)* mudanca_gradual;
	}

}
