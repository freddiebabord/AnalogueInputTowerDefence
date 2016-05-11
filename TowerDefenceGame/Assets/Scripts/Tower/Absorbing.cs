using UnityEngine;
using System.Collections;

public class Absorbing : TowerClass {

	RailManager rail;
	//LineRenderer line;
	PlayFireAnim bomb;

	Ice icy;

	// Use this for initialization
	protected override void Start () {

		gameObject.tag = "Absorbing";

		SetClassHealth (2);
		SetRadius (1);
		SetLevel (1);
		SetCooldown (2);
		SetHealth ();
		
		SetGoal(GameObject.FindGameObjectWithTag("Goal"));

		icy = GameObject.FindObjectOfType<Ice> ();
		bomb = gameObject.GetComponentInChildren<PlayFireAnim> ();

	}
	
	public override void Update () {

		base.Update ();
		
	}
	
	public override void Shooting ()
	{
		isFired = true;
		
		lastShot = 0f;

		bomb.Play ();
		
		//line.SetPosition (0, gameObject.transform.position);
		//line.SetPosition (1, GetChosen().transform.position);

		/*
		if (GetChosen ().gameObject.tag == "Enemy") 
		{
			GetChosen().gameObject.GetComponent<AIBase> ().ApplyDamage (10);
            GetChosen().gameObject.GetComponent<AIBase>().Speed /= 2;
		}
		*/

		if (GetChosen ().gameObject.tag == "Enemy") 
		{
			foreach (GameObject enemy in ai) 
			{
				if (Vector3.Distance (enemy.transform.position, gameObject.transform.position) <= GetRadius ()) 
				{
					enemy.gameObject.GetComponent<AIBase> ().ApplyDamage (10);
					if(enemy.gameObject.GetComponent<AIBase> ().Speed >= 3)
						enemy.gameObject.GetComponent<AIBase> ().Speed /= 2;
				}
			}
		}

	}
}
