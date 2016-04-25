using UnityEngine;
using System.Collections;

public class TestButtonSpawnObj : MonoBehaviour {

    public GameObject temp;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Spawn()
    {
        Instantiate(temp, transform.position, transform.rotation);
    }
}
