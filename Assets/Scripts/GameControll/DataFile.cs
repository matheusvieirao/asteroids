using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DataFile : MonoBehaviour {

	public static string fileName;
	public static string text;
	public static int currentLevel;


	public static void Init(){
		text = "";
		currentLevel = 1;
	}

	public static void SetFileName(string file){
		fileName = file;
	}

	public static void AddToTxtLevel(int asteroidsCount, float maxSpeed, string deaths, string time){
        float tm = float.Parse(time) / ((float)int.Parse(deaths)+1); //tempo medio por vida
        text +=  "Level " + currentLevel.ToString() +  " - Nº Asteroids: " + asteroidsCount + " | Velocidade: " + maxSpeed + " | Mortes: " + deaths + " | Tempo: " + time +" | Tempo por vida: " + tm;
    }

    public static void AddToTxtPerguntas2(string dificuldade, string tedio, string frustracao, string diversao)
    {
        text += " | Dificuldade: " + dificuldade + " | Tedio: " + tedio + " | Frustracao: " + frustracao + " | Diversao: " + diversao + "\n";
        currentLevel++;
    }

	public static void WriteFile(){
		File.WriteAllText (fileName+".txt",text);
    }

    public static int GetCurrentLevel ()
    {
        return currentLevel;
    }

}
