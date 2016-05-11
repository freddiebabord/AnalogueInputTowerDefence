using UnityEngine;
using System.Collections;

public class Mage : TowerClass {

	LineRenderer line;
	Magic magic;

	float health = 2f;
	float radius = 0.3f;
	int level = 1;
	int damage = 2;
	
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
		magic = GameObject.FindObjectOfType<Magic> ();
	}

	// Update is called once per frame
	public override void Update () {

		base.Upgrade (health, level, radius, damage);

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

		line.SetPosition (0, gameObject.transform.position);
		line.SetPosition (1, GetChosen().transform.position);

		if (GetChosen ().gameObject.tag == "Enemy")
			GetChosen().gameObject.GetComponent<AIBase> ().ApplyDamage (2);

	}

    public override void ApplyDamage(float HPLoss)
    {
        base.ApplyDamage(HPLoss);
    }
}
