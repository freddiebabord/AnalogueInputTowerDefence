using UnityEngine;
using System.Collections;

public class Mage : TowerClass {

	LineRenderer line;

	// Use this for initialization
	protected override void Start () {
		gameObject.tag = "Mage";

		SetClassHealth (3);
		SetRadius (0);
		SetLevel (1);
		SetCooldown (0);
		SetHealth ();
					
		SetGoal(GameObject.FindGameObjectWithTag("Goal"));

		line = gameObject.GetComponentInChildren<LineRenderer>();

	}

	// Update is called once per frame
	public override void Update () {

		if (!isFired) 
		{
			line.enabled = false;
		} 
		else 
		{
			line.enabled = true;
		}

		base.Update ();
	
	}

	public override void Shooting ()
	{
		isFired = true;
		
		lastShot = 0f;

		line.SetPosition (0, GameObject.FindGameObjectWithTag ("Mage").gameObject.transform.position);
		line.SetPosition (1, GetChosen().transform.position);

		if (GetChosen ().gameObject.tag == "Enemy")
			GetChosen().gameObject.GetComponent<AIBase> ().ApplyDamage (2);

	}
}
