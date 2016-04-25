using UnityEngine;
using System.Collections;

[System.Serializable]
public class AIBase : MonoBehaviour {

	[SerializeField]
	protected float hp = 100;
	[SerializeField]
	protected float speed = 10;
	[SerializeField]
	protected float damage;
	[SerializeField]
	protected bool flying;
    [HideInInspector]
	public GameObject currentTarget;
	[SerializeField]
	protected int goldDrop;
	protected GameManager game;
    protected Vector3 directionVector = new Vector3(0,1,0);
    protected int currentIndex = 0;

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
	}

	public virtual void Update()
	{
		if (hp <= 0)
			DieWithGold ();

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

}
