using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public bool invertXAxis = false;
    public bool invertYAxis = false;
    public float movementSpeed = 15;

    public bool shouldBeActive = true;
    public Transition levelFader;
    GameManager game;

    public float MIN_X = -10;
    public float MAX_X = -10;
    public float MIN_Y = 10;
    public float MAX_Y = 10;
    public float MIN_Z = 10;
    public float MAX_Z = 10;

    void Start()
    {
        game = GameObject.FindObjectOfType<GameManager>();
        GameObject.FindObjectOfType<WorldSpaceCanvasScaler>().UpdateSize();
        if (game) 
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
        {
            transform.Translate(Vector3.right * mag * movementSpeed * Time.deltaTime);
            transform.position = new Vector3(
               Mathf.Clamp(transform.position.x, MIN_X, MAX_X),
               Mathf.Clamp(transform.position.y, MIN_Y, MAX_Y),
               Mathf.Clamp(transform.position.z, MIN_Z, MAX_Z));
        }
    }

    public void TranslatCameraVertical(float mag)
    {
        if (shouldBeActive)
        {
            transform.Translate(Vector3.Normalize(Vector3.up + Vector3.forward) * (invertYAxis ? mag : -mag) * movementSpeed * Time.deltaTime);
            transform.position = new Vector3(
                   Mathf.Clamp(transform.position.x, MIN_X, MAX_X),
                   Mathf.Clamp(transform.position.y, MIN_Y, MAX_Y),
                   Mathf.Clamp(transform.position.z, MIN_Z, MAX_Z));
        } 
    }

    IEnumerator WaitForMapReady()
    {
        if (game != null)
        {
            while (!game.MapReady)
            {
                yield return null;
            }
        }
        levelFader.StartTranstion(true);
        
    }
}
