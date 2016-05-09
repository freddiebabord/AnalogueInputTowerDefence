using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CielaSpike;

public enum RailRotationMode
{
	None, Snap, Slerp
};

[System.Serializable]
public class Node
{

    public Node() { }
    public Node(Transform transform_)
    {
        transform = transform_;
    }

    public Transform transform;
}

[System.Serializable]
public class RailManager : MonoBehaviour {
	
	public bool activated = true;
    [HideInInspector][SerializeField]
    public List<Node> railNodes = new List<Node>();
    private List<AIBase> objectToMove = new List<AIBase>();
	public float nodeProximityDistance = 0.1f;
	public RailRotationMode rotationMode;
	public float slerpRotationSpeed = 1.0f;
    public float initialSpeed = 15;
    private WaveManager waveManager;
	private bool shouldClean = false;
	private bool inRoutine = false;
	[Range(3,80)][Tooltip("The amount of times the rail amanger updates the position of entites in the world per second")]
	public int updateIntervalPerSecond = 20;
	private float intervalRate;
	private float timeSinceLastUpdate = 0.0f;
    
    private float threadedDeltaTime = 0.0f;
    [SerializeField]
    private bool multiThreadWave = false;
    Task task;

    RailSystemThread railThread = new RailSystemThread();
    GameManager gameManager;



    public int aliveEnemies { get { return objectToMove.Count; } }

	//--------------------Unity Functions--------------------

    private bool updateingRail = false;

    void Start()
    {
        waveManager = GetComponent<WaveManager>();
		intervalRate = 1 / updateIntervalPerSecond;
        gameManager = GameObject.FindObjectOfType<GameManager>();
    }

	public void AddEntity(GameObject entity)
	{
        objectToMove.Add(SpawnEntity(entity).GetComponent<AIBase>());
	}

	public void AddEntity(GameObject entity, int index)
	{
        objectToMove.Insert(index, SpawnEntity(entity).GetComponent<AIBase>());
	}

	public void EnableEntityAtIndex(int index)
	{
		objectToMove [index].gameObject.SetActive (true);
	}

    public void EnableInactiveEntity()
    {
        foreach (AIBase entity in objectToMove)
        {
            if (!entity.gameObject.activeInHierarchy)
            {
                entity.gameObject.SetActive(true);
                return;
            }
        }
    }
	
    private GameObject SpawnEntity(GameObject objectToSpawn)
    {
        objectToSpawn.transform.parent = transform;
        if (objectToSpawn.GetComponent<AIBase>().Speed <= 0)
            objectToSpawn.GetComponent<AIBase>().Speed = initialSpeed;


        Vector3 targetPosition = new Vector3(((Random.insideUnitSphere.x * 2) * nodeProximityDistance),
                                                        0 + (objectToSpawn.collider.bounds.extents.magnitude) / 2,
                                                        ((Random.insideUnitSphere.z * 2) * nodeProximityDistance));
        objectToSpawn.GetComponent<AIBase>().currentNodeTarget = targetPosition + railNodes[objectToSpawn.GetComponent<AIBase>().CurrentIndex].transform.position;
        return objectToSpawn;
    }


    public void RemoveEntity(AIBase entity)
    {
        if (objectToMove.Contains(entity))
            objectToMove.Remove(entity);
		shouldClean = true;
        objectToMove.TrimExcess();
    }

	public void UpdateIntervalTime()
	{
		intervalRate = 1 / updateIntervalPerSecond;
	}

