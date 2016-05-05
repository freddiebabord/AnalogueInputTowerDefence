using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

    public GameObject menuRoot;
    public GameManager gameManager;
    public CanvasGroup canvasGroup;
    

    bool classic = false;
    bool showCanvas = false;
    bool hideCanvas = false;

    public Transition eoltransition;
    public Transition difficultyTransition;

    public void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        GameObject.FindObjectOfType<CameraController>().shouldBeActive = false;
    }

    public void LoadClassicGame()
    {
        gameManager.gameType = GameManager.GameType.Classic;
        difficultyTransition.StartTranstion();
    }

    public void LoadSurvivalGame()
    {
        gameManager.gameType = GameManager.GameType.Infinite;
        difficultyTransition.StartTranstion();
    }

    public void HideCanvas()
    {
        difficultyTransition.StartTranstion(true);
    }

    public void Dificulty(int difficulty)
    {
        gameManager.difficulty = difficulty;
        if(gameManager.gameType == GameManager.GameType.Classic)
            StartCoroutine(Load(1));
        else
            StartCoroutine(Load(2));
    }

    IEnumerator Load(int index)
    {
        eoltransition.StartTranstion();
        AsyncOperation async = Application.LoadLevelAsync(index);
        while (!eoltransition.TransitionComplete)
        {
            int i = 0; ++i;
        }
        yield return async;
    }
}
