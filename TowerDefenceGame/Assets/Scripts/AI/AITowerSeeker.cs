using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SphereCollider))]
public class AITowerSeeker : AIBase {

    public float seekRadius = 5.0f;
    List<Collider> triggerList = new List<Collider>();

    public override void Start() {

        SphereCollider sc = GetComponent<SphereCollider>();
        sc.radius = seekRadius;
        sc.isTrigger = true;

        base.Start();
	}
	
	// Update is called once per frame
    public override void Update()
    {
        base.Update();
	}

    public override void Die()
    {
        GameObject go = (GameObject)Instantiate(Resources.Load("Prefabs/VolumetricExplosion", typeof(GameObject)), transform.position, Quaternion.identity);
        go.GetComponent<VolumetricExplosion>().explosionDamage = damage;
        base.Die();
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.GetComponentInChildren<TowerClass>())
            Die();
    }

	void OnTriggerEnter(Collider other)
	{
        if (other.gameObject.GetComponentInChildren<TowerClass>())
        {
            if (!triggerList.Contains(other))
            {
                triggerList.Add(other);
                if (currentTarget)
                {
                    if (Vector3.Distance(transform.position, other.transform.position) < Vector3.Distance(transform.position, currentTarget.transform.position))
                        currentTarget = other.gameObject;
                }
                else
                    currentTarget = other.gameObject;
            }
        }
	}

    void OnTriggerExit(Collider other)
    {
        if (triggerList.Contains(other))
        {
            triggerList.Remove(other);
        }
    }

}
