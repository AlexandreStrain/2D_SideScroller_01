using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprite : MonoBehaviour
{
    //Let's organize our State Controller's variables into sections with Headers
    [Header("Statistics")]
    public SpriteStatistics _stats;

    //Are we on the ground: true or false? Other states to check will be grouped here
    [Header("States")]
    public float _currentSpeed;
    public float _currentHealth;
    public AttackType _currentAttackType;
    public MoveType _currentMoveType;
    public bool _isGrounded;
    public bool _isShooting;
    public bool _isAlive;

    [Header("Debug")]
    public Vector2 _movement;
    public Vector2 _spriteSize;
    public float _direction;
    public float _shootTimer;
  
    protected Rigidbody2D _rigidbody;
    protected Animator _animator;
    protected AudioSource _audioSource;


    //This method will handle everything to initialize our Sprite's variables 
    public virtual void Init(Vector2 startingPosition)
    {
        //Acquire a Component that exists on the child GameObject of this script
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
        _audioSource = GetComponent<AudioSource>();
        _isAlive = true;
        _direction = 1f;
        _spriteSize = _animator.transform.localScale;
        _currentSpeed = _stats._speed;
        _currentHealth = _stats._maxHealth;
        HandleAnimations();
        transform.position = startingPosition;

        GameManager.Instance.OnToggleGamePause += TogglePhysics;
    }

    //This will act as our Timer, and will be called as the Sprite Updates
    public virtual void Tick()
    {
        if (GameManager.Instance._isPaused)
        {
            return;
        }

        HandleAnimations();

        if (_isAlive)
        {
            HandleMovement(); //A call to the movement method created below
            HandleLanding();
            HandleShooting(); //call to the method
        }
    }

    private void TogglePhysics()
    {
        _rigidbody.simulated = !(GameManager.Instance._isPaused);
    }

    public virtual void TakeDamage(float amount)
    {
        if (_isAlive)
        {
            _currentHealth = Mathf.Clamp(_currentHealth - amount, 0f, _stats._maxHealth);

            if (_currentHealth <= 0)
            {
                HandleDeath();
            }
            //if we did not die, we got hurt, so else plays the hurt sound effect
            else
            {
                _audioSource.PlayOneShot(_stats._hurtSound);
            }
        }
    }

    protected virtual void HandleDeath()
    {
        _isAlive = false;
        _audioSource.PlayOneShot(_stats._deathSound);
    }

    protected virtual void HandleMovement()
    {
        DetermineDirection();
        XAxisMovement();
        YAxisMovement();
    }
    private void DetermineDirection()
    {
        if (_movement.x < 0f)
        {
            _direction = -1f;
        }
        else if (_movement.x > 0f)
        {
            _direction = 1f;
        }
    }
    private void XAxisMovement()
    {
        //There are short forms for Vector Directions, Vector2.right is 1x, 0y, 0z 
        //translation should work if the sprite isn't flying, unless handled in a move pattern
        if (_stats._spriteType != SpriteType.Flying && _currentMoveType != MoveType.Circle)
        {
            transform.Translate(Vector2.right * _movement.x * _currentSpeed * Time.deltaTime);
        }
    }
    private void YAxisMovement()
    {
        /*We also want to know when we are on the ground before jumping and
        Know when there is input in the y direction,
        sprites that can fly also need to be accounted for*/
        if (_movement.y > 0f && (_isGrounded || _stats._spriteType == SpriteType.Flying))
        {
            //Apply a force to the rigidbody in a direction using the jumpforce variable
            _rigidbody.AddForce(Vector3.up * _stats._jumpForce);
        }
    }

    //This method controls the Animator Controller attached to the Sprite GameObject
    protected virtual void HandleAnimations()
    {
        _animator.SetFloat(AnimationStates.movementX, Mathf.Abs(_movement.x));
        _animator.SetFloat(AnimationStates.direction, _direction);
        _animator.SetBool(AnimationStates.grounded, _isGrounded);
        _animator.SetBool(AnimationStates.shooting, _isShooting);
        _animator.SetBool(AnimationStates.alive, _isAlive);
    }
    

    //Create a method for detecting when to land with raycasting
    protected virtual void HandleLanding()
    {
        //find the left side of the Sprite using offsets
        //Scale the sprite based off its local scale, so enemies can be different sizes
        Vector3 leftPosition = (Vector3.down * (_stats._rayYOffset * _spriteSize.y)) +
                               (Vector3.left * (_stats._rayXOffset * _spriteSize.x));


        //Casts a vector direction away from a point to see what it collides with
        RaycastHit2D hitLeft = Physics2D.Raycast(transform.position + leftPosition,
                                                 Vector3.down, 1f, _stats._groundCheck);


        //find the right side of the Sprite using offsets
        //Scale the sprite based off its local scale, so enemies can be different sizes
        Vector3 rightPosition = (Vector3.down * (_stats._rayYOffset * _spriteSize.y)) +
                               (Vector3.right * (_stats._rayXOffset * _spriteSize.x));

        RaycastHit2D hitRight = Physics2D.Raycast(transform.position + rightPosition,
                                                  Vector3.down, 1f, _stats._groundCheck);

        /*For purposes of visualizing these raycasts in the UnityEditor,
        we can draw these out in the SceneView. We will be removing these later*/
        Debug.DrawRay(transform.position + leftPosition, Vector3.down, Color.green);
        Debug.DrawRay(transform.position + rightPosition, Vector3.down, Color.green);

        /*Check if any of the raycasts hit a collider - if so then
        we’re on top of either the ground or some other object */
        if (hitLeft.collider != null || hitRight.collider != null)
        {
            _isGrounded = true;
        }
        else
        {
            _isGrounded = false;
        }
    }

    protected virtual void HandleShooting()
    {
        //if no input says we're shooting, then don't shoot!
        if(!_isShooting)
        {
            return;
        }

        //increment shoot timer gradually over time
        _shootTimer += Time.deltaTime;

        //once the "_shootTimer" reaches beyond the "_timeBetweenShots"
        if (_shootTimer >= _stats._timeBetweenShots)
        {
            //Instantiate and store a clone of the Bullet Prefab from "_stats" 
            GameObject bulletPrefab = Instantiate(_stats._bullet);
            //get the Projectile script on the bulletPrefab
            Projectile bulletScript = bulletPrefab.GetComponent<Projectile>();

            //fire the bullet some offset away from the center of the sprite
            Vector3 offset = new Vector3(_stats._shotOffset.x * _direction, _stats._shotOffset.y, 0f);

            //launch projectile in the proper "_direction"
            bulletScript.Init(transform.position + offset, Vector2.right * _direction, this.gameObject);
            
            //a shot has been fired, reset timer
            _shootTimer = 0f;

            //play shooting sound effect
            _audioSource.PlayOneShot(_stats._shootSound);
        }
    }
}
