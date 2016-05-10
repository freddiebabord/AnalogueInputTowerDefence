using UnityEngine;
using System.Collections;

public class Ballistics : TowerClass {

	Animation anim;
	ballistaUI ui;

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
	
		if (!ui.hologram) 
		{
			Transform[] t = gameObject.GetComponentsInChildren<Transform>();
			Debug.Log (t.Length);
			foreach(Transform transform in t)
			{
				Debug.Log (t);
				if(transform.renderer != null)
					transform.renderer.material = Resources.Load("Prefabs/Materials/Holo") as Material;
			}
		}

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
