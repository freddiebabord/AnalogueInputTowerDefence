using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum RailRotationMode
{
	None, Snap, Slerp
};

public class RailManager : MonoBehaviour {
	
	public bool activated = true;
	public List<Transform> objectToMove = new List<Transform>();
	public float moveSpeed = 1.0f;
	public float nodeProximityDistance = 0.1f;
	public RailRotationMode rotationMode;
	public float slerpRotationSpeed = 1.0f;
	public List<Transform> railNodes = new List<Transform>();
	
	private List<int> targetNodeIndex = new List<int>();
	private List<Vector3> directionVector = new List<Vector3>();
	public int EnemiesThisCycle = 0;
	//--------------------Unity Functions--------------------
	
	void Start()
	{
		//Moving the object to the first node.
		int counter = 0;
		foreach (var obj in objectToMove) {
			targetNodeIndex.Add(0);
			directionVector.Add(new Vector3(0,0,0));
			counter++;
		}
	}

	public void AddEntity(GameObject entity)
	{
		targetNodeIndex.Add(0);
		directionVector.Add(new Vector3(0,0,0));
		entity.transform.parent = transform;
		objectToMove.Add (entity.transform);
	}

	public void AddEntity(GameObject entity, int index)
	{
		targetNodeIndex.Add(0);
		directionVector.Add(new Vector3(0,0,0));
		entity.transform.parent = transform;
		objectToMove.Insert (index, entity.transform);
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
			
			//Moving the object towards the target node.
			directionVector[i] = (railNodes[targetNodeIndex[i]].position - objectToMove[i].position).normalized;
			if(objectToMove[i].GetComponent<Rigidbody>() != null)
				objectToMove[i].GetComponent<Rigidbody>().velocity = directionVector[i] * moveSpeed * Time.deltaTime;
			objectToMove[i].Translate (directionVector[i] * Time.deltaTime * moveSpeed, Space.World);
			
			//Rotating the object to face the target node
			//depending on the specified rotation mode.
			switch (rotationMode) 
			{
			case RailRotationMode.Snap:
				objectToMove[i].LookAt (railNodes [targetNodeIndex[i]].position);
				break;
				
			case RailRotationMode.Slerp:
				Quaternion targetRotation = Quaternion.LookRotation(railNodes [targetNodeIndex[i]].position - objectToMove[i].position);
				objectToMove[i].rotation = Quaternion.Slerp(objectToMove[i].rotation, targetRotation, Time.deltaTime * slerpRotationSpeed);
				break;
				
			default:
				break;
			}
			
			//Incrementing the target node if the object 
			//has reached the previous target node.
			if (ObjectIsOnNode (targetNodeIndex[i], i)) 
			{
				//objectToMove[i].position = railNodes [targetNodeIndex[i]].position;
				targetNodeIndex[i]++;
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
			Vector3 previousNode = railNodes [0].position;
			foreach(var node in railNodes)
			{
				Gizmos.DrawSphere (node.position, 0.15f);
				Gizmos.DrawLine(previousNode, node.position);
				previousNode = node.position;
			}
		}
	}

	void AddNode(Transform newNode, int index)
	{
		railNodes.Insert (index, newNode);
	}
	
	//--------------------Private Functions--------------------
	
	bool ObjectIsOnNode(int nodeIndex, int index)
	{
		//Checking if the distance from the object to the target node is less than the proximity distance.
		return (Vector3.Distance (objectToMove[index].position, railNodes [nodeIndex].position) < nodeProximityDistance);
	}

	public void ResetEntities()
	{
		objectToMove.Clear ();
		targetNodeIndex.Clear ();
		directionVector.Clear ();
	}
}
