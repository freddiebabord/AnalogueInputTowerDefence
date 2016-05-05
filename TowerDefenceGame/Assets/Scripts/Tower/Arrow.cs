using UnityEngine;
using System.Collections;

public class Arrow : TowerClass {

	protected override void Start () {

		SetClassHealth (1);
		SetRadius (5);
		SetLevel (1);
		SetCooldown (0.5f);
		SetHealth ();
		
		SetGoal(GameObject.FindGameObjectWithTag("Goal"));
		
	}
	
	// Update is called once per frame
	public override void Update () {

		speed = GetLevel () * 4;
		base.Update ();
		
	}
	
	public override void Shooting()
	{
		base.Shooting ();
	}
}
