using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

	[SerializeField] private int enemiesPassed = 0;
	[SerializeField] private float goldQuantity = 0;

    [SerializeField] public GameType gameType;
    [SerializeField][Range(1,50)] public int difficulty;

    private RailManager railSystem;
    private WaveManager waveSystem;

	public Text goldQuantityText;
	public Slider healthSlider;
	public Text waveNumber;

	public int currentWave = 0;
	public int maxWaves = 0;

    public enum GameType
    {
        Classic = 0,
        Infinite
    }

    void Start()
    {
        DontDestroyOnLoad(this);
    }

	public void AddEnemyInHome()
	{
		enemiesPassed++;
	}

	public void AddGold(float amount)
	{
		goldQuantity += amount;
	}

    void OnLevelWasLoaded(int levelID)
    {
        if(levelID != 0)
        {
            StartCoroutine(WaitToStart());
        }
    }

    IEnumerator WaitToStart()
    {
        yield return new WaitForSeconds(5);
        waveSystem = GameObject.FindObjectOfType<WaveManager>();
        if (waveSystem)
            Debug.Log("Starting");
        switch (gameType)
        {
            case GameType.Classic:
                if (waveSystem)
                    waveSystem.StartClassic(1);
                break;
            case GameType.Infinite:
                if (waveSystem)
                    waveSystem.StartProcedural(1);
                break;
            default:
                print("Error: Wave type not defined / wave type has no logic.");
                break;
        }
    }

}
