using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class TileBase : MonoBehaviour {
	GameObject[] Tile;
	string[] Txt;
	Vector3 Temp;

	// Use this for initialization
	void Start () {
		CreateMap ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	void CreateMap()
	{
		string file = EditorUtility.OpenFilePanel ("Select Map","U:\\GameJam\\", "txt" );
		
		StreamReader reader =new  StreamReader(file);
		
		int RowX = int.Parse(reader.ReadLine ());
		int RowZ = int.Parse(reader.ReadLine ());
		
		GameObject[] Tile = new GameObject[RowX*RowZ];
		Txt = new string[RowZ];
		
		int count = 0;
		for (int z = 0; z < RowZ; z++)
		{
			Txt[z] = reader.ReadLine();
			for (int x = 0; x < RowX; x++)
			{
				string Index = getIndex(Txt[z][x]);
				if (Index != "")
				{
					Tile[count] = Instantiate(Resources.Load(Index)) as GameObject; 
					Temp.Set(x*4 , 0, z*4);
					Tile[count].transform.position = Temp;
					Tile[count].AddComponent("Tile");
					Tile[count].transform.parent = this.gameObject.transform;
				}
				count++;
			}
		}
	}

	string getIndex(char TileType)
	{
		if (TileType == 'G') {return "Prefabs/Tiles/GrassTile";}
		if (TileType == 'P') {return "Prefabs/Tiles/PathTile";}
		if (TileType == 'R') {return "Prefabs/Tiles/RockTile";}
		if (TileType == 'T') {return "Prefabs/Tiles/TreeTile";}
		if (TileType == 'W') {return "Prefabs/Tiles/WaterTile";}
		if (TileType == 'S') {return "Prefabs/Tiles/PathTile";}
		if (TileType == 'E') {return "Prefabs/Tiles/PathTile";}
		return "";
	}
}
