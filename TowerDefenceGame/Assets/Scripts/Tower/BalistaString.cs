using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]

public class BalistaString : MonoBehaviour {

	public Transform armPoint1;
	public Transform armPoint2;
	public Transform pullBack;

	public LineRenderer m_renderer;


	// Use this for initialization
	void Start () {
        if (m_renderer == null)
            m_renderer = GetComponent<LineRenderer>() as LineRenderer;
        m_renderer.SetVertexCount(3);
	}
	
	// Update is called once per frame
	void Update () {
        m_renderer.SetPosition(0, armPoint1.position);
        m_renderer.SetPosition(1, pullBack.position);
        m_renderer.SetPosition(2, armPoint2.position);

	}

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(armPoint1.position, pullBack.position);
        Gizmos.DrawLine(armPoint2.position, pullBack.position);
    }
    
}
