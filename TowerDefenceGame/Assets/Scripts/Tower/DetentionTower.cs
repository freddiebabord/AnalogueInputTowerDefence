using UnityEngine;
using System.Collections;
using System.Collections.Generic;


class DetentionTower : TowerBase
{
    protected override void Start()
    {
        base.Start();
        m_attackInterval = 2;
    }

    protected override IEnumerator Attack()
    {
        foreach (GameObject enemy in m_tragetList)
        {
            if (enemy)
            {
                AIBase ai = enemy.GetComponent<AIBase>();
                ai.ApplyDamage(10);
                if (ai.Speed >= 3)
                    ai.Speed /= 2;
            }
        }
        return base.Attack();
    }
}

