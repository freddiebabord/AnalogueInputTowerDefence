using UnityEngine;
using System.Collections;

public class Pathing : MonoBehaviour {

	GameObject[] pathUI;

	bool click = false;

	// Use this for initialization
	void Start () {

		pathUI = GameObject.FindGameObjectsWithTag("Path");
	
	}

	public void Click()
	{
		click = !click;

		foreach (GameObject obj in pathUI) 
		{
			obj.SetActive(click);
		}
	}
}
