using System.Collections;
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

	public PlayerState GetECGEmotion(){
		double[] LastMeasures =  new double[lines.Count];
		int aboveAvarage = 0;
		double avarage = 0;
		for (int i = 0; i < LastMeasures.Length ; i++){			
			LastMeasures[i] = MeasureToVoltageECG(GetECGValue(lines[i]));
			avarage = LastMeasures [i];
		}

		avarage = avarage / LastMeasures.Length;

		for (int i = 0; i < LastMeasures.Length; i++) {
			if (LastMeasures [i] > avarage)
				aboveAvarage++;
		}

//		LastMeasures[0] = MeasureToVoltageECG(GetECGValue(lines[lines.Count-2]));
//		LastMeasures[1] = MeasureToVoltageECG(GetECGValue(lines[lines.Count-1]));
//		if (LastMeasures [1] > LastMeasures [0])
//			return PlayerState.STRESSED;
//		else if (LastMeasures [1] == LastMeasures [0])
//			return PlayerState.NORMAL;
//		return PlayerState.BORED;
		if (aboveAvarage > (double)LastMeasures.Length / 2)
			return PlayerState.STRESSED;
		else if (aboveAvarage == (double)LastMeasures.Length / 2)
			return PlayerState.NORMAL;
		else
			return PlayerState.BORED;
	}

	public int GetECGValue(string line){
		string ECG = line.Split (',') [2];
		string ecgValue = ECG.Split (':')[1];
		int value = int.Parse (ecgValue);
		return value;
	}


	public PlayerState GetEDAEmotion(){
//		double[] LastMeasures =  new double[2];
//		LastMeasures[0] = MeasureToCondutanceEDA(GetEDAValue(lines[lines.Count-2]));
//		LastMeasures[1] = MeasureToCondutanceEDA(GetEDAValue(lines[lines.Count-1]));
		double[] LastMeasures =  new double[lines.Count];
		int aboveAvarage = 0;
		double avarage = 0;
		for (int i = 0; i < LastMeasures.Length ; i++){
			LastMeasures[i] = MeasureToCondutanceEDA(GetEDAValue(lines[i]));
			avarage = LastMeasures [i];
		}

		avarage = avarage / LastMeasures.Length;

		for (int i = 0; i < LastMeasures.Length; i++) {
			if (LastMeasures [i] > avarage)
				aboveAvarage++;
		}

		if (aboveAvarage > (double)LastMeasures.Length / 2)
			return PlayerState.STRESSED;
		else if (aboveAvarage == (double)LastMeasures.Length / 2)
			return PlayerState.NORMAL;
		else
			return PlayerState.BORED;
	}

	public int GetEDAValue(string line){
		string ECG = line.Split (',') [1];
		string ecgValue = ECG.Split (':')[1];
		int value = int.Parse (ecgValue);
		return value;
	}

	/// <summary>
	/// Measure to voltage ECG.
	/// this function return the value in milivolts of your heart hate
	/// </summary>
	/// <returns>The to voltage EC.</returns>
	/// <param name="measure">Measure.</param>

	public float MeasureToVoltageECG(float measure){
		float gain = 1100;
		float VCC = 3.3f;
		return 1000*((measure) / 1024 - 1 / 2) * VCC / gain;
	}

	/// <summary>
	/// Measure to condutance EDA.
	/// this function returns your skin condutance in Micro siemens
	/// </summary>
	/// <returns>The to condutance ED.</returns>
	/// <param name="measure">Measure.</param>
	public float MeasureToCondutanceEDA(float measure){
		float VCC = 3.3f;
		return ((measure)*VCC / 1024 ) /0.132f;
	
	}
		
}
