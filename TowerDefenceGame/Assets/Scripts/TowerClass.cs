using UnityEngine;
using System.Collections;

public class TowerClass : MonoBehaviour {

	GameObject[] ai;

	public GameObject bullet;

	public GameObject spawnPoint;

	GameObject goal;

	int health = 100;

	int damage = 5;

	float fireRate = 3f;

	public bool upgradable;

	float radius = 12f;
	
	public int level;

	float rotateSpeed = 4f;

	public float lastShot = 0f;

	public float cooldown = 3f;

	bool isFired = false;

	Vector3 direction = new Vector3(0,0,0);

	void Start()
	{
		goal = GameObject.FindGameObjectWithTag("Goal");
	}

	// Update is called once per frame
	void Update () {

	
		if (lastShot > cooldown) 
		{
			isFired = false;
		}

		if (!isFired)
			ai = GameObject.FindGameObjectsWithTag ("Enemy");


		if (HasEnemyInSight (ai) && !isFired) 
		{
			Shooting ();
		}

		lastShot += Time.deltaTime;

	}

	bool HasEnemyInSight(GameObject[] enemies)
	{
		GameObject enemy = GetClosestEnemy (enemies);

		if (Vector3.Distance (gameObject.transform.position, enemy.gameObject.transform.position) < radius) 
		{
			direction = enemy.gameObject.transform.position - gameObject.transform.position;

			Quaternion rotate = Quaternion.LookRotation (direction);
			rotate.x = 0;
			rotate.z = 0;

			gameObject.transform.rotation = Quaternion.Slerp (gameObject.transform.rotation, rotate, Time.deltaTime * rotateSpeed);

			return true;
		}

		return false;
	}

	virtual public void Shooting()
	{
		isFired = true;

		lastShot = 0f;

		Vector3 position = gameObject.transform.position;
		GameObject go = Instantiate (bullet, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
		go.transform.localScale = new Vector3 (1, 1, 1);
		go.GetComponent<Rigidbody> ().velocity = direction * 3;
	}

	GameObject GetClosestEnemy(GameObject[] enemies)
	{
		float close = 10000000;

		int index = 0;

		for (int i = 0; i < enemies.Length; i++) 
		{
			if(enemies[i] != null)
			{
				if (Vector3.Distance (gameObject.transform.position, enemies[i].gameObject.transform.position) < radius)
				{
					if (Vector3.Distance (enemies [i].transform.position, goal.transform.position) < close) 
					{
						close = Vector3.Distance (enemies [i].transform.position, goal.transform.position);
						index = i;
					}
				}
			}
		}

		return enemies [index];
	}
	
}
