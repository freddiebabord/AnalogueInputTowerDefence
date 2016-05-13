using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(SphereCollider))]
public class TowerBase : MonoBehaviour
{
    // --------------------- VARIABLES ---------------------

    protected float m_health = 100.0f;
    protected NodePath m_pathTile = null;
    protected GameObject m_goal = null;
    [SerializeField]protected GameObject m_currentTarget = null;
    protected List<GameObject> m_tragetList = new List<GameObject>();

    public float m_rotationSpeed = 4f;
    public GameObject m_rotationPoint = null;
    public GameObject m_explosion = null;
    public GameObject m_rangeIndicator = null;

    protected int m_cost = 100;
    protected bool m_upgradable = false;
    protected int m_xp = 0;
    protected int m_level = 1;

    protected float m_attackRadius = 15.0f;
    [SerializeField] protected float m_damage = 10.0f;
    protected bool m_isAttacking = false;
    protected float m_attackInterval = 2.5f;
    public Transform m_spawnPoint;
    protected Animation m_animation;

    // --------------------- VARIABLES ---------------------

    // --------------------- PROPERTIES ---------------------

    public float Health { get { return m_health; } set { m_health = value * m_level; } }
    public int Cost { get { return m_cost; } }
    public NodePath Tile { set { m_pathTile = value; } get { return m_pathTile; } }
    // --------------------- PROPERTIES ---------------------

    // --------------------- UNITY FUNCTIONS ---------------------

    protected virtual void Start()
    {
        m_goal = GameObject.FindGameObjectWithTag("EnemyEnd");
        SphereCollider collider = GetComponent<SphereCollider>();
        collider.isTrigger = true;
        collider.radius = m_attackRadius;
        m_animation = GetComponentInChildren<Animation>();
        gameObject.name.Replace("(Clone)", "");
    }

    protected virtual void Update() 
    {
        if (!m_upgradable && m_tragetList.Count > 0)
            m_xp++;

        FaceTarget();

        if (m_currentTarget == null && m_tragetList.Count > 0)
            m_currentTarget = GetClosestEnemy();

        if (!m_isAttacking)
        {
            m_isAttacking = true;
            StartCoroutine(Attack());
        }

        if (m_health < 0)
            Die();
    }

    protected virtual void OnTriggerEnter(Collider collider) 
    {
        if (collider.gameObject.tag == "Enemy")
        {
            m_tragetList.Add(collider.gameObject);
            m_currentTarget = GetClosestEnemy();
        }
    }
    protected virtual void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.tag == "Enemy")
        {
            if (m_tragetList.Contains(collider.gameObject))
            {
                m_tragetList.Remove(collider.gameObject);
                m_tragetList.TrimExcess();
                m_currentTarget = GetClosestEnemy();
            }
        }
    }
    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 1, 1, 0.5f);
        Gizmos.DrawWireSphere(transform.position, m_attackRadius);
    }

    // --------------------- UNITY FUNCTIONS ---------------------

    public virtual void ApplyDamage(float damageAmount)
    {
        m_health -= damageAmount;
    }

    protected void FaceTarget()
    {
        if (m_currentTarget != null)
        {
            Vector3 direction = m_currentTarget.transform.position - transform.position;

            Quaternion rotate = Quaternion.LookRotation(direction);
            rotate.x = 0;
            rotate.z = 0;

            m_rotationPoint.transform.rotation = Quaternion.Slerp(m_rotationPoint.transform.rotation, rotate, Time.deltaTime * m_rotationSpeed);
        }
    }

    protected virtual IEnumerator Attack() 
    {
        if (m_animation)
            m_animation.Play();
        yield return new WaitForSeconds(m_attackInterval);
        m_isAttacking = false;
    }

    public virtual void Upgrade()
    {
        m_xp = 1000 * m_level;
    }

    protected virtual GameObject GetClosestEnemy() 
    {
        GameObject currentClosest = m_currentTarget == null ? null : m_currentTarget;
        float currentClosestDistance = m_currentTarget != null ? 
            Mathf.Abs(Vector3.Distance(m_goal.transform.position, m_currentTarget.transform.position))
            : 9999.0f;

        foreach (GameObject enemy in m_tragetList)
        {
            if (enemy)
            {
                if (Mathf.Abs(Vector3.Distance(m_goal.transform.position, enemy.transform.position)) < currentClosestDistance)
                    currentClosest = enemy;
            }
        }
        
        return currentClosest; 
    }

    protected virtual void Die()
    {
        if (m_pathTile)
        {
            m_pathTile.placedTower = null;
            m_pathTile.towerPlaced = false;
        }
        if (m_explosion)
            Instantiate(m_explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}