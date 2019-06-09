using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPerguntas2 : MonoBehaviour
{
    public GameObject grupoDificuldade;
    public GameObject grupoTedio;
    public GameObject grupoFrustracao;
    public GameObject grupoDiversao;
    private string respostaDificuldade;
    private string respostaTedio;
    private string respostaFrustracao;
    private string respostaDiversao;

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    public void SubmitAnswer()
    {
        Transform child;

        for (int i = 0; i < grupoDificuldade.transform.childCount; i++)
        {
            child = grupoDificuldade.transform.GetChild(i);
            if (child.GetComponent<Toggle> () != null)
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

    }

}
