using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public bool invertXAxis = false;
    public bool invertYAxis = false;
    public float movementSpeed = 15;

    public bool shouldBeActive = true;
    public Transition levelFader;
    GameManager game;

    void Start()
    {
        game = GameObject.FindObjectOfType<GameManager>();
        GameObject.FindObjectOfType<WorldSpaceCanvasScaler>().UpdateSize();
        StartCoroutine(WaitForMapReady());
    }

	// Update is called once per frame
	void Update () {
        if (shouldBeActive)
        {
            if (Input.GetAxis("HorizontalRight") >= 0.1f || Input.GetAxis("HorizontalRight") <= -0.1f)
                transform.Translate(Vector3.right * (invertXAxis ? Input.GetAxis("HorizontalRight") : -Input.GetAxis("HorizontalRight")) * movementSpeed * Time.deltaTime);
            if (Input.GetAxis("VerticalRight") >= 0.1f || Input.GetAxis("VerticalRight") <= -0.1f)
                transform.Translate(Vector3.Normalize(Vector3.up + Vector3.forward) * (invertYAxis ? Input.GetAxis("VerticalRight") : -Input.GetAxis("VerticalRight")) * movementSpeed * Time.deltaTime);
        }
    }

    public void TranslatCameraHorisontal(float mag)
    {
        if (shouldBeActive)
            transform.Translate(Vector3.right * mag * movementSpeed * Time.deltaTime);
    }

    public void TranslatCameraVertical(float mag)
    {
        if(shouldBeActive)
            transform.Translate(Vector3.Normalize(Vector3.up + Vector3.forward) * (invertYAxis ? mag : -mag) * movementSpeed * Time.deltaTime);
    }
    IEnumerator WaitForMapReady()
    {
        while (!game.MapReady)
        {
            yield return null;
        }

        levelFader.StartTranstion(true);
        
    }
}
