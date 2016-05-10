using UnityEngine;
using System.Collections;

public class Arrow : TowerClass {

	ArrowUI ui;

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

		speed = GetLevel () * 4;
		base.Update ();
		
	}
	
	public override void Shooting()
	{
		base.Shooting ();
	}
}
