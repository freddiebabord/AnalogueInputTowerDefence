using UnityEngine;
using System.Collections;

public class PlayFireAnim : MonoBehaviour {

    public Animation fireAnim;

	// Use this for initialization
	void Start () {

        if(!fireAnim)
        {
            fireAnim = GetComponent<Animation>();
        }
	
	}

    public void Play()
    {
        if(fireAnim)
        {
            fireAnim.Play();
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
