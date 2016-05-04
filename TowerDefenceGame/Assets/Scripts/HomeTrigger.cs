using UnityEngine;
using System.Collections;

public class HomeTrigger : MonoBehaviour {


	private GameManager game;

	// Use this for initialization
	void Start () {
		game = FindObjectOfType<GameManager> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Enemy")
			game.AddEnemyInHome ();
	}
}
