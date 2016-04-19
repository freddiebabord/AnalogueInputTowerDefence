using UnityEngine;
using System.Collections;

public class DestroyBalls : MonoBehaviour {

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Enemy") 
		{
			AIBase ai = col.gameObject.GetComponent<AIBase>();
			ai.ApplyDamage(100);
		}

		Destroy (gameObject);
	}
}
