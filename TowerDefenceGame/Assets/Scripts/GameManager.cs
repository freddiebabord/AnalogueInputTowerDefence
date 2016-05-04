using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	[SerializeField]
	int enemiesPassed = 0;
	[SerializeField]
	int goldQuantity = 0;


	public void AddEnemyInHome()
	{
		enemiesPassed++;
	}

	public void AddGold(int amount)
	{
		goldQuantity += amount;
	}

}
