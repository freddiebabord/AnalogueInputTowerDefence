using UnityEngine;
using System.Collections;

public class VolumetricExplosion : MonoBehaviour {

    public float loopduration = 1.667f;
    public float fadeDuration = 2.5f;
    public float explosionDamage = 50;
    Coroutine explosionRoutine;
    public float radius = 5.0f;
	
	// Update is called once per frame
	void Update () {
        float r = Mathf.Sin((Time.time / loopduration) * (2 * Mathf.PI)) * 0.5f + 0.25f;
        float g = Mathf.Sin((Time.time / loopduration + 0.33333333f) * 2 * Mathf.PI) * 0.5f + 0.25f;
        float b = Mathf.Sin((Time.time / loopduration + 0.66666667f) * 2 * Mathf.PI) * 0.5f + 0.25f;
        float correction = 1 / (r + g + b);
        r *= correction;
        g *= correction;
        b *= correction;
        renderer.material.SetVector("_ChannelFactor", new Vector4(r, g, b, 0));
        
	}

    IEnumerator fadeOut()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

    void OnEnable()
    {
        explosionRoutine = StartCoroutine(ExplodeAfterTime());
        StartCoroutine(fadeOut());
    }

    void OnDisable()
    {
        if (explosionRoutine == null)
            StopCoroutine(explosionRoutine);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    IEnumerator ExplodeAfterTime()
    {
        yield return new WaitForSeconds(1.5f);
        var cols = Physics.OverlapSphere(transform.position, radius / 2);
        foreach (var c in cols)
        {
            if (c.collider.gameObject.tag != "Enemy" & c.collider.gameObject.tag != "Untagged")
            {
                c.collider.gameObject.BroadcastMessage("ApplyDamage", explosionDamage, SendMessageOptions.DontRequireReceiver);
            }
        }
    }
}
