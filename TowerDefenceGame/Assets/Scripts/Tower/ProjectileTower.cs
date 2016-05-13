using UnityEngine;
using System.Collections;
using System.Collections.Generic;


class ProjectileTower : TowerBase
{
    public GameObject m_bullet = null;
    public float m_speed = 5.0f;


    protected override IEnumerator Attack()
    {
        if (m_currentTarget)
        {
            GameObject projectile = Instantiate(m_bullet, m_spawnPoint.position, m_spawnPoint.rotation) as GameObject;
            projectile.GetComponent<Rigidbody>().velocity = m_rotationPoint.transform.forward * m_speed;
        }
        return base.Attack();
    }
}