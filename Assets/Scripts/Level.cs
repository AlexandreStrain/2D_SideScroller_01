using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    //get the default level music and the source that plays it
    public AudioClip _levelMusic;
    public AudioSource _audioSource;

    //keep track of the enemy and the area that triggers the fight to start
    public EnemyController _boss;
    public EventArea _bossArea;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _levelMusic = _audioSource.clip;


        //set the boss on the UserInterface to be the one the level has
        UserInterface.Instance._boss = _boss;
        UserInterface.Instance.Init();
        _boss.gameObject.SetActive(false);
        GameManager.Instance._currentLevel = this;
    }

    //depending on if the boss is defeated or not, enable or disable boss
    //and all that is associated with them
    public void ToggleBossState(bool state)
    {
        UserInterface.Instance.ToggleBossUI(state);
        
        //restart audio to play default music
        _audioSource.Stop();
        _audioSource.clip = _levelMusic;
        _audioSource.Play();

        if (_boss._isAlive && !state)
        {
            _bossArea._reset.Invoke();
            _boss._currentHealth = _boss._stats._maxHealth;

            //disable GameObject in scene only if boss is still alive
            _boss.gameObject.SetActive(state);
        }
    }

    //fix BossArea script so it can still toggle UI
    public void GetBossUI()
    {
        UserInterface.Instance.ToggleBossUI(true);
    }

    public void GoToNextLevel()
    {
        GameManager.Instance.LoadNextLevel();
    }
}
