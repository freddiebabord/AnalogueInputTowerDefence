using UnityEngine;
using System.Collections;

public class Stat : MonoBehaviour {

	Pointer point;

	TowerPlacement tower;

	GameObject tile;

	GameObject statUI;

	bool stats;
	// Use this for initialization
	void Start () {

		point = GameObject.FindObjectOfType<Pointer> ();
		tower = GameObject.FindObjectOfType<TowerPlacement> ();
		statUI = GameObject.FindGameObjectWithTag("Info");
	}
	
	// Update is called once per frame
	void Update () {

		if (!statUI.activeSelf)
			statUI.SetActive (true);

		if (stats) 
		{
			if(point.currentTile.GetComponent<NodePath>().towerPlaced)
			{
				Debug.Log ("Tower is here");
			}
		}
	
	}

	public void GetStats()
	{
		point.placeTower = !point.placeTower;
		stats = !stats;

	}
}
