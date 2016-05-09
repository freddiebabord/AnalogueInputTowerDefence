using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class CreateTileMap : MonoBehaviour {
	
	GameObject[] TileType;
	GameObject pointer;
	string[] Text;
	public int Column = 20;
	public int row = 20;
	Vector3 Temp;
	int index = 0;
	bool isMade = false;
	GameObject panel;
	TurnOff off;

	// Use this for initialization
	void Start () {
		panel = GameObject.FindGameObjectWithTag ("Panel");
		
		off = GameObject.FindObjectOfType<TurnOff> ();
	}

	public void CreateMap()
	{
		foreach (GameObject ui in off.towers())
			ui.SetActive (true);
		
		panel.SetActive (false);
		TileType = new GameObject[Column*row];
		Text = new string[row];
		pointer = Instantiate (Resources.Load("Prefabs/Towers/TowerBase 2")) as GameObject;
		for (int z = 0; z < row; z++)
		{
			for (int x = 0; x < Column; x++)
			{
				TileType[(z*Column)+x] = Instantiate(Resources.Load("Prefabs/Tiles/GrassTile")) as GameObject; 
				Temp.Set(x*4 , 0, z*4);
				TileType[(z*Column)+x].transform.position = Temp;
				TileType[(z*Column)+x].AddComponent("Tile");
				TileType[(z*Column)+x].transform.parent = this.gameObject.transform;

			}
		}
		isMade = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (isMade) 
		{
			getIndex ();
			pointer.transform.position = TileType [index].transform.position;
			/*
			if (Input.GetKeyDown ("1")) {changeTile ("GrassTile");}
			if (Input.GetKeyDown ("2")) {changeTile ("PathTile");}
			if (Input.GetKeyDown ("3")) {changeTile ("RockTile");}
			if (Input.GetKeyDown ("4")) {changeTile ("TreeTile");}
			if (Input.GetKeyDown ("5")) {changeTile ("WaterTile");}
			if (Input.GetKeyDown ("6")) {changeTile ("EmptyTile");}
			if (Input.GetKeyDown ("space")) {getTextFile ();}
			if (Input.GetKeyDown ("s")) {changeTile ("StartTile");}
			if (Input.GetKeyDown ("e")) {changeTile ("EndTile");}*/
		}
	}

	public void changeTile(string tile, GameObject Tile)
	{
		index = 0;
		while (TileType[index] != Tile) {index++;}
		if(tile == "EndTile")
		{
			for(int t = 0; t < TileType.Length; t++)
			{
				if (TileType[t].tag == "End") {return;}
			}
		}
		Temp = TileType[index].transform.position;
		Destroy(TileType[index]);
		TileType[index] = Instantiate(Resources.Load("Prefabs/Tiles/" + tile)) as GameObject;
		TileType[index].transform.position = Temp;
		TileType[index].transform.parent = this.gameObject.transform;
	}

	void getIndex()
	{
		if (Input.GetKeyDown ("up")) 
		{
			if(index != 0){index--;}
			else{index = (Column)*(row)-1;}
		}

		if (Input.GetKeyDown ("down")) 
		{
			if(index != (Column*row)-1){index++;}
			else{index = 0;}
		}

		if (Input.GetKeyDown ("left")) 
		{
			if(index >= Column){index -= Column;}
			else{index = (Column*row)- (Column -index);}
		}

		if (Input.GetKeyDown ("right")) 
		{
			if(index <= ((Column*row)-Column)-1){index+= Column;}
			else{index = Column-((Column*row)- index)-1;}
		}
	}

	void getTextFile()
	{
		for (int z = 0; z < row; z++)
		{
			for (int x = 0; x < Column; x++)
			{
				char TileLetter = GetTileLetter(TileType[(z*Column)+x].tag);
				Text[z] += TileLetter;
			}
		}

		string file = EditorUtility.SaveFilePanel ("Save File location", "U:\\GameJam\\", "TDMap.txt", ".txt");
		if (file != "") 
		{
			StreamWriter writer = new StreamWriter(file);
			writer.WriteLine(Column);
			writer.WriteLine(row);
			
			for (int z = 0; z < row; z++) {	writer.WriteLine(Text[z]);}
			writer.Close();
		}
	}

	char GetTileLetter(string tag)
	{
		if (tag == "Grass"){return (char)'G';}
		if (tag == "Water"){return (char)'W';}
		if (tag == "Tree"){return (char)'T';}
		if (tag == "Rock"){return (char)'R';}
		if (tag == "Path"){return (char)'P';}
		if (tag == "Null"){return (char)'N';}
		if (tag == "Start"){return (char)'S';}
		if (tag == "End"){return (char)'E';}
		return (char)'_';
	}
}