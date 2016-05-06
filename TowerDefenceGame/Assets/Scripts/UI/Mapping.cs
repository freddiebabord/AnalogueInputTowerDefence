using UnityEngine;
using System.Collections;

public class Mapping : MonoBehaviour {

	TurnOff off;

	GameObject panel;

	// Use this for initialization
	void Start () {
		panel = GameObject.FindGameObjectWithTag ("Panel");

		off = GameObject.FindObjectOfType<TurnOff> ();
	}

	public void Click()
	{
		foreach (GameObject ui in off.towers())
			ui.SetActive (true);

		panel.SetActive (false);

		for (int y = 0; y < 10; y++) 
		{
			for(int x = 0; x < 10; x++)
			{
				Instantiate(Resources.Load("Prefabs/Tiles/GrassTile"), new Vector3(x*4, 0, y*4), new Quaternion(0,0,0,0));
			}
		}

	}
}
