using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SairDoJogo : MonoBehaviour {

    public GameObject camera;

    private void Start() {

        camera.SetActive(false);

    }
    private void Update() {
        if (Input.GetKey(KeyCode.Q)) {
            if (Input.GetKeyDown(KeyCode.W)) {
                DataFile.addFlagEmpatica(System.DateTime.Now.Ticks);
            }
            if (Input.GetKey(KeyCode.W)) {
                camera.SetActive(true);
            }
            else {
                camera.SetActive(false);
            }
        }
    }

    public void Sair() {
        //Application.Quit();
    }

    void OnApplicationQuit() {
        if (SceneManager.GetActiveScene().name.Equals("Fim do Jogo")) {
            DataFile.WriteFile();
        }
    }
}
