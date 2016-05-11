using UnityEngine;
using System.Collections;

public class TowerPlacement : MonoBehaviour {

	public bool isTrue = false;
    public Animation animation;

	GameObject panel;
    bool isPlaying = false;
    bool done = false;  
	void Start()
	{
		panel = GameObject.FindGameObjectWithTag ("TowerSelect");
	}

	void Update()
	{
        if (isTrue && !done)
        {
            panel.SetActive(true);
            //animation["PlaceItemAnimation"].speed = 1;
            animation.Play();
            done = true;
        }
        else if (!isPlaying && !done)
        {
           // animation["PlaceItemAnimation"].speed = -1;
            animation.Play();
            StartCoroutine(waitForAnimComplete());
            done = true;
        }
        
	}

	public void Placement()
	{
		isTrue = !isTrue;
        done = false;
	}

    IEnumerator waitForAnimComplete()
    {
        isPlaying = true;
        while(animation.IsPlaying("PlaceItemAnimation"))
            yield return null;
        panel.SetActive(false);
        isPlaying = false;
    }
}
