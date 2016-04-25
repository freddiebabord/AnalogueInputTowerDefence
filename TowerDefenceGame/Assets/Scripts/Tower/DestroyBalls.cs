using UnityEngine;
using System.Collections;

public class DestroyBalls : MonoBehaviour {

	void Start()
	{
		StartCoroutine (DestroyTimer ());
	}

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Enemy") 
		{
			AIBase ai = col.gameObject.GetComponent<AIBase>();
			ai.ApplyDamage(100);
		}

		Destroy (gameObject);
	}

	IEnumerator DestroyTimer()
	{
		yield return new WaitForSeconds (5);
		Destroy (gameObject);
	}
}
