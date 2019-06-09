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

	public static void AddToTxt(string deaths, string time){
		text +=  "Level" + currentLevel.ToString() + "|" + deaths + "|" + time +"\n";
		currentLevel++;
	}

	public static void WriteFile(){
		File.WriteAllText (fileName+".txt",text);
    }


}
