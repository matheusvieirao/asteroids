using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// JSON que vai ser gerado, possui [System.Serializable] e não implementa MonoBehaviour justamente por causa disso
// contém as informações de cada level
[System.Serializable]
public class DataFileLevel
{
    public int numeroDeAsteroids;
    public float velocidadeMinimaDosAsteroids;
    public float velocidadeMaximaDosAsteroids;
    public long tempoInicial = 0; //em ticks
    public long tempoFinal; //em ticks
    public float tempoDuracao; //em segundos
    public float tempoPorVida;

    public string desempenho;
    public string excitacao;
    public string zona;

    public bool venceu;

    public int totalDeMortes;
    public List<long> mortes = new List<long>();

    public int totalDeTiros;
    public float percentualUp;
    public float percentualDown;
    public float percentualLeft;
    public float percentualRight;
    public List<long> tiros = new List<long>();
    public List<long> apertouUp = new List<long>();
    public List<long> soltouUp = new List<long>();
    public List<long> apertouDown = new List<long>();
    public List<long> soltouDown = new List<long>();
    public List<long> apertouLeft = new List<long>();
    public List<long> soltouLeft = new List<long>();
    public List<long> apertouRight = new List<long>();
    public List<long> soltouRight = new List<long>();

    public string dificuldade;
    public string tedio;
    public string frustracao;
    public string diversao;
    public string opiniao;
}
