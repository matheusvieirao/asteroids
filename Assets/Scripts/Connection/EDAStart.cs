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
    public EDASignals sinais; //onde ficam os sinais lidos.
    
    //private readonly double tempo_inicial_bd = 1570572504.2719;
    //private double tempo_inicial_jogo;
    //private TimerController timer;
    private int ultimo_id_lido;
    //private EDAProcessor edaProcessor; //Essa classe contem parte do programa do marcos usado para calcular nivel tonico
    //private double ultimo_tempo_eda_lido; //usado para gerar os graficos
    //EDATempoTonicoDTO objETT; //usado para guardar os dados dos graficos
    List<PicoEDA> picos = new List<PicoEDA>();

    //private double edaUltimoValor;
    //private double edaPenultimoValor;

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
        //tempo_inicial_jogo = System.DateTime.UtcNow.Subtract(new System.DateTime(1970, 1, 1)).TotalSeconds;
        //tempo_inicial_jogo = System.DateTimeOffset.UtcNow.ToUnixTimeSeconds(); //também é uma opção mas tem menos precisão
        //timer = new TimerController();
        //timer.Reset();

        //GetReadFromJsonFiles(25.0f);

    }

    void Update() {
        //timer.Run();
        //if (timer.GetElapsedTime() > 2) {
        //    StartCoroutine(GetReadBiggerSimulation(ultimo_id_lido));
        //    timer.Reset();
        //}


    }



    public void callGetReadBigger(bool calcularExcitacao) {
        StartCoroutine(GetReadBigger(calcularExcitacao));
    }

    // lê todos os números maiores que id
    //vou usar essa. o id vou salvar antes.
    //se calcularExcitacao for true, calcula a excitacao. Se for false, apenas le os dados (usado para descartar os dados do questionario)
    IEnumerator GetReadBigger(bool calcularExcitacao) {
        Debug.Log("Entrou no GetReadBigger com id: " + ultimo_id_lido);
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost/android_connect/read_bigger.php" + "?id=" + ultimo_id_lido)) {
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();

            if ((www.isNetworkError || www.isHttpError)) {
                Debug.Log("Erro de conecção");
                Debug.Log(www.error);
            }
            else {
                string jsonString = www.downloadHandler.text;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US"); //para converter os Doubles considerando '.' e nao ','
                sinais = JsonUtility.FromJson<EDASignals>(jsonString);
                if (sinais.eda.Count > 0) {
                    ultimo_id_lido = sinais.eda[sinais.eda.Count - 1].id;
                }
                Debug.Log(sinais.eda.Count + " sinais lidos");
                if (calcularExcitacao) {
                    CalculaPicos(); //pontos máximos e minimos relativos
                    Debug.Log(picos.Count + " picos achados");
                    if(picos.Count>10)
                    CaclulaExcitacao();
                }
            }
        }
    }

    private void CalculaPicos() {
        bool estava_subindo = false;
        bool estava_descendo = false;
        double tamanho = 0;
        double dif;
        int ignorar = 10; //numero de valores a se ignorar. o valor inicial pode ter mais relação com o questionario do que com o jogo em si. 6 foi encontrado por testes como um bom número

        //ordenar aqui eda por id?
        for(int i = 0; i < sinais.eda.Count; i++) {
            //descarta os 20 primeiros sinais
            if (i == ignorar) {
                dif = sinais.eda[ignorar].value - sinais.eda[ignorar-1].value;
                if (dif >= 0) {
                    estava_subindo = true;
                    tamanho = dif;
                }
                else {
                    estava_descendo = true;
                    tamanho = -dif;
                }
            }
            else if (i > ignorar) {
                dif = sinais.eda[i].value - sinais.eda[i - 1].value;
                //se subindo agora
                if (dif > 0) {
                    //se já estava subindo
                    if (estava_subindo) {
                        tamanho += dif;
                    }
                    //começou a subir só agora. encontrou um pico negativo em i-1
                    else if (estava_descendo) {
                        //add pico
                        picos.Add(new PicoEDA(sinais.eda[i-1],tamanho));
                        tamanho = dif;
                        estava_descendo = false;
                        estava_subindo = true;
                    }
                    else {
                        Debug.Log("Um erro está acontecendo ao calcular o EDA. (nao está detectando subida nem descida) (1)");
                    }
                }
                //se descendo agora
                else {
                    //se já estava descendo
                    if (estava_descendo) {
                        tamanho -= dif;
                    }
                    //comçou a descer só agora. encontrou um pico positivo em i-1
                    else if (estava_subindo) {
                        //add pico;
                        picos.Add(new PicoEDA(sinais.eda[i - 1], tamanho));
                        tamanho = -dif;
                        estava_subindo = false;
                        estava_descendo = true;
                    }
                    else {
                        Debug.Log("Um erro está acontecendo ao calcular o EDA. (nao está detectando subida nem descida) (2)");
                    }
                }
            }
        }
    }

    private void CaclulaExcitacao() {
        double edaInicialMedia = (picos[0].value + picos[1].value + picos[2].value + picos[3].value + picos[4].value + picos[5].value + picos[6].value + picos[7].value) /8;
        int s = picos.Count;
        double edaFinalMedia = (picos[s-1].value + picos[s-2].value + picos[s-3].value + picos[s-4].value + picos[s-5].value + picos[s-6].value + picos[s-7].value + picos[s-8].value)/8;
        picos.Sort((x, y) => x.size.CompareTo(y.size)); //ordena a lista pra encontrar o ruido
        double ruido = picos[(int)(picos.Count / 2)].size; //o ruido é considerado a amplitude mediana entre dois picos

        int escala = 2; //quantas vezes uma mudança tem que ter em relação ao ruido para se considerar se o jogador se excitou ou não

        Debug.Log("edaInicialMedia: " + edaInicialMedia);
        Debug.Log("edaFinalMedia: " + edaFinalMedia);
        Debug.Log("ruido: " + ruido);
        //subiu
        if (edaFinalMedia - edaInicialMedia > escala * ruido) {
            Debug.Log("Excitação: HIGH");
            DDAAply.instance.excitacao = State.PlayerState.HIGH;
        }
        //desceu
        else if(edaFinalMedia - edaInicialMedia < -1 * escala * ruido) {
            Debug.Log("Excitação: LOW");
            DDAAply.instance.excitacao = State.PlayerState.LOW;
        }
        else {
            Debug.Log("Excitação: NORMAL");
            DDAAply.instance.excitacao = State.PlayerState.NORMAL;
        }
    }















    /*
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
    }*/

}
