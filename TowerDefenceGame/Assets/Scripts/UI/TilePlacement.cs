using UnityEngine;
using System.Collections;

public class TilePlacement : MonoBehaviour {

	public bool isTrue = false;

	bool done = false;
	
	GameObject panel;

	GameObject[] paths;
	
	void Start()
	{
		panel = GameObject.FindGameObjectWithTag ("TileSelect");
		paths = GameObject.FindGameObjectsWithTag("Path");
	}
	
	void Update()
	{
		if (!done) 
		{
			foreach(GameObject obj in paths)
			{
				obj.SetActive(false);
			}

			done = true;
		}

		if (isTrue) 
		{
			panel.SetActive (true);

		} 
		else 
		{
			panel.SetActive(false);
			done = false;
		}
		
	}
	
	public void Placement()
	{
		isTrue = !isTrue;
		
	}
}
