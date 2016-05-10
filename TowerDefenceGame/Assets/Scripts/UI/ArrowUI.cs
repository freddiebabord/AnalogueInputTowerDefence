using UnityEngine;
using System.Collections;

public class ArrowUI : MonoBehaviour {

	Pointer point;
	
	TowerPlacement towers;
	
	GameObject tile;

	GameObject current;

	GameObject go;

	bool hologram = false;

	bool arrow = false;
    public float cost = 100;
	// Use this for initialization
	void Start () {
		
		towers = GameObject.FindObjectOfType<TowerPlacement> ();
		point = GameObject.FindObjectOfType<Pointer> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (arrow && !towers.isTrue)
			point.placeTower = false;
		
		if (arrow && point.placeTower && point.currentTile.GetComponent<NodePath>().pathType == NodePath.PathType.Grass && !point.currentTile.GetComponent<NodePath>().towerPlaced) 
		{
			tile = point.currentTile;

			if(!hologram)
			{
				go = Instantiate (Resources.Load ("Prefabs/Towers/ArrowTower"), tile.transform.position, tile.transform.rotation) as GameObject;

				Arrow arr = go.GetComponentInChildren<Arrow>();
				arr.enabled = false;
				hologram = true;

			}

			if (tile != current && hologram) 
			{
				go.gameObject.transform.position = tile.transform.position;
				go.gameObject.transform.rotation = tile.transform.rotation;
			}
			
			current = tile;
		} 
		else 
		{
			tile = null;
			current = null;
		}
		
		if (arrow && point.placeTower && Input.GetAxis ("TriggerSelectRight") >= 1) 
		{
            if (tile != null)
            {
                if (tile.GetComponent<NodePath>().pathType == NodePath.PathType.Grass && !tile.GetComponent<NodePath>().towerPlaced)
                {
                    if (GameObject.FindObjectOfType<GameManager>().gold - cost > 0)
                    {
                        Instantiate(Resources.Load("Prefabs/Towers/ArrowTower"), tile.transform.position, tile.transform.rotation);
                        tile.GetComponent<NodePath>().towerPlaced = true;
                        GameObject.FindObjectOfType<GameManager>().RemoveGold(cost);
                    }
                }
            }
		}

        if (Input.GetAxis("TriggerSelectLeft") >= 1)
        {
            arrow = false;
            point.placeTower = false;
        }
	}
	
	public void PlaceArrow()
	{
		point.placeTower = !point.placeTower;
		arrow = !arrow;
		hologram = false;

	}
}
