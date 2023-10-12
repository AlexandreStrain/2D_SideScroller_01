using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //Let's organize our State Controller's variables into sections with Headers
    [Header("Statistics")]
    public ProjectileStatistics _stats;

    //Are we on the ground: true or false? Other states to check will be grouped here
    [Header("States")]
    public float _currentSpeed;
    public bool _isExploding;

    [Header("Debug")]
    public Vector2 _direction;
    public float _durationTimer;
    public GameObject _owner; //the GameObject who fired this bullet

    public Collider2D _collider;
    protected SpriteRenderer _renderer;
    protected Animator _animator;
    protected AudioSource _audioSource;

    public void Init(Vector3 startPosition, Vector2 direction, GameObject whoFired)
    {
        transform.position = startPosition;
        _direction = direction; 
        _owner = whoFired;
        _currentSpeed = _stats._speed;
        _durationTimer = _stats._maxDuration;
        //conditonal operators can be stored into variables
        _renderer.flipX = _direction.x < 0f; 
    }

    //this will be called on the animation of the bullet - an animation event
    public void Explode()
    {
        //Destroy is a Unity Built-in method that deletes a GameObject from the scene
        Destroy(this.gameObject);
    }

    void Awake()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponentInChildren<Collider2D>();
        _renderer = GetComponentInChildren<SpriteRenderer>();
        _audioSource = GetComponent<AudioSource>();

        //You are able to cast an enum into an Integer!
        _animator.SetFloat(AnimationStates.bulletType, (int)_stats._bulletType);

        transform.parent = GameObject.Find("Projectiles").transform;
    }

    void Update()
    {
        if (GameManager.Instance._isPaused)
        {
            return;
        }

        _animator.SetBool(AnimationStates.exploding, _isExploding);

        //if the bullet has not exploded
        if(!_isExploding)
        {
            transform.Translate(_direction * _currentSpeed * Time.deltaTime);
            
            _durationTimer -= Time.deltaTime;
            //once the time reaches zero the bullet explodes
            if(_durationTimer < 0f)
            {
                _isExploding = true;
            }
        }
    }

    private void SetToExplode()
    {
        _currentSpeed = 0f;
        _isExploding = true;
        _collider.enabled = false;
        _audioSource.PlayOneShot(_stats._explodeSound);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Determine what the bullet hit, was it a Sprite?
        Sprite hitSprite = collision.transform.GetComponent<Sprite>();

        //If what was hit has the Sprite Script attached
        if(hitSprite)
        {
            //if it was the Player who fired this bullet - done by comparing tags
            if(_owner.CompareTag(Tags.player))
            {
                //if the bullet has not exploded AND the hitSprite is an enemy
                if(hitSprite.CompareTag(Tags.enemy))
                {
                    //hit sprite then needs to take damage - a method belonging to sprite
                    hitSprite.TakeDamage(_stats._damage);
                    //the bullet should stop moving...
                    SetToExplode();
                    //return exits this OnCollisionEnter2D method early
                    return;
                }
            }
            else if (_owner.CompareTag(Tags.enemy))
            {
                //if the bullet has not exploded AND the hit sprite is a player
                if (hitSprite.CompareTag(Tags.player))
                {
                    //hit sprite then needs to take damage - a method belonging to sprite
                    hitSprite.TakeDamage(_stats._damage);
                    //the bullet should stop moving...
                    SetToExplode();
                    //return exits this OnCollisionEnter2D method early
                    return;
                }
                else if (hitSprite.CompareTag(Tags.enemy))
                {
                    //change the collider from being a solid collider to a trigger
                    _collider.isTrigger = true;
                    return;
                }
            }
        }

        //determine if the bullet has not exploded AND if the bullet hit the world OR another projectile instead...
        if(collision.gameObject.CompareTag(Tags.world) || collision.gameObject.CompareTag(Tags.projectile))
        {
            SetToExplode();
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        /*
         * a trigger occurs when a collider first enters or exits an object
         * it isn't a solid barrier therefore the bullet travels through enemies
         */
        _collider.isTrigger = false;
    }
}
