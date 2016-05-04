using UnityEngine;
using System.Collections;

public class TowerClass : MonoBehaviour {
	
	protected GameObject[] ai;

	GameObject chosen;

	public GameObject bullet;

	public GameObject spawnPoint;

	GameObject goal;

	int health = 100;

	int damage = 5;

	int levelOfUpgrade = 0;
	
	bool upgradable = false;
	
	[SerializeField]
	float radius = 10f;
	
	int level = 0;

	float rotateSpeed = 4f;

	protected float lastShot = 0f;

	float cooldown = 0f;

	protected bool isFired = false;

	Vector3 direction = new Vector3(0,0,0);

	int classHealth = 0;

	public int speed = 0;

	[SerializeField]
	int exp = 0;
	 
	protected virtual void Start()
	{
		SetGoal(GameObject.FindGameObjectWithTag("Goal"));
		SetTower (0, 1, 0);
		SetCooldown (2);

	}

	// Update is called once per frame
	public virtual void Update () {

        if (exp == 100)
        {
            upgradable = true;
        }

		if (lastShot > cooldown) 
		{
			isFired = false;
		}

		if (!isFired)
			ai = GameObject.FindGameObjectsWithTag ("Enemy");


		if (HasEnemyInSight (ai) && !isFired) 
		{
			if(!upgradable)
			{
				exp += 5;
			}

			Shooting ();
		}

		lastShot += Time.deltaTime;

        if (health <= 0)
            OnDie();
	}

	bool HasEnemyInSight(GameObject[] enemies)
	{
		chosen = GetClosestEnemy (enemies);

		if (chosen != null) 
		{
			if (Vector3.Distance (gameObject.transform.position, chosen.gameObject.transform.position) < radius) 
			{
				direction = chosen.gameObject.transform.position - gameObject.transform.position;

				Quaternion rotate = Quaternion.LookRotation (direction);
				rotate.x = 0;
				rotate.z = 0;

				gameObject.transform.rotation = Quaternion.Slerp (gameObject.transform.rotation, rotate, Time.deltaTime * rotateSpeed);

				return true;
			}
		}

		return false;
	}

	public virtual void Shooting()
	{
		isFired = true;

		lastShot = 0f;

		Vector3 position = gameObject.transform.position;
		GameObject go = Instantiate (bullet, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
		go.transform.localScale = new Vector3 (1, 1, 1);
		go.GetComponent<Rigidbody> ().velocity = direction * speed;

	}

	GameObject GetClosestEnemy(GameObject[] enemies)
	{
		int index = 0;

		for (int i = 0; i < enemies.Length; i++) 
		{
			if(enemies[i] != null)
			{
				if(Vector3.Distance(transform.position, enemies[i].transform.position) < radius)
				{
					index = i;
					break;
				}
			}
		}

		return index < enemies.Length ? enemies[index] : null;
	}

	public GameObject GetChosen()
	{
		return chosen;
	}

	public void SetClassHealth(int hp)
	{
		classHealth = hp;
	}

	public void SetLevel(int level_)
	{
		level += level_;
	}

	public void SetHealth()
	{
		health += level * classHealth;
	}

	public void SetRadius(float radii)
	{
		radius += level * radii;
	}

	public void SetLevelOfUpgrade(int upgrade)
	{
		levelOfUpgrade += upgrade;
	}

	public void SetTower(int hp, int lvl, float rad)
	{
		SetClassHealth (hp);
		SetLevel (lvl);
		SetRadius (rad);
		SetHealth ();
	}

	public void SetGoal(GameObject goal_)
	{
		goal = goal_;
	}

	public int GetClassHealth()
	{
		return classHealth;
	}

	public int GetHealth()
	{
		return health;
	}

	public float GetRadius()
	{
		return radius;
	}

	public void SetCooldown(float cool)
	{
		cooldown = cool;
	}

	public int GetLevel()
	{
		return level;
	}

	public void Upgrade()
	{
		if (levelOfUpgrade >= 100 * level)
			upgradable = true;

		if (upgradable) 
		{
			Debug.Log ("Tower Upgradable");
			SetTower(GetHealth(), GetLevel(), GetRadius());
		}
	}

    public void SetBullet(GameObject shot)
    {
        bullet = shot;
    }

	void OnDrawGizmos()
	{
		Gizmos.color = new Color (1, 1, 1, 0.5f);
		Gizmos.DrawWireSphere (transform.position, radius);
	}

    void ApplyDamage(int HPLoss)
    {
        health -= HPLoss;
    }

    void ApplyDamage(float HPLoss)
    {
        health -= Mathf.RoundToInt(HPLoss);
    }

    protected virtual void OnDie()
    {
        Destroy(transform.parent.gameObject);
    }

}
