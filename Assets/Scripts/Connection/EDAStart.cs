using System.Collections;
using System.Collections.Generic;
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

        edaProcessor = new EDAProcessor();
    }
    
    void Update() {
        timer.Run();
        if (timer.GetElapsedTime() > 2) {
            StartCoroutine(GetReadBiggerSimulation(ultimo_id_lido));
            timer.Reset();
        }

    }

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

}
