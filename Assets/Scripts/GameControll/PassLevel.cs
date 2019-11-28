using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PassLevel : MonoBehaviour {

	public string nextLevel;

	private GameObject player;
	private Ship ship;

	void Start(){
		player = GameObject.FindGameObjectWithTag("Player");
		ship = player.GetComponent<Ship> ();
	}

	void Update()
    {
        if (ship.hasWon)
            BalanceAndPassLevel(true);
        if (Input.GetKeyDown(KeyCode.P)) //pula de nivel
            BalanceAndPassLevel(false);
    }

	private void BalanceAndPassLevel(bool venceu) {
        DataCenter.instance.SetTempoFinal();
        DDAAply.instance.BalanceAtPassLevel();
        DataCenter.instance.AddToOutputLevel(venceu);
        DataCenter.instance.resetDeath();
        SceneManager.LoadScene(nextLevel);
    }

    // Após a cena "Level" vem a cena do questionario, então quando se fecha Level, se assume que o nível terminou. 
    void OnApplicationQuit() {
        if (SceneManager.GetActiveScene().name.Equals("Level")) {
            DataCenter.instance.SetTempoFinal(); //setamos o tempo final aqui novamente para os casos quando se fecha o jogo no meio de um nível.
            DataCenter.instance.AddToOutputLevel(false);
            DataCenter.instance.AddLevelToJson();
            DataCenter.instance.Write();
        }
    }
}
