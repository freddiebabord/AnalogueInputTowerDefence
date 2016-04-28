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
	private List<int> targetNodeIndex = new List<int>();
	public float nodeProximityDistance = 0.1f;
	public RailRotationMode rotationMode;
	public float slerpRotationSpeed = 1.0f;
	public int EnemiesThisCycle = 0;
    public float initialSpeed = 15;

	//--------------------Unity Functions--------------------
	
	void Start()
	{
		//Moving the object to the first node.
		int counter = 0;
		for(int i = 0; i < objectToMove.Count; ++i){
			targetNodeIndex.Add(0);
			counter++;
		}
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

        if (objectToSpawn.GetComponent<AIBase>().CurrentIndex > 0)
            targetNodeIndex.Add(objectToSpawn.GetComponent<AIBase>().CurrentIndex);
        else
            targetNodeIndex.Add(0);

        Vector3 targetPosition = new Vector3((Random.insideUnitSphere.x * nodeProximityDistance),
                                                        0 + (objectToSpawn.collider.bounds.extents.magnitude) / 2,
                                                        (Random.insideUnitSphere.z * nodeProximityDistance));
        objectToSpawn.GetComponent<AIBase>().currentNodeTarget = targetPosition + railNodes[targetNodeIndex[0]].transform.position;

        return objectToSpawn;
    }

	void Update()
	{
		if(!activated) return;

		EnemiesThisCycle = 0;

		for (int i = 0; i < objectToMove.Count; i++) {

			if(objectToMove[i] == null)
				continue;

			//Exiting if the target node is 
			//outside of the railNodes list.
			if (targetNodeIndex[i] >= railNodes.Count) 
			{
				Destroy(objectToMove[i].gameObject);
				continue;
			}
			EnemiesThisCycle++;

            if (objectToMove[i].currentTarget == null)
            {
                //Moving the object towards the target node.
                if (objectToMove[i].currentNodeTarget == null)
                {
                    Vector3 targetPosition = new Vector3((Random.insideUnitSphere.x * nodeProximityDistance),
                                                        0 + (objectToMove[i].collider.bounds.extents.magnitude)/2, 
                                                        (Random.insideUnitSphere.z * nodeProximityDistance));
                    objectToMove[i].currentNodeTarget = targetPosition + railNodes[targetNodeIndex[i]].transform.position;

                }
                objectToMove[i].DirectionVector = (objectToMove[i].currentNodeTarget - objectToMove[i].transform.position ).normalized;
                if (objectToMove[i].GetComponent<Rigidbody>() != null)
                    objectToMove[i].GetComponent<Rigidbody>().velocity = objectToMove[i].DirectionVector * objectToMove[i].Speed * Time.deltaTime;
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
                if (ObjectIsOnNode(targetNodeIndex[i], i))
                {
                    //objectToMove[i].position = railNodes [targetNodeIndex[i]].position;
                    targetNodeIndex[i]++;
                    objectToMove[i].CurrentIndex = targetNodeIndex[i];
                    
                    Vector3 targetPosition = new Vector3((Random.insideUnitSphere.x * nodeProximityDistance),
                     0 + (objectToMove[i].collider.bounds.extents.magnitude) / 2, 
                     (Random.insideUnitSphere.z * nodeProximityDistance));
                    objectToMove[i].currentNodeTarget = targetPosition + railNodes[targetNodeIndex[i]].transform.position;
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
	
	//--------------------Private Functions--------------------
	
	bool ObjectIsOnNode(int nodeIndex, int index)
	{
		//Checking if the distance from the object to the target node is less than the proximity distance.
        return (Vector3.Distance(objectToMove[index].transform.position, objectToMove[index].currentNodeTarget) < nodeProximityDistance);
	}

	public void ResetEntities()
	{
		objectToMove.Clear ();
		targetNodeIndex.Clear ();
	}
}
