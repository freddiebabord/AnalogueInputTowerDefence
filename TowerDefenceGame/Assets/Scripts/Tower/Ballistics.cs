using UnityEngine;
using System.Collections;

public class Ballistics : TowerClass {

	Animation anim;
	ballistaUI ui;

	float health = 1f;
	float radius = 0.3f;
	int level = 1;
	int damage = 5;

	// Use this for initialization
	protected override void Start () {

		anim = GetComponentInChildren<Animation> ();

		SetClassHealth (4);
		SetRadius (0);
		SetLevel (1);
		SetCooldown (0.5f);
		SetHealth ();
		
		SetGoal(GameObject.FindGameObjectWithTag("Goal"));
		ui = GameObject.FindObjectOfType<ballistaUI> ();
	}
	
	// Update is called once per frame
	public override void Update () {

		base.Upgrade (health, level, radius, damage);
	
		speed = GetLevel () * 2;

		if (!isFired)
			anim.Stop ();

		base.Update ();

	}

	public override void Shooting()
	{
		anim.Play ();
		base.Shooting ();
	}
}
