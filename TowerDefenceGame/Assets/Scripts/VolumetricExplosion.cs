using UnityEngine;
using System.Collections;

public class VolumetricExplosion : MonoBehaviour {

    public float loopduration = 1.667f;
    public float fadeDuration = 2.5f;
    public float explosionDamage = 50;
    
    bool shouldFade = false;
    float startClipRange = 0.0f;
    float radius = 5.0f;

	// Use this for initialization
	void Start () {
        StartCoroutine(fadeOut());
        startClipRange = renderer.material.GetFloat("_ClipRange");
	}
	
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
        
        //float clipRange = shouldFade ? Mathf.Lerp(startClipRange, 0.0f, Time.time / fadeDuration) : startClipRange;
        //renderer.material.SetFloat("_ClipRange", clipRange);
        //Debug.Log(renderer.material.GetFloat("_ClipRange"));
	}

    IEnumerator fadeOut()
    {
        yield return new WaitForSeconds(loopduration);
        shouldFade = true;
        yield return new WaitForSeconds(fadeDuration + 1.5f);
        Destroy(gameObject);
    }

    void OnEnable()
    {
        var cols = Physics.OverlapSphere(transform.position, radius);
        foreach (var c in cols)
        {
            c.gameObject.SendMessage("ApplyDamage", explosionDamage, SendMessageOptions.DontRequireReceiver);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
