using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

    public GameObject menuRoot;
    public GameManager gameManager;
    public CanvasGroup canvasGroup;

	private bool showDifficultyPanel = false;
	public bool ShowingDifficultyPanel{ get { return showDifficultyPanel; } }

    public Transition eoltransition;
    public Transition difficultyTransition;

    public void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        GameObject.FindObjectOfType<CameraController>().shouldBeActive = false;
    }

	void Update()
	{
		if (AnalogueInput.GetLeftTrigger () >= 1) 
		{
			if(ShowingDifficultyPanel)
				HideCanvas();
		}
	}

    public void LoadClassicGame()
    {
        gameManager.gameType = GameManager.GameType.Classic;
        difficultyTransition.StartTranstion();
		showDifficultyPanel = true;
		GetComponent<Levels> ().DisableButtons ();
    }

    public void LoadSurvivalGame()
    {
        gameManager.gameType = GameManager.GameType.Infinite;
        difficultyTransition.StartTranstion();
		showDifficultyPanel = true;
		GetComponent<Levels> ().DisableButtons ();
    }

    public void HideCanvas()
    {
        difficultyTransition.StartTranstion(true);
		showDifficultyPanel = false;
		GetComponent<Levels> ().EnableButtons ();
    }

    public void Dificulty(int difficulty)
    {
        gameManager.difficulty = difficulty;
        if(gameManager.gameType == GameManager.GameType.Classic)
            StartCoroutine(Load(1));
        else
            StartCoroutine(Load(2));
    }

	public void MapMaker()
	{
		Application.LoadLevelAsync("MapMaker");
	}

    IEnumerator Load(int index)
    {
        AsyncOperation async = Application.LoadLevelAsync(index);
        yield return async;
        if (!eoltransition.gameObject.activeInHierarchy)
            eoltransition.gameObject.SetActive(true);
        if (!eoltransition.enabled)
            eoltransition.enabled = true;
        eoltransition.StartTranstion();
        yield return new WaitForSeconds(2.5f);
        while (!eoltransition.TransitionComplete)
        {
            int i = 0; ++i;
        }
    }
}
