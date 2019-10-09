﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Emotion;

public class GetPlayerData: MonoBehaviour{

	private List<string> lines =  new List<string>();

	public void BreakIntoLines(string file){
		string[] tempFile = file.Split('\n');
		foreach (string line in tempFile)
			lines.Add (line);
		lines.RemoveAt (lines.Count-1);//erase the blank line
	}
    
	public PlayerState GetEDAEmotion(){
		double[] LastMeasures =  new double[lines.Count];
        int aboveAvarage = 0;

		if (aboveAvarage > (double)LastMeasures.Length / 2)
			return PlayerState.STRESSED;
		else if (aboveAvarage == (double)LastMeasures.Length / 2)
			return PlayerState.NORMAL;
		else
			return PlayerState.BORED;
	}

	public int GetEDAValue(string line){
		return 1;
	}
		
}
