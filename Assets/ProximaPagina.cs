﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProximaPagina : MonoBehaviour
{
    public string nextScene;

    public void passLevel()
    {
        SceneManager.LoadScene(nextScene);
    }
}
