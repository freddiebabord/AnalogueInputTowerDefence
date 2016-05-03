using UnityEngine;
using System.Collections;

[System.Serializable]
public class AIBase : MonoBehaviour {

	[SerializeField] protected float hp = 100;
	[SerializeField] protected float speed = 10;
	[SerializeField] protected float damage;
	[SerializeField] protected bool flying;
	[SerializeField] protected int goldDrop;
    [SerializeField] protected int currentIndex = 0;
    [HideInInspector] public GameObject currentTarget;
    [HideInInspector] public Vector3 currentNodeTarget;
	protected GameManager game;
    protected Vector3 directionVector = new Vector3(0,1,0);
    protected Animation animations;
    protected Vector3 deathLocation;

    public int CurrentIndex { get { return currentIndex; } set { currentIndex = value; } }

    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    public Vector3 DirectionVector
    {
        get { return directionVector; }
        set { directionVector = value; }
    }

    public float Health
    {
        get { return hp; }
    }

	// Use this for initialization
	public virtual void Start () 
	{
		game = GameObject.FindObjectOfType<GameManager> ();
        animations = GetComponent<Animation>();
       
        foreach (MeshCollider child in gameObject.GetComponentsInChildren<MeshCollider>())
        {
            child.enabled = false;
            if (child.gameObject.GetComponent<Rigidbody>())
            {
                child.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                child.gameObject.GetComponent<Rigidbody>().detectCollisions = false;
            }
        }
	}

	public virtual void Update()
	{
        if (animations != null)
        {
            if (animations.GetClip("AI_Basic_Walk"))
            {
                if (!animations.IsPlaying("AI_Basic_Walk") && (hp > 0))
                    animations.Play("AI_Basic_Walk");
            }
        }
        if (hp <= 0)
        {
            GameObject.FindObjectOfType<RailManager>().RemoveEntity(this);
            GetComponent<Animator>().enabled = false;
            foreach (Transform child in gameObject.GetComponentsInChildren<Transform>())
            {
                if (child.transform.position.y <= 0)
                    child.transform.position = new Vector3(child.transform.position.x, 0.1f, child.transform.position.z);
                if (child.GetComponent<MeshCollider>())
                    child.GetComponent<MeshCollider>().enabled = true;
                if (child.gameObject.GetComponent<Rigidbody>())
                {
                    child.rigidbody.isKinematic = false;
                    child.rigidbody.detectCollisions = true;
                }
            }
            DieWithGold();
        }

	}

	public virtual void Attack()
	{
	}

	public virtual void DieWithGold()
	{
		game.AddGold (goldDrop);
        var cols = gameObject.GetComponentsInChildren<MeshCollider>();
        if (cols != null)
            Die(3.5f);
        else
            Die();
	}

    public virtual void Die(float delay = 0)
    {
        if (animations != null)
            StartCoroutine(DieAnimDelay());
        else
            StartCoroutine(DieWithDelay(delay));
        
    }

    protected IEnumerator DieAnimDelay()
    {
        yield return new WaitForSeconds(/*animations.GetClip("AI_Basic_Death").averageDuration + */2.5f);
        Destroy(gameObject);
    }

    protected IEnumerator DieWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

	public virtual void SwitchTarget(GameObject newTarget)
	{
		currentTarget = newTarget;
	}

	public virtual void ApplyDamage(float damageAmount)
	{
		hp -= damageAmount;
	}

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(currentNodeTarget, 0.25f);
        Gizmos.color = Color.grey;
        Gizmos.DrawLine(transform.position, currentNodeTarget);
    }

}
