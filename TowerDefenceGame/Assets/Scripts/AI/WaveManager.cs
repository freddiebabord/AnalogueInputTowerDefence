using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


[Serializable]
public struct MobWave {  
	public enum WaveType {
		Basic = 0,
		Medium,
		Heavy,
		Boss
	}


	public WaveType Type;
	public GameObject Prefab;
	public int Count;
	public float waveEndTimer;
}
[System.Serializable]
public class ProceduralWaveSetUp
{
	public List<GameObject> basic = new List<GameObject> ();
	public List<GameObject> medium = new List<GameObject> ();
	public List<GameObject> heavy = new List<GameObject> ();
	public List<GameObject> boss = new List<GameObject> ();
}

public class WaveManager : MonoBehaviour {

	[HideInInspector]
	public List<MobWave> Waves = new List<MobWave>();
	[SerializeField]
	private List<Transform> spawnPoint = new List<Transform>();
	private RailManager aiNodePathing;
    [SerializeField]
    private bool isSpawning = false;
    [SerializeField]
    private bool waveInterimWait = false;
	//Spawn rate in seconds
	[SerializeField][Range(0.1f, 1.0f)]
	private float enemySpawnRate = 0.5f;
	private int waveToSpawn = 0;
	private int spawnedEnemies = 0;
	public int maxWaves = 0;
	public int currentWave = 0;
    private int currentSpawnPoint = 0;
	public GameManager game;
	private EverloopMasterController everloopMaster;
	private bool hasStarted = false;
	private int difficulty;
	public ProceduralWaveSetUp proceduralWaveSetUp;

    public Text waveNumber;
    private bool godMode = false;

	// Use this for initialization
	void Start () {
		aiNodePathing = GetComponent<RailManager> ();
		foreach (var item in Waves) {
			if(item.waveEndTimer > 0)
				maxWaves++;			
		}

		game = FindObjectOfType<GameManager>();
		game.maxWaves = maxWaves;
		everloopMaster = FindObjectOfType<EverloopMasterController>();

        //if (Waves.Count <= 0)
        //    StartProcedural(20);
        //else
        //    StartClassic(20);
	}

	void Update () {
		if (!hasStarted)
			return;

        if (!game.MapReady)
            return;

        if (Input.GetKey(KeyCode.Space))
            godMode = !godMode;

		if (!isSpawning && !waveInterimWait) 
		{
            if (spawnedEnemies < Waves[waveToSpawn].Count)
            {
                StartCoroutine(SpawnEnemy());
            }
            else
            {
                if (aiNodePathing.aliveEnemies == 0)
                {
                    Debug.Log("Wave Complete");
                    StartCoroutine(WaveComplete());
                }
            }
		}
	}

    void OnGUI()
    {
        if (godMode)
        {
            int w = Screen.width, h = Screen.height;

            GUIStyle style = new GUIStyle();

            Rect rect = new Rect(0, 25, w, h * 2 / 100);
            style.alignment = TextAnchor.UpperLeft;
            style.fontStyle = FontStyle.Bold;
            style.fontSize = h * 2 / 100;
            style.normal.textColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            GUI.Label(rect, "GOD MODE ENABLED", style);
        }
    }

    public void AddSpawnPoint(Transform newSpawnPoint)
    {
        spawnPoint.Add(newSpawnPoint);
    }

	IEnumerator WaveComplete()
	{
		waveInterimWait = true;
		aiNodePathing.ResetEntities ();
		if (Waves [waveToSpawn].waveEndTimer > 0)
			game.maxWaves++;

		yield return new WaitForSeconds (Waves[waveToSpawn].waveEndTimer);
		waveToSpawn++;
        currentWave = waveToSpawn + 1;
        waveNumber.text = "Wave: " + currentWave;

        if (waveToSpawn < Waves.Count)
        {

            if (Waves[waveToSpawn].Type == MobWave.WaveType.Boss)
                everloopMaster.ChangeLoopBasedOnTheme(EverloopTheme.Theme.Tense);
            else
                everloopMaster.ChangeLoopBasedOnTheme(EverloopTheme.Theme.Normal);
            spawnedEnemies = 0;
            for (int i = 0; i < Waves[waveToSpawn].Count; ++i)
            {
                currentSpawnPoint = waveToSpawn > maxWaves ? 0 : UnityEngine.Random.Range(0, spawnPoint.Count);
                GameObject obj = Instantiate(Waves[waveToSpawn].Prefab, spawnPoint[currentSpawnPoint].position, spawnPoint[currentSpawnPoint].rotation) as GameObject;
                obj.GetComponent<AIBase>().Health *= (int)difficulty;
                aiNodePathing.AddEntity(obj);
                obj.SetActive(false);
            }
        }
        else
        {
            StartProcedural(difficulty);
        }
		waveInterimWait = false;
	}


	IEnumerator SpawnEnemy()
	{
        isSpawning = true;
        aiNodePathing.EnableInactiveEntity();
		spawnedEnemies++;
		yield return new WaitForSeconds (enemySpawnRate);
		isSpawning = false;
	}

