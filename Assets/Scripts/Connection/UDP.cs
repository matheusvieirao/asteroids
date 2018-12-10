using UnityEngine;
using System.Collections;

public class UDP : MonoBehaviour {
    public string send1;
    public string send2;
    public string send3;
    private int i=0;
    public int Level = 0;
    public int Difficulty = 1;
    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            i++;
            var udp = GameObject.FindGameObjectWithTag("UDP").GetComponentInChildren<UDPObj>();
            if (i == 1)
            {
                udp.sendData(send1 + "\n");
            }
            else if (i == 2)
            {
                udp.sendData(Level +""+ Difficulty + "\n");
            }
            else
            {
                udp.sendData(send3 + "\n");
            }

        }

    }
}
