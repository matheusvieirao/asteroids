using UnityEngine;
using System.Collections;

public class CreateAsteroids : MonoBehaviour {

	public int AsteroidCount = 500;
    
	public float minSize = 0.75f;
    public float maxSize = 0.75f;
    public float minSpeed = -5f;
    public float maxSpeed = 5f;
	public bool isNormal;

	public GameObject asteroidPrefab;

	void Start(){
		if(isNormal)
			AsteroidCount += DDAAply.instance.densityChange;
		for (int i = 0; i < AsteroidCount; i++){			

			GameObject asteroid = (GameObject)Instantiate(asteroidPrefab, AsteroidStartPosition.StartPosition (), Quaternion.Euler(0,0,0));
            
			float scale = Random.Range(minSize, maxSize);
			if (scale >= 5.0f && !asteroid.GetComponent<AsteroidType> ().explosive)
				asteroid.GetComponent<AsteroidType> ().indestructible = true;

			asteroid.transform.localScale = new Vector3(scale, scale, scale);

			AsteroidMovement movement = GameObject.FindObjectOfType<AsteroidMovement>();
			movement.Direction = AsteroidDirection();
        }
	}
		

	private Vector3 AsteroidDirection(){
		float mx = Random.Range(minSpeed, maxSpeed);
		float my = Random.Range(minSpeed, maxSpeed);
		float mz = 0f;

		float mmx = Random.Range(0, 2);
		float mmy = Random.Range(0, 2);

		if (mmx == 0)
			mx *= -1;
		
		if (mmy == 0)
			my *= -1;
		
		return new Vector3(mx, my, mz);
	}
				
}
