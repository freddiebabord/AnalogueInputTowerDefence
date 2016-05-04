using UnityEngine;
using System.Collections;

public class ObjectDeathTimer : MonoBehaviour {

	public float sleepTimer = 5.0f;

	// Use this for initialization
	void Start () {
		StartCoroutine (Wait ());
	}

	IEnumerator Wait()
	{
		yield return new WaitForSeconds(sleepTimer);
		Destroy (gameObject);
	}
}
