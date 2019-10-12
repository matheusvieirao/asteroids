using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EDAStart : MonoBehaviour
{
    public static EDAStart instance;
    public GameObject prefab;

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
    void Start()
    {
        StartCoroutine(GetReadBigger(10));

    }

    IEnumerator GetReadAll() {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost/android_connect/read_all.php")) {
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();

            if ((www.isNetworkError || www.isHttpError)) {
                Debug.Log(www.error);
            }
            else {
                string jsonString = www.downloadHandler.text;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US"); //para converter os Doubles considerando '.' e nao ','
                EDASignals sinais = JsonUtility.FromJson<EDASignals>(jsonString);
            }

        }
    }

    // lê todos os números maiores que id_inicio
    IEnumerator GetReadBigger(int id_inicio) {
        using (UnityWebRequest www = UnityWebRequest.Get("http://localhost/android_connect/read_bigger.php" + "?id=" + id_inicio)) {
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();

            if ((www.isNetworkError || www.isHttpError)) {
                Debug.Log(www.error);
            }
            else {
                string jsonString = www.downloadHandler.text;
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US"); //para converter os Doubles considerando '.' e nao ','
                EDASignals sinais = JsonUtility.FromJson<EDASignals>(jsonString);
            }
        }
    }

    // Update is called once per frame
    void Update() {

    }
}
