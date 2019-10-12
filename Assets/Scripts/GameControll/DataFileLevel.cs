using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DataFileLevel
{
    public long tempoInicial = 0; //em ticks
    public long tempoFinal; //em ticks
    public float tempoDuracao; //em segundos
    public List<long> apertouUp = new List<long>();
    public List<long> soltouUp = new List<long>();
    public List<long> apertouDown = new List<long>();
    public List<long> soltouDown = new List<long>();
    public List<long> apertouLeft = new List<long>();
    public List<long> soltouLeft = new List<long>();
    public List<long> apertouRight = new List<long>();
    public List<long> soltouRight = new List<long>();
    public List<long> tiros = new List<long>();
    public List<long> mortes = new List<long>();
    public float percentualUp;
    public float percentualDown;
    public float percentualLeft;
    public float percentualRight;

    public DataFileLevel() {
    }
}
