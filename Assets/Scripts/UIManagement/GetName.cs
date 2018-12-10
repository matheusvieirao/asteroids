using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GetName : MonoBehaviour {

	public InputField nome;

	public void ExtractFromUI(){
		DataFile.Init ();
		DataFile.SetFileName(nome.text);
	}

}

