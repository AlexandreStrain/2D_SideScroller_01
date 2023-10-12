using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Haunted Harbour/Projectile Statistics")]
public class ProjectileStatistics : ScriptableObject
{
    [Header("General")]
    public BulletType _bulletType;

    [Header("Attack")]
    public float _damage;

    [Header("Movement")]
    public float _speed;
    public float _maxDuration; //duration of bullet

    [Header("Audio")]
    public AudioClip _explodeSound;
}
