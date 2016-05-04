using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public bool invertXAxis = false;
    public bool invertYAxis = false;
    public float movementSpeed = 15;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetAxis("HorizontalRight") >= 0.1f || Input.GetAxis("HorizontalRight") <= -0.1f)
            transform.Translate(Vector3.right * (invertXAxis ? Input.GetAxis("HorizontalRight") : -Input.GetAxis("HorizontalRight")) * movementSpeed * Time.deltaTime);
        if (Input.GetAxis("VerticalRight") >= 0.1f || Input.GetAxis("VerticalRight") <= -0.1f)
            transform.Translate(Vector3.Normalize(Vector3.up+Vector3.forward) * (invertYAxis ? Input.GetAxis("VerticalRight") : -Input.GetAxis("VerticalRight")) * movementSpeed * Time.deltaTime);
    }
}
