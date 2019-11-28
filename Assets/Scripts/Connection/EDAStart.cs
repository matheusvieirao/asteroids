using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class EDAStart : MonoBehaviour
{
    public static EDAStart instance;
    public GameObject prefab;
    public EDASignals sinais;

    private readonly double tempo_inicial_bd = 1570572504.2719;
    private double tempo_inicial_jogo;
    private TimerController timer;
    private int ultimo_id_lido;
    private EDAProcessor edaProcessor;
    private double ultimo_tempo_eda_lido; //usado para gerar os graficos
    EDATempoTonicoDTO objETT; //usado para guardar os dados dos graficos

    void Awake() {
        if (instance == null) {
            instance = prefab.GetComponent<EDAStart>();
        }
        else if (instance != this) {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start() {
        ultimo_id_lido = 0;
        tempo_inicial_jogo = System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1)).TotalSeconds;
        //tempo_inicial_jogo = System.DateTimeOffset.UtcNow.ToUnixTimeSeconds(); //também é uma opção mas tem menos precisão
        timer = new TimerController();
        timer.Reset();

        GetReadFromJsonFiles(25.0f);

    }

    void Update() {
        //timer.Run();
        //if (timer.GetElapsedTime() > 2) {
        //    StartCoroutine(GetReadBiggerSimulation(ultimo_id_lido));
        //    timer.Reset();
        //}


    }

    // le todos os arquivos StreamingAssets/input_eda_tempo/EDA_tempo_<numero>.json e faz uma simulação em que:
    // - se le os dados em um intervalo de tempo determinado por tempo_buffer 
    // - calcula um nivel tonico e um nivel fasico para esse internvalo, 
    // - salva a saida em "ETT_<numero>.json
    private void GetReadFromJsonFiles(float tempo_buffer) {

        edaProcessor = new EDAProcessor();
        string path = Application.streamingAssetsPath + "/input_eda_tempo";
        for (int i_voluntario = 1; i_voluntario < 19; i_voluntario++) {
            ultimo_id_lido = 0;
            string jsonString = File.ReadAllText(path + "/EDA_tempo_" + i_voluntario.ToString() + ".json");
            objETT = JsonUtility.FromJson<EDATempoTonicoDTO>(jsonString); //entrada do eda e saida do tonic e phasic
            ultimo_tempo_eda_lido = objETT.tempoEda[0];
            List<EDASignal> sinais_buffer = new List<EDASignal>();
            edaProcessor.Reset();
            int id_aux = 0;
            for (int i = 0; i < objETT.tempoEda.Count; i++) {
                if (objETT.tempoEda[i] - ultimo_tempo_eda_lido < tempo_buffer) {
                    sinais_buffer.Add(new EDASignal(id_aux, objETT.tempoEda[i], objETT.eda[i], 0));
                    id_aux++;
                }
                else {

                    objETT.tempoTonicLevel.Add(objETT.tempoEda[i]);
                    int tonic = edaProcessor.GetTonicLevel(sinais_buffer);
                    objETT.tonicLevel.Add(tonic);
                    objETT.phasicLevel.Add(edaProcessor.GetPhasicLevel(sinais_buffer));
                    sinais_buffer = null;
                    sinais_buffer = new List<EDASignal>();
                    ultimo_tempo_eda_lido = objETT.tempoEda[i];

                }
            }
            jsonString = JsonUtility.ToJson(objETT, true);
            File.WriteAllText(path + "/ETT_" + i_voluntario + ".json", jsonString);
        }
    }

    // lê todos os dados na tabela eda do servidor
    IEnumerator GetReadAll() {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost/android_connect/read_all.php")) {
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();

            if ((www.isNetworkError || www.isHttpError)) {
                Debug.Log(www.error);
            }
            else {
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US"); //para converter os Doubles considerando '.' e nao ','
                string jsonString = www.downloadHandler.text;
                sinais = JsonUtility.FromJson<EDASignals>(jsonString);
            }

        }
    }

    // lê todos os números maiores que id
    IEnumerator GetReadBigger(int id) {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost/android_connect/read_bigger.php" + "?id=" + id)) {
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();

            if ((www.isNetworkError || www.isHttpError)) {
                Debug.Log(www.error);
            }
            else {
                string jsonString = www.downloadHandler.text;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US"); //para converter os Doubles considerando '.' e nao ','
                sinais = JsonUtility.FromJson<EDASignals>(jsonString);
                //ultimo_id_lido = sinais.eda[sinais.eda.Length - 1].id;
            }
        }
    }

    // lê todos os números maiores que id e menores do que determinado tempo passado em relação a tempo_inicial_bd
    // usado pra simular quando nao se esta pegando os dados em tempo real
    IEnumerator GetReadBiggerSimulation(int id) {
        double tempo_agora = System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1)).TotalSeconds;
        double tempo_aux = (tempo_agora - tempo_inicial_jogo) + tempo_inicial_bd;
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost/android_connect/read_between.php" + "?id=" + id + "&tempo=" + tempo_aux)) {
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();

            if ((www.isNetworkError || www.isHttpError)) {
                Debug.Log(www.error);
            }
            else {
                string jsonString = www.downloadHandler.text;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US"); //para converter os Doubles considerando '.' e nao ','
                sinais = JsonUtility.FromJson<EDASignals>(jsonString);
                Debug.Log(jsonString);
                //ultimo_id_lido = sinais.eda[sinais.eda.Length-1].id;
                ultimo_id_lido = sinais.eda[sinais.eda.Count - 1].id;
                print("GetTonicLevel: " + edaProcessor.GetTonicLevel(sinais.eda));
                print("GetPhasicLevel: " + edaProcessor.GetPhasicLevel(sinais.eda));
            }
        }
    }
    
    // lê todos os números maiores que id e menores do que determinado tempo passado como parametro
    IEnumerator GetReadBetween(int id, float tempo) {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost/android_connect/read_between.php" + "?id=" + id + "&tempo=" + tempo)) {
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();

            if ((www.isNetworkError || www.isHttpError)) {
                Debug.Log(www.error);
            }
            else {
                string jsonString = www.downloadHandler.text;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US"); //para converter os Doubles considerando '.' e nao ','
                sinais = JsonUtility.FromJson<EDASignals>(jsonString);
                //print(">>"+jsonString);
                ultimo_id_lido = sinais.eda[sinais.eda.Count - 1].id;
            }
        }
    }

}
