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
    public ScrollRect panel;

    void Start()
    {
        levelsPath = Application.dataPath + @"/Levels/";
        var info = new DirectoryInfo(levelsPath);
        var fileInfo = info.GetFiles("*.txt");

        foreach (FileInfo file in fileInfo)
        {
            string[] substrings = file.Name.Split('.');
            files.Add(substrings[0]);
        }

        currentLevelText.text = files[0];

        int i = 0;
        foreach (string fileName in files)
        {
            GameObject button = (GameObject)(Instantiate(buttonPrefab, transform.position, transform.rotation));
            button.transform.SetParent(panel.transform, true);

            // TODO: FIX THE POSIITONING!

            button.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
            button.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
            //button.GetComponent<RectTransform>().sizeDelta = new Vector2(-0.1f, 2.1f);
            //button.GetComponent<RectTransform>().anchoredPosition.Set(0.0f, 2 * i);
            button.GetComponent<RectTransform>().localPosition = new Vector3(0, -1 - (i * 2), 0);
            button.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, panel.GetComponent<RectTransform>().sizeDelta.x);
            //button.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, -panel.GetComponent<RectTransform>().sizeDelta.x / 2, panel.GetComponent<RectTransform>().sizeDelta.x/2);
            //button.transform.Translate(button.transform.up * 2 * (i * 2));
            //Vector3 currentPosition = button.transform.position;
            //Debug.Log(currentPosition.y);
           // Debug.Log(currentPosition.y);
            //button.transform.position = currentPosition;
            button.GetComponent<AnalogueButtons>().GetComponentInChildren<Text>().text = fileName;
            button.GetComponent<AnalogueButtons>().onClick.AddListener(() => { SetLevel(button.GetComponentInChildren<Text>().text); });
            
            ++i;
        }

    }

    public void SetLevel(string level)
    {
        currentLevel = level;
        GameManager.Instance.map = level;
        currentLevelText.text = currentLevel;
    }

}
