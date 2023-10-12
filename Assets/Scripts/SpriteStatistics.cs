using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Haunted Harbour/Sprite Statistics")]
public class SpriteStatistics : ScriptableObject
{
    [Header("General")]
    public SpriteType _spriteType;
    public float _maxHealth;
    //the maximum time spent on a single pattern, useful for bosses
    public float _patternDuration;
    //determine if this sprite is a boss enemy
    public bool _canChangePattern;

    [Header("Attack")]
    public AttackType _defaultAttackType;
    public GameObject _bullet;
    public float _timeBetweenShots;
    public Vector2 _shotOffset;

    [Header("Movement")]
    public MoveType _defaultMovementType;
    public float _speed;
    public float _jumpForce;
    /*A layer mask will be used to check for collisions between gameObjects on certain layers
rather than check for collisions on all layers which would be computer intensive!*/
    public LayerMask _groundCheck;
    /*We don't always want to Raycast from the center of the Sprite
these variables will be useful for larger sprites*/
    public float _rayXOffset = 0.1f;
    public float _rayYOffset = 0f;

    [Header("Audio")]
    public AudioClip _shootSound;
    public AudioClip _hurtSound;
    public AudioClip _deathSound;

    [Header("Items")]
    public ItemDrop[] _itemDrops;
}
