using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Presentation : MonoBehaviour
{
    public string nextScene;
    private bool jaClicouSubmit = false;
    public GameObject nome_go;
    public GameObject sobrenome_go;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (jaClicouSubmit) {
            string nome_str = nome_go.transform.Find("Texto").GetComponent<Text>().text;
            if (nome_str.Equals("")) {
                nome_go.GetComponent<Image>().color = new UnityEngine.Color32(246,115,115,255);
            }
            else {
                nome_go.GetComponent<Image>().color = new UnityEngine.Color32(255,255,255,255);
            }

            string sobrenome_str = sobrenome_go.transform.Find("Texto").GetComponent<Text>().text;
            if (sobrenome_str.Equals("")) {
                sobrenome_go.GetComponent<Image>().color = new UnityEngine.Color32(246, 115, 115, 255);
            }
            else {
                sobrenome_go.GetComponent<Image>().color = new UnityEngine.Color32(255, 255, 255, 255);
            }
        }
        
    }

    public void BotaoJogar() {
        jaClicouSubmit = true;

        string nome_str = nome_go.transform.Find("Texto").GetComponent<Text>().text;
        string sobrenome_str = sobrenome_go.transform.Find("Texto").GetComponent<Text>().text;

        if(!nome_str.Equals("") && !sobrenome_str.Equals("")) {
            DataFile.setNomeCompleto(nome_str, sobrenome_str);
            SceneManager.LoadScene(nextScene);
        }
    }
}
