using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


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
	private bool isSpawning = false;
	private bool waveInterimWait = false;
	//Spawn rate in seconds
	[SerializeField][Range(0.1f, 1.0f)]
	private float enemySpawnRate = 0.5f;
	private int waveToSpawn = 0;
	private int spawnedEnemies = 0;
	public int maxWaves = 0;
	public int currentWave = 0;
    private int currentSpawnPoint = 0;
	private GameManager game;
	private EverloopMasterController everloopMaster;
	private bool hasStarted = false;
	private GameManager.Difficulty difficulty;
	public ProceduralWaveSetUp proceduralWaveSetUp;

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

		StartProcedural (GameManager.Difficulty.Medium);
	}

	void Update () {
		if (!hasStarted)
			return;

		if (!isSpawning && !waveInterimWait) 
		{
			if(waveToSpawn < Waves.Count)
			{
				if(spawnedEnemies < Waves[waveToSpawn].Count)
				{
					StartCoroutine (SpawnEnemy ());
				}
				else
				{
					if(aiNodePathing.aliveEnemies == 0)
					{
						StartCoroutine(WaveComplete());
					}
				}
			}
			else
			{
				maxWaves++;
				MobWave.WaveType rand = (MobWave.WaveType)UnityEngine.Random.Range(0, 4);
				switch (rand) {
				case(MobWave.WaveType.Basic):
					int basicmaxCount = proceduralWaveSetUp.basic.Count;
					MobWave BasicbobWave = new MobWave(){
						Prefab = proceduralWaveSetUp.basic[UnityEngine.Random.Range(0,basicmaxCount)],
						Type = MobWave.WaveType.Basic,
						Count = maxWaves * (25 + UnityEngine.Random.Range(0, 25)),
						waveEndTimer = UnityEngine.Random.Range(5, 10)
					};
					Waves.Add(BasicbobWave);
					break;
				case(MobWave.WaveType.Medium):
					int MediummaxCount = proceduralWaveSetUp.basic.Count;
					MobWave MediumbobWave = new MobWave(){
						Prefab = proceduralWaveSetUp.basic[UnityEngine.Random.Range(0,MediummaxCount)],
						Type = MobWave.WaveType.Medium,
						Count = maxWaves * (15 + UnityEngine.Random.Range(0, 15)),
						waveEndTimer = UnityEngine.Random.Range(5, 10)
					};
					Waves.Add(MediumbobWave);
					break;
				case(MobWave.WaveType.Heavy):
					int HeavymaxCount = proceduralWaveSetUp.basic.Count;
					MobWave HeavybobWave = new MobWave(){
						Prefab = proceduralWaveSetUp.basic[UnityEngine.Random.Range(0,HeavymaxCount)],
						Type = MobWave.WaveType.Heavy,
						Count = maxWaves * (7 + UnityEngine.Random.Range(0, 7)),
						waveEndTimer = UnityEngine.Random.Range(5, 10)
					};
					Waves.Add(HeavybobWave);
					break;
				case(MobWave.WaveType.Boss):
					int BossmaxCount = proceduralWaveSetUp.basic.Count;
					MobWave BossbobWave = new MobWave(){
						Prefab = proceduralWaveSetUp.basic[UnityEngine.Random.Range(0,BossmaxCount)],
						Type = MobWave.WaveType.Boss,
						Count = maxWaves * (1 + UnityEngine.Random.Range(0, 3)),
						waveEndTimer = UnityEngine.Random.Range(5, 10)
					};
					Waves.Add(BossbobWave);
					break;
				default:
				break;
				}
				if(Waves[waveToSpawn].Type == MobWave.WaveType.Boss)
					everloopMaster.ChangeLoopBasedOnTheme(EverloopTheme.Theme.Tense);
				else
					everloopMaster.ChangeLoopBasedOnTheme(EverloopTheme.Theme.Normal);
				spawnedEnemies = 0;
				for(int i = 0; i < Waves[waveToSpawn].Count; ++i)
				{
					currentSpawnPoint = waveToSpawn > maxWaves ? 0 : UnityEngine.Random.Range(0, spawnPoint.Count);
					GameObject obj = Instantiate(Waves[waveToSpawn].Prefab, spawnPoint[currentSpawnPoint].position, spawnPoint[currentSpawnPoint].rotation) as GameObject;
					obj.GetComponent<AIBase>().Health *= (int)difficulty;
					aiNodePathing.AddEntity(obj);
					obj.SetActive(false);
				}

			}
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
        if(Waves[waveToSpawn].Type == MobWave.WaveType.Boss)
			everloopMaster.ChangeLoopBasedOnTheme(EverloopTheme.Theme.Tense);
        else
			everloopMaster.ChangeLoopBasedOnTheme(EverloopTheme.Theme.Normal);
		spawnedEnemies = 0;
		for(int i = 0; i < Waves[waveToSpawn].Count; ++i)
		{
			currentSpawnPoint = waveToSpawn > maxWaves ? 0 : UnityEngine.Random.Range(0, spawnPoint.Count);
			GameObject obj = Instantiate(Waves[waveToSpawn].Prefab, spawnPoint[currentSpawnPoint].position, spawnPoint[currentSpawnPoint].rotation) as GameObject;
			obj.GetComponent<AIBase>().Health *= (int)difficulty;
			aiNodePathing.AddEntity(obj);
			obj.SetActive(false);
		}

		waveInterimWait = false;
	}


	IEnumerator SpawnEnemy()
	{
        isSpawning = true;
		aiNodePathing.EnableEntityAtIndex (aiNodePathing.aliveEnemies);
		spawnedEnemies++;
		yield return new WaitForSeconds (enemySpawnRate);
		isSpawning = false;
	}

    public void StartClassic(GameManager.Difficulty difficulty_)
    {
		difficulty = difficulty_;
		for(int i = 0; i < Waves[waveToSpawn].Count; ++i)
		{
			currentSpawnPoint = waveToSpawn > maxWaves ? 0 : UnityEngine.Random.Range(0, spawnPoint.Count);
			GameObject obj = Instantiate(Waves[waveToSpawn].Prefab, spawnPoint[currentSpawnPoint].position, spawnPoint[currentSpawnPoint].rotation) as GameObject;
			obj.GetComponent<AIBase>().Health *= (int)difficulty_;
			aiNodePathing.AddEntity(obj);
			obj.SetActive(false);
		}
		Debug.Log ("Starting classic waves");
		hasStarted = true;
    }

    public void StartProcedural(GameManager.Difficulty difficulty_)
    {
		maxWaves++;
		MobWave.WaveType rand = (MobWave.WaveType)UnityEngine.Random.Range(0, 4);
		switch (rand) {
		case(MobWave.WaveType.Basic):
			int basicmaxCount = proceduralWaveSetUp.basic.Count;
			MobWave BasicbobWave = new MobWave(){
				Prefab = proceduralWaveSetUp.basic[UnityEngine.Random.Range(0,basicmaxCount)],
				Type = MobWave.WaveType.Basic,
				Count = maxWaves * (25 + UnityEngine.Random.Range(0, 25)),
				waveEndTimer = UnityEngine.Random.Range(5, 10)
			};
			Waves.Add(BasicbobWave);
			break;
		case(MobWave.WaveType.Medium):
			int MediummaxCount = proceduralWaveSetUp.basic.Count;
			MobWave MediumbobWave = new MobWave(){
				Prefab = proceduralWaveSetUp.basic[UnityEngine.Random.Range(0,MediummaxCount)],
				Type = MobWave.WaveType.Medium,
				Count = maxWaves * (15 + UnityEngine.Random.Range(0, 15)),
				waveEndTimer = UnityEngine.Random.Range(5, 10)
			};
			Waves.Add(MediumbobWave);
			break;
		case(MobWave.WaveType.Heavy):
			int HeavymaxCount = proceduralWaveSetUp.basic.Count;
			MobWave HeavybobWave = new MobWave(){
				Prefab = proceduralWaveSetUp.basic[UnityEngine.Random.Range(0,HeavymaxCount)],
				Type = MobWave.WaveType.Heavy,
				Count = maxWaves * (7 + UnityEngine.Random.Range(0, 7)),
				waveEndTimer = UnityEngine.Random.Range(5, 10)
			};
			Waves.Add(HeavybobWave);
			break;
		case(MobWave.WaveType.Boss):
			int BossmaxCount = proceduralWaveSetUp.basic.Count;
			MobWave BossbobWave = new MobWave(){
				Prefab = proceduralWaveSetUp.basic[UnityEngine.Random.Range(0,BossmaxCount)],
				Type = MobWave.WaveType.Boss,
				Count = maxWaves * (1 + UnityEngine.Random.Range(0, 3)),
				waveEndTimer = UnityEngine.Random.Range(5, 10)
			};
			Waves.Add(BossbobWave);
			break;
		default:
			break;
		}
		Debug.Log ("Starting procedural waves");

		if(Waves[waveToSpawn].Type == MobWave.WaveType.Boss)
			everloopMaster.ChangeLoopBasedOnTheme(EverloopTheme.Theme.Tense);
		else
			everloopMaster.ChangeLoopBasedOnTheme(EverloopTheme.Theme.Normal);
		spawnedEnemies = 0;
		for(int i = 0; i < Waves[waveToSpawn].Count; ++i)
		{
			currentSpawnPoint = waveToSpawn > maxWaves ? 0 : UnityEngine.Random.Range(0, spawnPoint.Count);
			GameObject obj = Instantiate(Waves[waveToSpawn].Prefab, spawnPoint[currentSpawnPoint].position, spawnPoint[currentSpawnPoint].rotation) as GameObject;
			obj.GetComponent<AIBase>().Health *= (int)difficulty;
			aiNodePathing.AddEntity(obj);
			obj.SetActive(false);
		}

		hasStarted = true;
    }

}

