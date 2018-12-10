using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipFire : MonoBehaviour {

	private ShipFireWeapon shipFireWeapon;

	void Start () {
		shipFireWeapon = GetComponent<ShipFireWeapon>();
	}

	void Update () {
		if(!PauseGame.isGamePaused)
			HandleFireWeapons ();
	}

	private void HandleFireWeapons(){
		if (HasPressedShoot())
			shipFireWeapon.FirePrimaryWeapon();
	}

	private bool HasPressedShoot(){
		return Input.GetKeyDown (KeyCode.Space);
	}
}
