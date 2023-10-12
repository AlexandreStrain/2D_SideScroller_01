using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public bool _isPaused = true;

    [Header("Player Information")]
    public InputController _controls;
    public Sprite _player;
    public Vector2 _playerStartLocation;
    public int _playerLives;
    public float _playerCoins;

    [Header("Level Information")]
    public Scene _currentScene;
    public Level _currentLevel;
    public int _currentLevelNumber = 1;
    public int _totalNumberOfLevels = 2;

    [Header("Boss Information")]
    public bool _isBossDefeated;
    public UnityEvent _onBossDefeat;

    [Header("Debug")]
    public float _respawnDelay = 2f;
    private float _respawnTimer;
    
    
    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<Sprite>();
        RestartLevel();
    }

    void Update()
    {
        if (!_player._isAlive && !_isPaused)
        {
            RespawnPlayer();
        } 
    }

    public void RespawnPlayer()
    {
        //increment the respawn timer gradually over time
        _respawnTimer += Time.deltaTime;
        //once it reaches the delay
        if (_respawnTimer >= _respawnDelay)
        {
            //reset respawn timer
            _respawnTimer = 0f;
            //if the player runs out of lives
            if (_playerLives-- <= 0)
            {
                //game starts back at the main menu on level one
                _isPaused = true;
                _currentLevelNumber = 1;
                _controls.Reset();
                UserInterface.Instance.ToggleTitleScreen(true);
                UserInterface.Instance.ToggleBossUI(false);
                UserInterface.Instance.ToggleGameScreen(false);
                RestartLevel();
            }
            else
            {
                //if player still has lives, bring player back to start of level
                _player.Init();
                _player.transform.position = _playerStartLocation;

                //if boss is still alive, then reset Boss UI if activated
                if (_currentLevel._boss._isAlive)
                {
                    _currentLevel.ToggleBossState(false);
                }
            }
        }
    }

    public void RestartLevel()
    {
        //lives and coins revert back to original values
        _playerLives = 3;
        _playerCoins = 0;

        _player.Init();
        _player.transform.position = _playerStartLocation;
        _isBossDefeated = false;
        LoadLevel("Level" + _currentLevelNumber);
    }

    public void LoadLevel(string levelName)
    {
        //if there is more than one scene open, then a level has previously been loaded in
        if (SceneManager.sceneCount > 1)
        {
            //unload the current level
            SceneManager.UnloadSceneAsync(_currentScene);
        }
        //load in the correct level scene 
        SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
        //keep track of the level scene that was loaded for later removal
        _currentScene = SceneManager.GetSceneByName(levelName);
    }

    public void LoadNextLevel()
    {
        _player.Init();
        _player.transform.position = _playerStartLocation;

        //increment level number, but keep it within the total amount of levels
        //using remainder %
        if (_currentLevelNumber % _totalNumberOfLevels == 0)
        {
            _currentLevelNumber = 1;
        }
        else
        {
            _currentLevelNumber++;
        }


        LoadLevel("Level" + _currentLevelNumber);

        _isBossDefeated = false;
    }

    public void OnBossDefeat()
    {
        _currentLevel.ToggleBossState(false);
        _currentLevel._bossArea._reset.Invoke();
        _currentLevel._bossArea.gameObject.SetActive(false);
    }

}
