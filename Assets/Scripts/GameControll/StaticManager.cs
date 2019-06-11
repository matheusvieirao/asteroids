using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticManager : MonoBehaviour {

    public GameObject seta;
    private bool mostrarSeta = false;

	void Awake () {
		TextEnable.Init ();
		TextEnable.EnableDistance ();
    }

    private void Start()
    {

        if (DataFile.GetCurrentLevel() == 1) {
            mostrarSeta = true;
        }
    }

    private void Update()
    {
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