	void Update()
	{
        if (!gameManager.MapReady)
            return;

		if (Input.GetKey (KeyCode.Space))
			UpdateIntervalTime ();

        timeSinceLastUpdate += Time.deltaTime;
        if (multiThreadWave)
        {
            threadedDeltaTime += Time.deltaTime;

            if (!updateingRail)
            {
                updateingRail = true;
                this.StartCoroutineAsync(RailCoroutine(), out task);
            }

            if (task.State == TaskState.Error)
            {
                Debug.LogException(task.Exception);
            }
        }
        else
        {

            objectToMove.TrimExcess();
            timeSinceLastUpdate += Time.deltaTime;
            if (timeSinceLastUpdate >= intervalRate)
            {
                for (int i = 0; i < objectToMove.Count; ++i)
                {
                    if (objectToMove[i] == null)
                    {
                        objectToMove.TrimExcess();
                        continue;
                    }
                    if (!objectToMove[i].gameObject.activeInHierarchy)
                        continue;

                    AIBase aiObj = objectToMove[i];
                    //Exiting if the target node is 
                    //outside of the railNodes list.
                    if (aiObj.CurrentIndex >= railNodes.Count)
                    {
                        RemoveEntity(aiObj);
                        Destroy(aiObj.gameObject);
                        objectToMove.TrimExcess();
                        gameManager.AddEnemyInHome();
                        continue;
                    }

                    if (aiObj.currentTarget == null)
                    {

                        aiObj.DirectionVector = (aiObj.currentNodeTarget - aiObj.transform.position).normalized;

                        aiObj.transform.Translate(aiObj.DirectionVector * Time.deltaTime * aiObj.Speed, Space.World);

                        Vector3 smudgeFactor = new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));

                        //Rotating the object to face the target node
                        //depending on the specified rotation mode.
                        switch (rotationMode)
                        {
                            case RailRotationMode.Snap:
                                aiObj.transform.LookAt(aiObj.currentNodeTarget + smudgeFactor);
                                break;

                            case RailRotationMode.Slerp:
                                Quaternion targetRotation = Quaternion.LookRotation((aiObj.currentNodeTarget + smudgeFactor) - aiObj.transform.position);
                                aiObj.transform.rotation = Quaternion.Slerp(aiObj.transform.rotation, targetRotation, Time.deltaTime * slerpRotationSpeed);
                                break;

                            default:
                                break;
                        }

                        //Incrementing the target node if the object 
                        //has reached the previous target node.
                        if (ObjectIsOnNode(aiObj))
                        {

                            aiObj.CurrentIndex++;

                            if (aiObj.CurrentIndex >= railNodes.Count)
                            {
                                RemoveEntity(aiObj);
                                Destroy(aiObj.gameObject);
                                objectToMove.TrimExcess();
                                gameManager.AddEnemyInHome();
                                continue;
                            }

                            Vector3 targetPosition = new Vector3(((Random.insideUnitSphere.x * 2) * nodeProximityDistance),
                                                                 0 + (aiObj.collider.bounds.extents.magnitude) / 2 + 0.5f,
                                                                 ((Random.insideUnitSphere.z * 2) * nodeProximityDistance));

                            
                            aiObj.currentNodeTarget = targetPosition + railNodes[aiObj.CurrentIndex].transform.position;
                        }
                    }
                    else
                    {

                        aiObj.DirectionVector = (aiObj.currentTarget.transform.position - aiObj.transform.position).normalized;

                        aiObj.transform.Translate(aiObj.DirectionVector * Time.deltaTime * aiObj.Speed, Space.World);

                        Quaternion targetRotation = Quaternion.LookRotation(aiObj.currentTarget.transform.position - aiObj.transform.position);
                        aiObj.transform.rotation = Quaternion.Slerp(aiObj.transform.rotation, targetRotation, Time.deltaTime * slerpRotationSpeed);
                    }
                }

                if (shouldClean && !inRoutine)
                    StartCoroutine(CleanList());

                timeSinceLastUpdate = 0.0f;

            }
        }
	}



    IEnumerator RailCoroutine()
    {
        objectToMove.TrimExcess();
        
        if (timeSinceLastUpdate >= intervalRate)
        {
            for (int i = 0; i < objectToMove.Count; ++i)
            {
                if (ReferenceEquals(objectToMove[i],null))
                {
                    //objectToMove.TrimExcess();
                    continue;
                }
                yield return Ninja.JumpToUnity;
                if (!objectToMove[i].gameObject.activeInHierarchy)
                    continue;
                yield return Ninja.JumpBack;
                AIBase aiObj = objectToMove[i];
                //Exiting if the target node is 
                //outside of the railNodes list.
                if (aiObj.CurrentIndex >= railNodes.Count)
                {
                    RemoveEntity(aiObj);
                    Destroy(aiObj.gameObject);
                    objectToMove.TrimExcess();
                    if(gameManager == null)
                        gameManager = GameObject.FindObjectOfType<GameManager>();
                    gameManager.AddEnemyInHome();
                    continue;
                }
                yield return Ninja.JumpToUnity;
                Vector3 aiPos = aiObj.transform.position;
                yield return Ninja.JumpBack;
                if (ReferenceEquals(aiObj.currentTarget, null))
                {

                    aiObj.DirectionVector = (aiObj.currentNodeTarget - aiPos).normalized;

                    yield return Ninja.JumpToUnity;
                    aiObj.transform.Translate(aiObj.DirectionVector * threadedDeltaTime * aiObj.Speed, Space.World);
                    yield return Ninja.JumpBack;

                    Vector3 smudgeFactor = new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));

                    //Rotating the object to face the target node
                    //depending on the specified rotation mode.
                    switch (rotationMode)
                    {
                        case RailRotationMode.Snap:
                            aiObj.transform.LookAt(aiObj.currentNodeTarget + smudgeFactor);
                            break;

                        case RailRotationMode.Slerp:
                            Quaternion targetRotation = Quaternion.LookRotation((aiObj.currentNodeTarget + smudgeFactor) - aiPos);
                            aiObj.transform.rotation = Quaternion.Slerp(aiObj.transform.rotation, targetRotation, threadedDeltaTime * slerpRotationSpeed);
                            break;

                        default:
                            break;
                    }

                    //Incrementing the target node if the object 
                    //has reached the previous target node.
                    if (ObjectIsOnNode(aiObj))
                    {

                        aiObj.CurrentIndex++;

                        Vector3 targetPosition = new Vector3(((Random.insideUnitSphere.x * 2) * nodeProximityDistance),
                                                             0 + (aiObj.collider.bounds.extents.magnitude) / 2 + 0.5f,
                                                             ((Random.insideUnitSphere.z * 2) * nodeProximityDistance));

                        aiObj.currentNodeTarget = targetPosition + railNodes[aiObj.CurrentIndex].transform.position;
                    }
                }
                else
                {
                    yield return Ninja.JumpToUnity;
                    Vector3 ctPos = aiObj.currentNodeTarget;
                    yield return Ninja.JumpBack;

                    aiObj.DirectionVector = (ctPos - aiPos).normalized;
                    Quaternion targetRotation = Quaternion.LookRotation(ctPos - aiPos);

                    yield return Ninja.JumpToUnity;
                    aiObj.transform.Translate(aiObj.DirectionVector * threadedDeltaTime * aiObj.Speed, Space.World);
                    aiObj.transform.rotation = Quaternion.Slerp(aiObj.transform.rotation, targetRotation, threadedDeltaTime * slerpRotationSpeed);
                    yield return Ninja.JumpBack;
                }
            }

            if (shouldClean && !inRoutine)
                objectToMove.TrimExcess();

            timeSinceLastUpdate = 0.0f;

        }
        updateingRail = false;
        threadedDeltaTime = 0.0f;
    }




	IEnumerator CleanList()
	{
		inRoutine = true;
		yield return new WaitForSeconds (2.5f);
		objectToMove.TrimExcess ();
		inRoutine = false;
		shouldClean = false;
	}
	
	void OnDrawGizmos()
	{
		//Drawing the rail nodes and the paths
		//between them as red spheres and lines.
		if(railNodes.Count > 0)
		{
			Gizmos.color = Color.red;
            Vector3 previousNode = railNodes[0].transform.position;
			foreach(var node in railNodes)
			{
                Gizmos.DrawWireSphere(node.transform.position, nodeProximityDistance);
                Gizmos.DrawLine(previousNode, node.transform.position);
                previousNode = node.transform.position;
			}
		}
	}

	void AddNode(Transform newNode, int index)
	{
        railNodes.Insert(index, new Node(newNode));
	}

    public void AddNode(Transform newNode)
    {
        railNodes.Add(new Node(newNode));
    }
	
	//--------------------Private Functions--------------------
	
	bool ObjectIsOnNode(int nodeIndex, int index)
	{
		//Checking if the distance from the object to the target node is less than the proximity distance.
        return (Vector3.Distance(objectToMove[index].transform.position, objectToMove[index].currentNodeTarget) < nodeProximityDistance);
	}

	bool ObjectIsOnNode(AIBase obj)
	{
		//Checking if the distance from the object to the target node is less than the proximity distance.
		return (Vector3.Distance(obj.transform.position, obj.currentNodeTarget) < nodeProximityDistance);
	}

	public void ResetEntities()
	{
        foreach (var item in objectToMove)
        {
            if(item != null)
                Destroy(item.gameObject);
        }
        objectToMove.Clear ();
	}

    private void ClearNodes()
    {
        foreach (var node in railNodes)
        {
            Destroy(node.transform.gameObject);
        }

        railNodes.Clear();
    }

    #region // Dynamic Node Creation

    private struct Map
    {
        public GameObject[,] map;
        public int sizeX;
        public int sizeY;
    }

    public void Addspawn(Transform point)
    {
        if(waveManager == null)
            waveManager = GetComponent<WaveManager>();
        waveManager.AddSpawnPoint(point);
    }

    public bool BuildNavigationMap(GameObject[,] tiles, int sizeX_, int sizeY_)
    {
        if (railNodes.Count > 0)
            ClearNodes();

        GameObject[] startNodes = GameObject.FindGameObjectsWithTag("EnemyStart");
        foreach (GameObject startNode in startNodes)
        {
            Vector3 newPos = new Vector3(startNode.transform.position.x, startNode.transform.position.y + 1, startNode.transform.position.z);
            GameObject spawnNode = new GameObject("AISpawnPoint");
            spawnNode.transform.parent = transform;
            spawnNode.transform.position = newPos;
            spawnNode.transform.Rotate(0, 0, 90);
            Addspawn(spawnNode.transform);
            Instantiate(Resources.Load("Prefabs/Portal"), newPos, spawnNode.transform.rotation);
        }

        Map map_ = new Map() { map = tiles, sizeX = sizeX_, sizeY = sizeY_ };
        FindNextPoint(map_, startNodes[0], startNodes[0]);
        if(gameManager == null)gameManager = GameObject.FindObjectOfType<GameManager>();
        gameManager.MapReady = true;
        return true;
    }

    private void FindNextPoint(Map map_, GameObject currentPoint, GameObject previousNode)
    {
        NodePath path_ = currentPoint.GetComponent<NodePath>();
        if (currentPoint.gameObject.tag == "EnemyEnd")
            return;


        if (path_.posX + 1 < map_.sizeX && previousNode != map_.map[path_.posX + 1, path_.posY].gameObject && IsEnemyPathTile(map_.map[path_.posX + 1, path_.posY]))
        {
            AddNode((map_.map[path_.posX + 1, path_.posY]).transform);
            FindNextPoint(map_, (map_.map[path_.posX + 1, path_.posY]), currentPoint);
        }

        else if (path_.posX - 1 >= 0 && previousNode != map_.map[path_.posX - 1, path_.posY].gameObject && IsEnemyPathTile(map_.map[path_.posX - 1, path_.posY]))
        {
            AddNode((map_.map[path_.posX - 1, path_.posY]).transform);
            FindNextPoint(map_, (map_.map[path_.posX - 1, path_.posY]), currentPoint);

        }

        else if (path_.posY + 1 < map_.sizeY && previousNode != map_.map[path_.posX, path_.posY + 1].gameObject && IsEnemyPathTile(map_.map[path_.posX, path_.posY + 1]))
        {
            AddNode((map_.map[path_.posX, path_.posY + 1]).transform);
            FindNextPoint(map_, (map_.map[path_.posX, path_.posY + 1]), currentPoint);

        }

        else if (path_.posY - 1 >= 0 && previousNode != map_.map[path_.posX, path_.posY - 1].gameObject && IsEnemyPathTile(map_.map[path_.posX, path_.posY - 1]))
        {
            AddNode((map_.map[path_.posX, path_.posY - 1]).transform);
            FindNextPoint(map_, (map_.map[path_.posX, path_.posY - 1]), currentPoint);

        }
    }

    private bool IsEnemyPathTile(GameObject tileToCheck)
    {
        return tileToCheck.GetComponent<NodePath>().pathType == NodePath.PathType.EnemyPath ? true : false;
    }

    #endregion 
}


