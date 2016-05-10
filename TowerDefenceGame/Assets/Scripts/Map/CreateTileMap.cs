using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class CreateTileMap : MonoBehaviour {
	
	GameObject[] TileType;
	GameObject pointer;
	string[] Text;
	public int Column = 20;
	public int row = 20;
	Vector3 Temp;
	int index = 0;
	GameObject panel;
	TurnOff off;
	string TileIndex;
	bool chooseSize = true;
	public Text Width;
	public Text Height;

	// Use this for initialization
	void Start () {
		panel = GameObject.FindGameObjectWithTag ("Panel");
		
		off = GameObject.FindObjectOfType<TurnOff> ();
		TileIndex = "GrassTile";
	}

	public void CreateMap()
	{
		chooseSize = false;
		foreach (GameObject ui in off.towers())
			ui.SetActive (true);
		
		panel.SetActive (false);
		TileType = new GameObject[Column*row];
		Text = new string[row];
		for (int z = 0; z < row; z++)
		{
			for (int x = 0; x < Column; x++)
			{
				TileType[(z*Column)+x] = Instantiate(Resources.Load("Prefabs/Tiles/GrassTile")) as GameObject; 
				Temp.Set(x*4 , 0, z*4);
				TileType[(z*Column)+x].transform.position = Temp;
				TileType[(z*Column)+x].transform.parent = this.gameObject.transform;

			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(chooseSize == true)
		{
			Width.text = Column.ToString();
			Height.text = row.ToString();
		}
	}

	public void SetGrass(){TileIndex = "GrassTile";}

	public void SetPath(){TileIndex = "PathTile";}

	public void SetRock(){TileIndex = "RockTile";}

	public void SetTree(){TileIndex = "TreeTile";}

	public void SetWater(){TileIndex = "WaterTile";}

	public void SetEmpty(){TileIndex = "EmptyTile";}

	public void SetSpawn(){TileIndex = "SpawnTile";}

	public void SetEnd(){TileIndex = "EndTile";}

	public void increaseX() {if (Column < 30) {Column ++;}}
	public void decreaseX() {if (Column > 10) {Column --;}}
	public void increaseZ() {if (row < 30) {row ++;}}
	public void decreaseZ() {if (row > 10) {row --;}}


	public void changeTile(GameObject raycastResults)
	{
		index = 0;
		while (TileType[index].transform != raycastResults.gameObject.transform) {index++;}
		if(TileIndex == "EndTile")
		{
			for(int t = 0; t < TileType.Length; t++)
			{
				if (TileType[t].name == "EndTile(Clone)") {return;}
			}
		}
		Temp = TileType[index].transform.position;
		Destroy(TileType[index]);
		TileType[index] = Instantiate(Resources.Load("Prefabs/Tiles/" + TileIndex)) as GameObject;
		TileType[index].transform.position = Temp;
		TileType[index].transform.parent = this.gameObject.transform;
	}

	public void save()
	{
		for (int z = 0; z < row; z++)
		{
			for (int x = 0; x < Column; x++)
			{
				char TileLetter = GetTileLetter(TileType[(z*Column)+x].name);
				Text[z] += TileLetter;
			}
		}

		string file = EditorUtility.SaveFilePanel ("Save File location", Application.dataPath + @"/Levels/", "TDMap.txt", ".txt");
		if (file != "") 
		{
			StreamWriter writer = new StreamWriter(file);
			writer.WriteLine(Column);
			writer.WriteLine(row);
			
			for (int z = 0; z < row; z++) {	writer.WriteLine(Text[z]);}
			writer.Close();
		}
	}

	char GetTileLetter(string name)
	{
		if (name == "GrassTile(Clone)"){return (char)'G';}
		if (name == "WaterTile(Clone)"){return (char)'W';}
		if (name == "TreeTile(Clone)"){return (char)'T';}
		if (name == "RockTile(Clone)"){return (char)'R';}
		if (name == "PathTile(Clone)"){return (char)'P';}
		if (name == "EmptyTile(Clone)"){return (char)'N';}
		if (name == "SpawnTile(Clone)"){return (char)'S';}
		if (name == "EndTile(Clone)"){return (char)'E';}
		return (char)'_';
	}
}