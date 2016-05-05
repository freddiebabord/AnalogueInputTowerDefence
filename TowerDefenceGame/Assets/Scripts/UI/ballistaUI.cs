using UnityEngine;
using System.Collections;

public class ballistaUI : MonoBehaviour {

	Pointer point;
	
	TowerPlacement towers;
	
	GameObject tile;

	bool ballista = false;
    public float cost = 150;
	// Use this for initialization
	void Start () {
		
		towers = GameObject.FindObjectOfType<TowerPlacement> ();
		point = GameObject.FindObjectOfType<Pointer> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (ballista && !towers.isTrue)
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
		
		if (ballista && point.placeTower && Input.GetAxis ("TriggerSelectRight") >= 1) 
		{
			
			if(tile.GetComponent<NodePath>().pathType == NodePath.PathType.Grass && !tile.GetComponent<NodePath>().towerPlaced)
			{
                if (GameObject.FindObjectOfType<GameManager>().gold - cost > 0)
                {
                    Instantiate(Resources.Load("Prefabs/Towers/Ballistics"), tile.transform.position, tile.transform.rotation);
                    tile.GetComponent<NodePath>().towerPlaced = true;
                    GameObject.FindObjectOfType<GameManager>().RemoveGold(cost);
                }
			}
		}
        if (Input.GetAxis("TriggerSelectLeft") >= 1)
        {
            ballista = false;
            point.placeTower = false;
        }

	}
	
	public void PlaceBallista()
	{
		point.placeTower = !point.placeTower;
		ballista = !ballista;
	}
}
