using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void LoadClassicGame()
    {
        StartCoroutine(Load(1));
    }

    IEnumerator Load(int index)
    {
        AsyncOperation async = Application.LoadLevelAsync(index);
        yield return async;
    }
}
