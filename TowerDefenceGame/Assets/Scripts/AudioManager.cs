using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour {


    public AudioSource enemyDeathSound;

    public void PlayEnemyDeath()
    {
        enemyDeathSound.Play();
    }
}
