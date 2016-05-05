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
	public int EnemiesThisCycle = 0;
    public float initialSpeed = 15;
    private WaveManager waveManager;

	//--------------------Unity Functions--------------------
	
    void Start()
    {
        waveManager = GetComponent<WaveManager>();
    }

	public void AddEntity(GameObject entity)
	{
        objectToMove.Add(SpawnEntity(entity).GetComponent<AIBase>());
	}

	public void AddEntity(GameObject entity, int index)
	{
        objectToMove.Insert(index, SpawnEntity(entity).GetComponent<AIBase>());
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

	void Update()
	{
		if(!activated) return;

		EnemiesThisCycle = 0;

		for (int i = 0; i < objectToMove.Count; ++i) {

			if(objectToMove[i] == null)
				continue;

            
			//Exiting if the target node is 
			//outside of the railNodes list.
            if (objectToMove[i].CurrentIndex >= railNodes.Count) 
			{
				Destroy(objectToMove[i].gameObject);
				continue;
			}
			EnemiesThisCycle++;

            if (objectToMove[i].currentTarget == null)
            {

                objectToMove[i].DirectionVector = (objectToMove[i].currentNodeTarget - objectToMove[i].transform.position ).normalized;
                /*if (objectToMove[i].GetComponent<Rigidbody>() != null)
                    objectToMove[i].GetComponent<Rigidbody>().velocity = objectToMove[i].DirectionVector * objectToMove[i].Speed * Time.deltaTime;*/
                
                objectToMove[i].transform.Translate(objectToMove[i].DirectionVector * Time.deltaTime * objectToMove[i].Speed, Space.World);

                Vector3 smudgeFactor = new Vector3(Random.Range(-2, 2), 0, Random.Range(-2, 2));

                //Rotating the object to face the target node
                //depending on the specified rotation mode.
                switch (rotationMode)
                {
                    case RailRotationMode.Snap:
                        objectToMove[i].transform.LookAt(objectToMove[i].currentNodeTarget + smudgeFactor);
                        break;

                    case RailRotationMode.Slerp:
                        Quaternion targetRotation = Quaternion.LookRotation((objectToMove[i].currentNodeTarget + smudgeFactor) - objectToMove[i].transform.position);
                        objectToMove[i].transform.rotation = Quaternion.Slerp(objectToMove[i].transform.rotation, targetRotation, Time.deltaTime * slerpRotationSpeed);
                        break;

                    default:
                        break;
                }

                //Incrementing the target node if the object 
                //has reached the previous target node.
                if (ObjectIsOnNode(objectToMove[i].CurrentIndex, i))
                {
                    //objectToMove[i].position = railNodes [targetNodeIndex[i]].position;
                    objectToMove[i].CurrentIndex++;

                    if (objectToMove[i].CurrentIndex >= railNodes.Count)
                    {
                        Destroy(objectToMove[i].gameObject);
                        continue;
                    }

                    
                    Vector3 targetPosition = new Vector3(((Random.insideUnitSphere.x * 2) * nodeProximityDistance),
                     0 + (objectToMove[i].collider.bounds.extents.magnitude) / 2, 
                     ((Random.insideUnitSphere.z * 2) * nodeProximityDistance));

                    objectToMove[i].currentNodeTarget = targetPosition + railNodes[objectToMove[i].CurrentIndex].transform.position;
                }
            }
            else
            {

                objectToMove[i].DirectionVector = (objectToMove[i].currentTarget.transform.position - objectToMove[i].transform.position).normalized;
                if (objectToMove[i].GetComponent<Rigidbody>() != null)
                    objectToMove[i].GetComponent<Rigidbody>().velocity = objectToMove[i].DirectionVector * objectToMove[i].Speed * Time.deltaTime;
                objectToMove[i].transform.Translate(objectToMove[i].DirectionVector * Time.deltaTime * objectToMove[i].Speed, Space.World);

                Quaternion targetRotation = Quaternion.LookRotation(objectToMove[i].currentTarget.transform.position - objectToMove[i].transform.position);
                objectToMove[i].transform.rotation = Quaternion.Slerp(objectToMove[i].transform.rotation, targetRotation, Time.deltaTime * slerpRotationSpeed);
            }
		}


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

    public bool BuildNavigationMap(GameObject[] tiles, int sizeX_, int sizeY_)
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


