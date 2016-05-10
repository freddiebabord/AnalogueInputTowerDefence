using UnityEngine;
using System.Collections;

public class ballistaUI : MonoBehaviour {

	Pointer point;
	
	TowerPlacement towers;
	
	GameObject tile;

	GameObject current;

	GameObject go;

	public bool hologram = false;

	bool ballista = false;
    public float cost = 150;
	// Use this for initialization
	void Start () {
		
		towers = GameObject.FindObjectOfType<TowerPlacement> ();
		point = GameObject.FindObjectOfType<Pointer> ();
	}
	
	// Update is called once per frame
	void Update () {

		if ((!hologram && go != null) || point.OverUI || !ballista)
			Destroy (go);

		if (ballista && !towers.isTrue)
			point.placeTower = false;

		if (!point.OverUI) {
			if (ballista && point.placeTower && point.currentTile.GetComponent<NodePath> ().pathType == NodePath.PathType.Grass && !point.currentTile.GetComponent<NodePath> ().towerPlaced) {
				tile = point.currentTile;

				if (!hologram) {
					go = Instantiate (Resources.Load ("Prefabs/Towers/Ballistics"), tile.transform.position, tile.transform.rotation) as GameObject;

					Ballistics bal = go.GetComponentInChildren<Ballistics> ();
					bal.enabled = false;
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
			} else {
				tile = null;
				current = null;
			}
		
			if (ballista && point.placeTower && Input.GetAxis ("TriggerSelectRight") >= 1) {
				if (tile != null) {
					if (tile.GetComponent<NodePath> ().pathType == NodePath.PathType.Grass && !tile.GetComponent<NodePath> ().towerPlaced) {
						if (GameObject.FindObjectOfType<GameManager> ().gold - cost > 0) {
							Instantiate (Resources.Load ("Prefabs/Towers/Ballistics"), tile.transform.position, tile.transform.rotation);
							tile.GetComponent<NodePath> ().towerPlaced = true;
							GameObject.FindObjectOfType<GameManager> ().RemoveGold (cost);
						}
					}
				}
			}
		}

        if (Input.GetAxis("TriggerSelectLeft") >= 1)
        {
            ballista = false;
            point.placeTower = false;
        }

	}
	
	public void PlaceBallista()
	{
		point.placeTower = !point.placeTower;
		ballista = !ballista;
		hologram = false;
	}
}
