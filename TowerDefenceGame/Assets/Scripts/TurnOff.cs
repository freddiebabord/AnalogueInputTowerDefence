using UnityEngine;
using System.Collections;

public class TurnOff : MonoBehaviour {

	public GameObject[] UI;

	// Use this for initialization
	void Start () {

		foreach (GameObject tower in UI)
			tower.SetActive (false);
	}

	public GameObject[] towers()
	{
		return UI;
	}
}
