﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIPerguntas2 : MonoBehaviour
{
    public GameObject grupoDificuldade;
    public GameObject grupoTedio;
    public GameObject grupoFrustracao;
    public GameObject grupoDiversao;
    public GameObject inputText;
    private string respostaDificuldade;
    private string respostaTedio;
    private string respostaFrustracao;
    private string respostaDiversao;
    private string respostaInputText;
    private bool jaClicouSubmit = false;

    void Start()
    {
        grupoDificuldade.transform.Find("Seta").GetComponent<Image>().enabled = false;
        grupoTedio.transform.Find("Seta").GetComponent<Image>().enabled = false;
        grupoFrustracao.transform.Find("Seta").GetComponent<Image>().enabled = false;
        grupoDiversao.transform.Find("Seta").GetComponent<Image>().enabled = false;
    }
    
    void Update()
    {
        if (jaClicouSubmit)
        {
            Transform child;
            // Checar as respostas clicadas
            for (int i = 0; i < grupoDificuldade.transform.childCount; i++)
            {
                child = grupoDificuldade.transform.GetChild(i);
                if (child.GetComponent<Toggle>() != null)
                {
                    if (child.GetComponent<Toggle>().isOn)
                    {
                        respostaDificuldade = child.Find("Label").GetComponent<Text>().text;
                    }
                }
            }
            for (int i = 0; i < grupoTedio.transform.childCount; i++)
            {
                child = grupoTedio.transform.GetChild(i);
                if (child.GetComponent<Toggle>() != null)
                {
                    if (child.GetComponent<Toggle>().isOn)
                    {
                        respostaTedio = child.Find("Label").GetComponent<Text>().text;
                    }
                }
            }
            for (int i = 0; i < grupoFrustracao.transform.childCount; i++)
            {
                child = grupoFrustracao.transform.GetChild(i);
                if (child.GetComponent<Toggle>() != null)
                {
                    if (child.GetComponent<Toggle>().isOn)
                    {
                        respostaFrustracao = child.Find("Label").GetComponent<Text>().text;
                    }
                }
            }
            for (int i = 0; i < grupoDiversao.transform.childCount; i++)
            {
                child = grupoDiversao.transform.GetChild(i);
                if (child.GetComponent<Toggle>() != null)
                {
                    if (child.GetComponent<Toggle>().isOn)
                    {
                        respostaDiversao = child.Find("Label").GetComponent<Text>().text;
                    }
                }
            }
            respostaInputText = inputText.GetComponent<Text>().text;
            
            //atualizar as setas que informam aonde ainda precisa ser clicado
            if (respostaDificuldade == null)
            {
                grupoDificuldade.transform.Find("Seta").GetComponent<Image>().enabled = true;
            }
            else
            {
                grupoDificuldade.transform.Find("Seta").GetComponent<Image>().enabled = false;
            }
            if (respostaTedio == null)
            {
                grupoTedio.transform.Find("Seta").GetComponent<Image>().enabled = true;
            }
            else
            {
                grupoTedio.transform.Find("Seta").GetComponent<Image>().enabled = false;
            }
            if (respostaFrustracao == null)
            {
                grupoFrustracao.transform.Find("Seta").GetComponent<Image>().enabled = true;
            }
            else
            {
                grupoFrustracao.transform.Find("Seta").GetComponent<Image>().enabled = false;
            }
            if (respostaDiversao == null)
            {
                grupoDiversao.transform.Find("Seta").GetComponent<Image>().enabled = true;
            }
            else
            {
                grupoDiversao.transform.Find("Seta").GetComponent<Image>().enabled = false;
            }
        }    
    }

    public void SubmitAnswer()
    {
        jaClicouSubmit = true;

        Update();

        if (respostaDificuldade != null && respostaTedio != null && respostaFrustracao != null && respostaDiversao != null)
        {
            DataColector.instance.AddToOutputPerguntas(respostaDificuldade, respostaTedio, respostaFrustracao, respostaDiversao, respostaInputText);
            DataColector.instance.AddLevelToJson();
            if(DataColector.instance.GetCurrentLevel() == 10) {
                gameObject.GetComponent<ProximaCena>().nextScene = "Fim do Jogo";
            }
            DataColector.instance.AddLevel();
            gameObject.GetComponent<ProximaCena>().passLevel();
        }
        
    }

    void OnApplicationQuit() {
        if (SceneManager.GetActiveScene().name.Equals("Perguntas2")) {
            jaClicouSubmit = true;
            Update();
            DataColector.instance.AddToOutputPerguntas(respostaDificuldade, respostaTedio, respostaFrustracao, respostaDiversao, respostaInputText);
            DataColector.instance.AddLevelToJson();
            DataColector.instance.Write();
        }
    }

}
