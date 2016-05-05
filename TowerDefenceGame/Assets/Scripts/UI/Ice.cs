using UnityEngine;
using System.Collections;

public class Ice : MonoBehaviour {

	Pointer point;
	
	TowerPlacement towers;
	
	GameObject tile;

	bool ice = false;
    public float cost = 200;
	// Use this for initialization
	void Start () {
		
		towers = GameObject.FindObjectOfType<TowerPlacement> ();
		point = GameObject.FindObjectOfType<Pointer> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (ice && !towers.isTrue)
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
		
		if (ice && point.placeTower && Input.GetAxis ("TriggerSelectRight") >= 1) 
		{
			
			if(tile.GetComponent<NodePath>().pathType == NodePath.PathType.Grass && !tile.GetComponent<NodePath>().towerPlaced)
			{
                if (GameObject.FindObjectOfType<GameManager>().gold - cost > 0)
                {
                    Instantiate(Resources.Load("Prefabs/Towers/IceTower"), tile.transform.position, tile.transform.rotation);
                    tile.GetComponent<NodePath>().towerPlaced = true;
                    GameObject.FindObjectOfType<GameManager>().RemoveGold(cost);
                }
			}
		}
        if (Input.GetAxis("TriggerSelectLeft") >= 1)
        {
            ice = false;
            point.placeTower = false;
        }

	}
	
	public void PlaceIce()
	{
		point.placeTower = !point.placeTower;
		ice = !ice;
	}
}
