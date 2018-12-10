﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SensorChooser : MonoBehaviour {

	public Dropdown sensor;
	public List<string> Options =  new List<string>();

	void Start () {
		sensor.ClearOptions ();
		sensor.AddOptions (Options);
	}

	public void SaveSensor(){
		Debug.Log (sensor.options[sensor.value].text);
		PlayerPrefs.SetString ("Sensor",sensor.options[sensor.value].text);
	}
	
}
