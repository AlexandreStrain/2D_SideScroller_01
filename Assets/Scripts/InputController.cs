using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    public Sprite _player;

    //Headers will be used to help organize our code
    [Header("DEBUG")]
    public Vector2 _movement;
    public bool _shootButton;

    //A reference variable to our PlayerInputActions asset we created earlier
    private PlayerInputActions _inputActions;

    // Start is called before the first frame update
    void Awake()
    {
        //Create a reference to our PlayerInputActions created inside Unity
        _inputActions = new PlayerInputActions();

        //Note: we are not calling a method here but attaching it to unity’s key pressed action
        _inputActions.Player.Movement.performed += GetMovementInput;
        _inputActions.Player.Movement.canceled += GetMovementInput;

        _inputActions.Player.Shoot.performed += GetShootInput;
        _inputActions.Player.Shoot.canceled += GetShootInput;

        //This is a Lambda Expression, an anonymous method we only create and use here
        _inputActions.Player.Pause.performed += _ => PauseGame();

        _inputActions.UI.Start.performed += _ => StartGame();
    }

    private void Start()
    {
        _player.Init();
    }

    // Update is called once per frame
    void Update()
    {
        _player._movement = _movement;
        _player._isShooting = _shootButton;
        _player.Tick();
    }

    //we'll make this method public because we will need to use it for the main menu later
    public void StartGame()
    {
        //For now, this only changes our Action Map from UI to Player
        Debug.Log("Starting or Resuming Game");

        _inputActions.UI.Disable();
        _inputActions.Player.Enable();

        GameManager.Instance._isPaused = false;

        UserInterface.Instance.ToggleTitleScreen(false);
        UserInterface.Instance.TogglePauseScreen(false);
        UserInterface.Instance.ToggleGameScreen(true);
    }

    //A callback is executable code that is passed as an argument to other code
    private void GetMovementInput(InputAction.CallbackContext ctx)
    {
        _movement = ctx.ReadValue<Vector2>();
    }

    private void GetShootInput(InputAction.CallbackContext ctx)
    {
        /*relational operators are not just for if statements,
         they can be stored in a variable*/
        _shootButton = ctx.ReadValue<float>() > 0f;
    }

    private void PauseGame()
    {
        //For now, this only changes our Action Map from Player to UI
        Debug.Log("Pausing Game");

        _inputActions.UI.Enable();
        _inputActions.Player.Disable();

        GameManager.Instance._isPaused = true;

        UserInterface.Instance.ToggleGameScreen(false);
        UserInterface.Instance.TogglePauseScreen(true);
    }

    public void Reset()
    {
        _inputActions.UI.Enable();
        _inputActions.Player.Disable();
    }

    private void OnEnable()
    {
        _inputActions.UI.Enable();
    }
    private void OnDisable()
    {
        _inputActions.UI.Disable();
        _inputActions.Player.Disable();
    }
}