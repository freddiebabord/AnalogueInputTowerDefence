using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
[RequireComponent (typeof(GameManager))]
public class RailManager : MonoBehaviour {
	
	public bool activated = true;
    [HideInInspector][SerializeField]
    public List<Node> railNodes = new List<Node>();
    public List<AIBase> objectToMove = new List<AIBase>();
	public float nodeProximityDistance = 0.1f;
	public RailRotationMode rotationMode;
	public float slerpRotationSpeed = 1.0f;
	public int aliveEnemies = 0;
    public float initialSpeed = 15;
    private WaveManager waveManager;
	private bool shouldClean = false;
	private bool inRoutine = false;
	[Range(3,80)][Tooltip("The amount of times the rail amanger updates the position of entites in the world per second")]
	public int updateIntervalPerSecond = 20;
	private float intervalRate;
	private float timeSinceLastUpdate = 0.0f;

	//--------------------Unity Functions--------------------
	
    void Start()
    {
        waveManager = GetComponent<WaveManager>();
		intervalRate = 1 / updateIntervalPerSecond;
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
		aliveEnemies++;
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
    }

	public void UpdateIntervalTime()
	{
		intervalRate = 1 / updateIntervalPerSecond;
		Debug.Log ("Updating rate");
	}

	void Update()
	{
		if (Input.GetKey (KeyCode.Space))
			UpdateIntervalTime ();

		timeSinceLastUpdate += Time.deltaTime;
		if (timeSinceLastUpdate >= intervalRate) {
			for (int i = 0; i < aliveEnemies; ++i) {
				AIBase aiObj = objectToMove[i];
				if(aiObj == null)
					continue;
				//Exiting if the target node is 
				//outside of the railNodes list.
				if (aiObj.CurrentIndex >= railNodes.Count) {
					Destroy (aiObj.gameObject);
					aliveEnemies--;
					shouldClean = true;
					continue;
				}

				if (aiObj.currentTarget == null) {

					aiObj.DirectionVector = (aiObj.currentNodeTarget - aiObj.transform.position).normalized;

					aiObj.transform.Translate (aiObj.DirectionVector * Time.deltaTime * aiObj.Speed, Space.World);

					Vector3 smudgeFactor = new Vector3 (Random.Range (-2, 2), 0, Random.Range (-2, 2));

					//Rotating the object to face the target node
					//depending on the specified rotation mode.
					switch (rotationMode) {
					case RailRotationMode.Snap:
						aiObj.transform.LookAt (aiObj.currentNodeTarget + smudgeFactor);
						break;

					case RailRotationMode.Slerp:
						Quaternion targetRotation = Quaternion.LookRotation ((aiObj.currentNodeTarget + smudgeFactor) - aiObj.transform.position);
						aiObj.transform.rotation = Quaternion.Slerp (aiObj.transform.rotation, targetRotation, Time.deltaTime * slerpRotationSpeed);
						break;

					default:
						break;
					}

					//Incrementing the target node if the object 
					//has reached the previous target node.
					if (ObjectIsOnNode (aiObj)) {

						aiObj.CurrentIndex++;

						if (aiObj.CurrentIndex >= railNodes.Count) {
							Destroy (aiObj.gameObject);
							shouldClean = true;
							aliveEnemies--;
							continue;
						}

                    
						Vector3 targetPosition = new Vector3 (((Random.insideUnitSphere.x * 2) * nodeProximityDistance),
										                     0 + (aiObj.collider.bounds.extents.magnitude) / 2 + 0.5f, 
										                     ((Random.insideUnitSphere.z * 2) * nodeProximityDistance));

						aiObj.currentNodeTarget = targetPosition + railNodes [aiObj.CurrentIndex].transform.position;
					}
				} else {

					aiObj.DirectionVector = (aiObj.currentTarget.transform.position - aiObj.transform.position).normalized;

					aiObj.transform.Translate (aiObj.DirectionVector * Time.deltaTime * aiObj.Speed, Space.World);

					Quaternion targetRotation = Quaternion.LookRotation (aiObj.currentTarget.transform.position - aiObj.transform.position);
					aiObj.transform.rotation = Quaternion.Slerp (aiObj.transform.rotation, targetRotation, Time.deltaTime * slerpRotationSpeed);
				}
			}

			if (shouldClean && !inRoutine) 
				StartCoroutine (CleanList ());

			timeSinceLastUpdate = 0.0f;

		}
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

    void AddNode(Transform newNode)
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

    public bool BuildNavigationMap(GameObject[,] tiles, int sizeX_, int sizeY_)
    {
        if (railNodes.Count > 0)
            ClearNodes();

        GameObject[] startNodes = GameObject.FindGameObjectsWithTag("EnemyStart");
        foreach (GameObject startNode in startNodes)
        {
            waveManager.AddSpawnPoint(startNode.transform);
        }

        Map map_ = new Map() { map = tiles, sizeX = sizeX_, sizeY = sizeY_ };
        FindNextPoint(map_, startNodes[0]);

        return true;
    }

    private void FindNextPoint(Map map_, GameObject currentPoint)
    {
        NodePath path_ = currentPoint.GetComponent<NodePath>();
        if(currentPoint.gameObject.tag == "EnemyEnd")
            return;

        if (path_.posX + 1 < map_.sizeX)
        {
            if (IsEnemyPathTile(map_.map[path_.posX + 1, path_.posY]))
            {
                AddNode((map_.map[path_.posX + 1, path_.posY]).transform);
                FindNextPoint(map_, (map_.map[path_.posX + 1, path_.posY]));
            }
        }

        if (path_.posX - 1 >= 0)
        {
            if (IsEnemyPathTile(map_.map[path_.posX - 1, path_.posY]))
            {
                AddNode((map_.map[path_.posX - 1, path_.posY]).transform);
                FindNextPoint(map_, (map_.map[path_.posX - 1, path_.posY]));
            }
        }

        if (path_.posY + 1 < map_.sizeY)
        {
            if (IsEnemyPathTile(map_.map[path_.posX, path_.posY + 1]))
            {
                AddNode((map_.map[path_.posX, path_.posY + 1]).transform);
                FindNextPoint(map_, (map_.map[path_.posX, path_.posY + 1]));
            }
        }

        if (path_.posY - 1 >= 0)
        {
            if (IsEnemyPathTile(map_.map[path_.posX, path_.posY - 1]))
            {
                AddNode((map_.map[path_.posX + 1, path_.posY]).transform);
                FindNextPoint(map_, (map_.map[path_.posX, path_.posY - 1]));
            }
        }
    }

    private bool IsEnemyPathTile(GameObject tileToCheck)
    {
        return tileToCheck.GetComponent<NodePath>().pathType == NodePath.PathType.EnemyPath ? true : false;
    }

    #endregion 
}


