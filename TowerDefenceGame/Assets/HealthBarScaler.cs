using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBarScaler : MonoBehaviour {

	public Text text;
	public Image bar;

	private GameManager manager;
	// Use this for initialization
	void Start () {
		if (!text)
			text = gameObject.GetComponentInChildren<Text> ();
		if (!bar)
			bar = gameObject.GetComponentInChildren<Image> ();
		if (!manager)
			manager = GameManager.Instance;

	}
	
	// Update is called once per frame
	void Update () {
		text.text = (manager.villageHealth - manager.EnemiesPassed) + "/" + manager.villageHealth;
		Vector2 barScale = new Vector2((1.0f / manager.villageHealth) * (manager.villageHealth - manager.EnemiesPassed), 1);
		bar.transform.localScale = barScale;
	}
}
