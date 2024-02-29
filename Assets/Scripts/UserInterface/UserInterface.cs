using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor;

public class UserInterface : Singleton<UserInterface>
{
    [Header("Screens")]
    [SerializeField] private GameObject _titleScreen;
    [SerializeField] private GameObject _pauseScreen;
    [SerializeField] private GameObject _gameScreen;

    [Header("Player Information")]
    [SerializeField] private StatBarUI playerHealthBar;
    [SerializeField] private TMP_Text _playerCoinText;
    [SerializeField] private TMP_Text _playerLivesText;

    [Header("Boss Information")]
    [SerializeField] private StatBarUI bossHealthBar;
    [SerializeField] private GameObject _bossInfo;
    [SerializeField] private TMP_Text _bossNameText;


    public void Init()
    {
        SetToTitleScreen();
        InitPlayerInfo();
        InitBossInfo();
    }

    private void InitPlayerInfo()
    {
        playerHealthBar.Init(GameManager.Instance._player._stats._maxHealth);

        _playerCoinText.text = GameManager.Instance._playerCoins.ToString();
        _playerLivesText.text = "Lives: " + GameManager.Instance._playerLives.ToString();
    }

    private void InitBossInfo()
    {
        bossHealthBar.Init(GameManager.Instance._currentLevel._boss._stats._maxHealth);
        _bossNameText.text = GameManager.Instance._currentLevel._boss._stats.name.ToString();
    }

    void Update()
    {
        UpdatePlayerStatBars();

        _playerCoinText.text = GameManager.Instance._playerCoins.ToString();
        _playerLivesText.text = "Lives: " + GameManager.Instance._playerLives.ToString();

        //only update the boss health if the Boss Info GameObject is active in the scene
        if (_bossInfo.activeSelf)
        {
            UpdateBossStatBars();
        }
    }

    private void UpdatePlayerStatBars()
    {
        playerHealthBar.Set(GameManager.Instance._player._currentHealth);
    }

    
    private void UpdateBossStatBars()
    {
        bossHealthBar.Set(GameManager.Instance._currentLevel._boss._currentHealth);
    }

    public void SetToTitleScreen()
    {
        ToggleTitleScreen(true);
        ToggleGameScreen(false);
        TogglePauseScreen(false);
        ToggleBossUI(false);
    }
    public void SetToGameScreen()
    {
        ToggleTitleScreen(false);
        ToggleGameScreen(true);
        TogglePauseScreen(false);
        ToggleBossUI(false);
    }
    public void SetToPauseScreen()
    {
        ToggleTitleScreen(false);
        ToggleGameScreen(false);
        TogglePauseScreen(true);
        ToggleBossUI(false);
    }

    public void ToggleTitleScreen(bool state)
    {
        _titleScreen.SetActive(state);
    }


    public void TogglePauseScreen(bool state)
    {
        _pauseScreen.SetActive(state);
    }

    public void ToggleGameScreen(bool state)
    {
        _gameScreen.SetActive(state);
    }

    public void ToggleBossUI(bool state)
    {
        _bossInfo.SetActive(state);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#endif
        Application.Quit();
    }
}
