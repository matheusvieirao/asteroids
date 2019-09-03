﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

[System.Serializable]
public class DataFile : MonoBehaviour {

	public static string fileName;
	public static string text;

	public static int currentLevel;
    public static long tempoInicial=0; //em ticks
    public static long tempoFinal; //em ticks
    public static float tempoDuracao; //em segundos
    public static List<long> botaoFlagEmpatica = new List<long>();
    public static List<long> apertouUp = new List<long>();
    public static List<long> soltouUp = new List<long>();
    public static List<long> apertouDown = new List<long>();
    public static List<long> soltouDown = new List<long>();
    public static List<long> apertouLeft = new List<long>();
    public static List<long> soltouLeft = new List<long>();
    public static List<long> apertouRight = new List<long>();
    public static List<long> soltouRight = new List<long>();
    public static List<long> tiros = new List<long>();
    public static List<long> mortes = new List<long>();
    private static string nomeCompleto;
    private static float percentualUp;
    private static float percentualDown;
    private static float percentualLeft;
    private static float percentualRight;

    public static void Init(){
        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US"); //para imprimir separar os floats com '.' e nao ','
        text = "{\n";
        text += "\t\"nome\": \"" + nomeCompleto + "\",\n";
        currentLevel = 1;
	}

	public static void SetFileName(string fName){
		fileName = fName + " " + nomeCompleto;
    }

    public static void setNomeCompleto(string nome, string sobrenome) {
        nomeCompleto = nome + " " + sobrenome;
        Debug.Log(nomeCompleto);
    }
    
    public static void addFlagEmpatica(long tempo) {
        botaoFlagEmpatica.Add(tempo);
        Debug.Log("Flag: " + (new System.DateTime(tempo)).ToString());
    }

    public static void addTempoFinal(long tempo) {
        tempoFinal = tempo;
        if(tempoInicial == 0) {
            tempoInicial = tempoFinal;
        }
    }

    public static void addApertouUp(long tempo) { apertouUp.Add(tempo); }
    public static void addApertouDown(long tempo) { apertouDown.Add(tempo); }
    public static void addApertouLeft(long tempo) { apertouLeft.Add(tempo); }
    public static void addApertouRight(long tempo) { apertouRight.Add(tempo); }
    public static void addSoltouUp(long tempo) { soltouUp.Add(tempo); }
    public static void addSoltouDown(long tempo) { soltouDown.Add(tempo); }
    public static void addSoltouLeft(long tempo) { soltouLeft.Add(tempo); }
    public static void addSoltouRight(long tempo) { soltouRight.Add(tempo); }
    public static void addTiro(long tempo) { tiros.Add(tempo); }
    public static void addMorte(long tempo) { mortes.Add(tempo); }


    public static void AddToTxtLevel(int asteroidsCount, float minSpeed, float maxSpeed, bool venceu) {
        if (apertouUp.Count() != 0)
            tempoInicial = apertouUp[0];
        tempoDuracao = ((float)(tempoFinal - tempoInicial)) / 10000000f;
        CalculaPercentuais();

        text += "\t\"Level " + currentLevel.ToString() + "\": {\n"
        + "\t\t\"numero de asteroids\": " + asteroidsCount + ",\n"
        + "\t\t\"velocidade minima dos asteroids\": " + minSpeed + ",\n"
        + "\t\t\"velocidade maxima dos asteroids\": " + maxSpeed + ",\n"
        + "\t\t\"tempo inicial\": " + tempoInicial + ",\n"
        + "\t\t\"tempo final\": " + tempoFinal + ",\n"
        + "\t\t\"tempo total\": " + tempoDuracao + ",\n"
        + "\t\t\"tempo por vida\": " + tempoDuracao / (mortes.Count()+1) + ",\n"
        + "\t\t\"venceu\": " + venceu.ToString().ToLower() + ",\n"
        + "\t\t\"total de mortes\": " + mortes.Count() + ",\n";
        AddToTxtMorte(mortes, "mortes");
        text += "\t\t\"Teclas\": {\n";
        text += "\t\t\t\"total de tiros\": " + tiros.Count() + ",\n";
        AddToTxtListLong(tiros, "tiros");
        AddToTxtListLong(apertouUp, "apertou up");
        AddToTxtListLong(soltouUp, "soltou up");
        text += "\t\t\t\"percentual up\": " + percentualUp + ",\n";
        AddToTxtListLong(apertouDown, "apertou down");
        AddToTxtListLong(soltouDown, "soltou down");
        text += "\t\t\t\"percentual down\": " + percentualDown + ",\n";
        AddToTxtListLong(apertouLeft, "apertou left");
        AddToTxtListLong(soltouLeft, "soltou left");
        text += "\t\t\t\"percentual left\": " + percentualLeft + ",\n";
        AddToTxtListLong(apertouRight, "apertou right");
        AddToTxtListLong(soltouRight, "soltou right");
        text += "\t\t\t\"percentual right\": " + percentualRight + "\n";
        text += "\t\t},\n";
    }

