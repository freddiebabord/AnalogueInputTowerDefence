using UnityEngine;
using System.Collections;

public class Magic : MonoBehaviour {

	Pointer point;

	TowerPlacement towers;

	GameObject tile;

	GameObject current;

	GameObject go;

	public bool hologram = false;

	bool mage = false;
    public float cost = 100;
	// Use this for initialization
	void Start () {

		towers = GameObject.FindObjectOfType<TowerPlacement> ();
		point = GameObject.FindObjectOfType<Pointer> ();
	}
	
	// Update is called once per frame
	void Update () {

		if ((!hologram && go != null) || point.OverUI || !mage)
			Destroy (go);

		if (mage && !towers.isTrue)
			point.placeTower = false;

		if (!point.OverUI) 
		{
			if (mage && point.placeTower && point.currentTile.GetComponent<NodePath> ().pathType == NodePath.PathType.Grass && !point.currentTile.GetComponent<NodePath> ().towerPlaced) 
			{
				tile = point.currentTile;

				if (!hologram) 
				{
					go = Instantiate (Resources.Load ("Prefabs/Towers/Magic"), tile.transform.position, tile.transform.rotation) as GameObject;

					Mage mag = go.GetComponentInChildren<Mage> ();
					mag.enabled = false;
					hologram = true;

				}

				if (tile != current && hologram) 
				{
					go.gameObject.transform.position = tile.transform.position;
					go.gameObject.transform.rotation = tile.transform.rotation;

					Transform[] t = go.gameObject.GetComponentsInChildren<Transform>();
					Debug.Log (t.Length);
					foreach(Transform transform in t)
					{
						Debug.Log (t);
						if(transform.renderer != null)
							transform.renderer.material = Resources.Load("Prefabs/Materials/Holo") as Material;
					}
				}
			
				current = tile;
			} 
			else 
			{
				tile = null;
				current = null;
			}
		
			if (mage && point.placeTower && Input.GetAxis ("TriggerSelectRight") >= 1) 
			{
				if (tile != null) 
				{
					if (tile.GetComponent<NodePath> ().pathType == NodePath.PathType.Grass && !tile.GetComponent<NodePath> ().towerPlaced) 
					{
						if (GameObject.FindObjectOfType<GameManager> ().gold - cost > 0) 
						{
							Instantiate (Resources.Load ("Prefabs/Towers/Magic"), tile.transform.position, tile.transform.rotation);
							tile.GetComponent<NodePath> ().towerPlaced = true;
							GameObject.FindObjectOfType<GameManager> ().RemoveGold (cost);
						}
					}
				}
			}
		}

        if(Input.GetAxis ("TriggerSelectLeft") >= 1)
        {
            mage = false;
            point.placeTower = false;
        }
	}

	public void PlaceMagic()
	{
		point.placeTower = !point.placeTower;
		mage = !mage;
		hologram = false;

	}
}
