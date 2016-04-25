using UnityEngine;
using System.Collections;

public class CreateTileMap : MonoBehaviour {
	
	GameObject[] TileType;
	GameObject pointer;
	public int Column = 20;
	public int row = 20;
	Vector3 Temp;
	int index = 0;
	// Use this for initialization
	void Start () {
		TileType = new GameObject[Column*row];
		pointer = Instantiate (Resources.Load("Prefabs/Towers/Gun")) as GameObject;
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
	}
	
	// Update is called once per frame
	void Update () {
		getIndex ();
		pointer.transform.position = TileType [index].transform.position;
		SetTile ();
	}

	void SetTile()
	{
		if (Input.GetKeyDown ("1")) 
		{
			Temp = TileType[index].transform.position;
			Destroy(TileType[index]);
			TileType[index] = Instantiate(Resources.Load("Prefabs/Tiles/GrassTile")) as GameObject;
			TileType[index].transform.position = Temp;
			TileType[index].transform.parent = this.gameObject.transform;
		}
		if (Input.GetKeyDown ("2")) 
		{
			Temp = TileType[index].transform.position;
			Destroy(TileType[index]);
			TileType[index] = Instantiate(Resources.Load("Prefabs/Tiles/PathTile")) as GameObject;
			TileType[index].transform.position = Temp;
			TileType[index].transform.parent = this.gameObject.transform;
		}
		if (Input.GetKeyDown ("3")) 
		{
			Temp = TileType[index].transform.position;
			Destroy(TileType[index]);
			TileType[index] = Instantiate(Resources.Load("Prefabs/Tiles/RockTile")) as GameObject;
			TileType[index].transform.position = Temp;
			TileType[index].transform.parent = this.gameObject.transform;
		}
		if (Input.GetKeyDown ("4")) 
		{
			Temp = TileType[index].transform.position;
			Destroy(TileType[index]);
			TileType[index] = Instantiate(Resources.Load("Prefabs/Tiles/TreeTile")) as GameObject;
			TileType[index].transform.position = Temp;
			TileType[index].transform.parent = this.gameObject.transform;
		}
		if (Input.GetKeyDown ("5")) 
		{
			Temp = TileType[index].transform.position;
			Destroy(TileType[index]);
			TileType[index] = Instantiate(Resources.Load("Prefabs/Tiles/WaterTile")) as GameObject;
			TileType[index].transform.position = Temp;
			TileType[index].transform.parent = this.gameObject.transform;
		}
		if (Input.GetKeyDown ("6")) 
		{
			Temp = TileType[index].transform.position;
			Destroy(TileType[index]);
			TileType[index] = Instantiate(Resources.Load("Prefabs/Tiles/EmptyTile")) as GameObject;
			TileType[index].transform.position = Temp;
			TileType[index].transform.parent = this.gameObject.transform;
		}
	}

	void getIndex()
	{
		if (Input.GetKeyDown ("up")) 
		{
			if(index != 0){index--;}
			else{index = Column*row;}
		}

		if (Input.GetKeyDown ("down")) 
		{
			if(index != Column*row){index++;}
			else{index = 0;}
		}

		if (Input.GetKeyDown ("left")) 
		{
			if(index >= Column){index -= Column;}
			else{index = (Column*row)- (Column -index);}
		}

		if (Input.GetKeyDown ("right")) 
		{
			if(index <= (Column*row)-Column){index+= Column;}
			else{index = Column-((Column*row)- index);}
		}

	}
}
