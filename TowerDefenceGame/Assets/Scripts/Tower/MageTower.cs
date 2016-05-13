using UnityEngine;
using System.Collections;
using System.Collections.Generic;



class MageTower : TowerBase
{

    private LineRenderer line;

    protected override void Start()
    {
        base.Start();
        m_attackInterval = 0;
        line = GetComponentInChildren<LineRenderer>();
    }

    protected override IEnumerator Attack()
    {
        line.SetPosition(0, m_spawnPoint.position);
        if (m_currentTarget)
        {
            line.SetPosition(1, m_currentTarget.transform.position);
            m_currentTarget.BroadcastMessage("ApplyDamage", m_damage);
            line.enabled = true;
        }
        else
        {
            m_isAttacking = false;
            line.enabled = false;
        }
        return base.Attack();
    }
}

