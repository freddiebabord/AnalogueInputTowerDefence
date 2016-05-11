using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Levels : MonoBehaviour {


    private string levelsPath = "";
    private string  currentLevel = "";

    List<string> files = new List<string>();
    public GameObject buttonPrefab;
    public Text currentLevelText;
    public GameObject panel;
	List<GameObject> buttons = new List<GameObject> ();
	public List<GameObject> mainButtons = new List<GameObject> ();

	MainMenu mainMenu;
    void Start()
    {
        levelsPath = Application.dataPath + @"/Levels/";
        var info = new DirectoryInfo(levelsPath);
        FileInfo[] fileInfo = info.GetFiles("*.txt");

        foreach (FileInfo file in fileInfo)
        {
            string[] substrings = file.Name.Split('.');
            files.Add(substrings[0]);
        }

        currentLevelText.text = files[0];
        SetLevel(files[0]);
        panel.GetComponent<RectTransform>().sizeDelta = new Vector2(panel.GetComponent<RectTransform>().sizeDelta.x, files.Count * 2);
        int i = 0;
        foreach (string fileName in files)
        {
            GameObject button = (GameObject)(Instantiate(buttonPrefab, transform.position, transform.rotation));
            button.transform.SetParent(panel.transform, false);
            button.transform.localPosition = new Vector3(0, 2+(files.Count/2) - i * 2, 0);
            button.transform.localRotation = Quaternion.identity;
            button.GetComponent<AnalogueButtons>().GetComponentInChildren<Text>().text = fileName;
            button.GetComponent<AnalogueButtons>().onClick.AddListener(() => { SetLevel(button.GetComponentInChildren<Text>().text); });
			buttons.Add(button);
			++i;
        }

		mainMenu = GetComponent<MainMenu> ();

    }

    void Update()
    {
        if (!mainMenu.ShowingDifficultyPanel) 
		{
			if (Input.GetAxis ("VerticalRight") > 0 && panel.transform.localPosition.y < files.Count)
				panel.transform.Translate (Vector3.up * 5.0f * Time.deltaTime, Space.Self);
			else if (Input.GetAxis ("VerticalRight") < 0 && panel.transform.localPosition.y > -files.Count)
				panel.transform.Translate (Vector3.up * -5.0f * Time.deltaTime, Space.Self);
		}
    }

    public void SetLevel(string level)
    {
        currentLevel = level;
        GameManager.Instance.map = level;
        currentLevelText.text = currentLevel;
    }

	public void DisableButtons()
	{
		foreach (var button in buttons) {
			button.GetComponent<AnalogueButtons>().interactable = false;
		}
		foreach (var button in mainButtons) {
			button.GetComponent<AnalogueButtons>().interactable = false;
		}
	}

	public void EnableButtons()
	{
		foreach (var button in buttons) {
			button.GetComponent<AnalogueButtons>().interactable = true;
		}
		foreach (var button in mainButtons) {
			button.GetComponent<AnalogueButtons>().interactable = true;
		}
	}

}
