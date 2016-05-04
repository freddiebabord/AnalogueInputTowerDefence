using UnityEngine;
using System.Collections;

public class TowerPlacement : MonoBehaviour {

	enum Towers
	{
		IceTower = 0,
		Magic,
		ArrowTower,
		Ballistics
	};

	Pointer point;
	Towers index = Towers.IceTower;

	void Start()
	{
		point = GameObject.FindObjectOfType<Pointer> ();
	}

	void Update()
	{
		if (point.placeTower && Input.GetAxis ("TriggerSelectRight") >= 1) 
		{
			GameObject tile = point.currentTile;

			if(tile.GetComponent<NodePath>().pathType == NodePath.PathType.Grass && !tile.GetComponent<NodePath>().towerPlaced)
			{
				Instantiate (Resources.Load("Prefabs/Towers/"+index.ToString()), tile.transform.position, tile.transform.rotation);
				tile.GetComponent<NodePath>().towerPlaced = true;
			}
		}

		if (Input.GetAxis ("TriggerSelectLeft") >= 1) 
		{
			if(index > Towers.Ballistics)
				index = Towers.IceTower;
			else
				index++;
		}
	}

	public void Placement()
	{
		point.placeTower = !point.placeTower;

	}
}
