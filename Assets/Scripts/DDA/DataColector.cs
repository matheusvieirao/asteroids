using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataColector : MonoBehaviour {

	public GameObject prefab;

	static public DataColector instance = null; 

	private TimerController timer = new TimerController();
	public TimerController levelTimer =  new TimerController();

	private int numberOfLevelDeaths;

	private List<double> deathTimes =  new List<double>();


	 public int Deaths{
		set{numberOfLevelDeaths = value;}
		get {return numberOfLevelDeaths;}
	}


	void Start () {
		if (instance == null)
			instance = prefab.GetComponent<DataColector> ();

		DontDestroyOnLoad (gameObject);
	}


	void Update(){
		timer.Run ();
		levelTimer.Run ();
	}

	public void AddDeath(){
		numberOfLevelDeaths++;
	}

	public void AddDeathTime(){
		deathTimes.Add (timer.GetElapsedTime());
		timer.Reset ();
	}

	public void ResetData(){
		DataFile.AddDeaths (numberOfLevelDeaths.ToString());
		numberOfLevelDeaths = 0;
		DataFile.AddTime (levelTimer.GetElapsedTime ().ToString ());
		deathTimes.Clear ();
		levelTimer.Reset ();
	}

	void OnApplicationQuit(){
		DataFile.WriteFile ();
	}

}
