using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserInterface : Singleton<UserInterface>
{
    [Header("Screens")]
    [SerializeField] private GameObject _titleScreen;
    [SerializeField] private GameObject _pauseScreen;
    [SerializeField] private GameObject _gameScreen;

    [Header("Player Information")]
    [SerializeField] private TMP_Text _playerHPText;
    [SerializeField] private TMP_Text _playerCoinText;
    [SerializeField] private TMP_Text _playerLivesText;
    [SerializeField] private Slider _playerHPBar;

    public Sprite _player;

    [Header("Boss Information")]
    [SerializeField] private GameObject _bossInfo;
    [SerializeField] private TMP_Text _bossNameText;
    [SerializeField] private TMP_Text _bossHPText;
    [SerializeField] private Slider _bossHPBar;

    public Sprite _boss;
    private void Awake()
    {
        //Activate only TitleScreen at start
        _titleScreen.SetActive(true);
    }

    public void Init()
    {

        //Assign correct Slider values to match player health
        _playerHPBar.maxValue = _player._stats._maxHealth;
        _playerHPBar.value = _player._stats._maxHealth;

        _playerCoinText.text = GameManager.Instance._playerCoins.ToString();
        _playerLivesText.text = "Lives: " + GameManager.Instance._playerLives.ToString();

        //Assign correct Slider values to match boss health
        _bossHPBar.maxValue = _boss._stats._maxHealth;
        _bossHPBar.value = _boss._stats._maxHealth;
        //Assign boss name to boss text
        _bossNameText.text = _boss._stats.name.ToString();
        //Deactivate Boss HP Bar until player encounters boss
        _bossInfo.SetActive(false);
    }


    void Update()
    {
        //update players hp each frame
        _playerHPBar.value = _player._currentHealth;

        /*Make text reflect new values for health... Putting a $ in front of a string "" in C# formats the text and avoids String Concatenation*/
        _playerHPText.text =
                          $"{_player._currentHealth}/{_player._stats._maxHealth}";

        _playerCoinText.text = GameManager.Instance._playerCoins.ToString();
        _playerLivesText.text = "Lives: " + GameManager.Instance._playerLives.ToString();

        //only update the boss health if the Boss Info GameObject is active in the scene
        if (_bossInfo.activeSelf)
        {
            _bossHPBar.value = _boss._currentHealth;
            _bossHPText.text =
                              $"{_boss._currentHealth}/{_boss._stats._maxHealth}";
        }
    }

    //Activate or Deactivate the Title Screen with a boolean variable
    public void ToggleTitleScreen(bool state)
    {
        _titleScreen.SetActive(state);
    }

    //Activate or Deactivate the Pause Screen with a boolean variable
    public void TogglePauseScreen(bool state)
    {
        _pauseScreen.SetActive(state);
    }

    //Activate or Deactivate the Game Screen with a boolean variable
    public void ToggleGameScreen(bool state)
    {
        _gameScreen.SetActive(state);
    }

    //Activate or Deactivate the Boss UI with a boolean variable
    public void ToggleBossUI(bool state)
    {
        _bossInfo.SetActive(state);
    }

    //In future, we might want to click a button to exit game
    public void QuitGame()
    {
        Application.Quit();
    }
}
