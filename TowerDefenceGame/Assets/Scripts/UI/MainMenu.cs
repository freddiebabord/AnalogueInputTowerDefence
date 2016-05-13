using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

    public GameObject menuRoot;
    public GameManager gameManager;
    public CanvasGroup canvasGroup;
	public GameObject creditsPanel;

	private bool showDifficultyPanel = false;
    private bool showingCredits = false;
	private bool showingOptions = false;
	public bool showOptions{ set { showingOptions = value; } }
	public bool ShowingDifficultyPanel{ get { return showDifficultyPanel; } }

    public Transition eoltransition;
    public Transition difficultyTransition;
	public GameObject optionsPanel;

    public void Start()
    {
		if (!Application.genuine)
			Application.CommitSuicide (1);

        gameManager = GameObject.FindObjectOfType<GameManager>();
        GameObject.FindObjectOfType<CameraController>().shouldBeActive = false;
    }

	void Update()
	{
		if (AnalogueInput.GetLeftTrigger () >= 1) 
		{
			if(ShowingDifficultyPanel)
				HideCanvas();
            if (showingCredits)
                HideCredits();
			if(showingOptions)
				HideOptions();
		}
	}

    public void LoadClassicGame()
    {
		GetComponent<Levels> ().DisableButtons ();
        gameManager.gameType = GameManager.GameType.Classic;
        difficultyTransition.StartTranstion();
		showDifficultyPanel = true;
    }

    public void LoadSurvivalGame()
    {
		GetComponent<Levels> ().DisableButtons ();
        gameManager.gameType = GameManager.GameType.Infinite;
        difficultyTransition.StartTranstion();
		showDifficultyPanel = true;
    }

    public void HideCanvas()
    {
        difficultyTransition.StartTranstion(true);
		showDifficultyPanel = false;
		GetComponent<Levels> ().EnableButtons ();
    }

	public void ShowCredits()
	{
		creditsPanel.SetActive (true);
		GetComponent<Levels> ().DisableButtons ();
        showingCredits = true;
	}

	public void HideCredits()
	{
		creditsPanel.SetActive (false);
		GetComponent<Levels> ().EnableButtons ();
        showingCredits = false;
	}

	public void ShowOptions()
	{
		optionsPanel.SetActive (true);
		GetComponent<Levels> ().DisableButtons ();
		showingOptions = true;
	}
	
	public void HideOptions()
	{
		optionsPanel.SetActive (false);
		GetComponent<Levels> ().EnableButtons ();
		showingOptions = false;
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

	public void Quit()
	{
		Application.Quit ();
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
