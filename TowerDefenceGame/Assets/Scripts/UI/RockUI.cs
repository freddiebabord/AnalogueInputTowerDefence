using UnityEngine;
using System.Collections;

public class RockUI : MonoBehaviour {

	Pointer point;
	
	TilePlacement tiles;
	
	GameObject tile;
	
	Mapping maps;
	
	bool rock = false;
	
	// Use this for initialization
	void Start () {
		
		tiles = GameObject.FindObjectOfType<TilePlacement> ();
		point = GameObject.FindObjectOfType<Pointer> ();
		maps = GameObject.FindObjectOfType<Mapping> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (rock && !tiles.isTrue)
			point.placeTile = false;
		
		if (point.placeTile) 
		{
			tile = point.currentTile;
			//GameObject go = Instantiate (Resources.Load ("Prefabs/tiles/" + index.ToString ()), tile.transform.position, tile.transform.rotation) as GameObject;
			
			if (Input.GetAxis ("TriggerSelectRight") < 1) 
			{
				//Destroy(go);
			}
		} 
		else 
		{
			tile = null;
		}
		
		if (rock && point.placeTile && Input.GetAxis ("TriggerSelectRight") >= 1) 
		{
			if (tile != null)
			{
				if (tile.GetComponent<NodePath>().pathType != NodePath.PathType.Rock)
				{
					Vector3 pos = tile.transform.position;
					Quaternion rot = tile.transform.rotation;
					
					int x = (int)pos.x/4;
					int y = (int)pos.z/4;
					
					int index = x + (y * maps.width);
					
					maps.map[index] = Instantiate(Resources.Load("Prefabs/Tiles/RockTile"), pos, rot) as GameObject;
					
					Destroy(tile.gameObject);
				}
			}
		}
		
		if(Input.GetAxis ("TriggerSelectLeft") >= 1)
		{
			rock = false;
			point.placeTile = false;
		}
	}
	
	public void PlaceRock()
	{
		point.placeTile = !point.placeTile;
		rock = !rock;
	}
}