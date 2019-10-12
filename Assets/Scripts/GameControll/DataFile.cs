using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

[System.Serializable]
public class DataFile : MonoBehaviour {

	public static string fileName;
	public static string text;
    private static string nomeCompleto;
    public static List<long> botaoFlagEmpatica = new List<long>();

    public List<DataFileLevel> dfLevel;
    static DataFileLevel level_aux;

    public static void Init(){
        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US"); //para imprimir separar os floats com '.' e nao ','
        text = "{\n";
        text += "\t\"nome\": \"" + nomeCompleto + "\",\n";
        level_aux = new DataFileLevel();

    }

	public static void SetFileName(string fName){
		fileName = fName + " " + nomeCompleto;
    }

    public static void setNomeCompleto(string nome, string sobrenome) {
        nomeCompleto = nome + " " + sobrenome;
    }
    
    public static void addFlagEmpatica(long tempo) {
        botaoFlagEmpatica.Add(tempo);
    }

    public static void addTempoFinal(long tempo) {
        level_aux.tempoFinal = tempo;
        if(level_aux.tempoInicial == 0) {
            level_aux.tempoInicial = level_aux.tempoFinal;
        }
    }

    public static void addApertouUp(long tempo) { level_aux.apertouUp.Add(tempo); }
    public static void addApertouDown(long tempo) { level_aux.apertouDown.Add(tempo); }
    public static void addApertouLeft(long tempo) { level_aux.apertouLeft.Add(tempo); }
    public static void addApertouRight(long tempo) { level_aux.apertouRight.Add(tempo); }
    public static void addSoltouUp(long tempo) { level_aux.soltouUp.Add(tempo); }
    public static void addSoltouDown(long tempo) { level_aux.soltouDown.Add(tempo); }
    public static void addSoltouLeft(long tempo) { level_aux.soltouLeft.Add(tempo); }
    public static void addSoltouRight(long tempo) { level_aux.soltouRight.Add(tempo); }
    public static void addTiro(long tempo) { level_aux.tiros.Add(tempo); }
    public static void addMorte(long tempo) { level_aux.mortes.Add(tempo); }


    public static void AddToTxtLevel(int asteroidsCount, float minSpeed, float maxSpeed, bool venceu) {
        if (level_aux.apertouUp.Count() != 0)
            level_aux.tempoInicial = level_aux.apertouUp[0];
        level_aux.tempoDuracao = ((float)(level_aux.tempoFinal - level_aux.tempoInicial)) / 10000000f;
        CalculaPercentuais();

        text += "\t\"Level " + DataColector.instance.GetCurrentLevel().ToString() + "\": {\n"
        + "\t\t\"numero de asteroids\": " + asteroidsCount + ",\n"
        + "\t\t\"velocidade minima dos asteroids\": " + minSpeed + ",\n"
        + "\t\t\"velocidade maxima dos asteroids\": " + maxSpeed + ",\n"
        + "\t\t\"tempo inicial\": " + level_aux.tempoInicial + ",\n"
        + "\t\t\"tempo final\": " + level_aux.tempoFinal + ",\n"
        + "\t\t\"tempo total\": " + level_aux.tempoDuracao + ",\n"
        + "\t\t\"tempo por vida\": " + level_aux.tempoDuracao / (level_aux.mortes.Count()+1) + ",\n"
        + "\t\t\"venceu\": " + venceu.ToString().ToLower() + ",\n"
        + "\t\t\"total de mortes\": " + level_aux.mortes.Count() + ",\n";
        AddToTxtMorte(level_aux.mortes, "mortes");
        text += "\t\t\"Teclas\": {\n";
        text += "\t\t\t\"total de tiros\": " + level_aux.tiros.Count() + ",\n";
        AddToTxtListLong(level_aux.tiros, "tiros");
        AddToTxtListLong(level_aux.apertouUp, "apertou up");
        AddToTxtListLong(level_aux.soltouUp, "soltou up");
        text += "\t\t\t\"percentual up\": " + level_aux.percentualUp + ",\n";
        AddToTxtListLong(level_aux.apertouDown, "apertou down");
        AddToTxtListLong(level_aux.soltouDown, "soltou down");
        text += "\t\t\t\"percentual down\": " + level_aux.percentualDown + ",\n";
        AddToTxtListLong(level_aux.apertouLeft, "apertou left");
        AddToTxtListLong(level_aux.soltouLeft, "soltou left");
        text += "\t\t\t\"percentual left\": " + level_aux.percentualLeft + ",\n";
        AddToTxtListLong(level_aux.apertouRight, "apertou right");
        AddToTxtListLong(level_aux.soltouRight, "soltou right");
        text += "\t\t\t\"percentual right\": " + level_aux.percentualRight + "\n";
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
        CalculaPercentual(level_aux.apertouUp, level_aux.soltouUp, "up");
        CalculaPercentual(level_aux.apertouDown, level_aux.soltouDown, "down");
        CalculaPercentual(level_aux.apertouLeft, level_aux.soltouLeft, "left");
        CalculaPercentual(level_aux.apertouRight, level_aux.soltouRight, "right");
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
            ticksApertando += level_aux.tempoFinal - apertouX[i];
        }

        if (modo.Equals("up")) {
            level_aux.percentualUp = (float)((float)ticksApertando) / ((float)(level_aux.tempoFinal - level_aux.tempoInicial));
        }
        else if (modo.Equals("down")) {
            level_aux.percentualDown = (float)((float)ticksApertando) / ((float)(level_aux.tempoFinal - level_aux.tempoInicial));
        }
        else if (modo.Equals("left")) {
            level_aux.percentualLeft = (float)((float)ticksApertando) / ((float)(level_aux.tempoFinal - level_aux.tempoInicial));
        }
        else if (modo.Equals("right")) {
            level_aux.percentualRight = (float)((float)ticksApertando) / ((float)(level_aux.tempoFinal - level_aux.tempoInicial));
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

}
