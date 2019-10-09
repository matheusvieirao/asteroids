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

	private float EDA = 0; //eda values

	public bool isFirstLevel = false;

	private PlayerState emotion;
	private bool IsEDA;

	void Awake () {
		if (instance == null) {
			instance = prefab.GetComponent<DDAAply> ();
			instance.emotion = PlayerState.NORMAL;
		}
        else if (instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad (gameObject);
		string sensor = PlayerPrefs.GetString ("Sensor");
		if (sensor == "EDA")
			IsEDA = true;
	}

	public void BalanceWithEmotion() {
        playerSignals = GameObject.Find("Game Controller").GetComponent<GetPlayerData>();
        string playerData = File.ReadAllText("FisiologicalData.txt");
        playerSignals.BreakIntoLines(playerData);

        print("BalanceWithEmotion - IsEDA: " + IsEDA);

        if (IsEDA)
            emotion = playerSignals.GetEDAEmotion();
        else
            emotion = PlayerState.NORMAL;
    }
				
    //apenas quando JumpLevel ou PassLevel
	public void SpeedBalanceNextLevel(){
        //a velocidade no nivel 1 é de 1-2, no nivel 2 de 2-3 e no nivel 10 de 10-11 (tudo em float).
        int mortes = DataColector.instance.numberOfLevelDeaths;
        float mudanca_gradual = mortes / 20;
        //primeiro verifica o quanto se morreu e dependendo do quanto morreu, se ajusta baseado no EDA
        if (mortes < 2) {
            if (emotion == PlayerState.STRESSED)
                lastSpeedChange -= 0.2f;
            else if (emotion == PlayerState.NORMAL)
                lastSpeedChange += 0;// 0.6f;
            else if (emotion == PlayerState.BORED)
                lastSpeedChange += 0.8f;
        }
        else if (mortes < 4) {
            if (emotion == PlayerState.STRESSED)
                lastSpeedChange -= mudanca_gradual + 0.2f; // se morreu 3, -0,35
            else if (emotion == PlayerState.NORMAL)
                lastSpeedChange += 0;// GradualSpeedChange(); // 0,25
            else if (emotion == PlayerState.BORED)
                lastSpeedChange += 0.2f;
        }
        else { //se morreu 5
		    if(emotion==PlayerState.STRESSED)
			    lastSpeedChange -= mudanca_gradual + 0.3f; //-0,55
		    else if(emotion==PlayerState.NORMAL)
			    lastSpeedChange -= mudanca_gradual; //-0,25
		    else if(emotion==PlayerState.BORED)
			    lastSpeedChange += mudanca_gradual; //+0,25 //perigo com essa pq se o eda nao estiver calibrado direito, simplemesnte só vai ficar impossivel dps de um tempo
        }

		speedChange = lastSpeedChange;
	}

    //apenas quando morre
	public void SpeedBalanceCurrentLevel() {
        int mortes = DataColector.instance.numberOfLevelDeaths;
        float mudanca_gradual = mortes / 20;
        speedChange = lastSpeedChange+ (-1)* mudanca_gradual;
	}

}
