using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System.IO;

public class BitalinoOpen : MonoBehaviour
{

    /*application dataPath => unity editor => assets*/
    /*application dataPath => windows => path to executable data folder*/


    Process bitalino;
    public bool IsRunning;
    public bool isFirstLevel;

    void Awake()
    {
        if (!isFirstLevel)
            Invoke("AcquireData", 2);
        Invoke("IsOpen", 5);
    }

    void Start()
    {
        bitalino = new Process();
        bitalino.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;/*dont open window*/
        bitalino.StartInfo.CreateNoWindow = true;/*do not show window*/
        bitalino.StartInfo.UseShellExecute = true;
        bitalino.StartInfo.FileName = Application.dataPath + "\\BitalinoReader.exe";
        if (isFirstLevel)
        {
            AcquireData();
        }
    }

    public void AcquireData()
    {
        bitalino.Start();
        IsRunning = true;
    }

    public void Kill()
    {
        bitalino.Kill();/*end bitalino process*/
    }

    public void IsOpen()
    {
        Invoke("IsOpen", 5);
        if (!bitalino.HasExited)
        {
        }
        else
        {
            IsRunning = false;
        }

    }
}


