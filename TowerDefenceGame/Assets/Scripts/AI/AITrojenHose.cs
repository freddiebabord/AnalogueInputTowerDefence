using UnityEngine;
using System.Collections;

public class AITrojenHose : AIBase {

    public GameObject objectToSpawn;
    public float spawnRate = 2.5f;
    public int maxSpawnCount = 20;
    private bool isSpawning = false;
    private int currentSpawnCount = 0;
    private RailManager aiNodePathing;
    public bool spawnOnDeath = false;
    public Transform spawnPoint;

    public override void Start()
    {
        aiNodePathing = GameObject.FindObjectOfType<RailManager>();
        base.Start();
    }

	// Update is called once per frame
	public override void Update () {
        if (!isSpawning && currentSpawnCount < maxSpawnCount && !spawnOnDeath)
            StartCoroutine(Spawn());

        base.Update();
	}

    IEnumerator Spawn()
    {
        isSpawning = true;
        GameObject child = (GameObject)Instantiate(objectToSpawn, spawnPoint.position, spawnPoint.rotation);
        child.GetComponent<AIBase>().CurrentIndex = currentIndex;
        aiNodePathing.AddEntity(child);
        currentSpawnCount++;
        yield return new WaitForSeconds(spawnRate);
        isSpawning = false;
    }

	public override void Die(float delay = 0)
    {
        if (spawnOnDeath)
        {
            for (int i = 0; i < maxSpawnCount; i++)
            {
                Vector3 spawnPosition = new Vector3((Random.insideUnitSphere.x * collider.bounds.extents.magnitude)/2,
                 transform.position.y + 2, (Random.insideUnitSphere.z * collider.bounds.extents.magnitude)/2);
                GameObject child = (GameObject)Instantiate(objectToSpawn, transform.position + spawnPosition, Quaternion.Euler(new Vector3(0, 0, 0)));
                child.GetComponent<AIBase>().CurrentIndex = currentIndex;
                aiNodePathing.AddEntity(child);
            }
        }
		base.Die();
    }

    protected override void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(transform.position, collider.bounds.extents.magnitude);
        base.OnDrawGizmos();
    }
}
