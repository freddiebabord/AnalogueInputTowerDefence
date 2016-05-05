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
	AsyncOperation level;

    public void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        GameObject.FindObjectOfType<CameraController>().shouldBeActive = false;
    }

    public void LoadClassicGame()
    {
        gameManager.gameType = GameManager.GameType.Classic;
		StartCoroutine (ToggleDifficultyPanel ());
    }

    public void LoadSurvivalGame()
    {
        gameManager.gameType = GameManager.GameType.Infinite;
		StartCoroutine (ToggleDifficultyPanel ());
    }

    public void HideCanvas()
    {
		StartCoroutine (ToggleDifficultyPanel (true));
    }

    public void Dificulty(int difficulty)
    {
        gameManager.difficulty = difficulty;
        if(gameManager.gameType == GameManager.GameType.Classic)
            StartCoroutine(Load(1));
        else
            StartCoroutine(Load(2));
    }

	IEnumerator ToggleDifficultyPanel(bool reverse = false)
	{
		difficultyTransition.StartTranstion(reverse);

		while(!difficultyTransition.TransitionComplete)
			yield return StartCoroutine (difficultyTransition.Wait ());

		difficultyTransition.transitionCanvas.interactable = !difficultyTransition.transitionCanvas.interactable;
		difficultyTransition.transitionCanvas.blocksRaycasts = !difficultyTransition.transitionCanvas.blocksRaycasts;
	}


    IEnumerator Load(int index)
    {
        eoltransition.StartTranstion();
		level = Application.LoadLevelAsync(index);


		while(!level.isDone)
			yield return StartCoroutine (eoltransition.Wait ());

		yield return new WaitForSeconds (3);

        yield return level;
    }
}
