using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class VideoPlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
        ((MovieTexture)GetComponent<RawImage>().mainTexture).loop = true;
        ((MovieTexture)GetComponent<RawImage>().mainTexture).Play();
	
	}
}
