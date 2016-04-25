using UnityEngine;
using System.Collections;

public class TileBase : MonoBehaviour {
	string[] TileIndex;
	public int Index = 0;
	GameObject[] Tile;
	int RowZ = 20;
	int RowX = 10;
	public Vector3 Temp;
	// Use this for initialization
	void Start () {
		TileIndex = new string[8];
		GameObject[] Tile = new GameObject[RowX*RowZ];
		TileIndex[0] = "Prefabs/Tiles/GrassTile";
		TileIndex[1] = "Prefabs/Tiles/PathTile";
		TileIndex[2] = "Prefabs/Tiles/RockTile";
		TileIndex[3] = "Prefabs/Tiles/TreeTile";
		TileIndex[4] = "Prefabs/Tiles/WaterTile";
		int count = 0;
		for (int z = 0; z < RowZ; z++)
		{
			for (int x = 0; x < RowX; x++)
			{
				Tile[count] = Instantiate(Resources.Load(TileIndex[Index])) as GameObject; 
				Temp.Set(x*4 , 0, z*4);
				Tile[count].transform.position = Temp;
				Tile[count].AddComponent("Tile");
				count++;
				Index = Random.Range(0, 5);
			}
		}



	}
	
	// Update is called once per frame
	void Update () {

	}
}
