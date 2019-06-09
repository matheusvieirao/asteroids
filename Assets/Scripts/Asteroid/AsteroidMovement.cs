using UnityEngine;
using System.Collections;

public class AsteroidMovement : MonoBehaviour {

	public int verticalLimit = 850;
	public int horizontalLimit = 100;
	public float speed = 0.4f;
	private const float MINIMUNSPEED = 0.1f;
	public Vector3 Direction;

	void Start(){
		speed += DDAAply.instance.speedChange;
		if (speed < MINIMUNSPEED)
			speed = 0.1f;
	}

	void Update (){
		if (!PauseGame.isGamePaused){
			KeepInVerticalLimits ();
			KeepInHorizontalLimits ();
			gameObject.transform.Translate (TranslateAsteroid ());
		}
	}
		
	private Vector3 TranslateAsteroid(){
		float x = Direction.x * Time.deltaTime*speed;        
		float y = Direction.y * Time.deltaTime*speed;
		float z = Direction.z;
		return new Vector3(x, y, z);
	}

	private void KeepInVerticalLimits(){
		if (transform.position.x > verticalLimit)
			Direction.x = -Direction.x;
	}

	private void KeepInHorizontalLimits(){
		if (transform.position.y > horizontalLimit || transform.position.y < -horizontalLimit)
			Direction.y = -Direction.y;	
	}


}