    public void StartClassic(int difficulty_)
    {
		Debug.Log ("Starting");
		difficulty = difficulty_;
        if (Waves.Count > 0)
        {
			spawnedEnemies = 0;
			for(int i = 0; i < Waves[waveToSpawn].Count; ++i)
			{
				currentSpawnPoint = waveToSpawn > maxWaves ? 0 : UnityEngine.Random.Range(0, spawnPoint.Count);
				GameObject obj = Instantiate(Waves[waveToSpawn].Prefab, spawnPoint[currentSpawnPoint].position, spawnPoint[currentSpawnPoint].rotation) as GameObject;
				if (!godMode)
					obj.GetComponent<AIBase>().Health *= (int)difficulty;
				else
					obj.GetComponent<AIBase>().Health = float.MaxValue;
				
				aiNodePathing.AddEntity(obj);
				obj.SetActive(false);
			}
			hasStarted = true;
        }
        else
            StartProcedural(difficulty_);
    }

    public void StartProcedural(int difficulty_)
    {
        difficulty = difficulty_;
        maxWaves++;
		int waveType = UnityEngine.Random.Range(0, 400);
		MobWave.WaveType rand;
		if (waveType < 200)
			rand = MobWave.WaveType.Basic;
		else if (waveType < 250)
			rand = MobWave.WaveType.Medium;
		else if (waveType < 300)
			rand = MobWave.WaveType.Heavy;
		else
			rand = MobWave.WaveType.Boss;
		Debug.Log ("Starting: " + waveType);
		switch (rand) {
		case(MobWave.WaveType.Basic):
			int basicmaxCount = proceduralWaveSetUp.basic.Count;
            if (basicmaxCount > 0)
            {
                MobWave BasicbobWave = new MobWave()
                {
                    Prefab = proceduralWaveSetUp.basic[UnityEngine.Random.Range(0, basicmaxCount)],
                    Type = MobWave.WaveType.Basic,
                    Count = maxWaves * (25 + UnityEngine.Random.Range(0, 25)),
                    waveEndTimer = UnityEngine.Random.Range(5, 10)
                };
                Waves.Add(BasicbobWave);
            }
            else
                StartProcedural(difficulty_);
			break;
		case(MobWave.WaveType.Medium):
			int MediummaxCount = proceduralWaveSetUp.medium.Count;
            if (MediummaxCount > 0)
            {
                MobWave MediumbobWave = new MobWave()
                {
					Prefab = proceduralWaveSetUp.medium[UnityEngine.Random.Range(0, MediummaxCount)],
                    Type = MobWave.WaveType.Medium,
                    Count = maxWaves * (15 + UnityEngine.Random.Range(0, 15)),
                    waveEndTimer = UnityEngine.Random.Range(5, 10)
                };
                Waves.Add(MediumbobWave);
            }
            else
                StartProcedural(difficulty_);
			break;
		case(MobWave.WaveType.Heavy):
			int HeavymaxCount = proceduralWaveSetUp.heavy.Count;
            if (HeavymaxCount > 0)
            {
                MobWave HeavybobWave = new MobWave()
                {
					Prefab = proceduralWaveSetUp.heavy[UnityEngine.Random.Range(0, HeavymaxCount)],
                    Type = MobWave.WaveType.Heavy,
                    Count = maxWaves * (7 + UnityEngine.Random.Range(0, 7)),
                    waveEndTimer = UnityEngine.Random.Range(5, 10)
                };
                Waves.Add(HeavybobWave);
            }
            else
                StartProcedural(difficulty_);
			break;
		case(MobWave.WaveType.Boss):
			int BossmaxCount = proceduralWaveSetUp.boss.Count;
            if (BossmaxCount > 0)
            {
                MobWave BossbobWave = new MobWave()
                {
					Prefab = proceduralWaveSetUp.boss[UnityEngine.Random.Range(0, BossmaxCount)],
                    Type = MobWave.WaveType.Boss,
                    Count = maxWaves * (1 + UnityEngine.Random.Range(0, 3)),
                    waveEndTimer = UnityEngine.Random.Range(5, 10)
                };
                Waves.Add(BossbobWave);
            }
            else
                StartProcedural(difficulty_);
			break;
		default:
			break;
		}

        currentWave = waveToSpawn + 1;
        waveNumber.text = "Wave: " + currentWave;

        if (Waves[waveToSpawn].Type == MobWave.WaveType.Boss)
            everloopMaster.ChangeLoopBasedOnTheme(EverloopTheme.Theme.Tense);
        else
            everloopMaster.ChangeLoopBasedOnTheme(EverloopTheme.Theme.Normal);

		spawnedEnemies = 0;
		for(int i = 0; i < Waves[waveToSpawn].Count; ++i)
		{
			currentSpawnPoint = waveToSpawn > maxWaves ? 0 : UnityEngine.Random.Range(0, spawnPoint.Count);
			GameObject obj = Instantiate(Waves[waveToSpawn].Prefab, spawnPoint[currentSpawnPoint].position, spawnPoint[currentSpawnPoint].rotation) as GameObject;
            if (!godMode)
                obj.GetComponent<AIBase>().Health *= (int)difficulty;
            else
                obj.GetComponent<AIBase>().Health = float.MaxValue;

			aiNodePathing.AddEntity(obj);
			obj.SetActive(false);
		}

		hasStarted = true;
    }

}

