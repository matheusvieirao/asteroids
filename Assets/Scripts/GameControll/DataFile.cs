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

	public static void AddToTxtLevel(string deaths, string time){
		text +=  "Level " + currentLevel.ToString() + " - Mortes: " + deaths + " | Tempo: " + time +"\n";
		currentLevel++;
	}

    public static void AddToTxtPerguntas2(string dificuldade, string tedio, string frustracao, string diversao)
    {
        text += "Perguntas 2 - Dificuldade: " + dificuldade + " | Tedio: " + tedio + " | Frustracao: " + frustracao + " | Diversao: " + diversao + "\n";
    }

	public static void WriteFile(){
		File.WriteAllText (fileName+".txt",text);
    }


}
