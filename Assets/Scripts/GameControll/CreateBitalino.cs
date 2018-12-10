using UnityEngine;
using System.Collections;

public class CreateBitalino : MonoBehaviour {

	void Start () {
        var bitalino = Resources.Load("Prefabs/BITalino");
        var line = Resources.Load("Prefabs/Line1");
        var gameObject = (GameObject)Instantiate(bitalino);                
        var gameObject2 = (GameObject)Instantiate(line);
    }
	
}
