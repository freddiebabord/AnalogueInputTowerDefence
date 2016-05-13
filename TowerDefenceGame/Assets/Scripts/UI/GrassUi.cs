using UnityEngine;
using System.Collections;

public class GrassUi : MonoBehaviour {

	Pointer point;
	
	TilePlacement tiles;
	
	GameObject tile;

	Mapping maps;
	
	bool grass = false;

	// Use this for initialization
	void Start () {
		
		tiles = GameObject.FindObjectOfType<TilePlacement> ();
		point = GameObject.FindObjectOfType<Pointer> ();
		maps = GameObject.FindObjectOfType<Mapping> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (grass && !tiles.isTrue)
			point.placeTile = false;
		
		if (point.placeTile) 
			tile = point.currentTile;
		else 
			tile = null;
		
		if (grass && point.placeTile && Input.GetAxis ("TriggerSelectRight") >= 1) 
		{
			if (tile != null)
			{
				if (tile.GetComponent<NodePath>().pathType != NodePath.PathType.Grass)
				{
					Vector3 pos = tile.transform.position;
					Quaternion rot = tile.transform.rotation;

					int x = (int)pos.x/4;
					int y = (int)pos.z/4;

					int index = x + (y * maps.width);

					maps.map[index] = Instantiate(Resources.Load("Prefabs/Tiles/GrassTile"), pos, rot) as GameObject;

					Destroy(tile.gameObject);
				}
			}
		}
		
		if(Input.GetAxis ("TriggerSelectLeft") >= 1)
		{
			grass = false;
			point.placeTile = false;
		}
	}
	
	public void PlaceGrass()
	{
		point.placeTile = !point.placeTile;
		grass = !grass;
	}
}
