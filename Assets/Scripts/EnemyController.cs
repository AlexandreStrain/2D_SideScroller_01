//For something we need to use below, bring in this import
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* Random is found in both UnityEngine and System
 * this line here tells the program we want to use
 * the one from the UnityEngine and not system*/
using Random = UnityEngine.Random;

public class EnemyController : Sprite
{
    public bool _isBoss;
    public Vector2 _spawnLocation;

    //for raycasts, we won't check as frequently for enemies
    public float _checkTimer;
    public float _patternTimer;

    public float _checkDelay;

    //for circle pattern, how far around a point to travel from
    public float _radius;
    public float _angle;

    //A target position eventually to aim at, if needed
    public Transform _target;

    protected AttackPattern _attack;
    protected MovePattern _move;

    public override void Init()
    {
        base.Init();
        //set the spawn location of the sprite
        _spawnLocation = transform.position;

        //parent this enemy to the "Enemies" GameObject's transform in the scene
        transform.parent = GameObject.Find("Enemies").transform;

        //set default move and attack pattern from Sprite Statistics
        _currentAttackType = _stats._defaultAttackType;
        _currentMoveType = _stats._defaultMovementType;
        SelectAttackType();
        SelectMovementType();
    }

    public override void Tick()
    {
        if (GameManager.Instance._isPaused)
        {
            _rigidbody.simulated = false;
            return;
        }
        else
        {
            _rigidbody.simulated = true;
        }

        if (_stats._canChangePattern)
        {
            _patternTimer += Time.deltaTime;
            if (_patternTimer >= _stats._patternDuration)
            {
                _patternTimer = 0f;

                //randomize the next movement or attack pattern here

                /* How many MovePatterns Enums did we define? 4

                 * We can put a hardcoded value here, but let's instead use
                 * the static method Enum.GetNames - which returns
                 * an array representing the names of all the items in the enum
                 * Example: [ None, Normal, Circle, Jump ] is what Enum.GetNames will give us

                 * Recall how Arrays use .Length to find out how many things are in them

                 * One can also cast from an integer into an Enum to store inside the
                 * enum variables _currentMoveType or _currentAttackType 

                 * This is a lot to unpack, make sure explain in phases

                 * WHY Enum.GetNames? if we want to create more enum patterns
                 * we do not need come back and change this line
                 */
                _currentMoveType = (MoveType)Random.Range(1,
                                             Enum.GetNames(typeof(MoveType)).Length);

                //then reselect movement or attack pattern
                SelectMovementType();
            }
        }
        base.Tick();
    }

    protected override void HandleLanding()
    {
        //the circle pattern is always done in the air
        if (_currentMoveType == MoveType.Circle)
        {
            _isGrounded = false;
            _rigidbody.velocity = Vector2.zero;
        }
        else
        {
            base.HandleLanding();
        }
    }

    protected override void HandleMovement()
    {
        /*
         * NOTE: Write the below if statement first
         * use the method simplification after
         * the simplification _move?.Invoke(this) checks if the variable
         * contains null with the ? (the null coalescing operator)
         * calls/invokes the method stored in the delegate if it exits
        if(_move != null)
        {
            _move(this);
        }
        */

        /*Enemies check every so often their surroundings
        if check time has not reached max check delay time, don't call pattern*/
        _checkTimer += Time.deltaTime;
        if (_checkTimer >= _checkDelay)
        {      
            //reset check timer
            _checkTimer = 0f;
            _move?.Invoke(this);
        }
        else
        {
            _movement.y = 0f; //no jumping input
        }
        //to visualize their side raycasts
        float xDistance = (_stats._rayXOffset * _spriteSize.x);
        Debug.DrawRay(transform.position + (Vector3.right * xDistance), Vector2.right * xDistance, Color.yellow);
        Debug.DrawRay(transform.position + (Vector3.left * xDistance), Vector2.left * xDistance, Color.yellow);
        base.HandleMovement();
    }

