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

	// Use this for initialization
	public virtual void Start () 
	{
		game = GameObject.FindObjectOfType<GameManager> ();
        animations = GetComponent<Animation>();
	}

	public virtual void Update()
	{
        if (animations != null)
        {
            if (!animations.IsPlaying("AI_Basic_Walk") && (hp > 0))
                animations.Play("AI_Basic_Walk");
        }
        if (hp <= 0)
        {
            if (deathLocation != transform.position)
                deathLocation = transform.position;
            if (animations != null)
            {
                if (!animations.IsPlaying("AI_Basic_Death"))
                {
                    animations.Stop();
                    animations.Play("AI_Basic_Death");
                    DieWithGold();
                }
            }
            else
            {
                DieWithGold();
            }
        }

	}

	public virtual void Attack()
	{
	}

	public virtual void DieWithGold()
	{
		game.AddGold (goldDrop);
        Die();
	}

    public virtual void Die()
    {
        if(animations != null)
            StartCoroutine(DieAnimDelay());
        else
            Destroy(gameObject);
        
    }

    protected IEnumerator DieAnimDelay()
    {
        yield return new WaitForSeconds(animations.GetClip("AI_Basic_Death").averageDuration + 2.5f);
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
