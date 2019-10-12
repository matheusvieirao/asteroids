using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

// JSON que vai ser gerado, possui [System.Serializable] e não implementa MonoBehaviour justamente por causa disso
// level (DataFileLevel) contem as informações de cada nível.
[System.Serializable]
public class DataFile {
    
    private string nomeCompleto;
    public List<long> botaoFlagEmpatica = new List<long>();

    public List<DataFileLevel> level;
    private DataFileLevel level_aux;

    public DataFile(){
        System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US"); //para imprimir separar os floats com '.' e nao ','
        level_aux = new DataFileLevel();
        level = new List<DataFileLevel>();
    }

    public void SetNomeCompleto(string nome) {
        nomeCompleto = nome;
    }
    
    public void addFlagEmpatica(long tempo) {
        botaoFlagEmpatica.Add(tempo);
    }

    public void SetTempoInicial(long tempo) {
        level_aux.tempoInicial = tempo;
    }

    public void SetTempoFinal(long tempo) {
        level_aux.tempoFinal = tempo;
        //se o tempo inicial nao foi setado corretamente, seta ele pro primeiro momento que se andou pra cima, se nao der, pro mesmo tempo que o final
        if(level_aux.tempoInicial == 0) {
            if (level_aux.apertouUp.Count() != 0)
                level_aux.tempoInicial = level_aux.apertouUp[0];
            else {
                level_aux.tempoInicial = level_aux.tempoFinal;
            }
        }
    }

    public void addApertouUp(long tempo) { level_aux.apertouUp.Add(tempo); }
    public void addApertouDown(long tempo) { level_aux.apertouDown.Add(tempo); }
    public void addApertouLeft(long tempo) { level_aux.apertouLeft.Add(tempo); }
    public void addApertouRight(long tempo) { level_aux.apertouRight.Add(tempo); }
    public void addSoltouUp(long tempo) { level_aux.soltouUp.Add(tempo); }
    public void addSoltouDown(long tempo) { level_aux.soltouDown.Add(tempo); }
    public void addSoltouLeft(long tempo) { level_aux.soltouLeft.Add(tempo); }
    public void addSoltouRight(long tempo) { level_aux.soltouRight.Add(tempo); }
    public void addTiro(long tempo) { level_aux.tiros.Add(tempo); }
    public void addMorte(long tempo) { level_aux.mortes.Add(tempo); }


// tempo inicial e tempo final já foram povoados. mortes, tiros, e teclas (up, down...) também.
    public void AddToOutputFileLevel(int asteroidsCount, float minSpeed, float maxSpeed, bool venceu) {
        CalculaPercentuais();

        level_aux.numeroDeAsteroids = asteroidsCount;
        level_aux.velocidadeMinimaDosAsteroids = minSpeed;
        level_aux.velocidadeMaximaDosAsteroids = maxSpeed;
        level_aux.tempoDuracao = ((float)(level_aux.tempoFinal - level_aux.tempoInicial)) / 10000000f;
        level_aux.tempoPorVida = level_aux.tempoDuracao / (level_aux.mortes.Count() + 1);
        level_aux.venceu = venceu;
        level_aux.totalDeMortes = level_aux.mortes.Count();
        level_aux.totalDeTiros = level_aux.tiros.Count();
    }

    private void CalculaPercentuais() {
        CalculaPercentual(level_aux.apertouUp, level_aux.soltouUp, "up");
        CalculaPercentual(level_aux.apertouDown, level_aux.soltouDown, "down");
        CalculaPercentual(level_aux.apertouLeft, level_aux.soltouLeft, "left");
        CalculaPercentual(level_aux.apertouRight, level_aux.soltouRight, "right");
    }

    private void CalculaPercentual(List<long> apertouX, List<long> soltouX, string modo) {
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

    public void AddToOutputFilePerguntas(string dificuldade, string tedio, string frustracao, string diversao, string opiniao) {
        level_aux.dificuldade = dificuldade;
        level_aux.tedio = tedio;
        level_aux.frustracao = frustracao;
        level_aux.diversao = diversao;
        level_aux.opiniao = opiniao;
    }

    public void AddLevelToJson() {
        level.Add(level_aux);
        level_aux = null;
        level_aux = new DataFileLevel();
    }

}
