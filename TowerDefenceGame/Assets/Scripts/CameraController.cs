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
			if (AnalogueInput.GetRightHorizontal() >= 0.1f || AnalogueInput.GetRightHorizontal() <= -0.1f)
				transform.Translate(Vector3.right * (ConfigSettings.Instance.invertXAxis ? AnalogueInput.GetRightHorizontal() : -AnalogueInput.GetRightHorizontal()) * ConfigSettings.Instance.sensitivity * Time.unscaledDeltaTime);
			if (AnalogueInput.GetRightVertical() >= 0.1f || AnalogueInput.GetRightVertical() <= -0.1f)
				transform.Translate(Vector3.Normalize(Vector3.up + Vector3.forward) * (ConfigSettings.Instance.invertYAxis ? AnalogueInput.GetRightVertical() : -AnalogueInput.GetRightVertical()) * ConfigSettings.Instance.sensitivity * Time.unscaledDeltaTime);
        }
    }

    public void TranslatCameraHorisontal(float mag)
    {
        if (shouldBeActive)
			transform.Translate(Vector3.right * (ConfigSettings.Instance.invertXAxis ? mag : -mag) * ConfigSettings.Instance.sensitivity * Time.unscaledDeltaTime);
    }

    public void TranslatCameraVertical(float mag)
    {
        if(shouldBeActive)
			transform.Translate(Vector3.Normalize(Vector3.up + Vector3.forward) * (ConfigSettings.Instance.invertYAxis ? mag : -mag) * ConfigSettings.Instance.sensitivity * Time.unscaledDeltaTime);
    }
    IEnumerator WaitForMapReady()
    {
        if (game != null) {
			while (!game.MapReady) {
				yield return null;
			}

			levelFader.StartTranstion (true);
		}
        
    }

    public void ReturnToMenu()
    {
        if (GameObject.FindObjectOfType<GameManager>())
            Destroy(GameObject.FindObjectOfType<GameManager>().gameObject);
        Application.LoadLevel(0);
    }
}