    private static void AddToTxtListLong(List<long> array, string arrayName) {
        text += "\t\t\t\"" + arrayName + "\": [\n";
        for (int i = 0; i < array.Count(); i++) {
            text += "\t\t\t\t" + array[i];
            if (i == array.Count() - 1) {
                text += "\n\t\t\t],\n";
            }
            else {
                text += ",\n";
            }
        }
        if (array.Count() == 0) {
            text += "\t\t\t],\n";
        }
        array.Clear();
    }

    private static void AddToTxtMorte(List<long> array, string arrayName) {
        text += "\t\t\"" + arrayName + "\": [\n";
        for (int i = 0; i < array.Count(); i++) {
            text += "\t\t\t" + array[i];
            if (i == array.Count() - 1) {
                text += "\n\t\t],\n";
            }
            else {
                text += ",\n";
            }
        }
        if (array.Count() == 0) {
            text += "\t\t],\n";
        }
        array.Clear();
    }

    private static void CalculaPercentuais() {
        CalculaPercentual(apertouUp, soltouUp, "up");
        CalculaPercentual(apertouDown, soltouDown, "down");
        CalculaPercentual(apertouLeft, soltouLeft, "left");
        CalculaPercentual(apertouRight, soltouRight, "right");
    }

        private static void CalculaPercentual(List<long> apertouX, List<long> soltouX, string modo) {
            int i = 0;
        float ticksApertando = 0;
        
        for (int j = 0; j < soltouX.Count(); j++) {
            if(i<apertouX.Count()) {
                ticksApertando += soltouX[j] - apertouX[i];
                i++;
            }
            else {
                Debug.Log("Conferir essa linha de codigo pq o soltouX[" + j + "]: " + soltouX[j] + " não foi adicionado");
            }
        }
        //terminou o jogo segurando a tecla
        if (apertouX.Count() > soltouX.Count()) {
            ticksApertando += tempoFinal - apertouX[i];
        }

        if (modo.Equals("up")) {
            percentualUp = (float)((float)ticksApertando) / ((float)(tempoFinal - tempoInicial));
        }
        else if (modo.Equals("down")) {
            percentualDown = (float)((float)ticksApertando) / ((float)(tempoFinal - tempoInicial));
        }
        else if (modo.Equals("left")) {
            percentualLeft = (float)((float)ticksApertando) / ((float)(tempoFinal - tempoInicial));
        }
        else if (modo.Equals("right")) {
            percentualRight = (float)((float)ticksApertando) / ((float)(tempoFinal - tempoInicial));
        }

    }

    public static void AddToTxtPerguntas2(string dificuldade, string tedio, string frustracao, string diversao, string opiniao) {
        if (opiniao == "") {
            opiniao = "\"\"";
        }
        text += "\t\t\"Questionario\": {\n"
        + "\t\t\t\"dificuldade\": " + dificuldade + ",\n"
        + "\t\t\t\"tedio\": " + tedio + ",\n"
        + "\t\t\t\"frustracao\": " + frustracao + ",\n"
        + "\t\t\t\"diversao\": " + diversao + ",\n"
        + "\t\t\t\"opiniao\": " + opiniao + "\n"
        + "\t\t}\n"
        +"\t},\n";
    }

	public static void WriteFile() {
        

        text += "\t\"Flag Empatica\": [\n";
        for (int i = 0; i < botaoFlagEmpatica.Count(); i++) {
            text += "\t\t" + botaoFlagEmpatica[i];
            if (i == botaoFlagEmpatica.Count() - 1) {
                text += "\n\t]\n";
            }
            else {
                text += ",\n";
            }
        }
        if (botaoFlagEmpatica.Count() == 0) {
            text += "\t]\n";
        }

        text += "}\n";

        File.WriteAllText (fileName+".txt",text);
    }

    public static int GetCurrentLevel () {
        return currentLevel;
    }

    public static void AddLevel() {
        currentLevel++;
    }

}
