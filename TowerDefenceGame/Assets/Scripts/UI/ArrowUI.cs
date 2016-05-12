using UnityEngine;
using System.Collections;

public class ArrowUI : MonoBehaviour {

	Stat stat;
	Magic magic;
	Ice ice;
	ballistaUI ball;

	Pointer point;
	
	TowerPlacement towers;
	
	GameObject tile;

	GameObject current;

	GameObject go;

	public bool hologram = false;

	public bool arrow = false;
	public float cost = 100;
	
	// Use this for initialization
	void Start () {
		
		towers = GameObject.FindObjectOfType<TowerPlacement> ();
		point = GameObject.FindObjectOfType<Pointer> ();

		stat = GameObject.FindObjectOfType<Stat> ();
		magic = GameObject.FindObjectOfType<Magic> ();
		ice = GameObject.FindObjectOfType<Ice> ();
		ball = GameObject.FindObjectOfType<ballistaUI> ();
	}
	
	// Update is called once per frame
	void Update () {

		if ((!hologram && go != null) || point.OverUI || !arrow)
			Destroy (go);

		if (arrow && !towers.isTrue)
			point.placeTower = false;

		if (arrow) 
		{
			stat.stats = false;
			magic.mage = false;
			ice.ice = false;
			ball.ballista = false;
		}

		if (!point.OverUI) {
			if (arrow && point.placeTower && point.currentTile.GetComponent<NodePath> ().pathType == NodePath.PathType.Grass && !point.currentTile.GetComponent<NodePath> ().towerPlaced) {
				tile = point.currentTile;

				if (!hologram) {
					go = Instantiate (Resources.Load ("Prefabs/Towers/ArrowTower"), tile.transform.position, tile.transform.rotation) as GameObject;

					Ballistics arr = go.GetComponentInChildren<Ballistics> ();
					arr.enabled = false;
					hologram = true;

				}

				if (hologram) 
				{
					go.gameObject.transform.position = tile.transform.position;
					go.gameObject.transform.rotation = tile.transform.rotation;
					
					Transform[] t = go.gameObject.GetComponentsInChildren<Transform>();
					foreach(Transform transform in t)
					{
						if(transform.renderer != null)
						{
							transform.renderer.material = Resources.Load("Prefabs/Materials/Holo") as Material;
							
							if(point.currentTile.GetComponent<NodePath> ().pathType == NodePath.PathType.Grass && !point.currentTile.GetComponent<NodePath>().towerPlaced)
								transform.renderer.material.SetColor("_Colour", new Color(0,0,1,1));
							else
								transform.renderer.material.SetColor("_Colour", new Color(1,0,0,1));
						}
						
					}
				}
			
				current = tile;
			} else {
				tile = null;
				current = null;
			}
		
			if (arrow && point.placeTower && Input.GetAxis ("TriggerSelectRight") >= 1) {
				if (tile != null) {
					if (tile.GetComponent<NodePath> ().pathType == NodePath.PathType.Grass && !tile.GetComponent<NodePath> ().towerPlaced) {
						if (GameObject.FindObjectOfType<GameManager> ().gold - cost > 0) {
							Instantiate (Resources.Load ("Prefabs/Towers/ArrowTower"), tile.transform.position, tile.transform.rotation);
							tile.GetComponent<NodePath> ().towerPlaced = true;
							GameObject.FindObjectOfType<GameManager> ().RemoveGold (cost);
						}
					}
				}
			}
		}

        if (Input.GetAxis("TriggerSelectLeft") >= 1)
        {
            arrow = false;
            point.placeTower = false;
        }
	}
	
	public void PlaceArrow()
	{
		point.placeTower = !point.placeTower;
		arrow = !arrow;
		hologram = false;

	}
}
