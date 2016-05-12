using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Options : MonoBehaviour {

	public Text sensitivityText;
	public Image invertXCheckMark;
	public Image invertYCheckmark;

	void Start()
	{
		sensitivityText.text = ConfigSettings.Instance.sensitivity.ToString ();
	}

	public void IncreaseSensitivity()
	{
		ConfigSettings.Instance.sensitivity++;
		sensitivityText.text = ConfigSettings.Instance.sensitivity.ToString ();
	}

	public void DecreaseSensitivity()
	{
		ConfigSettings.Instance.sensitivity--;
		sensitivityText.text = ConfigSettings.Instance.sensitivity.ToString ();
	}

	public void InvertXAxis()
	{
		ConfigSettings.Instance.invertXAxis = !ConfigSettings.Instance.invertXAxis;
		invertXCheckMark.gameObject.SetActive(ConfigSettings.Instance.invertXAxis);
	}

	public void InvertYAxis()
	{
		ConfigSettings.Instance.invertYAxis = !ConfigSettings.Instance.invertYAxis;
		invertYCheckmark.gameObject.SetActive(ConfigSettings.Instance.invertYAxis);
	}

	public void ClosePanel()
	{
		if (Application.loadedLevel == 0) {
			FindObjectOfType<MainMenu>().showOptions = false;
		}

		gameObject.SetActive (false);
	}
}