    protected override void HandleShooting()
    {
        /*A visual representation of the aim of the enemy
        in the scene view using a debug ray
        only works if enemy has a target to fire at*/
        //Vector3 directionToTarget = Vector3.zero;
        if(_target)
        {
            Vector3 directionToTarget = (_target.position - transform.position).normalized;
            Debug.DrawRay(transform.position, directionToTarget, Color.red);
        }

        _shootTimer += Time.deltaTime;
        if(_shootTimer >= _stats._timeBetweenShots)
        {
            /*if we have an attack todo (it's not null, the ? checks this)
            then call/invoke the method stored in the delegate*/
            _attack?.Invoke(this);
            _shootTimer = 0f;

            _audioSource.PlayOneShot(_stats._shootSound);
        }
        
    }

    protected override void HandleDeath()
    {
        base.HandleDeath();

        foreach (Transform c in transform)
        {
            c.gameObject.layer = LayerMask.NameToLayer(Tags.world);
            c.tag = Tags.world;
        }

        if(_stats._spriteType == SpriteType.Flying)
        {
            transform.GetComponentInChildren<CircleCollider2D>().enabled = false;
        }

        if (_isBoss)
        {
            Debug.Log("Boss Defeated! Maybe do something special here?");
            GameManager.Instance._isBossDefeated = true;
            GameManager.Instance._onBossDefeat.Invoke();
        }
        DropItems();
    }

    protected void DropItems()
    {
        //go through all items the sprites can drop based on sprite statistics
        for(int i = 0; i < _stats._itemDrops.Length; i++)
        {
            // drop items depending on the quantity the sprite has
            for (int j = 0; j < _stats._itemDrops[i]._quantity; j++)
            {
                //create the item in unity with Instantiate, place it where the enemy is
                GameObject item = Instantiate(_stats._itemDrops[i]._itemPrefab,
                                         transform.position, Quaternion.identity);

                //Let's add some fun and make the items shoot out from the enemy
                //using rigidbody2D to launch the items with random force
                Rigidbody2D droppedItemRb = item.GetComponent<Rigidbody2D>();
                droppedItemRb.AddForce(new Vector2(Random.Range(-6, 6), Random.Range(0, 9)),
                                       ForceMode2D.Impulse);
            }
        }
    }

    void SelectAttackType()
    {
        switch (_currentAttackType)
        {
            case AttackType.Constant:
                _target = null;
                _attack = AttackPatterns.KeepShooting;
                break;
            case AttackType.Target:
                _target = GameObject.FindGameObjectWithTag(Tags.player).transform;
                _attack = AttackPatterns.TargetEntity;
                break;
            default:
                _target = null;
                _attack = null;
                break;
        }

    }

    void SelectMovementType()
    {
        //reset current speed to default speed in Sprite Statistics
        _currentSpeed = _stats._speed;
        _checkDelay = 0.5f;

        //choose direction to move randomly
        int randomDirection = Random.Range(-1, 2);
        /*
         * there are two directions: -1 = left and +1 = right
         * we can get 0 with randomizing between -1 and 2
         * so continue randomizing until we get either -1 or 1
        */
        while (randomDirection == 0)
        {
            randomDirection = Random.Range(-1, 2);
        }

        _movement = Vector2.right * randomDirection;

        //as _currentMoveType is an Enum, there’s only 4 choices
        switch (_currentMoveType)
        {
            case MoveType.Normal:
                _move = MovePatterns.MoveOnPlatform;
                //cases need to exit or break to not fall into another case
                break;
            case MoveType.Jump:
                _move = MovePatterns.Jump;
                break;
            case MoveType.Circle:
                _angle = Random.Range(0f, 360f);
                _currentSpeed += 100;
                _checkDelay = 0f;
                _move = MovePatterns.CircleAroundPoint;
                break;
            //default is like an else and handles the rest
            default:
                _move = null;
                break;
        }

    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        Tick();
    }
}
