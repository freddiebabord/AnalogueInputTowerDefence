using UnityEngine;
using System.Collections;

public class Arrow : TowerClass {

	ArrowUI ui;

	float health = 1f;
	float radius = 1f;
	int level = 1;
	int damage = 2;

	protected override void Start () {

		SetClassHealth (1);
		SetRadius (5);
		SetLevel (1);
		SetCooldown (0.5f);
		SetHealth ();
		
		SetGoal(GameObject.FindGameObjectWithTag("Goal"));
		ui = GameObject.FindObjectOfType<ArrowUI> ();
	}
	
	// Update is called once per frame
	public override void Update () {

		base.Upgrade (health, level, radius, damage);

		speed = GetLevel () * 4;
		base.Update ();
		
	}
	
	public override void Shooting()
	{
		base.Shooting ();
	}
}
