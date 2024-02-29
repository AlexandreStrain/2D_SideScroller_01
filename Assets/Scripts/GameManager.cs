using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public bool _isPaused = true;
    public event Pause OnToggleGamePause;
    public event Reset OnGameRestart;

    [Header("Player Information")]
    public Sprite _player;
    public int _playerLives;
    public float _playerCoins;

    [Header("Level Information")]
    public Scene _currentScene;
    public Level _currentLevel;
    public int _currentLevelNumber = 1;
    public int _totalNumberOfLevels = 2;

    [Header("Debug")] 
    public float _respawnDelay = 2f;
    private float _respawnTimer;
    
    
    private void Awake()
    {
        GetPlayer();
        LoadLevel("Level" + _currentLevelNumber);
    }

    private void Start()
    {
        RestartGame();
        ToggleGamePause(true);
    }

    private void GetPlayer()
    {
        _player = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<Sprite>();
    }

    private void RestartGame()
    {
        //lives and coins revert back to original values
        _playerLives = 3;
        _playerCoins = 0;

        _currentLevel.RestartLevel();
    }
    public void ToggleGamePause(bool state)
    {
        _isPaused = state;
        OnToggleGamePause();
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
                OnGameRestart();
                //game starts back at the main menu on level one
                ToggleGamePause(true);
                _currentLevelNumber = 1;
                LoadLevel("Level" + _currentLevelNumber);
                UserInterface.Instance.SetToTitleScreen();

                RestartGame();

            }
            else
            {
                _currentLevel.RestartLevel();
                //if player still has lives, bring player back to start of level
                _currentLevel.ResetBoss();
            }
        }
    }

    public void LoadLevel(string levelName)
    {
        UnloadPreviousLevelScene();

        //load in the correct level scene 
        SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);

        //keep track of the level scene that was loaded for later removal
        _currentScene = SceneManager.GetSceneByName(levelName);
    }

    private void UnloadPreviousLevelScene()
    {
        //if there is more than one scene open, then a level has previously been loaded in
        if (SceneManager.sceneCount > 1)
        {
            SceneManager.UnloadSceneAsync(_currentScene);
        }
    }
    public void ResetPlayer()
    {
        _player.Init(_currentLevel.info._playerStartLocation);
    }

    public void LoadNextLevel()
    {
        IncreaseLevelNumber();

        LoadLevel("Level" + _currentLevelNumber);
    }
    private void IncreaseLevelNumber()
    {
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
    }
}
