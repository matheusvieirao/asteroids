using UnityEngine;
using System.Collections;

public class CreateAsteroids : MonoBehaviour {

    public int AsteroidCount = 500;

    private float minSize = 1.75f;
    private float maxSize = 2.00f;

    public float maxSpeed = 5f;

    public static float minX = 20f;
    public static float maxX = 600f;
    public static float minY = -100f;
    public static float maxY = 100f;

    public bool isNormal;

    public GameObject asteroidPrefab;

    void Start() {

        int level = DataFile.GetCurrentLevel();
        int inicial_densidade = 350;
        float inicial_velocidade = 4.5f;
        int razao_densidade = 280;
        float razao_velocidade = 0.1f;
        AsteroidCount = inicial_densidade + level * razao_densidade;
        maxSpeed = inicial_velocidade + level * razao_velocidade;
        

        //if(isNormal)
        //AsteroidCount += DDAAply.instance.densityChange; // TODO criar DDA
        for (int i = 0; i < AsteroidCount; i++) {

            GameObject asteroid = (GameObject)Instantiate(asteroidPrefab, StartPosition(), Quaternion.Euler(0, 0, 0));

            float scale = Random.Range(minSize, maxSize);
            if (scale >= 5.0f && !asteroid.GetComponent<AsteroidType>().explosive)
                asteroid.GetComponent<AsteroidType>().indestructible = true;

            asteroid.transform.localScale = new Vector3(scale, scale, scale);

            AsteroidMovement movement = GameObject.FindObjectOfType<AsteroidMovement>();
            movement.Direction = AsteroidDirection();
        }
    }


    private Vector3 AsteroidDirection() {
        float angle = Random.Range(0, 360);
        float speed = Random.Range(0, maxSpeed * 2);
        float mx = speed * Mathf.Cos(Mathf.Deg2Rad * angle);
        float my = speed * Mathf.Sin(Mathf.Deg2Rad * angle);
        float mz = 0f;

        return new Vector3(mx, my, mz);
    }

    static public Vector3 StartPosition() {
        var x = Random.Range(minX, maxX);
        var y = Random.Range(minY, maxY);
        var z = 0f;
        return new Vector3(x, y, z);
    }

    public int GetAsteroidCount() {
        return AsteroidCount;
    }

    public float GetMaxSpeed() {
        return maxSpeed;
    }
    
}
