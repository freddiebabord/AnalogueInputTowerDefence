using System.Collections.Generic;
using UnityEngine;


class RailSystemThread : ThreadedJob
{
    public List<AIBase> InData;  // arbitary job data
    public List<AIBase> OutData; // arbitary job data
    public List<Node> railNodes;
    public List<GameObject> objectsToRemove = new List<GameObject>();
    public RailRotationMode rotationMode;
    public float slerpRotationSpeed = 1.0f;
    public float nodeProximityDistance = 0.1f;
    public List<Vector3> newPositions = new List<Vector3>();
    public List<Quaternion> newRotations = new List<Quaternion>();

    protected override void ThreadFunction()
    {
        for (int i = 0; i < InData.Count; ++i)
        {
            if (InData[i] == null)
            {
                InData.TrimExcess();
                continue;
            }
            //if (!InData[i].gameObject.activeInHierarchy)
            //    continue;

            AIBase aiObj = InData[i];
            //Exiting if the target node is 
            //outside of the railNodes list.
            if (aiObj.CurrentIndex >= railNodes.Count)
            {
                InData.Remove(aiObj);
                objectsToRemove.Add(aiObj.gameObject);
                InData.TrimExcess();
                continue;
            }

            if (ReferenceEquals(aiObj.currentTarget, null))
            {

                aiObj.DirectionVector = (aiObj.currentNodeTarget - aiObj.transform.position).normalized;

                Vector3 translationAmount = aiObj.DirectionVector * Time.deltaTime * aiObj.Speed;

                newPositions.Add(translationAmount);

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
                        Quaternion rotationAmount = Quaternion.Slerp(aiObj.transform.rotation, targetRotation, Time.deltaTime * slerpRotationSpeed);
                        newRotations.Add(rotationAmount);
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

                aiObj.DirectionVector = (aiObj.currentTarget.transform.position - aiObj.transform.position).normalized;

                newPositions.Add(aiObj.DirectionVector * Time.deltaTime * aiObj.Speed);

                Quaternion targetRotation = Quaternion.LookRotation(aiObj.currentTarget.transform.position - aiObj.transform.position);
                newRotations.Add(Quaternion.Slerp(aiObj.transform.rotation, targetRotation, Time.deltaTime * slerpRotationSpeed));
            }
        }
    }

    protected override void OnFinished()
    {
        for (int i = 0; i < objectsToRemove.Count; ++i)
        {
            UnityEngine.GameObject.Destroy(InData[i].gameObject);
            InData.RemoveAt(i);
        }
        InData.TrimExcess();
        int count = 0;
        foreach (AIBase entity in InData)
        {
            entity.transform.Translate(newPositions[count]);
            entity.transform.Rotate(newRotations[count].eulerAngles);
            OutData.Add(entity);
            count++;
        }
    }

    bool ObjectIsOnNode(AIBase obj)
    {
        //Checking if the distance from the object to the target node is less than the proximity distance.
        return (Vector3.Distance(obj.transform.position, obj.currentNodeTarget) < nodeProximityDistance);
    }





}
