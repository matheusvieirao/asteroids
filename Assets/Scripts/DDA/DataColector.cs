using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataColector : MonoBehaviour {

	public GameObject prefab;

    public string outputFileName;

	static public DataColector instance = null; 

	private TimerController deathTimer = new TimerController();
	public TimerController levelTimer =  new TimerController();

	private int numberOfLevelDeaths;

	private List<double> deathTimes =  new List<double>();


	 public int Deaths{
		set{numberOfLevelDeaths = value;}
		get {return numberOfLevelDeaths;}
	}


	void Start () {
		if (instance == null)
        {
			instance = prefab.GetComponent<DataColector> ();
            DataFile.SetFileName(outputFileName);
            DataFile.Init ();
        }

		DontDestroyOnLoad (gameObject);
	}


	void Update(){
		deathTimer.Run ();
		levelTimer.Run ();
	}

	public void AddDeath(){
		numberOfLevelDeaths++;
	}

	public void AddDeathTime(){
		deathTimes.Add (deathTimer.GetElapsedTime());
		deathTimer.Reset ();
	}

	public void ResetData() {
        int AsteroidCount = GameObject.FindGameObjectWithTag("LevelController").GetComponent<CreateAsteroids>().GetAsteroidCount();
        float maxSpeed = GameObject.FindGameObjectWithTag("LevelController").GetComponent<CreateAsteroids>().GetMaxSpeed();
        DataFile.AddToTxtLevel (AsteroidCount, maxSpeed, numberOfLevelDeaths.ToString(), levelTimer.GetElapsedTime().ToString());
		numberOfLevelDeaths = 0;
		deathTimes.Clear ();
		levelTimer.Reset ();
	}

	void OnApplicationQuit(){
		DataFile.WriteFile ();
	}

}
