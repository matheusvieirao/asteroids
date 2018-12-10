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

	public static void AddDeaths(string deaths){
		text += deaths;
	}

	public static void AddTime(string time){
		text +="|" +time +"| level"+currentLevel.ToString()+"\n";
		currentLevel++;
	}

	public static void WriteFile(){
		File.WriteAllText (fileName+".txt",text);
	}


}
