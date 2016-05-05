using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class TileBase : MonoBehaviour {
	GameObject[] Tile;
	string[] Txt;
	Vector3 Temp;
	int RowX;
	int RowZ;
    RailManager railManager;

	// Use this for initialization
	void Start () {
        railManager = GameObject.FindObjectOfType<RailManager>();
        CreateMap ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	void CreateMap()
	{
		string file = EditorUtility.OpenFilePanel ("Select Map", Application.dataPath, "txt" );
		
		StreamReader reader =new  StreamReader(file);
		
		RowX = int.Parse(reader.ReadLine ());
		RowZ = int.Parse(reader.ReadLine ());

        Debug.Log(RowX + " " + RowZ);

        GameObject[,]map = new GameObject[RowX, RowZ];
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
                    map[x, z] = Instantiate(Resources.Load(Index)) as GameObject; 
					Temp.Set(x*4 , 0, z*4);
                    map[x, z].transform.position = Temp;
                    map[x, z].AddComponent<NodePath>();
                    if(Index == "G")
                        map[x, z].GetComponent<NodePath>().pathType = NodePath.PathType.Grass;
                    else if (Index == "W")
                        map[x, z].GetComponent<NodePath>().pathType = NodePath.PathType.Water;
                    else if (Index == "R")
                        map[x, z].GetComponent<NodePath>().pathType = NodePath.PathType.Rock;
                    else if (Index == "T")
                        map[x, z].GetComponent<NodePath>().pathType = NodePath.PathType.Tree;
                    else if (Index == "P")
                    {
                        map[x, z].GetComponent<NodePath>().pathType = NodePath.PathType.EnemyPath;
                    }
                    map[x, z].GetComponent<NodePath>().posX = x ;
                    map[x, z].GetComponent<NodePath>().posY = z ;
                    map[x, z].transform.parent = this.gameObject.transform;
                    if (Txt[z][x] == 'S') { 
                        map[x, z].tag = "EnemyStart";
                    }
                    if (Txt[z][x] == 'E') { map[x, z].tag = "EnemyEnd"; } 
				}
				count++;
			}
		}
        GameObject.FindObjectOfType<RailManager>().BuildNavigationMap(map, RowX, RowZ);
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
