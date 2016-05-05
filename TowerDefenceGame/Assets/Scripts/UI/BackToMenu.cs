using UnityEngine;
using System.Collections;

public class BackToMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LoadMenu()
    {
        Destroy(GameObject.FindObjectOfType<GameManager>().gameObject);
        Application.LoadLevel(0);
    }
}
