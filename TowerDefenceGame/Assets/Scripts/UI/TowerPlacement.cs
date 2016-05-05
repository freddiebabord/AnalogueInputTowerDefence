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

	GameObject tile;

	GameObject currentTile;

	GameObject panel;

	float timer = 0f;

	void Start()
	{
		panel = GameObject.FindGameObjectWithTag ("TowerSelect");
		point = GameObject.FindObjectOfType<Pointer> ();
		tile = null;
	}

	void Update()
	{
		if (point.placeTower) 
		{
			panel.SetActive (true);
			tile = point.currentTile;
			GameObject go = Instantiate (Resources.Load ("Prefabs/Towers/" + index.ToString ()), tile.transform.position, tile.transform.rotation) as GameObject;

			if (Input.GetAxis ("TriggerSelectRight") < 1) 
			{
				//Destroy(go);
			}
		} 
		else 
		{
			panel.SetActive(false);
			tile = null;
		}

		if (point.placeTower && Input.GetAxis ("TriggerSelectRight") >= 1) 
		{

			if(tile.GetComponent<NodePath>().pathType == NodePath.PathType.Grass && !tile.GetComponent<NodePath>().towerPlaced)
			{
				Instantiate (Resources.Load("Prefabs/Towers/"+index.ToString()), tile.transform.position, tile.transform.rotation);
				tile.GetComponent<NodePath>().towerPlaced = true;
			}
		}

		if (point.placeTower && Input.GetAxis ("TriggerSelectLeft") >= 1) 
		{
			if(timer == 2f)
				timer = 0f;

			if(timer == 0f)
			{
				index++;

				if(index > Towers.Ballistics)
				{
					index = Towers.IceTower;
				}
			}

			timer += Time.deltaTime;

		}
	}

	public void Placement()
	{
		point.placeTower = !point.placeTower;

	}
}
