using System;
using UnityEngine;


public static class Tags
{
    //Sprites
    public static string player = "Player";
    public static string enemy = "Enemy";

    //Other
    public static string world = "World";
    public static string projectile = "Projectile";
    public static string items = "Items";
    public static string events = "Event";
}



public static class AnimationStates
{
    //Sprites
    public static int alive = Animator.StringToHash("IsAlive");
    public static int movementX = Animator.StringToHash("MovementX");
    public static int direction = Animator.StringToHash("Direction");
    public static int grounded = Animator.StringToHash("IsGrounded");
    public static int shooting = Animator.StringToHash("IsShooting");


    //Bullets
    public static int bulletType = Animator.StringToHash("Type");
    public static int exploding = Animator.StringToHash("IsExploding");
}




//Sprites
public enum SpriteType { Grounded, Flying }; 
public enum AttackType { None, Constant, Target };
public enum MoveType { None, Normal, Circle, Jump };

//Bullets
public enum BulletType { Purple, Green, Red, Fire };

//Collectables
public enum ItemType { Health, Money };


//Delegates
public delegate void AttackPattern(EnemyController ec);
public delegate void MovePattern(EnemyController ec);
public delegate void Pause();
public delegate void OnDefeat();
public delegate void Reset();



[Serializable]
public struct ItemDrop
{
    public GameObject _itemPrefab;
    public int _quantity;
}