using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SphereCollider))]
public class AITowerSeeker : AIBase {

    public float seekRadius = 5.0f;
    List<Collider> triggerList = new List<Collider>();

    List<string> tags = new List<string>() { "EnemyStart", "EnemyEnd", "Grass", "Enemy", "Untagged" };

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

    public override void Die(float delay = 0)
    {
        explosion.GetComponent<VolumetricExplosion>().explosionDamage = damage;
        base.Die();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!tags.Contains(collision.collider.gameObject.tag))
        {
            collision.collider.gameObject.BroadcastMessage("ApplyDamage", damage, SendMessageOptions.RequireReceiver);
            Die();
        }
    }

	void OnTriggerEnter(Collider other)
	{
        if (!tags.Contains(other.gameObject.tag))
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
