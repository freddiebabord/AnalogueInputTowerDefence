using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Mapping : MonoBehaviour {

	TurnOff off;

	GameObject panel;

	public int height = 10;

	public int width = 10;

	//[HideInInspector]
	public List<GameObject> map;

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

		for (int y = 0; y < height; y++) 
		{
			for(int x = 0; x < width; x++)
			{
				GameObject go = Instantiate(Resources.Load("Prefabs/Tiles/GrassTile"), new Vector3(x*4, 0, y*4), new Quaternion(0,0,0,0)) as GameObject;
				map.Add(go);
			}
		}

	}
}
