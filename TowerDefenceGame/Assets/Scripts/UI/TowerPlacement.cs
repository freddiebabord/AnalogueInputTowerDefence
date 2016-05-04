using UnityEngine;
using System.Collections;

public class TowerPlacement : MonoBehaviour {

	Pointer point;

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
				Instantiate (Resources.Load("Prefabs/Towers/AbsorbingTower"), tile.transform.position, tile.transform.rotation);
				tile.GetComponent<NodePath>().towerPlaced = true;
			}
		}
	}

	public void Placement()
	{
		point.placeTower = !point.placeTower;

	}
}
