using UnityEngine;
using System.Collections;

public class ArrowUI : MonoBehaviour {

	Pointer point;
	
	TowerPlacement towers;
	
	GameObject tile;

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
		
		if (point.placeTower) 
		{
			tile = point.currentTile;
			//GameObject go = Instantiate (Resources.Load ("Prefabs/Towers/" + index.ToString ()), tile.transform.position, tile.transform.rotation) as GameObject;
			
			if (Input.GetAxis ("TriggerSelectRight") < 1) 
			{
				//Destroy(go);
			}
		} 
		else 
		{
			tile = null;
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
	}
}
