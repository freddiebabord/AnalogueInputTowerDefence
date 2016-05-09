using UnityEngine;
using System.Collections;

public class PathUI : MonoBehaviour {

	Pointer point;
	
	TilePlacement tiles;
	
	GameObject tile;
	
	bool path = false;
	
	// Use this for initialization
	void Start () {
		
		tiles = GameObject.FindObjectOfType<TilePlacement> ();
		point = GameObject.FindObjectOfType<Pointer> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (path && !tiles.isTrue)
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
		
		if (path && point.placeTile && Input.GetAxis ("TriggerSelectRight") >= 1) 
		{
			if (tile != null)
			{
				if (tile.GetComponent<NodePath>().pathType != NodePath.PathType.EnemyPath)
				{
					Vector3 pos = tile.transform.position;
					Quaternion rot = tile.transform.rotation;
					
					Destroy(tile.gameObject);
					
					Instantiate(Resources.Load("Prefabs/Tiles/PathTile"), pos, rot);
					
				}
			}
		}
		
		if(Input.GetAxis ("TriggerSelectLeft") >= 1)
		{
			path = false;
			point.placeTile = false;
		}
	}
	
	public void PlacePath()
	{
		point.placeTile = !point.placeTile;
		path = !path;
	}
}
