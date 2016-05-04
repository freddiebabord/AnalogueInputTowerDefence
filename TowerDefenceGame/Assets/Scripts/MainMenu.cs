using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

    public GameObject menuRoot;



    public void LoadClassicGame()
    {
        GameObject.FindObjectOfType<GameManager>().gameType = GameManager.GameType.Classic;
        StartCoroutine(Load(1));
    }

    public void LoadSurvivalGame()
    {
        GameObject.FindObjectOfType<GameManager>().gameType = GameManager.GameType.Infinite;
        StartCoroutine(Load(2));
    }

    IEnumerator Load(int index)
    {
        menuRoot.SetActive(false);
        AsyncOperation async = Application.LoadLevelAsync(index);
        yield return async;
    }
}
