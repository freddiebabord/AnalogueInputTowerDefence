using UnityEngine;
using System.Collections;

[System.Serializable]
public class AIBase : MonoBehaviour {

	[SerializeField] protected float hp = 100;
	[SerializeField] protected float speed = 10;
	[SerializeField] protected float damage;
	[SerializeField] protected bool flying;
	[SerializeField][Range(0,100)] protected int goldDrop;
	[HideInInspector] protected int currentIndex = 0;
    [HideInInspector] public GameObject currentTarget;
    [HideInInspector] public Vector3 currentNodeTarget = Vector3.zero;
	[HideInInspector] public bool railUpdatingEntity = false;
	protected GameManager game;
    protected Vector3 directionVector = new Vector3(0,1,0);
    protected Animation animations;
    protected Vector3 deathLocation;
    private bool hasDied = false;
    public GameObject shadow;
	public GameObject explosion;
	public AudioSource screemEffect;
	public AudioClip altScreemEffect;

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
		set { hp = value;}
    }

    public int GoldDrop { set { goldDrop = value; } get { return goldDrop; } }

	// Use this for initialization
	public virtual void Start () 
	{
		game = GameObject.FindObjectOfType<GameManager> ();
        animations = GetComponent<Animation>();
        if (explosion != null)
        {
            explosion.transform.parent = transform.parent;
            explosion.SetActive(false);
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
            if (!hasDied)
            {
                GameObject.FindObjectOfType<AudioManager>().PlayEnemyDeath();
				GameObject.FindObjectOfType<RailManager>().RemoveEntity(this);
                if (GetComponent<Animator>())
                    GetComponent<Animator>().enabled = false;
				if (GetComponent<BoxCollider>())
					GetComponent<BoxCollider>().enabled = false;


                if (shadow)
                    shadow.SetActive(false);
				Die();
            }
            foreach (Transform child in gameObject.GetComponentsInChildren<Transform>())
            {
                if (child.transform.position.y <= 0)
                    Destroy(child.gameObject);
            }
        }

	}

	public virtual void Attack()
	{
	}


    public virtual void Die(float delay = 0)
    {
		hasDied = true;
        if(goldDrop > 0)
			game.AddGold (goldDrop);

        if (explosion != null)
        {
            explosion.transform.position = transform.position;
            explosion.transform.rotation = transform.rotation;
            explosion.transform.parent = null;
            explosion.SetActive(true);
        }
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
        Gizmos.DrawSphere(currentNodeTarget, 0.15f);
        Gizmos.color = Color.grey;
        Gizmos.DrawLine(transform.position, currentNodeTarget);
    }

}
