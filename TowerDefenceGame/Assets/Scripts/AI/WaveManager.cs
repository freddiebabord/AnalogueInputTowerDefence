using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


[Serializable]
public struct MobWave {  
	public enum WaveType {
		Basic,
		Medium,
		Heavy,
		Boss
	}


	public WaveType Type;
	public GameObject Prefab;
	public int Count;
	public float waveEndTimer;
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
	[SerializeField]
	private float enemySpawnRate = 0.5f;
	private int waveToSpawn = 0;
	private int spawnedEnemies = 0;
	public int maxWaves = 0;
	public int currentWave = 0;
    private int currentSpawnPoint;

	// Use this for initialization
	void Start () {
		aiNodePathing = GetComponent<RailManager> ();
	}

	void Update () {
		if (!isSpawning && !waveInterimWait) {
			if(waveToSpawn < Waves.Count)
			{
				if(spawnedEnemies < Waves[waveToSpawn].Count)
				{
					StartCoroutine (SpawnEnemy ());
				}
				else
				{
					if(aiNodePathing.EnemiesThisCycle == 0)
					{
						StartCoroutine(WaveComplete());
					}
				}
			}
		}
	}

	IEnumerator WaveComplete()
	{
		waveInterimWait = true;
		aiNodePathing.ResetEntities ();
		yield return new WaitForSeconds (Waves[waveToSpawn].waveEndTimer);
		waveToSpawn++;
		spawnedEnemies = 0;
		waveInterimWait = false;
	}


	IEnumerator SpawnEnemy()
	{
		isSpawning = true;
        currentSpawnPoint = currentWave > 5 ? 0 : UnityEngine.Random.Range(0, spawnPoint.Count);
        GameObject obj = Instantiate(Waves[waveToSpawn].Prefab, spawnPoint[currentSpawnPoint].position, spawnPoint[currentSpawnPoint].rotation) as GameObject;
		aiNodePathing.AddEntity(obj);
		yield return new WaitForSeconds (enemySpawnRate);
		spawnedEnemies++;
		isSpawning = false;
	}



}

