using UnityEngine;
using UnityEngine.Events;

public class Level : MonoBehaviour
{
    private AudioSource _audioSource;
    public LevelInformation info;


    [Header("Boss Information")]
    //keep track of the enemy and the area that triggers the fight to start
    public EnemyController _boss;
    public bool _isBossDefeated; //useful??
    public EventArea _bossArea;
    public UnityEvent OnBossDefeat;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        SetAndPlayMusic(info._defaultMusic);

        GameManager.Instance._currentLevel = this;
        UserInterface.Instance.Init();
        ToggleBossState(false);
        ResetBossArena();
    }

    public void RestartLevel()
    {
        GameManager.Instance._player.Init(info._playerStartLocation);
        ResetBoss();
    }

    public void BossDefeated()
    {
        _isBossDefeated = true;
        ToggleBossState(false);
        OnBossDefeat.Invoke();
        ResetLevelMusic();
    }

    private void ResetLevelMusic()
    {
        SetAndPlayMusic(info._defaultMusic);
    }

    private void SetAndPlayMusic(AudioClip ac)
    {
        _audioSource.Stop();
        _audioSource.clip = ac;
        _audioSource.Play();
    }

    public void ResetBoss()
    {
        //if boss is still alive, then reset Boss UI if activated
        if (_boss._isAlive)
        {
            ToggleBossState(false);
            ResetBossArena();
        }
    }

    //depending on if the boss is defeated or not, enable or disable boss
    //and all that is associated with them
    public void ToggleBossState(bool state)
    {
        UserInterface.Instance.ToggleBossUI(state);
        _boss.gameObject.SetActive(state);
    }

    private void ResetBossArena()
    {
        _bossArea._reset.Invoke();
        _boss._currentHealth = _boss._stats._maxHealth;
        SetAndPlayMusic(info._defaultMusic);
    }

    public void GoToNextLevel()
    {
        GameManager.Instance.LoadNextLevel();
        GameManager.Instance.ResetPlayer();
    }
}
