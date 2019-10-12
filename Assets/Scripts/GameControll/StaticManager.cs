using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticManager : MonoBehaviour {

    public GameObject seta;
    public GameObject cameraIcone;
    private bool mostrarSeta = false;

	void Awake () {
		TextEnable.Init ();
		TextEnable.EnableDistance ();
    }

    private void Start()
    {

        cameraIcone.SetActive(false);
        if (DataColector.instance.GetCurrentLevel() == 1) {
            mostrarSeta = true;
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Q)) {
            if(Input.GetKey(KeyCode.W)) {
                cameraIcone.SetActive(true);
            }
            else {
                cameraIcone.SetActive(false);
            }
        }
        if (mostrarSeta) {
            if (Input.GetKey("up"))
            {
                mostrarSeta = false;
            }
        }
        else {
            seta.SetActive(false);
        }
        
    }
    //deixar sempre desligado, se for level 1 ligar no awake

}
