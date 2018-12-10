using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticManager : MonoBehaviour {

	void Awake () {
		TextEnable.Init ();
		TextEnable.EnableDistance ();
	}


}
