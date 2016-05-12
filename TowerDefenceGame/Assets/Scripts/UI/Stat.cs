using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Stat : MonoBehaviour {

	Magic magic;
	ballistaUI ball;
	Ice ice;
	ArrowUI arr;

	Pointer point;

	TowerPlacement tower;

	GameObject tile;

	GameObject go;

	GameObject statUI;

	public Text name;
	public Text level;
	public Text damage;
	public GameObject upgradeButton;

	public bool stats = false;
	// Use this for initialization
	void Start () {

		point = GameObject.FindObjectOfType<Pointer> ();
		tower = GameObject.FindObjectOfType<TowerPlacement> ();
		statUI = GameObject.FindGameObjectWithTag("Info");

		magic = GameObject.FindObjectOfType<Magic> ();
		ball = GameObject.FindObjectOfType<ballistaUI> ();
		ice = GameObject.FindObjectOfType<Ice> ();
		arr = GameObject.FindObjectOfType<ArrowUI> ();

	}
	
	// Update is called once per frame
	void Update () {

		if (!statUI.activeSelf)
			statUI.SetActive (true);

		if (stats) 
		{
			magic.mage = false;
			ball.ballista = false;
			ice.ice = false;
			arr.arrow = false;

			if(point.currentTile.GetComponent<NodePath>().towerPlaced && Input.GetAxis("TriggerSelectRight") >= 1)
			{
				go = SearchTower(point.currentTile.gameObject.transform.position);
				name.text = go.tag;

				if(name.text == "Mage")
				{
					level.text = go.GetComponentInChildren<Mage>().GetLevel().ToString();
					damage.text = go.GetComponentInChildren<Mage>().GetHealth().ToString();

					if(go.GetComponentInChildren<Mage>().upgradable)
						upgradeButton.GetComponent<AnalogueButtons>().interactable = true;
					else
						upgradeButton.GetComponent<AnalogueButtons>().interactable = false;

				}
				else if(name.text == "Arrow")
				{
					level.text = go.GetComponentInChildren<Arrow>().GetLevel().ToString();
					damage.text = go.GetComponentInChildren<Arrow>().GetHealth().ToString();

					if(go.GetComponentInChildren<Mage>().upgradable)
						upgradeButton.GetComponent<AnalogueButtons>().interactable = true;
					else
						upgradeButton.GetComponent<AnalogueButtons>().interactable = false;
				}
				else if(name.text == "Freeze")
				{
					level.text = go.GetComponentInChildren<Absorbing>().GetLevel().ToString();
					damage.text = go.GetComponentInChildren<Absorbing>().GetHealth().ToString();

					if(go.GetComponentInChildren<Mage>().upgradable)
						upgradeButton.GetComponent<AnalogueButtons>().interactable = true;
					else
						upgradeButton.GetComponent<AnalogueButtons>().interactable = false;
				}
				else if(name.text == "Balista")
				{
					level.text = go.GetComponentInChildren<Ballistics>().GetLevel().ToString();
					damage.text = go.GetComponentInChildren<Ballistics>().GetHealth().ToString();

					if(go.GetComponentInChildren<Mage>().upgradable)
						upgradeButton.GetComponent<AnalogueButtons>().interactable = true;
					else
						upgradeButton.GetComponent<AnalogueButtons>().interactable = false;
				}

			}
		}
	
	}

	GameObject SearchTower(Vector3 pos)
	{
		GameObject[] ice = GameObject.FindGameObjectsWithTag ("Freeze"); 
		GameObject[] mage = GameObject.FindGameObjectsWithTag ("Mage"); 
		GameObject[] balls = GameObject.FindGameObjectsWithTag ("Balista"); 
		GameObject[] arr = GameObject.FindGameObjectsWithTag ("Arrow"); 

		foreach (GameObject t in ice) 
		{
			if(t.transform.position == pos)
				return t;
			else
				continue;
		}

		foreach (GameObject t in mage) 
		{
			if(t.transform.position == pos)
				return t;
			else
				continue;
		}

		foreach (GameObject t in balls) 
		{
			if(t.transform.position == pos)
				return t;
			else
				continue;
		}

		foreach (GameObject t in arr) 
		{
			if(t.transform.position == pos)
				return t;
			else
				continue;
		}

		return null;
	}

	GameObject SearchTile(Vector3 pos)
	{
		GameObject[] tiles = GameObject.FindGameObjectsWithTag ("Grass");

		foreach (GameObject t in tiles) 
		{
			if(t.transform.position == pos)
				return t;
			else
				continue;
		}

		return null;
	}

	public void GetStats()
	{
		point.placeTower = !point.placeTower;
		stats = !stats;
	}

	public void Upgrading()
	{
		if (go.tag == "Arrow") 
		{
			go.GetComponentInChildren<Arrow> ().upgradePressed = true;
		}
		else if (go.tag == "Mage") 
		{
			go.GetComponentInChildren<Mage> ().upgradePressed = true;
		}
		else if (go.tag == "Balista") 
		{
			go.GetComponentInChildren<Ballistics> ().upgradePressed = true;
		}
		else if (go.tag == "Freeze") 
		{
			go.GetComponentInChildren<Absorbing> ().upgradePressed = true;
		}
	}

	public void Delete()
	{
		if (go.tag == "Arrow") 
		{
			var bui = GameObject.FindObjectOfType<ArrowUI>();
			
			float gold = bui.cost -  (10*(bui.cost/ bui.cost));
			GameManager.Instance.AddGold(gold);
			GameObject tiley = SearchTile(go.gameObject.transform.position);
			tiley.GetComponent<NodePath>().towerPlaced = false;
			Destroy (go);
		}
		else if (go.tag == "Mage") 
		{
			var bui = GameObject.FindObjectOfType<Magic>();
			
			float gold = bui.cost -  (10*(bui.cost/ bui.cost));
			GameManager.Instance.AddGold(gold);
			GameObject tiley = SearchTile(go.gameObject.transform.position);
			tiley.GetComponent<NodePath>().towerPlaced = false;
			Destroy (go);
		}
		else if (go.tag == "Balista") 
		{
			var bui = GameObject.FindObjectOfType<ballistaUI>();

			float gold = bui.cost -  (10*(bui.cost/ bui.cost));
			GameManager.Instance.AddGold(gold);
			GameObject tiley = SearchTile(go.gameObject.transform.position);
			tiley.GetComponent<NodePath>().towerPlaced = false;
			Destroy (go);

		}
		else if (go.tag == "Freeze") 
		{
			var bui = GameObject.FindObjectOfType<Ice>();
			
			float gold = bui.cost -  (10*(bui.cost/ bui.cost));
			GameManager.Instance.AddGold(gold);
			GameObject tiley = SearchTile(go.gameObject.transform.position);
			tiley.GetComponent<NodePath>().towerPlaced = false;
			Destroy (go);

		}
	}

}
