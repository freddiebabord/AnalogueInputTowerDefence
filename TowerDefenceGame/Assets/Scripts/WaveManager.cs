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
}

public class WaveManager : MonoBehaviour {

	[HideInInspector]
	public List<MobWave> Waves = new List<MobWave>();
	[SerializeField]
	private Transform spawnPoint;
	private RailManager aiNodePathing;
	private bool isSpawning = false;
	private bool waveInterimWait = false;
	//Spawn rate in seconds
	[SerializeField]
	private float enemySpawnRate = 0.5f;
	[SerializeField]
	private float waveSleepTimer = 5;
	private int waveToSpawn = 0;
	private int spawnedEnemies = 0;

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
						spawnedEnemies = 0;
						StartCoroutine(WaveComplete());
					}
				}
			}
		}
	}

	IEnumerator WaveComplete()
	{
		waveInterimWait = true;
		aiNodePathing.objectToMove.Clear ();
		yield return new WaitForSeconds (waveSleepTimer);
		waveToSpawn++;
		waveInterimWait = false;
	}


	IEnumerator SpawnEnemy()
	{
		isSpawning = true;
		GameObject obj = Instantiate (Waves [waveToSpawn].Prefab, spawnPoint.position, spawnPoint.rotation) as GameObject;
		obj.tag = "Enemy";
		aiNodePathing.AddEntity(obj, spawnedEnemies);
		yield return new WaitForSeconds (enemySpawnRate);
		spawnedEnemies++;
		isSpawning = false;
	}



}

