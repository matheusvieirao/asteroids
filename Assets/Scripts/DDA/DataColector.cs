using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.IO;
using System;

// Possui uma unica instancia. 
// É usada para se passar dados dados entre diferentes cenas do jogo.
// Está atrelada ao prefab "DataReceiver"
// No final escreve os dados no JSON representado por pela variavel df (DataFile)
public class DataColector : MonoBehaviour {

    public GameObject prefab;

    static public DataColector instance = null;
    public static int currentLevel;
    public string nomeCompleto;
    public int numberOfLevelDeaths;
    DataFile df;

    void Start() {
        if (instance == null)
        {
            instance = prefab.GetComponent<DataColector>();
            df = new DataFile();
            currentLevel = 1;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }


    void Update() {
        long agora = System.DateTime.Now.Ticks;

        if (Input.GetKey(KeyCode.Q)){
            if (Input.GetKeyDown(KeyCode.W)) {
                df.addFlagEmpatica(agora);
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            df.addApertouUp(agora);
        }
        if (Input.GetKeyUp(KeyCode.UpArrow)) {
            df.addSoltouUp(agora);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            df.addApertouDown(agora);
        }
        if (Input.GetKeyUp(KeyCode.DownArrow)) {
            df.addSoltouDown(agora);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            df.addApertouLeft(agora);
        }
        if (Input.GetKeyUp(KeyCode.LeftArrow)) {
            df.addSoltouLeft(agora);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            df.addApertouRight(agora);
        }
        if (Input.GetKeyUp(KeyCode.RightArrow)) {
            df.addSoltouRight(agora);
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            df.addTiro(agora);
        }
	}

	public void AddDeath(){
		numberOfLevelDeaths++;
        df.addMorte(System.DateTime.Now.Ticks);
	}

	public void AddToOutputLevel(bool venceu) {
        int AsteroidCount = GameObject.FindGameObjectWithTag("LevelController").GetComponent<CreateAsteroids>().GetAsteroidCount();
        float minSpeed = GameObject.FindGameObjectWithTag("LevelController").GetComponent<CreateAsteroids>().GetMinSpeed();
        float maxSpeed = GameObject.FindGameObjectWithTag("LevelController").GetComponent<CreateAsteroids>().GetMaxSpeed();
        df.AddToOutputFileLevel (AsteroidCount, minSpeed, maxSpeed, venceu);
		numberOfLevelDeaths = 0;
    }

    public void AddToOutputPerguntas(string respostaDificuldade, string respostaTedio, string respostaFrustracao, string respostaDiversao, string respostaInputText) {
        df.AddToOutputFilePerguntas(respostaDificuldade, respostaTedio, respostaFrustracao, respostaDiversao, respostaInputText);
    }

    public void Write() {
        string jsonstring = JsonUtility.ToJson(df, true);
        File.WriteAllText("Output "+nomeCompleto+".txt", jsonstring);
    }

    public void SetNomeCompleto(string nome_str, string sobrenome_str) {
        nomeCompleto = nome_str + " " + sobrenome_str;
        df.SetNomeCompleto(nomeCompleto);
    }

    public void SetTempoInicial() {
        df.SetTempoInicial(System.DateTime.Now.Ticks);
    }

    public void SetTempoFinal() {
        df.SetTempoFinal(System.DateTime.Now.Ticks);
    }

    public int GetCurrentLevel() {
        return currentLevel;
    }

    public void AddLevel() {
        currentLevel++;
    }

    public void AddLevelToJson() {
        df.AddLevelToJson();
    }

}
